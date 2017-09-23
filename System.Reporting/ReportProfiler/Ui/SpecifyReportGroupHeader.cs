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
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.Ui
{
   public partial class SpecifyReportGroupHeader : UserControl
   {
      public SpecifyReportGroupHeader()
      {
         InitializeComponent();
      }

      private void LangChangeToFarsi(object sender, EventArgs e)
      {
         Job _LangChangeToFarsi = new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LanChangeToFarsi */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToFarsi);
      }

      private void LangChangeToEnglish(object sender, EventArgs e)
      {
         Job _LangChangeToEnglish = new Job(SendType.External, "Localhost", "Commons", 09 /* Execute LanChangeToEnglish */, SendType.Self);
         _DefaultGateway.Gateway(_LangChangeToEnglish);
      }

      #region Section 01
      private bool freeForm = true;
      private bool showNewItemPanel = false;

      private void DisabledControls()
      {
         cb_roles.Enabled = sb_rolesettings.Enabled = sb_rlroles.Enabled = lv_groupheaders.Enabled = false;
      }

      private void EnabledControls()
      {
         cb_roles.Enabled = sb_rolesettings.Enabled = sb_rlroles.Enabled = lv_groupheaders.Enabled = true;
      }

      private void LockForm()
      {
         freeForm = false;
         pn_newitem.Visible = showNewItemPanel;
      }

      private void FreeForm()
      {
         freeForm = true;
         pn_newitem.Visible = false;
         showNewItemPanel = false;
      }


      private void ClearForm()
      {
         te_rw_faname.Text = te_rw_enname.Text = te_rw_shortcut.Text = "";
         cbe_roles.Text = "";
      }

      private string rootTag = "";
      private string globalFunctionCall = "";
      private string privilege = "";
      private Func<string> XMLDATA;

      private void sb_addnew_Click(object sender, EventArgs e)
      {
         ClearForm();
         showNewItemPanel = true;
         DisabledControls();
         LockForm();
         cb_remove.Enabled = false;
         rootTag = @"<Request Type=""AddNewItem""><GroupHeader>{0}</GroupHeader></Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>19</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;

         te_rw_faname.Focus();

         XMLDATA = new Func<string>
         (() =>
         {
            string role = "";
            cbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => role += string.Format("<Role>{0}</Role>", c.Value));
            return string.Format("<FaName>{0}</FaName><EnName>{1}</EnName><ShortCut>{2}</ShortCut><Datasource>{3}</Datasource><Roles>{4}</Roles>",
               te_rw_faname.Text, te_rw_enname.Text, te_rw_shortcut.Text, cb_datasource.SelectedValue, role);
         });
      }

      private void sb_update_Click(object sender, EventArgs e)
      {
         if (lv_groupheaders.SelectedItems.Count == 0)
            return;

         pn_newitem.Tag = lv_groupheaders.SelectedItems[0].Tag;

         showNewItemPanel = true;

         DisabledControls();
         LockForm();
         cb_remove.Enabled = true;
         rootTag = @"<Request Type=""Update"">{0}</Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>20</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;

         te_rw_faname.Focus();

         XMLDATA = new Func<string>
         (() =>
         {
            string role = "";
            cbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).ToList().ForEach(c => role += string.Format("<Role>{0}</Role>", c.Value));
            return string.Format(@"<GroupHeader>{0}</GroupHeader><FaName>{1}</FaName><EnName>{2}</EnName><ShortCut>{3}</ShortCut><Datasource>{4}</Datasource><Roles RemoveType=""{6}"">{5}</Roles>",
               pn_newitem.Tag, te_rw_faname.Text, te_rw_enname.Text, te_rw_shortcut.Text, cb_datasource.SelectedValue, role, cb_remove.Checked);
         });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 09 /* Execute LoadProfilesOfRole */, SendType.SelfToUserInterface) { Input = "SingleDetail" });
      }

      private void sb_duplicate_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();
         rootTag = @"<Request Type=""Duplicate""><GroupHeader>{0}</GroupHeader></Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>21</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;

         te_rw_faname.Focus();

         XMLDATA = new Func<string>
         (() =>
         {
            string role = "";
            cbe_roles.Properties.GetItems().OfType<CheckedListBoxItem>().ToList().ForEach(c => role += string.Format("<Role>{0}</Role>", c.Value));
            return string.Format("<GroupHeader>{0}</GroupHeader><FaName>{1}</FaName><EnName>{2}</EnName><ShortCut>{3}</ShortCut><Datasource>{4}</Datasource><Roles>{5}</Roles>",
               pn_newitem.Tag, te_rw_faname.Text, te_rw_enname.Text, te_rw_shortcut.Text, cb_datasource.SelectedValue, role);
         });
      }

      private void sb_delete_Click(object sender, EventArgs e)
      {
         LockForm();
         rootTag = @"<Request Type=""Delete"">{0}</Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>22</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_newitem.Enabled = false;
         lv_groupheaders.CheckBoxes = true;

         XMLDATA = new Func<string>(
            () =>
            {
               string profiles = "";
               lv_groupheaders.Items.OfType<ListViewItem>().Where(c => c.Checked).ToList().ForEach(c => profiles += string.Format("<GroupHeader>{0}</GroupHeader>", c.Tag));
               return string.Format("<Role>{0}</Role><GroupHeaders>{1}</GroupHeaders>", cb_roles.SelectedValue, profiles);
            });
      }

      private void sb_undelete_Click(object sender, EventArgs e)
      {
         LockForm();
         rootTag = @"<Request Type=""UnDelete"">{0}</Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>23</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_newitem.Enabled = false;
         lv_groupheaders.CheckBoxes = true;

         XMLDATA = new Func<string>(
            () =>
            {
               string profiles = "";
               lv_groupheaders.Items.OfType<ListViewItem>().Where(c => c.Checked).ToList().ForEach(c => profiles += string.Format("<GroupHeader>{0}</GroupHeader>", c.Tag));
               return string.Format("<Role>{0}</Role><GroupHeaders>{1}</GroupHeaders>", cb_roles.SelectedValue, profiles);
            });

         /* Load Delete Profile And Insert Ino ListView */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 09 /* Execute LoadProfilesOfRole */, SendType.SelfToUserInterface) { Input = "Deleted" });
      }

      private void sb_deactive_Click(object sender, EventArgs e)
      {
         LockForm();
         rootTag = @"<Request Type=""Deactive"">{0}</Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>24</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_newitem.Enabled = false;
         lv_groupheaders.CheckBoxes = true;

         XMLDATA = new Func<string>(
            () =>
            {
               string profiles = "";
               lv_groupheaders.Items.OfType<ListViewItem>().Where(c => c.Checked).ToList().ForEach(c => profiles += string.Format("<GroupHeader>{0}</GroupHeader>", c.Tag));
               return string.Format("<Role>{0}</Role><GroupHeaders>{1}</GroupHeaders>", cb_roles.SelectedValue, profiles);
            });

         /* Load Actived Profile And Insert Ino ListView */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 09 /* Execute LoadProfilesOfRole */, SendType.SelfToUserInterface) { Input = "Actived" });
      }

      private void sb_active_Click(object sender, EventArgs e)
      {
         LockForm();
         rootTag = @"<Request Type=""Active"">{0}</Request>";
         globalFunctionCall = "Report.SetGroupHeaderInRole";
         privilege = "<Privilege>25</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_newitem.Enabled = false;
         lv_groupheaders.CheckBoxes = true;

         XMLDATA = new Func<string>(
            () =>
            {
               string profiles = "";
               lv_groupheaders.Items.OfType<ListViewItem>().Where(c => c.Checked).ToList().ForEach(c => profiles += string.Format("<GroupHeader>{0}</GroupHeader>", c.Tag));
               return string.Format("<Role>{0}</Role><GroupHeaders>{1}</GroupHeaders>", cb_roles.SelectedValue, profiles);
            });

         /* Load Actived Profile And Insert Ino ListView */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 09 /* Execute LoadProfilesOfRole */, SendType.SelfToUserInterface) { Input = "Deactived" });
      }


      private void sb_leave_Click(object sender, EventArgs e)
      {
         LockForm();
         rootTag = @"<Request Type=""Leave"">{0}</Request>";
         globalFunctionCall = "Report.[SetGroupHeaderInRole]";
         privilege = "<Privilege>44</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_newitem.Enabled = false;
         lv_groupheaders.CheckBoxes = true;

         XMLDATA = new Func<string>(
            () =>
            {
               string groupheader = "";
               lv_groupheaders.Items.OfType<ListViewItem>().Where(c => c.Checked).ToList().ForEach(c => groupheader += string.Format("<GroupHeader>{0}</GroupHeader>", c.Tag));
               return string.Format("<Role>{0}</Role>{1}", cb_roles.SelectedValue, groupheader);
            });
      }

      private void sb_join_Click(object sender, EventArgs e)
      {
         LockForm();
         rootTag = @"<Request Type=""Join"">{0}</Request>";
         globalFunctionCall = "Report.[SetGroupHeaderInRole]";
         privilege = "<Privilege>43</Privilege><Sub_Sys>2</Sub_Sys>";

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_newitem.Enabled = false;
         lv_groupheaders.CheckBoxes = true;

         XMLDATA = new Func<string>(
            () =>
            {
               string groupheader = "";
               lv_groupheaders.Items.OfType<ListViewItem>().Where(c => c.Checked).ToList().ForEach(c => groupheader += string.Format("<GroupHeader>{0}</GroupHeader>", c.Tag));
               return string.Format("<Role>{0}</Role>{1}", cb_roles.SelectedValue, groupheader);
            });

         /* Load Actived Profile And Insert Ino ListView */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 09 /* Execute LoadProfilesOfRole */, SendType.SelfToUserInterface) { Input = "Leaved" });
      }

      private void sb_cancel1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
      }

      private void sb_submit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "Localhost",
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
                                                   null
                                                   //new Action(() => 
                                                   //{
                                                   //   _DefaultGateway.Gateway(new Job(SendType.External, "ServiceDefinition", "Services", 11 /* Execute LoadServicesOfParentService */, SendType.SelfToUserInterface));
                                                   //})
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
                                 string.Format(rootTag, XMLDATA()),
                                 string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                                 "iProject",
                                 "scott"
                              }
                           }
                           #endregion                        
                        }),
                     new Job(SendType.SelfToUserInterface, "SpecifyReportGroupHeader", 07 /* Execute LoadData */)
                  }));
      }

      private void sb_deselect_Click(object sender, EventArgs e)
      {
         lv_groupheaders.Controls.OfType<ListViewItem>().ToList().ForEach(c => c.Checked = false);
      }

      private void sb_invertselect_Click(object sender, EventArgs e)
      {
         lv_groupheaders.Controls.OfType<ListViewItem>().ToList().ForEach(c => c.Checked = !c.Checked);
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         lv_groupheaders.Controls.OfType<ListViewItem>().ToList().ForEach(c => c.Checked = true);
      }
      #endregion

      #region Section 02
      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 08 /* Execute LoadRolesOfUser */, SendType.SelfToUserInterface));
      }

      private void sb_rlroles_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                     })
               }));
      }

      private void cb_roles_SelectedIndexChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 09 /* Execute LoadGroupHeadersOfRole */, SendType.SelfToUserInterface) { Input = "Normal" });
      }
      #endregion

      private void sb_groupheaders_Click(object sender, EventArgs e)
      {
         if (lv_groupheaders.SelectedItems.Count == 0 || lv_groupheaders.SelectedItems.Count > 1)
            return;

         string xml = string.Format(@"<Profiler ID=""{0}""><FaName>{1}</FaName></Profiler>", lv_groupheaders.SelectedItems[0].Tag, lv_groupheaders.SelectedItems[0].Text);
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 03 /* Execute DoWork4SpecifyProfilerGroupHeader*/, SendType.Self) { Input = xml });
      }

      private void sb_rldatasource_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyReportGroupHeader", 10 /* Execute LoadActiveDataSource */, SendType.SelfToUserInterface));
      }

      private void sb_datasourcesetting_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway:Datasource", 02 /* Execute DoWork4Configuration */, SendType.Self));
      }

      private void sb_groupitems_Click(object sender, EventArgs e)
      {
         if (lv_groupheaders.SelectedItems.Count == 0 || lv_groupheaders.SelectedItems.Count > 1)
            return;

         string xml = string.Format(@"<Group ID=""{0}""><FaName>{1}</FaName></Group>", lv_groupheaders.SelectedItems[0].Tag, lv_groupheaders.SelectedItems[0].Text);
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 05 /* Execute DoWork4SpecifyGroupItems */, SendType.Self) { Input = xml });
      }

      private void lv_groupheaders_DoubleClick(object sender, EventArgs e)
      {
         sb_groupitems_Click(null, null);
      }


   }
}
