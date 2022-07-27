namespace MyProject.Programs.Ui
{
    partial class Wall
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Wall));
         this.SysNtfy_Ni = new System.Windows.Forms.NotifyIcon();
         this.SuspendLayout();
         // 
         // SysNtfy_Ni
         // 
         this.SysNtfy_Ni.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
         this.SysNtfy_Ni.BalloonTipText = "اینترنت دستگاه شما جهت ارسال پیام غیرفعال یا قطع می باشد";
         this.SysNtfy_Ni.BalloonTipTitle = "وضعیت اتصال اینترنتی";
         this.SysNtfy_Ni.Icon = ((System.Drawing.Icon)(resources.GetObject("SysNtfy_Ni.Icon")));
         this.SysNtfy_Ni.Text = "اطلاع رسانی رله سافت";
         this.SysNtfy_Ni.Visible = true;
         // 
         // Wall
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
         this.AutoScroll = true;
         this.ClientSize = new System.Drawing.Size(527, 395);
         this.DoubleBuffered = true;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "Wall";
         this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Wall_FormClosing);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon SysNtfy_Ni;


    }
}