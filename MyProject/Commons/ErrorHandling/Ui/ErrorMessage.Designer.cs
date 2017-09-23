namespace MyProject.Commons.ErrorHandling.Ui
{
   partial class ErrorMessage
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
         this.Submit_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.ErrorMessage_Lbl = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.Color.Salmon;
         this.label1.Image = global::MyProject.Properties.Resources.IMAGE_1004;
         this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.label1.Location = new System.Drawing.Point(402, 12);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(176, 52);
         this.label1.TabIndex = 1;
         this.label1.Text = "هشدار سیستم";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // Submit_Butn
         // 
         this.Submit_Butn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Submit_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Submit_Butn.Appearance.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Submit_Butn.Appearance.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
         this.Submit_Butn.Appearance.Options.UseBackColor = true;
         this.Submit_Butn.Appearance.Options.UseFont = true;
         this.Submit_Butn.Appearance.Options.UseForeColor = true;
         this.Submit_Butn.Location = new System.Drawing.Point(262, 159);
         this.Submit_Butn.LookAndFeel.SkinName = "Office 2010 Black";
         this.Submit_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Submit_Butn.Name = "Submit_Butn";
         this.Submit_Butn.Size = new System.Drawing.Size(113, 42);
         this.Submit_Butn.TabIndex = 2;
         this.Submit_Butn.Text = "متوجه شدم";
         this.Submit_Butn.Click += new System.EventHandler(this.Submit_Butn_Click);
         // 
         // ErrorMessage_Lbl
         // 
         this.ErrorMessage_Lbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ErrorMessage_Lbl.Font = new System.Drawing.Font("B Mitra", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.ErrorMessage_Lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(166)))));
         this.ErrorMessage_Lbl.Location = new System.Drawing.Point(50, 71);
         this.ErrorMessage_Lbl.Name = "ErrorMessage_Lbl";
         this.ErrorMessage_Lbl.Size = new System.Drawing.Size(521, 109);
         this.ErrorMessage_Lbl.TabIndex = 3;
         this.ErrorMessage_Lbl.Text = "دوره استفاده از تجهیزات باشگاه برای خانم قبادی، الینا در تاریخ 1395/07/23 به پایا" +
    "ن رسیده جهت تمدید دوره لطفا اقدام فرمایید";
         // 
         // ErrorMessage
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.WindowFrame;
         this.Controls.Add(this.Submit_Butn);
         this.Controls.Add(this.ErrorMessage_Lbl);
         this.Controls.Add(this.label1);
         this.Name = "ErrorMessage";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(625, 212);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.SimpleButton Submit_Butn;
      private System.Windows.Forms.Label ErrorMessage_Lbl;
   }
}
