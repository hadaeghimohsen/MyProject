namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class POSConfig
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
         this.Btn_Back = new System.Windows.Forms.Button();
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.tabPage2 = new System.Windows.Forms.TabPage();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         this.SuspendLayout();
         // 
         // Btn_Back
         // 
         this.Btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
         this.Btn_Back.Location = new System.Drawing.Point(402, 581);
         this.Btn_Back.Name = "Btn_Back";
         this.Btn_Back.Size = new System.Drawing.Size(75, 23);
         this.Btn_Back.TabIndex = 2;
         this.Btn_Back.Text = "بازگشت";
         this.Btn_Back.UseVisualStyleBackColor = true;
         // 
         // tabControl1
         // 
         this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Controls.Add(this.tabPage2);
         this.tabControl1.Location = new System.Drawing.Point(3, 59);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.RightToLeftLayout = true;
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(872, 516);
         this.tabControl1.TabIndex = 3;
         // 
         // tabPage1
         // 
         this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
         this.tabPage1.Controls.Add(this.label2);
         this.tabPage1.Location = new System.Drawing.Point(4, 23);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(864, 489);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "پیکربندی";
         // 
         // tabPage2
         // 
         this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
         this.tabPage2.Location = new System.Drawing.Point(4, 23);
         this.tabPage2.Name = "tabPage2";
         this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage2.Size = new System.Drawing.Size(864, 489);
         this.tabPage2.TabIndex = 1;
         this.tabPage2.Text = "گزارش عملکرد دستگاه ها و بخشها";
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label2.Image = global::System.DataGuard.Properties.Resources.IMAGE_1151;
         this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.label2.Location = new System.Drawing.Point(571, 3);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(287, 56);
         this.label2.TabIndex = 0;
         this.label2.Text = "ثبت اطلاعات دستگاه های کارت خوان بانک ها";
         this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.Font = new System.Drawing.Font("B Koodak", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.Image = global::System.DataGuard.Properties.Resources.IMAGE_1089;
         this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.label1.Location = new System.Drawing.Point(535, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(340, 56);
         this.label1.TabIndex = 1;
         this.label1.Text = "پیکربندی دستگاه های کارت خوان";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // POSConfig
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tabControl1);
         this.Controls.Add(this.Btn_Back);
         this.Controls.Add(this.label1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "POSConfig";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(878, 616);
         this.tabControl1.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Label label1;
      private Windows.Forms.Button Btn_Back;
      private Windows.Forms.TabControl tabControl1;
      private Windows.Forms.TabPage tabPage1;
      private Windows.Forms.TabPage tabPage2;
      private Windows.Forms.Label label2;
   }
}
