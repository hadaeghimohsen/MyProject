namespace System.MaxUi
{
   partial class RoundedButton
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
         this._image_pb = new System.Windows.Forms.PictureBox();
         this.BallonTip_Tip = new System.Windows.Forms.ToolTip();
         ((System.ComponentModel.ISupportInitialize)(this._image_pb)).BeginInit();
         this.SuspendLayout();
         // 
         // _image_pb
         // 
         this._image_pb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this._image_pb.Location = new System.Drawing.Point(0, 0);
         this._image_pb.Name = "_image_pb";
         this._image_pb.Size = new System.Drawing.Size(150, 150);
         this._image_pb.TabIndex = 0;
         this._image_pb.TabStop = false;
         this._image_pb.Click += new System.EventHandler(this._image_pb_Click);
         this._image_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this._image_pb_MouseDown);
         this._image_pb.MouseEnter += new System.EventHandler(this._image_pb_MouseEnter);
         this._image_pb.MouseLeave += new System.EventHandler(this._image_pb_MouseLeave);
         this._image_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this._image_pb_MouseUp);
         // 
         // BallonTip_Tip
         // 
         this.BallonTip_Tip.AutoPopDelay = 5000;
         this.BallonTip_Tip.InitialDelay = 500;
         this.BallonTip_Tip.ReshowDelay = 0;
         this.BallonTip_Tip.ShowAlways = true;
         // 
         // RoundedButton
         // 
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this._image_pb);
         this.Name = "RoundedButton";
         ((System.ComponentModel.ISupportInitialize)(this._image_pb)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private Windows.Forms.PictureBox _image_pb;
      private Windows.Forms.ToolTip BallonTip_Tip;

   }
}
