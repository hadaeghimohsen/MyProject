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

namespace System.DataGuard.SecPolicy.User.Ui
{
   public partial class CurrentUserInfo : UserControl
   {
      public CurrentUserInfo()
      {
         InitializeComponent();
      }

      private void Btn_OK_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void LL_CurrentChangeUserTitle_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.Self, "localhost", "", 09 /* Execute DoWork4CurrentChangeUserName */, SendType.Self) { Input = LB_CurrentUserName.Text }
         );
      }

      private void LL_CurrentChangePassword_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.Self, "localhost", "", 10 /* Execute DoWork4CurrentChangeUserName */, SendType.Self) { Input = LB_CurrentUserName.Text }
         );
      }

      private void LL_CurrentChangeUserType_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.Self, "localhost", "", 11 /* Execute DoWork4CurrentChangeUserType */, SendType.Self) { Input = LB_CurrentUserName.Text }
         );
      }

      private void LL_ChangeAnotherUser_Click(object sender, EventArgs e)
      {
         bool HavPrivilege = false;
         Job _ManageAnotherUsers = new Job(SendType.External, "User",
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
                           "<Privilege>28</Privilege>",
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                           {
                              HavPrivilege = true;
                              return;
                           }

                           MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 28 امنیتی");
                        })
                     },
                     #endregion                     
                  })
            });
         _DefaultGateway.Gateway(_ManageAnotherUsers);

         if (!HavPrivilege) return;

         _DefaultGateway.Gateway(
            new Job(SendType.Self, "localhost", "", 12 /* Execute DoWork4OtherUsers */, SendType.Self)
         );
      }
   }
}
