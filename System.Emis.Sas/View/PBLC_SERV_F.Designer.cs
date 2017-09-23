namespace System.Emis.Sas.View
{
   partial class PBLC_SERV_F
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
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.label1 = new System.Windows.Forms.Label();
         this.gridControl1 = new DevExpress.XtraGrid.GridControl();
         this.Change_Sas_PublicBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colRefServ = new DevExpress.XtraGrid.Columns.GridColumn();
         this.INS_SERV = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
         this.colSERV_FILE_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colREGN_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRECT_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.REC_TYPEBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.colCELL_PHON = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colZIP_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCUST_TEL = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colNEW_WORK_DAY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colNEW_BLOK_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colNEW_SEQ_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colACTN_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.colRQRO_RQST_RQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Btn_RunRqst = new System.Windows.Forms.Button();
         this.Cmb_Rqtp = new System.Windows.Forms.ComboBox();
         this.label2 = new System.Windows.Forms.Label();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.Txt_LettOwnr = new System.Windows.Forms.TextBox();
         this.Txt_LettDate = new System.Windows.Forms.TextBox();
         this.Txt_LettNo = new System.Windows.Forms.TextBox();
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Change_Sas_PublicBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.INS_SERV)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.REC_TYPEBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.Location = new System.Drawing.Point(613, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(246, 36);
         this.label1.TabIndex = 0;
         this.label1.Text = "تغییر مشخصات عمومی مشترکین";
         // 
         // gridControl1
         // 
         this.gridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.gridControl1.DataSource = this.Change_Sas_PublicBindingSource;
         this.gridControl1.Location = new System.Drawing.Point(3, 39);
         this.gridControl1.MainView = this.gridView1;
         this.gridControl1.Name = "gridControl1";
         this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.persianRepositoryItemDateEdit1,
            this.repositoryItemLookUpEdit1,
            this.INS_SERV});
         this.gridControl1.Size = new System.Drawing.Size(856, 480);
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
            this.colRefServ,
            this.colSERV_FILE_NO,
            this.colREGN_CODE,
            this.colRECT_CODE,
            this.colCELL_PHON,
            this.colZIP_CODE,
            this.colCUST_TEL,
            this.colNEW_WORK_DAY,
            this.colNEW_BLOK_CODE,
            this.colNEW_SEQ_CODE,
            this.colACTN_DATE,
            this.colRQRO_RQST_RQID});
         this.gridView1.GridControl = this.gridControl1;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowFooter = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colRefServ
         // 
         this.colRefServ.Caption = "اطلاعات عمومی";
         this.colRefServ.ColumnEdit = this.INS_SERV;
         this.colRefServ.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRefServ.Name = "colRefServ";
         this.colRefServ.Visible = true;
         this.colRefServ.VisibleIndex = 6;
         this.colRefServ.Width = 85;
         // 
         // INS_SERV
         // 
         this.INS_SERV.AutoHeight = false;
         this.INS_SERV.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "...", 20, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.INS_SERV.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
         this.INS_SERV.Name = "INS_SERV";
         this.INS_SERV.SingleClick = true;
         this.INS_SERV.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
         this.INS_SERV.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.INS_SERV_ButtonClick);
         // 
         // colSERV_FILE_NO
         // 
         this.colSERV_FILE_NO.Caption = "شماره پرونده";
         this.colSERV_FILE_NO.FieldName = "SERV_FILE_NO";
         this.colSERV_FILE_NO.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colSERV_FILE_NO.Name = "colSERV_FILE_NO";
         this.colSERV_FILE_NO.OptionsColumn.AllowEdit = false;
         this.colSERV_FILE_NO.OptionsColumn.ReadOnly = true;
         this.colSERV_FILE_NO.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
         this.colSERV_FILE_NO.Visible = true;
         this.colSERV_FILE_NO.VisibleIndex = 11;
         this.colSERV_FILE_NO.Width = 80;
         // 
         // colREGN_CODE
         // 
         this.colREGN_CODE.Caption = "کد ناحیه";
         this.colREGN_CODE.FieldName = "REGN_CODE";
         this.colREGN_CODE.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colREGN_CODE.Name = "colREGN_CODE";
         this.colREGN_CODE.OptionsColumn.AllowEdit = false;
         this.colREGN_CODE.OptionsColumn.ReadOnly = true;
         this.colREGN_CODE.Visible = true;
         this.colREGN_CODE.VisibleIndex = 10;
         this.colREGN_CODE.Width = 79;
         // 
         // colRECT_CODE
         // 
         this.colRECT_CODE.Caption = "وضعیت رکورد";
         this.colRECT_CODE.ColumnEdit = this.repositoryItemLookUpEdit1;
         this.colRECT_CODE.FieldName = "RECT_CODE";
         this.colRECT_CODE.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRECT_CODE.Name = "colRECT_CODE";
         this.colRECT_CODE.OptionsColumn.AllowEdit = false;
         this.colRECT_CODE.OptionsColumn.ReadOnly = true;
         this.colRECT_CODE.Visible = true;
         this.colRECT_CODE.VisibleIndex = 9;
         // 
         // repositoryItemLookUpEdit1
         // 
         this.repositoryItemLookUpEdit1.AutoHeight = false;
         this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.repositoryItemLookUpEdit1.DataSource = this.REC_TYPEBindingSource;
         this.repositoryItemLookUpEdit1.DisplayMember = "RECT_DESC";
         this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
         this.repositoryItemLookUpEdit1.NullText = "";
         this.repositoryItemLookUpEdit1.ValueMember = "CODE";
         // 
         // REC_TYPEBindingSource
         // 
         this.REC_TYPEBindingSource.DataSource = typeof(System.Emis.Sas.Model.SAS_REC_TYPE);
         // 
         // colCELL_PHON
         // 
         this.colCELL_PHON.Caption = "شماره تلفن همراه";
         this.colCELL_PHON.FieldName = "CELL_PHON";
         this.colCELL_PHON.Name = "colCELL_PHON";
         this.colCELL_PHON.OptionsColumn.AllowEdit = false;
         this.colCELL_PHON.OptionsColumn.ReadOnly = true;
         this.colCELL_PHON.Visible = true;
         this.colCELL_PHON.VisibleIndex = 0;
         this.colCELL_PHON.Width = 39;
         // 
         // colZIP_CODE
         // 
         this.colZIP_CODE.Caption = "کد پستی";
         this.colZIP_CODE.FieldName = "ZIP_CODE";
         this.colZIP_CODE.Name = "colZIP_CODE";
         this.colZIP_CODE.OptionsColumn.AllowEdit = false;
         this.colZIP_CODE.OptionsColumn.ReadOnly = true;
         this.colZIP_CODE.Visible = true;
         this.colZIP_CODE.VisibleIndex = 1;
         this.colZIP_CODE.Width = 39;
         // 
         // colCUST_TEL
         // 
         this.colCUST_TEL.Caption = "شماره تلفن ثابت";
         this.colCUST_TEL.FieldName = "CUST_TEL";
         this.colCUST_TEL.Name = "colCUST_TEL";
         this.colCUST_TEL.OptionsColumn.AllowEdit = false;
         this.colCUST_TEL.OptionsColumn.ReadOnly = true;
         this.colCUST_TEL.Visible = true;
         this.colCUST_TEL.VisibleIndex = 2;
         this.colCUST_TEL.Width = 39;
         // 
         // colNEW_WORK_DAY
         // 
         this.colNEW_WORK_DAY.Caption = "روزکار";
         this.colNEW_WORK_DAY.FieldName = "NEW_WORK_DAY";
         this.colNEW_WORK_DAY.Name = "colNEW_WORK_DAY";
         this.colNEW_WORK_DAY.OptionsColumn.AllowEdit = false;
         this.colNEW_WORK_DAY.OptionsColumn.ReadOnly = true;
         this.colNEW_WORK_DAY.Visible = true;
         this.colNEW_WORK_DAY.VisibleIndex = 5;
         this.colNEW_WORK_DAY.Width = 111;
         // 
         // colNEW_BLOK_CODE
         // 
         this.colNEW_BLOK_CODE.Caption = "مامور";
         this.colNEW_BLOK_CODE.FieldName = "NEW_BLOK_CODE";
         this.colNEW_BLOK_CODE.Name = "colNEW_BLOK_CODE";
         this.colNEW_BLOK_CODE.OptionsColumn.AllowEdit = false;
         this.colNEW_BLOK_CODE.OptionsColumn.ReadOnly = true;
         this.colNEW_BLOK_CODE.Visible = true;
         this.colNEW_BLOK_CODE.VisibleIndex = 4;
         this.colNEW_BLOK_CODE.Width = 44;
         // 
         // colNEW_SEQ_CODE
         // 
         this.colNEW_SEQ_CODE.Caption = "ترتیب";
         this.colNEW_SEQ_CODE.FieldName = "NEW_SEQ_CODE";
         this.colNEW_SEQ_CODE.Name = "colNEW_SEQ_CODE";
         this.colNEW_SEQ_CODE.OptionsColumn.AllowEdit = false;
         this.colNEW_SEQ_CODE.OptionsColumn.ReadOnly = true;
         this.colNEW_SEQ_CODE.Visible = true;
         this.colNEW_SEQ_CODE.VisibleIndex = 3;
         this.colNEW_SEQ_CODE.Width = 39;
         // 
         // colACTN_DATE
         // 
         this.colACTN_DATE.Caption = "تاریخ اقدام";
         this.colACTN_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colACTN_DATE.FieldName = "ACTN_DATE";
         this.colACTN_DATE.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colACTN_DATE.Name = "colACTN_DATE";
         this.colACTN_DATE.OptionsColumn.AllowEdit = false;
         this.colACTN_DATE.OptionsColumn.ReadOnly = true;
         this.colACTN_DATE.Visible = true;
         this.colACTN_DATE.VisibleIndex = 7;
         this.colACTN_DATE.Width = 90;
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
         // colRQRO_RQST_RQID
         // 
         this.colRQRO_RQST_RQID.Caption = "شماره درخواست";
         this.colRQRO_RQST_RQID.FieldName = "RQRO_RQST_RQID";
         this.colRQRO_RQST_RQID.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
         this.colRQRO_RQST_RQID.Name = "colRQRO_RQST_RQID";
         this.colRQRO_RQST_RQID.OptionsColumn.AllowEdit = false;
         this.colRQRO_RQST_RQID.OptionsColumn.ReadOnly = true;
         this.colRQRO_RQST_RQID.Visible = true;
         this.colRQRO_RQST_RQID.VisibleIndex = 8;
         this.colRQRO_RQST_RQID.Width = 100;
         // 
         // Btn_RunRqst
         // 
         this.Btn_RunRqst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.Btn_RunRqst.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Btn_RunRqst.Location = new System.Drawing.Point(378, 602);
         this.Btn_RunRqst.Name = "Btn_RunRqst";
         this.Btn_RunRqst.Size = new System.Drawing.Size(151, 41);
         this.Btn_RunRqst.TabIndex = 3;
         this.Btn_RunRqst.Text = "اجرای درخواست";
         this.Btn_RunRqst.UseVisualStyleBackColor = true;
         this.Btn_RunRqst.Click += new System.EventHandler(this.Btn_RunRqst_Click);
         // 
         // Cmb_Rqtp
         // 
         this.Cmb_Rqtp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.Cmb_Rqtp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.Cmb_Rqtp.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Cmb_Rqtp.FormattingEnabled = true;
         this.Cmb_Rqtp.Items.AddRange(new object[] {
            "تغییر شناسایی"});
         this.Cmb_Rqtp.Location = new System.Drawing.Point(535, 602);
         this.Cmb_Rqtp.Name = "Cmb_Rqtp";
         this.Cmb_Rqtp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Cmb_Rqtp.Size = new System.Drawing.Size(211, 41);
         this.Cmb_Rqtp.TabIndex = 2;
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label2.Location = new System.Drawing.Point(752, 605);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(107, 33);
         this.label2.TabIndex = 4;
         this.label2.Text = "نوع درخواست";
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.Controls.Add(this.label5);
         this.groupBox1.Controls.Add(this.label4);
         this.groupBox1.Controls.Add(this.label3);
         this.groupBox1.Controls.Add(this.Txt_LettOwnr);
         this.groupBox1.Controls.Add(this.Txt_LettDate);
         this.groupBox1.Controls.Add(this.Txt_LettNo);
         this.groupBox1.Location = new System.Drawing.Point(230, 525);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.groupBox1.Size = new System.Drawing.Size(629, 71);
         this.groupBox1.TabIndex = 1;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "اطلاعات نامه و درخواست کننده توزیع";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(223, 34);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(47, 13);
         this.label5.TabIndex = 1;
         this.label5.Text = "فرستنده";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(384, 34);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(49, 13);
         this.label4.TabIndex = 1;
         this.label4.Text = "تاریخ نامه";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(554, 34);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(58, 13);
         this.label3.TabIndex = 1;
         this.label3.Text = "شماره نامه";
         // 
         // Txt_LettOwnr
         // 
         this.Txt_LettOwnr.Location = new System.Drawing.Point(21, 31);
         this.Txt_LettOwnr.Name = "Txt_LettOwnr";
         this.Txt_LettOwnr.Size = new System.Drawing.Size(196, 21);
         this.Txt_LettOwnr.TabIndex = 2;
         // 
         // Txt_LettDate
         // 
         this.Txt_LettDate.Location = new System.Drawing.Point(278, 31);
         this.Txt_LettDate.Name = "Txt_LettDate";
         this.Txt_LettDate.Size = new System.Drawing.Size(100, 21);
         this.Txt_LettDate.TabIndex = 1;
         // 
         // Txt_LettNo
         // 
         this.Txt_LettNo.Location = new System.Drawing.Point(448, 31);
         this.Txt_LettNo.Name = "Txt_LettNo";
         this.Txt_LettNo.Size = new System.Drawing.Size(100, 21);
         this.Txt_LettNo.TabIndex = 0;
         // 
         // PBLC_SERV_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.Cmb_Rqtp);
         this.Controls.Add(this.Btn_RunRqst);
         this.Controls.Add(this.gridControl1);
         this.Controls.Add(this.label1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "PBLC_SERV_F";
         this.Size = new System.Drawing.Size(862, 658);
         ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Change_Sas_PublicBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.INS_SERV)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.REC_TYPEBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.Label label1;
      private DevExpress.XtraGrid.GridControl gridControl1;
      private Windows.Forms.BindingSource Change_Sas_PublicBindingSource;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colSERV_FILE_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colREGN_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colRECT_CODE;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
      private Windows.Forms.BindingSource REC_TYPEBindingSource;
      private DevExpress.XtraGrid.Columns.GridColumn colCELL_PHON;
      private DevExpress.XtraGrid.Columns.GridColumn colZIP_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colCUST_TEL;
      private DevExpress.XtraGrid.Columns.GridColumn colNEW_WORK_DAY;
      private DevExpress.XtraGrid.Columns.GridColumn colNEW_BLOK_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colNEW_SEQ_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colACTN_DATE;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RQST_RQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRefServ;
      private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit INS_SERV;
      private Windows.Forms.Button Btn_RunRqst;
      private Windows.Forms.ComboBox Cmb_Rqtp;
      private Windows.Forms.Label label2;
      private Windows.Forms.GroupBox groupBox1;
      private Windows.Forms.Label label3;
      private Windows.Forms.TextBox Txt_LettNo;
      private Windows.Forms.Label label5;
      private Windows.Forms.Label label4;
      private Windows.Forms.TextBox Txt_LettOwnr;
      private Windows.Forms.TextBox Txt_LettDate;
   }
}
