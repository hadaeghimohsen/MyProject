namespace System.CRM.Ui.Notification
{
   partial class NOTF_TOTL_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NOTF_TOTL_F));
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition3 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition4 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition5 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.colREAD_RMND = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colTimePeriod = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.panel1 = new System.Windows.Forms.Panel();
         this.label2 = new System.Windows.Forms.Label();
         this.Lnk_MarkAllAsRead = new System.Windows.Forms.LinkLabel();
         this.Lnk_Settings = new System.Windows.Forms.LinkLabel();
         this.label1 = new System.Windows.Forms.Label();
         this.RmndBs = new System.Windows.Forms.BindingSource(this.components);
         this.reminderGridControl = new DevExpress.XtraGrid.GridControl();
         this.Notification_Gv = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
         this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.colNotf_Butn = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.Notf_Butn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
         this.colFROM_JOBP_CODE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colSERV_FILE_NO = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colRQST_RQID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colTO_JOBP_CODE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colCODE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colREAD_NOTF = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colRECD_STAT = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colJob_Personnel = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colRequest = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colService = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colJob_Personnel1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colGroupTimePeriod = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colALRM_TIME = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
         this.colALRM_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.panel2 = new System.Windows.Forms.Panel();
         this.panel3 = new System.Windows.Forms.Panel();
         this.Refresh_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.Close_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.RmndBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.reminderGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Notification_Gv)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Notf_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         this.panel3.SuspendLayout();
         this.SuspendLayout();
         // 
         // colREAD_RMND
         // 
         this.colREAD_RMND.FieldName = "READ_RMND";
         this.colREAD_RMND.Name = "colREAD_RMND";
         // 
         // colTimePeriod
         // 
         this.colTimePeriod.Caption = "مدت زمان";
         this.colTimePeriod.FieldName = "DATE_TIME_DESC";
         this.colTimePeriod.Image = ((System.Drawing.Image)(resources.GetObject("colTimePeriod.Image")));
         this.colTimePeriod.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colTimePeriod.Name = "colTimePeriod";
         this.colTimePeriod.OptionsColumn.AllowEdit = false;
         this.colTimePeriod.OptionsColumn.ReadOnly = true;
         this.colTimePeriod.UnboundType = DevExpress.Data.UnboundColumnType.String;
         this.colTimePeriod.Visible = true;
         this.colTimePeriod.Width = 137;
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
         this.panel1.Controls.Add(this.label2);
         this.panel1.Controls.Add(this.Lnk_MarkAllAsRead);
         this.panel1.Controls.Add(this.Lnk_Settings);
         this.panel1.Controls.Add(this.label1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(687, 38);
         this.panel1.TabIndex = 4;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Symbol", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.label2.Location = new System.Drawing.Point(56, 10);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(15, 19);
         this.label2.TabIndex = 6;
         this.label2.Text = "·";
         // 
         // Lnk_MarkAllAsRead
         // 
         this.Lnk_MarkAllAsRead.AutoSize = true;
         this.Lnk_MarkAllAsRead.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
         this.Lnk_MarkAllAsRead.Location = new System.Drawing.Point(77, 13);
         this.Lnk_MarkAllAsRead.Name = "Lnk_MarkAllAsRead";
         this.Lnk_MarkAllAsRead.Size = new System.Drawing.Size(207, 14);
         this.Lnk_MarkAllAsRead.TabIndex = 5;
         this.Lnk_MarkAllAsRead.TabStop = true;
         this.Lnk_MarkAllAsRead.Text = "همه را به عنوان خوانده شده علامت بزن";
         this.Lnk_MarkAllAsRead.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Lnk_MarkAllAsRead_LinkClicked);
         // 
         // Lnk_Settings
         // 
         this.Lnk_Settings.AutoSize = true;
         this.Lnk_Settings.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
         this.Lnk_Settings.Location = new System.Drawing.Point(3, 13);
         this.Lnk_Settings.Name = "Lnk_Settings";
         this.Lnk_Settings.Size = new System.Drawing.Size(47, 14);
         this.Lnk_Settings.TabIndex = 5;
         this.Lnk_Settings.TabStop = true;
         this.Lnk_Settings.Text = "تنظیمات";
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Iranian Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.Location = new System.Drawing.Point(546, 10);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(125, 19);
         this.label1.TabIndex = 4;
         this.label1.Text = "اخطار و یادآوری";
         // 
         // RmndBs
         // 
         this.RmndBs.DataSource = typeof(System.CRM.Data.Reminder);
         // 
         // reminderGridControl
         // 
         this.reminderGridControl.DataSource = this.RmndBs;
         this.reminderGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.reminderGridControl.Location = new System.Drawing.Point(0, 38);
         this.reminderGridControl.MainView = this.Notification_Gv;
         this.reminderGridControl.Name = "reminderGridControl";
         this.reminderGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.persianRepositoryItemDateEdit1,
            this.repositoryItemTimeEdit1,
            this.Notf_Butn});
         this.reminderGridControl.Size = new System.Drawing.Size(673, 442);
         this.reminderGridControl.TabIndex = 5;
         this.reminderGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.Notification_Gv});
         // 
         // Notification_Gv
         // 
         this.Notification_Gv.Appearance.GroupRow.Font = new System.Drawing.Font("IRAN Sans", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Notification_Gv.Appearance.GroupRow.Options.UseFont = true;
         this.Notification_Gv.Appearance.GroupRow.Options.UseTextOptions = true;
         this.Notification_Gv.Appearance.GroupRow.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Notification_Gv.Appearance.HeaderPanel.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Notification_Gv.Appearance.HeaderPanel.Options.UseFont = true;
         this.Notification_Gv.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.Notification_Gv.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Notification_Gv.Appearance.Row.Options.UseTextOptions = true;
         this.Notification_Gv.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Notification_Gv.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
         this.Notification_Gv.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colRQST_RQID,
            this.colSERV_FILE_NO,
            this.colFROM_JOBP_CODE,
            this.colTO_JOBP_CODE,
            this.colCODE,
            this.colREAD_RMND,
            this.colREAD_NOTF,
            this.colALRM_DATE,
            this.colRECD_STAT,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colJob_Personnel,
            this.colRequest,
            this.colService,
            this.colJob_Personnel1,
            this.colALRM_TIME,
            this.colTimePeriod,
            this.colNotf_Butn,
            this.colGroupTimePeriod});
         styleFormatCondition1.Appearance.Font = new System.Drawing.Font("IRAN Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         styleFormatCondition1.Appearance.Options.UseFont = true;
         styleFormatCondition1.ApplyToRow = true;
         styleFormatCondition1.Column = this.colREAD_RMND;
         styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition1.Value1 = "001";
         styleFormatCondition2.Appearance.Font = new System.Drawing.Font("IRAN Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         styleFormatCondition2.Appearance.Options.UseFont = true;
         styleFormatCondition2.ApplyToRow = true;
         styleFormatCondition2.Column = this.colREAD_RMND;
         styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition2.Value1 = "002";
         styleFormatCondition3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(247)))), ((int)(((byte)(230)))));
         styleFormatCondition3.Appearance.Options.UseBackColor = true;
         styleFormatCondition3.ApplyToRow = true;
         styleFormatCondition3.Column = this.colTimePeriod;
         styleFormatCondition3.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
         styleFormatCondition3.Expression = "DateDiffTick([ALRM_DATE], Now())  <= 0";
         styleFormatCondition4.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
         styleFormatCondition4.Appearance.Options.UseBackColor = true;
         styleFormatCondition4.ApplyToRow = true;
         styleFormatCondition4.Column = this.colTimePeriod;
         styleFormatCondition4.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
         styleFormatCondition4.Expression = "DateDiffTick([ALRM_DATE], Now())  > 0";
         styleFormatCondition5.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
         styleFormatCondition5.Appearance.Font = new System.Drawing.Font("IRAN Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         styleFormatCondition5.Appearance.Options.UseBackColor = true;
         styleFormatCondition5.Appearance.Options.UseFont = true;
         styleFormatCondition5.ApplyToRow = true;
         styleFormatCondition5.Condition = DevExpress.XtraGrid.FormatConditionEnum.Expression;
         styleFormatCondition5.Expression = "[READ_RMND] == \'001\' AND DateDiffTick([ALRM_DATE], Now())  > 0";
         this.Notification_Gv.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2,
            styleFormatCondition3,
            styleFormatCondition4,
            styleFormatCondition5});
         this.Notification_Gv.GridControl = this.reminderGridControl;
         this.Notification_Gv.GroupCount = 1;
         this.Notification_Gv.GroupFormat = "{1}";
         this.Notification_Gv.Name = "Notification_Gv";
         this.Notification_Gv.OptionsBehavior.AutoExpandAllGroups = true;
         this.Notification_Gv.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.Notification_Gv.OptionsView.ColumnAutoWidth = true;
         this.Notification_Gv.OptionsView.ShowGroupPanel = false;
         this.Notification_Gv.OptionsView.ShowIndicator = false;
         this.Notification_Gv.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
         this.Notification_Gv.RowHeight = 40;
         this.Notification_Gv.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colGroupTimePeriod, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colALRM_DATE, DevExpress.Data.ColumnSortOrder.Descending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colALRM_TIME, DevExpress.Data.ColumnSortOrder.Descending)});
         this.Notification_Gv.CustomUnboundColumnData += new DevExpress.XtraGrid.Views.Base.CustomColumnDataEventHandler(this.Notification_Gv_CustomUnboundColumnData);
         this.Notification_Gv.DoubleClick += new System.EventHandler(this.Notification_Gv_DoubleClick);
         // 
         // gridBand1
         // 
         this.gridBand1.Columns.Add(this.colNotf_Butn);
         this.gridBand1.Columns.Add(this.colTimePeriod);
         this.gridBand1.Columns.Add(this.colTO_JOBP_CODE);
         this.gridBand1.Columns.Add(this.colFROM_JOBP_CODE);
         this.gridBand1.Columns.Add(this.colSERV_FILE_NO);
         this.gridBand1.Columns.Add(this.colRQST_RQID);
         this.gridBand1.Columns.Add(this.colCODE);
         this.gridBand1.Columns.Add(this.colREAD_RMND);
         this.gridBand1.Columns.Add(this.colREAD_NOTF);
         this.gridBand1.Columns.Add(this.colRECD_STAT);
         this.gridBand1.Columns.Add(this.colCRET_BY);
         this.gridBand1.Columns.Add(this.colCRET_DATE);
         this.gridBand1.Columns.Add(this.colMDFY_BY);
         this.gridBand1.Columns.Add(this.colMDFY_DATE);
         this.gridBand1.Columns.Add(this.colJob_Personnel);
         this.gridBand1.Columns.Add(this.colRequest);
         this.gridBand1.Columns.Add(this.colService);
         this.gridBand1.Columns.Add(this.colJob_Personnel1);
         this.gridBand1.Columns.Add(this.colGroupTimePeriod);
         this.gridBand1.Columns.Add(this.colALRM_TIME);
         this.gridBand1.Columns.Add(this.colALRM_DATE);
         this.gridBand1.Name = "gridBand1";
         this.gridBand1.VisibleIndex = 0;
         this.gridBand1.Width = 671;
         // 
         // colNotf_Butn
         // 
         this.colNotf_Butn.ColumnEdit = this.Notf_Butn;
         this.colNotf_Butn.Name = "colNotf_Butn";
         this.colNotf_Butn.OptionsColumn.FixedWidth = true;
         this.colNotf_Butn.Visible = true;
         this.colNotf_Butn.Width = 37;
         // 
         // Notf_Butn
         // 
         this.Notf_Butn.AutoHeight = false;
         this.Notf_Butn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("Notf_Butn.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.Notf_Butn.Name = "Notf_Butn";
         this.Notf_Butn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
         this.Notf_Butn.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Notf_Butn_ButtonClick);
         // 
         // colFROM_JOBP_CODE
         // 
         this.colFROM_JOBP_CODE.Caption = "ارسال کننده";
         this.colFROM_JOBP_CODE.FieldName = "Job_Personnel.USER_DESC_DNRM";
         this.colFROM_JOBP_CODE.Image = ((System.Drawing.Image)(resources.GetObject("colFROM_JOBP_CODE.Image")));
         this.colFROM_JOBP_CODE.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colFROM_JOBP_CODE.Name = "colFROM_JOBP_CODE";
         this.colFROM_JOBP_CODE.OptionsColumn.AllowEdit = false;
         this.colFROM_JOBP_CODE.OptionsColumn.ReadOnly = true;
         this.colFROM_JOBP_CODE.Visible = true;
         this.colFROM_JOBP_CODE.Width = 113;
         // 
         // colSERV_FILE_NO
         // 
         this.colSERV_FILE_NO.Caption = "مشتری";
         this.colSERV_FILE_NO.FieldName = "Service.NAME_DNRM";
         this.colSERV_FILE_NO.Image = ((System.Drawing.Image)(resources.GetObject("colSERV_FILE_NO.Image")));
         this.colSERV_FILE_NO.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colSERV_FILE_NO.Name = "colSERV_FILE_NO";
         this.colSERV_FILE_NO.OptionsColumn.AllowEdit = false;
         this.colSERV_FILE_NO.OptionsColumn.ReadOnly = true;
         this.colSERV_FILE_NO.Visible = true;
         this.colSERV_FILE_NO.Width = 165;
         // 
         // colRQST_RQID
         // 
         this.colRQST_RQID.Caption = "درخواست";
         this.colRQST_RQID.FieldName = "Request.Request_Type.RQTP_DESC";
         this.colRQST_RQID.Image = ((System.Drawing.Image)(resources.GetObject("colRQST_RQID.Image")));
         this.colRQST_RQID.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colRQST_RQID.Name = "colRQST_RQID";
         this.colRQST_RQID.OptionsColumn.AllowEdit = false;
         this.colRQST_RQID.OptionsColumn.ReadOnly = true;
         this.colRQST_RQID.Visible = true;
         this.colRQST_RQID.Width = 111;
         // 
         // colTO_JOBP_CODE
         // 
         this.colTO_JOBP_CODE.Caption = "دریافت کننده";
         this.colTO_JOBP_CODE.FieldName = "Job_Personnel1.USER_DESC_DNRM";
         this.colTO_JOBP_CODE.Image = ((System.Drawing.Image)(resources.GetObject("colTO_JOBP_CODE.Image")));
         this.colTO_JOBP_CODE.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colTO_JOBP_CODE.Name = "colTO_JOBP_CODE";
         this.colTO_JOBP_CODE.Visible = true;
         this.colTO_JOBP_CODE.Width = 104;
         // 
         // colCODE
         // 
         this.colCODE.FieldName = "CODE";
         this.colCODE.Name = "colCODE";
         // 
         // colREAD_NOTF
         // 
         this.colREAD_NOTF.FieldName = "READ_NOTF";
         this.colREAD_NOTF.Name = "colREAD_NOTF";
         // 
         // colRECD_STAT
         // 
         this.colRECD_STAT.FieldName = "RECD_STAT";
         this.colRECD_STAT.Name = "colRECD_STAT";
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
         // colJob_Personnel
         // 
         this.colJob_Personnel.FieldName = "Job_Personnel";
         this.colJob_Personnel.Name = "colJob_Personnel";
         // 
         // colRequest
         // 
         this.colRequest.FieldName = "Request";
         this.colRequest.Name = "colRequest";
         // 
         // colService
         // 
         this.colService.FieldName = "Service";
         this.colService.Name = "colService";
         // 
         // colJob_Personnel1
         // 
         this.colJob_Personnel1.FieldName = "Job_Personnel1";
         this.colJob_Personnel1.Name = "colJob_Personnel1";
         // 
         // colGroupTimePeriod
         // 
         this.colGroupTimePeriod.AppearanceCell.Font = new System.Drawing.Font("IRAN Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.colGroupTimePeriod.AppearanceCell.Options.UseFont = true;
         this.colGroupTimePeriod.AppearanceCell.Options.UseTextOptions = true;
         this.colGroupTimePeriod.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.colGroupTimePeriod.Caption = " ";
         this.colGroupTimePeriod.FieldName = "GROP_DATE_DESC";
         this.colGroupTimePeriod.Name = "colGroupTimePeriod";
         this.colGroupTimePeriod.RowIndex = 1;
         this.colGroupTimePeriod.SortMode = DevExpress.XtraGrid.ColumnSortMode.DisplayText;
         this.colGroupTimePeriod.UnboundType = DevExpress.Data.UnboundColumnType.Object;
         // 
         // colALRM_TIME
         // 
         this.colALRM_TIME.Caption = "زمان";
         this.colALRM_TIME.ColumnEdit = this.repositoryItemTimeEdit1;
         this.colALRM_TIME.FieldName = "ALRM_DATE";
         this.colALRM_TIME.Image = ((System.Drawing.Image)(resources.GetObject("colALRM_TIME.Image")));
         this.colALRM_TIME.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colALRM_TIME.Name = "colALRM_TIME";
         this.colALRM_TIME.OptionsColumn.AllowEdit = false;
         this.colALRM_TIME.OptionsColumn.ReadOnly = true;
         this.colALRM_TIME.RowIndex = 2;
         this.colALRM_TIME.Width = 477;
         // 
         // repositoryItemTimeEdit1
         // 
         this.repositoryItemTimeEdit1.AutoHeight = false;
         this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemTimeEdit1.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
         this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
         this.repositoryItemTimeEdit1.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
         // 
         // colALRM_DATE
         // 
         this.colALRM_DATE.Caption = "تاریخ انجام";
         this.colALRM_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colALRM_DATE.FieldName = "ALRM_DATE";
         this.colALRM_DATE.Image = ((System.Drawing.Image)(resources.GetObject("colALRM_DATE.Image")));
         this.colALRM_DATE.ImageAlignment = System.Drawing.StringAlignment.Far;
         this.colALRM_DATE.Name = "colALRM_DATE";
         this.colALRM_DATE.OptionsColumn.AllowEdit = false;
         this.colALRM_DATE.OptionsColumn.ReadOnly = true;
         this.colALRM_DATE.RowIndex = 2;
         this.colALRM_DATE.Width = 477;
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
         // panel2
         // 
         this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
         this.panel2.Location = new System.Drawing.Point(673, 38);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(14, 491);
         this.panel2.TabIndex = 6;
         // 
         // panel3
         // 
         this.panel3.Controls.Add(this.Refresh_Butn);
         this.panel3.Controls.Add(this.Close_Butn);
         this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panel3.Location = new System.Drawing.Point(0, 480);
         this.panel3.Name = "panel3";
         this.panel3.Size = new System.Drawing.Size(673, 49);
         this.panel3.TabIndex = 7;
         // 
         // Refresh_Butn
         // 
         this.Refresh_Butn.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.Refresh_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.Refresh_Butn.Appearance.BorderColor = System.Drawing.Color.WhiteSmoke;
         this.Refresh_Butn.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Refresh_Butn.Appearance.Options.UseBackColor = true;
         this.Refresh_Butn.Appearance.Options.UseBorderColor = true;
         this.Refresh_Butn.Appearance.Options.UseFont = true;
         this.Refresh_Butn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.Refresh_Butn.Image = global::System.CRM.Properties.Resources.IMAGE_1562;
         this.Refresh_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
         this.Refresh_Butn.Location = new System.Drawing.Point(49, 6);
         this.Refresh_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Refresh_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Refresh_Butn.Name = "Refresh_Butn";
         this.Refresh_Butn.Size = new System.Drawing.Size(36, 36);
         this.Refresh_Butn.TabIndex = 8;
         this.Refresh_Butn.ToolTip = "اضافه کردن یادداشت جدید";
         this.Refresh_Butn.Click += new System.EventHandler(this.Refresh_Butn_Click);
         // 
         // Close_Butn
         // 
         this.Close_Butn.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.Close_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.Close_Butn.Appearance.BorderColor = System.Drawing.Color.WhiteSmoke;
         this.Close_Butn.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Close_Butn.Appearance.Options.UseBackColor = true;
         this.Close_Butn.Appearance.Options.UseBorderColor = true;
         this.Close_Butn.Appearance.Options.UseFont = true;
         this.Close_Butn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.Close_Butn.Image = global::System.CRM.Properties.Resources.IMAGE_1196;
         this.Close_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
         this.Close_Butn.Location = new System.Drawing.Point(7, 6);
         this.Close_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Close_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Close_Butn.Name = "Close_Butn";
         this.Close_Butn.Size = new System.Drawing.Size(36, 36);
         this.Close_Butn.TabIndex = 8;
         this.Close_Butn.ToolTip = "اضافه کردن یادداشت جدید";
         this.Close_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // NOTF_TOTL_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.reminderGridControl);
         this.Controls.Add(this.panel3);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "NOTF_TOTL_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(687, 529);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.RmndBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.reminderGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Notification_Gv)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Notf_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         this.panel3.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private Windows.Forms.Label label1;
      private Windows.Forms.BindingSource RmndBs;
      private DevExpress.XtraGrid.GridControl reminderGridControl;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView Notification_Gv;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colRQST_RQID;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colFROM_JOBP_CODE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTO_JOBP_CODE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCODE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colREAD_RMND;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colREAD_NOTF;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colRECD_STAT;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCRET_BY;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colJob_Personnel;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colRequest;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colService;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colJob_Personnel1;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSERV_FILE_NO;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colALRM_DATE;
      private Windows.Forms.Label label2;
      private Windows.Forms.LinkLabel Lnk_MarkAllAsRead;
      private Windows.Forms.LinkLabel Lnk_Settings;
      private Windows.Forms.Panel panel2;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colALRM_TIME;
      private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTimePeriod;
      private Windows.Forms.Panel panel3;
      private DevExpress.XtraEditors.SimpleButton Refresh_Butn;
      private DevExpress.XtraEditors.SimpleButton Close_Butn;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colNotf_Butn;
      private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit Notf_Butn;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colGroupTimePeriod;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
   }
}
