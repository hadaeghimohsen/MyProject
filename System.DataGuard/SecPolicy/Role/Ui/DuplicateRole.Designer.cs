namespace System.DataGuard.SecPolicy.Role.Ui
{
   partial class DuplicateRole
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
         this.sb_duplicaterole = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancel = new DevExpress.XtraEditors.SimpleButton();
         this.lbl_title = new DevExpress.XtraEditors.LabelControl();
         this.te_titleen = new DevExpress.XtraEditors.TextEdit();
         this.te_titlefa = new DevExpress.XtraEditors.TextEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.cb_saveprivileges = new System.Windows.Forms.CheckBox();
         this.cb_saveusers = new System.Windows.Forms.CheckBox();
         this.label3 = new System.Windows.Forms.Label();
         this.SUBSYS_LOV = new DevExpress.XtraEditors.ComboBoxEdit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.SUBSYS_LOV.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // sb_duplicaterole
         // 
         this.sb_duplicaterole.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.sb_duplicaterole.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
         this.sb_duplicaterole.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_duplicaterole.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_duplicaterole.Appearance.Options.UseBackColor = true;
         this.sb_duplicaterole.Appearance.Options.UseFont = true;
         this.sb_duplicaterole.Appearance.Options.UseForeColor = true;
         this.sb_duplicaterole.Location = new System.Drawing.Point(204, 275);
         this.sb_duplicaterole.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_duplicaterole.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_duplicaterole.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_duplicaterole.Name = "sb_duplicaterole";
         this.sb_duplicaterole.Size = new System.Drawing.Size(116, 33);
         this.sb_duplicaterole.TabIndex = 67;
         this.sb_duplicaterole.Text = "ثبت اطلاعات";
         this.sb_duplicaterole.Click += new System.EventHandler(this.sb_duplicaterole_Click);
         // 
         // sb_cancel
         // 
         this.sb_cancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.sb_cancel.Appearance.BackColor = System.Drawing.Color.Red;
         this.sb_cancel.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_cancel.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancel.Appearance.Options.UseBackColor = true;
         this.sb_cancel.Appearance.Options.UseFont = true;
         this.sb_cancel.Appearance.Options.UseForeColor = true;
         this.sb_cancel.Location = new System.Drawing.Point(38, 275);
         this.sb_cancel.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancel.Name = "sb_cancel";
         this.sb_cancel.Size = new System.Drawing.Size(119, 33);
         this.sb_cancel.TabIndex = 68;
         this.sb_cancel.Text = "انصراف";
         this.sb_cancel.Visible = false;
         // 
         // lbl_title
         // 
         this.lbl_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lbl_title.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.lbl_title.Appearance.ForeColor = System.Drawing.Color.White;
         this.lbl_title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.lbl_title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.lbl_title.LineColor = System.Drawing.Color.Black;
         this.lbl_title.LineVisible = true;
         this.lbl_title.Location = new System.Drawing.Point(3, 3);
         this.lbl_title.Name = "lbl_title";
         this.lbl_title.Size = new System.Drawing.Size(418, 32);
         this.lbl_title.TabIndex = 71;
         this.lbl_title.Text = "مشحصه گروه دسترسی";
         // 
         // te_titleen
         // 
         this.te_titleen.EditValue = "";
         this.te_titleen.Location = new System.Drawing.Point(38, 80);
         this.te_titleen.Name = "te_titleen";
         this.te_titleen.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_titleen.Properties.Appearance.Options.UseFont = true;
         this.te_titleen.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titleen.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titleen.Properties.Mask.EditMask = "d";
         this.te_titleen.Size = new System.Drawing.Size(282, 34);
         this.te_titleen.TabIndex = 66;
         this.te_titleen.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // te_titlefa
         // 
         this.te_titlefa.EditValue = "";
         this.te_titlefa.Location = new System.Drawing.Point(38, 41);
         this.te_titlefa.Name = "te_titlefa";
         this.te_titlefa.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_titlefa.Properties.Appearance.Options.UseFont = true;
         this.te_titlefa.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titlefa.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titlefa.Properties.Mask.EditMask = "d";
         this.te_titlefa.Size = new System.Drawing.Size(282, 34);
         this.te_titlefa.TabIndex = 65;
         this.te_titlefa.Enter += new System.EventHandler(this.LangChangeToFarsi);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label2.ForeColor = System.Drawing.Color.White;
         this.label2.Location = new System.Drawing.Point(326, 81);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(70, 30);
         this.label2.TabIndex = 70;
         this.label2.Text = "نام لاتین :";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.Color.White;
         this.label1.Location = new System.Drawing.Point(326, 44);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(75, 30);
         this.label1.TabIndex = 69;
         this.label1.Text = "نام فارسی :";
         // 
         // cb_saveprivileges
         // 
         this.cb_saveprivileges.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.cb_saveprivileges.AutoSize = true;
         this.cb_saveprivileges.Checked = true;
         this.cb_saveprivileges.CheckState = System.Windows.Forms.CheckState.Checked;
         this.cb_saveprivileges.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_saveprivileges.ForeColor = System.Drawing.Color.White;
         this.cb_saveprivileges.Location = new System.Drawing.Point(149, 181);
         this.cb_saveprivileges.Name = "cb_saveprivileges";
         this.cb_saveprivileges.Size = new System.Drawing.Size(171, 34);
         this.cb_saveprivileges.TabIndex = 72;
         this.cb_saveprivileges.Text = "انتساب سطح های دسترسی";
         this.cb_saveprivileges.UseVisualStyleBackColor = true;
         // 
         // cb_saveusers
         // 
         this.cb_saveusers.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.cb_saveusers.AutoSize = true;
         this.cb_saveusers.Checked = true;
         this.cb_saveusers.CheckState = System.Windows.Forms.CheckState.Checked;
         this.cb_saveusers.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_saveusers.ForeColor = System.Drawing.Color.White;
         this.cb_saveusers.Location = new System.Drawing.Point(209, 221);
         this.cb_saveusers.Name = "cb_saveusers";
         this.cb_saveusers.Size = new System.Drawing.Size(111, 34);
         this.cb_saveusers.TabIndex = 72;
         this.cb_saveusers.Text = "انتساب کاربران";
         this.cb_saveusers.UseVisualStyleBackColor = true;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label3.ForeColor = System.Drawing.Color.White;
         this.label3.Location = new System.Drawing.Point(326, 122);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(73, 30);
         this.label3.TabIndex = 74;
         this.label3.Text = "زیر سیستم :";
         // 
         // SUBSYS_LOV
         // 
         this.SUBSYS_LOV.EditValue = "";
         this.SUBSYS_LOV.Location = new System.Drawing.Point(38, 120);
         this.SUBSYS_LOV.Name = "SUBSYS_LOV";
         this.SUBSYS_LOV.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.SUBSYS_LOV.Properties.Appearance.Options.UseFont = true;
         this.SUBSYS_LOV.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SUBSYS_LOV.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.SUBSYS_LOV.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.SUBSYS_LOV.Properties.Items.AddRange(new object[] {
            "امنیتی",
            "تعاریف خدمات سیستم",
            "آمار و گزارشات",
            "چاپ قبض شرکت گاز",
            "تنظیمات سیستم",
            "سیستم مدیریت باشگاه",
            "خدمات پس از فروش شرکت برق",
            "سیستم ارسال پیام و اطلاع رسانی",
            "سیستم مدیریت تدارکات",
            "سیستم مدیریت مجتمع مسکونی",
            "سیستم مدیریت مشترکین اینترنتی"});
         this.SUBSYS_LOV.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.SUBSYS_LOV.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.SUBSYS_LOV.Properties.PopupSizeable = true;
         this.SUBSYS_LOV.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.SUBSYS_LOV.Size = new System.Drawing.Size(282, 36);
         this.SUBSYS_LOV.TabIndex = 73;
         // 
         // DuplicateRole
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.CornflowerBlue;
         this.Controls.Add(this.label3);
         this.Controls.Add(this.SUBSYS_LOV);
         this.Controls.Add(this.cb_saveusers);
         this.Controls.Add(this.cb_saveprivileges);
         this.Controls.Add(this.sb_duplicaterole);
         this.Controls.Add(this.sb_cancel);
         this.Controls.Add(this.lbl_title);
         this.Controls.Add(this.te_titleen);
         this.Controls.Add(this.te_titlefa);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Name = "DuplicateRole";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(421, 319);
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.SUBSYS_LOV.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton sb_duplicaterole;
      private DevExpress.XtraEditors.SimpleButton sb_cancel;
      private DevExpress.XtraEditors.LabelControl lbl_title;
      private DevExpress.XtraEditors.TextEdit te_titleen;
      private DevExpress.XtraEditors.TextEdit te_titlefa;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label label1;
      private Windows.Forms.CheckBox cb_saveprivileges;
      private Windows.Forms.CheckBox cb_saveusers;
      private Windows.Forms.Label label3;
      private DevExpress.XtraEditors.ComboBoxEdit SUBSYS_LOV;
   }
}
