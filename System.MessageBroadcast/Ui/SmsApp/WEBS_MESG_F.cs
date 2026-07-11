using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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

        public enum QueueItemType { Business, Service, Customer }

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

        // ============================================================
        // 2. فیلدها
        // ============================================================

        private LidomaMarket _lidoma;
        private string _baseUrl;
        private System.Windows.Forms.Timer _netTimer;
        private readonly SemaphoreSlim _sendLock = new SemaphoreSlim(1, 1);
        private BindingList<QueueItem> _queue;
        private string _queueFile;

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
            dataGridView1.DataSource = _queue;
            dataGridView1.DataBindingComplete += (s, ev) =>
            {
                if (dataGridView1.Columns["JsonPayload"] != null)
                    dataGridView1.Columns["JsonPayload"].Visible = false;
            };

            LoadQueue();

            _netTimer = new System.Windows.Forms.Timer { Interval = 30000 };
            _netTimer.Tick += _netTimer_Tick;
            _netTimer.Start();

            Log("فرم بارگذاری شد. بررسی وضعیت اینترنت...");
            bool ok = await IsInternetAvailableAsync();
            SetConnectionStatus(ok);
            if (ok)
                await RunSendAllAsync();
        }

        private void WEBS_MESG_F_Disposed(object sender, EventArgs e)
        {
            if (_netTimer != null) _netTimer.Stop();
            SaveQueue();
        }

        // ============================================================
        // 4. بررسی اتصال اینترنت (هر 30 ثانیه)
        // ============================================================

        private async void _netTimer_Tick(object sender, EventArgs e)
        {
            bool ok = await IsInternetAvailableAsync();
            SetConnectionStatus(ok);
            if (ok)
                await RunSendAllAsync();
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
            if (_lidoma != null && _lidoma.IsAuthenticated) return true;

            var conf = ReadLidomaConfig();
            if (conf == null)
            {
                Log("تنظیمات لیدوما (Serv_Type=005) یافت نشد.");
                return false;
            }
            _baseUrl = conf.Item1;
            _lidoma = new LidomaMarket(_baseUrl ?? "https://api.lidomamarket.ir");
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
                _lidoma = new LidomaMarket(baseUrl);
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
            await RunSendAsync(QueueItemType.Service);
        }

        private async void btnSendCustomers_Click(object sender, EventArgs e)
        {
            await RunSendAsync(QueueItemType.Customer);
        }

        private async void btnSendAll_Click(object sender, EventArgs e)
        {
            await RunSendAllAsync();
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
                await SendByTypeCoreAsync(QueueItemType.Business);
                await SendByTypeCoreAsync(QueueItemType.Service);
                await SendByTypeCoreAsync(QueueItemType.Customer);
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
                            JObject res = (type == QueueItemType.Business)
                                ? await _lidoma.CreateStoreAsync(payload)
                                : await _lidoma.CreateServiceAsync(payload);

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

            // === مرحله ۲: ارسال خودکار Clubهای pending (فقط Business) ===
            if (type == QueueItemType.Business)
            {
                await SyncClubsAsync();
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

                    List<Data.Club> pending = all.Where(c => c.LDMA_STAT == "001" || c.LDMA_STAT == "003").ToList();

                    if (pending.Count == 0)
                    {
                        Log("باشگاهی برای همگام‌سازی یافت نشد.");
                        return;
                    }

                    Log(string.Format("شروع همگام‌سازی {0} باشگاه با لیدوما...", pending.Count));
                    SetProgress(0, pending.Count);

                    int done = 0;
                    foreach (var c in pending)
                    {
                        try
                        {
                            string city = "";
                            var region = iScscLocal.Regions
                                .FirstOrDefault(r => r.PRVN_CNTY_CODE == c.REGN_PRVN_CNTY_CODE
                                                  && r.PRVN_CODE == c.REGN_PRVN_CODE
                                                  && r.CODE == c.REGN_CODE);
                        if (region != null) city = region.NAME ?? "";

                        var storeData = new JObject();
                        storeData.Add("name", c.NAME ?? "");
                        storeData.Add("template", c.TEMP_TAG ?? "gym");
                        storeData.Add("ownerPhone", settings.WEB_SITE_LOGN ?? "");
                        storeData.Add("ownerPassword", settings.WEB_SITE_PSWD ?? "");
                        storeData.Add("city", city);

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
                            contact.Add("type", "phone");
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
                            contact.Add("type", "postal");
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

                        JObject res = await _lidoma.CreateStoreAsync(storeData);

                        if (IsSuccess(res))
                        {
                            c.LDMA_STAT = "002";
                            c.LDMA_DATE = DateTime.Now;
                            Log(string.Format("ارسال موفق: {0} (CODE={1})", c.NAME, c.CODE));
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

        private bool IsSuccess(JObject res)
        {
            if (res == null) return false;
            if (res.Property("success") == null) return true;
            return (bool)res["success"];
        }

        // ============================================================
        // 10. مدیریت صف و ذخیرهٔ آفلاین (JSON)
        // ============================================================

        public void EnqueueBusiness(BusinessModel model) { AddItem(QueueItemType.Business, model); }
        public void EnqueueService(ServiceModel model) { AddItem(QueueItemType.Service, model); }
        public void EnqueueCustomer(CustomerModel model) { AddItem(QueueItemType.Customer, model); }

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
