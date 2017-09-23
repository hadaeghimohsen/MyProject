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
   partial class SpecifyProfilerGroupHeader : ISendRequest
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
               LoadActiveDataSource(job);
               break;
            case 10:
               LoadGroupHeaderOfRole(job);
               break;
            case 11:
               LoadGroupHeadersOfProfiler(job);
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
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 04 /* Execute UnPaint */);
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
         string xml = job.Input.ToString();
         tb_profiler.Text = XDocument.Parse(xml).Element("Profiler").Element("FaName").Value;
         tb_profiler.Tag = XDocument.Parse(xml).Element("Profiler").Attribute("ID").Value;
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportProfiler:SpecifyProfilerGroupHeader", this }  },
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
            new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 08 /* LoadRolesOfUser */);
         
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
         pn_command.Controls.OfType<Control>().Where(c => !(c.Enabled)).ToList().ForEach(c => c.Enabled = true);
         pn_edit.Visible = false;
         pn_orderindex.Visible = false;

         gb_ds.Enabled = true;
         lv_profilergroups.CheckBoxes = false;

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
                                    cb_roles_SelectedIndexChanged(null, null);
                                 })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 11 /* Execute LoadGroupHeadersOfProfiler */){Input = "Normal"},
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 10 /* Executive LoadGroupHeaderOfRole */){Input = "Normal"},
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 09 /* Executive LoadActiveDatasource */)
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
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
                     cb_datasource.DataSource = ds.Tables["Datasources"];
                     cb_datasource.DisplayMember = "TitleFa";
                     cb_datasource.ValueMember = "ID";
                  })
               #endregion
            });
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadGroupHeaderOfRole(Job job)
      {
         if (cb_roles.SelectedValue == null)
            return;
         try { if(cb_roles.SelectedValue is DataRowView) return; }
         catch { return; }

         string Request = string.Format(@"<Request type=""{0}""><Role>{1}</Role></Request>", job.Input, cb_roles.SelectedValue);
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
                  "{ Call Report.GetGroupHeadersOfRole(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                     {
                        DataSet ds = output as DataSet;
                        cbe_groupheaders.Properties.DataSource = ds.Tables["GroupHeaders"];
                        cbe_groupheaders.Properties.DisplayMember = "FaName";
                        cbe_groupheaders.Properties.ValueMember = "ID";
                     })
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadGroupHeadersOfProfiler(Job job)
      {
         string Request = string.Format(@"<Request type=""{0}"" limited=""{1}""><Role>{2}</Role><Profiler>{3}</Profiler></Request>", job.Input, cb_showallrole.Checked, cb_roles.SelectedValue, tb_profiler.Tag);
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
                  "{ Call Report.GetProfilerGroupHeader(?) }",
                  "iProject",
                  "scott"
               },
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     DataSet ds = output as DataSet;
                     lv_profilergroups.Items.Clear();
                     ds.Tables["RPG"].Rows.OfType<DataRow>()
                           .ToList()
                           .ForEach(c =>
                           {
                              lv_profilergroups.Items.Add(new ListViewItem(c["GFaName"].ToString()) 
                              { 
                                 Tag = string.Format(@"<Group id=""{0}""><Role>{1}</Role><Datasource from=""{2}"" type=""{3}"">{4}</Datasource><OrderIndex>{5}</OrderIndex></Group>",
                                                      c["Gid"],
                                                      c["Rid"],
                                                      c["DsFrom"],
                                                      c["DsType"],
                                                      c["Dsid"],
                                                      c["OrderIndex"]), 
                                 ImageIndex = Convert.ToBoolean(c["State"]) ? 4 : 5 });
                              lv_profilergroups.Items[lv_profilergroups.Items.Count - 1].SubItems.AddRange(new[] { c["RFaName"].ToString(), c["DbFaName"].ToString() });
                           });
                  })
            });
         job.Status = StatusType.Successful;
      }        
   }
}
