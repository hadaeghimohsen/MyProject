namespace System.Scsc.Ui.Reason
{
   partial class CMN_RESN_F
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
         System.Windows.Forms.Label rQIDLabel;
         System.Windows.Forms.Label rQTP_DESCLabel;
         System.Windows.Forms.Label fILE_NOLabel;
         System.Windows.Forms.Label rWNOLabel;
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.Btn_Back = new System.MaxUi.NewMaxBtn();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.RsrqGv1 = new DevExpress.XtraGrid.GridControl();
         this.RsrqBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.RqroBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.RqstBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colRESN_RQTP_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRESN_RWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.ResnBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.colRQRO_RQST_RQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQRO_RWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colOTHR_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colReason_Spec = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequest_Row = new DevExpress.XtraGrid.Columns.GridColumn();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.rWNOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.fILE_NOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.rQTP_DESCTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.rQIDTextEdit = new DevExpress.XtraEditors.TextEdit();
         rQIDLabel = new System.Windows.Forms.Label();
         rQTP_DESCLabel = new System.Windows.Forms.Label();
         fILE_NOLabel = new System.Windows.Forms.Label();
         rWNOLabel = new System.Windows.Forms.Label();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.RsrqGv1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RsrqBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqroBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ResnBs1)).BeginInit();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rWNOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.fILE_NOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rQTP_DESCTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rQIDTextEdit.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // rQIDLabel
         // 
         rQIDLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         rQIDLabel.AutoSize = true;
         rQIDLabel.Location = new System.Drawing.Point(696, 16);
         rQIDLabel.Name = "rQIDLabel";
         rQIDLabel.Size = new System.Drawing.Size(93, 14);
         rQIDLabel.TabIndex = 1;
         rQIDLabel.Text = "شماره درخواست";
         // 
         // rQTP_DESCLabel
         // 
         rQTP_DESCLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         rQTP_DESCLabel.AutoSize = true;
         rQTP_DESCLabel.Location = new System.Drawing.Point(567, 16);
         rQTP_DESCLabel.Name = "rQTP_DESCLabel";
         rQTP_DESCLabel.Size = new System.Drawing.Size(52, 14);
         rQTP_DESCLabel.TabIndex = 1;
         rQTP_DESCLabel.Text = "نوع تقاضا";
         // 
         // fILE_NOLabel
         // 
         fILE_NOLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         fILE_NOLabel.AutoSize = true;
         fILE_NOLabel.Location = new System.Drawing.Point(327, 16);
         fILE_NOLabel.Name = "fILE_NOLabel";
         fILE_NOLabel.Size = new System.Drawing.Size(73, 14);
         fILE_NOLabel.TabIndex = 1;
         fILE_NOLabel.Text = "شماره پرونده";
         // 
         // rWNOLabel
         // 
         rWNOLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         rWNOLabel.AutoSize = true;
         rWNOLabel.Location = new System.Drawing.Point(419, 16);
         rWNOLabel.Name = "rWNOLabel";
         rWNOLabel.Size = new System.Drawing.Size(84, 14);
         rWNOLabel.TabIndex = 1;
         rWNOLabel.Text = "ردیف درخواست";
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.BackColor = System.Drawing.Color.Transparent;
         this.Btn_Back.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_Back.Caption = "بازگشت";
         this.Btn_Back.Disabled = false;
         this.Btn_Back.EnterColor = System.Drawing.Color.Transparent;
         this.Btn_Back.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Btn_Back.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Back.ImageIndex = -1;
         this.Btn_Back.ImageList = null;
         this.Btn_Back.InToBold = false;
         this.Btn_Back.Location = new System.Drawing.Point(385, 492);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(93, 27);
         this.Btn_Back.TabIndex = 4;
         this.Btn_Back.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Back.TextColor = System.Drawing.Color.Black;
         this.Btn_Back.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // tabControl1
         // 
         this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.tabControl1.Location = new System.Drawing.Point(19, 21);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.tabControl1.RightToLeftLayout = true;
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(823, 465);
         this.tabControl1.TabIndex = 5;
         // 
         // tabPage1
         // 
         this.tabPage1.AutoScroll = true;
         this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.tabPage1.Controls.Add(this.RsrqGv1);
         this.tabPage1.Controls.Add(this.groupBox1);
         this.tabPage1.Location = new System.Drawing.Point(4, 23);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(815, 438);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "دلایل درخواست";
         // 
         // RsrqGv1
         // 
         this.RsrqGv1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.RsrqGv1.DataSource = this.RsrqBs1;
         this.RsrqGv1.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.RsrqGv1_Bn_ButtonClick);
         this.RsrqGv1.Location = new System.Drawing.Point(6, 81);
         this.RsrqGv1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.RsrqGv1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.RsrqGv1.MainView = this.gridView1;
         this.RsrqGv1.Name = "RsrqGv1";
         this.RsrqGv1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
         this.RsrqGv1.Size = new System.Drawing.Size(803, 351);
         this.RsrqGv1.TabIndex = 3;
         this.RsrqGv1.UseEmbeddedNavigator = true;
         this.RsrqGv1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // RsrqBs1
         // 
         this.RsrqBs1.DataMember = "Reason_Requests";
         this.RsrqBs1.DataSource = this.RqroBs1;
         // 
         // RqroBs1
         // 
         this.RqroBs1.DataMember = "Request_Rows";
         this.RqroBs1.DataSource = this.RqstBs1;
         // 
         // RqstBs1
         // 
         this.RqstBs1.DataSource = typeof(System.Scsc.Data.Request);
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
            this.colRESN_RQTP_CODE,
            this.colRESN_RWNO,
            this.colRQRO_RQST_RQID,
            this.colRQRO_RWNO,
            this.colRWNO,
            this.colOTHR_DESC,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colReason_Spec,
            this.colRequest_Row});
         this.gridView1.GridControl = this.RsrqGv1;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colRWNO, DevExpress.Data.ColumnSortOrder.Ascending)});
         // 
         // colRESN_RQTP_CODE
         // 
         this.colRESN_RQTP_CODE.FieldName = "RESN_RQTP_CODE";
         this.colRESN_RQTP_CODE.Name = "colRESN_RQTP_CODE";
         // 
         // colRESN_RWNO
         // 
         this.colRESN_RWNO.Caption = "نوع دلیل";
         this.colRESN_RWNO.ColumnEdit = this.repositoryItemLookUpEdit1;
         this.colRESN_RWNO.FieldName = "RESN_RWNO";
         this.colRESN_RWNO.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRESN_RWNO.Name = "colRESN_RWNO";
         this.colRESN_RWNO.Visible = true;
         this.colRESN_RWNO.VisibleIndex = 1;
         this.colRESN_RWNO.Width = 224;
         // 
         // repositoryItemLookUpEdit1
         // 
         this.repositoryItemLookUpEdit1.AutoHeight = false;
         this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("RQTP_CODE", "RQTP_CODE", 84, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("RWNO", "ردیف", 42, DevExpress.Utils.FormatType.Numeric, "", true, DevExpress.Utils.HorzAlignment.Far),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("RESN_DESC", "شرح دلیل", 68, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CRET_BY", "CRET_BY", 54, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CRET_DATE", "CRET_DATE", 68, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MDFY_BY", "MDFY_BY", 55, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MDFY_DATE", "MDFY_DATE", 69, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Request_Type", "Request_Type", 80, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near)});
         this.repositoryItemLookUpEdit1.DataSource = this.ResnBs1;
         this.repositoryItemLookUpEdit1.DisplayMember = "RESN_DESC";
         this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
         this.repositoryItemLookUpEdit1.NullText = "";
         this.repositoryItemLookUpEdit1.ValueMember = "RWNO";
         // 
         // ResnBs1
         // 
         this.ResnBs1.DataSource = typeof(System.Scsc.Data.Reason_Spec);
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
         // colRWNO
         // 
         this.colRWNO.Caption = "ردیف";
         this.colRWNO.FieldName = "RWNO";
         this.colRWNO.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRWNO.Name = "colRWNO";
         this.colRWNO.Visible = true;
         this.colRWNO.VisibleIndex = 2;
         this.colRWNO.Width = 48;
         // 
         // colOTHR_DESC
         // 
         this.colOTHR_DESC.Caption = "توضیحات";
         this.colOTHR_DESC.FieldName = "OTHR_DESC";
         this.colOTHR_DESC.Name = "colOTHR_DESC";
         this.colOTHR_DESC.Visible = true;
         this.colOTHR_DESC.VisibleIndex = 0;
         this.colOTHR_DESC.Width = 525;
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
         // colReason_Spec
         // 
         this.colReason_Spec.FieldName = "Reason_Spec";
         this.colReason_Spec.Name = "colReason_Spec";
         // 
         // colRequest_Row
         // 
         this.colRequest_Row.FieldName = "Request_Row";
         this.colRequest_Row.Name = "colRequest_Row";
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.Controls.Add(rWNOLabel);
         this.groupBox1.Controls.Add(fILE_NOLabel);
         this.groupBox1.Controls.Add(this.rWNOTextEdit);
         this.groupBox1.Controls.Add(rQTP_DESCLabel);
         this.groupBox1.Controls.Add(this.fILE_NOTextEdit);
         this.groupBox1.Controls.Add(rQIDLabel);
         this.groupBox1.Controls.Add(this.rQTP_DESCTextEdit);
         this.groupBox1.Controls.Add(this.rQIDTextEdit);
         this.groupBox1.Location = new System.Drawing.Point(6, 6);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(803, 69);
         this.groupBox1.TabIndex = 0;
         this.groupBox1.TabStop = false;
         // 
         // rWNOTextEdit
         // 
         this.rWNOTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rWNOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.RqroBs1, "RWNO", true));
         this.rWNOTextEdit.Location = new System.Drawing.Point(419, 33);
         this.rWNOTextEdit.Name = "rWNOTextEdit";
         this.rWNOTextEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rWNOTextEdit.Properties.Appearance.Options.UseFont = true;
         this.rWNOTextEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.rWNOTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.rWNOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.rWNOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rWNOTextEdit.Properties.ReadOnly = true;
         this.rWNOTextEdit.Size = new System.Drawing.Size(76, 22);
         this.rWNOTextEdit.TabIndex = 2;
         // 
         // fILE_NOTextEdit
         // 
         this.fILE_NOTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.fILE_NOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.RqroBs1, "FIGH_FILE_NO", true));
         this.fILE_NOTextEdit.Location = new System.Drawing.Point(313, 32);
         this.fILE_NOTextEdit.Name = "fILE_NOTextEdit";
         this.fILE_NOTextEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.fILE_NOTextEdit.Properties.Appearance.Options.UseFont = true;
         this.fILE_NOTextEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.fILE_NOTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.fILE_NOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.fILE_NOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.fILE_NOTextEdit.Properties.ReadOnly = true;
         this.fILE_NOTextEdit.Size = new System.Drawing.Size(100, 22);
         this.fILE_NOTextEdit.TabIndex = 2;
         // 
         // rQTP_DESCTextEdit
         // 
         this.rQTP_DESCTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rQTP_DESCTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.RqstBs1, "Request_Type.RQTP_DESC", true));
         this.rQTP_DESCTextEdit.Location = new System.Drawing.Point(501, 32);
         this.rQTP_DESCTextEdit.Name = "rQTP_DESCTextEdit";
         this.rQTP_DESCTextEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rQTP_DESCTextEdit.Properties.Appearance.Options.UseFont = true;
         this.rQTP_DESCTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.rQTP_DESCTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rQTP_DESCTextEdit.Properties.ReadOnly = true;
         this.rQTP_DESCTextEdit.Size = new System.Drawing.Size(181, 22);
         this.rQTP_DESCTextEdit.TabIndex = 2;
         // 
         // rQIDTextEdit
         // 
         this.rQIDTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rQIDTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.RqstBs1, "RQID", true));
         this.rQIDTextEdit.EditValue = "";
         this.rQIDTextEdit.Location = new System.Drawing.Point(688, 32);
         this.rQIDTextEdit.Name = "rQIDTextEdit";
         this.rQIDTextEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rQIDTextEdit.Properties.Appearance.Options.UseFont = true;
         this.rQIDTextEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.rQIDTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.rQIDTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.rQIDTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rQIDTextEdit.Properties.ReadOnly = true;
         this.rQIDTextEdit.Size = new System.Drawing.Size(100, 22);
         this.rQIDTextEdit.TabIndex = 2;
         // 
         // CMN_RESN_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.Controls.Add(this.tabControl1);
         this.Controls.Add(this.Btn_Back);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "CMN_RESN_F";
         this.Size = new System.Drawing.Size(862, 536);
         this.tabControl1.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.RsrqGv1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RsrqBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqroBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ResnBs1)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rWNOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.fILE_NOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rQTP_DESCTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rQIDTextEdit.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private MaxUi.NewMaxBtn Btn_Back;
      private Windows.Forms.TabControl tabControl1;
      private Windows.Forms.TabPage tabPage1;
      private Windows.Forms.GroupBox groupBox1;
      private Windows.Forms.BindingSource RqstBs1;
      private DevExpress.XtraEditors.TextEdit fILE_NOTextEdit;
      private DevExpress.XtraEditors.TextEdit rQTP_DESCTextEdit;
      private DevExpress.XtraEditors.TextEdit rQIDTextEdit;
      private DevExpress.XtraGrid.GridControl RsrqGv1;
      private Windows.Forms.BindingSource RsrqBs1;
      private Windows.Forms.BindingSource RqroBs1;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colRESN_RQTP_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRESN_RWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RQST_RQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colRWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colOTHR_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colReason_Spec;
      private DevExpress.XtraGrid.Columns.GridColumn colRequest_Row;
      private DevExpress.XtraEditors.TextEdit rWNOTextEdit;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
      private Windows.Forms.BindingSource ResnBs1;
   }
}
