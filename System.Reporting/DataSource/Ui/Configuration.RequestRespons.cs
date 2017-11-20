using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors;

namespace System.Reporting.DataSource.Ui
{
   partial class Configuration : ISendRequest
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
               LoadDataSources(job);
               break;
            case 09:
               LoadSingleDataSource(job);
               break;
            case 10:
               LoadReminderDataSourceWithCondition(job);
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
            job.Next =
               new Job(SendType.SelfToUserInterface, "Configuration", 04 /* Execute UnPaint */);
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
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:Datasource:Configuration", this }  },
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
         gb_connectionstring.Visible = false;
         pn_CommandMaster.Controls.OfType<Control>().ToList().ForEach(c => c.Enabled = true);
         cb_databaseserver.SelectedIndex = 0;
         lv_datasources.CheckBoxes = false;
         pn_editcstr.Visible = false;

         job.Next =
            new Job(SendType.SelfToUserInterface,"Configuration" , 08 /* Execute LoadDataSources */);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadDataSources(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           #region Odbc
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
                           #region Load Data Source
                           AfterChangedOutput = new Action<object>
                           ((output) => 
                              {
                                 DataSet ds = output as DataSet;

                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };

                                 lv_datasources.Items.Clear();

                                 ds.Tables["Datasources"].Rows
                                    .OfType<DataRow>()
                                    .ToList()
                                    .ForEach(dbs => 
                                    {
                                       ListViewItem lvi = new ListViewItem(dbs["TitleFa"].ToString()){Tag = dbs["ID"], ImageIndex = GetImageIndex(dbs["IsActive"])};
                                       lvi.SubItems.Add(dbs["DatabaseServer"].ToString());
                                       lv_datasources.Items.Add(lvi);
                                    });
                              })
                           #endregion
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadSingleDataSource(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           #region Odbc
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              job.Input,
                              "{ Call Report.LoadSingleDataSource(?) }",
                              "iProject",
                              "scott"
                           },
                           #endregion
                           #region Load Data Source
                           AfterChangedOutput = new Action<object>
                           ((output) => 
                              {
                                 var ds = (output as DataSet).Tables["DataSource"].Rows.OfType<DataRow>().First();
                                 
                                 cb_databaseserver.SelectedIndex = Convert.ToInt16(ds["DatabaseServer"]);
                                 gb_connectionstring
                                    .Controls
                                    .OfType<TextEdit>()
                                    .ToList()
                                    .ForEach(te =>
                                    {
                                       te.Text = ds[te.Tag.ToString()].ToString();
                                    });

                                 gb_connectionstring.Visible = true;
                                 te_rw_f_ip.Focus();
                              })
                           #endregion
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadReminderDataSourceWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Datasource",
               new List<Job>
                  {
                     new Job(SendType.External, "Commons",
                        new List<Job>
                        {
                           new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                           {
                              #region Odbc
                              Input = new List<object>
                              {
                                 false,
                                 "procedure",
                                 true,
                                 true,
                                 "xml",
                                 job.Input,
                                 "{ Call Report.LoadReminderDataSourceWithCondition(?) }",
                                 "iProject",
                                 "scott"
                              },
                              #endregion
                              #region Load Data Source
                              AfterChangedOutput = new Action<object>
                              ((output) => 
                                 {
                                    DataSet ds = output as DataSet;

                                    Func<object, int> GetImageIndex = (ra) =>
                                    {
                                       bool value = (bool)ra;
                                       return value ? 2 : 5;
                                    };

                                    lv_datasources.Items.Clear();
                                    lv_datasources.CheckBoxes = true;

                                    ds.Tables["Datasources"].Rows
                                       .OfType<DataRow>()
                                       .ToList()
                                       .ForEach(dbs => 
                                       {
                                          ListViewItem lvi = new ListViewItem(dbs["TitleFa"].ToString()){Tag = dbs["ID"], ImageIndex = GetImageIndex(dbs["IsActive"])};
                                          lvi.SubItems.Add(dbs["DatabaseServer"].ToString());
                                          lv_datasources.Items.Add(lvi);
                                       });
                                 })
                              #endregion
                           }
                        })
                  }));
      }
   }
}
