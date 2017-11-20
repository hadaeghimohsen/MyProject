using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class WHR_SCON_F : IDefaultGateway
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
               AddGroup(job);
               break;
            case 09:
               LoadActiveDataSource(job);
               break;
            case 10:
               UpdateNewFilePath(job);
               break;
            default:
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
         else if (keyData == Keys.F8){
            ShowFormulaSelection();
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.F5)
         {
            ShowPreview();
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
         xRunReport = (XDocument)job.Input;
         xReportFileContent = null;
         resultQuery = false;
         flp_wherecondition.Controls.Clear();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:WorkFlow:WHR_SCON_F", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
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
         if (InvokeRequired) Invoke(new Action(() =>
            { 
               mpbc_loading.Visible = true;
               bp_flow.Buttons[2].Properties.Enabled = false;
            }));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "DefaultGateway",
                     new List<Job>
                     {
                        new Job(SendType.External, "ReportProfiler",
                           new List<Job>
                           {
                              new Job(SendType.External, "ProfilerGroup",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 02 /* Execute PrepareNProfilerDesign4Role */)
                                    {
                                       Input = xRunReport.Descendants("Profiler").First(),
                                       Executive = ExecutiveType.Asynchronous,
                                       AfterChangedOutput = new Action<object>((output) =>
                                          {
                                             Invoke(new Action(() =>
                                             {
                                                /* آماده کردن برای مقایسه کردن */
                                                ResultQuery = true;
                                             }));
                                          })
                                    }
                                 }),
                              new Job(SendType.External, "ReportFile",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 01 /* Execute GetXMLReport */)
                                    {
                                       Input = xRunReport.Descendants("Report").First(),
                                       Executive = ExecutiveType.Asynchronous,
                                       AfterChangedOutput = new Action<object>((output) =>
                                          {
                                             Invoke(new Action(() =>
                                             {
                                                /* آماده کردن برای مقایسه کردن */
                                                XReportFileContent = XDocument.Parse(output.ToString());
                                             }));
                                          })
                                    }
                                 })
                           })
                     })
               }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void AddGroup(Job job)
      {
         Invoke(new Action(() =>
            {
               FlowLayoutPanel flp = job.Input as FlowLayoutPanel;
               flp.Width = flp_wherecondition.Width - 10;
               flp_wherecondition.Controls.Add(flp);               
            }));

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
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
                     Invoke(new Action(() =>
                     {
                        cb_datasource.DataSource = ds.Tables["Datasources"];
                        cb_datasource.DisplayMember = "TitleFa";
                        cb_datasource.ValueMember = "ID";

                        cb_datasource.SelectedValue = xRunReport.Descendants("Profiler").First().Attribute("dataSource").Value;
                     }));
                  })
               #endregion
            });
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void UpdateNewFilePath(Job job)
      {
         xReportFileContent = null;
         resultQuery = false;
         flp_wherecondition.Controls.Clear();
         xRunReport.Descendants("PhysicalName").First().SetValue(job.Input);
         job.Next =
            new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 07 /* Execute LoadData */);
         job.Status = StatusType.Successful;
      }
   }
}
