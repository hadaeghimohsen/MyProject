using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_CHNG_F
   {
      partial void wbp_submit_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Tag.ToString())
         {
            case "1":
               if (be_profiler_name.Text.Trim().Length == 0)
               {
                  be_profiler_name.Focus();
                  return;
               }

               xProfiler.Descendants("Profiler").First().Attribute("faName").SetValue(be_profiler_name.Text.Trim());

               XDocument xRequest = XDocument.Parse(@"<Request type=""Rename""/>");
               xRequest.Element("Request").Add(xProfiler.Descendants("Profilers").Elements());

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
                                    "<Privilege>13</Privilege><Sub_Sys>2</Sub_Sys>",
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
                                    xRequest.ToString(),
                                    "{ CALL [Report].[SetProfiler](?) }",
                                    "iProject",
                                    "scott"
                                 }                                 
                              }
                              #endregion                        
                           })                     
                  }));
               break;
         }
         _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "PRF_CHNG_F", 06 /* Execute Close */, SendType.SelfToUserInterface));
      }

      partial void be_profiler_name_Enter(object sender, EventArgs e)
      {
         FaKeyboard();
      }
   }
}
