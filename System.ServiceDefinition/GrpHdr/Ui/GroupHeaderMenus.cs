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
using System.Xml.Linq;

namespace System.ServiceDefinition.GrpHdr.Ui
{
   public partial class GroupHeaderMenus : UserControl
   {
      public GroupHeaderMenus()
      {
         InitializeComponent();
      }

      private string rootTag;
      private string globalFunctionCall;
      private string privilege;

      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "GroupHeader", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(_RoleSettings4CurrentUser);
      }

      private void lv_groupheaders_SelectedIndexChanged(object sender, EventArgs e)
      {
         ListView item = sender as ListView;
         if (item.FocusedItem == null) return;
         bool ghactive = Convert.ToBoolean(XElement.Parse(item.FocusedItem.Tag.ToString()).Element("GhActive").Value);
         lb_groupheaderstatus.Appearance.ImageIndex = ghactive ? 24 : 23;
         lb_groupheaderstatus.Text = ghactive ? string.Format("سرفصل {0} در سیستم وضعیت عادی و فعال دارد.کافی ایست که سرفصل درون یکی از گروه های دسترسی عضو باشد تا بتوانید به خدمات آن دسترسی داشته باشید", item.FocusedItem.Text) : string.Format("سرفصل {0} در سیستم وضعیت غیرفعال دارد. دراین حالت حتی با عضو بودن درون گروه های دسترسی شما قادر به دیدن خدمات این سرفصل نیستید", item.FocusedItem.Text);
      }

      #region Edit Menu
      private void sb_cancel_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 09 /* Execute LoadGroupHeadersOfRoles */)
               }));
      }

      private void sb_submit_Click(object sender, EventArgs e)
      {
         Func<string> XmlData = new Func<string>(
            () =>
            {
               string selectGroupHeaders = "";
               lv_groupheaders.Items.Cast<ListViewItem>()
                  .Where(p => p.Checked)
                  .ToList().ForEach(p => selectGroupHeaders += p.Tag);
               return string.Format(rootTag, selectGroupHeaders);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader",
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
                              Job _ShowError = new Job(SendType.External, "GroupHeader",
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
                                                   _DefaultGateway.Gateway(new Job(SendType.External, "GroupHeader", "GroupHeaderMenus", 09 /* Execute LoadGroupHeadersOfRoles */, SendType.SelfToUserInterface));
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
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 09 /* Execute LoadGroupHeadersOfRoles */)
               }));
      }

      private void sb_deselectall_Click(object sender, EventArgs e)
      {
         lv_groupheaders.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = false);
      }

      private void sb_invertselect_Click(object sender, EventArgs e)
      {
         lv_groupheaders.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = !item.Checked);
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         lv_groupheaders.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
      }
      #endregion

      #region Command Menu
      private void sb_create_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "", 04 /* Execute DoWork4CreateNew */, SendType.Self));
      }

      private void sb_update_Click(object sender, EventArgs e)
      {
         if (lv_groupheaders.Items.Count == 0 || lv_groupheaders.SelectedItems.Count != 1)
            return;

         string _groupHeader = lv_groupheaders.SelectedItems[0].Tag as string;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "", 05 /* Execute DoWork4Update */, SendType.Self) { Input = _groupHeader });
      }

      private void sb_duplicate_Click(object sender, EventArgs e)
      {
         if (lv_groupheaders.Items.Count == 0 || lv_groupheaders.SelectedItems.Count != 1)
            return;

         string _groupHeader = lv_groupheaders.SelectedItems[0].Tag as string;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "", 06 /* Execute DoWork4Duplicate */, SendType.Self) { Input = _groupHeader });
      }

      private void sb_activegrphdr_Click(object sender, EventArgs e)
      {
         rootTag = "<Active>{0}</Active>";
         globalFunctionCall = "ServiceDef.ActiveGroupHeaders";
         privilege = "<Privilege>6</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_groupheaders.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "GroupHeaderMenus", 10 /* Execute LoadGroupHeaderWithCondition */, SendType.SelfToUserInterface) { Input = "<WhatActive>false</WhatActive>"});
      }

      private void sb_deactivegrphdr_Click(object sender, EventArgs e)
      {
         rootTag = "<Deactive>{0}</Deactive>";
         globalFunctionCall = "ServiceDef.DeactiveGroupHeaders";
         privilege = "<Privilege>7</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_groupheaders.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "GroupHeaderMenus", 10 /* Execute LoadGroupHeaderWithCondition */, SendType.SelfToUserInterface) { Input = "<WhatActive>true</WhatActive>" });
      }

      private void sb_leave_Click(object sender, EventArgs e)
      {
         rootTag = "<Leave>{0}</Leave>";
         globalFunctionCall = "ServiceDef.LeaveGroupHeadersFromRoles";
         privilege = "<Privilege>8</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_groupheaders.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);         
      }

      private void sb_join_Click(object sender, EventArgs e)
      {
         rootTag = "<Join>{0}</Join>";
         globalFunctionCall = "ServiceDef.JoinGroupHeadersToRoles";
         privilege = "<Privilege>9</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_groupheaders.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "GroupHeaderMenus", 11 /* Execute LoadGroupHeadersForJoinToRoles */, SendType.SelfToUserInterface));
      }

      private void sb_active_Click(object sender, EventArgs e)
      {
         rootTag = "<Active>{0}</Active>";
         globalFunctionCall = "ServiceDef.ActiveGroupHeadersToRoles";
         privilege = "<Privilege>10</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_groupheaders.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "GroupHeaderMenus", 12 /* Execute LoadJoinGroupHeaderWithCondition */, SendType.SelfToUserInterface) { Input = "<WhatActive>false</WhatActive>" });
      }

      private void sb_deactive_Click(object sender, EventArgs e)
      {
         rootTag = "<Deactive>{0}</Deactive>";
         globalFunctionCall = "ServiceDef.DeactiveGroupHeadersToRoles";
         privilege = "<Privilege>11</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_groupheaders.CheckBoxes = true;
         pn_edit.Visible = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "GroupHeader", "GroupHeaderMenus", 12 /* Execute LoadJoinGroupHeaderWithCondition */, SendType.SelfToUserInterface) { Input = "<WhatActive>true</WhatActive>" });
      }
      #endregion
   }
}
