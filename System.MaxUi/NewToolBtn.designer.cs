namespace System.MaxUi
{
    partial class NewToolBtn
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
            this.right = new System.Windows.Forms.Label();
            this.left = new System.Windows.Forms.Label();
            this.down = new System.Windows.Forms.Label();
            this.top = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // right
            // 
            this.right.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.right.BackColor = System.Drawing.Color.Transparent;
            this.right.Location = new System.Drawing.Point(39, 0);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(1, 39);
            this.right.TabIndex = 8;
            // 
            // left
            // 
            this.left.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.left.BackColor = System.Drawing.Color.Transparent;
            this.left.Location = new System.Drawing.Point(0, 0);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(1, 39);
            this.left.TabIndex = 7;
            // 
            // down
            // 
            this.down.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.down.BackColor = System.Drawing.Color.Transparent;
            this.down.Location = new System.Drawing.Point(0, 39);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(39, 1);
            this.down.TabIndex = 6;
            // 
            // top
            // 
            this.top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.top.BackColor = System.Drawing.Color.Transparent;
            this.top.Location = new System.Drawing.Point(0, 0);
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(39, 1);
            this.top.TabIndex = 4;
            // 
            // name
            // 
            this.name.AllowDrop = true;
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.name.BackColor = System.Drawing.Color.Transparent;
            this.name.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.name.Location = new System.Drawing.Point(2, 2);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(36, 36);
            this.name.TabIndex = 5;
            this.name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.name.MouseLeave += new System.EventHandler(this.name_MouseLeave);
            this.name.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.name_QueryContinueDrag);
            this.name.DragOver += new System.Windows.Forms.DragEventHandler(this.name_DragOver);
            this.name.MouseDown += new System.Windows.Forms.MouseEventHandler(this.name_MouseDown);
            this.name.DragDrop += new System.Windows.Forms.DragEventHandler(this.name_DragDrop);
            this.name.DragEnter += new System.Windows.Forms.DragEventHandler(this.name_DragEnter);
            this.name.MouseEnter += new System.EventHandler(this.name_MouseEnter);
            this.name.MouseUp += new System.Windows.Forms.MouseEventHandler(this.name_MouseUp);
            this.name.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.name_GiveFeedback);
            this.name.DragLeave += new System.EventHandler(this.name_DragLeave);
            // 
            // NewToolBtn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.right);
            this.Controls.Add(this.left);
            this.Controls.Add(this.down);
            this.Controls.Add(this.top);
            this.Controls.Add(this.name);
            this.Name = "NewToolBtn";
            this.Size = new System.Drawing.Size(40, 40);
            this.SizeChanged += new System.EventHandler(this.NewToolBtn_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label name;
        public System.Windows.Forms.Label left;
        public System.Windows.Forms.Label right;
        public System.Windows.Forms.Label down;
        public System.Windows.Forms.Label top;

    }
}
