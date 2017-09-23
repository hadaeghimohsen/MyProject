using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml.Linq;
using System.Data;
using CrystalDecisions.Shared;
using System.IO;

namespace System.Reporting.ReportViewers.Ui
{
   partial class Viewers : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               ReportSourceSetup(job);
               break;
            case 09:
               VerifyReportSetup(job);
               break;
            case 10:
               FormulaSelectionSetup(job);
               break;
            case 11:
               ParametersSetup(job);
               break;
            case 12:
               ReportShow(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            reportDocument.Close();
            job.Next =
               new Job(SendType.SelfToUserInterface, "Viewers", 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "DefaultGateway",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportViewers:Viewers", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         cr_reportviewer.ReportSource = null;
         cr_reportviewer.SelectionFormula = null;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void ReportSourceSetup(Job job)
      {
         try
         {
            reportDocument = new ReportDocument();
            reportDocument.Load(new FileInfo(job.Input.ToString()).FullName);
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Failed;
         }         
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void VerifyReportSetup(Job job)
      {
         var GetCrntUser = new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(GetCrntUser);

         Action<ConnectionInfo, Tables> SetCrystalTablesLogin =
            new Action<ConnectionInfo, Tables>(
               (ci, ts) =>
               {
                  foreach (Table oTable in ts)
                  {
                     TableLogOnInfo oLogonInfo = oTable.LogOnInfo;
                     oLogonInfo.ConnectionInfo = ci;
                     oTable.ApplyLogOnInfo(oLogonInfo);
                  }
               });

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
                  string.Format(@"<Request type=""{0}"" crntuser=""{2}""><DataSource id=""{1}""/></Request>", "Single", job.Input, GetCrntUser.Output),
                  "{ Call Report.GetDataSource(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                     {
                        XDocument xdoc = XDocument.Parse((output as DataSet).Tables[0].Rows[0]["XmlData"].ToString());
                        ConnectionInfo coninfo = new ConnectionInfo() 
                        {                            
                           UserID = xdoc.Element("DataSources").Element("DataSource").Attribute("user").Value ,
                           Password = xdoc.Element("DataSources").Element("DataSource").Attribute("password").Value,
                           Type = ConnectionInfoType.CRQE,                           
                        };
                        switch (xdoc.Element("DataSources").Element("DataSource").Attribute("dsType").Value)
                        {
                           case "oracle":
                           case "0":
                              coninfo.ServerName = string.Format("{0}:{1}/{2}",
                                 xdoc.Element("DataSources").Element("DataSource").Attribute("server").Value,
                                 xdoc.Element("DataSources").Element("DataSource").Attribute("port").Value,
                                 xdoc.Element("DataSources").Element("DataSource").Attribute("database").Value);
                              //coninfo.DatabaseName = xdoc.Element("DataSources").Element("DataSource").Attribute("database").Value;                              
                              break;
                           case "sql server":
                           case "1":
                              coninfo.ServerName = xdoc.Element("DataSources").Element("DataSource").Attribute("server").Value;
                              coninfo.DatabaseName = xdoc.Element("DataSources").Element("DataSource").Attribute("database").Value;
                              break;
                        }                        

                        // Set the logon credentials for all tables
                        SetCrystalTablesLogin(coninfo, reportDocument.Database.Tables);

                        // Check for subreports
                        foreach (Section oSection in reportDocument.ReportDefinition.Sections)
                        {
                           foreach (ReportObject oRptObj in oSection.ReportObjects)
                           {
                              if (oRptObj.Kind == ReportObjectKind.SubreportObject)
                              {
                                 // This is a subreport so set the logon credentials for this report's tables
                                 SubreportObject oSubRptObj = oRptObj as SubreportObject;

                                 // Open the subreport
                                 ReportDocument oSubRpt = oSubRptObj.OpenSubreport(oSubRptObj.SubreportName);

                                 SetCrystalTablesLogin(coninfo, oSubRpt.Database.Tables);
                              }
                           }
                        }

                        reportDocument.Refresh();

                        reportDocument.SetDatabaseLogon(coninfo.UserID, coninfo.Password, coninfo.ServerName, coninfo.DatabaseName);
                        
                        reportDocument.VerifyDatabase();

                        reportDocument.Refresh();
                        
                        cr_reportviewer.ReportSource = reportDocument;
                     })
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void FormulaSelectionSetup(Job job)
      {
         if (job.Input.ToString().Trim().Length == 0)
         {
            job.Status = StatusType.Successful;
            return;
         }

         if (cr_reportviewer.SelectionFormula == null)
            cr_reportviewer.SelectionFormula = job.Input.ToString();
         else
            cr_reportviewer.SelectionFormula = cr_reportviewer.SelectionFormula.Length > 1 ? cr_reportviewer.SelectionFormula + " AND " + job.Input : job.Input.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void ParametersSetup(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void ReportShow(Job job)
      {
         if (InvokeRequired)
            Invoke(new Action(() => cr_reportviewer.RefreshReport()));
         else
            cr_reportviewer.Refresh();
         job.Status = StatusType.Successful;
      }
   }
}
