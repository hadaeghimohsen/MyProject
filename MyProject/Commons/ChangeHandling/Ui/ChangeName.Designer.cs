namespace MyProject.Commons.ChangeHandling.Ui
{
   partial class ChangeName
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
         this.lbl_title = new DevExpress.XtraEditors.LabelControl();
         this.te_titlefa = new DevExpress.XtraEditors.TextEdit();
         this.label1 = new System.Windows.Forms.Label();
         this.sb_changenewrolename = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // lbl_title
         // 
         this.lbl_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lbl_title.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.lbl_title.Appearance.ForeColor = System.Drawing.Color.White;
         this.lbl_title.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.lbl_title.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.lbl_title.LineColor = System.Drawing.Color.Black;
         this.lbl_title.LineVisible = true;
         this.lbl_title.Location = new System.Drawing.Point(3, 3);
         this.lbl_title.Name = "lbl_title";
         this.lbl_title.Size = new System.Drawing.Size(396, 32);
         this.lbl_title.TabIndex = 67;
         this.lbl_title.Text = "عنوان جدید";
         // 
         // te_titlefa
         // 
         this.te_titlefa.EditValue = "";
         this.te_titlefa.Location = new System.Drawing.Point(32, 52);
         this.te_titlefa.Name = "te_titlefa";
         this.te_titlefa.Properties.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.te_titlefa.Properties.Appearance.Options.UseFont = true;
         this.te_titlefa.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.te_titlefa.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.te_titlefa.Properties.Mask.EditMask = "d";
         this.te_titlefa.Size = new System.Drawing.Size(298, 34);
         this.te_titlefa.TabIndex = 65;
         this.te_titlefa.Enter += new System.EventHandler(this.LangChangeToFarsi);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.Color.Cornsilk;
         this.label1.Location = new System.Drawing.Point(336, 55);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(38, 30);
         this.label1.TabIndex = 66;
         this.label1.Text = "نام :";
         // 
         // sb_changenewrolename
         // 
         this.sb_changenewrolename.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
         this.sb_changenewrolename.Appearance.Font = new System.Drawing.Font("B Kamran", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_changenewrolename.Appearance.ForeColor = System.Drawing.Color.White;
         this.sb_changenewrolename.Appearance.Options.UseBackColor = true;
         this.sb_changenewrolename.Appearance.Options.UseFont = true;
         this.sb_changenewrolename.Appearance.Options.UseForeColor = true;
         this.sb_changenewrolename.Location = new System.Drawing.Point(214, 102);
         this.sb_changenewrolename.LookAndFeel.SkinName = "Office 2010 Silver";
         this.sb_changenewrolename.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_changenewrolename.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_changenewrolename.Name = "sb_changenewrolename";
         this.sb_changenewrolename.Size = new System.Drawing.Size(116, 33);
         this.sb_changenewrolename.TabIndex = 68;
         this.sb_changenewrolename.Text = "ثبت اطلاعات";
         this.sb_changenewrolename.Click += new System.EventHandler(this.sb_changenewrolename_Click);
         // 
         // ChangeName
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
         this.Controls.Add(this.sb_changenewrolename);
         this.Controls.Add(this.lbl_title);
         this.Controls.Add(this.te_titlefa);
         this.Controls.Add(this.label1);
         this.Name = "ChangeName";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(402, 150);
         ((System.ComponentModel.ISupportInitialize)(this.te_titlefa.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl lbl_title;
      private DevExpress.XtraEditors.TextEdit te_titlefa;
      private System.Windows.Forms.Label label1;
      private DevExpress.XtraEditors.SimpleButton sb_changenewrolename;
   }
}
