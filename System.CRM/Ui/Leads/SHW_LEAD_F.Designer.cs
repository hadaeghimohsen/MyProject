namespace System.CRM.Ui.Leads
{
   partial class SHW_LEAD_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SHW_LEAD_F));
         this.LeadBs = new System.Windows.Forms.BindingSource(this.components);
         this.DsstgBs = new System.Windows.Forms.BindingSource(this.components);
         this.Menu_Rbn = new C1.Win.C1Ribbon.C1Ribbon();
         this.ribbonApplicationMenu1 = new C1.Win.C1Ribbon.RibbonApplicationMenu();
         this.ribbonBottomToolBar1 = new C1.Win.C1Ribbon.RibbonBottomToolBar();
         this.ribbonConfigToolBar1 = new C1.Win.C1Ribbon.RibbonConfigToolBar();
         this.ribbonQat1 = new C1.Win.C1Ribbon.RibbonQat();
         this.ribbonTab1 = new C1.Win.C1Ribbon.RibbonTab();
         this.ribbonGroup11 = new C1.Win.C1Ribbon.RibbonGroup();
         this.New_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Edit_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Delete_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.BulkDelete_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Active_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Deactive_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.FindDuplicateSelected_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.FindDuplicateAll_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Follow_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.UnFollow_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Merge_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Report_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Import_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.Export_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.ribbonGroup1 = new C1.Win.C1Ribbon.RibbonGroup();
         this.Filter_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.ShowMap_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.GridFind_Tgbt = new C1.Win.C1Ribbon.RibbonToggleButton();
         this.ribbonGroup2 = new C1.Win.C1Ribbon.RibbonGroup();
         this.Back_Butn = new C1.Win.C1Ribbon.RibbonButton();
         this.ribbonTopToolBar1 = new C1.Win.C1Ribbon.RibbonTopToolBar();
         this.Acnt_Gc = new DevExpress.XtraGrid.GridControl();
         this.Comp_Gv = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colOWNR_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colTOPC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSTAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.colFRST_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLAST_NAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_TIME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemTimeEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
         ((System.ComponentModel.ISupportInitialize)(this.LeadBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DsstgBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Menu_Rbn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Acnt_Gc)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Comp_Gv)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).BeginInit();
         this.SuspendLayout();
         // 
         // LeadBs
         // 
         this.LeadBs.DataSource = typeof(System.CRM.Data.Lead);
         // 
         // DsstgBs
         // 
         this.DsstgBs.DataSource = typeof(System.CRM.Data.D_SSTG);
         // 
         // Menu_Rbn
         // 
         this.Menu_Rbn.ApplicationMenuHolder = this.ribbonApplicationMenu1;
         this.Menu_Rbn.BottomToolBarHolder = this.ribbonBottomToolBar1;
         this.Menu_Rbn.ConfigToolBarHolder = this.ribbonConfigToolBar1;
         this.Menu_Rbn.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Menu_Rbn.Location = new System.Drawing.Point(0, 0);
         this.Menu_Rbn.Name = "Menu_Rbn";
         this.Menu_Rbn.QatHolder = this.ribbonQat1;
         this.Menu_Rbn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Menu_Rbn.Size = new System.Drawing.Size(903, 118);
         this.Menu_Rbn.Tabs.Add(this.ribbonTab1);
         this.Menu_Rbn.ToolTipSettings.AutomaticDelay = 0;
         this.Menu_Rbn.ToolTipSettings.BackColor = System.Drawing.Color.Khaki;
         this.Menu_Rbn.ToolTipSettings.Font = new System.Drawing.Font("IRANSans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Menu_Rbn.ToolTipSettings.IsBalloon = true;
         this.Menu_Rbn.ToolTipSettings.RoundedCorners = true;
         this.Menu_Rbn.ToolTipSettings.Shadow = true;
         this.Menu_Rbn.ToolTipSettings.ShowAlways = true;
         this.Menu_Rbn.ToolTipSettings.UseFading = true;
         this.Menu_Rbn.TopToolBarHolder = this.ribbonTopToolBar1;
         this.Menu_Rbn.VisualStyle = C1.Win.C1Ribbon.VisualStyle.Custom;
         // 
         // ribbonApplicationMenu1
         // 
         this.ribbonApplicationMenu1.Name = "ribbonApplicationMenu1";
         this.ribbonApplicationMenu1.Visible = false;
         // 
         // ribbonBottomToolBar1
         // 
         this.ribbonBottomToolBar1.Name = "ribbonBottomToolBar1";
         this.ribbonBottomToolBar1.Visible = false;
         // 
         // ribbonConfigToolBar1
         // 
         this.ribbonConfigToolBar1.Name = "ribbonConfigToolBar1";
         this.ribbonConfigToolBar1.Visible = false;
         // 
         // ribbonQat1
         // 
         this.ribbonQat1.Name = "ribbonQat1";
         this.ribbonQat1.Visible = false;
         // 
         // ribbonTab1
         // 
         this.ribbonTab1.Groups.Add(this.ribbonGroup11);
         this.ribbonTab1.Groups.Add(this.ribbonGroup1);
         this.ribbonTab1.Groups.Add(this.ribbonGroup2);
         this.ribbonTab1.Name = "ribbonTab1";
         this.ribbonTab1.Text = "سرنخ های تجاری";
         // 
         // ribbonGroup11
         // 
         this.ribbonGroup11.Items.Add(this.New_Butn);
         this.ribbonGroup11.Items.Add(this.Edit_Butn);
         this.ribbonGroup11.Items.Add(this.Delete_Butn);
         this.ribbonGroup11.Items.Add(this.BulkDelete_Butn);
         this.ribbonGroup11.Items.Add(this.Active_Butn);
         this.ribbonGroup11.Items.Add(this.Deactive_Butn);
         this.ribbonGroup11.Items.Add(this.FindDuplicateSelected_Butn);
         this.ribbonGroup11.Items.Add(this.FindDuplicateAll_Butn);
         this.ribbonGroup11.Items.Add(this.Follow_Butn);
         this.ribbonGroup11.Items.Add(this.UnFollow_Butn);
         this.ribbonGroup11.Items.Add(this.Merge_Butn);
         this.ribbonGroup11.Items.Add(this.Report_Butn);
         this.ribbonGroup11.Items.Add(this.Import_Butn);
         this.ribbonGroup11.Items.Add(this.Export_Butn);
         this.ribbonGroup11.Name = "ribbonGroup11";
         this.ribbonGroup11.Text = "منوها";
         // 
         // New_Butn
         // 
         this.New_Butn.Name = "New_Butn";
         this.New_Butn.SmallImage = global::System.CRM.Properties.Resources.IMAGE_1198;
         this.New_Butn.ToolTip = "جدید";
         // 
         // Edit_Butn
         // 
         this.Edit_Butn.Name = "Edit_Butn";
         this.Edit_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Edit_Butn.SmallImage")));
         this.Edit_Butn.ToolTip = "ویرایش";
         // 
         // Delete_Butn
         // 
         this.Delete_Butn.Name = "Delete_Butn";
         this.Delete_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Delete_Butn.SmallImage")));
         this.Delete_Butn.ToolTip = "حذف";
         // 
         // BulkDelete_Butn
         // 
         this.BulkDelete_Butn.Name = "BulkDelete_Butn";
         this.BulkDelete_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("BulkDelete_Butn.SmallImage")));
         this.BulkDelete_Butn.ToolTip = "حذف فله";
         // 
         // Active_Butn
         // 
         this.Active_Butn.Name = "Active_Butn";
         this.Active_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Active_Butn.SmallImage")));
         this.Active_Butn.ToolTip = "فعال";
         // 
         // Deactive_Butn
         // 
         this.Deactive_Butn.Name = "Deactive_Butn";
         this.Deactive_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Deactive_Butn.SmallImage")));
         this.Deactive_Butn.ToolTip = "غیرفعال";
         // 
         // FindDuplicateSelected_Butn
         // 
         this.FindDuplicateSelected_Butn.Name = "FindDuplicateSelected_Butn";
         this.FindDuplicateSelected_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("FindDuplicateSelected_Butn.SmallImage")));
         this.FindDuplicateSelected_Butn.ToolTip = "پیدا کردن رکورد تکراری برای رکوردهای انتخاب شده";
         // 
         // FindDuplicateAll_Butn
         // 
         this.FindDuplicateAll_Butn.Name = "FindDuplicateAll_Butn";
         this.FindDuplicateAll_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("FindDuplicateAll_Butn.SmallImage")));
         this.FindDuplicateAll_Butn.ToolTip = "پیدا کردن رکورد تکراری برای همه رکوردها";
         // 
         // Follow_Butn
         // 
         this.Follow_Butn.Name = "Follow_Butn";
         this.Follow_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Follow_Butn.SmallImage")));
         this.Follow_Butn.ToolTip = "دنبال کردن";
         // 
         // UnFollow_Butn
         // 
         this.UnFollow_Butn.Name = "UnFollow_Butn";
         this.UnFollow_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("UnFollow_Butn.SmallImage")));
         this.UnFollow_Butn.ToolTip = "دنبال نکردن";
         // 
         // Merge_Butn
         // 
         this.Merge_Butn.Name = "Merge_Butn";
         this.Merge_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Merge_Butn.SmallImage")));
         this.Merge_Butn.ToolTip = "ادغام";
         // 
         // Report_Butn
         // 
         this.Report_Butn.Name = "Report_Butn";
         this.Report_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Report_Butn.SmallImage")));
         this.Report_Butn.ToolTip = "اجرای گزارش";
         // 
         // Import_Butn
         // 
         this.Import_Butn.Name = "Import_Butn";
         this.Import_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Import_Butn.SmallImage")));
         this.Import_Butn.ToolTip = "ورود اطلاعات از اکسل";
         // 
         // Export_Butn
         // 
         this.Export_Butn.Name = "Export_Butn";
         this.Export_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Export_Butn.SmallImage")));
         this.Export_Butn.ToolTip = "خروجی گرفتن به اکسل";
         // 
         // ribbonGroup1
         // 
         this.ribbonGroup1.Items.Add(this.Filter_Butn);
         this.ribbonGroup1.Items.Add(this.ShowMap_Butn);
         this.ribbonGroup1.Items.Add(this.GridFind_Tgbt);
         this.ribbonGroup1.Name = "ribbonGroup1";
         this.ribbonGroup1.Text = "جستجو";
         // 
         // Filter_Butn
         // 
         this.Filter_Butn.Name = "Filter_Butn";
         this.Filter_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Filter_Butn.SmallImage")));
         this.Filter_Butn.ToolTip = "معیار های جستجو گزینشی";
         // 
         // ShowMap_Butn
         // 
         this.ShowMap_Butn.Name = "ShowMap_Butn";
         this.ShowMap_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("ShowMap_Butn.SmallImage")));
         this.ShowMap_Butn.ToolTip = "نمایش رکورد ها روی نقشه";
         // 
         // GridFind_Tgbt
         // 
         this.GridFind_Tgbt.Name = "GridFind_Tgbt";
         this.GridFind_Tgbt.SmallImage = ((System.Drawing.Image)(resources.GetObject("GridFind_Tgbt.SmallImage")));
         this.GridFind_Tgbt.ToolTip = "نمایش پنل جستجو ردیفی";
         // 
         // ribbonGroup2
         // 
         this.ribbonGroup2.Items.Add(this.Back_Butn);
         this.ribbonGroup2.Name = "ribbonGroup2";
         this.ribbonGroup2.Text = "عملیات فرم";
         // 
         // Back_Butn
         // 
         this.Back_Butn.LargeImage = global::System.CRM.Properties.Resources.IMAGE_1015;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.SmallImage = ((System.Drawing.Image)(resources.GetObject("Back_Butn.SmallImage")));
         this.Back_Butn.TextImageRelation = C1.Win.C1Ribbon.TextImageRelation.ImageBeforeText;
         this.Back_Butn.ToolTip = "بستن فرم";
         // 
         // ribbonTopToolBar1
         // 
         this.ribbonTopToolBar1.Name = "ribbonTopToolBar1";
         this.ribbonTopToolBar1.Visible = false;
         // 
         // Acnt_Gc
         // 
         this.Acnt_Gc.DataSource = this.LeadBs;
         this.Acnt_Gc.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Acnt_Gc.Location = new System.Drawing.Point(0, 118);
         this.Acnt_Gc.LookAndFeel.SkinName = "Office 2013";
         this.Acnt_Gc.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Acnt_Gc.MainView = this.Comp_Gv;
         this.Acnt_Gc.Name = "Acnt_Gc";
         this.Acnt_Gc.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.persianRepositoryItemDateEdit1,
            this.repositoryItemLookUpEdit1,
            this.repositoryItemTimeEdit1});
         this.Acnt_Gc.Size = new System.Drawing.Size(903, 336);
         this.Acnt_Gc.TabIndex = 2;
         this.Acnt_Gc.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.Comp_Gv});
         // 
         // Comp_Gv
         // 
         this.Comp_Gv.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.Comp_Gv.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Comp_Gv.Appearance.FocusedRow.Options.UseBackColor = true;
         this.Comp_Gv.Appearance.FocusedRow.Options.UseFont = true;
         this.Comp_Gv.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F);
         this.Comp_Gv.Appearance.HeaderPanel.Options.UseFont = true;
         this.Comp_Gv.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.Comp_Gv.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Comp_Gv.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F);
         this.Comp_Gv.Appearance.Row.Options.UseFont = true;
         this.Comp_Gv.Appearance.Row.Options.UseTextOptions = true;
         this.Comp_Gv.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Comp_Gv.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.Comp_Gv.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colOWNR_CODE,
            this.colTOPC,
            this.colSTAT,
            this.colCRET_DATE,
            this.colFRST_NAME,
            this.colLAST_NAME,
            this.colCRET_TIME});
         this.Comp_Gv.GridControl = this.Acnt_Gc;
         this.Comp_Gv.Name = "Comp_Gv";
         this.Comp_Gv.OptionsBehavior.Editable = false;
         this.Comp_Gv.OptionsBehavior.ReadOnly = true;
         this.Comp_Gv.OptionsFind.AlwaysVisible = true;
         this.Comp_Gv.OptionsFind.FindDelay = 100;
         this.Comp_Gv.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.Comp_Gv.OptionsSelection.MultiSelect = true;
         this.Comp_Gv.OptionsView.ShowDetailButtons = false;
         this.Comp_Gv.OptionsView.ShowGroupPanel = false;
         this.Comp_Gv.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.False;
         this.Comp_Gv.OptionsView.ShowIndicator = false;
         this.Comp_Gv.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
         // 
         // colOWNR_CODE
         // 
         this.colOWNR_CODE.Caption = "مالک";
         this.colOWNR_CODE.FieldName = "Job_Personnel.USER_DESC_DNRM";
         this.colOWNR_CODE.Name = "colOWNR_CODE";
         this.colOWNR_CODE.OptionsColumn.FixedWidth = true;
         this.colOWNR_CODE.Visible = true;
         this.colOWNR_CODE.VisibleIndex = 0;
         this.colOWNR_CODE.Width = 129;
         // 
         // colTOPC
         // 
         this.colTOPC.Caption = "عنوان";
         this.colTOPC.FieldName = "TOPC";
         this.colTOPC.Name = "colTOPC";
         this.colTOPC.Visible = true;
         this.colTOPC.VisibleIndex = 4;
         this.colTOPC.Width = 323;
         // 
         // colSTAT
         // 
         this.colSTAT.Caption = "وضعیت";
         this.colSTAT.ColumnEdit = this.repositoryItemLookUpEdit1;
         this.colSTAT.FieldName = "STAT";
         this.colSTAT.Name = "colSTAT";
         this.colSTAT.OptionsColumn.FixedWidth = true;
         this.colSTAT.Visible = true;
         this.colSTAT.VisibleIndex = 3;
         this.colSTAT.Width = 92;
         // 
         // colCRET_DATE
         // 
         this.colCRET_DATE.Caption = "تاریخ ایجاد";
         this.colCRET_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colCRET_DATE.FieldName = "CRET_DATE";
         this.colCRET_DATE.Name = "colCRET_DATE";
         this.colCRET_DATE.OptionsColumn.FixedWidth = true;
         this.colCRET_DATE.Visible = true;
         this.colCRET_DATE.VisibleIndex = 2;
         this.colCRET_DATE.Width = 76;
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
         // repositoryItemLookUpEdit1
         // 
         this.repositoryItemLookUpEdit1.AutoHeight = false;
         this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemLookUpEdit1.DataSource = this.DsstgBs;
         this.repositoryItemLookUpEdit1.DisplayMember = "DOMN_DESC";
         this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
         this.repositoryItemLookUpEdit1.NullText = "---";
         this.repositoryItemLookUpEdit1.ValueMember = "VALU";
         // 
         // colFRST_NAME
         // 
         this.colFRST_NAME.Caption = "نام";
         this.colFRST_NAME.FieldName = "Request_Row.Service_Publics.FRST_NAME";
         this.colFRST_NAME.Name = "colFRST_NAME";
         this.colFRST_NAME.OptionsColumn.FixedWidth = true;
         this.colFRST_NAME.Visible = true;
         this.colFRST_NAME.VisibleIndex = 6;
         this.colFRST_NAME.Width = 93;
         // 
         // colLAST_NAME
         // 
         this.colLAST_NAME.Caption = "نام خانوادگی";
         this.colLAST_NAME.FieldName = "Request_Row.Service_Publics.LAST_NAME";
         this.colLAST_NAME.Name = "colLAST_NAME";
         this.colLAST_NAME.OptionsColumn.FixedWidth = true;
         this.colLAST_NAME.Visible = true;
         this.colLAST_NAME.VisibleIndex = 5;
         this.colLAST_NAME.Width = 139;
         // 
         // colCRET_TIME
         // 
         this.colCRET_TIME.Caption = "ساعت";
         this.colCRET_TIME.ColumnEdit = this.repositoryItemTimeEdit1;
         this.colCRET_TIME.FieldName = "CRET_DATE";
         this.colCRET_TIME.Name = "colCRET_TIME";
         this.colCRET_TIME.OptionsColumn.FixedWidth = true;
         this.colCRET_TIME.Visible = true;
         this.colCRET_TIME.VisibleIndex = 1;
         this.colCRET_TIME.Width = 51;
         // 
         // repositoryItemTimeEdit1
         // 
         this.repositoryItemTimeEdit1.AutoHeight = false;
         this.repositoryItemTimeEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemTimeEdit1.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
         this.repositoryItemTimeEdit1.DisplayFormat.FormatString = "HH:mm";
         this.repositoryItemTimeEdit1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.repositoryItemTimeEdit1.Name = "repositoryItemTimeEdit1";
         this.repositoryItemTimeEdit1.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
         // 
         // SHW_LEAD_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.Acnt_Gc);
         this.Controls.Add(this.Menu_Rbn);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "SHW_LEAD_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(903, 454);
         ((System.ComponentModel.ISupportInitialize)(this.LeadBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DsstgBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Menu_Rbn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Acnt_Gc)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Comp_Gv)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTimeEdit1)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraGrid.Columns.GridColumn Actn_Clmn;
      private Windows.Forms.BindingSource LeadBs;
      private Windows.Forms.BindingSource DsstgBs;
      private C1.Win.C1Ribbon.C1Ribbon Menu_Rbn;
      private C1.Win.C1Ribbon.RibbonApplicationMenu ribbonApplicationMenu1;
      private C1.Win.C1Ribbon.RibbonBottomToolBar ribbonBottomToolBar1;
      private C1.Win.C1Ribbon.RibbonConfigToolBar ribbonConfigToolBar1;
      private C1.Win.C1Ribbon.RibbonQat ribbonQat1;
      private C1.Win.C1Ribbon.RibbonTab ribbonTab1;
      private C1.Win.C1Ribbon.RibbonGroup ribbonGroup11;
      private C1.Win.C1Ribbon.RibbonButton New_Butn;
      private C1.Win.C1Ribbon.RibbonButton Edit_Butn;
      private C1.Win.C1Ribbon.RibbonButton Delete_Butn;
      private C1.Win.C1Ribbon.RibbonButton BulkDelete_Butn;
      private C1.Win.C1Ribbon.RibbonButton Active_Butn;
      private C1.Win.C1Ribbon.RibbonButton Deactive_Butn;
      private C1.Win.C1Ribbon.RibbonButton FindDuplicateSelected_Butn;
      private C1.Win.C1Ribbon.RibbonButton FindDuplicateAll_Butn;
      private C1.Win.C1Ribbon.RibbonButton Follow_Butn;
      private C1.Win.C1Ribbon.RibbonButton UnFollow_Butn;
      private C1.Win.C1Ribbon.RibbonButton Merge_Butn;
      private C1.Win.C1Ribbon.RibbonButton Report_Butn;
      private C1.Win.C1Ribbon.RibbonButton Import_Butn;
      private C1.Win.C1Ribbon.RibbonButton Export_Butn;
      private C1.Win.C1Ribbon.RibbonGroup ribbonGroup1;
      private C1.Win.C1Ribbon.RibbonButton Filter_Butn;
      private C1.Win.C1Ribbon.RibbonButton ShowMap_Butn;
      private C1.Win.C1Ribbon.RibbonToggleButton GridFind_Tgbt;
      private C1.Win.C1Ribbon.RibbonGroup ribbonGroup2;
      private C1.Win.C1Ribbon.RibbonButton Back_Butn;
      private C1.Win.C1Ribbon.RibbonTopToolBar ribbonTopToolBar1;
      private DevExpress.XtraGrid.GridControl Acnt_Gc;
      private DevExpress.XtraGrid.Views.Grid.GridView Comp_Gv;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private DevExpress.XtraGrid.Columns.GridColumn colOWNR_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colTOPC;
      private DevExpress.XtraGrid.Columns.GridColumn colSTAT;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colFRST_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn colLAST_NAME;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_TIME;
      private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit repositoryItemTimeEdit1;
   }
}
