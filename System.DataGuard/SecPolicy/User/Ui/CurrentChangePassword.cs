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
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.User.Ui
{
   public partial class CurrentChangePassword : UserControl
   {
      public CurrentChangePassword()
      {
         InitializeComponent();
      }

      private void Btn_Cancel_Click(object sender, EventArgs e)
      {
         Txt_NewPassword.Text = Txt_ConfirmNewPassword.Text = "";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_OK_Click(object sender, EventArgs e)
      {
         try
         {
            if(Txt_NewPassword.Text.Trim() == "")
            {
               Txt_NewPassword.Focus();
               return;
            }

            if(Txt_ConfirmNewPassword.Text.Trim() == "")
            {
               Txt_ConfirmNewPassword.Focus();
               return;
            }

            if(Txt_NewPassword.Text != Txt_ConfirmNewPassword.Text)
            {
               Txt_ConfirmNewPassword.Focus();
               return;
            }

            Job _UpdateUser = new Job(SendType.External, "User",
            new List<Job>
            {               
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = new List<string>
                        {
                           "<Privilege>21</Privilege>",
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;

                           MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 21 امنیتی");
                        })
                     },
                     #endregion                     
                  })
            });
            _DefaultGateway.Gateway(_UpdateUser);

            if(iProject.Users.Count(u => u.STitleEn.ToLower() == LB_CurrentUserName.Text.ToLower()) > 1)
            {
               MessageBox.Show(this, "نام کاربر تکراری می باشد");
               Txt_NewPassword.Focus();
               return;
            }

            var userinfo = iProject.Users.FirstOrDefault(u => u.STitleEn.ToLower() == LB_CurrentUserName.Text.ToLower());
            userinfo.Password = Txt_NewPassword.Text;
            
            userinfo.PASS_MDFY_DATE = DateTime.Now;
            userinfo.MUST_CHNG_PASS = Ts_MustChngPass.IsOn ? "002" : "001";

            iProject.SubmitChanges();
            
            Btn_Cancel_Click(null, null);
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }
   }
}
