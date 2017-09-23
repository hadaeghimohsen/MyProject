namespace System.Reporting.WorkFlow.Ui
{
   partial class WHR_SCON_F
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WHR_SCON_F));
         this.bp_flow = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
         this.mpbc_loading = new DevExpress.XtraEditors.MarqueeProgressBarControl();
         this.flp_wherecondition = new System.Windows.Forms.FlowLayoutPanel();
         this.cb_datasource = new System.Windows.Forms.ComboBox();
         this.bp_flow.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.mpbc_loading.Properties)).BeginInit();
         this.SuspendLayout();
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
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("2", ((System.Drawing.Image)(resources.GetObject("bp_flow.Buttons1"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, 1, true, null, true, false, true, null, null, 0, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("3", ((System.Drawing.Image)(resources.GetObject("bp_flow.Buttons2"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, 2, false, null, true, false, true, null, null, 0, false, false),
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", ((System.Drawing.Image)(resources.GetObject("bp_flow.Buttons3"))), -1, DevExpress.XtraBars.Docking2010.ImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, true, false, true, null, null, -1, true, false)});
         this.bp_flow.Controls.Add(this.mpbc_loading);
         this.bp_flow.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.bp_flow.ForeColor = System.Drawing.Color.White;
         this.bp_flow.Location = new System.Drawing.Point(0, 578);
         this.bp_flow.Name = "bp_flow";
         this.bp_flow.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
         this.bp_flow.Size = new System.Drawing.Size(813, 87);
         this.bp_flow.TabIndex = 106;
         this.bp_flow.Text = "windowsUIButtonPanel1";
         this.bp_flow.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.bp_flow_ButtonClick);
         // 
         // mpbc_loading
         // 
         this.mpbc_loading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.mpbc_loading.EditValue = "Report file downloading...";
         this.mpbc_loading.Location = new System.Drawing.Point(664, 26);
         this.mpbc_loading.Name = "mpbc_loading";
         this.mpbc_loading.Properties.LookAndFeel.SkinName = "Office 2013";
         this.mpbc_loading.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
         this.mpbc_loading.Properties.ShowTitle = true;
         this.mpbc_loading.Properties.TextOrientation = DevExpress.Utils.Drawing.TextOrientation.Horizontal;
         this.mpbc_loading.Size = new System.Drawing.Size(146, 29);
         this.mpbc_loading.TabIndex = 0;
         // 
         // flp_wherecondition
         // 
         this.flp_wherecondition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.flp_wherecondition.AutoScroll = true;
         this.flp_wherecondition.BackColor = System.Drawing.Color.Transparent;
         this.flp_wherecondition.Location = new System.Drawing.Point(18, 74);
         this.flp_wherecondition.Name = "flp_wherecondition";
         this.flp_wherecondition.Padding = new System.Windows.Forms.Padding(0, 0, 0, 50);
         this.flp_wherecondition.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.flp_wherecondition.Size = new System.Drawing.Size(778, 485);
         this.flp_wherecondition.TabIndex = 107;
         // 
         // cb_datasource
         // 
         this.cb_datasource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.cb_datasource.BackColor = System.Drawing.Color.MidnightBlue;
         this.cb_datasource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.cb_datasource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.cb_datasource.Font = new System.Drawing.Font("B Koodak", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.cb_datasource.ForeColor = System.Drawing.Color.White;
         this.cb_datasource.FormattingEnabled = true;
         this.cb_datasource.Location = new System.Drawing.Point(392, 23);
         this.cb_datasource.Name = "cb_datasource";
         this.cb_datasource.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.cb_datasource.Size = new System.Drawing.Size(404, 36);
         this.cb_datasource.TabIndex = 108;
         // 
         // WHR_SCON_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Navy;
         this.Controls.Add(this.cb_datasource);
         this.Controls.Add(this.flp_wherecondition);
         this.Controls.Add(this.bp_flow);
         this.ForeColor = System.Drawing.Color.White;
         this.Name = "WHR_SCON_F";
         this.Size = new System.Drawing.Size(813, 665);
         this.bp_flow.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.mpbc_loading.Properties)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel bp_flow;
      private Windows.Forms.FlowLayoutPanel flp_wherecondition;
      private Windows.Forms.ComboBox cb_datasource;
      private DevExpress.XtraEditors.MarqueeProgressBarControl mpbc_loading;

   }
}
