namespace System.MaxUi
{
    partial class NumUpDown
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NumUpDown));
            this.ValueText = new System.Windows.Forms.RichTextBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Runner = new System.Windows.Forms.Timer(this.components);
            this.Up = new NewMaxBtn();
            this.Down = new NewMaxBtn();
            this.SuspendLayout();
            // 
            // ValueText
            // 
            this.ValueText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ValueText.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ValueText.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.ValueText.Location = new System.Drawing.Point(13, 0);
            this.ValueText.Multiline = false;
            this.ValueText.Name = "ValueText";
            this.ValueText.Size = new System.Drawing.Size(46, 18);
            this.ValueText.TabIndex = 0;
            this.ValueText.Text = "0";
            this.ValueText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ValueText_KeyDown);
            this.ValueText.Leave += new System.EventHandler(this.ValueText_Leave);
            this.ValueText.TextChanged += new System.EventHandler(this.ValueText_TextChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Up.ico");
            this.imageList1.Images.SetKeyName(1, "Down.ico");
            // 
            // Runner
            // 
            this.Runner.Tick += new System.EventHandler(this.Runner_Tick);
            // 
            // Up
            // 
            this.Up.BackColor = System.Drawing.Color.Transparent;
            this.Up.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
            this.Up.Caption = "";
            this.Up.Disabled = false;
            this.Up.EnterColor = System.Drawing.Color.Transparent;
            this.Up.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Up.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Up.ImageIndex = 0;
            this.Up.ImageList = this.imageList1;
            this.Up.InToBold = false;
            this.Up.Location = new System.Drawing.Point(0, 0);
            this.Up.Name = "Up";
            this.Up.Size = new System.Drawing.Size(13, 9);
            this.Up.TabIndex = 1;
            this.Up.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Up.TextColor = System.Drawing.Color.Black;
            this.Up.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
            this.Up.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Up_KeyUp);
            this.Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Up_MouseDown);
            this.Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Up_MouseUp);
            this.Up.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Up_KeyDown);
            // 
            // Down
            // 
            this.Down.BackColor = System.Drawing.Color.Transparent;
            this.Down.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
            this.Down.Caption = "";
            this.Down.Disabled = false;
            this.Down.EnterColor = System.Drawing.Color.Transparent;
            this.Down.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Down.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Down.ImageIndex = 1;
            this.Down.ImageList = this.imageList1;
            this.Down.InToBold = false;
            this.Down.Location = new System.Drawing.Point(0, 9);
            this.Down.Name = "Down";
            this.Down.Size = new System.Drawing.Size(13, 9);
            this.Down.TabIndex = 2;
            this.Down.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Down.TextColor = System.Drawing.Color.Black;
            this.Down.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
            this.Down.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Down_KeyUp);
            this.Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_MouseDown);
            this.Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Down_MouseUp);
            this.Down.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Down_KeyDown);
            // 
            // NumUpDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Up);
            this.Controls.Add(this.Down);
            this.Controls.Add(this.ValueText);
            this.Name = "NumUpDown";
            this.Size = new System.Drawing.Size(59, 18);
            this.Load += new System.EventHandler(this.NumUpDown_Load);
            this.RightToLeftChanged += new System.EventHandler(this.NumUpDown_RightToLeftChanged);
            this.SizeChanged += new System.EventHandler(this.NumUpDown_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox ValueText;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer Runner;
        private NewMaxBtn Down;
        private NewMaxBtn Up;
    }
}
