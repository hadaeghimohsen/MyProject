namespace System.Scsc.Ui.MasterPage
{
   partial class STRT_PROC_F
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
         this.Master_Tc = new System.Windows.Forms.TabControl();
         this.tp_001 = new System.Windows.Forms.TabPage();
         this.AddNew_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.NewExtension_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.Other_Butn = new DevExpress.XtraEditors.SimpleButton();
         this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
         this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
         this.Info_Lb = new DevExpress.XtraEditors.LabelControl();
         this.Master_Tc.SuspendLayout();
         this.tp_001.SuspendLayout();
         this.SuspendLayout();
         // 
         // Master_Tc
         // 
         this.Master_Tc.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
         this.Master_Tc.Controls.Add(this.tp_001);
         this.Master_Tc.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Master_Tc.Location = new System.Drawing.Point(0, 0);
         this.Master_Tc.Name = "Master_Tc";
         this.Master_Tc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.Master_Tc.RightToLeftLayout = true;
         this.Master_Tc.SelectedIndex = 0;
         this.Master_Tc.Size = new System.Drawing.Size(777, 715);
         this.Master_Tc.TabIndex = 69;
         // 
         // tp_001
         // 
         this.tp_001.Controls.Add(this.Info_Lb);
         this.tp_001.Controls.Add(this.labelControl2);
         this.tp_001.Controls.Add(this.labelControl1);
         this.tp_001.Controls.Add(this.Other_Butn);
         this.tp_001.Controls.Add(this.NewExtension_Butn);
         this.tp_001.Controls.Add(this.AddNew_Butn);
         this.tp_001.Location = new System.Drawing.Point(4, 26);
         this.tp_001.Name = "tp_001";
         this.tp_001.Padding = new System.Windows.Forms.Padding(3);
         this.tp_001.Size = new System.Drawing.Size(769, 685);
         this.tp_001.TabIndex = 0;
         this.tp_001.Text = "صفحه نخست";
         this.tp_001.UseVisualStyleBackColor = true;
         // 
         // AddNew_Butn
         // 
         this.AddNew_Butn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.AddNew_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.AddNew_Butn.Appearance.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Bold);
         this.AddNew_Butn.Appearance.Options.UseBackColor = true;
         this.AddNew_Butn.Appearance.Options.UseFont = true;
         this.AddNew_Butn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.AddNew_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1054;
         this.AddNew_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
         this.AddNew_Butn.Location = new System.Drawing.Point(522, 243);
         this.AddNew_Butn.Name = "AddNew_Butn";
         this.AddNew_Butn.Size = new System.Drawing.Size(200, 200);
         this.AddNew_Butn.TabIndex = 69;
         this.AddNew_Butn.Text = "ثبت نام جدید";
         this.AddNew_Butn.MouseEnter += new System.EventHandler(this.AddNew_Butn_MouseEnter);
         this.AddNew_Butn.MouseLeave += new System.EventHandler(this.AddNew_Butn_MouseLeave);
         // 
         // NewExtension_Butn
         // 
         this.NewExtension_Butn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.NewExtension_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.NewExtension_Butn.Appearance.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Bold);
         this.NewExtension_Butn.Appearance.Options.UseBackColor = true;
         this.NewExtension_Butn.Appearance.Options.UseFont = true;
         this.NewExtension_Butn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.NewExtension_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1056;
         this.NewExtension_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
         this.NewExtension_Butn.Location = new System.Drawing.Point(284, 243);
         this.NewExtension_Butn.Name = "NewExtension_Butn";
         this.NewExtension_Butn.Size = new System.Drawing.Size(200, 200);
         this.NewExtension_Butn.TabIndex = 69;
         this.NewExtension_Butn.Text = "تمدید";
         this.NewExtension_Butn.MouseEnter += new System.EventHandler(this.NewExtension_Butn_MouseEnter);
         this.NewExtension_Butn.MouseLeave += new System.EventHandler(this.NewExtension_Butn_MouseLeave);
         // 
         // Other_Butn
         // 
         this.Other_Butn.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.Other_Butn.Appearance.BackColor = System.Drawing.Color.Transparent;
         this.Other_Butn.Appearance.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Bold);
         this.Other_Butn.Appearance.Options.UseBackColor = true;
         this.Other_Butn.Appearance.Options.UseFont = true;
         this.Other_Butn.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
         this.Other_Butn.Image = global::System.Scsc.Properties.Resources.IMAGE_1007;
         this.Other_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
         this.Other_Butn.Location = new System.Drawing.Point(46, 243);
         this.Other_Butn.Name = "Other_Butn";
         this.Other_Butn.Size = new System.Drawing.Size(200, 200);
         this.Other_Butn.TabIndex = 69;
         this.Other_Butn.Text = "متفرقه";
         this.Other_Butn.MouseEnter += new System.EventHandler(this.Other_Butn_MouseEnter);
         this.Other_Butn.MouseLeave += new System.EventHandler(this.Other_Butn_MouseLeave);
         // 
         // labelControl1
         // 
         this.labelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.labelControl1.Appearance.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Bold);
         this.labelControl1.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1067;
         this.labelControl1.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl1.Location = new System.Drawing.Point(432, 15);
         this.labelControl1.Name = "labelControl1";
         this.labelControl1.Size = new System.Drawing.Size(321, 49);
         this.labelControl1.TabIndex = 70;
         this.labelControl1.Text = "به سیستم خودپرداز خوش آمدین";
         // 
         // labelControl2
         // 
         this.labelControl2.Anchor = System.Windows.Forms.AnchorStyles.Top;
         this.labelControl2.Appearance.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Bold);
         this.labelControl2.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1093;
         this.labelControl2.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.labelControl2.Location = new System.Drawing.Point(210, 132);
         this.labelControl2.Name = "labelControl2";
         this.labelControl2.Size = new System.Drawing.Size(348, 49);
         this.labelControl2.TabIndex = 70;
         this.labelControl2.Text = "لطفا یکی از موارد زیر را انتخاب کنید";
         // 
         // Info_Lb
         // 
         this.Info_Lb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Info_Lb.Appearance.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Info_Lb.Appearance.Image = global::System.Scsc.Properties.Resources.IMAGE_1147;
         this.Info_Lb.Appearance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
         this.Info_Lb.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         this.Info_Lb.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.NoWrap;
         this.Info_Lb.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
         this.Info_Lb.Location = new System.Drawing.Point(89, 498);
         this.Info_Lb.Name = "Info_Lb";
         this.Info_Lb.Size = new System.Drawing.Size(590, 153);
         this.Info_Lb.TabIndex = 70;
         this.Info_Lb.Text = "لطفا یکی از موارد زیر را انتخاب کنید";
         // 
         // STRT_PROC_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.Master_Tc);
         this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "STRT_PROC_F";
         this.Size = new System.Drawing.Size(777, 715);
         this.Master_Tc.ResumeLayout(false);
         this.tp_001.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.TabControl Master_Tc;
      private Windows.Forms.TabPage tp_001;
      private DevExpress.XtraEditors.SimpleButton Other_Butn;
      private DevExpress.XtraEditors.SimpleButton NewExtension_Butn;
      private DevExpress.XtraEditors.SimpleButton AddNew_Butn;
      private DevExpress.XtraEditors.LabelControl labelControl1;
      private DevExpress.XtraEditors.LabelControl labelControl2;
      private DevExpress.XtraEditors.LabelControl Info_Lb;

   }
}
