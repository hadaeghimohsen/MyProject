using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsPrivacy : UserControl
   {
      public SettingsPrivacy()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.User.USERDB.ToUpper() == CurrentUser.ToUpper() && ug.VALD_TYPE == "002"); 
      }

      private void PackageUserGatewayBs_CurrentChanged(object sender, EventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;

         #region Tab001
         // Set Region For Wheater
         piug.WETR_ACES_STAT = piug.WETR_ACES_STAT == null ? "002" : piug.WETR_ACES_STAT;
         switch (piug.WETR_ACES_STAT)
         {
            case "001":
               Ts_WheaterAccess.IsOn = false;
               break;
            case "002":
               Ts_WheaterAccess.IsOn = true;
               break;
         }
         #endregion
         #region Tab002
         // Set Show Message On Lock Screen
         piug.SHOW_NOTF_LOCK_SCRN = piug.SHOW_NOTF_LOCK_SCRN == null ? "002" : piug.SHOW_NOTF_LOCK_SCRN;
         switch (piug.SHOW_NOTF_LOCK_SCRN)
         {
            case "001":
               Ts_ShowNotificationLockScreen.IsOn = false;
               break;
            case "002":
               Ts_ShowNotificationLockScreen.IsOn = true;
               break;
         }

         // Set Show Warning & Reminder
         piug.SHOW_NOTF_WRMD_STAT = piug.SHOW_NOTF_WRMD_STAT == null ? "002" : piug.SHOW_NOTF_WRMD_STAT;
         switch (piug.SHOW_NOTF_WRMD_STAT)
         {
            case "001":
               Ts_ShowWarningReminder.IsOn = false;
               break;
            case "002":
               Ts_ShowWarningReminder.IsOn = true;
               break;
         }

         // Set Show System Suggest
         piug.SHOW_SYS_SUGT_STAT = piug.SHOW_SYS_SUGT_STAT == null ? "002" : piug.SHOW_SYS_SUGT_STAT;
         switch (piug.SHOW_SYS_SUGT_STAT)
         {
            case "001":
               Ts_ShowSystemSuggest.IsOn = false;
               break;
            case "002":
               Ts_ShowSystemSuggest.IsOn = true;
               break;
         }
         #endregion
      }

      private void PackageUserGatewayBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void ClearRegion_Butn_Click(object sender, EventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;

         piug.REGN_CODE = null;
         piug.REGN_PRVN_CODE = null;
         piug.REGN_PRVN_CNTY_CODE = null;
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PackageUserGatewayBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch 
         {}
      }

      private void Ts_NotificationStatus_Toggled(object sender, EventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         piug.SHOW_NOTF_STAT = ts.IsOn ? "002" : "001";
      }

      private void Ts_ShowNotificationLockScreen_Toggled(object sender, EventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         piug.SHOW_NOTF_LOCK_SCRN = ts.IsOn ? "002" : "001";
      }

      private void Ts_ShowWarningReminder_Toggled(object sender, EventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         piug.SHOW_NOTF_WRMD_STAT = ts.IsOn ? "002" : "001";
      }

      private void Ts_ShowSystemSuggest_Toggled(object sender, EventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         piug.SHOW_SYS_SUGT_STAT = ts.IsOn ? "002" : "001";
      }

      private void Regn_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         var piug = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (piug == null) return;
         
         var regn = RegnBs.List.OfType<Data.Region>().Where(r => r.CODE == e.NewValue.ToString()).FirstOrDefault();
         if(regn == null)return;

         piug.REGN_PRVN_CODE = regn.PRVN_CODE;
         piug.REGN_PRVN_CNTY_CODE = regn.PRVN_CNTY_CODE;
      }
   }
}
