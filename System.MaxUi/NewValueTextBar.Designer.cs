namespace System.MaxUi
{
    partial class NewValueTextBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewValueTextBar));
            this.TextValue = new System.Windows.Forms.RichTextBox();
            this.Frame = new System.Windows.Forms.Panel();
            this.ValueChange = new NewMaxBtn();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.Bar = new System.Windows.Forms.Timer(this.components);
            this.ShowCV = new NewMaxBtn();
            this.Frame.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextValue
            // 
            this.TextValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.TextValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.TextValue.Location = new System.Drawing.Point(25, 0);
            this.TextValue.Multiline = false;
            this.TextValue.Name = "TextValue";
            this.TextValue.Size = new System.Drawing.Size(65, 18);
            this.TextValue.TabIndex = 0;
            this.TextValue.Text = "0";
            this.TextValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextValue_KeyDown);
            this.TextValue.Enter += new System.EventHandler(this.TextValue_Enter);
            this.TextValue.Leave += new System.EventHandler(this.TextValue_Leave);
            // 
            // Frame
            // 
            this.Frame.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Frame.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Frame.Controls.Add(this.ValueChange);
            this.Frame.Controls.Add(this.label1);
            this.Frame.Location = new System.Drawing.Point(1, 19);
            this.Frame.Name = "Frame";
            this.Frame.Size = new System.Drawing.Size(88, 16);
            this.Frame.TabIndex = 2;
            // 
            // ValueChange
            // 
            this.ValueChange.BackColor = System.Drawing.Color.Transparent;
            this.ValueChange.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
            this.ValueChange.Caption = "";
            this.ValueChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ValueChange.Disabled = false;
            this.ValueChange.EnterColor = System.Drawing.Color.Transparent;
            this.ValueChange.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ValueChange.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ValueChange.ImageIndex = -1;
            this.ValueChange.ImageList = null;
            this.ValueChange.InToBold = false;
            this.ValueChange.Location = new System.Drawing.Point(37, 1);
            this.ValueChange.Name = "ValueChange";
            this.ValueChange.Size = new System.Drawing.Size(10, 10);
            this.ValueChange.TabIndex = 3;
            this.ValueChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ValueChange.TextColor = System.Drawing.Color.Black;
            this.ValueChange.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
            this.ValueChange.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ValueChange_MouseDown);
            this.ValueChange.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ValueChange_MouseUp);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(1, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 2);
            this.label1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ValueSetH.ico");
            // 
            // Bar
            // 
            this.Bar.Interval = 1;
            this.Bar.Tick += new System.EventHandler(this.Bar_Tick);
            // 
            // ShowCV
            // 
            this.ShowCV.BackColor = System.Drawing.Color.Transparent;
            this.ShowCV.BackGroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(204)))), ((int)(((byte)(85)))));
            this.ShowCV.Caption = "";
            this.ShowCV.Disabled = false;
            this.ShowCV.EnterColor = System.Drawing.Color.Transparent;
            this.ShowCV.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ShowCV.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ShowCV.ImageIndex = 0;
            this.ShowCV.ImageList = this.imageList1;
            this.ShowCV.InToBold = false;
            this.ShowCV.Location = new System.Drawing.Point(1, 0);
            this.ShowCV.Name = "ShowCV";
            this.ShowCV.Size = new System.Drawing.Size(23, 18);
            this.ShowCV.TabIndex = 1;
            this.ShowCV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ShowCV.TextColor = System.Drawing.Color.Black;
            this.ShowCV.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
            this.ShowCV.Click += new System.EventHandler(this.ShowCV_Click);
            // 
            // NewValueTextBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Frame);
            this.Controls.Add(this.ShowCV);
            this.Controls.Add(this.TextValue);
            this.Name = "NewValueTextBar";
            this.Size = new System.Drawing.Size(90, 36);
            this.Load += new System.EventHandler(this.NewValueTextBar_Load);
            this.RightToLeftChanged += new System.EventHandler(this.NewValueTextBar_RightToLeftChanged);
            this.Frame.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox TextValue;
        private NewMaxBtn ShowCV;
        private System.Windows.Forms.Panel Frame;
        private NewMaxBtn ValueChange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer Bar;
    }
}
