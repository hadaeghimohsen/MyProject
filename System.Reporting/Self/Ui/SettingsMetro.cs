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

namespace System.Reporting.Self.Ui
{
   public partial class SettingsMetro : UserControl
   {
      public SettingsMetro()
      {
         InitializeComponent();
      }

      private void ti_database_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Reporting",
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
                                 "<Privilege>2</Privilege><Sub_Sys>2</Sub_Sys>",
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 #region Error Fire
                                 Job _ShowError = new Job(SendType.External, "Datasource",
                                    new List<Job>
                                    {
                                       new Job(SendType.External, "Commons",
                                          new List<Job>
                                          {
                                             new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                             {
                                                Input = "Not Imp"
                                             }
                                          })
                                    });
                                 _DefaultGateway.Gateway(_ShowError);
                                 #endregion
                              })
                           },
                           #endregion
                        }),
                     new Job(SendType.External, "Datasource", "" , 02 /* Execute DoWork4Configuration */, SendType.Self)
                  }));
      }

      private void ti_spcyrptfiles_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Reporting", "ReportUnitType", 02 /* Execute DoWork4ReportUnitType */, SendType.Self) );
      }

      private void ti_specifyappdecision_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
          _DefaultGateway.Gateway(
              new Job(SendType.External, "Reporting", "ReportUnitType", 03 /* Execute DoWork4SpecifyAppDecision */, SendType.Self));
      }

      private void ti_specifyreportprofile_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Reporting", "ReportProfiler", 02 /* Execute DoWork4SpecifyReportProfiler */, SendType.Self));
      }

      private void ti_specifyreportgroupheader_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Reporting", "ReportProfiler", 04 /* Execute DoWork4SpecifyReportGroupHeader */, SendType.Self));
      }

      private void ti_profilertemplate_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Reporting", "ReportProfiler", 07 /* Execute DoWork4ProfilerTemplate */, SendType.Self));
      }
   }
}
