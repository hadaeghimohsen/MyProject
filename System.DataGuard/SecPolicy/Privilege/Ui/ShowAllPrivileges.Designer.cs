namespace System.DataGuard.SecPolicy.Privilege.Ui
{
   partial class ShowAllPrivileges
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
         this.listView2 = new System.Windows.Forms.ListView();
         this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
         this.textBox3 = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // listView2
         // 
         this.listView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
         this.listView2.Location = new System.Drawing.Point(41, 105);
         this.listView2.Name = "listView2";
         this.listView2.RightToLeftLayout = true;
         this.listView2.Size = new System.Drawing.Size(505, 283);
         this.listView2.TabIndex = 23;
         this.listView2.UseCompatibleStateImageBehavior = false;
         this.listView2.View = System.Windows.Forms.View.Details;
         // 
         // columnHeader2
         // 
         this.columnHeader2.Text = "نام سطح دسترسی";
         this.columnHeader2.Width = 363;
         // 
         // textBox3
         // 
         this.textBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.textBox3.BackColor = System.Drawing.Color.AliceBlue;
         this.textBox3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.textBox3.Location = new System.Drawing.Point(41, 77);
         this.textBox3.Name = "textBox3";
         this.textBox3.Size = new System.Drawing.Size(505, 22);
         this.textBox3.TabIndex = 22;
         // 
         // ShowAllPrivileges
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.listView2);
         this.Controls.Add(this.textBox3);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "ShowAllPrivileges";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(587, 422);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.ListView listView2;
      private Windows.Forms.ColumnHeader columnHeader2;
      private Windows.Forms.TextBox textBox3;
   }
}
