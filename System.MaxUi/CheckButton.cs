using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace System.MaxUi
{
    /// <summary>
    /// A checkbutton which looks like a 3dsmax checkbutton. Inherits from Windows.System.Forms.CheckBox
    /// </summary>
    public class CheckButton : CheckBox, IMaxControl
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

        private bool showFocusFrame;
        /// <summary>
        /// When true, a dotted frame is show around the button when the button has keyboard focus. Default value: true.
        /// </summary>
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

        public CheckButton()
        {
            TextAlign = ContentAlignment.MiddleCenter;
            Appearance = Appearance.Button;
            showFocusFrame = true;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.UseVisualStyleBackColor = false;
            MaxConnection.Instance.RegisterControl(this);
        }

        /// <summary>
        /// Updates colors of the control based on colors from <see cref="MaxConnection"/>. Do not call it, it is called internally by the system.
        /// </summary>
        public void UpdateColors()
        {
            ForeColor = MaxConnection.Instance.ForeColor;

            Color bg = (Checked || (mouseOver && mouseDown)) ? MaxConnection.Instance.CheckedColor : MaxConnection.Instance.BackColor;
            BackColor = bg;
            FlatAppearance.CheckedBackColor = bg;
            FlatAppearance.MouseDownBackColor = bg;
            FlatAppearance.BorderColor = bg;
            FlatAppearance.MouseOverBackColor = bg;
            FlatAppearance.BorderSize = 0;
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            UpdateColors();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            mouseDown = true;
            UpdateColors();
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            mouseDown = false;
            mouseWasDown = false;
            UpdateColors();
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
                    UpdateColors();
                    Invalidate();
                }
                if (!mouseOver)
                {
                    mouseOver = true;
                    UpdateColors();
                    Invalidate();
                }
            }
            else
            {
                if (mouseDown)
                {
                    mouseDown = false;
                    mouseWasDown = true;
                    UpdateColors();
                    Invalidate();
                }
                if (mouseOver)
                {
                    mouseOver = false;
                    UpdateColors();
                    Invalidate();
                }
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mouseOver = true;
            UpdateColors();
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseOver = false;
            UpdateColors();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Checked || (mouseDown && mouseOver))
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
            if (mouseDown || Checked)
            {
                Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(MaxConnection.Instance.BackPen, r);
                r.Inflate(-1, -1);
                e.Graphics.DrawRectangle(MaxConnection.Instance.BackPen, r);
            }
            if (!FrameOnMouseOverOnly || mouseOver || Checked)
            {
                MaxConnection.Instance.DrawFrame(e.Graphics, ClientRectangle, mouseDown || Checked);
            }
            if (showFocusFrame && Focused)
            {
                Rectangle r = new Rectangle(3, 3, Width - 7, Height - 7);
                e.Graphics.DrawRectangle(MaxConnection.Instance.FocusPen, r);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                MaxConnection.Instance.UnregisterControl(this);
            }
        }

        protected override bool ShowFocusCues
        {
            get { return false; }
        }
    }
}
