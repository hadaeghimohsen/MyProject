namespace MyProject.Commons.HelpHandling.Ui
{
   partial class HelpHandle
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
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.Wb_HelpBrowser = new System.Windows.Forms.WebBrowser();
         this.SuspendLayout();
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Kamran", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.LineColor = System.Drawing.Color.Black;
         this.labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Bottom;
         this.labelControl1.LineOrientation = DevExpress.XtraEditors.LabelLineOrientation.Horizontal;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(15, 12);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(325, 47);
         this.labelControl1.TabIndex = 0;
         this.labelControl1.Text = "راهنما";
         // 
         // Wb_HelpBrowser
         // 
         this.Wb_HelpBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.Wb_HelpBrowser.Location = new System.Drawing.Point(15, 76);
         this.Wb_HelpBrowser.MinimumSize = new System.Drawing.Size(20, 20);
         this.Wb_HelpBrowser.Name = "Wb_HelpBrowser";
         this.Wb_HelpBrowser.Size = new System.Drawing.Size(325, 616);
         this.Wb_HelpBrowser.TabIndex = 1;
         // 
         // HelpHandle
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DarkSlateBlue;
         this.Controls.Add(this.Wb_HelpBrowser);
         this.Controls.Add(this.labelControl1);
         this.Name = "HelpHandle";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(363, 719);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl labelControl1;
      private System.Windows.Forms.WebBrowser Wb_HelpBrowser;
   }
}
