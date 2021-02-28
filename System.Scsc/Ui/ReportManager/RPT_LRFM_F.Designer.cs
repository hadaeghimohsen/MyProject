namespace System.Scsc.Ui.ReportManager
{
   partial class RPT_LRFM_F
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
         this.modual_ReportGridControl = new DevExpress.XtraGrid.GridControl();
         this.MdrpBs1 = new System.Windows.Forms.BindingSource();
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colCODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MdulName_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.MudlDesc_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSECT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.SectDesc_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Rwno_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.RprtDesc_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRPRT_PATH = new DevExpress.XtraGrid.Columns.GridColumn();
         this.ShowPrvw_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.DYsnoBs1 = new System.Windows.Forms.BindingSource();
         this.colDFLT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSTAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panel1 = new System.Windows.Forms.Panel();
         this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
         this.TitlForm_Lb = new DevExpress.XtraEditors.LabelControl();
         this.Back_Butn = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.modual_ReportGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MdrpBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DYsnoBs1)).BeginInit();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // modual_ReportGridControl
         // 
         this.modual_ReportGridControl.DataSource = this.MdrpBs1;
         this.modual_ReportGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.modual_ReportGridControl.Location = new System.Drawing.Point(0, 59);
         this.modual_ReportGridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.modual_ReportGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.modual_ReportGridControl.MainView = this.gridView1;
         this.modual_ReportGridControl.Name = "modual_ReportGridControl";
         this.modual_ReportGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
         this.modual_ReportGridControl.Size = new System.Drawing.Size(867, 518);
         this.modual_ReportGridControl.TabIndex = 1;
         this.modual_ReportGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // MdrpBs1
         // 
         this.MdrpBs1.DataSource = typeof(System.Scsc.Data.Modual_Report);
         // 
         // gridView1
         // 
         this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView1.Appearance.Row.Options.UseFont = true;
         this.gridView1.Appearance.Row.Options.UseTextOptions = true;
         this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCODE,
            this.MdulName_Clm,
            this.MudlDesc_Clm,
            this.colSECT_NAME,
            this.SectDesc_Clm,
            this.Rwno_Clm,
            this.RprtDesc_Clm,
            this.colRPRT_PATH,
            this.ShowPrvw_Clm,
            this.colDFLT,
            this.colSTAT,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE});
         this.gridView1.GridControl = this.modual_ReportGridControl;
         this.gridView1.GroupCount = 2;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
         this.gridView1.OptionsBehavior.Editable = false;
         this.gridView1.OptionsBehavior.ReadOnly = true;
         this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.MdulName_Clm, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colSECT_NAME, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.Rwno_Clm, DevExpress.Data.ColumnSortOrder.Ascending)});
         this.gridView1.DoubleClick += new System.EventHandler(this.Rptlrfm_Gv_DoubleClick);
         // 
         // colCODE
         // 
         this.colCODE.FieldName = "CODE";
         this.colCODE.Name = "colCODE";
         // 
         // MdulName_Clm
         // 
         this.MdulName_Clm.Caption = "نام فرم";
         this.MdulName_Clm.FieldName = "MDUL_NAME";
         this.MdulName_Clm.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.MdulName_Clm.Name = "MdulName_Clm";
         this.MdulName_Clm.Visible = true;
         this.MdulName_Clm.VisibleIndex = 6;
         // 
         // MudlDesc_Clm
         // 
         this.MudlDesc_Clm.Caption = "توضیحات فرم";
         this.MudlDesc_Clm.FieldName = "MDUL_DESC";
         this.MudlDesc_Clm.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.MudlDesc_Clm.Name = "MudlDesc_Clm";
         this.MudlDesc_Clm.Visible = true;
         this.MudlDesc_Clm.VisibleIndex = 3;
         this.MudlDesc_Clm.Width = 260;
         // 
         // colSECT_NAME
         // 
         this.colSECT_NAME.Caption = "نام قطعه";
         this.colSECT_NAME.FieldName = "SECT_NAME";
         this.colSECT_NAME.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colSECT_NAME.Name = "colSECT_NAME";
         this.colSECT_NAME.Visible = true;
         this.colSECT_NAME.VisibleIndex = 4;
         // 
         // SectDesc_Clm
         // 
         this.SectDesc_Clm.Caption = "توضیحات قطعه";
         this.SectDesc_Clm.FieldName = "SECT_DESC";
         this.SectDesc_Clm.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.SectDesc_Clm.Name = "SectDesc_Clm";
         this.SectDesc_Clm.Visible = true;
         this.SectDesc_Clm.VisibleIndex = 2;
         this.SectDesc_Clm.Width = 150;
         // 
         // Rwno_Clm
         // 
         this.Rwno_Clm.Caption = "ردیف";
         this.Rwno_Clm.FieldName = "RWNO";
         this.Rwno_Clm.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.Rwno_Clm.Name = "Rwno_Clm";
         this.Rwno_Clm.Visible = true;
         this.Rwno_Clm.VisibleIndex = 4;
         this.Rwno_Clm.Width = 42;
         // 
         // RprtDesc_Clm
         // 
         this.RprtDesc_Clm.Caption = "توضیحات چاپ";
         this.RprtDesc_Clm.FieldName = "RPRT_DESC";
         this.RprtDesc_Clm.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.RprtDesc_Clm.Name = "RprtDesc_Clm";
         this.RprtDesc_Clm.Visible = true;
         this.RprtDesc_Clm.VisibleIndex = 0;
         this.RprtDesc_Clm.Width = 150;
         // 
         // colRPRT_PATH
         // 
         this.colRPRT_PATH.Caption = "مسیر فایل";
         this.colRPRT_PATH.FieldName = "RPRT_PATH";
         this.colRPRT_PATH.Name = "colRPRT_PATH";
         // 
         // ShowPrvw_Clm
         // 
         this.ShowPrvw_Clm.Caption = "پیش نمایش چاپ";
         this.ShowPrvw_Clm.ColumnEdit = this.repositoryItemLookUpEdit1;
         this.ShowPrvw_Clm.FieldName = "SHOW_PRVW";
         this.ShowPrvw_Clm.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.ShowPrvw_Clm.Name = "ShowPrvw_Clm";
         this.ShowPrvw_Clm.Visible = true;
         this.ShowPrvw_Clm.VisibleIndex = 1;
         this.ShowPrvw_Clm.Width = 150;
         // 
         // repositoryItemLookUpEdit1
         // 
         this.repositoryItemLookUpEdit1.AutoHeight = false;
         this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "پیش نمایش چاپ", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.repositoryItemLookUpEdit1.DataSource = this.DYsnoBs1;
         this.repositoryItemLookUpEdit1.DisplayMember = "DOMN_DESC";
         this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
         this.repositoryItemLookUpEdit1.ValueMember = "VALU";
         // 
         // DYsnoBs1
         // 
         this.DYsnoBs1.DataSource = typeof(System.Scsc.Data.D_YSNO);
         // 
         // colDFLT
         // 
         this.colDFLT.FieldName = "DFLT";
         this.colDFLT.Name = "colDFLT";
         // 
         // colSTAT
         // 
         this.colSTAT.FieldName = "STAT";
         this.colSTAT.Name = "colSTAT";
         // 
         // colCRET_BY
         // 
         this.colCRET_BY.FieldName = "CRET_BY";
         this.colCRET_BY.Name = "colCRET_BY";
         // 
         // colCRET_DATE
         // 
         this.colCRET_DATE.FieldName = "CRET_DATE";
         this.colCRET_DATE.Name = "colCRET_DATE";
         // 
         // colMDFY_BY
         // 
         this.colMDFY_BY.FieldName = "MDFY_BY";
         this.colMDFY_BY.Name = "colMDFY_BY";
         // 
         // colMDFY_DATE
         // 
         this.colMDFY_DATE.FieldName = "MDFY_DATE";
         this.colMDFY_DATE.Name = "colMDFY_DATE";
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.simpleButton3);
         this.panel1.Controls.Add(this.simpleButton2);
         this.panel1.Controls.Add(this.simpleButton1);
         this.panel1.Controls.Add(this.TitlForm_Lb);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(867, 59);
         this.panel1.TabIndex = 5;
         // 
         // simpleButton3
         // 
         this.simpleButton3.Appearance.BackColor = System.Drawing.SystemColors.ControlLight;
         this.simpleButton3.Appearance.Options.UseBackColor = true;
         this.simpleButton3.Dock = System.Windows.Forms.DockStyle.Left;
         this.simpleButton3.Image = global::System.Scsc.Properties.Resources.IMAGE_1059;
         this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.simpleButton3.Location = new System.Drawing.Point(122, 0);
         this.simpleButton3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton3.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton3.Name = "simpleButton3";
         this.simpleButton3.Size = new System.Drawing.Size(61, 59);
         this.simpleButton3.TabIndex = 4;
         this.simpleButton3.ToolTip = "بازگشت";
         this.simpleButton3.Click += new System.EventHandler(this.mb_SelectReport_Click);
         // 
         // simpleButton2
         // 
         this.simpleButton2.Appearance.BackColor = System.Drawing.SystemColors.ControlLight;
         this.simpleButton2.Appearance.Options.UseBackColor = true;
         this.simpleButton2.Dock = System.Windows.Forms.DockStyle.Left;
         this.simpleButton2.Image = global::System.Scsc.Properties.Resources.IMAGE_1369;
         this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.simpleButton2.Location = new System.Drawing.Point(61, 0);
         this.simpleButton2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton2.Name = "simpleButton2";
         this.simpleButton2.Size = new System.Drawing.Size(61, 59);
         this.simpleButton2.TabIndex = 3;
         this.simpleButton2.ToolTip = "بازگشت";
         this.simpleButton2.Click += new System.EventHandler(this.mb_reloading_Click);
         // 
         // simpleButton1
         // 
         this.simpleButton1.Appearance.BackColor = System.Drawing.SystemColors.ControlLight;
         this.simpleButton1.Appearance.Options.UseBackColor = true;
         this.simpleButton1.Dock = System.Windows.Forms.DockStyle.Left;
         this.simpleButton1.Image = global::System.Scsc.Properties.Resources.IMAGE_1370;
         this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.simpleButton1.Location = new System.Drawing.Point(0, 0);
         this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(61, 59);
         this.simpleButton1.TabIndex = 2;
         this.simpleButton1.ToolTip = "بازگشت";
         this.simpleButton1.Click += new System.EventHandler(this.mb_CfgStngF_Click);
         // 
         // TitlForm_Lb
         // 
         this.TitlForm_Lb.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.TitlForm_Lb.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1059;
         this.TitlForm_Lb.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.TitlForm_Lb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.TitlForm_Lb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.TitlForm_Lb.Dock = System.Windows.Forms.DockStyle.Right;
         this.TitlForm_Lb.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.TitlForm_Lb.Location = new System.Drawing.Point(413, 0);
         this.TitlForm_Lb.Name = "TitlForm_Lb";
         this.TitlForm_Lb.Size = new System.Drawing.Size(393, 59);
         this.TitlForm_Lb.TabIndex = 1;
         this.TitlForm_Lb.Text = "انتخاب چاپ گزارش";
         // 
         // Back_Butn
         // 
         this.Back_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
         this.Back_Butn.Appearance.Options.UseBackColor = true;
         this.Back_Butn.Dock = System.Windows.Forms.DockStyle.Right;
         this.Back_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1371;
         this.Back_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Back_Butn.Location = new System.Drawing.Point(806, 0);
         this.Back_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Back_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.Size = new System.Drawing.Size(61, 59);
         this.Back_Butn.TabIndex = 0;
         this.Back_Butn.ToolTip = "بازگشت";
         this.Back_Butn.Click += new System.EventHandler(this.mb_back_Click);
         // 
         // RPT_LRFM_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.Controls.Add(this.modual_ReportGridControl);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "RPT_LRFM_F";
         this.Size = new System.Drawing.Size(867, 577);
         ((System.ComponentModel.ISupportInitialize)(this.modual_ReportGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MdrpBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DYsnoBs1)).EndInit();
         this.panel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.BindingSource MdrpBs1;
      private DevExpress.XtraGrid.GridControl modual_ReportGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colCODE;
      private DevExpress.XtraGrid.Columns.GridColumn MdulName_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn MudlDesc_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn colSECT_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn SectDesc_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn Rwno_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn RprtDesc_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn colRPRT_PATH;
      private DevExpress.XtraGrid.Columns.GridColumn ShowPrvw_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn colDFLT;
      private DevExpress.XtraGrid.Columns.GridColumn colSTAT;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
      private Windows.Forms.BindingSource DYsnoBs1;
      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton simpleButton3;
      private DevExpress.XtraEditors.SimpleButton simpleButton2;
      private DevExpress.XtraEditors.SimpleButton simpleButton1;
      private DevExpress.XtraEditors.LabelControl TitlForm_Lb;
      private DevExpress.XtraEditors.SimpleButton Back_Butn;
   }
}
