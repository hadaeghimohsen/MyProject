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
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsAccount : UserControl
   {
      public SettingsAccount()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, ImageAccount_Pb.Width, ImageAccount_Pb.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         ImageAccount_Pb.Region = rg;
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
         UserBs.DataSource = iProject.Users.Where(u => u.USERDB.ToUpper() == CurrentUser.ToUpper());
      }

      private void UserBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            UserBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch (Exception)
         {

         }         
      }

      private void TakeImage_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 21 /* Execute DoWork4SettingsAccountCamera */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountCamera", 10 /* Execute ActionCallWindow */){Input = user}
               }
            )
         );
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         if(user.USER_IMAG == null)
         {
            byte[] bytes = null;
            MemoryStream ms = new MemoryStream();
            Image img = global::System.DataGuard.Properties.Resources.IMAGE_1429;
            img.Save(ms, ImageFormat.Bmp);
            bytes = ms.ToArray();

            user.USER_IMAG = bytes;
            ImageAccount_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         } 
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            ImageAccount_Pb.Image = Image.FromStream(stream);
         }

         // Set Contact Public Status
         user.CONT_PBLC_STAT = user.CONT_PBLC_STAT == null ? "001" : user.CONT_PBLC_STAT;
         switch (user.CONT_PBLC_STAT)
         {
            case "001":
               Ts_ContactPublicStatus.IsOn = false;
               break;
            case "002":
               Ts_ContactPublicStatus.IsOn = true;
               break;
         }

         // Set Privacy Lock Screen Status
         user.PRVC_LOCK_SCRN_STAT = user.PRVC_LOCK_SCRN_STAT == null ? "001" : user.PRVC_LOCK_SCRN_STAT;
         switch (user.PRVC_LOCK_SCRN_STAT)
         {
            case "001":
               Ts_PrivacyLockScreenStat.IsOn = false;
               break;
            case "002":
               Ts_PrivacyLockScreenStat.IsOn = true;
               break;
         }

         // Set Show Login Form
         user.SHOW_LOGN_FORM = user.SHOW_LOGN_FORM == null ? "001" : user.SHOW_LOGN_FORM;
         switch (user.SHOW_LOGN_FORM)
         {
            case "001":
               Ts_ShowLoginForm.IsOn = false;
               break;
            case "002":
               Ts_ShowLoginForm.IsOn = true;
               break;
         }

         // Set Default User Mail Server
         user.DFLT_USER_HELP_SRVR = user.DFLT_USER_HELP_SRVR == null ? "001" : user.DFLT_USER_HELP_SRVR;
         switch (user.DFLT_USER_HELP_SRVR)
         {
            case "001":
               Ts_DefaultUserMailServer.IsOn = false;
               break;
            case "002":
               Ts_DefaultUserMailServer.IsOn = true;
               break;
         }
      }

      private void SelectImageGallery_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 22 /* Execute DoWork4SettingsAccountGallery */){Input = "SettingsAccount"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccountGallery", 10 /* Execute ActionCallWindow */){Input = user}
               }
            )
         );
      }

      private void ChangePassword_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 23 /* Execute DoWork4SettingsAccountChangePassword */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountChangePassword", 10 /* Execute ActionCallWindow */){Input = user}
               }
            )
         );
      }

      private void Ts_PrivacyLockScreenStat_Toggled(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         user.PRVC_LOCK_SCRN_STAT = ts.IsOn ? "002" : "001";
      }

      private void Lnk_LockScreen_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 13 /* Execute DoWork4SettingsPersonalization */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 13 /* Execute DoWork4SettingsPersonalization */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 08 /* Execute LoadDataAsync */){Executive = ExecutiveType.Asynchronous}, 
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 10 /* Execute ActionCallWindows */){Input = "lockscreen"}
               }
            )
         );
      }

      private void Lnk_LocalSecurityPolicy_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 13 /* Execute DoWork4SettingsPersonalization */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 24 /* Execute DoWork4SettingsAccountLocalSecurityPolicy */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountLocalSecurityPolicy", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void Ts_ContactPublicStatus_Toggled(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         user.CONT_PBLC_STAT = ts.IsOn ? "002" : "001";
      }

      private void OtherUsers_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 25 /* Execute DoWork4SettingsAccountChangePassword */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void MailServer_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 35 /* Execute DoWork4SettingsAccountSetEmailAddress */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountSetEmailAddress", 10 /* Execute ActionCallWindow */){Input = UserBs.Current}
               }
            )
         );
      }

      private void MailServer_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 34 /* Execute DoWork4SettingsMailServer */),
                  new Job(SendType.SelfToUserInterface, "SettingsMailServer", 10 /* Execute ActionCallWindow */)
               }
            )
         );
      }

      private void SendEmail_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         if((user.EMAL_ADRS == null || user.EMAL_ADRS == "") || (user.EMAL_PASS == null || user.EMAL_PASS == "") || (user.MAIL_SRVR == null || user.MAIL_SRVR == ""))
         {
            MessageBox.Show(this, "اطلاعات ایمیل ارسال شما به درستی وارد نشده لطفا در قسمت تنظیمات ایمیل و سرور ایمیل بررسی کنید و مشکل را بر طرف کنید", "اطلاعات ایمیل و سرور ایمیل", MessageBoxButtons.OK, MessageBoxIcon.Question);
            return;
         }

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 36 /* Execute DoWork4SendEmail */),
                  new Job(SendType.SelfToUserInterface, "SettingsSendEmail", 10 /* Execute ActionCallWindow */)
                  {
                     Input = 
                        new XElement("SendEmail",
                           new XAttribute("username", user.USERDB),
                           new XAttribute("type", "normal"),
                           new XAttribute("subsys", "0")
                        )

                  }
               }
            )
         );
      }

      private void Ts_ShowLoginForm_Toggled(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         user.SHOW_LOGN_FORM = ts.IsOn ? "002" : "001";
      }

      private void JobSchedule_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "" , 10 /* Execute DoWork4SettingsSystem */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 10 /* Execute DoWork4SettingsSystem */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystem", 10 /* Execute ActionCallWindows */)
                  {
                     Input = 
                        new XElement("System",
                           new XAttribute("tabmenu", "job")
                        )
                  }
               }
            )
         );
      }

      private void Ts_DefaultUserMailServer_Toggled(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         user.DFLT_USER_HELP_SRVR = ts.IsOn ? "002" : "001";
      }

      private void DuplicateUser_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 28 /* Execute DoWork4SettingsDuplicateAccount */),
                  new Job(SendType.SelfToUserInterface, "SettingsDuplicateAccount", 10 /* Execute ActionCallWindow */){Input = user}
               }
            )
         );
      }

      private void ChangeFinger_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 43 /* Execute DoWork4SettingsAccountFinger */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountFinger", 10 /* Execute ActionCallWindow */){Input = user}
               }
            )
         );
      }
   }
}
