namespace System.Reporting.ReportViewers.Ui
{
   partial class Viewers
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
         this.cr_reportviewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
         this.SuspendLayout();
         // 
         // cr_reportviewer
         // 
         this.cr_reportviewer.ActiveViewIndex = -1;
         this.cr_reportviewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.cr_reportviewer.Cursor = System.Windows.Forms.Cursors.Default;
         this.cr_reportviewer.Dock = System.Windows.Forms.DockStyle.Fill;
         this.cr_reportviewer.Location = new System.Drawing.Point(0, 0);
         this.cr_reportviewer.Name = "cr_reportviewer";
         this.cr_reportviewer.Size = new System.Drawing.Size(769, 670);
         this.cr_reportviewer.TabIndex = 0;
         this.cr_reportviewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
         // 
         // Viewers
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.cr_reportviewer);
         this.Name = "Viewers";
         this.Size = new System.Drawing.Size(769, 670);
         this.ResumeLayout(false);

      }

      #endregion

      private CrystalDecisions.Windows.Forms.CrystalReportViewer cr_reportviewer;
   }
}
