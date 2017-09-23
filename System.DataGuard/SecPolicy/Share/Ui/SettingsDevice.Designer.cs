namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SettingsDevice
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
         this.panel1 = new System.Windows.Forms.Panel();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.Back_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.Tb_Master = new System.Windows.Forms.TabControl();
         this.tp_001 = new System.Windows.Forms.TabPage();
         this.ActiveSessionList_Flp = new System.Windows.Forms.FlowLayoutPanel();
         this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
         this.ActiveSessionBs = new System.Windows.Forms.BindingSource(this.components);
         this.tp_002 = new System.Windows.Forms.TabPage();
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.ClientList_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.OtherDevice_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         this.Tb_Master.SuspendLayout();
         this.tp_001.SuspendLayout();
         this.ActiveSessionList_Flp.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ActiveSessionBs)).BeginInit();
         this.flowLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(762, 59);
         this.panel1.TabIndex = 0;
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1362;
         this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
         this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl1.Location = new System.Drawing.Point(496, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(205, 59);
         this.labelControl1.TabIndex = 1;
         this.labelControl1.Text = "دستگاه های متصل";
         // 
         // Back_Butn
         // 
         this.Back_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(36)))), ((int)(((byte)(248)))));
         this.Back_Butn.Appearance.Options.UseBackColor = true;
         this.Back_Butn.Dock = System.Windows.Forms.DockStyle.Right;
         this.Back_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1371;
         this.Back_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Back_Butn.Location = new System.Drawing.Point(701, 0);
         this.Back_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Back_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.Size = new System.Drawing.Size(61, 59);
         this.Back_Butn.TabIndex = 0;
         this.Back_Butn.ToolTip = "بازگشت";
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
         this.splitContainerControl1.IsSplitterFixed = true;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 59);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.Tb_Master);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Controls.Add(this.flowLayoutPanel1);
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(762, 459);
         this.splitContainerControl1.SplitterPosition = 219;
         this.splitContainerControl1.TabIndex = 1;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // Tb_Master
         // 
         this.Tb_Master.Controls.Add(this.tp_001);
         this.Tb_Master.Controls.Add(this.tp_002);
         this.Tb_Master.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Tb_Master.Location = new System.Drawing.Point(0, 0);
         this.Tb_Master.Name = "Tb_Master";
         this.Tb_Master.RightToLeftLayout = true;
         this.Tb_Master.SelectedIndex = 0;
         this.Tb_Master.Size = new System.Drawing.Size(538, 459);
         this.Tb_Master.TabIndex = 0;
         // 
         // tp_001
         // 
         this.tp_001.Controls.Add(this.ActiveSessionList_Flp);
         this.tp_001.Location = new System.Drawing.Point(4, 23);
         this.tp_001.Name = "tp_001";
         this.tp_001.Padding = new System.Windows.Forms.Padding(3);
         this.tp_001.Size = new System.Drawing.Size(530, 432);
         this.tp_001.TabIndex = 0;
         this.tp_001.Tag = "1";
         this.tp_001.Text = "سیستم های متصل";
         this.tp_001.UseVisualStyleBackColor = true;
         // 
         // ActiveSessionList_Flp
         // 
         this.ActiveSessionList_Flp.Controls.Add(this.simpleButton1);
         this.ActiveSessionList_Flp.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ActiveSessionList_Flp.Location = new System.Drawing.Point(3, 3);
         this.ActiveSessionList_Flp.Name = "ActiveSessionList_Flp";
         this.ActiveSessionList_Flp.Size = new System.Drawing.Size(524, 426);
         this.ActiveSessionList_Flp.TabIndex = 1;
         // 
         // simpleButton1
         // 
         this.simpleButton1.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton1.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton1.Appearance.Options.UseFont = true;
         this.simpleButton1.Appearance.Options.UseForeColor = true;
         this.simpleButton1.Appearance.Options.UseTextOptions = true;
         this.simpleButton1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton1.Image = global::System.DataGuard.Properties.Resources.IMAGE_1415;
         this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton1.Location = new System.Drawing.Point(306, 3);
         this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(215, 72);
         this.simpleButton1.TabIndex = 1;
         this.simpleButton1.Tag = "1";
         this.simpleButton1.Text = "<b>mohsen-lt</b><br><color=Gray><size=12>artauser</size></color><br>";
         // 
         // ActiveSessionBs
         // 
         this.ActiveSessionBs.DataSource = typeof(System.DataGuard.Data.Active_Session);
         // 
         // tp_002
         // 
         this.tp_002.Location = new System.Drawing.Point(4, 23);
         this.tp_002.Name = "tp_002";
         this.tp_002.Padding = new System.Windows.Forms.Padding(3);
         this.tp_002.Size = new System.Drawing.Size(530, 432);
         this.tp_002.TabIndex = 1;
         this.tp_002.Tag = "2";
         this.tp_002.Text = "دستگاه های جانبی متصل";
         this.tp_002.UseVisualStyleBackColor = true;
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Controls.Add(this.ClientList_Butn);
         this.flowLayoutPanel1.Controls.Add(this.OtherDevice_Butn);
         this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(219, 459);
         this.flowLayoutPanel1.TabIndex = 1;
         // 
         // ClientList_Butn
         // 
         this.ClientList_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ClientList_Butn.Appearance.Font = new System.Drawing.Font("B Traffic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.ClientList_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ClientList_Butn.Appearance.Options.UseFont = true;
         this.ClientList_Butn.Appearance.Options.UseForeColor = true;
         this.ClientList_Butn.Appearance.Options.UseTextOptions = true;
         this.ClientList_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.ClientList_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1413;
         this.ClientList_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.ClientList_Butn.Location = new System.Drawing.Point(1, 3);
         this.ClientList_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.ClientList_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ClientList_Butn.Name = "ClientList_Butn";
         this.ClientList_Butn.Size = new System.Drawing.Size(215, 46);
         this.ClientList_Butn.TabIndex = 0;
         this.ClientList_Butn.Tag = "1";
         this.ClientList_Butn.Text = "سیستم های متصل";
         this.ClientList_Butn.Click += new System.EventHandler(this.RightButns_Click);
         // 
         // OtherDevice_Butn
         // 
         this.OtherDevice_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.OtherDevice_Butn.Appearance.Font = new System.Drawing.Font("B Traffic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.OtherDevice_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.OtherDevice_Butn.Appearance.Options.UseFont = true;
         this.OtherDevice_Butn.Appearance.Options.UseForeColor = true;
         this.OtherDevice_Butn.Appearance.Options.UseTextOptions = true;
         this.OtherDevice_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.OtherDevice_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1375;
         this.OtherDevice_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.OtherDevice_Butn.Location = new System.Drawing.Point(1, 55);
         this.OtherDevice_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.OtherDevice_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.OtherDevice_Butn.Name = "OtherDevice_Butn";
         this.OtherDevice_Butn.Size = new System.Drawing.Size(215, 46);
         this.OtherDevice_Butn.TabIndex = 1;
         this.OtherDevice_Butn.Tag = "2";
         this.OtherDevice_Butn.Text = "دستگاه های جانبی متصل";
         this.OtherDevice_Butn.Click += new System.EventHandler(this.RightButns_Click);
         // 
         // SettingsDevice
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.splitContainerControl1);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "SettingsDevice";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(762, 518);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         this.Tb_Master.ResumeLayout(false);
         this.tp_001.ResumeLayout(false);
         this.ActiveSessionList_Flp.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ActiveSessionBs)).EndInit();
         this.flowLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton Back_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private DevExpress.XtraEditors.SimpleButton ClientList_Butn;
      private DevExpress.XtraEditors.SimpleButton OtherDevice_Butn;
      private Windows.Forms.TabControl Tb_Master;
      private Windows.Forms.TabPage tp_001;
      private Windows.Forms.TabPage tp_002;
      private Windows.Forms.BindingSource ActiveSessionBs;
      private Windows.Forms.FlowLayoutPanel ActiveSessionList_Flp;
      private DevExpress.XtraEditors.SimpleButton simpleButton1;

   }
}
