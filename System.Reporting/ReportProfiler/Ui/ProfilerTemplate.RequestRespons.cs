using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Data;
using System.Xml.Linq;
using System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui;

namespace System.Reporting.ReportProfiler.Ui
{
   partial class ProfilerTemplate : ISendRequest
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
               LoadActiveDataSource(job);
               break;
            case 09:
               DoWork4SelectedProfiler(job);
               break;
            case 10:
               AddGroup(job);
               break;
            case 11:
               ResetColumnsValidation(job);
               break;
            case 12:
               CheckColumnsValidation(job);
               break;
            case 13:
               SetReportOnPlace(job);
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
         else if (keyData == Keys.F5)
         {
            sb_print_Click(null, null);
         }
         else if (keyData ==( Keys.Control | Keys.Back))
         {
            ShowFormulaSelection();
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 04 /* Execute UnPaint */);
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
         Tag = job.Input;
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportProfiler:ProfilerTemplate", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */) {  Input = this }
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
         job.Next =
            new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 08 /* Execute  LoadActiveDataSource */) { Input = "All" };

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadActiveDataSource(Job job)
      {
         List<string> dsnList = null;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 20 /* Execute DoWork4GetRegisterOdbcDatasource */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     dsnList = output as List<string>;
                  })
            });
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               #region Input
               Input = new List<object>
               {
                  false,
                  "procedure",
                  false,
                  true,
                  "",
                  "",
                  "{ Call Report.LoadDataSources }",
                  "iProject",
                  "scott"
               },
               #endregion
               #region After Changed Output
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     DataSet ds = output as DataSet;
                     ds.Tables["Datasources"].Columns.Add(
                        new DataColumn("isValid", typeof(bool)) { DefaultValue = false });
                     var query = from dsn in dsnList
                                 join dbcs in ds.Tables["Datasources"].Rows.OfType<DataRow>()
                                    on dsn equals string.Format("Db_{0}", dbcs["ID"])
                                 select dbcs;
                     query.ToList().ForEach(c => c["isValid"] = true);

                     ds.Tables["Datasources"].Rows.OfType<DataRow>().Where(c => (!Convert.ToBoolean(c["isValid"]))).ToList().ForEach(c => c.Delete());
                     cb_datasource.DataSource = ds.Tables["Datasources"];
                     cb_datasource.DisplayMember = "TitleFa";
                     cb_datasource.ValueMember = "ID";
                  })
               #endregion
            });
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SelectedProfiler(Job job)
      {
         XDocument xProfiler = XDocument.Parse(job.Input.ToString());
         lnb_profiler.Text = xProfiler.Element("Profiler").Attribute("faName").Value;
         lnb_profiler.Tag = xProfiler;

         flp_profiler.Controls.Clear();

         /* Fire DoWork 4 Prepare Profiler Design */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "ProfilerGroup", cb_allroles.Checked ? 02 : 01, SendType.Self) { Input = xProfiler });

         if (lnb_reportfile.Tag != null)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 11 /* Execute ResetColumnsValidation */),
                     new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 12 /* Execute CheckColumnsValidation */)
                  }));
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void AddGroup(Job job)
      {
         GroupBox gb = job.Input as GroupBox;
         gb.Width = flp_profiler.Width - 10;
         gb.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
         flp_profiler.Controls.Add(gb);
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void ResetColumnsValidation(Job job)
      {
         flp_profiler.Controls.OfType<GroupBox>()
            .ToList()
            .ForEach(gb =>
            {
               gb.Visible = true;
               gb.Controls.OfType<FlowLayoutPanel>().First()
                  .Controls.OfType<Filter>()
                  .ToList()
                  .ForEach(f => f.Visible = true);
            });
         job.Status = StatusType.Successful;
      }

      XDocument xReport;
      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void CheckColumnsValidation(Job job)
      {
         if(job.Input != null)
            xReport = XDocument.Parse(job.Input.ToString());

         flp_profiler.Controls.OfType<GroupBox>()
            .ToList()
            .ForEach(gb =>
            {
               gb.Controls.OfType<FlowLayoutPanel>().First()
                  .Controls.OfType<Filter>()
                  .ToList()
                  .ForEach(f =>
                  {
                     if (!(xReport.Element("XmlReport").Elements("Table")
                           .Where(t =>
                              t.Attribute("name").Value == f.XmlTemplate.Element("Table").Attribute("enName").Value &&
                              t.Elements("Feild").Where(c =>
                                 c.Attribute("name").Value == f.XmlTemplate.Element("Column").Attribute("enName").Value 
                                 //&& c.Attribute("type").Value == f.XmlTemplate.Element("Column").Attribute("type").Value
                              ).Count() == 1
                           ).Count() == 1))
                        f.Visible = false;
                  });
               if (gb.Controls.OfType<FlowLayoutPanel>().First().Controls.OfType<Control>().Where(c => c.Visible).Count() == 0)
                  gb.Visible = false;
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void SetReportOnPlace(Job job)
      {
         XDocument xRep = XDocument.Parse(job.Input.ToString());
         lnb_reportfile.Text = xRep.Element("Report").Element("LogicalName").Value;
         lnb_reportfile.Tag = xRep;
         job.Status = StatusType.Successful;
      }
   }
}
