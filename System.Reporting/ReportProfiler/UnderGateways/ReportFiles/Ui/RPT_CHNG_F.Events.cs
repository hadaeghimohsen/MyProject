using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reporting.ReportProfiler.UnderGateways.ReportFiles.Ui
{
   partial class RPT_CHNG_F
   {
      private void FaKeyboard()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* LangChangToFarsi */, SendType.Self));
      }

      private void EnKeyboard()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 09 /* LangChangToEnglish */, SendType.Self));
      }

      partial void be_report_name_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Tag.ToString())
         {
            case "0":
               be_report_name.Text = "";
               break;
            case "1":
               if (ofd_report_path.ShowDialog() != Windows.Forms.DialogResult.OK)
                  return;

               be_report_name.Text = ofd_report_path.FileName;
               break;
         }
      }

      partial void wbp_submit_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Tag.ToString())
         {
            case "0":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "RPT_CHNG_F", 04 /* Execute UnPaint */, SendType.SelfToUserInterface)
               );
               break;
            case "1":
               if (be_report_name.Properties.AppearanceFocused.BackColor != Color.GreenYellow)
                  return;

               _DefaultGateway.Gateway(
                 new Job(SendType.External, "Localhost",
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
                                    "<Privilege>27</Privilege><Sub_Sys>1</Sub_Sys>",
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Error Fire
                                    Job _ShowError = new Job(SendType.External, "GroupHeader",
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
                                                         _DefaultGateway.Gateway(new Job(SendType.External, "ServiceDefinition", "Services", 11 /* Execute LoadServicesOfParentService */, SendType.SelfToUserInterface));
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
                                    string.Format(@"<Request type=""Change_Title_En""><Service oldName=""{0}"" newName=""{1}""/></Request>",be_report_name.Tag, be_report_name.Text),
                                    "{ CALL [ServiceDef].[SetService](?) }",
                                    "iProject",
                                    "scott"
                                 }                                 
                              }
                              #endregion                        
                           }),
                        new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 04 /* Execute UnPaint */),
                        new Job(SendType.External, "DefaultGateway", "DefaultGateway:WorkFlow:WHR_SCON_F", 10 /* Execute UpdateNewFilePath */, SendType.SelfToUserInterface){Input = be_report_name.Text}
                  }));
               break;
            case "2":
               try
               {
                  ReportDocument crDoc = new ReportDocument();
                  crDoc.Load(be_report_name.Text);
                  be_report_name.Properties.AppearanceFocused.BackColor = be_report_name.Properties.AppearanceFocused.BackColor2 = Color.GreenYellow;
                  be_report_name.Properties.AppearanceFocused.BorderColor = Color.ForestGreen;
               }
               catch 
               {
                  be_report_name.Properties.AppearanceFocused.BackColor = be_report_name.Properties.AppearanceFocused.BackColor2 = Color.MistyRose;
                  be_report_name.Properties.AppearanceFocused.BorderColor = Color.Red;
               }
               break;
         }
      }
   }
}
