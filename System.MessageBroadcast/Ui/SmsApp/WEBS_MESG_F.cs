using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.JobRouting.Jobs;
using System.MessageBroadcast.Code;

namespace System.MessageBroadcast.Ui.SmsApp
{
    public partial class WEBS_MESG_F : UserControl
    {
        public WEBS_MESG_F()
        {
            InitializeComponent();
        }

        /// <summary>
        /// نمونهٔ کلاینت لیدوما؛ بعد از لاگین موفق پر می‌شود تا سایر تب‌ها بتوانند از توکن استفاده کنند.
        /// </summary>
        private LidomaMarket _lidoma;

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

                Status_Mmo.Text = string.Format("در حال ورود به {0} ...", baseUrl);
                _lidoma = new LidomaMarket(baseUrl);
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
            Status_Mmo.Text = "خروج انجام شد و نمونهٔ کلاینت بسته شد.";
        }

        private void Back_Butn_Click(object sender, EventArgs e)
        {
            _DefaultGateway.Gateway(
                new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
            );
        }

        // ============================================================
        // Shared helper
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
        // tp_002 - Status & Credit
        // ============================================================

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

        // ============================================================
        // tp_003 - Send SMS
        // ============================================================

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

        // ============================================================
        // tp_004 - Stores
        // ============================================================

        private async void BtnGetStores_Click(object sender, EventArgs e)
        {
            string reason;
            if (!EnsureLoggedIn(out reason)) { StoreOut_Mmo.Text = reason; return; }
            try
            {
                int p;
                int page = int.TryParse(Page_Txt.Text.Trim(), out p) ? p : 1;
                int l;
                int limit = int.TryParse(Limit_Txt.Text.Trim(), out l) ? l : 20;
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

        // ============================================================
        // tp_005 - Customers
        // ============================================================

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
    }
}
