namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class CurrentChangePassword
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
         this.Btn_OK = new System.MaxUi.NewMaxBtn();
         this.label2 = new System.Windows.Forms.Label();
         this.LB_CurrentUserName = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.Txt_NewPassword = new DevExpress.XtraEditors.TextEdit();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.Txt_ConfirmNewPassword = new DevExpress.XtraEditors.TextEdit();
         this.label6 = new System.Windows.Forms.Label();
         this.Btn_Cancel = new System.MaxUi.NewMaxBtn();
         this.Ts_MustChngPass = new DevExpress.XtraEditors.ToggleSwitch();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_NewPassword.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_ConfirmNewPassword.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Ts_MustChngPass.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // Btn_OK
         // 
         this.Btn_OK.BackColor = System.Drawing.Color.Transparent;
         this.Btn_OK.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_OK.Caption = "OK";
         this.Btn_OK.Disabled = false;
         this.Btn_OK.EnterColor = System.Drawing.Color.Transparent;
         this.Btn_OK.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Btn_OK.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Btn_OK.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_OK.ImageIndex = -1;
         this.Btn_OK.ImageList = null;
         this.Btn_OK.InToBold = false;
         this.Btn_OK.Location = new System.Drawing.Point(51, 431);
         this.Btn_OK.Name = "Btn_OK";
         this.Btn_OK.Size = new System.Drawing.Size(80, 25);
         this.Btn_OK.TabIndex = 3;
         this.Btn_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_OK.TextColor = System.Drawing.Color.Black;
         this.Btn_OK.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Btn_OK.Click += new System.EventHandler(this.Btn_OK_Click);
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label2.BackColor = System.Drawing.Color.DarkGray;
         this.label2.Location = new System.Drawing.Point(597, 31);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(10, 511);
         this.label2.TabIndex = 9;
         // 
         // LB_CurrentUserName
         // 
         this.LB_CurrentUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.LB_CurrentUserName.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LB_CurrentUserName.ForeColor = System.Drawing.Color.DarkMagenta;
         this.LB_CurrentUserName.Location = new System.Drawing.Point(197, 167);
         this.LB_CurrentUserName.Name = "LB_CurrentUserName";
         this.LB_CurrentUserName.Size = new System.Drawing.Size(261, 33);
         this.LB_CurrentUserName.TabIndex = 6;
         this.LB_CurrentUserName.Text = "(Username)";
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("B Kamran", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.Color.Crimson;
         this.label1.Location = new System.Drawing.Point(408, 31);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(183, 45);
         this.label1.TabIndex = 8;
         this.label1.Text = "تغییر رمز عبور کاربری";
         // 
         // label3
         // 
         this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label3.Font = new System.Drawing.Font("B Kamran", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label3.Image = global::System.DataGuard.Properties.Resources.IMAGE_1001;
         this.label3.Location = new System.Drawing.Point(464, 93);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(95, 106);
         this.label3.TabIndex = 7;
         // 
         // Txt_NewPassword
         // 
         this.Txt_NewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Txt_NewPassword.EditValue = "";
         this.Txt_NewPassword.Location = new System.Drawing.Point(80, 203);
         this.Txt_NewPassword.Name = "Txt_NewPassword";
         this.Txt_NewPassword.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Txt_NewPassword.Properties.Appearance.Options.UseFont = true;
         this.Txt_NewPassword.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Txt_NewPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Txt_NewPassword.Properties.NullValuePrompt = "رمز عبور جدید";
         this.Txt_NewPassword.Properties.NullValuePromptShowForEmptyValue = true;
         this.Txt_NewPassword.Properties.UseSystemPasswordChar = true;
         this.Txt_NewPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.Txt_NewPassword.Size = new System.Drawing.Size(473, 30);
         this.Txt_NewPassword.TabIndex = 0;
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label4.ForeColor = System.Drawing.Color.DarkBlue;
         this.label4.Location = new System.Drawing.Point(201, 236);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(355, 16);
         this.label4.TabIndex = 8;
         this.label4.Text = "رمز عبور سیستمی که شما با آن مجوز ورود به سیستم را دارید.";
         // 
         // label5
         // 
         this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label5.AutoSize = true;
         this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label5.ForeColor = System.Drawing.Color.DarkBlue;
         this.label5.Location = new System.Drawing.Point(355, 288);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(201, 16);
         this.label5.TabIndex = 8;
         this.label5.Text = "لطفا رمز عبور وارد شده را تکرار کنید";
         // 
         // Txt_ConfirmNewPassword
         // 
         this.Txt_ConfirmNewPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Txt_ConfirmNewPassword.EditValue = "";
         this.Txt_ConfirmNewPassword.Location = new System.Drawing.Point(80, 255);
         this.Txt_ConfirmNewPassword.Name = "Txt_ConfirmNewPassword";
         this.Txt_ConfirmNewPassword.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Txt_ConfirmNewPassword.Properties.Appearance.Options.UseFont = true;
         this.Txt_ConfirmNewPassword.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Txt_ConfirmNewPassword.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Txt_ConfirmNewPassword.Properties.NullValuePrompt = "تکرار رمز عبور";
         this.Txt_ConfirmNewPassword.Properties.NullValuePromptShowForEmptyValue = true;
         this.Txt_ConfirmNewPassword.Properties.UseSystemPasswordChar = true;
         this.Txt_ConfirmNewPassword.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.Txt_ConfirmNewPassword.Size = new System.Drawing.Size(473, 30);
         this.Txt_ConfirmNewPassword.TabIndex = 1;
         // 
         // label6
         // 
         this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label6.BackColor = System.Drawing.Color.DarkGray;
         this.label6.Location = new System.Drawing.Point(51, 400);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(505, 10);
         this.label6.TabIndex = 12;
         // 
         // Btn_Cancel
         // 
         this.Btn_Cancel.BackColor = System.Drawing.Color.Transparent;
         this.Btn_Cancel.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_Cancel.Caption = "Cancel";
         this.Btn_Cancel.Disabled = false;
         this.Btn_Cancel.EnterColor = System.Drawing.Color.Transparent;
         this.Btn_Cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Btn_Cancel.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Cancel.ImageIndex = -1;
         this.Btn_Cancel.ImageList = null;
         this.Btn_Cancel.InToBold = false;
         this.Btn_Cancel.Location = new System.Drawing.Point(137, 431);
         this.Btn_Cancel.Name = "Btn_Cancel";
         this.Btn_Cancel.Size = new System.Drawing.Size(80, 25);
         this.Btn_Cancel.TabIndex = 2;
         this.Btn_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Cancel.TextColor = System.Drawing.Color.Black;
         this.Btn_Cancel.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
         // 
         // Ts_MustChngPass
         // 
         this.Ts_MustChngPass.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Ts_MustChngPass.Location = new System.Drawing.Point(78, 322);
         this.Ts_MustChngPass.Name = "Ts_MustChngPass";
         this.Ts_MustChngPass.Properties.LookAndFeel.SkinName = "Office 2013";
         this.Ts_MustChngPass.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Ts_MustChngPass.Properties.OffText = "غیرفعال";
         this.Ts_MustChngPass.Properties.OnText = "فعال";
         this.Ts_MustChngPass.Size = new System.Drawing.Size(111, 24);
         this.Ts_MustChngPass.TabIndex = 14;
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Kamran", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
         this.labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(80, 307);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(473, 54);
         this.labelControl1.TabIndex = 15;
         this.labelControl1.Text = "بعد از ورود به سیستم، رمز جدید گرفته شود";
         // 
         // CurrentChangePassword
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.LightGray;
         this.Controls.Add(this.Ts_MustChngPass);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.Txt_ConfirmNewPassword);
         this.Controls.Add(this.Txt_NewPassword);
         this.Controls.Add(this.Btn_Cancel);
         this.Controls.Add(this.Btn_OK);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.LB_CurrentUserName);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.labelControl1);
         this.Name = "CurrentChangePassword";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(655, 572);
         ((System.ComponentModel.ISupportInitialize)(this.Txt_NewPassword.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_ConfirmNewPassword.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Ts_MustChngPass.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private MaxUi.NewMaxBtn Btn_OK;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label LB_CurrentUserName;
      private Windows.Forms.Label label3;
      private Windows.Forms.Label label1;
      private DevExpress.XtraEditors.TextEdit Txt_NewPassword;
      private Windows.Forms.Label label4;
      private Windows.Forms.Label label5;
      private DevExpress.XtraEditors.TextEdit Txt_ConfirmNewPassword;
      private Windows.Forms.Label label6;
      private MaxUi.NewMaxBtn Btn_Cancel;
      private DevExpress.XtraEditors.ToggleSwitch Ts_MustChngPass;
      private DevExpress.XtraEditors.LabelControl labelControl1;
   }
}
