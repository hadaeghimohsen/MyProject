using System;
using System.Collections.Generic;
using System.Linq;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Data;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SecuritySettings : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }

      public void SendRequest(JobRouting.Jobs.Job job)
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
            case 10:
               LoadPrivileges(job);
               break;
            case 11:
               LoadUsers(job);
               break;
            case 12:
               LoadReminderPrivilegesOfRole(job);
               break;
            case 13:
               LoadPrivilegesOfRoleWithCondition(job);
               break;
            case 14:
               LoadReminderPrivilegesOfUser(job);
               break;
            case 15:
               LoadPrivilegesOfUserWithCondition(job);
               break;
            case 16:
               LoadPrivilegesOfUser(job);
               break;
            case 17:
               LoadReminderUsersOfRole(job);
               break;
            case 18:
               LoadUsersOfRoleWithCondition(job);
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
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Documents\DataGuard\SecPolicy\Share\SecuritySettings.html"
                     }
                  });
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "SecuritySettings", 04 /* Execute UnPaint */);
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
         Job _Paint = new Job(SendType.External, "DataGuard",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"DataGuard:SecurityPolicy:SecuritySettings", this}},
                  new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){ Input = this }
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
         Job _RetreiveRoles = new Job(SendType.External, "SecurityPolicy", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  false,
                  true,
                  "",
                  "",
                  "{ Call DataGuard.LoadRoles }",
                  "iProject",
                  "scott"
               },
               Executive = ExecutiveType.Asynchronous,
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     DataSet ds = output as DataSet;
                     Invoke(new Action(() =>
                     {
                        Func<object, int> GetImageIndex = (ra) =>
                           {
                              bool value = (bool)ra;
                              return value ? 1 : 4;
                           };
                           
                        tv_roles.Nodes.Clear();
                        tv_roles.Nodes.Add(new TreeNode("گروه های دسترسی") { Tag = "<RoleID>0</RoleID>" });
                        ds.Tables["Roles"].Rows.Cast<DataRow>().ToList().ForEach(r => tv_roles.Nodes[0].Nodes.Add(new TreeNode(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]), SelectedImageIndex = GetImageIndex(r["IsActive"]),  Tag = string.Format("<RoleID>{0}</RoleID>", r["ID"]) }));
                        tv_roles.ExpandAll();
                     }));
                  })
            };
         _DefaultGateway.Gateway(_RetreiveRoles);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void LoadPrivileges(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Load Privileges
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return privileges in roles */
                     {                        
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call Global.LoadPrivilegesRole(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = (bool)ra;
                                 return value ? 2 : 5;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                                 {
                                    lv_privileges.Items.Clear();
                                    lv_privileges.Groups.Clear();

                                    var _GetSchemaEnName = ds.Tables["Privileges"].Rows.Cast<DataRow>().Select(p => p["SchemaFaName"].ToString()).Distinct();

                                    _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                       {
                                          var group = new ListViewGroup(schemaFaName);
                                          lv_privileges.Groups.Add(group);

                                          ds.Tables["Privileges"].Rows.Cast<DataRow>()
                                             .Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                             .ToList()
                                             .ForEach(rp => 
                                             {
                                                lv_privileges.Items.Add(
                                                   new ListViewItem(rp["TitleFa"].ToString()) 
                                                   { 
                                                      ImageIndex = GetImageIndex(rp["IsActive"]),  
                                                      Tag = string.Format("<PrivilegeID>{0}</PrivilegeID><SchemaEnName>{1}</SchemaEnName><Sub_Sys>{2}</Sub_Sys>", 
                                                      rp["PrivilegeID"], rp["SchemaEnName"], rp["Sub_Sys"]),
                                                      Group = group
                                                   });
                                             });
                                       });
                                    //ds.Tables["Privileges"].Rows.Cast<DataRow>().ToList().ForEach(r => lv_privileges.Items.Add(new ListViewItem(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]),  Tag = string.Format("<PrivilegeID>{0}</PrivilegeID>", r["PrivilegeID"]) }));
                                 }));
                           }
                        )
                     },
                     #endregion                     
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void LoadUsers(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {                     
                     #region Load Users
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return user join in roles */
                     {
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call DataGuard.LoadUsersRole(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = (bool)ra;
                                 return value ? 3 : 4;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                              {
                                 lv_users.CheckBoxes = false;
                                 lv_users.Items.Clear();
                                 lv_userprivileges.Items.Clear();
                                 ds.Tables["Users"].Rows.Cast<DataRow>().ToList().ForEach(r => lv_users.Items.Add(new ListViewItem(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]), Tag = string.Format("<UserID>{0}</UserID>", r["UserID"]) }));
                              }));
                           }
                        )
                     }
                     #endregion
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void LoadReminderPrivilegesOfRole(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Load Privileges
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return privileges in roles */
                     {                        
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call [Global].[LoadReminderPrivilegesOfRole](?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = Convert.ToBoolean(ra);
                                 return value ? 2 : 5;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                                 {
                                    lv_privileges.Items.Clear();
                                    lv_privileges.Groups.Clear();

                                    var _GetSchemaEnName = ds.Tables["Privileges"].Rows.Cast<DataRow>().Select(p => p["SchemaFaName"].ToString()).Distinct();

                                    _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                       {
                                          var group = new ListViewGroup(schemaFaName);
                                          lv_privileges.Groups.Add(group);

                                          ds.Tables["Privileges"].Rows.Cast<DataRow>()
                                             .Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                             .ToList()
                                             .ForEach(rp => 
                                             {
                                                lv_privileges.Items.Add(
                                                   new ListViewItem(rp["TitleFa"].ToString()) 
                                                   { 
                                                      ImageIndex = GetImageIndex(rp["IsActive"]),  
                                                      Tag = string.Format("<Privilege><PrivilegeID>{0}</PrivilegeID><SchemaEnName>{1}</SchemaEnName></Privilege>", rp["PrivilegeID"], rp["SchemaEnName"]),
                                                      Group = group
                                                   });
                                             });
                                       });
                                    //ds.Tables["Privileges"].Rows.Cast<DataRow>().ToList().ForEach(r => lv_privileges.Items.Add(new ListViewItem(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]),  Tag = string.Format("<PrivilegeID>{0}</PrivilegeID>", r["PrivilegeID"]) }));
                                 }));
                           }
                        )
                     },
                     #endregion                     
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void LoadPrivilegesOfRoleWithCondition(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Load Privileges
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return privileges in roles */
                     {                        
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call Global.LoadPrivilegesOfRoleWithCondition(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = (bool)ra;
                                 return value ? 2 : 5;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                                 {
                                    lv_privileges.Items.Clear();
                                    lv_privileges.Groups.Clear();

                                    var _GetSchemaEnName = ds.Tables["Privileges"].Rows.Cast<DataRow>().Select(p => p["SchemaFaName"].ToString()).Distinct();

                                    _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                       {
                                          var group = new ListViewGroup(schemaFaName);
                                          lv_privileges.Groups.Add(group);

                                          ds.Tables["Privileges"].Rows.Cast<DataRow>()
                                             .Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                             .ToList()
                                             .ForEach(rp => 
                                             {
                                                lv_privileges.Items.Add(
                                                   new ListViewItem(rp["TitleFa"].ToString()) 
                                                   { 
                                                      ImageIndex = GetImageIndex(rp["IsActive"]),  
                                                      Tag = string.Format("<Privilege><PrivilegeID>{0}</PrivilegeID><SchemaEnName>{1}</SchemaEnName></Privilege>", 
                                                      rp["PrivilegeID"], rp["SchemaEnName"]),
                                                      Group = group
                                                   });
                                             });
                                       });
                                    //ds.Tables["Privileges"].Rows.Cast<DataRow>().ToList().ForEach(r => lv_privileges.Items.Add(new ListViewItem(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]),  Tag = string.Format("<PrivilegeID>{0}</PrivilegeID>", r["PrivilegeID"]) }));
                                 }));
                           }
                        )
                     },
                     #endregion                     
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void LoadReminderPrivilegesOfUser(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Load Privileges
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return privileges in roles */
                     {                        
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call [Global].[LoadReminderPrivilegesOfUser](?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = Convert.ToBoolean(ra);
                                 return value ? 2 : 5;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                                 {
                                    lv_userprivileges.Items.Clear();
                                    lv_userprivileges.Groups.Clear();

                                    var _GetSchemaEnName = ds.Tables["Privileges"].Rows.Cast<DataRow>().Select(p => p["SchemaFaName"].ToString()).Distinct();

                                    _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                       {
                                          var group = new ListViewGroup(schemaFaName);
                                          lv_userprivileges.Groups.Add(group);

                                          ds.Tables["Privileges"].Rows.Cast<DataRow>()
                                             .Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                             .ToList()
                                             .ForEach(rp => 
                                             {
                                                lv_userprivileges.Items.Add(
                                                   new ListViewItem(rp["TitleFa"].ToString()) 
                                                   { 
                                                      ImageIndex = GetImageIndex(rp["IsActive"]),  
                                                      Tag = string.Format("<Privilege><PrivilegeID>{0}</PrivilegeID><SchemaEnName>{1}</SchemaEnName></Privilege>", rp["PrivilegeID"], rp["SchemaEnName"]),
                                                      Group = group
                                                   });
                                             });
                                       });                                    
                                 }));
                           }
                        )
                     },
                     #endregion                     
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
      }

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void LoadPrivilegesOfUserWithCondition(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Load Privileges
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return privileges in roles */
                     {                        
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call Global.LoadPrivilegesOfUserWithCondition(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = (bool)ra;
                                 return value ? 2 : 5;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                                 {
                                    lv_userprivileges.Items.Clear();
                                    lv_userprivileges.Groups.Clear();

                                    var _GetSchemaEnName = ds.Tables["Privileges"].Rows.Cast<DataRow>().Select(p => p["SchemaFaName"].ToString()).Distinct();

                                    _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                       {
                                          var group = new ListViewGroup(schemaFaName);
                                          lv_userprivileges.Groups.Add(group);

                                          ds.Tables["Privileges"].Rows.Cast<DataRow>()
                                             .Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                             .ToList()
                                             .ForEach(rp => 
                                             {
                                                lv_userprivileges.Items.Add(
                                                   new ListViewItem(rp["TitleFa"].ToString()) 
                                                   { 
                                                      ImageIndex = GetImageIndex(rp["IsActive"]),  
                                                      Tag = string.Format("<Privilege><PrivilegeID>{0}</PrivilegeID><SchemaEnName>{1}</SchemaEnName></Privilege>", 
                                                      rp["PrivilegeID"], rp["SchemaEnName"]),
                                                      Group = group
                                                   });
                                             });
                                       });
                                 }));
                           }
                        )
                     },
                     #endregion                     
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void LoadPrivilegesOfUser(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Load Privileges
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return privileges in roles */
                     {                        
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call Global.LoadPrivilegesUser(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = (bool)ra;
                                 return value ? 2 : 5;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                                 {
                                    lv_userprivileges.Items.Clear();
                                    lv_userprivileges.Groups.Clear();

                                    var _GetSchemaEnName = ds.Tables["Privileges"].Rows.Cast<DataRow>().Select(p => p["SchemaFaName"].ToString()).Distinct();

                                    _GetSchemaEnName.ToList().ForEach(schemaFaName =>
                                       {
                                          var group = new ListViewGroup(schemaFaName);
                                          lv_userprivileges.Groups.Add(group);

                                          ds.Tables["Privileges"].Rows.Cast<DataRow>()
                                             .Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                             .ToList()
                                             .ForEach(rp => 
                                             {
                                                lv_userprivileges.Items.Add(
                                                   new ListViewItem(rp["TitleFa"].ToString()) 
                                                   { 
                                                      ImageIndex = GetImageIndex(rp["IsActive"]),  
                                                      Tag = string.Format("<Privilege><PrivilegeID>{0}</PrivilegeID><SchemaEnName>{1}</SchemaEnName></Privilege>", 
                                                      rp["PrivilegeID"], rp["SchemaEnName"]),
                                                      Group = group
                                                   });
                                             });
                                       });
                                 }));
                           }
                        )
                     },
                     #endregion
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }
      
      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void LoadReminderUsersOfRole(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {                     
                     #region Load Users
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/) /* return user join in roles */
                     {
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call DataGuard.LoadReminderUsersOfRole(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = Convert.ToBoolean(ra);
                                 return value ? 3 : 4;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                              {
                                 lv_users.Items.Clear();
                                 lv_userprivileges.Items.Clear();
                                 ds.Tables["Users"].Rows.Cast<DataRow>()
                                    .ToList()
                                    .ForEach(r => lv_users.Items.Add(new ListViewItem(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]), Tag = string.Format("<UserID>{0}</UserID>", r["UserID"]) }));
                              }));
                           }
                        )
                     }
                     #endregion
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 18
      /// </summary>
      /// <param name="job"></param>
      private void LoadUsersOfRoleWithCondition(Job job)
      {
         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {                     
                     #region Load Users
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */) /* return user join in roles */
                     {
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "{ Call DataGuard.LoadUsersOfRoleWithCondition(?) }",
                           "iProject",
                           "scott"
                        },
                        AfterChangedOutput = new Action<object>
                        ((output) =>
                           {
                              Func<object, int> GetImageIndex = (ra) =>
                              {
                                 bool value = (bool)ra;
                                 return value ? 3 : 4;
                              };

                              DataSet ds = output as DataSet;
                              Invoke(new Action(() =>
                              {
                                 lv_users.Items.Clear();
                                 lv_userprivileges.Items.Clear();
                                 ds.Tables["Users"].Rows.Cast<DataRow>().ToList().ForEach(r => lv_users.Items.Add(new ListViewItem(r["TitleFa"].ToString()) { ImageIndex = GetImageIndex(r["IsActive"]), Tag = string.Format("<UserID>{0}</UserID>", r["UserID"]) }));
                              }));
                           }
                        )
                     }
                     #endregion
                  })
            });
         _DefaultGateway.Gateway(_RetreiveData);
         job.Status = StatusType.Successful;
      }
   }
}
