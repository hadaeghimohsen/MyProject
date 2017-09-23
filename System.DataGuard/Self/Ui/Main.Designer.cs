namespace System.DataGuard.Self.Ui
{
   partial class Main
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
         this.sb_securitysettings = new DevExpress.XtraEditors.SimpleButton();
         this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
         this.SuspendLayout();
         // 
         // sb_securitysettings
         // 
         this.sb_securitysettings.Appearance.BackColor = System.Drawing.Color.Blue;
         this.sb_securitysettings.Appearance.Font = new System.Drawing.Font("B Kamran", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_securitysettings.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_securitysettings.Appearance.Options.UseBackColor = true;
         this.sb_securitysettings.Appearance.Options.UseFont = true;
         this.sb_securitysettings.Appearance.Options.UseForeColor = true;
         this.sb_securitysettings.Location = new System.Drawing.Point(34, 39);
         this.sb_securitysettings.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_securitysettings.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_securitysettings.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_securitysettings.Name = "sb_securitysettings";
         this.sb_securitysettings.Size = new System.Drawing.Size(273, 42);
         this.sb_securitysettings.TabIndex = 2;
         this.sb_securitysettings.Text = "تنظیمات پیشرفته امنیتی";
         this.sb_securitysettings.Click += new System.EventHandler(this.sb_securitysettings_Click);
         // 
         // simpleButton1
         // 
         this.simpleButton1.Appearance.BackColor = System.Drawing.Color.Blue;
         this.simpleButton1.Appearance.Font = new System.Drawing.Font("B Kamran", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.simpleButton1.Appearance.ForeColor = System.Drawing.Color.White;
         this.simpleButton1.Appearance.Options.UseBackColor = true;
         this.simpleButton1.Appearance.Options.UseFont = true;
         this.simpleButton1.Appearance.Options.UseForeColor = true;
         this.simpleButton1.Location = new System.Drawing.Point(34, 98);
         this.simpleButton1.LookAndFeel.SkinName = "Office 2010 Silver";
         this.simpleButton1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.simpleButton1.LookAndFeel.UseDefaultLookAndFeel = false;
         this.simpleButton1.Name = "simpleButton1";
         this.simpleButton1.Size = new System.Drawing.Size(273, 42);
         this.simpleButton1.TabIndex = 3;
         this.simpleButton1.Text = "تنظیمات کاربر فعلی";
         this.simpleButton1.Visible = false;
         // 
         // Main
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.Controls.Add(this.simpleButton1);
         this.Controls.Add(this.sb_securitysettings);
         this.Name = "Main";
         this.Size = new System.Drawing.Size(340, 180);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.SimpleButton sb_securitysettings;
      private DevExpress.XtraEditors.SimpleButton simpleButton1;
   }
}
