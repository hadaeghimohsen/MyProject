using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Data;

namespace System.Reporting.ReportProfiler.Ui
{
   partial class SpecifyGroupItems : ISendRequest
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
               LoadRegisteredTables(job);
               break;
            case 10:
               LoadColumnsOfTable(job);
               break;
            case 11: 
               LoadSavedItemsinGroup(job);
               break;
            case 12:
               LoadXMLReportInLeftTree(job);
               break;
            case 13:
               PreSync(job);
               break;
            case 14:
               SetDescReport(job);
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
            if (freeForm)
               job.Next =
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 04 /* Execute UnPaint */);
            else
               sb_cancel_Click(null, null);
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
         tv_left.Nodes.Clear();
         string xml = job.Input.ToString();
         tb_group.Text = XDocument.Parse(xml).Element("Group").Element("FaName").Value;
         tb_group.Tag = XDocument.Parse(xml).Element("Group").Attribute("ID").Value;
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportProfiler:SpecifyGroupItems", this }  },
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
            new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 08 /* LoadRolesOfUser */);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUser(Job job)
      {
         EnabledControls();
         FreeForm();
         pn_command.Enabled = true;
         pn_transfer.Enabled = true;
         pn_command.Controls.OfType<Control>().Where(c => !(c.Enabled)).ToList().ForEach(c => c.Enabled = true);
         pn_edit.Visible = false;
         pn_orderindex.Visible = false;
         tv_right.CheckBoxes = false;

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
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 09 /* Execute LoadRegisteredTables */){Input = "Normal"},
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 11 /* Execute LoadSavedItemsinGroup */){Input = "Normal"}
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadRegisteredTables(Job job)
      {
         string Request = string.Format(@"<Request type=""{0}""/>", job.Input);

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
                  Request,
                  "{ Call Report.GetTableUsage(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                     {
                        DataSet ds = output as DataSet;
                        cb_regtables.DataSource = ds.Tables["TableUsage"];
                        cb_regtables.DisplayMember = "FaName";
                        cb_regtables.ValueMember = "ID";
                     })               
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadColumnsOfTable(Job job)
      {
         if (cb_regtables.SelectedValue == null)
            return;
         try { if (cb_regtables.SelectedValue is DataRowView) return; }
         catch { return; }

         string Request = string.Format(@"<Request type=""{0}""><Table>{1}</Table></Request>", job.Input, cb_regtables.SelectedValue);
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
                  Request,
                  "{ Call Report.GetColumnUsage(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                     {
                        DataSet ds = output as DataSet;
                        cbe_column.Properties.DataSource = ds.Tables["ColumnUsage"];
                        cbe_column.Properties.DisplayMember = "FaName";
                        cbe_column.Properties.ValueMember = "ID";
                     })
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadSavedItemsinGroup(Job job)
      {
         if (cb_roles.SelectedItem == null || cb_roles.SelectedValue is DataRowView)
            return;

         tv_right.Nodes.Clear();

         string Request = string.Format(@"<Request type=""{0}""><Group>{1}</Group><Role>{2}</Role></Request>", job.Input, tb_group.Tag, cb_roles.SelectedValue);
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
                  Request,
                  "{ Call Report.GetItemsInGroup(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>
               ((output) =>
                  {
                     DataSet ds = output as DataSet;
                     if (ds.Tables.Count == 0)
                        return;

                     var Tables = ds.Tables["Filter"].Rows.OfType<DataRow>().Select(t => new { TableID = t["TableUsageID"].ToString(), TFaName = t["TFaName"].ToString(), TEnName = t["TEnName"].ToString(), TOrderIndex = t["TOrderIndex"].ToString() }).Distinct();
                     Tables
                        .ToList()
                        .ForEach(t =>
                        {
                           TreeNode tr = new TreeNode(t.TFaName) { Tag = string.Format(@"<Table id=""{0}"" faName=""{1}"" enName=""{2}"" orderIndex=""{3}"">{{0}}</Table>", t.TableID, t.TFaName, t.TEnName, t.TOrderIndex), ImageIndex = 2 , SelectedImageIndex = 2 };
                           tv_right.Nodes.Add(tr);
                           ds.Tables["Filter"].Rows.OfType<DataRow>()
                              .Where(f => f["TableUsageID"].ToString() == t.TableID)
                              .ToList()
                              .ForEach(f =>
                              {
                                 Func<object, int> GetImageIndex = new Func<object, int>(
                                    (active) =>
                                    {
                                       return Convert.ToBoolean(active) ? 3 : 4;
                                    });
                                 TreeNode tf = new TreeNode(f["CFaName"].ToString()) { Tag = string.Format(@"<Field global=""{4}"" id=""{0}"" faName=""{1}"" enName=""{2}"" orderIndex=""{3}""/>", f["ColumnUsageID"], f["CFaName"], f["CEnName"], f["COrderIndex"], f["ID"]), ImageIndex = GetImageIndex(f["IsActive"]), SelectedImageIndex = GetImageIndex(f["IsActive"])};
                                 tr.Nodes.Add(tf);
                              });
                           tv_right.ExpandAll();
                        });
                  })
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void LoadXMLReportInLeftTree(Job job)
      {
         tv_left.Tag = job.Input;
         tv_left.Nodes.Clear();

         XDocument.Parse(job.Input.ToString())
            .Element("XmlReport")
            .Nodes()
            .ToList()
            .ForEach(t =>
            {
               XElement tab = (XElement)t;
               TreeNode tr = new TreeNode(tab.Attribute("name").Value) { Tag = string.Format(@"<Table faName=""{0}"" enName=""{0}"" enRealName=""{1}"" orderIndex="""">{{0}}</Table>",tab.Attribute("name").Value, tab.Attribute("enRealName").Value), ImageIndex = 0, SelectedImageIndex = 0 };
               tv_left.Nodes.Add(tr);
               tab.Nodes()
                  .ToList()
                  .ForEach(f =>
                  {
                     XElement field = (XElement)f;
                     TreeNode fr = new TreeNode(field.Attribute("name").Value) { Tag = string.Format(@"<Field faName=""{0}"" enName=""{0}"" type=""{1}"" formulaName=""{2}"" orderIndex=""""/>", field.Attribute("name").Value, field.Attribute("type").Value, field.Attribute("formulaName").Value), ImageIndex = 1, SelectedImageIndex = 1};                     
                     tr.Nodes.Add(fr);
                  });
            });
         tv_left.ExpandAll();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void PreSync(Job job)
      {         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void SetDescReport(Job job)
      {
         XDocument xReport = XDocument.Parse(job.Input.ToString());
         tb_reportdesc.Text = xReport.Element("Report").Element("LogicalName").Value;
         tb_reportdesc.Tag = tb_physical.Text = xReport.Element("Report").Element("PhysicalName").Value;
         job.Status = StatusType.Successful;
      }
   }
}
