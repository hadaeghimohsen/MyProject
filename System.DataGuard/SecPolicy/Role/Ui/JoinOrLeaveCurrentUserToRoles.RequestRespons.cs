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

namespace System.DataGuard.SecPolicy.Role.Ui
{
   partial class JoinOrLeaveCurrentUserToRoles : ISendRequest
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
               LoadUserRoles(job);
               break;
            case 09:
               LoadReminderRoles(job);
               break;
            case 10:
               LoadCurrentUserRoleWithCondition(job);
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
                        Input = @".\Documents\DataGuard\SecPolicy\Role\CreateNewRole.html"
                     }
                  });
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "JLCU2R", 04 /* Execute UnPaint */);
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
         UserName = job.Input as string;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Role",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"DataGuard:SecurityPolicy:Role:JLCU2R", this}},
                  new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){Input = this},
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
         #region Get Current User
         Job _GetCurrentUser =
            new Job(SendType.External, "Role",
               new List<Job>
               {
                  new Job(SendType.External, "SecurityPolicy",
                     new List<Job>
                     {
                        new Job(SendType.External, "DataGuard",
                           new List<Job>
                           {
                              new Job(SendType.External, "Login",
                                 new List<Job>
                                 {
                                    new Job(SendType.SelfToUserInterface, "Login", 01 /* Execute Get */)
                                    {
                                       Input = "both",
                                       AfterChangedOutput = new Action<object>
                                       ((output) =>
                                       {
                                          txt_currentuser.Text = XElement.Parse(output.ToString()).Descendants("UserFaName").First().Value;
                                          txt_currentuser.Tag = XElement.Parse(output.ToString()).Elements("UserID").First().ToString();
                                       })
                                    }

                                 })
                           })
                     }),
                  new Job(SendType.SelfToUserInterface, "JLCU2R", 08 /* Execute LoadUserRoles */)
               });
         _DefaultGateway.Gateway(_GetCurrentUser);
         #endregion

         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadUserRoles(Job job)
      {
         #region Get User Roles
         Job _GetUserRoles =
            new Job(SendType.External, "JLCU2R",
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
                              txt_currentuser.Tag,
                              "{ Call DataGuard.GetUserRoles(?) }",
                              "iProject",
                              "scott"
                           },
                           AfterChangedOutput = new Action<object>(
                              (output) => 
                              {
                                 Func<bool, int> GetImageIndex = new Func<bool, int>(
                                    (isactive) =>
                                    {
                                       return isactive ? 2 : 5;
                                    }
                                 );

                                 DataSet ds = output as DataSet;
                                 lv_roles.Items.Clear();
                                 lv_roles.CheckBoxes = false;
                                 pn_edit.Visible = false;
                                 pn_command.Controls.OfType<Control>().ToList().ForEach(item => item.Enabled = true);

                                 ds.Tables["UserRoles"].Rows.Cast<DataRow>()
                                       //.Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                                       .ToList()
                                       .ForEach(rp =>
                                       {
                                          lv_roles.Items.Add(
                                             new ListViewItem(rp["TitleFa"].ToString())
                                             {
                                                ImageIndex = GetImageIndex((bool)rp["IsActive"]),
                                                Tag = string.Format("<RoleID>{0}</RoleID>",
                                                rp["RoleID"]),
                                             });
                                       });
                              })
                        }
                     })
               });
         _DefaultGateway.Gateway(_GetUserRoles);
         #endregion
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void LoadReminderRoles(Job job)
      {
         job.Next =
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
                        "{ Call Global.LoadReminderRoles(?) }",
                        "iProject",
                        "scott"
                     },
                     AfterChangedOutput = new Action<object>((output) =>
                     {
                        DataSet ds = output as DataSet;
                        Func<bool, int> GetImageIndex = new Func<bool, int>(
                                    (isactive) =>
                                    {
                                       return isactive ? 2 : 5;
                                    }
                                 );

                        lv_roles.Items.Clear();                                 

                        ds.Tables["Roles"].Rows.Cast<DataRow>()
                              //.Where(rp => rp["SchemaFaName"].Equals(schemaFaName))
                              .ToList()
                              .ForEach(rp =>
                              {
                                 lv_roles.Items.Add(
                                    new ListViewItem(rp["TitleFa"].ToString())
                                    {
                                       ImageIndex = GetImageIndex(Convert.ToBoolean(rp["IsActive"])),
                                       Tag = string.Format("<RoleID>{0}</RoleID>",
                                       rp["RoleID"]),
                                    });
                              });
                     })
                  }
               });

         job.Status = StatusType.Successful;
      }

      private void LoadCurrentUserRoleWithCondition(Job job)
      {
         job.Next =
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
                        "{ Call Global.LoadCurrentUserRoleWithCondition(?) }",
                        "iProject",
                        "scott"
                     },
                     AfterChangedOutput = new Action<object>((output) =>
                     {
                        DataSet ds = output as DataSet;
                        Func<bool, int> GetImageIndex = new Func<bool, int>(
                                    (isactive) =>
                                    {
                                       return isactive ? 2 : 5;
                                    }
                                 );

                        lv_roles.Items.Clear();                                 

                        ds.Tables["Roles"].Rows.Cast<DataRow>()
                              .ToList()
                              .ForEach(rp =>
                              {
                                 lv_roles.Items.Add(
                                    new ListViewItem(rp["TitleFa"].ToString())
                                    {
                                       ImageIndex = GetImageIndex(Convert.ToBoolean(rp["IsActive"])),
                                       Tag = string.Format("<RoleID>{0}</RoleID>",
                                       rp["RoleID"]),
                                    });
                              });
                     })
                  }
               });

         job.Status = StatusType.Successful;
      }

   }
}
