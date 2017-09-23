namespace System.CRM.Ui.BaseDefination
{
   partial class DCSP_DFIN_F
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be dCRMosed; otherwise, false.</param>
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DCSP_DFIN_F));
         this.tb_master = new C1.Win.C1Command.C1DockingTab();
         this.tp_001 = new C1.Win.C1Command.C1DockingTabPage();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.countryGridControl = new DevExpress.XtraGrid.GridControl();
         this.DcspBs = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colDSID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colDCMT_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.DcspBn = new System.Windows.Forms.BindingNavigator(this.components);
         this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
         this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
         this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
         this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
         this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
         this.Tsb_DelEpit = new System.Windows.Forms.ToolStripButton();
         this.countryBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
         this.Btn_Back = new C1.Win.C1Input.C1Button();
         ((System.ComponentModel.ISupportInitialize)(this.tb_master)).BeginInit();
         this.tb_master.SuspendLayout();
         this.tp_001.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.countryGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DcspBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DcspBn)).BeginInit();
         this.DcspBn.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).BeginInit();
         this.SuspendLayout();
         // 
         // tb_master
         // 
         this.tb_master.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tb_master.Controls.Add(this.tp_001);
         this.tb_master.Location = new System.Drawing.Point(0, 0);
         this.tb_master.Name = "tb_master";
         this.tb_master.RightToLeftLayout = true;
         this.tb_master.Size = new System.Drawing.Size(686, 456);
         this.tb_master.TabIndex = 0;
         this.tb_master.TabsSpacing = 0;
         this.tb_master.TabStyle = C1.Win.C1Command.TabStyleEnum.WindowsXP;
         this.tb_master.VisualStyle = C1.Win.C1Command.VisualStyle.WindowsXP;
         this.tb_master.VisualStyleBase = C1.Win.C1Command.VisualStyle.WindowsXP;
         // 
         // tp_001
         // 
         this.tp_001.Controls.Add(this.groupBox1);
         this.tp_001.Location = new System.Drawing.Point(2, 27);
         this.tp_001.Name = "tp_001";
         this.tp_001.Size = new System.Drawing.Size(680, 425);
         this.tp_001.TabIndex = 0;
         this.tp_001.Text = "اطلاعات پایه";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.countryGridControl);
         this.groupBox1.Controls.Add(this.DcspBn);
         this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.groupBox1.Location = new System.Drawing.Point(0, 0);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(680, 425);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "اسناد و مدارک";
         // 
         // countryGridControl
         // 
         this.countryGridControl.DataSource = this.DcspBs;
         this.countryGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.countryGridControl.Location = new System.Drawing.Point(3, 43);
         this.countryGridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.countryGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.countryGridControl.MainView = this.gridView1;
         this.countryGridControl.Name = "countryGridControl";
         this.countryGridControl.Size = new System.Drawing.Size(674, 379);
         this.countryGridControl.TabIndex = 1;
         this.countryGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // DcspBs
         // 
         this.DcspBs.DataSource = typeof(System.CRM.Data.Document_Spec);
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
            this.colDSID,
            this.colDCMT_DESC,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE});
         this.gridView1.GridControl = this.countryGridControl;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView1.OptionsView.ShowDetailButtons = false;
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
         this.colDCMT_DESC.Caption = "شرح مدرک";
         this.colDCMT_DESC.FieldName = "DCMT_DESC";
         this.colDCMT_DESC.Name = "colDCMT_DESC";
         this.colDCMT_DESC.Visible = true;
         this.colDCMT_DESC.VisibleIndex = 0;
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
         // DcspBn
         // 
         this.DcspBn.AddNewItem = this.bindingNavigatorAddNewItem;
         this.DcspBn.AutoSize = false;
         this.DcspBn.BindingSource = this.DcspBs;
         this.DcspBn.CountItem = this.bindingNavigatorCountItem;
         this.DcspBn.DeleteItem = null;
         this.DcspBn.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
         this.DcspBn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.Tsb_DelEpit,
            this.countryBindingNavigatorSaveItem});
         this.DcspBn.Location = new System.Drawing.Point(3, 18);
         this.DcspBn.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
         this.DcspBn.MoveLastItem = this.bindingNavigatorMoveLastItem;
         this.DcspBn.MoveNextItem = this.bindingNavigatorMoveNextItem;
         this.DcspBn.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
         this.DcspBn.Name = "DcspBn";
         this.DcspBn.PositionItem = this.bindingNavigatorPositionItem;
         this.DcspBn.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
         this.DcspBn.Size = new System.Drawing.Size(674, 25);
         this.DcspBn.TabIndex = 0;
         this.DcspBn.Text = "bindingNavigator1";
         // 
         // bindingNavigatorAddNewItem
         // 
         this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
         this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
         this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorAddNewItem.Text = "Add new";
         // 
         // bindingNavigatorCountItem
         // 
         this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
         this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
         this.bindingNavigatorCountItem.Text = "of {0}";
         this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
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
         // Tsb_DelEpit
         // 
         this.Tsb_DelEpit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.Tsb_DelEpit.Image = ((System.Drawing.Image)(resources.GetObject("Tsb_DelEpit.Image")));
         this.Tsb_DelEpit.Name = "Tsb_DelEpit";
         this.Tsb_DelEpit.RightToLeftAutoMirrorImage = true;
         this.Tsb_DelEpit.Size = new System.Drawing.Size(23, 22);
         this.Tsb_DelEpit.Text = "Delete";
         this.Tsb_DelEpit.Click += new System.EventHandler(this.Tsb_DelDcsp_Click);
         // 
         // countryBindingNavigatorSaveItem
         // 
         this.countryBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.countryBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("countryBindingNavigatorSaveItem.Image")));
         this.countryBindingNavigatorSaveItem.Name = "countryBindingNavigatorSaveItem";
         this.countryBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
         this.countryBindingNavigatorSaveItem.Text = "Save Data";
         this.countryBindingNavigatorSaveItem.Click += new System.EventHandler(this.SubmitChanged_Clicked);
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.Location = new System.Drawing.Point(303, 462);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(80, 27);
         this.Btn_Back.TabIndex = 0;
         this.Btn_Back.Text = "بازگشت";
         this.Btn_Back.UseVisualStyleBackColor = true;
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // DCSP_DFIN_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.Btn_Back);
         this.Controls.Add(this.tb_master);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "DCSP_DFIN_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(686, 498);
         ((System.ComponentModel.ISupportInitialize)(this.tb_master)).EndInit();
         this.tb_master.ResumeLayout(false);
         this.tp_001.ResumeLayout(false);
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.countryGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DcspBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DcspBn)).EndInit();
         this.DcspBn.ResumeLayout(false);
         this.DcspBn.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private C1.Win.C1Command.C1DockingTab tb_master;
      private C1.Win.C1Command.C1DockingTabPage tp_001;
      private Windows.Forms.BindingSource DcspBs;
      private C1.Win.C1Input.C1Button Btn_Back;
      private Windows.Forms.GroupBox groupBox1;
      private DevExpress.XtraGrid.GridControl countryGridControl;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private Windows.Forms.BindingNavigator DcspBn;
      private Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
      private Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
      private Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
      private Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
      private Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
      private Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
      private Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
      private Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
      private Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
      private Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
      private Windows.Forms.ToolStripButton Tsb_DelEpit;
      private Windows.Forms.ToolStripButton countryBindingNavigatorSaveItem;
      private DevExpress.XtraGrid.Columns.GridColumn colDSID;
      private DevExpress.XtraGrid.Columns.GridColumn colDCMT_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
   }
}
