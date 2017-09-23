namespace System.DataGuard.SecPolicy.Role.Ui
{
   partial class CreateNewRole
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
         this.te_titleen = new DevExpress.XtraEditors.TextEdit();
         this.te_titlefa = new DevExpress.XtraEditors.TextEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.lbl_title = new DevExpress.XtraEditors.LabelControl();
         this.sb_createnewrole = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancel = new DevExpress.XtraEditors.SimpleButton();
         this.label3 = new System.Windows.Forms.Label();
         this.SUBSYS_LOV = new DevExpress.XtraEditors.ComboBoxEdit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.SUBSYS_LOV.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // te_titleen
         // 
         this.te_titleen.EditValue = "";
         this.te_titleen.Location = new System.Drawing.Point(38, 77);
         this.te_titleen.Name = "te_titleen";
         this.te_titleen.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_titleen.Properties.Appearance.Options.UseFont = true;
         this.te_titleen.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titleen.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titleen.Properties.Mask.EditMask = "d";
         this.te_titleen.Size = new System.Drawing.Size(282, 34);
         this.te_titleen.TabIndex = 1;
         this.te_titleen.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // te_titlefa
         // 
         this.te_titlefa.EditValue = "";
         this.te_titlefa.Location = new System.Drawing.Point(38, 38);
         this.te_titlefa.Name = "te_titlefa";
         this.te_titlefa.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_titlefa.Properties.Appearance.Options.UseFont = true;
         this.te_titlefa.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titlefa.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titlefa.Properties.Mask.EditMask = "d";
         this.te_titlefa.Size = new System.Drawing.Size(282, 34);
         this.te_titlefa.TabIndex = 0;
         this.te_titlefa.Enter += new System.EventHandler(this.LangChangeToFarsi);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label2.Location = new System.Drawing.Point(326, 78);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(70, 30);
         this.label2.TabIndex = 61;
         this.label2.Text = "نام لاتین :";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.Location = new System.Drawing.Point(326, 41);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(75, 30);
         this.label1.TabIndex = 60;
         this.label1.Text = "نام فارسی :";
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
         this.lbl_title.Location = new System.Drawing.Point(3, 0);
         this.lbl_title.Name = "lbl_title";
         this.lbl_title.Size = new System.Drawing.Size(412, 32);
         this.lbl_title.TabIndex = 64;
         this.lbl_title.Text = "مشحصه گروه دسترسی";
         // 
         // sb_createnewrole
         // 
         this.sb_createnewrole.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
         this.sb_createnewrole.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_createnewrole.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_createnewrole.Appearance.Options.UseBackColor = true;
         this.sb_createnewrole.Appearance.Options.UseFont = true;
         this.sb_createnewrole.Appearance.Options.UseForeColor = true;
         this.sb_createnewrole.Location = new System.Drawing.Point(204, 171);
         this.sb_createnewrole.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_createnewrole.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_createnewrole.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_createnewrole.Name = "sb_createnewrole";
         this.sb_createnewrole.Size = new System.Drawing.Size(116, 33);
         this.sb_createnewrole.TabIndex = 2;
         this.sb_createnewrole.Text = "ثبت اطلاعات";
         this.sb_createnewrole.Click += new System.EventHandler(this.sb_createnewrole_Click);
         // 
         // sb_cancel
         // 
         this.sb_cancel.Appearance.BackColor = System.Drawing.Color.Red;
         this.sb_cancel.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_cancel.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancel.Appearance.Options.UseBackColor = true;
         this.sb_cancel.Appearance.Options.UseFont = true;
         this.sb_cancel.Appearance.Options.UseForeColor = true;
         this.sb_cancel.Location = new System.Drawing.Point(38, 171);
         this.sb_cancel.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancel.Name = "sb_cancel";
         this.sb_cancel.Size = new System.Drawing.Size(119, 33);
         this.sb_cancel.TabIndex = 3;
         this.sb_cancel.Text = "انصراف";
         this.sb_cancel.Visible = false;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label3.Location = new System.Drawing.Point(326, 119);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(73, 30);
         this.label3.TabIndex = 61;
         this.label3.Text = "زیر سیستم :";
         // 
         // SUBSYS_LOV
         // 
         this.SUBSYS_LOV.EditValue = "";
         this.SUBSYS_LOV.Location = new System.Drawing.Point(38, 117);
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
            "سیستم مدیریت مشترکین اینترنتی",
            "نرم افزار مدیریت ارتباط با مشتریان",
            "نرم افزار مدیریت شبکه های اجتماعی"});
         this.SUBSYS_LOV.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.SUBSYS_LOV.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.SUBSYS_LOV.Properties.PopupSizeable = true;
         this.SUBSYS_LOV.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
         this.SUBSYS_LOV.Size = new System.Drawing.Size(282, 36);
         this.SUBSYS_LOV.TabIndex = 2;
         this.SUBSYS_LOV.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // CreateNewRole
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DarkSeaGreen;
         this.Controls.Add(this.sb_createnewrole);
         this.Controls.Add(this.sb_cancel);
         this.Controls.Add(this.lbl_title);
         this.Controls.Add(this.te_titleen);
         this.Controls.Add(this.te_titlefa);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.SUBSYS_LOV);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "CreateNewRole";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(418, 212);
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.SUBSYS_LOV.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.TextEdit te_titleen;
      private DevExpress.XtraEditors.TextEdit te_titlefa;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label label1;
      private DevExpress.XtraEditors.LabelControl lbl_title;
      private DevExpress.XtraEditors.SimpleButton sb_createnewrole;
      private DevExpress.XtraEditors.SimpleButton sb_cancel;
      private Windows.Forms.Label label3;
      private DevExpress.XtraEditors.ComboBoxEdit SUBSYS_LOV;
   }
}
