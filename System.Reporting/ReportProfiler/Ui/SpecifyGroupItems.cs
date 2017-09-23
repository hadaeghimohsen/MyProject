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
using DevExpress.XtraEditors.Controls;

namespace System.Reporting.ReportProfiler.Ui
{
   public partial class SpecifyGroupItems : UserControl
   {
      public SpecifyGroupItems()
      {
         InitializeComponent();
      }

      #region Section One
      private bool freeForm = true;
      private bool isSaving = false;
      private string rootTag = "";
      private string globalFunctionCall = "";
      private string privilege = "";
      private Func<string> XMLDATA;

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
      }

      private void FreeForm()
      {
         freeForm = true;
      }
      #endregion

      #region Section Two
      private void sb_choosereportfile_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
             new Job(SendType.External, "Localhost",
                new List<Job>
                {
                   new Job(SendType.External, "DefaultGateway",
                      new List<Job>
                      {
                         new Job(SendType.External, "ReportUnitType",
                            new List<Job>
                            {
                               new Job(SendType.Self, 02 /* Execute DoWork4ReportUnitType */),
                               new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 14 /* Execute ShowChooseReport */)
                            })
                      })                
                }));
      }

      bool isNotCompletedOperation = false;
      private void tv_left_AfterCheck(object sender, TreeViewEventArgs e)
      {
         if (isNotCompletedOperation)
            return;

         /* Field IS SELECTED */
         if (e.Node.Parent != null)
         {
            isNotCompletedOperation = true;
            if (e.Node.Checked)               
               e.Node.Parent.Checked = true;            
            else
               if (e.Node.Parent.Nodes.OfType<TreeNode>().Where(f => f.Checked).Count() == 0)
                  e.Node.Parent.Checked = false;
            isNotCompletedOperation = false;
         }
         else /* Table IS SELECTED*/
         {
            isNotCompletedOperation = true;
            e.Node.Nodes.OfType<TreeNode>().ToList().ForEach(f => f.Checked = e.Node.Checked);
            isNotCompletedOperation = false;
         }
      }
      private void tv_right_AfterCheck(object sender, TreeViewEventArgs e)
      {
         if (isNotCompletedOperation)
            return;

         /* Field IS SELECTED */
         if (e.Node.Parent != null)
         {
            isNotCompletedOperation = true;
            if (e.Node.Checked)
               e.Node.Parent.Checked = true;
            else
               if (e.Node.Parent.Nodes.OfType<TreeNode>().Where(f => f.Checked).Count() == 0)
                  e.Node.Parent.Checked = false;
            isNotCompletedOperation = false;
         }
         else /* Table IS SELECTED*/
         {
            isNotCompletedOperation = true;
            e.Node.Nodes.OfType<TreeNode>().ToList().ForEach(f => f.Checked = e.Node.Checked);
            isNotCompletedOperation = false;
         }
      }

      private void sb_transfer_Click(object sender, EventArgs e)
      {
         if (cb_roles.SelectedValue == null || cb_roles.Items.Count == 0)
            return;

         DisabledControls();
         LockForm();
         pn_command.Enabled = false;
         pn_orderindex.Visible = false;
         pn_edit.Visible = true;
         rootTag = @"<Request type=""Transfer"">{0}</Request>";
         globalFunctionCall = "Report.SetItemsInGroup";
         privilege = "<Privilege>33</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>
         (() =>
            {
               List<string> xmlTables = new List<string>();
               string xmlTable = string.Empty;
               List<string> xmlFields = new List<string>();
               tv_right.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(t =>
                  {
                     xmlTable = t.Tag.ToString();
                     xmlFields.Clear();
                     t.Nodes
                        .OfType<TreeNode>()
                        .Where(f => XDocument.Parse(f.Tag.ToString()).Element("Field").Attribute("global") == null)
                        .ToList()
                        .ForEach(f =>
                        {
                           xmlFields.Add(f.Tag.ToString());
                        });
                     xmlTables.Add(string.Format(xmlTable, string.Join("\n",xmlFields)));
                  });
               return string.Format(@"<Group id=""{1}""/><Role id=""{2}""/><Filters>{0}</Filters>", string.Join("\n", xmlTables), tb_group.Tag, cb_roles.SelectedValue);
            });

         #region Processing
         Func<int> maxTOrderIndex = new Func<int>
         (() =>
            {
               if (tv_right.Nodes.Count == 0)
                  return 0;
               return tv_right.Nodes
                  .OfType<TreeNode>()
                  //.Where(t => XDocument.Parse(t.Tag.ToString()).Element("Table") != null)
                  .Max(t => Convert.ToInt32(XDocument.Parse(t.Tag.ToString()).Element("Table").Attribute("orderIndex").Value));
            });

         Func<TreeNode, int> maxFOrderIndex = new Func<TreeNode, int>
         ((tn) =>
            {
               if (tn.Nodes.Count == 0)
                  return 0;
               return tn.Nodes
                  .OfType<TreeNode>()
                  .Max(f => Convert.ToInt32(XDocument.Parse(f.Tag.ToString()).Element("Field").Attribute("orderIndex").Value));
            });

         tv_left.Nodes.OfType<TreeNode>()
            .Where(tl => tl.Checked)
            .ToList()
            .ForEach(tl =>
            {
               /* INSERT TABLE IN TREEVIEW RIGHT */
               XDocument tlxdoc = XDocument.Parse(tl.Tag.ToString());

               /* CHECKED EXISTS TABLE IN RIGHT TREEVIEW */
               TreeNode tv_r =
                  tv_right.Nodes.OfType<TreeNode>()
                     .Where(tr => /*XDocument.Parse(tr.Tag.ToString()).Element("Table") != null &&*/
                                  XDocument.Parse(tr.Tag.ToString()).Element("Table").Attribute("enName").Value ==
                                  tlxdoc.Element("Table").Attribute("enName").Value)
                     .FirstOrDefault<TreeNode>();
               if (tv_r == null)
               {                  
                  tlxdoc.Element("Table").Attribute("orderIndex").SetValue(maxTOrderIndex() + 1);
                  tv_r = new TreeNode(tlxdoc.Element("Table").Attribute("faName").Value) { Tag = tlxdoc, ImageIndex = 0, SelectedImageIndex = 0 };
               }

               /* IF RIGHT NODE IN TREEVIEW IS NOT ANY Field IN LIST */
               if (tv_r.Nodes.Count == 0)
               {
                  tl.Nodes.OfType<TreeNode>()
                     .Where(fl => fl.Checked)
                     .ToList()
                     .ForEach(fl =>
                     {
                        /* SET OrderIndex TO ANY FIELD FOR INSERT */
                        XDocument flxdoc = XDocument.Parse(fl.Tag.ToString());
                        flxdoc.Element("Field").Attribute("orderIndex").SetValue(maxFOrderIndex(tv_r) + 1);
                        tv_r.Nodes.Add(
                           new TreeNode(flxdoc.Element("Field").Attribute("faName").Value) { Tag = flxdoc, ImageIndex = 1, SelectedImageIndex = 1 });
                     });
                  /* AT LAST ADD TreeNode IN TreeView IF DOSE NOT EXISTS IN RIGHT TREEVIEW */
                  if(!tv_right.Nodes.Contains(tv_r))
                     tv_right.Nodes.Add(tv_r);
               }
               else /* APPEND NEW Field ITEM IN RIGHT NODE IN TREEVIEW */
                  tl.Nodes.OfType<TreeNode>()
                     .Where(f => f.Checked) /* SELECTED Field IN LEFT TREEVIEW */
                     .Where(f => tv_r.Nodes
                        .OfType<TreeNode>()
                        .Where(fr => XDocument.Parse(fr.Tag.ToString()).Element("Field").Attribute("enName").Value ==
                                     XDocument.Parse(f.Tag.ToString()).Element("Field").Attribute("enName").Value)
                        .Count() == 0
                        ) /* CHECKED IF NOT EXISTS Field IN RIGHT TREEVIEW*/
                     .ToList()
                     .ForEach(fl =>
                     {
                        /* SET OrderIndex TO ANY FIELD FOR INSERT */
                        XDocument flxdoc = XDocument.Parse(fl.Tag.ToString());
                        flxdoc.Element("Field").Attribute("orderIndex").SetValue(maxFOrderIndex(tv_r) + 1);
                        tv_r.Nodes.Add(
                           new TreeNode(flxdoc.Element("Field").Attribute("faName").Value) { Tag = flxdoc, ImageIndex = 1, SelectedImageIndex = 1 });
                     });               
            });
         tv_right.ExpandAll();
         #endregion
      }
      #endregion

      #region Edit
      private void sb_cancel_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 07 /* Execute LoadData */, SendType.SelfToUserInterface));
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
                                 Job _ShowError = new Job(SendType.External, "Localhost",
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
                     new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 07 /* Execute LoadData */)
                  }));
         isSaving = false;
      }

      private void sb_selectall_Click(object sender, EventArgs e)
      {
         tv_right.Nodes
            .OfType<TreeNode>()
            .ToList()
            .ForEach(t => t.Checked = true);
      }

      private void sb_invertselect_Click(object sender, EventArgs e)
      {

      }

      private void sb_deselect_Click(object sender, EventArgs e)
      {
         tv_right.Nodes
            .OfType<TreeNode>()
            .ToList()
            .ForEach(t => t.Checked = false);
      }
      #endregion

      #region Role
      private void cb_roles_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (isSaving)
            return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 11 /* Execute LoadSavedItemsinGroup */, SendType.SelfToUserInterface) { Input = "Normal" });
      }

      private void sb_rlroles_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 08 /* Execute LoadRolesOfUser */, SendType.SelfToUserInterface));         
      }

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
      #endregion

      #region Table & Field
      private void cb_regtables_SelectedIndexChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 10 /* Execute LoadColumnsOfTable */, SendType.SelfToUserInterface) { Input = "Normal"});
      }

      private void sb_rlregtables_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 09 /* Execute LoadRegisteredTables */, SendType.SelfToUserInterface) { Input = "Normal" });
      }

      private void sb_tablesettings_Click(object sender, EventArgs e)
      {

      }

      private void sb_rlfields_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 10 /* Execute LoadColumnsOfTable */, SendType.SelfToUserInterface) { Input = "Normal" });
      }

      private void sb_fieldsettings_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Command
      private void sb_add_Click(object sender, EventArgs e)
      {
         if (cbe_column.Text == "")
            return;

         DisabledControls();
         LockForm();
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_orderindex.Visible = false;
         pn_edit.Visible = true;
         pn_transfer.Enabled = false;
         rootTag = @"<Request type=""AddNewItem"">{0}</Request>";
         globalFunctionCall = "Report.SetItemsInGroup";
         privilege = "<Privilege>34</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>
         (() =>
         {
            List<string> xmlTables = new List<string>();
            string xmlTable = string.Empty;
            List<string> xmlFields = new List<string>();
            tv_right.Nodes
               .OfType<TreeNode>()
               .Where(t => XDocument.Parse(t.Tag.ToString()).Element("Table").Attribute("id") != null)
               .ToList()
               .ForEach(t =>
               {
                  xmlTable = t.Tag.ToString();
                  xmlFields.Clear();
                  t.Nodes
                     .OfType<TreeNode>()
                     .Where(f => XDocument.Parse(f.Tag.ToString()).Element("Field").Attribute("id") != null)
                     .ToList()                     
                     .ForEach(f =>
                     {
                        xmlFields.Add(f.Tag.ToString());
                     });
                  xmlTables.Add(string.Format(xmlTable, string.Join("\n", xmlFields)));
               });
            return string.Format(@"<Group id=""{1}""/><Role id=""{2}""/><Filters>{0}</Filters>", string.Join("\n", xmlTables), tb_group.Tag, cb_roles.SelectedValue);
         });

         /* CHECKED SELECTED TABLE IN TREE VIEW */
         #region Processing
         Func<int> maxTOrderIndex = new Func<int>
         (() =>
         {
            if (tv_right.Nodes.Count == 0)
               return 0;
            return tv_right.Nodes
               .OfType<TreeNode>()
               //.Where(t => XDocument.Parse(t.Tag.ToString()).Element("Table") != null)
               .Max(t => Convert.ToInt32(XDocument.Parse(t.Tag.ToString()).Element("Table").Attribute("orderIndex").Value));
         });

         Func<TreeNode, int> maxFOrderIndex = new Func<TreeNode, int>
         ((tn) =>
         {
            if (tn.Nodes.Count == 0)
               return 0;
            return tn.Nodes
               .OfType<TreeNode>()
               .Max(f => Convert.ToInt32(XDocument.Parse(f.Tag.ToString()).Element("Field").Attribute("orderIndex").Value));
         });

         cbe_column.Properties.GetItems().OfType<CheckedListBoxItem>()
            .Where(tl => tl.CheckState == CheckState.Checked)
            .ToList()
            .ForEach(tl =>
            {
               /* INSERT TABLE IN TREEVIEW RIGHT */
               XDocument tlxdoc = XDocument.Parse(string.Format(@"<Table id=""{0}"" enName=""{1}"" orderIndex="""">{{0}}</Table>", cb_regtables.SelectedValue, cb_regtables.Text));

               /* CHECKED EXISTS TABLE IN RIGHT TREEVIEW */
               TreeNode tv_r =
                  tv_right.Nodes.OfType<TreeNode>()
                     .Where(tr => /*XDocument.Parse(tr.Tag.ToString()).Element("Table") != null &&*/
                                  XDocument.Parse(tr.Tag.ToString()).Element("Table").Attribute("enName").Value ==
                                  cb_regtables.Text)
                     .FirstOrDefault<TreeNode>();
               if (tv_r == null)
               {
                  tlxdoc.Element("Table").Attribute("orderIndex").SetValue(maxTOrderIndex() + 1);
                  tv_r = new TreeNode(cb_regtables.Text) { Tag = tlxdoc, ImageIndex = 0, SelectedImageIndex = 0 };
               }

               /* IF RIGHT NODE IN TREEVIEW IS NOT ANY Field IN LIST */
               if (tv_r.Nodes.Count == 0)
               {
                  cbe_column.Properties.GetItems().OfType<CheckedListBoxItem>()
                     .Where(fl => fl.CheckState == CheckState.Checked)
                     .ToList()
                     .ForEach(fl =>
                     {
                        /* SET OrderIndex TO ANY FIELD FOR INSERT */
                        XDocument flxdoc = XDocument.Parse(string.Format(@"<Field id=""{0}"" enName=""{1}"" orderIndex=""""/>", fl.Value, fl.Description));
                        flxdoc.Element("Field").Attribute("orderIndex").SetValue(maxFOrderIndex(tv_r) + 1);
                        tv_r.Nodes.Add(
                           new TreeNode(fl.Description) { Tag = flxdoc, ImageIndex = 1, SelectedImageIndex = 1 });
                     });
                  /* AT LAST ADD TreeNode IN TreeView IF DOSE NOT EXISTS IN RIGHT TREEVIEW */
                  if (!tv_right.Nodes.Contains(tv_r))
                     tv_right.Nodes.Add(tv_r);
               }
               else /* APPEND NEW Field ITEM IN RIGHT NODE IN TREEVIEW */
                  cbe_column.Properties.GetItems().OfType<CheckedListBoxItem>()
                     .Where(fl => fl.CheckState == CheckState.Checked) /* SELECTED Field IN COMBOBOX */
                     .Where(f => tv_r.Nodes
                        .OfType<TreeNode>()
                        .Where(fr => XDocument.Parse(fr.Tag.ToString()).Element("Field").Attribute("enName").Value ==
                                     f.Description)
                        .Count() == 0
                        ) /* CHECKED IF NOT EXISTS Field IN RIGHT TREEVIEW*/
                     .ToList()
                     .ForEach(fl =>
                     {
                        /* SET OrderIndex TO ANY FIELD FOR INSERT */
                        XDocument flxdoc = XDocument.Parse(string.Format(@"<Field id=""{0}"" enName=""{1}"" orderIndex=""""/>", fl.Value, fl.Description));
                        flxdoc.Element("Field").Attribute("orderIndex").SetValue(maxFOrderIndex(tv_r) + 1);
                        tv_r.Nodes.Add(
                           new TreeNode(fl.Description) { Tag = flxdoc, ImageIndex = 1, SelectedImageIndex = 1 });
                     });
            });
         tv_right.ExpandAll();
         #endregion         
      }

      private void sb_leave_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_orderindex.Visible = false;
         pn_edit.Visible = true;
         pn_transfer.Enabled = false;
         tv_right.CheckBoxes = true;
         rootTag = @"<Request type=""Leave"">{0}</Request>";
         globalFunctionCall = "Report.SetItemsInGroup";
         privilege = "<Privilege>35</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>
         (() =>
         {
            List<string> xmlTables = new List<string>();
            string xmlTable = string.Empty;
            List<string> xmlFields = new List<string>();
            tv_right.Nodes
               .OfType<TreeNode>()
               .Where(t => t.Checked)
               .ToList()
               .ForEach(t =>
               {
                  xmlTable = t.Tag.ToString();
                  xmlFields.Clear();
                  t.Nodes
                     .OfType<TreeNode>()
                     .Where(f => f.Checked)
                     .ToList()
                     .ForEach(f =>
                     {
                        xmlFields.Add(f.Tag.ToString());
                     });
                  xmlTables.Add(string.Format(xmlTable, string.Join("\n", xmlFields)));
               });
            return string.Format(@"<Group id=""{1}""/><Role id=""{2}""/><Filters>{0}</Filters>", string.Join("\n", xmlTables), tb_group.Tag, cb_roles.SelectedValue);
         });
      }

      private void sb_join_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_orderindex.Visible = false;
         pn_edit.Visible = true;
         pn_transfer.Enabled = false;
         tv_right.CheckBoxes = true;
         rootTag = @"<Request type=""Join"">{0}</Request>";
         globalFunctionCall = "Report.SetItemsInGroup";
         privilege = "<Privilege>36</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>
         (() =>
         {
            List<string> xmlTables = new List<string>();
            string xmlTable = string.Empty;
            List<string> xmlFields = new List<string>();
            tv_right.Nodes
               .OfType<TreeNode>()
               .Where(t => t.Checked)
               .ToList()
               .ForEach(t =>
               {
                  xmlTable = t.Tag.ToString();
                  xmlFields.Clear();
                  t.Nodes
                     .OfType<TreeNode>()
                     .Where(f => f.Checked)
                     .ToList()
                     .ForEach(f =>
                     {
                        xmlFields.Add(f.Tag.ToString());
                     });
                  xmlTables.Add(string.Format(xmlTable, string.Join("\n", xmlFields)));
               });
            return string.Format(@"<Group id=""{1}""/><Role id=""{2}""/><Filters>{0}</Filters>", string.Join("\n", xmlTables), tb_group.Tag, cb_roles.SelectedValue);
         });

         #region Show Leave Items
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 11 /* Execute LoadSavedIteminGroup */, SendType.SelfToUserInterface) { Input = "Leave" });
         #endregion
      }

      private void sb_active_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_orderindex.Visible = false;
         pn_edit.Visible = true;
         pn_transfer.Enabled = false;
         tv_right.CheckBoxes = true;
         rootTag = @"<Request type=""Active"">{0}</Request>";
         globalFunctionCall = "Report.SetItemsInGroup";
         privilege = "<Privilege>37</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>
         (() =>
         {
            List<string> xmlTables = new List<string>();
            string xmlTable = string.Empty;
            List<string> xmlFields = new List<string>();
            tv_right.Nodes
               .OfType<TreeNode>()
               .Where(t => t.Checked)
               .ToList()
               .ForEach(t =>
               {
                  xmlTable = t.Tag.ToString();
                  xmlFields.Clear();
                  t.Nodes
                     .OfType<TreeNode>()
                     .Where(f => f.Checked)
                     .ToList()
                     .ForEach(f =>
                     {
                        xmlFields.Add(f.Tag.ToString());
                     });
                  xmlTables.Add(string.Format(xmlTable, string.Join("\n", xmlFields)));
               });
            return string.Format(@"<Group id=""{1}""/><Role id=""{2}""/><Filters>{0}</Filters>", string.Join("\n", xmlTables), tb_group.Tag, cb_roles.SelectedValue);
         });

         #region Show Leave Items
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "SpecifyGroupItems", 11 /* Execute LoadSavedIteminGroup */, SendType.SelfToUserInterface) { Input = "Deactive" });
         #endregion
      }

      private void sb_deactive_Click(object sender, EventArgs e)
      {
         DisabledControls();
         LockForm();
         pn_command.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         pn_orderindex.Visible = false;
         pn_edit.Visible = true;
         pn_transfer.Enabled = false;
         tv_right.CheckBoxes = true;
         rootTag = @"<Request type=""Deactive"">{0}</Request>";
         globalFunctionCall = "Report.SetItemsInGroup";
         privilege = "<Privilege>38</Privilege><Sub_Sys>2</Sub_Sys>";

         XMLDATA = new Func<string>
         (() =>
         {
            List<string> xmlTables = new List<string>();
            string xmlTable = string.Empty;
            List<string> xmlFields = new List<string>();
            tv_right.Nodes
               .OfType<TreeNode>()
               .Where(t => t.Checked)
               .ToList()
               .ForEach(t =>
               {
                  xmlTable = t.Tag.ToString();
                  xmlFields.Clear();
                  t.Nodes
                     .OfType<TreeNode>()
                     .Where(f => f.Checked)
                     .ToList()
                     .ForEach(f =>
                     {
                        xmlFields.Add(f.Tag.ToString());
                     });
                  xmlTables.Add(string.Format(xmlTable, string.Join("\n", xmlFields)));
               });
            return string.Format(@"<Group id=""{1}""/><Role id=""{2}""/><Filters>{0}</Filters>", string.Join("\n", xmlTables), tb_group.Tag, cb_roles.SelectedValue);
         });
      }

      private void sb_ordered_Click(object sender, EventArgs e)
      {

      }

      private void sb_editds_Click(object sender, EventArgs e)
      {
         if(tv_right.Nodes.Count == 0 || tv_right.SelectedNode == null || tv_right.SelectedNode.Parent == null)
            return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 06 /* Execute DoWork4SpecifyFilter */, SendType.Self) { Input = XDocument.Parse(tv_right.SelectedNode.Tag.ToString()).Element("Field").Attribute("global").Value });
      }
      #endregion

   }
}
