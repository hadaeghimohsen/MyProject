namespace System.Scsc.Ui.Regulation
{
   partial class MSTR_DCMT_F
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
         System.Windows.Forms.Label dCMT_DESCLabel;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MSTR_DCMT_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.backstageViewControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewControl();
         this.backstageViewClientControl1 = new DevExpress.XtraBars.Ribbon.BackstageViewClientControl();
         this.Btn_Back = new System.MaxUi.NewMaxBtn();
         this.Pnl_Item = new System.Windows.Forms.Panel();
         this.Btn_Cancel = new System.Windows.Forms.Button();
         this.Btn_SubmitChange = new System.Windows.Forms.Button();
         this.Txt_DcmtDesc = new DevExpress.XtraEditors.TextEdit();
         this.Btn_AddNewItem = new System.Windows.Forms.Button();
         this.document_SpecGridControl = new DevExpress.XtraGrid.GridControl();
         this.document_SpecBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colDSID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colDCMT_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colUpdateDelete = new DevExpress.XtraGrid.Columns.GridColumn();
         this.LV_UPDEL = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
         this.document_SpecBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
         this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
         this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
         this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
         this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
         this.document_SpecBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.backstageViewButtonItem1 = new DevExpress.XtraBars.Ribbon.BackstageViewButtonItem();
         this.backstageViewTabItem1 = new DevExpress.XtraBars.Ribbon.BackstageViewTabItem();
         dCMT_DESCLabel = new System.Windows.Forms.Label();
         this.backstageViewControl1.SuspendLayout();
         this.backstageViewClientControl1.SuspendLayout();
         this.Pnl_Item.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_DcmtDesc.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.document_SpecGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.document_SpecBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.LV_UPDEL)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.document_SpecBindingNavigator)).BeginInit();
         this.document_SpecBindingNavigator.SuspendLayout();
         this.SuspendLayout();
         // 
         // dCMT_DESCLabel
         // 
         dCMT_DESCLabel.AutoSize = true;
         dCMT_DESCLabel.Location = new System.Drawing.Point(312, 18);
         dCMT_DESCLabel.Name = "dCMT_DESCLabel";
         dCMT_DESCLabel.Size = new System.Drawing.Size(101, 13);
         dCMT_DESCLabel.TabIndex = 0;
         dCMT_DESCLabel.Text = "عنوان مدرک / مجوز :";
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
         this.backstageViewControl1.Size = new System.Drawing.Size(869, 646);
         this.backstageViewControl1.TabIndex = 0;
         this.backstageViewControl1.Text = "backstageViewControl1";
         // 
         // backstageViewClientControl1
         // 
         this.backstageViewClientControl1.AutoScroll = true;
         this.backstageViewClientControl1.AutoScrollMinSize = new System.Drawing.Size(0, 646);
         this.backstageViewClientControl1.Controls.Add(this.Btn_Back);
         this.backstageViewClientControl1.Controls.Add(this.Pnl_Item);
         this.backstageViewClientControl1.Controls.Add(this.Btn_AddNewItem);
         this.backstageViewClientControl1.Controls.Add(this.document_SpecGridControl);
         this.backstageViewClientControl1.Controls.Add(this.document_SpecBindingNavigator);
         this.backstageViewClientControl1.Controls.Add(this.labelControl1);
         this.backstageViewClientControl1.Location = new System.Drawing.Point(188, 0);
         this.backstageViewClientControl1.Name = "backstageViewClientControl1";
         this.backstageViewClientControl1.Size = new System.Drawing.Size(681, 646);
         this.backstageViewClientControl1.TabIndex = 0;
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.Btn_Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.Btn_Back.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_Back.Caption = "بازگشت";
         this.Btn_Back.Disabled = false;
         this.Btn_Back.EnterColor = System.Drawing.Color.Transparent;
         this.Btn_Back.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Btn_Back.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Back.ImageIndex = -1;
         this.Btn_Back.ImageList = null;
         this.Btn_Back.InToBold = false;
         this.Btn_Back.Location = new System.Drawing.Point(29, 428);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(93, 27);
         this.Btn_Back.TabIndex = 40;
         this.Btn_Back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Back.TextColor = System.Drawing.Color.Black;
         this.Btn_Back.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // Pnl_Item
         // 
         this.Pnl_Item.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.Pnl_Item.BackColor = System.Drawing.Color.Transparent;
         this.Pnl_Item.Controls.Add(dCMT_DESCLabel);
         this.Pnl_Item.Controls.Add(this.Btn_Cancel);
         this.Pnl_Item.Controls.Add(this.Btn_SubmitChange);
         this.Pnl_Item.Controls.Add(this.Txt_DcmtDesc);
         this.Pnl_Item.Location = new System.Drawing.Point(219, 457);
         this.Pnl_Item.Name = "Pnl_Item";
         this.Pnl_Item.Size = new System.Drawing.Size(436, 79);
         this.Pnl_Item.TabIndex = 39;
         this.Pnl_Item.Visible = false;
         // 
         // Btn_Cancel
         // 
         this.Btn_Cancel.Location = new System.Drawing.Point(120, 53);
         this.Btn_Cancel.Name = "Btn_Cancel";
         this.Btn_Cancel.Size = new System.Drawing.Size(93, 23);
         this.Btn_Cancel.TabIndex = 38;
         this.Btn_Cancel.Text = "انصراف";
         this.Btn_Cancel.UseVisualStyleBackColor = true;
         this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
         // 
         // Btn_SubmitChange
         // 
         this.Btn_SubmitChange.Location = new System.Drawing.Point(3, 53);
         this.Btn_SubmitChange.Name = "Btn_SubmitChange";
         this.Btn_SubmitChange.Size = new System.Drawing.Size(93, 23);
         this.Btn_SubmitChange.TabIndex = 38;
         this.Btn_SubmitChange.Text = "ثبت اطلاعات";
         this.Btn_SubmitChange.UseVisualStyleBackColor = true;
         this.Btn_SubmitChange.Click += new System.EventHandler(this.Btn_SubmitChange_Click);
         // 
         // Txt_DcmtDesc
         // 
         this.Txt_DcmtDesc.Location = new System.Drawing.Point(120, 15);
         this.Txt_DcmtDesc.Name = "Txt_DcmtDesc";
         this.Txt_DcmtDesc.Size = new System.Drawing.Size(186, 20);
         this.Txt_DcmtDesc.TabIndex = 1;
         // 
         // Btn_AddNewItem
         // 
         this.Btn_AddNewItem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.Btn_AddNewItem.Location = new System.Drawing.Point(494, 428);
         this.Btn_AddNewItem.Name = "Btn_AddNewItem";
         this.Btn_AddNewItem.Size = new System.Drawing.Size(161, 23);
         this.Btn_AddNewItem.TabIndex = 38;
         this.Btn_AddNewItem.Text = "اضافه کردن مدرک و مجوز جدید";
         this.Btn_AddNewItem.UseVisualStyleBackColor = true;
         this.Btn_AddNewItem.Click += new System.EventHandler(this.Btn_AddNewItem_Click);
         // 
         // document_SpecGridControl
         // 
         this.document_SpecGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.document_SpecGridControl.DataSource = this.document_SpecBindingSource;
         this.document_SpecGridControl.Location = new System.Drawing.Point(29, 63);
         this.document_SpecGridControl.LookAndFeel.SkinName = "Office 2010 Black";
         this.document_SpecGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.document_SpecGridControl.MainView = this.gridView1;
         this.document_SpecGridControl.Name = "document_SpecGridControl";
         this.document_SpecGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.LV_UPDEL});
         this.document_SpecGridControl.Size = new System.Drawing.Size(626, 359);
         this.document_SpecGridControl.TabIndex = 37;
         this.document_SpecGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // document_SpecBindingSource
         // 
         this.document_SpecBindingSource.DataSource = typeof(System.Scsc.Data.Document_Spec);
         // 
         // gridView1
         // 
         this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
         this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView1.Appearance.Row.Options.UseFont = true;
         this.gridView1.Appearance.Row.Options.UseTextOptions = true;
         this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDSID,
            this.colDCMT_DESC,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colUpdateDelete});
         this.gridView1.GridControl = this.document_SpecGridControl;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsDetail.EnableMasterViewMode = false;
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colDSID
         // 
         this.colDSID.FieldName = "DSID";
         this.colDSID.Name = "colDSID";
         // 
         // colDCMT_DESC
         // 
         this.colDCMT_DESC.Caption = "عنوان مدرک / مجوز";
         this.colDCMT_DESC.FieldName = "DCMT_DESC";
         this.colDCMT_DESC.Name = "colDCMT_DESC";
         this.colDCMT_DESC.Visible = true;
         this.colDCMT_DESC.VisibleIndex = 5;
         this.colDCMT_DESC.Width = 161;
         // 
         // colCRET_BY
         // 
         this.colCRET_BY.Caption = "کاربر ایجاد کننده";
         this.colCRET_BY.FieldName = "CRET_BY";
         this.colCRET_BY.Name = "colCRET_BY";
         this.colCRET_BY.Visible = true;
         this.colCRET_BY.VisibleIndex = 3;
         this.colCRET_BY.Width = 108;
         // 
         // colCRET_DATE
         // 
         this.colCRET_DATE.Caption = "تاریخ ایجاد";
         this.colCRET_DATE.FieldName = "CRET_DATE";
         this.colCRET_DATE.Name = "colCRET_DATE";
         this.colCRET_DATE.Visible = true;
         this.colCRET_DATE.VisibleIndex = 2;
         this.colCRET_DATE.Width = 108;
         // 
         // colMDFY_BY
         // 
         this.colMDFY_BY.Caption = "کاربر ویرایش کننده";
         this.colMDFY_BY.FieldName = "MDFY_BY";
         this.colMDFY_BY.Name = "colMDFY_BY";
         this.colMDFY_BY.Visible = true;
         this.colMDFY_BY.VisibleIndex = 1;
         this.colMDFY_BY.Width = 108;
         // 
         // colMDFY_DATE
         // 
         this.colMDFY_DATE.Caption = "تاریخ ویرایش";
         this.colMDFY_DATE.FieldName = "MDFY_DATE";
         this.colMDFY_DATE.Name = "colMDFY_DATE";
         this.colMDFY_DATE.Visible = true;
         this.colMDFY_DATE.VisibleIndex = 0;
         this.colMDFY_DATE.Width = 108;
         // 
         // colUpdateDelete
         // 
         this.colUpdateDelete.ColumnEdit = this.LV_UPDEL;
         this.colUpdateDelete.Name = "colUpdateDelete";
         this.colUpdateDelete.OptionsColumn.AllowSize = false;
         this.colUpdateDelete.OptionsColumn.FixedWidth = true;
         this.colUpdateDelete.OptionsColumn.ReadOnly = true;
         this.colUpdateDelete.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
         this.colUpdateDelete.Visible = true;
         this.colUpdateDelete.VisibleIndex = 4;
         this.colUpdateDelete.Width = 26;
         // 
         // LV_UPDEL
         // 
         this.LV_UPDEL.AutoHeight = false;
         this.LV_UPDEL.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("LV_UPDEL.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", "Delete", null, true)});
         this.LV_UPDEL.Name = "LV_UPDEL";
         this.LV_UPDEL.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.LV_UPDEL_ButtonClick);
         // 
         // document_SpecBindingNavigator
         // 
         this.document_SpecBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
         this.document_SpecBindingNavigator.BackColor = System.Drawing.Color.Transparent;
         this.document_SpecBindingNavigator.BindingSource = this.document_SpecBindingSource;
         this.document_SpecBindingNavigator.CountItem = this.bindingNavigatorCountItem;
         this.document_SpecBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
         this.document_SpecBindingNavigator.Dock = System.Windows.Forms.DockStyle.None;
         this.document_SpecBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.document_SpecBindingNavigatorSaveItem});
         this.document_SpecBindingNavigator.Location = new System.Drawing.Point(29, 35);
         this.document_SpecBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
         this.document_SpecBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
         this.document_SpecBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
         this.document_SpecBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
         this.document_SpecBindingNavigator.Name = "document_SpecBindingNavigator";
         this.document_SpecBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
         this.document_SpecBindingNavigator.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
         this.document_SpecBindingNavigator.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.document_SpecBindingNavigator.Size = new System.Drawing.Size(232, 25);
         this.document_SpecBindingNavigator.TabIndex = 1;
         this.document_SpecBindingNavigator.Text = "bindingNavigator1";
         // 
         // bindingNavigatorAddNewItem
         // 
         this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorAddNewItem.Enabled = false;
         this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
         this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
         this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorAddNewItem.Text = "Add new";
         this.bindingNavigatorAddNewItem.Visible = false;
         // 
         // bindingNavigatorCountItem
         // 
         this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
         this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
         this.bindingNavigatorCountItem.Text = "of {0}";
         this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
         // 
         // bindingNavigatorDeleteItem
         // 
         this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorDeleteItem.Enabled = false;
         this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
         this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
         this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorDeleteItem.Text = "Delete";
         this.bindingNavigatorDeleteItem.Visible = false;
         // 
         // bindingNavigatorMoveFirstItem
         // 
         this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
         this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
         this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorMoveFirstItem.Text = "Move first";
         // 
         // bindingNavigatorMovePreviousItem
         // 
         this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
         this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
         this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorMovePreviousItem.Text = "Move previous";
         // 
         // bindingNavigatorSeparator
         // 
         this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
         this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
         // 
         // bindingNavigatorPositionItem
         // 
         this.bindingNavigatorPositionItem.AccessibleName = "Position";
         this.bindingNavigatorPositionItem.AutoSize = false;
         this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
         this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
         this.bindingNavigatorPositionItem.Text = "0";
         this.bindingNavigatorPositionItem.ToolTipText = "Current position";
         // 
         // bindingNavigatorSeparator1
         // 
         this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
         this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
         // 
         // bindingNavigatorMoveNextItem
         // 
         this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
         this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
         this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorMoveNextItem.Text = "Move next";
         // 
         // bindingNavigatorMoveLastItem
         // 
         this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
         this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
         this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorMoveLastItem.Text = "Move last";
         // 
         // bindingNavigatorSeparator2
         // 
         this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
         this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
         // 
         // document_SpecBindingNavigatorSaveItem
         // 
         this.document_SpecBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.document_SpecBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("document_SpecBindingNavigatorSaveItem.Image")));
         this.document_SpecBindingNavigatorSaveItem.Name = "document_SpecBindingNavigatorSaveItem";
         this.document_SpecBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
         this.document_SpecBindingNavigatorSaveItem.Text = "Save Data";
         this.document_SpecBindingNavigatorSaveItem.Click += new System.EventHandler(this.document_SpecBindingNavigatorSaveItem_Click);
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(29, 3);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(626, 29);
         this.labelControl1.TabIndex = 37;
         this.labelControl1.Text = "مدارک و مجوزات";
         // 
         // backstageViewButtonItem1
         // 
         this.backstageViewButtonItem1.Caption = "مدارک و مجوزات";
         this.backstageViewButtonItem1.Glyph = ((System.Drawing.Image)(resources.GetObject("backstageViewButtonItem1.Glyph")));
         this.backstageViewButtonItem1.Name = "backstageViewButtonItem1";
         // 
         // backstageViewTabItem1
         // 
         this.backstageViewTabItem1.Caption = "اطلاعات مدارک و مجوزات";
         this.backstageViewTabItem1.ContentControl = this.backstageViewClientControl1;
         this.backstageViewTabItem1.Name = "backstageViewTabItem1";
         this.backstageViewTabItem1.Selected = true;
         // 
         // MSTR_DCMT_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.backstageViewControl1);
         this.Name = "MSTR_DCMT_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(869, 646);
         this.backstageViewControl1.ResumeLayout(false);
         this.backstageViewClientControl1.ResumeLayout(false);
         this.backstageViewClientControl1.PerformLayout();
         this.Pnl_Item.ResumeLayout(false);
         this.Pnl_Item.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_DcmtDesc.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.document_SpecGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.document_SpecBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.LV_UPDEL)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.document_SpecBindingNavigator)).EndInit();
         this.document_SpecBindingNavigator.ResumeLayout(false);
         this.document_SpecBindingNavigator.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Ribbon.BackstageViewControl backstageViewControl1;
      private DevExpress.XtraBars.Ribbon.BackstageViewButtonItem backstageViewButtonItem1;
      private DevExpress.XtraBars.Ribbon.BackstageViewClientControl backstageViewClientControl1;
      private DevExpress.XtraBars.Ribbon.BackstageViewTabItem backstageViewTabItem1;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private Windows.Forms.BindingNavigator document_SpecBindingNavigator;
      private Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
      private Windows.Forms.BindingSource document_SpecBindingSource;
      private Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
      private Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
      private Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
      private Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
      private Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
      private Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
      private Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
      private Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
      private Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
      private Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
      private Windows.Forms.ToolStripButton document_SpecBindingNavigatorSaveItem;
      private DevExpress.XtraGrid.GridControl document_SpecGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colDSID;
      private DevExpress.XtraGrid.Columns.GridColumn colDCMT_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colUpdateDelete;
      private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit LV_UPDEL;
      private Windows.Forms.Panel Pnl_Item;
      private Windows.Forms.Button Btn_Cancel;
      private Windows.Forms.Button Btn_SubmitChange;
      private DevExpress.XtraEditors.TextEdit Txt_DcmtDesc;
      private Windows.Forms.Button Btn_AddNewItem;
      private MaxUi.NewMaxBtn Btn_Back;
   }
}
