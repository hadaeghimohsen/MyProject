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
using DevExpress.XtraEditors.Controls;

namespace System.Reporting.ReportUnitType.Ui
{
   public partial class SpecifyReportFile : UserControl
   {
      public SpecifyReportFile()
      {
         InitializeComponent();
      }

      private string rootTag = "<ReportFile>{0}</ReportFile>";
      private string callFunction = "Report.SubmitReportFiles";
      private string privilege = "<Privilege>10<Privilege><Sub_Sys>2</Sub_Sys>";

      #region Roles
      private void cb_roles_SelectedIndexChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 09 /* Execute LoadTypeOfRoles */, SendType.SelfToUserInterface));
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
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 08 /* Execute LoadRolesOfUser */, SendType.SelfToUserInterface));
      }
      #endregion

      #region Cabinet
      private void ccbe_cabinet_EditValueChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 11 /* Execute LoadReportPrinter */, SendType.SelfToUserInterface));
      }

      private void sb_cabinetsettings_Click(object sender, EventArgs e)
      {
         Job _GroupHeaderSetting4CurrentUser =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 18 /* Execute DoWork4GroupHeaderSettings4CurrentUser */)
                     })
               });
         _DefaultGateway.Gateway(_GroupHeaderSetting4CurrentUser);
      }

      private void sb_rlcabinets_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 10 /* Execute LoadCabinetOfRoles */, SendType.SelfToUserInterface));
      }
      #endregion

      #region Report Type
      private void ccbe_servtype_EditValueChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 12 /* Execute LoadReportFiles */, SendType.SelfToUserInterface));
      }

      private void sb_servtypesettings_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 19 /* Execute DoWork4ServiceUnitType */, SendType.Self));
      }

      private void sb_rlservtype_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 09 /* Execute LoadTypeOfRoles */, SendType.SelfToUserInterface));
      }
      #endregion

      private void tv_reports_AfterSelect(object sender, TreeViewEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportFile", 12 /* Execute LoadReportFiles */, SendType.SelfToUserInterface));
      }

      private void lv_reportfiles_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            var xmlReport = lv_reportfiles.SelectedItems[0].Tag.ToString();
            txt_reportaddress.Text = XDocument.Parse(xmlReport).Element("Report").Value;
         }
         catch { }
      }

      private void sb_commit_Click(object sender, EventArgs e)
      {
         sb_fetchdata.Visible = false;
         Func<string> xmlData = new Func<string>(
            () =>
            {
               string xCabinet = "", xFolder = "";
               ccbe_cabinet.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => xCabinet += string.Format("<Cid>{0}</Cid>", c.Value));
               ccbe_servtype.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => xFolder += string.Format("<Fid>{0}</Fid>", c.Value));

               return string.Format("<RoleID>{0}</RoleID><Cabinets>{1}</Cabinets><Folders>{2}</Folders>", cb_roles.SelectedValue, xCabinet, xFolder);
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
                                                      _DefaultGateway.Gateway(new Job(SendType.External, "ReportUnitType", "SpecifyReportFile", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
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
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 04 /* Execute UnPaint */)
               }));
      }

      private void sb_fetchdata_Click(object sender, EventArgs e)
      {
         if (lv_reportfiles.SelectedItems.Count == 0 || lv_reportfiles.SelectedItems.Count > 1)
            return;

         string xmlReport = string.Format(@"<Report ID=""{2}""><LogicalName>{0}</LogicalName><PhysicalName>{1}</PhysicalName></Report>",
            lv_reportfiles.SelectedItems[0].Text,
            XDocument.Parse(lv_reportfiles.SelectedItems[0].Tag.ToString()).Element("Report").Value,
            XDocument.Parse(lv_reportfiles.SelectedItems[0].Tag.ToString()).Element("Report").Attribute("ID").Value
            );

         if (sb_fetchdata.Tag == null)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  new Job(SendType.External, "DefaultGateway",
                     new List<Job>
                     {
                        new Job(SendType.External, "ReportProfiler",                           
                           new  List<Job>
                           {
                              new Job(SendType.External, "ReportFile",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 01 /* Execute GetXMLReport */){Input = xmlReport}
                                 }),
                              new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 12 /* Execute LoadXMLReportInLeftTree */){WhereIsInputData = WhereIsInputDataType.StepBack},
                              new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 13 /* Execute PreSync */){WhereIsInputData = WhereIsInputDataType.StepBack},
                              new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 14 /* Execute SetDescReport */){Input = xmlReport}
                           }){GenerateInputData = GenerateDataType.Dynamic}
                     }),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 04 /* Execute UnPaint */)
               }));
            sb_fetchdata.Tag = null;
         }
         else if (sb_fetchdata.Tag.ToString() == "ProfilerTemplate")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
               {
                  new Job(SendType.External, "DefaultGateway",
                     new List<Job>
                     {
                        new Job(SendType.External, "ReportProfiler",                           
                           new  List<Job>
                           {
                              new Job(SendType.External, "ReportFile",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 01 /* Execute GetXMLReport */){Input = xmlReport}
                                 }),
                              new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 11 /* Execute ResetColumnsValidation */){WhereIsInputData = WhereIsInputDataType.StepBack},
                              new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 12 /* Execute CheckColumnsValidation */){WhereIsInputData = WhereIsInputDataType.StepBack},
                              new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 13 /* Execute SetReportOnPlace */){Input = xmlReport}
                           }){GenerateInputData = GenerateDataType.Dynamic}
                     }),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 04 /* Execute UnPaint */)
               }));
         }
      }
   }
}
