using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace System.MaxUi
{
   /// <summary>A special custom rounding GroupBox with many painting features.</summary>
   //[ToolboxBitmap(typeof(Grouper), "mmMaxControls.Grouper.bmp")]
   [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
   public class Rollout : UserControl
   {
      #region Enumerations

      /// <summary>A special gradient enumeration.</summary>
      public enum GroupBoxGradientMode
      {
         /// <summary>Specifies no gradient mode.</summary>
         None = 4,

         /// <summary>Specifies a gradient from upper right to lower left.</summary>
         BackwardDiagonal = 3,

         /// <summary>Specifies a gradient from upper left to lower right.</summary>
         ForwardDiagonal = 2,

         /// <summary>Specifies a gradient from left to right.</summary>
         Horizontal = 0,

         /// <summary>Specifies a gradient from top to bottom.</summary>
         Vertical = 1
      }


      #endregion

      #region Variables

      private System.ComponentModel.Container components = null;
      private int V_RoundCorners = 2;
      private string V_RolloutTitle = "The Rollout name";
      private System.Drawing.Color V_TitleBorderColor = Color.Gray;
      private System.Drawing.Color V_RolloutBorderColor = Color.Gray;
      private float V_BorderThickness = 1;
      private bool V_ShadowControl = false;
      private System.Drawing.Color V_BackgroundColor = SystemColors.Control;
      private System.Drawing.Color V_BackgroundGradientColor = SystemColors.Control;
      private GroupBoxGradientMode V_BackgroundGradientMode = GroupBoxGradientMode.None;
      private System.Drawing.Color V_ShadowColor = Color.DarkGray;
      private int V_ShadowThickness = 3;
      private System.Drawing.Image V_GroupImage = null;
      private System.Drawing.Color V_CustomRolloutColor = Color.White;
      private bool V_PaintRollout = false;
      private System.Drawing.Color V_BackColor = Color.Transparent;
      private bool V_RolloutStatus = true;
      private BorderStyle V_RolloutFrameType = BorderStyle.Fixed3D;
      private int v_MaxHeight;

      private bool mouseOver;
      private bool mouseDown;
      private bool mouseWasDown;
      private bool showFocusFrame;
      private bool frameOnMouseOverOnly;

      #endregion

      #region Properties
      /// <summary>This feature will paint the background color of the control.</summary>
      [Category("Appearance"), Description("This feature will paint the background color of the control.")]
      public bool RolloutStatus { 
         get { return V_RolloutStatus; } 
         set 
         { 
            V_RolloutStatus = value; 
            if(V_RolloutStatus)
            {
               Height = v_MaxHeight;
            }
            else
            {               
               Height = 22;
            }

            this.Refresh(); 
         } 
      }

      /// <summary>This feature will paint the background color of the control.</summary>
      [Category("Appearance"), Description("This feature will paint the background color of the control.")]
      public override System.Drawing.Color BackColor { get { return V_BackColor; } set { V_BackColor = value; this.Refresh(); } }

      /// <summary>This feature will paint the group title background to the specified color if PaintGroupBox is set to true.</summary>
      [Category("Appearance"), Description("This feature will paint the group title background to the specified color if PaintGroupBox is set to true.")]
      public System.Drawing.Color CustomRolloutColor { get { return V_CustomRolloutColor; } set { V_CustomRolloutColor = value; this.Refresh(); } }

      /// <summary>This feature will paint the group title background to the CustomGroupBoxColor.</summary>
      [Category("Appearance"), Description("This feature will paint the group title background to the CustomGroupBoxColor.")]
      public bool PaintRollout { get { return V_PaintRollout; } set { V_PaintRollout = value; this.Refresh(); } }

      /// <summary>This feature can add a 16 x 16 image to the group title bar.</summary>
      [Category("Appearance"), Description("This feature can add a 16 x 16 image to the group title bar.")]
      public System.Drawing.Image GroupImage { get { return V_GroupImage; } set { V_GroupImage = value; this.Refresh(); } }

      /// <summary>This feature will change the control's shadow color.</summary>
      [Category("Appearance"), Description("This feature will change the control's shadow color.")]
      public System.Drawing.Color ShadowColor { get { return V_ShadowColor; } set { V_ShadowColor = value; this.Refresh(); } }

      /// <summary>This feature will change the size of the shadow border.</summary>
      [Category("Appearance"), Description("This feature will change the size of the shadow border.")]
      public int ShadowThickness
      {
         get { return V_ShadowThickness; }
         set
         {
            if (value > 10)
            {
               V_ShadowThickness = 10;
            }
            else
            {
               if (value < 1) { V_ShadowThickness = 1; }
               else { V_ShadowThickness = value; }
            }

            this.Refresh();
         }
      }

      /// <summary>This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.</summary>
      [Category("Appearance"), Description("This feature will change the group control color. This color can also be used in combination with BackgroundGradientColor for a gradient paint.")]
      public System.Drawing.Color BackgroundColor { get { return V_BackgroundColor; } set { V_BackgroundColor = value; this.Refresh(); } }

      /// <summary>This feature can be used in combination with BackgroundColor to create a gradient background.</summary>
      [Category("Appearance"), Description("This feature can be used in combination with BackgroundColor to create a gradient background.")]
      public System.Drawing.Color BackgroundGradientColor { get { return V_BackgroundGradientColor; } set { V_BackgroundGradientColor = value; this.Refresh(); } }

      /// <summary>This feature turns on background gradient painting.</summary>
      [Category("Appearance"), Description("This feature turns on background gradient painting.")]
      public GroupBoxGradientMode BackgroundGradientMode { get { return V_BackgroundGradientMode; } set { V_BackgroundGradientMode = value; this.Refresh(); } }

      /// <summary>This feature will round the corners of the control.</summary>
      [Category("Appearance"), Description("This feature will round the corners of the control.")]
      public int RoundCorners
      {
         get { return V_RoundCorners; }
         set
         {
            if (value > 25)
            {
               V_RoundCorners = 25;
            }
            else
            {
               if (value < 1) { V_RoundCorners = 1; }
               else { V_RoundCorners = value; }
            }

            this.Refresh();
         }
      }

      /// <summary>This feature will add a group title to the control.</summary>
      [Category("Appearance"), Description("This feature will add a group title to the control.")]
      public string RolloutTitle { get { return V_RolloutTitle; } set { V_RolloutTitle = value; this.Refresh(); } }

      /// <summary>This feature will allow you to change the color of the control's border.</summary>
      [Category("Appearance"), Description("This feature will allow you to change the color of the control's border.")]
      public System.Drawing.Color TitleBorderColor { get { return V_TitleBorderColor; } set { V_TitleBorderColor = value; this.Refresh(); } }

      /// <summary>This feature will allow you to change the color of the control's border.</summary>
      [Category("Appearance"), Description("This feature will allow you to change the color of the control's border.")]
      public System.Drawing.Color RolloutBorderColor { get { return V_RolloutBorderColor; } set { V_RolloutBorderColor = value; this.Refresh(); } }

      /// <summary>This feature will allow you to set the control's border size.</summary>
      [Category("Appearance"), Description("This feature will allow you to set the control's border size.")]
      public float BorderThickness
      {
         get { return V_BorderThickness; }
         set
         {
            if (value > 3)
            {
               V_BorderThickness = 3;
            }
            else
            {
               if (value < 1) { V_BorderThickness = 1; }
               else { V_BorderThickness = value; }
            }
            this.Refresh();
         }
      }

      /// <summary>This feature will allow you to turn on control shadowing.</summary>
      [Category("Appearance"), Description("This feature will allow you to turn on control shadowing.")]
      public bool ShadowControl { get { return V_ShadowControl; } set { V_ShadowControl = value; this.Refresh(); } }

      /// <summary>
      /// When true, A frame is only shown around the button when the mouse is over it. Default value: false.
      /// </summary>
      [Category("Appearance"), Description("When true, A frame is only shown around the button when the mouse is over it. Default value: false.")]
      public bool FrameOnMouseOverOnly
      {
         get { return frameOnMouseOverOnly; }
         set
         {
            frameOnMouseOverOnly = value;
            Invalidate();
         }
      }

      /// <summary>
      /// When true, a dotted frame is show around the button when the button has keyboard focus. Default value: true.
      /// </summary>
      [Category("Appearance"), Description("When true, a dotted frame is show around the button when the button has keyboard focus. Default value: true.")]
      public bool ShowFocusFrame
      {
         get { return showFocusFrame; }
         set
         {
            showFocusFrame = value;
            Invalidate();
         }
      }

      /// <summary>
      /// When true, a dotted frame is show around the button when the button has keyboard focus. Default value: true.
      /// </summary>
      [Category("Appearance"), Description("When true, a dotted frame is show around the button when the button has keyboard focus. Default value: true.")]
      public BorderStyle RolloutFrameType
      {
         get { return V_RolloutFrameType; }
         set
         {
            V_RolloutFrameType = value;
            Invalidate();
         }
      }

      #endregion

      #region Constructor
      /// <summary>This method will construct a new GroupBox control.</summary>
      public Rollout()
      {
         InitializeStyles();
         InitializeRollout();
      }
      #endregion

      #region DeConstructor

      /// <summary>This method will dispose of the GroupBox control.</summary>
      protected override void Dispose(bool disposing)
      {
         if (disposing) { if (components != null) { components.Dispose(); } }
         base.Dispose(disposing);
      }


      #endregion

      #region Initialization
      /// <summary>This method will initialize the controls custom styles.</summary>
      private void InitializeStyles()
      {
         //Set the control styles----------------------------------
         this.SetStyle(ControlStyles.DoubleBuffer, true);
         this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
         this.SetStyle(ControlStyles.UserPaint, true);
         this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
         //--------------------------------------------------------
      }

      /// <summary>This method will initialize the GroupBox control.</summary>
      private void InitializeRollout()
      {
         components = new System.ComponentModel.Container();
         this.Resize += new EventHandler(Rollout_Resize);
         this.DockPadding.All = 20;
         this.Name = "Rollout";
         this.Size = new System.Drawing.Size(368, 288);
         this.Padding = new Windows.Forms.Padding(4,30,3,3);
         v_MaxHeight = Height;
      }
      #endregion

      #region Protected Methods

      protected override void OnMouseDown(MouseEventArgs e)
      {
         base.OnMouseDown(e);
         if ((e.X >= 4 && e.X <= (Width - 6)) && (e.Y >= 0 && e.Y <= 21))
         {
            mouseDown = true;
            Invalidate();
         }
      }

      protected override void OnMouseUp(MouseEventArgs e)
      {
         base.OnMouseUp(e);
         if ((e.X >= 4 && e.X <= (Width - 6)) && (e.Y >= 0 && e.Y <= 21))
         {
            mouseDown = false;
            mouseWasDown = false;
            Invalidate();

            RolloutStatus = !RolloutStatus;
         }
      }

      protected override void OnMouseEnter(EventArgs e)
      {
         base.OnMouseEnter(e);
         mouseOver = true;
         Invalidate();
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         base.OnMouseMove(e);
         //if (Bounds.Contains(e.Location))
         if ((e.X >= 4 && e.X <= (Width - 6)) && (e.Y >= 0 && e.Y <= 21))
         {
            if (mouseWasDown)
            {
               mouseDown = true;
               mouseWasDown = false;
               Invalidate();
            }
            if (!mouseOver)
            {
               mouseOver = true;
               Invalidate();
            }
         }
         else
         {
            if (mouseDown)
            {
               mouseDown = false;
               mouseWasDown = true;
               Invalidate();
            }
            if (mouseOver)
            {
               mouseOver = false;
               Invalidate();
            }
         }
      }

      protected override void OnMouseLeave(EventArgs e)
      {
         base.OnMouseLeave(e);
         mouseOver = false;
         Invalidate();
      }

      protected override bool ShowFocusCues
      {
         get { return false; }
      }

      /// <summary>Overrides the OnPaint method to paint control.</summary>
      /// <param name="e">The paint event arguments.</param>
      protected override void OnPaint(PaintEventArgs e)
      {         
         PaintBack(e.Graphics);
         PaintRolloutText(e.Graphics);
         PaintRolloutButton(e);
      }

      #endregion

      #region Private Methods

      /// <summary>This method will paint the group title.</summary>
      /// <param name="g">The paint event graphics object.</param>
      private void PaintRolloutText(System.Drawing.Graphics g)
      {
         //Check if string has something-------------
         if (this.RolloutTitle == string.Empty) { return; }
         //------------------------------------------

         //Set Graphics smoothing mode to Anit-Alias-- 
         g.SmoothingMode = SmoothingMode.AntiAlias;
         //-------------------------------------------

         //Declare Variables------------------
         SizeF StringSize = g.MeasureString(this.RolloutTitle, this.Font);
         Size StringSize2 = StringSize.ToSize();
         if (this.GroupImage != null) { StringSize2.Width += 18; }
         int ArcWidth = this.RoundCorners;
         int ArcHeight = this.RoundCorners;
         int ArcX1 = 5;
         int ArcX2 = this.Width - 7;//(StringSize2.Width + 34) - (ArcWidth + 1);
         int ArcY1 = 0;
         int ArcY2 = 22 - (ArcHeight + 1);
         System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
         System.Drawing.Brush BorderBrush = new SolidBrush(this.TitleBorderColor);
         System.Drawing.Pen BorderPen = new Pen(BorderBrush, this.BorderThickness);
         System.Drawing.Drawing2D.LinearGradientBrush BackgroundGradientBrush = null;
         System.Drawing.Brush BackgroundBrush = (this.PaintRollout) ? new SolidBrush(this.CustomRolloutColor) : new SolidBrush(this.BackgroundColor);
         System.Drawing.SolidBrush TextColorBrush = new SolidBrush(this.ForeColor);
         System.Drawing.SolidBrush ShadowBrush = null;
         System.Drawing.Drawing2D.GraphicsPath ShadowPath = null;
         //-----------------------------------

         //Check if shadow is needed----------
         if (this.ShadowControl)
         {
            ShadowBrush = new SolidBrush(this.ShadowColor);
            ShadowPath = new System.Drawing.Drawing2D.GraphicsPath();
            ShadowPath.AddArc(ArcX1 + (this.ShadowThickness - 1), ArcY1 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
            ShadowPath.AddArc(ArcX2 + (this.ShadowThickness - 1), ArcY1 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
            ShadowPath.AddArc(ArcX2 + (this.ShadowThickness - 1), ArcY2 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
            ShadowPath.AddArc(ArcX1 + (this.ShadowThickness - 1), ArcY2 + (this.ShadowThickness - 1), ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
            ShadowPath.CloseAllFigures();

            //Paint Rounded Rectangle------------
            g.FillPath(ShadowBrush, ShadowPath);
            //-----------------------------------
         }
         //-----------------------------------

         //Create Rounded Rectangle Path------
         path.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
         path.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
         path.AddArc(ArcX2, ArcY2, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
         path.AddArc(ArcX1, ArcY2, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left

         path.CloseAllFigures();
         //-----------------------------------

         //Check if Gradient Mode is enabled--
         if (this.PaintRollout)
         {
            //Paint Rounded Rectangle------------
            g.FillPath(BackgroundBrush, path);
            //-----------------------------------
         }
         else
         {
            if (this.BackgroundGradientMode == GroupBoxGradientMode.None)
            {
               //Paint Rounded Rectangle------------
               g.FillPath(BackgroundBrush, path);
               //-----------------------------------
            }
            else
            {
               BackgroundGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), this.BackgroundColor, this.BackgroundGradientColor, (LinearGradientMode)this.BackgroundGradientMode);

               //Paint Rounded Rectangle------------
               g.FillPath(BackgroundGradientBrush, path);
               //-----------------------------------
            }
         }
         //-----------------------------------

         //Paint Borded-----------------------
         g.DrawPath(BorderPen, path);
         //-----------------------------------

         //Paint Text-------------------------         
         g.DrawString(this.RolloutTitle, this.Font, TextColorBrush, (Width - StringSize2.Width) / 2, 4);

         var font = new Font("Microsoft Sans Serif", 11, FontStyle.Bold);         
         if(V_RolloutStatus)
            g.DrawString("-", font, TextColorBrush, 10, 2);
         else
            g.DrawString("+", font, TextColorBrush, 10, 2);
         //-----------------------------------

         //Draw GroupImage if there is one----
         if (this.GroupImage != null)
         {
            g.DrawImage(this.GroupImage, 28, 4, 16, 16);
         }
         //-----------------------------------

         //Destroy Graphic Objects------------
         if (path != null) { path.Dispose(); }
         if (BorderBrush != null) { BorderBrush.Dispose(); }
         if (BorderPen != null) { BorderPen.Dispose(); }
         if (BackgroundGradientBrush != null) { BackgroundGradientBrush.Dispose(); }
         if (BackgroundBrush != null) { BackgroundBrush.Dispose(); }
         if (TextColorBrush != null) { TextColorBrush.Dispose(); }
         if (ShadowBrush != null) { ShadowBrush.Dispose(); }
         if (ShadowPath != null) { ShadowPath.Dispose(); }
         //-----------------------------------
      }

      /// <summary>This method will paint the control.</summary>
      /// <param name="g">The paint event graphics object.</param>
      private void PaintBack(System.Drawing.Graphics g)
      {
         if (RolloutStatus)
            v_MaxHeight = Height;

         //Set Graphics smoothing mode to Anit-Alias-- 
         g.SmoothingMode = SmoothingMode.AntiAlias;
         //-------------------------------------------

         //Declare Variables------------------
         int ArcWidth = this.RoundCorners * 2;
         int ArcHeight = this.RoundCorners * 2;
         int ArcX1 = 0;
         int ArcX2 = (this.ShadowControl) ? (this.Width - (ArcWidth + 1)) - this.ShadowThickness : this.Width - (ArcWidth + 1);
         int ArcY1;
         int ArcY2;
         
         if (RolloutStatus)
         {
            ArcY1 = 10;
            ArcY2 = (this.ShadowControl) ? (this.Height - (ArcHeight + 1)) - this.ShadowThickness : this.Height - (ArcHeight + 1);
         }
         else
         {
            ArcY1 = 4;
            ArcY2 = 14;
         }
         
         System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
         System.Drawing.Brush BorderBrush = new SolidBrush(this.RolloutBorderColor);
         System.Drawing.Pen BorderPen = new Pen(BorderBrush, this.BorderThickness);
         System.Drawing.Drawing2D.LinearGradientBrush BackgroundGradientBrush = null;
         System.Drawing.Brush BackgroundBrush = new SolidBrush(this.BackgroundColor);
         System.Drawing.SolidBrush ShadowBrush = null;
         System.Drawing.Drawing2D.GraphicsPath ShadowPath = null;
         //-----------------------------------

         //Check if shadow is needed----------
         if (this.ShadowControl)
         {
            ShadowBrush = new SolidBrush(this.ShadowColor);
            ShadowPath = new System.Drawing.Drawing2D.GraphicsPath();
            ShadowPath.AddArc(ArcX1 + this.ShadowThickness, ArcY1 + this.ShadowThickness, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
            ShadowPath.AddArc(ArcX2 + this.ShadowThickness, ArcY1 + this.ShadowThickness, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
            ShadowPath.AddArc(ArcX2 + this.ShadowThickness, ArcY2 + this.ShadowThickness, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
            ShadowPath.AddArc(ArcX1 + this.ShadowThickness, ArcY2 + this.ShadowThickness, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
            ShadowPath.CloseAllFigures();

            //Paint Rounded Rectangle------------
            g.FillPath(ShadowBrush, ShadowPath);
            //-----------------------------------
         }
         //-----------------------------------

         //Create Rounded Rectangle Path------
         path.AddArc(ArcX1, ArcY1, ArcWidth, ArcHeight, 180, GroupBoxConstants.SweepAngle); // Top Left
         path.AddArc(ArcX2, ArcY1, ArcWidth, ArcHeight, 270, GroupBoxConstants.SweepAngle); //Top Right
         path.AddArc(ArcX2, ArcY2, ArcWidth, ArcHeight, 360, GroupBoxConstants.SweepAngle); //Bottom Right
         path.AddArc(ArcX1, ArcY2, ArcWidth, ArcHeight, 90, GroupBoxConstants.SweepAngle); //Bottom Left
         path.CloseAllFigures();
         //-----------------------------------

         //Check if Gradient Mode is enabled--
         if (this.BackgroundGradientMode == GroupBoxGradientMode.None)
         {
            //Paint Rounded Rectangle------------
            g.FillPath(BackgroundBrush, path);
            //-----------------------------------
         }
         else
         {
            BackgroundGradientBrush = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), this.BackgroundColor, this.BackgroundGradientColor, (LinearGradientMode)this.BackgroundGradientMode);

            //Paint Rounded Rectangle------------
            g.FillPath(BackgroundGradientBrush, path);
            //-----------------------------------
         }
         //-----------------------------------

         //Paint Borded-----------------------
         g.DrawPath(BorderPen, path);
         //-----------------------------------

         //Destroy Graphic Objects------------
         if (path != null) { path.Dispose(); }
         if (BorderBrush != null) { BorderBrush.Dispose(); }
         if (BorderPen != null) { BorderPen.Dispose(); }
         if (BackgroundGradientBrush != null) { BackgroundGradientBrush.Dispose(); }
         if (BackgroundBrush != null) { BackgroundBrush.Dispose(); }
         if (ShadowBrush != null) { ShadowBrush.Dispose(); }
         if (ShadowPath != null) { ShadowPath.Dispose(); }
         //-----------------------------------
      }

      /// <summary>This method fires when the GroupBox resize event occurs.</summary>
      /// <param name="sender">The object the sent the event.</param>
      /// <param name="e">The event arguments.</param>
      private void Rollout_Resize(object sender, EventArgs e)
      {
         this.Refresh();
      }

      /// <summary>This method will paint the rollout button.</summary>
      /// <param name="g">The paint event graphics object.</param>
      private void PaintRolloutButton(PaintEventArgs e)
      {
         if (V_RolloutFrameType == BorderStyle.Fixed3D)
         {
            if (mouseDown)
            {
               using (Matrix m = e.Graphics.Transform)
               {
                  m.Translate(1, 1);
                  e.Graphics.Transform = m;
                  base.OnPaint(e);
                  e.Graphics.ResetTransform();
               }
            }
            else
            {
               base.OnPaint(e);
            }
            if (!FrameOnMouseOverOnly || mouseOver)
            {
               MaxConnection.Instance.DrawFrame(e.Graphics, new Rectangle(5, 0, Width - 10, 22), mouseDown);
            }
            if (mouseDown)
            {
               Rectangle r = new Rectangle(5, 0, Width - 10, 22);
               e.Graphics.DrawRectangle(MaxConnection.Instance.BackPen, r);
            }
         }
      }

      #endregion
   }
}
