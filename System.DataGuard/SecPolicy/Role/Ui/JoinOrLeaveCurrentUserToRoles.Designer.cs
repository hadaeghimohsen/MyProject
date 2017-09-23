namespace System.DataGuard.SecPolicy.Role.Ui
{
   partial class JoinOrLeaveCurrentUserToRoles
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JoinOrLeaveCurrentUserToRoles));
         this.panel1 = new System.Windows.Forms.Panel();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.txt_currentuser = new System.Windows.Forms.TextBox();
         this.lv_roles = new System.Windows.Forms.ListView();
         this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.il_icons = new System.Windows.Forms.ImageList(this.components);
         this.label3 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.panel2 = new System.Windows.Forms.Panel();
         this.sb_advanced = new DevExpress.XtraEditors.SimpleButton();
         this.sb_addprivilege = new DevExpress.XtraEditors.SimpleButton();
         this.pn_command = new System.Windows.Forms.Panel();
         this.sb_restore = new DevExpress.XtraEditors.SimpleButton();
         this.sb_join = new DevExpress.XtraEditors.SimpleButton();
         this.sb_leave = new DevExpress.XtraEditors.SimpleButton();
         this.sb_active = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deactive = new DevExpress.XtraEditors.SimpleButton();
         this.pn_edit = new System.Windows.Forms.Panel();
         this.sb_selectall = new DevExpress.XtraEditors.SimpleButton();
         this.sb_invertselect = new DevExpress.XtraEditors.SimpleButton();
         this.sb_deselectall = new DevExpress.XtraEditors.SimpleButton();
         this.sb_submit = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancel = new DevExpress.XtraEditors.SimpleButton();
         this.panel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.pn_command.SuspendLayout();
         this.pn_edit.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
         this.panel1.Controls.Add(this.label2);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(775, 63);
         this.panel1.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("B Kamran", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label2.ForeColor = System.Drawing.Color.White;
         this.label2.Location = new System.Drawing.Point(497, 15);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(263, 33);
         this.label2.TabIndex = 1;
         this.label2.Text = "ملحق شدن و خارج شدن از گروه دسترسی";
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(680, 86);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(77, 13);
         this.label1.TabIndex = 4;
         this.label1.Text = "نام کاربر جاری :";
         // 
         // txt_currentuser
         // 
         this.txt_currentuser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txt_currentuser.BackColor = System.Drawing.SystemColors.Control;
         this.txt_currentuser.Location = new System.Drawing.Point(42, 83);
         this.txt_currentuser.Name = "txt_currentuser";
         this.txt_currentuser.ReadOnly = true;
         this.txt_currentuser.Size = new System.Drawing.Size(632, 21);
         this.txt_currentuser.TabIndex = 5;
         // 
         // lv_roles
         // 
         this.lv_roles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lv_roles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
         this.lv_roles.LargeImageList = this.il_icons;
         this.lv_roles.Location = new System.Drawing.Point(42, 176);
         this.lv_roles.Name = "lv_roles";
         this.lv_roles.RightToLeftLayout = true;
         this.lv_roles.Size = new System.Drawing.Size(632, 434);
         this.lv_roles.SmallImageList = this.il_icons;
         this.lv_roles.TabIndex = 6;
         this.lv_roles.UseCompatibleStateImageBehavior = false;
         this.lv_roles.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader1
         // 
         this.columnHeader1.Text = "نام گروه دسترسی";
         this.columnHeader1.Width = 628;
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
         this.il_icons.Images.SetKeyName(14, "ICONS_1015.ico");
         this.il_icons.Images.SetKeyName(15, "ICONS_1018.ico");
         // 
         // label3
         // 
         this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label3.Location = new System.Drawing.Point(42, 107);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(632, 38);
         this.label3.TabIndex = 4;
         this.label3.Text = "دراین فرم شما برای گروه های دسترسی خود می توانید مشخص کنید که آیا می خواهید به گر" +
    "وه های دسترسی دیگری دسترسی داشته باشید یا اینکه می خواهید از گروه های دسترسی فعل" +
    "ی میخواهید خارج شوید؟!";
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(180, 154);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(497, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "لیست گروه های دسترسی که شما در آن عضو هستید یا اینکه می خواهید به آنها عضو شوید ی" +
    "ا خارج شوید :";
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.DarkSeaGreen;
         this.panel2.Controls.Add(this.sb_advanced);
         this.panel2.Controls.Add(this.sb_addprivilege);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panel2.Location = new System.Drawing.Point(0, 616);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(775, 78);
         this.panel2.TabIndex = 8;
         // 
         // sb_advanced
         // 
         this.sb_advanced.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_advanced.Appearance.Font = new System.Drawing.Font("B Kamran", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_advanced.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_advanced.Appearance.Options.UseBackColor = true;
         this.sb_advanced.Appearance.Options.UseFont = true;
         this.sb_advanced.Appearance.Options.UseForeColor = true;
         this.sb_advanced.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
         this.sb_advanced.ImageIndex = 6;
         this.sb_advanced.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_advanced.Location = new System.Drawing.Point(42, 19);
         this.sb_advanced.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_advanced.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_advanced.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_advanced.Name = "sb_advanced";
         this.sb_advanced.Size = new System.Drawing.Size(134, 41);
         this.sb_advanced.TabIndex = 15;
         this.sb_advanced.Text = "تنظیمات پیشرفته";
         this.sb_advanced.Click += new System.EventHandler(this.sb_advanced_Click);
         // 
         // sb_addprivilege
         // 
         this.sb_addprivilege.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_addprivilege.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_addprivilege.Appearance.BorderColor = System.Drawing.Color.SlateGray;
         this.sb_addprivilege.Appearance.Font = new System.Drawing.Font("B Kamran", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_addprivilege.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_addprivilege.Appearance.Options.UseBackColor = true;
         this.sb_addprivilege.Appearance.Options.UseBorderColor = true;
         this.sb_addprivilege.Appearance.Options.UseFont = true;
         this.sb_addprivilege.Appearance.Options.UseForeColor = true;
         this.sb_addprivilege.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
         this.sb_addprivilege.ImageIndex = 6;
         this.sb_addprivilege.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_addprivilege.Location = new System.Drawing.Point(591, 19);
         this.sb_addprivilege.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_addprivilege.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_addprivilege.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_addprivilege.Name = "sb_addprivilege";
         this.sb_addprivilege.Size = new System.Drawing.Size(134, 41);
         this.sb_addprivilege.TabIndex = 14;
         this.sb_addprivilege.Text = "تایید و اعمال";
         // 
         // pn_command
         // 
         this.pn_command.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.pn_command.Controls.Add(this.sb_restore);
         this.pn_command.Controls.Add(this.sb_join);
         this.pn_command.Controls.Add(this.sb_leave);
         this.pn_command.Controls.Add(this.sb_active);
         this.pn_command.Controls.Add(this.sb_deactive);
         this.pn_command.Location = new System.Drawing.Point(680, 405);
         this.pn_command.Name = "pn_command";
         this.pn_command.Size = new System.Drawing.Size(45, 205);
         this.pn_command.TabIndex = 27;
         // 
         // sb_restore
         // 
         this.sb_restore.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_restore.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_restore.Appearance.Options.UseFont = true;
         this.sb_restore.Appearance.Options.UseForeColor = true;
         this.sb_restore.ImageIndex = 14;
         this.sb_restore.ImageList = this.il_icons;
         this.sb_restore.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_restore.Location = new System.Drawing.Point(6, 7);
         this.sb_restore.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_restore.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_restore.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_restore.Name = "sb_restore";
         this.sb_restore.Size = new System.Drawing.Size(33, 33);
         this.sb_restore.TabIndex = 15;
         this.sb_restore.Visible = false;
         // 
         // sb_join
         // 
         this.sb_join.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_join.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_join.Appearance.Options.UseFont = true;
         this.sb_join.Appearance.Options.UseForeColor = true;
         this.sb_join.ImageIndex = 6;
         this.sb_join.ImageList = this.il_icons;
         this.sb_join.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_join.Location = new System.Drawing.Point(6, 163);
         this.sb_join.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_join.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_join.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_join.Name = "sb_join";
         this.sb_join.Size = new System.Drawing.Size(33, 33);
         this.sb_join.TabIndex = 13;
         this.sb_join.Click += new System.EventHandler(this.sb_join_Click);
         // 
         // sb_leave
         // 
         this.sb_leave.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_leave.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_leave.Appearance.Options.UseFont = true;
         this.sb_leave.Appearance.Options.UseForeColor = true;
         this.sb_leave.ImageIndex = 7;
         this.sb_leave.ImageList = this.il_icons;
         this.sb_leave.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_leave.Location = new System.Drawing.Point(6, 124);
         this.sb_leave.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_leave.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_leave.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_leave.Name = "sb_leave";
         this.sb_leave.Size = new System.Drawing.Size(33, 33);
         this.sb_leave.TabIndex = 14;
         this.sb_leave.Click += new System.EventHandler(this.sb_leave_Click);
         // 
         // sb_active
         // 
         this.sb_active.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_active.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_active.Appearance.Options.UseFont = true;
         this.sb_active.Appearance.Options.UseForeColor = true;
         this.sb_active.ImageIndex = 2;
         this.sb_active.ImageList = this.il_icons;
         this.sb_active.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_active.Location = new System.Drawing.Point(6, 46);
         this.sb_active.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_active.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_active.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_active.Name = "sb_active";
         this.sb_active.Size = new System.Drawing.Size(33, 33);
         this.sb_active.TabIndex = 11;
         this.sb_active.Click += new System.EventHandler(this.sb_active_Click);
         // 
         // sb_deactive
         // 
         this.sb_deactive.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deactive.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deactive.Appearance.Options.UseFont = true;
         this.sb_deactive.Appearance.Options.UseForeColor = true;
         this.sb_deactive.ImageIndex = 5;
         this.sb_deactive.ImageList = this.il_icons;
         this.sb_deactive.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deactive.Location = new System.Drawing.Point(6, 85);
         this.sb_deactive.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deactive.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deactive.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deactive.Name = "sb_deactive";
         this.sb_deactive.Size = new System.Drawing.Size(33, 33);
         this.sb_deactive.TabIndex = 12;
         this.sb_deactive.Click += new System.EventHandler(this.sb_deactive_Click);
         // 
         // pn_edit
         // 
         this.pn_edit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.pn_edit.Controls.Add(this.sb_selectall);
         this.pn_edit.Controls.Add(this.sb_invertselect);
         this.pn_edit.Controls.Add(this.sb_deselectall);
         this.pn_edit.Controls.Add(this.sb_submit);
         this.pn_edit.Controls.Add(this.sb_cancel);
         this.pn_edit.Location = new System.Drawing.Point(680, 176);
         this.pn_edit.Name = "pn_edit";
         this.pn_edit.Size = new System.Drawing.Size(45, 212);
         this.pn_edit.TabIndex = 26;
         this.pn_edit.Visible = false;
         // 
         // sb_selectall
         // 
         this.sb_selectall.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_selectall.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_selectall.Appearance.Options.UseFont = true;
         this.sb_selectall.Appearance.Options.UseForeColor = true;
         this.sb_selectall.ImageIndex = 13;
         this.sb_selectall.ImageList = this.il_icons;
         this.sb_selectall.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_selectall.Location = new System.Drawing.Point(6, 6);
         this.sb_selectall.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_selectall.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_selectall.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_selectall.Name = "sb_selectall";
         this.sb_selectall.Size = new System.Drawing.Size(33, 33);
         this.sb_selectall.TabIndex = 25;
         this.sb_selectall.Click += new System.EventHandler(this.sb_selectall_Click);
         // 
         // sb_invertselect
         // 
         this.sb_invertselect.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_invertselect.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_invertselect.Appearance.Options.UseFont = true;
         this.sb_invertselect.Appearance.Options.UseForeColor = true;
         this.sb_invertselect.ImageIndex = 11;
         this.sb_invertselect.ImageList = this.il_icons;
         this.sb_invertselect.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_invertselect.Location = new System.Drawing.Point(6, 48);
         this.sb_invertselect.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_invertselect.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_invertselect.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_invertselect.Name = "sb_invertselect";
         this.sb_invertselect.Size = new System.Drawing.Size(33, 33);
         this.sb_invertselect.TabIndex = 23;
         this.sb_invertselect.Click += new System.EventHandler(this.sb_invertselect_Click);
         // 
         // sb_deselectall
         // 
         this.sb_deselectall.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_deselectall.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_deselectall.Appearance.Options.UseFont = true;
         this.sb_deselectall.Appearance.Options.UseForeColor = true;
         this.sb_deselectall.ImageIndex = 12;
         this.sb_deselectall.ImageList = this.il_icons;
         this.sb_deselectall.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_deselectall.Location = new System.Drawing.Point(6, 90);
         this.sb_deselectall.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_deselectall.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_deselectall.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_deselectall.Name = "sb_deselectall";
         this.sb_deselectall.Size = new System.Drawing.Size(33, 33);
         this.sb_deselectall.TabIndex = 24;
         this.sb_deselectall.Click += new System.EventHandler(this.sb_deselectall_Click);
         // 
         // sb_submit
         // 
         this.sb_submit.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_submit.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_submit.Appearance.Options.UseFont = true;
         this.sb_submit.Appearance.Options.UseForeColor = true;
         this.sb_submit.ImageIndex = 8;
         this.sb_submit.ImageList = this.il_icons;
         this.sb_submit.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_submit.Location = new System.Drawing.Point(6, 132);
         this.sb_submit.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_submit.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_submit.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_submit.Name = "sb_submit";
         this.sb_submit.Size = new System.Drawing.Size(33, 33);
         this.sb_submit.TabIndex = 22;
         this.sb_submit.Click += new System.EventHandler(this.sb_submit_Click);
         // 
         // sb_cancel
         // 
         this.sb_cancel.Appearance.Font = new System.Drawing.Font("Symbol", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_cancel.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancel.Appearance.Options.UseFont = true;
         this.sb_cancel.Appearance.Options.UseForeColor = true;
         this.sb_cancel.ImageIndex = 9;
         this.sb_cancel.ImageList = this.il_icons;
         this.sb_cancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.sb_cancel.Location = new System.Drawing.Point(6, 174);
         this.sb_cancel.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancel.Name = "sb_cancel";
         this.sb_cancel.Size = new System.Drawing.Size(33, 33);
         this.sb_cancel.TabIndex = 22;
         this.sb_cancel.Click += new System.EventHandler(this.sb_cancel_Click);
         // 
         // JoinOrLeaveCurrentUserToRoles
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.pn_command);
         this.Controls.Add(this.pn_edit);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.lv_roles);
         this.Controls.Add(this.txt_currentuser);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "JoinOrLeaveCurrentUserToRoles";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(775, 694);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         this.panel2.ResumeLayout(false);
         this.pn_command.ResumeLayout(false);
         this.pn_edit.ResumeLayout(false);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label label1;
      private Windows.Forms.TextBox txt_currentuser;
      private Windows.Forms.ListView lv_roles;
      private Windows.Forms.Label label3;
      private Windows.Forms.Label label4;
      private Windows.Forms.Panel panel2;
      private DevExpress.XtraEditors.SimpleButton sb_advanced;
      private DevExpress.XtraEditors.SimpleButton sb_addprivilege;
      private Windows.Forms.Panel pn_command;
      private DevExpress.XtraEditors.SimpleButton sb_restore;
      private Windows.Forms.ImageList il_icons;
      private DevExpress.XtraEditors.SimpleButton sb_join;
      private DevExpress.XtraEditors.SimpleButton sb_leave;
      private DevExpress.XtraEditors.SimpleButton sb_active;
      private DevExpress.XtraEditors.SimpleButton sb_deactive;
      private Windows.Forms.Panel pn_edit;
      private DevExpress.XtraEditors.SimpleButton sb_selectall;
      private DevExpress.XtraEditors.SimpleButton sb_invertselect;
      private DevExpress.XtraEditors.SimpleButton sb_deselectall;
      private DevExpress.XtraEditors.SimpleButton sb_submit;
      private DevExpress.XtraEditors.SimpleButton sb_cancel;
      private Windows.Forms.ColumnHeader columnHeader1;
   }
}
