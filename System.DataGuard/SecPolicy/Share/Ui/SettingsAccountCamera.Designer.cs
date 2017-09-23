namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class SettingsAccountCamera
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
         System.Windows.Forms.Label label4;
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsAccountCamera));
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
         this.panel1 = new System.Windows.Forms.Panel();
         this.SubmitChange_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.Back_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.UserBs = new System.Windows.Forms.BindingSource();
         this.LOV_VideoSrc = new DevExpress.XtraEditors.LookUpEdit();
         this.pb_source = new System.Windows.Forms.PictureBox();
         this.Pb_FaceZone = new System.Windows.Forms.PictureBox();
         this.Ts_ChangeSizeButn = new DevExpress.XtraEditors.ToggleSwitch();
         this.TakeImage_Butn = new System.MaxUi.RoundedButton();
         this.Tm_NewFrameProcess = new System.Windows.Forms.Timer();
         label4 = new System.Windows.Forms.Label();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.UserBs)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.LOV_VideoSrc.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_source)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pb_FaceZone)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.Ts_ChangeSizeButn.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // label4
         // 
         label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
         label4.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         label4.Image = global::System.DataGuard.Properties.Resources.IMAGE_1424;
         label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         label4.Location = new System.Drawing.Point(467, 85);
         label4.Name = "label4";
         label4.Size = new System.Drawing.Size(151, 40);
         label4.TabIndex = 14;
         label4.Text = "انتخاب دوربین :";
         label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.SubmitChange_Butn);
         this.panel1.Controls.Add(this.labelControl1);
         this.panel1.Controls.Add(this.Back_Butn);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(632, 59);
         this.panel1.TabIndex = 0;
         // 
         // SubmitChange_Butn
         // 
         this.SubmitChange_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.SubmitChange_Butn.Appearance.Options.UseBackColor = true;
         this.SubmitChange_Butn.Dock = System.Windows.Forms.DockStyle.Left;
         this.SubmitChange_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1440;
         this.SubmitChange_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.SubmitChange_Butn.Location = new System.Drawing.Point(0, 0);
         this.SubmitChange_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.SubmitChange_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.SubmitChange_Butn.Name = "SubmitChange_Butn";
         this.SubmitChange_Butn.Size = new System.Drawing.Size(61, 59);
         this.SubmitChange_Butn.TabIndex = 2;
         this.SubmitChange_Butn.ToolTip = "بازگشت";
         this.SubmitChange_Butn.Click += new System.EventHandler(this.SubmitChange_Butn_Click);
         // 
         // labelControl1
         // 
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Koodak", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.labelControl1.Appearance.Image = global::System.DataGuard.Properties.Resources.IMAGE_1365;
         this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Dock = System.Windows.Forms.DockStyle.Right;
         this.labelControl1.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.RightCenter;
         this.labelControl1.Location = new System.Drawing.Point(419, 0);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(152, 59);
         this.labelControl1.TabIndex = 1;
         this.labelControl1.Text = "گرفتن تصویر";
         // 
         // Back_Butn
         // 
         this.Back_Butn.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(36)))), ((int)(((byte)(248)))));
         this.Back_Butn.Appearance.Options.UseBackColor = true;
         this.Back_Butn.Dock = System.Windows.Forms.DockStyle.Right;
         this.Back_Butn.Image = global::System.DataGuard.Properties.Resources.IMAGE_1371;
         this.Back_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
         this.Back_Butn.Location = new System.Drawing.Point(571, 0);
         this.Back_Butn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
         this.Back_Butn.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Back_Butn.Name = "Back_Butn";
         this.Back_Butn.Size = new System.Drawing.Size(61, 59);
         this.Back_Butn.TabIndex = 0;
         this.Back_Butn.ToolTip = "بازگشت";
         this.Back_Butn.Click += new System.EventHandler(this.Back_Butn_Click);
         // 
         // UserBs
         // 
         this.UserBs.DataSource = typeof(System.DataGuard.Data.User);
         this.UserBs.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.UserBs_ListChanged);
         // 
         // LOV_VideoSrc
         // 
         this.LOV_VideoSrc.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.LOV_VideoSrc.Location = new System.Drawing.Point(60, 85);
         this.LOV_VideoSrc.Name = "LOV_VideoSrc";
         this.LOV_VideoSrc.Properties.Appearance.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.LOV_VideoSrc.Properties.Appearance.Options.UseFont = true;
         this.LOV_VideoSrc.Properties.Appearance.Options.UseTextOptions = true;
         this.LOV_VideoSrc.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.LOV_VideoSrc.Properties.AppearanceFocused.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
         this.LOV_VideoSrc.Properties.AppearanceFocused.Options.UseBackColor = true;
         this.LOV_VideoSrc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("LOV_VideoSrc.Properties.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, "", null, null, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("LOV_VideoSrc.Properties.Buttons1"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject6, "", null, null, true)});
         this.LOV_VideoSrc.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
         this.LOV_VideoSrc.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.LOV_VideoSrc.Properties.NullText = "";
         this.LOV_VideoSrc.Size = new System.Drawing.Size(401, 40);
         this.LOV_VideoSrc.TabIndex = 13;
         this.LOV_VideoSrc.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.LOV_VideoSrc_ButtonClick);
         // 
         // pb_source
         // 
         this.pb_source.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.pb_source.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.pb_source.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.pb_source.Location = new System.Drawing.Point(60, 131);
         this.pb_source.Name = "pb_source";
         this.pb_source.Size = new System.Drawing.Size(401, 244);
         this.pb_source.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.pb_source.TabIndex = 15;
         this.pb_source.TabStop = false;
         // 
         // Pb_FaceZone
         // 
         this.Pb_FaceZone.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.Pb_FaceZone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         this.Pb_FaceZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Pb_FaceZone.Location = new System.Drawing.Point(467, 241);
         this.Pb_FaceZone.Name = "Pb_FaceZone";
         this.Pb_FaceZone.Size = new System.Drawing.Size(113, 134);
         this.Pb_FaceZone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
         this.Pb_FaceZone.TabIndex = 16;
         this.Pb_FaceZone.TabStop = false;
         // 
         // Ts_ChangeSizeButn
         // 
         this.Ts_ChangeSizeButn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.Ts_ChangeSizeButn.Location = new System.Drawing.Point(509, 381);
         this.Ts_ChangeSizeButn.Name = "Ts_ChangeSizeButn";
         this.Ts_ChangeSizeButn.Properties.LookAndFeel.SkinName = "Office 2013";
         this.Ts_ChangeSizeButn.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Ts_ChangeSizeButn.Size = new System.Drawing.Size(71, 24);
         this.Ts_ChangeSizeButn.TabIndex = 22;
         this.Ts_ChangeSizeButn.Tag = "001";
         // 
         // TakeImage_Butn
         // 
         this.TakeImage_Butn.Active = true;
         this.TakeImage_Butn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.TakeImage_Butn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
         this.TakeImage_Butn.ButtonStyle = System.MaxUi.RoundedButton.ButtonStyles.Ellipse;
         this.TakeImage_Butn.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold);
         this.TakeImage_Butn.GradientStyle = System.MaxUi.RoundedButton.GradientStyles.Vertical;
         this.TakeImage_Butn.HoverBorderColor = System.Drawing.Color.Gold;
         this.TakeImage_Butn.HoverColorA = System.Drawing.Color.LightGray;
         this.TakeImage_Butn.HoverColorB = System.Drawing.Color.LightGray;
         this.TakeImage_Butn.ImageProfile = global::System.DataGuard.Properties.Resources.IMAGE_1428;
         this.TakeImage_Butn.ImageSizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.TakeImage_Butn.Location = new System.Drawing.Point(228, 381);
         this.TakeImage_Butn.Name = "TakeImage_Butn";
         this.TakeImage_Butn.NormalBorderColor = System.Drawing.Color.Black;
         this.TakeImage_Butn.NormalColorA = System.Drawing.Color.White;
         this.TakeImage_Butn.NormalColorB = System.Drawing.Color.White;
         this.TakeImage_Butn.Size = new System.Drawing.Size(65, 65);
         this.TakeImage_Butn.SmoothingQuality = System.MaxUi.RoundedButton.SmoothingQualities.AntiAlias;
         this.TakeImage_Butn.TabIndex = 23;
         this.TakeImage_Butn.Click += new System.EventHandler(this.TakeImage_Butn_Click);
         // 
         // Tm_NewFrameProcess
         // 
         this.Tm_NewFrameProcess.Tick += new System.EventHandler(this.Tm_NewFrameProcess_Tick);
         // 
         // SettingsAccountCamera
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.TakeImage_Butn);
         this.Controls.Add(this.Ts_ChangeSizeButn);
         this.Controls.Add(this.Pb_FaceZone);
         this.Controls.Add(this.pb_source);
         this.Controls.Add(label4);
         this.Controls.Add(this.LOV_VideoSrc);
         this.Controls.Add(this.panel1);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "SettingsAccountCamera";
         this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Size = new System.Drawing.Size(632, 483);
         this.panel1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.UserBs)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.LOV_VideoSrc.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.pb_source)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Pb_FaceZone)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.Ts_ChangeSizeButn.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.Panel panel1;
      private DevExpress.XtraEditors.SimpleButton Back_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private Windows.Forms.BindingSource UserBs;
      private DevExpress.XtraEditors.SimpleButton SubmitChange_Butn;
      private DevExpress.XtraEditors.LookUpEdit LOV_VideoSrc;
      private Windows.Forms.PictureBox pb_source;
      private Windows.Forms.PictureBox Pb_FaceZone;
      private DevExpress.XtraEditors.ToggleSwitch Ts_ChangeSizeButn;
      private MaxUi.RoundedButton TakeImage_Butn;
      private Windows.Forms.Timer Tm_NewFrameProcess;

   }
}
