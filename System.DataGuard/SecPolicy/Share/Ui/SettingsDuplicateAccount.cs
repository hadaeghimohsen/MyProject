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
   public partial class SettingsDuplicateAccount : UserControl
   {
      public SettingsDuplicateAccount()
      {
         InitializeComponent();

         System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
         gp.AddEllipse(0, 0, ImageSourceAccount_Pb.Width, ImageSourceAccount_Pb.Height);
         System.Drawing.Region rg = new System.Drawing.Region(gp);
         ImageSourceAccount_Pb.Region = ImageTargetAccount_Pb.Region = rg;

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
         UserBs.DataSource = User;
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

            // <Duplicate><NewUser><TitleFa>{0}</TitleFa><TitleEn>{1}</TitleEn><STitleEn>{2}</STitleEn><Password>{3}</Password><IsLock>{4}</IsLock><GrantToRole>{5}</GrantToRole><GrantToPrivilege>{6}</GrantToPrivilege></NewUser>{7}</Duplicate>
            iProject.DuplicateUser(
               new XElement("Duplicate",
                  new XElement("Role",
                     new XElement("User",
                        new XElement("UserID", User.ID)
                     )
                  ),
                  new XElement("NewUser",
                     new XElement("TitleFa", UserTitleName_Txt.Text),
                     new XElement("TitleEn", UserName_Be.Text),
                     new XElement("STitleEn", UserName_Be.Text),
                     new XElement("Password", NewPassword_Be.Text),
                     new XElement("IsLock", false),
                     new XElement("GrantToRole", Ts_GrantToRole.IsOn),
                     new XElement("GrantToPrivilege", Ts_GrantToPrivilege.IsOn)
                  )
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
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void UserName_Be_TextChanged(object sender, EventArgs e)
      {
         TargetUserName_Lb.Text = UserName_Be.Text;
      }
   }
}
