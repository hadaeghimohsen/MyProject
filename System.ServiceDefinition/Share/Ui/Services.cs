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

namespace System.ServiceDefinition.Share.Ui
{
   public partial class Services : UserControl
   {
      public Services()
      {
         InitializeComponent();

      }

      private string rootTag;
      private string globalFunctionCall;
      private string privilege;
      private Func<string> XmlData;

      private void sb_rolesettings_Click(object sender, EventArgs e)
      {
         Job _RoleSettings4CurrentUser =
            new Job(SendType.External, "ServiceDefinition",
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

      private void sb_grphdrsettings_Click(object sender, EventArgs e)
      {
         Job _GroupHeaderSetting4CurrentUser =
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  new Job(SendType.External, "GroupHeader",
                     new List<Job>
                     {
                        new Job(SendType.Self, 03 /* Execute DoWork4GroupHeaderSettings4CurrentUser */)
                     })
               });
         _DefaultGateway.Gateway(_GroupHeaderSetting4CurrentUser);
      }

      #region Parent Service Command Menu
      private void sb_creategrpsrv_Click(object sender, EventArgs e)
      {
         #region InterFunction
         Func<string> GetParentName = new Func<string>(
         () =>
         {
            if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
               return "به عنوان گروه خدمت سطح یک سرفصل";
            else
               return tv_grpsrv.SelectedNode.Text;            
         });

         Func<string> GetParentId = new Func<string>(
            () =>
            {
               if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
                  return "";
               else
                  return tv_grpsrv.SelectedNode.Tag.ToString();
            });
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        new Job(SendType.Self, 03 /* Execute DoWork4CreateNew */)
                        {
                           Input = new List<object>
                           {
                              1,
                              GetParentName(),
                              GetParentId(),
                              ccbe_groupheader
                           }
                        }
                     })
               }));
      }

      private void sb_updategrpsrv_Click(object sender, EventArgs e)
      {
         if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
            return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        new Job(SendType.Self, 05 /* Execute DoWork4Update */)
                        {
                           Input = new List<object>
                           {
                              1,
                              tv_grpsrv.SelectedNode.Tag,
                              ccbe_groupheader
                           }
                        }
                     })
               }));
      }

      private void sb_deactivegrpsrv_Click(object sender, EventArgs e)
      {
         rootTag = "<Deactive>{0}</Deactive>";
         globalFunctionCall = "ServiceDef.DeactiveGrpSrv";
         privilege = "<Privilege>14</Privilege><Sub_Sys>1</Sub_Sys>";
         tv_grpsrv.CheckBoxes = true;
         pn_editgrpsrv.Visible = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);
         
         string xmldata = "";
         #region XmlData
         Action<TreeNode> PrepareSubXmlData = null;
         Action<TreeNode> GetSubXmlData = new Action<TreeNode>(
            (node) =>
               {
                  node.Nodes
                     .OfType<TreeNode>()
                     .ToList()
                     .ForEach(snode =>
                        {
                           if(snode.Checked)
                              xmldata += string.Format("<GrpSrv>{0}</GrpSrv>",snode.Tag);
                           if(snode.Nodes.Count > 0)
                              PrepareSubXmlData(snode);
                        });
                  
               });
         PrepareSubXmlData = new Action<TreeNode>((node) => { GetSubXmlData(node); });
         XmlData = new Func<string>(
            () =>
            {
               tv_grpsrv.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(node =>
                     {
                        if (node.Checked)
                           xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", node.Tag);
                        if (node.Nodes.Count > 0)
                           GetSubXmlData(node);
                     });
               return xmldata;
            }
         );
