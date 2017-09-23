namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class Duplicate
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
         this.cb_granttorole = new System.Windows.Forms.CheckBox();
         this.cb_granttoprivilege = new System.Windows.Forms.CheckBox();
         this.sb_accept = new DevExpress.XtraEditors.SimpleButton();
         this.sb_cancel = new DevExpress.XtraEditors.SimpleButton();
         this.lbl_title = new DevExpress.XtraEditors.LabelControl();
         this.te_titleen = new DevExpress.XtraEditors.TextEdit();
         this.te_titlefa = new DevExpress.XtraEditors.TextEdit();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.cb_islock = new System.Windows.Forms.CheckBox();
         this.te_password = new DevExpress.XtraEditors.TextEdit();
         this.label3 = new System.Windows.Forms.Label();
         this.te_passwordconf = new DevExpress.XtraEditors.TextEdit();
         this.label4 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_passwordconf.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // cb_granttorole
         // 
         this.cb_granttorole.AutoSize = true;
         this.cb_granttorole.Checked = true;
         this.cb_granttorole.CheckState = System.Windows.Forms.CheckState.Checked;
         this.cb_granttorole.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_granttorole.ForeColor = System.Drawing.Color.White;
         this.cb_granttorole.Location = new System.Drawing.Point(188, 283);
         this.cb_granttorole.Name = "cb_granttorole";
         this.cb_granttorole.Size = new System.Drawing.Size(168, 34);
         this.cb_granttorole.TabIndex = 5;
         this.cb_granttorole.Text = "انتساب گروه های دسترسی";
         this.cb_granttorole.UseVisualStyleBackColor = true;
         // 
         // cb_granttoprivilege
         // 
         this.cb_granttoprivilege.AutoSize = true;
         this.cb_granttoprivilege.Checked = true;
         this.cb_granttoprivilege.CheckState = System.Windows.Forms.CheckState.Checked;
         this.cb_granttoprivilege.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_granttoprivilege.ForeColor = System.Drawing.Color.White;
         this.cb_granttoprivilege.Location = new System.Drawing.Point(185, 243);
         this.cb_granttoprivilege.Name = "cb_granttoprivilege";
         this.cb_granttoprivilege.Size = new System.Drawing.Size(171, 34);
         this.cb_granttoprivilege.TabIndex = 4;
         this.cb_granttoprivilege.Text = "انتساب سطح های دسترسی";
         this.cb_granttoprivilege.UseVisualStyleBackColor = true;
         // 
         // sb_accept
         // 
         this.sb_accept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_accept.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
         this.sb_accept.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_accept.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_accept.Appearance.Options.UseBackColor = true;
         this.sb_accept.Appearance.Options.UseFont = true;
         this.sb_accept.Appearance.Options.UseForeColor = true;
         this.sb_accept.Location = new System.Drawing.Point(273, 381);
         this.sb_accept.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_accept.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_accept.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_accept.Name = "sb_accept";
         this.sb_accept.Size = new System.Drawing.Size(184, 33);
         this.sb_accept.TabIndex = 7;
         this.sb_accept.Text = "ثبت اطلاعات";
         this.sb_accept.Click += new System.EventHandler(this.sb_accept_Click);
         // 
         // sb_cancel
         // 
         this.sb_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.sb_cancel.Appearance.BackColor = System.Drawing.Color.Red;
         this.sb_cancel.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_cancel.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_cancel.Appearance.Options.UseBackColor = true;
         this.sb_cancel.Appearance.Options.UseFont = true;
         this.sb_cancel.Appearance.Options.UseForeColor = true;
         this.sb_cancel.Location = new System.Drawing.Point(39, 381);
         this.sb_cancel.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_cancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_cancel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_cancel.Name = "sb_cancel";
         this.sb_cancel.Size = new System.Drawing.Size(187, 33);
         this.sb_cancel.TabIndex = 8;
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
         this.lbl_title.LineColor = System.Drawing.Color.CornflowerBlue;
         this.lbl_title.LineVisible = true;
         this.lbl_title.Location = new System.Drawing.Point(39, 22);
         this.lbl_title.Name = "lbl_title";
         this.lbl_title.Size = new System.Drawing.Size(418, 32);
         this.lbl_title.TabIndex = 79;
         this.lbl_title.Text = "مشحصه کاربری";
         // 
         // te_titleen
         // 
         this.te_titleen.EditValue = "";
         this.te_titleen.Location = new System.Drawing.Point(74, 114);
         this.te_titleen.Name = "te_titleen";
         this.te_titleen.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_titleen.Properties.Appearance.Options.UseFont = true;
         this.te_titleen.Properties.Appearance.Options.UseTextOptions = true;
         this.te_titleen.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.te_titleen.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titleen.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titleen.Properties.Mask.EditMask = "d";
         this.te_titleen.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_titleen.Size = new System.Drawing.Size(282, 34);
         this.te_titleen.TabIndex = 1;
         this.te_titleen.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // te_titlefa
         // 
         this.te_titlefa.EditValue = "";
         this.te_titlefa.Location = new System.Drawing.Point(74, 75);
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
         this.label2.ForeColor = System.Drawing.Color.White;
         this.label2.Location = new System.Drawing.Point(362, 115);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(70, 30);
         this.label2.TabIndex = 78;
         this.label2.Text = "نام لاتین :";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.Color.White;
         this.label1.Location = new System.Drawing.Point(362, 78);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(75, 30);
         this.label1.TabIndex = 77;
         this.label1.Text = "نام فارسی :";
         // 
         // cb_islock
         // 
         this.cb_islock.AutoSize = true;
         this.cb_islock.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_islock.ForeColor = System.Drawing.Color.White;
         this.cb_islock.Location = new System.Drawing.Point(135, 323);
         this.cb_islock.Name = "cb_islock";
         this.cb_islock.Size = new System.Drawing.Size(221, 34);
         this.cb_islock.TabIndex = 6;
         this.cb_islock.Text = "کاربر به صورت موقت غیر فعال باشد";
         this.cb_islock.UseVisualStyleBackColor = true;
         this.cb_islock.Visible = false;
         // 
         // te_password
         // 
         this.te_password.EditValue = "";
         this.te_password.Location = new System.Drawing.Point(74, 153);
         this.te_password.Name = "te_password";
         this.te_password.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_password.Properties.Appearance.Options.UseFont = true;
         this.te_password.Properties.Appearance.Options.UseTextOptions = true;
         this.te_password.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.te_password.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_password.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_password.Properties.Mask.EditMask = "d";
         this.te_password.Properties.UseSystemPasswordChar = true;
         this.te_password.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_password.Size = new System.Drawing.Size(282, 34);
         this.te_password.TabIndex = 2;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label3.ForeColor = System.Drawing.Color.White;
         this.label3.Location = new System.Drawing.Point(362, 154);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(65, 30);
         this.label3.TabIndex = 84;
         this.label3.Text = "رمز عبور :";
         // 
         // te_passwordconf
         // 
         this.te_passwordconf.EditValue = "";
         this.te_passwordconf.Location = new System.Drawing.Point(74, 192);
         this.te_passwordconf.Name = "te_passwordconf";
         this.te_passwordconf.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_passwordconf.Properties.Appearance.Options.UseFont = true;
         this.te_passwordconf.Properties.Appearance.Options.UseTextOptions = true;
         this.te_passwordconf.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.te_passwordconf.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_passwordconf.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_passwordconf.Properties.Mask.EditMask = "d";
         this.te_passwordconf.Properties.UseSystemPasswordChar = true;
         this.te_passwordconf.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_passwordconf.Size = new System.Drawing.Size(282, 34);
         this.te_passwordconf.TabIndex = 3;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label4.ForeColor = System.Drawing.Color.White;
         this.label4.Location = new System.Drawing.Point(362, 193);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(93, 30);
         this.label4.TabIndex = 86;
         this.label4.Text = "تکرار رمز عبور :";
         // 
         // Duplicate
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DarkBlue;
         this.Controls.Add(this.te_passwordconf);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.te_password);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.cb_islock);
         this.Controls.Add(this.cb_granttorole);
         this.Controls.Add(this.cb_granttoprivilege);
         this.Controls.Add(this.sb_accept);
         this.Controls.Add(this.sb_cancel);
         this.Controls.Add(this.lbl_title);
         this.Controls.Add(this.te_titleen);
         this.Controls.Add(this.te_titlefa);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Name = "Duplicate";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(497, 439);
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_passwordconf.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.CheckBox cb_granttorole;
      private Windows.Forms.CheckBox cb_granttoprivilege;
      private DevExpress.XtraEditors.SimpleButton sb_accept;
      private DevExpress.XtraEditors.SimpleButton sb_cancel;
      private DevExpress.XtraEditors.LabelControl lbl_title;
      private DevExpress.XtraEditors.TextEdit te_titleen;
      private DevExpress.XtraEditors.TextEdit te_titlefa;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label label1;
      private Windows.Forms.CheckBox cb_islock;
      private DevExpress.XtraEditors.TextEdit te_password;
      private Windows.Forms.Label label3;
      private DevExpress.XtraEditors.TextEdit te_passwordconf;
      private Windows.Forms.Label label4;
   }
}
