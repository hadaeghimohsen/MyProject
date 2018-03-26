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
         this.tp_002 = new System.Windows.Forms.TabPage();
         this.tp_003 = new System.Windows.Forms.TabPage();
         this.NewPos_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
         this.PosList_Flp = new System.Windows.Forms.FlowLayoutPanel();
         this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.ClientList_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.OtherDevice_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.Pos_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.ActiveSessionBs = new System.Windows.Forms.BindingSource(this.components);
         this.PosBs = new System.Windows.Forms.BindingSource(this.components);
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         this.Tb_Master.SuspendLayout();
         this.tp_001.SuspendLayout();
         this.ActiveSessionList_Flp.SuspendLayout();
         this.tp_003.SuspendLayout();
         this.PosList_Flp.SuspendLayout();
         this.flowLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ActiveSessionBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.PosBs)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(1184, 59);
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
         this.labelControl1.Location = new System.Drawing.Point(918, 0);
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
         this.Back_Butn.Location = new System.Drawing.Point(1123, 0);
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
         this.splitContainerControl1.Size = new System.Drawing.Size(1184, 429);
         this.splitContainerControl1.SplitterPosition = 219;
         this.splitContainerControl1.TabIndex = 1;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // Tb_Master
         // 
         this.Tb_Master.Controls.Add(this.tp_001);
         this.Tb_Master.Controls.Add(this.tp_002);
         this.Tb_Master.Controls.Add(this.tp_003);
         this.Tb_Master.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Tb_Master.Location = new System.Drawing.Point(0, 0);
         this.Tb_Master.Name = "Tb_Master";
         this.Tb_Master.RightToLeftLayout = true;
         this.Tb_Master.SelectedIndex = 0;
         this.Tb_Master.Size = new System.Drawing.Size(960, 429);
         this.Tb_Master.TabIndex = 0;
         // 
         // tp_001
         // 
         this.tp_001.Controls.Add(this.ActiveSessionList_Flp);
         this.tp_001.Location = new System.Drawing.Point(4, 23);
         this.tp_001.Name = "tp_001";
         this.tp_001.Padding = new System.Windows.Forms.Padding(3);
         this.tp_001.Size = new System.Drawing.Size(952, 402);
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
         this.ActiveSessionList_Flp.Size = new System.Drawing.Size(946, 396);
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
         this.simpleButton1.Location = new System.Drawing.Point(728, 3);
         this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(215, 72);
         this.simpleButton1.TabIndex = 1;
         this.simpleButton1.Tag = "1";
         this.simpleButton1.Text = "<b>mohsen-lt</b><br><color=Gray><size=12>artauser</size></color><br>";
         // 
         // tp_002
         // 
         this.tp_002.Location = new System.Drawing.Point(4, 23);
         this.tp_002.Name = "tp_002";
         this.tp_002.Padding = new System.Windows.Forms.Padding(3);
         this.tp_002.Size = new System.Drawing.Size(952, 402);
         this.tp_002.TabIndex = 1;
         this.tp_002.Tag = "2";
         this.tp_002.Text = "دستگاه های جانبی متصل";
         this.tp_002.UseVisualStyleBackColor = true;
         // 
         // tp_003
         // 
         this.tp_003.Controls.Add(this.NewPos_Butn);
         this.tp_003.Controls.Add(this.labelControl3);
         this.tp_003.Controls.Add(this.PosList_Flp);
         this.tp_003.Controls.Add(this.labelControl2);
         this.tp_003.Location = new System.Drawing.Point(4, 23);
         this.tp_003.Name = "tp_003";
         this.tp_003.Padding = new System.Windows.Forms.Padding(3);
         this.tp_003.Size = new System.Drawing.Size(952, 402);
         this.tp_003.TabIndex = 2;
         this.tp_003.Tag = "3";
         this.tp_003.Text = "POS";
         this.tp_003.UseVisualStyleBackColor = true;
         // 
         // NewPos_Butn
         // 
         this.NewPos_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.NewPos_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.NewPos_Butn.Appearance.Font = new System.Drawing.Font("IRANSans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.NewPos_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.NewPos_Butn.Appearance.Options.UseBackColor = true;
         this.NewPos_Butn.Appearance.Options.UseFont = true;
         this.NewPos_Butn.Appearance.Options.UseForeColor = true;
         this.NewPos_Butn.Appearance.Options.UseTextOptions = true;
         this.NewPos_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.NewPos_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1422;
         this.NewPos_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.NewPos_Butn.Location = new System.Drawing.Point(600, 99);
         this.NewPos_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.NewPos_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.NewPos_Butn.Name = "NewPos_Butn";
         this.NewPos_Butn.Size = new System.Drawing.Size(346, 35);
         this.NewPos_Butn.TabIndex = 17;
         this.NewPos_Butn.Text = "اضافه کردن پایانه فروش جدید";
         this.NewPos_Butn.Click += new System.EventHandler(this.NewPos_Butn_Click);
         // 
         // labelControl3
         // 
         this.labelControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl3.Appearance.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.labelControl3.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
         this.labelControl3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl3.Location = new System.Drawing.Point(428, 45);
         this.labelControl3.Name = "labelControl3";
         this.labelControl3.Size = new System.Drawing.Size(518, 48);
         this.labelControl3.TabIndex = 12;
         this.labelControl3.Text = "اطلاعات دستگاه های پایانه فروش در این قسمت نمایش داده شده. شما می توانید برای ایج" +
    "اد کردن دستگاه جدید و پیش فرض قرار دادن آن از این قسمت اقدام کنید";
         // 
         // PosList_Flp
         // 
         this.PosList_Flp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PosList_Flp.Controls.Add(this.simpleButton2);
         this.PosList_Flp.Controls.Add(this.simpleButton3);
         this.PosList_Flp.Controls.Add(this.simpleButton4);
         this.PosList_Flp.Controls.Add(this.simpleButton5);
         this.PosList_Flp.Location = new System.Drawing.Point(3, 150);
         this.PosList_Flp.Name = "PosList_Flp";
         this.PosList_Flp.Size = new System.Drawing.Size(946, 249);
         this.PosList_Flp.TabIndex = 11;
         // 
         // simpleButton2
         // 
         this.simpleButton2.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.simpleButton2.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
         this.simpleButton2.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton2.Appearance.Options.UseBackColor = true;
         this.simpleButton2.Appearance.Options.UseBorderColor = true;
         this.simpleButton2.Appearance.Options.UseFont = true;
         this.simpleButton2.Appearance.Options.UseForeColor = true;
         this.simpleButton2.Appearance.Options.UseTextOptions = true;
         this.simpleButton2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton2.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
         this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton2.Location = new System.Drawing.Point(728, 3);
         this.simpleButton2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton2.Name = "simpleButton2";
         this.simpleButton2.Size = new System.Drawing.Size(215, 59);
         this.simpleButton2.TabIndex = 1;
         this.simpleButton2.Tag = "1";
         this.simpleButton2.Text = "POS I  </b>*</b></b>@</b><br><color=Gray><size=9>بانک صادرات، شعبه 1247</size></c" +
    "olor><br><color=Green><size=9>شماره حساب : 1236548759</size></color><br>";
         // 
         // simpleButton3
         // 
         this.simpleButton3.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.simpleButton3.Appearance.BorderColor = System.Drawing.Color.Silver;
         this.simpleButton3.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton3.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton3.Appearance.Options.UseBackColor = true;
         this.simpleButton3.Appearance.Options.UseBorderColor = true;
         this.simpleButton3.Appearance.Options.UseFont = true;
         this.simpleButton3.Appearance.Options.UseForeColor = true;
         this.simpleButton3.Appearance.Options.UseTextOptions = true;
         this.simpleButton3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton3.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
         this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton3.Location = new System.Drawing.Point(507, 3);
         this.simpleButton3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton3.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton3.Name = "simpleButton3";
         this.simpleButton3.Size = new System.Drawing.Size(215, 59);
         this.simpleButton3.TabIndex = 2;
         this.simpleButton3.Tag = "1";
         this.simpleButton3.Text = "POS II<br><color=Gray><size=9>بانک پارسیان، شعبه 1358</size></color><br><color=Gr" +
    "een><size=9>شماره حساب : 4595848759</size></color><br>";
         // 
         // simpleButton4
         // 
         this.simpleButton4.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton4.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.simpleButton4.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
         this.simpleButton4.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton4.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton4.Appearance.Options.UseBackColor = true;
         this.simpleButton4.Appearance.Options.UseBorderColor = true;
         this.simpleButton4.Appearance.Options.UseFont = true;
         this.simpleButton4.Appearance.Options.UseForeColor = true;
         this.simpleButton4.Appearance.Options.UseTextOptions = true;
         this.simpleButton4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton4.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton4.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton4.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
         this.simpleButton4.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton4.Location = new System.Drawing.Point(286, 3);
         this.simpleButton4.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton4.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton4.Name = "simpleButton4";
         this.simpleButton4.Size = new System.Drawing.Size(215, 59);
         this.simpleButton4.TabIndex = 3;
         this.simpleButton4.Tag = "1";
         this.simpleButton4.Text = "POS III<br><color=Gray><size=9>بانک صادرات، شعبه 1247</size></color><br><color=Gr" +
    "een><size=9>شماره حساب : 1236548758</size></color><br>";
         // 
         // simpleButton5
         // 
         this.simpleButton5.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
         this.simpleButton5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.simpleButton5.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.simpleButton5.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
         this.simpleButton5.Appearance.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.simpleButton5.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.simpleButton5.Appearance.Options.UseBackColor = true;
         this.simpleButton5.Appearance.Options.UseBorderColor = true;
         this.simpleButton5.Appearance.Options.UseFont = true;
         this.simpleButton5.Appearance.Options.UseForeColor = true;
         this.simpleButton5.Appearance.Options.UseTextOptions = true;
         this.simpleButton5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton5.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
         this.simpleButton5.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton5.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
         this.simpleButton5.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.simpleButton5.Location = new System.Drawing.Point(65, 3);
         this.simpleButton5.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton5.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton5.Name = "simpleButton5";
         this.simpleButton5.Size = new System.Drawing.Size(215, 59);
         this.simpleButton5.TabIndex = 4;
         this.simpleButton5.Tag = "1";
         this.simpleButton5.Text = "POS IV<br><color=Gray><size=9>بانک پارسیان، شعبه 1358</size></color><br><color=Gr" +
    "een><size=9>شماره حساب : 4605848759</size></color><br>";
         // 
         // labelControl2
         // 
         this.labelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.labelControl2.Appearance.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.labelControl2.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
         this.labelControl2.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
         this.labelControl2.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl2.Location = new System.Drawing.Point(3, 3);
         this.labelControl2.Name = "labelControl2";
         this.labelControl2.Size = new System.Drawing.Size(946, 36);
         this.labelControl2.TabIndex = 10;
         this.labelControl2.Text = "POS";
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Controls.Add(this.ClientList_Butn);
         this.flowLayoutPanel1.Controls.Add(this.OtherDevice_Butn);
         this.flowLayoutPanel1.Controls.Add(this.Pos_Butn);
         this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(219, 429);
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
         // Pos_Butn
         // 
         this.Pos_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Pos_Butn.Appearance.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Pos_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Pos_Butn.Appearance.Options.UseFont = true;
         this.Pos_Butn.Appearance.Options.UseForeColor = true;
         this.Pos_Butn.Appearance.Options.UseTextOptions = true;
         this.Pos_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Pos_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1622;
         this.Pos_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.Pos_Butn.Location = new System.Drawing.Point(1, 107);
         this.Pos_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Pos_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Pos_Butn.Name = "Pos_Butn";
         this.Pos_Butn.Size = new System.Drawing.Size(215, 46);
         this.Pos_Butn.TabIndex = 2;
         this.Pos_Butn.Tag = "3";
         this.Pos_Butn.Text = "POS";
         this.Pos_Butn.Click += new System.EventHandler(this.RightButns_Click);
         // 
         // ActiveSessionBs
         // 
         this.ActiveSessionBs.DataSource = typeof(System.DataGuard.Data.Active_Session);
         // 
         // PosBs
         // 
         this.PosBs.DataSource = typeof(System.DataGuard.Data.Pos_Device);
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
         this.Size = new System.Drawing.Size(1184, 488);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         this.Tb_Master.ResumeLayout(false);
         this.tp_001.ResumeLayout(false);
         this.ActiveSessionList_Flp.ResumeLayout(false);
         this.tp_003.ResumeLayout(false);
         this.PosList_Flp.ResumeLayout(false);
         this.flowLayoutPanel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ActiveSessionBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.PosBs)).EndInit();
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
      private DevExpress.XtraEditors.SimpleButton Pos_Butn;
      private Windows.Forms.TabPage tp_003;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private Windows.Forms.FlowLayoutPanel PosList_Flp;
      private DevExpress.XtraEditors.SimpleButton simpleButton2;
      private DevExpress.XtraEditors.SimpleButton simpleButton3;
      private DevExpress.XtraEditors.LabelControl labelControl3;
      private DevExpress.XtraEditors.SimpleButton NewPos_Butn;
      private DevExpress.XtraEditors.SimpleButton simpleButton4;
      private DevExpress.XtraEditors.SimpleButton simpleButton5;
      private Windows.Forms.BindingSource PosBs;

   }
}
