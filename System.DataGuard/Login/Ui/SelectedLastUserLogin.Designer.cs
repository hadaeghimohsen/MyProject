namespace System.DataGuard.Login.Ui
{
   partial class SelectedLastUserLogin
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectedLastUserLogin));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         this.User_Txt = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.Password_Be = new DevExpress.XtraEditors.ButtonEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.Lb_ShowLoginDesc = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.pictureBox1 = new System.Windows.Forms.PictureBox();
         this.CheckValidation_RondButn = new System.MaxUi.RoundedButton();
         this.Cancel_RondButn = new System.MaxUi.RoundedButton();
         this.User_RondButn = new System.MaxUi.RoundedButton();
         this.ErrorValidation_Lbl = new System.Windows.Forms.Label();
         this.pictureBox2 = new System.Windows.Forms.PictureBox();
         this.label2 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.Password_Be.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
         this.SuspendLayout();
         // 
         // User_Txt
         // 
         this.User_Txt.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.User_Txt.Font = new System.Drawing.Font("Consolas", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.User_Txt.ForeColor = System.Drawing.Color.White;
         this.User_Txt.Location = new System.Drawing.Point(264, 331);
         this.User_Txt.Name = "User_Txt";
         this.User_Txt.Size = new System.Drawing.Size(235, 34);
         this.User_Txt.TabIndex = 4;
         this.User_Txt.Text = "mohsen";
         this.User_Txt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // label4
         // 
         this.label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.label4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.ForeColor = System.Drawing.Color.White;
         this.label4.Location = new System.Drawing.Point(331, 618);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(100, 23);
         this.label4.TabIndex = 4;
         this.label4.Text = "Cancel";
         this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Password_Be
         // 
         this.Password_Be.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.Password_Be.EditValue = "";
         this.Password_Be.Location = new System.Drawing.Point(264, 374);
         this.Password_Be.Name = "Password_Be";
         this.Password_Be.Properties.Appearance.BackColor = System.Drawing.Color.White;
         this.Password_Be.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Password_Be.Properties.Appearance.Options.UseBackColor = true;
         this.Password_Be.Properties.Appearance.Options.UseFont = true;
         this.Password_Be.Properties.AppearanceFocused.BorderColor = System.Drawing.Color.Gold;
         this.Password_Be.Properties.AppearanceFocused.Options.UseBorderColor = true;
         this.Password_Be.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("Password_Be.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
         this.Password_Be.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Password_Be.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Password_Be.Properties.NullValuePrompt = "Enter Password";
         this.Password_Be.Properties.NullValuePromptShowForEmptyValue = true;
         this.Password_Be.Properties.UseSystemPasswordChar = true;
         this.Password_Be.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.Password_Be.Size = new System.Drawing.Size(235, 26);
         this.Password_Be.TabIndex = 6;
         this.Password_Be.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.Password_Be_ButtonPressed);
         this.Password_Be.TextChanged += new System.EventHandler(this.Password_Be_TextChanged);
         this.Password_Be.Enter += new System.EventHandler(this.Control_Enter);
         this.Password_Be.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputValidation);
         this.Password_Be.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Password_Be_MouseUp);
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.White;
         this.label1.Location = new System.Drawing.Point(690, 2);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(69, 23);
         this.label1.TabIndex = 4;
         this.label1.Text = "12:45 pm";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Lb_ShowLoginDesc
         // 
         this.Lb_ShowLoginDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Lb_ShowLoginDesc.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Lb_ShowLoginDesc.ForeColor = System.Drawing.Color.White;
         this.Lb_ShowLoginDesc.Location = new System.Drawing.Point(39, 496);
         this.Lb_ShowLoginDesc.Name = "Lb_ShowLoginDesc";
         this.Lb_ShowLoginDesc.Size = new System.Drawing.Size(685, 58);
         this.Lb_ShowLoginDesc.TabIndex = 4;
         this.Lb_ShowLoginDesc.Text = "برای رسیدن به بهترین ها، باید بسیار تلاش کرد";
         this.Lb_ShowLoginDesc.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // label3
         // 
         this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.ForeColor = System.Drawing.Color.White;
         this.label3.Location = new System.Drawing.Point(39, 106);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(685, 32);
         this.label3.TabIndex = 4;
         this.label3.Text = "Anar Team Corporation \r\nCellphone : 0917 101 5031 - Telephone : 3 842 1422";
         this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // pictureBox1
         // 
         this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.pictureBox1.Image = global::System.DataGuard.Properties.Resources.IMAGE_1483;
         this.pictureBox1.Location = new System.Drawing.Point(356, 53);
         this.pictureBox1.Name = "pictureBox1";
         this.pictureBox1.Size = new System.Drawing.Size(50, 50);
         this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.pictureBox1.TabIndex = 7;
         this.pictureBox1.TabStop = false;
         // 
         // CheckValidation_RondButn
         // 
         this.CheckValidation_RondButn.Active = true;
         this.CheckValidation_RondButn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.CheckValidation_RondButn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Rectangle;
         this.CheckValidation_RondButn.Caption = "";
         this.CheckValidation_RondButn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.CheckValidation_RondButn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.CheckValidation_RondButn.HoverBorderColor = System.Drawing.Color.Chartreuse;
         this.CheckValidation_RondButn.HoverColorA = System.Drawing.Color.Transparent;
         this.CheckValidation_RondButn.HoverColorB = System.Drawing.Color.Transparent;
         this.CheckValidation_RondButn.ImageProfile = global::System.DataGuard.Properties.Resources.IMAGE_1487;
         this.CheckValidation_RondButn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.CheckValidation_RondButn.ImageVisiable = true;
         this.CheckValidation_RondButn.Location = new System.Drawing.Point(499, 373);
         this.CheckValidation_RondButn.Name = "CheckValidation_RondButn";
         this.CheckValidation_RondButn.NormalBorderColor = System.Drawing.Color.White;
         this.CheckValidation_RondButn.NormalColorA = System.Drawing.Color.Transparent;
         this.CheckValidation_RondButn.NormalColorB = System.Drawing.Color.Transparent;
         this.CheckValidation_RondButn.Size = new System.Drawing.Size(28, 28);
         this.CheckValidation_RondButn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.CheckValidation_RondButn.TabIndex = 8;
         this.CheckValidation_RondButn.Click += new System.EventHandler(this.CheckValidation_RondButn_Click);
         // 
         // Cancel_RondButn
         // 
         this.Cancel_RondButn.Active = true;
         this.Cancel_RondButn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Cancel_RondButn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.Cancel_RondButn.Caption = "";
         this.Cancel_RondButn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.Cancel_RondButn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.Cancel_RondButn.HoverBorderColor = System.Drawing.Color.DarkGoldenrod;
         this.Cancel_RondButn.HoverColorA = System.Drawing.Color.Transparent;
         this.Cancel_RondButn.HoverColorB = System.Drawing.Color.Transparent;
         this.Cancel_RondButn.ImageProfile = global::System.DataGuard.Properties.Resources.IMAGE_1484;
         this.Cancel_RondButn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.Cancel_RondButn.ImageVisiable = true;
         this.Cancel_RondButn.Location = new System.Drawing.Point(364, 580);
         this.Cancel_RondButn.Name = "Cancel_RondButn";
         this.Cancel_RondButn.NormalBorderColor = System.Drawing.Color.White;
         this.Cancel_RondButn.NormalColorA = System.Drawing.Color.Transparent;
         this.Cancel_RondButn.NormalColorB = System.Drawing.Color.Transparent;
         this.Cancel_RondButn.Size = new System.Drawing.Size(35, 35);
         this.Cancel_RondButn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.Cancel_RondButn.TabIndex = 1;
         this.Cancel_RondButn.Click += new System.EventHandler(this.Cancel_RondButn_Click);
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
         this.User_RondButn.ImageProfile = global::System.DataGuard.Properties.Resources.IMAGE_1486;
         this.User_RondButn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.User_RondButn.ImageVisiable = true;
         this.User_RondButn.Location = new System.Drawing.Point(306, 173);
         this.User_RondButn.Name = "User_RondButn";
         this.User_RondButn.NormalBorderColor = System.Drawing.Color.Black;
         this.User_RondButn.NormalColorA = System.Drawing.Color.White;
         this.User_RondButn.NormalColorB = System.Drawing.Color.White;
         this.User_RondButn.Size = new System.Drawing.Size(150, 150);
         this.User_RondButn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.User_RondButn.TabIndex = 1;
         // 
         // ErrorValidation_Lbl
         // 
         this.ErrorValidation_Lbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.ErrorValidation_Lbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.ErrorValidation_Lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
         this.ErrorValidation_Lbl.Location = new System.Drawing.Point(291, 403);
         this.ErrorValidation_Lbl.Name = "ErrorValidation_Lbl";
         this.ErrorValidation_Lbl.Size = new System.Drawing.Size(181, 23);
         this.ErrorValidation_Lbl.TabIndex = 4;
         this.ErrorValidation_Lbl.Text = "رمز عبور نادرست می باشد";
         this.ErrorValidation_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ErrorValidation_Lbl.Visible = false;
         // 
         // pictureBox2
         // 
         this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.pictureBox2.Image = global::System.DataGuard.Properties.Resources.IMAGE_1551;
         this.pictureBox2.Location = new System.Drawing.Point(16, 601);
         this.pictureBox2.Name = "pictureBox2";
         this.pictureBox2.Size = new System.Drawing.Size(40, 40);
         this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.pictureBox2.TabIndex = 10;
         this.pictureBox2.TabStop = false;
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.ForeColor = System.Drawing.Color.White;
         this.label2.Location = new System.Drawing.Point(62, 627);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(158, 14);
         this.label2.TabIndex = 9;
         this.label2.Text = "Power By : Job Routing 1.0";
         this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         // 
         // SelectedLastUserLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DarkSlateBlue;
         this.Controls.Add(this.pictureBox2);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.CheckValidation_RondButn);
         this.Controls.Add(this.pictureBox1);
         this.Controls.Add(this.Password_Be);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.Lb_ShowLoginDesc);
         this.Controls.Add(this.ErrorValidation_Lbl);
         this.Controls.Add(this.User_Txt);
         this.Controls.Add(this.Cancel_RondButn);
         this.Controls.Add(this.User_RondButn);
         this.Name = "SelectedLastUserLogin";
         this.Size = new System.Drawing.Size(762, 660);
         ((System.ComponentModel.ISupportInitialize)(this.Password_Be.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private MaxUi.RoundedButton User_RondButn;
        private Windows.Forms.Label User_Txt;
        private MaxUi.RoundedButton Cancel_RondButn;
        private Windows.Forms.Label label4;
        private DevExpress.XtraEditors.ButtonEdit Password_Be;
        private Windows.Forms.Label label1;
        private Windows.Forms.PictureBox pictureBox1;
        private Windows.Forms.Label Lb_ShowLoginDesc;
        private Windows.Forms.Label label3;
        private MaxUi.RoundedButton CheckValidation_RondButn;
        private Windows.Forms.Label ErrorValidation_Lbl;
        private Windows.Forms.PictureBox pictureBox2;
        private Windows.Forms.Label label2;




    }
}
