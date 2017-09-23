namespace System.MaxUi
{
    partial class Rollout03
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
         this.Rollout = new System.Windows.Forms.Label();
         this.plus_min = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // Rollout
         // 
         this.Rollout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Rollout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
         this.Rollout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.Rollout.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.Rollout.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
         this.Rollout.Location = new System.Drawing.Point(7, 0);
         this.Rollout.Name = "Rollout";
         this.Rollout.Size = new System.Drawing.Size(290, 18);
         this.Rollout.TabIndex = 2;
         this.Rollout.Text = "نام سربرگ";
         this.Rollout.TextAlign = System.Drawing.ContentAlignment.TopCenter;
         this.Rollout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rollout_MouseDown);
         this.Rollout.MouseEnter += new System.EventHandler(this.Rollout_MouseEnter);
         this.Rollout.MouseLeave += new System.EventHandler(this.Rollout_MouseLeave);
         // 
         // plus_min
         // 
         this.plus_min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.plus_min.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
         this.plus_min.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
         this.plus_min.Location = new System.Drawing.Point(275, 1);
         this.plus_min.Name = "plus_min";
         this.plus_min.Size = new System.Drawing.Size(14, 13);
         this.plus_min.TabIndex = 0;
         this.plus_min.Text = "̶";
         this.plus_min.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Rollout_MouseDown);
         this.plus_min.MouseEnter += new System.EventHandler(this.Rollout_MouseEnter);
         this.plus_min.MouseLeave += new System.EventHandler(this.Rollout_MouseLeave);
         // 
         // Rollout03
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this.plus_min);
         this.Controls.Add(this.Rollout);
         this.Name = "Rollout03";
         this.Size = new System.Drawing.Size(304, 215);
         this.Resize += new System.EventHandler(this.Rollout03_Resize);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label plus_min;
        internal System.Windows.Forms.Label Rollout;
    }
}
