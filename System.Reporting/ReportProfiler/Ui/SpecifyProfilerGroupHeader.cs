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
   public partial class SpecifyProfilerGroupHeader : UserControl
   {
      public SpecifyProfilerGroupHeader()
      {
         InitializeComponent();
      }

      #region Section01
      private void sb_rolesettings_Click(object sender, EventArgs e)
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

      private void sb_rlroles_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 08 /* Execute LoadRolesOfUser */, SendType.SelfToUserInterface));         
      }

      private void sb_datasourcesetting_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "DefaultGateway:Datasource", 02 /* Execute DoWork4Configuration */, SendType.Self));
      }

      private void sb_rldatasource_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 09 /* Execute LoadActiveDataSource */, SendType.SelfToUserInterface));
      }

      private void sb_groupsettings_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 04 /* Execute DoWork4SpecifyReportGroupHeader */, SendType.Self));
      }

      private void sb_rlgroups_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 10 /* Execute LoadGroupHeaderOfRole */ , SendType.SelfToUserInterface) { Input = "Normal" });
      }

      private void cb_roles_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (isSaving)
            return;

         if (cb_showallrole.Checked)
         {
            /* دیگر نیازی به بارگذاری اطلاعات سربرگ های متعلق به پروفایل نیست
             * تنها کافی است که اطلاعات سربرگ مربوط به گروه دسترسی جمع آوری شود */
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 10 /* Execute LoadGroupHeaderOfRole */ , SendType.SelfToUserInterface) { Input = "Normal"});
         }
         else
         {
            /* دراینجا باید هم سربرگ های گروه دسترسی در منو کشویی قرار بگیرد هم
             * اطلاعات مربوط به سربرگ های این گروه دسترسی متعلق به پروفایل بایستی
             در لیست کلی نمایش داده شود */
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 11 /* Execute LoadGroupHeadersOfProfiler */) {Input = "Normal"},
                     new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 10 /* Execute LoadGroupHeaderOfRole */) {Input = "Normal"}
                  })
            );

         }
      }
      #endregion

      #region
      private string rootTag = "";
      private string globalFunctionCall = "";
      private string privilege = "";
      private Func<string> XMLDATA;

      private bool freeForm = true;
      private bool showNewItemPanel = false;
      private bool isSaving = false;

      private void DisabledControls()
      {
         cb_roles.Enabled = sb_rolesettings.Enabled = sb_rlroles.Enabled = false;
      }

      private void EnabledControls()
      {
         cb_roles.Enabled = sb_rolesettings.Enabled = sb_rlroles.Enabled = true;
      }

      private void LockForm()
      {
         freeForm = false;
         gb_ds.Visible = showNewItemPanel;
      }

      private void FreeForm()
      {
         freeForm = true;
         gb_ds.Visible = false;
         showNewItemPanel = false;
      }
      #endregion

      #region Command
      private void sb_add_Click(object sender, EventArgs e)
      {
         if (cbe_groupheaders.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked).Count() == 0)
            return;
         
         DisabledControls();
         LockForm();

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         //pn_orderindex.Visible = true;

         rootTag = @"<Request type=""AddNewItem"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>26</Privilege><Sub_Sys>2</Sub_Sys>";

         #region Section 01
         Func<int> MaxOrderIndex = new Func<int>(
            () =>
               {
                  if (lv_profilergroups.Items.Count == 0)
                     return 1;

                  string lastItem = lv_profilergroups.Items[lv_profilergroups.Items.Count - 1].Tag.ToString();
                  return Convert.ToInt32(XDocument.Parse(lastItem).Element("Group").Element("OrderIndex").Value) + 1;
               });

         cbe_groupheaders.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == CheckState.Checked)
            .Where(c =>
               lv_profilergroups.Items.OfType<ListViewItem>()
               .Where(t =>
                  XDocument
                  .Parse(t.Tag.ToString())
                  .Element("Group")
                  .Attribute("id").Value == c.Value.ToString()
                  ).Count() == 0)
                  .ToList()
                  .ForEach(c =>
                     {
                        lv_profilergroups.Items.Add(
                           new ListViewItem(c.Description)
                           {
                              Tag = string.Format(@"<Group id=""{0}""><Role>{1}</Role><Datasource from=""{2}"" type=""{3}"">{4}</Datasource><OrderIndex>{5}</OrderIndex></Group>",
                                                   c.Value,
                                                   cb_roles.SelectedValue,
                                                   "1" /* GroupHeader DataSource */,
                                                   true /* Ref Link to Datasource */,
                                                   "",
                                                   MaxOrderIndex()),
                              ImageIndex = 4
                           });
                        lv_profilergroups.Items[lv_profilergroups.Items.Count - 1].SubItems.AddRange(new[] { cb_roles.Text, "ارجاع به منبع سربرگ" });
                     });
            #endregion

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()
                  .Where(c => XDocument.Parse(c.Tag.ToString()).Element("Group").Element("Datasource").Value == "")
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               return string.Format("<Profiler>{0}</Profiler><Groups>{1}</Groups>", tb_profiler.Tag ,xml);
            });
      }

      private void sb_leave_Click(object sender, EventArgs e)
      {
         if (lv_profilergroups.Items.Count == 0)
            return;

         DisabledControls();
         LockForm();

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         lv_profilergroups.CheckBoxes = true;

         rootTag = @"<Request type=""Leave"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>27</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()
                  .Where(c => c.Checked)
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               return string.Format("<Profiler>{0}</Profiler><Groups>{1}</Groups>", tb_profiler.Tag, xml);
            });


      }

      private void sb_join_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         lv_profilergroups.CheckBoxes = true;

         rootTag = @"<Request type=""Join"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>28</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()
                  .Where(c => c.Checked)
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               return string.Format("<Profiler>{0}</Profiler><Groups>{1}</Groups>", tb_profiler.Tag, xml);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 11 /* Execute LoadGroupHeadersOfProfiler */, SendType.SelfToUserInterface) { Input = "Leave" });

      }

      private void sb_active_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         lv_profilergroups.CheckBoxes = true;

         rootTag = @"<Request type=""Active"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>29</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()
                  .Where(c => c.Checked)
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               return string.Format("<Profiler>{0}</Profiler><Groups>{1}</Groups>", tb_profiler.Tag, xml);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 11 /* Execute LoadGroupHeadersOfProfiler */, SendType.SelfToUserInterface) { Input = "Deactived" });
      }

      private void sb_deactive_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         lv_profilergroups.CheckBoxes = true;

         rootTag = @"<Request type=""Deactive"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>30</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()
                  .Where(c => c.Checked)
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               return string.Format("<Profiler>{0}</Profiler><Groups>{1}</Groups>", tb_profiler.Tag, xml);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 11 /* Execute LoadGroupHeadersOfProfiler */, SendType.SelfToUserInterface) { Input = "Actived" });
      }

      private void sb_ordered_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();

         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         pn_orderindex.Visible = true;

         rootTag = @"<Request type=""ChangeOrder"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>31</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()                  
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               return string.Format("<Profiler>{0}</Profiler><Groups>{1}</Groups>", tb_profiler.Tag, xml);
            });
      }

      private void sb_editds_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();

         lv_profilergroups.CheckBoxes = true;
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_edit.Visible = true;
         gb_ds.Visible = true;

         rootTag = @"<Request type=""ChangeDatasource"">{0}</Request>";
         globalFunctionCall = "Report.SetProfilerGroupHeader";
         privilege = "<Privilege>32</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>(
            () =>
            {
               string xml = "";
               lv_profilergroups.Items.OfType<ListViewItem>()
                  .Where(c => c.Checked)
                  .ToList()
                  .ForEach(c =>
                  {
                     xml += c.Tag;
                  });
               int from = rb_newds.Checked ? 3 : (rb_profiler.Checked ? 2 : (rb_groupheader.Checked ? 1 : 0));
               bool type = rb_copyds.Checked ? true : (rb_refds.Checked ? false : false);
               return string.Format(@"<Profiler>{0}</Profiler><Groups>{1}</Groups><DatasourceTarget from=""{2}""><DirectDs value=""{3}""/><DsLink type=""{4}""/></DatasourceTarget>", tb_profiler.Tag, xml, from, cb_datasource.SelectedValue, type);
            });
      }
      #endregion

      #region Edit
      private void sb_cancel_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
           new Job(SendType.External, "Localhost", "SpecifyProfilerGroupHeader", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
      }

      private void sb_submit_Click(object sender, EventArgs e)
      {
         isSaving = true;
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
                                                   new Action(() => 
                                                   {
                                                      _DefaultGateway.Gateway(new Job(SendType.External, "ServiceDefinition", "Services", 11 /* Execute LoadServicesOfParentService */, SendType.SelfToUserInterface));
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
                                 string.Format(rootTag, XMLDATA()),
                                 string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                                 "iProject",
                                 "scott"
                              }
                           }
                           #endregion                        
                        }),
                     new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 07 /* Execute LoadData */)
                  }));
         isSaving = false;
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         lv_profilergroups.Items.OfType<ListViewItem>().ToList().ForEach(c => c.Checked = true);
      }

      private void sb_invertselect_Click(object sender, EventArgs e)
      {
         lv_profilergroups.Items.OfType<ListViewItem>().ToList().ForEach(c => c.Checked = !c.Checked);
      }

      private void sb_deselect_Click(object sender, EventArgs e)
      {
         lv_profilergroups.Items.OfType<ListViewItem>().ToList().ForEach(c => c.Checked = false);
      }
      #endregion

      #region UpDown Order
      private void sb_up_Click(object sender, EventArgs e)
      {
         if (lv_profilergroups.Items.Count == 0 || lv_profilergroups.SelectedItems.Count == 0)
            return;

         var currentIndex = lv_profilergroups.SelectedItems[0].Index;
         var itemGoUp = lv_profilergroups.Items[currentIndex];
         if (currentIndex > 0)
         {
            var itemGoDown = lv_profilergroups.Items[currentIndex - 1];

            lv_profilergroups.Items.RemoveAt(currentIndex);
            lv_profilergroups.Items.Insert(currentIndex - 1, itemGoUp);

            /* Get Order Index For Change Order Index */
            string orderGoDown = XDocument.Parse(itemGoDown.Tag.ToString()).Element("Group").Element("OrderIndex").Value;
            string orderGoUp = XDocument.Parse(itemGoUp.Tag.ToString()).Element("Group").Element("OrderIndex").Value;

            /* Create XML */
            var xmlGoDown = XDocument.Parse(itemGoDown.Tag.ToString());
            var xmlGoUp = XDocument.Parse(itemGoUp.Tag.ToString());

            /* Change OrderIndex */
            xmlGoDown.Element("Group").Element("OrderIndex").SetValue(orderGoUp);
            xmlGoUp.Element("Group").Element("OrderIndex").SetValue(orderGoDown);

            /* Submit Change Order Index */
            itemGoDown.Tag = xmlGoDown.ToString();
            itemGoUp.Tag = xmlGoUp.ToString();

            lv_profilergroups.Focus();
            lv_profilergroups.Items[currentIndex - 1].Selected = true;
         }
         
      }

      private void sb_down_Click(object sender, EventArgs e)
      {
         if (lv_profilergroups.Items.Count == 0 || lv_profilergroups.SelectedItems.Count == 0)
            return;

         var currentIndex = lv_profilergroups.SelectedItems[0].Index;
         var itemGoDown = lv_profilergroups.Items[currentIndex];
         if (currentIndex < lv_profilergroups.Items.Count - 1)
         {
            var itemGoUp = lv_profilergroups.Items[currentIndex + 1];

            lv_profilergroups.Items.RemoveAt(currentIndex);
            lv_profilergroups.Items.Insert(currentIndex + 1, itemGoDown);

            /* Get Order Index For Change Order Index */
            string orderGoDown = XDocument.Parse(itemGoDown.Tag.ToString()).Element("Group").Element("OrderIndex").Value;
            string orderGoUp = XDocument.Parse(itemGoUp.Tag.ToString()).Element("Group").Element("OrderIndex").Value;

            /* Create XML */
            var xmlGoDown = XDocument.Parse(itemGoDown.Tag.ToString());
            var xmlGoUp = XDocument.Parse(itemGoUp.Tag.ToString());

            /* Change OrderIndex */
            xmlGoDown.Element("Group").Element("OrderIndex").SetValue(orderGoUp);
            xmlGoUp.Element("Group").Element("OrderIndex").SetValue(orderGoDown);

            /* Submit Change Order Index */
            itemGoDown.Tag = xmlGoDown.ToString();
            itemGoUp.Tag = xmlGoUp.ToString();

            lv_profilergroups.Focus();
            lv_profilergroups.Items[currentIndex + 1].Selected = true;
         }
      }
      #endregion

      private void lv_profilergroups_DoubleClick(object sender, EventArgs e)
      {
         if (lv_profilergroups.SelectedItems.Count == 0 || lv_profilergroups.SelectedItems.Count > 1)
            return;

         string xml = string.Format(@"<Group ID=""{0}""><FaName>{1}</FaName></Group>", 
            XDocument.Parse(lv_profilergroups.SelectedItems[0].Tag.ToString()).Element("Group").Attribute("id").Value, 
            lv_profilergroups.SelectedItems[0].Text
         );
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 05 /* Execute DoWork4SpecifyGroupItems */, SendType.Self) { Input = xml });
      }
   }
}
