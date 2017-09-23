namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class PropertySingleMenu
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
         this.sb_profile = new DevExpress.XtraEditors.SimpleButton();
         this.sb_create = new DevExpress.XtraEditors.SimpleButton();
         this.sb_duplicate = new DevExpress.XtraEditors.SimpleButton();
         this.SuspendLayout();
         // 
         // sb_profile
         // 
         this.sb_profile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_profile.Appearance.BackColor = System.Drawing.Color.Blue;
         this.sb_profile.Appearance.Font = new System.Drawing.Font("B Kamran", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_profile.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_profile.Appearance.Options.UseBackColor = true;
         this.sb_profile.Appearance.Options.UseFont = true;
         this.sb_profile.Appearance.Options.UseForeColor = true;
         this.sb_profile.Location = new System.Drawing.Point(18, 16);
         this.sb_profile.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_profile.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_profile.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_profile.Name = "sb_profile";
         this.sb_profile.Size = new System.Drawing.Size(273, 42);
         this.sb_profile.TabIndex = 1;
         this.sb_profile.Text = "اطلاعات مربوط به کاربری";
         this.sb_profile.Click += new System.EventHandler(this.sb_profile_Click);
         // 
         // sb_create
         // 
         this.sb_create.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_create.Appearance.BackColor = System.Drawing.Color.Blue;
         this.sb_create.Appearance.Font = new System.Drawing.Font("B Kamran", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_create.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_create.Appearance.Options.UseBackColor = true;
         this.sb_create.Appearance.Options.UseFont = true;
         this.sb_create.Appearance.Options.UseForeColor = true;
         this.sb_create.Location = new System.Drawing.Point(18, 64);
         this.sb_create.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_create.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_create.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_create.Name = "sb_create";
         this.sb_create.Size = new System.Drawing.Size(273, 42);
         this.sb_create.TabIndex = 2;
         this.sb_create.Text = "ایجاد کاربر جدید";
         this.sb_create.Click += new System.EventHandler(this.sb_create_Click);
         // 
         // sb_duplicate
         // 
         this.sb_duplicate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_duplicate.Appearance.BackColor = System.Drawing.Color.Blue;
         this.sb_duplicate.Appearance.Font = new System.Drawing.Font("B Kamran", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_duplicate.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_duplicate.Appearance.Options.UseBackColor = true;
         this.sb_duplicate.Appearance.Options.UseFont = true;
         this.sb_duplicate.Appearance.Options.UseForeColor = true;
         this.sb_duplicate.Location = new System.Drawing.Point(18, 112);
         this.sb_duplicate.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_duplicate.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_duplicate.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_duplicate.Name = "sb_duplicate";
         this.sb_duplicate.Size = new System.Drawing.Size(273, 42);
         this.sb_duplicate.TabIndex = 4;
         this.sb_duplicate.Text = "نمونه برداری از کاربر";
         this.sb_duplicate.Click += new System.EventHandler(this.sb_duplicate_Click);
         // 
         // PropertySingleMenu
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlDark;
         this.Controls.Add(this.sb_duplicate);
         this.Controls.Add(this.sb_create);
         this.Controls.Add(this.sb_profile);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "PropertySingleMenu";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(311, 170);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton sb_profile;
      private DevExpress.XtraEditors.SimpleButton sb_create;
      private DevExpress.XtraEditors.SimpleButton sb_duplicate;
   }
}
