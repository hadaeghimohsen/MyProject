﻿namespace System.Scsc.Ui.UserAction
{
   partial class USR_CTBL_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(USR_CTBL_F));
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         this.colRQTT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.Back_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.panel1 = new System.Windows.Forms.Panel();
         this.CnclAllRqst_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.Reload_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.Request_Gv = new DevExpress.XtraGrid.GridControl();
         this.RqstBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colREGN_PRVN_CNTY_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colREGN_PRVN_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colREGN_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQST_RQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQTP_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSUB_SYS = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQST_STAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQST_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.colSAVE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLETT_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLETT_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLETT_OWNR = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSSTT_MSTT_SUB_SYS = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSSTT_MSTT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSSTT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colYEAR = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCYCL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSEND_EXPN = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDUL_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSECT_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRegion = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequest1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequester_Type = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSub_State = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequest_Type = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMonth_Base = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQST_CHEK = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Actn_Butn = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Request_Gv)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Actn_Butn)).BeginInit();
         this.SuspendLayout();
         // 
         // colRQTT_CODE
         // 
         this.colRQTT_CODE.Caption = "متقاضی درخواست";
         this.colRQTT_CODE.FieldName = "RQTT_CODE";
         this.colRQTT_CODE.Name = "colRQTT_CODE";
         this.colRQTT_CODE.OptionsColumn.AllowEdit = false;
         this.colRQTT_CODE.OptionsColumn.ReadOnly = true;
         this.colRQTT_CODE.Width = 103;
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.ForeColor = System.Drawing.SystemColors.Control;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Location = new System.Drawing.Point(248, 4);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(127, 50);
         this.labelControl1.TabIndex = 3;
         this.labelControl1.Text = "لیست کارتابل";
         // 
         // Back_Butn
         // 
         this.Back_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Back_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Back_Butn.Appearance.Options.UseBackColor = true;
         this.Back_Butn.Image = ((System.Drawing.Image)(resources.GetObject("Back_Butn.Image")));
         this.Back_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Back_Butn.Location = new System.Drawing.Point(381, 3);
         this.Back_Butn.LookAndFeel.SkinName = "Office 2010 Silver";
         this.Back_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Back_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.Size = new System.Drawing.Size(56, 51);
         this.Back_Butn.TabIndex = 2;
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // panel1
         // 
         this.panel1.BackColor = System.Drawing.Color.Gray;
         this.panel1.Controls.Add(this.CnclAllRqst_Butn);
         this.panel1.Controls.Add(this.Reload_Butn);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(452, 61);
         this.panel1.TabIndex = 4;
         // 
         // CnclAllRqst_Butn
         // 
         this.CnclAllRqst_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.CnclAllRqst_Butn.Appearance.Options.UseBackColor = true;
         this.CnclAllRqst_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1057;
         this.CnclAllRqst_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.CnclAllRqst_Butn.Location = new System.Drawing.Point(63, 3);
         this.CnclAllRqst_Butn.LookAndFeel.SkinName = "Office 2010 Silver";
         this.CnclAllRqst_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.CnclAllRqst_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.CnclAllRqst_Butn.Name = "CnclAllRqst_Butn";
         this.CnclAllRqst_Butn.Size = new System.Drawing.Size(54, 51);
         this.CnclAllRqst_Butn.TabIndex = 4;
         this.CnclAllRqst_Butn.Click += new System.EventHandler(this.CnclAllRqst_Butn_Click);
         // 
         // Reload_Butn
         // 
         this.Reload_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Reload_Butn.Appearance.Options.UseBackColor = true;
         this.Reload_Butn.Image = ((System.Drawing.Image)(resources.GetObject("Reload_Butn.Image")));
         this.Reload_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Reload_Butn.Location = new System.Drawing.Point(3, 3);
         this.Reload_Butn.LookAndFeel.SkinName = "Office 2010 Silver";
         this.Reload_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Reload_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Reload_Butn.Name = "Reload_Butn";
         this.Reload_Butn.Size = new System.Drawing.Size(54, 51);
         this.Reload_Butn.TabIndex = 4;
         this.Reload_Butn.Click += new System.EventHandler(this.Reload_Butn_Click);
         // 
         // Request_Gv
         // 
         this.Request_Gv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Request_Gv.DataSource = this.RqstBs1;
         this.Request_Gv.Location = new System.Drawing.Point(0, 61);
         this.Request_Gv.LookAndFeel.SkinName = "Office 2013 Dark Gray";
         this.Request_Gv.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Request_Gv.MainView = this.gridView1;
         this.Request_Gv.Name = "Request_Gv";
         this.Request_Gv.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Actn_Butn,
            this.persianRepositoryItemDateEdit1});
         this.Request_Gv.Size = new System.Drawing.Size(437, 680);
         this.Request_Gv.TabIndex = 5;
         this.Request_Gv.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // RqstBs1
         // 
         this.RqstBs1.DataSource = typeof(System.Scsc.Data.Request);
         // 
         // gridView1
         // 
         this.gridView1.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9F);
         this.gridView1.Appearance.FooterPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F);
         this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F);
         this.gridView1.Appearance.Row.Options.UseFont = true;
         this.gridView1.Appearance.Row.Options.UseTextOptions = true;
         this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colREGN_PRVN_CNTY_CODE,
            this.colREGN_PRVN_CODE,
            this.colREGN_CODE,
            this.colRQST_RQID,
            this.colRQID,
            this.colRQTP_CODE,
            this.colRQTT_CODE,
            this.colSUB_SYS,
            this.colRQST_STAT,
            this.colRQST_DATE,
            this.colSAVE_DATE,
            this.colLETT_NO,
            this.colLETT_DATE,
            this.colLETT_OWNR,
            this.colSSTT_MSTT_SUB_SYS,
            this.colSSTT_MSTT_CODE,
            this.colSSTT_CODE,
            this.colYEAR,
            this.colCYCL,
            this.colSEND_EXPN,
            this.colMDUL_NAME,
            this.colSECT_NAME,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colRegion,
            this.colRequest1,
            this.colRequester_Type,
            this.colSub_State,
            this.colRequest_Type,
            this.colMonth_Base,
            this.colRQST_CHEK});
         styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         styleFormatCondition1.Appearance.Options.UseBackColor = true;
         styleFormatCondition1.ApplyToRow = true;
         styleFormatCondition1.Column = this.colRQTT_CODE;
         styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition1.Value1 = "001";
         styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         styleFormatCondition2.Appearance.Options.UseBackColor = true;
         styleFormatCondition2.ApplyToRow = true;
         styleFormatCondition2.Column = this.colRQTT_CODE;
         styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition2.Value1 = "004";
         this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2});
         this.gridView1.GridControl = this.Request_Gv;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowDetailButtons = false;
         this.gridView1.OptionsView.ShowFooter = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colREGN_PRVN_CNTY_CODE
         // 
         this.colREGN_PRVN_CNTY_CODE.FieldName = "REGN_PRVN_CNTY_CODE";
         this.colREGN_PRVN_CNTY_CODE.Name = "colREGN_PRVN_CNTY_CODE";
         // 
         // colREGN_PRVN_CODE
         // 
         this.colREGN_PRVN_CODE.FieldName = "REGN_PRVN_CODE";
         this.colREGN_PRVN_CODE.Name = "colREGN_PRVN_CODE";
         // 
         // colREGN_CODE
         // 
         this.colREGN_CODE.FieldName = "REGN_CODE";
         this.colREGN_CODE.Name = "colREGN_CODE";
         // 
         // colRQST_RQID
         // 
         this.colRQST_RQID.FieldName = "RQST_RQID";
         this.colRQST_RQID.Name = "colRQST_RQID";
         // 
         // colRQID
         // 
         this.colRQID.FieldName = "RQID";
         this.colRQID.Name = "colRQID";
         // 
         // colRQTP_CODE
         // 
         this.colRQTP_CODE.Caption = "نوع درخواست";
         this.colRQTP_CODE.FieldName = "Request_Type.RQTP_DESC";
         this.colRQTP_CODE.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.Value;
         this.colRQTP_CODE.Name = "colRQTP_CODE";
         this.colRQTP_CODE.OptionsColumn.AllowEdit = false;
         this.colRQTP_CODE.OptionsColumn.ReadOnly = true;
         this.colRQTP_CODE.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
         this.colRQTP_CODE.Visible = true;
         this.colRQTP_CODE.VisibleIndex = 3;
         this.colRQTP_CODE.Width = 187;
         // 
         // colSUB_SYS
         // 
         this.colSUB_SYS.FieldName = "SUB_SYS";
         this.colSUB_SYS.Name = "colSUB_SYS";
         // 
         // colRQST_STAT
         // 
         this.colRQST_STAT.FieldName = "RQST_STAT";
         this.colRQST_STAT.Name = "colRQST_STAT";
         // 
         // colRQST_DATE
         // 
         this.colRQST_DATE.Caption = "تاریخ درخواست";
         this.colRQST_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colRQST_DATE.FieldName = "RQST_DATE";
         this.colRQST_DATE.Name = "colRQST_DATE";
         this.colRQST_DATE.OptionsColumn.AllowEdit = false;
         this.colRQST_DATE.OptionsColumn.ReadOnly = true;
         this.colRQST_DATE.Visible = true;
         this.colRQST_DATE.VisibleIndex = 1;
         this.colRQST_DATE.Width = 91;
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
         // colSAVE_DATE
         // 
         this.colSAVE_DATE.FieldName = "SAVE_DATE";
         this.colSAVE_DATE.Name = "colSAVE_DATE";
         // 
         // colLETT_NO
         // 
         this.colLETT_NO.FieldName = "LETT_NO";
         this.colLETT_NO.Name = "colLETT_NO";
         // 
         // colLETT_DATE
         // 
         this.colLETT_DATE.FieldName = "LETT_DATE";
         this.colLETT_DATE.Name = "colLETT_DATE";
         // 
         // colLETT_OWNR
         // 
         this.colLETT_OWNR.FieldName = "LETT_OWNR";
         this.colLETT_OWNR.Name = "colLETT_OWNR";
         // 
         // colSSTT_MSTT_SUB_SYS
         // 
         this.colSSTT_MSTT_SUB_SYS.FieldName = "SSTT_MSTT_SUB_SYS";
         this.colSSTT_MSTT_SUB_SYS.Name = "colSSTT_MSTT_SUB_SYS";
         // 
         // colSSTT_MSTT_CODE
         // 
         this.colSSTT_MSTT_CODE.FieldName = "SSTT_MSTT_CODE";
         this.colSSTT_MSTT_CODE.Name = "colSSTT_MSTT_CODE";
         // 
         // colSSTT_CODE
         // 
         this.colSSTT_CODE.FieldName = "SSTT_CODE";
         this.colSSTT_CODE.Name = "colSSTT_CODE";
         // 
         // colYEAR
         // 
         this.colYEAR.FieldName = "YEAR";
         this.colYEAR.Name = "colYEAR";
         // 
         // colCYCL
         // 
         this.colCYCL.FieldName = "CYCL";
         this.colCYCL.Name = "colCYCL";
         // 
         // colSEND_EXPN
         // 
         this.colSEND_EXPN.FieldName = "SEND_EXPN";
         this.colSEND_EXPN.Name = "colSEND_EXPN";
         // 
         // colMDUL_NAME
         // 
         this.colMDUL_NAME.FieldName = "MDUL_NAME";
         this.colMDUL_NAME.Name = "colMDUL_NAME";
         // 
         // colSECT_NAME
         // 
         this.colSECT_NAME.FieldName = "SECT_NAME";
         this.colSECT_NAME.Name = "colSECT_NAME";
         // 
         // colCRET_BY
         // 
         this.colCRET_BY.Caption = "کاربر ثبت";
         this.colCRET_BY.FieldName = "CRET_BY";
         this.colCRET_BY.Name = "colCRET_BY";
         this.colCRET_BY.OptionsColumn.AllowEdit = false;
         this.colCRET_BY.OptionsColumn.ReadOnly = true;
         this.colCRET_BY.Visible = true;
         this.colCRET_BY.VisibleIndex = 2;
         this.colCRET_BY.Width = 89;
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
         // colRegion
         // 
         this.colRegion.FieldName = "Region";
         this.colRegion.Name = "colRegion";
         // 
         // colRequest1
         // 
         this.colRequest1.FieldName = "Request1";
         this.colRequest1.Name = "colRequest1";
         // 
         // colRequester_Type
         // 
         this.colRequester_Type.FieldName = "Requester_Type";
         this.colRequester_Type.Name = "colRequester_Type";
         // 
         // colSub_State
         // 
         this.colSub_State.FieldName = "Sub_State";
         this.colSub_State.Name = "colSub_State";
         // 
         // colRequest_Type
         // 
         this.colRequest_Type.FieldName = "Request_Type";
         this.colRequest_Type.Name = "colRequest_Type";
         // 
         // colMonth_Base
         // 
         this.colMonth_Base.FieldName = "Month_Base";
         this.colMonth_Base.Name = "colMonth_Base";
         // 
         // colRQST_CHEK
         // 
         this.colRQST_CHEK.Caption = "عملیات";
         this.colRQST_CHEK.ColumnEdit = this.Actn_Butn;
         this.colRQST_CHEK.Name = "colRQST_CHEK";
         this.colRQST_CHEK.OptionsColumn.FixedWidth = true;
         this.colRQST_CHEK.Visible = true;
         this.colRQST_CHEK.VisibleIndex = 0;
         this.colRQST_CHEK.Width = 68;
         // 
         // Actn_Butn
         // 
         this.Actn_Butn.AutoHeight = false;
         this.Actn_Butn.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("Actn_Butn.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "رسیدگی به درخواست", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("Actn_Butn.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "انصراف درخواست", null, null, true)});
         this.Actn_Butn.Name = "Actn_Butn";
         this.Actn_Butn.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
         this.Actn_Butn.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Actn_Butn_ButtonClick);
         // 
         // USR_CTBL_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DimGray;
         this.Controls.Add(this.Request_Gv);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "USR_CTBL_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(452, 741);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Request_Gv)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Actn_Butn)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.SimpleButton Back_Butn;
      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton CnclAllRqst_Butn;
      private DevExpress.XtraEditors.SimpleButton Reload_Butn;
      private DevExpress.XtraGrid.GridControl Request_Gv;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit Actn_Butn;
      private Windows.Forms.BindingSource RqstBs1;
      private DevExpress.XtraGrid.Columns.GridColumn colREGN_PRVN_CNTY_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colREGN_PRVN_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colREGN_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRQST_RQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTP_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTT_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colSUB_SYS;
      private DevExpress.XtraGrid.Columns.GridColumn colRQST_STAT;
      private DevExpress.XtraGrid.Columns.GridColumn colRQST_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colSAVE_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colLETT_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colLETT_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colLETT_OWNR;
      private DevExpress.XtraGrid.Columns.GridColumn colSSTT_MSTT_SUB_SYS;
      private DevExpress.XtraGrid.Columns.GridColumn colSSTT_MSTT_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colSSTT_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colYEAR;
      private DevExpress.XtraGrid.Columns.GridColumn colCYCL;
      private DevExpress.XtraGrid.Columns.GridColumn colSEND_EXPN;
      private DevExpress.XtraGrid.Columns.GridColumn colMDUL_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn colSECT_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colRegion;
      private DevExpress.XtraGrid.Columns.GridColumn colRequest1;
      private DevExpress.XtraGrid.Columns.GridColumn colRequester_Type;
      private DevExpress.XtraGrid.Columns.GridColumn colSub_State;
      private DevExpress.XtraGrid.Columns.GridColumn colRequest_Type;
      private DevExpress.XtraGrid.Columns.GridColumn colMonth_Base;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private DevExpress.XtraGrid.Columns.GridColumn colRQST_CHEK;
   }
}
