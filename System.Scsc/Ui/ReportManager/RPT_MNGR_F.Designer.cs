namespace System.Scsc.Ui.ReportManager
{
   partial class RPT_MNGR_F
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
         this.tabControl1 = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.vc_reportviewer = new Stimulsoft.Report.Viewer.StiRibbonViewerControl();
         this.tabControl1.SuspendLayout();
         this.tabPage1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tabControl1
         // 
         this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.tabControl1.Controls.Add(this.tabPage1);
         this.tabControl1.Location = new System.Drawing.Point(22, 23);
         this.tabControl1.Name = "tabControl1";
         this.tabControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.tabControl1.RightToLeftLayout = true;
         this.tabControl1.SelectedIndex = 0;
         this.tabControl1.Size = new System.Drawing.Size(738, 629);
         this.tabControl1.TabIndex = 0;
         // 
         // tabPage1
         // 
         this.tabPage1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(208)))), ((int)(((byte)(200)))));
         this.tabPage1.Controls.Add(this.vc_reportviewer);
         this.tabPage1.Location = new System.Drawing.Point(4, 22);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(730, 603);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "نمایش گزارش";
         // 
         // vc_reportviewer
         // 
         this.vc_reportviewer.AllowDrop = true;
         this.vc_reportviewer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.vc_reportviewer.IgnoreApplyStyle = false;
         this.vc_reportviewer.Location = new System.Drawing.Point(3, 3);
         this.vc_reportviewer.Name = "vc_reportviewer";
         this.vc_reportviewer.Report = null;
         this.vc_reportviewer.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.vc_reportviewer.ShowToolbar = false;
         this.vc_reportviewer.ShowZoom = true;
         this.vc_reportviewer.Size = new System.Drawing.Size(724, 597);
         this.vc_reportviewer.TabIndex = 0;
         this.vc_reportviewer.Close += new System.EventHandler(this.vc_reportviewer_Close);
         // 
         // RPT_MNGR_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
         this.Controls.Add(this.tabControl1);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "RPT_MNGR_F";
         this.Size = new System.Drawing.Size(783, 675);
         this.tabControl1.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.TabControl tabControl1;
      private Windows.Forms.TabPage tabPage1;
      private Stimulsoft.Report.Viewer.StiRibbonViewerControl vc_reportviewer;
   }
}
