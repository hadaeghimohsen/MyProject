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
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsNewAccount : UserControl
   {
      public SettingsNewAccount()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }            
         );
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         UserTitleName_Txt.Text = UserName_Be.Text = NewPassword_Be.Text = ReenterNewPassword_Be.Text = "";
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if(UserTitleName_Txt.EditValue.ToString().Trim() == "")
            {
               UserTitleName_Txt.Focus();
               return;
            }

            if(UserName_Be.Text.Trim() == "")
            {
               UserName_Be.Focus();
               return;
            }

            // Second Step Check New Password
            if(NewPassword_Be.EditValue.ToString() != ReenterNewPassword_Be.EditValue.ToString())
            {
               ReenterNewPassword_Be.Focus();
               return;
            }

            // Third Step Check Current Password not equal New Password
            if((UserTitleName_Txt.EditValue.ToString() == NewPassword_Be.EditValue.ToString()) ||
               (NewPassword_Be.EditValue.ToString() == "" || ReenterNewPassword_Be.EditValue.ToString() == ""))
            {
               NewPassword_Be.Focus();
               return;
            }

            // Last Step Check Password Policy

            if(iProject.Users.Any(u => u.USERDB.ToLower() == UserName_Be.Text.ToLower()))
            {
               MessageBox.Show("نام کاربری قبلا در سیستم ثبت گردیده است. لطفا از گزینه دیگری استفاده کنید");
               return;
            }

            iProject.CreateNewUser(
               new XElement("User",
                  new XElement("TitleFa", UserTitleName_Txt.Text),
                  new XElement("TitleEn", UserName_Be.Text),
                  new XElement("STitleEn", UserName_Be.Text),
                  new XElement("Password", NewPassword_Be.Text),
                  new XElement("IsLock", false)
               )
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     //new Job(SendType.Self, 25 /* Execute DoWork4SettingsAccountChangePassword */),                  
                     new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 10 /* Execute ActionCallWindows */)
                  }
               )
            );
            Back_Butn_Click(null, null);
         }
         catch (Exception)
         {

         }
      }
   }
}
