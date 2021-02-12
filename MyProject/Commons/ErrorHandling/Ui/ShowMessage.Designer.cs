namespace MyProject.Commons.ErrorHandling.Ui
{
   partial class ShowMessage
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
         this.ShowMessage_Lbl = new System.Windows.Forms.Label();
         this.Waiting_Tm = new System.Windows.Forms.Timer();
         this.SuspendLayout();
         // 
         // ShowMessage_Lbl
         // 
         this.ShowMessage_Lbl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.ShowMessage_Lbl.Font = new System.Drawing.Font("IRANSans", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.ShowMessage_Lbl.ForeColor = System.Drawing.Color.White;
         this.ShowMessage_Lbl.Location = new System.Drawing.Point(0, 0);
         this.ShowMessage_Lbl.Name = "ShowMessage_Lbl";
         this.ShowMessage_Lbl.Size = new System.Drawing.Size(1314, 401);
         this.ShowMessage_Lbl.TabIndex = 3;
         this.ShowMessage_Lbl.Text = "لطفا کمی صبر کنید تا عملیات ذخیره سازی به اتمام برسد";
         this.ShowMessage_Lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // Waiting_Tm
         // 
         this.Waiting_Tm.Interval = 500;
         this.Waiting_Tm.Tick += new System.EventHandler(this.Waiting_Tm_Tick);
         // 
         // ShowMessage
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Highlight;
         this.Controls.Add(this.ShowMessage_Lbl);
         this.Name = "ShowMessage";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(1314, 401);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label ShowMessage_Lbl;
      private System.Windows.Forms.Timer Waiting_Tm;
   }
}
