using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace System.MaxUi
{
   [Description("Color Button Control")]
   [ToolboxBitmap(typeof(Button))]
   [Designer(typeof(RoundedButtonDesigner))]
   [DefaultEvent("Click")]
   public partial class RoundedButton : System.Windows.Forms.UserControl
   {
      private enum _States
      {
         Normal,
         MouseOver,
         Clicked
      }

      public enum SmoothingQualities
      {
         None,
         HighSpeed,
         AntiAlias,
         HighQuality
      }

      public enum ButtonStyles
      {
         Rectangle,
         Ellipse
      }

      public enum GradientStyles
      {
         Horizontal,
         Vertical,
         ForwardDiagonal,
         BackwardDiagonal
      }

      // default values
      private bool _Active = true;
      private _States _State = _States.Normal;

      private GradientStyles _GradientStyle = GradientStyles.Vertical;
      private ButtonStyles _ButtonStyle = ButtonStyles.Rectangle;
      private SmoothingQualities _SmoothingQuality = SmoothingQualities.AntiAlias;

      private Color _NormalBorderColor = Color.Black;
      private Color _NormalColorA = Color.White;
      private Color _NormalColorB = Color.White;

      private Color _HoverBorderColor = Color.Gold;
      private Color _HoverColorA = Color.LightGray;
      private Color _HoverColorB = Color.LightGray;

      private string _Tooltip;

      public RoundedButton()
      {
         // initiate button size and font
         base.Size = new Size(200, 40);
         base.Font = new Font("Verdana", 10, FontStyle.Bold);
         SetStyle(
            ControlStyles.AllPaintingInWmPaint |
            ControlStyles.ResizeRedraw |
            ControlStyles.DoubleBuffer |
            ControlStyles.Selectable |
            ControlStyles.UserMouse,
            true
            );

         InitializeComponent();
      }

      [Description("Image"), Category("ColorButton")]
      public Image ImageProfile 
      { 
         get { return _image_pb.Image; } 
         set 
         { 
            _image_pb.Image = value;
            this.Invalidate(); 
         } 
      }
      
      [Description("Image Size Mode"), Category("ColorButton")]
      public PictureBoxSizeMode ImageSizeMode { get { return _image_pb.SizeMode; } set { _image_pb.SizeMode = value; this.Invalidate(); } }

      [Description("Smoothing Quality"), Category("ColorButton")]
      public SmoothingQualities SmoothingQuality
      {
         get
         {
            return _SmoothingQuality;
         }
         set
         {
            _SmoothingQuality = value;
            this.Invalidate();
         }
      }

      [Description("Gradient Style"), Category("ColorButton")]
      public GradientStyles GradientStyle
      {
         get
         {
            return _GradientStyle;
         }
         set
         {
            _GradientStyle = value;
            this.Invalidate();
         }

      }

      [Description("Button Style"), Category("ColorButton")]
      public ButtonStyles ButtonStyle
      {
         get
         {
            return _ButtonStyle;
         }
         set
         {
            _ButtonStyle = value;
            this.Invalidate();
         }
      }

      [Description("The normal border color"), Category("ColorButton")]
      public Color NormalBorderColor
      {
         get
         {
            return _NormalBorderColor;
         }
         set
         {
            _NormalBorderColor = value;
            this.Invalidate();
         }

      }

      [Description("The hover border color"), Category("ColorButton")]
      public Color HoverBorderColor
      {
         get
         {
            return _HoverBorderColor;
         }
         set
         {
            _HoverBorderColor = value;
            this.Invalidate();
         }

      }

      [Description("Normal color A"), Category("ColorButton")]
      public Color NormalColorA
      {
         get
         {
            return _NormalColorA;
         }
         set
         {
            _NormalColorA = value;
            this.Invalidate();
         }
      }

      [Description("Normal color B"), Category("ColorButton")]
      public Color NormalColorB
      {
         get
         {
            return _NormalColorB;
         }
         set
         {
            _NormalColorB = value;
            this.Invalidate();
         }
      }

      [Description("Hover color A"), Category("ColorButton")]
      public Color HoverColorA
      {
         get
         {
            return _HoverColorA;
         }
         set
         {
            _HoverColorA = value;
            this.Invalidate();
         }
      }

      [Description("Hover color B"), Category("ColorButton")]
      public Color HoverColorB
      {
         get
         {
            return _HoverColorB;
         }
         set
         {
            _HoverColorB = value;
            this.Invalidate();
         }
      }

      [Description("Enable the button?"), Category("ColorButton")]
      public bool Active
      {
         get
         {
            return _Active;
         }
         set
         {
            _Active = value;            
            this.Invalidate();
         }
      }

      [Description("Show Image?"), Category("ColorButton")]
      public bool ImageVisiable
      {
         get
         {
            return _image_pb.Visible;
         }
         set
         {
            _image_pb.Visible = value;
            this.Invalidate();
         }
      }

      // to make sure the control is invalidated(repainted) when the text is changed
      [Description("Caption?"), Category("ColorButton")]
      public string Caption
      {
         get
         {
            return base.Text;
         }
         set
         {
            base.Text = value;
            this.Invalidate();
         }
      }

      [Description("ToolTip?"), Category("ColorButton")]
      public string Tooltip
      {
         get
         {
            return _Tooltip;
         }
         set
         {
            _Tooltip = value;
         }
      }

      protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
      {
         LinearGradientBrush brush;
         LinearGradientMode mode;

         //
         // set SmoothingMode
         //
         switch (_SmoothingQuality)
         {
            case SmoothingQualities.None:
               e.Graphics.SmoothingMode = SmoothingMode.Default;
               break;
            case SmoothingQualities.HighSpeed:
               e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
               break;
            case SmoothingQualities.AntiAlias:
               e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
               break;
            case SmoothingQualities.HighQuality:
               e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
               break;
         }

         //
         // set LinearGradientMode
         //
         switch (_GradientStyle)
         {
            case GradientStyles.Horizontal:
               mode = LinearGradientMode.Horizontal;
               break;
            case GradientStyles.Vertical:
               mode = LinearGradientMode.Vertical;
               break;
            case GradientStyles.ForwardDiagonal:
               mode = LinearGradientMode.ForwardDiagonal;
               break;
            case GradientStyles.BackwardDiagonal:
               mode = LinearGradientMode.BackwardDiagonal;
               break;
            default:
               mode = LinearGradientMode.Vertical;
               break;
         }

         SizeF textSize = e.Graphics.MeasureString(this.Text, base.Font);
         int textX = (int)(base.Size.Width / 2) - (int)(textSize.Width / 2);
         int textY = (int)(base.Size.Height / 2) - (int)(textSize.Height / 2);
         Rectangle newRect = new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1,
            ClientRectangle.Width - 3, ClientRectangle.Height - 3);

         if (_Active)
         {
            switch (_State)
            {
               case _States.Normal:
                  brush = new LinearGradientBrush(newRect, _NormalColorA, _NormalColorB, mode);
                  switch (_ButtonStyle)
                  {
                     case ButtonStyles.Rectangle:
                        e.Graphics.FillRectangle(brush, newRect);
                        e.Graphics.DrawRectangle(new Pen(_NormalBorderColor, 1), newRect);
                        break;
                     case ButtonStyles.Ellipse:
                        e.Graphics.FillEllipse(brush, newRect);
                        e.Graphics.DrawEllipse(new Pen(_NormalBorderColor, 1), newRect);
                        break;

                  }
                  e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX, textY);
                  break;

               case _States.MouseOver:
                  brush = new LinearGradientBrush(newRect, _HoverColorA, _HoverColorB, mode);
                  switch (_ButtonStyle)
                  {
                     case ButtonStyles.Rectangle:
                        e.Graphics.FillRectangle(brush, newRect);
                        e.Graphics.DrawRectangle(new Pen(_HoverBorderColor, 1), newRect);
                        break;
                     case ButtonStyles.Ellipse:
                        e.Graphics.FillEllipse(brush, newRect);
                        e.Graphics.DrawEllipse(new Pen(_HoverBorderColor, 1), newRect);
                        break;

                  }
                  e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX, textY);
                  break;

               case _States.Clicked:
                  brush = new LinearGradientBrush(newRect, _HoverColorA, _HoverColorB, mode);
                  switch (_ButtonStyle)
                  {
                     case ButtonStyles.Rectangle:
                        e.Graphics.FillRectangle(brush, newRect);
                        e.Graphics.DrawRectangle(new Pen(_HoverBorderColor, 2), newRect);
                        break;
                     case ButtonStyles.Ellipse:
                        e.Graphics.FillEllipse(brush, newRect);
                        e.Graphics.DrawEllipse(new Pen(_HoverBorderColor, 2), newRect);
                        break;

                  }
                  e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(base.ForeColor), textX + 1, textY + 1);
                  break;
            }
         }
         else
         {
            brush = new LinearGradientBrush(newRect, _NormalColorA, _NormalColorB, mode);
            switch (_ButtonStyle)
            {
               case ButtonStyles.Rectangle:
                  e.Graphics.FillRectangle(brush, newRect);
                  e.Graphics.DrawRectangle(new Pen(_NormalBorderColor, 1), newRect);
                  break;
               case ButtonStyles.Ellipse:
                  e.Graphics.FillEllipse(brush, newRect);
                  e.Graphics.DrawEllipse(new Pen(_NormalBorderColor, 1), newRect);
                  break;

            }
            e.Graphics.DrawString(this.Text, base.Font, new SolidBrush(_NormalColorA), textX, textY);
         }
         // Rounded Image
         if(_image_pb.Image != null)
         {
            _image_pb.Left = ClientRectangle.X ;
            _image_pb.Top = ClientRectangle.Y ;
            _image_pb.Width = ClientRectangle.Width;
            _image_pb.Height = ClientRectangle.Height;
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(ClientRectangle.X + 4, ClientRectangle.Y + 4, ClientRectangle.Width - 8, ClientRectangle.Height - 8);
            Region rg = new Region(gp);
            _image_pb.Region = rg;
         }
      }

      protected override void OnMouseLeave(System.EventArgs e)
      {
         if (_Active)
         {
            _State = _States.Normal;
            //BallonTip_Tip.Hide(this);
            this.Invalidate();
            base.OnMouseLeave(e);
         }
      }

      protected override void OnMouseEnter(System.EventArgs e)
      {
         if (_Active)
         {
            _State = _States.MouseOver;
            BallonTip_Tip.Show(this.Tooltip, this);
            this.Invalidate();
            base.OnMouseEnter(e);
         }
      }

      protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
      {
         if (_Active)
         {
            _State = _States.MouseOver;
            this.Invalidate();
            base.OnMouseUp(e);
         }
      }

      protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
      {
         if (_Active)
         {
            _State = _States.Clicked;
            this.Invalidate();
            base.OnMouseDown(e);
         }
      }

      protected override void OnClick(System.EventArgs e)
      {
         // prevent click when button is inactive
         if (_Active)
         {
            if (_State == _States.Clicked)
            {
               base.OnClick(e);
            }
         }
      }

      private void _image_pb_Click(object sender, EventArgs e)
      {
         // prevent click when button is inactive
         if (_Active)
         {
            if (_State == _States.Clicked)
            {
               base.OnClick(e);
            }
         }
      }

      private void _image_pb_MouseDown(object sender, MouseEventArgs e)
      {
         if (_Active)
         {
            _State = _States.Clicked;
            this.Invalidate();
            base.OnMouseDown(e);
         }
      }

      private void _image_pb_MouseUp(object sender, MouseEventArgs e)
      {
         if (_Active)
         {
            _State = _States.MouseOver;
            this.Invalidate();
            base.OnMouseUp(e);
         }
      }

      private void _image_pb_MouseEnter(object sender, EventArgs e)
      {
         if (_Active)
         {
            _State = _States.MouseOver;
            BallonTip_Tip.SetToolTip(_image_pb, Tooltip);
            this.Invalidate();
            base.OnMouseEnter(e);
         }
      }

      private void _image_pb_MouseLeave(object sender, EventArgs e)
      {
         if (_Active)
         {
            _State = _States.Normal;
            BallonTip_Tip.SetToolTip(_image_pb, Tooltip);
            this.Invalidate();
            base.OnMouseLeave(e);
         }
      }
   }
}
