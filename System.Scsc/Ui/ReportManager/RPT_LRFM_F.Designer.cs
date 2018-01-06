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
         this.components = new System.ComponentModel.Container();
         this.modual_ReportGridControl = new DevExpress.XtraGrid.GridControl();
         this.MdrpBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colCODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDUL_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDUL_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSECT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSECT_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRPRT_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRPRT_PATH = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSHOW_PRVW = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.DYsnoBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.colDFLT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSTAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.label1 = new System.Windows.Forms.Label();
         this.mb_SelectReport = new System.MaxUi.NewMaxBtn();
         this.mb_CfgStngF = new System.MaxUi.NewMaxBtn();
         this.mb_reloading = new System.MaxUi.NewMaxBtn();
         this.mb_back = new System.MaxUi.NewMaxBtn();
         ((System.ComponentModel.ISupportInitialize)(this.modual_ReportGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MdrpBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DYsnoBs1)).BeginInit();
         this.SuspendLayout();
         // 
         // modual_ReportGridControl
         // 
         this.modual_ReportGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
         this.modual_ReportGridControl.DataSource = this.MdrpBs1;
         this.modual_ReportGridControl.Location = new System.Drawing.Point(55, 76);
         this.modual_ReportGridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.modual_ReportGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.modual_ReportGridControl.MainView = this.gridView1;
         this.modual_ReportGridControl.Name = "modual_ReportGridControl";
         this.modual_ReportGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
         this.modual_ReportGridControl.Size = new System.Drawing.Size(758, 425);
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
            this.colMDUL_NAME,
            this.colMDUL_DESC,
            this.colSECT_NAME,
            this.colSECT_DESC,
            this.colRWNO,
            this.colRPRT_DESC,
            this.colRPRT_PATH,
            this.colSHOW_PRVW,
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
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colMDUL_NAME, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colSECT_NAME, DevExpress.Data.ColumnSortOrder.Ascending)});
         // 
         // colCODE
         // 
         this.colCODE.FieldName = "CODE";
         this.colCODE.Name = "colCODE";
         // 
         // colMDUL_NAME
         // 
         this.colMDUL_NAME.Caption = "نام فرم";
         this.colMDUL_NAME.FieldName = "MDUL_NAME";
         this.colMDUL_NAME.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colMDUL_NAME.Name = "colMDUL_NAME";
         this.colMDUL_NAME.Visible = true;
         this.colMDUL_NAME.VisibleIndex = 6;
         // 
         // colMDUL_DESC
         // 
         this.colMDUL_DESC.Caption = "توضیحات فرم";
         this.colMDUL_DESC.FieldName = "MDUL_DESC";
         this.colMDUL_DESC.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colMDUL_DESC.Name = "colMDUL_DESC";
         this.colMDUL_DESC.Visible = true;
         this.colMDUL_DESC.VisibleIndex = 3;
         this.colMDUL_DESC.Width = 260;
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
         // colSECT_DESC
         // 
         this.colSECT_DESC.Caption = "توضیحات قطعه";
         this.colSECT_DESC.FieldName = "SECT_DESC";
         this.colSECT_DESC.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colSECT_DESC.Name = "colSECT_DESC";
         this.colSECT_DESC.Visible = true;
         this.colSECT_DESC.VisibleIndex = 2;
         this.colSECT_DESC.Width = 150;
         // 
         // colRWNO
         // 
         this.colRWNO.Caption = "ردیف";
         this.colRWNO.FieldName = "RWNO";
         this.colRWNO.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRWNO.Name = "colRWNO";
         this.colRWNO.Visible = true;
         this.colRWNO.VisibleIndex = 4;
         this.colRWNO.Width = 42;
         // 
         // colRPRT_DESC
         // 
         this.colRPRT_DESC.Caption = "توضیحات چاپ";
         this.colRPRT_DESC.FieldName = "RPRT_DESC";
         this.colRPRT_DESC.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRPRT_DESC.Name = "colRPRT_DESC";
         this.colRPRT_DESC.Visible = true;
         this.colRPRT_DESC.VisibleIndex = 0;
         this.colRPRT_DESC.Width = 150;
         // 
         // colRPRT_PATH
         // 
         this.colRPRT_PATH.Caption = "مسیر فایل";
         this.colRPRT_PATH.FieldName = "RPRT_PATH";
         this.colRPRT_PATH.Name = "colRPRT_PATH";
         // 
         // colSHOW_PRVW
         // 
         this.colSHOW_PRVW.Caption = "پیش نمایش چاپ";
         this.colSHOW_PRVW.ColumnEdit = this.repositoryItemLookUpEdit1;
         this.colSHOW_PRVW.FieldName = "SHOW_PRVW";
         this.colSHOW_PRVW.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colSHOW_PRVW.Name = "colSHOW_PRVW";
         this.colSHOW_PRVW.Visible = true;
         this.colSHOW_PRVW.VisibleIndex = 1;
         this.colSHOW_PRVW.Width = 150;
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
         // label1
         // 
         this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.label1.BackColor = System.Drawing.Color.DarkSalmon;
         this.label1.Font = new System.Drawing.Font("IRANSans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
         this.label1.Image = global::System.Scsc.Properties.Resources.IMAGE_1059;
         this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.label1.Location = new System.Drawing.Point(55, 19);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(758, 56);
         this.label1.TabIndex = 2;
         this.label1.Text = "انتخاب چاپ گزارش";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // mb_SelectReport
         // 
         this.mb_SelectReport.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.mb_SelectReport.BackColor = System.Drawing.Color.Transparent;
         this.mb_SelectReport.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.mb_SelectReport.Caption = "انتخاب گزارش";
         this.mb_SelectReport.Disabled = false;
         this.mb_SelectReport.EnterColor = System.Drawing.Color.Transparent;
         this.mb_SelectReport.ForeColor = System.Drawing.SystemColors.ControlText;
         this.mb_SelectReport.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_SelectReport.ImageIndex = -1;
         this.mb_SelectReport.ImageList = null;
         this.mb_SelectReport.InToBold = false;
         this.mb_SelectReport.Location = new System.Drawing.Point(733, 507);
         this.mb_SelectReport.Name = "mb_SelectReport";
         this.mb_SelectReport.Size = new System.Drawing.Size(80, 25);
         this.mb_SelectReport.TabIndex = 3;
         this.mb_SelectReport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_SelectReport.TextColor = System.Drawing.Color.Black;
         this.mb_SelectReport.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.mb_SelectReport.Click += new System.EventHandler(this.mb_SelectReport_Click);
         // 
         // mb_CfgStngF
         // 
         this.mb_CfgStngF.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.mb_CfgStngF.BackColor = System.Drawing.Color.Transparent;
         this.mb_CfgStngF.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.mb_CfgStngF.Caption = "تنظیمات چاپ فرم گزارش";
         this.mb_CfgStngF.Disabled = false;
         this.mb_CfgStngF.Enabled = false;
         this.mb_CfgStngF.EnterColor = System.Drawing.Color.Transparent;
         this.mb_CfgStngF.ForeColor = System.Drawing.SystemColors.ControlText;
         this.mb_CfgStngF.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_CfgStngF.ImageIndex = -1;
         this.mb_CfgStngF.ImageList = null;
         this.mb_CfgStngF.InToBold = false;
         this.mb_CfgStngF.Location = new System.Drawing.Point(55, 507);
         this.mb_CfgStngF.Name = "mb_CfgStngF";
         this.mb_CfgStngF.Size = new System.Drawing.Size(144, 25);
         this.mb_CfgStngF.TabIndex = 3;
         this.mb_CfgStngF.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_CfgStngF.TextColor = System.Drawing.Color.Black;
         this.mb_CfgStngF.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.mb_CfgStngF.Visible = false;
         this.mb_CfgStngF.Click += new System.EventHandler(this.mb_CfgStngF_Click);
         // 
         // mb_reloading
         // 
         this.mb_reloading.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.mb_reloading.BackColor = System.Drawing.Color.Transparent;
         this.mb_reloading.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.mb_reloading.Caption = "بارگذاری مجدد";
         this.mb_reloading.Disabled = false;
         this.mb_reloading.EnterColor = System.Drawing.Color.Transparent;
         this.mb_reloading.ForeColor = System.Drawing.SystemColors.ControlText;
         this.mb_reloading.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_reloading.ImageIndex = -1;
         this.mb_reloading.ImageList = null;
         this.mb_reloading.InToBold = false;
         this.mb_reloading.Location = new System.Drawing.Point(647, 507);
         this.mb_reloading.Name = "mb_reloading";
         this.mb_reloading.Size = new System.Drawing.Size(80, 25);
         this.mb_reloading.TabIndex = 3;
         this.mb_reloading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_reloading.TextColor = System.Drawing.Color.Black;
         this.mb_reloading.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.mb_reloading.Click += new System.EventHandler(this.mb_reloading_Click);
         // 
         // mb_back
         // 
         this.mb_back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.mb_back.BackColor = System.Drawing.Color.Transparent;
         this.mb_back.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.mb_back.Caption = "بازگشت";
         this.mb_back.Disabled = false;
         this.mb_back.EnterColor = System.Drawing.Color.Transparent;
         this.mb_back.ForeColor = System.Drawing.SystemColors.ControlText;
         this.mb_back.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_back.ImageIndex = -1;
         this.mb_back.ImageList = null;
         this.mb_back.InToBold = false;
         this.mb_back.Location = new System.Drawing.Point(561, 507);
         this.mb_back.Name = "mb_back";
         this.mb_back.Size = new System.Drawing.Size(80, 25);
         this.mb_back.TabIndex = 4;
         this.mb_back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.mb_back.TextColor = System.Drawing.Color.Black;
         this.mb_back.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.mb_back.Click += new System.EventHandler(this.mb_back_Click);
         // 
         // RPT_LRFM_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
         this.Controls.Add(this.mb_back);
         this.Controls.Add(this.mb_CfgStngF);
         this.Controls.Add(this.mb_reloading);
         this.Controls.Add(this.mb_SelectReport);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.modual_ReportGridControl);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "RPT_LRFM_F";
         this.Size = new System.Drawing.Size(867, 577);
         ((System.ComponentModel.ISupportInitialize)(this.modual_ReportGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MdrpBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DYsnoBs1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.BindingSource MdrpBs1;
      private DevExpress.XtraGrid.GridControl modual_ReportGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colCODE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDUL_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn colMDUL_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colSECT_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn colSECT_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colRWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colRPRT_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colRPRT_PATH;
      private DevExpress.XtraGrid.Columns.GridColumn colSHOW_PRVW;
      private DevExpress.XtraGrid.Columns.GridColumn colDFLT;
      private DevExpress.XtraGrid.Columns.GridColumn colSTAT;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
      private Windows.Forms.BindingSource DYsnoBs1;
      private Windows.Forms.Label label1;
      private MaxUi.NewMaxBtn mb_SelectReport;
      private MaxUi.NewMaxBtn mb_CfgStngF;
      private MaxUi.NewMaxBtn mb_reloading;
      private MaxUi.NewMaxBtn mb_back;
   }
}
