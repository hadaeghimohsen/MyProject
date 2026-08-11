// ========== COMPLETED - DO NOT MODIFY WITHOUT REVIEW ==========
// STABLE VERSION 1.0 - 2026-08-07
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.JobRouting.Jobs;
using System.MessageBroadcast.Code;

namespace System.MessageBroadcast.Ui.SmsApp
{
   public partial class WEBS_MESG_F : UserControl
   {
      // ============================================================
      // 1. مدل‌های داده
      // ============================================================

      public enum QueueItemType { Business, Service, Customer, Expense, Organ }

      public class BusinessModel
      {
         public string Name { get; set; }
         public string Address { get; set; }
         public string Phone { get; set; }
         public string Mobile { get; set; }
         public string Email { get; set; }
         public string WebSite { get; set; }
         public string Description { get; set; }
      }

      public enum ServiceType { RegistrationRenewal, MiscIncome }

      public class ServiceModel
      {
         public ServiceType Type { get; set; }
         public string ServiceName { get; set; }
         public decimal RegisterFee { get; set; }
         public decimal RenewalFee { get; set; }
         public int Duration { get; set; }
         public int SessionsCount { get; set; }
         public string ProductName { get; set; }
         public decimal Price { get; set; }
         public int Quantity { get; set; }
         public decimal TotalAmount { get; set; }
      }

      public class CustomerModel
      {
         public string FirstName { get; set; }
         public string LastName { get; set; }
         public string Mobile { get; set; }
         public string Gender { get; set; }
         public string NationalId { get; set; }
         public string Email { get; set; }
         public string Address { get; set; }
      }

      public class QueueItem : INotifyPropertyChanged
      {
         public Guid Id { get; set; }
         public QueueItemType Type { get; set; }
         public string JsonPayload { get; set; }
         public DateTime CreatedAt { get; set; }

         private string _status;
         public string Status
         {
            get { return _status; }
            set { _status = value; OnChanged("Status"); }
         }

         private string _error;
         public string Error
         {
            get { return _error; }
            set { _error = value; OnChanged("Error"); }
         }

         public event PropertyChangedEventHandler PropertyChanged;
         private void OnChanged(string p)
         {
            if (PropertyChanged != null)
               PropertyChanged(this, new PropertyChangedEventArgs(p));
         }
      }

      public class ExpenseModel
      {
         public string storeId { get; set; }
         public long code { get; set; }
         public long groupCode { get; set; }
         public long categoryCode { get; set; }
         public string description { get; set; }
         public decimal price { get; set; }
         public int sessionCount { get; set; }
         public int cycleDays { get; set; }
         public int reminderDays { get; set; }
         public string hasFiscalId { get; set; }
         public string fiscalId { get; set; }
      }

      // ============================================================
      // 2. فیلدها
      // ============================================================

      private LidomaMarket _lidoma;
      private string _baseUrl;
      private System.Windows.Forms.Timer _netTimer;
      private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);
      private BindingList<QueueItem> _queue;
      private string _queueFile;
      private bool _serverStatusChecked = false;
      private DateTime _lastServerStatusCheck = DateTime.MinValue;
      private JObject _cachedServerStatus;
      private JObject _cachedAccountStatus;

      public WEBS_MESG_F()
      {
         InitializeComponent();
         this.Disposed += WEBS_MESG_F_Disposed;
      }

      // ============================================================
      // 3. رویدادهای فرم
      // ============================================================

      private async void WEBS_MESG_F_Load(object sender, EventArgs e)
      {
         _queueFile = Path.Combine(
             Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
             "LidomaQueue", "queue.json");
         Directory.CreateDirectory(Path.GetDirectoryName(_queueFile));

         _queue = new BindingList<QueueItem>();
         //dataGridView1.DataSource = _queue;
         //dataGridView1.DataBindingComplete += (s, ev) =>
         //{
         //   if (dataGridView1.Columns["JsonPayload"] != null)
         //      dataGridView1.Columns["JsonPayload"].Visible = false;
         //};

         LoadQueue();

         _netTimer = new System.Windows.Forms.Timer { Interval = GetConfigIntervalMs("ServerStatusIntervalMinutes", 5) };
         _netTimer.Tick += _netTimer_Tick;
         _netTimer.Start();

         Log("فرم بارگذاری شد. بررسی وضعیت اینترنت...");
         bool ok = await IsInternetAvailableAsync();
         SetConnectionStatus(ok);
         if (ok)
            await RunSendAllAsync();
      }

      private int GetConfigIntervalMs(string key, int defaultMinutes)
      {
         try
         {
            var val = ConfigurationManager.AppSettings[key];
            if (val != null)
            {
               int minutes;
               if (int.TryParse(val, out minutes) && minutes > 0)
                  return minutes * 60 * 1000;
            }
         }
         catch (Exception ex)
         {
            Log("خطا در خواندن تنظیمات تایمر: " + ex.Message);
         }
         return defaultMinutes * 60 * 1000;
      }

      private int GetBackgroundIntervalMs()
      {
         try
         {
            if (IProjectConnectionString != null)
            {
               using (var iProjectLocal = new Data.iProjectDataContext(IProjectConnectionString))
               {
                  var setting = iProjectLocal.Message_Broad_Settings
                      .FirstOrDefault(m => m.SERV_TYPE == "005");
                  if (setting != null && setting.BGWK_INTR.HasValue)
                  {
                     return setting.BGWK_INTR.Value;
                  }
               }
            }
         }
         catch (Exception ex)
         {
            Log("خطا در خواندن BGWK_INTR از دیتابیس: " + ex.Message);
         }
         return 600000;
      }

      private bool IsBackgroundWorkEnabled()
      {
         try
         {
            if (IProjectConnectionString != null)
            {
               using (var iProjectLocal = new Data.iProjectDataContext(IProjectConnectionString))
               {
                  var setting = iProjectLocal.Message_Broad_Settings
                      .FirstOrDefault(m => m.SERV_TYPE == "005" && m.BGWK_STAT == "002");
                  return setting != null;
               }
            }
         }
         catch (Exception ex)
         {
            Log("خطا در خواندن BGWK_STAT از دیتابیس: " + ex.Message);
         }
         return true;
      }

      private void WEBS_MESG_F_Disposed(object sender, EventArgs e)
      {
         if (_netTimer != null) _netTimer.Stop();
         SaveQueue();
      }

      // ============================================================
      // 4. بررسی اتصال اینترنت (هر 10 دقیقه)
      // ============================================================

      private async void _netTimer_Tick(object sender, EventArgs e)
      {
         _netTimer.Stop();
         try
         {
            // Use database-configured interval
            _netTimer.Interval = GetBackgroundIntervalMs();

            // Check if background work is enabled for Lidoma (SERV_TYPE = '005', BGWK_STAT = '002')
            if (!IsBackgroundWorkEnabled())
            {
               Log("ارسال پس‌زمینه غیرفعال است (BGWK_STAT != '002').");
               return;
            }

            bool ok = await IsInternetAvailableAsync();
            SetConnectionStatus(ok);
            if (ok)
               await RunSendAllAsync();
         }
         catch (Exception ex)
         {
            Log("خطا در هنگام ارسال: " + ex.Message);
         }
         finally
         {
            _netTimer.Start();
         }
      }

