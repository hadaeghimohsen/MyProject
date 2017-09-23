namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui
{
   partial class Filter
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
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.cb_filterdesc = new System.Windows.Forms.CheckBox();
         this.cbx_sourcepath = new System.Windows.Forms.ComboBox();
         this.sb_filepath = new DevExpress.XtraEditors.SimpleButton();
         this.cbx_compare = new System.Windows.Forms.ComboBox();
         this.te_minvalue = new DevExpress.XtraEditors.TextEdit();
         this.te_maxvalue = new DevExpress.XtraEditors.TextEdit();
         this.cbe_lookupvalue = new DevExpress.XtraEditors.CheckedComboBoxEdit();
         this.ofd_selector = new System.Windows.Forms.OpenFileDialog();
         ((System.ComponentModel.ISupportInitialize)(this.te_minvalue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_maxvalue.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_lookupvalue.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // cb_filterdesc
         // 
         this.cb_filterdesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cb_filterdesc.AutoSize = true;
         this.cb_filterdesc.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_filterdesc.ForeColor = System.Drawing.Color.White;
         this.cb_filterdesc.Location = new System.Drawing.Point(297, 3);
         this.cb_filterdesc.Name = "cb_filterdesc";
         this.cb_filterdesc.Size = new System.Drawing.Size(426, 18);
         this.cb_filterdesc.TabIndex = 0;
         this.cb_filterdesc.Text = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
         this.cb_filterdesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.cb_filterdesc.UseVisualStyleBackColor = true;
         this.cb_filterdesc.CheckedChanged += new System.EventHandler(this.PostCheckedChangedFilterDesc);
         // 
         // cbx_sourcepath
         // 
         this.cbx_sourcepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cbx_sourcepath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbx_sourcepath.DropDownWidth = 200;
         this.cbx_sourcepath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.cbx_sourcepath.FormattingEnabled = true;
         this.cbx_sourcepath.Items.AddRange(new object[] {
            "ورودی مستقیم توسط کاربر",
            "وروردی از طریق اطلاعات درون فایل",
            "وروردی از طریق اطلاعات درون پایگاه داده"});
         this.cbx_sourcepath.Location = new System.Drawing.Point(508, 22);
         this.cbx_sourcepath.Name = "cbx_sourcepath";
         this.cbx_sourcepath.Size = new System.Drawing.Size(191, 21);
         this.cbx_sourcepath.TabIndex = 1;
         this.cbx_sourcepath.Visible = false;
         this.cbx_sourcepath.SelectedIndexChanged += new System.EventHandler(this.PostIndexChangedSourcePath);
         this.cbx_sourcepath.VisibleChanged += new System.EventHandler(this.PostVisibleSourcePath);
         // 
         // sb_filepath
         // 
         this.sb_filepath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_filepath.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_filepath.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.sb_filepath.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_filepath.Appearance.Options.UseBackColor = true;
         this.sb_filepath.Appearance.Options.UseFont = true;
         this.sb_filepath.Appearance.Options.UseForeColor = true;
         this.sb_filepath.ImageIndex = 4;
         this.sb_filepath.Location = new System.Drawing.Point(469, 22);
         this.sb_filepath.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
         this.sb_filepath.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_filepath.Name = "sb_filepath";
         this.sb_filepath.Size = new System.Drawing.Size(33, 21);
         this.sb_filepath.TabIndex = 2;
         this.sb_filepath.Text = "...";
         this.sb_filepath.Visible = false;
         this.sb_filepath.Click += new System.EventHandler(this.sb_filepath_Click);
         // 
         // cbx_compare
         // 
         this.cbx_compare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cbx_compare.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cbx_compare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.cbx_compare.FormattingEnabled = true;
         this.cbx_compare.Items.AddRange(new object[] {
            "مساوی",
            "نامساوی"});
         this.cbx_compare.Location = new System.Drawing.Point(287, 22);
         this.cbx_compare.Name = "cbx_compare";
         this.cbx_compare.Size = new System.Drawing.Size(176, 21);
         this.cbx_compare.TabIndex = 3;
         this.cbx_compare.Visible = false;
         this.cbx_compare.SelectedIndexChanged += new System.EventHandler(this.PostIndexChangedCompare);
         this.cbx_compare.VisibleChanged += new System.EventHandler(this.PostVisibleCompare);
         // 
         // te_minvalue
         // 
         this.te_minvalue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.te_minvalue.EditValue = "";
         this.te_minvalue.Location = new System.Drawing.Point(157, 22);
         this.te_minvalue.Name = "te_minvalue";
         this.te_minvalue.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_minvalue.Properties.Appearance.Options.UseFont = true;
         this.te_minvalue.Properties.Appearance.Options.UseTextOptions = true;
         this.te_minvalue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.te_minvalue.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_minvalue.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_minvalue.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
         this.te_minvalue.Properties.Mask.EditMask = "d3";
         this.te_minvalue.Properties.Mask.IgnoreMaskBlank = false;
         this.te_minvalue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.te_minvalue.Properties.Mask.PlaceHolder = '0';
         this.te_minvalue.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.te_minvalue.Properties.MaxLength = 26;
         this.te_minvalue.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_minvalue.Size = new System.Drawing.Size(126, 20);
         this.te_minvalue.TabIndex = 4;
         this.te_minvalue.Visible = false;
         this.te_minvalue.Leave += new System.EventHandler(this.PostMinValueValidation);
         // 
         // te_maxvalue
         // 
         this.te_maxvalue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.te_maxvalue.EditValue = "";
         this.te_maxvalue.Location = new System.Drawing.Point(25, 22);
         this.te_maxvalue.Name = "te_maxvalue";
         this.te_maxvalue.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_maxvalue.Properties.Appearance.Options.UseFont = true;
         this.te_maxvalue.Properties.Appearance.Options.UseTextOptions = true;
         this.te_maxvalue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.te_maxvalue.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_maxvalue.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_maxvalue.Properties.Mask.AutoComplete = DevExpress.XtraEditors.Mask.AutoCompleteType.Optimistic;
         this.te_maxvalue.Properties.Mask.IgnoreMaskBlank = false;
         this.te_maxvalue.Properties.Mask.PlaceHolder = '0';
         this.te_maxvalue.Properties.Mask.UseMaskAsDisplayFormat = true;
         this.te_maxvalue.Properties.MaxLength = 26;
         this.te_maxvalue.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_maxvalue.Size = new System.Drawing.Size(126, 20);
         this.te_maxvalue.TabIndex = 5;
         this.te_maxvalue.Visible = false;
         this.te_maxvalue.Leave += new System.EventHandler(this.PostMaxValueValidation);
         // 
         // cbe_lookupvalue
         // 
         this.cbe_lookupvalue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cbe_lookupvalue.EditValue = "";
         this.cbe_lookupvalue.Location = new System.Drawing.Point(9, 22);
         this.cbe_lookupvalue.Name = "cbe_lookupvalue";
         this.cbe_lookupvalue.Properties.Appearance.Options.UseTextOptions = true;
         this.cbe_lookupvalue.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.cbe_lookupvalue.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.cbe_lookupvalue.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.cbe_lookupvalue.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.cbe_lookupvalue.Properties.DropDownRows = 100;
         this.cbe_lookupvalue.Properties.IncrementalSearch = true;
         this.cbe_lookupvalue.Properties.NullValuePromptShowForEmptyValue = true;
         this.cbe_lookupvalue.Properties.PopupFormMinSize = new System.Drawing.Size(0, 500);
         this.cbe_lookupvalue.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
         this.cbe_lookupvalue.Size = new System.Drawing.Size(274, 20);
         this.cbe_lookupvalue.TabIndex = 6;
         this.cbe_lookupvalue.Tag = "";
         this.cbe_lookupvalue.Visible = false;
         // 
         // ofd_selector
         // 
         this.ofd_selector.Filter = "Text file|*.txt";
         this.ofd_selector.Title = "انتخاب فایل ورودی";
         // 
         // Filter
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.te_maxvalue);
         this.Controls.Add(this.te_minvalue);
         this.Controls.Add(this.cbx_compare);
         this.Controls.Add(this.sb_filepath);
         this.Controls.Add(this.cbx_sourcepath);
         this.Controls.Add(this.cb_filterdesc);
         this.Controls.Add(this.cbe_lookupvalue);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "Filter";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(726, 48);
         ((System.ComponentModel.ISupportInitialize)(this.te_minvalue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_maxvalue.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_lookupvalue.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.CheckBox cb_filterdesc;
      private Windows.Forms.ComboBox cbx_sourcepath;
      private DevExpress.XtraEditors.SimpleButton sb_filepath;
      private Windows.Forms.ComboBox cbx_compare;
      private DevExpress.XtraEditors.TextEdit te_minvalue;
      private DevExpress.XtraEditors.TextEdit te_maxvalue;
      private DevExpress.XtraEditors.CheckedComboBoxEdit cbe_lookupvalue;
      private Windows.Forms.OpenFileDialog ofd_selector;
   }
}
