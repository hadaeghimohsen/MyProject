namespace System.Reporting.ReportProfiler.UnderGateways.ReportFiles.Ui
{
   partial class RPT_CHNG_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPT_CHNG_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         this.wbp_submit = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         this.be_report_name = new DevExpress.XtraEditors.ButtonEdit();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.ofd_report_path = new System.Windows.Forms.OpenFileDialog();
         ((System.ComponentModel.ISupportInitialize)(this.be_report_name.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // wbp_submit
         // 
         this.wbp_submit.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.wbp_submit.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_submit.Buttons"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "0", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_submit.Buttons1"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "1", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_submit.Buttons2"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "2", -1, true, false)});
         this.wbp_submit.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
         this.wbp_submit.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
         this.wbp_submit.Location = new System.Drawing.Point(63, 122);
         this.wbp_submit.Name = "wbp_submit";
         this.wbp_submit.Size = new System.Drawing.Size(510, 59);
         this.wbp_submit.TabIndex = 5;
         this.wbp_submit.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.wbp_submit_ButtonClick);
         // 
         // be_report_name
         // 
         this.be_report_name.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.be_report_name.EditValue = "\\\\192.168.1.149\\Reports\\GAS_CASH_R.RPT";
         this.be_report_name.Location = new System.Drawing.Point(63, 73);
         this.be_report_name.Name = "be_report_name";
         this.be_report_name.Properties.Appearance.BackColor = System.Drawing.Color.Turquoise;
         this.be_report_name.Properties.Appearance.BackColor2 = System.Drawing.Color.Turquoise;
         this.be_report_name.Properties.Appearance.BorderColor = System.Drawing.Color.Turquoise;
         this.be_report_name.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.be_report_name.Properties.Appearance.ForeColor = System.Drawing.Color.RoyalBlue;
         this.be_report_name.Properties.Appearance.Options.UseBackColor = true;
         this.be_report_name.Properties.Appearance.Options.UseBorderColor = true;
         this.be_report_name.Properties.Appearance.Options.UseFont = true;
         this.be_report_name.Properties.Appearance.Options.UseForeColor = true;
         this.be_report_name.Properties.Appearance.Options.UseTextOptions = true;
         this.be_report_name.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.be_report_name.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
         this.be_report_name.Properties.AppearanceDisabled.BackColor2 = System.Drawing.Color.White;
         this.be_report_name.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.be_report_name.Properties.AppearanceFocused.BackColor = System.Drawing.Color.MistyRose;
         this.be_report_name.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.MistyRose;
         this.be_report_name.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.Red;
         this.be_report_name.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Black;
         this.be_report_name.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.be_report_name.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.be_report_name.Properties.AppearanceFocused.Options.UseForeColor = true;
         this.be_report_name.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.Transparent;
         this.be_report_name.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.Transparent;
         this.be_report_name.Properties.AppearanceReadOnly.BorderColor = System.Drawing.Color.Transparent;
         this.be_report_name.Properties.AppearanceReadOnly.Options.UseBackColor = true;
         this.be_report_name.Properties.AppearanceReadOnly.Options.UseBorderColor = true;
         this.be_report_name.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.be_report_name.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("be_report_name.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", "0", null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("be_report_name.Properties.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", "1", null, true)});
         this.be_report_name.Properties.NullValuePrompt = "نام پروفایل خود را اینجا وارد کنید....";
         this.be_report_name.Properties.NullValuePromptShowForEmptyValue = true;
         this.be_report_name.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.be_report_name_Properties_ButtonClick);
         this.be_report_name.Size = new System.Drawing.Size(510, 38);
         this.be_report_name.TabIndex = 4;
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Location = new System.Drawing.Point(331, 17);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(242, 50);
         this.labelControl1.TabIndex = 3;
         this.labelControl1.Text = "اصلاح مسیر فایل گزارش";
         // 
         // ofd_report_path
         // 
         this.ofd_report_path.Filter = "Crystal Report file|*.Rpt";
         this.ofd_report_path.InitialDirectory = "\\\\Localhost";
         // 
         // RPT_CHNG_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Turquoise;
         this.Controls.Add(this.wbp_submit);
         this.Controls.Add(this.be_report_name);
         this.Controls.Add(this.labelControl1);
         this.Name = "RPT_CHNG_F";
         this.Size = new System.Drawing.Size(637, 197);
         ((System.ComponentModel.ISupportInitialize)(this.be_report_name.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel wbp_submit;
      private DevExpress.XtraEditors.ButtonEdit be_report_name;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private Windows.Forms.OpenFileDialog ofd_report_path;
   }
}
