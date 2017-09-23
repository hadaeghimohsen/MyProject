using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class RPT_MBAR_M
   {
      partial void wbp_book_mark_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         string userName = "";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>((output) =>
                  {
                     userName = output.ToString();
                  })
            });

         XDocument xSelectedReports = null;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "RPT_SRPT_F", 14 /* Execute GetSelectedProfilers */, SendType.SelfToUserInterface)
            {
               AfterChangedOutput = new Action<object>((output) =>
                  {
                     xSelectedReports = output as XDocument;
                  })
            });
         
         XDocument xRequest = null;
         switch (e.Button.Properties.Tag.ToString())
         {
            case "0":
               if (xSelectedReports == null)
                  return;
               xRequest = XDocument.Parse(@"<Request type=""Create""/>");
               xRequest.Element("Request").Add(xSelectedReports.Elements());
               xRequest.Element("Request").Add(new XElement("User", new XAttribute("name", userName)));
               
               break;
            case "1":
               if (xSelectedReports == null)
                  return;
               xRequest = XDocument.Parse(@"<Request type=""Remove""/>");
               xRequest.Element("Request").Add(xSelectedReports.Elements());
               xRequest.Element("Request").Add(new XElement("User", new XAttribute("name", userName)));
               break;
            case "2":
               xRequest = XDocument.Parse(string.Format(@"<Request type=""RemoveAll""><User name=""{0}""/></Request>", userName));
               break;
            case "3":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "DefaultGateway", 03 /*Execute DoWork4InteractWithSettingsMetro*/, SendType.Self)
                  );
               return;
         }
         if (xRequest == null) return;

         xSelectedReports = null;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  xRequest.ToString(),
                  "{ CALL [Report].[SetUserReportBookMark](?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>((output) =>{
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "RPT_MBAR_M", 06 /* Execute Close */, SendType.SelfToUserInterface));
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "RPT_SRPT_F", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
               })
            });
      }
   }
}
