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
using System.IO;
using System.Drawing.Imaging;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsAccountLocalSecurityPolicy : UserControl
   {
      public SettingsAccountLocalSecurityPolicy()
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
         SecurityManagmentBs.DataSource = iProject.Security_Managments;
      }

      private void SecurityManagmentBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            SecurityManagmentBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch (Exception)
         {

         }         
      }      

      private void SecurityManagmentBs_CurrentChanged(object sender, EventArgs e)
      {
         var securitymanagment = SecurityManagmentBs.Current as Data.Security_Managment;
         if (securitymanagment == null) return;

         // Set Local Password Policy Status
         securitymanagment.PLCY_SECR_PSWD = securitymanagment.PLCY_SECR_PSWD == null ? "001" : securitymanagment.PLCY_SECR_PSWD;
         switch (securitymanagment.PLCY_SECR_PSWD)
         {
            case "001":
               Ts_PolicySecurityPassword.IsOn = false;
               break;
            case "002":
               Ts_PolicySecurityPassword.IsOn = true;
               break;
         }

         securitymanagment.PSWD_HIST_NUMB = securitymanagment.PSWD_HIST_NUMB == null ? 0 : securitymanagment.PSWD_HIST_NUMB;
         securitymanagment.MAX_PSWD_AGE = securitymanagment.MAX_PSWD_AGE == null ? 0 : securitymanagment.MAX_PSWD_AGE;
         securitymanagment.MIN_PSWD_AGE = securitymanagment.MIN_PSWD_AGE == null ? 0 : securitymanagment.MIN_PSWD_AGE;
         securitymanagment.MIN_PSWD_LEN = securitymanagment.MIN_PSWD_LEN == null ? 0 : securitymanagment.MIN_PSWD_LEN;

         // Set Meet Complexity Passwors
         securitymanagment.PLCY_PSWD_CMPX = securitymanagment.PLCY_PSWD_CMPX == null ? "001" : securitymanagment.PLCY_PSWD_CMPX;
         switch (securitymanagment.PLCY_PSWD_CMPX)
         {
            case "001":
               Ts_MeetComplexPassword.IsOn = false;
               break;
            case "002":
               Ts_MeetComplexPassword.IsOn = true;
               break;
         }

         // Set Store Encrypt Password
         securitymanagment.HASH_PASS = securitymanagment.HASH_PASS == null ? "001" : securitymanagment.HASH_PASS;
         switch (securitymanagment.HASH_PASS)
         {
            case "001":
               Ts_StoreEncryptionPassword.IsOn = false;
               break;
            case "002":
               Ts_StoreEncryptionPassword.IsOn = true;
               break;
         }
      }

      private void Ts_PolicySecurityPassword_Toggled(object sender, EventArgs e)
      {
         var securitymanagment = SecurityManagmentBs.Current as Data.Security_Managment;
         if (securitymanagment == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         securitymanagment.PLCY_SECR_PSWD = ts.IsOn ? "002" : "001";
      }

      private void Ts_MeetComplexPassword_Toggled(object sender, EventArgs e)
      {
         var securitymanagment = SecurityManagmentBs.Current as Data.Security_Managment;
         if (securitymanagment == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         securitymanagment.PLCY_PSWD_CMPX = ts.IsOn ? "002" : "001";
      }

      private void Ts_StoreEncryptionPassword_Toggled(object sender, EventArgs e)
      {
         var securitymanagment = SecurityManagmentBs.Current as Data.Security_Managment;
         if (securitymanagment == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         securitymanagment.HASH_PASS = ts.IsOn ? "002" : "001";
      }
   }
}
