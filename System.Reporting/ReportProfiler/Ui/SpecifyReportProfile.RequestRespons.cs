using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.Ui
{
   partial class SpecifyReportProfile : ISendRequest
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
               LoadProfilesOfRole(job);
               break;
            case 10:
               LoadActiveDataSource(job);
               break;
            case 11:
               DoWork4SelectingProfiler(job);
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
                  new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 04 /* Execute UnPaint */);
            else
               sb_cancel1_Click(null, null);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Reporting:ReportProfiler:SpecifyReportProfile", this }  },
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
         sb_chooseprofiler.Visible = false;
         job.Next =
            new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 08 /* LoadRolesOfUser */);
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
         pn_newitem.Enabled = true;
         lv_profilers.CheckBoxes = false;

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

                                    DataSet dsdown = ds.Copy();
                                    cbe_roles.Properties.DataSource = dsdown.Tables["Roles"];
                                    cbe_roles.Properties.DisplayMember = "RoleFaName";
                                    cbe_roles.Properties.ValueMember = "RoleID";
                                 })
                        }
                     }),
                  #endregion
                  //new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 09 /* Executive LoadProfilesOfRole */){Input = "Normal"},
                  new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 10 /* Executive LoadActiveDatasource */)
               });
         _DefaultGateway.Gateway(_LoadRoleOfUser);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadProfilesOfRole(Job job)
      {
         if (cb_roles.SelectedValue == null)
            return;
         try { if (cb_roles.SelectedValue is DataRowView) return; }
         catch(Exception) { return; }
         /* Input Role And type */
         /* Type = Normal, Deleted, Activted, Deactived, SingleDetail */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               #region Input
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  string.Format(@"<Request type=""{0}""><Role>{1}</Role><Profiler>{2}</Profiler></Request>", job.Input,cb_roles.SelectedValue, pn_newitem.Tag),
                  "{ Call Report.GetProfilesofRole(?) }",
                  "iProject",
                  "scott"
               },
               #endregion
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     DataSet ds = output as DataSet;
                     #region Part 01
                     if (ds.Tables.Contains("Profilers"))
                     {
                        lv_profilers.Items.Clear();
                        ds.Tables["Profilers"].Rows.OfType<DataRow>()
                           .ToList()
                           .ForEach(c =>
                           {
                              lv_profilers.Items.Add(new ListViewItem(c["FaName"].ToString()) { Tag = c["ID"], ImageIndex = Convert.ToBoolean(c["State"]) ? 4 : 5 });
                              lv_profilers.Items[lv_profilers.Items.Count - 1].SubItems.Add(c["DbFaName"].ToString());
                           });
                     }
                     #endregion
                     #region Part 02
                     else
                     {
                        Action<Control, object> assignValue = new Action<Control, object>(
                           (c, value) =>
                           {
                              if (c is TextEdit)
                                 (c as TextEdit).EditValue = value;
                              else if (c is System.Windows.Forms.ComboBox)
                                 (c as System.Windows.Forms.ComboBox).SelectedValue = value;
                           });
                        pn_newitem.Controls.OfType<Control>()
                           .Where(c => c.Tag != null && c.Tag.ToString().StartsWith("Profiler_")).ToList().ForEach(c => assignValue(c, ds.Tables["Profiler"].Rows[0][c.Tag.ToString().Split('_')[1]]));
                        cbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(c => c.CheckState = CheckState.Unchecked);
                        /* Checked Role Item In Combo Box If Value In Table Roles */
                        cbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => ds.Tables["Roles"].Select(string.Format("RoleID = {0}", c.Value)).Count() > 0).ToList().ForEach(c => c.CheckState = CheckState.Checked); ;
                     }
                     #endregion
                  })
            });
      }

      /// <summary>
      /// Code 10
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

      bool allRoles = false;
      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SelectingProfiler(Job job)
      {
         sb_chooseprofiler.Visible = true;
         allRoles = Convert.ToBoolean(XDocument.Parse(job.Input.ToString()).Element("Profiler").Element("Role").Attribute("all").Value);
         job.Status = StatusType.Successful;
      }
   }
}
