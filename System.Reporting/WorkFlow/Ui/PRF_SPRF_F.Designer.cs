namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_SPRF_F
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
         DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PRF_SPRF_F));
         this.cbe_role = new DevExpress.XtraEditors.CheckedComboBoxEdit();
         this.Tile_Ctrl = new DevExpress.XtraEditors.TileControl();
         this.bp_flow = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         ((System.ComponentModel.ISupportInitialize)(this.cbe_role.Properties)).BeginInit();
         this.SuspendLayout();
         // 
         // cbe_role
         // 
         this.cbe_role.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cbe_role.Location = new System.Drawing.Point(422, 28);
         this.cbe_role.Name = "cbe_role";
         this.cbe_role.Properties.AllowMultiSelect = true;
         this.cbe_role.Properties.Appearance.BackColor = System.Drawing.Color.MidnightBlue;
         this.cbe_role.Properties.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cbe_role.Properties.Appearance.ForeColor = System.Drawing.Color.White;
         this.cbe_role.Properties.Appearance.Options.UseBackColor = true;
         this.cbe_role.Properties.Appearance.Options.UseFont = true;
         this.cbe_role.Properties.Appearance.Options.UseForeColor = true;
         this.cbe_role.Properties.Appearance.Options.UseTextOptions = true;
         this.cbe_role.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.cbe_role.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo, "", 25, true, true, true, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
         this.cbe_role.Properties.LookAndFeel.SkinName = "Office 2013 Light Gray";
         this.cbe_role.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.cbe_role.Properties.NullValuePrompt = "گروه های دسترسی";
         this.cbe_role.Properties.NullValuePromptShowForEmptyValue = true;
         this.cbe_role.Size = new System.Drawing.Size(374, 40);
         this.cbe_role.TabIndex = 103;
         this.cbe_role.EditValueChanged += new System.EventHandler(this.cbe_role_EditValueChanged);
         // 
         // Tile_Ctrl
         // 
         this.Tile_Ctrl.AllowGroupHighlighting = true;
         this.Tile_Ctrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Tile_Ctrl.AppearanceGroupText.Font = new System.Drawing.Font("B Koodak", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Tile_Ctrl.AppearanceGroupText.ForeColor = System.Drawing.Color.White;
         this.Tile_Ctrl.AppearanceGroupText.Options.UseFont = true;
         this.Tile_Ctrl.AppearanceGroupText.Options.UseForeColor = true;
         this.Tile_Ctrl.AppearanceGroupText.Options.UseTextOptions = true;
         this.Tile_Ctrl.AppearanceGroupText.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
         this.Tile_Ctrl.AppearanceText.Font = new System.Drawing.Font("B Koodak", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Tile_Ctrl.AppearanceText.ForeColor = System.Drawing.Color.Yellow;
         this.Tile_Ctrl.AppearanceText.Options.UseFont = true;
         this.Tile_Ctrl.AppearanceText.Options.UseForeColor = true;
         this.Tile_Ctrl.HorizontalContentAlignment = DevExpress.Utils.HorzAlignment.Near;
         this.Tile_Ctrl.ItemContentAnimation = DevExpress.XtraEditors.TileItemContentAnimationType.ScrollTop;
         this.Tile_Ctrl.Location = new System.Drawing.Point(13, 89);
         this.Tile_Ctrl.LookAndFeel.SkinName = "Office 2013";
         this.Tile_Ctrl.LookAndFeel.UseDefaultLookAndFeel = false;
         this.Tile_Ctrl.MaxId = 18;
         this.Tile_Ctrl.Name = "Tile_Ctrl";
         this.Tile_Ctrl.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollBar;
         this.Tile_Ctrl.ShowGroupText = true;
         this.Tile_Ctrl.ShowText = true;
         this.Tile_Ctrl.Size = new System.Drawing.Size(783, 438);
         this.Tile_Ctrl.TabIndex = 104;
         this.Tile_Ctrl.Text = "پروفایل";
         this.Tile_Ctrl.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Top;
         this.Tile_Ctrl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tile_Ctrl_MouseClick);
         // 
         // bp_flow
         // 
         this.bp_flow.AppearanceButton.Hovered.Font = new System.Drawing.Font("Tahoma", 9.75F);
         this.bp_flow.AppearanceButton.Hovered.Options.UseFont = true;
         this.bp_flow.AppearanceButton.Normal.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.bp_flow.AppearanceButton.Normal.Options.UseFont = true;
         this.bp_flow.BackColor = System.Drawing.Color.MidnightBlue;
         this.bp_flow.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("1", ((System.Drawing.Image)(resources.GetObject("bp_flow.Buttons"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, 0, true, null, true, false, true, null, null, 0, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("2", ((System.Drawing.Image)(resources.GetObject("bp_flow.Buttons1"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, 1, false, null, true, false, true, null, null, 0, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("3", null, -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, 2, false, null, true, false, true, null, null, 0, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("bp_flow.Buttons2"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, null, -1, true, false)});
         this.bp_flow.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.bp_flow.ForeColor = System.Drawing.Color.White;
         this.bp_flow.Location = new System.Drawing.Point(0, 578);
         this.bp_flow.Name = "bp_flow";
         this.bp_flow.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
         this.bp_flow.Size = new System.Drawing.Size(813, 87);
         this.bp_flow.TabIndex = 105;
         this.bp_flow.Text = "windowsUIButtonPanel1";
         this.bp_flow.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.bp_flow_ButtonClick);
         // 
         // PRF_SPRF_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Navy;
         this.Controls.Add(this.bp_flow);
         this.Controls.Add(this.Tile_Ctrl);
         this.Controls.Add(this.cbe_role);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Name = "PRF_SPRF_F";
         this.Size = new System.Drawing.Size(813, 665);
         ((System.ComponentModel.ISupportInitialize)(this.cbe_role.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraEditors.CheckedComboBoxEdit cbe_role;
      private DevExpress.XtraEditors.TileControl Tile_Ctrl;
      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel bp_flow;
   }
}
