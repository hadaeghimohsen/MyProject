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
   public partial class Profile : UserControl
   {
      public Profile()
      {
         InitializeComponent();
      }

      private string InputXmlData;

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "User", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "User", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }

      private void btn_accept_Click(object sender, EventArgs e)
      {         
         #region Check Precondition
         if (te_titlefa.Text.Trim() == "")
         {
            te_titlefa.Focus();
            return;
         }

         if (te_titleen.Text.Trim() == "")
         {
            te_titleen.Focus();
            return;
         }

         if (te_password.Text.Length == 0 || (te_password.Text != te_passwordconf.Text))
         {
            te_password.Focus();
            return;
         }
         #endregion

         Job _CreateNewUser = new Job(SendType.External, "User",
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

                           Job _ShowError = new Job(SendType.External, "User", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                           {
                              Input = @"<HTML>
                                          <body>
                                             <p style=""float:right"">
                                                <ol>
                                                   <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                   <ul>
                                                      <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                   </ul>
                                                </ol>
                                             </p>
                                          </body>
                                        </HTML>"
                           };
                           _DefaultGateway.Gateway(_ShowError);
                        })
                     },
                     #endregion                     
                     #region DoWork
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */){
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           false,
                           "xml",
                           string.Format("<User><ID>{0}</ID><TitleFa>{1}</TitleFa><Password>{2}</Password><IsLock>{3}</IsLock></User>", Tag.ToString() ,te_titlefa.Text,  te_password.Text, cb_islock.Checked),
                           "{ Call DataGuard.ChangeUserProfile(?) }",
                           "iProject",
                           "scott"
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint", string.Format("<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>2</SectionID><Explain>ویرایش کاربر {0}</Explain>", te_titlefa.Text)}}
                     #endregion
                  }),
               new Job (SendType.SelfToUserInterface, "Profile", 04 /* Execute UnPaint */),
               new Job(SendType.External ,"SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface )
            });
         _DefaultGateway.Gateway(_CreateNewUser);
      }
   }
}
