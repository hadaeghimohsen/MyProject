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

namespace System.DataGuard.SecPolicy.Role.Ui
{
   public partial class CreateNewRole : UserControl
   {
      public CreateNewRole()
      {
         InitializeComponent();
      }

      private void sb_createnewrole_Click(object sender, EventArgs e)
      {
         if (te_titlefa.Text.Trim().Length == 0 || SUBSYS_LOV.SelectedIndex < 0)
            return;

         Job _CreateNewRole = new Job(SendType.External, "Role",
            new List<Job>
            {
               #region Not Exists Role
               new Job(SendType.Self, 07 /* Execute DoWork4NotExistsRole */)
               {
                  Input = string.Format("<STitleFa>{0}</STitleFa>",te_titlefa.Text.Replace(" ", string.Empty)),                        
                  AfterChangedOutput = new Action<object>((output) => {
                     if ((bool)output)
                     {
                        return;
                     }

                     Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                     {
                        Input = @"Not Imp"
                     };
                     _DefaultGateway.Gateway(_ShowError);
                  })
               },
               #endregion
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = new List<string>
                        {
                           "<Privilege>3</Privilege>",
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;

                           Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
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
                           string.Format("<Role><TitleFa>{0}</TitleFa><TitleEn>{1}</TitleEn><STitleFa>{2}</STitleFa><SubSys>{3}</SubSys></Role>", te_titlefa.Text, te_titleen.Text, te_titlefa.Text.Trim(), SUBSYS_LOV.SelectedIndex),
                           "{ Call DataGuard.CreateNewRole(?) }",
                           "iProject",
                           "scott"
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint", string.Format("<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>2</SectionID><Explain>ایجاد گروه دسترسی {0}</Explain>", te_titlefa.Text)}}
                     #endregion
                  }),
               new Job (SendType.SelfToUserInterface, "CreateNewRole", 04 /* Execute UnPaint */),
               new Job(SendType.External ,"SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface )
            });
         _DefaultGateway.Gateway(_CreateNewRole);
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "Role", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "Role", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }
   }
}
