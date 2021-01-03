using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.MaxUi
{
   /// <summary>
   /// A button that looks like a 3dsmax button.
   /// </summary>
   public class Button : System.Windows.Forms.Button, IMaxControl
   {

      private bool frameOnMouseOverOnly;
      /// <summary>
      /// When true, A frame is only shown around the button when the mouse is over it. Default value: false.
      /// </summary>
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
      private bool showFocusFrame;
      public bool ShowFocusFrame
      {
         get { return showFocusFrame; }
         set
         {
            showFocusFrame = value;
            Invalidate();
         }
      }

      private bool mouseOver;
      private bool mouseDown;
      private bool mouseWasDown;

      public Button()
      {
         this.FlatStyle = FlatStyle.Flat;
         this.UseVisualStyleBackColor = false;
         showFocusFrame = true;
         MaxConnection.Instance.RegisterControl(this);
      }

      /// <summary>
      /// Updates colors of the control based on colors from <see cref="MaxConnection"/>. Do not call it, it is called internally by the system.
      /// </summary>
      public void UpdateColors()
      {
         BackColor = MaxConnection.Instance.BackColor;
         ForeColor = MaxConnection.Instance.ForeColor;
         FlatAppearance.CheckedBackColor = MaxConnection.Instance.CheckedColor;
         FlatAppearance.MouseOverBackColor = MaxConnection.Instance.BackColor;
         FlatAppearance.MouseDownBackColor = MaxConnection.Instance.CheckedColor;
         FlatAppearance.BorderColor = MaxConnection.Instance.BackColor;
         FlatAppearance.BorderSize = 0;
      }

      protected override void OnMouseDown(MouseEventArgs mevent)
      {
         base.OnMouseDown(mevent);
         mouseDown = true;
         Invalidate();
      }

      protected override void OnMouseUp(MouseEventArgs mevent)
      {
         base.OnMouseUp(mevent);
         mouseDown = false;
         mouseWasDown = false;
         Invalidate();
      }

      protected override void OnPaint(PaintEventArgs e)
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
            MaxConnection.Instance.DrawFrame(e.Graphics, ClientRectangle, mouseDown);
         }
         if (mouseDown)
         {
            Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);
            e.Graphics.DrawRectangle(MaxConnection.Instance.BackPen, r);
         }
         if (showFocusFrame && Focused)
         {
            Rectangle r = new Rectangle(3, 3, Width - 7, Height - 7);
            e.Graphics.DrawRectangle(MaxConnection.Instance.FocusPen, r);
         }
      }

      protected override void OnMouseEnter(EventArgs e)
      {
         base.OnMouseEnter(e);
         mouseOver = true;
         Invalidate();
      }

      protected override void OnMouseMove(MouseEventArgs mevent)
      {
         base.OnMouseMove(mevent);
         if (Bounds.Contains(mevent.Location))
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

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (disposing)
         {
            MaxConnection.Instance.UnregisterControl(this);
         }
      }
   }
}