#        endregion
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 12 /* Execute LoadGrpSrvWithCondition */, SendType.SelfToUserInterface) { Input = "<WhatAction>true</WhatAction>" });
      }

      private void sb_activegrpsrv_Click(object sender, EventArgs e)
      {
         rootTag = "<Active>{0}</Active>";
         globalFunctionCall = "ServiceDef.ActiveGrpSrv";
         privilege = "<Privilege>15</Privilege><Sub_Sys>1</Sub_Sys>";
         tv_grpsrv.CheckBoxes = true;
         pn_editgrpsrv.Visible = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         Action<TreeNode> PrepareSubXmlData = null;
         Action<TreeNode> GetSubXmlData = new Action<TreeNode>(
            (node) =>
            {
               node.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(snode =>
                  {
                     if (snode.Checked)
                        xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", snode.Tag);
                     if (snode.Nodes.Count > 0)
                        PrepareSubXmlData(snode);
                  });

            });
         PrepareSubXmlData = new Action<TreeNode>((node) => { GetSubXmlData(node); });
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               tv_grpsrv.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(node =>
                  {
                     if (node.Checked)
                        xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", node.Tag);
                     if (node.Nodes.Count > 0)
                        GetSubXmlData(node);
                  });
               return xmldata;
            }
         );
         #        endregion
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 12 /* Execute LoadGrpSrvWithCondition */, SendType.SelfToUserInterface) { Input = "<WhatAction>false</WhatAction>" });
      }

      private void sb_joingrpsrv_Click(object sender, EventArgs e)
      {
         rootTag = "<Join>{0}</Join>";
         globalFunctionCall = "ServiceDef.JoinGroupServiceIntoGroupHeader";
         privilege = "<Privilege>16</Privilege><Sub_Sys>1</Sub_Sys>";
         tv_grpsrv.CheckBoxes = true;
         pn_editgrpsrv.Visible = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         Action<TreeNode> PrepareSubXmlData = null;
         Action<TreeNode> GetSubXmlData = new Action<TreeNode>(
            (node) =>
            {
               node.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(snode =>
                  {
                     if (snode.Checked)
                        xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", snode.Tag);
                     if (snode.Nodes.Count > 0)
                        PrepareSubXmlData(snode);
                  });

            });
         PrepareSubXmlData = new Action<TreeNode>((node) => { GetSubXmlData(node); });
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               tv_grpsrv.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(node =>
                  {                     
                     xmldata += string.Format(@"<GroupHeader ID=""{0}"">", node.Tag);
                     if (node.Nodes.Count > 0)
                        GetSubXmlData(node);
                     xmldata += "</GroupHeader>";
                  });
               return xmldata;
            }
         );
         #endregion

         Func<string> XmlCondition = new Func<string>(
            () =>
               {
                  string xmlcondition = "";
                  ccbe_groupheader.Properties.GetItems()
                     .OfType<CheckedListBoxItem>()
                     .Where(gh => gh.CheckState == CheckState.Checked)
                     .ToList()
                     .ForEach(gh => { xmlcondition += string.Format("<GhID>{0}</GhID>", gh.Value); });
                  return string.Format("<GroupHeader>{0}</GroupHeader>", xmlcondition);
               });
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 13 /* Execute LoadGrpSrvForJoin */, SendType.SelfToUserInterface) { Input =  XmlCondition() });
      }

      private void sb_leavegrpsrv_Click(object sender, EventArgs e)
      {
         rootTag = "<Leave>{0}</Leave>";
         globalFunctionCall = "ServiceDef.LeaveGroupServiceFromGroupHeader";
         privilege = "<Privilege>17</Privilege><Sub_Sys>1</Sub_Sys>";
         tv_grpsrv.CheckBoxes = true;
         pn_editgrpsrv.Visible = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         Action<TreeNode> PrepareSubXmlData = null;
         Action<TreeNode> GetSubXmlData = new Action<TreeNode>(
            (node) =>
            {
               node.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(snode =>
                  {
                     if (snode.Checked)
                        xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", snode.Tag);
                     if (snode.Nodes.Count > 0)
                        PrepareSubXmlData(snode);
                  });

            });
         PrepareSubXmlData = new Action<TreeNode>((node) => { GetSubXmlData(node); });
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               tv_grpsrv.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(node =>
                  {
                     xmldata += string.Format(@"<GroupHeader ID=""{0}"">", node.ToolTipText);
                     if (node.Nodes.Count > 0)
                        GetSubXmlData(node);
                     xmldata += "</GroupHeader>";
                  });
               return xmldata;
            }
         );
         #endregion
      }

      private void sb_activegrpsrvingrphdr_Click(object sender, EventArgs e)
      {
         rootTag = "<Active>{0}</Active>";
         globalFunctionCall = "ServiceDef.ActiveGrpSrvInGrpHdr";
         privilege = "<Privilege>18</Privilege><Sub_Sys>1</Sub_Sys>";
         tv_grpsrv.CheckBoxes = true;
         pn_editgrpsrv.Visible = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         Action<TreeNode> PrepareSubXmlData = null;
         Action<TreeNode> GetSubXmlData = new Action<TreeNode>(
            (node) =>
            {
               node.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(snode =>
                  {
                     if (snode.Checked)
                        xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", snode.Tag);
                     if (snode.Nodes.Count > 0)
                        PrepareSubXmlData(snode);
                  });

            });
         PrepareSubXmlData = new Action<TreeNode>((node) => { GetSubXmlData(node); });
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               tv_grpsrv.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(node =>
                  {
                     xmldata += string.Format(@"<GroupHeader ID=""{0}"">", node.Tag);
                     if (node.Nodes.Count > 0)
                        GetSubXmlData(node);
                     xmldata += "</GroupHeader>";
                  });
               return xmldata;
            }
         );
         #endregion

         Func<string> XmlCondition = new Func<string>(
            () =>
            {
               string xmlcondition = "";
               ccbe_groupheader.Properties.GetItems()
                  .OfType<CheckedListBoxItem>()
                  .Where(gh => gh.CheckState == CheckState.Checked)
                  .ToList()
                  .ForEach(gh => { xmlcondition += string.Format("<GhID>{0}</GhID>", gh.Value); });
               return string.Format("<GroupHeader><WhatAction>false</WhatAction>{0}</GroupHeader>", xmlcondition);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 14 /* Execute LoadGrpSrvInGrpHdrWithCondition */, SendType.SelfToUserInterface) { Input = XmlCondition() });
      }

      private void sb_deactivegrpsrvingrphdr_Click(object sender, EventArgs e)
      {
         rootTag = "<Deactive>{0}</Deactive>";
         globalFunctionCall = "ServiceDef.DeactiveGrpSrvFromGrpHdr";
         privilege = "<Privilege>19</Privilege><Sub_Sys>1</Sub_Sys>";
         tv_grpsrv.CheckBoxes = true;
         pn_editgrpsrv.Visible = true;
         pn_commandgrpsrv.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         Action<TreeNode> PrepareSubXmlData = null;
         Action<TreeNode> GetSubXmlData = new Action<TreeNode>(
            (node) =>
            {
               node.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(snode =>
                  {
                     if (snode.Checked)
                        xmldata += string.Format("<GrpSrv>{0}</GrpSrv>", snode.Tag);
                     if (snode.Nodes.Count > 0)
                        PrepareSubXmlData(snode);
                  });

            });
         PrepareSubXmlData = new Action<TreeNode>((node) => { GetSubXmlData(node); });
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               tv_grpsrv.Nodes
                  .OfType<TreeNode>()
                  .ToList()
                  .ForEach(node =>
                  {
                     xmldata += string.Format(@"<GroupHeader ID=""{0}"">", node.Tag);
                     if (node.Nodes.Count > 0)
                        GetSubXmlData(node);
                     xmldata += "</GroupHeader>";
                  });
               return xmldata;
            }
         );
         #endregion

         Func<string> XmlCondition = new Func<string>(
            () =>
            {
               string xmlcondition = "";
               ccbe_groupheader.Properties.GetItems()
                  .OfType<CheckedListBoxItem>()
                  .Where(gh => gh.CheckState == CheckState.Checked)
                  .ToList()
                  .ForEach(gh => { xmlcondition += string.Format("<GhID>{0}</GhID>", gh.Value); });
               return string.Format("<GroupHeader><WhatAction>true</WhatAction>{0}</GroupHeader>", xmlcondition);
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 14 /* Execute LoadGrpSrvInGrpHdrWithCondition */, SendType.SelfToUserInterface) { Input = XmlCondition() });
      }
      #endregion

      #region Parent Service Edit Menu
      private void sb_cancelparent_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "Services", 10 /* Execute LoadParentServicesOfGroupHeaders */)
                  }));
      }

      private void sb_submitparent_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
           new Job(SendType.External, "ServiceDefinition",
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
                                                   _DefaultGateway.Gateway(new Job(SendType.External, "ServiceDefinition", "Services", 10 /* Execute LoadParentServicesOfGroupHeaders */, SendType.SelfToUserInterface));
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
                              string.Format(rootTag, XmlData()),
                              string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                              "iProject",
                              "scott"
                           }
                        }
                        #endregion
                        
                     }),
                  new Job(SendType.SelfToUserInterface, "Services", 10 /* Execute LoadParentServicesOfGroupHeaders */)
               }));
      }

      private void sb_deselectparent_Click(object sender, EventArgs e)
      {

      }

      private void sb_selectinvertparent_Click(object sender, EventArgs e)
      {

      }

      private void sb_selectallparent_Click(object sender, EventArgs e)
      {

      }
      #endregion            

      #region Service Command Menu
      private void sb_createsrv_Click(object sender, EventArgs e)
      {
         #region InterFunction
         Func<string> GetParentName = new Func<string>(
         () =>
         {
            if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
               return "به عنوان گروه خدمت سطح یک سرفصل";
            else
               return tv_grpsrv.SelectedNode.Text;
         });

         Func<string> GetParentId = new Func<string>(
            () =>
            {
               if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
                  return "";
               else
                  return tv_grpsrv.SelectedNode.Tag.ToString();
            });
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        new Job(SendType.Self, 03 /* Execute DoWork4CreateNew */)
                        {
                           Input = new List<object>
                           {
                              0,
                              GetParentName(),
                              GetParentId(),
                              ccbe_groupheader
                           }
                        }
                     })
               }));
      }

      private void sb_updatesrv_Click(object sender, EventArgs e)
      {
         if (lv_services.SelectedItems.Count != 1)
            return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
               {
                  new Job(SendType.External, "Service",
                     new List<Job>
                     {
                        new Job(SendType.Self, 05 /* Execute DoWork4Update */)
                        {
                           Input = new List<object>
                           {
                              0,
                              lv_services.SelectedItems[0].Tag,
                              ccbe_groupheader
                           }
                        }
                     })
               }));
      }

      private void sb_disabledsrv_Click(object sender, EventArgs e)
      {
         rootTag = "<Disabled>{0}</Disabled>";
         globalFunctionCall = "ServiceDef.DisbaledService";
         privilege = "<Privilege>28</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_services.CheckBoxes = true;
         pn_editservice.Visible = true;
         pn_commandservice.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               lv_services.CheckedItems.OfType<ListViewItem>().ToList().ForEach(s => xmldata +=  s.Tag.ToString() );
               return xmldata;
            });
         #endregion
      }

      private void sb_enabledsrv_Click(object sender, EventArgs e)
      {
         if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
            return;

         rootTag = "<Enabled>{0}</Enabled>";
         globalFunctionCall = "ServiceDef.EnabledService";
         privilege = "<Privilege>29</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_services.CheckBoxes = true;
         pn_editservice.Visible = true;
         pn_commandservice.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               lv_services.CheckedItems.OfType<ListViewItem>().ToList().ForEach(s => xmldata += s.Tag.ToString());
               return xmldata;
            });
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 15 /* Execute LoadServiceInGrpHdrWithCondition */, SendType.SelfToUserInterface) { Input = string.Format("<GrpSrv>{0}</GrpSrv>", tv_grpsrv.SelectedNode.Tag)});
      }

      private void sb_joinsrv_Click(object sender, EventArgs e)
      {
         if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
            return;

         rootTag = "<Join>{0}</Join>";
         globalFunctionCall = "ServiceDef.JoinService";
         privilege = "<Privilege>30</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_services.CheckBoxes = true;
         pn_editservice.Visible = true;
         pn_commandservice.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               lv_services.CheckedItems.OfType<ListViewItem>().ToList().ForEach(s => xmldata += s.Tag.ToString());
               return string.Format("<GrpSrv>{0}</GrpSrv>{1}", tv_grpsrv.SelectedNode.Tag, xmldata);
            });
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 16 /* Execute LoadJoinServiceWithCondition */, SendType.SelfToUserInterface));
      }

      private void sb_leavesrv_Click(object sender, EventArgs e)
      {
         rootTag = "<Leave>{0}</Leave>";
         globalFunctionCall = "ServiceDef.LeaveService";
         privilege = "<Privilege>31</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_services.CheckBoxes = true;
         pn_editservice.Visible = true;
         pn_commandservice.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               lv_services.CheckedItems.OfType<ListViewItem>().ToList().ForEach(s => xmldata += s.Tag.ToString());
               return xmldata;
            });
         #endregion

      }

      private void sb_activeservice_Click(object sender, EventArgs e)
      {
         if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
            return;

         rootTag = "<Active>{0}</Active>";
         globalFunctionCall = "ServiceDef.ActiveService";
         privilege = "<Privilege>32</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_services.CheckBoxes = true;
         pn_editservice.Visible = true;
         pn_commandservice.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               lv_services.CheckedItems.OfType<ListViewItem>().ToList().ForEach(s => xmldata += s.Tag.ToString());
               return xmldata;
            });
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 17 /* Execute LoadServicesWithCondition */, SendType.SelfToUserInterface));
      }

      private void sb_deactiveservice_Click(object sender, EventArgs e)
      {
         if (tv_grpsrv.SelectedNode == null || tv_grpsrv.SelectedNode.Tag == null)
            return;

         rootTag = "<Deactive>{0}</Deactive>";
         globalFunctionCall = "ServiceDef.DeactiveService";
         privilege = "<Privilege>33</Privilege><Sub_Sys>1</Sub_Sys>";
         lv_services.CheckBoxes = true;
         pn_editservice.Visible = true;
         pn_commandservice.Controls.OfType<Control>().Where(c => c != (Control)sender).ToList().ForEach(c => c.Enabled = false);

         string xmldata = "";
         #region XmlData
         XmlData = new Func<string>(
            () =>
            {
               xmldata = "";
               lv_services.CheckedItems.OfType<ListViewItem>().ToList().ForEach(s => xmldata += s.Tag.ToString());
               return xmldata;
            });
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 17 /* Execute LoadServicesWithCondition */, SendType.SelfToUserInterface));
      }

      #endregion

      #region Service Edit Menu
      private void sb_cancelservice_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition",
               new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "Services", 11 /* Execute LoadServicesOfParentService */)
                  }));
      }

      private void sb_submitservice_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
              new Job(SendType.External, "ServiceDefinition",
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
                                 string.Format(rootTag, XmlData()),
                                 string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                                 "iProject",
                                 "scott"
                              }
                           }
                           #endregion                        
                        }),
                     new Job(SendType.SelfToUserInterface, "Services", 11 /* Execute LoadServicesOfParentService */)
                  }));
      }

      private void sb_deselectservice_Click(object sender, EventArgs e)
      {
         lv_services.Items.OfType<ListViewItem>().ToList().ForEach(s => s.Checked = false);
      }

      private void sb_invertselectservice_Click(object sender, EventArgs e)
      {
         lv_services.Items.OfType<ListViewItem>().ToList().ForEach(s => s.Checked = !s.Checked);
      }

      private void sb_selectallservice_Click(object sender, EventArgs e)
      {
         lv_services.Items.OfType<ListViewItem>().ToList().ForEach(s => s.Checked = true);
      }
      #endregion

      #region Events
      private void ccbe_roles_EditValueChanged(object sender, EventArgs e)
      {         
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 09 /* Execute LoadGroupHeadersOfRoles */,SendType.SelfToUserInterface));
      }

      private void ccbe_groupheader_EditValueChanged(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 10 /* Execute LoadParentServicesOfGroupHeaders */, SendType.SelfToUserInterface));
      }

      private void tv_grpsrv_AfterSelect(object sender, TreeViewEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "ServiceDefinition", "Services", 11 /* Execute LoadServicesOfParentService */, SendType.SelfToUserInterface));
      }
      #endregion

      #region Drag And Drop 4 TreeView
      private void tv_grpsrv_ItemDrag(object sender, ItemDragEventArgs e)
      {
         //if (((TreeNode)e.Item).Tag == null)
         //   return;

         DoDragDrop(e.Item, DragDropEffects.Move);
      }

      private void tv_grpsrv_DragEnter(object sender, DragEventArgs e)
      {
         e.Effect = DragDropEffects.Move;
      }

      private void tv_grpsrv_DragOver(object sender, DragEventArgs e)
      {
         e.Effect = DragDropEffects.Move;
      }

      private void tv_grpsrv_DragDrop(object sender, DragEventArgs e)
      {
         TreeNode dragNode;
         if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
         {
            Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            TreeNode desNode = ((TreeView)sender).GetNodeAt(pt);
            dragNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

            if (dragNode.Tag == null)
               return;

            if (dragNode.Nodes.Contains(desNode))
               return;

            if (desNode != null)
            {
               rootTag = "<ChangeParent>{0}</ChangeParent>";
               privilege = "<Privilege>34</Privilege><Sub_Sys>1</Sub_Sys>";
               globalFunctionCall = "ServiceDef.ChangeParentOfService";
               XmlData = new Func<string>(
                  () =>
                  {
                     return string.Format("<ParentID>{0}</ParentID><ServiceID>{1}</ServiceID>", desNode.Tag == null ? 0 : desNode.Tag, dragNode.Tag);
                  });
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Loaclhost",
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
                                       true,
                                       "xml",
                                       string.Format(rootTag, XmlData()),
                                       string.Format("{{ Call {0}(?) }}", globalFunctionCall),
                                       "iProject",
                                       "scott"
                                    },
                                    AfterChangedOutput = new Action<object>(
                                       (output) =>
                                       {
                                          dragNode.Remove();
                                          desNode.Nodes.Add(dragNode);
                                          desNode.ExpandAll();
                                       })
                                 },
                                 #endregion                        
                              })
                        
                     }));
            }            
         }
      }
      #endregion


   }
}
