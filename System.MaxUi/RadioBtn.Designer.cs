namespace System.MaxUi
{
    partial class RadioBtn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadioBtn));
            this.Ltext = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Limage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Ltext
            // 
            this.Ltext.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Ltext.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Ltext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ltext.ImageIndex = 0;
            this.Ltext.Location = new System.Drawing.Point(2, 3);
            this.Ltext.Name = "Ltext";
            this.Ltext.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Ltext.Size = new System.Drawing.Size(93, 19);
            this.Ltext.TabIndex = 0;
            this.Ltext.Text = "منوی تک انتخابی";
            this.Ltext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Ltext.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LImage_MouseDown);
            this.Ltext.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LImage_MouseUp);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "N.png");
            this.imageList1.Images.SetKeyName(1, "DN.png");
            this.imageList1.Images.SetKeyName(2, "SN.png");
            this.imageList1.Images.SetKeyName(3, "S.png");
            this.imageList1.Images.SetKeyName(4, "DS.png");
            this.imageList1.Images.SetKeyName(5, "SS.png");
            // 
            // Limage
            // 
            this.Limage.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Limage.ImageIndex = 0;
            this.Limage.ImageList = this.imageList1;
            this.Limage.Location = new System.Drawing.Point(96, 6);
            this.Limage.Name = "Limage";
            this.Limage.Size = new System.Drawing.Size(13, 13);
            this.Limage.TabIndex = 1;
            this.Limage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LImage_MouseDown);
            this.Limage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LImage_MouseUp);
            // 
            // RadioBtn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Limage);
            this.Controls.Add(this.Ltext);
            this.Name = "RadioBtn";
            this.Size = new System.Drawing.Size(112, 25);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LImage_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LImage_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Ltext;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label Limage;
    }
}
