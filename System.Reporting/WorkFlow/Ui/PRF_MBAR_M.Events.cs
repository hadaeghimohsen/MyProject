using System;
using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_MBAR_M
   {
      partial void wbp_profiler_desktop_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         XDocument xSelectedProfilers = null;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "PRF_SPRF_F", 14 /* Execute GetSelectedProfilers */, SendType.SelfToUserInterface)
            {
               AfterChangedOutput = new Action<object>((output) =>
               {
                  xSelectedProfilers = output as XDocument;
               })
            });

         /* تمام پروفایل های انتخاب شده باید در یک گروه دسترسی قرار داشته باشند. */
         if (xSelectedProfilers != null && xSelectedProfilers.Descendants("Profiler").Attributes("role_id").Select(role => role.Value).Distinct().Count() != 1)
            return;
         XDocument xRequest = null;
         switch (e.Button.Properties.Tag.ToString())
         {
            case "0":
               if (xSelectedProfilers.Descendants("Profiler").Count() < 2)
                  return;

               xRequest = XDocument.Parse(@"<Request type=""CreateNewCoProfiler""/>");
               xRequest.Element("Request").Add(xSelectedProfilers.Elements());

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
                                    "<Privilege>12</Privilege><Sub_Sys>2</Sub_Sys>",
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
                                    true,
                                    "xml",
                                    xRequest.ToString(),
                                    "{ CALL [Report].[SetProfiler](?) }",
                                    "iProject",
                                    "scott"
                                 },
                                 AfterChangedOutput = new Action<object>((output) =>
                                 {
                                    /* بازکردن صفحه فرم تغییر نام پروفایل */
                                    _DefaultGateway.Gateway(
                                       new Job(SendType.External, "Localhost", "", 06 /* Execute DoWork4PRF_CHNG_F */, SendType.Self)
                                       {
                                          Input = XDocument.Parse((output as DataSet).Tables[0].Rows[0][0].ToString())
                                       });
                                 })
                              }
                              #endregion                        
                           })                     
                  }));

               
               break;
            case "1":
               if ( xSelectedProfilers == null || xSelectedProfilers.Descendants("Profiler").Count() != 1)
                  return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "", 06 /* Execute DoWork4PRF_CHNG_F */, SendType.Self)
                  {
                     Input = xSelectedProfilers
                  });

               break;
            case "2":
               /* باز کردن صفحه نمایش تنظمیات پروفایل ها 
                  اگر کاربر پروفایلی را انتخاب کرده باشد صفحه تنظیمات سربرگ آن پروفایل را باز میکنیم
                  در غیر اینصورت صفحه فرم پروفایل های موجود را باز میکنیم 
               */
               if (xSelectedProfilers == null || xSelectedProfilers.Descendants("Profiler").Count() != 1)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "DefaultGateway:ReportProfiler", 02 /* Execute DoWork4SpecifyReportProfiler */, SendType.Self));
               }
               else
               {
                  string xml = string.Format(@"<Profiler ID=""{0}""><FaName>{1}</FaName></Profiler>", 
                     xSelectedProfilers.Descendants("Profiler").First().Attribute("id").Value, 
                     xSelectedProfilers.Descendants("Profiler").First().Attribute("faName").Value);
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "DefaultGateway:ReportProfiler", 03 /* Execute DoWork4SpecifyProfilerGroupHeader*/, SendType.Self) { Input = xml });
               }
               break;
            default:
               break;
         }         
      }
   }
}
