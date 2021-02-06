namespace System.RoboTech.Ui.Action
{
   partial class STNG_RPRT_F
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
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STNG_RPRT_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
         this.Actv_Lov = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.DactvBs = new System.Windows.Forms.BindingSource();
         this.Tb_master = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.countryGridControl = new DevExpress.XtraGrid.GridControl();
         this.MdrpBs = new System.Windows.Forms.BindingSource();
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
         this.Ysno_Lov = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.DysnoBs = new System.Windows.Forms.BindingSource();
         this.colDFLT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colPRNT_AFTR_PAY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSTAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.panel1 = new System.Windows.Forms.Panel();
         this.Del_Butn = new C1.Win.C1Input.C1Button();
         this.Save_Butn = new C1.Win.C1Input.C1Button();
         this.Add_Butn = new C1.Win.C1Input.C1Button();
         this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
         this.SelectFile_Butn = new DevExpress.XtraEditors.ButtonEdit();
         this.Btn_Back = new C1.Win.C1Input.C1Button();
         this.Ofd_ReportFiles = new System.Windows.Forms.OpenFileDialog();
         ((System.ComponentModel.ISupportInitialize)(this.Actv_Lov)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DactvBs)).BeginInit();
         this.Tb_master.SuspendLayout();
         this.tabPage1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.countryGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.MdrpBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Ysno_Lov)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DysnoBs)).BeginInit();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Del_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Save_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Add_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.SelectFile_Butn.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).BeginInit();
         this.SuspendLayout();
         // 
         // Actv_Lov
         // 
         this.Actv_Lov.AutoHeight = false;
         this.Actv_Lov.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.Actv_Lov.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "وضعیت", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.Actv_Lov.DataSource = this.DactvBs;
         this.Actv_Lov.DisplayMember = "DOMN_DESC";
         this.Actv_Lov.Name = "Actv_Lov";
         this.Actv_Lov.NullText = "";
         this.Actv_Lov.ValueMember = "VALU";
         // 
         // DactvBs
         // 
         this.DactvBs.DataSource = typeof(System.RoboTech.Data.D_ACTV);
         // 
         // Tb_master
         // 
         this.Tb_master.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Tb_master.Controls.Add(this.tabPage1);
         this.Tb_master.Location = new System.Drawing.Point(3, 3);
         this.Tb_master.Name = "Tb_master";
         this.Tb_master.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Tb_master.RightToLeftLayout = true;
         this.Tb_master.SelectedIndex = 0;
         this.Tb_master.Size = new System.Drawing.Size(732, 477);
         this.Tb_master.TabIndex = 0;
         // 
         // tabPage1
         // 
         this.tabPage1.Controls.Add(this.countryGridControl);
         this.tabPage1.Controls.Add(this.panel1);
         this.tabPage1.Location = new System.Drawing.Point(4, 23);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(724, 450);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "تنظیمات گزارش";
         this.tabPage1.UseVisualStyleBackColor = true;
         // 
         // countryGridControl
         // 
         this.countryGridControl.DataSource = this.MdrpBs;
         this.countryGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.countryGridControl.Location = new System.Drawing.Point(3, 3);
         this.countryGridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.countryGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.countryGridControl.MainView = this.gridView1;
         this.countryGridControl.Name = "countryGridControl";
         this.countryGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Actv_Lov,
            this.Ysno_Lov});
         this.countryGridControl.Size = new System.Drawing.Size(718, 388);
         this.countryGridControl.TabIndex = 1;
         this.countryGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // MdrpBs
         // 
         this.MdrpBs.DataSource = typeof(System.RoboTech.Data.Modual_Report);
         // 
         // gridView1
         // 
         this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F);
         this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F);
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
            this.colPRNT_AFTR_PAY,
            this.colSTAT,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE});
         styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         styleFormatCondition1.Appearance.Options.UseBackColor = true;
         styleFormatCondition1.ApplyToRow = true;
         styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition1.Value1 = "001";
         styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         styleFormatCondition2.Appearance.Options.UseBackColor = true;
         styleFormatCondition2.ApplyToRow = true;
         styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.Equal;
         styleFormatCondition2.Value1 = "002";
         this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1,
            styleFormatCondition2});
         this.gridView1.GridControl = this.countryGridControl;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsFind.AlwaysVisible = true;
         this.gridView1.OptionsFind.FindDelay = 100;
         this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView1.OptionsView.ShowDetailButtons = false;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
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
         this.colMDUL_NAME.Name = "colMDUL_NAME";
         this.colMDUL_NAME.OptionsColumn.AllowEdit = false;
         this.colMDUL_NAME.OptionsColumn.FixedWidth = true;
         this.colMDUL_NAME.OptionsColumn.ReadOnly = true;
         this.colMDUL_NAME.Visible = true;
         this.colMDUL_NAME.VisibleIndex = 5;
         this.colMDUL_NAME.Width = 94;
         // 
         // colMDUL_DESC
         // 
         this.colMDUL_DESC.Caption = "عنوان فرم";
         this.colMDUL_DESC.FieldName = "MDUL_DESC";
         this.colMDUL_DESC.Name = "colMDUL_DESC";
         // 
         // colSECT_NAME
         // 
         this.colSECT_NAME.Caption = "تب فرم";
         this.colSECT_NAME.FieldName = "SECT_NAME";
         this.colSECT_NAME.Name = "colSECT_NAME";
         this.colSECT_NAME.OptionsColumn.FixedWidth = true;
         this.colSECT_NAME.Visible = true;
         this.colSECT_NAME.VisibleIndex = 4;
         this.colSECT_NAME.Width = 88;
         // 
         // colSECT_DESC
         // 
         this.colSECT_DESC.FieldName = "SECT_DESC";
         this.colSECT_DESC.Name = "colSECT_DESC";
         // 
         // colRWNO
         // 
         this.colRWNO.Caption = "ردیف";
         this.colRWNO.FieldName = "RWNO";
         this.colRWNO.Name = "colRWNO";
         this.colRWNO.OptionsColumn.FixedWidth = true;
         this.colRWNO.Visible = true;
         this.colRWNO.VisibleIndex = 6;
         this.colRWNO.Width = 40;
         // 
         // colRPRT_DESC
         // 
         this.colRPRT_DESC.Caption = "عنوان چاپ";
         this.colRPRT_DESC.FieldName = "RPRT_DESC";
         this.colRPRT_DESC.Name = "colRPRT_DESC";
         this.colRPRT_DESC.Visible = true;
         this.colRPRT_DESC.VisibleIndex = 3;
         this.colRPRT_DESC.Width = 245;
         // 
         // colRPRT_PATH
         // 
         this.colRPRT_PATH.Caption = "مسیر گزارش";
         this.colRPRT_PATH.FieldName = "RPRT_PATH";
         this.colRPRT_PATH.Name = "colRPRT_PATH";
         // 
         // colSHOW_PRVW
         // 
         this.colSHOW_PRVW.Caption = "گزارش نمایش داده شود";
         this.colSHOW_PRVW.ColumnEdit = this.Ysno_Lov;
         this.colSHOW_PRVW.FieldName = "SHOW_PRVW";
         this.colSHOW_PRVW.Name = "colSHOW_PRVW";
         this.colSHOW_PRVW.OptionsColumn.FixedWidth = true;
         this.colSHOW_PRVW.Visible = true;
         this.colSHOW_PRVW.VisibleIndex = 1;
         this.colSHOW_PRVW.Width = 132;
         // 
         // Ysno_Lov
         // 
         this.Ysno_Lov.AutoHeight = false;
         this.Ysno_Lov.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
         this.Ysno_Lov.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "DOMN_DESC", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.Ysno_Lov.DataSource = this.DysnoBs;
         this.Ysno_Lov.DisplayMember = "DOMN_DESC";
         this.Ysno_Lov.Name = "Ysno_Lov";
         this.Ysno_Lov.NullText = "";
         this.Ysno_Lov.ValueMember = "VALU";
         // 
         // DysnoBs
         // 
         this.DysnoBs.DataSource = typeof(System.RoboTech.Data.D_YSNO);
         // 
         // colDFLT
         // 
         this.colDFLT.Caption = "پیشفرض";
         this.colDFLT.ColumnEdit = this.Ysno_Lov;
         this.colDFLT.FieldName = "DFLT";
         this.colDFLT.Name = "colDFLT";
         this.colDFLT.OptionsColumn.FixedWidth = true;
         this.colDFLT.Visible = true;
         this.colDFLT.VisibleIndex = 2;
         this.colDFLT.Width = 60;
         // 
         // colPRNT_AFTR_PAY
         // 
         this.colPRNT_AFTR_PAY.FieldName = "PRNT_AFTR_PAY";
         this.colPRNT_AFTR_PAY.Name = "colPRNT_AFTR_PAY";
         // 
         // colSTAT
         // 
         this.colSTAT.Caption = "وضعیت";
         this.colSTAT.ColumnEdit = this.Actv_Lov;
         this.colSTAT.FieldName = "STAT";
         this.colSTAT.Name = "colSTAT";
         this.colSTAT.OptionsColumn.FixedWidth = true;
         this.colSTAT.Visible = true;
         this.colSTAT.VisibleIndex = 0;
         this.colSTAT.Width = 55;
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
         this.panel1.Controls.Add(this.Del_Butn);
         this.panel1.Controls.Add(this.Save_Butn);
         this.panel1.Controls.Add(this.Add_Butn);
         this.panel1.Controls.Add(this.labelControl13);
         this.panel1.Controls.Add(this.SelectFile_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.panel1.Location = new System.Drawing.Point(3, 391);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(718, 56);
         this.panel1.TabIndex = 2;
         // 
         // Del_Butn
         // 
         this.Del_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.Del_Butn.Image = global::System.RoboTech.Properties.Resources.IMAGE_1197;
         this.Del_Butn.Location = new System.Drawing.Point(14, 11);
         this.Del_Butn.Name = "Del_Butn";
         this.Del_Butn.Size = new System.Drawing.Size(35, 33);
         this.Del_Butn.TabIndex = 44;
         this.Del_Butn.UseVisualStyleBackColor = true;
         this.Del_Butn.Click += new System.EventHandler(this.Del_Butn_Click);
         // 
         // Save_Butn
         // 
         this.Save_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.Save_Butn.Image = global::System.RoboTech.Properties.Resources.IMAGE_1195;
         this.Save_Butn.Location = new System.Drawing.Point(55, 11);
         this.Save_Butn.Name = "Save_Butn";
         this.Save_Butn.Size = new System.Drawing.Size(35, 33);
         this.Save_Butn.TabIndex = 42;
         this.Save_Butn.UseVisualStyleBackColor = true;
         this.Save_Butn.Click += new System.EventHandler(this.Save_Butn_Click_1);
         // 
         // Add_Butn
         // 
         this.Add_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.Add_Butn.Image = global::System.RoboTech.Properties.Resources.IMAGE_1198;
         this.Add_Butn.Location = new System.Drawing.Point(96, 11);
         this.Add_Butn.Name = "Add_Butn";
         this.Add_Butn.Size = new System.Drawing.Size(35, 33);
         this.Add_Butn.TabIndex = 43;
         this.Add_Butn.UseVisualStyleBackColor = true;
         this.Add_Butn.Click += new System.EventHandler(this.Add_Butn_Click);
         // 
         // labelControl13
         // 
         this.labelControl13.AllowHtmlString = true;
         this.labelControl13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl13.Appearance.Font = new System.Drawing.Font("IRANSans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl13.Appearance.ImageAlign = System.Drawing.ContentAlignment.TopRight;
         this.labelControl13.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl13.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl13.Location = new System.Drawing.Point(604, 15);
         this.labelControl13.Name = "labelControl13";
         this.labelControl13.Size = new System.Drawing.Size(98, 27);
         this.labelControl13.TabIndex = 6;
         this.labelControl13.Text = "انتخاب فایل";
         // 
         // SelectFile_Butn
         // 
         this.SelectFile_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.SelectFile_Butn.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.MdrpBs, "RPRT_PATH", true));
         this.SelectFile_Butn.EditValue = "";
         this.SelectFile_Butn.Location = new System.Drawing.Point(137, 12);
         this.SelectFile_Butn.Name = "SelectFile_Butn";
         this.SelectFile_Butn.Properties.Appearance.BackColor = System.Drawing.Color.White;
         this.SelectFile_Butn.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.SelectFile_Butn.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.SelectFile_Butn.Properties.Appearance.Options.UseBackColor = true;
         this.SelectFile_Butn.Properties.Appearance.Options.UseBorderColor = true;
         this.SelectFile_Butn.Properties.Appearance.Options.UseFont = true;
         this.SelectFile_Butn.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SelectFile_Butn.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.SelectFile_Butn.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.SelectFile_Butn.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.SelectFile_Butn.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.SelectFile_Butn.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("SelectFile_Butn.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
         this.SelectFile_Butn.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.SelectFile_Butn.Properties.NullText = "مسیر فایل دریافتی";
         this.SelectFile_Butn.Properties.NullValuePrompt = "مسیر فایل دریافتی";
         this.SelectFile_Butn.Properties.NullValuePromptShowForEmptyValue = true;
         this.SelectFile_Butn.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.SelectFile_Butn.Size = new System.Drawing.Size(461, 30);
         this.SelectFile_Butn.TabIndex = 4;
         this.SelectFile_Butn.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.SelectFile_Butn_ButtonClick);
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.Location = new System.Drawing.Point(329, 486);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(80, 27);
         this.Btn_Back.TabIndex = 9;
         this.Btn_Back.Text = "بازگشت";
         this.Btn_Back.UseVisualStyleBackColor = true;
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // Ofd_ReportFiles
         // 
         this.Ofd_ReportFiles.Filter = "Stimulsoft Report|*.mrt";
         // 
         // STNG_RPRT_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.Btn_Back);
         this.Controls.Add(this.Tb_master);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "STNG_RPRT_F";
         this.Size = new System.Drawing.Size(738, 527);
         ((System.ComponentModel.ISupportInitialize)(this.Actv_Lov)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DactvBs)).EndInit();
         this.Tb_master.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.countryGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.MdrpBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Ysno_Lov)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DysnoBs)).EndInit();
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.Del_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Save_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Add_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.SelectFile_Butn.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.TabControl Tb_master;
      private Windows.Forms.TabPage tabPage1;
      private C1.Win.C1Input.C1Button Btn_Back;
      private Windows.Forms.BindingSource MdrpBs;
      private DevExpress.XtraGrid.GridControl countryGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Ysno_Lov;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Actv_Lov;
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
      private DevExpress.XtraGrid.Columns.GridColumn colPRNT_AFTR_PAY;
      private DevExpress.XtraGrid.Columns.GridColumn colSTAT;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.ButtonEdit SelectFile_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl13;
      private Windows.Forms.OpenFileDialog Ofd_ReportFiles;
      private C1.Win.C1Input.C1Button Del_Butn;
      private C1.Win.C1Input.C1Button Save_Butn;
      private C1.Win.C1Input.C1Button Add_Butn;
      private Windows.Forms.BindingSource DactvBs;
      private Windows.Forms.BindingSource DysnoBs;
   }
}
