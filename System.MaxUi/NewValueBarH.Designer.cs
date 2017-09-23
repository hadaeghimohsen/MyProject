namespace System.MaxUi
{
    partial class NewValueBarH
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewValueBarH));
            this.Frame = new System.Windows.Forms.Panel();
            this.Progress = new System.Windows.Forms.Label();
            this.DMin = new System.Windows.Forms.Label();
            this.DMax = new System.Windows.Forms.Label();
            this.ValueChange = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Bar = new System.Windows.Forms.Timer(this.components);
            this.Frame.SuspendLayout();
            this.SuspendLayout();
            // 
            // Frame
            // 
            this.Frame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Frame.BackColor = System.Drawing.SystemColors.Window;
            this.Frame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Frame.Controls.Add(this.Progress);
            this.Frame.Location = new System.Drawing.Point(9, 3);
            this.Frame.Name = "Frame";
            this.Frame.Size = new System.Drawing.Size(173, 24);
            this.Frame.TabIndex = 0;
            // 
            // Progress
            // 
            this.Progress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Progress.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Progress.Location = new System.Drawing.Point(1, 1);
            this.Progress.Name = "Progress";
            this.Progress.Size = new System.Drawing.Size(85, 19);
            this.Progress.TabIndex = 0;
            // 
            // DMin
            // 
            this.DMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DMin.AutoSize = true;
            this.DMin.BackColor = System.Drawing.Color.Transparent;
            this.DMin.Location = new System.Drawing.Point(3, 42);
            this.DMin.Name = "DMin";
            this.DMin.Size = new System.Drawing.Size(15, 13);
            this.DMin.TabIndex = 0;
            this.DMin.Text = "m";
            // 
            // DMax
            // 
            this.DMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DMax.AutoSize = true;
            this.DMax.BackColor = System.Drawing.Color.Transparent;
            this.DMax.Location = new System.Drawing.Point(172, 42);
            this.DMax.Name = "DMax";
            this.DMax.Size = new System.Drawing.Size(16, 13);
            this.DMax.TabIndex = 0;
            this.DMax.Text = "M";
            // 
            // ValueChange
            // 
            this.ValueChange.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ValueChange.BackColor = System.Drawing.Color.Transparent;
            this.ValueChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ValueChange.ImageIndex = 0;
            this.ValueChange.ImageList = this.imageList1;
            this.ValueChange.Location = new System.Drawing.Point(88, 28);
            this.ValueChange.Name = "ValueChange";
            this.ValueChange.Size = new System.Drawing.Size(18, 10);
            this.ValueChange.TabIndex = 0;
            this.ValueChange.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ValueChange_MouseDown);
            this.ValueChange.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ValueChange_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ValueChange.ico");
            this.imageList1.Images.SetKeyName(1, "PFSizeTestNub.ico");
            // 
            // Bar
            // 
            this.Bar.Interval = 1;
            this.Bar.Tick += new System.EventHandler(this.Bar_Tick);
            // 
            // NewValueBarH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ValueChange);
            this.Controls.Add(this.DMax);
            this.Controls.Add(this.DMin);
            this.Controls.Add(this.Frame);
            this.Name = "NewValueBarH";
            this.Size = new System.Drawing.Size(190, 63);
            this.RightToLeftChanged += new System.EventHandler(this.NewValueBarH_RightToLeftChanged);
            this.SizeChanged += new System.EventHandler(this.ValueBarH_SizeChanged);
            this.Frame.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Frame;
        private System.Windows.Forms.Label Progress;
        private System.Windows.Forms.Label DMin;
        private System.Windows.Forms.Label DMax;
        private System.Windows.Forms.Label ValueChange;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer Bar;
    }
}
