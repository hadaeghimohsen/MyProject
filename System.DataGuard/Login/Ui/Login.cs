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

namespace System.DataGuard.Login.Ui
{
   public partial class Login : UserControl
   {
      public Login()
      {
         InitializeComponent();
      }

      private void InputValidation(object sender, KeyEventArgs e)
      {
         if (e.KeyData != Keys.Return)
            return;

         if (te_username.Text.Length == 0)
            return;
         
         Job _Login = new Job(SendType.External, "Login",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                     {
                        #region DoWork4Odbc
                        Input = new List<object>
                        {
                           #region Input
                           false,
                           "func",
                           true,
                           false,
                           "xml",
                           string.Format("<Login><UserName>{0}</UserName><Password>{1}</Password></Login>", te_username.Text, te_password.Text),
                           "Select DataGuard.TryLogin(?)",
                           "iProject",
                           "scott"
                           #endregion
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              #region AfterChangedStatus

                              Job _Action = null;
                              switch ((bool)output)
	                           {
                                 case true:
                                    #region GetConnectionSrting
                                    var GetConnectionString =
                                          new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };

                                    _DefaultGateway.Gateway(GetConnectionString);

                                    ConnectionString = GetConnectionString.Output as string;
                                    #endregion
                                    #region Access Valid
                                    _Action =
                                       new Job(SendType.External, "Login",
                                          new List<Job>
                                          {
                                             new Job(SendType.SelfToUserInterface, "Login", 07 /* Execute LoadData */),
                                             new Job(SendType.SelfToUserInterface, "Login", 04 /* Execute UnPaint */),
                                             new Job(SendType.External, "Commons",
                                                new List<Job>
                                                {
                                                   new Job(SendType.Self, 05 /* Execute DoWork4Desktop */),
                                                   //new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */)
                                                   //{                                                      
                                                   //   Input = new List<string>{"savepoint", string.Format("<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>1</SectionID><Explain>ورود مجاز به سیستم</Explain>")}
                                                   //}
                                                }),
                                             new Job(SendType.External, "DataGuard", "Program", 02 /* Start_Service_Component */, SendType.Self)
                                          });
                                    #endregion
                                    break;
                                 case false:
                                    #region Access Deny
                                    _Action =
                                       new Job(SendType.External, "Login",
                                          new List<Job>
                                          {
                                             new Job(SendType.External, "Commons",
                                                new List<Job>
                                                {
                                                   new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                                   {
                                                      Input =  @"<HTML>
                                                                  <body>
                                                                     <p style=""float:right"">
                                                                        <ol>
                                                                           <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در نحوه ورود مشخصات کاربری</font></li>
                                                                           <ul>
                                                                              <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد نام کاربری یا رمز عبور را اشتباه وارد کرده باشید.</font></li>                                                                                 
                                                                           </ul>
                                                                           <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در نحوه ارتباط با سرور مرکزی</font></li>
                                                                           <ul>
                                                                              <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال دیگری که می توان به آن تمرکز کرد سرور پایگاه داده می باشد که ممکن است سرویس آن از کار افتاده باشد</font></li>                                                                                 
                                                                           </ul>
                                                                           <li><font face=""Tahoma"" size=""2"" color=""red"">نحوه ورود به سیستم</font></li>
                                                                           <ul>
                                                                              <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال دیگری که می توان به آن اشاره کرد بسته شدن دسترسی شما برای ورود به سیستم می باشد. در این مورد از مدیر فن آوری اطلاعات کمک بگیرید</font></li>                                                                                 
                                                                           </ul>
                                                                        </ol>
                                                                     </p>
                                                                  </body>
                                                                  </HTML>"
                                                   }
                                                })
                                          });
                                    break;
                                    #endregion
                                 default:
                                    break;
	                           }
                              if (_Action != null)
                                 _DefaultGateway.Gateway(_Action);

                              #endregion
                           }
                        )
                        #endregion
                     }
                  })
            });
         te_password.EditValue = null;
         _DefaultGateway.Gateway(_Login);
      }

      private void GotoValidation(object sender, EventArgs e)
      {
         InputValidation(sender, new KeyEventArgs(Keys.Return));
      }

      private void Control_Enter(object sender, EventArgs e)
      {
         TextEdit control = sender as TextEdit;
         control.SelectAll();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 09 /* Execute LangChangToEnglish */, SendType.Self)
         );
      }

      private void LastUserLogin_RondButn_Click(object sender, EventArgs e)
      {
         Job _LastUserLogin = new Job(SendType.External, "Localhost",
             new List<Job>
             {
                new Job(SendType.SelfToUserInterface, "Login", 04 /* Execute UnPaint */),
                new Job(SendType.Self, 05 /* Execute DoWork4LastUserLogin */)
             });
         _DefaultGateway.Gateway(_LastUserLogin);
      }

      private Image GetUserImage(Data.User user)
      {
         if (user == null)
         {
            return global::System.DataGuard.Properties.Resources.IMAGE_1482;
         }
         else if (user.USER_IMAG == null)
         {
            return global::System.DataGuard.Properties.Resources.IMAGE_1429;
         }
         else
         {
            var stream = new MemoryStream(user.USER_IMAG.ToArray());
            return Image.FromStream(stream);
         }
      }

   }
}
