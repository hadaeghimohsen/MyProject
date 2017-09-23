using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_SPRF_F : IDefaultGateway
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
               CancelFilterItem(job);
               break;
            case 09:
               TitleCountItem(job);
               break;
            case 10:
               TitleFilterItem(job);
               break;
            case 13:
               SetItemCheckMode(job);
               break;
            case 14:
               GetSelectedProfilers(job);
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
         else if (keyData == Keys.F3)
         {
            job.Next =
               new Job(SendType.Self, 04 /* Execute DoWork4PRF_SRCH_F */);
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 04 /* Execute UnPaint */);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:WorkFlow:PRF_SPRF_F", this }  },
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
         #region Get Current User Name And Call GetAccessReports Procedure
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                        {
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 #region Prepare Execute Procedure
                                 _DefaultGateway.Gateway(
                                    new Job(SendType.External, "Localhost",
                                       new List<Job>
                                       {
                                          new Job(SendType.External, "Commons",
                                             new List<Job>
                                             {
                                                new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                                                {
                                                   #region Execute Procedure
                                                   Input = new List<object>
                                                   {
                                                      false,
                                                      "procedure",
                                                      true,
                                                      true,
                                                      "xml",
                                                      string.Format(@"<Request type=""USER""><User>{0}</User></Request>", output.ToString()),
                                                      "{ Call Report.GetAccessProfilers(?) }",
                                                      "iProject",
                                                      "scott"
                                                   }, 
                                                   AfterChangedOutput = new Action<object>(
                                                      (xoutput) =>
                                                         {
                                                            try{
                                                            OperationComplete = false;
                                                            DataSet ds = xoutput as DataSet;
                                                            xAccessProfiler = XDocument.Parse(ds.Tables["AccessProfiler"].Rows[0]["xAccessProfiler"].ToString());
                                                            Processing();
                                                            OperationComplete = true;
                                                            }catch
                                                            {
                                                               OperationComplete = true;
                                                            }
                                                         })
                                                   #endregion
                                                }
                                             })
                                       }));                                       
                                 #endregion
                              })
                        }
                     })
               }));
         #endregion
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void CancelFilterItem(Job job)
      {
         Tile_Ctrl.Groups.OfType<TileGroup>().ToList()
            .ForEach(g =>
            {
               g.Items.OfType<TileItem>().Where(item => !item.Visible).ToList()
                  .ForEach(item =>
                  {
                     item.Visible = true;
                  });
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void TitleCountItem(Job job)
      {
         job.Output = xAccessProfiler.Descendants("Profiler").Where(ln => ln.Attribute("faName").Value.Replace(" ", string.Empty).Contains(job.Input.ToString())).Count();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void TitleFilterItem(Job job)
      {
         Tile_Ctrl.Groups.OfType<TileGroup>().ToList()
            .ForEach(g =>
            {
               g.Items.OfType<TileItem>().Where(item => !item.Elements[0].Text.Replace(" ", string.Empty).Contains(job.Input.ToString())).ToList()
                  .ForEach(item =>
                  {
                     item.Visible = false;
                  });
               g.Items.OfType<TileItem>().Where(item => item.Elements[0].Text.Replace(" ", string.Empty).Contains(job.Input.ToString())).ToList()
                  .ForEach(item =>
                  {
                     item.Visible = true;
                  });
            });
         job.Output = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void SetItemCheckMode(Job job)
      {
         Tile_Ctrl.ItemCheckMode = TileItemCheckMode.None;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void GetSelectedProfilers(Job job)
      {
         XDocument xSelectedProfilers = XDocument.Parse("<Profilers/>");
         Tile_Ctrl.Groups.OfType<TileGroup>()
            //.Where(g => g.Text != "گزارشات پرکاربرد")
            .ToList()
            .ForEach(g =>
            {
               g.Items.OfType<TileItem>()
                  .Where(item => item.Checked)
                  /*.Where(item => xSelectedProfilers
                                 .Descendants("Report")
                                 .Where(rpt => rpt.Attribute("id").Value == (item.Tag as XElement).Attribute("id").Value &&
                                               rpt.Attribute("roleId").Value == (item.Tag as XElement).Attribute("roleId").Value)
                                 .Count() == 0)*/
                  .ToList()
                  .ForEach(item => xSelectedProfilers.Element("Profilers").Add(item.Tag));
            });
         if (xSelectedProfilers.Descendants("Profiler").Count() > 0)
            job.Output = xSelectedProfilers;
         job.Status = StatusType.Successful;
      }

   }
}
