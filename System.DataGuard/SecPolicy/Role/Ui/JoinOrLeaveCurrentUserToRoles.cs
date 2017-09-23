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

namespace System.DataGuard.SecPolicy.Role.Ui
{
   public partial class JoinOrLeaveCurrentUserToRoles : UserControl
   {
      public JoinOrLeaveCurrentUserToRoles()
      {
         InitializeComponent();
      }

      private string UserName;
      private string rootTag;
      private string globalFunctionCall;
      private string privilege;


      private void sb_join_Click(object sender, EventArgs e)
      {
         rootTag = string.Format("<Join>{0}<Roles>{{0}}</Roles></Join>", txt_currentuser.Tag);
         globalFunctionCall = "Global.JoinUserToRoles";
         privilege = "<Privilege>24</Privilege>";
         lv_roles.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Role",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "JLCU2R", 09 /* Execute LoadReminderRoles */){Input = txt_currentuser.Tag}
               }));
      }
      private void sb_leave_Click(object sender, EventArgs e)
      {
         rootTag = string.Format("<Leave>{0}<Roles>{{0}}</Roles></Leave>", txt_currentuser.Tag);
         globalFunctionCall = "Global.LeaveUserFromRoles";
         privilege = "<Privilege>25</Privilege>";
         lv_roles.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
      }

      private void sb_deactive_Click(object sender, EventArgs e)
      {
         rootTag = string.Format("<Deactive>{0}<Roles>{{0}}</Roles></Deactive>", txt_currentuser.Tag);
         globalFunctionCall = "Global.DeactiveCurrentUserToRoles";
         privilege = "<Privilege>26</Privilege>";
         lv_roles.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Role",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "JLCU2R", 10 /* Execute GetUserRoleWithCondition */){Input = string.Format("{0}<RUActive>true</RUActive>", txt_currentuser.Tag)}
               }));
      }
      private void sb_active_Click(object sender, EventArgs e)
      {
         rootTag = string.Format("<Active>{0}<Roles>{{0}}</Roles></Active>", txt_currentuser.Tag);
         globalFunctionCall = "Global.ActiveCurrentUserToRoles";
         privilege = "<Privilege>27</Privilege>";
         lv_roles.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Role",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "JLCU2R", 10 /* Execute GetUserRoleWithCondition */){Input = string.Format("{0}<RUActive>false</RUActive>", txt_currentuser.Tag)}
               }));
      }

      #region Edit Menu
      private void sb_cancel_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Role",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "JLCU2R", 08 /* Execute LoadUserRoles */){Input = txt_currentuser.Tag}
               }));
      }

      private void sb_submit_Click(object sender, EventArgs e)
      {
         Func<string> XmlData = new Func<string>(
            () =>
            {
               string selectRoles = "";
               lv_roles.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectRoles += p.Tag);
               return string.Format(rootTag, selectRoles);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Role",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
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
                                                   _DefaultGateway.Gateway(new Job(SendType.External, "Role", "JLCU2R", 08 /* Execute LoadUserRoles */, SendType.SelfToUserInterface));
                                                })
                                             }
                                          }
                                       })
                                 });
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion
                           })
                        },
                        #region DoWork
                        new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                        {
                           Input = new List<object>
                           {
                              false,
                              "procedure",
                              true,
                              false,
                              "xml",
                              XmlData(),
                              string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                              "iProject",
                              "scott"
                           }
                        }
                        #endregion
                        
                     }),
                  new Job(SendType.SelfToUserInterface, "JLCU2R", 08 /* Execute LoadUserRoles */){Input = txt_currentuser.Tag}
               }));
      }

      private void sb_deselectall_Click(object sender, EventArgs e)
      {
         lv_roles.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = false);
      }

      private void sb_invertselect_Click(object sender, EventArgs e)
      {
         lv_roles.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = !item.Checked);
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         lv_roles.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
      }
      #endregion



      private void sb_advanced_Click(object sender, EventArgs e)
      {
         Job _ShowSecuritySettings = new Job(SendType.External, "Role",
            new List<Job>
            {
               #region Access Privilege
               new Job(SendType.External, "Commons",
                     new List<Job>
                     {                        
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>2</Privilege>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },                        
                     }),
               #endregion
               new Job(SendType.External,"SecurityPolicy", "", 02 /* Execute DoWork4SecuritySettings */, SendType.Self)
            });
         _DefaultGateway.Gateway(_ShowSecuritySettings);
      }

      
   }
}
