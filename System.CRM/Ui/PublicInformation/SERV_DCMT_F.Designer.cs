namespace System.CRM.Ui.PublicInformation
{
   partial class SERV_DCMT_F
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
         this.Btn_Back = new C1.Win.C1Input.C1Button();
         this.Tc_Dcmt = new C1.Win.C1Command.C1DockingTab();
         this.c1DockingTabPage1 = new C1.Win.C1Command.C1DockingTabPage();
         this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
         this.receive_DocumentGridControl = new DevExpress.XtraGrid.GridControl();
         this.vRqdcBs = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colRQTP_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQTP_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQTT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQTT_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQST_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.colSAVE_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.ShowRecieveDocument_Butn = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Tc_Dcmt)).BeginInit();
         this.Tc_Dcmt.SuspendLayout();
         this.c1DockingTabPage1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
         this.splitContainerControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.receive_DocumentGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.vRqdcBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         this.SuspendLayout();
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.Location = new System.Drawing.Point(432, 554);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(80, 27);
         this.Btn_Back.TabIndex = 2;
         this.Btn_Back.Text = "بازگشت";
         this.Btn_Back.UseVisualStyleBackColor = true;
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // Tc_Dcmt
         // 
         this.Tc_Dcmt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Tc_Dcmt.Controls.Add(this.c1DockingTabPage1);
         this.Tc_Dcmt.Location = new System.Drawing.Point(0, 3);
         this.Tc_Dcmt.Name = "Tc_Dcmt";
         this.Tc_Dcmt.RightToLeftLayout = true;
         this.Tc_Dcmt.Size = new System.Drawing.Size(941, 545);
         this.Tc_Dcmt.TabIndex = 3;
         this.Tc_Dcmt.TabsSpacing = 0;
         // 
         // c1DockingTabPage1
         // 
         this.c1DockingTabPage1.Controls.Add(this.splitContainerControl1);
         this.c1DockingTabPage1.Location = new System.Drawing.Point(1, 25);
         this.c1DockingTabPage1.Name = "c1DockingTabPage1";
         this.c1DockingTabPage1.Size = new System.Drawing.Size(939, 519);
         this.c1DockingTabPage1.TabIndex = 0;
         this.c1DockingTabPage1.Text = "مدارک پرونده";
         // 
         // splitContainerControl1
         // 
         this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
         this.splitContainerControl1.Horizontal = false;
         this.splitContainerControl1.IsSplitterFixed = true;
         this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
         this.splitContainerControl1.Name = "splitContainerControl1";
         this.splitContainerControl1.Panel1.Controls.Add(this.receive_DocumentGridControl);
         this.splitContainerControl1.Panel1.Text = "Panel1";
         this.splitContainerControl1.Panel2.Controls.Add(this.ShowRecieveDocument_Butn);
         this.splitContainerControl1.Panel2.Text = "Panel2";
         this.splitContainerControl1.Size = new System.Drawing.Size(939, 519);
         this.splitContainerControl1.SplitterPosition = 75;
         this.splitContainerControl1.TabIndex = 0;
         this.splitContainerControl1.Text = "splitContainerControl1";
         // 
         // receive_DocumentGridControl
         // 
         this.receive_DocumentGridControl.DataSource = this.vRqdcBs;
         this.receive_DocumentGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.receive_DocumentGridControl.Location = new System.Drawing.Point(0, 0);
         this.receive_DocumentGridControl.LookAndFeel.SkinName = "Office 2013 Dark Gray";
         this.receive_DocumentGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.receive_DocumentGridControl.MainView = this.gridView1;
         this.receive_DocumentGridControl.Name = "receive_DocumentGridControl";
         this.receive_DocumentGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.persianRepositoryItemDateEdit1});
         this.receive_DocumentGridControl.Size = new System.Drawing.Size(939, 439);
         this.receive_DocumentGridControl.TabIndex = 0;
         this.receive_DocumentGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // vRqdcBs
         // 
         this.vRqdcBs.DataSource = typeof(System.CRM.Data.VF_Request_DocumentResult);
         // 
         // gridView1
         // 
         this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Appearance.Row.Font = new System.Drawing.Font("IRANSans", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.gridView1.Appearance.Row.Options.UseFont = true;
         this.gridView1.Appearance.Row.Options.UseTextOptions = true;
         this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colRQTP_CODE,
            this.colRQTP_DESC,
            this.colRQTT_CODE,
            this.colRQTT_DESC,
            this.colRQID,
            this.colRWNO,
            this.colRQST_DATE,
            this.colSAVE_DATE});
         this.gridView1.GridControl = this.receive_DocumentGridControl;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsBehavior.Editable = false;
         this.gridView1.OptionsBehavior.ReadOnly = true;
         this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowDetailButtons = false;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colRQTP_CODE
         // 
         this.colRQTP_CODE.Caption = "کد نوع تقاضا";
         this.colRQTP_CODE.FieldName = "RQTP_CODE";
         this.colRQTP_CODE.Name = "colRQTP_CODE";
         this.colRQTP_CODE.Visible = true;
         this.colRQTP_CODE.VisibleIndex = 6;
         // 
         // colRQTP_DESC
         // 
         this.colRQTP_DESC.Caption = "شرح تقاضا";
         this.colRQTP_DESC.FieldName = "RQTP_DESC";
         this.colRQTP_DESC.Name = "colRQTP_DESC";
         this.colRQTP_DESC.Visible = true;
         this.colRQTP_DESC.VisibleIndex = 5;
         // 
         // colRQTT_CODE
         // 
         this.colRQTT_CODE.Caption = "کد نوع متقاضی";
         this.colRQTT_CODE.FieldName = "RQTT_CODE";
         this.colRQTT_CODE.Name = "colRQTT_CODE";
         this.colRQTT_CODE.Visible = true;
         this.colRQTT_CODE.VisibleIndex = 4;
         // 
         // colRQTT_DESC
         // 
         this.colRQTT_DESC.Caption = "شرح متقاضی";
         this.colRQTT_DESC.FieldName = "RQTT_DESC";
         this.colRQTT_DESC.Name = "colRQTT_DESC";
         this.colRQTT_DESC.Visible = true;
         this.colRQTT_DESC.VisibleIndex = 3;
         // 
         // colRQID
         // 
         this.colRQID.Caption = "شماره درخواست";
         this.colRQID.FieldName = "RQID";
         this.colRQID.Name = "colRQID";
         this.colRQID.Visible = true;
         this.colRQID.VisibleIndex = 2;
         // 
         // colRWNO
         // 
         this.colRWNO.Caption = "ردیف درخواست";
         this.colRWNO.FieldName = "RWNO";
         this.colRWNO.Name = "colRWNO";
         // 
         // colRQST_DATE
         // 
         this.colRQST_DATE.Caption = "تاریخ ثبت درخواست";
         this.colRQST_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colRQST_DATE.FieldName = "RQST_DATE";
         this.colRQST_DATE.Name = "colRQST_DATE";
         this.colRQST_DATE.Visible = true;
         this.colRQST_DATE.VisibleIndex = 1;
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
         this.colSAVE_DATE.Caption = "تاریخ تایید درخواست";
         this.colSAVE_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colSAVE_DATE.FieldName = "SAVE_DATE";
         this.colSAVE_DATE.Name = "colSAVE_DATE";
         this.colSAVE_DATE.Visible = true;
         this.colSAVE_DATE.VisibleIndex = 0;
         // 
         // ShowRecieveDocument_Butn
         // 
         this.ShowRecieveDocument_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.ShowRecieveDocument_Butn.Appearance.Font = new System.Drawing.Font("B Traffic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.ShowRecieveDocument_Butn.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.ShowRecieveDocument_Butn.Appearance.Options.UseFont = true;
         this.ShowRecieveDocument_Butn.Appearance.Options.UseForeColor = true;
         this.ShowRecieveDocument_Butn.Appearance.Options.UseTextOptions = true;
         this.ShowRecieveDocument_Butn.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.ShowRecieveDocument_Butn.Image = global::System.CRM.Properties.Resources.IMAGE_1425;
         this.ShowRecieveDocument_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.ShowRecieveDocument_Butn.Location = new System.Drawing.Point(584, 4);
         this.ShowRecieveDocument_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.ShowRecieveDocument_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ShowRecieveDocument_Butn.Name = "ShowRecieveDocument_Butn";
         this.ShowRecieveDocument_Butn.Size = new System.Drawing.Size(346, 62);
         this.ShowRecieveDocument_Butn.TabIndex = 17;
         this.ShowRecieveDocument_Butn.Text = "مدارک ضمیمه شده درخواست";
         this.ShowRecieveDocument_Butn.Click += new System.EventHandler(this.ShowRecieveDocument_Butn_Click);
         // 
         // SERV_DCMT_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.Tc_Dcmt);
         this.Controls.Add(this.Btn_Back);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "SERV_DCMT_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(944, 584);
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Tc_Dcmt)).EndInit();
         this.Tc_Dcmt.ResumeLayout(false);
         this.c1DockingTabPage1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
         this.splitContainerControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.receive_DocumentGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.vRqdcBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private C1.Win.C1Input.C1Button Btn_Back;
      private C1.Win.C1Command.C1DockingTab Tc_Dcmt;
      private C1.Win.C1Command.C1DockingTabPage c1DockingTabPage1;
      private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
      private DevExpress.XtraGrid.GridControl receive_DocumentGridControl;
      private Windows.Forms.BindingSource vRqdcBs;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private DevExpress.XtraEditors.SimpleButton ShowRecieveDocument_Butn;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTP_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTP_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTT_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRQTT_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colRQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colRQST_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colSAVE_DATE;
   }
}
