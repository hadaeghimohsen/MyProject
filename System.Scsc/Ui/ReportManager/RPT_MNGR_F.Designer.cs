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
         this.vc_reportviewer = new Stimulsoft.Report.Viewer.StiRibbonViewerControl();
         this.SuspendLayout();
         // 
         // vc_reportviewer
         // 
         this.vc_reportviewer.AllowDrop = true;
         this.vc_reportviewer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.vc_reportviewer.IgnoreApplyStyle = false;
         this.vc_reportviewer.Location = new System.Drawing.Point(0, 0);
         this.vc_reportviewer.Name = "vc_reportviewer";
         this.vc_reportviewer.PageViewMode = Stimulsoft.Report.Viewer.StiPageViewMode.SinglePage;
         this.vc_reportviewer.Report = null;
         this.vc_reportviewer.RightToLeft = System.Windows.Forms.RightToLeft.No;
         this.vc_reportviewer.ShowToolbar = false;
         this.vc_reportviewer.ShowZoom = true;
         this.vc_reportviewer.Size = new System.Drawing.Size(1070, 675);
         this.vc_reportviewer.TabIndex = 0;
         this.vc_reportviewer.Close += new System.EventHandler(this.vc_reportviewer_Close);
         // 
         // RPT_MNGR_F
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Control;
         this.Controls.Add(this.vc_reportviewer);
         this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.Name = "RPT_MNGR_F";
         this.Size = new System.Drawing.Size(1070, 675);
         this.ResumeLayout(false);

      }

      #endregion

      private Stimulsoft.Report.Viewer.StiRibbonViewerControl vc_reportviewer;
   }
}
