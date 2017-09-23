namespace System.Gas.Gnrt_Qrcd.Ui
{
   partial class RPRT_PBLC_F
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
         DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RPRT_PBLC_F));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.label1 = new System.Windows.Forms.Label();
         this.panel1 = new System.Windows.Forms.Panel();
         this.tb_qr_desc = new System.Windows.Forms.TextBox();
         this.radioButton2 = new System.Windows.Forms.RadioButton();
         this.rb_qr_desc_type = new System.Windows.Forms.RadioButton();
         this.bc_qrcode = new DevExpress.XtraEditors.BarCodeControl();
         this.be_open_fldr = new DevExpress.XtraEditors.ButtonEdit();
         this.wbp_book_mark = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         this.panel2 = new System.Windows.Forms.Panel();
         this.fbd_show = new System.Windows.Forms.FolderBrowserDialog();
         this.cbe_databaseservers = new DevExpress.XtraEditors.CheckedComboBoxEdit();
         this.cbe_seri_no = new DevExpress.XtraEditors.CheckedComboBoxEdit();
         this.mpbc_saving = new DevExpress.XtraEditors.MarqueeProgressBarControl();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.be_open_fldr.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_databaseservers.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_seri_no.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.mpbc_saving.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Black;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
         this.labelControl1.LineVisible = true;
         this.labelControl1.Location = new System.Drawing.Point(22, 31);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(799, 33);
         this.labelControl1.TabIndex = 0;
         this.labelControl1.Text = "آماده سازی چاپ به صورت گروهی";
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.label1.ForeColor = System.Drawing.Color.Black;
         this.label1.Location = new System.Drawing.Point(731, 234);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(95, 19);
         this.label1.TabIndex = 2;
         this.label1.Text = "متن QR کد :";
         // 
         // panel1
         // 
         this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.panel1.Controls.Add(this.tb_qr_desc);
         this.panel1.Controls.Add(this.radioButton2);
         this.panel1.Controls.Add(this.rb_qr_desc_type);
         this.panel1.Location = new System.Drawing.Point(356, 271);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(466, 242);
         this.panel1.TabIndex = 3;
         // 
         // tb_qr_desc
         // 
         this.tb_qr_desc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.tb_qr_desc.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.tb_qr_desc.Location = new System.Drawing.Point(14, 105);
         this.tb_qr_desc.MaxLength = 385;
         this.tb_qr_desc.Multiline = true;
         this.tb_qr_desc.Name = "tb_qr_desc";
         this.tb_qr_desc.Size = new System.Drawing.Size(440, 128);
         this.tb_qr_desc.TabIndex = 1;
         this.tb_qr_desc.Text = "متن خود را اینجا وارد کنید...";
         this.tb_qr_desc.TextChanged += new System.EventHandler(this.tb_qr_desc_TextChanged);
         // 
         // radioButton2
         // 
         this.radioButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.radioButton2.AutoSize = true;
         this.radioButton2.Checked = true;
         this.radioButton2.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.radioButton2.ForeColor = System.Drawing.Color.Black;
         this.radioButton2.Location = new System.Drawing.Point(266, 54);
         this.radioButton2.Name = "radioButton2";
         this.radioButton2.Size = new System.Drawing.Size(188, 32);
         this.radioButton2.TabIndex = 0;
         this.radioButton2.TabStop = true;
         this.radioButton2.Text = "به صورت دستی تنظیم می شود";
         this.radioButton2.UseVisualStyleBackColor = true;
         // 
         // rb_qr_desc_type
         // 
         this.rb_qr_desc_type.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.rb_qr_desc_type.AutoSize = true;
         this.rb_qr_desc_type.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.rb_qr_desc_type.ForeColor = System.Drawing.Color.Black;
         this.rb_qr_desc_type.Location = new System.Drawing.Point(240, 16);
         this.rb_qr_desc_type.Name = "rb_qr_desc_type";
         this.rb_qr_desc_type.Size = new System.Drawing.Size(214, 32);
         this.rb_qr_desc_type.TabIndex = 0;
         this.rb_qr_desc_type.Text = "اطلاعات درون پایگاه داده قرار دارد";
         this.rb_qr_desc_type.UseVisualStyleBackColor = true;
         this.rb_qr_desc_type.CheckedChanged += new System.EventHandler(this.rb_qr_desc_type_CheckedChanged);
         // 
         // bc_qrcode
         // 
         this.bc_qrcode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.bc_qrcode.BackColor = System.Drawing.Color.WhiteSmoke;
         this.bc_qrcode.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
         this.bc_qrcode.ForeColor = System.Drawing.Color.Black;
         this.bc_qrcode.Location = new System.Drawing.Point(167, 377);
         this.bc_qrcode.Module = 4D;
         this.bc_qrcode.Name = "bc_qrcode";
         this.bc_qrcode.Padding = new System.Windows.Forms.Padding(10, 2, 10, 0);
         this.bc_qrcode.ShowText = false;
         this.bc_qrcode.Size = new System.Drawing.Size(184, 167);
         qrCodeGenerator1.CompactionMode = DevExpress.XtraPrinting.BarCode.QRCodeCompactionMode.Byte;
         qrCodeGenerator1.Version = DevExpress.XtraPrinting.BarCode.QRCodeVersion.Version6;
         this.bc_qrcode.Symbology = qrCodeGenerator1;
         this.bc_qrcode.TabIndex = 4;
         this.bc_qrcode.Text = "عصر اندیشه 0711-2330005";
         // 
         // be_open_fldr
         // 
         this.be_open_fldr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.be_open_fldr.EditValue = "D:\\QR-Code\\S01\\Public";
         this.be_open_fldr.Location = new System.Drawing.Point(370, 520);
         this.be_open_fldr.Name = "be_open_fldr";
         this.be_open_fldr.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.be_open_fldr.Properties.Appearance.Options.UseFont = true;
         this.be_open_fldr.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("be_open_fldr.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", "0", null, true)});
         this.be_open_fldr.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.be_open_fldr_ButtonClick);
         this.be_open_fldr.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.be_open_fldr.Size = new System.Drawing.Size(452, 24);
         this.be_open_fldr.TabIndex = 5;
         // 
         // wbp_book_mark
         // 
         this.wbp_book_mark.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.wbp_book_mark.AppearanceButton.Hovered.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.wbp_book_mark.AppearanceButton.Hovered.Options.UseBackColor = true;
         this.wbp_book_mark.AppearanceButton.Normal.BackColor = System.Drawing.Color.Black;
         this.wbp_book_mark.AppearanceButton.Normal.BackColor2 = System.Drawing.Color.Black;
         this.wbp_book_mark.AppearanceButton.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.wbp_book_mark.AppearanceButton.Normal.ForeColor = System.Drawing.Color.Black;
         this.wbp_book_mark.AppearanceButton.Normal.Options.UseBackColor = true;
         this.wbp_book_mark.AppearanceButton.Normal.Options.UseFont = true;
         this.wbp_book_mark.AppearanceButton.Normal.Options.UseForeColor = true;
         this.wbp_book_mark.AppearanceButton.Pressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.wbp_book_mark.AppearanceButton.Pressed.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
         this.wbp_book_mark.AppearanceButton.Pressed.Options.UseBackColor = true;
         this.wbp_book_mark.BackColor = System.Drawing.Color.Chartreuse;
         this.wbp_book_mark.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_book_mark.Buttons"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "0", -1, true, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_book_mark.Buttons1"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "1", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_book_mark.Buttons2"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "2", -1, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("wbp_book_mark.Buttons3"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, "3", -1, false, false)});
         this.wbp_book_mark.ContentAlignment = System.Drawing.ContentAlignment.MiddleRight;
         this.wbp_book_mark.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.wbp_book_mark.ForeColor = System.Drawing.Color.Black;
         this.wbp_book_mark.Location = new System.Drawing.Point(0, 610);
         this.wbp_book_mark.Name = "wbp_book_mark";
         this.wbp_book_mark.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
         this.wbp_book_mark.Size = new System.Drawing.Size(897, 78);
         this.wbp_book_mark.TabIndex = 6;
         this.wbp_book_mark.Text = "windowsUIButtonPanel1";
         this.wbp_book_mark.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.wbp_book_mark_ButtonClick);
         // 
         // panel2
         // 
         this.panel2.BackColor = System.Drawing.Color.MediumSeaGreen;
         this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
         this.panel2.Location = new System.Drawing.Point(832, 0);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(65, 610);
         this.panel2.TabIndex = 8;
         // 
         // fbd_show
         // 
         this.fbd_show.Description = "مسیر ذخیره سازی عکس بارکد خود را انتخاب کنید...";
         this.fbd_show.SelectedPath = "\\\\Localhost";
         // 
         // cbe_databaseservers
         // 
         this.cbe_databaseservers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cbe_databaseservers.EditValue = "";
         this.cbe_databaseservers.Location = new System.Drawing.Point(486, 104);
         this.cbe_databaseservers.Name = "cbe_databaseservers";
         this.cbe_databaseservers.Properties.Appearance.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cbe_databaseservers.Properties.Appearance.Options.UseFont = true;
         this.cbe_databaseservers.Properties.Appearance.Options.UseTextOptions = true;
         this.cbe_databaseservers.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.cbe_databaseservers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", "1", null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("cbe_databaseservers.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", "0", null, true)});
         this.cbe_databaseservers.Properties.NullValuePrompt = "لطفا سرور مورد نظر خود را انتخاب کنید...";
         this.cbe_databaseservers.Properties.NullValuePromptShowForEmptyValue = true;
         this.cbe_databaseservers.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cbe_databaseservers_Properties_ButtonClick);
         this.cbe_databaseservers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.cbe_databaseservers.Size = new System.Drawing.Size(336, 38);
         this.cbe_databaseservers.TabIndex = 9;
         this.cbe_databaseservers.EditValueChanged += new System.EventHandler(this.cbe_databaseservers_EditValueChanged);
         // 
         // cbe_seri_no
         // 
         this.cbe_seri_no.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cbe_seri_no.EditValue = "";
         this.cbe_seri_no.Location = new System.Drawing.Point(485, 163);
         this.cbe_seri_no.Name = "cbe_seri_no";
         this.cbe_seri_no.Properties.Appearance.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cbe_seri_no.Properties.Appearance.Options.UseFont = true;
         this.cbe_seri_no.Properties.Appearance.Options.UseTextOptions = true;
         this.cbe_seri_no.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.cbe_seri_no.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", "1", null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("cbe_seri_no.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", "0", null, true)});
         this.cbe_seri_no.Properties.NullValuePrompt = "لطفا سری گاز خودرا انتخاب کنید ...";
         this.cbe_seri_no.Properties.NullValuePromptShowForEmptyValue = true;
         this.cbe_seri_no.Properties.SelectAllItemVisible = false;
         this.cbe_seri_no.Properties.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.cbe_seri_no_buttonclick);
         this.cbe_seri_no.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.cbe_seri_no.Size = new System.Drawing.Size(336, 38);
         this.cbe_seri_no.TabIndex = 10;
         // 
         // mpbc_saving
         // 
         this.mpbc_saving.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.mpbc_saving.EditValue = "ذخیره کردن فایل عکس بارکد";
         this.mpbc_saving.Location = new System.Drawing.Point(370, 550);
         this.mpbc_saving.Name = "mpbc_saving";
         this.mpbc_saving.Properties.LookAndFeel.SkinName = "Office 2013";
         this.mpbc_saving.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.mpbc_saving.Properties.ShowTitle = true;
         this.mpbc_saving.Properties.TextOrientation = DevExpress.Utils.Drawing.TextOrientation.Horizontal;
         this.mpbc_saving.Size = new System.Drawing.Size(190, 29);
         this.mpbc_saving.TabIndex = 11;
         this.mpbc_saving.Visible = false;
         // 
         // RPRT_PBLC_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.WhiteSmoke;
         this.Controls.Add(this.mpbc_saving);
         this.Controls.Add(this.cbe_seri_no);
         this.Controls.Add(this.cbe_databaseservers);
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.wbp_book_mark);
         this.Controls.Add(this.be_open_fldr);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.labelControl1);
         this.Controls.Add(this.bc_qrcode);
         this.ForeColor = System.Drawing.Color.White;
         this.Name = "RPRT_PBLC_F";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(897, 688);
         this.panel1.ResumeLayout(false);
         this.panel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.be_open_fldr.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_databaseservers.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_seri_no.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.mpbc_saving.Properties)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private DevExpress.XtraEditors.LabelControl labelControl1;
      private Windows.Forms.Label label1;
      private Windows.Forms.Panel panel1;
      private Windows.Forms.TextBox tb_qr_desc;
      private Windows.Forms.RadioButton radioButton2;
      private Windows.Forms.RadioButton rb_qr_desc_type;
      private DevExpress.XtraEditors.BarCodeControl bc_qrcode;
      private DevExpress.XtraEditors.ButtonEdit be_open_fldr;
      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel wbp_book_mark;
      private Windows.Forms.Panel panel2;
      private Windows.Forms.FolderBrowserDialog fbd_show;
      private DevExpress.XtraEditors.CheckedComboBoxEdit cbe_databaseservers;
      private DevExpress.XtraEditors.CheckedComboBoxEdit cbe_seri_no;
      private DevExpress.XtraEditors.MarqueeProgressBarControl mpbc_saving;
   }
}
