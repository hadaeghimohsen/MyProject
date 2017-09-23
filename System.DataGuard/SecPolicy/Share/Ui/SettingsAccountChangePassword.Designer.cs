namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SettingsAccountChangePassword
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsAccountChangePassword));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
         this.panel1 = new System.Windows.Forms.Panel();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.Back_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.UserBs = new System.Windows.Forms.BindingSource(this.components);
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.FirstStep_Pn = new System.Windows.Forms.Panel();
         this.CurrentPassword_Be = new DevExpress.XtraEditors.ButtonEdit();
         this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
         this.ImageAccount_Pb = new System.Windows.Forms.PictureBox();
         this.CurrentPassword_Lb = new DevExpress.XtraEditors.LabelControl();
         this.StepOneCurrentPassword_Lb = new DevExpress.XtraEditors.LabelControl();
         this.SecondStep_Butn = new System.Windows.Forms.Panel();
         this.ReenterNewPassword_Be = new DevExpress.XtraEditors.ButtonEdit();
         this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
         this.NewPassword_Be = new DevExpress.XtraEditors.ButtonEdit();
         this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
         this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
         this.Save_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.UserBs)).BeginInit();
         this.flowLayoutPanel1.SuspendLayout();
         this.FirstStep_Pn.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.CurrentPassword_Be.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImageAccount_Pb)).BeginInit();
         this.SecondStep_Butn.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ReenterNewPassword_Be.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.NewPassword_Be.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(651, 59);
         this.panel1.TabIndex = 0;
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.labelControl1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("labelControl1.Appearance.Image")));
         this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
         this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl1.Location = new System.Drawing.Point(410, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(180, 59);
         this.labelControl1.TabIndex = 1;
         this.labelControl1.Text = "تغییر رمز کاربری";
         // 
         // Back_Butn
         // 
         this.Back_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.Back_Butn.Appearance.Options.UseBackColor = true;
         this.Back_Butn.Dock = System.Windows.Forms.DockStyle.Right;
         this.Back_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1371;
         this.Back_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Back_Butn.Location = new System.Drawing.Point(590, 0);
         this.Back_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Back_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.Size = new System.Drawing.Size(61, 59);
         this.Back_Butn.TabIndex = 0;
         this.Back_Butn.ToolTip = "بازگشت";
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // UserBs
         // 
         this.UserBs.DataSource = typeof(System.DataGuard.Data.User);
         this.UserBs.CurrentChanged += new System.EventHandler(this.UserBs_CurrentChanged);
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.flowLayoutPanel1.AutoScroll = true;
         this.flowLayoutPanel1.Controls.Add(this.FirstStep_Pn);
         this.flowLayoutPanel1.Controls.Add(this.SecondStep_Butn);
         this.flowLayoutPanel1.Location = new System.Drawing.Point(39, 88);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(571, 470);
         this.flowLayoutPanel1.TabIndex = 0;
         // 
         // FirstStep_Pn
         // 
         this.FirstStep_Pn.Controls.Add(this.CurrentPassword_Be);
         this.FirstStep_Pn.Controls.Add(this.labelControl3);
         this.FirstStep_Pn.Controls.Add(this.ImageAccount_Pb);
         this.FirstStep_Pn.Controls.Add(this.CurrentPassword_Lb);
         this.FirstStep_Pn.Controls.Add(this.StepOneCurrentPassword_Lb);
         this.FirstStep_Pn.Location = new System.Drawing.Point(139, 3);
         this.FirstStep_Pn.Name = "FirstStep_Pn";
         this.FirstStep_Pn.Size = new System.Drawing.Size(429, 190);
         this.FirstStep_Pn.TabIndex = 0;
         // 
         // CurrentPassword_Be
         // 
         this.CurrentPassword_Be.EditValue = "";
         this.CurrentPassword_Be.Location = new System.Drawing.Point(58, 148);
         this.CurrentPassword_Be.Name = "CurrentPassword_Be";
         this.CurrentPassword_Be.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
         this.CurrentPassword_Be.Properties.Appearance.BorderColor = System.Drawing.Color.Gray;
         this.CurrentPassword_Be.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.CurrentPassword_Be.Properties.Appearance.Options.UseBackColor = true;
         this.CurrentPassword_Be.Properties.Appearance.Options.UseBorderColor = true;
         this.CurrentPassword_Be.Properties.Appearance.Options.UseFont = true;
         this.CurrentPassword_Be.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.CurrentPassword_Be.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         this.CurrentPassword_Be.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.CurrentPassword_Be.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.CurrentPassword_Be.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.CurrentPassword_Be.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("CurrentPassword_Be.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.CurrentPassword_Be.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.CurrentPassword_Be.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.CurrentPassword_Be.Properties.UseSystemPasswordChar = true;
         this.CurrentPassword_Be.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.CurrentPassword_Be.Size = new System.Drawing.Size(237, 26);
         this.CurrentPassword_Be.TabIndex = 0;
         // 
         // labelControl3
         // 
         this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl3.Appearance.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.labelControl3.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UserBs, "TitleEn", true));
         this.labelControl3.Location = new System.Drawing.Point(158, 36);
         this.labelControl3.Name = "labelControl3";
         this.labelControl3.Size = new System.Drawing.Size(165, 29);
         this.labelControl3.TabIndex = 18;
         this.labelControl3.Text = "ArtaUser";
         // 
         // ImageAccount_Pb
         // 
         this.ImageAccount_Pb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.ImageAccount_Pb.DataBindings.Add(new System.Windows.Forms.Binding("Image", this.UserBs, "USER_IMAG", true));
         this.ImageAccount_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         this.ImageAccount_Pb.Location = new System.Drawing.Point(329, 36);
         this.ImageAccount_Pb.Name = "ImageAccount_Pb";
         this.ImageAccount_Pb.Size = new System.Drawing.Size(85, 85);
         this.ImageAccount_Pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.ImageAccount_Pb.TabIndex = 17;
         this.ImageAccount_Pb.TabStop = false;
         // 
         // CurrentPassword_Lb
         // 
         this.CurrentPassword_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.CurrentPassword_Lb.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.CurrentPassword_Lb.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.CurrentPassword_Lb.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.CurrentPassword_Lb.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.CurrentPassword_Lb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.CurrentPassword_Lb.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.CurrentPassword_Lb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.CurrentPassword_Lb.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.CurrentPassword_Lb.Location = new System.Drawing.Point(316, 146);
         this.CurrentPassword_Lb.Name = "CurrentPassword_Lb";
         this.CurrentPassword_Lb.Size = new System.Drawing.Size(98, 33);
         this.CurrentPassword_Lb.TabIndex = 16;
         this.CurrentPassword_Lb.Text = "رمز فعلی";
         // 
         // StepOneCurrentPassword_Lb
         // 
         this.StepOneCurrentPassword_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.StepOneCurrentPassword_Lb.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.StepOneCurrentPassword_Lb.Appearance.Font = new System.Drawing.Font("B Roya", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.StepOneCurrentPassword_Lb.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.StepOneCurrentPassword_Lb.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.StepOneCurrentPassword_Lb.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.StepOneCurrentPassword_Lb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.StepOneCurrentPassword_Lb.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.StepOneCurrentPassword_Lb.Location = new System.Drawing.Point(19, 3);
         this.StepOneCurrentPassword_Lb.Name = "StepOneCurrentPassword_Lb";
         this.StepOneCurrentPassword_Lb.Size = new System.Drawing.Size(407, 27);
         this.StepOneCurrentPassword_Lb.TabIndex = 16;
         this.StepOneCurrentPassword_Lb.Text = "گام اول، رمز فعلی خود را وارد کنید";
         // 
         // SecondStep_Butn
         // 
         this.SecondStep_Butn.Controls.Add(this.ReenterNewPassword_Be);
         this.SecondStep_Butn.Controls.Add(this.labelControl4);
         this.SecondStep_Butn.Controls.Add(this.NewPassword_Be);
         this.SecondStep_Butn.Controls.Add(this.labelControl5);
         this.SecondStep_Butn.Controls.Add(this.labelControl6);
         this.SecondStep_Butn.Location = new System.Drawing.Point(139, 199);
         this.SecondStep_Butn.Name = "SecondStep_Butn";
         this.SecondStep_Butn.Size = new System.Drawing.Size(429, 188);
         this.SecondStep_Butn.TabIndex = 1;
         // 
         // ReenterNewPassword_Be
         // 
         this.ReenterNewPassword_Be.EditValue = "";
         this.ReenterNewPassword_Be.Location = new System.Drawing.Point(58, 97);
         this.ReenterNewPassword_Be.Name = "ReenterNewPassword_Be";
         this.ReenterNewPassword_Be.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
         this.ReenterNewPassword_Be.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ReenterNewPassword_Be.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ReenterNewPassword_Be.Properties.Appearance.Options.UseBackColor = true;
         this.ReenterNewPassword_Be.Properties.Appearance.Options.UseBorderColor = true;
         this.ReenterNewPassword_Be.Properties.Appearance.Options.UseFont = true;
         this.ReenterNewPassword_Be.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.ReenterNewPassword_Be.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         this.ReenterNewPassword_Be.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.ReenterNewPassword_Be.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.ReenterNewPassword_Be.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.ReenterNewPassword_Be.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("ReenterNewPassword_Be.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
         this.ReenterNewPassword_Be.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.ReenterNewPassword_Be.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ReenterNewPassword_Be.Properties.UseSystemPasswordChar = true;
         this.ReenterNewPassword_Be.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.ReenterNewPassword_Be.Size = new System.Drawing.Size(237, 26);
         this.ReenterNewPassword_Be.TabIndex = 1;
         // 
         // labelControl4
         // 
         this.labelControl4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl4.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.labelControl4.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl4.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.labelControl4.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.labelControl4.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl4.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl4.Location = new System.Drawing.Point(311, 95);
         this.labelControl4.Name = "labelControl4";
         this.labelControl4.Size = new System.Drawing.Size(103, 33);
         this.labelControl4.TabIndex = 16;
         this.labelControl4.Text = "تکرار رمز جدید";
         // 
         // NewPassword_Be
         // 
         this.NewPassword_Be.EditValue = "";
         this.NewPassword_Be.Location = new System.Drawing.Point(58, 65);
         this.NewPassword_Be.Name = "NewPassword_Be";
         this.NewPassword_Be.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
         this.NewPassword_Be.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.NewPassword_Be.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.NewPassword_Be.Properties.Appearance.Options.UseBackColor = true;
         this.NewPassword_Be.Properties.Appearance.Options.UseBorderColor = true;
         this.NewPassword_Be.Properties.Appearance.Options.UseFont = true;
         this.NewPassword_Be.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.NewPassword_Be.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         this.NewPassword_Be.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.NewPassword_Be.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.NewPassword_Be.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.NewPassword_Be.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("NewPassword_Be.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
         this.NewPassword_Be.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.NewPassword_Be.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.NewPassword_Be.Properties.UseSystemPasswordChar = true;
         this.NewPassword_Be.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.NewPassword_Be.Size = new System.Drawing.Size(237, 26);
         this.NewPassword_Be.TabIndex = 0;
         // 
         // labelControl5
         // 
         this.labelControl5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl5.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.labelControl5.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl5.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.labelControl5.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.labelControl5.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl5.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl5.Location = new System.Drawing.Point(311, 63);
         this.labelControl5.Name = "labelControl5";
         this.labelControl5.Size = new System.Drawing.Size(103, 33);
         this.labelControl5.TabIndex = 16;
         this.labelControl5.Text = "رمز جدید";
         // 
         // labelControl6
         // 
         this.labelControl6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl6.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.labelControl6.Appearance.Font = new System.Drawing.Font("B Roya", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl6.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.labelControl6.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl6.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl6.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl6.Location = new System.Drawing.Point(19, 3);
         this.labelControl6.Name = "labelControl6";
         this.labelControl6.Size = new System.Drawing.Size(407, 27);
         this.labelControl6.TabIndex = 16;
         this.labelControl6.Text = "گام دوم، رمز جدید خود را وارد کنید";
         // 
         // Save_Butn
         // 
         this.Save_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.Save_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         this.Save_Butn.Appearance.Font = new System.Drawing.Font("B Traffic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Save_Butn.Appearance.ForeColor = System.Drawing.SystemColors.ButtonFace;
         this.Save_Butn.Appearance.Options.UseBackColor = true;
         this.Save_Butn.Appearance.Options.UseFont = true;
         this.Save_Butn.Appearance.Options.UseForeColor = true;
         this.Save_Butn.Appearance.Options.UseTextOptions = true;
         this.Save_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Save_Butn.Image = ((System.Drawing.Image)(resources.GetObject("Save_Butn.Image")));
         this.Save_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.Save_Butn.Location = new System.Drawing.Point(494, 580);
         this.Save_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Save_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Save_Butn.Name = "Save_Butn";
         this.Save_Butn.Size = new System.Drawing.Size(116, 42);
         this.Save_Butn.TabIndex = 1;
         this.Save_Butn.Text = "ذخیره";
         this.Save_Butn.Click += new System.EventHandler(this.Save_Butn_Click);
         // 
         // SettingsAccountChangePassword
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Highlight;
         this.Controls.Add(this.flowLayoutPanel1);
         this.Controls.Add(this.Save_Butn);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "SettingsAccountChangePassword";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(651, 660);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.UserBs)).EndInit();
         this.flowLayoutPanel1.ResumeLayout(false);
         this.FirstStep_Pn.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.CurrentPassword_Be.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImageAccount_Pb)).EndInit();
         this.SecondStep_Butn.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ReenterNewPassword_Be.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.NewPassword_Be.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton Back_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private Windows.Forms.BindingSource UserBs;
      private Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private Windows.Forms.Panel FirstStep_Pn;
      private DevExpress.XtraEditors.LabelControl StepOneCurrentPassword_Lb;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      private Windows.Forms.PictureBox ImageAccount_Pb;
      private DevExpress.XtraEditors.ButtonEdit CurrentPassword_Be;
      private DevExpress.XtraEditors.LabelControl CurrentPassword_Lb;
      private Windows.Forms.Panel SecondStep_Butn;
      private DevExpress.XtraEditors.ButtonEdit ReenterNewPassword_Be;
      private DevExpress.XtraEditors.LabelControl labelControl4;
      private DevExpress.XtraEditors.ButtonEdit NewPassword_Be;
      private DevExpress.XtraEditors.LabelControl labelControl5;
      private DevExpress.XtraEditors.LabelControl labelControl6;
      private DevExpress.XtraEditors.SimpleButton Save_Butn;
   }
}
