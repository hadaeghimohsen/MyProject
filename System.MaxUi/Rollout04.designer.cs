namespace System.MaxUi
{
    partial class Rollout04
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
         this.plus_min = new System.Windows.Forms.Label();
         this.Rollout = new System.MaxUi.NewMaxBtn();
         this.SuspendLayout();
         // 
         // plus_min
         // 
         this.plus_min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.plus_min.BackColor = System.Drawing.Color.Transparent;
         this.plus_min.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
         this.plus_min.Location = new System.Drawing.Point(277, 2);
         this.plus_min.Name = "plus_min";
         this.plus_min.Size = new System.Drawing.Size(14, 13);
         this.plus_min.TabIndex = 1;
         this.plus_min.Text = "̶";
         this.plus_min.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rollout_MouseUp);
         // 
         // Rollout
         // 
         this.Rollout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.Rollout.BackColor = System.Drawing.Color.Transparent;
         this.Rollout.BackGroundColor = System.Drawing.Color.Transparent;
         this.Rollout.Caption = "";
         this.Rollout.Disabled = false;
         this.Rollout.EnterColor = System.Drawing.Color.Transparent;
         this.Rollout.ForeColor = System.Drawing.SystemColors.ControlText;
         this.Rollout.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Rollout.ImageIndex = -1;
         this.Rollout.ImageList = null;
         this.Rollout.InToBold = false;
         this.Rollout.Location = new System.Drawing.Point(7, 0);
         this.Rollout.Name = "Rollout";
         this.Rollout.Size = new System.Drawing.Size(290, 18);
         this.Rollout.TabIndex = 2;
         this.Rollout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.Rollout.TextColor = System.Drawing.Color.Black;
         this.Rollout.TextFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)), true);
         this.Rollout.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Rollout_MouseUp);
         // 
         // Rollout04
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this.plus_min);
         this.Controls.Add(this.Rollout);
         this.Name = "Rollout04";
         this.Size = new System.Drawing.Size(304, 215);
         this.Resize += new System.EventHandler(this.Rollout04_Resize);
         this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label plus_min;
        internal NewMaxBtn Rollout;
    }
}
