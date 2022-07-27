namespace MyProject.Commons.Desktop.Ui
{
   partial class StartDrawer
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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartDrawer));
         this.Btn_Start = new System.MaxUi.NewToolBtn();
         this.imageList1 = new System.Windows.Forms.ImageList(this.components);
         this.Btn_SettingsAccount = new System.MaxUi.NewToolBtn();
         this.Btn_Settings = new System.MaxUi.NewToolBtn();
         this.SuspendLayout();
         // 
         // Btn_Start
         // 
         this.Btn_Start.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.Btn_Start.BackColor = System.Drawing.Color.Transparent;
         this.Btn_Start.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_Start.Caption = "";
         this.Btn_Start.Disabled = false;
         this.Btn_Start.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Start.ImageIndex = 0;
         this.Btn_Start.ImageList = this.imageList1;
         this.Btn_Start.Location = new System.Drawing.Point(3, 87);
         this.Btn_Start.Name = "Btn_Start";
         this.Btn_Start.Size = new System.Drawing.Size(75, 75);
         this.Btn_Start.TabIndex = 0;
         this.Btn_Start.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Start.TextColor = System.Drawing.Color.Empty;
         this.Btn_Start.ToolDownFont = null;
         this.Btn_Start.ToolUpFont = null;
         // 
         // imageList1
         // 
         this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
         this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
         this.imageList1.Images.SetKeyName(0, "IMAGE_1757.png");
         this.imageList1.Images.SetKeyName(1, "IMAGE_1133.png");
         this.imageList1.Images.SetKeyName(2, "IMAGE_1370.png");
         this.imageList1.Images.SetKeyName(3, "IMAGE_1427.png");
         // 
         // Btn_SettingsAccount
         // 
         this.Btn_SettingsAccount.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.Btn_SettingsAccount.BackColor = System.Drawing.Color.Transparent;
         this.Btn_SettingsAccount.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_SettingsAccount.Caption = "";
         this.Btn_SettingsAccount.Disabled = false;
         this.Btn_SettingsAccount.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_SettingsAccount.ImageIndex = 3;
         this.Btn_SettingsAccount.ImageList = this.imageList1;
         this.Btn_SettingsAccount.Location = new System.Drawing.Point(3, 168);
         this.Btn_SettingsAccount.Name = "Btn_SettingsAccount";
         this.Btn_SettingsAccount.Size = new System.Drawing.Size(75, 75);
         this.Btn_SettingsAccount.TabIndex = 0;
         this.Btn_SettingsAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_SettingsAccount.TextColor = System.Drawing.Color.Empty;
         this.Btn_SettingsAccount.ToolDownFont = null;
         this.Btn_SettingsAccount.ToolUpFont = null;
         this.Btn_SettingsAccount.Click += new System.EventHandler(this.Btn_SettingsAccount_Click);
         // 
         // Btn_Settings
         // 
         this.Btn_Settings.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.Btn_Settings.BackColor = System.Drawing.Color.Transparent;
         this.Btn_Settings.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
         this.Btn_Settings.Caption = "";
         this.Btn_Settings.Disabled = false;
         this.Btn_Settings.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Settings.ImageIndex = 2;
         this.Btn_Settings.ImageList = this.imageList1;
         this.Btn_Settings.Location = new System.Drawing.Point(3, 249);
         this.Btn_Settings.Name = "Btn_Settings";
         this.Btn_Settings.Size = new System.Drawing.Size(75, 75);
         this.Btn_Settings.TabIndex = 0;
         this.Btn_Settings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Btn_Settings.TextColor = System.Drawing.Color.Empty;
         this.Btn_Settings.ToolDownFont = null;
         this.Btn_Settings.ToolUpFont = null;
         this.Btn_Settings.Click += new System.EventHandler(this.Btn_Settings_Click);
         // 
         // StartDrawer
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.LightGray;
         this.Controls.Add(this.Btn_Settings);
         this.Controls.Add(this.Btn_SettingsAccount);
         this.Controls.Add(this.Btn_Start);
         this.Name = "StartDrawer";
         this.Size = new System.Drawing.Size(100, 411);
         this.ResumeLayout(false);

      }

      #endregion

      private System.MaxUi.NewToolBtn Btn_Start;
      private System.Windows.Forms.ImageList imageList1;
      private System.MaxUi.NewToolBtn Btn_SettingsAccount;
      private System.MaxUi.NewToolBtn Btn_Settings;
   }
}
