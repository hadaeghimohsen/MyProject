namespace System.Gas.Self.Ui
{
   partial class STRT_MTRO_M
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
         DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(STRT_MTRO_M));
         DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
         this.Start_Menu = new DevExpress.XtraEditors.TileControl();
         this.Start_Group = new DevExpress.XtraEditors.TileGroup();
         this.RPRT_PBLC_M = new DevExpress.XtraEditors.TileItem();
         this.RPRT_PRVT_M = new DevExpress.XtraEditors.TileItem();
         this.SuspendLayout();
         // 
         // Start_Menu
         // 
         this.Start_Menu.AppearanceText.Font = new System.Drawing.Font("B Koodak", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Start_Menu.AppearanceText.ForeColor = System.Drawing.Color.White;
         this.Start_Menu.AppearanceText.Options.UseFont = true;
         this.Start_Menu.AppearanceText.Options.UseForeColor = true;
         this.Start_Menu.BackColor = System.Drawing.Color.Transparent;
         this.Start_Menu.Dock = System.Windows.Forms.DockStyle.Fill;
         this.Start_Menu.Groups.Add(this.Start_Group);
         this.Start_Menu.HorizontalContentAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.Start_Menu.Location = new System.Drawing.Point(0, 0);
         this.Start_Menu.Name = "Start_Menu";
         this.Start_Menu.ShowText = true;
         this.Start_Menu.Size = new System.Drawing.Size(1156, 692);
         this.Start_Menu.TabIndex = 0;
         this.Start_Menu.Text = "شروع";
         this.Start_Menu.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
         this.Start_Menu.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Start_Menu_MouseClick);
         // 
         // Start_Group
         // 
         this.Start_Group.Items.Add(this.RPRT_PBLC_M);
         this.Start_Group.Items.Add(this.RPRT_PRVT_M);
         this.Start_Group.Name = "Start_Group";
         this.Start_Group.Text = null;
         // 
         // RPRT_PBLC_M
         // 
         tileItemElement1.Appearance.Normal.BackColor = System.Drawing.Color.Blue;
         tileItemElement1.Appearance.Normal.BackColor2 = System.Drawing.Color.Blue;
         tileItemElement1.Appearance.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         tileItemElement1.Appearance.Normal.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         tileItemElement1.Appearance.Normal.Options.UseBackColor = true;
         tileItemElement1.Appearance.Normal.Options.UseBorderColor = true;
         tileItemElement1.Appearance.Normal.Options.UseFont = true;
         tileItemElement1.Appearance.Normal.Options.UseTextOptions = true;
         tileItemElement1.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         tileItemElement1.Image = ((System.Drawing.Image)(resources.GetObject("tileItemElement1.Image")));
         tileItemElement1.Text = "چاپ عمومی قبض های گاز";
         this.RPRT_PBLC_M.Elements.Add(tileItemElement1);
         this.RPRT_PBLC_M.IsLarge = true;
         this.RPRT_PBLC_M.Name = "RPRT_PBLC_M";
         this.RPRT_PBLC_M.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.RPRT_PBLC_M_ItemClick);
         // 
         // RPRT_PRVT_M
         // 
         tileItemElement2.Appearance.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         tileItemElement2.Appearance.Normal.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
         tileItemElement2.Appearance.Normal.BorderColor = System.Drawing.Color.Blue;
         tileItemElement2.Appearance.Normal.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         tileItemElement2.Appearance.Normal.Options.UseBackColor = true;
         tileItemElement2.Appearance.Normal.Options.UseBorderColor = true;
         tileItemElement2.Appearance.Normal.Options.UseFont = true;
         tileItemElement2.Appearance.Normal.Options.UseTextOptions = true;
         tileItemElement2.Appearance.Normal.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
         tileItemElement2.Image = ((System.Drawing.Image)(resources.GetObject("tileItemElement2.Image")));
         tileItemElement2.Text = "چاپ خصوصی قبض های گاز";
         this.RPRT_PRVT_M.Elements.Add(tileItemElement2);
         this.RPRT_PRVT_M.IsLarge = true;
         this.RPRT_PRVT_M.Name = "RPRT_PRVT_M";
         this.RPRT_PRVT_M.ItemClick += new DevExpress.XtraEditors.TileItemClickEventHandler(this.RPRT_PRVT_M_ItemClick);
         // 
         // STRT_MTRO_M
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackgroundImage = global::System.Gas.Properties.Resources.IMAGE_1040;
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
         this.Controls.Add(this.Start_Menu);
         this.DoubleBuffered = true;
         this.Name = "STRT_MTRO_M";
         this.Size = new System.Drawing.Size(1156, 692);
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.TileControl Start_Menu;
      private DevExpress.XtraEditors.TileGroup Start_Group;
      private DevExpress.XtraEditors.TileItem RPRT_PBLC_M;
      private DevExpress.XtraEditors.TileItem RPRT_PRVT_M;
   }
}
