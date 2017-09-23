namespace MyProject.Commons.ErrorHandling.Ui
{
   partial class ErrorHandle
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
         this.label1 = new System.Windows.Forms.Label();
         this.Wb_ErrorHandle = new System.Windows.Forms.WebBrowser();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Font = new System.Drawing.Font("Palatino Linotype", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Image = global::MyProject.Properties.Resources.IMAGE_1004;
         this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.label1.Location = new System.Drawing.Point(17, 11);
         this.label1.Name = "label1";
         this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.label1.Size = new System.Drawing.Size(294, 69);
         this.label1.TabIndex = 0;
         this.label1.Text = "Error Detection";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Wb_ErrorHandle
         // 
         this.Wb_ErrorHandle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Wb_ErrorHandle.Location = new System.Drawing.Point(22, 92);
         this.Wb_ErrorHandle.MinimumSize = new System.Drawing.Size(20, 20);
         this.Wb_ErrorHandle.Name = "Wb_ErrorHandle";
         this.Wb_ErrorHandle.Size = new System.Drawing.Size(878, 231);
         this.Wb_ErrorHandle.TabIndex = 1;
         // 
         // ErrorHandle
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DarkSalmon;
         this.Controls.Add(this.Wb_ErrorHandle);
         this.Controls.Add(this.label1);
         this.Name = "ErrorHandle";
         this.Size = new System.Drawing.Size(921, 342);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.WebBrowser Wb_ErrorHandle;
   }
}
