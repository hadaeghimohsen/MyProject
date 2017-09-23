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

namespace System.ServiceDefinition.GrpHdr.Ui
{
   partial class GroupHeaderMenus : ISendRequest
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
               LoadGroupHeadersWithCondition(job);
               break;
            case 11:
               LoadGroupHeadersForJoinToRoles(job);
               break;
            case 12:
               LoadJoinGroupHeaderWithCondition(job);
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
               new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 04 /* Execute UnPaint */);
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
         Job _Paint = new Job(SendType.External, "GroupHeader",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"ServiceDefinition:GroupHeader:GroupHeaderMenus", this}},
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
            new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 08 /* LoadRolesOfUser */);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadRolesOfUser(Job job)
      {
         Job _LoadRoleOfUser =
            new Job(SendType.External, "GroupHeader",
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
                                    ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(role => role.CheckState = CheckState.Checked);

                                    lv_groupheaders.Items.Clear();
                                    lv_groupheaders.CheckBoxes = false;
                                    pn_edit.Visible = false;                                    
                                    pn_command.Controls.OfType<Control>().ToList().ForEach(item => item.Enabled = true);                                    
                                 })
                        }
                     }),
                  #endregion
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 09 /* Execute LoadGroupHeadersOfRoles */)
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
         lv_groupheaders.Items.Clear();
         lv_groupheaders.CheckBoxes = false;
         pn_edit.Visible = false;
         pn_command.Controls.OfType<Control>().ToList().ForEach(item => item.Enabled = true);   

         if (ccbe_roles.Properties.GetItems().Count == 0)
         {
            job.Status = StatusType.Failed;
            return;
         }

         Func<string> XmlData = new Func<string>(
            () =>
            {
               string XmlRoleID = "";
               ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked ).ToList().ForEach(role => XmlRoleID += string.Format("<RoleID>{0}</RoleID>",role.Value));
               return string.Format("<GH>{0}</GH>",XmlRoleID);
            });
         

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region DoWork4Odbc
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              XmlData(),
                              "{ Call ServiceDef.GetGroupHeadersOfRoles(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 #region After Changed Output
                                 DataSet ds = output as DataSet;
                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };
                                 lv_groupheaders.Items.Clear();
                                 lv_groupheaders.Groups.Clear();

                                 var _GetSchemaEnName = ds.Tables["GroupHeader"].Rows.Cast<DataRow>().Select(p => p["RoleFaName"].ToString()).Distinct();

                                 _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                    {
                                       var group = new ListViewGroup(schemaFaName);
                                       lv_groupheaders.Groups.Add(group);

                                       ds.Tables["GroupHeader"].Rows.Cast<DataRow>()
                                          .Where(gh => gh["RoleFaName"].Equals(schemaFaName))
                                          .ToList()
                                          .ForEach(gh => 
                                          {
                                             ListViewItem item = new ListViewItem(gh["GhFaName"].ToString())
                                                {
                                                   ImageIndex = GetImageIndex(gh["RGActive"]),
                                                   Tag = string.Format("<GroupHeader><GroupHeaderID>{0}</GroupHeaderID><RoleID>{1}</RoleID><GhFaName>{2}</GhFaName><GhEnName>{3}</GhEnName><GhActive>{4}</GhActive></GroupHeader>",
                                                   gh["GHID"], gh["RoleID"], gh["GhFaName"], gh["GhEnName"], gh["GhActive"]),
                                                   Group = group
                                                };                                             
                                             lv_groupheaders.Items.Add(item);
                                          });
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
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadGroupHeadersWithCondition(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region DoWork4Odbc
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
                              "{ Call ServiceDef.LoadGroupHeadersWithCondition(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 #region Add GroupHeaders in List
                                 DataSet ds = output as DataSet;
                                 
                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };
                                 
                                 lv_groupheaders.Items.Clear();
                                 lv_groupheaders.Groups.Clear();

                                 ds.Tables["GroupHeader"].Rows.Cast<DataRow>()
                                    .ToList()
                                    .ForEach(gh =>
                                    {
                                       ListViewItem item = new ListViewItem(gh["TitleFa"].ToString())
                                       {
                                          ImageIndex = GetImageIndex(gh["IsActive"]),
                                          Tag = string.Format("<ID>{0}</ID>", gh["ID"])
                                       };
                                       lv_groupheaders.Items.Add(item);
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
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadGroupHeadersForJoinToRoles(Job job)
      {
         Func<string> XmlData = new Func<string>(
            () =>
            {
               string XmlRoleID = "";
               ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked).ToList().ForEach(role => XmlRoleID += string.Format("<RoleID>{0}</RoleID>", role.Value));
               return string.Format("<Join>{0}</Join>", XmlRoleID);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region DoWork4Odbc
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              XmlData(),
                              "{ Call ServiceDef.LoadGroupHeadersForJoinToRoles(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 #region GroupHeader In List
                                 DataSet ds = output as DataSet;

                                 lv_groupheaders.Items.Clear();
                                 lv_groupheaders.Groups.Clear();

                                 var _GetSchemaEnName = ds.Tables["GroupHeader"].Rows.Cast<DataRow>().Select(p => p["RoleFaName"].ToString()).Distinct();

                                 _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                    {
                                       var group = new ListViewGroup(schemaFaName);
                                       lv_groupheaders.Groups.Add(group);

                                       ds.Tables["GroupHeader"].Rows.Cast<DataRow>()
                                          .Where(gh => gh["RoleFaName"].Equals(schemaFaName))
                                          .ToList()
                                          .ForEach(gh => 
                                          {
                                             ListViewItem item = new ListViewItem(gh["GhFaName"].ToString())
                                                {
                                                   ImageIndex = 2,
                                                   Tag = string.Format("<GroupHeader><GroupHeaderID>{0}</GroupHeaderID><RoleID>{1}</RoleID></GroupHeader>",
                                                   gh["GhID"], gh["RoleID"]),
                                                   Group = group
                                                };                                             
                                             lv_groupheaders.Items.Add(item);
                                          });
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
      private void LoadJoinGroupHeaderWithCondition(Job job)
      {
         Func<string, string> XmlData = new Func<string, string>(
            (whatactive) =>
            {
               string xmldata = "";
               ccbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(role => role.CheckState == CheckState.Checked ).ToList().ForEach(role => xmldata += string.Format("<RoleID>{0}</RoleID>", role.Value));
               return string.Format("<JGH>{0}<Roles>{1}</Roles></JGH>", whatactive, xmldata);
            });
         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region DoWork4Odbc
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              true,
                              "xml",
                              XmlData(job.Input as string),
                              "{ Call ServiceDef.LoadJoinGroupHeadersWithCondition(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 #region Add GroupHeaders in List
                                 DataSet ds = output as DataSet;
                                 
                                 Func<object, int> GetImageIndex = (ra) =>
                                 {
                                    bool value = (bool)ra;
                                    return value ? 2 : 5;
                                 };
                                 
                                 lv_groupheaders.Items.Clear();
                                 lv_groupheaders.Groups.Clear();

                                 var _GetSchemaEnName = ds.Tables["GroupHeader"].Rows.Cast<DataRow>().Select(p => p["RoleFaName"].ToString()).Distinct();

                                 _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                    {
                                       var group = new ListViewGroup(schemaFaName);
                                       lv_groupheaders.Groups.Add(group);

                                       ds.Tables["GroupHeader"].Rows.Cast<DataRow>()
                                          .Where(gh => gh["RoleFaName"].Equals(schemaFaName))
                                          .ToList()
                                          .ForEach(gh => 
                                          {
                                             ListViewItem item = new ListViewItem(gh["GhFaName"].ToString())
                                                {
                                                   ImageIndex = GetImageIndex(gh["RgActive"]),
                                                   Tag = string.Format("<GroupHeader><GroupHeaderID>{0}</GroupHeaderID><RoleID>{1}</RoleID></GroupHeader>", gh["GhID"], gh["RoleID"]),
                                                   Group = group
                                                };                                             
                                             lv_groupheaders.Items.Add(item);
                                          });
                                    });                                 
                                 #endregion
                              })
                        }
                        #endregion
                     })
               }));

         job.Status = StatusType.Successful;
      }
   }
}
