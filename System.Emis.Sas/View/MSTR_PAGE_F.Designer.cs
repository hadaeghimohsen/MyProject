namespace System.Emis.Sas.View
{
   partial class MSTR_PAGE_F
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
         DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
         DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
         DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
         DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSTR_PAGE_F));
         this.backstageViewControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewControl();
         this.backstageViewClientControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
         this.cb_datasource = new System.Windows.Forms.ComboBox();
         this.ABS_MENU = new DevExpress.XtraEditors.TileControl();
         this.ADM = new DevExpress.XtraEditors.TileGroup();
         this.BIL = new DevExpress.XtraEditors.TileGroup();
         this.BIL_SERV_INFO = new DevExpress.XtraEditors.TileItem();
         this.SAS = new DevExpress.XtraEditors.TileGroup();
         this.SAS_RQST_INFO = new DevExpress.XtraEditors.TileItem();
         this.SAS_CHNG_IDTY_GROP = new DevExpress.XtraEditors.TileItem();
         this.SAS_REGL = new DevExpress.XtraEditors.TileItem();
         this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
         this.label1 = new System.Windows.Forms.Label();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.backstageViewButtonItem1 = new DevExpress.XtraBars.Ribbon.BackstageViewButtonItem();
         this.backstageViewTabItem1 = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
         this.backstageViewControl1.SuspendLayout();
         this.backstageViewClientControl1.SuspendLayout();
         this.SuspendLayout();
         // 
         // backstageViewControl1
         // 
         this.backstageViewControl1.ColorScheme = DevExpress.XtraBars.Ribbon.RibbonControlColorScheme.Yellow;
         this.backstageViewControl1.Controls.Add(this.backstageViewClientControl1);
         this.backstageViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.backstageViewControl1.Items.Add(this.backstageViewButtonItem1);
         this.backstageViewControl1.Items.Add(this.backstageViewTabItem1);
         this.backstageViewControl1.Location = new System.Drawing.Point(0, 0);
         this.backstageViewControl1.Name = "backstageViewControl1";
         this.backstageViewControl1.SelectedTab = this.backstageViewTabItem1;
         this.backstageViewControl1.SelectedTabIndex = 1;
         this.backstageViewControl1.Size = new System.Drawing.Size(832, 673);
         this.backstageViewControl1.TabIndex = 0;
         this.backstageViewControl1.Text = "backstageViewControl1";
         // 
         // backstageViewClientControl1
         // 
         this.backstageViewClientControl1.Controls.Add(this.cb_datasource);
         this.backstageViewClientControl1.Controls.Add(this.ABS_MENU);
         this.backstageViewClientControl1.Controls.Add(this.labelControl2);
         this.backstageViewClientControl1.Controls.Add(this.label1);
         this.backstageViewClientControl1.Controls.Add(this.labelControl1);
         this.backstageViewClientControl1.Location = new System.Drawing.Point(207, 0);
         this.backstageViewClientControl1.Name = "backstageViewClientControl1";
         this.backstageViewClientControl1.Size = new System.Drawing.Size(625, 673);
         this.backstageViewClientControl1.TabIndex = 0;
         // 
         // cb_datasource
         // 
         this.cb_datasource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cb_datasource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cb_datasource.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_datasource.FormattingEnabled = true;
         this.cb_datasource.IntegralHeight = false;
         this.cb_datasource.ItemHeight = 28;
         this.cb_datasource.Location = new System.Drawing.Point(265, 30);
         this.cb_datasource.MaxDropDownItems = 100;
         this.cb_datasource.Name = "cb_datasource";
         this.cb_datasource.Size = new System.Drawing.Size(271, 36);
         this.cb_datasource.TabIndex = 41;
         // 
         // ABS_MENU
         // 
         this.ABS_MENU.AllowItemHover = true;
         this.ABS_MENU.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ABS_MENU.BackColor = System.Drawing.Color.Transparent;
         this.ABS_MENU.Groups.Add(this.ADM);
         this.ABS_MENU.Groups.Add(this.BIL);
         this.ABS_MENU.Groups.Add(this.SAS);
         this.ABS_MENU.HorizontalContentAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.ABS_MENU.Location = new System.Drawing.Point(5, 100);
         this.ABS_MENU.Name = "ABS_MENU";
         this.ABS_MENU.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.ABS_MENU.ShowGroupText = true;
         this.ABS_MENU.ShowText = true;
         this.ABS_MENU.Size = new System.Drawing.Size(616, 570);
         this.ABS_MENU.TabIndex = 45;
         this.ABS_MENU.Text = "سیستم جامع خدمات مشترکین";
         this.ABS_MENU.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
         // 
         // ADM
         // 
         this.ADM.Name = "ADM";
         this.ADM.Text = "فروش انشعاب";
         // 
         // BIL
         // 
         this.BIL.Items.Add(this.BIL_SERV_INFO);
         this.BIL.Name = "BIL";
         this.BIL.Text = "فروش انرژی";
         // 
         // BIL_SERV_INFO
         // 
         this.BIL_SERV_INFO.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.BIL_SERV_INFO.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.BIL_SERV_INFO.AppearanceItem.Normal.ForeColor = System.Drawing.Color.Black;
         this.BIL_SERV_INFO.AppearanceItem.Normal.Options.UseBackColor = true;
         this.BIL_SERV_INFO.AppearanceItem.Normal.Options.UseForeColor = true;
         tileItemElement1.Appearance.Normal.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         tileItemElement1.Appearance.Normal.Options.UseFont = true;
         tileItemElement1.Text = "اطلاعات مشترکین";
         tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
         this.BIL_SERV_INFO.Elements.Add(tileItemElement1);
         this.BIL_SERV_INFO.IsLarge = true;
         this.BIL_SERV_INFO.Name = "BIL_SERV_INFO";
         this.BIL_SERV_INFO.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.BIL_SERV_INFO_ItemClick);
         // 
         // SAS
         // 
         this.SAS.Items.Add(this.SAS_RQST_INFO);
         this.SAS.Items.Add(this.SAS_CHNG_IDTY_GROP);
         this.SAS.Items.Add(this.SAS_REGL);
         this.SAS.Name = "SAS";
         this.SAS.Text = "خدمات پس از فروش";
         // 
         // SAS_RQST_INFO
         // 
         this.SAS_RQST_INFO.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SAS_RQST_INFO.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SAS_RQST_INFO.AppearanceItem.Normal.ForeColor = System.Drawing.Color.Black;
         this.SAS_RQST_INFO.AppearanceItem.Normal.Options.UseBackColor = true;
         this.SAS_RQST_INFO.AppearanceItem.Normal.Options.UseForeColor = true;
         tileItemElement2.Appearance.Normal.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         tileItemElement2.Appearance.Normal.Options.UseFont = true;
         tileItemElement2.Text = "اطلاعات درخواست";
         tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
         this.SAS_RQST_INFO.Elements.Add(tileItemElement2);
         this.SAS_RQST_INFO.IsLarge = true;
         this.SAS_RQST_INFO.Name = "SAS_RQST_INFO";
         this.SAS_RQST_INFO.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.SAS_RQST_INFO_ItemClick);
         // 
         // SAS_CHNG_IDTY_GROP
         // 
         this.SAS_CHNG_IDTY_GROP.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SAS_CHNG_IDTY_GROP.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SAS_CHNG_IDTY_GROP.AppearanceItem.Normal.Options.UseBackColor = true;
         tileItemElement3.Appearance.Normal.BackColor = System.Drawing.SystemColors.MenuHighlight;
         tileItemElement3.Appearance.Normal.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         tileItemElement3.Appearance.Normal.ForeColor = System.Drawing.Color.Black;
         tileItemElement3.Appearance.Normal.Options.UseBackColor = true;
         tileItemElement3.Appearance.Normal.Options.UseFont = true;
         tileItemElement3.Appearance.Normal.Options.UseForeColor = true;
         tileItemElement3.Text = "تغییر مشخصات عمومی";
         tileItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
         this.SAS_CHNG_IDTY_GROP.Elements.Add(tileItemElement3);
         this.SAS_CHNG_IDTY_GROP.IsLarge = true;
         this.SAS_CHNG_IDTY_GROP.Name = "SAS_CHNG_IDTY_GROP";
         this.SAS_CHNG_IDTY_GROP.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.SAS_CHNG_IDTY_GROP_ItemClick);
         // 
         // SAS_REGL
         // 
         this.SAS_REGL.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SAS_REGL.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SAS_REGL.AppearanceItem.Normal.ForeColor = System.Drawing.Color.Black;
         this.SAS_REGL.AppearanceItem.Normal.Options.UseBackColor = true;
         this.SAS_REGL.AppearanceItem.Normal.Options.UseForeColor = true;
         tileItemElement4.Appearance.Normal.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         tileItemElement4.Appearance.Normal.Options.UseFont = true;
         tileItemElement4.Text = "اطلاعات آیین نامه";
         tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
         this.SAS_REGL.Elements.Add(tileItemElement4);
         this.SAS_REGL.IsLarge = true;
         this.SAS_REGL.Name = "SAS_REGL";
         this.SAS_REGL.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.SAS_REGL_ItemClick);
         // 
         // labelControl2
         // 
         this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl2.LineVisible = true;
         this.labelControl2.Location = new System.Drawing.Point(4, 65);
         this.labelControl2.Name = "labelControl2";
         this.labelControl2.Size = new System.Drawing.Size(617, 29);
         this.labelControl2.TabIndex = 43;
         this.labelControl2.Text = "فعالیت ها";
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.BackColor = System.Drawing.Color.Transparent;
         this.label1.Location = new System.Drawing.Point(542, 41);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(39, 13);
         this.label1.TabIndex = 42;
         this.label1.Text = "سرور :";
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(5, 3);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(617, 29);
         this.labelControl1.TabIndex = 40;
         this.labelControl1.Text = "انتخاب سرور";
         // 
         // backstageViewButtonItem1
         // 
         this.backstageViewButtonItem1.Caption = "اطلاعات خدمات پس از فروش";
         this.backstageViewButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("backstageViewButtonItem1.Glyph")));
         this.backstageViewButtonItem1.Name = "backstageViewButtonItem1";
         // 
         // backstageViewTabItem1
         // 
         this.backstageViewTabItem1.Caption = "عملیات عمومی";
         this.backstageViewTabItem1.ContentControl = this.backstageViewClientControl1;
         this.backstageViewTabItem1.Name = "backstageViewTabItem1";
         this.backstageViewTabItem1.Selected = true;
         // 
         // MSTR_PAGE_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.backstageViewControl1);
         this.Name = "MSTR_PAGE_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(832, 673);
         this.backstageViewControl1.ResumeLayout(false);
         this.backstageViewClientControl1.ResumeLayout(false);
         this.backstageViewClientControl1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.BackstageViewControl backstageViewControl1;
      private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl1;
      private DevExpress.XtraBars.Ribbon.BackstageViewButtonItem backstageViewButtonItem1;
      private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private Windows.Forms.Label label1;
      private Windows.Forms.ComboBox cb_datasource;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.TileControl ABS_MENU;
      private DevExpress.XtraEditors.TileGroup ADM;
      private DevExpress.XtraEditors.TileGroup BIL;
      private DevExpress.XtraEditors.TileItem BIL_SERV_INFO;
      private DevExpress.XtraEditors.TileGroup SAS;
      private DevExpress.XtraEditors.TileItem SAS_RQST_INFO;
      private DevExpress.XtraEditors.TileItem SAS_CHNG_IDTY_GROP;
      private DevExpress.XtraEditors.TileItem SAS_REGL;
   }
}
