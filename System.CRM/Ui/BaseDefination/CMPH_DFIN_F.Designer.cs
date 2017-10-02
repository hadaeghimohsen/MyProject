namespace System.CRM.Ui.BaseDefination
{
   partial class CMPH_DFIN_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMPH_DFIN_F));
         this.panel1 = new System.Windows.Forms.Panel();
         this.Back_Butn = new System.MaxUi.RoundedButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.MyUserAccount_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.Amail_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.LoginMethod_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tp_001 = new System.Windows.Forms.TabPage();
         this.tp_002 = new System.Windows.Forms.TabPage();
         this.tp_003 = new System.Windows.Forms.TabPage();
         this.tp_004 = new System.Windows.Forms.TabPage();
         this.tp_005 = new System.Windows.Forms.TabPage();
         this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
         this.tp_006 = new System.Windows.Forms.TabPage();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         this.flowLayoutPanel1.SuspendLayout();
         this.tabControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(913, 53);
         this.panel1.TabIndex = 0;
         // 
         // Back_Butn
         // 
         this.Back_Butn.Active = true;
         this.Back_Butn.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.Back_Butn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.Back_Butn.Caption = "";
         this.Back_Butn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.Back_Butn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.Back_Butn.HoverBorderColor = System.Drawing.Color.Gold;
         this.Back_Butn.HoverColorA = System.Drawing.Color.LightGray;
         this.Back_Butn.HoverColorB = System.Drawing.Color.LightGray;
         this.Back_Butn.ImageProfile = global::System.CRM.Properties.Resources.IMAGE_1520;
         this.Back_Butn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.Back_Butn.ImageVisiable = true;
         this.Back_Butn.Location = new System.Drawing.Point(4, 3);
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.NormalBorderColor = System.Drawing.Color.LightGray;
         this.Back_Butn.NormalColorA = System.Drawing.Color.White;
         this.Back_Butn.NormalColorB = System.Drawing.Color.White;
         this.Back_Butn.Size = new System.Drawing.Size(48, 48);
         this.Back_Butn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.Back_Butn.TabIndex = 4;
         this.Back_Butn.Tooltip = null;
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("IRAN Sans", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("labelControl1.Appearance.Image")));
         this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
         this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
         this.labelControl1.Location = new System.Drawing.Point(647, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(266, 53);
         this.labelControl1.TabIndex = 1;
         this.labelControl1.Text = "اطلاعات شرکت و شعبه های شما";
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.IsSplitterFixed = true;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 53);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.tabControl1);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Controls.Add(this.flowLayoutPanel1);
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(913, 572);
         this.splitContainerControl1.SplitterPosition = 672;
         this.splitContainerControl1.TabIndex = 1;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Controls.Add(this.MyUserAccount_Butn);
         this.flowLayoutPanel1.Controls.Add(this.Amail_Butn);
         this.flowLayoutPanel1.Controls.Add(this.LoginMethod_Butn);
         this.flowLayoutPanel1.Controls.Add(this.simpleButton1);
         this.flowLayoutPanel1.Controls.Add(this.simpleButton2);
         this.flowLayoutPanel1.Controls.Add(this.simpleButton3);
         this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(236, 572);
         this.flowLayoutPanel1.TabIndex = 0;
         // 
         // MyUserAccount_Butn
         // 
         this.MyUserAccount_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.MyUserAccount_Butn.Appearance.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.MyUserAccount_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.MyUserAccount_Butn.Appearance.Options.UseFont = true;
         this.MyUserAccount_Butn.Appearance.Options.UseForeColor = true;
         this.MyUserAccount_Butn.Appearance.Options.UseTextOptions = true;
         this.MyUserAccount_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.MyUserAccount_Butn.Image = ((System.Drawing.Image)(resources.GetObject("MyUserAccount_Butn.Image")));
         this.MyUserAccount_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.MyUserAccount_Butn.Location = new System.Drawing.Point(18, 3);
         this.MyUserAccount_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.MyUserAccount_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.MyUserAccount_Butn.Name = "MyUserAccount_Butn";
         this.MyUserAccount_Butn.Size = new System.Drawing.Size(215, 46);
         this.MyUserAccount_Butn.TabIndex = 3;
         this.MyUserAccount_Butn.Tag = "1";
         this.MyUserAccount_Butn.Text = "خوش آمد گویی";
         // 
         // Amail_Butn
         // 
         this.Amail_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Amail_Butn.Appearance.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Amail_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Amail_Butn.Appearance.Options.UseFont = true;
         this.Amail_Butn.Appearance.Options.UseForeColor = true;
         this.Amail_Butn.Appearance.Options.UseTextOptions = true;
         this.Amail_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Amail_Butn.Image = ((System.Drawing.Image)(resources.GetObject("Amail_Butn.Image")));
         this.Amail_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.Amail_Butn.Location = new System.Drawing.Point(18, 55);
         this.Amail_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Amail_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Amail_Butn.Name = "Amail_Butn";
         this.Amail_Butn.Size = new System.Drawing.Size(215, 46);
         this.Amail_Butn.TabIndex = 4;
         this.Amail_Butn.Tag = "2";
         this.Amail_Butn.Text = "اطلاعات شرکت و شعبه ها";
         // 
         // LoginMethod_Butn
         // 
         this.LoginMethod_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.LoginMethod_Butn.Appearance.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.LoginMethod_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.LoginMethod_Butn.Appearance.Options.UseFont = true;
         this.LoginMethod_Butn.Appearance.Options.UseForeColor = true;
         this.LoginMethod_Butn.Appearance.Options.UseTextOptions = true;
         this.LoginMethod_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.LoginMethod_Butn.Image = ((System.Drawing.Image)(resources.GetObject("LoginMethod_Butn.Image")));
         this.LoginMethod_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.LoginMethod_Butn.Location = new System.Drawing.Point(18, 107);
         this.LoginMethod_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.LoginMethod_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.LoginMethod_Butn.Name = "LoginMethod_Butn";
         this.LoginMethod_Butn.Size = new System.Drawing.Size(215, 46);
         this.LoginMethod_Butn.TabIndex = 5;
         this.LoginMethod_Butn.Tag = "3";
         this.LoginMethod_Butn.Text = "آدرس و نقشه جغرافیایی";
         // 
         // simpleButton1
         // 
         this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton1.Appearance.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton1.Appearance.Options.UseFont = true;
         this.simpleButton1.Appearance.Options.UseForeColor = true;
         this.simpleButton1.Appearance.Options.UseTextOptions = true;
         this.simpleButton1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
         this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton1.Location = new System.Drawing.Point(18, 159);
         this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(215, 46);
         this.simpleButton1.TabIndex = 6;
         this.simpleButton1.Tag = "3";
         this.simpleButton1.Text = "ایام و ساعت کاری";
         // 
         // simpleButton2
         // 
         this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton2.Appearance.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.simpleButton2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton2.Appearance.Options.UseFont = true;
         this.simpleButton2.Appearance.Options.UseForeColor = true;
         this.simpleButton2.Appearance.Options.UseTextOptions = true;
         this.simpleButton2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton2.Image = global::System.CRM.Properties.Resources.IMAGE_1526;
         this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton2.Location = new System.Drawing.Point(18, 211);
         this.simpleButton2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton2.Name = "simpleButton2";
         this.simpleButton2.Size = new System.Drawing.Size(215, 46);
         this.simpleButton2.TabIndex = 7;
         this.simpleButton2.Tag = "3";
         this.simpleButton2.Text = "اطلاعات تماس";
         // 
         // tabControl1
         // 
         this.tabControl1.Controls.Add(this.tp_001);
         this.tabControl1.Controls.Add(this.tp_002);
         this.tabControl1.Controls.Add(this.tp_003);
         this.tabControl1.Controls.Add(this.tp_004);
         this.tabControl1.Controls.Add(this.tp_005);
         this.tabControl1.Controls.Add(this.tp_006);
         this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tabControl1.Location = new System.Drawing.Point(0, 0);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.RightToLeftLayout = true;
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(672, 572);
         this.tabControl1.TabIndex = 0;
         // 
         // tp_001
         // 
         this.tp_001.Location = new System.Drawing.Point(4, 22);
         this.tp_001.Name = "tp_001";
         this.tp_001.Padding = new System.Windows.Forms.Padding(3);
         this.tp_001.Size = new System.Drawing.Size(664, 375);
         this.tp_001.TabIndex = 0;
         this.tp_001.Text = "خوش آمد گویی";
         this.tp_001.UseVisualStyleBackColor = true;
         // 
         // tp_002
         // 
         this.tp_002.Location = new System.Drawing.Point(4, 22);
         this.tp_002.Name = "tp_002";
         this.tp_002.Padding = new System.Windows.Forms.Padding(3);
         this.tp_002.Size = new System.Drawing.Size(664, 375);
         this.tp_002.TabIndex = 1;
         this.tp_002.Text = "اطلاعات شرکت و شعبه ها";
         this.tp_002.UseVisualStyleBackColor = true;
         // 
         // tp_003
         // 
         this.tp_003.Location = new System.Drawing.Point(4, 22);
         this.tp_003.Name = "tp_003";
         this.tp_003.Size = new System.Drawing.Size(664, 375);
         this.tp_003.TabIndex = 2;
         this.tp_003.Text = "آدرس و نقشه جغرافیایی";
         this.tp_003.UseVisualStyleBackColor = true;
         // 
         // tp_004
         // 
         this.tp_004.Location = new System.Drawing.Point(4, 22);
         this.tp_004.Name = "tp_004";
         this.tp_004.Size = new System.Drawing.Size(664, 375);
         this.tp_004.TabIndex = 3;
         this.tp_004.Text = "ایام و ساعت کاری";
         this.tp_004.UseVisualStyleBackColor = true;
         // 
         // tp_005
         // 
         this.tp_005.Location = new System.Drawing.Point(4, 22);
         this.tp_005.Name = "tp_005";
         this.tp_005.Size = new System.Drawing.Size(664, 375);
         this.tp_005.TabIndex = 4;
         this.tp_005.Text = "اطلاعات تماس";
         this.tp_005.UseVisualStyleBackColor = true;
         // 
         // simpleButton3
         // 
         this.simpleButton3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton3.Appearance.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.simpleButton3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton3.Appearance.Options.UseFont = true;
         this.simpleButton3.Appearance.Options.UseForeColor = true;
         this.simpleButton3.Appearance.Options.UseTextOptions = true;
         this.simpleButton3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton3.Image = global::System.CRM.Properties.Resources.IMAGE_1195;
         this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton3.Location = new System.Drawing.Point(18, 263);
         this.simpleButton3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton3.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton3.Name = "simpleButton3";
         this.simpleButton3.Size = new System.Drawing.Size(215, 46);
         this.simpleButton3.TabIndex = 8;
         this.simpleButton3.Tag = "3";
         this.simpleButton3.Text = "ثبت و دخیره نهایی";
         // 
         // tp_006
         // 
         this.tp_006.Location = new System.Drawing.Point(4, 22);
         this.tp_006.Name = "tp_006";
         this.tp_006.Size = new System.Drawing.Size(664, 546);
         this.tp_006.TabIndex = 5;
         this.tp_006.Text = "ثبت و ذخیره نهایی";
         this.tp_006.UseVisualStyleBackColor = true;
         // 
         // CMPH_DFIN_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.splitContainerControl1);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "CMPH_DFIN_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(913, 625);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         this.flowLayoutPanel1.ResumeLayout(false);
         this.tabControl1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private MaxUi.RoundedButton Back_Butn;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private DevExpress.XtraEditors.SimpleButton MyUserAccount_Butn;
      private DevExpress.XtraEditors.SimpleButton Amail_Butn;
      private DevExpress.XtraEditors.SimpleButton LoginMethod_Butn;
      private DevExpress.XtraEditors.SimpleButton simpleButton1;
      private DevExpress.XtraEditors.SimpleButton simpleButton2;
      private Windows.Forms.TabControl tabControl1;
      private Windows.Forms.TabPage tp_001;
      private Windows.Forms.TabPage tp_002;
      private Windows.Forms.TabPage tp_003;
      private Windows.Forms.TabPage tp_004;
      private Windows.Forms.TabPage tp_005;
      private Windows.Forms.TabPage tp_006;
      private DevExpress.XtraEditors.SimpleButton simpleButton3;
   }
}
