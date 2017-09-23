namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_CHNG_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PRF_CHNG_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.be_profiler_name = new DevExpress.XtraEditors.ButtonEdit();
         this.wbp_submit = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         ((System.ComponentModel.ISupportInitialize)(this.be_profiler_name.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Location = new System.Drawing.Point(395, 20);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(166, 50);
         this.labelControl1.TabIndex = 0;
         this.labelControl1.Text = "تغییر نام پروفایل";
         // 
         // be_profiler_name
         // 
         this.be_profiler_name.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.be_profiler_name.EditValue = "";
         this.be_profiler_name.Location = new System.Drawing.Point(51, 77);
         this.be_profiler_name.Name = "be_profiler_name";
         this.be_profiler_name.Properties.Appearance.BackColor = System.Drawing.Color.White;
         this.be_profiler_name.Properties.Appearance.BackColor2 = System.Drawing.Color.White;
         this.be_profiler_name.Properties.Appearance.Font = new System.Drawing.Font("B Nazanin", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.be_profiler_name.Properties.Appearance.Options.UseBackColor = true;
         this.be_profiler_name.Properties.Appearance.Options.UseFont = true;
         this.be_profiler_name.Properties.Appearance.Options.UseTextOptions = true;
         this.be_profiler_name.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.be_profiler_name.Properties.AppearanceDisabled.BackColor = System.Drawing.Color.White;
         this.be_profiler_name.Properties.AppearanceDisabled.BackColor2 = System.Drawing.Color.White;
         this.be_profiler_name.Properties.AppearanceDisabled.Options.UseBackColor = true;
         this.be_profiler_name.Properties.AppearanceFocused.BackColor = System.Drawing.Color.White;
         this.be_profiler_name.Properties.AppearanceFocused.BackColor2 = System.Drawing.Color.White;
         this.be_profiler_name.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.be_profiler_name.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
         this.be_profiler_name.Properties.AppearanceReadOnly.BackColor2 = System.Drawing.Color.White;
         this.be_profiler_name.Properties.AppearanceReadOnly.Options.UseBackColor = true;
         this.be_profiler_name.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.be_profiler_name.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("be_profiler_name.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", "0", null, true)});
         this.be_profiler_name.Properties.NullValuePrompt = "نام پروفایل خود را اینجا وارد کنید....";
         this.be_profiler_name.Properties.NullValuePromptShowForEmptyValue = true;
         this.be_profiler_name.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.be_profiler_name_Properties_ButtonClick);
         this.be_profiler_name.Size = new System.Drawing.Size(481, 38);
         this.be_profiler_name.TabIndex = 1;
         this.be_profiler_name.Enter += new System.EventHandler(this.be_profiler_name_Enter);
         // 
         // wbp_submit
         // 
         this.wbp_submit.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.wbp_submit.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_submit.Buttons"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "0", -1, true, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_submit.Buttons1"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "1", -1, true, false)});
         this.wbp_submit.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
         this.wbp_submit.Location = new System.Drawing.Point(51, 126);
         this.wbp_submit.Name = "wbp_submit";
         this.wbp_submit.Size = new System.Drawing.Size(481, 59);
         this.wbp_submit.TabIndex = 2;
         this.wbp_submit.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.wbp_submit_ButtonClick);
         // 
         // PRF_CHNG_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Orange;
         this.Controls.Add(this.wbp_submit);
         this.Controls.Add(this.be_profiler_name);
         this.Controls.Add(this.labelControl1);
         this.Name = "PRF_CHNG_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(600, 200);
         ((System.ComponentModel.ISupportInitialize)(this.be_profiler_name.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.ButtonEdit be_profiler_name;
      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel wbp_submit;
   }
}
