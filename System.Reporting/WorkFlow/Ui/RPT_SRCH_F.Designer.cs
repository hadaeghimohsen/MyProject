namespace System.Reporting.WorkFlow.Ui
{
   partial class RPT_SRCH_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPT_SRCH_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         this.be_search = new DevExpress.XtraEditors.ButtonEdit();
         this.sb_title = new DevExpress.XtraEditors.SimpleButton();
         this.sb_content = new DevExpress.XtraEditors.SimpleButton();
         this.sb_creator = new DevExpress.XtraEditors.SimpleButton();
         this.sb_filesystem = new DevExpress.XtraEditors.SimpleButton();
         ((System.ComponentModel.ISupportInitialize)(this.be_search.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // be_search
         // 
         this.be_search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.be_search.EditValue = "";
         this.be_search.Location = new System.Drawing.Point(36, 40);
         this.be_search.Name = "be_search";
         this.be_search.Properties.Appearance.Font = new System.Drawing.Font("B Koodak", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.be_search.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
         this.be_search.Properties.Appearance.Options.UseFont = true;
         this.be_search.Properties.Appearance.Options.UseForeColor = true;
         this.be_search.Properties.Appearance.Options.UseTextOptions = true;
         this.be_search.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.be_search.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("be_search.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.be_search.Properties.LookAndFeel.SkinName = "VS2010";
         this.be_search.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.be_search.Properties.NullValuePrompt = "عبارت جستجوی خود را اینجا وارد کنید";
         this.be_search.Properties.NullValuePromptShowForEmptyValue = true;
         this.be_search.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.be_search.Size = new System.Drawing.Size(325, 36);
         this.be_search.TabIndex = 0;
         this.be_search.EditValueChanged += new System.EventHandler(this.be_search_EditValueChanged);
         // 
         // sb_title
         // 
         this.sb_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_title.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_title.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_title.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_title.Appearance.Options.UseBackColor = true;
         this.sb_title.Appearance.Options.UseFont = true;
         this.sb_title.Appearance.Options.UseForeColor = true;
         this.sb_title.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.sb_title.Image = ((System.Drawing.Image)(resources.GetObject("sb_title.Image")));
         this.sb_title.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.sb_title.Location = new System.Drawing.Point(0, 104);
         this.sb_title.LookAndFeel.SkinName = "Office 2010 Blue";
         this.sb_title.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_title.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_title.Name = "sb_title";
         this.sb_title.Size = new System.Drawing.Size(400, 60);
         this.sb_title.TabIndex = 1;
         this.sb_title.Tag = "1";
         this.sb_title.Text = "عنوان گزارشات";
         this.sb_title.Click += new System.EventHandler(this.sb_filtertype_Click);
         // 
         // sb_content
         // 
         this.sb_content.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_content.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_content.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_content.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_content.Appearance.Options.UseBackColor = true;
         this.sb_content.Appearance.Options.UseFont = true;
         this.sb_content.Appearance.Options.UseForeColor = true;
         this.sb_content.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.sb_content.Image = ((System.Drawing.Image)(resources.GetObject("sb_content.Image")));
         this.sb_content.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.sb_content.Location = new System.Drawing.Point(0, 170);
         this.sb_content.LookAndFeel.SkinName = "Office 2010 Blue";
         this.sb_content.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_content.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_content.Name = "sb_content";
         this.sb_content.Size = new System.Drawing.Size(400, 60);
         this.sb_content.TabIndex = 2;
         this.sb_content.Tag = "2";
         this.sb_content.Text = "محتوای گزارشات";
         this.sb_content.Click += new System.EventHandler(this.sb_filtertype_Click);
         // 
         // sb_creator
         // 
         this.sb_creator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_creator.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_creator.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_creator.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_creator.Appearance.Options.UseBackColor = true;
         this.sb_creator.Appearance.Options.UseFont = true;
         this.sb_creator.Appearance.Options.UseForeColor = true;
         this.sb_creator.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.sb_creator.Image = ((System.Drawing.Image)(resources.GetObject("sb_creator.Image")));
         this.sb_creator.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.sb_creator.Location = new System.Drawing.Point(0, 236);
         this.sb_creator.LookAndFeel.SkinName = "Office 2010 Blue";
         this.sb_creator.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_creator.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_creator.Name = "sb_creator";
         this.sb_creator.Size = new System.Drawing.Size(400, 60);
         this.sb_creator.TabIndex = 3;
         this.sb_creator.Tag = "3";
         this.sb_creator.Text = "سازنده گزارش";
         this.sb_creator.Click += new System.EventHandler(this.sb_filtertype_Click);
         // 
         // sb_filesystem
         // 
         this.sb_filesystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.sb_filesystem.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.sb_filesystem.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.sb_filesystem.Appearance.ForeColor = System.Drawing.Color.Black;
         this.sb_filesystem.Appearance.Options.UseBackColor = true;
         this.sb_filesystem.Appearance.Options.UseFont = true;
         this.sb_filesystem.Appearance.Options.UseForeColor = true;
         this.sb_filesystem.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.sb_filesystem.Image = ((System.Drawing.Image)(resources.GetObject("sb_filesystem.Image")));
         this.sb_filesystem.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;
         this.sb_filesystem.Location = new System.Drawing.Point(0, 302);
         this.sb_filesystem.LookAndFeel.SkinName = "Office 2010 Blue";
         this.sb_filesystem.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.sb_filesystem.LookAndFeel.UseDefaultLookAndFeel = false;
         this.sb_filesystem.Name = "sb_filesystem";
         this.sb_filesystem.Size = new System.Drawing.Size(400, 60);
         this.sb_filesystem.TabIndex = 4;
         this.sb_filesystem.Tag = "4";
         this.sb_filesystem.Text = "نام فایل گزارش";
         this.sb_filesystem.Click += new System.EventHandler(this.sb_filtertype_Click);
         // 
         // RPT_SRCH_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.DarkOrange;
         this.Controls.Add(this.sb_filesystem);
         this.Controls.Add(this.sb_content);
         this.Controls.Add(this.sb_creator);
         this.Controls.Add(this.sb_title);
         this.Controls.Add(this.be_search);
         this.Name = "RPT_SRCH_F";
         this.Size = new System.Drawing.Size(400, 771);
         ((System.ComponentModel.ISupportInitialize)(this.be_search.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.ButtonEdit be_search;
      private DevExpress.XtraEditors.SimpleButton sb_title;
      private DevExpress.XtraEditors.SimpleButton sb_content;
      private DevExpress.XtraEditors.SimpleButton sb_creator;
      private DevExpress.XtraEditors.SimpleButton sb_filesystem;
   }
}
