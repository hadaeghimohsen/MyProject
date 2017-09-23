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
using DevExpress.XtraEditors.Controls;

namespace System.ServiceDefinition.GrpHdr.Ui
{
   public partial class Update : UserControl
   {
      public Update()
      {
         InitializeComponent();
      }
      Int64 RoleID = 0;
      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "GroupHeader", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "GroupHeader", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }

      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "GroupHeader", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(_RoleSettings4CurrentUser);
      }

      private void sb_update_Click(object sender, EventArgs e)
      {
         if (te_titlefa.Text.Trim() == "")
         {
            te_titlefa.Focus();
            return;
         }

         if (ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked).Count() == 0)
         {
            ccbe_roles.Focus();
            return;
         }

         Func<string> XmlData = new Func<string>(
            () =>
            {
               string groupname = string.Format("<GroupHeaderID>{2}</GroupHeaderID><FaName>{0}</FaName><EnName>{1}</EnName><GhActive>{3}</GhActive>", te_titlefa.Text, te_titleen.Text, te_titlefa.Tag, cb_ghactive.Checked);
               string roles = "";
               ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked ).ToList().ForEach(role => roles += string.Format("<RoleID>{0}</RoleID>", role.Value));

               return string.Format("<Update>{0}<Roles>{1}</Roles></Update>", groupname, roles);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
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
                              "<Privilege>4</Privilege><Sub_Sys>1</Sub_Sys>",
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
                              XmlData(),
                              "{ Call ServiceDef.UpdateGroupHeader(?) }",
                              "iProject",
                              "scott"
                           }
                        },
                        #endregion
                     }),
                  new Job(SendType.SelfToUserInterface, "Update", 04 /* Execute UnPaint */),
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 09 /* Execute LoadGroupHeaders */)
               }));
      }
   }
}
