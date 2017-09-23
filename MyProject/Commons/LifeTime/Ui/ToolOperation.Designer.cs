namespace MyProject.Commons.LifeTime.Ui
{
   partial class ToolOperation
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
         this.lb_title = new DevExpress.XtraEditors.LabelControl();
         this.lb_caption = new System.Windows.Forms.Label();
         this.btn_cancel = new DevExpress.XtraEditors.SimpleButton();
         this.btn_accept = new DevExpress.XtraEditors.SimpleButton();
         this.SuspendLayout();
         // 
         // lb_title
         // 
         this.lb_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lb_title.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.lb_title.Appearance.ForeColor = System.Drawing.Color.White;
         this.lb_title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.lb_title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.lb_title.LineColor = System.Drawing.Color.Black;
         this.lb_title.LineVisible = true;
         this.lb_title.Location = new System.Drawing.Point(3, 3);
         this.lb_title.Name = "lb_title";
         this.lb_title.Size = new System.Drawing.Size(480, 32);
         this.lb_title.TabIndex = 34;
         this.lb_title.Text = "Title";
         // 
         // lb_caption
         // 
         this.lb_caption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.lb_caption.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.lb_caption.ForeColor = System.Drawing.Color.White;
         this.lb_caption.Location = new System.Drawing.Point(35, 39);
         this.lb_caption.Name = "lb_caption";
         this.lb_caption.Size = new System.Drawing.Size(425, 65);
         this.lb_caption.TabIndex = 35;
         this.lb_caption.Text = "Caption";
         // 
         // btn_cancel
         // 
         this.btn_cancel.Appearance.BackColor = System.Drawing.Color.Blue;
         this.btn_cancel.Appearance.Font = new System.Drawing.Font("B Kamran", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.btn_cancel.Appearance.ForeColor = System.Drawing.Color.White;
         this.btn_cancel.Appearance.Options.UseBackColor = true;
         this.btn_cancel.Appearance.Options.UseFont = true;
         this.btn_cancel.Appearance.Options.UseForeColor = true;
         this.btn_cancel.Location = new System.Drawing.Point(35, 114);
         this.btn_cancel.LookAndFeel.SkinName = "Office 2010 Silver";
         this.btn_cancel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.btn_cancel.LookAndFeel.UseDefaultLookAndFeel = false;
         this.btn_cancel.Name = "btn_cancel";
         this.btn_cancel.Size = new System.Drawing.Size(119, 42);
         this.btn_cancel.TabIndex = 0;
         this.btn_cancel.Text = "خیر";
         this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
         // 
         // btn_accept
         // 
         this.btn_accept.Appearance.BackColor = System.Drawing.Color.Blue;
         this.btn_accept.Appearance.Font = new System.Drawing.Font("B Kamran", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.btn_accept.Appearance.ForeColor = System.Drawing.Color.White;
         this.btn_accept.Appearance.Options.UseBackColor = true;
         this.btn_accept.Appearance.Options.UseFont = true;
         this.btn_accept.Appearance.Options.UseForeColor = true;
         this.btn_accept.Location = new System.Drawing.Point(337, 114);
         this.btn_accept.LookAndFeel.SkinName = "Office 2010 Silver";
         this.btn_accept.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.btn_accept.LookAndFeel.UseDefaultLookAndFeel = false;
         this.btn_accept.Name = "btn_accept";
         this.btn_accept.Size = new System.Drawing.Size(116, 42);
         this.btn_accept.TabIndex = 1;
         this.btn_accept.Text = "بله";
         this.btn_accept.Click += new System.EventHandler(this.btn_accept_Click);
         // 
         // Recycle
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Goldenrod;
         this.Controls.Add(this.btn_accept);
         this.Controls.Add(this.btn_cancel);
         this.Controls.Add(this.lb_caption);
         this.Controls.Add(this.lb_title);
         this.Name = "ToolOperation";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(486, 170);
         this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lb_title;
        private System.Windows.Forms.Label lb_caption;
        private DevExpress.XtraEditors.SimpleButton btn_cancel;
        private DevExpress.XtraEditors.SimpleButton btn_accept;


   }
}
