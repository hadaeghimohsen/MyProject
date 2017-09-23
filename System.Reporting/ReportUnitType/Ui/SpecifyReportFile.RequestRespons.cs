using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Controls;

namespace System.Reporting.ReportUnitType.Ui
{
   partial class SpecifyReportFile : ISendRequest
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
               LoadRolesOfUser(job);
               break;
            case 09:
               LoadTypeOfRoles(job);
               break;
            case 10:
               LoadCabinetOfRoles(job);
               break;
            case 11:
               LoadReportPrinter(job);
               break;
            case 12:
               LoadReportFiles(job);
               break;
            case 13:
               LoadWhatsRepprtingFiles(job);
               break;
            case 14:
               ShowChooseReport(job);
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
               new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 04 /* Execute UnPaint */);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportUnitType:SpecifyReportFile", this }  },
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
         job.Next =
            new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 08 /* LoadRolesOfUser */);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUser(Job job)
      {
         Job _LoadRoleOfUser =
            new Job(SendType.External, "DefaultGateway",
               new List<Job>
               {
                  #region Read All Access Roles
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute ReadAllAccessRoles */)
                        {
                           AfterChangedOutput = 
                              new Action<object>(
                                 (output) =>
                                 {
                                    DataSet ds = output as DataSet;
                                    cb_roles.DataSource = ds.Tables["Roles"];
                                    cb_roles.DisplayMember = "RoleFaName";
                                    cb_roles.ValueMember = "RoleID";                                    
                                 })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 09 /* Exective LoadTypeOfRoles */)
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadTypeOfRoles(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "DefaultGateway",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 14 /* Execute ReadTypeOfRoles*/)
                        {
                           Input = string.Format("<RoleID>{0}</RoleID><UnitType>1</UnitType>", cb_roles.SelectedValue),
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 DataSet ds = output as DataSet;
                                 ccbe_servtype.Properties.DataSource = ds.Tables["UnitType"];
                                 ccbe_servtype.Properties.DisplayMember = "TitleFa";
                                 ccbe_servtype.Properties.ValueMember = "UnitTypeid";
                                 //ccbe_servtype.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(s => s.CheckState = CheckState.Checked);
                              })
                        }
                     }),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 10 /* Execute LoadGrpHdrOfRoles */)
               }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadCabinetOfRoles(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "DefaultGateway",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 15 /* Execute DoWork4ReadGroupHeaderOfRoles */)
                        {
                           Input = string.Format("<GH><Distinct>true</Distinct><RoleID>{0}</RoleID></GH>", cb_roles.SelectedValue),
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 DataSet ds = output as DataSet;
                                 ccbe_cabinet.Properties.DataSource = ds.Tables["GroupHeader"];
                                 ccbe_cabinet.Properties.DisplayMember = "GHFaName";
                                 ccbe_cabinet.Properties.ValueMember = "GHID";
                                 //ccbe_cabinet.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(s => s.CheckState = CheckState.Checked);
                              })
                        }
                     }),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 13 /* Execute LoadWhatsRepprtingFiles */),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 11 /* Execute LoadReportPrinter */)
               }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadReportPrinter(Job job)
      {
         #region Section 1
         if (ccbe_servtype.Properties.GetItems().OfType<CheckedListBoxItem>().Where(type => type.CheckState == CheckState.Checked).Count() == 0)
            lv_reportfiles.Items.Clear();

         if (ccbe_cabinet.Properties.GetItems().OfType<CheckedListBoxItem>().Where(type => type.CheckState == CheckState.Checked).Count() == 0)
         {
            tv_reports.Nodes.Clear();
            lv_reportfiles.Items.Clear();
            job.Status = StatusType.Failed;
            return;
         }
         #endregion


         Func<string> XmlData = new Func<string>(() =>
         {
            string xmlData = "";
            ccbe_cabinet.Properties.GetItems().OfType<CheckedListBoxItem>().Where(g => g.CheckState == CheckState.Checked).ToList()
               .ForEach(g =>
               {
                  xmlData += string.Format("<GroupHeaderID>{0}</GroupHeaderID>", g.Value);
               });
            return string.Format("<GroupHeader>{0}</GroupHeader>", xmlData);
         });

         Job _LoadHeaderGroupOfRoles =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  #region Read Header Groups of Roles
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 16 /* Execute DoWork4LoadParentServicesOfGroupHeaders */)
                        {
                           Input = XmlData(),
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 var GroupHeader = ds.Tables["ParentService"].Rows.OfType<DataRow>().Select(r => new {GhId = (Int64)r["GroupHeaderID"], GhFaName = r["GhFaName"].ToString()/*, SGActive = Convert.ToBoolean(r["SGActive"])*/}).Distinct();
                                 tv_reports.Nodes.Clear();
                                 tv_reports.CheckBoxes = false;


                                 GroupHeader
                                    .ToList()
                                    .ForEach(
                                    g =>
                                    {
                                       TreeNode n0 = new TreeNode(g.GhFaName){ ToolTipText = g.GhId.ToString(), SelectedImageIndex = 2, ImageIndex = 2};
                                       tv_reports.Nodes.Add(n0);

                                       #region Create Sub Node
                                       Action<TreeNode, object> PrepareSubNode = null;

                                       Action<TreeNode, object> AddSubNodeInTree =
                                          new Action<TreeNode, object>(
                                             (tn, pid) =>
                                             {
                                                ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                                   .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId && (Int64)row["ParentID"] == (Int64)pid && Convert.ToBoolean(row["SgActive"]))
                                                   .ToList()
                                                   .ForEach(r =>
                                                   {
                                                      r["IsVisited"] = true;
                                                      TreeNode ni = new TreeNode(r["SFaName"].ToString()) { Tag = r["ServiceID"], SelectedImageIndex = 1, ImageIndex =1 };
                                                      tn.Nodes.Add(ni);
                                                      PrepareSubNode(ni, r["ServiceID"]);
                                                   });
                                             });

                                       PrepareSubNode = new Action<TreeNode, object>((tn, pid) => AddSubNodeInTree(tn, pid));
                                       #endregion

                                       #region Create Master Node
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId  && Convert.ToInt64(row["ParentID"]) == 0 && Convert.ToBoolean(row["SgActive"]))
                                          .Distinct()
                                          .ToList()
                                          .ForEach(r =>
                                          {
                                             r["IsVisited"] = true;
                                             TreeNode n1 = new TreeNode(r["SFaName"].ToString()){Tag = r["ServiceID"], SelectedImageIndex = 1, ImageIndex = 1};
                                             n0.Nodes.Add(n1);
                                             AddSubNodeInTree(n1, r["ServiceID"]);                                             
                                          });
                                       #endregion

                                       #region Reminder Row
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId && Convert.ToBoolean(row["IsVisited"]) == false && Convert.ToBoolean(row["SgActive"]))
                                          .ToList()
                                          .ForEach(remn =>
                                          {
                                             if(!Convert.ToBoolean(remn["IsVisited"]))
                                             {
                                                remn["IsVisited"] = true;
                                                TreeNode nr = new TreeNode(remn["SFaName"].ToString()) { Tag = remn["ServiceID"], SelectedImageIndex =1, ImageIndex = 1 };
                                                n0.Nodes.Add(nr);
                                                AddSubNodeInTree(nr, remn["ServiceID"]);
                                             }
                                          });
                                       #endregion                                                
                                    });
                                 tv_reports.ExpandAll();
                              })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 12 /* Exective LoadReportFiles */)
               });
         _DefaultGateway.Gateway(_LoadHeaderGroupOfRoles);
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void LoadReportFiles(Job job)
      {
         if (tv_reports.Nodes.Count == 0 || tv_reports.SelectedNode == null || tv_reports.SelectedNode.Tag == null || ccbe_servtype.Properties.GetCheckedItems() == null)
            return;

         string v_temp = "";
         ccbe_servtype.Properties.GetItems().OfType<CheckedListBoxItem>().Where(item => item.CheckState == CheckState.Checked).ToList().ForEach(ic => v_temp += string.Format("<Typeid>{0}</Typeid>", ic.Value));

         var selectGrpSrv = string.Format("<Query><GrpSrv>{0}</GrpSrv><Types>{1}</Types></Query>", tv_reports.SelectedNode.Tag, v_temp);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Load Services
                        new Job(SendType.Self, 17 /* Execute DoWork4LoadServicesOfParentService */)
                        {
                           Input = selectGrpSrv,
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 #region Add Service in list
                                 DataSet ds = output as DataSet;
                                 lv_reportfiles.Items.Clear();

                                 ds.Tables["Services"].Rows.OfType<DataRow>()
                                    .ToList()
                                    .ForEach(rpt =>
                                    {
                                       ListViewItem ins = new ListViewItem(rpt["TitleFa"].ToString()){Tag = string.Format(@"<Report ID=""{0}"">{1}</Report>", rpt["ID"], rpt["TitleEn"]), ImageIndex = 0};
                                       lv_reportfiles.Items.Add(ins);
                                    });

                                 #endregion
                              })
                        }
                        #endregion
                     })
               }));

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void LoadWhatsRepprtingFiles(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              string.Format("<RoleID>{0}</RoleID>", cb_roles.SelectedValue),
                              "{ Call Report.WhatsReportingFiles(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 ds.Tables["Cabinets"].Rows.OfType<DataRow>().ToList().ForEach(c => 
                                 {
                                    ccbe_cabinet.Properties.GetItems().OfType<CheckedListBoxItem>().Where(cc => Convert.ToInt64(cc.Value) == Convert.ToInt64(c["Cid"])).ToList().ForEach(cc => cc.CheckState = CheckState.Checked);
                                 });

                                 ds.Tables["Folders"].Rows.OfType<DataRow>().ToList().ForEach(f =>
                                 {
                                    ccbe_servtype.Properties.GetItems().OfType<CheckedListBoxItem>().Where(ff => Convert.ToInt64(ff.Value) == Convert.ToInt64(f["Tid"])).ToList().ForEach(ff => ff.CheckState = CheckState.Checked);
                                 });
                              })                     
                        }
                     })
               }));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void ShowChooseReport(Job job)
      {
         sb_fetchdata.Visible = true;
         sb_fetchdata.Tag = job.Input;
         job.Status = StatusType.Successful;
      }

   }
}
