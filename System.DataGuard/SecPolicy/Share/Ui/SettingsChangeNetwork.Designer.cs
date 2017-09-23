namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SettingsChangeNetwork
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsChangeNetwork));
         this.panel1 = new System.Windows.Forms.Panel();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.Back_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
         this.SourceUserName_Lb = new DevExpress.XtraEditors.LabelControl();
         this.ImageAccount_Pb = new System.Windows.Forms.PictureBox();
         this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
         this.HostName_Lbl = new DevExpress.XtraEditors.LabelControl();
         this.AccessUserDatasource_Flp = new System.Windows.Forms.FlowLayoutPanel();
         this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
         this.UserBs = new System.Windows.Forms.BindingSource(this.components);
         this.SubSysBs = new System.Windows.Forms.BindingSource(this.components);
         this.AccessUserDatasourceBs = new System.Windows.Forms.BindingSource(this.components);
         this.GatewayBs = new System.Windows.Forms.BindingSource(this.components);
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ImageAccount_Pb)).BeginInit();
         this.AccessUserDatasource_Flp.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.UserBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.SubSysBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.AccessUserDatasourceBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.GatewayBs)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(583, 59);
         this.panel1.TabIndex = 0;
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.labelControl1.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("labelControl1.Appearance.Image")));
         this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
         this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl1.Location = new System.Drawing.Point(362, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(160, 59);
         this.labelControl1.TabIndex = 1;
         this.labelControl1.Text = "تغییر پل ارتباطی";
         // 
         // Back_Butn
         // 
         this.Back_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.Back_Butn.Appearance.Options.UseBackColor = true;
         this.Back_Butn.Dock = System.Windows.Forms.DockStyle.Right;
         this.Back_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1371;
         this.Back_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Back_Butn.Location = new System.Drawing.Point(522, 0);
         this.Back_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Back_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.Size = new System.Drawing.Size(61, 59);
         this.Back_Butn.TabIndex = 0;
         this.Back_Butn.ToolTip = "بازگشت";
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // labelControl6
         // 
         this.labelControl6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl6.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.labelControl6.Appearance.Font = new System.Drawing.Font("B Mitra", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl6.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.labelControl6.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1398;
         this.labelControl6.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl6.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl6.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.SubSysBs, "DESC", true));
         this.labelControl6.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl6.Location = new System.Drawing.Point(20, 191);
         this.labelControl6.Name = "labelControl6";
         this.labelControl6.Size = new System.Drawing.Size(538, 32);
         this.labelControl6.TabIndex = 16;
         this.labelControl6.Text = "هسته نرم افزار انار";
         // 
         // SourceUserName_Lb
         // 
         this.SourceUserName_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.SourceUserName_Lb.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.SourceUserName_Lb.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.SourceUserName_Lb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.SourceUserName_Lb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.SourceUserName_Lb.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.UserBs, "USERDB", true));
         this.SourceUserName_Lb.Location = new System.Drawing.Point(343, 156);
         this.SourceUserName_Lb.Name = "SourceUserName_Lb";
         this.SourceUserName_Lb.Size = new System.Drawing.Size(165, 29);
         this.SourceUserName_Lb.TabIndex = 22;
         this.SourceUserName_Lb.Text = "User Name";
         // 
         // ImageAccount_Pb
         // 
         this.ImageAccount_Pb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.ImageAccount_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         this.ImageAccount_Pb.Location = new System.Drawing.Point(387, 65);
         this.ImageAccount_Pb.Name = "ImageAccount_Pb";
         this.ImageAccount_Pb.Size = new System.Drawing.Size(85, 85);
         this.ImageAccount_Pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.ImageAccount_Pb.TabIndex = 21;
         this.ImageAccount_Pb.TabStop = false;
         // 
         // labelControl3
         // 
         this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl3.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.labelControl3.Appearance.Font = new System.Drawing.Font("B Mitra", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl3.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.labelControl3.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1415;
         this.labelControl3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl3.Location = new System.Drawing.Point(114, 65);
         this.labelControl3.Name = "labelControl3";
         this.labelControl3.Size = new System.Drawing.Size(85, 85);
         this.labelControl3.TabIndex = 23;
         // 
         // HostName_Lbl
         // 
         this.HostName_Lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.HostName_Lbl.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.HostName_Lbl.Appearance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
         this.HostName_Lbl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.HostName_Lbl.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.HostName_Lbl.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.GatewayBs, "COMP_NAME_DNRM", true));
         this.HostName_Lbl.Location = new System.Drawing.Point(92, 156);
         this.HostName_Lbl.Name = "HostName_Lbl";
         this.HostName_Lbl.Size = new System.Drawing.Size(128, 29);
         this.HostName_Lbl.TabIndex = 22;
         this.HostName_Lbl.Text = "Host Name";
         // 
         // AccessUserDatasource_Flp
         // 
         this.AccessUserDatasource_Flp.Controls.Add(this.simpleButton2);
         this.AccessUserDatasource_Flp.Controls.Add(this.simpleButton3);
         this.AccessUserDatasource_Flp.Location = new System.Drawing.Point(20, 229);
         this.AccessUserDatasource_Flp.Name = "AccessUserDatasource_Flp";
         this.AccessUserDatasource_Flp.Size = new System.Drawing.Size(538, 309);
         this.AccessUserDatasource_Flp.TabIndex = 24;
         // 
         // simpleButton2
         // 
         this.simpleButton2.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton2.Appearance.BackColor = System.Drawing.Color.SkyBlue;
         this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton2.Appearance.Options.UseBackColor = true;
         this.simpleButton2.Appearance.Options.UseFont = true;
         this.simpleButton2.Appearance.Options.UseForeColor = true;
         this.simpleButton2.Appearance.Options.UseTextOptions = true;
         this.simpleButton2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton2.Image = global::System.DataGuard.Properties.Resources.IMAGE_1421;
         this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton2.Location = new System.Drawing.Point(366, 3);
         this.simpleButton2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton2.Name = "simpleButton2";
         this.simpleButton2.Size = new System.Drawing.Size(169, 57);
         this.simpleButton2.TabIndex = 4;
         this.simpleButton2.Tag = "1";
         this.simpleButton2.Text = "<b>سرور اصلی انار</b><br><color=DimGray><size=9>192.168.1.10</size></color><br>";
         // 
         // simpleButton3
         // 
         this.simpleButton3.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton3.Appearance.BackColor = System.Drawing.Color.Gainsboro;
         this.simpleButton3.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton3.Appearance.Options.UseBackColor = true;
         this.simpleButton3.Appearance.Options.UseFont = true;
         this.simpleButton3.Appearance.Options.UseForeColor = true;
         this.simpleButton3.Appearance.Options.UseTextOptions = true;
         this.simpleButton3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton3.Image = global::System.DataGuard.Properties.Resources.IMAGE_1384;
         this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton3.Location = new System.Drawing.Point(191, 3);
         this.simpleButton3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton3.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton3.Name = "simpleButton3";
         this.simpleButton3.Size = new System.Drawing.Size(169, 57);
         this.simpleButton3.TabIndex = 5;
         this.simpleButton3.Tag = "1";
         this.simpleButton3.Text = "<b><u>Amir</u></b><br><color=DimGray><size=9>پیمان محمدی</size></color><br>";
         // 
         // UserBs
         // 
         this.UserBs.DataSource = typeof(System.DataGuard.Data.User);
         // 
         // SubSysBs
         // 
         this.SubSysBs.DataSource = typeof(System.DataGuard.Data.Sub_System);
         // 
         // AccessUserDatasourceBs
         // 
         this.AccessUserDatasourceBs.DataSource = typeof(System.DataGuard.Data.Access_User_Datasource);
         // 
         // GatewayBs
         // 
         this.GatewayBs.DataSource = typeof(System.DataGuard.Data.Gateway);
         // 
         // SettingsChangeNetwork
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlLight;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.AccessUserDatasource_Flp);
         this.Controls.Add(this.labelControl3);
         this.Controls.Add(this.HostName_Lbl);
         this.Controls.Add(this.SourceUserName_Lb);
         this.Controls.Add(this.ImageAccount_Pb);
         this.Controls.Add(this.labelControl6);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "SettingsChangeNetwork";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(583, 564);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ImageAccount_Pb)).EndInit();
         this.AccessUserDatasource_Flp.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.UserBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.SubSysBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.AccessUserDatasourceBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.GatewayBs)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton Back_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl6;
      private DevExpress.XtraEditors.LabelControl SourceUserName_Lb;
      private Windows.Forms.PictureBox ImageAccount_Pb;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      private DevExpress.XtraEditors.LabelControl HostName_Lbl;
      private Windows.Forms.FlowLayoutPanel AccessUserDatasource_Flp;
      private DevExpress.XtraEditors.SimpleButton simpleButton2;
      private DevExpress.XtraEditors.SimpleButton simpleButton3;
      private Windows.Forms.BindingSource UserBs;
      private Windows.Forms.BindingSource SubSysBs;
      private Windows.Forms.BindingSource AccessUserDatasourceBs;
      private Windows.Forms.BindingSource GatewayBs;

   }
}
