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
   public partial class SettingsOtherAccount : UserControl
   {
      public SettingsOtherAccount()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, ImageAccount_Pb.Width, ImageAccount_Pb.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         ImageAccount_Pb.Region = rg;
      }

      private bool requery = false;

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

         // Set Must Change Password
         user.MUST_CHNG_PASS = user.MUST_CHNG_PASS == null ? "001" : user.MUST_CHNG_PASS;
         switch (user.MUST_CHNG_PASS)
         {
            case "001":
               Ts_MustChangePassword.IsOn = false;
               break;
            case "002":
               Ts_MustChangePassword.IsOn = true;
               break;
         }

         // Set Policy Force Safe Enter
         user.PLCY_FORC_SAFE_ENTR = user.PLCY_FORC_SAFE_ENTR == null ? "002" : user.PLCY_FORC_SAFE_ENTR;
         switch (user.PLCY_FORC_SAFE_ENTR)
         {
            case "001":
               Ts_PolicyForceEnter.IsOn = false;
               break;
            case "002":
               Ts_PolicyForceEnter.IsOn = true;
               break;
         }

         // Set Add Computer List
         user.ADD_COMP_LIST = user.ADD_COMP_LIST == null ? "002" : user.ADD_COMP_LIST;
         switch (user.ADD_COMP_LIST)
         {
            case "001":
               Ts_AddComputerList.IsOn = false;
               break;
            case "002":
               Ts_AddComputerList.IsOn = true;
               break;
         }

         // Set Show Login Form
         user.SHOW_LOGN_FORM = user.SHOW_LOGN_FORM == null ? "002" : user.SHOW_LOGN_FORM;
         switch (user.SHOW_LOGN_FORM)
         {
            case "001":
               Ts_ShowLoginForm.IsOn = false;
               break;
            case "002":
               Ts_ShowLoginForm.IsOn = true;
               break;
         }

         // Set Lock
         switch (user.IsLock)
         {
            case false:
               Ts_Lock.IsOn = false;
               break;
            case true:
               Ts_Lock.IsOn = true;
               break;
         }

         // Set First Login
         user.FRST_LOGN = user.FRST_LOGN == null ? "001" : user.FRST_LOGN;
         switch (user.FRST_LOGN)
         {
            case "001":
               Ts_FirstLogin.IsOn = false;
               break;
            case "002":
               Ts_FirstLogin.IsOn = true;
               break;
         }

         // Set Default Factory
         user.DFLT_FACT = user.DFLT_FACT == null ? "001" : user.DFLT_FACT;
         switch (user.DFLT_FACT)
         {
            case "001":
               Ts_DefaultFactory.IsOn = false;
               break;
            case "002":
               Ts_DefaultFactory.IsOn = true;
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
                  new Job(SendType.Self, 22 /* Execute DoWork4SettingsAccountGallery */){Input = "SettingsOtherAccount"},
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

      private void Ts_Object_Toggled(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         ToggleSwitch ts = (ToggleSwitch)sender;

         switch (ts.Tag.ToString())
         {
            case "1":
               user.MUST_CHNG_PASS = ts.IsOn ? "002" : "001";
               break;
            case "2":
               user.PLCY_FORC_SAFE_ENTR = ts.IsOn ? "002" : "001";
               break;
            case "3":
               user.IsLock = ts.IsOn;
               break;
            case "4":
               user.FRST_LOGN = ts.IsOn ? "002" : "001";
               break;
            case "5":
               user.DFLT_FACT = ts.IsOn ? "002" : "001";
               break;
            case "6":
               user.ADD_COMP_LIST = ts.IsOn ? "002" : "001";
               break;
            case "7":
               user.SHOW_LOGN_FORM = ts.IsOn ? "002" : "001";
               break;
            case "8":
               user.DFLT_USER_HELP_SRVR = ts.IsOn ? "002" : "001";
               break;
            default:
               break;
         }
         
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

      private void DeleteUser_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var user = UserBs.Current as Data.User;
            if (user == null) return;

            if (MessageBox.Show(this, "آیا می خواهید اطلاعات سیستمی کاربر را حذف کنید. با این عمل تمامی اطلاعات از سیستم برای کاربر از بین میرود؟", "حذف کاربری", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.DeleteUser(
               new XElement("DeleteUser",
                  new XAttribute("id", user.ID)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Back_Butn_Click(null, null);
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 10 /* Execute ActionCallWindow */)
                     }
                  )
               );
            }
         }
      }

      private void UserMail_Butn_Click(object sender, EventArgs e)
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

      private void SendEmail_Butn_Click(object sender, EventArgs e)
      {
         var user = UserBs.Current as Data.User;
         if (user == null) return;

         if ((user.EMAL_ADRS == null || user.EMAL_ADRS == "") || (user.EMAL_PASS == null || user.EMAL_PASS == "") || (user.MAIL_SRVR == null || user.MAIL_SRVR == ""))
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

   }
}
