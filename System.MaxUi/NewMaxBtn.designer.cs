namespace System.MaxUi
{
    partial class NewMaxBtn
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
         this.name = new System.Windows.Forms.Label();
         this.top = new System.Windows.Forms.Label();
         this.down = new System.Windows.Forms.Label();
         this.left = new System.Windows.Forms.Label();
         this.right = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // name
         // 
         this.name.AllowDrop = true;
         this.name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.name.BackColor = System.Drawing.Color.Transparent;
         this.name.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.name.ForeColor = System.Drawing.SystemColors.ControlText;
         this.name.Location = new System.Drawing.Point(2, 2);
         this.name.Name = "name";
         this.name.Size = new System.Drawing.Size(76, 21);
         this.name.TabIndex = 0;
         this.name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.name.DragDrop += new System.Windows.Forms.DragEventHandler(this.name_DragDrop);
         this.name.DragEnter += new System.Windows.Forms.DragEventHandler(this.name_DragEnter);
         this.name.DragOver += new System.Windows.Forms.DragEventHandler(this.name_DragOver);
         this.name.DragLeave += new System.EventHandler(this.name_DragLeave);
         this.name.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.name_GiveFeedback);
         this.name.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.name_QueryContinueDrag);
         this.name.MouseDown += new System.Windows.Forms.MouseEventHandler(this.name_MouseDown);
         this.name.MouseEnter += new System.EventHandler(this.name_MouseEnter);
         this.name.MouseUp += new System.Windows.Forms.MouseEventHandler(this.name_MouseUp);
         // 
         // top
         // 
         this.top.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
         this.top.Location = new System.Drawing.Point(0, 0);
         this.top.Name = "top";
         this.top.Size = new System.Drawing.Size(79, 1);
         this.top.TabIndex = 0;
         // 
         // down
         // 
         this.down.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.down.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
         this.down.Location = new System.Drawing.Point(0, 24);
         this.down.Name = "down";
         this.down.Size = new System.Drawing.Size(79, 1);
         this.down.TabIndex = 1;
         // 
         // left
         // 
         this.left.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
         this.left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(225)))));
         this.left.Location = new System.Drawing.Point(0, 0);
         this.left.Name = "left";
         this.left.Size = new System.Drawing.Size(1, 24);
         this.left.TabIndex = 2;
         // 
         // right
         // 
         this.right.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
         this.right.Location = new System.Drawing.Point(79, 0);
         this.right.Name = "right";
         this.right.Size = new System.Drawing.Size(1, 24);
         this.right.TabIndex = 3;
         // 
         // NewMaxBtn
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this.right);
         this.Controls.Add(this.left);
         this.Controls.Add(this.down);
         this.Controls.Add(this.top);
         this.Controls.Add(this.name);
         this.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Name = "NewMaxBtn";
         this.Size = new System.Drawing.Size(80, 25);
         this.SizeChanged += new System.EventHandler(this.NewMaxBtn_SizeChanged);
         this.Enter += new System.EventHandler(this.NewMaxBtn_Enter);
         this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewMaxBtn_KeyDown);
         this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.NewMaxBtn_KeyUp);
         this.Leave += new System.EventHandler(this.NewMaxBtn_Leave);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label top;
        private System.Windows.Forms.Label down;
        private System.Windows.Forms.Label left;
        private System.Windows.Forms.Label right;
        public System.Windows.Forms.Label name;
    }
}
