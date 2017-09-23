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
using DevExpress.XtraEditors.Controls;

namespace System.ServiceDefinition.SrvDef.UnderGateways.SrvUnitType.Ui
{
   public partial class UnitTypeMenus : UserControl
   {
      public UnitTypeMenus()
      {
         InitializeComponent();
      }

      private string rootTag;
      private string globalFunctionCall;
      private string privilege;
      private Func<string> XmlData;

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "UnitType", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      #region UnitType
      private void cb_unittypeshow_SelectedIndexChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 07 /* Execute LoadData */,  SendType.SelfToUserInterface));
      }
      #endregion

      #region Command Menu
      private void sb_create_Click(object sender, EventArgs e)
      {
         rootTag = "<Create>{0}</Create>";
         globalFunctionCall = "ServiceDef.CreateUnitType";
         privilege = "<Privilege>20</Privilege><Sub_Sys>1</Sub_Sys>";
         pn_editgrpsrv.Visible = true;
         pn_desc.Enabled = true;
         cb_unittype.SelectedIndex = cb_unittypeshow.SelectedIndex - 1;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         XmlData = new Func<string>(
            () =>
            {
               ccbe_roles.Properties.GetItems()
                  .OfType<CheckedListBoxItem>()
                  .Where(role => role.CheckState == CheckState.Checked)
                  .ToList()
                  .ForEach(rs => xmldata += string.Format("<RoleID>{0}</RoleID>",rs.Value));
               return string.Format("<TitleFa>{1}</TitleFa><UnitType>{2}</UnitType><Roles>{0}</Roles>", xmldata, te_rw_f_titlefa.Text, cb_unittype.SelectedIndex);
            });
      }

      private void sb_update_Click(object sender, EventArgs e)
      {
         if (lv_unittype.SelectedItems.Count != 1)
            return;

         rootTag = "<Update>{0}</Update>";
         globalFunctionCall = "ServiceDef.UpdateUnitType";
         privilege = "<Privilege>21</Privilege><Sub_Sys>1</Sub_Sys>";
         pn_editgrpsrv.Visible = true;
         pn_desc.Enabled = true;
         cb_unittype.SelectedIndex = cb_unittypeshow.SelectedIndex - 1;
         lv_unittype.Enabled = false;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         XmlData = new Func<string>(
            () =>
            {
               ccbe_roles.Properties.GetItems()
                  .OfType<CheckedListBoxItem>()
                  .Where(role => role.CheckState == CheckState.Checked)
                  .ToList()
                  .ForEach(rs => xmldata += string.Format("<RoleID>{0}</RoleID>", rs.Value));
               return string.Format("<UnitTypeID>{0}</UnitTypeID><TitleFa>{1}</TitleFa><UnitType>{2}</UnitType><Roles>{3}</Roles>", lv_unittype.SelectedItems[0].Tag, te_rw_f_titlefa.Text, cb_unittype.SelectedIndex, xmldata);
            });

         // Get Role(s) Of UnitType Item From ListView
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 10 /* Execute LoadRolesOfUnitType */, SendType.SelfToUserInterface) { Input = string.Format("<UnitTypeID>{0}</UnitTypeID>", lv_unittype.SelectedItems[0].Tag)});
      }

      private void sb_duplicate_Click(object sender, EventArgs e)
      {

      }

      private void sb_active_Click(object sender, EventArgs e)
      {
         rootTag = "<Active>{0}</Active>";
         globalFunctionCall = "ServiceDef.ActiveUnitType";
         privilege = "<Privilege>22</Privilege><Sub_Sys>1</Sub_Sys>";
         pn_editgrpsrv.Visible = true;
         lv_unittype.CheckBoxes = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         XmlData = new Func<string>(
            () =>
            {
               lv_unittype.CheckedItems.OfType<ListViewItem>()
                  .ToList()
                  .ForEach(rs => xmldata += string.Format("<UnitTypeID>{0}</UnitTypeID>", rs.Tag));
               return string.Format("<UnitType>{0}</UnitType>",  xmldata);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 11 /* Execute LoadActiveUnitTypeWithCondition */, SendType.SelfToUserInterface) { Input = string.Format("<Where><WhatAction>false</WhatAction><UnitType>{0}</UnitType></Where>", cb_unittypeshow.SelectedIndex) });
      }

      private void sb_deactive_Click(object sender, EventArgs e)
      {
         rootTag = "<Deactive>{0}</Deactive>";
         globalFunctionCall = "ServiceDef.DeactiveUnitType";
         privilege = "<Privilege>23</Privilege><Sub_Sys>1</Sub_Sys>";
         pn_editgrpsrv.Visible = true;
         lv_unittype.CheckBoxes = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         XmlData = new Func<string>(
            () =>
            {
               lv_unittype.CheckedItems.OfType<ListViewItem>()
                  .ToList()
                  .ForEach(rs => xmldata += string.Format("<UnitTypeID>{0}</UnitTypeID>", rs.Tag));
               return string.Format("<UnitType>{0}</UnitType>", xmldata);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 11 /* Execute LoadActiveUnitTypeWithCondition */, SendType.SelfToUserInterface) { Input = string.Format("<Where><WhatAction>true</WhatAction><UnitType>{0}</UnitType></Where>", cb_unittypeshow.SelectedIndex) });
      }

      private void sb_enabled_Click(object sender, EventArgs e)
      {
         rootTag = "<Enabled>{0}</Enabled>";
         globalFunctionCall = "ServiceDef.EnabledUnitType";
         privilege = "<Privilege>24</Privilege><Sub_Sys>1</Sub_Sys>";
         pn_editgrpsrv.Visible = true;
         lv_unittype.CheckBoxes = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         XmlData = new Func<string>(
            () =>
            {
               lv_unittype.CheckedItems.OfType<ListViewItem>()
                  .ToList()
                  .ForEach(rs => xmldata += string.Format("<UnitTypeID>{0}</UnitTypeID>", rs.Tag));
               return string.Format("<UnitType>{0}</UnitType>", xmldata);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 12 /* Execute LoadVisibleUnitTypeWithCondition */, SendType.SelfToUserInterface) { Input = string.Format("<Where><WhatAction>false</WhatAction><UnitType>{0}</UnitType></Where>", cb_unittypeshow.SelectedIndex) });
      }

      private void sb_disabled_Click(object sender, EventArgs e)
      {
         rootTag = "<Disabled>{0}</Disabled>";
         globalFunctionCall = "ServiceDef.DisabledUnitType";
         privilege = "<Privilege>25</Privilege><Sub_Sys>1</Sub_Sys>";
         pn_editgrpsrv.Visible = true;
         lv_unittype.CheckBoxes = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         XmlData = new Func<string>(
            () =>
            {
               lv_unittype.CheckedItems.OfType<ListViewItem>()
                  .ToList()
                  .ForEach(rs => xmldata += string.Format("<UnitTypeID>{0}</UnitTypeID>", rs.Tag));
               return string.Format("<UnitType>{0}</UnitType>", xmldata);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 12 /* Execute LoadVisibleUnitTypeWithCondition */, SendType.SelfToUserInterface) { Input = string.Format("<Where><WhatAction>true</WhatAction><UnitType>{0}</UnitType></Where>", cb_unittypeshow.SelectedIndex) });
      }
      #endregion

      #region Edit Menu
      private void sb_commit_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType",
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

                                       Job _ShowError = new Job(SendType.External, "UnitType", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
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
                                    })
                                 },
                                 #endregion                     
                                 #region DoWork
                                 new Job(SendType.Self, 04 /* Execute DoWork4Odbc*/)
                                 {
                                    Input = new List<object>
                                    {
                                       false,
                                       "procedure",
                                       true,
                                       false,
                                       "xml",
                                       string.Format(rootTag, XmlData()),
                                       string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                                       "iProject",
                                       "scott"
                                    }
                                 }
                                 #endregion
                              }),
                           new Job(SendType.SelfToUserInterface, "SUTM", 07 /* Execute LoadData */),
                        }));
      }

      private void sb_rollback_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "UnitType", "SUTM", 07 /* Execute LoadData */,SendType.SelfToUserInterface));
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         lv_unittype.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = true);
      }

      private void sb_selectinvert_Click(object sender, EventArgs e)
      {
         lv_unittype.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = !item.Checked);
      }

      private void sb_deselect_Click(object sender, EventArgs e)
      {
         lv_unittype.Items.OfType<ListViewItem>().ToList().ForEach(item => item.Checked = false);
      }
      #endregion

      #region Role
      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "UnitType",
               new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                              })
                        });
         _DefaultGateway.Gateway(_RoleSettings4CurrentUser);
      }
      #endregion

   }
}
