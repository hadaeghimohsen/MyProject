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

namespace System.Reporting.ReportUnitType.Ui
{
   public partial class SpecifyAppDecision : UserControl
   {
      public SpecifyAppDecision()
      {
         InitializeComponent();
      }

      private string rootTag = "<AppDecision>{0}</AppDecision>";
      private string callFunction = "[Report].[SubmitAppDecisionType]";
      private string privilege = "<Privilege>11</Privilege><Sub_Sys>2</Sub_Sys>";

      #region Roles
      private void cb_roles_SelectedIndexChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyAppDecision", 09 /* Execute LoadTypeOfRoles */, SendType.SelfToUserInterface));
      }

      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                     })
               });
         _DefaultGateway.Gateway(_RoleSettings4CurrentUser);
      }

      private void sb_rlroles_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyAppDecision", 08 /* Execute LoadRolesOfUser */, SendType.SelfToUserInterface));
      }
      #endregion

      #region Report Unit
      private void sb_servunitsettings_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 19 /* Execute DoWork4ServiceUnitType */, SendType.Self));
      }

      private void sb_rlservunit_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyAppDecision", 09 /* Execute LoadUnitOfRoles */, SendType.SelfToUserInterface));
      }
      #endregion

      private void cb_islinked_CheckedChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyAppDecision", 09 /* Execute LoadUnitOfRoles */, SendType.SelfToUserInterface));
      }

      private void ti_crystalreport_CheckedChanged(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         if (!e.Item.Checked)
            return;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyAppDecision", 10 /* Execute LoadWhatsFileType */, SendType.SelfToUserInterface) { Input = "<AppDecision>1</AppDecision>" });
      }

      private void ti_jasper_CheckedChanged(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         if (!e.Item.Checked)
            return;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyAppDecision", 10 /* Execute LoadWhatsFileType */, SendType.SelfToUserInterface) { Input = "<AppDecision>2</AppDecision>" });
      }

      private void sb_commit_Click(object sender, EventArgs e)
      {
         if(!(ti_crystalreport.Checked || ti_jasper.Checked))
            return;

         Func<string> xmlData = new Func<string>(
            () =>
            {
               string xFileType = "";
               ccbe_servunit.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => xFileType += string.Format("<Fid>{0}</Fid>", c.Value));

               return string.Format("<RoleID>{0}</RoleID><ForceUpdate>{1}</ForceUpdate><Adid>{2}</Adid><FileType>{3}</FileType>", cb_roles.SelectedValue, cb_forceupdate.Checked, (ti_crystalreport.Checked ? 1 : 2), xFileType);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Loaclhost",
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
                                 privilege,
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 #region Error Fire
                                 Job _ShowError = new Job(SendType.External, "Localhost",
                                    new List<Job>
                                    {
                                       new Job(SendType.External, "Commons",
                                          new List<Job>
                                          {
                                             new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                             {
                                                Input = new List<object>
                                                {
                                                   "Not Imp",
                                                   new Action(() => 
                                                   {
                                                      _DefaultGateway.Gateway(new Job(SendType.External, "ReportUnitType", "SpecifyAppDecision", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
                                                   })
                                                }
                                             }
                                          })
                                    });
                                 _DefaultGateway.Gateway(_ShowError);
                                 #endregion
                              })
                           },
                           #endregion
                           #region DoWork
                           new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                           {
                              Input = new List<object>
                              {
                                 false,
                                 "procedure",
                                 true,
                                 false,
                                 "xml",
                                 string.Format(rootTag, xmlData()),
                                 string.Format("{{ Call {0}(?) }}", callFunction),
                                 "iProject",
                                 "scott"
                              }
                           },
                           #endregion                        
                        }),
                  new Job(SendType.SelfToUserInterface, "SpecifyAppDecision", 04 /* Execute UnPaint */)
               }));
      }
   }
}
