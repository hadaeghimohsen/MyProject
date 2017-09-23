namespace System.MaxUi
{
    partial class GroupBox
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
         this.top = new System.Windows.Forms.Label();
         this.title = new System.Windows.Forms.Label();
         this.down = new System.Windows.Forms.Label();
         this.left = new System.Windows.Forms.Label();
         this.right = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // top
         // 
         this.top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
         this.top.Location = new System.Drawing.Point(0, 11);
         this.top.Name = "top";
         this.top.Size = new System.Drawing.Size(122, 1);
         this.top.TabIndex = 1;
         // 
         // title
         // 
         this.title.AutoSize = true;
         this.title.BackColor = System.Drawing.Color.Transparent;
         this.title.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.title.Location = new System.Drawing.Point(85, 3);
         this.title.Name = "title";
         this.title.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
         this.title.Size = new System.Drawing.Size(24, 11);
         this.title.TabIndex = 2;
         this.title.Text = "گروه:";
         // 
         // down
         // 
         this.down.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.down.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
         this.down.Location = new System.Drawing.Point(0, 149);
         this.down.Name = "down";
         this.down.Size = new System.Drawing.Size(122, 1);
         this.down.TabIndex = 3;
         // 
         // left
         // 
         this.left.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
         this.left.Location = new System.Drawing.Point(0, 11);
         this.left.Name = "left";
         this.left.Size = new System.Drawing.Size(1, 139);
         this.left.TabIndex = 4;
         // 
         // right
         // 
         this.right.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
         this.right.Location = new System.Drawing.Point(121, 11);
         this.right.Name = "right";
         this.right.Size = new System.Drawing.Size(1, 139);
         this.right.TabIndex = 5;
         // 
         // GroupBox
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this.title);
         this.Controls.Add(this.right);
         this.Controls.Add(this.left);
         this.Controls.Add(this.down);
         this.Controls.Add(this.top);
         this.Name = "GroupBox";
         this.Size = new System.Drawing.Size(122, 150);
         this.RightToLeftChanged += new System.EventHandler(this.GroupBox_RightToLeftChanged);
         this.SizeChanged += new System.EventHandler(this.GroupBox_SizeChanged);
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label top;
        private System.Windows.Forms.Label down;
        private System.Windows.Forms.Label left;
        private System.Windows.Forms.Label right;
        private System.Windows.Forms.Label title;
    }
}
