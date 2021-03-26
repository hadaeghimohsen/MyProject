namespace System.DataGuard.Login.Ui
{
   partial class Login
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
         this.te_username = new DevExpress.XtraEditors.TextEdit();
         this.te_password = new DevExpress.XtraEditors.TextEdit();
         this.sb_login = new DevExpress.XtraEditors.SimpleButton();
         this.label2 = new System.Windows.Forms.Label();
         this.linkLabel2 = new System.Windows.Forms.LinkLabel();
         this.linkLabel1 = new System.Windows.Forms.LinkLabel();
         this.linkLabel3 = new System.Windows.Forms.LinkLabel();
         this.label3 = new System.Windows.Forms.Label();
         this.linkLabel4 = new System.Windows.Forms.LinkLabel();
         this.linkLabel5 = new System.Windows.Forms.LinkLabel();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.label6 = new System.Windows.Forms.Label();
         this.Cancel_Lbl = new System.Windows.Forms.Label();
         this.SwitchUser_RondButn = new System.MaxUi.RoundedButton();
         this.pictureBox2 = new System.Windows.Forms.PictureBox();
         this.User_RondButn = new System.MaxUi.RoundedButton();
         this.FngrDev_Pb = new System.Windows.Forms.PictureBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ActvLicnTime_LL = new System.Windows.Forms.LinkLabel();
         ((System.ComponentModel.ISupportInitialize)(this.te_username.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.FngrDev_Pb)).BeginInit();
         this.SuspendLayout();
         // 
         // te_username
         // 
         this.te_username.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.te_username.EditValue = "";
         this.te_username.Location = new System.Drawing.Point(243, 350);
         this.te_username.Name = "te_username";
         this.te_username.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.te_username.Properties.Appearance.Options.UseFont = true;
         this.te_username.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_username.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_username.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.te_username.Properties.NullValuePrompt = "someone@anar.com";
         this.te_username.Properties.NullValuePromptShowForEmptyValue = true;
         this.te_username.Size = new System.Drawing.Size(235, 24);
         this.te_username.TabIndex = 0;
         this.te_username.Enter += new System.EventHandler(this.Control_Enter);
         this.te_username.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputValidation);
         // 
         // te_password
         // 
         this.te_password.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.te_password.EditValue = "";
         this.te_password.Location = new System.Drawing.Point(243, 380);
         this.te_password.Name = "te_password";
         this.te_password.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.te_password.Properties.Appearance.Options.UseFont = true;
         this.te_password.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_password.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_password.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
         this.te_password.Properties.NullValuePrompt = "Password";
         this.te_password.Properties.NullValuePromptShowForEmptyValue = true;
         this.te_password.Properties.UseSystemPasswordChar = true;
         this.te_password.Size = new System.Drawing.Size(235, 24);
         this.te_password.TabIndex = 1;
         this.te_password.Enter += new System.EventHandler(this.Control_Enter);
         this.te_password.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputValidation);
         // 
         // sb_login
         // 
         this.sb_login.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.sb_login.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_login.Appearance.Font = new System.Drawing.Font("Webdings", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
         this.sb_login.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_login.Appearance.Options.UseBackColor = true;
         this.sb_login.Appearance.Options.UseFont = true;
         this.sb_login.Appearance.Options.UseForeColor = true;
         this.sb_login.Location = new System.Drawing.Point(484, 377);
         this.sb_login.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_login.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_login.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_login.Name = "sb_login";
         this.sb_login.Size = new System.Drawing.Size(30, 30);
         this.sb_login.TabIndex = 2;
         this.sb_login.Text = "4";
         this.sb_login.Click += new System.EventHandler(this.GotoValidation);
         this.sb_login.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputValidation);
         // 
         // label2
         // 
         this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.label2.Location = new System.Drawing.Point(242, 423);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(78, 15);
         this.label2.TabIndex = 5;
         this.label2.Text = "Anar account";
         // 
         // linkLabel2
         // 
         this.linkLabel2.ActiveLinkColor = System.Drawing.Color.Blue;
         this.linkLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.linkLabel2.AutoSize = true;
         this.linkLabel2.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.linkLabel2.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
         this.linkLabel2.Location = new System.Drawing.Point(323, 423);
         this.linkLabel2.Name = "linkLabel2";
         this.linkLabel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.linkLabel2.Size = new System.Drawing.Size(70, 15);
         this.linkLabel2.TabIndex = 3;
         this.linkLabel2.TabStop = true;
         this.linkLabel2.Text = "What\'s this?";
         this.linkLabel2.VisitedLinkColor = System.Drawing.Color.Blue;
         // 
         // linkLabel1
         // 
         this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Blue;
         this.linkLabel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.linkLabel1.AutoSize = true;
         this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
         this.linkLabel1.Location = new System.Drawing.Point(242, 448);
         this.linkLabel1.Name = "linkLabel1";
         this.linkLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.linkLabel1.Size = new System.Drawing.Size(150, 15);
         this.linkLabel1.TabIndex = 5;
         this.linkLabel1.TabStop = true;
         this.linkLabel1.Text = "Can\'t access your account?";
         this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
         // 
         // linkLabel3
         // 
         this.linkLabel3.ActiveLinkColor = System.Drawing.Color.Blue;
         this.linkLabel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.linkLabel3.AutoSize = true;
         this.linkLabel3.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
         this.linkLabel3.Location = new System.Drawing.Point(400, 475);
         this.linkLabel3.Name = "linkLabel3";
         this.linkLabel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.linkLabel3.Size = new System.Drawing.Size(73, 15);
         this.linkLabel3.TabIndex = 6;
         this.linkLabel3.TabStop = true;
         this.linkLabel3.Text = "Sign up now";
         this.linkLabel3.VisitedLinkColor = System.Drawing.Color.Blue;
         // 
         // label3
         // 
         this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.label3.Location = new System.Drawing.Point(242, 475);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(152, 15);
         this.label3.TabIndex = 9;
         this.label3.Text = "Don\'t have a Anar account?";
         // 
         // linkLabel4
         // 
         this.linkLabel4.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.linkLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.linkLabel4.AutoSize = true;
         this.linkLabel4.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.linkLabel4.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
         this.linkLabel4.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.linkLabel4.Location = new System.Drawing.Point(614, 667);
         this.linkLabel4.Name = "linkLabel4";
         this.linkLabel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.linkLabel4.Size = new System.Drawing.Size(57, 15);
         this.linkLabel4.TabIndex = 7;
         this.linkLabel4.TabStop = true;
         this.linkLabel4.Text = "Feedback";
         this.linkLabel4.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         // 
         // linkLabel5
         // 
         this.linkLabel5.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.linkLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.linkLabel5.AutoSize = true;
         this.linkLabel5.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.linkLabel5.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
         this.linkLabel5.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.linkLabel5.Location = new System.Drawing.Point(536, 667);
         this.linkLabel5.Name = "linkLabel5";
         this.linkLabel5.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.linkLabel5.Size = new System.Drawing.Size(70, 15);
         this.linkLabel5.TabIndex = 8;
         this.linkLabel5.TabStop = true;
         this.linkLabel5.Text = "Help Center";
         this.linkLabel5.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.label4.Location = new System.Drawing.Point(13, 667);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(169, 15);
         this.label4.TabIndex = 9;
         this.label4.Text = "© 1392 Anar Team Corporation";
         // 
         // label5
         // 
         this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label5.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.ForeColor = System.Drawing.Color.Black;
         this.label5.Location = new System.Drawing.Point(664, 0);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(69, 23);
         this.label5.TabIndex = 13;
         this.label5.Text = "Ver 2.5";
         this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label6
         // 
         this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.ForeColor = System.Drawing.Color.Black;
         this.label6.Location = new System.Drawing.Point(63, 116);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(602, 32);
         this.label6.TabIndex = 14;
         this.label6.Text = "Anar Team Corporation \r\nCellphone : 0917 101 5031 - 0935 744 5978\r\n";
         this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // Cancel_Lbl
         // 
         this.Cancel_Lbl.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Cancel_Lbl.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Cancel_Lbl.ForeColor = System.Drawing.Color.Black;
         this.Cancel_Lbl.Location = new System.Drawing.Point(318, 660);
         this.Cancel_Lbl.Name = "Cancel_Lbl";
         this.Cancel_Lbl.Size = new System.Drawing.Size(100, 23);
         this.Cancel_Lbl.TabIndex = 17;
         this.Cancel_Lbl.Text = "Switch User";
         this.Cancel_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // SwitchUser_RondButn
         // 
         this.SwitchUser_RondButn.Active = true;
         this.SwitchUser_RondButn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.SwitchUser_RondButn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.SwitchUser_RondButn.Caption = "";
         this.SwitchUser_RondButn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.SwitchUser_RondButn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.SwitchUser_RondButn.HoverBorderColor = System.Drawing.Color.Gold;
         this.SwitchUser_RondButn.HoverColorA = System.Drawing.Color.LightGray;
         this.SwitchUser_RondButn.HoverColorB = System.Drawing.Color.LightGray;
         this.SwitchUser_RondButn.ImageProfile = global::System.DataGuard.Properties.Resources.IMAGE_1482;
         this.SwitchUser_RondButn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.SwitchUser_RondButn.ImageVisiable = true;
         this.SwitchUser_RondButn.Location = new System.Drawing.Point(343, 607);
         this.SwitchUser_RondButn.Name = "SwitchUser_RondButn";
         this.SwitchUser_RondButn.NormalBorderColor = System.Drawing.Color.Black;
         this.SwitchUser_RondButn.NormalColorA = System.Drawing.Color.White;
         this.SwitchUser_RondButn.NormalColorB = System.Drawing.Color.White;
         this.SwitchUser_RondButn.Size = new System.Drawing.Size(50, 50);
         this.SwitchUser_RondButn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.SwitchUser_RondButn.TabIndex = 18;
         this.SwitchUser_RondButn.Tooltip = null;
         this.SwitchUser_RondButn.Click += new System.EventHandler(this.LastUserLogin_RondButn_Click);
         // 
         // pictureBox2
         // 
         this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.pictureBox2.Image = global::System.DataGuard.Properties.Resources.IMAGE_1619;
         this.pictureBox2.Location = new System.Drawing.Point(339, 63);
         this.pictureBox2.Name = "pictureBox2";
         this.pictureBox2.Size = new System.Drawing.Size(50, 50);
         this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.pictureBox2.TabIndex = 15;
         this.pictureBox2.TabStop = false;
         // 
         // User_RondButn
         // 
         this.User_RondButn.Active = true;
         this.User_RondButn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.User_RondButn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.User_RondButn.Caption = "";
         this.User_RondButn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.User_RondButn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.User_RondButn.HoverBorderColor = System.Drawing.Color.Gold;
         this.User_RondButn.HoverColorA = System.Drawing.Color.LightGray;
         this.User_RondButn.HoverColorB = System.Drawing.Color.LightGray;
         this.User_RondButn.ImageProfile = global::System.DataGuard.Properties.Resources.IMAGE_1429;
         this.User_RondButn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.User_RondButn.ImageVisiable = true;
         this.User_RondButn.Location = new System.Drawing.Point(293, 183);
         this.User_RondButn.Name = "User_RondButn";
         this.User_RondButn.NormalBorderColor = System.Drawing.Color.Black;
         this.User_RondButn.NormalColorA = System.Drawing.Color.White;
         this.User_RondButn.NormalColorB = System.Drawing.Color.White;
         this.User_RondButn.Size = new System.Drawing.Size(150, 150);
         this.User_RondButn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.User_RondButn.TabIndex = 12;
         this.User_RondButn.Tooltip = null;
         // 
         // FngrDev_Pb
         // 
         this.FngrDev_Pb.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.FngrDev_Pb.Image = global::System.DataGuard.Properties.Resources.IMAGE_1211;
         this.FngrDev_Pb.Location = new System.Drawing.Point(681, 642);
         this.FngrDev_Pb.Name = "FngrDev_Pb";
         this.FngrDev_Pb.Size = new System.Drawing.Size(40, 40);
         this.FngrDev_Pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.FngrDev_Pb.TabIndex = 19;
         this.FngrDev_Pb.TabStop = false;
         this.FngrDev_Pb.Visible = false;
         // 
         // label1
         // 
         this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.label1.Location = new System.Drawing.Point(242, 501);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(141, 15);
         this.label1.TabIndex = 9;
         this.label1.Text = "Your Active License Time:";
         // 
         // ActvLicnTime_LL
         // 
         this.ActvLicnTime_LL.ActiveLinkColor = System.Drawing.Color.Blue;
         this.ActvLicnTime_LL.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.ActvLicnTime_LL.AutoSize = true;
         this.ActvLicnTime_LL.Font = new System.Drawing.Font("Segoe UI", 9F);
         this.ActvLicnTime_LL.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
         this.ActvLicnTime_LL.Location = new System.Drawing.Point(400, 501);
         this.ActvLicnTime_LL.Name = "ActvLicnTime_LL";
         this.ActvLicnTime_LL.Size = new System.Drawing.Size(105, 15);
         this.ActvLicnTime_LL.TabIndex = 6;
         this.ActvLicnTime_LL.TabStop = true;
         this.ActvLicnTime_LL.Text = "9105-0001 - 1400/01/01";
         this.ActvLicnTime_LL.VisitedLinkColor = System.Drawing.Color.Blue;
         // 
         // Login
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
         this.Controls.Add(this.FngrDev_Pb);
         this.Controls.Add(this.SwitchUser_RondButn);
         this.Controls.Add(this.Cancel_Lbl);
         this.Controls.Add(this.pictureBox2);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label6);
         this.Controls.Add(this.User_RondButn);
         this.Controls.Add(this.linkLabel5);
         this.Controls.Add(this.linkLabel4);
         this.Controls.Add(this.ActvLicnTime_LL);
         this.Controls.Add(this.linkLabel3);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.linkLabel1);
         this.Controls.Add(this.linkLabel2);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.sb_login);
         this.Controls.Add(this.te_password);
         this.Controls.Add(this.te_username);
         this.Name = "Login";
         this.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.Size = new System.Drawing.Size(736, 695);
         ((System.ComponentModel.ISupportInitialize)(this.te_username.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.te_password.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.FngrDev_Pb)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.TextEdit te_username;
      private DevExpress.XtraEditors.TextEdit te_password;
      private DevExpress.XtraEditors.SimpleButton sb_login;
      private Windows.Forms.Label label2;
      private Windows.Forms.LinkLabel linkLabel2;
      private Windows.Forms.LinkLabel linkLabel1;
      private Windows.Forms.LinkLabel linkLabel3;
      private Windows.Forms.Label label3;
      private Windows.Forms.LinkLabel linkLabel4;
      private Windows.Forms.LinkLabel linkLabel5;
      private Windows.Forms.Label label4;
      private Windows.Forms.PictureBox pictureBox2;
      private Windows.Forms.Label label5;
      private Windows.Forms.Label label6;
      private MaxUi.RoundedButton User_RondButn;
      private Windows.Forms.Label Cancel_Lbl;
      private MaxUi.RoundedButton SwitchUser_RondButn;
      private Windows.Forms.PictureBox FngrDev_Pb;
      private Windows.Forms.Label label1;
      private Windows.Forms.LinkLabel ActvLicnTime_LL;
   }
}
