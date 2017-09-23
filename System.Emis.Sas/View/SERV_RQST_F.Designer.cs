namespace System.Emis.Sas.View
{
   partial class SERV_RQST_F
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
         this.gridControl1 = new DevExpress.XtraGrid.GridControl();
         this.RqstBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colRQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQTP_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQTP_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.REQTPBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.colRQTP_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLETT_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colLETT_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colAGRE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colNEW_OLD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.ROWNBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.colSERV_FILE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.REQTPBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ROWNBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         this.SuspendLayout();
         // 
         // gridControl1
         // 
         this.gridControl1.DataSource = this.RqstBindingSource;
         this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.gridControl1.Location = new System.Drawing.Point(0, 0);
         this.gridControl1.MainView = this.gridView1;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2,
            this.persianRepositoryItemDateEdit1});
         this.gridControl1.Size = new System.Drawing.Size(1094, 562);
         this.gridControl1.TabIndex = 0;
         this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // gridView1
         // 
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Appearance.Row.Options.UseTextOptions = true;
         this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRQID,
            this.colRQTP_CODE,
            this.colRQTP_TYPE,
            this.colRQTP_DESC,
            this.colRWNO,
            this.colLETT_NO,
            this.colLETT_DATE,
            this.colAGRE_DATE,
            this.colNEW_OLD,
            this.colSERV_FILE_NO});
         this.gridView1.GridControl = this.gridControl1;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsBehavior.Editable = false;
         this.gridView1.OptionsBehavior.ReadOnly = true;
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowFooter = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colRQID
         // 
         this.colRQID.Caption = "شماره درخواست";
         this.colRQID.FieldName = "RQID";
         this.colRQID.Name = "colRQID";
         this.colRQID.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
         this.colRQID.Visible = true;
         this.colRQID.VisibleIndex = 8;
         // 
         // colRQTP_CODE
         // 
         this.colRQTP_CODE.Caption = "کد درخواست";
         this.colRQTP_CODE.FieldName = "RQTP_CODE";
         this.colRQTP_CODE.Name = "colRQTP_CODE";
         this.colRQTP_CODE.Visible = true;
         this.colRQTP_CODE.VisibleIndex = 6;
         // 
         // colRQTP_TYPE
         // 
         this.colRQTP_TYPE.Caption = "نوع درخواست";
         this.colRQTP_TYPE.ColumnEdit = this.repositoryItemLookUpEdit2;
         this.colRQTP_TYPE.FieldName = "RQTP_TYPE";
         this.colRQTP_TYPE.Name = "colRQTP_TYPE";
         this.colRQTP_TYPE.Visible = true;
         this.colRQTP_TYPE.VisibleIndex = 5;
         // 
         // repositoryItemLookUpEdit2
         // 
         this.repositoryItemLookUpEdit2.AutoHeight = false;
         this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemLookUpEdit2.DataSource = this.REQTPBindingSource;
         this.repositoryItemLookUpEdit2.DisplayMember = "RV_MEANING";
         this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
         this.repositoryItemLookUpEdit2.ValueMember = "RV_LOW_VALUE";
         // 
         // REQTPBindingSource
         // 
         this.REQTPBindingSource.DataSource = typeof(System.Emis.Sas.Model.CG_REF_CODES);
         // 
         // colRQTP_DESC
         // 
         this.colRQTP_DESC.Caption = "شرح درخواست";
         this.colRQTP_DESC.FieldName = "RQTP_DESC";
         this.colRQTP_DESC.Name = "colRQTP_DESC";
         this.colRQTP_DESC.Visible = true;
         this.colRQTP_DESC.VisibleIndex = 4;
         // 
         // colRWNO
         // 
         this.colRWNO.Caption = "ردیف درخواست";
         this.colRWNO.FieldName = "RWNO";
         this.colRWNO.Name = "colRWNO";
         this.colRWNO.Visible = true;
         this.colRWNO.VisibleIndex = 7;
         // 
         // colLETT_NO
         // 
         this.colLETT_NO.Caption = "شماره نامه";
         this.colLETT_NO.FieldName = "LETT_NO";
         this.colLETT_NO.Name = "colLETT_NO";
         this.colLETT_NO.Visible = true;
         this.colLETT_NO.VisibleIndex = 2;
         // 
         // colLETT_DATE
         // 
         this.colLETT_DATE.Caption = "تاریخ نامه";
         this.colLETT_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colLETT_DATE.FieldName = "LETT_DATE";
         this.colLETT_DATE.Name = "colLETT_DATE";
         this.colLETT_DATE.Visible = true;
         this.colLETT_DATE.VisibleIndex = 1;
         // 
         // colAGRE_DATE
         // 
         this.colAGRE_DATE.Caption = "تاریخ تایید";
         this.colAGRE_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colAGRE_DATE.FieldName = "AGRE_DATE";
         this.colAGRE_DATE.Name = "colAGRE_DATE";
         this.colAGRE_DATE.Visible = true;
         this.colAGRE_DATE.VisibleIndex = 3;
         // 
         // colNEW_OLD
         // 
         this.colNEW_OLD.Caption = "وضعیت درخواست";
         this.colNEW_OLD.ColumnEdit = this.repositoryItemLookUpEdit1;
         this.colNEW_OLD.FieldName = "NEW_OLD";
         this.colNEW_OLD.Name = "colNEW_OLD";
         this.colNEW_OLD.Visible = true;
         this.colNEW_OLD.VisibleIndex = 0;
         // 
         // repositoryItemLookUpEdit1
         // 
         this.repositoryItemLookUpEdit1.AutoHeight = false;
         this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemLookUpEdit1.DataSource = this.ROWNBindingSource;
         this.repositoryItemLookUpEdit1.DisplayMember = "RV_MEANING";
         this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
         this.repositoryItemLookUpEdit1.ValueMember = "RV_LOW_VALUE";
         // 
         // ROWNBindingSource
         // 
         this.ROWNBindingSource.DataSource = typeof(System.Emis.Sas.Model.CG_REF_CODES);
         // 
         // colSERV_FILE_NO
         // 
         this.colSERV_FILE_NO.Caption = "شماره پرونده";
         this.colSERV_FILE_NO.FieldName = "SERV_FILE_NO";
         this.colSERV_FILE_NO.Name = "colSERV_FILE_NO";
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
         // SERV_RQST_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.gridControl1);
         this.Name = "SERV_RQST_F";
         this.Size = new System.Drawing.Size(1094, 562);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.REQTPBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ROWNBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraGrid.GridControl gridControl1;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colRQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTP_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTP_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTP_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colRWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colLETT_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colLETT_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colAGRE_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colNEW_OLD;
      private DevExpress.XtraGrid.Columns.GridColumn colSERV_FILE_NO;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
      private Windows.Forms.BindingSource ROWNBindingSource;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
      private Windows.Forms.BindingSource REQTPBindingSource;
      private Windows.Forms.BindingSource RqstBindingSource;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
   }
}
