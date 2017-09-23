namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class ActionCenter
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActionCenter));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         this.ShowDesktop_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.AcctBs = new System.Windows.Forms.BindingSource();
         this.action_CenterGridControl = new DevExpress.XtraGrid.GridControl();
         this.advBandedGridView1 = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
         this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
         this.colMESG_TYPE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colCODE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colSUB_SYS = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colUSER_ID = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colACTN_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.persianRepositoryItemDateEdit1 = new dxExample.PersianRepositoryItemDateEdit();
         this.colMESG_STAT = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colCRET_BY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colCRET_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colMDFY_BY = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colMDFY_DATE = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colSub_System = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colUser = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.colMESG_TEXT = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
         this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
         this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
         this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.AcctBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.action_CenterGridControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
         this.flowLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // ShowDesktop_Butn
         // 
         this.ShowDesktop_Butn.Appearance.BorderColor = System.Drawing.Color.White;
         this.ShowDesktop_Butn.Appearance.Options.UseBorderColor = true;
         this.ShowDesktop_Butn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.ShowDesktop_Butn.Image = ((System.Drawing.Image)(resources.GetObject("ShowDesktop_Butn.Image")));
         this.ShowDesktop_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.ShowDesktop_Butn.Location = new System.Drawing.Point(3, 12);
         this.ShowDesktop_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.ShowDesktop_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.ShowDesktop_Butn.Name = "ShowDesktop_Butn";
         this.ShowDesktop_Butn.Size = new System.Drawing.Size(108, 34);
         this.ShowDesktop_Butn.TabIndex = 7;
         this.ShowDesktop_Butn.Text = "پاک کردن همه";
         this.ShowDesktop_Butn.ToolTip = "خالی کردن صفحه نمایش از فرم های در حال اجرا";
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Tabassom", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(3, 3);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(366, 53);
         this.labelControl1.TabIndex = 8;
         this.labelControl1.Text = "مرکز فعالیت ها";
         // 
         // AcctBs
         // 
         this.AcctBs.DataSource = typeof(System.DataGuard.Data.Action_Center);
         // 
         // action_CenterGridControl
         // 
         this.action_CenterGridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.action_CenterGridControl.DataSource = this.AcctBs;
         this.action_CenterGridControl.Location = new System.Drawing.Point(3, 62);
         this.action_CenterGridControl.LookAndFeel.SkinName = "Office 2013";
         this.action_CenterGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.action_CenterGridControl.MainView = this.advBandedGridView1;
         this.action_CenterGridControl.Name = "action_CenterGridControl";
         this.action_CenterGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.persianRepositoryItemDateEdit1,
            this.repositoryItemButtonEdit1});
         this.action_CenterGridControl.Size = new System.Drawing.Size(377, 262);
         this.action_CenterGridControl.TabIndex = 9;
         this.action_CenterGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.advBandedGridView1});
         // 
         // advBandedGridView1
         // 
         this.advBandedGridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.advBandedGridView1.Appearance.HeaderPanel.Options.UseFont = true;
         this.advBandedGridView1.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.advBandedGridView1.Appearance.Row.Options.UseFont = true;
         this.advBandedGridView1.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
         this.advBandedGridView1.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colCODE,
            this.colSUB_SYS,
            this.colUSER_ID,
            this.colACTN_DATE,
            this.colMESG_TEXT,
            this.colMESG_TYPE,
            this.colMESG_STAT,
            this.colCRET_BY,
            this.colCRET_DATE,
            this.colMDFY_BY,
            this.colMDFY_DATE,
            this.colSub_System,
            this.colUser,
            this.bandedGridColumn1});
         this.advBandedGridView1.GridControl = this.action_CenterGridControl;
         this.advBandedGridView1.GroupCount = 1;
         this.advBandedGridView1.Name = "advBandedGridView1";
         this.advBandedGridView1.OptionsBehavior.AutoExpandAllGroups = true;
         this.advBandedGridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
         this.advBandedGridView1.OptionsView.ColumnAutoWidth = true;
         this.advBandedGridView1.OptionsView.ShowDetailButtons = false;
         this.advBandedGridView1.OptionsView.ShowGroupPanel = false;
         this.advBandedGridView1.OptionsView.ShowIndicator = false;
         this.advBandedGridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colMESG_TYPE, DevExpress.Data.ColumnSortOrder.Ascending)});
         // 
         // gridBand1
         // 
         this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.gridBand1.AppearanceHeader.Options.UseFont = true;
         this.gridBand1.Caption = "رخدادها و پیام ها";
         this.gridBand1.Columns.Add(this.colMESG_TYPE);
         this.gridBand1.Columns.Add(this.colCODE);
         this.gridBand1.Columns.Add(this.colSUB_SYS);
         this.gridBand1.Columns.Add(this.colUSER_ID);
         this.gridBand1.Columns.Add(this.colACTN_DATE);
         this.gridBand1.Columns.Add(this.colMESG_STAT);
         this.gridBand1.Columns.Add(this.colCRET_BY);
         this.gridBand1.Columns.Add(this.colCRET_DATE);
         this.gridBand1.Columns.Add(this.colMDFY_BY);
         this.gridBand1.Columns.Add(this.colMDFY_DATE);
         this.gridBand1.Columns.Add(this.colSub_System);
         this.gridBand1.Columns.Add(this.colUser);
         this.gridBand1.Columns.Add(this.colMESG_TEXT);
         this.gridBand1.Columns.Add(this.bandedGridColumn1);
         this.gridBand1.Name = "gridBand1";
         this.gridBand1.VisibleIndex = 0;
         this.gridBand1.Width = 375;
         // 
         // colMESG_TYPE
         // 
         this.colMESG_TYPE.Caption = "نوع رخدادها";
         this.colMESG_TYPE.FieldName = "MESG_TYPE";
         this.colMESG_TYPE.Name = "colMESG_TYPE";
         this.colMESG_TYPE.OptionsColumn.FixedWidth = true;
         this.colMESG_TYPE.Visible = true;
         this.colMESG_TYPE.Width = 259;
         // 
         // colCODE
         // 
         this.colCODE.FieldName = "CODE";
         this.colCODE.Name = "colCODE";
         // 
         // colSUB_SYS
         // 
         this.colSUB_SYS.FieldName = "SUB_SYS";
         this.colSUB_SYS.Name = "colSUB_SYS";
         // 
         // colUSER_ID
         // 
         this.colUSER_ID.FieldName = "USER_ID";
         this.colUSER_ID.Name = "colUSER_ID";
         // 
         // colACTN_DATE
         // 
         this.colACTN_DATE.Caption = "تاریخ";
         this.colACTN_DATE.ColumnEdit = this.persianRepositoryItemDateEdit1;
         this.colACTN_DATE.FieldName = "ACTN_DATE";
         this.colACTN_DATE.Name = "colACTN_DATE";
         this.colACTN_DATE.OptionsColumn.FixedWidth = true;
         this.colACTN_DATE.Visible = true;
         this.colACTN_DATE.Width = 112;
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
         // colMESG_STAT
         // 
         this.colMESG_STAT.FieldName = "MESG_STAT";
         this.colMESG_STAT.Name = "colMESG_STAT";
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
         // colSub_System
         // 
         this.colSub_System.FieldName = "Sub_System";
         this.colSub_System.Name = "colSub_System";
         // 
         // colUser
         // 
         this.colUser.FieldName = "User";
         this.colUser.Name = "colUser";
         // 
         // colMESG_TEXT
         // 
         this.colMESG_TEXT.Caption = "متن پیام";
         this.colMESG_TEXT.FieldName = "MESG_TEXT";
         this.colMESG_TEXT.Image = ((System.Drawing.Image)(resources.GetObject("colMESG_TEXT.Image")));
         this.colMESG_TEXT.Name = "colMESG_TEXT";
         this.colMESG_TEXT.OptionsColumn.FixedWidth = true;
         this.colMESG_TEXT.RowIndex = 1;
         this.colMESG_TEXT.Visible = true;
         this.colMESG_TEXT.Width = 263;
         // 
         // bandedGridColumn1
         // 
         this.bandedGridColumn1.Caption = "فعالیت بر رخداد";
         this.bandedGridColumn1.ColumnEdit = this.repositoryItemButtonEdit1;
         this.bandedGridColumn1.Name = "bandedGridColumn1";
         this.bandedGridColumn1.OptionsColumn.FixedWidth = true;
         this.bandedGridColumn1.RowIndex = 1;
         this.bandedGridColumn1.Visible = true;
         this.bandedGridColumn1.Width = 112;
         // 
         // repositoryItemButtonEdit1
         // 
         this.repositoryItemButtonEdit1.AutoHeight = false;
         this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit1.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "حذف رخداد", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("repositoryItemButtonEdit1.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "نمایش اطلاعات و پیام رخداد", null, null, true)});
         this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
         this.repositoryItemButtonEdit1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
         // 
         // flowLayoutPanel1
         // 
         this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.flowLayoutPanel1.BackColor = System.Drawing.Color.Gainsboro;
         this.flowLayoutPanel1.Controls.Add(this.simpleButton1);
         this.flowLayoutPanel1.Controls.Add(this.simpleButton2);
         this.flowLayoutPanel1.Controls.Add(this.simpleButton3);
         this.flowLayoutPanel1.Controls.Add(this.simpleButton4);
         this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
         this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 330);
         this.flowLayoutPanel1.Name = "flowLayoutPanel1";
         this.flowLayoutPanel1.Size = new System.Drawing.Size(380, 146);
         this.flowLayoutPanel1.TabIndex = 10;
         // 
         // simpleButton1
         // 
         this.simpleButton1.Appearance.BorderColor = System.Drawing.Color.White;
         this.simpleButton1.Appearance.Options.UseBorderColor = true;
         this.simpleButton1.Appearance.Options.UseTextOptions = true;
         this.simpleButton1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
         this.simpleButton1.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopLeft;
         this.simpleButton1.Location = new System.Drawing.Point(3, 3);
         this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(120, 67);
         this.simpleButton1.TabIndex = 7;
         this.simpleButton1.Text = "ارتباط با سرور";
         // 
         // simpleButton2
         // 
         this.simpleButton2.Appearance.BorderColor = System.Drawing.Color.White;
         this.simpleButton2.Appearance.Options.UseBorderColor = true;
         this.simpleButton2.Appearance.Options.UseTextOptions = true;
         this.simpleButton2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton2.Image = global::System.DataGuard.Properties.Resources.IMAGE_1211;
         this.simpleButton2.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopLeft;
         this.simpleButton2.Location = new System.Drawing.Point(129, 3);
         this.simpleButton2.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton2.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton2.Name = "simpleButton2";
         this.simpleButton2.Size = new System.Drawing.Size(120, 67);
         this.simpleButton2.TabIndex = 7;
         this.simpleButton2.Text = "ارتباط با دستگاه انگشتی";
         // 
         // simpleButton3
         // 
         this.simpleButton3.Appearance.BorderColor = System.Drawing.Color.White;
         this.simpleButton3.Appearance.Options.UseBorderColor = true;
         this.simpleButton3.Appearance.Options.UseTextOptions = true;
         this.simpleButton3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton3.Image = global::System.DataGuard.Properties.Resources.IMAGE_1212;
         this.simpleButton3.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopLeft;
         this.simpleButton3.Location = new System.Drawing.Point(255, 3);
         this.simpleButton3.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton3.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton3.Name = "simpleButton3";
         this.simpleButton3.Size = new System.Drawing.Size(120, 67);
         this.simpleButton3.TabIndex = 7;
         this.simpleButton3.Text = "ارتباط با دستگاه بارکد";
         // 
         // simpleButton4
         // 
         this.simpleButton4.Appearance.BorderColor = System.Drawing.Color.White;
         this.simpleButton4.Appearance.Options.UseBorderColor = true;
         this.simpleButton4.Appearance.Options.UseTextOptions = true;
         this.simpleButton4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.simpleButton4.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
         this.simpleButton4.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton4.Image")));
         this.simpleButton4.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopLeft;
         this.simpleButton4.Location = new System.Drawing.Point(3, 76);
         this.simpleButton4.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton4.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton4.Name = "simpleButton4";
         this.simpleButton4.Size = new System.Drawing.Size(120, 67);
         this.simpleButton4.TabIndex = 7;
         this.simpleButton4.Text = "پنل کنترل";
         // 
         // ActionCenter
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.flowLayoutPanel1);
         this.Controls.Add(this.action_CenterGridControl);
         this.Controls.Add(this.ShowDesktop_Butn);
         this.Controls.Add(this.labelControl1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "ActionCenter";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(400, 476);
         ((System.ComponentModel.ISupportInitialize)(this.AcctBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.action_CenterGridControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.advBandedGridView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.persianRepositoryItemDateEdit1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
         this.flowLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton ShowDesktop_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private Windows.Forms.BindingSource AcctBs;
      private DevExpress.XtraGrid.GridControl action_CenterGridControl;
      private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView advBandedGridView1;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMESG_TYPE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCODE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSUB_SYS;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colUSER_ID;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colACTN_DATE;
      private dxExample.PersianRepositoryItemDateEdit persianRepositoryItemDateEdit1;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMESG_STAT;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCRET_BY;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCRET_DATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMDFY_BY;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMDFY_DATE;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colSub_System;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colUser;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colMESG_TEXT;
      private Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
      private DevExpress.XtraEditors.SimpleButton simpleButton1;
      private DevExpress.XtraEditors.SimpleButton simpleButton2;
      private DevExpress.XtraEditors.SimpleButton simpleButton3;
      private DevExpress.XtraEditors.SimpleButton simpleButton4;
      private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
      private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
      private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
   }
}
