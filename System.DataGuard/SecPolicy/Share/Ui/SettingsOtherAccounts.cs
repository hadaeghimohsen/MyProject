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
using DevExpress.XtraEditors;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsOtherAccounts : UserControl
   {
      public SettingsOtherAccounts()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         if (Tb_Master.SelectedTab == tp_001)
         {
            UserBs.DataSource = iProject.Users/*.Where(u => u.USERDB.ToUpper() != CurrentUser.ToUpper())*/;
            CreateUserMenu();
         }
         else if (Tb_Master.SelectedTab == tp_002 || Tb_Master.SelectedTab == tp_003 || Tb_Master.SelectedTab == tp_004 || Tb_Master.SelectedTab == tp_005)
         {
            int subsys = SubSysBs.Position;
            int role = RoleBs.Position;
            int privilege = PrivilegeBs.Position;
            int boxp = BoxBs.Position;
            int pbox = BoxPrivilegeBs.Position;
            SubSysBs.DataSource = iProject.Sub_Systems.Where(s => s.INST_STAT == "002");
            SubSysBs.Position = subsys;
            RoleBs.Position = role;
            PrivilegeBs.Position = privilege;
            BoxBs.Position = boxp;
            BoxPrivilegeBs.Position = pbox;
            if (Tb_Master.SelectedTab == tp_004 || Tb_Master.SelectedTab == tp_005)
            {
               int user = UserBs.Position;
               UserBs.DataSource = iProject.Users;//.Where(u => u.USERDB.ToUpper() != CurrentUser.ToUpper());
               UserBs.Position = user;
            }
         }
      }

      private void CreateUserMenu()
      {
         UserList_Flp.Controls.Clear();
         foreach (Data.User user in UserBs.List.OfType<Data.User>())
         {
            var simplebutton = new SimpleButton();

            simplebutton.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            simplebutton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
               | System.Windows.Forms.AnchorStyles.Right)));
            simplebutton.Appearance.BackColor = user.IsLock ? Color.Gainsboro : Color.SkyBlue;
            simplebutton.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            simplebutton.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            simplebutton.Appearance.Options.UseBackColor = true;
            simplebutton.Appearance.Options.UseFont = true;
            simplebutton.Appearance.Options.UseForeColor = true;
            simplebutton.Appearance.Options.UseTextOptions = true;
            simplebutton.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            simplebutton.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            simplebutton.Image = global::System.DataGuard.Properties.Resources.IMAGE_1384;
            simplebutton.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            simplebutton.Location = new System.Drawing.Point(530, 3);
            simplebutton.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            simplebutton.LookAndFeel.UseDefaultLookAndFeel = false;
            simplebutton.Name = "simpleButton2";
            simplebutton.Size = new System.Drawing.Size(169, 57);
            simplebutton.TabIndex = 3;
            simplebutton.Tag = user;
            simplebutton.Text = string.Format("<b><u>{0}</u></b><br><color=DimGray><size=9>{1}</size></color><br>", user.USERDB.ToLower(), user.TitleFa);
            simplebutton.Click += UserInfo_Click;
            UserList_Flp.Controls.Add(simplebutton);
         }
      }

      private void UserInfo_Click(object sender, EventArgs e)
      {
         var u = ((SimpleButton)sender).Tag as Data.User;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 27 /* Execute DoWork4SettingsOtherAccount */){Input = u.USERDB},
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccount", 10 /* Execute ActionCallWindow */)
               }
            )
         );
      }

      private void NewUserAccount_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 26 /* Execute DoWork4SettingsNewAccount */),                  
               }
            )
         );
      }

      private void SubSysBs_CurrentChanged(object sender, EventArgs e)
      {
         var subsys = SubSysBs.Current as Data.Sub_System;
         //BoxPrivilegeBs.DataSource = iProject.Box_Privileges.Where(bp => bp.SUB_SYS == subsys.SUB_SYS);

         if (Tb_Master.SelectedTab == tp_002)
         {
            BoxPrivilegeBs.List.Clear();
            switch(subsys.SUB_SYS)
            {
               case 0:
                  PrivilegeBs.DataSource = iProject.DataGuard_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 1:
                  PrivilegeBs.DataSource = iProject.ServiceDef_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 2:
                  PrivilegeBs.DataSource = iProject.Report_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 3:
                  PrivilegeBs.DataSource = iProject.Gas_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 4:
                  PrivilegeBs.DataSource = iProject.Global_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 5:
                  PrivilegeBs.DataSource = iProject.Scsc_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 6:
                  PrivilegeBs.DataSource = iProject.Sas_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 7:
                  PrivilegeBs.DataSource = iProject.Msgb_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 8:
                  // Supplies
                  break;
               case 9:
                  // Resident Complex
                  break;
               case 10:
                  PrivilegeBs.DataSource = iProject.ISP_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 11:
                  PrivilegeBs.DataSource = iProject.CRM_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
               case 12:
                  PrivilegeBs.DataSource = iProject.RoboTech_Privileges.Where(p => p.BOXP_BPID == null);
                  break;
            }
         }
         else if (Tb_Master.SelectedTab == tp_003 || Tb_Master.SelectedTab == tp_005)
         {
            switch (subsys.SUB_SYS)
            {
               case 0:
                  PrivilegeBs.DataSource = iProject.DataGuard_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 1:
                  PrivilegeBs.DataSource = iProject.ServiceDef_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 2:
                  PrivilegeBs.DataSource = iProject.Report_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 3:
                  PrivilegeBs.DataSource = iProject.Gas_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 4:
                  PrivilegeBs.DataSource = iProject.Global_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 5:
                  PrivilegeBs.DataSource = iProject.Scsc_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 6:
                  PrivilegeBs.DataSource = iProject.Sas_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 7:
                  PrivilegeBs.DataSource = iProject.Msgb_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 8:
                  // Supplies
                  break;
               case 9:
                  // Resident Complex
                  break;
               case 10:
                  PrivilegeBs.DataSource = iProject.ISP_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 11:
                  PrivilegeBs.DataSource = iProject.CRM_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
               case 12:
                  PrivilegeBs.DataSource = iProject.RoboTech_Privileges.Where(p => p.BOXP_BPID != null);
                  break;
            }
         }
      }

      private void AddBoxPrivilege_Butn_Click(object sender, EventArgs e)
      {
         BoxBs.AddNew();
      }

      private void DeleteBoxPrivilege_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var box = BoxBs.Current as Data.Box_Privilege;
            if (box == null && MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "حذف جعبه های دسترسی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.DeleteBoxPrivilege(
               new XElement("Box_Privilege",
                  new XAttribute("bpid", box.BPID),
                  new XAttribute("subsys", box.SUB_SYS)                  
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally { 
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void SaveBoxPrivilege_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            BoxBs.EndEdit();

            var box = BoxBs.Current as Data.Box_Privilege;
            if (box.BOXP_DESC == "") return;

            iProject.CreateOrReplaceBoxPrivilege(
               new XElement("Box_Privilege",
                  new XAttribute("bpid", box.BPID),
                  new XAttribute("subsys", box.SUB_SYS),
                  new XAttribute("boxpdesc", box.BOXP_DESC)
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {            
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void BoxBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var box = BoxBs.Current as Data.Box_Privilege;
            if (box == null)
               return;

            if (Tb_Master.SelectedTab == tp_002)
            {
               switch (box.SUB_SYS)
               {
                  case 0:
                     BoxPrivilegeBs.DataSource = iProject.DataGuard_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 1:
                     BoxPrivilegeBs.DataSource = iProject.ServiceDef_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 2:
                     BoxPrivilegeBs.DataSource = iProject.Report_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 3:
                     BoxPrivilegeBs.DataSource = iProject.Gas_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 4:
                     BoxPrivilegeBs.DataSource = iProject.Global_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 5:
                     BoxPrivilegeBs.DataSource = iProject.Scsc_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 6:
                     BoxPrivilegeBs.DataSource = iProject.Sas_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 7:
                     BoxPrivilegeBs.DataSource = iProject.Msgb_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 8:
                     // Supplies
                     break;
                  case 9:
                     // Resident Complex
                     break;
                  case 10:
                     BoxPrivilegeBs.DataSource = iProject.ISP_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 11:
                     BoxPrivilegeBs.DataSource = iProject.CRM_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
                  case 12:
                     BoxPrivilegeBs.DataSource = iProject.RoboTech_Privileges.Where(p => p.BOXP_BPID == box.BPID);
                     break;
               }
            }
         }
         catch (Exception)
         {            
         }         
      }

      private void AddPrivilegeToBox_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic p = null;
            var subsys = SubSysBs.Current as Data.Sub_System;

            switch (subsys.SUB_SYS)
            {
               case 0:
                  p = PrivilegeBs.Current as Data.DataGuard_Privilege;
                  break;
               case 1:
                  p = PrivilegeBs.Current as Data.ServiceDef_Privilege;
                  break;
               case 2:
                  p = PrivilegeBs.Current as Data.Report_Privilege;
                  break;
               case 3:
                  p = PrivilegeBs.Current as Data.Gas_Privilege;
                  break;
               case 4:
                  p = PrivilegeBs.Current as Data.Global_Privilege;
                  break;
               case 5:
                  p = PrivilegeBs.Current as Data.Scsc_Privilege;
                  break;
               case 6:
                  p = PrivilegeBs.Current as Data.Sas_Privilege;
                  break;
               case 7:
                  p = PrivilegeBs.Current as Data.Msgb_Privilege;
                  break;
               case 8:
                  // Supplies
                  break;
               case 9:
                  // Resident Complex
                  break;
               case 10:
                  p = PrivilegeBs.Current as Data.ISP_Privilege;
                  break;
               case 11:
                  p = PrivilegeBs.Current as Data.CRM_Privilege;
                  break;
               case 12:
                  p = PrivilegeBs.Current as Data.RoboTech_Privilege;
                  break;
            }

            if (p == null) return;

            var b = BoxBs.Current as Data.Box_Privilege;
            if (b == null) return;

            iProject.UpdatePrivilegeToBox(
               new XElement("PrivilegeToBox",
                  new XAttribute("bpid", b.BPID),
                  new XAttribute("privilegeid", p.ID),
                  new XAttribute("subsys", b.SUB_SYS)
               )
            );
            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void DeletePrivilegeFromBox_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            dynamic p = null;
            var subsys = SubSysBs.Current as Data.Sub_System;

            switch (subsys.SUB_SYS)
            {
               case 0:
                  p = BoxPrivilegeBs.Current as Data.DataGuard_Privilege;
                  break;
               case 1:
                  p = BoxPrivilegeBs.Current as Data.ServiceDef_Privilege;
                  break;
               case 2:
                  p = BoxPrivilegeBs.Current as Data.Report_Privilege;
                  break;
               case 3:
                  p = BoxPrivilegeBs.Current as Data.Gas_Privilege;
                  break;
               case 4:
                  p = BoxPrivilegeBs.Current as Data.Global_Privilege;
                  break;
               case 5:
                  p = BoxPrivilegeBs.Current as Data.Scsc_Privilege;
                  break;
               case 6:
                  p = BoxPrivilegeBs.Current as Data.Sas_Privilege;
                  break;
               case 7:
                  p = BoxPrivilegeBs.Current as Data.Msgb_Privilege;
                  break;
               case 8:
                  // Supplies
                  break;
               case 9:
                  // Resident Complex
                  break;
               case 10:
                  p = BoxPrivilegeBs.Current as Data.ISP_Privilege;
                  break;
               case 11:
                  p = BoxPrivilegeBs.Current as Data.CRM_Privilege;
                  break;
               case 12:
                  p = BoxPrivilegeBs.Current as Data.RoboTech_Privilege;
                  break;
            }

            if (p == null) return;
            if (p != null && MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "حذف دسترسی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.UpdatePrivilegeToBox(
               new XElement("PrivilegeToBox",
                  new XAttribute("bpid", ""),
                  new XAttribute("privilegeid", p.ID),
                  new XAttribute("subsys", p.Box_Privilege.SUB_SYS)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void RoleBs_CurrentChanged(object sender, EventArgs e)
      {
         
         if (Tb_Master.SelectedTab == tp_003)
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var role = RoleBs.Current as Data.Role;
            if (role == null) return;

            RolePrivilegeBs.Clear();

            RolePrivilegeBs.DataSource = iProject.Role_Privileges.Where(rp => rp.Sub_Sys == subsys.SUB_SYS && rp.RoleID == role.ID && rp.IsVisible);
         }
      }

      private void AddNewRole_Butn_Click(object sender, EventArgs e)
      {
         RoleBs.AddNew();
      }

      private void DeleteRole_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var role = RoleBs.Current as Data.Role;
            if (role == null) return;

            if (role != null && MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "حذف گروه دسترسی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iProject.IUDRole(
               new XElement("Role",
                  new XAttribute("id", role.ID),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", "003")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void SaveRole_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var role = RoleBs.Current as Data.Role;
            if (role == null || role.TitleFa == "") return;

            iProject.IUDRole(
               new XElement("Role",
                  new XAttribute("id", role.ID),
                  new XAttribute("name", role.TitleFa),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", role.ID == 0 ? "001" : "002")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void EnableDisabledRole_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var role = RoleBs.Current as Data.Role;
            if (role == null ) return;

            role.IsActive = role.IsActive == null ? true : role.IsActive;

            iProject.IUDRole(
               new XElement("Role",
                  new XAttribute("id", role.ID),
                  new XAttribute("name", role.TitleFa),
                  new XAttribute("active", (bool)role.IsActive ? false : true),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", role.ID == 0 ? "001" : "002")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void GrantBoxPrivilegeToRole_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var role = RoleBs.Current as Data.Role;
            if (role == null) return;

            var boxp = BoxBs.Current as Data.Box_Privilege;
            if (boxp == null) return;

            iProject.GRRole(
               new XElement("BoxPrivilege",
                  new XAttribute("bpid", boxp.BPID),
                  new XAttribute("roleid", role.ID),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", "001")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void RevokeBoxPrivilegeFromRole_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var role = RoleBs.Current as Data.Role;
            if (role == null) return;

            var boxp = BoxBs.Current as Data.Box_Privilege;
            if (boxp == null) return;

            iProject.GRRole(
               new XElement("BoxPrivilege",
                  new XAttribute("bpid", boxp.BPID),
                  new XAttribute("roleid", role.ID),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", "002")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void EnableDisabledRolePrivilege_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var roleprivilege = RolePrivilegeBs.Current as Data.Role_Privilege;
            if (roleprivilege == null) return;

            if(roleprivilege.IsActive)
               iProject.DeactivePrivilegesToRole(
                  new XElement("Deactive",
                     new XElement("RoleID", roleprivilege.RoleID),
                     new XElement("Privilege", 
                        new XElement("PrivilegeID", roleprivilege.PrivilegeID)
                     )
                  )
               );
            else
               iProject.ActivePrivilegesInRole(
                  new XElement("Active",
                     new XElement("RoleID", roleprivilege.RoleID),
                     new XElement("Privilege",
                        new XElement("PrivilegeID", roleprivilege.PrivilegeID)
                     )
                  )
               );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void GrantRoleUser_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var role = RoleBs.Current as Data.Role;
            if (role == null) return;

            var user = UserBs.Current as Data.User;
            if (user == null) return;

            iProject.JoinUserToRoles(
               new XElement("Join",
                  new XElement("UserID", user.ID),
                  new XElement("Roles",
                     new XElement("RoleID", role.ID)
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void EnableDisabledRoleUser_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var roleuser = RoleUserBs.Current as Data.Role_User;
            if (roleuser == null) return;

            if(roleuser.IsVisible)
               iProject.LeaveUserFromRoles(
                  new XElement("Leave",
                     new XElement("UserID", roleuser.UserID),
                     new XElement("Roles",
                        new XElement("RoleID", roleuser.RoleID)
                     )
                  )
               );
            else
               iProject.JoinUserToRoles(
                  new XElement("Join",
                     new XElement("UserID", roleuser.UserID),
                     new XElement("Roles",
                        new XElement("RoleID", roleuser.RoleID)
                     )
                  )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void GrantBoxPrivilegeToUser_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var user = UserBs.Current as Data.User;
            if (user == null) return;

            var boxp = BoxPrivilegeBs.Current as Data.Box_Privilege;
            if (boxp == null) return;

            iProject.GRUser(
               new XElement("BoxPrivilege",
                  new XAttribute("bpid", boxp.BPID),
                  new XAttribute("userid", user.ID),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", "001")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void RevokeBoxPrivilegeFromUser_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var subsys = SubSysBs.Current as Data.Sub_System;
            if (subsys == null) return;

            var user = UserBs.Current as Data.User;
            if (user == null) return;

            var boxp = BoxPrivilegeBs.Current as Data.Box_Privilege;
            if (boxp == null) return;

            iProject.GRUser(
               new XElement("BoxPrivilege",
                  new XAttribute("bpid", boxp.BPID),
                  new XAttribute("userid", user.ID),
                  new XAttribute("subsys", subsys.SUB_SYS),
                  new XAttribute("actiontype", "002")
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void UserBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {

            if (Tb_Master.SelectedTab == tp_005)
            {
               var subsys = SubSysBs.Current as Data.Sub_System;
               if (subsys == null) return;

               var user = UserBs.Current as Data.User;
               if (user == null) return;

               UserPrivilegeBs.Clear();
               UserPrivilegeBs.DataSource = iProject.User_Privileges.Where(up => up.Sub_Sys == subsys.SUB_SYS && up.UserID == user.ID && up.IsVisible);
            }
         }
         catch { }
      }

      private void EnableDisabledUserPrivilege_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var userprivilege = UserPrivilegeBs.Current as Data.User_Privilege;
            if (userprivilege == null) return;

            if (userprivilege.IsActive)
               iProject.DeactivePrivilegesToUser(
                  new XElement("Deactive",
                     new XElement("UserID", userprivilege.UserID),
                     new XElement("Privilege",
                        new XElement("PrivilegeID", userprivilege.PrivilegeID)
                     )
                  )
               );
            else
               iProject.ActivePrivilegesInRole(
                  new XElement("Active",
                     new XElement("UserID", userprivilege.UserID),
                     new XElement("Privilege",
                        new XElement("PrivilegeID", userprivilege.PrivilegeID)
                     )
                  )
               );

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

   }
}
