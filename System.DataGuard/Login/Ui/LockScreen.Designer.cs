namespace System.DataGuard.Login.Ui
{
   partial class LockScreen
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
         this.LB_DATE = new DevExpress.XtraEditors.LabelControl();
         this.LB_TIME = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // LB_DATE
         // 
         this.LB_DATE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.LB_DATE.Appearance.Font = new System.Drawing.Font("B Kamran", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.LB_DATE.Appearance.ForeColor = System.Drawing.Color.Honeydew;
         this.LB_DATE.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.LB_DATE.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.LB_DATE.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.LB_DATE.Location = new System.Drawing.Point(51, 617);
         this.LB_DATE.Name = "LB_DATE";
         this.LB_DATE.Size = new System.Drawing.Size(408, 68);
         this.LB_DATE.TabIndex = 1;
         this.LB_DATE.Text = "جمعه 25 تیرماه 1395";
         // 
         // LB_TIME
         // 
         this.LB_TIME.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.LB_TIME.AutoSize = true;
         this.LB_TIME.BackColor = System.Drawing.Color.Transparent;
         this.LB_TIME.Font = new System.Drawing.Font("Segoe UI Light", 120F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LB_TIME.ForeColor = System.Drawing.Color.Honeydew;
         this.LB_TIME.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
         this.LB_TIME.Location = new System.Drawing.Point(3, 420);
         this.LB_TIME.Name = "LB_TIME";
         this.LB_TIME.Size = new System.Drawing.Size(456, 212);
         this.LB_TIME.TabIndex = 0;
         this.LB_TIME.Text = "03:45";
         // 
         // LockScreen
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.WindowFrame;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.Controls.Add(this.LB_DATE);
         this.Controls.Add(this.LB_TIME);
         this.DoubleBuffered = true;
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "LockScreen";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(927, 704);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.Label LB_TIME;
      private DevExpress.XtraEditors.LabelControl LB_DATE;
   }
}
