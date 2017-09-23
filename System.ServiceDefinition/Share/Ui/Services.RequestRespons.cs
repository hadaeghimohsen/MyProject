using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors.Controls;

namespace System.ServiceDefinition.Share.Ui
{
   partial class Services : ISendRequest
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
               LoadGroupHeadersOfRoles(job);
               break;
            case 10:
               LoadParentServicesOfGroupHeaders(job);
               break;
            case 11:
               LoadServicesOfParentService(job);
               break;
            case 12:
               LoadGrpSrvWithCondition(job);
               break;
            case 13:
               LoadGrpSrvForJoin(job);
               break;
            case 14:
               LoadGrpSrvInGrpHdrWithCondition(job);
               break;
            case 15:
               LoadServiceInGrpHdrWithCondition(job);
               break;
            case 16:
               LoadJoinServiceWithCondition(job);
               break;
            case 17:
               LoadServicesWithCondition(job);
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

         if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "Services", 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "ServiceDef",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"ServiceDefinition:Services", this}},
                  new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){Input = this},
              });
         _DefaultGateway.Gateway(_Paint);
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
            new Job(SendType.SelfToUserInterface, "Services", 08 /* LoadRolesOfUser */);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUser(Job job)
      {
         Job _LoadRoleOfUser =
            new Job(SendType.External, "ServiceDefinition",
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
                                    ccbe_roles.Properties.DataSource = ds.Tables["Roles"];
                                    ccbe_roles.Properties.DisplayMember = "RoleFaName";
                                    ccbe_roles.Properties.ValueMember = "RoleID";
                                    ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Take(1).ToList().ForEach(role => role.CheckState = CheckState.Checked);
                                 })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "Services", 09 /* Exective LoadGroupHeadersOfRoles */)
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadGroupHeadersOfRoles(Job job)
      {
         if (ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked).Count() == 0)
         {
            job.Status = StatusType.Failed;
            return;
         }

         Func<string> XmlData = new Func<string>(() =>
            {
               string xmlData = "";
               ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(r => r.CheckState == CheckState.Checked ).ToList()
                  .ForEach(r => 
                     {
                        xmlData += string.Format("<RoleID>{0}</RoleID>",r.Value);
                     });
               return string.Format("<GH><Distinct>true</Distinct>{0}</GH>",xmlData);
            });

         Job _LoadHeaderGroupOfRoles =
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  #region Read Header Groups of Roles
                  new Job(SendType.External, "GroupHeader",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4ReadAccessGroupHeader */)
                        {
                           Input = XmlData(),
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 ccbe_groupheader.Properties.DataSource = ds.Tables["GroupHeader"];
                                 ccbe_groupheader.Properties.DisplayMember = "GHFaName";
                                 ccbe_groupheader.Properties.ValueMember = "GHID";
                                 ccbe_groupheader.Properties.GetItems().OfType<CheckedListBoxItem>().Take(1).ToList().ForEach(hg => hg.CheckState = CheckState.Checked);
                              })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "Services", 10 /* Exective LoadParentServicesOfGroupHeaders */)
               });
         _DefaultGateway.Gateway(_LoadHeaderGroupOfRoles);
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadParentServicesOfGroupHeaders(Job job)
      {
         if (ccbe_groupheader.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked).Count() == 0)
         {
            job.Status = StatusType.Failed;
            return;
         }

         Func<string> XmlData = new Func<string>(() =>
         {
            string xmlData = "";
            ccbe_groupheader.Properties.GetItems().OfType<CheckedListBoxItem>().Where(g => g.CheckState == CheckState.Checked).ToList()
               .ForEach(g =>
               {
                  xmlData += string.Format("<GroupHeaderID>{0}</GroupHeaderID>", g.Value);
               });
            return string.Format("<GroupHeader>{0}</GroupHeader>", xmlData);
         });

         Job _LoadHeaderGroupOfRoles =
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  #region Read Header Groups of Roles
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4LoadParentServicesOfGroupHeaders */)
                        {
                           Input = XmlData(),
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 var GroupHeader = ds.Tables["ParentService"].Rows.OfType<DataRow>().Select(r => new {GhId = (Int64)r["GroupHeaderID"], GhFaName = r["GhFaName"].ToString()/*, SGActive = Convert.ToBoolean(r["SGActive"])*/}).Distinct();
                                 tv_grpsrv.Nodes.Clear();
                                 tv_grpsrv.CheckBoxes = false;
                                 pn_editgrpsrv.Visible = false;
                                 pn_commandgrpsrv.Controls.OfType<Control>().ToList().ForEach(item => item.Enabled = true);   

                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };

                                 GroupHeader
                                    .ToList()
                                    .ForEach(
                                    g =>
                                    {
                                       TreeNode n0 = new TreeNode(g.GhFaName){ ToolTipText = g.GhId.ToString(), SelectedImageIndex = 2, ImageIndex = 2};
                                       tv_grpsrv.Nodes.Add(n0);

                                       #region Create Sub Node
                                       Action<TreeNode, object> PrepareSubNode = null;

                                       Action<TreeNode, object> AddSubNodeInTree =
                                          new Action<TreeNode, object>(
                                             (tn, pid) =>
                                             {
                                                #region Comment
                                                ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                                   .Where(row =>
                                                      Convert.ToInt64(row["GroupHeaderID"]) == Convert.ToInt64(g.GhId) &&
                                                      Convert.ToInt64(row["ParentID"]) == Convert.ToInt64(pid) &&
                                                      Convert.ToBoolean(row["IsVisited"]) == false)
                                                   .ToList()
                                                   .ForEach(r =>
                                                   {
                                                      r["IsVisited"] = true;
                                                      TreeNode ni = new TreeNode(r["SFaName"].ToString())
                                                      {
                                                         Tag = r["ServiceID"],
                                                         SelectedImageIndex = GetImageIndex(r["SGActive"]),
                                                         ImageIndex = GetImageIndex(r["SGActive"])
                                                      };
                                                      tn.Nodes.Add(ni);
                                                      PrepareSubNode(ni, r["ServiceID"]);
                                                   });
                                                #endregion
                                             });

                                       PrepareSubNode = new Action<TreeNode, object>((tn, pid) => AddSubNodeInTree(tn, pid));
                                       #endregion

                                       #region Create Master Node
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId  && Convert.ToInt64(row["ParentID"]) == 0)
                                          .Distinct()
                                          .ToList()
                                          .ForEach(r =>
                                          {
                                             r["IsVisited"] = true;
                                             TreeNode n1 = new TreeNode(r["SFaName"].ToString()){Tag = r["ServiceID"], SelectedImageIndex = GetImageIndex(r["SGActive"]), ImageIndex = GetImageIndex(r["SGActive"])};
                                             n0.Nodes.Add(n1);
                                             AddSubNodeInTree(n1, r["ServiceID"]);                                             
                                          });
                                       #endregion

                                       #region Reminder Row
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId && Convert.ToBoolean(row["IsVisited"]) == false)
                                          .ToList()
                                          .ForEach(remn =>
                                          {
                                             if(!Convert.ToBoolean(remn["IsVisited"]))
                                             { 
                                                remn["IsVisited"] = true;
                                                TreeNode nr = new TreeNode(remn["SFaName"].ToString()) { Tag = remn["ServiceID"], SelectedImageIndex = GetImageIndex(remn["SGActive"]), ImageIndex = GetImageIndex(remn["SGActive"]) };
                                                n0.Nodes.Add(nr);
                                                AddSubNodeInTree(nr, remn["ServiceID"]);
                                             }
                                          });
                                       #endregion                                                
                                    });
                                 tv_grpsrv.ExpandAll();
                              })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "Services", 11 /* Exective LoadServicesOfParentService */)
               });
         _DefaultGateway.Gateway(_LoadHeaderGroupOfRoles);
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadServicesOfParentService(Job job)
      {
         pn_editservice.Visible = false;
         lv_services.CheckBoxes = false;
         lv_services.Items.Clear();
         pn_commandservice.Controls.OfType<Control>().ToList().ForEach(c => c.Enabled = true);

         if (tv_grpsrv.Nodes.Count == 0 || tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
         {
            return;
         }
         var selectGrpSrv = string.Format("<GrpSrv>{0}</GrpSrv>", tv_grpsrv.SelectedNode.Tag);         

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        #region Load Services
                        new Job(SendType.Self, 04 /* Execute DoWork4LoadServicesOfParentService */)
                        {
                           Input = selectGrpSrv,
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 #region Add Service in list
                                 DataSet ds = output as DataSet;
                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };

                                 

                                 ds.Tables["Services"].Rows.Cast<DataRow>()
                                    .ToList()
                                    .ForEach(rp =>
                                    {
                                       var item = new ListViewItem(rp["TitleFa"].ToString())
                                          {
                                             ImageIndex = GetImageIndex(rp["IsActive"]),
                                             Tag = string.Format("<ServiceID>{0}</ServiceID>", rp["ID"])
                                          };
                                       item.SubItems.Add(rp["Price"].ToString());
                                       lv_services.Items.Add(item);
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
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void LoadGrpSrvWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
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
                              job.Input,
                              "{ Call ServiceDef.LoadGrpSrvWithCondition(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 
                                 tv_grpsrv.Nodes.Clear();
                                 if(job.Input.ToString() == "<WhatAction>true</WhatAction>")
                                 { 
                                    tv_grpsrv.SelectedImageIndex = 2;
                                    tv_grpsrv.ImageIndex = 2;
                                 }
                                 else
                                 {
                                    tv_grpsrv.SelectedImageIndex = 5;
                                    tv_grpsrv.ImageIndex = 5;
                                 }
                                 var GroupHeader = ds.Tables["ParentService"].Rows.OfType<DataRow>().Select(ps => new {ParentID = ps["ParentID"]}).Distinct();//.OrderBy(ps => ps.ParentID);
                                 GroupHeader
                                    .ToList()
                                    .ForEach(
                                    g =>
                                    {
                                       #region Create Sub Node
                                       Action<TreeNode, object> PrepareSubNode = null;

                                       Action<TreeNode, object> AddSubNodeInTree =
                                          new Action<TreeNode, object>(
                                             (tn, pid) =>
                                             {
                                                ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                                   .Where(row => Convert.ToInt64(row["ParentID"]) == Convert.ToInt64(pid))
                                                   .ToList()
                                                   .ForEach(r =>
                                                   {
                                                      r["IsVisited"] = true;
                                                      TreeNode ni = new TreeNode(r["TitleFa"].ToString()) { Tag = r["ID"] };
                                                      tn.Nodes.Add(ni);
                                                      PrepareSubNode(ni, r["ID"]);
                                                   });
                                             });

                                       PrepareSubNode = new Action<TreeNode, object>((tn, pid) => AddSubNodeInTree(tn, pid));
                                       #endregion

                                       #region Create Master Node
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["ParentID"]) == Convert.ToInt64(g.ParentID) && !Convert.ToBoolean(row["IsVisited"]))
                                          .Distinct()
                                          .ToList()
                                          .ForEach(r =>
                                          {
                                             r["IsVisited"] = true;
                                             TreeNode n1 = new TreeNode(r["TitleFa"].ToString()){Tag = r["ID"]};
                                             tv_grpsrv.Nodes.Add(n1);
                                             AddSubNodeInTree(n1, r["ID"]);                                             
                                          });
                                       #endregion

                                       //#region Reminder Row
                                       //ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                       //   .Where(row => Convert.ToBoolean(row["IsVisited"]) == false)
                                       //   .ToList()
                                       //   .ForEach(remn =>
                                       //   {
                                       //      remn["IsVisited"] = true;
                                       //      TreeNode nr = new TreeNode(remn["TitleFa"].ToString()) { Tag = remn["ID"] };
                                       //      tv_grpsrv.Nodes.Add(nr);
                                       //      //AddSubNodeInTree(nr, remn["ID"]);
                                       //   });
                                       //#endregion           
                                    });

                                 #region Reminder Row
                                 ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                    .Where(row => Convert.ToBoolean(row["IsVisited"]) == false)
                                    .ToList()
                                    .ForEach(remn =>
                                    {
                                       if(!Convert.ToBoolean(remn["IsVisited"]))
                                       { 
                                          remn["IsVisited"] = true;
                                          TreeNode nr = new TreeNode(remn["TitleFa"].ToString()) { Tag = remn["ID"] };
                                          tv_grpsrv.Nodes.Add(nr);
                                          //AddSubNodeInTree(nr, remn["ID"]);
                                       }
                                    });
                                 #endregion                                                
                                 tv_grpsrv.ExpandAll();

                              })
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void LoadGrpSrvForJoin(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
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
                              job.Input,
                              "{ Call ServiceDef.LoadJoinGroupServices(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 var groups = ds.Tables["ParentService"].Rows.OfType<DataRow>().Select(g => new { GhFaName = g["GhFaName"].ToString(), GhID = Convert.ToInt64(g["GhID"]) }).Distinct();
                                 tv_grpsrv.Nodes.Clear();
                                 groups
                                    .ToList()
                                    .ForEach(g => 
                                    {
                                       TreeNode n0 = new TreeNode(g.GhFaName) { Tag = g.GhID, SelectedImageIndex = 2, ImageIndex = 2 };
                                       tv_grpsrv.Nodes.Add(n0);

                                       #region Create Sub Node
                                       Action<TreeNode, object> PrepareSubNode = null;

                                       Action<TreeNode, object> AddSubNodeInTree =
                                          new Action<TreeNode, object>(
                                             (tn, pid) =>
                                             {
                                                ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                                   .Where(row => Convert.ToInt64(row["GhID"]) == g.GhID && (Int64)row["ParentID"] == (Int64)pid)
                                                   .ToList()
                                                   .ForEach(r =>
                                                   {
                                                      r["IsVisited"] = true;
                                                      TreeNode ni = new TreeNode(r["SFaName"].ToString()) { Tag = r["ServiceID"], SelectedImageIndex = 2, ImageIndex = 2 };
                                                      tn.Nodes.Add(ni);
                                                      PrepareSubNode(ni, r["ServiceID"]);
                                                   });
                                             });

                                       PrepareSubNode = new Action<TreeNode, object>((tn, pid) => AddSubNodeInTree(tn, pid));
                                       #endregion

                                       #region Create Master Node
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GhID"]) == g.GhID && Convert.ToInt64(row["ParentID"]) == 0)
                                          .Distinct()
                                          .ToList()
                                          .ForEach(r =>
                                          {
                                             r["IsVisited"] = true;
                                             TreeNode n1 = new TreeNode(r["SFaName"].ToString()) { Tag = r["ServiceID"], SelectedImageIndex = 2, ImageIndex = 2 };
                                             n0.Nodes.Add(n1);
                                             AddSubNodeInTree(n1, r["ServiceID"]);
                                          });
                                       #endregion

                                       #region Reminder Row
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GhID"]) == g.GhID && Convert.ToBoolean(row["IsVisited"]) == false)
                                          .ToList()
                                          .ForEach(remn =>
                                          {
                                             if(!Convert.ToBoolean(remn["IsVisited"]))
                                             {
                                                remn["IsVisited"] = true;
                                                TreeNode nr = new TreeNode(remn["SFaName"].ToString()) { Tag = remn["ServiceID"], SelectedImageIndex = 2, ImageIndex = 2 };
                                                n0.Nodes.Add(nr);
                                                AddSubNodeInTree(nr, remn["ServiceID"]);
                                             }
                                          });
                                       #endregion                                                
                                    });
                                 tv_grpsrv.ExpandAll();
                              })
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void LoadGrpSrvInGrpHdrWithCondition(Job job)
      {
         if (ccbe_groupheader.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked).Count() == 0)
         {
            job.Status = StatusType.Failed;
            return;
         }

         Job _LoadHeaderGroupOfRoles =
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  #region Read Header Groups of Roles
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        new Job(SendType.Self, 06 /* Execute DoWork4LoadGrpSrvInGrpHdrWithCondition */)
                        {
                           Input = job.Input,
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 var GroupHeader = ds.Tables["ParentService"].Rows
                                                                             .OfType<DataRow>()
                                                                             .Select(r => 
                                                                                new {
                                                                                       GhId = (Int64)r["GroupHeaderID"], 
                                                                                       GhFaName = r["GhFaName"].ToString(), 
                                                                                       SGActive = Convert.ToBoolean(r["SGActive"])})
                                                                             .Distinct();
                                 tv_grpsrv.Nodes.Clear();
                                 //pn_commandgrpsrv.Controls.OfType<Control>().ToList().ForEach(item => item.Enabled = true);   

                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };

                                 GroupHeader
                                    .ToList()
                                    .ForEach(
                                    g =>
                                    {
                                       TreeNode n0 = new TreeNode(g.GhFaName){ Tag = g.GhId, SelectedImageIndex = GetImageIndex(g.SGActive), ImageIndex = GetImageIndex(g.SGActive)};
                                       tv_grpsrv.Nodes.Add(n0);

                                       #region Create Sub Node
                                       Action<TreeNode, object> PrepareSubNode = null;

                                       Action<TreeNode, object> AddSubNodeInTree =
                                          new Action<TreeNode, object>(
                                             (tn, pid) =>
                                             {
                                                ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                                   .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId && (Int64)row["ParentID"] == (Int64)pid)
                                                   .ToList()
                                                   .ForEach(r =>
                                                   {
                                                      r["IsVisited"] = true;
                                                      TreeNode ni = new TreeNode(r["SFaName"].ToString()) { Tag = r["ServiceID"], SelectedImageIndex = GetImageIndex(r["SGActive"]), ImageIndex = GetImageIndex(r["SGActive"]) };
                                                      tn.Nodes.Add(ni);
                                                      PrepareSubNode(ni, r["ServiceID"]);
                                                   });
                                             });

                                       PrepareSubNode = new Action<TreeNode, object>((tn, pid) => AddSubNodeInTree(tn, pid));
                                       #endregion

                                       #region Create Master Node
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId  && Convert.ToInt64(row["ParentID"]) == 0)
                                          .Distinct()
                                          .ToList()
                                          .ForEach(r =>
                                          {
                                             r["IsVisited"] = true;
                                             TreeNode n1 = new TreeNode(r["SFaName"].ToString()){Tag = r["ServiceID"], SelectedImageIndex = GetImageIndex(r["SGActive"]), ImageIndex = GetImageIndex(r["SGActive"])};
                                             n0.Nodes.Add(n1);
                                             AddSubNodeInTree(n1, r["ServiceID"]);                                             
                                          });
                                       #endregion

                                       #region Reminder Row
                                       ds.Tables["ParentService"].Rows.OfType<DataRow>()
                                          .Where(row => Convert.ToInt64(row["GroupHeaderID"]) == g.GhId && Convert.ToBoolean(row["IsVisited"]) == false)
                                          .ToList()
                                          .ForEach(remn =>
                                          {
                                             if(!Convert.ToBoolean(remn["IsVisited"]))
                                             {
                                                remn["IsVisited"] = true;
                                                TreeNode nr = new TreeNode(remn["SFaName"].ToString()) { Tag = remn["ServiceID"], SelectedImageIndex = GetImageIndex(remn["SGActive"]), ImageIndex = GetImageIndex(remn["SGActive"]) };
                                                n0.Nodes.Add(nr);
                                                AddSubNodeInTree(nr, remn["ServiceID"]);
                                             }
                                          });
                                       #endregion                                                
                                    });
                                 tv_grpsrv.ExpandAll();
                              })
                        }
                     }),
                  #endregion
                  //new Job(SendType.SelfToUserInterface, "Services", 11 /* Exective LoadServicesOfParentService */)
               });
         _DefaultGateway.Gateway(_LoadHeaderGroupOfRoles);
      }

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void LoadServiceInGrpHdrWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDef",
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
                              job.Input,
                              "{ Call ServiceDef.LoadServiceInGrpHdrWithCondition(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 lv_services.Items.Clear();

                                 ds.Tables["Services"].Rows.Cast<DataRow>()
                                    .ToList()
                                    .ForEach(rp =>
                                    {
                                       var item = new ListViewItem(rp["TitleFa"].ToString())
                                          {
                                             ImageIndex = 23,
                                             Tag = string.Format("<ServiceID>{0}</ServiceID>", rp["ID"])
                                          };
                                       item.SubItems.Add(rp["Price"].ToString());
                                       lv_services.Items.Add(item);
                                    });
                              })
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void LoadJoinServiceWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDef",
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
                              "",
                              "{ Call ServiceDef.LoadJoinServiceWithCondition }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 lv_services.Items.Clear();

                                 ds.Tables["Services"].Rows.Cast<DataRow>()
                                    .ToList()
                                    .ForEach(rp =>
                                    {
                                       var item = new ListViewItem(rp["TitleFa"].ToString())
                                          {
                                             ImageIndex = 21,
                                             Tag = string.Format("<ServiceID>{0}</ServiceID>", rp["ID"])
                                          };
                                       item.SubItems.Add(rp["Price"].ToString());
                                       lv_services.Items.Add(item);
                                    });
                              })
                        }
                     })
               }));
      }

      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void LoadServicesWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDef",
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
                              "",
                              "{ Call ServiceDef.LoadServicesWithCondition }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 DataSet ds = output as DataSet;
                                 lv_services.Items.Clear();

                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };


                                 ds.Tables["Services"].Rows.Cast<DataRow>()
                                    .ToList()
                                    .ForEach(rp =>
                                    {
                                       var item = new ListViewItem(rp["TitleFa"].ToString())
                                          {
                                             ImageIndex = GetImageIndex(rp["IsActive"]),
                                             Tag = string.Format("<ServiceID>{0}</ServiceID>", rp["ID"])
                                          };
                                       item.SubItems.Add(rp["Price"].ToString());
                                       lv_services.Items.Add(item);
                                    });
                              })
                        }
                     })
               }));
      }

   }
}
