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
   public partial class CurrentChangeUserType : UserControl
   {
      public CurrentChangeUserType()
      {
         InitializeComponent();
      }

      private void Btn_Cancel_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_OK_Click(object sender, EventArgs e)
      {
         try
         {
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

            var userinfo = iProject.Users.FirstOrDefault(u => u.STitleEn.ToLower() == LB_CurrentUserName.Text.ToLower());
            userinfo.DFLT_FACT = Ts_DefaultFactory.IsOn ? "002" : "001";
            userinfo.ADD_COMP_LIST = Ts_AddComputer.IsOn ? "002" : "001";
            userinfo.PLCY_FORC_SAFE_ENTR = Ts_SafeEnter.IsOn ? "002" : "001";
            userinfo.EMAL_ADRS = Txt_EmailAddress.Text;

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
