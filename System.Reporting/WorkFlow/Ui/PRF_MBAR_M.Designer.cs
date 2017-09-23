namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_MBAR_M
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PRF_MBAR_M));
         this.wbp_profiler_desktop = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         this.SuspendLayout();
         // 
         // wbp_profiler_desktop
         // 
         this.wbp_profiler_desktop.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.wbp_profiler_desktop.AppearanceButton.Hovered.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.wbp_profiler_desktop.AppearanceButton.Hovered.Options.UseBackColor = true;
         this.wbp_profiler_desktop.AppearanceButton.Normal.BackColor = System.Drawing.Color.White;
         this.wbp_profiler_desktop.AppearanceButton.Normal.BackColor2 = System.Drawing.Color.White;
         this.wbp_profiler_desktop.AppearanceButton.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.wbp_profiler_desktop.AppearanceButton.Normal.ForeColor = System.Drawing.Color.Maroon;
         this.wbp_profiler_desktop.AppearanceButton.Normal.Options.UseBackColor = true;
         this.wbp_profiler_desktop.AppearanceButton.Normal.Options.UseFont = true;
         this.wbp_profiler_desktop.AppearanceButton.Normal.Options.UseForeColor = true;
         this.wbp_profiler_desktop.AppearanceButton.Pressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.wbp_profiler_desktop.AppearanceButton.Pressed.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.wbp_profiler_desktop.AppearanceButton.Pressed.Options.UseBackColor = true;
         this.wbp_profiler_desktop.ButtonInterval = 30;
         this.wbp_profiler_desktop.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("ایجاد ترکیب", ((System.Drawing.Image)(resources.GetObject("wbp_profiler_desktop.Buttons"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "0", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("تغییر نام", ((System.Drawing.Image)(resources.GetObject("wbp_profiler_desktop.Buttons1"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "1", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("تنظیمات", ((System.Drawing.Image)(resources.GetObject("wbp_profiler_desktop.Buttons2"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "2", -1, false, false)});
         this.wbp_profiler_desktop.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
         this.wbp_profiler_desktop.Dock = System.Windows.Forms.DockStyle.Fill;
         this.wbp_profiler_desktop.Location = new System.Drawing.Point(0, 0);
         this.wbp_profiler_desktop.Name = "wbp_profiler_desktop";
         this.wbp_profiler_desktop.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
         this.wbp_profiler_desktop.Size = new System.Drawing.Size(800, 100);
         this.wbp_profiler_desktop.TabIndex = 1;
         this.wbp_profiler_desktop.Text = "windowsUIButtonPanel1";
         this.wbp_profiler_desktop.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.wbp_profiler_desktop_ButtonClick);
         // 
         // PRF_MBAR_M
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Coral;
         this.Controls.Add(this.wbp_profiler_desktop);
         this.Name = "PRF_MBAR_M";
         this.Size = new System.Drawing.Size(800, 100);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel wbp_profiler_desktop;
   }
}
