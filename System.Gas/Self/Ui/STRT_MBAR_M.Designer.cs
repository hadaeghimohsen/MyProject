namespace System.Gas.Self.Ui
{
   partial class STRT_MBAR_M
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STRT_MBAR_M));
         this.wbp_book_mark = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         this.SuspendLayout();
         // 
         // wbp_book_mark
         // 
         this.wbp_book_mark.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.wbp_book_mark.AppearanceButton.Hovered.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.wbp_book_mark.AppearanceButton.Hovered.Options.UseBackColor = true;
         this.wbp_book_mark.AppearanceButton.Normal.BackColor = System.Drawing.Color.White;
         this.wbp_book_mark.AppearanceButton.Normal.BackColor2 = System.Drawing.Color.White;
         this.wbp_book_mark.AppearanceButton.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.wbp_book_mark.AppearanceButton.Normal.ForeColor = System.Drawing.Color.White;
         this.wbp_book_mark.AppearanceButton.Normal.Options.UseBackColor = true;
         this.wbp_book_mark.AppearanceButton.Normal.Options.UseFont = true;
         this.wbp_book_mark.AppearanceButton.Normal.Options.UseForeColor = true;
         this.wbp_book_mark.AppearanceButton.Pressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.wbp_book_mark.AppearanceButton.Pressed.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.wbp_book_mark.AppearanceButton.Pressed.Options.UseBackColor = true;
         this.wbp_book_mark.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_book_mark.Buttons"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "0", -1, false, false)});
         this.wbp_book_mark.ContentAlignment = System.Drawing.ContentAlignment.MiddleLeft;
         this.wbp_book_mark.Dock = System.Windows.Forms.DockStyle.Fill;
         this.wbp_book_mark.Location = new System.Drawing.Point(0, 0);
         this.wbp_book_mark.Name = "wbp_book_mark";
         this.wbp_book_mark.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
         this.wbp_book_mark.Size = new System.Drawing.Size(800, 100);
         this.wbp_book_mark.TabIndex = 1;
         this.wbp_book_mark.Text = "windowsUIButtonPanel1";
         this.wbp_book_mark.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.wbp_book_mark_ButtonClick);
         // 
         // STRT_MBAR_M
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.RoyalBlue;
         this.Controls.Add(this.wbp_book_mark);
         this.Name = "STRT_MBAR_M";
         this.Size = new System.Drawing.Size(800, 100);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel wbp_book_mark;
   }
}