      private async Task<bool> IsInternetAvailableAsync()
      {
         if (string.IsNullOrEmpty(_baseUrl))
         {
            var conf = ReadLidomaConfig();
            _baseUrl = conf != null ? conf.Item1 : null;
         }
         if (string.IsNullOrEmpty(_baseUrl)) return false;

         try
         {
            using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) })
            {
               await client.GetAsync(_baseUrl, HttpCompletionOption.ResponseHeadersRead);
               return true;
            }
         }
         catch
         {
            return false;
         }
      }

      // ============================================================
      // 5. احراز هویت (Bearer Token) - خواندن از تنظیمات سرویس 005
      // ============================================================

      private Tuple<string, string, string> ReadLidomaConfig()
      {
         try
         {
            var conf = MgbsBs.List.OfType<Data.Message_Broad_Setting>()
                .FirstOrDefault(m => m.SERV_TYPE == "005");
            if (conf == null) return null;
            return Tuple.Create(conf.BASE_URL, conf.USER_NAME, conf.PASS_WORD);
         }
         catch { return null; }
      }

      private async Task<bool> EnsureLoggedInAsync()
      {
         if (_lidoma != null /*&& _lidoma.IsAuthenticated*/) return true;

         var conf = ReadLidomaConfig();
         if (conf == null)
         {
            Log("تنظیمات لیدوما (Serv_Type=005) یافت نشد.");
            return false;
         }
         _baseUrl = conf.Item1;
         _lidoma = LidomaMarket.Instance;
         bool ok = await _lidoma.LoginAsync(conf.Item2, conf.Item3);
         Log(ok ? "ورود به لیدوما موفق بود." : "ورود به لیدوما ناموفق بود.");
         return ok;
      }

      // ============================================================
      // 6. همگام‌سازی (برای تب‌های قدیمی که نیاز به _lidoma دارند)
      // ============================================================

      private bool EnsureLoggedIn(out string reason)
      {
         reason = null;
         if (_lidoma == null)
         {
            reason = "ابتدا در تب «اتصال و احراز هویت» وارد شوید.";
            return false;
         }
         return true;
      }

      private static JObject ParseJson(string json)
      {
         return JObject.Parse(json);
      }

      // ============================================================
      // 7. رویدادهای تب‌های قدیمی
      // ============================================================

      // tp_001 - Login / Logout

      private async void LoginBtn_Click(object sender, EventArgs e)
      {
         try
         {
            var baseUrl = (BaseUrl_Txt.EditValue ?? "").ToString().Trim();
            var user = (UserName_Txt.EditValue ?? "").ToString().Trim();
            var pass = (Password_Txt.EditValue ?? "").ToString();

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
               Status_Mmo.Text = "لطفا آدرس پایه، نام کاربری و رمز عبور را وارد کنید.";
               return;
            }

            Status_Mmo.Text = "در حال ورود به " + baseUrl + " ...";
            _lidoma = LidomaMarket.Instance;
            _baseUrl = baseUrl;
            bool ok = await _lidoma.LoginAsync(user, pass);

            Status_Mmo.Text = ok
                ? "ورود موفقیت‌آمیز بود.\r\nتوکن دریافت شد و کلاینت آمادهٔ استفاده در سایر تب‌هاست."
                : "ورود ناموفق بود. نام کاربری یا رمز عبور را بررسی کنید.";
         }
         catch (Exception ex)
         {
            Status_Mmo.Text = "خطا در هنگام ورود:\r\n" + ex.Message;
         }
      }

      private void LogoutBtn_Click(object sender, EventArgs e)
      {
         _lidoma = null;
         _baseUrl = null;
         Status_Mmo.Text = "خروج انجام شد و نمونهٔ کلاینت بسته شد.";
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
             new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      // tp_002 - Status & Credit

      private async void BtnServerStatus_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StatusOut_Mmo.Text = reason; return; }
         try
         {
            StatusOut_Mmo.Text = "در حال دریافت وضعیت سرور...";
            var res = await _lidoma.GetServerStatusAsync();
            StatusOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StatusOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnAccountStatus_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StatusOut_Mmo.Text = reason; return; }
         try
         {
            StatusOut_Mmo.Text = "در حال دریافت وضعیت حساب...";
            var res = await _lidoma.GetAccountStatusAsync();
            StatusOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StatusOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnAccountCharge_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StatusOut_Mmo.Text = reason; return; }
         try
         {
            StatusOut_Mmo.Text = "در حال دریافت اعتبار حساب...";
            var res = await _lidoma.GetAccountChargeAsync();
            StatusOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StatusOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnMsgStatus_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StatusOut_Mmo.Text = reason; return; }
         var msgId = MsgId_Txt.Text.Trim();
         if (string.IsNullOrEmpty(msgId)) { StatusOut_Mmo.Text = "شناسه پیامک را وارد کنید."; return; }
         try
         {
            StatusOut_Mmo.Text = "در حال استعلام وضعیت پیامک...";
            var res = await _lidoma.GetMessageStatusAsync(msgId);
            StatusOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StatusOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      // tp_003 - Send SMS

      private async void BtnSend_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { SendOut_Mmo.Text = reason; return; }
         try
         {
            var storeId = StoreId_Txt.Text.Trim();
            var senders = Sender_Txt.Text.Trim();
            var message = MsgBody_Mmo.Text;
            int branchIndex;
            if (!int.TryParse(Branch_Txt.Text.Trim(), out branchIndex)) branchIndex = 0;

            if (BulkChk.Checked)
            {
               var receptors = Receptor_Mmo.Text
                   .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                   .Select(r => r.Trim()).ToArray();
               SendOut_Mmo.Text = "در حال ارسال گروهی...";
               var res = await _lidoma.SendBatchAsync(storeId, branchIndex, senders, receptors, message);
               SendOut_Mmo.Text = res.ToString(Formatting.Indented);
            }
            else
            {
               var receptor = Receptor_Mmo.Text.Trim();
               SendOut_Mmo.Text = "در حال ارسال...";
               var res = await _lidoma.SendSingleAsync(storeId, branchIndex, senders, receptor, message);
               SendOut_Mmo.Text = res.ToString(Formatting.Indented);
            }
         }
         catch (Exception ex) { SendOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      // tp_004 - Stores

      private async void BtnGetStores_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         try
         {
            int page = 1, limit = 20;
            int p;
            if (int.TryParse(Page_Txt.Text.Trim(), out p)) page = p;
            int l;
            if (int.TryParse(Limit_Txt.Text.Trim(), out l)) limit = l;
            StoreOut_Mmo.Text = "در حال دریافت لیست فروشگاه‌ها...";
            var res = await _lidoma.GetStoresAsync(page, limit);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnGetStore_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var slug = StoreSlug_Txt.Text.Trim();
         if (string.IsNullOrEmpty(slug)) { StoreOut_Mmo.Text = "نامک فروشگاه را وارد کنید."; return; }
         try
         {
            StoreOut_Mmo.Text = "در حال دریافت فروشگاه...";
            var res = await _lidoma.GetStoreAsync(slug);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnCreateStore_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var json = StoreData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(json)) { StoreOut_Mmo.Text = "دادهٔ فروشگاه (JSON) را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            StoreOut_Mmo.Text = "در حال ایجاد فروشگاه...";
            var res = await _lidoma.CreateStoreAsync(data);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnCreateStoresBulk_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var json = StoreData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(json)) { StoreOut_Mmo.Text = "دادهٔ فروشگاه‌ها (JSON) را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            StoreOut_Mmo.Text = "در حال ایجاد انبوه فروشگاه‌ها...";
            var res = await _lidoma.CreateStoresBulkAsync(data);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnUpdateStore_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var slug = StoreSlug_Txt.Text.Trim();
         var json = StoreData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(json)) { StoreOut_Mmo.Text = "نامک و دادهٔ به‌روزرسانی را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            StoreOut_Mmo.Text = "در حال به‌روزرسانی فروشگاه...";
            var res = await _lidoma.UpdateStoreAsync(slug, data);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnDeleteStore_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var slug = StoreSlug_Txt.Text.Trim();
         if (string.IsNullOrEmpty(slug)) { StoreOut_Mmo.Text = "نامک فروشگاه را وارد کنید."; return; }
         try
         {
            StoreOut_Mmo.Text = "در حال حذف فروشگاه...";
            var res = await _lidoma.DeleteStoreAsync(slug);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      // tp_004 - Store Management Extensions (Organs, Services, Revenues, Bulk)

      private async void BtnGetStoreOrgans_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         try
         {
            StoreOut_Mmo.Text = "در حال دریافت ارگان‌ها از دیتابیس...";
            var organs = await GetOrgansFromDatabaseAsync();
            StoreOut_Mmo.Text = organs.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnSetStoreOrgans_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var storeId = StoreSlug_Txt.Text.Trim();
         if (string.IsNullOrEmpty(storeId)) { StoreOut_Mmo.Text = "شناسه فروشگاه را وارد کنید."; return; }
         try
         {
            StoreOut_Mmo.Text = "در حال ساخت JSON ارگان‌ها از دیتابیس...";
            var organs = await GetOrgansFromDatabaseAsync();
            var subscriptionDiscounts = await GetSubscriptionDiscountsFromDatabaseAsync();
            var productSalesDiscounts = await GetProductSalesDiscountsFromDatabaseAsync();

            var discountsObj = new JObject(
                new JProperty("subscriptions", subscriptionDiscounts),
                new JProperty("productSales", productSalesDiscounts)
            );

            var organsData = new JObject(
                new JProperty("organs", new JObject(
                    new JProperty("items", organs),
                    new JProperty("discounts", discountsObj)
                ))
            );

            StoreOut_Mmo.Text = "در حال ذخیره ارگان‌ها به Lidoma API...";
            var res = await _lidoma.SetStoreOrgansAsync(storeId, organsData);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnGetStoreServices_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var storeId = StoreSlug_Txt.Text.Trim();
         if (string.IsNullOrEmpty(storeId)) { StoreOut_Mmo.Text = "شناسه فروشگاه را وارد کنید."; return; }
         try
         {
            StoreOut_Mmo.Text = "در حال دریافت سرویس‌ها...";
            var res = await _lidoma.GetStoreServicesAsync(storeId);
            StoreOut_Mmo.Text = res != null ? res.ToString(Formatting.Indented) : "نتیجه‌ای یافت نشد.";
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnSetStoreServices_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var storeId = StoreSlug_Txt.Text.Trim();
         var json = StoreData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(storeId) || string.IsNullOrEmpty(json)) { StoreOut_Mmo.Text = "شناسه فروشگاه و دادهٔ سرویس‌ها (JSON) را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            StoreOut_Mmo.Text = "در حال ذخیره سرویس‌ها...";
            var res = await _lidoma.SetStoreServicesAsync(storeId, data);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnGetStoreRevenues_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var storeId = StoreSlug_Txt.Text.Trim();
         if (string.IsNullOrEmpty(storeId)) { StoreOut_Mmo.Text = "شناسه فروشگاه را وارد کنید."; return; }
         try
         {
            StoreOut_Mmo.Text = "در حال دریافت درآمدها...";
            var res = await _lidoma.GetStoreRevenuesAsync(storeId);
            StoreOut_Mmo.Text = res != null ? res.ToString(Formatting.Indented) : "نتیجه‌ای یافت نشد.";
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnCreateServicesBulk_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
         var json = StoreData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(json)) { StoreOut_Mmo.Text = "دادهٔ سرویس‌ها (JSON) را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            StoreOut_Mmo.Text = "در حال ایجاد سرویس‌های گروهی...";
            var res = await _lidoma.CreateServicesBulkAsync(data);
            StoreOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { StoreOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private void BtnOpenOrgansForm_Click(object sender, EventArgs e)
      {
         var storeId = StoreSlug_Txt.Text.Trim();
         //var form = new frmStoreOrgans(storeId);
         //form.Show();
      }

      // ============================================================
      // Helper Methods: Database Reading for Organs & Discounts
      // ============================================================

      /// <summary>
      /// Executes Query 1 from dbo.Sub_Unit to get organs.
      /// Returns JArray with items: { code, name }
      /// </summary>
      private async Task<JArray> GetOrgansFromDatabaseAsync()
      {
         Log("Executing Query 1: Getting organs from dbo.Sub_Unit");
         var organs = new JArray();

         try
         {
            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               var query = iScscLocal.Sub_Units
                  .Where(s => s.LDMA_STAT == null || s.LDMA_STAT == "003")
                  .OrderBy(s => s.ORGN_CODE_DNRM);

               foreach (var s in query)
               {
                  var organ = new JObject(
                      new JProperty("code", s.ORGN_CODE_DNRM != null ? s.ORGN_CODE_DNRM : null),
                      new JProperty("name", s.SUNT_DESC != null ? s.SUNT_DESC : null)
                  );
                  organs.Add(organ);
               }
            }
         }
         catch (Exception ex)
         {
            Log("Error getting organs: " + ex.Message);
            throw;
         }

         Log(string.Format("Retrieved {0} organs", organs.Count));
         return organs;
      }

      /// <summary>
      /// Executes Query 2 from dbo.Basic_Calculate_Discount (Rqtp_Code IN 001, 009).
      /// Returns JArray with subscription discounts.
      /// IMPORTANT: Only adds startsAt/endsAt if Kind == 'dateRange' AND value is not null.
      /// </summary>
      private async Task<JArray> GetSubscriptionDiscountsFromDatabaseAsync()
      {
         Log("Executing Query 2: Getting subscription discounts (Rqtp_Code IN 001, 009)");
         var discounts = new JArray();

         try
         {
            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               var query = iScscLocal.Basic_Calculate_Discounts
                  .Where(d => (d.RQTP_CODE == "001" || d.RQTP_CODE == "009")
                     && ((d.LDMA_STAT ?? "001") == "001" || d.LDMA_STAT == "003")
                     && d.Expense.EXPN_STAT == "002"
                     && (d.Method.MTOD_STAT == "002" && d.Method.SHOW_STAT == "002")
                     && (d.Category_Belt.CTGY_STAT == "002" && d.Category_Belt.SHOW_STAT == "002")
                     && iScscLocal.Club_Methods.Any(a => a.MTOD_CODE == d.MTOD_CODE && a.MTOD_STAT == "002"))
                  .OrderBy(d => d.CODE);

               foreach (var d in query)
               {
                  var discount = new JObject(
                      new JProperty("code", d.CODE.ToString()),
                      new JProperty("organCode", d.ORGN_CODE_DNRM != null ? d.ORGN_CODE_DNRM : null),
                      new JProperty("revenueCode", d.EXPN_CODE.HasValue ? d.EXPN_CODE.Value.ToString() : null)
                  );

                  var kind = GetDiscountKind(d.ACTN_TYPE);
                  var type = d.DSCT_TYPE == "001" ? "percent" : "amount";
                  var value = d.PRCT_DSCT;
                  var isActive = d.STAT == "002";

                  discount.Add(new JProperty("kind", kind));
                  discount.Add(new JProperty("type", type));
                  discount.Add(new JProperty("value", value));
                  discount.Add(new JProperty("isActive", isActive));

                  // ONLY add startsAt and endsAt if Kind == 'dateRange' AND value is not null
                  if (kind == "dateRange" && value.HasValue)
                  {
                     if (d.FROM_DATE.HasValue)
                     {
                        discount.Add(new JProperty("startsAt", d.FROM_DATE.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")));
                     }
                     if (d.TO_DATE.HasValue)
                     {
                        discount.Add(new JProperty("endsAt", d.TO_DATE.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")));
                     }
                  }

                  discounts.Add(discount);
               }
            }
         }
         catch (Exception ex)
         {
            Log("Error getting subscription discounts: " + ex.Message);
            throw;
         }

         Log(string.Format("Retrieved {0} subscription discounts", discounts.Count));
         return discounts;
      }

      private static string GetDiscountKind(string actnType)
      {
         switch (actnType)
         {
            case "001": return "regular";
            case "002": return "periodic";
            case "003": return "dateRange";
            case "004": return "deposit";
            case "005": return "loyalCustomer";
            case "006": return "newCustomerReferral";
            case "007": return "campaign";
            case "008": return "birthdayGift";
            case "009": return "serviceCommission";
            case "010": return "referralCommission";
            default: return null;
         }
      }

      /// <summary>
      /// Executes Query 3 from dbo.Basic_Calculate_Discount (Rqtp_Code IN 016).
      /// Returns JArray with product sales discounts.
      /// </summary>
      private async Task<JArray> GetProductSalesDiscountsFromDatabaseAsync()
      {
         Log("Executing Query 3: Getting product sales discounts (Rqtp_Code IN 016)");
         var discounts = new JArray();

         try
         {
            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               var query = iScscLocal.Basic_Calculate_Discounts
                  .Where(d => d.RQTP_CODE == "016"
                     && d.Expense.EXPN_STAT == "002"
                     && ((d.LDMA_STAT ?? "001") == "001" || d.LDMA_STAT == "003")
                     && (d.Method.MTOD_STAT == "002" && d.Method.SHOW_STAT == "002")
                     && (d.Category_Belt.CTGY_STAT == "002" && d.Category_Belt.SHOW_STAT == "002")
                     && iScscLocal.Club_Methods.Any(a => a.MTOD_CODE == d.MTOD_CODE && a.MTOD_STAT == "002"))
                  .OrderBy(d => d.CODE);

               foreach (var d in query)
               {
                  var discount = new JObject(
                      new JProperty("code", d.CODE.ToString()),
                      new JProperty("organCode", d.ORGN_CODE_DNRM != null ? d.ORGN_CODE_DNRM : null),
                      new JProperty("revenueCode", d.EXPN_CODE.HasValue ? d.EXPN_CODE.Value.ToString() : null)
                  );

                  var kind = GetDiscountKind(d.ACTN_TYPE);
                  var type = d.DSCT_TYPE == "001" ? "percent" : "amount";
                  var value = d.PRCT_DSCT;
                  var isActive = d.STAT == "002";

                  discount.Add(new JProperty("kind", kind));
                  discount.Add(new JProperty("type", type));
                  discount.Add(new JProperty("value", value));
                  discount.Add(new JProperty("isActive", isActive));

                  discounts.Add(discount);
               }
            }
         }
         catch (Exception ex)
         {
            Log("Error getting product sales discounts: " + ex.Message);
            throw;
         }

         Log(string.Format("Retrieved {0} product sales discounts", discounts.Count));
         return discounts;
      }

      // tp_005 - Customers

      private async void BtnCreateCustomer_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { CustOut_Mmo.Text = reason; return; }
         var json = CustData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(json)) { CustOut_Mmo.Text = "دادهٔ مشتری (JSON) را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            CustOut_Mmo.Text = "در حال ایجاد مشتری...";
            var res = await _lidoma.CreateCustomerAsync(data);
            CustOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { CustOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      private async void BtnCreateCustomersBulk_Click(object sender, EventArgs e)
      {
         string reason;
         if (!EnsureLoggedIn(out reason)) { CustOut_Mmo.Text = reason; return; }
         var json = CustData_Mmo.Text.Trim();
         if (string.IsNullOrEmpty(json)) { CustOut_Mmo.Text = "دادهٔ مشتریان (JSON) را وارد کنید."; return; }
         try
         {
            var data = ParseJson(json);
            CustOut_Mmo.Text = "در حال ایجاد انبوه مشتریان...";
            var res = await _lidoma.CreateCustomersBulkAsync(data);
            CustOut_Mmo.Text = res.ToString(Formatting.Indented);
         }
         catch (Exception ex) { CustOut_Mmo.Text = "خطا: " + ex.Message; }
      }

      // ============================================================
      // 8. دکمه‌های ارسال صف آفلاین
      // ============================================================

      private async void btnSendBusiness_Click(object sender, EventArgs e)
      {
         await RunSendAsync(QueueItemType.Business);
      }

      private async void btnSendServices_Click(object sender, EventArgs e)
      {
         if (!await HasAnyClubWithStoreIdAsync())
         {
            Log("هیچ باشگاهی دارای StoreId (LDMA_CODE) نیست. ابتدا باشگاه‌ها را همگام‌سازی کنید.");
            MessageBox.Show("هیچ باشگاهی در لیدوما ثبت نشده است. ابتدا دکمه «ارسال باشگاه» را بزنید.", "پیش‌نیاز", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }
         await RunSendAsync(QueueItemType.Service);
      }

      private async void btnSendCustomers_Click(object sender, EventArgs e)
      {
         if (!await HasAnyClubWithStoreIdAsync())
         {
            Log("هیچ باشگاهی دارای StoreId (LDMA_CODE) نیست. ابتدا باشگاه‌ها را همگام‌سازی کنید.");
            MessageBox.Show("هیچ باشگاهی در لیدوما ثبت نشده است. ابتدا دکمه «ارسال باشگاه» را بزنید.", "پیش‌نیاز", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }
         await RunSendAsync(QueueItemType.Customer);
      }

      private async void btnSendAll_Click(object sender, EventArgs e)
      {
         await RunSendAllAsync();
      }

      private async Task<bool> HasAnyClubWithStoreIdAsync()
      {
         try
         {
            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               return iScscLocal.Clubs.Any(c => c.LDMA_CODE != null && c.LDMA_CODE != "");
            }
         }
         catch { return false; }
      }

      private async void btnCheckNow_Click(object sender, EventArgs e)
      {
         bool ok = await IsInternetAvailableAsync();
         SetConnectionStatus(ok);
         if (ok) await RunSendAllAsync();
      }

      // ============================================================
      // 9. منطق ارسال پس‌زمینه
      // ============================================================

      private async Task RunSendAsync(QueueItemType type)
      {
         if (!await _sendLock.WaitAsync(0))
         {
            Log("ارسال در حال انجام است؛ لطفاً صبر کنید.");
            return;
         }
         try { await SendByTypeCoreAsync(type); }
         finally { _sendLock.Release(); }
      }

      private async Task RunSendAllAsync()
      {
         if (!await _sendLock.WaitAsync(0))
         {
            Log("ارسال در حال انجام است؛ لطفاً صبر کنید.");
            return;
         }
         try
         {
            // 1. ابتدا باشگاه‌ها (Business)
            await SendByTypeCoreAsync(QueueItemType.Business);

            // 3. بررسی اینکه حداقل یک باشگاه StoreId داشته باشد
            bool hasClubWithStoreId = await HasAnyClubWithStoreIdAsync();
            if (!hasClubWithStoreId)
            {
               Log("هیچ باشگاهی StoreId (LDMA_CODE) ندارد. ارسال Services و Customers متوقف شد.");
               return;
            }

            // 4. سپس Services و Customers
            await SendByTypeCoreAsync(QueueItemType.Service);
            await SendByTypeCoreAsync(QueueItemType.Customer);
            await SendByTypeCoreAsync(QueueItemType.Expense);
            await SendByTypeCoreAsync(QueueItemType.Organ);
         }
         finally { _sendLock.Release(); }
      }

      private async Task SendByTypeCoreAsync(QueueItemType type)
      {
         if (!await EnsureLoggedInAsync())
         {
            Log("احراز هویت انجام نشده؛ ارسال لغو شد.");
            return;
         }

         // === مرحله ۱: ارسال آیتم‌های صف (دسته‌ای یا تکی) ===
         var items = _queue.Where(q => q.Type == type && q.Status != "Sent").ToList();
         if (items.Count > 0)
         {
            int done = 0;
            SetProgress(0, items.Count);
            Log(string.Format("شروع ارسال {0} مورد از نوع {1} از صف...", items.Count, type));

            if (type == QueueItemType.Customer)
            {
               for (int i = 0; i < items.Count; i += 50)
               {
                  var batch = items.Skip(i).Take(50).ToList();
                  var arr = new JArray(batch.Select(b => JObject.Parse(b.JsonPayload)));
                  JObject res = await _lidoma.CreateCustomersBulkAsync(arr);
                  bool ok = IsSuccess(res);
                  foreach (var it in batch)
                  {
                     it.Status = ok ? "Sent" : "Failed";
                     if (!ok) it.Error = res != null ? res.ToString() : "پاسخی دریافت نشد";
                  }
                  done += batch.Count;
                  SetProgress(done, items.Count);
                  Log((ok ? "ارسال دسته مشتریان موفق: " : "خطا در ارسال دسته مشتریان: ") + batch.Count);
                  if (!ok) break;
               }
            }
            else
            {
               foreach (var it in items)
               {
                  try
                  {
                     JObject payload = JObject.Parse(it.JsonPayload);
                     JObject res;
                     if (type == QueueItemType.Business)
                     {
                        res = await _lidoma.CreateStoreAsync(payload);
                     }
                     else if (type == QueueItemType.Service)
                     {
                        res = await _lidoma.CreateServiceAsync(payload);
                     }
                     else if (type == QueueItemType.Expense)
                     {
                        // Send individual expense via SetStoreRevenuesAsync
                        var storeIdToken = payload["storeId"];
                        var storeId = (storeIdToken != null) ? storeIdToken.ToString() : null;
                        if (string.IsNullOrEmpty(storeId))
                        {
                           res = JObject.FromObject(new { success = false, error = "storeId is required for expense" });
                        }
                        else
                        {
                           // Build revenues payload from single expense
                           var revenues = new JObject();
                           var reminderDaysToken = payload["reminderDays"];
                           var expenseType = (reminderDaysToken != null) ? "productSales" : "subscriptions";
                           var expenseArr = new JArray { payload };
                           revenues[expenseType] = expenseArr;
                           var revenuePayload = new JObject();
                           revenuePayload["revenues"] = revenues;
                           res = await _lidoma.SetStoreRevenuesAsync(storeId, revenuePayload);
                        }
                     }
                     else
                     {
                        res = await _lidoma.CreateServiceAsync(payload);
                     }

                     if (IsSuccess(res))
                     {
                        it.Status = "Sent";
                     }
                     else
                     {
                        it.Status = "Failed";
                        it.Error = res != null ? res.ToString() : "پاسخی دریافت نشد";
                     }
                  }
                  catch (Exception ex)
                  {
                     it.Status = "Failed";
                     it.Error = ex.Message;
                  }
                  done++;
                  SetProgress(done, items.Count);
                  Log(string.Format("ارسال {0} [{1}]: {2}", type, it.Id, it.Status));
               }
            }
            SaveQueue();
         }

         // === مرحله ۲: ارسال خودکار رکوردهای pending از پایگاه داده ===
         if (type == QueueItemType.Business)
         {
            await SyncClubsAsync();
         }
         else if (type == QueueItemType.Customer)
         {
            await SyncCustomersAsync();
         }
         else if (type == QueueItemType.Expense)
         {
            await SyncExpensesAsync();
         }
         else if (type == QueueItemType.Organ)
         {
            await SyncOrgansAsync();
         }

         SetStatusLabel("آخرین ارسال: " + DateTime.Now.ToString("HH:mm:ss"));
         SetProgress(0, 0);
         Log("پایان پردازش ارسال.");
      }

      private async Task SyncClubsAsync()
      {
         try
         {
            using (var iProjectLocal = new Data.iProjectDataContext(IProjectConnectionString))
            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               var settings = iProjectLocal.Message_Broad_Settings.FirstOrDefault(s => s.SERV_TYPE == "005");
               if (settings == null)
               {
                  Log("تنظیمات لیدوما (SERV_TYPE=005) یافت نشد.");
                  return;
               }

               List<Data.Club> all = iScscLocal.Clubs.ToList();
               Log(string.Format("تعداد کل باشگاه‌ها در دیتابیس: {0}", all.Count));
               foreach (var x in all)
                  Log(string.Format("  CODE={0}, NAME={1}, LDMA_STAT='{2}'", x.CODE, x.NAME, x.LDMA_STAT ?? "NULL"));

               // صرفاً بررسی LDMA_STAT باشگاه خود کافی است
               // اگر LDMA_STAT = '001' (جدید) یا '003' (به‌روزرسانی) باشد، باشگاه در صف قرار می‌گیرد
               // اگر هر جدول وابسته‌ای تغییر کرده باشد، Triggerهای دیتابیسی به‌صورت خودکار
               // LDMA_STAT باشگاه را به '003' تغییر می‌دهند
               List<Data.Club> pending = all.Where(c => (c.LDMA_STAT ?? "001") == "001" || c.LDMA_STAT == "003").ToList();

               if (pending.Count == 0)
               {
                  Log("باشگاهی برای همگام‌سازی یافت نشد.");
                  return;
               }

               Log(string.Format("شروع همگام‌سازی {0} باشگاه با لیدوما...", pending.Count));
               SetProgress(0, pending.Count);

               int done = 0;
               foreach (var c in pending.Take(1))
               {
                  try
                  {
                     string city = "";
                     var region = iScscLocal.Regions
                         .FirstOrDefault(r => r.PRVN_CNTY_CODE == c.REGN_PRVN_CNTY_CODE
                                           && r.PRVN_CODE == c.REGN_PRVN_CODE
                                           && r.CODE == c.REGN_CODE);
                     if (region != null) city = region.NAME ?? "";

                     var regl = iScscLocal.Regulations.FirstOrDefault(a => a.TYPE == "001" && a.REGL_STAT == "002");

                     var storeData = new JObject();
                     storeData.Add("name", c.CLUB_DESC ?? "");
                     storeData.Add("template", c.TEMP_TAG ?? "gym");
                     storeData.Add("about", c.CMNT ?? "");
                     storeData.Add("billingType", "percentage");
                     storeData.Add("ownerPhone", settings.WEB_SITE_LOGN ?? "");
                     storeData.Add("ownerPassword", settings.WEB_SITE_PSWD ?? "");
                     storeData.Add("city", city);

                     var regulation = new JObject();
                     regulation.Add("amntType", regl.AMNT_TYPE ?? "002");
                     regulation.Add("amntTypeDesc", iScscLocal.D_ATYPs.FirstOrDefault(a => a.VALU == regl.AMNT_TYPE).DOMN_DESC ?? "تومان");
                     storeData.Add("regulation", regulation);

                     var location = new JObject();
                     location.Add("lat", c.CORD_X ?? 0.0);
                     location.Add("lng", c.CORD_Y ?? 0.0);
                     storeData.Add("location", location);

                     var branch = new JObject();
                     branch.Add("name", c.NAME ?? "");
                     branch.Add("address", c.POST_ADRS ?? "");
                     branch.Add("phone", c.TELL_PHON ?? "");

                     var branchLoc = new JObject();
                     branchLoc.Add("lat", c.CORD_X ?? 0.0);
                     branchLoc.Add("lng", c.CORD_Y ?? 0.0);
                     branch.Add("location", branchLoc);

                     var contacts = new JArray();

                     if (!string.IsNullOrEmpty(c.CELL_PHON))
                     {
                        var contact = new JObject();
                        contact.Add("type", "whatsapp");
                        contact.Add("value", c.CELL_PHON);
                        contact.Add("label", "پشتیبانی");
                        contacts.Add(contact);
                     }
                     if (!string.IsNullOrEmpty(c.TELL_PHON))
                     {
                        var contact = new JObject();
                        contact.Add("type", "mobile");
                        contact.Add("value", c.TELL_PHON);
                        contact.Add("label", "تلفن ثابت");
                        contacts.Add(contact);
                     }
                     if (!string.IsNullOrEmpty(c.EMAL_ADRS))
                     {
                        var contact = new JObject();
                        contact.Add("type", "email");
                        contact.Add("value", c.EMAL_ADRS);
                        contact.Add("label", "ایمیل");
                        contacts.Add(contact);
                     }
                     if (!string.IsNullOrEmpty(c.WEB_SITE))
                     {
                        var contact = new JObject();
                        contact.Add("type", "website");
                        contact.Add("value", c.WEB_SITE);
                        contact.Add("label", "وب‌سایت");
                        contacts.Add(contact);
                     }
                     if (!string.IsNullOrEmpty(c.INST_PAGE))
                     {
                        var contact = new JObject();
                        contact.Add("type", "instagram");
                        contact.Add("value", c.INST_PAGE);
                        contact.Add("label", "اینستاگرام");
                        contacts.Add(contact);
                     }
                     if (!string.IsNullOrEmpty(c.ZIP_CODE))
                     {
                        var contact = new JObject();
                        contact.Add("type", "other");
                        contact.Add("value", c.ZIP_CODE);
                        contact.Add("label", "کد پستی");
                        contacts.Add(contact);
                     }

                     branch.Add("contacts", contacts);

                     var branches = new JArray();
                     branches.Add(branch);
                     storeData.Add("branches", branches);

                     var modules = new JArray();
                     modules.Add("menu");
                     modules.Add("customers");
                     storeData.Add("modules", modules);

                     // ===== services section =====
                     var services = new JObject();

                     // 1) متدهای باشگاه (دست‌نخالص: MTOD_CODE یکتا) - فقط Club_Method با MTOD_STAT='002'
                     var clubMethods = iScscLocal.Club_Methods
                         .Where(cm => /*cm.CLUB_CODE == c.CODE &&*/ cm.MTOD_STAT == "002").ToList();

                     var uniqueMethodCodes = clubMethods
                         .Where(cm => cm.MTOD_CODE != null)
                         .Select(cm => cm.MTOD_CODE.Value)
                         .Distinct()
                         .ToList();

                     var methodsArr = new JArray();
                     foreach (var mtodCode in uniqueMethodCodes)
                     {
                        // Method باید SHOW_STAT='002' و MTOD_STAT='002' داشته باشه
                        var method = iScscLocal.Methods.FirstOrDefault(m => m.CODE == mtodCode
                            && m.SHOW_STAT == "002" && m.MTOD_STAT == "002");
                        if (method == null) continue;

                        var methodObj = new JObject();
                        methodObj.Add("code", method.CODE.ToString());
                        methodObj.Add("natlcode", method.NATL_CODE ?? "");
                        methodObj.Add("name", method.MTOD_DESC ?? "");

                        // categories از Category_Belt با همین MTOD_CODE و SHOW_STAT='002' و CTGY_STAT='002'
                        var categoriesArr = new JArray();
                        var categoryBelts = iScscLocal.Category_Belts
                            .Where(cb => cb.MTOD_CODE == mtodCode
                                      && cb.SHOW_STAT == "002"
                                      && cb.CTGY_STAT == "002").ToList();
                        foreach (var cb in categoryBelts)
                        {
                           var catObj = new JObject();
                           catObj.Add("code", cb.CODE.ToString());
                           catObj.Add("natlcode", cb.NATL_CODE ?? "");
                           catObj.Add("name", cb.CTGY_DESC ?? "");
                           catObj.Add("numbOfAttnMont", cb.NUMB_OF_ATTN_MONT ?? 0);
                           catObj.Add("numbCyclDay", cb.NUMB_CYCL_DAY ?? 0);
                           catObj.Add("pric", cb.PRIC ?? 0);
                           categoriesArr.Add(catObj);
                        }
                        methodObj.Add("categories", categoriesArr);
                        methodsArr.Add(methodObj);
                     }

                     // اگر هیچ متد معتبری نیاورد، کلوب رو رد کن
                     if (!methodsArr.Any())
                     {
                        Log(string.Format("باشگاه {0} (CODE={1}) متد معتبری برای ارسال ندارد - رد شد", c.NAME, c.CODE));
                        done++;
                        SetProgress(done, pending.Count);
                        continue;
                     }

                     services.Add("methods", methodsArr);

                     // 2) مربی‌ها (Fighter با FGPB_TYPE_DNRM='003' و ACTV_TAG_DNRM='101')
                     var trainers = iScscLocal.Fighters
                         .Where(f => f.FGPB_TYPE_DNRM == "003"
                                  && f.ACTV_TAG_DNRM == "101"
                                  && f.CLUB_CODE_DNRM == c.CODE).ToList();

                     var personnelsArr = new JArray();
                     foreach (var t in trainers)
                     {
                        var serviceMethodsArr = new JArray();
                        var trainerMethods = clubMethods.Where(cm => cm.COCH_FILE_NO == t.FILE_NO).ToList();

                        foreach (var tm in trainerMethods)
                        {
                           // متد مربی باید MTOD_STAT='002' داشته باشه و متد اصلی باید SHOW_STAT='002' و MTOD_STAT='002' داشته باشه
                           if (tm.MTOD_CODE == null) continue;

                           var method = iScscLocal.Methods.FirstOrDefault(m => m.CODE == tm.MTOD_CODE
                               && m.SHOW_STAT == "002" && m.MTOD_STAT == "002");
                           if (method == null) continue;

                           var smObj = new JObject();
                           smObj.Add("code", tm.CODE.ToString());
                           smObj.Add("mtodCode", tm.MTOD_CODE ?? 0);
                           smObj.Add("clubCode", c.CODE.ToString());
                           smObj.Add("natlCode", tm.NATL_CODE ?? "");
                           smObj.Add("dayType", tm.DAY_TYPE ?? "");
                           smObj.Add("strtTime", tm.STRT_TIME != default(TimeSpan)
                               ? tm.STRT_TIME.ToString(@"hh\:mm")
                               : "");
                           smObj.Add("endTime", tm.END_TIME != default(TimeSpan)
                               ? tm.END_TIME.ToString(@"hh\:mm")
                               : "");
                           smObj.Add("mtodStat", tm.MTOD_STAT ?? "");
                           smObj.Add("sexType", tm.SEX_TYPE ?? "");
                           smObj.Add("cbmtDesc", tm.CBMT_DESC ?? "");
                           smObj.Add("cpctStat", tm.CPCT_STAT ?? "");
                           smObj.Add("cpctNumb", tm.CPCT_NUMB ?? 0);

                           var weekDaysArr = new JArray();
                           var weekDays = iScscLocal.Club_Method_Weekdays
                               .Where(w => w.CBMT_CODE == tm.CODE && w.STAT == "002").ToList();
                           foreach (var wd in weekDays)
                           {
                              if (!string.IsNullOrEmpty(wd.WEEK_DAY))
                                 weekDaysArr.Add(wd.WEEK_DAY);
                           }
                           smObj.Add("weekDays", weekDaysArr);
                           serviceMethodsArr.Add(smObj);
                        }

                        var persObj = new JObject();
                        persObj.Add("frstName", t.FRST_NAME_DNRM ?? "");
                        persObj.Add("lastName", t.LAST_NAME_DNRM ?? "");
                        persObj.Add("fileNo", t.FILE_NO.ToString());
                        persObj.Add("sexType", t.SEX_TYPE_DNRM ?? "");
                        persObj.Add("brthDate", t.BRTH_DATE_DNRM.HasValue
                            ? t.BRTH_DATE_DNRM.Value.ToString("yyyy-MM-dd")
                            : "");
                        persObj.Add("cellPhon", t.CELL_PHON_DNRM ?? "");
                        persObj.Add("natlCode", t.NATL_CODE_DNRM ?? "");
                        persObj.Add("chatId", "");
                        persObj.Add("serviceMethods", serviceMethodsArr);
                        personnelsArr.Add(persObj);
                     }
                     services.Add("personnels", personnelsArr);

                     storeData.Add("services", services);
                     // ===== end services =====

                     JObject res;
                     bool isFirstSync = string.IsNullOrEmpty(c.LDMA_CODE);
                     if (!isFirstSync)
                     {
                        res = await _lidoma.UpdateStoreAsync(c.LDMA_CODE, storeData);
                     }
                     else
                     {
                        res = await _lidoma.CreateStoreAsync(storeData);
                     }

                     if (IsSuccess(res))
                     {
                        c.LDMA_STAT = "002";
                        c.LDMA_DATE = DateTime.Now;
                        if (res["entries"] != null && res["entries"]["storeId"] != null)
                        {
                           c.LDMA_CODE = (string)res["entries"]["storeId"];
                        }
                        Log(string.Format("ارسال موفق: {0} (CODE={1}) storeId={2}", c.NAME, c.CODE, c.LDMA_CODE ?? "-"));
                     }
                     else
                     {
                        Log(string.Format("خطا در ارسال {0} (CODE={1}): {2}", c.NAME, c.CODE, res.ToString()));
                     }
                  }
                  catch (Exception ex)
                  {
                     Log(string.Format("خطا در ارسال باشگاه (CODE={0}): {1}", c.CODE, ex.Message));
                  }

                  done++;
                  SetProgress(done, pending.Count);
               }

               iScscLocal.SubmitChanges();
               Log(string.Format("همگام‌سازی باشگاه‌ها پایان یافت. {0} مورد پردازش شد.", done));
            }
         }
         catch (Exception ex)
         {
            Log("خطا در همگام‌سازی باشگاه‌ها: " + ex.Message);
         }
      }

      private async Task SyncExpensesAsync()
      {
         try
         {
            if (!await EnsureLoggedInAsync())
            {
               Log("اتصال به لیدوما انجام نشد. همگام‌سازی هزینه‌ها لغو شد.");
               return;
            }

            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               // 1. چک کن آیا حداقل یک Club LDMA_CODE داره
               var clubs = iScscLocal.Clubs
                   .Where(c => c.LDMA_CODE != null && c.LDMA_CODE != "")
                   .ToList();

               if (clubs.Count == 0)
               {
                  Log("هیچ باشگاهی StoreId (LDMA_CODE) ندارد. ابتدا باشگاه‌ها را همگام‌سازی کنید.");
                  return;
               }

               // 2. تمام Expenseهای pending و فعال
               var pendingExpenses = iScscLocal.Expenses
                   .Where(e => e.EXPN_STAT == "002" &&
                               e.Method.MTOD_STAT == "002" && e.Method.SHOW_STAT == "002" &&
                               e.Category_Belt.CTGY_STAT == "002" && e.Category_Belt.CTGY_STAT == "002" &&
                               iScscLocal.Club_Methods.Any(a => a.MTOD_CODE == e.MTOD_CODE && a.MTOD_STAT == "002") &&
                              ((e.LDMA_STAT ?? "001") == "001" || e.LDMA_STAT == "003"))
                   .ToList();

               if (pendingExpenses.Count == 0)
               {
                  Log("هزینه/درآمدی برای همگام‌سازی یافت نشد.");
                  return;
               }

               Log(string.Format("تعداد کل هزینه/درآمدهای pending: {0}", pendingExpenses.Count));

               // 3. تفکیک به دو دسته
               var subItems = pendingExpenses
                   .Where(e => e.Expense_Type.Request_Requester.RQTP_CODE == "001" || e.Expense_Type.Request_Requester.RQTP_CODE == "009")
                   .Select(e => new
                   {
                      code = e.CODE.ToString(),
                      groupCode = e.MTOD_CODE.ToString(),
                      categoryCode = e.CTGY_CODE.ToString(),
                      description = (e.Method.MTOD_DESC + " - " + e.Category_Belt.CTGY_DESC) ?? "",
                      price = e.PRIC,
                      sessionCount = e.NUMB_OF_ATTN_MONT,
                      cycleDays = e.NUMB_CYCL_DAY,
                      hasFiscalId = e.EXPN_IDTY_STAT == "002",
                      fiscalId = e.EXPN_IDTY_STAT == "002" && e.EXPN_IDTY_VALU != null
                                  ? e.EXPN_IDTY_VALU.ToString() : ""
                   })
                   .ToList();

               var psItems = pendingExpenses
                  .Where(e => e.Expense_Type.Request_Requester.RQTP_CODE == "016")
                  .Select(e => new
                  {
                     code = e.CODE.ToString(),
                     groupCode = e.MTOD_CODE.ToString(),
                     categoryCode = e.CTGY_CODE.ToString(),
                     description = (e.Method.MTOD_DESC + " - " + e.Category_Belt.CTGY_DESC) ?? "",
                     price = e.PRIC,
                     reminderDays = e.NUMB_CYCL_DAY,
                     hasFiscalId = e.EXPN_IDTY_STAT == "002",
                     fiscalId = e.EXPN_IDTY_STAT == "002" && e.EXPN_IDTY_VALU != null
                                 ? e.EXPN_IDTY_VALU.ToString() : ""
                  })
                  .ToList();

               if (subItems.Count == 0 && psItems.Count == 0)
               {
                  Log("هیچ هزینه/درآمد معتبری برای ارسال وجود ندارد.");
                  return;
               }

               var revenues = new JObject();
               revenues["subscriptions"] = JToken.FromObject(subItems);
               revenues["productSales"] = JToken.FromObject(psItems);

               var payload = new JObject();
               payload["revenues"] = revenues;

               // 4. برای هر Club که StoreId داره، ارسال کن
               int totalDone = 0;

               foreach (var club in clubs.Take(1))
               {
                  try
                  {
                     Log(string.Format("در حال ارسال هزینه‌ها برای باشگاه: {0} (StoreId: {1})", club.NAME, club.LDMA_CODE));
                     var res = await _lidoma.SetStoreRevenuesAsync(club.LDMA_CODE, payload);

                     bool syncOk = IsSuccess(res);
                     if (syncOk)
                     {
                        // تمام pending expenses رو mark کن
                        foreach (var exp in pendingExpenses)
                           exp.LDMA_STAT = "002";

                        iScscLocal.SubmitChanges();
                        totalDone = pendingExpenses.Count;

                        Log(string.Format("باشگاه {0} با موفقیت همگام شد. ({1} هزینه)", club.NAME, pendingExpenses.Count));
                     }
                     else
                     {
                        Log(string.Format("خطا در ارسال هزینه‌ها برای باشگاه {0}: {1}",
                            club.NAME, res["error"] != null ? res["error"].ToString() : "خطای ناشناس"));
                     }
                  }
                  catch (Exception ex)
                  {
                     Log(string.Format("خطا در پردازش باشگاه {0}: {1}", club.NAME, ex.Message));
                  }
               }

               Log(string.Format("همگام‌سازی هزینه‌ها تمام شد. کل {0} هزینه پردازش شد.", totalDone));
            }
         }
         catch (Exception ex)
         {
            Log("خطا در SyncExpensesAsync: " + ex.Message);
         }
      }

      private async Task SyncCustomersAsync()
      {
         try
         {
            if (!await EnsureLoggedInAsync())
            {
               Log("اتصال به لیدوما انجام نشد. همگام‌سازی مشتریان لغو شد.");
               return;
            }

            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               // 1. اول بررسی کن کدام باشگاه‌ها StoreId (LDMA_CODE) دارند
               var clubs = iScscLocal.Clubs
                   .Where(a => a.LDMA_CODE != null && a.LDMA_CODE != "")
                   .ToList();

               if (clubs.Count == 0)
               {
                  Log("هیچ باشگاهی StoreId ندارد. ابتدا باشگاه‌ها را همگام‌سازی کنید.");
                  return;
               }

               // 2. مشتریان (Fighter) این باشگاه‌ها را پیدا کن
               var clubCodes = clubs.Select(c => c.CODE).ToList();
               List<Data.Fighter> all = iScscLocal.Fighters
                  //.Where(f => f.CLUB_CODE_DNRM.HasValue && clubCodes.Contains(f.CLUB_CODE_DNRM.Value))
                   .ToList();

               Log(string.Format("تعداد کل اعضا/مشتریان در دیتابیس: {0}", all.Count));

               List<Data.Fighter> pending = all
                   .Where(c => (c.LDMA_STAT ?? "001") == "001" || c.LDMA_STAT == "003")
                   .ToList();

               if (pending.Count == 0)
               {
                  Log("عضو/مشتری‌ای برای همگام‌سازی یافت نشد.");
                  return;
               }

               Log(string.Format("شروع همگام‌سازی {0} عضو با لیدوما (به صورت بسته‌های تطبیقی)...", pending.Count));
               SetProgress(0, pending.Count);

               int done = 0;
               int currentBatchSize = 50;

               // Throttling/Delay configuration
               int defaultDelayMs = 3000;
               int minDelayMs = 1000;
               int maxDelayMs = 5000;

               var cfgDelay = ConfigurationManager.AppSettings["BatchDelayMs"];
               if (cfgDelay != null)
               {
                  int parsed;
                  if (Int32.TryParse(cfgDelay, out parsed) && parsed > 0)
                     defaultDelayMs = parsed;
               }
               var cfgMin = ConfigurationManager.AppSettings["MinBatchDelayMs"];
               if (cfgMin != null)
               {
                  int parsed;
                  if (Int32.TryParse(cfgMin, out parsed) && parsed > 0)
                     minDelayMs = parsed;
               }
               var cfgMax = ConfigurationManager.AppSettings["MaxBatchDelayMs"];
               if (cfgMax != null)
               {
                  int parsed;
                  if (Int32.TryParse(cfgMax, out parsed) && parsed > 0)
                     maxDelayMs = parsed;
               }

               for (int i = 0; i < pending.Count; i += currentBatchSize)
               {
                  var batch = pending.Skip(i).Take(currentBatchSize).ToList();

                  // Split batch into new customers (no LDMA_CODE) and existing customers (has LDMA_CODE)
                  var newCustomers = new List<Data.Fighter>();
                  var existingCustomers = new List<Data.Fighter>();

                  foreach (var f in batch)
                  {
                     // فیلترهای مشتری
                     if (f.FGPB_TYPE_DNRM != "001" && f.FGPB_TYPE_DNRM != "005") continue;    // مشتری - مهمان
                     if (f.ACTV_TAG_DNRM != "101") continue;      // فعال
                     if (f.CONF_STAT != "002") continue;         // تأیید شده
                     if (f.FGPB_TYPE_DNRM == "001" && (f.CELL_PHON_DNRM == null || f.CELL_PHON_DNRM == "" || f.CELL_PHON_DNRM.Length != 11)) continue; // شماره موبایل الزامی برای مشتریان واقعی
                     if (String.IsNullOrEmpty(f.DAD_CELL_PHON_DNRM) || f.DAD_CELL_PHON_DNRM.Length != 11) f.DAD_CELL_PHON_DNRM = "";
                     if (String.IsNullOrEmpty(f.MOM_CELL_PHON_DNRM) || f.MOM_CELL_PHON_DNRM.Length != 11) f.MOM_CELL_PHON_DNRM = "";

                     if (f.LDMA_CODE == null || f.LDMA_CODE == "")
                        newCustomers.Add(f);
                     else
                        existingCustomers.Add(f);
                  }

                  bool batchSuccess = true;

                  // 1. Process new customers via CreateCustomersBulkAsync with adaptive batch size
                  if (newCustomers.Count > 0)
                  {
                     var requestPayload = BuildCustomerBulkPayload(newCustomers, clubs);
                     if (requestPayload == null)
                     {
                        Log("عدم امکان ساخت درخواست: هیچ باشگاهی برای مشتریان یافت نشد.");
                        batchSuccess = false;
                        // Mark all new customers as failed since we couldn't build the request
                        foreach (var f in newCustomers)
                        {
                           f.LDMA_STAT = "004";
                           f.LDMA_DATE = DateTime.Now;
                        }
                     }
                     else
                     {
                        JObject res = await _lidoma.CreateCustomersBulkAsync(requestPayload);

                        if (IsSuccess(res))
                        {
                           // Track which customers were successfully processed via phone matching
                           List<Data.Fighter> matchedCustomers = new List<Data.Fighter>();

                           var entries = res["entries"] as JArray;
                           if (entries != null)
                           {
                              foreach (var entry in entries)
                              {
                                 var phoneToken = entry["phone"];
                                 var customerIdToken = entry["customerId"];
                                 var phone = (phoneToken != null) ? phoneToken.ToString() : null;
                                 var customerId = (customerIdToken != null) ? customerIdToken.ToString() : null;
                                 if (!String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(customerId))
                                 {
                                    var fighter = newCustomers.FirstOrDefault(f => f.CELL_PHON_DNRM == phone);
                                    if (fighter != null)
                                    {
                                       fighter.LDMA_CODE = customerId;
                                       fighter.LDMA_STAT = "002";
                                       fighter.LDMA_DATE = DateTime.Now;
                                       matchedCustomers.Add(fighter);
                                    }
                                 }
                                 else
                                 {
                                    // Entry with null/empty phone or customerId — mark as failed
                                    Log(String.Format("ورودی entries دارای phone یا customerId خالی: phone={0}, customerId={1}",
                                        String.IsNullOrEmpty(phone) ? "null/empty" : phone,
                                        String.IsNullOrEmpty(customerId) ? "null/empty" : customerId));
                                 }
                              }
                           }
                           else
                           {
                              // entries array is null — fallback: mark all as success (API returned no entries)
                              Log("API returned success but no entries array — marking all new customers as sent (fallback).");
                              foreach (var f in newCustomers)
                              {
                                 f.LDMA_STAT = "002";
                                 f.LDMA_DATE = DateTime.Now;
                              }
                           }

                           // Mark any new customers NOT in the matched list as FAILED (individual failure)
                           var unmatchedNew = newCustomers.Except(matchedCustomers).ToList();
                           foreach (var f in unmatchedNew)
                           {
                              f.LDMA_STAT = "004";
                              f.LDMA_DATE = DateTime.Now;
                              Log(String.Format("مشتری جدید ارسال نشد (phone mismatch): fileNo={0}", f.FILE_NO));
                           }

                           Log(String.Format("ایجاد موفق {0} مشتری جدید ({1} نامطابقت)", newCustomers.Count, unmatchedNew.Count));
                        }
                        else
                        {
                           batchSuccess = false;
                           Log(String.Format("خطا در ایجاد مشتریان جدید: {0}", res != null ? res.ToString() : "پاسخی دریافت نشد"));
                        }
                     }
                  }

                  // 2. Process existing customers via UpdateCustomerAsync
                  foreach (var f in existingCustomers)
                  {
                     if (f.DEBT_DNRM < 0)
                     {
                        f.DPST_AMNT_DNRM += f.DEBT_DNRM * -1;
                        f.DEBT_DNRM = 0;
                     }

                     var customerObj = new JObject();
                     customerObj.Add("fileNo", f.FILE_NO.ToString());
                     customerObj.Add("firstName", f.FRST_NAME_DNRM ?? "");
                     customerObj.Add("lastName", f.LAST_NAME_DNRM ?? "");
                     customerObj.Add("fatherName", f.FATH_NAME_DNRM ?? "");
                     customerObj.Add("debtAmount", f.DEBT_DNRM.HasValue ? f.DEBT_DNRM.Value.ToString() : "0");
                     customerObj.Add("depositAmount", f.DPST_AMNT_DNRM.HasValue ? f.DPST_AMNT_DNRM.Value.ToString() : "0");
                     customerObj.Add("confirmedAt", f.CONF_DATE.HasValue
                         ? f.CONF_DATE.Value.ToString("yyyy/MM/dd hh:mm:ss tt")
                         : "");
                     customerObj.Add("gender", (f.SEX_TYPE_DNRM ?? "") == "001" ? "male" : "female");
                     customerObj.Add("birthDate", f.BRTH_DATE_DNRM.HasValue
                         ? f.BRTH_DATE_DNRM.Value.ToString("yyyy/MM/dd")
                         : "");
                     customerObj.Add("phone", f.CELL_PHON_DNRM ?? "");
                     customerObj.Add("landline", f.TELL_PHON_DNRM ?? "");
                     customerObj.Add("insuranceNumber", f.INSR_NUMB_DNRM ?? "");
                     customerObj.Add("insuranceExpiresAt", f.INSR_DATE_DNRM.HasValue
                         ? f.INSR_DATE_DNRM.Value.ToString("yyyy/MM/dd")
                         : "");
                     customerObj.Add("fingerprintCode", f.FNGR_PRNT_DNRM ?? "");
                     customerObj.Add("organizationCode", f.ORGN_CODE_DNRM ?? "0000000000");
                     customerObj.Add("subscriptionNo", f.SERV_NO_DNRM ?? "");
                     customerObj.Add("nationlCode", f.INSR_NUMB_DNRM ?? "");
                     customerObj.Add("fatherMobile", f.DAD_CELL_PHON_DNRM ?? "");
                     customerObj.Add("dadTellPhon", f.DAD_TELL_PHON_DNRM ?? "");
                     customerObj.Add("motherMobile", f.MOM_CELL_PHON_DNRM ?? "");
                     customerObj.Add("momTellPhon", f.MOM_TELL_PHON_DNRM ?? "");
                     customerObj.Add("bankName", f.DPST_ACNT_SLRY_BANK_DNRM ?? "");
                     customerObj.Add("bankAccount", f.DPST_ACNT_SLRY_DNRM ?? "");

                     var resUpdate = await _lidoma.UpdateCustomerAsync(f.LDMA_CODE, customerObj);

                     if (IsSuccess(resUpdate))
                     {
                        f.LDMA_STAT = "002";
                        f.LDMA_DATE = DateTime.Now;
                        Log(String.Format("به‌روزرسانی موفق مشتری: fileNo={0}", f.FILE_NO));
                     }
                     else
                     {
                        batchSuccess = false;
                        Log(String.Format("خطا در به‌روزرسانی مشتری fileNo={0}: {1}",
                            f.FILE_NO, resUpdate != null ? resUpdate.ToString() : "پاسخی دریافت نشد"));
                     }
                  }

                  iScscLocal.SubmitChanges();

                  if (batchSuccess)
                  {
                     // Increase batch size by 2x on success, up to maximum of 200 customers per request
                     int maxBatchSize = 200;
                     var maxBatchConfig = ConfigurationManager.AppSettings["MaxBatchSize"];
                     if (maxBatchConfig != null)
                     {
                        int parsedMax;
                        if (Int32.TryParse(maxBatchConfig, out parsedMax) && parsedMax > 0)
                           maxBatchSize = parsedMax;
                     }

                     currentBatchSize = Math.Min(currentBatchSize * 2, maxBatchSize);
                     if (currentBatchSize > 50)
                        Log(String.Format("Batch size increased to {0} (adaptive growth)", currentBatchSize));
                  }
                  else
                  {
                     // Batch failed: reduce batch size and mark only unprocessed customers as failed
                     if (currentBatchSize > 1)
                     {
                        int previousBatchSize = currentBatchSize;
                        currentBatchSize = Math.Max(currentBatchSize / 2, 1);
                        Log(String.Format("Batch failed at size {0}; reducing to {1}", previousBatchSize, currentBatchSize));

                        // Mark only NEW customers that weren't successfully sent as '004'
                        // (customers already marked '002' above should remain '002')
                        foreach (var f in newCustomers)
                        {
                           // Only mark as failed if NOT already marked as '002' (success)
                           if ((f.LDMA_STAT ?? "001") != "002")
                           {
                              f.LDMA_STAT = "004";
                              f.LDMA_DATE = DateTime.Now;
                           }
                        }
                        // Mark only EXISTING customers that weren't successfully updated as '004'
                        foreach (var f in existingCustomers)
                        {
                           // Only mark as failed if NOT already marked as '002' (success)
                           if ((f.LDMA_STAT ?? "001") != "002")
                           {
                              f.LDMA_STAT = "004";
                              f.LDMA_DATE = DateTime.Now;
                           }
                        }
                        iScscLocal.SubmitChanges();

                        // Roll back the loop index so the failing customers are retried with the smaller batch size
                        i = i - previousBatchSize;
                     }
                     else
                     {
                        // Already at batch size 1 and still failing: mark remaining as '004'
                        // Customers already marked '002' remain successful
                        foreach (var f in newCustomers)
                        {
                           if ((f.LDMA_STAT ?? "001") != "002")
                           {
                              f.LDMA_STAT = "004";
                              f.LDMA_DATE = DateTime.Now;
                           }
                        }
                        foreach (var f in existingCustomers)
                        {
                           if ((f.LDMA_STAT ?? "001") != "002")
                           {
                              f.LDMA_STAT = "004";
                              f.LDMA_DATE = DateTime.Now;
                           }
                        }
                        iScscLocal.SubmitChanges();
                        Log("Batch size already at 1 and still failing. Failed customers remain marked as LDMA_STAT='004'.");
                     }
                  }
                  done += newCustomers.Count + existingCustomers.Count;
                  SetProgress(done, pending.Count);

                  // Throttling: Delay before next batch to prevent system freeze
                  // Dynamic delay: larger batches get longer delay, smaller batches get shorter delay
                  int delayMs;
                  if (currentBatchSize >= 200)
                  {
                     delayMs = maxDelayMs;
                  }
                  else if (currentBatchSize >= 100)
                  {
                     delayMs = (minDelayMs + maxDelayMs) / 2;
                  }
                  else if (currentBatchSize >= 50)
                  {
                     delayMs = defaultDelayMs;
                  }
                  else
                  {
                     delayMs = Math.Max(minDelayMs / 2, 500);
                  }

                  SetStatusLabel(String.Format("در حال ارسال... ({0}/{1}) - استراحت {2}ms", done, pending.Count, delayMs));
                  Log(String.Format("Delay {0}ms before next batch (size={1})...", delayMs, currentBatchSize));
                  await Task.Delay(delayMs);
               }

               Log(string.Format("همگام‌سازی اعضا پایان یافت. {0} مورد پردازش شد.", done));
            }
         }
         catch (Exception ex)
         {
            Log("خطا در همگام‌سازی اعضا: " + ex.Message);
         }
      }

      private JObject BuildCustomerBulkPayload(List<Data.Fighter> customers, List<Data.Club> clubs)
      {
         var customersArr = new JArray();
         foreach (var f in customers)
         {
            if (f.DEBT_DNRM < 0)
            {
               f.DPST_AMNT_DNRM += f.DEBT_DNRM * -1;
               f.DEBT_DNRM = 0;
            }

            var customerObj = new JObject();
            customerObj.Add("fileNo", f.FILE_NO.ToString());
            customerObj.Add("firstName", f.FRST_NAME_DNRM ?? "");
            customerObj.Add("lastName", f.LAST_NAME_DNRM ?? "");
            customerObj.Add("fatherName", f.FATH_NAME_DNRM ?? "");
            customerObj.Add("debtAmount", f.DEBT_DNRM.HasValue ? f.DEBT_DNRM.Value.ToString() : "0");
            customerObj.Add("depositAmount", f.DPST_AMNT_DNRM.HasValue ? f.DPST_AMNT_DNRM.Value.ToString() : "0");
            customerObj.Add("confirmedAt", f.CONF_DATE.HasValue
                ? f.CONF_DATE.Value.ToString("yyyy/MM/dd hh:mm:ss tt")
                : "");
            customerObj.Add("gender", (f.SEX_TYPE_DNRM ?? "") == "001" ? "male" : "female");
            customerObj.Add("birthDate", f.BRTH_DATE_DNRM.HasValue
                ? f.BRTH_DATE_DNRM.Value.ToString("yyyy/MM/dd")
                : "");
            customerObj.Add("phone", f.CELL_PHON_DNRM ?? "");
            customerObj.Add("landline", f.TELL_PHON_DNRM ?? "");
            customerObj.Add("insuranceNumber", f.INSR_NUMB_DNRM ?? "");
            customerObj.Add("insuranceExpiresAt", f.INSR_DATE_DNRM.HasValue
                ? f.INSR_DATE_DNRM.Value.ToString("yyyy/MM/dd")
                : "");
            customerObj.Add("fingerprintCode", f.FNGR_PRNT_DNRM ?? "");
            customerObj.Add("organizationCode", f.ORGN_CODE_DNRM ?? "0000000000");
            customerObj.Add("subscriptionNo", f.SERV_NO_DNRM ?? "");
            customerObj.Add("nationlCode", f.INSR_NUMB_DNRM ?? "");
            customerObj.Add("fatherMobile", f.DAD_CELL_PHON_DNRM ?? "");
            customerObj.Add("dadTellPhon", f.DAD_TELL_PHON_DNRM ?? "");
            customerObj.Add("motherMobile", f.MOM_CELL_PHON_DNRM ?? "");
            customerObj.Add("momTellPhon", f.MOM_TELL_PHON_DNRM ?? "");
            customerObj.Add("bankName", f.DPST_ACNT_SLRY_BANK_DNRM ?? "");
            customerObj.Add("bankAccount", f.DPST_ACNT_SLRY_DNRM ?? "");

            customersArr.Add(customerObj);
         }

         //var firstWithClub = customers.FirstOrDefault(f => f.CLUB_CODE_DNRM.HasValue);
         //if (firstWithClub == null) return null;
         var club = clubs.FirstOrDefault(c => c.LDMA_CODE != null);
         if (club == null) return null;

         var requestPayload = new JObject();
         requestPayload.Add("storeId", club.LDMA_CODE);
         requestPayload.Add("customers", customersArr);
         return requestPayload;
      }

      private async Task SyncOrgansAsync()
      {
         try
         {
            if (!await EnsureLoggedInAsync())
            {
               Log("اتصال به لیدوما انجام نشد. همگام‌سازی ارگان‌ها لغو شد.");
               return;
            }

            using (var iScscLocal = new Data.iScscDataContext(IScscConnectionString))
            {
               // 1. Check if any club has a store ID (LDMA_CODE)
               var clubsWithStoreId = iScscLocal.Clubs
                   .Where(c => c.LDMA_CODE != null && c.LDMA_CODE != "")
                   .ToList();

               if (clubsWithStoreId.Count == 0)
               {
                  Log("هیچ باشگاهی StoreId (LDMA_CODE) ندارد. ابتدا باشگاه‌ها را همگام‌سازی کنید.");
                  return;
               }

               Log(string.Format("شروع همگام‌سازی ارگان‌ها برای {0} باشگاه...", clubsWithStoreId.Count));
               SetProgress(0, clubsWithStoreId.Count);

               int done = 0;
               foreach (var club in clubsWithStoreId.Take(1))
               {
                  try
                  {
                     var storeSlug = club.LDMA_CODE;
                     if (string.IsNullOrEmpty(storeSlug))
                     {
                        Log(string.Format("باشگاه {0} (CODE={1}) StoreId ندارد - رد شد", club.NAME, club.CODE));
                        done++;
                        SetProgress(done, clubsWithStoreId.Count);
                        continue;
                     }

                     // Read data from database
                     Log(string.Format("در حال ساخت JSON ارگان‌ها برای باشگاه {0}...", storeSlug));

                     // Check if any organ has a ready for send
                     var _subunits = iScscLocal.Sub_Units
                        .Where(a => a.LDMA_STAT == null || a.LDMA_STAT == "003")
                        .ToList();

                     if (_subunits.Count == 0)
                     {
                        Log("هیچ سازمان و ارگانی برای ارسال وجود ندارد");
                        return;
                     }

                     var organs = await GetOrgansFromDatabaseAsync();
                     var subscriptionDiscounts = await GetSubscriptionDiscountsFromDatabaseAsync();
                     var productSalesDiscounts = await GetProductSalesDiscountsFromDatabaseAsync();

                     // Build the complete JSON structure
                     var discountsObj = new JObject(
                         new JProperty("subscriptions", subscriptionDiscounts),
                         new JProperty("productSales", productSalesDiscounts)
                     );

                     var organsData = new JObject(
                         new JProperty("organs", new JObject(
                             new JProperty("items", organs),
                             new JProperty("discounts", discountsObj)
                         ))
                     );

                     Log("در حال ارسال ارگان‌ها به Lidoma API...");
                     var res = await _lidoma.SetStoreOrgansAsync(storeSlug, organsData);

                     if (IsSuccess(res))
                     {
                        Log(string.Format("ارسال ارگان‌ها برای باشگاه {0} موفق بود", storeSlug));

                        // Update LDMA_STAT = '002' (Completed) for all pending record
                        foreach (var organ in iScscLocal.Sub_Units.Where(o => (o.LDMA_STAT == null || o.LDMA_STAT == "003")).ToList())
                           organ.LDMA_STAT = "002";

                        foreach (var sub in iScscLocal.Basic_Calculate_Discounts
                           .Where(d => (d.RQTP_CODE == "001" || d.RQTP_CODE == "009") && (d.LDMA_STAT == null || d.LDMA_STAT == "003")).ToList())
                           sub.LDMA_STAT = "002";

                        foreach (var prod in iScscLocal.Basic_Calculate_Discounts
                           .Where(d => d.RQTP_CODE == "016" && d.Expense.EXPN_STAT == "002" && (d.LDMA_STAT == null || d.LDMA_STAT == "003")).ToList())
                           prod.LDMA_STAT = "002";
                     }
                     else
                     {
                        Log(string.Format("خطا در ارسال ارگان‌ها برای باشگاه {0}: {1}", storeSlug, res != null ? res.ToString() : "پاسخی دریافت نشد"));

                        //// Update LDMA_STAT = '004' (Failed) for all pending record
                        //foreach (var organ in iScscLocal.Sub_Units.Where(o => o.LDMA_STAT == "003").ToList())
                        //   organ.LDMA_STAT = "004";

                        //foreach (var sub in iScscLocal.Basic_Calculate_Discounts
                        //   .Where(d => (d.RQTP_CODE == "001" || d.RQTP_CODE == "009") && d.LDMA_STAT == "003").ToList())
                        //   sub.LDMA_STAT = "004";

                        //foreach (var prod in iScscLocal.Basic_Calculate_Discounts
                        //   .Where(d => d.RQTP_CODE == "016" && d.Expense.EXPN_STAT == "002" && d.LDMA_STAT == "003").ToList())
                        //   prod.LDMA_STAT = "004";
                     }
                  }
                  catch (Exception ex)
                  {
                     Log(string.Format("خطا در همگام‌سازی ارگان‌ها برای باشگاه: {0}", ex.Message));
                  }

                  done++;
                  SetProgress(done, clubsWithStoreId.Count);
               }

               iScscLocal.SubmitChanges();
               Log(string.Format("همگام‌سازی ارگان‌ها پایان یافت. {0} مورد پردازش شد.", done));
            }
         }
         catch (Exception ex)
         {
            Log("خطا در همگام‌سازی ارگان‌ها: " + ex.Message);
         }
      }

      private bool IsSuccess(JObject res)
      {
         if (res == null || res["return"]["status"] == null) return false;
         if (res["return"]["status"].ToString() == "200") return true;
         else return false;
         //return (bool)res["success"];
      }

      // ============================================================
      // 10. مدیریت صف و ذخیرهٔ آفلاین (JSON)
      // ============================================================

      public void EnqueueBusiness(BusinessModel model) { AddItem(QueueItemType.Business, model); }
      public void EnqueueService(ServiceModel model) { AddItem(QueueItemType.Service, model); }
      public void EnqueueCustomer(CustomerModel model) { AddItem(QueueItemType.Customer, model); }
      public void EnqueueExpense(ExpenseModel model) { AddItem(QueueItemType.Expense, model); }

      private void AddItem(QueueItemType type, object model)
      {
         _queue.Add(new QueueItem
         {
            Id = Guid.NewGuid(),
            Type = type,
            JsonPayload = JsonConvert.SerializeObject(model),
            Status = "Pending",
            CreatedAt = DateTime.Now
         });
         SaveQueue();
         Log("افزوده شد به صف: " + type);
      }

      private void SaveQueue()
      {
         try
         {
            if (_queue != null)
               File.WriteAllText(_queueFile, JsonConvert.SerializeObject(_queue.ToList()));
         }
         catch (Exception ex) { Log("خطا در ذخیرهٔ صف: " + ex.Message); }
      }

      private void LoadQueue()
      {
         try
         {
            if (File.Exists(_queueFile))
            {
               var list = JsonConvert.DeserializeObject<List<QueueItem>>(File.ReadAllText(_queueFile));
               if (list != null)
                  foreach (var it in list) _queue.Add(it);
            }
         }
         catch (Exception ex) { Log("خطا در بارگذاری صف: " + ex.Message); }
      }

      // ============================================================
      // 11. به‌روزرسانی رابط کاربری (با رعایت thread-safety)
      // ============================================================

      private void Log(string message)
      {
         Action act = () => richTextBoxLog.AppendText(
             DateTime.Now.ToString("HH:mm:ss") + " - " + message + Environment.NewLine);
         if (InvokeRequired) Invoke(act); else act();
      }

      private void SetProgress(int value, int maximum)
      {
         Action act = () =>
         {
            progressBar1.Maximum = Math.Max(1, maximum);
            progressBar1.Value = Math.Min(value, progressBar1.Maximum);
            toolStripProgressBar1.Maximum = Math.Max(1, maximum);
            toolStripProgressBar1.Value = Math.Min(value, toolStripProgressBar1.Maximum);
         };
         if (InvokeRequired) Invoke(act); else act();
      }

      private void SetConnectionStatus(bool online)
      {
         Action act = () => toolStripStatusLabelConn.Text = online ? "وضعیت: آنلاین" : "وضعیت: آفلاین";
         if (InvokeRequired) Invoke(act); else act();
      }

      private void SetStatusLabel(string text)
      {
         Action act = () => lblStatus.Text = text;
         if (InvokeRequired) Invoke(act); else act();
      }
   }
}