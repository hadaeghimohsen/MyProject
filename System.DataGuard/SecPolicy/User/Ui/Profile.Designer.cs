namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class Profile
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
         this.lbl_title = new DevExpress.XtraEditors.LabelControl();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.te_shortcut = new DevExpress.XtraEditors.TextEdit();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.cb_islock = new System.Windows.Forms.CheckBox();
         this.cb_showpass = new System.Windows.Forms.CheckBox();
         this.btn_accept = new DevExpress.XtraEditors.SimpleButton();
         this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
         this.te_titlefa = new DevExpress.XtraEditors.TextEdit();
         this.te_titleen = new DevExpress.XtraEditors.TextEdit();
         this.te_password = new DevExpress.XtraEditors.TextEdit();
         this.te_passwordconf = new DevExpress.XtraEditors.TextEdit();
         this.pb_userimg = new System.Windows.Forms.PictureBox();
         ((System.ComponentModel.ISupportInitialize)(this.te_shortcut.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_passwordconf.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_userimg)).BeginInit();
         this.SuspendLayout();
         // 
         // lbl_title
         // 
         this.lbl_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lbl_title.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.lbl_title.Appearance.ForeColor = System.Drawing.Color.White;
         this.lbl_title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.lbl_title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.lbl_title.LineColor = System.Drawing.Color.Black;
         this.lbl_title.LineVisible = true;
         this.lbl_title.Location = new System.Drawing.Point(3, 3);
         this.lbl_title.Name = "lbl_title";
         this.lbl_title.Size = new System.Drawing.Size(406, 32);
         this.lbl_title.TabIndex = 35;
         this.lbl_title.Text = "مشخصه کاربر";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(325, 112);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(64, 13);
         this.label1.TabIndex = 37;
         this.label1.Text = "نام فارسی :";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(325, 140);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(52, 13);
         this.label2.TabIndex = 39;
         this.label2.Text = "نام لاتین :";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(325, 84);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(58, 13);
         this.label3.TabIndex = 41;
         this.label3.Text = "کد کاربری :";
         // 
         // te_shortcut
         // 
         this.te_shortcut.Location = new System.Drawing.Point(219, 81);
         this.te_shortcut.Name = "te_shortcut";
         this.te_shortcut.Properties.Appearance.Options.UseTextOptions = true;
         this.te_shortcut.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.te_shortcut.Properties.Mask.EditMask = "d";
         this.te_shortcut.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
         this.te_shortcut.Properties.ReadOnly = true;
         this.te_shortcut.Size = new System.Drawing.Size(100, 20);
         this.te_shortcut.TabIndex = 0;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(325, 188);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(52, 13);
         this.label4.TabIndex = 44;
         this.label4.Text = "رمز عبور :";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(325, 216);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(76, 13);
         this.label5.TabIndex = 46;
         this.label5.Text = "تکرار رمز عبور :";
         // 
         // cb_islock
         // 
         this.cb_islock.AutoSize = true;
         this.cb_islock.Location = new System.Drawing.Point(253, 270);
         this.cb_islock.Name = "cb_islock";
         this.cb_islock.Size = new System.Drawing.Size(66, 17);
         this.cb_islock.TabIndex = 6;
         this.cb_islock.Text = "غیر فعال";
         this.cb_islock.UseVisualStyleBackColor = true;
         this.cb_islock.Visible = false;
         // 
         // cb_showpass
         // 
         this.cb_showpass.AutoSize = true;
         this.cb_showpass.Location = new System.Drawing.Point(28, 240);
         this.cb_showpass.Name = "cb_showpass";
         this.cb_showpass.Size = new System.Drawing.Size(97, 17);
         this.cb_showpass.TabIndex = 5;
         this.cb_showpass.Text = "نمایش رمز عبور";
         this.cb_showpass.UseVisualStyleBackColor = true;
         // 
         // btn_accept
         // 
         this.btn_accept.Appearance.BackColor = System.Drawing.Color.Blue;
         this.btn_accept.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btn_accept.Appearance.ForeColor = System.Drawing.Color.White;
         this.btn_accept.Appearance.Options.UseBackColor = true;
         this.btn_accept.Appearance.Options.UseFont = true;
         this.btn_accept.Appearance.Options.UseForeColor = true;
         this.btn_accept.Location = new System.Drawing.Point(270, 336);
         this.btn_accept.LookAndFeel.SkinName = "Office 2010 Silver";
         this.btn_accept.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.btn_accept.LookAndFeel.UseDefaultLookAndFeel = false;
         this.btn_accept.Name = "btn_accept";
         this.btn_accept.Size = new System.Drawing.Size(116, 42);
         this.btn_accept.TabIndex = 7;
         this.btn_accept.Text = "ثبت اطلاعات";
         this.btn_accept.Click += new System.EventHandler(this.btn_accept_Click);
         // 
         // btn_cancel
         // 
         this.btn_cancel.Appearance.BackColor = System.Drawing.Color.Blue;
         this.btn_cancel.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btn_cancel.Appearance.ForeColor = System.Drawing.Color.White;
         this.btn_cancel.Appearance.Options.UseBackColor = true;
         this.btn_cancel.Appearance.Options.UseFont = true;
         this.btn_cancel.Appearance.Options.UseForeColor = true;
         this.btn_cancel.Location = new System.Drawing.Point(28, 336);
         this.btn_cancel.LookAndFeel.SkinName = "Office 2010 Silver";
         this.btn_cancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.btn_cancel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.btn_cancel.Name = "btn_cancel";
         this.btn_cancel.Size = new System.Drawing.Size(119, 42);
         this.btn_cancel.TabIndex = 8;
         this.btn_cancel.Text = "انصراف";
         // 
         // te_titlefa
         // 
         this.te_titlefa.Location = new System.Drawing.Point(28, 109);
         this.te_titlefa.Name = "te_titlefa";
         this.te_titlefa.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titlefa.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titlefa.Properties.Mask.EditMask = "d";
         this.te_titlefa.Size = new System.Drawing.Size(291, 20);
         this.te_titlefa.TabIndex = 1;
         this.te_titlefa.Enter += new System.EventHandler(this.LangChangeToFarsi);
         // 
         // te_titleen
         // 
         this.te_titleen.EditValue = "";
         this.te_titleen.Location = new System.Drawing.Point(28, 137);
         this.te_titleen.Name = "te_titleen";
         this.te_titleen.Properties.Appearance.Options.UseTextOptions = true;
         this.te_titleen.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.te_titleen.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titleen.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titleen.Properties.Mask.EditMask = "d";
         this.te_titleen.Properties.ReadOnly = true;
         this.te_titleen.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_titleen.Size = new System.Drawing.Size(291, 20);
         this.te_titleen.TabIndex = 2;
         this.te_titleen.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // te_password
         // 
         this.te_password.EditValue = "";
         this.te_password.Location = new System.Drawing.Point(28, 185);
         this.te_password.Name = "te_password";
         this.te_password.Properties.Appearance.Options.UseTextOptions = true;
         this.te_password.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.te_password.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_password.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_password.Properties.Mask.EditMask = "d";
         this.te_password.Properties.UseSystemPasswordChar = true;
         this.te_password.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_password.Size = new System.Drawing.Size(291, 20);
         this.te_password.TabIndex = 3;
         this.te_password.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // te_passwordconf
         // 
         this.te_passwordconf.EditValue = "";
         this.te_passwordconf.Location = new System.Drawing.Point(28, 213);
         this.te_passwordconf.Name = "te_passwordconf";
         this.te_passwordconf.Properties.Appearance.Options.UseTextOptions = true;
         this.te_passwordconf.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.te_passwordconf.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_passwordconf.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_passwordconf.Properties.Mask.EditMask = "d";
         this.te_passwordconf.Properties.UseSystemPasswordChar = true;
         this.te_passwordconf.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.te_passwordconf.Size = new System.Drawing.Size(291, 20);
         this.te_passwordconf.TabIndex = 4;
         this.te_passwordconf.Enter += new System.EventHandler(this.LangChangeToEnglish);
         // 
         // pb_userimg
         // 
         this.pb_userimg.Location = new System.Drawing.Point(28, 38);
         this.pb_userimg.Name = "pb_userimg";
         this.pb_userimg.Size = new System.Drawing.Size(50, 50);
         this.pb_userimg.TabIndex = 49;
         this.pb_userimg.TabStop = false;
         // 
         // Profile
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlDark;
         this.Controls.Add(this.te_passwordconf);
         this.Controls.Add(this.te_password);
         this.Controls.Add(this.te_titleen);
         this.Controls.Add(this.te_titlefa);
         this.Controls.Add(this.pb_userimg);
         this.Controls.Add(this.btn_accept);
         this.Controls.Add(this.btn_cancel);
         this.Controls.Add(this.cb_showpass);
         this.Controls.Add(this.cb_islock);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.te_shortcut);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.lbl_title);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "Profile";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(412, 404);
         ((System.ComponentModel.ISupportInitialize)(this.te_shortcut.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_titleen.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_passwordconf.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_userimg)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl lbl_title;
      private Windows.Forms.Label label1;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label label3;
      private DevExpress.XtraEditors.TextEdit te_shortcut;
      private Windows.Forms.Label label4;
      private Windows.Forms.Label label5;
      private Windows.Forms.CheckBox cb_islock;
      private Windows.Forms.CheckBox cb_showpass;
      private DevExpress.XtraEditors.SimpleButton btn_accept;
      private DevExpress.XtraEditors.SimpleButton btn_cancel;
      private Windows.Forms.PictureBox pb_userimg;
      private DevExpress.XtraEditors.TextEdit te_titlefa;
      private DevExpress.XtraEditors.TextEdit te_titleen;
      private DevExpress.XtraEditors.TextEdit te_password;
      private DevExpress.XtraEditors.TextEdit te_passwordconf;
   }
}
