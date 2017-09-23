namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class CurrentChangeUserName
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
         this.Txt_UserName = new DevExpress.XtraEditors.TextEdit();
         this.label4 = new System.Windows.Forms.Label();
         this.label5 = new System.Windows.Forms.Label();
         this.Txt_UserDesc = new DevExpress.XtraEditors.TextEdit();
         this.label6 = new System.Windows.Forms.Label();
         this.Btn_Cancel = new System.MaxUi.NewMaxBtn();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_UserName.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_UserDesc.Properties)).BeginInit();
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
         this.Btn_OK.Location = new System.Drawing.Point(51, 371);
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
         this.label1.Location = new System.Drawing.Point(447, 31);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(144, 45);
         this.label1.TabIndex = 8;
         this.label1.Text = "تغییر نام کاربری";
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
         // Txt_UserName
         // 
         this.Txt_UserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Txt_UserName.Location = new System.Drawing.Point(217, 203);
         this.Txt_UserName.Name = "Txt_UserName";
         this.Txt_UserName.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Txt_UserName.Properties.Appearance.Options.UseFont = true;
         this.Txt_UserName.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Txt_UserName.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Txt_UserName.Properties.NullValuePrompt = "نام کاربری";
         this.Txt_UserName.Properties.NullValuePromptShowForEmptyValue = true;
         this.Txt_UserName.Size = new System.Drawing.Size(336, 30);
         this.Txt_UserName.TabIndex = 0;
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label4.ForeColor = System.Drawing.Color.DarkBlue;
         this.label4.Location = new System.Drawing.Point(27, 236);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(529, 16);
         this.label4.TabIndex = 8;
         this.label4.Text = "نام کاربری همان شناسایی سیستمی می باشد که برای ورود به سیستم از آن استفاده میکنید" +
    ".";
         // 
         // label5
         // 
         this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label5.ForeColor = System.Drawing.Color.DarkBlue;
         this.label5.Location = new System.Drawing.Point(27, 288);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(526, 38);
         this.label5.TabIndex = 8;
         this.label5.Text = "نام کاربر که به صورت نام و نام خانوادگی زده می شود که برای خوش آمد گویی از آن است" +
    "فاده میکنیم.";
         // 
         // Txt_UserDesc
         // 
         this.Txt_UserDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.Txt_UserDesc.Location = new System.Drawing.Point(217, 255);
         this.Txt_UserDesc.Name = "Txt_UserDesc";
         this.Txt_UserDesc.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Txt_UserDesc.Properties.Appearance.Options.UseFont = true;
         this.Txt_UserDesc.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.Txt_UserDesc.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Txt_UserDesc.Properties.NullValuePrompt = "نام و نام خانوادگی";
         this.Txt_UserDesc.Properties.NullValuePromptShowForEmptyValue = true;
         this.Txt_UserDesc.Size = new System.Drawing.Size(336, 30);
         this.Txt_UserDesc.TabIndex = 1;
         // 
         // label6
         // 
         this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label6.BackColor = System.Drawing.Color.DarkGray;
         this.label6.Location = new System.Drawing.Point(51, 340);
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
         this.Btn_Cancel.Location = new System.Drawing.Point(137, 371);
         this.Btn_Cancel.Name = "Btn_Cancel";
         this.Btn_Cancel.Size = new System.Drawing.Size(80, 25);
         this.Btn_Cancel.TabIndex = 2;
         this.Btn_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Cancel.TextColor = System.Drawing.Color.Black;
         this.Btn_Cancel.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Btn_Cancel.Click += new System.EventHandler(this.Btn_Cancel_Click);
         // 
         // CurrentChangeUserName
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.LightGray;
         this.Controls.Add(this.label6);
         this.Controls.Add(this.Txt_UserDesc);
         this.Controls.Add(this.Txt_UserName);
         this.Controls.Add(this.Btn_Cancel);
         this.Controls.Add(this.Btn_OK);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.LB_CurrentUserName);
         this.Controls.Add(this.label5);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.label1);
         this.Name = "CurrentChangeUserName";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(655, 572);
         ((System.ComponentModel.ISupportInitialize)(this.Txt_UserName.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Txt_UserDesc.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private MaxUi.NewMaxBtn Btn_OK;
      private Windows.Forms.Label label2;
      private Windows.Forms.Label LB_CurrentUserName;
      private Windows.Forms.Label label3;
      private Windows.Forms.Label label1;
      private DevExpress.XtraEditors.TextEdit Txt_UserName;
      private Windows.Forms.Label label4;
      private Windows.Forms.Label label5;
      private DevExpress.XtraEditors.TextEdit Txt_UserDesc;
      private Windows.Forms.Label label6;
      private MaxUi.NewMaxBtn Btn_Cancel;
   }
}
