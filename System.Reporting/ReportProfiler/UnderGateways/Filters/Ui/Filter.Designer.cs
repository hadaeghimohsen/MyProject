namespace System.Reporting.ReportProfiler.UnderGateways.Filters.Ui
{
   partial class Filter
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
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         this.comboBox1 = new System.Windows.Forms.ComboBox();
         this.sb_rldatasource = new DevExpress.XtraEditors.SimpleButton();
         this.comboBox2 = new System.Windows.Forms.ComboBox();
         this.SuspendLayout();
         // 
         // checkBox1
         // 
         this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.checkBox1.AutoSize = true;
         this.checkBox1.Location = new System.Drawing.Point(425, 3);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(376, 17);
         this.checkBox1.TabIndex = 0;
         this.checkBox1.Text = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
         this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // comboBox1
         // 
         this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.comboBox1.FormattingEnabled = true;
         this.comboBox1.Items.AddRange(new object[] {
            "ورودی مستقیم توسط کاربر",
            "وروردی از طریق اطلاعات درون فایل",
            "وروردی از طریق اطلاعات درون پایگاه داده"});
         this.comboBox1.Location = new System.Drawing.Point(298, -1);
         this.comboBox1.Name = "comboBox1";
         this.comboBox1.Size = new System.Drawing.Size(121, 21);
         this.comboBox1.TabIndex = 1;
         // 
         // sb_rldatasource
         // 
         this.sb_rldatasource.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_rldatasource.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.sb_rldatasource.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_rldatasource.Appearance.Options.UseBackColor = true;
         this.sb_rldatasource.Appearance.Options.UseFont = true;
         this.sb_rldatasource.Appearance.Options.UseForeColor = true;
         this.sb_rldatasource.ImageIndex = 4;
         this.sb_rldatasource.Location = new System.Drawing.Point(259, -1);
         this.sb_rldatasource.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
         this.sb_rldatasource.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_rldatasource.Name = "sb_rldatasource";
         this.sb_rldatasource.Size = new System.Drawing.Size(33, 21);
         this.sb_rldatasource.TabIndex = 95;
         this.sb_rldatasource.Text = "...";
         // 
         // comboBox2
         // 
         this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.comboBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.comboBox2.FormattingEnabled = true;
         this.comboBox2.Items.AddRange(new object[] {
            "مساوی",
            "نامساوی"});
         this.comboBox2.Location = new System.Drawing.Point(625, 22);
         this.comboBox2.Name = "comboBox2";
         this.comboBox2.Size = new System.Drawing.Size(176, 21);
         this.comboBox2.TabIndex = 96;
         // 
         // Filter
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Controls.Add(this.comboBox2);
         this.Controls.Add(this.sb_rldatasource);
         this.Controls.Add(this.comboBox1);
         this.Controls.Add(this.checkBox1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "Filter";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(804, 46);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private Windows.Forms.CheckBox checkBox1;
      private Windows.Forms.ComboBox comboBox1;
      private DevExpress.XtraEditors.SimpleButton sb_rldatasource;
      private Windows.Forms.ComboBox comboBox2;
   }
}
