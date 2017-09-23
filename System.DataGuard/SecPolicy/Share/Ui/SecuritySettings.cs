using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SecuritySettings : UserControl
   {
      public SecuritySettings()
      {
         InitializeComponent();
      }
      private string Role;
          
      #region Role Settings
      private void AfterSelectdRole(object sender, TreeViewEventArgs e)
      {
         if(e.Node.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         Role = e.Node.Tag.ToString();

         Job _RetreiveData = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "SecuritySettings", 10 /* Execute LoadPrivileges */){Input = e.Node.Tag},
               new Job(SendType.SelfToUserInterface, "SecuritySettings", 11 /* Execute LoadUsers */){Input = e.Node.Tag}
            });
         _DefaultGateway.Gateway(_RetreiveData);
      }

      private void sb_createnewrole_Click(object sender, EventArgs e)
      {
         Job _CreateNewRole = new Job(SendType.External, "SecurityPolicy", "Role", 02 /* Execute DoWork4CreateNewRole */, SendType.Self);
         _DefaultGateway.Gateway(_CreateNewRole);
      }

      private void sb_removerole_Click(object sender, EventArgs e)
      {
         if (tv_roles.SelectedNode == null)
            return;

         if(tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         Job _RemoveRole = new Job(SendType.External, "SecurityPolicy", "Role", 03 /* Execute DoWork4RemoveRole */, SendType.Self)
            {
               Input = new List<object>
               {
                  new Action(() => {
                     Job _LoadData = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 07 /* Execute LoadData */, SendType.SelfToUserInterface);
                     _DefaultGateway.Gateway(_LoadData);
                  }),
                  "Commons.RemoveObject", /* Scope Action */
                  "DataGuard.RemoveRole",
                   string.Format("<Role>{0}<RoleName>{1}</RoleName></Role>",tv_roles.SelectedNode.Tag, tv_roles.SelectedNode.Text),
                  "iProject",
                  "scott",
                  "<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>2</SectionID><Explain>{0}</Expain>",
                  new List<string>{"<Privilege>4<Privilege>", "DataGuard"}
               }
            };
         _DefaultGateway.Gateway(_RemoveRole);
      }

      private void sb_rolechanging_Click(object sender, EventArgs e)
      {
         if( tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0<RoleID>")
            return;

         Job _RoleChanging = new Job(SendType.External, "SecurityPolicy", "Role", 04 /* Execute DoWork4RoleChaning */, SendType.Self) { Input = string.Format("<Role>{0}<RoleName>{1}</RoleName></Role>",tv_roles.SelectedNode.Tag, tv_roles.SelectedNode.Text) };
         _DefaultGateway.Gateway(_RoleChanging);
      }
      #endregion

      private string rootTag;
      private string globalFunctionCall;
      private string privilege;
      private string explain;

      #region Privilege Settings

      private void sb_addprivilege_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;
         
         rootTag = string.Format("<Grant>{0}{{0}}</Grant>", Role);
         globalFunctionCall = "Global.GrantPrivilegesToRole";
         privilege = "<Privilege>9</Privilege>";
         explain = "مجوز دسترسی به {0} به گروه دسترسی ( {1} ) داده شد";
         pn_roles.Enabled = false;
         sb_removeprivilege.Enabled = sb_deactiveprivilege.Enabled = sb_activeprivilege.Enabled = false;
         pn_editprivilege.Visible = true;
         lv_privileges.CheckBoxes = true;

         _DefaultGateway.Gateway(createReminderPrivilegesOfRole());
      }

      private void sb_removeprivilege_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Revoke>{0}{{0}}</Revoke>", Role);
         globalFunctionCall = "Global.RevokePrivilegesFromRole";
         privilege = "<Privilege>10</Privilege>";
         explain = "مجوز دسترسی به {0} از گروه دسترسی ( {1} ) گرفته شد";

         pn_roles.Enabled = false;
         sb_addprivilege.Enabled = sb_deactiveprivilege.Enabled = sb_activeprivilege.Enabled = false;
         pn_editprivilege.Visible = true;
         lv_privileges.CheckBoxes = true;
      }

      private void sb_deactiveprivilege_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Deactive>{0}{{0}}</Deactive>", Role);
         globalFunctionCall = "[Global].[DeactivePrivilegesToRole]";
         privilege = "<Privilege>11</Privilege>";
         explain = "مجوز استفاده ازدسترسی {0} از گروه دسترسی ( {1} ) به صورت موقت مسدود شد";

         pn_roles.Enabled = false;         
         sb_addprivilege.Enabled = sb_removeprivilege.Enabled = sb_activeprivilege.Enabled = false;
         pn_editprivilege.Visible = true;
         lv_privileges.CheckBoxes = true;

         _DefaultGateway.Gateway(createLoadActivePrivilegeOfRole());
      }

      private void sb_activeprivilege_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Active>{0}{{0}}</Active>", Role);
         globalFunctionCall = "[Global].[ActivePrivilegesInRole]";
         privilege = "<Privilege>12</Privilege>";
         explain = "مجوز استفاده ازدسترسی {0} به گروه دسترسی ( {1} ) برگردانده شد";

         pn_roles.Enabled = false;
         sb_addprivilege.Enabled = sb_removeprivilege.Enabled = sb_deactiveprivilege.Enabled = false;
         pn_editprivilege.Visible = true;
         lv_privileges.CheckBoxes = true;

         _DefaultGateway.Gateway(createLoadDeactivePrivilegesOfRole());
      }

      private void sb_submitprivilegechange_Click(object sender, EventArgs e)
      {
         pn_roles.Enabled = true;
         tp_users.Show();
         sb_addprivilege.Enabled = sb_removeprivilege.Enabled = sb_deactiveprivilege.Enabled = sb_activeprivilege.Enabled = true;
         pn_editprivilege.Visible = false;
         lv_privileges.CheckBoxes = false;

         _DefaultGateway.Gateway(createPrivilegeJobs());
      }

      private void sb_cancelprivilegechange_Click(object sender, EventArgs e)
      {
         pn_roles.Enabled = true;
         tp_users.Show();
         sb_addprivilege.Enabled = sb_removeprivilege.Enabled = sb_deactiveprivilege.Enabled = sb_activeprivilege.Enabled = true;
         pn_editprivilege.Visible = false;
         lv_privileges.CheckBoxes = false;

         _DefaultGateway.Gateway(createLoadPrivilegesOfRole());
      }

      private void sb_selectallprivileges_Click(object sender, EventArgs e)
      {
         lv_privileges.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = true);
      }

      private void sb_selectinvertprivileges_Click(object sender, EventArgs e)
      {
         lv_privileges.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = !p.Checked);
      }

      private void sb_deselectprivileges_Click(object sender, EventArgs e)
      {
         lv_privileges.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = false);
      }

      #region Jobs
      private Job createPrivilegeJobs()
      {
         Func<string> UpdateXmlData = 
            () =>
            {
               string selectPrivileges = "";
               lv_privileges.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectPrivileges += p.Tag);
               return string.Format(rootTag, selectPrivileges);
            };

         Func<string, string> UpdateXmlRedoLog =
            (textTemplate) =>
            {
               string selectPrivileges = "";
               lv_privileges.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectPrivileges += "( " + p.Text + " ), ");
               selectPrivileges = string.Format(explain, selectPrivileges, tv_roles.SelectedNode.Text);
               return string.Format(textTemplate, selectPrivileges);
            };

         Job _DoWork = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = new List<string>
                        {
                           privilege,
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;
                           #region Error Fire
                           Job _ShowError = new Job(SendType.External, "Role",
                              new List<Job>
                              {
                                 new Job(SendType.External, "Commons",
                                    new List<Job>
                                    {
                                       new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                       {
                                          Input = new List<object>
                                          {
                                             "Not Imp",
                                             new Action(() => 
                                             {
                                                _DefaultGateway.Gateway(createLoadPrivilegesOfRole());
                                             })
                                          }
                                       }
                                    })
                              });
                           _DefaultGateway.Gateway(_ShowError);
                           #endregion
                        })
                     },
                     #endregion
                     #region DoWork
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */){
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           false,
                           "xml",
                           UpdateXmlData(),
                           string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                           "iProject",
                           "scott"
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint",  UpdateXmlRedoLog(@"<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>7</SectionID><Explain>{0}</Explain>")}}
                     #endregion
                  }),
               new Job(SendType.SelfToUserInterface, "SecuritySettings", 10 /* Execute LoadPrivileges */){Input = Role}
            });
         return _DoWork;
      }

      private Job createReminderPrivilegesOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 12 /* Execute ReminderPrivilegesOfRole */, SendType.SelfToUserInterface) { Input = Role };
         return _DoWork;
      }

      private Job createLoadPrivilegesOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 10 /* Execute LoadPrivileges */, SendType.SelfToUserInterface) { Input = Role};
         return _DoWork;
      }

      private Job createLoadActivePrivilegeOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 13 /* Execute LoadPrivilegesOfRoleWithCondition */, SendType.SelfToUserInterface) { Input = Role + "<RPActive>true</RPActive>" };
         return _DoWork;
      }

      private Job createLoadDeactivePrivilegesOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 13 /* Execute LoadPrivilegesOfRoleWithCondition */, SendType.SelfToUserInterface) { Input = Role + "<RPActive>false</RPActive>" };
         return _DoWork;
      }
      #endregion

      #endregion

      private string User;
      #region Set Private Privilege For User
      private void sb_grantprivilegetouser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>" || User == null)
            return;

         rootTag = string.Format("<Grant>{0}{{0}}</Grant>", User);
         globalFunctionCall = "Global.GrantPrivilegesToUser";
         privilege = "<Privilege>13</Privilege>";
         explain = "مجوز دسترسی به {0} به کاربر ( {1} ) داده شد";

         pn_roles.Enabled = false;
         lv_users.Enabled = false;
         sb_revokeprivilegefromuser.Enabled = sb_deactiveprivilegefromuser.Enabled = sb_activeprivilegetouser.Enabled = false;
         pn_edituserprivilege.Visible = true;
         lv_userprivileges.CheckBoxes = true;

         _DefaultGateway.Gateway(createReminderPrivilegesOfUser());
      }

      private void sb_revokeprivilegefromuser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>" || User == null)
            return;

         rootTag = string.Format("<Revoke>{0}{{0}}</Revoke>", User);
         globalFunctionCall = "Global.RevokePrivilegesFromUser";
         privilege = "<Privilege>14</Privilege>";
         explain = "مجوز دسترسی به {0} از کاربر ( {1} ) گرفته شد";

         pn_roles.Enabled = false;
         lv_users.Enabled = false;
         sb_grantprivilegetouser.Enabled = sb_deactiveprivilegefromuser.Enabled = sb_activeprivilegetouser.Enabled = false;
         pn_edituserprivilege.Visible = true;
         lv_userprivileges.CheckBoxes = true;
      }

      private void sb_deactiveprivilegeformuser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>" || User == null)
            return;

         rootTag = string.Format("<Deactive>{0}{{0}}</Deactive>", User);
         globalFunctionCall = "[Global].[DeactivePrivilegesToUser]";
         privilege = "<Privilege>15</Privilege>";
         explain = "مجوز استفاده ازدسترسی {0} از کاربر ( {1} ) به صورت موقت مسدود شد";

         pn_roles.Enabled = false;
         lv_users.Enabled = false;
         sb_grantprivilegetouser.Enabled = sb_revokeprivilegefromuser.Enabled = sb_activeprivilegetouser.Enabled = false;
         pn_edituserprivilege.Visible = true;
         lv_userprivileges.CheckBoxes = true;

         _DefaultGateway.Gateway(createLoadActivePrivilegeOfUser());
      }

      private void sb_activeprivilegetouser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>" || User == null)
            return;

         rootTag = string.Format("<Active>{0}{{0}}</Active>", User);
         globalFunctionCall = "[Global].[ActivePrivilegesInUser]";
         privilege = "<Privilege>16</Privilege>";
         explain = "مجوز استفاده ازدسترسی {0} به کاربر ( {1} ) برگردانده شد";

         pn_roles.Enabled = false;
         lv_users.Enabled = false;
         sb_grantprivilegetouser.Enabled = sb_revokeprivilegefromuser.Enabled = sb_deactiveprivilegefromuser.Enabled = false;
         pn_edituserprivilege.Visible = true;
         lv_userprivileges.CheckBoxes = true;

         _DefaultGateway.Gateway(createLoadDeactivePrivilegesOfUser());
      }

      private void sb_submitchangeprivilegetouser_Click(object sender, EventArgs e)
      {
         pn_roles.Enabled = true;
         lv_users.Enabled = true;         
         sb_grantprivilegetouser.Enabled = sb_revokeprivilegefromuser.Enabled = sb_deactiveprivilegefromuser.Enabled = sb_activeprivilegetouser.Enabled = true;
         pn_edituserprivilege.Visible = false;
         lv_userprivileges.CheckBoxes = false;

         _DefaultGateway.Gateway(createUserPrivilegeJobs());
      }

      private void sb_cancelchangeprivilegetouser_Click(object sender, EventArgs e)
      {
         pn_roles.Enabled = true;
         lv_users.Enabled = true;
         sb_grantprivilegetouser.Enabled = sb_revokeprivilegefromuser.Enabled = sb_deactiveprivilegefromuser.Enabled = sb_activeprivilegetouser.Enabled = true;
         pn_edituserprivilege.Visible = false;
         lv_userprivileges.CheckBoxes = false;

         _DefaultGateway.Gateway(createLoadPrivilegesOfUser());
      }

      private void sb_selectallprivilegeforuser_Click(object sender, EventArgs e)
      {
         lv_userprivileges.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = true);
      }

      private void sb_invertselectprivielegeforuser_Click(object sender, EventArgs e)
      {
         lv_userprivileges.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = !p.Checked);
      }

      private void sb_deselectprivilegeforuser_Click(object sender, EventArgs e)
      {
         lv_userprivileges.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = false);
      }

      #region Jobs
      private Job createUserPrivilegeJobs()
      {
         Func<string> UpdateXmlData =
            () =>
            {
               string selectPrivileges = "";
               lv_userprivileges.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectPrivileges += p.Tag);
               return string.Format(rootTag, selectPrivileges);
            };

         Func<string, string> UpdateXmlRedoLog =
            (textTemplate) =>
            {
               string selectPrivileges = "";
               lv_userprivileges.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectPrivileges += "( " + p.Text + " ), ");
               selectPrivileges = string.Format(explain, selectPrivileges, lv_users.SelectedItems[0].Text);
               return string.Format(textTemplate, selectPrivileges);
            };

         Job _DoWork = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = new List<string>
                        {
                           privilege,
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;
                           #region Error Fire
                           Job _ShowError = new Job(SendType.External, "Role",
                              new List<Job>
                              {
                                 new Job(SendType.External, "Commons",
                                    new List<Job>
                                    {
                                       new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                       {
                                          Input = new List<object>
                                          {
                                             "Not Imp",
                                             new Action(() => 
                                             {
                                                _DefaultGateway.Gateway(createLoadPrivilegesOfRole());
                                             })
                                          }
                                       }
                                    })
                              });
                           _DefaultGateway.Gateway(_ShowError);
                           _DefaultGateway.Gateway(createLoadPrivilegesOfUser());
                           #endregion
                        })
                     },
                     #endregion
                     #region DoWork
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */){
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           false,
                           "xml",
                           UpdateXmlData(),
                           string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                           "iProject",
                           "scott"
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint",  UpdateXmlRedoLog(@"<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>7</SectionID><Explain>{0}</Explain>")}}
                     #endregion
                  }),
               new Job(SendType.SelfToUserInterface, "SecuritySettings", 16 /* Execute LoadPrivilegesOfUser */){Input = User}
            });
         return _DoWork;
      }

      private Job createReminderPrivilegesOfUser()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 14 /* Execute ReminderPrivilegesOfUser */, SendType.SelfToUserInterface) { Input = User };
         return _DoWork;
      }

      private Job createLoadPrivilegesOfUser()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 16 /* Execute LoadPrivilegesOfUser */, SendType.SelfToUserInterface) { Input = User };
         return _DoWork;
      }

      private Job createLoadActivePrivilegeOfUser()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 15 /* Execute LoadPrivilegesOfUserWithCondition */, SendType.SelfToUserInterface) { Input = User + "<UPActive>true</UPActive>" };
         return _DoWork;
      }

      private Job createLoadDeactivePrivilegesOfUser()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 15 /* Execute LoadPrivilegesOfUserWithCondition */, SendType.SelfToUserInterface) { Input = User + "<UPActive>false</UPActive>" };
         return _DoWork;
      }
      #endregion

      #endregion

      #region User Settings
      private void lv_users_SelectedIndexChanged(object sender, EventArgs e)
      {
         try
         {
            User = lv_users.SelectedItems[0].Tag.ToString();
            _DefaultGateway.Gateway(createLoadPrivilegesOfUser());
         }
         catch { }
      }

      #region Change User Profile
      private void sb_grantuser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Grant>{0}{{0}}</Grant>", Role);
         globalFunctionCall = "DataGuard.GrantUsersToRole";
         privilege = "<Privilege>17</Privilege>";
         explain = "مجوزهای دسترسی به گروه {0} به کاربر ( {1} ) داده شد";
         
         pn_roles.Enabled = false;
         sb_revokeuser.Enabled = sb_deactiveuser.Enabled = sb_activeuser.Enabled = sb_usersinglepropertymenu.Enabled = false;
         pn_01.Enabled = false;
         pn_edituser.Visible = true;
         lv_users.CheckBoxes = true;

         _DefaultGateway.Gateway(createReminderUsersOfRole());
      }

      private void sb_revokeuser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Revoke>{0}{{0}}</Revoke>", Role);
         globalFunctionCall = "DataGuard.RevokeUsersFromRole";
         privilege = "<Privilege>18</Privilege>";
         explain = "مجوز دسترسی به گروه {0} از کاربر ( {1} ) گرفته شد";

         pn_roles.Enabled = false;
         sb_grantuser.Enabled = sb_deactiveuser.Enabled = sb_activeuser.Enabled = sb_usersinglepropertymenu.Enabled = false;
         pn_01.Enabled = false;
         pn_edituser.Visible = true;
         lv_users.CheckBoxes = true;
      }

      private void sb_deactiveuser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Deactive>{0}{{0}}</Deactive>", Role);
         globalFunctionCall = "DataGuard.DeactiveUsersFromRole";
         privilege = "<Privilege>19</Privilege>";
         explain = "فعالیت کاربر {0} درگروه دسترسی ( {1} ) به حالت تعلیق درآمد";

         pn_roles.Enabled = false;
         sb_grantuser.Enabled = sb_revokeuser.Enabled = sb_activeuser.Enabled = sb_usersinglepropertymenu.Enabled = false;
         pn_01.Enabled = false;
         pn_edituser.Visible = true;
         lv_users.CheckBoxes = true;

         _DefaultGateway.Gateway(createLoadActiveUsersOfRole());
      }

      private void sb_activeuser_Click(object sender, EventArgs e)
      {
         if (pn_roles.Enabled == false || tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0</RoleID>")
            return;

         rootTag = string.Format("<Active>{0}{{0}}</Active>", Role);
         globalFunctionCall = "DataGuard.ActiveUsersFromRole";
         privilege = "<Privilege>20</Privilege>";
         explain = "فعالیت کاربر {0} درگروه دسترسی ( {1} ) به حالت فعال درآمد";

         pn_roles.Enabled = false;
         sb_grantuser.Enabled = sb_revokeuser.Enabled = sb_deactiveuser.Enabled = sb_usersinglepropertymenu.Enabled = false;
         pn_01.Enabled = false;
         pn_edituser.Visible = true;
         lv_users.CheckBoxes = true;

         _DefaultGateway.Gateway(createLoadDeactiveUsersOfRole());
      }

      private void sb_usersinglepropertymenu_Click(object sender, EventArgs e)
      {
         if (tv_roles.SelectedNode == null || tv_roles.SelectedNode.Tag.ToString() == "<RoleID>0<RoleID>")
            return;

         Job _UserSinglePropertyMenu = null;
         if(lv_users.SelectedItems.Count == 1) 
            _UserSinglePropertyMenu = 
               new Job(SendType.External, "SecurityPolicy", "User", 03 /* Execute DoWork4PropertySingleMenu */, SendType.Self) 
               { 
                  Input = string.Format("<Role>{0}<User>{1}<Name>{2}</Name></User></Role>", tv_roles.SelectedNode.Tag, lv_users.SelectedItems[0].Tag, lv_users.SelectedItems[0].Text) 
               };
         else
            _UserSinglePropertyMenu =
               new Job(SendType.External, "SecurityPolicy", "User", 06 /* Execute DoWork4PropertySingleMenu */, SendType.Self)
               {
                  Input = "CreateNewUser"
               };

         _DefaultGateway.Gateway(_UserSinglePropertyMenu);
      }
      #endregion

      #region Select Items
      private void sb_selectalluser_Click(object sender, EventArgs e)
      {
         lv_users.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = true);
      }

      private void sb_invertselectuser_Click(object sender, EventArgs e)
      {
         lv_users.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = !p.Checked);
      }

      private void sb_deselectuser_Click(object sender, EventArgs e)
      {
         lv_users.Items.Cast<ListViewItem>().ToList().ForEach(p => p.Checked = false);
      }
      #endregion

      #region Commit or Rollback Change
      private void sb_submitchangeuser_Click(object sender, EventArgs e)
      {
         pn_01.Enabled = true;
         pn_roles.Enabled = true;
         sb_grantuser.Enabled = sb_revokeuser.Enabled = sb_deactiveuser.Enabled = sb_activeuser.Enabled = sb_usersinglepropertymenu.Enabled = true;
         pn_edituser.Visible = false;

         _DefaultGateway.Gateway(createUsersRoleJobs());
         lv_users.CheckBoxes = false;
      }

      private void sb_cancelchangeuser_Click(object sender, EventArgs e)
      {
         pn_01.Enabled = true;
         pn_roles.Enabled = true;
         sb_grantuser.Enabled = sb_revokeuser.Enabled = sb_deactiveuser.Enabled = sb_activeuser.Enabled = sb_usersinglepropertymenu.Enabled = true;
         pn_edituser.Visible = false;
         lv_users.CheckBoxes = false;

         _DefaultGateway.Gateway(createLoadUsersOfRole());
      }
      #endregion

      #region Jobs
      private Job createUsersRoleJobs()
      {
         Func<string> UpdateXmlData =
            () =>
            {
               string selectUsers = "";
               lv_users.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectUsers += p.Tag);
               return string.Format(rootTag, selectUsers);
            };

         Func<string, string> UpdateXmlRedoLog =
            (textTemplate) =>
            {
               string selectUsers = "";
               lv_users.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectUsers += "( " + p.Text + " ), ");
               selectUsers = string.Format(explain, selectUsers, tv_roles.SelectedNode.Text);
               return string.Format(textTemplate, selectUsers);
            };

         Job _DoWork = new Job(SendType.External, "SecurityPolicy",
            new List<Job>
            {
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     #region Access Privilege
                     new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                     {
                        Input = new List<string>
                        {
                           privilege,
                           "DataGuard"
                        },
                        AfterChangedOutput = new Action<object>((output) => {
                           if ((bool)output)
                              return;
                           #region Error Fire
                           Job _ShowError = new Job(SendType.External, "Role",
                              new List<Job>
                              {
                                 new Job(SendType.External, "Commons",
                                    new List<Job>
                                    {
                                       new Job(SendType.Self, 02 /* Execute DoWork4ErrorHandling */)
                                       {
                                          Input = new List<object>
                                          {
                                             "Not Imp",
                                             new Action(() => 
                                             {
                                                _DefaultGateway.Gateway(createLoadPrivilegesOfRole());
                                             })
                                          }
                                       }
                                    })
                              });
                           _DefaultGateway.Gateway(_ShowError);
                           _DefaultGateway.Gateway(createLoadUsersOfRole());
                           #endregion
                        })
                     },
                     #endregion
                     #region DoWork
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */){
                        Input = new List<object>
                        {
                           false,
                           "procedure",
                           true,
                           false,
                           "xml",
                           UpdateXmlData(),
                           string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                           "iProject",
                           "scott"
                        }
                     },
                     #endregion
                     #region Save Log
                     new Job(SendType.Self, 06 /* Execute DoWork4RedoLog */){Input = new List<string>{"savepoint",  UpdateXmlRedoLog(@"<PrgGroupShortCut>1</PrgGroupShortCut><SectionID>7</SectionID><Explain>{0}</Explain>")}}
                     #endregion
                  }),
               new Job(SendType.SelfToUserInterface, "SecuritySettings", 11 /* Execute LoadUsers */){Input = Role}
            });
         return _DoWork;
      }

      private Job createReminderUsersOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 17 /* Execute ReminderUsersOfRole */, SendType.SelfToUserInterface) { Input = Role };
         return _DoWork;
      }

      private Job createLoadUsersOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 11 /* Execute LoadUsers */, SendType.SelfToUserInterface) { Input = Role };
         return _DoWork;
      }

      private Job createLoadActiveUsersOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 18 /* Execute LoadUsersOfRoleWithCondition */, SendType.SelfToUserInterface) { Input = Role + "<RUActive>true</RUActive>" };
         return _DoWork;
      }

      private Job createLoadDeactiveUsersOfRole()
      {
         Job _DoWork = new Job(SendType.External, "SecurityPolicy", "SecuritySettings", 18 /* Execute LoadUsersOfRoleWithCondition */, SendType.SelfToUserInterface) { Input = Role + "<RUActive>false</RUActive>" };
         return _DoWork;
      }
      #endregion

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
      #endregion
   }
}
