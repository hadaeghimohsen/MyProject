﻿namespace System.CRM.Ui.HistoryAction
{
   partial class HST_LOGC_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HST_LOGC_F));
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.colLOG_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panel1 = new System.Windows.Forms.Panel();
         this.AddLeads_Butn = new System.MaxUi.RoundedButton();
         this.Back_Butn = new System.MaxUi.RoundedButton();
         this.roundedButton3 = new System.MaxUi.RoundedButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.gridControl1 = new DevExpress.XtraGrid.GridControl();
         this.LogcBs = new System.Windows.Forms.BindingSource(this.components);
         this.Logc_Gv = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colRQRO_RQST_RQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQRO_RWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSERV_FILE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSRPB_RWNO_DNRM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSRPN_RECT_CODE_DNRM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCOMP_CODE_DNRM = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLCID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSUBJ_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLOG_CMNT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLOG_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRSLT_STAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCompany = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequest_Row = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colService = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colService_Public = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colTime_Period = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colGrop_Time_Period = new DevExpress.XtraGrid.Columns.GridColumn();
         this.RqstActn_Butn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.Optn_Pn = new System.Windows.Forms.Panel();
         this.RqstDate_Butn = new System.MaxUi.RoundedButton();
         this.RqstToDate_Date = new Atf.UI.DateTimeSelector();
         this.ChosseDayRqst_Cb = new System.Windows.Forms.RadioButton();
         this.TodayRqst_Cb = new System.Windows.Forms.RadioButton();
         this.YesterdayRqst_Cb = new System.Windows.Forms.RadioButton();
         this.AllRqst_Cb = new System.Windows.Forms.RadioButton();
         this.MonthRqst_Cb = new System.Windows.Forms.RadioButton();
         this.WeekRqst_Cb = new System.Windows.Forms.RadioButton();
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.Comment_Txt = new DevExpress.XtraEditors.MemoEdit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.LogcBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Logc_Gv)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstActn_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         this.Optn_Pn.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Comment_Txt.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // colLOG_TYPE
         // 
         this.colLOG_TYPE.FieldName = "LOG_TYPE";
         this.colLOG_TYPE.Name = "colLOG_TYPE";
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.panel1.Controls.Add(this.AddLeads_Butn);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Controls.Add(this.roundedButton3);
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(604, 53);
         this.panel1.TabIndex = 0;
         // 
         // AddLeads_Butn
         // 
         this.AddLeads_Butn.Active = true;
         this.AddLeads_Butn.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.AddLeads_Butn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.AddLeads_Butn.Caption = "";
         this.AddLeads_Butn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.AddLeads_Butn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.AddLeads_Butn.HoverBorderColor = System.Drawing.Color.Gold;
         this.AddLeads_Butn.HoverColorA = System.Drawing.Color.LightGray;
         this.AddLeads_Butn.HoverColorB = System.Drawing.Color.LightGray;
         this.AddLeads_Butn.ImageProfile = global::System.CRM.Properties.Resources.IMAGE_1522;
         this.AddLeads_Butn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.AddLeads_Butn.ImageVisiable = true;
         this.AddLeads_Butn.Location = new System.Drawing.Point(105, 7);
         this.AddLeads_Butn.Name = "AddLeads_Butn";
         this.AddLeads_Butn.NormalBorderColor = System.Drawing.Color.LightGray;
         this.AddLeads_Butn.NormalColorA = System.Drawing.Color.White;
         this.AddLeads_Butn.NormalColorB = System.Drawing.Color.White;
         this.AddLeads_Butn.Size = new System.Drawing.Size(40, 40);
         this.AddLeads_Butn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.AddLeads_Butn.TabIndex = 3;
         this.AddLeads_Butn.Visible = false;
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
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // roundedButton3
         // 
         this.roundedButton3.Active = true;
         this.roundedButton3.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.roundedButton3.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.roundedButton3.Caption = "";
         this.roundedButton3.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.roundedButton3.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.roundedButton3.HoverBorderColor = System.Drawing.Color.Gold;
         this.roundedButton3.HoverColorA = System.Drawing.Color.LightGray;
         this.roundedButton3.HoverColorB = System.Drawing.Color.LightGray;
         this.roundedButton3.ImageProfile = global::System.CRM.Properties.Resources.IMAGE_1521;
         this.roundedButton3.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.roundedButton3.ImageVisiable = true;
         this.roundedButton3.Location = new System.Drawing.Point(59, 7);
         this.roundedButton3.Name = "roundedButton3";
         this.roundedButton3.NormalBorderColor = System.Drawing.Color.LightGray;
         this.roundedButton3.NormalColorA = System.Drawing.Color.White;
         this.roundedButton3.NormalColorB = System.Drawing.Color.White;
         this.roundedButton3.Size = new System.Drawing.Size(40, 40);
         this.roundedButton3.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.roundedButton3.TabIndex = 4;
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
         this.labelControl1.Location = new System.Drawing.Point(390, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(214, 53);
         this.labelControl1.TabIndex = 1;
         this.labelControl1.Text = "تاریخچه تماس تلفنی";
         // 
         // gridControl1
         // 
         this.gridControl1.DataSource = this.LogcBs;
         this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl1.Location = new System.Drawing.Point(0, 0);
         this.gridControl1.LookAndFeel.SkinName = "Office 2013";
         this.gridControl1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.gridControl1.MainView = this.Logc_Gv;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RqstActn_Butn,
            this.persianRepositoryItemDateEdit1});
         this.gridControl1.Size = new System.Drawing.Size(604, 431);
         this.gridControl1.TabIndex = 1;
         this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.Logc_Gv});
         // 
         // LogcBs
         // 
         this.LogcBs.DataSource = typeof(System.CRM.Data.Log_Call);
         // 
         // Logc_Gv
         // 
         this.Logc_Gv.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.Logc_Gv.Appearance.FocusedRow.Options.UseBackColor = true;
         this.Logc_Gv.Appearance.GroupRow.Options.UseTextOptions = true;
         this.Logc_Gv.Appearance.GroupRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Logc_Gv.Appearance.HeaderPanel.Font = new System.Drawing.Font("IRANSans", 9F);
         this.Logc_Gv.Appearance.HeaderPanel.Options.UseFont = true;
         this.Logc_Gv.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.Logc_Gv.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Logc_Gv.Appearance.Row.Font = new System.Drawing.Font("IRANSans", 9F);
         this.Logc_Gv.Appearance.Row.Options.UseFont = true;
         this.Logc_Gv.Appearance.Row.Options.UseTextOptions = true;
         this.Logc_Gv.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Logc_Gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRQRO_RQST_RQID,
            this.colRQRO_RWNO,
            this.colSERV_FILE_NO,
            this.colSRPB_RWNO_DNRM,
            this.colSRPN_RECT_CODE_DNRM,
            this.colCOMP_CODE_DNRM,
            this.colLCID,
            this.colLOG_TYPE,
            this.colSUBJ_DESC,
            this.colLOG_CMNT,
            this.colLOG_DATE,
            this.colRSLT_STAT,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colCompany,
            this.colRequest_Row,
            this.colService,
            this.colService_Public,
            this.colTime_Period,
            this.colGrop_Time_Period});
         styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
         styleFormatCondition1.Appearance.Options.UseBackColor = true;
         styleFormatCondition1.ApplyToRow = true;
         styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
         styleFormatCondition1.Expression = "[LOG_TYPE] == \'002\'  And [RSLT_STAT]  == \'002\'";
         styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
         styleFormatCondition2.Appearance.Options.UseBackColor = true;
         styleFormatCondition2.ApplyToRow = true;
         styleFormatCondition2.Column = this.colLOG_TYPE;
         styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition2.Value1 = "001";
         styleFormatCondition3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
         styleFormatCondition3.Appearance.Options.UseBackColor = true;
         styleFormatCondition3.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
         styleFormatCondition3.Expression = "[LOG_TYPE] == \'002\' And [RSLT_STAT] == \'001\'";
         this.Logc_Gv.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2,
            styleFormatCondition3});
         this.Logc_Gv.GridControl = this.gridControl1;
         this.Logc_Gv.GroupCount = 1;
         this.Logc_Gv.Name = "Logc_Gv";
         this.Logc_Gv.OptionsBehavior.AutoExpandAllGroups = true;
         this.Logc_Gv.OptionsBehavior.Editable = false;
         this.Logc_Gv.OptionsBehavior.ReadOnly = true;
         this.Logc_Gv.OptionsFind.AlwaysVisible = true;
         this.Logc_Gv.OptionsFind.FindDelay = 100;
         this.Logc_Gv.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.Logc_Gv.OptionsView.EnableAppearanceEvenRow = true;
         this.Logc_Gv.OptionsView.ShowDetailButtons = false;
         this.Logc_Gv.OptionsView.ShowIndicator = false;
         this.Logc_Gv.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colGrop_Time_Period, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colCRET_BY, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colTime_Period, DevExpress.Data.ColumnSortOrder.Ascending)});
         this.Logc_Gv.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.Logc_Gv_CustomUnboundColumnData);
         this.Logc_Gv.DoubleClick += new System.EventHandler(this.Logc_Gv_DoubleClick);
         // 
         // colRQRO_RQST_RQID
         // 
         this.colRQRO_RQST_RQID.FieldName = "RQRO_RQST_RQID";
         this.colRQRO_RQST_RQID.Name = "colRQRO_RQST_RQID";
         // 
         // colRQRO_RWNO
         // 
         this.colRQRO_RWNO.FieldName = "RQRO_RWNO";
         this.colRQRO_RWNO.Name = "colRQRO_RWNO";
         // 
         // colSERV_FILE_NO
         // 
         this.colSERV_FILE_NO.FieldName = "SERV_FILE_NO";
         this.colSERV_FILE_NO.Name = "colSERV_FILE_NO";
         // 
         // colSRPB_RWNO_DNRM
         // 
         this.colSRPB_RWNO_DNRM.FieldName = "SRPB_RWNO_DNRM";
         this.colSRPB_RWNO_DNRM.Name = "colSRPB_RWNO_DNRM";
         // 
         // colSRPN_RECT_CODE_DNRM
         // 
         this.colSRPN_RECT_CODE_DNRM.FieldName = "SRPN_RECT_CODE_DNRM";
         this.colSRPN_RECT_CODE_DNRM.Name = "colSRPN_RECT_CODE_DNRM";
         // 
         // colCOMP_CODE_DNRM
         // 
         this.colCOMP_CODE_DNRM.FieldName = "COMP_CODE_DNRM";
         this.colCOMP_CODE_DNRM.Name = "colCOMP_CODE_DNRM";
         // 
         // colLCID
         // 
         this.colLCID.FieldName = "LCID";
         this.colLCID.Name = "colLCID";
         // 
         // colSUBJ_DESC
         // 
         this.colSUBJ_DESC.Caption = "موضوع";
         this.colSUBJ_DESC.FieldName = "SUBJ_DESC";
         this.colSUBJ_DESC.Name = "colSUBJ_DESC";
         this.colSUBJ_DESC.Visible = true;
         this.colSUBJ_DESC.VisibleIndex = 2;
         // 
         // colLOG_CMNT
         // 
         this.colLOG_CMNT.FieldName = "LOG_CMNT";
         this.colLOG_CMNT.Name = "colLOG_CMNT";
         // 
         // colLOG_DATE
         // 
         this.colLOG_DATE.FieldName = "LOG_DATE";
         this.colLOG_DATE.Name = "colLOG_DATE";
         // 
         // colRSLT_STAT
         // 
         this.colRSLT_STAT.FieldName = "RSLT_STAT";
         this.colRSLT_STAT.Name = "colRSLT_STAT";
         // 
         // colCRET_BY
         // 
         this.colCRET_BY.Caption = "کاربر ثبت کننده";
         this.colCRET_BY.FieldName = "CRET_BY";
         this.colCRET_BY.Name = "colCRET_BY";
         this.colCRET_BY.Visible = true;
         this.colCRET_BY.VisibleIndex = 1;
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
         // colCompany
         // 
         this.colCompany.FieldName = "Company";
         this.colCompany.Name = "colCompany";
         // 
         // colRequest_Row
         // 
         this.colRequest_Row.FieldName = "Request_Row";
         this.colRequest_Row.Name = "colRequest_Row";
         // 
         // colService
         // 
         this.colService.FieldName = "Service";
         this.colService.Name = "colService";
         // 
         // colService_Public
         // 
         this.colService_Public.FieldName = "Service_Public";
         this.colService_Public.Name = "colService_Public";
         // 
         // colTime_Period
         // 
         this.colTime_Period.Caption = "زمان";
         this.colTime_Period.FieldName = "DATE_TIME_DESC";
         this.colTime_Period.Name = "colTime_Period";
         this.colTime_Period.UnboundType = DevExpress.Data.UnboundColumnType.Object;
         this.colTime_Period.Visible = true;
         this.colTime_Period.VisibleIndex = 0;
         // 
         // colGrop_Time_Period
         // 
         this.colGrop_Time_Period.Caption = "زمانی";
         this.colGrop_Time_Period.FieldName = "GROP_DATE_DESC";
         this.colGrop_Time_Period.Name = "colGrop_Time_Period";
         this.colGrop_Time_Period.UnboundType = DevExpress.Data.UnboundColumnType.Object;
         this.colGrop_Time_Period.Visible = true;
         this.colGrop_Time_Period.VisibleIndex = 3;
         // 
         // RqstActn_Butn
         // 
         this.RqstActn_Butn.AutoHeight = false;
         this.RqstActn_Butn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, global::System.CRM.Properties.Resources.IMAGE_1583, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "مشاهده تاریخچه فعالیت", null, null, true)});
         this.RqstActn_Butn.Name = "RqstActn_Butn";
         this.RqstActn_Butn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
         // 
         // persianRepositoryItemDateEdit1
         // 
         this.persianRepositoryItemDateEdit1.AutoHeight = false;
         this.persianRepositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.persianRepositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.persianRepositoryItemDateEdit1.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
         this.persianRepositoryItemDateEdit1.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
         this.persianRepositoryItemDateEdit1.Name = "persianRepositoryItemDateEdit1";
         // 
         // Optn_Pn
         // 
         this.Optn_Pn.Controls.Add(this.RqstDate_Butn);
         this.Optn_Pn.Controls.Add(this.RqstToDate_Date);
         this.Optn_Pn.Controls.Add(this.ChosseDayRqst_Cb);
         this.Optn_Pn.Controls.Add(this.TodayRqst_Cb);
         this.Optn_Pn.Controls.Add(this.YesterdayRqst_Cb);
         this.Optn_Pn.Controls.Add(this.AllRqst_Cb);
         this.Optn_Pn.Controls.Add(this.MonthRqst_Cb);
         this.Optn_Pn.Controls.Add(this.WeekRqst_Cb);
         this.Optn_Pn.Dock = System.Windows.Forms.DockStyle.Top;
         this.Optn_Pn.Location = new System.Drawing.Point(0, 53);
         this.Optn_Pn.Name = "Optn_Pn";
         this.Optn_Pn.Size = new System.Drawing.Size(604, 104);
         this.Optn_Pn.TabIndex = 0;
         // 
         // RqstDate_Butn
         // 
         this.RqstDate_Butn.Active = true;
         this.RqstDate_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.RqstDate_Butn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.RqstDate_Butn.Caption = "";
         this.RqstDate_Butn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.RqstDate_Butn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.RqstDate_Butn.HoverBorderColor = System.Drawing.Color.Gold;
         this.RqstDate_Butn.HoverColorA = System.Drawing.Color.LightGray;
         this.RqstDate_Butn.HoverColorB = System.Drawing.Color.LightGray;
         this.RqstDate_Butn.ImageProfile = null;
         this.RqstDate_Butn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.RqstDate_Butn.ImageVisiable = false;
         this.RqstDate_Butn.Location = new System.Drawing.Point(133, 74);
         this.RqstDate_Butn.Name = "RqstDate_Butn";
         this.RqstDate_Butn.NormalBorderColor = System.Drawing.Color.LightGray;
         this.RqstDate_Butn.NormalColorA = System.Drawing.Color.Red;
         this.RqstDate_Butn.NormalColorB = System.Drawing.Color.Red;
         this.RqstDate_Butn.Size = new System.Drawing.Size(23, 23);
         this.RqstDate_Butn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.RqstDate_Butn.TabIndex = 3;
         this.RqstDate_Butn.Click += new System.EventHandler(this.RqstDate_Butn_Click);
         // 
         // RqstToDate_Date
         // 
         this.RqstToDate_Date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.RqstToDate_Date.CustomFormat = "dd/MM/yyyy";
         this.RqstToDate_Date.Format = Atf.UI.DateTimeSelectorFormat.Custom;
         this.RqstToDate_Date.Location = new System.Drawing.Point(162, 74);
         this.RqstToDate_Date.Name = "RqstToDate_Date";
         this.RqstToDate_Date.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.RqstToDate_Date.Size = new System.Drawing.Size(100, 22);
         this.RqstToDate_Date.TabIndex = 11;
         this.RqstToDate_Date.UsePersianFormat = true;
         // 
         // ChosseDayRqst_Cb
         // 
         this.ChosseDayRqst_Cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.ChosseDayRqst_Cb.AutoSize = true;
         this.ChosseDayRqst_Cb.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.ChosseDayRqst_Cb.Location = new System.Drawing.Point(283, 72);
         this.ChosseDayRqst_Cb.Name = "ChosseDayRqst_Cb";
         this.ChosseDayRqst_Cb.Size = new System.Drawing.Size(91, 26);
         this.ChosseDayRqst_Cb.TabIndex = 0;
         this.ChosseDayRqst_Cb.Tag = "006";
         this.ChosseDayRqst_Cb.Text = "انتخاب تاریخ";
         this.ChosseDayRqst_Cb.UseVisualStyleBackColor = true;
         this.ChosseDayRqst_Cb.CheckedChanged += new System.EventHandler(this.rb_requestsearch_CheckedChanged);
         // 
         // TodayRqst_Cb
         // 
         this.TodayRqst_Cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.TodayRqst_Cb.AutoSize = true;
         this.TodayRqst_Cb.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.TodayRqst_Cb.Location = new System.Drawing.Point(437, 72);
         this.TodayRqst_Cb.Name = "TodayRqst_Cb";
         this.TodayRqst_Cb.Size = new System.Drawing.Size(106, 26);
         this.TodayRqst_Cb.TabIndex = 0;
         this.TodayRqst_Cb.Tag = "005";
         this.TodayRqst_Cb.Text = "تماس های امروز";
         this.TodayRqst_Cb.UseVisualStyleBackColor = true;
         this.TodayRqst_Cb.CheckedChanged += new System.EventHandler(this.rb_requestsearch_CheckedChanged);
         // 
         // YesterdayRqst_Cb
         // 
         this.YesterdayRqst_Cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.YesterdayRqst_Cb.AutoSize = true;
         this.YesterdayRqst_Cb.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.YesterdayRqst_Cb.Location = new System.Drawing.Point(267, 40);
         this.YesterdayRqst_Cb.Name = "YesterdayRqst_Cb";
         this.YesterdayRqst_Cb.Size = new System.Drawing.Size(107, 26);
         this.YesterdayRqst_Cb.TabIndex = 0;
         this.YesterdayRqst_Cb.Tag = "004";
         this.YesterdayRqst_Cb.Text = "تماس های دیروز";
         this.YesterdayRqst_Cb.UseVisualStyleBackColor = true;
         this.YesterdayRqst_Cb.CheckedChanged += new System.EventHandler(this.rb_requestsearch_CheckedChanged);
         // 
         // AllRqst_Cb
         // 
         this.AllRqst_Cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.AllRqst_Cb.AutoSize = true;
         this.AllRqst_Cb.Checked = true;
         this.AllRqst_Cb.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.AllRqst_Cb.Location = new System.Drawing.Point(450, 8);
         this.AllRqst_Cb.Name = "AllRqst_Cb";
         this.AllRqst_Cb.Size = new System.Drawing.Size(93, 26);
         this.AllRqst_Cb.TabIndex = 0;
         this.AllRqst_Cb.TabStop = true;
         this.AllRqst_Cb.Tag = "001";
         this.AllRqst_Cb.Text = "کلیه تماس ها";
         this.AllRqst_Cb.UseVisualStyleBackColor = true;
         this.AllRqst_Cb.CheckedChanged += new System.EventHandler(this.rb_requestsearch_CheckedChanged);
         // 
         // MonthRqst_Cb
         // 
         this.MonthRqst_Cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.MonthRqst_Cb.AutoSize = true;
         this.MonthRqst_Cb.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.MonthRqst_Cb.Location = new System.Drawing.Point(236, 8);
         this.MonthRqst_Cb.Name = "MonthRqst_Cb";
         this.MonthRqst_Cb.Size = new System.Drawing.Size(138, 26);
         this.MonthRqst_Cb.TabIndex = 0;
         this.MonthRqst_Cb.Tag = "002";
         this.MonthRqst_Cb.Text = "تماس های یکماه پیش";
         this.MonthRqst_Cb.UseVisualStyleBackColor = true;
         this.MonthRqst_Cb.CheckedChanged += new System.EventHandler(this.rb_requestsearch_CheckedChanged);
         // 
         // WeekRqst_Cb
         // 
         this.WeekRqst_Cb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.WeekRqst_Cb.AutoSize = true;
         this.WeekRqst_Cb.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.WeekRqst_Cb.Location = new System.Drawing.Point(407, 40);
         this.WeekRqst_Cb.Name = "WeekRqst_Cb";
         this.WeekRqst_Cb.Size = new System.Drawing.Size(136, 26);
         this.WeekRqst_Cb.TabIndex = 0;
         this.WeekRqst_Cb.Tag = "003";
         this.WeekRqst_Cb.Text = "تماس های هفته پیش";
         this.WeekRqst_Cb.UseVisualStyleBackColor = true;
         this.WeekRqst_Cb.CheckedChanged += new System.EventHandler(this.rb_requestsearch_CheckedChanged);
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.Horizontal = false;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 157);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Controls.Add(this.Comment_Txt);
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(604, 511);
         this.splitContainerControl1.SplitterPosition = 431;
         this.splitContainerControl1.TabIndex = 3;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // Comment_Txt
         // 
         this.Comment_Txt.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.LogcBs, "LOG_CMNT", true));
         this.Comment_Txt.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Comment_Txt.Location = new System.Drawing.Point(0, 0);
         this.Comment_Txt.Name = "Comment_Txt";
         this.Comment_Txt.Properties.Appearance.BorderColor = System.Drawing.Color.Black;
         this.Comment_Txt.Properties.Appearance.Font = new System.Drawing.Font("IRANSans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Comment_Txt.Properties.Appearance.Options.UseBorderColor = true;
         this.Comment_Txt.Properties.Appearance.Options.UseFont = true;
         this.Comment_Txt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.Comment_Txt.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.Comment_Txt.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.Comment_Txt.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.Comment_Txt.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
         this.Comment_Txt.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Comment_Txt.Properties.ReadOnly = true;
         this.Comment_Txt.Size = new System.Drawing.Size(604, 75);
         this.Comment_Txt.TabIndex = 2;
         // 
         // HST_LOGC_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.splitContainerControl1);
         this.Controls.Add(this.Optn_Pn);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "HST_LOGC_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(604, 668);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.LogcBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Logc_Gv)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstActn_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         this.Optn_Pn.ResumeLayout(false);
         this.Optn_Pn.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Comment_Txt.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private MaxUi.RoundedButton AddLeads_Butn;
      private MaxUi.RoundedButton roundedButton3;
      private MaxUi.RoundedButton Back_Butn;
      private Windows.Forms.Panel Optn_Pn;
      private Windows.Forms.RadioButton ChosseDayRqst_Cb;
      private Windows.Forms.RadioButton YesterdayRqst_Cb;
      private Windows.Forms.RadioButton WeekRqst_Cb;
      private Atf.UI.DateTimeSelector RqstToDate_Date;
      private DevExpress.XtraGrid.GridControl gridControl1;
      private DevExpress.XtraGrid.Views.Grid.GridView Logc_Gv;
      private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit RqstActn_Butn;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private MaxUi.RoundedButton RqstDate_Butn;
      private Windows.Forms.RadioButton TodayRqst_Cb;
      private Windows.Forms.RadioButton AllRqst_Cb;
      private Windows.Forms.RadioButton MonthRqst_Cb;
      private Windows.Forms.BindingSource LogcBs;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RQST_RQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colSERV_FILE_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colSRPB_RWNO_DNRM;
      private DevExpress.XtraGrid.Columns.GridColumn colSRPN_RECT_CODE_DNRM;
      private DevExpress.XtraGrid.Columns.GridColumn colCOMP_CODE_DNRM;
      private DevExpress.XtraGrid.Columns.GridColumn colLCID;
      private DevExpress.XtraGrid.Columns.GridColumn colLOG_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn colSUBJ_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colLOG_CMNT;
      private DevExpress.XtraGrid.Columns.GridColumn colLOG_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colRSLT_STAT;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colCompany;
      private DevExpress.XtraGrid.Columns.GridColumn colRequest_Row;
      private DevExpress.XtraGrid.Columns.GridColumn colService;
      private DevExpress.XtraGrid.Columns.GridColumn colService_Public;
      private DevExpress.XtraGrid.Columns.GridColumn colTime_Period;
      private DevExpress.XtraGrid.Columns.GridColumn colGrop_Time_Period;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private DevExpress.XtraEditors.MemoEdit Comment_Txt;
   }
}
