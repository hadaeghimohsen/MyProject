﻿namespace System.Scsc.Ui.Document
{
   partial class CMN_DCMT_F
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
         System.Windows.Forms.Label StrtDate_Lb;
         System.Windows.Forms.Label EndDate_Lb;
         System.Windows.Forms.Label DelvDate_Lb;
         System.Windows.Forms.Label RcdcDesc_Lb;
         System.Windows.Forms.Label RcdcStat_Lb;
         System.Windows.Forms.Label PermStat_Lb;
         System.Windows.Forms.Label Rwno_Lb;
         System.Windows.Forms.Label FileName_Lb;
         System.Windows.Forms.Label ImageAlign_Lb;
         System.Windows.Forms.Label ImageQulity_Lb;
         System.Windows.Forms.Label Dimsn_Lb;
         System.Windows.Forms.Label SelectCamera_Lb;
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMN_DCMT_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
         this.TC_Dcmt = new System.Windows.Forms.TabControl();
         this.tp_001 = new System.Windows.Forms.TabPage();
         this.Dcmt_Gb = new System.Windows.Forms.GroupBox();
         this.CB_ImageSize = new System.Windows.Forms.ComboBox();
         this.Bt_SelectFile = new System.Windows.Forms.Button();
         this.pERM_STATLookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
         this.receive_DocumentBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.dPRSTBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.FILE_NAME_TextBox = new System.Windows.Forms.TextBox();
         this.ImdcBs1 = new System.Windows.Forms.BindingSource(this.components);
         this.rCDC_STATLookUpEdit = new DevExpress.XtraEditors.LookUpEdit();
         this.dDCMTBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.rWNOTextBox = new System.Windows.Forms.TextBox();
         this.rCDC_DESCTextBox = new System.Windows.Forms.TextBox();
         this.dELV_DATEPersianDateEdit = new dxExample.PersianDateEdit();
         this.eND_DATEPersianDateEdit = new dxExample.PersianDateEdit();
         this.sTRT_DATEPersianDateEdit = new dxExample.PersianDateEdit();
         this.receive_DocumentGridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.RqtpCode_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.RqtpDesc_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.RqttCode_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.RqttDesc_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.DcmtDesc_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.NeedType_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.NEED_LOV = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.dDCNDBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.OrigType_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.ORIG_LOV = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.dDCTPBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.FrstNeed_Clm = new DevExpress.XtraGrid.Columns.GridColumn();
         this.FRST_LOV = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.dYSNOBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.tp_002 = new System.Windows.Forms.TabPage();
         this.Image_Gb = new System.Windows.Forms.GroupBox();
         this.Btn_SetProfileImage = new System.MaxUi.NewToolBtn();
         this.img = new System.Windows.Forms.ImageList(this.components);
         this.Bt_RemvImage = new System.Windows.Forms.Button();
         this.UD_Interpolation = new System.Windows.Forms.NumericUpDown();
         this.ZC_ZoomImage = new DevExpress.XtraEditors.ZoomTrackBarControl();
         this.CB_Alignment = new System.Windows.Forms.ComboBox();
         this.CB_AllowMouseDrag = new System.Windows.Forms.CheckBox();
         this.CB_ShowScroll = new System.Windows.Forms.CheckBox();
         this.PE_ImageShow = new DevExpress.XtraEditors.PictureEdit();
         this.tp_003 = new System.Windows.Forms.TabPage();
         this.Btn_AcceptPicture = new DevExpress.XtraEditors.SimpleButton();
         this.Btn_TakePicture = new DevExpress.XtraEditors.SimpleButton();
         this.Npb_Face3x4Zone = new System.MaxUi.NewPickBtn();
         this.Npb_FaceZone = new System.MaxUi.NewPickBtn();
         this.Tb_StartStopVideo = new System.MaxUi.NewPickBtn();
         this.LB_Result = new System.Windows.Forms.Label();
         this.LOV_VideoSrc = new DevExpress.XtraEditors.LookUpEdit();
         this.Pb_Face3x4Zone = new System.Windows.Forms.PictureBox();
         this.Pb_FaceZone = new System.Windows.Forms.PictureBox();
         this.pb_destination = new System.Windows.Forms.PictureBox();
         this.pb_source = new System.Windows.Forms.PictureBox();
         this.DG_SelectImage = new System.Windows.Forms.OpenFileDialog();
         this.Tm_NewFrameProcess = new System.Windows.Forms.Timer(this.components);
         this.RqstBn1 = new System.Windows.Forms.BindingNavigator(this.components);
         this.RqstBnNew1 = new System.Windows.Forms.ToolStripButton();
         this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
         this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
         this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
         this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
         this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
         this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
         this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
         this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
         this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
         this.RqstBnDelete1 = new System.Windows.Forms.ToolStripButton();
         this.RqstBnARqt1 = new System.Windows.Forms.ToolStripButton();
         this.RqstMBnDefaultPrint1 = new System.Windows.Forms.ToolStripSplitButton();
         this.RqstBnDefaultPrint1 = new System.Windows.Forms.ToolStripMenuItem();
         this.RqstBnPrint1 = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
         this.RqstBnSettingPrint1 = new System.Windows.Forms.ToolStripMenuItem();
         this.RqstBnASav1 = new System.Windows.Forms.ToolStripButton();
         this.RqstBnAResn1 = new System.Windows.Forms.ToolStripButton();
         this.RqstBnADoc1 = new System.Windows.Forms.ToolStripButton();
         this.RqstBnRegl01 = new System.Windows.Forms.ToolStripButton();
         this.RqstBnExit1 = new System.Windows.Forms.ToolStripButton();
         StrtDate_Lb = new System.Windows.Forms.Label();
         EndDate_Lb = new System.Windows.Forms.Label();
         DelvDate_Lb = new System.Windows.Forms.Label();
         RcdcDesc_Lb = new System.Windows.Forms.Label();
         RcdcStat_Lb = new System.Windows.Forms.Label();
         PermStat_Lb = new System.Windows.Forms.Label();
         Rwno_Lb = new System.Windows.Forms.Label();
         FileName_Lb = new System.Windows.Forms.Label();
         ImageAlign_Lb = new System.Windows.Forms.Label();
         ImageQulity_Lb = new System.Windows.Forms.Label();
         Dimsn_Lb = new System.Windows.Forms.Label();
         SelectCamera_Lb = new System.Windows.Forms.Label();
         this.TC_Dcmt.SuspendLayout();
         this.tp_001.SuspendLayout();
         this.Dcmt_Gb.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pERM_STATLookUpEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.receive_DocumentBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dPRSTBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImdcBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rCDC_STATLookUpEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dDCMTBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dELV_DATEPersianDateEdit.Properties.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dELV_DATEPersianDateEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.eND_DATEPersianDateEdit.Properties.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.eND_DATEPersianDateEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sTRT_DATEPersianDateEdit.Properties.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.sTRT_DATEPersianDateEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.receive_DocumentGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.NEED_LOV)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dDCNDBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ORIG_LOV)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dDCTPBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.FRST_LOV)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.dYSNOBindingSource)).BeginInit();
         this.tp_002.SuspendLayout();
         this.Image_Gb.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.UD_Interpolation)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ZC_ZoomImage)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.ZC_ZoomImage.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.PE_ImageShow.Properties)).BeginInit();
         this.tp_003.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.LOV_VideoSrc.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pb_Face3x4Zone)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pb_FaceZone)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_destination)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_source)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBn1)).BeginInit();
         this.RqstBn1.SuspendLayout();
         this.SuspendLayout();
         // 
         // StrtDate_Lb
         // 
         StrtDate_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         StrtDate_Lb.AutoSize = true;
         StrtDate_Lb.Location = new System.Drawing.Point(766, 24);
         StrtDate_Lb.Name = "StrtDate_Lb";
         StrtDate_Lb.Size = new System.Drawing.Size(102, 14);
         StrtDate_Lb.TabIndex = 1;
         StrtDate_Lb.Text = "تاریخ شروع مدرک :";
         // 
         // EndDate_Lb
         // 
         EndDate_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         EndDate_Lb.AutoSize = true;
         EndDate_Lb.Location = new System.Drawing.Point(766, 50);
         EndDate_Lb.Name = "EndDate_Lb";
         EndDate_Lb.Size = new System.Drawing.Size(97, 14);
         EndDate_Lb.TabIndex = 3;
         EndDate_Lb.Text = "تاریخ اتمام مدرک :";
         // 
         // DelvDate_Lb
         // 
         DelvDate_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         DelvDate_Lb.AutoSize = true;
         DelvDate_Lb.Location = new System.Drawing.Point(766, 76);
         DelvDate_Lb.Name = "DelvDate_Lb";
         DelvDate_Lb.Size = new System.Drawing.Size(101, 14);
         DelvDate_Lb.TabIndex = 5;
         DelvDate_Lb.Text = "تاریخ تحویل مدرک :";
         // 
         // RcdcDesc_Lb
         // 
         RcdcDesc_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         RcdcDesc_Lb.AutoSize = true;
         RcdcDesc_Lb.Location = new System.Drawing.Point(766, 102);
         RcdcDesc_Lb.Name = "RcdcDesc_Lb";
         RcdcDesc_Lb.Size = new System.Drawing.Size(58, 14);
         RcdcDesc_Lb.TabIndex = 7;
         RcdcDesc_Lb.Text = "توضیحات :";
         // 
         // RcdcStat_Lb
         // 
         RcdcStat_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         RcdcStat_Lb.AutoSize = true;
         RcdcStat_Lb.Location = new System.Drawing.Point(489, 24);
         RcdcStat_Lb.Name = "RcdcStat_Lb";
         RcdcStat_Lb.Size = new System.Drawing.Size(82, 14);
         RcdcStat_Lb.TabIndex = 9;
         RcdcStat_Lb.Text = "وضعیت مدرک :";
         // 
         // PermStat_Lb
         // 
         PermStat_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         PermStat_Lb.AutoSize = true;
         PermStat_Lb.Location = new System.Drawing.Point(489, 50);
         PermStat_Lb.Name = "PermStat_Lb";
         PermStat_Lb.Size = new System.Drawing.Size(68, 14);
         PermStat_Lb.TabIndex = 11;
         PermStat_Lb.Text = "تايید مدرک :";
         // 
         // Rwno_Lb
         // 
         Rwno_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         Rwno_Lb.AutoSize = true;
         Rwno_Lb.Location = new System.Drawing.Point(766, 128);
         Rwno_Lb.Name = "Rwno_Lb";
         Rwno_Lb.Size = new System.Drawing.Size(39, 14);
         Rwno_Lb.TabIndex = 0;
         Rwno_Lb.Text = "ردیف :";
         // 
         // FileName_Lb
         // 
         FileName_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         FileName_Lb.AutoSize = true;
         FileName_Lb.Enabled = false;
         FileName_Lb.Location = new System.Drawing.Point(766, 154);
         FileName_Lb.Name = "FileName_Lb";
         FileName_Lb.Size = new System.Drawing.Size(68, 14);
         FileName_Lb.TabIndex = 2;
         FileName_Lb.Text = "آدرس فایل :";
         FileName_Lb.Visible = false;
         // 
         // ImageAlign_Lb
         // 
         ImageAlign_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         ImageAlign_Lb.AutoSize = true;
         ImageAlign_Lb.Location = new System.Drawing.Point(667, 68);
         ImageAlign_Lb.Name = "ImageAlign_Lb";
         ImageAlign_Lb.Size = new System.Drawing.Size(159, 14);
         ImageAlign_Lb.TabIndex = 7;
         ImageAlign_Lb.Text = "قرارگیری عکس بر روی صفحه :";
         // 
         // ImageQulity_Lb
         // 
         ImageQulity_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         ImageQulity_Lb.AutoSize = true;
         ImageQulity_Lb.Location = new System.Drawing.Point(697, 96);
         ImageQulity_Lb.Name = "ImageQulity_Lb";
         ImageQulity_Lb.Size = new System.Drawing.Size(135, 14);
         ImageQulity_Lb.TabIndex = 8;
         ImageQulity_Lb.Text = "نوع نمایش کیفیت عکس :";
         // 
         // Dimsn_Lb
         // 
         Dimsn_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         Dimsn_Lb.AutoSize = true;
         Dimsn_Lb.Location = new System.Drawing.Point(629, 128);
         Dimsn_Lb.Name = "Dimsn_Lb";
         Dimsn_Lb.Size = new System.Drawing.Size(98, 14);
         Dimsn_Lb.TabIndex = 0;
         Dimsn_Lb.Text = "ابعاد تصویر مدرک :";
         // 
         // SelectCamera_Lb
         // 
         SelectCamera_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         SelectCamera_Lb.AutoSize = true;
         SelectCamera_Lb.Location = new System.Drawing.Point(429, 213);
         SelectCamera_Lb.Name = "SelectCamera_Lb";
         SelectCamera_Lb.Size = new System.Drawing.Size(82, 14);
         SelectCamera_Lb.TabIndex = 12;
         SelectCamera_Lb.Text = "انتخاب دوربین :";
         // 
         // TC_Dcmt
         // 
         this.TC_Dcmt.Controls.Add(this.tp_001);
         this.TC_Dcmt.Controls.Add(this.tp_002);
         this.TC_Dcmt.Controls.Add(this.tp_003);
         this.TC_Dcmt.Dock = System.Windows.Forms.DockStyle.Fill;
         this.TC_Dcmt.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.TC_Dcmt.Location = new System.Drawing.Point(0, 47);
         this.TC_Dcmt.Name = "TC_Dcmt";
         this.TC_Dcmt.RightToLeftLayout = true;
         this.TC_Dcmt.SelectedIndex = 0;
         this.TC_Dcmt.Size = new System.Drawing.Size(893, 728);
         this.TC_Dcmt.TabIndex = 0;
         this.TC_Dcmt.SelectedIndexChanged += new System.EventHandler(this.TC_Dcmt_SelectedIndexChanged);
         // 
         // tp_001
         // 
         this.tp_001.AutoScroll = true;
         this.tp_001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.tp_001.Controls.Add(this.Dcmt_Gb);
         this.tp_001.Location = new System.Drawing.Point(4, 23);
         this.tp_001.Name = "tp_001";
         this.tp_001.Padding = new System.Windows.Forms.Padding(3);
         this.tp_001.Size = new System.Drawing.Size(885, 701);
         this.tp_001.TabIndex = 0;
         this.tp_001.Text = "مدارک و مجوزات";
         // 
         // Dcmt_Gb
         // 
         this.Dcmt_Gb.BackColor = System.Drawing.SystemColors.Control;
         this.Dcmt_Gb.Controls.Add(this.CB_ImageSize);
         this.Dcmt_Gb.Controls.Add(this.Bt_SelectFile);
         this.Dcmt_Gb.Controls.Add(PermStat_Lb);
         this.Dcmt_Gb.Controls.Add(FileName_Lb);
         this.Dcmt_Gb.Controls.Add(this.pERM_STATLookUpEdit);
         this.Dcmt_Gb.Controls.Add(this.FILE_NAME_TextBox);
         this.Dcmt_Gb.Controls.Add(RcdcStat_Lb);
         this.Dcmt_Gb.Controls.Add(Dimsn_Lb);
         this.Dcmt_Gb.Controls.Add(Rwno_Lb);
         this.Dcmt_Gb.Controls.Add(this.rCDC_STATLookUpEdit);
         this.Dcmt_Gb.Controls.Add(this.rWNOTextBox);
         this.Dcmt_Gb.Controls.Add(RcdcDesc_Lb);
         this.Dcmt_Gb.Controls.Add(this.rCDC_DESCTextBox);
         this.Dcmt_Gb.Controls.Add(DelvDate_Lb);
         this.Dcmt_Gb.Controls.Add(this.dELV_DATEPersianDateEdit);
         this.Dcmt_Gb.Controls.Add(EndDate_Lb);
         this.Dcmt_Gb.Controls.Add(this.eND_DATEPersianDateEdit);
         this.Dcmt_Gb.Controls.Add(StrtDate_Lb);
         this.Dcmt_Gb.Controls.Add(this.sTRT_DATEPersianDateEdit);
         this.Dcmt_Gb.Controls.Add(this.receive_DocumentGridControl);
         this.Dcmt_Gb.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Dcmt_Gb.Location = new System.Drawing.Point(3, 3);
         this.Dcmt_Gb.Name = "Dcmt_Gb";
         this.Dcmt_Gb.Size = new System.Drawing.Size(879, 695);
         this.Dcmt_Gb.TabIndex = 0;
         this.Dcmt_Gb.TabStop = false;
         this.Dcmt_Gb.Text = "مدارک و مجوزات درخواست";
         // 
         // CB_ImageSize
         // 
         this.CB_ImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.CB_ImageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.CB_ImageSize.FormattingEnabled = true;
         this.CB_ImageSize.Items.AddRange(new object[] {
            "عکس 3*4",
            "کارت ملی",
            "کاغذ A5",
            "کاغذ A4 ایستاده",
            "کاغذ A4 نشسته",
            "اندازه واقعی خود عکس"});
         this.CB_ImageSize.Location = new System.Drawing.Point(504, 125);
         this.CB_ImageSize.Name = "CB_ImageSize";
         this.CB_ImageSize.Size = new System.Drawing.Size(121, 22);
         this.CB_ImageSize.TabIndex = 13;
         // 
         // Bt_SelectFile
         // 
         this.Bt_SelectFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Bt_SelectFile.Location = new System.Drawing.Point(395, 124);
         this.Bt_SelectFile.Name = "Bt_SelectFile";
         this.Bt_SelectFile.Size = new System.Drawing.Size(109, 23);
         this.Bt_SelectFile.TabIndex = 5;
         this.Bt_SelectFile.Text = "انتخاب فایل مدرک";
         this.Bt_SelectFile.UseVisualStyleBackColor = true;
         this.Bt_SelectFile.Click += new System.EventHandler(this.Bt_SelectFile_Click);
         // 
         // pERM_STATLookUpEdit
         // 
         this.pERM_STATLookUpEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.pERM_STATLookUpEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.receive_DocumentBindingSource, "PERM_STAT", true));
         this.pERM_STATLookUpEdit.EditValue = "001";
         this.pERM_STATLookUpEdit.Location = new System.Drawing.Point(294, 47);
         this.pERM_STATLookUpEdit.Name = "pERM_STATLookUpEdit";
         this.pERM_STATLookUpEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.pERM_STATLookUpEdit.Properties.Appearance.Options.UseFont = true;
         this.pERM_STATLookUpEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.pERM_STATLookUpEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.pERM_STATLookUpEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.pERM_STATLookUpEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.pERM_STATLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.pERM_STATLookUpEdit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "تاییدیه مدرک", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.pERM_STATLookUpEdit.Properties.DataSource = this.dPRSTBindingSource;
         this.pERM_STATLookUpEdit.Properties.DisplayMember = "DOMN_DESC";
         this.pERM_STATLookUpEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.pERM_STATLookUpEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.pERM_STATLookUpEdit.Properties.NullText = "";
         this.pERM_STATLookUpEdit.Properties.ValueMember = "VALU";
         this.pERM_STATLookUpEdit.Size = new System.Drawing.Size(189, 22);
         this.pERM_STATLookUpEdit.TabIndex = 12;
         // 
         // receive_DocumentBindingSource
         // 
         this.receive_DocumentBindingSource.DataSource = typeof(System.Scsc.Data.Receive_Document);
         // 
         // dPRSTBindingSource
         // 
         this.dPRSTBindingSource.DataSource = typeof(System.Scsc.Data.D_PRST);
         // 
         // FILE_NAME_TextBox
         // 
         this.FILE_NAME_TextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.FILE_NAME_TextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ImdcBs1, "FILE_NAME", true));
         this.FILE_NAME_TextBox.Enabled = false;
         this.FILE_NAME_TextBox.Location = new System.Drawing.Point(395, 151);
         this.FILE_NAME_TextBox.Name = "FILE_NAME_TextBox";
         this.FILE_NAME_TextBox.Size = new System.Drawing.Size(365, 22);
         this.FILE_NAME_TextBox.TabIndex = 3;
         this.FILE_NAME_TextBox.Visible = false;
         // 
         // ImdcBs1
         // 
         this.ImdcBs1.DataMember = "Image_Documents";
         this.ImdcBs1.DataSource = this.receive_DocumentBindingSource;
         // 
         // rCDC_STATLookUpEdit
         // 
         this.rCDC_STATLookUpEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rCDC_STATLookUpEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.receive_DocumentBindingSource, "RCDC_STAT", true));
         this.rCDC_STATLookUpEdit.EditValue = "001";
         this.rCDC_STATLookUpEdit.Location = new System.Drawing.Point(294, 21);
         this.rCDC_STATLookUpEdit.Name = "rCDC_STATLookUpEdit";
         this.rCDC_STATLookUpEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.rCDC_STATLookUpEdit.Properties.Appearance.Options.UseFont = true;
         this.rCDC_STATLookUpEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.rCDC_STATLookUpEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.rCDC_STATLookUpEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.rCDC_STATLookUpEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.rCDC_STATLookUpEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
         this.rCDC_STATLookUpEdit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "وضعیت مدرک", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.rCDC_STATLookUpEdit.Properties.DataSource = this.dDCMTBindingSource;
         this.rCDC_STATLookUpEdit.Properties.DisplayMember = "DOMN_DESC";
         this.rCDC_STATLookUpEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.rCDC_STATLookUpEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rCDC_STATLookUpEdit.Properties.NullText = "";
         this.rCDC_STATLookUpEdit.Properties.ValueMember = "VALU";
         this.rCDC_STATLookUpEdit.Size = new System.Drawing.Size(189, 22);
         this.rCDC_STATLookUpEdit.TabIndex = 10;
         // 
         // dDCMTBindingSource
         // 
         this.dDCMTBindingSource.DataSource = typeof(System.Scsc.Data.D_DCMT);
         // 
         // rWNOTextBox
         // 
         this.rWNOTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rWNOTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.ImdcBs1, "RWNO", true));
         this.rWNOTextBox.Location = new System.Drawing.Point(728, 125);
         this.rWNOTextBox.Name = "rWNOTextBox";
         this.rWNOTextBox.ReadOnly = true;
         this.rWNOTextBox.Size = new System.Drawing.Size(32, 22);
         this.rWNOTextBox.TabIndex = 1;
         // 
         // rCDC_DESCTextBox
         // 
         this.rCDC_DESCTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rCDC_DESCTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.receive_DocumentBindingSource, "RCDC_DESC", true));
         this.rCDC_DESCTextBox.Location = new System.Drawing.Point(294, 99);
         this.rCDC_DESCTextBox.Name = "rCDC_DESCTextBox";
         this.rCDC_DESCTextBox.Size = new System.Drawing.Size(466, 22);
         this.rCDC_DESCTextBox.TabIndex = 8;
         // 
         // dELV_DATEPersianDateEdit
         // 
         this.dELV_DATEPersianDateEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.dELV_DATEPersianDateEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.receive_DocumentBindingSource, "DELV_DATE", true));
         this.dELV_DATEPersianDateEdit.EditValue = new System.DateTime(2016, 3, 29, 13, 30, 23, 436);
         this.dELV_DATEPersianDateEdit.Location = new System.Drawing.Point(573, 73);
         this.dELV_DATEPersianDateEdit.Name = "dELV_DATEPersianDateEdit";
         this.dELV_DATEPersianDateEdit.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.dELV_DATEPersianDateEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.dELV_DATEPersianDateEdit.Properties.Appearance.Options.UseBackColor = true;
         this.dELV_DATEPersianDateEdit.Properties.Appearance.Options.UseFont = true;
         this.dELV_DATEPersianDateEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.dELV_DATEPersianDateEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.dELV_DATEPersianDateEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.dELV_DATEPersianDateEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.dELV_DATEPersianDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, null, true)});
         this.dELV_DATEPersianDateEdit.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
         this.dELV_DATEPersianDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.dELV_DATEPersianDateEdit.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
         this.dELV_DATEPersianDateEdit.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
         this.dELV_DATEPersianDateEdit.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
         this.dELV_DATEPersianDateEdit.Properties.DisplayFormat.FormatString = "D";
         this.dELV_DATEPersianDateEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.dELV_DATEPersianDateEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.dELV_DATEPersianDateEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.dELV_DATEPersianDateEdit.Properties.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
         this.dELV_DATEPersianDateEdit.Properties.MinValue = new System.DateTime(622, 3, 21, 0, 0, 0, 0);
         this.dELV_DATEPersianDateEdit.Properties.ShowWeekNumbers = true;
         this.dELV_DATEPersianDateEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.dELV_DATEPersianDateEdit.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
         this.dELV_DATEPersianDateEdit.Size = new System.Drawing.Size(187, 22);
         this.dELV_DATEPersianDateEdit.TabIndex = 6;
         // 
         // eND_DATEPersianDateEdit
         // 
         this.eND_DATEPersianDateEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.eND_DATEPersianDateEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.receive_DocumentBindingSource, "END_DATE", true));
         this.eND_DATEPersianDateEdit.EditValue = new System.DateTime(2016, 3, 29, 13, 30, 15, 982);
         this.eND_DATEPersianDateEdit.Location = new System.Drawing.Point(573, 47);
         this.eND_DATEPersianDateEdit.Name = "eND_DATEPersianDateEdit";
         this.eND_DATEPersianDateEdit.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
         this.eND_DATEPersianDateEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.eND_DATEPersianDateEdit.Properties.Appearance.Options.UseBackColor = true;
         this.eND_DATEPersianDateEdit.Properties.Appearance.Options.UseFont = true;
         this.eND_DATEPersianDateEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.eND_DATEPersianDateEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.eND_DATEPersianDateEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.eND_DATEPersianDateEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.eND_DATEPersianDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true)});
         this.eND_DATEPersianDateEdit.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
         this.eND_DATEPersianDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.eND_DATEPersianDateEdit.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
         this.eND_DATEPersianDateEdit.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
         this.eND_DATEPersianDateEdit.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
         this.eND_DATEPersianDateEdit.Properties.DisplayFormat.FormatString = "D";
         this.eND_DATEPersianDateEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.eND_DATEPersianDateEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.eND_DATEPersianDateEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.eND_DATEPersianDateEdit.Properties.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
         this.eND_DATEPersianDateEdit.Properties.MinValue = new System.DateTime(622, 3, 21, 0, 0, 0, 0);
         this.eND_DATEPersianDateEdit.Properties.ShowWeekNumbers = true;
         this.eND_DATEPersianDateEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.eND_DATEPersianDateEdit.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
         this.eND_DATEPersianDateEdit.Size = new System.Drawing.Size(187, 22);
         this.eND_DATEPersianDateEdit.TabIndex = 4;
         // 
         // sTRT_DATEPersianDateEdit
         // 
         this.sTRT_DATEPersianDateEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.sTRT_DATEPersianDateEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.receive_DocumentBindingSource, "STRT_DATE", true));
         this.sTRT_DATEPersianDateEdit.EditValue = new System.DateTime(2016, 3, 29, 13, 29, 37, 0);
         this.sTRT_DATEPersianDateEdit.Location = new System.Drawing.Point(573, 21);
         this.sTRT_DATEPersianDateEdit.Name = "sTRT_DATEPersianDateEdit";
         this.sTRT_DATEPersianDateEdit.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         this.sTRT_DATEPersianDateEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.sTRT_DATEPersianDateEdit.Properties.Appearance.Options.UseBackColor = true;
         this.sTRT_DATEPersianDateEdit.Properties.Appearance.Options.UseFont = true;
         this.sTRT_DATEPersianDateEdit.Properties.Appearance.Options.UseTextOptions = true;
         this.sTRT_DATEPersianDateEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.sTRT_DATEPersianDateEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.sTRT_DATEPersianDateEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.sTRT_DATEPersianDateEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, true)});
         this.sTRT_DATEPersianDateEdit.Properties.CalendarTimeEditing = DevExpress.Utils.DefaultBoolean.True;
         this.sTRT_DATEPersianDateEdit.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.sTRT_DATEPersianDateEdit.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
         this.sTRT_DATEPersianDateEdit.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
         this.sTRT_DATEPersianDateEdit.Properties.CalendarView = DevExpress.XtraEditors.Repository.CalendarView.Vista;
         this.sTRT_DATEPersianDateEdit.Properties.DisplayFormat.FormatString = "D";
         this.sTRT_DATEPersianDateEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
         this.sTRT_DATEPersianDateEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.sTRT_DATEPersianDateEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sTRT_DATEPersianDateEdit.Properties.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
         this.sTRT_DATEPersianDateEdit.Properties.MinValue = new System.DateTime(622, 3, 21, 0, 0, 0, 0);
         this.sTRT_DATEPersianDateEdit.Properties.ShowWeekNumbers = true;
         this.sTRT_DATEPersianDateEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.sTRT_DATEPersianDateEdit.Properties.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
         this.sTRT_DATEPersianDateEdit.Size = new System.Drawing.Size(187, 22);
         this.sTRT_DATEPersianDateEdit.TabIndex = 2;
         // 
         // receive_DocumentGridControl
         // 
         this.receive_DocumentGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.receive_DocumentGridControl.DataSource = this.receive_DocumentBindingSource;
         this.receive_DocumentGridControl.Location = new System.Drawing.Point(9, 179);
         this.receive_DocumentGridControl.LookAndFeel.SkinName = "DevExpress Design";
         this.receive_DocumentGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.receive_DocumentGridControl.MainView = this.gridView1;
         this.receive_DocumentGridControl.Name = "receive_DocumentGridControl";
         this.receive_DocumentGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.NEED_LOV,
            this.ORIG_LOV,
            this.FRST_LOV});
         this.receive_DocumentGridControl.Size = new System.Drawing.Size(861, 510);
         this.receive_DocumentGridControl.TabIndex = 0;
         this.receive_DocumentGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
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
            this.RqtpCode_Clm,
            this.RqtpDesc_Clm,
            this.RqttCode_Clm,
            this.RqttDesc_Clm,
            this.DcmtDesc_Clm,
            this.NeedType_Clm,
            this.OrigType_Clm,
            this.FrstNeed_Clm});
         this.gridView1.GridControl = this.receive_DocumentGridControl;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsBehavior.Editable = false;
         this.gridView1.OptionsBehavior.ReadOnly = true;
         this.gridView1.OptionsDetail.EnableMasterViewMode = false;
         this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // RqtpCode_Clm
         // 
         this.RqtpCode_Clm.Caption = "کد نوع تقاضا";
         this.RqtpCode_Clm.FieldName = "Request_Document.Request_Requester.RQTP_CODE";
         this.RqtpCode_Clm.MaxWidth = 70;
         this.RqtpCode_Clm.Name = "RqtpCode_Clm";
         this.RqtpCode_Clm.Visible = true;
         this.RqtpCode_Clm.VisibleIndex = 7;
         this.RqtpCode_Clm.Width = 63;
         // 
         // RqtpDesc_Clm
         // 
         this.RqtpDesc_Clm.Caption = "شرح تقاضا";
         this.RqtpDesc_Clm.FieldName = "Request_Document.Request_Requester.Request_Type.RQTP_DESC";
         this.RqtpDesc_Clm.MaxWidth = 120;
         this.RqtpDesc_Clm.Name = "RqtpDesc_Clm";
         this.RqtpDesc_Clm.Visible = true;
         this.RqtpDesc_Clm.VisibleIndex = 6;
         this.RqtpDesc_Clm.Width = 86;
         // 
         // RqttCode_Clm
         // 
         this.RqttCode_Clm.Caption = "کد نوع متقاضی";
         this.RqttCode_Clm.FieldName = "Request_Document.Request_Requester.RQTT_CODE";
         this.RqttCode_Clm.MaxWidth = 90;
         this.RqttCode_Clm.Name = "RqttCode_Clm";
         this.RqttCode_Clm.Visible = true;
         this.RqttCode_Clm.VisibleIndex = 5;
         this.RqttCode_Clm.Width = 90;
         // 
         // RqttDesc_Clm
         // 
         this.RqttDesc_Clm.Caption = "شرح متقاضی";
         this.RqttDesc_Clm.FieldName = "Request_Document.Request_Requester.Requester_Type.RQTT_DESC";
         this.RqttDesc_Clm.MaxWidth = 120;
         this.RqttDesc_Clm.Name = "RqttDesc_Clm";
         this.RqttDesc_Clm.Visible = true;
         this.RqttDesc_Clm.VisibleIndex = 4;
         this.RqttDesc_Clm.Width = 93;
         // 
         // DcmtDesc_Clm
         // 
         this.DcmtDesc_Clm.Caption = "شرح مدرک";
         this.DcmtDesc_Clm.FieldName = "Request_Document.Document_Spec.DCMT_DESC";
         this.DcmtDesc_Clm.Name = "DcmtDesc_Clm";
         this.DcmtDesc_Clm.Visible = true;
         this.DcmtDesc_Clm.VisibleIndex = 3;
         // 
         // NeedType_Clm
         // 
         this.NeedType_Clm.Caption = "نوع نياز";
         this.NeedType_Clm.ColumnEdit = this.NEED_LOV;
         this.NeedType_Clm.FieldName = "Request_Document.NEED_TYPE";
         this.NeedType_Clm.MaxWidth = 70;
         this.NeedType_Clm.Name = "NeedType_Clm";
         this.NeedType_Clm.Visible = true;
         this.NeedType_Clm.VisibleIndex = 2;
         this.NeedType_Clm.Width = 70;
         // 
         // NEED_LOV
         // 
         this.NEED_LOV.AutoHeight = false;
         this.NEED_LOV.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.NEED_LOV.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "نوع نیاز", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.NEED_LOV.DataSource = this.dDCNDBindingSource;
         this.NEED_LOV.DisplayMember = "DOMN_DESC";
         this.NEED_LOV.Name = "NEED_LOV";
         this.NEED_LOV.ValueMember = "VALU";
         // 
         // dDCNDBindingSource
         // 
         this.dDCNDBindingSource.DataSource = typeof(System.Scsc.Data.D_DCND);
         // 
         // OrigType_Clm
         // 
         this.OrigType_Clm.Caption = "اصل / کپی";
         this.OrigType_Clm.ColumnEdit = this.ORIG_LOV;
         this.OrigType_Clm.FieldName = "Request_Document.ORIG_TYPE";
         this.OrigType_Clm.MaxWidth = 70;
         this.OrigType_Clm.Name = "OrigType_Clm";
         this.OrigType_Clm.Visible = true;
         this.OrigType_Clm.VisibleIndex = 1;
         this.OrigType_Clm.Width = 70;
         // 
         // ORIG_LOV
         // 
         this.ORIG_LOV.AutoHeight = false;
         this.ORIG_LOV.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.ORIG_LOV.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "اصل / کپی", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.ORIG_LOV.DataSource = this.dDCTPBindingSource;
         this.ORIG_LOV.DisplayMember = "DOMN_DESC";
         this.ORIG_LOV.Name = "ORIG_LOV";
         this.ORIG_LOV.ValueMember = "VALU";
         // 
         // dDCTPBindingSource
         // 
         this.dDCTPBindingSource.DataSource = typeof(System.Scsc.Data.D_DCTP);
         // 
         // FrstNeed_Clm
         // 
         this.FrstNeed_Clm.Caption = "نیاز در ابتدا";
         this.FrstNeed_Clm.ColumnEdit = this.FRST_LOV;
         this.FrstNeed_Clm.FieldName = "Request_Document.FRST_NEED";
         this.FrstNeed_Clm.MaxWidth = 70;
         this.FrstNeed_Clm.Name = "FrstNeed_Clm";
         this.FrstNeed_Clm.Visible = true;
         this.FrstNeed_Clm.VisibleIndex = 0;
         this.FrstNeed_Clm.Width = 70;
         // 
         // FRST_LOV
         // 
         this.FRST_LOV.AutoHeight = false;
         this.FRST_LOV.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.FRST_LOV.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "نیاز در ابتدا", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.FRST_LOV.DataSource = this.dYSNOBindingSource;
         this.FRST_LOV.DisplayMember = "DOMN_DESC";
         this.FRST_LOV.Name = "FRST_LOV";
         this.FRST_LOV.ValueMember = "VALU";
         // 
         // dYSNOBindingSource
         // 
         this.dYSNOBindingSource.DataSource = typeof(System.Scsc.Data.D_YSNO);
         // 
         // tp_002
         // 
         this.tp_002.AutoScroll = true;
         this.tp_002.AutoScrollMinSize = new System.Drawing.Size(822, 717);
         this.tp_002.BackColor = System.Drawing.SystemColors.Control;
         this.tp_002.Controls.Add(this.Image_Gb);
         this.tp_002.Location = new System.Drawing.Point(4, 23);
         this.tp_002.Name = "tp_002";
         this.tp_002.Padding = new System.Windows.Forms.Padding(3);
         this.tp_002.Size = new System.Drawing.Size(885, 701);
         this.tp_002.TabIndex = 2;
         this.tp_002.Text = "تصویر مدرک";
         // 
         // Image_Gb
         // 
         this.Image_Gb.Controls.Add(this.Btn_SetProfileImage);
         this.Image_Gb.Controls.Add(this.Bt_RemvImage);
         this.Image_Gb.Controls.Add(this.UD_Interpolation);
         this.Image_Gb.Controls.Add(ImageQulity_Lb);
         this.Image_Gb.Controls.Add(ImageAlign_Lb);
         this.Image_Gb.Controls.Add(this.ZC_ZoomImage);
         this.Image_Gb.Controls.Add(this.CB_Alignment);
         this.Image_Gb.Controls.Add(this.CB_AllowMouseDrag);
         this.Image_Gb.Controls.Add(this.CB_ShowScroll);
         this.Image_Gb.Controls.Add(this.PE_ImageShow);
         this.Image_Gb.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Image_Gb.Location = new System.Drawing.Point(3, 3);
         this.Image_Gb.Name = "Image_Gb";
         this.Image_Gb.Size = new System.Drawing.Size(862, 717);
         this.Image_Gb.TabIndex = 0;
         this.Image_Gb.TabStop = false;
         this.Image_Gb.Text = "تصویر مدرک";
         // 
         // Btn_SetProfileImage
         // 
         this.Btn_SetProfileImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.Btn_SetProfileImage.BackColor = System.Drawing.Color.Transparent;
         this.Btn_SetProfileImage.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_SetProfileImage.Caption = "";
         this.Btn_SetProfileImage.Disabled = false;
         this.Btn_SetProfileImage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Btn_SetProfileImage.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_SetProfileImage.ImageIndex = 0;
         this.Btn_SetProfileImage.ImageList = this.img;
         this.Btn_SetProfileImage.Location = new System.Drawing.Point(15, 47);
         this.Btn_SetProfileImage.Name = "Btn_SetProfileImage";
         this.Btn_SetProfileImage.Size = new System.Drawing.Size(50, 50);
         this.Btn_SetProfileImage.TabIndex = 11;
         this.Btn_SetProfileImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_SetProfileImage.TextColor = System.Drawing.Color.Empty;
         this.Btn_SetProfileImage.ToolDownFont = null;
         this.Btn_SetProfileImage.ToolUpFont = null;
         this.Btn_SetProfileImage.Click += new System.EventHandler(this.Btn_SetProfileImage_Click);
         // 
         // img
         // 
         this.img.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("img.ImageStream")));
         this.img.TransparentColor = System.Drawing.Color.Transparent;
         this.img.Images.SetKeyName(0, "IMAGE_1115.png");
         this.img.Images.SetKeyName(1, "IMAGE_1208.png");
         this.img.Images.SetKeyName(2, "IMAGE_1210.png");
         // 
         // Bt_RemvImage
         // 
         this.Bt_RemvImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.Bt_RemvImage.Location = new System.Drawing.Point(15, 17);
         this.Bt_RemvImage.Name = "Bt_RemvImage";
         this.Bt_RemvImage.Size = new System.Drawing.Size(75, 23);
         this.Bt_RemvImage.TabIndex = 10;
         this.Bt_RemvImage.Text = "حذف تصویر";
         this.Bt_RemvImage.UseVisualStyleBackColor = true;
         this.Bt_RemvImage.Click += new System.EventHandler(this.Bt_RemvImage_Click);
         // 
         // UD_Interpolation
         // 
         this.UD_Interpolation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.UD_Interpolation.Location = new System.Drawing.Point(628, 92);
         this.UD_Interpolation.Maximum = new decimal(new int[] {
            7,
            0,
            0,
            0});
         this.UD_Interpolation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.UD_Interpolation.Name = "UD_Interpolation";
         this.UD_Interpolation.Size = new System.Drawing.Size(63, 22);
         this.UD_Interpolation.TabIndex = 9;
         this.UD_Interpolation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
         this.UD_Interpolation.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.UD_Interpolation.ValueChanged += new System.EventHandler(this.UD_Interpolation_ValueChanged);
         // 
         // ZC_ZoomImage
         // 
         this.ZC_ZoomImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ZC_ZoomImage.EditValue = 100;
         this.ZC_ZoomImage.Location = new System.Drawing.Point(15, 103);
         this.ZC_ZoomImage.Name = "ZC_ZoomImage";
         this.ZC_ZoomImage.Properties.LookAndFeel.SkinName = "Office 2013";
         this.ZC_ZoomImage.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ZC_ZoomImage.Properties.Maximum = 1000;
         this.ZC_ZoomImage.Properties.Middle = 5;
         this.ZC_ZoomImage.Properties.Minimum = 1;
         this.ZC_ZoomImage.Properties.ScrollThumbStyle = DevExpress.XtraEditors.Repository.ScrollThumbStyle.ArrowDownRight;
         this.ZC_ZoomImage.Properties.ShowValueToolTip = true;
         this.ZC_ZoomImage.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.ZC_ZoomImage.Size = new System.Drawing.Size(373, 16);
         this.ZC_ZoomImage.TabIndex = 6;
         this.ZC_ZoomImage.Value = 100;
         this.ZC_ZoomImage.ValueChanged += new System.EventHandler(this.ZC_ZoomImage_ValueChanged);
         // 
         // CB_Alignment
         // 
         this.CB_Alignment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.CB_Alignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.CB_Alignment.FormattingEnabled = true;
         this.CB_Alignment.Items.AddRange(new object[] {
            "بالا چپ",
            "بالا مرکز",
            "بالا راست",
            "وسط چپ",
            "وسط مرکز",
            "وسط راست",
            "پایین چپ",
            "پایین مرکز",
            "پایین راست"});
         this.CB_Alignment.Location = new System.Drawing.Point(513, 65);
         this.CB_Alignment.Name = "CB_Alignment";
         this.CB_Alignment.Size = new System.Drawing.Size(148, 22);
         this.CB_Alignment.TabIndex = 3;
         this.CB_Alignment.SelectedIndexChanged += new System.EventHandler(this.CB_Alignment_SelectedIndexChanged);
         // 
         // CB_AllowMouseDrag
         // 
         this.CB_AllowMouseDrag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.CB_AllowMouseDrag.AutoSize = true;
         this.CB_AllowMouseDrag.Location = new System.Drawing.Point(512, 41);
         this.CB_AllowMouseDrag.Name = "CB_AllowMouseDrag";
         this.CB_AllowMouseDrag.Size = new System.Drawing.Size(333, 18);
         this.CB_AllowMouseDrag.TabIndex = 2;
         this.CB_AllowMouseDrag.Text = "آیا توسط Mouse بتوانید عمل جابجایی عکس را داشته باشید؟";
         this.CB_AllowMouseDrag.UseVisualStyleBackColor = true;
         this.CB_AllowMouseDrag.CheckedChanged += new System.EventHandler(this.CB_AllowMouseDrag_CheckedChanged);
         // 
         // CB_ShowScroll
         // 
         this.CB_ShowScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.CB_ShowScroll.AutoSize = true;
         this.CB_ShowScroll.Location = new System.Drawing.Point(636, 20);
         this.CB_ShowScroll.Name = "CB_ShowScroll";
         this.CB_ShowScroll.Size = new System.Drawing.Size(209, 18);
         this.CB_ShowScroll.TabIndex = 1;
         this.CB_ShowScroll.Text = "آیا منوی ScrollBar نمایش داده شود؟";
         this.CB_ShowScroll.UseVisualStyleBackColor = true;
         this.CB_ShowScroll.CheckedChanged += new System.EventHandler(this.CB_ShowScroll_CheckedChanged);
         // 
         // PE_ImageShow
         // 
         this.PE_ImageShow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PE_ImageShow.Location = new System.Drawing.Point(15, 125);
         this.PE_ImageShow.Name = "PE_ImageShow";
         this.PE_ImageShow.Properties.AllowScrollViaMouseDrag = false;
         this.PE_ImageShow.Properties.LookAndFeel.SkinName = "Office 2010 Blue";
         this.PE_ImageShow.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.PE_ImageShow.Size = new System.Drawing.Size(830, 583);
         this.PE_ImageShow.TabIndex = 0;
         // 
         // tp_003
         // 
         this.tp_003.AutoScroll = true;
         this.tp_003.AutoScrollMinSize = new System.Drawing.Size(771, 0);
         this.tp_003.BackColor = System.Drawing.SystemColors.Control;
         this.tp_003.Controls.Add(this.Btn_AcceptPicture);
         this.tp_003.Controls.Add(this.Btn_TakePicture);
         this.tp_003.Controls.Add(this.Npb_Face3x4Zone);
         this.tp_003.Controls.Add(this.Npb_FaceZone);
         this.tp_003.Controls.Add(this.Tb_StartStopVideo);
         this.tp_003.Controls.Add(this.LB_Result);
         this.tp_003.Controls.Add(SelectCamera_Lb);
         this.tp_003.Controls.Add(this.LOV_VideoSrc);
         this.tp_003.Controls.Add(this.Pb_Face3x4Zone);
         this.tp_003.Controls.Add(this.Pb_FaceZone);
         this.tp_003.Controls.Add(this.pb_destination);
         this.tp_003.Controls.Add(this.pb_source);
         this.tp_003.Location = new System.Drawing.Point(4, 23);
         this.tp_003.Name = "tp_003";
         this.tp_003.Padding = new System.Windows.Forms.Padding(3);
         this.tp_003.Size = new System.Drawing.Size(885, 701);
         this.tp_003.TabIndex = 1;
         this.tp_003.Text = "عکس فوری";
         // 
         // Btn_AcceptPicture
         // 
         this.Btn_AcceptPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Btn_AcceptPicture.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Btn_AcceptPicture.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Btn_AcceptPicture.Appearance.ForeColor = System.Drawing.Color.Black;
         this.Btn_AcceptPicture.Appearance.Options.UseBackColor = true;
         this.Btn_AcceptPicture.Appearance.Options.UseFont = true;
         this.Btn_AcceptPicture.Appearance.Options.UseForeColor = true;
         this.Btn_AcceptPicture.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.Btn_AcceptPicture.Image = global::System.Scsc.Properties.Resources.IMAGE_1115;
         this.Btn_AcceptPicture.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Btn_AcceptPicture.Location = new System.Drawing.Point(826, 295);
         this.Btn_AcceptPicture.LookAndFeel.SkinName = "Office 2010 Blue";
         this.Btn_AcceptPicture.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Btn_AcceptPicture.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Btn_AcceptPicture.Name = "Btn_AcceptPicture";
         this.Btn_AcceptPicture.Size = new System.Drawing.Size(53, 50);
         this.Btn_AcceptPicture.TabIndex = 16;
         this.Btn_AcceptPicture.Tag = "1";
         this.Btn_AcceptPicture.ToolTip = "تایید عکس پروفایل";
         this.Btn_AcceptPicture.Click += new System.EventHandler(this.Btn_AcceptPicture_Click);
         // 
         // Btn_TakePicture
         // 
         this.Btn_TakePicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Btn_TakePicture.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Btn_TakePicture.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Btn_TakePicture.Appearance.ForeColor = System.Drawing.Color.Black;
         this.Btn_TakePicture.Appearance.Options.UseBackColor = true;
         this.Btn_TakePicture.Appearance.Options.UseFont = true;
         this.Btn_TakePicture.Appearance.Options.UseForeColor = true;
         this.Btn_TakePicture.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.Btn_TakePicture.Image = global::System.Scsc.Properties.Resources.IMAGE_1209;
         this.Btn_TakePicture.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Btn_TakePicture.Location = new System.Drawing.Point(553, 295);
         this.Btn_TakePicture.LookAndFeel.SkinName = "Office 2010 Blue";
         this.Btn_TakePicture.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Btn_TakePicture.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Btn_TakePicture.Name = "Btn_TakePicture";
         this.Btn_TakePicture.Size = new System.Drawing.Size(53, 50);
         this.Btn_TakePicture.TabIndex = 16;
         this.Btn_TakePicture.Tag = "1";
         this.Btn_TakePicture.ToolTip = "گرفتن عکس";
         this.Btn_TakePicture.Click += new System.EventHandler(this.Btn_TakePicture_Click);
         // 
         // Npb_Face3x4Zone
         // 
         this.Npb_Face3x4Zone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Npb_Face3x4Zone.BackColor = System.Drawing.Color.Transparent;
         this.Npb_Face3x4Zone.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Npb_Face3x4Zone.Disabled = false;
         this.Npb_Face3x4Zone.First = this.Npb_FaceZone;
         this.Npb_Face3x4Zone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Npb_Face3x4Zone.ForceSelect = true;
         this.Npb_Face3x4Zone.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Npb_Face3x4Zone.ImageIndexPickDown = 0;
         this.Npb_Face3x4Zone.ImageIndexPickUp = 0;
         this.Npb_Face3x4Zone.ImageList = null;
         this.Npb_Face3x4Zone.Location = new System.Drawing.Point(334, 435);
         this.Npb_Face3x4Zone.Name = "Npb_Face3x4Zone";
         this.Npb_Face3x4Zone.Next = null;
         this.Npb_Face3x4Zone.PickChecked = true;
         this.Npb_Face3x4Zone.PickDownFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Npb_Face3x4Zone.PickDownText = null;
         this.Npb_Face3x4Zone.PickDownTextColor = System.Drawing.Color.Empty;
         this.Npb_Face3x4Zone.PickUpFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Npb_Face3x4Zone.PickUpText = null;
         this.Npb_Face3x4Zone.PickUpTextColor = System.Drawing.Color.Empty;
         this.Npb_Face3x4Zone.Size = new System.Drawing.Size(25, 25);
         this.Npb_Face3x4Zone.TabIndex = 15;
         this.Npb_Face3x4Zone.TextAligns = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Npb_FaceZone
         // 
         this.Npb_FaceZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Npb_FaceZone.BackColor = System.Drawing.Color.Transparent;
         this.Npb_FaceZone.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Npb_FaceZone.Disabled = false;
         this.Npb_FaceZone.First = this.Npb_FaceZone;
         this.Npb_FaceZone.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Npb_FaceZone.ForceSelect = true;
         this.Npb_FaceZone.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Npb_FaceZone.ImageIndexPickDown = 0;
         this.Npb_FaceZone.ImageIndexPickUp = 0;
         this.Npb_FaceZone.ImageList = null;
         this.Npb_FaceZone.Location = new System.Drawing.Point(215, 435);
         this.Npb_FaceZone.Name = "Npb_FaceZone";
         this.Npb_FaceZone.Next = this.Npb_Face3x4Zone;
         this.Npb_FaceZone.PickChecked = false;
         this.Npb_FaceZone.PickDownFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Npb_FaceZone.PickDownText = null;
         this.Npb_FaceZone.PickDownTextColor = System.Drawing.Color.Empty;
         this.Npb_FaceZone.PickUpFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Npb_FaceZone.PickUpText = null;
         this.Npb_FaceZone.PickUpTextColor = System.Drawing.Color.Empty;
         this.Npb_FaceZone.Size = new System.Drawing.Size(25, 25);
         this.Npb_FaceZone.TabIndex = 15;
         this.Npb_FaceZone.TextAligns = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Tb_StartStopVideo
         // 
         this.Tb_StartStopVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Tb_StartStopVideo.BackColor = System.Drawing.Color.Transparent;
         this.Tb_StartStopVideo.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Tb_StartStopVideo.Disabled = false;
         this.Tb_StartStopVideo.First = null;
         this.Tb_StartStopVideo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Tb_StartStopVideo.ForceSelect = false;
         this.Tb_StartStopVideo.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Tb_StartStopVideo.ImageIndexPickDown = 2;
         this.Tb_StartStopVideo.ImageIndexPickUp = 1;
         this.Tb_StartStopVideo.ImageList = this.img;
         this.Tb_StartStopVideo.Location = new System.Drawing.Point(215, 238);
         this.Tb_StartStopVideo.Name = "Tb_StartStopVideo";
         this.Tb_StartStopVideo.Next = null;
         this.Tb_StartStopVideo.PickChecked = false;
         this.Tb_StartStopVideo.PickDownFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Tb_StartStopVideo.PickDownText = "";
         this.Tb_StartStopVideo.PickDownTextColor = System.Drawing.Color.Empty;
         this.Tb_StartStopVideo.PickUpFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Tb_StartStopVideo.PickUpText = "";
         this.Tb_StartStopVideo.PickUpTextColor = System.Drawing.Color.Empty;
         this.Tb_StartStopVideo.Size = new System.Drawing.Size(65, 51);
         this.Tb_StartStopVideo.TabIndex = 13;
         this.Tb_StartStopVideo.TextAligns = System.Drawing.ContentAlignment.MiddleCenter;
         this.Tb_StartStopVideo.PickCheckedChange += new System.MaxUi.NewPickBtn.PickCheckedHandel(this.Tb_StartStopVideo_PickCheckedChange);
         // 
         // LB_Result
         // 
         this.LB_Result.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.LB_Result.AutoSize = true;
         this.LB_Result.Location = new System.Drawing.Point(286, 275);
         this.LB_Result.Name = "LB_Result";
         this.LB_Result.Size = new System.Drawing.Size(62, 14);
         this.LB_Result.TabIndex = 12;
         this.LB_Result.Text = "Get Signal";
         // 
         // LOV_VideoSrc
         // 
         this.LOV_VideoSrc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.LOV_VideoSrc.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.receive_DocumentBindingSource, "RCDC_STAT", true));
         this.LOV_VideoSrc.Location = new System.Drawing.Point(215, 210);
         this.LOV_VideoSrc.Name = "LOV_VideoSrc";
         this.LOV_VideoSrc.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LOV_VideoSrc.Properties.Appearance.Options.UseFont = true;
         this.LOV_VideoSrc.Properties.Appearance.Options.UseTextOptions = true;
         this.LOV_VideoSrc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.LOV_VideoSrc.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.LOV_VideoSrc.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.LOV_VideoSrc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, "", null, null, true)});
         this.LOV_VideoSrc.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MonikerString", "نام دستگاه"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name2", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
         this.LOV_VideoSrc.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.LOV_VideoSrc.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.LOV_VideoSrc.Properties.NullText = "";
         this.LOV_VideoSrc.Size = new System.Drawing.Size(207, 22);
         this.LOV_VideoSrc.TabIndex = 11;
         // 
         // Pb_Face3x4Zone
         // 
         this.Pb_Face3x4Zone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Pb_Face3x4Zone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Pb_Face3x4Zone.Location = new System.Drawing.Point(334, 295);
         this.Pb_Face3x4Zone.Name = "Pb_Face3x4Zone";
         this.Pb_Face3x4Zone.Size = new System.Drawing.Size(113, 134);
         this.Pb_Face3x4Zone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.Pb_Face3x4Zone.TabIndex = 0;
         this.Pb_Face3x4Zone.TabStop = false;
         // 
         // Pb_FaceZone
         // 
         this.Pb_FaceZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Pb_FaceZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Pb_FaceZone.Location = new System.Drawing.Point(215, 295);
         this.Pb_FaceZone.Name = "Pb_FaceZone";
         this.Pb_FaceZone.Size = new System.Drawing.Size(113, 134);
         this.Pb_FaceZone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.Pb_FaceZone.TabIndex = 0;
         this.Pb_FaceZone.TabStop = false;
         // 
         // pb_destination
         // 
         this.pb_destination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.pb_destination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.pb_destination.Location = new System.Drawing.Point(553, 6);
         this.pb_destination.Name = "pb_destination";
         this.pb_destination.Size = new System.Drawing.Size(326, 283);
         this.pb_destination.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pb_destination.TabIndex = 0;
         this.pb_destination.TabStop = false;
         // 
         // pb_source
         // 
         this.pb_source.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.pb_source.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.pb_source.Location = new System.Drawing.Point(215, 6);
         this.pb_source.Name = "pb_source";
         this.pb_source.Size = new System.Drawing.Size(326, 198);
         this.pb_source.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pb_source.TabIndex = 0;
         this.pb_source.TabStop = false;
         // 
         // DG_SelectImage
         // 
         this.DG_SelectImage.FileName = "openFileDialog1";
         // 
         // Tm_NewFrameProcess
         // 
         this.Tm_NewFrameProcess.Tick += new System.EventHandler(this.Tm_NewFrameProcess_Tick);
         // 
         // RqstBn1
         // 
         this.RqstBn1.AddNewItem = this.RqstBnNew1;
         this.RqstBn1.BackColor = System.Drawing.Color.Khaki;
         this.RqstBn1.BindingSource = this.receive_DocumentBindingSource;
         this.RqstBn1.CountItem = this.toolStripLabel1;
         this.RqstBn1.DeleteItem = null;
         this.RqstBn1.ImageScalingSize = new System.Drawing.Size(40, 40);
         this.RqstBn1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripTextBox1,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator3,
            this.RqstBnNew1,
            this.RqstBnDelete1,
            this.RqstBnARqt1,
            this.RqstMBnDefaultPrint1,
            this.RqstBnASav1,
            this.RqstBnAResn1,
            this.RqstBnADoc1,
            this.RqstBnRegl01,
            this.RqstBnExit1});
         this.RqstBn1.Location = new System.Drawing.Point(0, 0);
         this.RqstBn1.MoveFirstItem = this.toolStripButton1;
         this.RqstBn1.MoveLastItem = this.toolStripButton4;
         this.RqstBn1.MoveNextItem = this.toolStripButton3;
         this.RqstBn1.MovePreviousItem = this.toolStripButton2;
         this.RqstBn1.Name = "RqstBn1";
         this.RqstBn1.PositionItem = this.toolStripTextBox1;
         this.RqstBn1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
         this.RqstBn1.Size = new System.Drawing.Size(893, 47);
         this.RqstBn1.TabIndex = 26;
         this.RqstBn1.Text = "bindingNavigator1";
         // 
         // RqstBnNew1
         // 
         this.RqstBnNew1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnNew1.Enabled = false;
         this.RqstBnNew1.Image = global::System.Scsc.Properties.Resources.IMAGE_1054;
         this.RqstBnNew1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
         this.RqstBnNew1.Name = "RqstBnNew1";
         this.RqstBnNew1.RightToLeftAutoMirrorImage = true;
         this.RqstBnNew1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnNew1.Text = "Add new";
         this.RqstBnNew1.ToolTipText = "ثبت ورودی جدید";
         this.RqstBnNew1.Visible = false;
         // 
         // toolStripLabel1
         // 
         this.toolStripLabel1.Name = "toolStripLabel1";
         this.toolStripLabel1.Size = new System.Drawing.Size(35, 44);
         this.toolStripLabel1.Text = "of {0}";
         this.toolStripLabel1.ToolTipText = "Total number of items";
         // 
         // toolStripButton1
         // 
         this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.toolStripButton1.Image = global::System.Scsc.Properties.Resources.IMAGE_1062;
         this.toolStripButton1.Name = "toolStripButton1";
         this.toolStripButton1.RightToLeftAutoMirrorImage = true;
         this.toolStripButton1.Size = new System.Drawing.Size(44, 44);
         this.toolStripButton1.Text = "Move first";
         // 
         // toolStripButton2
         // 
         this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.toolStripButton2.Image = global::System.Scsc.Properties.Resources.IMAGE_1060;
         this.toolStripButton2.Name = "toolStripButton2";
         this.toolStripButton2.RightToLeftAutoMirrorImage = true;
         this.toolStripButton2.Size = new System.Drawing.Size(44, 44);
         this.toolStripButton2.Text = "Move previous";
         // 
         // toolStripSeparator1
         // 
         this.toolStripSeparator1.Name = "toolStripSeparator1";
         this.toolStripSeparator1.Size = new System.Drawing.Size(6, 47);
         // 
         // toolStripTextBox1
         // 
         this.toolStripTextBox1.AccessibleName = "Position";
         this.toolStripTextBox1.AutoSize = false;
         this.toolStripTextBox1.BackColor = System.Drawing.Color.Khaki;
         this.toolStripTextBox1.Name = "toolStripTextBox1";
         this.toolStripTextBox1.Size = new System.Drawing.Size(50, 23);
         this.toolStripTextBox1.Text = "0";
         this.toolStripTextBox1.ToolTipText = "Current position";
         // 
         // toolStripSeparator2
         // 
         this.toolStripSeparator2.Name = "toolStripSeparator2";
         this.toolStripSeparator2.Size = new System.Drawing.Size(6, 47);
         // 
         // toolStripButton3
         // 
         this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.toolStripButton3.Image = global::System.Scsc.Properties.Resources.IMAGE_1061;
         this.toolStripButton3.Name = "toolStripButton3";
         this.toolStripButton3.RightToLeftAutoMirrorImage = true;
         this.toolStripButton3.Size = new System.Drawing.Size(44, 44);
         this.toolStripButton3.Text = "Move next";
         // 
         // toolStripButton4
         // 
         this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.toolStripButton4.Image = global::System.Scsc.Properties.Resources.IMAGE_1063;
         this.toolStripButton4.Name = "toolStripButton4";
         this.toolStripButton4.RightToLeftAutoMirrorImage = true;
         this.toolStripButton4.Size = new System.Drawing.Size(44, 44);
         this.toolStripButton4.Text = "Move last";
         // 
         // toolStripSeparator3
         // 
         this.toolStripSeparator3.Name = "toolStripSeparator3";
         this.toolStripSeparator3.Size = new System.Drawing.Size(6, 47);
         // 
         // RqstBnDelete1
         // 
         this.RqstBnDelete1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnDelete1.Enabled = false;
         this.RqstBnDelete1.Image = global::System.Scsc.Properties.Resources.IMAGE_1057;
         this.RqstBnDelete1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
         this.RqstBnDelete1.Name = "RqstBnDelete1";
         this.RqstBnDelete1.RightToLeftAutoMirrorImage = true;
         this.RqstBnDelete1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnDelete1.Text = "Delete";
         this.RqstBnDelete1.ToolTipText = "انصراف ثبت نام";
         this.RqstBnDelete1.Visible = false;
         // 
         // RqstBnARqt1
         // 
         this.RqstBnARqt1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnARqt1.Image = global::System.Scsc.Properties.Resources.IMAGE_1144;
         this.RqstBnARqt1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
         this.RqstBnARqt1.Name = "RqstBnARqt1";
         this.RqstBnARqt1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnARqt1.Text = "Save Data";
         this.RqstBnARqt1.ToolTipText = "ثبت مجدد اطلاعات";
         this.RqstBnARqt1.Click += new System.EventHandler(this.receive_DocumentBindingNavigatorSaveItem_Click);
         // 
         // RqstMBnDefaultPrint1
         // 
         this.RqstMBnDefaultPrint1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstMBnDefaultPrint1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RqstBnDefaultPrint1,
            this.RqstBnPrint1,
            this.toolStripMenuItem1,
            this.RqstBnSettingPrint1});
         this.RqstMBnDefaultPrint1.Image = global::System.Scsc.Properties.Resources.IMAGE_1059;
         this.RqstMBnDefaultPrint1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.RqstMBnDefaultPrint1.Name = "RqstMBnDefaultPrint1";
         this.RqstMBnDefaultPrint1.Size = new System.Drawing.Size(56, 44);
         this.RqstMBnDefaultPrint1.Text = "چاپ پیش فرض";
         // 
         // RqstBnDefaultPrint1
         // 
         this.RqstBnDefaultPrint1.Image = global::System.Scsc.Properties.Resources.IMAGE_1059;
         this.RqstBnDefaultPrint1.Name = "RqstBnDefaultPrint1";
         this.RqstBnDefaultPrint1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
         this.RqstBnDefaultPrint1.Size = new System.Drawing.Size(203, 22);
         this.RqstBnDefaultPrint1.Text = "چاپ پیش فرض";
         // 
         // RqstBnPrint1
         // 
         this.RqstBnPrint1.Image = global::System.Scsc.Properties.Resources.IMAGE_1090;
         this.RqstBnPrint1.Name = "RqstBnPrint1";
         this.RqstBnPrint1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.P)));
         this.RqstBnPrint1.Size = new System.Drawing.Size(203, 22);
         this.RqstBnPrint1.Text = "انتخاب چاپ";
         // 
         // toolStripMenuItem1
         // 
         this.toolStripMenuItem1.Name = "toolStripMenuItem1";
         this.toolStripMenuItem1.Size = new System.Drawing.Size(200, 6);
         // 
         // RqstBnSettingPrint1
         // 
         this.RqstBnSettingPrint1.Image = global::System.Scsc.Properties.Resources.IMAGE_1091;
         this.RqstBnSettingPrint1.Name = "RqstBnSettingPrint1";
         this.RqstBnSettingPrint1.Size = new System.Drawing.Size(203, 22);
         this.RqstBnSettingPrint1.Text = "تنظیمات چاپ";
         // 
         // RqstBnASav1
         // 
         this.RqstBnASav1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnASav1.Enabled = false;
         this.RqstBnASav1.Image = global::System.Scsc.Properties.Resources.IMAGE_1056;
         this.RqstBnASav1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.RqstBnASav1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
         this.RqstBnASav1.Name = "RqstBnASav1";
         this.RqstBnASav1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnASav1.Text = "toolStripButton1";
         this.RqstBnASav1.ToolTipText = "ذخیره نهایی";
         this.RqstBnASav1.Visible = false;
         // 
         // RqstBnAResn1
         // 
         this.RqstBnAResn1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnAResn1.Enabled = false;
         this.RqstBnAResn1.Image = global::System.Scsc.Properties.Resources.IMAGE_1116;
         this.RqstBnAResn1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.RqstBnAResn1.Name = "RqstBnAResn1";
         this.RqstBnAResn1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnAResn1.Text = "اسناد و مدارک مورد نیاز";
         this.RqstBnAResn1.Visible = false;
         // 
         // RqstBnADoc1
         // 
         this.RqstBnADoc1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnADoc1.Enabled = false;
         this.RqstBnADoc1.Image = global::System.Scsc.Properties.Resources.IMAGE_1086;
         this.RqstBnADoc1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.RqstBnADoc1.Name = "RqstBnADoc1";
         this.RqstBnADoc1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnADoc1.Text = "toolStripButton1";
         this.RqstBnADoc1.Visible = false;
         // 
         // RqstBnRegl01
         // 
         this.RqstBnRegl01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnRegl01.Enabled = false;
         this.RqstBnRegl01.Image = global::System.Scsc.Properties.Resources.IMAGE_1075;
         this.RqstBnRegl01.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.RqstBnRegl01.Name = "RqstBnRegl01";
         this.RqstBnRegl01.Size = new System.Drawing.Size(44, 44);
         this.RqstBnRegl01.Text = "toolStripButton1";
         this.RqstBnRegl01.Visible = false;
         // 
         // RqstBnExit1
         // 
         this.RqstBnExit1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.RqstBnExit1.Image = global::System.Scsc.Properties.Resources.IMAGE_1058;
         this.RqstBnExit1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.RqstBnExit1.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
         this.RqstBnExit1.Name = "RqstBnExit1";
         this.RqstBnExit1.Size = new System.Drawing.Size(44, 44);
         this.RqstBnExit1.Text = "toolStripButton3";
         this.RqstBnExit1.ToolTipText = "خروج";
         this.RqstBnExit1.Click += new System.EventHandler(this.mb_back_Click);
         // 
         // CMN_DCMT_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.TC_Dcmt);
         this.Controls.Add(this.RqstBn1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "CMN_DCMT_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(893, 775);
         this.TC_Dcmt.ResumeLayout(false);
         this.tp_001.ResumeLayout(false);
         this.Dcmt_Gb.ResumeLayout(false);
         this.Dcmt_Gb.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.pERM_STATLookUpEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.receive_DocumentBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dPRSTBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ImdcBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rCDC_STATLookUpEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dDCMTBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dELV_DATEPersianDateEdit.Properties.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dELV_DATEPersianDateEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.eND_DATEPersianDateEdit.Properties.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.eND_DATEPersianDateEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sTRT_DATEPersianDateEdit.Properties.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.sTRT_DATEPersianDateEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.receive_DocumentGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.NEED_LOV)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dDCNDBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ORIG_LOV)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dDCTPBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.FRST_LOV)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.dYSNOBindingSource)).EndInit();
         this.tp_002.ResumeLayout(false);
         this.Image_Gb.ResumeLayout(false);
         this.Image_Gb.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.UD_Interpolation)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ZC_ZoomImage.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.ZC_ZoomImage)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.PE_ImageShow.Properties)).EndInit();
         this.tp_003.ResumeLayout(false);
         this.tp_003.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.LOV_VideoSrc.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pb_Face3x4Zone)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pb_FaceZone)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_destination)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_source)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RqstBn1)).EndInit();
         this.RqstBn1.ResumeLayout(false);
         this.RqstBn1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.TabControl TC_Dcmt;
      private Windows.Forms.TabPage tp_001;
      private Windows.Forms.TabPage tp_003;
      private Windows.Forms.GroupBox Dcmt_Gb;
      private DevExpress.XtraGrid.GridControl receive_DocumentGridControl;
      private Windows.Forms.BindingSource receive_DocumentBindingSource;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn RqtpCode_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn RqtpDesc_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn RqttCode_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn RqttDesc_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn DcmtDesc_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn NeedType_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn OrigType_Clm;
      private DevExpress.XtraGrid.Columns.GridColumn FrstNeed_Clm;
      private Windows.Forms.TextBox rCDC_DESCTextBox;
      private dxExample.PersianDateEdit dELV_DATEPersianDateEdit;
      private dxExample.PersianDateEdit eND_DATEPersianDateEdit;
      private dxExample.PersianDateEdit sTRT_DATEPersianDateEdit;
      private DevExpress.XtraEditors.LookUpEdit pERM_STATLookUpEdit;
      private DevExpress.XtraEditors.LookUpEdit rCDC_STATLookUpEdit;
      private Windows.Forms.TextBox FILE_NAME_TextBox;
      private Windows.Forms.BindingSource ImdcBs1;
      private Windows.Forms.TextBox rWNOTextBox;
      private Windows.Forms.TabPage tp_002;
      private Windows.Forms.GroupBox Image_Gb;
      private DevExpress.XtraEditors.PictureEdit PE_ImageShow;
      private DevExpress.XtraEditors.ZoomTrackBarControl ZC_ZoomImage;
      private Windows.Forms.ComboBox CB_Alignment;
      private Windows.Forms.CheckBox CB_AllowMouseDrag;
      private Windows.Forms.CheckBox CB_ShowScroll;
      private Windows.Forms.Button Bt_SelectFile;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit NEED_LOV;
      private Windows.Forms.BindingSource dDCNDBindingSource;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ORIG_LOV;
      private Windows.Forms.BindingSource dDCTPBindingSource;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit FRST_LOV;
      private Windows.Forms.BindingSource dYSNOBindingSource;
      private Windows.Forms.BindingSource dPRSTBindingSource;
      private Windows.Forms.BindingSource dDCMTBindingSource;
      private Windows.Forms.OpenFileDialog DG_SelectImage;
      private Windows.Forms.NumericUpDown UD_Interpolation;
      private Windows.Forms.Button Bt_RemvImage;
      private Windows.Forms.ComboBox CB_ImageSize;
      private MaxUi.NewToolBtn Btn_SetProfileImage;
      private Windows.Forms.ImageList img;
      private Windows.Forms.PictureBox pb_destination;
      private Windows.Forms.PictureBox pb_source;
      private DevExpress.XtraEditors.LookUpEdit LOV_VideoSrc;
      private MaxUi.NewPickBtn Tb_StartStopVideo;
      private Windows.Forms.Timer Tm_NewFrameProcess;
      private Windows.Forms.Label LB_Result;
      private Windows.Forms.PictureBox Pb_FaceZone;
      private Windows.Forms.PictureBox Pb_Face3x4Zone;
      private MaxUi.NewPickBtn Npb_Face3x4Zone;
      private MaxUi.NewPickBtn Npb_FaceZone;
      private DevExpress.XtraEditors.SimpleButton Btn_AcceptPicture;
      private DevExpress.XtraEditors.SimpleButton Btn_TakePicture;
      private Windows.Forms.BindingNavigator RqstBn1;
      private Windows.Forms.ToolStripButton RqstBnNew1;
      private Windows.Forms.ToolStripLabel toolStripLabel1;
      private Windows.Forms.ToolStripButton toolStripButton1;
      private Windows.Forms.ToolStripButton toolStripButton2;
      private Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private Windows.Forms.ToolStripTextBox toolStripTextBox1;
      private Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private Windows.Forms.ToolStripButton toolStripButton3;
      private Windows.Forms.ToolStripButton toolStripButton4;
      private Windows.Forms.ToolStripSeparator toolStripSeparator3;
      private Windows.Forms.ToolStripButton RqstBnDelete1;
      private Windows.Forms.ToolStripButton RqstBnARqt1;
      private Windows.Forms.ToolStripSplitButton RqstMBnDefaultPrint1;
      private Windows.Forms.ToolStripMenuItem RqstBnDefaultPrint1;
      private Windows.Forms.ToolStripMenuItem RqstBnPrint1;
      private Windows.Forms.ToolStripSeparator toolStripMenuItem1;
      private Windows.Forms.ToolStripMenuItem RqstBnSettingPrint1;
      private Windows.Forms.ToolStripButton RqstBnASav1;
      private Windows.Forms.ToolStripButton RqstBnAResn1;
      private Windows.Forms.ToolStripButton RqstBnADoc1;
      private Windows.Forms.ToolStripButton RqstBnRegl01;
      private Windows.Forms.ToolStripButton RqstBnExit1;
   }
}
