namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SecuritySettings
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("گروه های دسترسی");
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecuritySettings));
         this.tv_roles = new System.Windows.Forms.TreeView();
         this.il_icons = new System.Windows.Forms.ImageList();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.tc_role = new System.Windows.Forms.TabControl();
         this.tp_privilege = new System.Windows.Forms.TabPage();
         this.pn_editprivilege = new System.Windows.Forms.Panel();
         this.sb_selectallprivileges = new DevExpress.XtraEditors.SimpleButton();
         this.sb_selectinvertprivileges = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deselectprivileges = new DevExpress.XtraEditors.SimpleButton();
         this.sb_submitprivilegechange = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancelprivilegechange = new DevExpress.XtraEditors.SimpleButton();
         this.sb_activeprivilege = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deactiveprivilege = new DevExpress.XtraEditors.SimpleButton();
         this.sb_removeprivilege = new DevExpress.XtraEditors.SimpleButton();
         this.sb_addprivilege = new DevExpress.XtraEditors.SimpleButton();
         this.lv_privileges = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.textBox2 = new System.Windows.Forms.TextBox();
         this.tp_users = new System.Windows.Forms.TabPage();
         this.pn_01 = new System.Windows.Forms.Panel();
         this.sb_activeprivilegetouser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_grantprivilegetouser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_revokeprivilegefromuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deactiveprivilegefromuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_usersinglepropertymenu = new DevExpress.XtraEditors.SimpleButton();
         this.pn_edituserprivilege = new System.Windows.Forms.Panel();
         this.sb_selectallprivilegeforuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_invertselectprivielegeforuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deselectprivilegeforuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_submitprivilegechangeforuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancelprivielegechangeforuser = new DevExpress.XtraEditors.SimpleButton();
         this.lv_userprivileges = new System.Windows.Forms.ListView();
         this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.pn_edituser = new System.Windows.Forms.Panel();
         this.sb_selectalluser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_invertselectuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deselectuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_submituserchange = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancelchangeuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_activeuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deactiveuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_revokeuser = new DevExpress.XtraEditors.SimpleButton();
         this.sb_grantuser = new DevExpress.XtraEditors.SimpleButton();
         this.lv_users = new System.Windows.Forms.ListView();
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.textBox3 = new System.Windows.Forms.TextBox();
         this.sb_rolechanging = new DevExpress.XtraEditors.SimpleButton();
         this.sb_removerole = new DevExpress.XtraEditors.SimpleButton();
         this.sb_createnewrole = new DevExpress.XtraEditors.SimpleButton();
         this.pn_roles = new System.Windows.Forms.Panel();
         this.Btn_Back = new System.MaxUi.NewMaxBtn();
         this.tc_role.SuspendLayout();
         this.tp_privilege.SuspendLayout();
         this.pn_editprivilege.SuspendLayout();
         this.tp_users.SuspendLayout();
         this.pn_01.SuspendLayout();
         this.pn_edituserprivilege.SuspendLayout();
         this.pn_edituser.SuspendLayout();
         this.pn_roles.SuspendLayout();
         this.SuspendLayout();
         // 
         // tv_roles
         // 
         this.tv_roles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tv_roles.BackColor = System.Drawing.Color.WhiteSmoke;
         this.tv_roles.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tv_roles.ImageIndex = 1;
         this.tv_roles.ImageList = this.il_icons;
         this.tv_roles.Location = new System.Drawing.Point(19, 41);
         this.tv_roles.Name = "tv_roles";
         treeNode1.ImageIndex = 1;
         treeNode1.Name = "Roles";
         treeNode1.Text = "گروه های دسترسی";
         this.tv_roles.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
         this.tv_roles.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.tv_roles.RightToLeftLayout = true;
         this.tv_roles.SelectedImageIndex = 1;
         this.tv_roles.Size = new System.Drawing.Size(239, 420);
         this.tv_roles.TabIndex = 0;
         this.tv_roles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AfterSelectdRole);
         // 
         // il_icons
         // 
         this.il_icons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il_icons.ImageStream")));
         this.il_icons.TransparentColor = System.Drawing.Color.Transparent;
         this.il_icons.Images.SetKeyName(0, "ICONS_1000.ico");
         this.il_icons.Images.SetKeyName(1, "ICONS_1001.ico");
         this.il_icons.Images.SetKeyName(2, "ICONS_1005.ico");
         this.il_icons.Images.SetKeyName(3, "ICONS_1003.ico");
         this.il_icons.Images.SetKeyName(4, "ICONS_1004.ico");
         this.il_icons.Images.SetKeyName(5, "ICONS_1006.ico");
         this.il_icons.Images.SetKeyName(6, "ICONS_1007.ico");
         this.il_icons.Images.SetKeyName(7, "ICONS_1010.ico");
         this.il_icons.Images.SetKeyName(8, "ICONS_1009.ico");
         this.il_icons.Images.SetKeyName(9, "ICONS_1008.ico");
         this.il_icons.Images.SetKeyName(10, "ICONS_1011.ico");
         this.il_icons.Images.SetKeyName(11, "ICONS_1012.ico");
         this.il_icons.Images.SetKeyName(12, "ICONS_1013.ico");
         this.il_icons.Images.SetKeyName(13, "ICONS_1014.ico");
         // 
         // textBox1
         // 
         this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox1.BackColor = System.Drawing.Color.AliceBlue;
         this.textBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox1.Location = new System.Drawing.Point(19, 15);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(239, 22);
         this.textBox1.TabIndex = 1;
         // 
         // tc_role
         // 
         this.tc_role.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tc_role.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
         this.tc_role.Controls.Add(this.tp_privilege);
         this.tc_role.Controls.Add(this.tp_users);
         this.tc_role.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.tc_role.Location = new System.Drawing.Point(21, 18);
         this.tc_role.Name = "tc_role";
         this.tc_role.RightToLeftLayout = true;
         this.tc_role.SelectedIndex = 0;
         this.tc_role.Size = new System.Drawing.Size(657, 506);
         this.tc_role.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
         this.tc_role.TabIndex = 6;
         // 
         // tp_privilege
         // 
         this.tp_privilege.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.tp_privilege.Controls.Add(this.pn_editprivilege);
         this.tp_privilege.Controls.Add(this.sb_activeprivilege);
         this.tp_privilege.Controls.Add(this.sb_deactiveprivilege);
         this.tp_privilege.Controls.Add(this.sb_removeprivilege);
         this.tp_privilege.Controls.Add(this.sb_addprivilege);
         this.tp_privilege.Controls.Add(this.lv_privileges);
         this.tp_privilege.Controls.Add(this.textBox2);
         this.tp_privilege.Location = new System.Drawing.Point(4, 25);
         this.tp_privilege.Name = "tp_privilege";
         this.tp_privilege.Padding = new System.Windows.Forms.Padding(3);
         this.tp_privilege.Size = new System.Drawing.Size(649, 477);
         this.tp_privilege.TabIndex = 0;
         this.tp_privilege.Text = "سطح دسترسی";
         // 
         // pn_editprivilege
         // 
         this.pn_editprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.pn_editprivilege.Controls.Add(this.sb_selectallprivileges);
         this.pn_editprivilege.Controls.Add(this.sb_selectinvertprivileges);
         this.pn_editprivilege.Controls.Add(this.sb_deselectprivileges);
         this.pn_editprivilege.Controls.Add(this.sb_submitprivilegechange);
         this.pn_editprivilege.Controls.Add(this.sb_cancelprivilegechange);
         this.pn_editprivilege.Location = new System.Drawing.Point(6, 427);
         this.pn_editprivilege.Name = "pn_editprivilege";
         this.pn_editprivilege.Size = new System.Drawing.Size(242, 44);
         this.pn_editprivilege.TabIndex = 23;
         this.pn_editprivilege.Visible = false;
         // 
         // sb_selectallprivileges
         // 
         this.sb_selectallprivileges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_selectallprivileges.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_selectallprivileges.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_selectallprivileges.Appearance.Options.UseFont = true;
         this.sb_selectallprivileges.Appearance.Options.UseForeColor = true;
         this.sb_selectallprivileges.ImageIndex = 13;
         this.sb_selectallprivileges.ImageList = this.il_icons;
         this.sb_selectallprivileges.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_selectallprivileges.Location = new System.Drawing.Point(203, 6);
         this.sb_selectallprivileges.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_selectallprivileges.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_selectallprivileges.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_selectallprivileges.Name = "sb_selectallprivileges";
         this.sb_selectallprivileges.Size = new System.Drawing.Size(33, 33);
         this.sb_selectallprivileges.TabIndex = 25;
         this.sb_selectallprivileges.Click += new System.EventHandler(this.sb_selectallprivileges_Click);
         // 
         // sb_selectinvertprivileges
         // 
         this.sb_selectinvertprivileges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_selectinvertprivileges.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_selectinvertprivileges.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_selectinvertprivileges.Appearance.Options.UseFont = true;
         this.sb_selectinvertprivileges.Appearance.Options.UseForeColor = true;
         this.sb_selectinvertprivileges.ImageIndex = 11;
         this.sb_selectinvertprivileges.ImageList = this.il_icons;
         this.sb_selectinvertprivileges.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_selectinvertprivileges.Location = new System.Drawing.Point(164, 6);
         this.sb_selectinvertprivileges.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_selectinvertprivileges.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_selectinvertprivileges.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_selectinvertprivileges.Name = "sb_selectinvertprivileges";
         this.sb_selectinvertprivileges.Size = new System.Drawing.Size(33, 33);
         this.sb_selectinvertprivileges.TabIndex = 23;
         this.sb_selectinvertprivileges.Click += new System.EventHandler(this.sb_selectinvertprivileges_Click);
         // 
         // sb_deselectprivileges
         // 
         this.sb_deselectprivileges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_deselectprivileges.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deselectprivileges.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deselectprivileges.Appearance.Options.UseFont = true;
         this.sb_deselectprivileges.Appearance.Options.UseForeColor = true;
         this.sb_deselectprivileges.ImageIndex = 12;
         this.sb_deselectprivileges.ImageList = this.il_icons;
         this.sb_deselectprivileges.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deselectprivileges.Location = new System.Drawing.Point(121, 6);
         this.sb_deselectprivileges.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deselectprivileges.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deselectprivileges.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deselectprivileges.Name = "sb_deselectprivileges";
         this.sb_deselectprivileges.Size = new System.Drawing.Size(33, 33);
         this.sb_deselectprivileges.TabIndex = 24;
         this.sb_deselectprivileges.Click += new System.EventHandler(this.sb_deselectprivileges_Click);
         // 
         // sb_submitprivilegechange
         // 
         this.sb_submitprivilegechange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_submitprivilegechange.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_submitprivilegechange.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_submitprivilegechange.Appearance.Options.UseFont = true;
         this.sb_submitprivilegechange.Appearance.Options.UseForeColor = true;
         this.sb_submitprivilegechange.ImageIndex = 8;
         this.sb_submitprivilegechange.ImageList = this.il_icons;
         this.sb_submitprivilegechange.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_submitprivilegechange.Location = new System.Drawing.Point(46, 6);
         this.sb_submitprivilegechange.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_submitprivilegechange.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_submitprivilegechange.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_submitprivilegechange.Name = "sb_submitprivilegechange";
         this.sb_submitprivilegechange.Size = new System.Drawing.Size(33, 33);
         this.sb_submitprivilegechange.TabIndex = 22;
         this.sb_submitprivilegechange.Click += new System.EventHandler(this.sb_submitprivilegechange_Click);
         // 
         // sb_cancelprivilegechange
         // 
         this.sb_cancelprivilegechange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_cancelprivilegechange.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_cancelprivilegechange.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancelprivilegechange.Appearance.Options.UseFont = true;
         this.sb_cancelprivilegechange.Appearance.Options.UseForeColor = true;
         this.sb_cancelprivilegechange.ImageIndex = 9;
         this.sb_cancelprivilegechange.ImageList = this.il_icons;
         this.sb_cancelprivilegechange.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_cancelprivilegechange.Location = new System.Drawing.Point(3, 6);
         this.sb_cancelprivilegechange.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancelprivilegechange.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancelprivilegechange.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancelprivilegechange.Name = "sb_cancelprivilegechange";
         this.sb_cancelprivilegechange.Size = new System.Drawing.Size(33, 33);
         this.sb_cancelprivilegechange.TabIndex = 22;
         this.sb_cancelprivilegechange.Click += new System.EventHandler(this.sb_cancelprivilegechange_Click);
         // 
         // sb_activeprivilege
         // 
         this.sb_activeprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_activeprivilege.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_activeprivilege.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_activeprivilege.Appearance.Options.UseFont = true;
         this.sb_activeprivilege.Appearance.Options.UseForeColor = true;
         this.sb_activeprivilege.ImageIndex = 2;
         this.sb_activeprivilege.ImageList = this.il_icons;
         this.sb_activeprivilege.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_activeprivilege.Location = new System.Drawing.Point(493, 433);
         this.sb_activeprivilege.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_activeprivilege.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_activeprivilege.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_activeprivilege.Name = "sb_activeprivilege";
         this.sb_activeprivilege.Size = new System.Drawing.Size(33, 33);
         this.sb_activeprivilege.TabIndex = 7;
         this.sb_activeprivilege.Click += new System.EventHandler(this.sb_activeprivilege_Click);
         // 
         // sb_deactiveprivilege
         // 
         this.sb_deactiveprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_deactiveprivilege.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deactiveprivilege.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deactiveprivilege.Appearance.Options.UseFont = true;
         this.sb_deactiveprivilege.Appearance.Options.UseForeColor = true;
         this.sb_deactiveprivilege.ImageIndex = 5;
         this.sb_deactiveprivilege.ImageList = this.il_icons;
         this.sb_deactiveprivilege.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deactiveprivilege.Location = new System.Drawing.Point(532, 433);
         this.sb_deactiveprivilege.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deactiveprivilege.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deactiveprivilege.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deactiveprivilege.Name = "sb_deactiveprivilege";
         this.sb_deactiveprivilege.Size = new System.Drawing.Size(33, 33);
         this.sb_deactiveprivilege.TabIndex = 7;
         this.sb_deactiveprivilege.Click += new System.EventHandler(this.sb_deactiveprivilege_Click);
         // 
         // sb_removeprivilege
         // 
         this.sb_removeprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_removeprivilege.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_removeprivilege.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_removeprivilege.Appearance.Options.UseFont = true;
         this.sb_removeprivilege.Appearance.Options.UseForeColor = true;
         this.sb_removeprivilege.ImageIndex = 7;
         this.sb_removeprivilege.ImageList = this.il_icons;
         this.sb_removeprivilege.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_removeprivilege.Location = new System.Drawing.Point(571, 433);
         this.sb_removeprivilege.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_removeprivilege.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_removeprivilege.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_removeprivilege.Name = "sb_removeprivilege";
         this.sb_removeprivilege.Size = new System.Drawing.Size(33, 33);
         this.sb_removeprivilege.TabIndex = 10;
         this.sb_removeprivilege.Click += new System.EventHandler(this.sb_removeprivilege_Click);
         // 
         // sb_addprivilege
         // 
         this.sb_addprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_addprivilege.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_addprivilege.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_addprivilege.Appearance.Options.UseFont = true;
         this.sb_addprivilege.Appearance.Options.UseForeColor = true;
         this.sb_addprivilege.ImageIndex = 6;
         this.sb_addprivilege.ImageList = this.il_icons;
         this.sb_addprivilege.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_addprivilege.Location = new System.Drawing.Point(610, 433);
         this.sb_addprivilege.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_addprivilege.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_addprivilege.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_addprivilege.Name = "sb_addprivilege";
         this.sb_addprivilege.Size = new System.Drawing.Size(33, 33);
         this.sb_addprivilege.TabIndex = 9;
         this.sb_addprivilege.Click += new System.EventHandler(this.sb_addprivilege_Click);
         // 
         // lv_privileges
         // 
         this.lv_privileges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lv_privileges.BackColor = System.Drawing.Color.WhiteSmoke;
         this.lv_privileges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
         this.lv_privileges.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lv_privileges.FullRowSelect = true;
         this.lv_privileges.Location = new System.Drawing.Point(6, 34);
         this.lv_privileges.Name = "lv_privileges";
         this.lv_privileges.RightToLeftLayout = true;
         this.lv_privileges.Size = new System.Drawing.Size(637, 387);
         this.lv_privileges.SmallImageList = this.il_icons;
         this.lv_privileges.TabIndex = 8;
         this.lv_privileges.UseCompatibleStateImageBehavior = false;
         this.lv_privileges.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader1
         // 
         this.columnHeader1.Text = "سطوح دسترسی";
         this.columnHeader1.Width = 363;
         // 
         // textBox2
         // 
         this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox2.BackColor = System.Drawing.Color.AliceBlue;
         this.textBox2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox2.Location = new System.Drawing.Point(6, 6);
         this.textBox2.Name = "textBox2";
         this.textBox2.Size = new System.Drawing.Size(637, 22);
         this.textBox2.TabIndex = 7;
         // 
         // tp_users
         // 
         this.tp_users.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.tp_users.Controls.Add(this.pn_01);
         this.tp_users.Controls.Add(this.sb_usersinglepropertymenu);
         this.tp_users.Controls.Add(this.pn_edituserprivilege);
         this.tp_users.Controls.Add(this.lv_userprivileges);
         this.tp_users.Controls.Add(this.pn_edituser);
         this.tp_users.Controls.Add(this.sb_activeuser);
         this.tp_users.Controls.Add(this.sb_deactiveuser);
         this.tp_users.Controls.Add(this.sb_revokeuser);
         this.tp_users.Controls.Add(this.sb_grantuser);
         this.tp_users.Controls.Add(this.lv_users);
         this.tp_users.Controls.Add(this.textBox3);
         this.tp_users.Location = new System.Drawing.Point(4, 25);
         this.tp_users.Name = "tp_users";
         this.tp_users.Padding = new System.Windows.Forms.Padding(3);
         this.tp_users.Size = new System.Drawing.Size(649, 477);
         this.tp_users.TabIndex = 1;
         this.tp_users.Text = "کاربران";
         // 
         // pn_01
         // 
         this.pn_01.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.pn_01.Controls.Add(this.sb_activeprivilegetouser);
         this.pn_01.Controls.Add(this.sb_grantprivilegetouser);
         this.pn_01.Controls.Add(this.sb_revokeprivilegefromuser);
         this.pn_01.Controls.Add(this.sb_deactiveprivilegefromuser);
         this.pn_01.Location = new System.Drawing.Point(138, 338);
         this.pn_01.Name = "pn_01";
         this.pn_01.Size = new System.Drawing.Size(309, 58);
         this.pn_01.TabIndex = 35;
         // 
         // sb_activeprivilegetouser
         // 
         this.sb_activeprivilegetouser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_activeprivilegetouser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_activeprivilegetouser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_activeprivilegetouser.Appearance.Options.UseFont = true;
         this.sb_activeprivilegetouser.Appearance.Options.UseForeColor = true;
         this.sb_activeprivilegetouser.ImageIndex = 2;
         this.sb_activeprivilegetouser.ImageList = this.il_icons;
         this.sb_activeprivilegetouser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_activeprivilegetouser.Location = new System.Drawing.Point(148, 6);
         this.sb_activeprivilegetouser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_activeprivilegetouser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_activeprivilegetouser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_activeprivilegetouser.Name = "sb_activeprivilegetouser";
         this.sb_activeprivilegetouser.Size = new System.Drawing.Size(33, 33);
         this.sb_activeprivilegetouser.TabIndex = 30;
         this.sb_activeprivilegetouser.Click += new System.EventHandler(this.sb_activeprivilegetouser_Click);
         // 
         // sb_grantprivilegetouser
         // 
         this.sb_grantprivilegetouser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_grantprivilegetouser.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_grantprivilegetouser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_grantprivilegetouser.Appearance.Options.UseFont = true;
         this.sb_grantprivilegetouser.Appearance.Options.UseForeColor = true;
         this.sb_grantprivilegetouser.ImageIndex = 6;
         this.sb_grantprivilegetouser.ImageList = this.il_icons;
         this.sb_grantprivilegetouser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_grantprivilegetouser.Location = new System.Drawing.Point(265, 6);
         this.sb_grantprivilegetouser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_grantprivilegetouser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_grantprivilegetouser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_grantprivilegetouser.Name = "sb_grantprivilegetouser";
         this.sb_grantprivilegetouser.Size = new System.Drawing.Size(33, 33);
         this.sb_grantprivilegetouser.TabIndex = 32;
         this.sb_grantprivilegetouser.Click += new System.EventHandler(this.sb_grantprivilegetouser_Click);
         // 
         // sb_revokeprivilegefromuser
         // 
         this.sb_revokeprivilegefromuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_revokeprivilegefromuser.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_revokeprivilegefromuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_revokeprivilegefromuser.Appearance.Options.UseFont = true;
         this.sb_revokeprivilegefromuser.Appearance.Options.UseForeColor = true;
         this.sb_revokeprivilegefromuser.ImageIndex = 7;
         this.sb_revokeprivilegefromuser.ImageList = this.il_icons;
         this.sb_revokeprivilegefromuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_revokeprivilegefromuser.Location = new System.Drawing.Point(226, 6);
         this.sb_revokeprivilegefromuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_revokeprivilegefromuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_revokeprivilegefromuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_revokeprivilegefromuser.Name = "sb_revokeprivilegefromuser";
         this.sb_revokeprivilegefromuser.Size = new System.Drawing.Size(33, 33);
         this.sb_revokeprivilegefromuser.TabIndex = 33;
         this.sb_revokeprivilegefromuser.Click += new System.EventHandler(this.sb_revokeprivilegefromuser_Click);
         // 
         // sb_deactiveprivilegefromuser
         // 
         this.sb_deactiveprivilegefromuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_deactiveprivilegefromuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deactiveprivilegefromuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deactiveprivilegefromuser.Appearance.Options.UseFont = true;
         this.sb_deactiveprivilegefromuser.Appearance.Options.UseForeColor = true;
         this.sb_deactiveprivilegefromuser.ImageIndex = 5;
         this.sb_deactiveprivilegefromuser.ImageList = this.il_icons;
         this.sb_deactiveprivilegefromuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deactiveprivilegefromuser.Location = new System.Drawing.Point(187, 6);
         this.sb_deactiveprivilegefromuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deactiveprivilegefromuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deactiveprivilegefromuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deactiveprivilegefromuser.Name = "sb_deactiveprivilegefromuser";
         this.sb_deactiveprivilegefromuser.Size = new System.Drawing.Size(33, 33);
         this.sb_deactiveprivilegefromuser.TabIndex = 31;
         this.sb_deactiveprivilegefromuser.Click += new System.EventHandler(this.sb_deactiveprivilegeformuser_Click);
         // 
         // sb_usersinglepropertymenu
         // 
         this.sb_usersinglepropertymenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_usersinglepropertymenu.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_usersinglepropertymenu.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_usersinglepropertymenu.Appearance.Options.UseFont = true;
         this.sb_usersinglepropertymenu.Appearance.Options.UseForeColor = true;
         this.sb_usersinglepropertymenu.ImageIndex = 10;
         this.sb_usersinglepropertymenu.ImageList = this.il_icons;
         this.sb_usersinglepropertymenu.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_usersinglepropertymenu.Location = new System.Drawing.Point(453, 433);
         this.sb_usersinglepropertymenu.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_usersinglepropertymenu.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_usersinglepropertymenu.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_usersinglepropertymenu.Name = "sb_usersinglepropertymenu";
         this.sb_usersinglepropertymenu.Size = new System.Drawing.Size(33, 33);
         this.sb_usersinglepropertymenu.TabIndex = 34;
         this.sb_usersinglepropertymenu.Click += new System.EventHandler(this.sb_usersinglepropertymenu_Click);
         // 
         // pn_edituserprivilege
         // 
         this.pn_edituserprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.pn_edituserprivilege.Controls.Add(this.sb_selectallprivilegeforuser);
         this.pn_edituserprivilege.Controls.Add(this.sb_invertselectprivielegeforuser);
         this.pn_edituserprivilege.Controls.Add(this.sb_deselectprivilegeforuser);
         this.pn_edituserprivilege.Controls.Add(this.sb_submitprivilegechangeforuser);
         this.pn_edituserprivilege.Controls.Add(this.sb_cancelprivielegechangeforuser);
         this.pn_edituserprivilege.Location = new System.Drawing.Point(6, 338);
         this.pn_edituserprivilege.Name = "pn_edituserprivilege";
         this.pn_edituserprivilege.Size = new System.Drawing.Size(126, 83);
         this.pn_edituserprivilege.TabIndex = 29;
         this.pn_edituserprivilege.Visible = false;
         // 
         // sb_selectallprivilegeforuser
         // 
         this.sb_selectallprivilegeforuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_selectallprivilegeforuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_selectallprivilegeforuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_selectallprivilegeforuser.Appearance.Options.UseFont = true;
         this.sb_selectallprivilegeforuser.Appearance.Options.UseForeColor = true;
         this.sb_selectallprivilegeforuser.ImageIndex = 13;
         this.sb_selectallprivilegeforuser.ImageList = this.il_icons;
         this.sb_selectallprivilegeforuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_selectallprivilegeforuser.Location = new System.Drawing.Point(85, 6);
         this.sb_selectallprivilegeforuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_selectallprivilegeforuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_selectallprivilegeforuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_selectallprivilegeforuser.Name = "sb_selectallprivilegeforuser";
         this.sb_selectallprivilegeforuser.Size = new System.Drawing.Size(33, 33);
         this.sb_selectallprivilegeforuser.TabIndex = 25;
         this.sb_selectallprivilegeforuser.Click += new System.EventHandler(this.sb_selectallprivilegeforuser_Click);
         // 
         // sb_invertselectprivielegeforuser
         // 
         this.sb_invertselectprivielegeforuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_invertselectprivielegeforuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_invertselectprivielegeforuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_invertselectprivielegeforuser.Appearance.Options.UseFont = true;
         this.sb_invertselectprivielegeforuser.Appearance.Options.UseForeColor = true;
         this.sb_invertselectprivielegeforuser.ImageIndex = 11;
         this.sb_invertselectprivielegeforuser.ImageList = this.il_icons;
         this.sb_invertselectprivielegeforuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_invertselectprivielegeforuser.Location = new System.Drawing.Point(46, 6);
         this.sb_invertselectprivielegeforuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_invertselectprivielegeforuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_invertselectprivielegeforuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_invertselectprivielegeforuser.Name = "sb_invertselectprivielegeforuser";
         this.sb_invertselectprivielegeforuser.Size = new System.Drawing.Size(33, 33);
         this.sb_invertselectprivielegeforuser.TabIndex = 23;
         this.sb_invertselectprivielegeforuser.Click += new System.EventHandler(this.sb_invertselectprivielegeforuser_Click);
         // 
         // sb_deselectprivilegeforuser
         // 
         this.sb_deselectprivilegeforuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_deselectprivilegeforuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deselectprivilegeforuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deselectprivilegeforuser.Appearance.Options.UseFont = true;
         this.sb_deselectprivilegeforuser.Appearance.Options.UseForeColor = true;
         this.sb_deselectprivilegeforuser.ImageIndex = 12;
         this.sb_deselectprivilegeforuser.ImageList = this.il_icons;
         this.sb_deselectprivilegeforuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deselectprivilegeforuser.Location = new System.Drawing.Point(3, 6);
         this.sb_deselectprivilegeforuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deselectprivilegeforuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deselectprivilegeforuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deselectprivilegeforuser.Name = "sb_deselectprivilegeforuser";
         this.sb_deselectprivilegeforuser.Size = new System.Drawing.Size(33, 33);
         this.sb_deselectprivilegeforuser.TabIndex = 24;
         this.sb_deselectprivilegeforuser.Click += new System.EventHandler(this.sb_deselectprivilegeforuser_Click);
         // 
         // sb_submitprivilegechangeforuser
         // 
         this.sb_submitprivilegechangeforuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_submitprivilegechangeforuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_submitprivilegechangeforuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_submitprivilegechangeforuser.Appearance.Options.UseFont = true;
         this.sb_submitprivilegechangeforuser.Appearance.Options.UseForeColor = true;
         this.sb_submitprivilegechangeforuser.ImageIndex = 8;
         this.sb_submitprivilegechangeforuser.ImageList = this.il_icons;
         this.sb_submitprivilegechangeforuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_submitprivilegechangeforuser.Location = new System.Drawing.Point(46, 45);
         this.sb_submitprivilegechangeforuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_submitprivilegechangeforuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_submitprivilegechangeforuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_submitprivilegechangeforuser.Name = "sb_submitprivilegechangeforuser";
         this.sb_submitprivilegechangeforuser.Size = new System.Drawing.Size(33, 33);
         this.sb_submitprivilegechangeforuser.TabIndex = 22;
         this.sb_submitprivilegechangeforuser.Click += new System.EventHandler(this.sb_submitchangeprivilegetouser_Click);
         // 
         // sb_cancelprivielegechangeforuser
         // 
         this.sb_cancelprivielegechangeforuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_cancelprivielegechangeforuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_cancelprivielegechangeforuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancelprivielegechangeforuser.Appearance.Options.UseFont = true;
         this.sb_cancelprivielegechangeforuser.Appearance.Options.UseForeColor = true;
         this.sb_cancelprivielegechangeforuser.ImageIndex = 9;
         this.sb_cancelprivielegechangeforuser.ImageList = this.il_icons;
         this.sb_cancelprivielegechangeforuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_cancelprivielegechangeforuser.Location = new System.Drawing.Point(3, 45);
         this.sb_cancelprivielegechangeforuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancelprivielegechangeforuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancelprivielegechangeforuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancelprivielegechangeforuser.Name = "sb_cancelprivielegechangeforuser";
         this.sb_cancelprivielegechangeforuser.Size = new System.Drawing.Size(33, 33);
         this.sb_cancelprivielegechangeforuser.TabIndex = 22;
         this.sb_cancelprivielegechangeforuser.Click += new System.EventHandler(this.sb_cancelchangeprivilegetouser_Click);
         // 
         // lv_userprivileges
         // 
         this.lv_userprivileges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lv_userprivileges.BackColor = System.Drawing.Color.WhiteSmoke;
         this.lv_userprivileges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3});
         this.lv_userprivileges.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lv_userprivileges.FullRowSelect = true;
         this.lv_userprivileges.Location = new System.Drawing.Point(6, 34);
         this.lv_userprivileges.Name = "lv_userprivileges";
         this.lv_userprivileges.RightToLeftLayout = true;
         this.lv_userprivileges.Size = new System.Drawing.Size(442, 298);
         this.lv_userprivileges.SmallImageList = this.il_icons;
         this.lv_userprivileges.TabIndex = 29;
         this.lv_userprivileges.UseCompatibleStateImageBehavior = false;
         this.lv_userprivileges.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader3
         // 
         this.columnHeader3.Text = "سطوح دسترسی";
         this.columnHeader3.Width = 299;
         // 
         // pn_edituser
         // 
         this.pn_edituser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.pn_edituser.Controls.Add(this.sb_selectalluser);
         this.pn_edituser.Controls.Add(this.sb_invertselectuser);
         this.pn_edituser.Controls.Add(this.sb_deselectuser);
         this.pn_edituser.Controls.Add(this.sb_submituserchange);
         this.pn_edituser.Controls.Add(this.sb_cancelchangeuser);
         this.pn_edituser.Location = new System.Drawing.Point(6, 427);
         this.pn_edituser.Name = "pn_edituser";
         this.pn_edituser.Size = new System.Drawing.Size(242, 44);
         this.pn_edituser.TabIndex = 28;
         this.pn_edituser.Visible = false;
         // 
         // sb_selectalluser
         // 
         this.sb_selectalluser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_selectalluser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_selectalluser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_selectalluser.Appearance.Options.UseFont = true;
         this.sb_selectalluser.Appearance.Options.UseForeColor = true;
         this.sb_selectalluser.ImageIndex = 13;
         this.sb_selectalluser.ImageList = this.il_icons;
         this.sb_selectalluser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_selectalluser.Location = new System.Drawing.Point(203, 6);
         this.sb_selectalluser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_selectalluser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_selectalluser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_selectalluser.Name = "sb_selectalluser";
         this.sb_selectalluser.Size = new System.Drawing.Size(33, 33);
         this.sb_selectalluser.TabIndex = 25;
         this.sb_selectalluser.Click += new System.EventHandler(this.sb_selectalluser_Click);
         // 
         // sb_invertselectuser
         // 
         this.sb_invertselectuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_invertselectuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_invertselectuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_invertselectuser.Appearance.Options.UseFont = true;
         this.sb_invertselectuser.Appearance.Options.UseForeColor = true;
         this.sb_invertselectuser.ImageIndex = 11;
         this.sb_invertselectuser.ImageList = this.il_icons;
         this.sb_invertselectuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_invertselectuser.Location = new System.Drawing.Point(164, 6);
         this.sb_invertselectuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_invertselectuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_invertselectuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_invertselectuser.Name = "sb_invertselectuser";
         this.sb_invertselectuser.Size = new System.Drawing.Size(33, 33);
         this.sb_invertselectuser.TabIndex = 23;
         this.sb_invertselectuser.Click += new System.EventHandler(this.sb_invertselectuser_Click);
         // 
         // sb_deselectuser
         // 
         this.sb_deselectuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_deselectuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deselectuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deselectuser.Appearance.Options.UseFont = true;
         this.sb_deselectuser.Appearance.Options.UseForeColor = true;
         this.sb_deselectuser.ImageIndex = 12;
         this.sb_deselectuser.ImageList = this.il_icons;
         this.sb_deselectuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deselectuser.Location = new System.Drawing.Point(121, 6);
         this.sb_deselectuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deselectuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deselectuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deselectuser.Name = "sb_deselectuser";
         this.sb_deselectuser.Size = new System.Drawing.Size(33, 33);
         this.sb_deselectuser.TabIndex = 24;
         this.sb_deselectuser.Click += new System.EventHandler(this.sb_deselectuser_Click);
         // 
         // sb_submituserchange
         // 
         this.sb_submituserchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_submituserchange.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_submituserchange.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_submituserchange.Appearance.Options.UseFont = true;
         this.sb_submituserchange.Appearance.Options.UseForeColor = true;
         this.sb_submituserchange.ImageIndex = 8;
         this.sb_submituserchange.ImageList = this.il_icons;
         this.sb_submituserchange.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_submituserchange.Location = new System.Drawing.Point(46, 6);
         this.sb_submituserchange.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_submituserchange.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_submituserchange.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_submituserchange.Name = "sb_submituserchange";
         this.sb_submituserchange.Size = new System.Drawing.Size(33, 33);
         this.sb_submituserchange.TabIndex = 22;
         this.sb_submituserchange.Click += new System.EventHandler(this.sb_submitchangeuser_Click);
         // 
         // sb_cancelchangeuser
         // 
         this.sb_cancelchangeuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_cancelchangeuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_cancelchangeuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancelchangeuser.Appearance.Options.UseFont = true;
         this.sb_cancelchangeuser.Appearance.Options.UseForeColor = true;
         this.sb_cancelchangeuser.ImageIndex = 9;
         this.sb_cancelchangeuser.ImageList = this.il_icons;
         this.sb_cancelchangeuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_cancelchangeuser.Location = new System.Drawing.Point(3, 6);
         this.sb_cancelchangeuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancelchangeuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancelchangeuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancelchangeuser.Name = "sb_cancelchangeuser";
         this.sb_cancelchangeuser.Size = new System.Drawing.Size(33, 33);
         this.sb_cancelchangeuser.TabIndex = 22;
         this.sb_cancelchangeuser.Click += new System.EventHandler(this.sb_cancelchangeuser_Click);
         // 
         // sb_activeuser
         // 
         this.sb_activeuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_activeuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_activeuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_activeuser.Appearance.Options.UseFont = true;
         this.sb_activeuser.Appearance.Options.UseForeColor = true;
         this.sb_activeuser.ImageIndex = 3;
         this.sb_activeuser.ImageList = this.il_icons;
         this.sb_activeuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_activeuser.Location = new System.Drawing.Point(492, 433);
         this.sb_activeuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_activeuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_activeuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_activeuser.Name = "sb_activeuser";
         this.sb_activeuser.Size = new System.Drawing.Size(33, 33);
         this.sb_activeuser.TabIndex = 24;
         this.sb_activeuser.Click += new System.EventHandler(this.sb_activeuser_Click);
         // 
         // sb_deactiveuser
         // 
         this.sb_deactiveuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_deactiveuser.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deactiveuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deactiveuser.Appearance.Options.UseFont = true;
         this.sb_deactiveuser.Appearance.Options.UseForeColor = true;
         this.sb_deactiveuser.ImageIndex = 4;
         this.sb_deactiveuser.ImageList = this.il_icons;
         this.sb_deactiveuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deactiveuser.Location = new System.Drawing.Point(531, 433);
         this.sb_deactiveuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deactiveuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deactiveuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deactiveuser.Name = "sb_deactiveuser";
         this.sb_deactiveuser.Size = new System.Drawing.Size(33, 33);
         this.sb_deactiveuser.TabIndex = 25;
         this.sb_deactiveuser.Click += new System.EventHandler(this.sb_deactiveuser_Click);
         // 
         // sb_revokeuser
         // 
         this.sb_revokeuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_revokeuser.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_revokeuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_revokeuser.Appearance.Options.UseFont = true;
         this.sb_revokeuser.Appearance.Options.UseForeColor = true;
         this.sb_revokeuser.ImageIndex = 7;
         this.sb_revokeuser.ImageList = this.il_icons;
         this.sb_revokeuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_revokeuser.Location = new System.Drawing.Point(570, 433);
         this.sb_revokeuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_revokeuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_revokeuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_revokeuser.Name = "sb_revokeuser";
         this.sb_revokeuser.Size = new System.Drawing.Size(33, 33);
         this.sb_revokeuser.TabIndex = 27;
         this.sb_revokeuser.Click += new System.EventHandler(this.sb_revokeuser_Click);
         // 
         // sb_grantuser
         // 
         this.sb_grantuser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_grantuser.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_grantuser.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_grantuser.Appearance.Options.UseFont = true;
         this.sb_grantuser.Appearance.Options.UseForeColor = true;
         this.sb_grantuser.ImageIndex = 6;
         this.sb_grantuser.ImageList = this.il_icons;
         this.sb_grantuser.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_grantuser.Location = new System.Drawing.Point(609, 433);
         this.sb_grantuser.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_grantuser.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_grantuser.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_grantuser.Name = "sb_grantuser";
         this.sb_grantuser.Size = new System.Drawing.Size(33, 33);
         this.sb_grantuser.TabIndex = 26;
         this.sb_grantuser.Click += new System.EventHandler(this.sb_grantuser_Click);
         // 
         // lv_users
         // 
         this.lv_users.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lv_users.BackColor = System.Drawing.Color.WhiteSmoke;
         this.lv_users.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
         this.lv_users.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lv_users.Location = new System.Drawing.Point(454, 34);
         this.lv_users.MultiSelect = false;
         this.lv_users.Name = "lv_users";
         this.lv_users.RightToLeftLayout = true;
         this.lv_users.Size = new System.Drawing.Size(189, 387);
         this.lv_users.SmallImageList = this.il_icons;
         this.lv_users.TabIndex = 12;
         this.lv_users.UseCompatibleStateImageBehavior = false;
         this.lv_users.View = System.Windows.Forms.View.Details;
         this.lv_users.SelectedIndexChanged += new System.EventHandler(this.lv_users_SelectedIndexChanged);
         // 
         // columnHeader2
         // 
         this.columnHeader2.Text = "نام کاربران";
         this.columnHeader2.Width = 242;
         // 
         // textBox3
         // 
         this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox3.BackColor = System.Drawing.Color.AliceBlue;
         this.textBox3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox3.Location = new System.Drawing.Point(6, 6);
         this.textBox3.Name = "textBox3";
         this.textBox3.Size = new System.Drawing.Size(637, 22);
         this.textBox3.TabIndex = 11;
         // 
         // sb_rolechanging
         // 
         this.sb_rolechanging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_rolechanging.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_rolechanging.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_rolechanging.Appearance.Options.UseFont = true;
         this.sb_rolechanging.Appearance.Options.UseForeColor = true;
         this.sb_rolechanging.ImageIndex = 10;
         this.sb_rolechanging.ImageList = this.il_icons;
         this.sb_rolechanging.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_rolechanging.Location = new System.Drawing.Point(147, 473);
         this.sb_rolechanging.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_rolechanging.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_rolechanging.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_rolechanging.Name = "sb_rolechanging";
         this.sb_rolechanging.Size = new System.Drawing.Size(33, 33);
         this.sb_rolechanging.TabIndex = 11;
         this.sb_rolechanging.Click += new System.EventHandler(this.sb_rolechanging_Click);
         // 
         // sb_removerole
         // 
         this.sb_removerole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_removerole.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_removerole.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_removerole.Appearance.Options.UseFont = true;
         this.sb_removerole.Appearance.Options.UseForeColor = true;
         this.sb_removerole.ImageIndex = 7;
         this.sb_removerole.ImageList = this.il_icons;
         this.sb_removerole.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_removerole.Location = new System.Drawing.Point(186, 473);
         this.sb_removerole.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_removerole.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_removerole.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_removerole.Name = "sb_removerole";
         this.sb_removerole.Size = new System.Drawing.Size(33, 33);
         this.sb_removerole.TabIndex = 13;
         this.sb_removerole.Click += new System.EventHandler(this.sb_removerole_Click);
         // 
         // sb_createnewrole
         // 
         this.sb_createnewrole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_createnewrole.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_createnewrole.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_createnewrole.Appearance.Options.UseFont = true;
         this.sb_createnewrole.Appearance.Options.UseForeColor = true;
         this.sb_createnewrole.ImageIndex = 6;
         this.sb_createnewrole.ImageList = this.il_icons;
         this.sb_createnewrole.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_createnewrole.Location = new System.Drawing.Point(225, 473);
         this.sb_createnewrole.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_createnewrole.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_createnewrole.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_createnewrole.Name = "sb_createnewrole";
         this.sb_createnewrole.Size = new System.Drawing.Size(33, 33);
         this.sb_createnewrole.TabIndex = 12;
         this.sb_createnewrole.Click += new System.EventHandler(this.sb_createnewrole_Click);
         // 
         // pn_roles
         // 
         this.pn_roles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.pn_roles.Controls.Add(this.Btn_Back);
         this.pn_roles.Controls.Add(this.tv_roles);
         this.pn_roles.Controls.Add(this.textBox1);
         this.pn_roles.Controls.Add(this.sb_rolechanging);
         this.pn_roles.Controls.Add(this.sb_createnewrole);
         this.pn_roles.Controls.Add(this.sb_removerole);
         this.pn_roles.Location = new System.Drawing.Point(684, 3);
         this.pn_roles.Name = "pn_roles";
         this.pn_roles.Size = new System.Drawing.Size(275, 518);
         this.pn_roles.TabIndex = 23;
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.BackColor = System.Drawing.Color.Transparent;
         this.Btn_Back.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_Back.Caption = "بازگشت";
         this.Btn_Back.Disabled = false;
         this.Btn_Back.EnterColor = System.Drawing.Color.Transparent;
         this.Btn_Back.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Btn_Back.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Btn_Back.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Back.ImageIndex = -1;
         this.Btn_Back.ImageList = null;
         this.Btn_Back.InToBold = false;
         this.Btn_Back.Location = new System.Drawing.Point(19, 477);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(108, 29);
         this.Btn_Back.TabIndex = 14;
         this.Btn_Back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Back.TextColor = System.Drawing.Color.Black;
         this.Btn_Back.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // SecuritySettings
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.Controls.Add(this.pn_roles);
         this.Controls.Add(this.tc_role);
         this.Name = "SecuritySettings";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(962, 524);
         this.tc_role.ResumeLayout(false);
         this.tp_privilege.ResumeLayout(false);
         this.tp_privilege.PerformLayout();
         this.pn_editprivilege.ResumeLayout(false);
         this.tp_users.ResumeLayout(false);
         this.tp_users.PerformLayout();
         this.pn_01.ResumeLayout(false);
         this.pn_edituserprivilege.ResumeLayout(false);
         this.pn_edituser.ResumeLayout(false);
         this.pn_roles.ResumeLayout(false);
         this.pn_roles.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.TreeView tv_roles;
      private Windows.Forms.TextBox textBox1;
      private Windows.Forms.TabControl tc_role;
      private Windows.Forms.TabPage tp_privilege;
      private DevExpress.XtraEditors.SimpleButton sb_deactiveprivilege;
      private DevExpress.XtraEditors.SimpleButton sb_removeprivilege;
      private DevExpress.XtraEditors.SimpleButton sb_addprivilege;
      private Windows.Forms.ListView lv_privileges;
      private Windows.Forms.ColumnHeader columnHeader1;
      private Windows.Forms.TextBox textBox2;
      private Windows.Forms.TabPage tp_users;
      private Windows.Forms.ListView lv_users;
      private Windows.Forms.ColumnHeader columnHeader2;
      private Windows.Forms.TextBox textBox3;
      private DevExpress.XtraEditors.SimpleButton sb_rolechanging;
      private DevExpress.XtraEditors.SimpleButton sb_removerole;
      private DevExpress.XtraEditors.SimpleButton sb_createnewrole;
      private DevExpress.XtraEditors.SimpleButton sb_submitprivilegechange;
      private Windows.Forms.ImageList il_icons;
      private Windows.Forms.Panel pn_roles;
      private DevExpress.XtraEditors.SimpleButton sb_cancelprivilegechange;
      private DevExpress.XtraEditors.SimpleButton sb_activeprivilege;
      private Windows.Forms.Panel pn_editprivilege;
      private DevExpress.XtraEditors.SimpleButton sb_selectallprivileges;
      private DevExpress.XtraEditors.SimpleButton sb_selectinvertprivileges;
      private DevExpress.XtraEditors.SimpleButton sb_deselectprivileges;
      private Windows.Forms.Panel pn_edituser;
      private DevExpress.XtraEditors.SimpleButton sb_selectalluser;
      private DevExpress.XtraEditors.SimpleButton sb_invertselectuser;
      private DevExpress.XtraEditors.SimpleButton sb_deselectuser;
      private DevExpress.XtraEditors.SimpleButton sb_submituserchange;
      private DevExpress.XtraEditors.SimpleButton sb_cancelchangeuser;
      private DevExpress.XtraEditors.SimpleButton sb_activeuser;
      private DevExpress.XtraEditors.SimpleButton sb_deactiveuser;
      private DevExpress.XtraEditors.SimpleButton sb_revokeuser;
      private DevExpress.XtraEditors.SimpleButton sb_grantuser;
      private Windows.Forms.Panel pn_edituserprivilege;
      private DevExpress.XtraEditors.SimpleButton sb_selectallprivilegeforuser;
      private DevExpress.XtraEditors.SimpleButton sb_activeprivilegetouser;
      private DevExpress.XtraEditors.SimpleButton sb_invertselectprivielegeforuser;
      private DevExpress.XtraEditors.SimpleButton sb_deactiveprivilegefromuser;
      private DevExpress.XtraEditors.SimpleButton sb_deselectprivilegeforuser;
      private DevExpress.XtraEditors.SimpleButton sb_revokeprivilegefromuser;
      private DevExpress.XtraEditors.SimpleButton sb_submitprivilegechangeforuser;
      private DevExpress.XtraEditors.SimpleButton sb_grantprivilegetouser;
      private DevExpress.XtraEditors.SimpleButton sb_cancelprivielegechangeforuser;
      private Windows.Forms.ListView lv_userprivileges;
      private Windows.Forms.ColumnHeader columnHeader3;
      private DevExpress.XtraEditors.SimpleButton sb_usersinglepropertymenu;
      private Windows.Forms.Panel pn_01;
      private MaxUi.NewMaxBtn Btn_Back;
   }
}
