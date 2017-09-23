namespace System.ISP.Ui.Payment
{
   partial class PAY_MTOD_F
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
         System.Windows.Forms.Label cASH_CODELabel;
         System.Windows.Forms.Label label1;
         System.Windows.Forms.Label label2;
         System.Windows.Forms.Label label3;
         System.Windows.Forms.Label label4;
         System.Windows.Forms.Label tERM_NOLabel;
         System.Windows.Forms.Label tRAN_NOLabel;
         System.Windows.Forms.Label cARD_NOLabel;
         System.Windows.Forms.Label bANKLabel;
         System.Windows.Forms.Label fLOW_NOLabel;
         System.Windows.Forms.Label rEF_NOLabel;
         System.Windows.Forms.Label label5;
         System.Windows.Forms.Label label6;
         System.Windows.Forms.Label label13;
         System.Windows.Forms.Label label11;
         System.Windows.Forms.Label label9;
         System.Windows.Forms.Label label12;
         System.Windows.Forms.Label label10;
         System.Windows.Forms.Label label7;
         DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState1 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
         DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState2 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
         DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState3 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
         DevExpress.XtraGauges.Core.Model.IndicatorState indicatorState4 = new DevExpress.XtraGauges.Core.Model.IndicatorState();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition1 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.XtraGrid.StyleFormatCondition styleFormatCondition2 = new DevExpress.XtraGrid.StyleFormatCondition();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         System.Windows.Forms.Label label8;
         this.colRWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRWNO1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.StatusSaving_Gc = new DevExpress.XtraGauges.Win.GaugeControl();
         this.stateIndicatorGauge1 = new DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorGauge();
         this.StatusSaving_Sic = new DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorComponent();
         this.Btn_Back = new C1.Win.C1Input.C1Button();
         this.c1DockingTab1 = new C1.Win.C1Command.C1DockingTab();
         this.c1DockingTabPage1 = new C1.Win.C1Command.C1DockingTabPage();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.c1DockingTab2 = new C1.Win.C1Command.C1DockingTab();
         this.c1DockingTabPage2 = new C1.Win.C1Command.C1DockingTabPage();
         this.DelPmmt_Butn = new C1.Win.C1Input.C1Button();
         this.SavePmmt_Butn = new C1.Win.C1Input.C1Button();
         this.AddPmmt_Butn = new C1.Win.C1Input.C1Button();
         this.comboBox1 = new System.Windows.Forms.ComboBox();
         this.PmmtBs1 = new System.Windows.Forms.BindingSource();
         this.PymtBs1 = new System.Windows.Forms.BindingSource();
         this.DrcmtBs1 = new System.Windows.Forms.BindingSource();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.StrtDate_Dat = new Atf.UI.DateTimeSelector();
         this.rEF_NOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.fLOW_NOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.bANKTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.cARD_NOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.tRAN_NOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.tERM_NOTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.PmmtGC = new DevExpress.XtraGrid.GridControl();
         this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colPYMT_CASH_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colPYMT_RQST_RQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQRO_RQST_RQID = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQRO_RWNO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colAMNT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRCPT_MTOD = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Rcmt_Lov = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.colTERM_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colTRAN_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCARD_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colBANK = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colFLOW_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colREF_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colACTN_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colSHOP_NO = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequest_Row = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colPayment = new DevExpress.XtraGrid.Columns.GridColumn();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.Te_TotlRemnAmnt = new DevExpress.XtraEditors.TextEdit();
         this.Te_TotlDebtAmnt = new DevExpress.XtraEditors.TextEdit();
         this.cASH_CODEComboBox = new System.Windows.Forms.ComboBox();
         this.CashBs1 = new System.Windows.Forms.BindingSource();
         this.c1DockingTab3 = new C1.Win.C1Command.C1DockingTab();
         this.c1DockingTabPage3 = new C1.Win.C1Command.C1DockingTabPage();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.PydsBs2 = new System.Windows.Forms.BindingSource();
         this.DelPyds_Butn = new C1.Win.C1Input.C1Button();
         this.SavePyds_Butn = new C1.Win.C1Input.C1Button();
         this.payment_DiscountsGridControl = new DevExpress.XtraGrid.GridControl();
         this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
         this.colPYMT_CASH_CODE1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colPYMT_RQST_RQID1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRQRO_RWNO1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colEXPN_CODE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colAMNT1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colAMNT_TYPE = new DevExpress.XtraGrid.Columns.GridColumn();
         this.Pyds_Lov = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
         this.DpydsBs1 = new System.Windows.Forms.BindingSource();
         this.colSTAT = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colPYDS_DESC = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_BY1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colCRET_DATE1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_BY1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colMDFY_DATE1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colRequest_Row1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colExpense = new DevExpress.XtraGrid.Columns.GridColumn();
         this.colPayment1 = new DevExpress.XtraGrid.Columns.GridColumn();
         this.AddPyds_Butn = new C1.Win.C1Input.C1Button();
         this.GB_DebtStat = new System.Windows.Forms.GroupBox();
         this.DEBT_DNRMTextEdit = new DevExpress.XtraEditors.TextEdit();
         this.AddDebtDiscount_Butn002 = new C1.Win.C1Input.C1Button();
         this.CashByDeposit_Txt002 = new DevExpress.XtraEditors.TextEdit();
         this.RemindAmnt_Txt002 = new DevExpress.XtraEditors.TextEdit();
         this.dEBT_DNRMLabel = new System.Windows.Forms.Label();
         this.comboBox2 = new System.Windows.Forms.ComboBox();
         cASH_CODELabel = new System.Windows.Forms.Label();
         label1 = new System.Windows.Forms.Label();
         label2 = new System.Windows.Forms.Label();
         label3 = new System.Windows.Forms.Label();
         label4 = new System.Windows.Forms.Label();
         tERM_NOLabel = new System.Windows.Forms.Label();
         tRAN_NOLabel = new System.Windows.Forms.Label();
         cARD_NOLabel = new System.Windows.Forms.Label();
         bANKLabel = new System.Windows.Forms.Label();
         fLOW_NOLabel = new System.Windows.Forms.Label();
         rEF_NOLabel = new System.Windows.Forms.Label();
         label5 = new System.Windows.Forms.Label();
         label6 = new System.Windows.Forms.Label();
         label13 = new System.Windows.Forms.Label();
         label11 = new System.Windows.Forms.Label();
         label9 = new System.Windows.Forms.Label();
         label12 = new System.Windows.Forms.Label();
         label10 = new System.Windows.Forms.Label();
         label7 = new System.Windows.Forms.Label();
         label8 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.stateIndicatorGauge1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.StatusSaving_Sic)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab1)).BeginInit();
         this.c1DockingTab1.SuspendLayout();
         this.c1DockingTabPage1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab2)).BeginInit();
         this.c1DockingTab2.SuspendLayout();
         this.c1DockingTabPage2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.DelPmmt_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.SavePmmt_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.AddPmmt_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.PmmtBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.PymtBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DrcmtBs1)).BeginInit();
         this.groupBox2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rEF_NOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.fLOW_NOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.bANKTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cARD_NOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tRAN_NOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tERM_NOTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.PmmtGC)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Rcmt_Lov)).BeginInit();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Te_TotlRemnAmnt.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Te_TotlDebtAmnt.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.CashBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab3)).BeginInit();
         this.c1DockingTab3.SuspendLayout();
         this.c1DockingTabPage3.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.PydsBs2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DelPyds_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.SavePyds_Butn)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.payment_DiscountsGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pyds_Lov)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.DpydsBs1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.AddPyds_Butn)).BeginInit();
         this.GB_DebtStat.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.DEBT_DNRMTextEdit.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.AddDebtDiscount_Butn002)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.CashByDeposit_Txt002.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.RemindAmnt_Txt002.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // cASH_CODELabel
         // 
         cASH_CODELabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         cASH_CODELabel.AutoSize = true;
         cASH_CODELabel.Location = new System.Drawing.Point(353, 28);
         cASH_CODELabel.Name = "cASH_CODELabel";
         cASH_CODELabel.Size = new System.Drawing.Size(65, 14);
         cASH_CODELabel.TabIndex = 0;
         cASH_CODELabel.Text = "پرداخت به :";
         // 
         // label1
         // 
         label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         label1.AutoSize = true;
         label1.Location = new System.Drawing.Point(353, 56);
         label1.Name = "label1";
         label1.Size = new System.Drawing.Size(55, 14);
         label1.TabIndex = 0;
         label1.Text = "کل مبلغ :";
         // 
         // label2
         // 
         label2.AutoSize = true;
         label2.Location = new System.Drawing.Point(78, 56);
         label2.Name = "label2";
         label2.Size = new System.Drawing.Size(44, 14);
         label2.TabIndex = 0;
         label2.Text = "( ریال )";
         // 
         // label3
         // 
         label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         label3.AutoSize = true;
         label3.Location = new System.Drawing.Point(353, 82);
         label3.Name = "label3";
         label3.Size = new System.Drawing.Size(83, 14);
         label3.TabIndex = 0;
         label3.Text = "مبلغ باقیمانده :";
         // 
         // label4
         // 
         label4.AutoSize = true;
         label4.Location = new System.Drawing.Point(78, 82);
         label4.Name = "label4";
         label4.Size = new System.Drawing.Size(44, 14);
         label4.TabIndex = 0;
         label4.Text = "( ریال )";
         // 
         // tERM_NOLabel
         // 
         tERM_NOLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         tERM_NOLabel.AutoSize = true;
         tERM_NOLabel.Location = new System.Drawing.Point(328, 33);
         tERM_NOLabel.Name = "tERM_NOLabel";
         tERM_NOLabel.Size = new System.Drawing.Size(86, 14);
         tERM_NOLabel.TabIndex = 0;
         tERM_NOLabel.Text = "شماره ترمینال :";
         // 
         // tRAN_NOLabel
         // 
         tRAN_NOLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         tRAN_NOLabel.AutoSize = true;
         tRAN_NOLabel.Location = new System.Drawing.Point(328, 59);
         tRAN_NOLabel.Name = "tRAN_NOLabel";
         tRAN_NOLabel.Size = new System.Drawing.Size(89, 14);
         tRAN_NOLabel.TabIndex = 2;
         tRAN_NOLabel.Text = "شماره تراکنش :";
         // 
         // cARD_NOLabel
         // 
         cARD_NOLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         cARD_NOLabel.AutoSize = true;
         cARD_NOLabel.Location = new System.Drawing.Point(328, 85);
         cARD_NOLabel.Name = "cARD_NOLabel";
         cARD_NOLabel.Size = new System.Drawing.Size(76, 14);
         cARD_NOLabel.TabIndex = 4;
         cARD_NOLabel.Text = "شماره کارت :";
         // 
         // bANKLabel
         // 
         bANKLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         bANKLabel.AutoSize = true;
         bANKLabel.Location = new System.Drawing.Point(328, 111);
         bANKLabel.Name = "bANKLabel";
         bANKLabel.Size = new System.Drawing.Size(37, 14);
         bANKLabel.TabIndex = 6;
         bANKLabel.Text = "بانک :";
         // 
         // fLOW_NOLabel
         // 
         fLOW_NOLabel.AutoSize = true;
         fLOW_NOLabel.Location = new System.Drawing.Point(129, 33);
         fLOW_NOLabel.Name = "fLOW_NOLabel";
         fLOW_NOLabel.Size = new System.Drawing.Size(84, 14);
         fLOW_NOLabel.TabIndex = 8;
         fLOW_NOLabel.Text = "شماره پیگیری :";
         // 
         // rEF_NOLabel
         // 
         rEF_NOLabel.AutoSize = true;
         rEF_NOLabel.Location = new System.Drawing.Point(129, 59);
         rEF_NOLabel.Name = "rEF_NOLabel";
         rEF_NOLabel.Size = new System.Drawing.Size(78, 14);
         rEF_NOLabel.TabIndex = 10;
         rEF_NOLabel.Text = "شماره ارجاع :";
         // 
         // label5
         // 
         label5.AutoSize = true;
         label5.Location = new System.Drawing.Point(129, 87);
         label5.Name = "label5";
         label5.Size = new System.Drawing.Size(77, 14);
         label5.TabIndex = 10;
         label5.Text = "تاریخ پرداخت :";
         // 
         // label6
         // 
         label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         label6.AutoSize = true;
         label6.Location = new System.Drawing.Point(233, 342);
         label6.Name = "label6";
         label6.Size = new System.Drawing.Size(70, 14);
         label6.TabIndex = 0;
         label6.Text = "نوع پرداخت :";
         // 
         // label13
         // 
         label13.AutoSize = true;
         label13.Location = new System.Drawing.Point(113, 84);
         label13.Name = "label13";
         label13.Size = new System.Drawing.Size(36, 14);
         label13.TabIndex = 2;
         label13.Text = "(ریال)";
         // 
         // label11
         // 
         label11.AutoSize = true;
         label11.Location = new System.Drawing.Point(113, 56);
         label11.Name = "label11";
         label11.Size = new System.Drawing.Size(36, 14);
         label11.TabIndex = 3;
         label11.Text = "(ریال)";
         // 
         // label9
         // 
         label9.AutoSize = true;
         label9.Location = new System.Drawing.Point(113, 28);
         label9.Name = "label9";
         label9.Size = new System.Drawing.Size(36, 14);
         label9.TabIndex = 4;
         label9.Text = "(ریال)";
         // 
         // label12
         // 
         label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         label12.AutoSize = true;
         label12.Location = new System.Drawing.Point(261, 83);
         label12.Name = "label12";
         label12.Size = new System.Drawing.Size(105, 14);
         label12.TabIndex = 5;
         label12.Text = "مبلغ مانده حساب :";
         // 
         // label10
         // 
         label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         label10.AutoSize = true;
         label10.Location = new System.Drawing.Point(261, 55);
         label10.Name = "label10";
         label10.Size = new System.Drawing.Size(170, 14);
         label10.TabIndex = 6;
         label10.Text = "میزان براشت از مبلغ بستانکاری :";
         // 
         // label7
         // 
         label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         label7.AutoSize = true;
         label7.Location = new System.Drawing.Point(368, 385);
         label7.Name = "label7";
         label7.Size = new System.Drawing.Size(75, 14);
         label7.TabIndex = 5;
         label7.Text = "شرح تخفیف :";
         // 
         // colRWNO
         // 
         this.colRWNO.Caption = "ردیف";
         this.colRWNO.FieldName = "RWNO";
         this.colRWNO.Name = "colRWNO";
         this.colRWNO.OptionsColumn.AllowEdit = false;
         this.colRWNO.OptionsColumn.ReadOnly = true;
         this.colRWNO.Visible = true;
         this.colRWNO.VisibleIndex = 1;
         this.colRWNO.Width = 45;
         // 
         // colRWNO1
         // 
         this.colRWNO1.Caption = "ردیف";
         this.colRWNO1.FieldName = "RWNO";
         this.colRWNO1.Name = "colRWNO1";
         this.colRWNO1.Visible = true;
         this.colRWNO1.VisibleIndex = 2;
         this.colRWNO1.Width = 36;
         // 
         // StatusSaving_Gc
         // 
         this.StatusSaving_Gc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.StatusSaving_Gc.BackColor = System.Drawing.SystemColors.Control;
         this.StatusSaving_Gc.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.StatusSaving_Gc.Gauges.AddRange(new DevExpress.XtraGauges.Base.IGauge[] {
            this.stateIndicatorGauge1});
         this.StatusSaving_Gc.Location = new System.Drawing.Point(4, 606);
         this.StatusSaving_Gc.Margin = new System.Windows.Forms.Padding(4);
         this.StatusSaving_Gc.Name = "StatusSaving_Gc";
         this.StatusSaving_Gc.Size = new System.Drawing.Size(116, 97);
         this.StatusSaving_Gc.TabIndex = 35;
         // 
         // stateIndicatorGauge1
         // 
         this.stateIndicatorGauge1.Bounds = new System.Drawing.Rectangle(6, 6, 104, 85);
         this.stateIndicatorGauge1.Indicators.AddRange(new DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorComponent[] {
            this.StatusSaving_Sic});
         this.stateIndicatorGauge1.Name = "stateIndicatorGauge1";
         // 
         // StatusSaving_Sic
         // 
         this.StatusSaving_Sic.Center = new DevExpress.XtraGauges.Core.Base.PointF2D(124F, 124F);
         this.StatusSaving_Sic.Name = "stateIndicatorComponent1";
         this.StatusSaving_Sic.Size = new System.Drawing.SizeF(200F, 200F);
         this.StatusSaving_Sic.StateIndex = 0;
         indicatorState1.Name = "New Request";
         indicatorState1.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight1;
         indicatorState2.Name = "Error On Saving";
         indicatorState2.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight2;
         indicatorState3.Name = "Wait For Saving";
         indicatorState3.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight3;
         indicatorState4.Name = "Successfull Saving";
         indicatorState4.ShapeType = DevExpress.XtraGauges.Core.Model.StateIndicatorShapeType.ElectricLight4;
         this.StatusSaving_Sic.States.AddRange(new DevExpress.XtraGauges.Core.Model.IIndicatorState[] {
            indicatorState1,
            indicatorState2,
            indicatorState3,
            indicatorState4});
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.Location = new System.Drawing.Point(413, 606);
         this.Btn_Back.Margin = new System.Windows.Forms.Padding(4);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(94, 29);
         this.Btn_Back.TabIndex = 34;
         this.Btn_Back.Text = "بازگشت";
         this.Btn_Back.UseVisualStyleBackColor = true;
         this.Btn_Back.Click += new System.EventHandler(this.Btn_Back_Click);
         // 
         // c1DockingTab1
         // 
         this.c1DockingTab1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.c1DockingTab1.Controls.Add(this.c1DockingTabPage1);
         this.c1DockingTab1.Location = new System.Drawing.Point(0, 0);
         this.c1DockingTab1.Name = "c1DockingTab1";
         this.c1DockingTab1.RightToLeftLayout = true;
         this.c1DockingTab1.Size = new System.Drawing.Size(920, 599);
         this.c1DockingTab1.TabIndex = 36;
         this.c1DockingTab1.TabsSpacing = 0;
         this.c1DockingTab1.TabStyle = C1.Win.C1Command.TabStyleEnum.WindowsXP;
         this.c1DockingTab1.VisualStyle = C1.Win.C1Command.VisualStyle.WindowsXP;
         this.c1DockingTab1.VisualStyleBase = C1.Win.C1Command.VisualStyle.WindowsXP;
         // 
         // c1DockingTabPage1
         // 
         this.c1DockingTabPage1.Controls.Add(this.splitContainer1);
         this.c1DockingTabPage1.Location = new System.Drawing.Point(2, 27);
         this.c1DockingTabPage1.Name = "c1DockingTabPage1";
         this.c1DockingTabPage1.Size = new System.Drawing.Size(914, 568);
         this.c1DockingTabPage1.TabIndex = 0;
         this.c1DockingTabPage1.Text = "اطلاعات ریز پرداخت و تخفیفات";
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.c1DockingTab2);
         this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.c1DockingTab3);
         this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.splitContainer1.Size = new System.Drawing.Size(914, 568);
         this.splitContainer1.SplitterDistance = 457;
         this.splitContainer1.TabIndex = 0;
         // 
         // c1DockingTab2
         // 
         this.c1DockingTab2.Controls.Add(this.c1DockingTabPage2);
         this.c1DockingTab2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.c1DockingTab2.Location = new System.Drawing.Point(0, 0);
         this.c1DockingTab2.Name = "c1DockingTab2";
         this.c1DockingTab2.RightToLeftLayout = true;
         this.c1DockingTab2.Size = new System.Drawing.Size(457, 568);
         this.c1DockingTab2.TabIndex = 0;
         this.c1DockingTab2.TabsSpacing = 0;
         this.c1DockingTab2.TabStyle = C1.Win.C1Command.TabStyleEnum.WindowsXP;
         this.c1DockingTab2.VisualStyleBase = C1.Win.C1Command.VisualStyle.WindowsXP;
         // 
         // c1DockingTabPage2
         // 
         this.c1DockingTabPage2.Controls.Add(this.DelPmmt_Butn);
         this.c1DockingTabPage2.Controls.Add(this.SavePmmt_Butn);
         this.c1DockingTabPage2.Controls.Add(this.AddPmmt_Butn);
         this.c1DockingTabPage2.Controls.Add(this.comboBox1);
         this.c1DockingTabPage2.Controls.Add(this.groupBox2);
         this.c1DockingTabPage2.Controls.Add(this.PmmtGC);
         this.c1DockingTabPage2.Controls.Add(this.groupBox1);
         this.c1DockingTabPage2.Controls.Add(label6);
         this.c1DockingTabPage2.Image = global::System.ISP.Properties.Resources.IMAGE_1200;
         this.c1DockingTabPage2.Location = new System.Drawing.Point(2, 35);
         this.c1DockingTabPage2.Name = "c1DockingTabPage2";
         this.c1DockingTabPage2.Size = new System.Drawing.Size(451, 529);
         this.c1DockingTabPage2.TabIndex = 0;
         this.c1DockingTabPage2.Text = "زیر پرداخت ها";
         // 
         // DelPmmt_Butn
         // 
         this.DelPmmt_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.DelPmmt_Butn.Image = global::System.ISP.Properties.Resources.IMAGE_1196;
         this.DelPmmt_Butn.Location = new System.Drawing.Point(331, 339);
         this.DelPmmt_Butn.Name = "DelPmmt_Butn";
         this.DelPmmt_Butn.Size = new System.Drawing.Size(35, 35);
         this.DelPmmt_Butn.TabIndex = 35;
         this.DelPmmt_Butn.UseVisualStyleBackColor = true;
         this.DelPmmt_Butn.Click += new System.EventHandler(this.DelPmmt_Butn_Click);
         // 
         // SavePmmt_Butn
         // 
         this.SavePmmt_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.SavePmmt_Butn.Image = global::System.ISP.Properties.Resources.IMAGE_1195;
         this.SavePmmt_Butn.Location = new System.Drawing.Point(372, 339);
         this.SavePmmt_Butn.Name = "SavePmmt_Butn";
         this.SavePmmt_Butn.Size = new System.Drawing.Size(35, 35);
         this.SavePmmt_Butn.TabIndex = 35;
         this.SavePmmt_Butn.UseVisualStyleBackColor = true;
         this.SavePmmt_Butn.Click += new System.EventHandler(this.SavePmmt_Butn_Click);
         // 
         // AddPmmt_Butn
         // 
         this.AddPmmt_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.AddPmmt_Butn.Image = global::System.ISP.Properties.Resources.IMAGE_1198;
         this.AddPmmt_Butn.Location = new System.Drawing.Point(413, 339);
         this.AddPmmt_Butn.Name = "AddPmmt_Butn";
         this.AddPmmt_Butn.Size = new System.Drawing.Size(35, 35);
         this.AddPmmt_Butn.TabIndex = 35;
         this.AddPmmt_Butn.UseVisualStyleBackColor = true;
         this.AddPmmt_Butn.Click += new System.EventHandler(this.AddPmmt_Butn_Click);
         // 
         // comboBox1
         // 
         this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.PmmtBs1, "RCPT_MTOD", true));
         this.comboBox1.DataSource = this.DrcmtBs1;
         this.comboBox1.DisplayMember = "DOMN_DESC";
         this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboBox1.FormattingEnabled = true;
         this.comboBox1.Location = new System.Drawing.Point(3, 339);
         this.comboBox1.Name = "comboBox1";
         this.comboBox1.Size = new System.Drawing.Size(224, 22);
         this.comboBox1.TabIndex = 1;
         this.comboBox1.ValueMember = "VALU";
         // 
         // PmmtBs1
         // 
         this.PmmtBs1.DataMember = "Payment_Methods";
         this.PmmtBs1.DataSource = this.PymtBs1;
         this.PmmtBs1.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.BindingSource_ListChanged);
         // 
         // PymtBs1
         // 
         this.PymtBs1.DataSource = typeof(System.ISP.Data.Payment);
         // 
         // DrcmtBs1
         // 
         this.DrcmtBs1.DataSource = typeof(System.ISP.Data.D_RCMT);
         // 
         // groupBox2
         // 
         this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox2.Controls.Add(this.StrtDate_Dat);
         this.groupBox2.Controls.Add(label5);
         this.groupBox2.Controls.Add(rEF_NOLabel);
         this.groupBox2.Controls.Add(this.rEF_NOTextEdit);
         this.groupBox2.Controls.Add(fLOW_NOLabel);
         this.groupBox2.Controls.Add(this.fLOW_NOTextEdit);
         this.groupBox2.Controls.Add(bANKLabel);
         this.groupBox2.Controls.Add(this.bANKTextEdit);
         this.groupBox2.Controls.Add(cARD_NOLabel);
         this.groupBox2.Controls.Add(this.cARD_NOTextEdit);
         this.groupBox2.Controls.Add(tRAN_NOLabel);
         this.groupBox2.Controls.Add(this.tRAN_NOTextEdit);
         this.groupBox2.Controls.Add(tERM_NOLabel);
         this.groupBox2.Controls.Add(this.tERM_NOTextEdit);
         this.groupBox2.Location = new System.Drawing.Point(3, 380);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(445, 146);
         this.groupBox2.TabIndex = 3;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "ریز پرداخت های دستگاه های کارت خوان";
         // 
         // StrtDate_Dat
         // 
         this.StrtDate_Dat.CustomFormat = "dd/MM/yyyy";
         this.StrtDate_Dat.Format = Atf.UI.DateTimeSelectorFormat.Custom;
         this.StrtDate_Dat.Location = new System.Drawing.Point(23, 82);
         this.StrtDate_Dat.Name = "StrtDate_Dat";
         this.StrtDate_Dat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.StrtDate_Dat.Size = new System.Drawing.Size(100, 23);
         this.StrtDate_Dat.TabIndex = 12;
         this.StrtDate_Dat.UsePersianFormat = true;
         // 
         // rEF_NOTextEdit
         // 
         this.rEF_NOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.PmmtBs1, "REF_NO", true));
         this.rEF_NOTextEdit.Location = new System.Drawing.Point(23, 57);
         this.rEF_NOTextEdit.Name = "rEF_NOTextEdit";
         this.rEF_NOTextEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.rEF_NOTextEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.rEF_NOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.rEF_NOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.rEF_NOTextEdit.Size = new System.Drawing.Size(100, 22);
         this.rEF_NOTextEdit.TabIndex = 11;
         // 
         // fLOW_NOTextEdit
         // 
         this.fLOW_NOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.PmmtBs1, "FLOW_NO", true));
         this.fLOW_NOTextEdit.Location = new System.Drawing.Point(23, 31);
         this.fLOW_NOTextEdit.Name = "fLOW_NOTextEdit";
         this.fLOW_NOTextEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.fLOW_NOTextEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.fLOW_NOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.fLOW_NOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.fLOW_NOTextEdit.Size = new System.Drawing.Size(100, 22);
         this.fLOW_NOTextEdit.TabIndex = 9;
         // 
         // bANKTextEdit
         // 
         this.bANKTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.bANKTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.PmmtBs1, "BANK", true));
         this.bANKTextEdit.Location = new System.Drawing.Point(23, 109);
         this.bANKTextEdit.Name = "bANKTextEdit";
         this.bANKTextEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.bANKTextEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.bANKTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.bANKTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.bANKTextEdit.Size = new System.Drawing.Size(298, 22);
         this.bANKTextEdit.TabIndex = 7;
         // 
         // cARD_NOTextEdit
         // 
         this.cARD_NOTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cARD_NOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.PmmtBs1, "CARD_NO", true));
         this.cARD_NOTextEdit.Location = new System.Drawing.Point(221, 83);
         this.cARD_NOTextEdit.Name = "cARD_NOTextEdit";
         this.cARD_NOTextEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.cARD_NOTextEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.cARD_NOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.cARD_NOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.cARD_NOTextEdit.Size = new System.Drawing.Size(100, 22);
         this.cARD_NOTextEdit.TabIndex = 5;
         // 
         // tRAN_NOTextEdit
         // 
         this.tRAN_NOTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tRAN_NOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.PmmtBs1, "TRAN_NO", true));
         this.tRAN_NOTextEdit.Location = new System.Drawing.Point(222, 57);
         this.tRAN_NOTextEdit.Name = "tRAN_NOTextEdit";
         this.tRAN_NOTextEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.tRAN_NOTextEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.tRAN_NOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.tRAN_NOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.tRAN_NOTextEdit.Size = new System.Drawing.Size(100, 22);
         this.tRAN_NOTextEdit.TabIndex = 3;
         // 
         // tERM_NOTextEdit
         // 
         this.tERM_NOTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tERM_NOTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.PmmtBs1, "TERM_NO", true));
         this.tERM_NOTextEdit.Location = new System.Drawing.Point(222, 31);
         this.tERM_NOTextEdit.Name = "tERM_NOTextEdit";
         this.tERM_NOTextEdit.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.tERM_NOTextEdit.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.tERM_NOTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.tERM_NOTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.tERM_NOTextEdit.Size = new System.Drawing.Size(100, 22);
         this.tERM_NOTextEdit.TabIndex = 1;
         // 
         // PmmtGC
         // 
         this.PmmtGC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.PmmtGC.DataSource = this.PmmtBs1;
         this.PmmtGC.Location = new System.Drawing.Point(3, 127);
         this.PmmtGC.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.PmmtGC.LookAndFeel.UseDefaultLookAndFeel = false;
         this.PmmtGC.MainView = this.gridView1;
         this.PmmtGC.Name = "PmmtGC";
         this.PmmtGC.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Rcmt_Lov});
         this.PmmtGC.Size = new System.Drawing.Size(445, 206);
         this.PmmtGC.TabIndex = 2;
         this.PmmtGC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
         // 
         // gridView1
         // 
         this.gridView1.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView1.Appearance.FooterPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F);
         this.gridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView1.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView1.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F);
         this.gridView1.Appearance.Row.Options.UseFont = true;
         this.gridView1.Appearance.Row.Options.UseTextOptions = true;
         this.gridView1.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPYMT_CASH_CODE,
            this.colPYMT_RQST_RQID,
            this.colRQRO_RQST_RQID,
            this.colRQRO_RWNO,
            this.colRWNO,
            this.colAMNT,
            this.colRCPT_MTOD,
            this.colTERM_NO,
            this.colTRAN_NO,
            this.colCARD_NO,
            this.colBANK,
            this.colFLOW_NO,
            this.colREF_NO,
            this.colACTN_DATE,
            this.colSHOP_NO,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colRequest_Row,
            this.colPayment});
         styleFormatCondition1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         styleFormatCondition1.Appearance.Options.UseBackColor = true;
         styleFormatCondition1.ApplyToRow = true;
         styleFormatCondition1.Column = this.colRWNO;
         styleFormatCondition1.Condition = DevExpress.XtraGrid.FormatConditionEnum.NotEqual;
         styleFormatCondition1.Value1 = "0";
         this.gridView1.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition1});
         this.gridView1.GridControl = this.PmmtGC;
         this.gridView1.Name = "gridView1";
         this.gridView1.OptionsView.ShowDetailButtons = false;
         this.gridView1.OptionsView.ShowFooter = true;
         this.gridView1.OptionsView.ShowGroupPanel = false;
         this.gridView1.OptionsView.ShowIndicator = false;
         // 
         // colPYMT_CASH_CODE
         // 
         this.colPYMT_CASH_CODE.FieldName = "PYMT_CASH_CODE";
         this.colPYMT_CASH_CODE.Name = "colPYMT_CASH_CODE";
         // 
         // colPYMT_RQST_RQID
         // 
         this.colPYMT_RQST_RQID.FieldName = "PYMT_RQST_RQID";
         this.colPYMT_RQST_RQID.Name = "colPYMT_RQST_RQID";
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
         // colAMNT
         // 
         this.colAMNT.Caption = "مبلغ";
         this.colAMNT.DisplayFormat.FormatString = "{0:n0}";
         this.colAMNT.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.colAMNT.FieldName = "AMNT";
         this.colAMNT.GroupFormat.FormatString = "{0:n0}";
         this.colAMNT.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.colAMNT.Name = "colAMNT";
         this.colAMNT.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AMNT", "{0:n0}")});
         this.colAMNT.Visible = true;
         this.colAMNT.VisibleIndex = 0;
         this.colAMNT.Width = 249;
         // 
         // colRCPT_MTOD
         // 
         this.colRCPT_MTOD.Caption = "نوع پرداخت";
         this.colRCPT_MTOD.ColumnEdit = this.Rcmt_Lov;
         this.colRCPT_MTOD.FieldName = "RCPT_MTOD";
         this.colRCPT_MTOD.Name = "colRCPT_MTOD";
         this.colRCPT_MTOD.Width = 147;
         // 
         // Rcmt_Lov
         // 
         this.Rcmt_Lov.AutoHeight = false;
         this.Rcmt_Lov.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.Rcmt_Lov.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VALU", "VALU", 48, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Near),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("DOMN_DESC", "نوع پرداخت", 72, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
         this.Rcmt_Lov.DataSource = this.DrcmtBs1;
         this.Rcmt_Lov.DisplayMember = "DOMN_DESC";
         this.Rcmt_Lov.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Rcmt_Lov.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Rcmt_Lov.Name = "Rcmt_Lov";
         this.Rcmt_Lov.NullText = "";
         this.Rcmt_Lov.ValueMember = "VALU";
         // 
         // colTERM_NO
         // 
         this.colTERM_NO.FieldName = "TERM_NO";
         this.colTERM_NO.Name = "colTERM_NO";
         // 
         // colTRAN_NO
         // 
         this.colTRAN_NO.FieldName = "TRAN_NO";
         this.colTRAN_NO.Name = "colTRAN_NO";
         // 
         // colCARD_NO
         // 
         this.colCARD_NO.FieldName = "CARD_NO";
         this.colCARD_NO.Name = "colCARD_NO";
         // 
         // colBANK
         // 
         this.colBANK.FieldName = "BANK";
         this.colBANK.Name = "colBANK";
         // 
         // colFLOW_NO
         // 
         this.colFLOW_NO.FieldName = "FLOW_NO";
         this.colFLOW_NO.Name = "colFLOW_NO";
         // 
         // colREF_NO
         // 
         this.colREF_NO.FieldName = "REF_NO";
         this.colREF_NO.Name = "colREF_NO";
         // 
         // colACTN_DATE
         // 
         this.colACTN_DATE.FieldName = "ACTN_DATE";
         this.colACTN_DATE.Name = "colACTN_DATE";
         // 
         // colSHOP_NO
         // 
         this.colSHOP_NO.FieldName = "SHOP_NO";
         this.colSHOP_NO.Name = "colSHOP_NO";
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
         // colRequest_Row
         // 
         this.colRequest_Row.FieldName = "Request_Row";
         this.colRequest_Row.Name = "colRequest_Row";
         // 
         // colPayment
         // 
         this.colPayment.FieldName = "Payment";
         this.colPayment.Name = "colPayment";
         // 
         // groupBox1
         // 
         this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.groupBox1.Controls.Add(this.Te_TotlRemnAmnt);
         this.groupBox1.Controls.Add(this.Te_TotlDebtAmnt);
         this.groupBox1.Controls.Add(label4);
         this.groupBox1.Controls.Add(this.cASH_CODEComboBox);
         this.groupBox1.Controls.Add(label3);
         this.groupBox1.Controls.Add(label2);
         this.groupBox1.Controls.Add(label1);
         this.groupBox1.Controls.Add(cASH_CODELabel);
         this.groupBox1.Location = new System.Drawing.Point(3, 3);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(445, 118);
         this.groupBox1.TabIndex = 2;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "مبلغ قابل پرداخت";
         // 
         // Te_TotlRemnAmnt
         // 
         this.Te_TotlRemnAmnt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Te_TotlRemnAmnt.EditValue = "0";
         this.Te_TotlRemnAmnt.Location = new System.Drawing.Point(128, 79);
         this.Te_TotlRemnAmnt.Name = "Te_TotlRemnAmnt";
         this.Te_TotlRemnAmnt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Te_TotlRemnAmnt.Properties.Appearance.Options.UseFont = true;
         this.Te_TotlRemnAmnt.Properties.Appearance.Options.UseTextOptions = true;
         this.Te_TotlRemnAmnt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Te_TotlRemnAmnt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.Te_TotlRemnAmnt.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.Te_TotlRemnAmnt.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Te_TotlRemnAmnt.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Te_TotlRemnAmnt.Properties.Mask.EditMask = "n0";
         this.Te_TotlRemnAmnt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.Te_TotlRemnAmnt.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.Te_TotlRemnAmnt.Properties.ReadOnly = true;
         this.Te_TotlRemnAmnt.Size = new System.Drawing.Size(219, 22);
         this.Te_TotlRemnAmnt.TabIndex = 3;
         // 
         // Te_TotlDebtAmnt
         // 
         this.Te_TotlDebtAmnt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Te_TotlDebtAmnt.EditValue = "0";
         this.Te_TotlDebtAmnt.Location = new System.Drawing.Point(128, 53);
         this.Te_TotlDebtAmnt.Name = "Te_TotlDebtAmnt";
         this.Te_TotlDebtAmnt.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Te_TotlDebtAmnt.Properties.Appearance.Options.UseFont = true;
         this.Te_TotlDebtAmnt.Properties.Appearance.Options.UseTextOptions = true;
         this.Te_TotlDebtAmnt.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Te_TotlDebtAmnt.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.Te_TotlDebtAmnt.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.Te_TotlDebtAmnt.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Te_TotlDebtAmnt.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Te_TotlDebtAmnt.Properties.Mask.EditMask = "n0";
         this.Te_TotlDebtAmnt.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.Te_TotlDebtAmnt.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.Te_TotlDebtAmnt.Properties.ReadOnly = true;
         this.Te_TotlDebtAmnt.Size = new System.Drawing.Size(219, 22);
         this.Te_TotlDebtAmnt.TabIndex = 3;
         // 
         // cASH_CODEComboBox
         // 
         this.cASH_CODEComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cASH_CODEComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PymtBs1, "CASH_CODE", true));
         this.cASH_CODEComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.PymtBs1, "CASH_CODE", true));
         this.cASH_CODEComboBox.DataSource = this.CashBs1;
         this.cASH_CODEComboBox.DisplayMember = "NAME";
         this.cASH_CODEComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cASH_CODEComboBox.Enabled = false;
         this.cASH_CODEComboBox.FormattingEnabled = true;
         this.cASH_CODEComboBox.Location = new System.Drawing.Point(19, 25);
         this.cASH_CODEComboBox.Name = "cASH_CODEComboBox";
         this.cASH_CODEComboBox.Size = new System.Drawing.Size(328, 22);
         this.cASH_CODEComboBox.TabIndex = 1;
         this.cASH_CODEComboBox.ValueMember = "CODE";
         // 
         // CashBs1
         // 
         this.CashBs1.DataSource = typeof(System.ISP.Data.Cash);
         // 
         // c1DockingTab3
         // 
         this.c1DockingTab3.Controls.Add(this.c1DockingTabPage3);
         this.c1DockingTab3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.c1DockingTab3.Location = new System.Drawing.Point(0, 0);
         this.c1DockingTab3.Name = "c1DockingTab3";
         this.c1DockingTab3.RightToLeftLayout = true;
         this.c1DockingTab3.Size = new System.Drawing.Size(453, 568);
         this.c1DockingTab3.TabIndex = 0;
         this.c1DockingTab3.TabsSpacing = 0;
         this.c1DockingTab3.TabStyle = C1.Win.C1Command.TabStyleEnum.WindowsXP;
         this.c1DockingTab3.VisualStyleBase = C1.Win.C1Command.VisualStyle.WindowsXP;
         // 
         // c1DockingTabPage3
         // 
         this.c1DockingTabPage3.Controls.Add(this.richTextBox1);
         this.c1DockingTabPage3.Controls.Add(this.DelPyds_Butn);
         this.c1DockingTabPage3.Controls.Add(this.SavePyds_Butn);
         this.c1DockingTabPage3.Controls.Add(this.comboBox2);
         this.c1DockingTabPage3.Controls.Add(this.payment_DiscountsGridControl);
         this.c1DockingTabPage3.Controls.Add(this.AddPyds_Butn);
         this.c1DockingTabPage3.Controls.Add(this.GB_DebtStat);
         this.c1DockingTabPage3.Controls.Add(label8);
         this.c1DockingTabPage3.Controls.Add(label7);
         this.c1DockingTabPage3.Image = global::System.ISP.Properties.Resources.IMAGE_1199;
         this.c1DockingTabPage3.Location = new System.Drawing.Point(2, 35);
         this.c1DockingTabPage3.Name = "c1DockingTabPage3";
         this.c1DockingTabPage3.Size = new System.Drawing.Size(447, 529);
         this.c1DockingTabPage3.TabIndex = 0;
         this.c1DockingTabPage3.Text = "تخفیفات";
         // 
         // richTextBox1
         // 
         this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.richTextBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.PydsBs2, "PYDS_DESC", true));
         this.richTextBox1.Location = new System.Drawing.Point(3, 406);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.Size = new System.Drawing.Size(441, 120);
         this.richTextBox1.TabIndex = 36;
         this.richTextBox1.Text = "";
         // 
         // PydsBs2
         // 
         this.PydsBs2.DataMember = "Payment_Discounts";
         this.PydsBs2.DataSource = this.PymtBs1;
         this.PydsBs2.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.BindingSource_ListChanged);
         // 
         // DelPyds_Butn
         // 
         this.DelPyds_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.DelPyds_Butn.Image = global::System.ISP.Properties.Resources.IMAGE_1196;
         this.DelPyds_Butn.Location = new System.Drawing.Point(327, 339);
         this.DelPyds_Butn.Name = "DelPyds_Butn";
         this.DelPyds_Butn.Size = new System.Drawing.Size(35, 35);
         this.DelPyds_Butn.TabIndex = 35;
         this.DelPyds_Butn.UseVisualStyleBackColor = true;
         this.DelPyds_Butn.Click += new System.EventHandler(this.DelPyds_Butn_Click);
         // 
         // SavePyds_Butn
         // 
         this.SavePyds_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.SavePyds_Butn.Image = global::System.ISP.Properties.Resources.IMAGE_1195;
         this.SavePyds_Butn.Location = new System.Drawing.Point(368, 339);
         this.SavePyds_Butn.Name = "SavePyds_Butn";
         this.SavePyds_Butn.Size = new System.Drawing.Size(35, 35);
         this.SavePyds_Butn.TabIndex = 35;
         this.SavePyds_Butn.UseVisualStyleBackColor = true;
         this.SavePyds_Butn.Click += new System.EventHandler(this.SavePyds_Butn_Click);
         // 
         // payment_DiscountsGridControl
         // 
         this.payment_DiscountsGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.payment_DiscountsGridControl.DataSource = this.PydsBs2;
         this.payment_DiscountsGridControl.Location = new System.Drawing.Point(3, 127);
         this.payment_DiscountsGridControl.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.payment_DiscountsGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.payment_DiscountsGridControl.MainView = this.gridView2;
         this.payment_DiscountsGridControl.Name = "payment_DiscountsGridControl";
         this.payment_DiscountsGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Pyds_Lov});
         this.payment_DiscountsGridControl.Size = new System.Drawing.Size(441, 206);
         this.payment_DiscountsGridControl.TabIndex = 1;
         this.payment_DiscountsGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
         // 
         // gridView2
         // 
         this.gridView2.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView2.Appearance.FooterPanel.Options.UseFont = true;
         this.gridView2.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView2.Appearance.HeaderPanel.Options.UseFont = true;
         this.gridView2.Appearance.HeaderPanel.Options.UseTextOptions = true;
         this.gridView2.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView2.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridView2.Appearance.Row.Options.UseFont = true;
         this.gridView2.Appearance.Row.Options.UseTextOptions = true;
         this.gridView2.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colPYMT_CASH_CODE1,
            this.colPYMT_RQST_RQID1,
            this.colRQRO_RWNO1,
            this.colRWNO1,
            this.colEXPN_CODE,
            this.colAMNT1,
            this.colAMNT_TYPE,
            this.colSTAT,
            this.colPYDS_DESC,
            this.colCRET_BY1,
            this.colCRET_DATE1,
            this.colMDFY_BY1,
            this.colMDFY_DATE1,
            this.colRequest_Row1,
            this.colExpense,
            this.colPayment1});
         styleFormatCondition2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
         styleFormatCondition2.Appearance.Options.UseBackColor = true;
         styleFormatCondition2.ApplyToRow = true;
         styleFormatCondition2.Column = this.colRWNO1;
         styleFormatCondition2.Condition = DevExpress.XtraGrid.FormatConditionEnum.NotEqual;
         styleFormatCondition2.Value1 = "0";
         this.gridView2.FormatConditions.AddRange(new DevExpress.XtraGrid.StyleFormatCondition[] {
            styleFormatCondition2});
         this.gridView2.GridControl = this.payment_DiscountsGridControl;
         this.gridView2.Name = "gridView2";
         this.gridView2.OptionsView.ShowFooter = true;
         this.gridView2.OptionsView.ShowGroupPanel = false;
         this.gridView2.OptionsView.ShowIndicator = false;
         // 
         // colPYMT_CASH_CODE1
         // 
         this.colPYMT_CASH_CODE1.FieldName = "PYMT_CASH_CODE";
         this.colPYMT_CASH_CODE1.Name = "colPYMT_CASH_CODE1";
         // 
         // colPYMT_RQST_RQID1
         // 
         this.colPYMT_RQST_RQID1.FieldName = "PYMT_RQST_RQID";
         this.colPYMT_RQST_RQID1.Name = "colPYMT_RQST_RQID1";
         // 
         // colRQRO_RWNO1
         // 
         this.colRQRO_RWNO1.FieldName = "RQRO_RWNO";
         this.colRQRO_RWNO1.Name = "colRQRO_RWNO1";
         // 
         // colEXPN_CODE
         // 
         this.colEXPN_CODE.Caption = "نوع آیتم تخفیف";
         this.colEXPN_CODE.FieldName = "Expense.EXPN_DESC";
         this.colEXPN_CODE.Name = "colEXPN_CODE";
         this.colEXPN_CODE.Visible = true;
         this.colEXPN_CODE.VisibleIndex = 1;
         this.colEXPN_CODE.Width = 140;
         // 
         // colAMNT1
         // 
         this.colAMNT1.Caption = "مبلغ";
         this.colAMNT1.DisplayFormat.FormatString = "{0:n0}";
         this.colAMNT1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.colAMNT1.FieldName = "AMNT";
         this.colAMNT1.GroupFormat.FormatString = "{0:n0}";
         this.colAMNT1.GroupFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
         this.colAMNT1.Name = "colAMNT1";
         this.colAMNT1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AMNT", "{0:n0}")});
         this.colAMNT1.Visible = true;
         this.colAMNT1.VisibleIndex = 0;
         this.colAMNT1.Width = 87;
         // 
         // colAMNT_TYPE
         // 
         this.colAMNT_TYPE.Caption = "نوع تخفیف";
         this.colAMNT_TYPE.ColumnEdit = this.Pyds_Lov;
         this.colAMNT_TYPE.FieldName = "AMNT_TYPE";
         this.colAMNT_TYPE.Name = "colAMNT_TYPE";
         this.colAMNT_TYPE.OptionsColumn.AllowEdit = false;
         this.colAMNT_TYPE.OptionsColumn.ReadOnly = true;
         this.colAMNT_TYPE.Width = 87;
         // 
         // Pyds_Lov
         // 
         this.Pyds_Lov.AutoHeight = false;
         this.Pyds_Lov.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
         this.Pyds_Lov.DataSource = this.DpydsBs1;
         this.Pyds_Lov.DisplayMember = "DOMN_DESC";
         this.Pyds_Lov.Name = "Pyds_Lov";
         this.Pyds_Lov.NullText = "";
         this.Pyds_Lov.ValueMember = "VALU";
         // 
         // DpydsBs1
         // 
         this.DpydsBs1.DataSource = typeof(System.ISP.Data.D_PYD);
         // 
         // colSTAT
         // 
         this.colSTAT.Caption = "وضعیت";
         this.colSTAT.FieldName = "STAT";
         this.colSTAT.Name = "colSTAT";
         this.colSTAT.Width = 87;
         // 
         // colPYDS_DESC
         // 
         this.colPYDS_DESC.FieldName = "PYDS_DESC";
         this.colPYDS_DESC.Name = "colPYDS_DESC";
         // 
         // colCRET_BY1
         // 
         this.colCRET_BY1.FieldName = "CRET_BY";
         this.colCRET_BY1.Name = "colCRET_BY1";
         // 
         // colCRET_DATE1
         // 
         this.colCRET_DATE1.FieldName = "CRET_DATE";
         this.colCRET_DATE1.Name = "colCRET_DATE1";
         // 
         // colMDFY_BY1
         // 
         this.colMDFY_BY1.FieldName = "MDFY_BY";
         this.colMDFY_BY1.Name = "colMDFY_BY1";
         // 
         // colMDFY_DATE1
         // 
         this.colMDFY_DATE1.FieldName = "MDFY_DATE";
         this.colMDFY_DATE1.Name = "colMDFY_DATE1";
         // 
         // colRequest_Row1
         // 
         this.colRequest_Row1.FieldName = "Request_Row";
         this.colRequest_Row1.Name = "colRequest_Row1";
         // 
         // colExpense
         // 
         this.colExpense.FieldName = "Expense";
         this.colExpense.Name = "colExpense";
         // 
         // colPayment1
         // 
         this.colPayment1.FieldName = "Payment";
         this.colPayment1.Name = "colPayment1";
         // 
         // AddPyds_Butn
         // 
         this.AddPyds_Butn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.AddPyds_Butn.Image = global::System.ISP.Properties.Resources.IMAGE_1198;
         this.AddPyds_Butn.Location = new System.Drawing.Point(409, 339);
         this.AddPyds_Butn.Name = "AddPyds_Butn";
         this.AddPyds_Butn.Size = new System.Drawing.Size(35, 35);
         this.AddPyds_Butn.TabIndex = 35;
         this.AddPyds_Butn.UseVisualStyleBackColor = true;
         this.AddPyds_Butn.Click += new System.EventHandler(this.AddPyds_Butn_Click);
         // 
         // GB_DebtStat
         // 
         this.GB_DebtStat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.GB_DebtStat.Controls.Add(label13);
         this.GB_DebtStat.Controls.Add(this.DEBT_DNRMTextEdit);
         this.GB_DebtStat.Controls.Add(this.AddDebtDiscount_Butn002);
         this.GB_DebtStat.Controls.Add(label11);
         this.GB_DebtStat.Controls.Add(this.CashByDeposit_Txt002);
         this.GB_DebtStat.Controls.Add(label9);
         this.GB_DebtStat.Controls.Add(this.RemindAmnt_Txt002);
         this.GB_DebtStat.Controls.Add(label12);
         this.GB_DebtStat.Controls.Add(this.dEBT_DNRMLabel);
         this.GB_DebtStat.Controls.Add(label10);
         this.GB_DebtStat.Location = new System.Drawing.Point(3, 3);
         this.GB_DebtStat.Name = "GB_DebtStat";
         this.GB_DebtStat.Size = new System.Drawing.Size(441, 118);
         this.GB_DebtStat.TabIndex = 0;
         this.GB_DebtStat.TabStop = false;
         // 
         // DEBT_DNRMTextEdit
         // 
         this.DEBT_DNRMTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.DEBT_DNRMTextEdit.EditValue = "";
         this.DEBT_DNRMTextEdit.Location = new System.Drawing.Point(155, 25);
         this.DEBT_DNRMTextEdit.Name = "DEBT_DNRMTextEdit";
         this.DEBT_DNRMTextEdit.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.DEBT_DNRMTextEdit.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.DEBT_DNRMTextEdit.Properties.Mask.EditMask = "n0";
         this.DEBT_DNRMTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.DEBT_DNRMTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.DEBT_DNRMTextEdit.Properties.ReadOnly = true;
         this.DEBT_DNRMTextEdit.Size = new System.Drawing.Size(179, 22);
         this.DEBT_DNRMTextEdit.TabIndex = 10;
         // 
         // AddDebtDiscount_Butn002
         // 
         this.AddDebtDiscount_Butn002.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.AddDebtDiscount_Butn002.Image = global::System.ISP.Properties.Resources.IMAGE_1199;
         this.AddDebtDiscount_Butn002.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.AddDebtDiscount_Butn002.Location = new System.Drawing.Point(7, 77);
         this.AddDebtDiscount_Butn002.Margin = new System.Windows.Forms.Padding(4);
         this.AddDebtDiscount_Butn002.Name = "AddDebtDiscount_Butn002";
         this.AddDebtDiscount_Butn002.Size = new System.Drawing.Size(94, 29);
         this.AddDebtDiscount_Butn002.TabIndex = 34;
         this.AddDebtDiscount_Butn002.Text = "ثبت تخفیف";
         this.AddDebtDiscount_Butn002.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.AddDebtDiscount_Butn002.UseVisualStyleBackColor = true;
         this.AddDebtDiscount_Butn002.Click += new System.EventHandler(this.AddDebtDiscount_Butn002_Click);
         // 
         // CashByDeposit_Txt002
         // 
         this.CashByDeposit_Txt002.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.CashByDeposit_Txt002.EditValue = "";
         this.CashByDeposit_Txt002.Location = new System.Drawing.Point(155, 53);
         this.CashByDeposit_Txt002.Name = "CashByDeposit_Txt002";
         this.CashByDeposit_Txt002.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.CashByDeposit_Txt002.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.CashByDeposit_Txt002.Properties.Mask.EditMask = "n0";
         this.CashByDeposit_Txt002.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.CashByDeposit_Txt002.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.CashByDeposit_Txt002.Size = new System.Drawing.Size(100, 22);
         this.CashByDeposit_Txt002.TabIndex = 9;
         // 
         // RemindAmnt_Txt002
         // 
         this.RemindAmnt_Txt002.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.RemindAmnt_Txt002.EditValue = "";
         this.RemindAmnt_Txt002.Location = new System.Drawing.Point(155, 81);
         this.RemindAmnt_Txt002.Name = "RemindAmnt_Txt002";
         this.RemindAmnt_Txt002.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.RemindAmnt_Txt002.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.RemindAmnt_Txt002.Properties.Mask.EditMask = "n0";
         this.RemindAmnt_Txt002.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.RemindAmnt_Txt002.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.RemindAmnt_Txt002.Properties.ReadOnly = true;
         this.RemindAmnt_Txt002.Size = new System.Drawing.Size(100, 22);
         this.RemindAmnt_Txt002.TabIndex = 8;
         // 
         // dEBT_DNRMLabel
         // 
         this.dEBT_DNRMLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.dEBT_DNRMLabel.AutoSize = true;
         this.dEBT_DNRMLabel.Location = new System.Drawing.Point(340, 28);
         this.dEBT_DNRMLabel.Name = "dEBT_DNRMLabel";
         this.dEBT_DNRMLabel.Size = new System.Drawing.Size(91, 14);
         this.dEBT_DNRMLabel.TabIndex = 7;
         this.dEBT_DNRMLabel.Text = "مبلغ بستانکاری :";
         // 
         // label8
         // 
         label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         label8.AutoSize = true;
         label8.Location = new System.Drawing.Point(233, 342);
         label8.Name = "label8";
         label8.Size = new System.Drawing.Size(66, 14);
         label8.TabIndex = 0;
         label8.Text = "نوع تخفیف :";
         // 
         // comboBox2
         // 
         this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.comboBox2.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.PydsBs2, "AMNT_TYPE", true));
         this.comboBox2.DataSource = this.DpydsBs1;
         this.comboBox2.DisplayMember = "DOMN_DESC";
         this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboBox2.FormattingEnabled = true;
         this.comboBox2.Location = new System.Drawing.Point(3, 339);
         this.comboBox2.Name = "comboBox2";
         this.comboBox2.Size = new System.Drawing.Size(224, 22);
         this.comboBox2.TabIndex = 1;
         this.comboBox2.ValueMember = "VALU";
         // 
         // PAY_MTOD_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.c1DockingTab1);
         this.Controls.Add(this.StatusSaving_Gc);
         this.Controls.Add(this.Btn_Back);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Margin = new System.Windows.Forms.Padding(4);
         this.Name = "PAY_MTOD_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(920, 707);
         ((System.ComponentModel.ISupportInitialize)(this.stateIndicatorGauge1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.StatusSaving_Sic)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Btn_Back)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab1)).EndInit();
         this.c1DockingTab1.ResumeLayout(false);
         this.c1DockingTabPage1.ResumeLayout(false);
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
         this.splitContainer1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab2)).EndInit();
         this.c1DockingTab2.ResumeLayout(false);
         this.c1DockingTabPage2.ResumeLayout(false);
         this.c1DockingTabPage2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.DelPmmt_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.SavePmmt_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.AddPmmt_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.PmmtBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.PymtBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DrcmtBs1)).EndInit();
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.rEF_NOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.fLOW_NOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.bANKTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cARD_NOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tRAN_NOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tERM_NOTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.PmmtGC)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Rcmt_Lov)).EndInit();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.Te_TotlRemnAmnt.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Te_TotlDebtAmnt.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.CashBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.c1DockingTab3)).EndInit();
         this.c1DockingTab3.ResumeLayout(false);
         this.c1DockingTabPage3.ResumeLayout(false);
         this.c1DockingTabPage3.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.PydsBs2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DelPyds_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.SavePyds_Butn)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.payment_DiscountsGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pyds_Lov)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.DpydsBs1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.AddPyds_Butn)).EndInit();
         this.GB_DebtStat.ResumeLayout(false);
         this.GB_DebtStat.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.DEBT_DNRMTextEdit.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.AddDebtDiscount_Butn002)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.CashByDeposit_Txt002.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.RemindAmnt_Txt002.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraGauges.Win.GaugeControl StatusSaving_Gc;
      private DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorGauge stateIndicatorGauge1;
      private DevExpress.XtraGauges.Win.Gauges.State.StateIndicatorComponent StatusSaving_Sic;
      private C1.Win.C1Input.C1Button Btn_Back;
      private C1.Win.C1Command.C1DockingTab c1DockingTab1;
      private C1.Win.C1Command.C1DockingTabPage c1DockingTabPage1;
      private Windows.Forms.SplitContainer splitContainer1;
      private Windows.Forms.BindingSource PymtBs1;
      private C1.Win.C1Command.C1DockingTab c1DockingTab2;
      private C1.Win.C1Command.C1DockingTabPage c1DockingTabPage2;
      private C1.Win.C1Command.C1DockingTab c1DockingTab3;
      private C1.Win.C1Command.C1DockingTabPage c1DockingTabPage3;
      private Windows.Forms.GroupBox groupBox1;
      private DevExpress.XtraEditors.TextEdit Te_TotlRemnAmnt;
      private DevExpress.XtraEditors.TextEdit Te_TotlDebtAmnt;
      private Windows.Forms.ComboBox cASH_CODEComboBox;
      private DevExpress.XtraGrid.GridControl PmmtGC;
      private Windows.Forms.BindingSource PmmtBs1;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
      private DevExpress.XtraGrid.Columns.GridColumn colPYMT_CASH_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colPYMT_RQST_RQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RQST_RQID;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colRWNO;
      private DevExpress.XtraGrid.Columns.GridColumn colAMNT;
      private DevExpress.XtraGrid.Columns.GridColumn colRCPT_MTOD;
      private Windows.Forms.BindingSource DrcmtBs1;
      private DevExpress.XtraGrid.Columns.GridColumn colTERM_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colTRAN_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colCARD_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colBANK;
      private DevExpress.XtraGrid.Columns.GridColumn colFLOW_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colREF_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colACTN_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colSHOP_NO;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Columns.GridColumn colRequest_Row;
      private DevExpress.XtraGrid.Columns.GridColumn colPayment;
      private Windows.Forms.GroupBox groupBox2;
      private C1.Win.C1Input.C1Button DelPmmt_Butn;
      private C1.Win.C1Input.C1Button SavePmmt_Butn;
      private C1.Win.C1Input.C1Button AddPmmt_Butn;
      private DevExpress.XtraEditors.TextEdit tERM_NOTextEdit;
      private DevExpress.XtraEditors.TextEdit rEF_NOTextEdit;
      private DevExpress.XtraEditors.TextEdit fLOW_NOTextEdit;
      private DevExpress.XtraEditors.TextEdit bANKTextEdit;
      private DevExpress.XtraEditors.TextEdit cARD_NOTextEdit;
      private DevExpress.XtraEditors.TextEdit tRAN_NOTextEdit;
      private Atf.UI.DateTimeSelector StrtDate_Dat;
      private Windows.Forms.BindingSource CashBs1;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Rcmt_Lov;
      private Windows.Forms.ComboBox comboBox1;
      private Windows.Forms.GroupBox GB_DebtStat;
      private DevExpress.XtraEditors.TextEdit DEBT_DNRMTextEdit;
      private C1.Win.C1Input.C1Button AddDebtDiscount_Butn002;
      private DevExpress.XtraEditors.TextEdit CashByDeposit_Txt002;
      private DevExpress.XtraEditors.TextEdit RemindAmnt_Txt002;
      private Windows.Forms.Label dEBT_DNRMLabel;
      private DevExpress.XtraGrid.GridControl payment_DiscountsGridControl;
      private Windows.Forms.BindingSource PydsBs2;
      private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
      private DevExpress.XtraGrid.Columns.GridColumn colPYMT_CASH_CODE1;
      private DevExpress.XtraGrid.Columns.GridColumn colPYMT_RQST_RQID1;
      private DevExpress.XtraGrid.Columns.GridColumn colRQRO_RWNO1;
      private DevExpress.XtraGrid.Columns.GridColumn colRWNO1;
      private DevExpress.XtraGrid.Columns.GridColumn colEXPN_CODE;
      private DevExpress.XtraGrid.Columns.GridColumn colAMNT1;
      private DevExpress.XtraGrid.Columns.GridColumn colAMNT_TYPE;
      private DevExpress.XtraGrid.Columns.GridColumn colSTAT;
      private DevExpress.XtraGrid.Columns.GridColumn colPYDS_DESC;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_BY1;
      private DevExpress.XtraGrid.Columns.GridColumn colCRET_DATE1;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_BY1;
      private DevExpress.XtraGrid.Columns.GridColumn colMDFY_DATE1;
      private DevExpress.XtraGrid.Columns.GridColumn colRequest_Row1;
      private DevExpress.XtraGrid.Columns.GridColumn colExpense;
      private DevExpress.XtraGrid.Columns.GridColumn colPayment1;
      private C1.Win.C1Input.C1Button DelPyds_Butn;
      private C1.Win.C1Input.C1Button SavePyds_Butn;
      private C1.Win.C1Input.C1Button AddPyds_Butn;
      private Windows.Forms.RichTextBox richTextBox1;
      private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Pyds_Lov;
      private Windows.Forms.BindingSource DpydsBs1;
      private Windows.Forms.ComboBox comboBox2;
   }
}
