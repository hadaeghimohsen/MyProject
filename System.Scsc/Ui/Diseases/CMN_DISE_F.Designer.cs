namespace System.Scsc.Ui.Diseases
{
   partial class CMN_DISE_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMN_DISE_F));
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.diseases_TypeBindingNavigator = new System.Windows.Forms.BindingNavigator();
         this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
         this.diseases_TypeBindingSource = new System.Windows.Forms.BindingSource();
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
         this.diseases_TypeBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
         this.diseases_TypeGridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colCODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colNAME = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colDISE_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.tabPage2 = new System.Windows.Forms.TabPage();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.diseases_TypeBindingNavigator)).BeginInit();
         this.diseases_TypeBindingNavigator.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.diseases_TypeBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.diseases_TypeGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         this.SuspendLayout();
         // 
         // tabControl1
         // 
         this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Controls.Add(this.tabPage2);
         this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.tabControl1.Location = new System.Drawing.Point(24, 25);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.RightToLeftLayout = true;
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(552, 550);
         this.tabControl1.TabIndex = 0;
         // 
         // tabPage1
         // 
         this.tabPage1.Controls.Add(this.diseases_TypeBindingNavigator);
         this.tabPage1.Controls.Add(this.diseases_TypeGridControl);
         this.tabPage1.Location = new System.Drawing.Point(4, 23);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(544, 523);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "مشخصات وضعیت های جسمی";
         this.tabPage1.UseVisualStyleBackColor = true;
         // 
         // diseases_TypeBindingNavigator
         // 
         this.diseases_TypeBindingNavigator.AddNewItem = this.bindingNavigatorAddNewItem;
         this.diseases_TypeBindingNavigator.BindingSource = this.diseases_TypeBindingSource;
         this.diseases_TypeBindingNavigator.CountItem = this.bindingNavigatorCountItem;
         this.diseases_TypeBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
         this.diseases_TypeBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.diseases_TypeBindingNavigatorSaveItem});
         this.diseases_TypeBindingNavigator.Location = new System.Drawing.Point(3, 3);
         this.diseases_TypeBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
         this.diseases_TypeBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
         this.diseases_TypeBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
         this.diseases_TypeBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
         this.diseases_TypeBindingNavigator.Name = "diseases_TypeBindingNavigator";
         this.diseases_TypeBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
         this.diseases_TypeBindingNavigator.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
         this.diseases_TypeBindingNavigator.Size = new System.Drawing.Size(538, 25);
         this.diseases_TypeBindingNavigator.TabIndex = 1;
         this.diseases_TypeBindingNavigator.Text = "bindingNavigator1";
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
         // diseases_TypeBindingSource
         // 
         this.diseases_TypeBindingSource.DataSource = typeof(System.Scsc.Data.Diseases_Type);
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
         this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
         this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
         this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
         this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
         this.bindingNavigatorDeleteItem.Text = "Delete";
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
         // diseases_TypeBindingNavigatorSaveItem
         // 
         this.diseases_TypeBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.diseases_TypeBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("diseases_TypeBindingNavigatorSaveItem.Image")));
         this.diseases_TypeBindingNavigatorSaveItem.Name = "diseases_TypeBindingNavigatorSaveItem";
         this.diseases_TypeBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
         this.diseases_TypeBindingNavigatorSaveItem.Text = "Save Data";
         this.diseases_TypeBindingNavigatorSaveItem.Click += new System.EventHandler(this.diseases_TypeBindingNavigatorSaveItem_Click);
         // 
         // diseases_TypeGridControl
         // 
         this.diseases_TypeGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.diseases_TypeGridControl.DataSource = this.diseases_TypeBindingSource;
         this.diseases_TypeGridControl.Location = new System.Drawing.Point(6, 31);
         this.diseases_TypeGridControl.LookAndFeel.SkinName = "VS2010";
         this.diseases_TypeGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.diseases_TypeGridControl.MainView = this.gridView1;
         this.diseases_TypeGridControl.Name = "diseases_TypeGridControl";
         this.diseases_TypeGridControl.Size = new System.Drawing.Size(532, 486);
         this.diseases_TypeGridControl.TabIndex = 0;
         this.diseases_TypeGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
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
            this.colNAME,
            this.colDISE_DESC,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE});
         this.gridView1.GridControl = this.diseases_TypeGridControl;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsDetail.EnableMasterViewMode = false;
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colCODE
         // 
         this.colCODE.FieldName = "CODE";
         this.colCODE.Name = "colCODE";
         // 
         // colNAME
         // 
         this.colNAME.Caption = "نام لاتین";
         this.colNAME.FieldName = "NAME";
         this.colNAME.Name = "colNAME";
         this.colNAME.Visible = true;
         this.colNAME.VisibleIndex = 0;
         // 
         // colDISE_DESC
         // 
         this.colDISE_DESC.Caption = "نام فارسی";
         this.colDISE_DESC.FieldName = "DISE_DESC";
         this.colDISE_DESC.Name = "colDISE_DESC";
         this.colDISE_DESC.Visible = true;
         this.colDISE_DESC.VisibleIndex = 1;
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
         // tabPage2
         // 
         this.tabPage2.Location = new System.Drawing.Point(4, 22);
         this.tabPage2.Name = "tabPage2";
         this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage2.Size = new System.Drawing.Size(544, 524);
         this.tabPage2.TabIndex = 1;
         this.tabPage2.Text = "توضیحات";
         this.tabPage2.UseVisualStyleBackColor = true;
         // 
         // CMN_DISE_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabControl1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "CMN_DISE_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(600, 601);
         this.tabControl1.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         this.tabPage1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.diseases_TypeBindingNavigator)).EndInit();
         this.diseases_TypeBindingNavigator.ResumeLayout(false);
         this.diseases_TypeBindingNavigator.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.diseases_TypeBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.diseases_TypeGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.TabControl tabControl1;
      private Windows.Forms.TabPage tabPage1;
      private Windows.Forms.TabPage tabPage2;
      private DevExpress.XtraGrid.GridControl diseases_TypeGridControl;
      private Windows.Forms.BindingSource diseases_TypeBindingSource;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colCODE;
      private DevExpress.XtraGrid.Columns.GridColumn colNAME;
      private DevExpress.XtraGrid.Columns.GridColumn colDISE_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private Windows.Forms.BindingNavigator diseases_TypeBindingNavigator;
      private Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
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
      private Windows.Forms.ToolStripButton diseases_TypeBindingNavigatorSaveItem;
   }
}
