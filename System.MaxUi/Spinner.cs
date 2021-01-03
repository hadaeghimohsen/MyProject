using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace System.MaxUi
{
   /// <summary>
   /// A .NET replica of the 3dsmax spinner control.
   /// </summary>
   /// <remarks>
   /// The 3dsmax native %Spinner always resets to 0.0 (or the closest value in range) when right-clicked, even if the default value is not 0.0. This Spinner resets to the default value.
   /// <para><b>When used inside a MAXScript rollout, you must manually specify the <i>Height</i> parameter to be 16 pixels high, 
   /// as MAXScript defaults all <i>DotNetControl</i> at 14 pixels high.</b></para>
   /// <para>Unlike the native 3dsmax spinner, it can not be linked to an animation controller.</para>
   /// <para>The internal value is stored as a System.Decimal. MAXScript erroneously converts a Decimal value to a MAXScript integer, instead of a float.
   /// To work around this limitation, use the <see cref="FloatValue"/> property.</para>
   /// </remarks>
   public class Spinner : Panel, IMaxControl
   {
      /// <summary>
      /// Raised after the value of the Spinner changes.
      /// </summary>
      public event EventHandler ValueChanged;

      /// <summary>
      /// Raised when the arrows have been pressed using the mouse.
      /// </summary>
      public event EventHandler<EventArgs> ButtonDown;

      /// <summary>
      /// Raised when the control leaves spin mode.
      /// <para>The SpinCancelled property of the SpinnerButtonEventArgs returns true if the spin operation was cancelled using the right mouse button.</para>
      /// </summary>
      public event EventHandler<SpinnerButtonEventArgs> ButtonUp;

      /// <summary>
      /// Updates colors of the control based on colors from <see cref="MaxConnection"/>. Do not call it, it is called internally by the system.
      /// </summary>
      public void UpdateColors()
      {
         this.BackColor = MaxConnection.Instance.BackColor;
         this.ForeColor = MaxConnection.Instance.ForeColor;
         tbValue.UpdateColors();
         Invalidate(true);
      }

      private SpinnerTextBox tbValue;
      private Rectangle tbBounds;
      private Rectangle btnUpBounds;
      private Rectangle btnDownBounds;

      private Timer valueTimer;
      private int timerTicks;

      private bool btnUpClicked;
      private bool btnDnClicked;
      private decimal valueAtButtonDown;
      private bool spinCancelled;

      private bool dragging;
      private Point dragPoint;
      private decimal value;
      private decimal dragValue;
      private decimal defaultValue;
      private decimal minimum;
      private decimal maximum;
      private decimal increment;
      private int cycleValue;
      private int screenTop;
      private int screenBottom;
      private bool wrappedMouse;

      /// <summary>
      /// The value of the Spinner. See remarks for working with float values.
      /// </summary>
      /// <remarks>
      /// The internal value is stored as a System.Decimal. MAXScript erroneously converts a Decimal value to a MAXScript integer, instead of a float.
      /// To work around this limitation, use the <see cref="FloatValue"/> property.
      /// </remarks>
      public decimal Value
      {
         get { return value; }
         set
         {
            decimal limitedValue = Math.Min(Math.Max(value, Minimum), Maximum);
            tbValue.Value = limitedValue;
            if (limitedValue != this.value)
            {
               this.value = limitedValue;
               if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
            }
         }
      }

      /// <summary>
      /// Get/set the number of decimal digits to display.
      /// </summary>
      public int DecimalPlaces
      {
         get { return tbValue.DecimalLength; }
         set { tbValue.DecimalLength = value; }
      }

      /// <summary>
      /// Get/set the minimum permitted value.
      /// </summary>
      public decimal Minimum
      {
         get { return minimum; }
         set { minimum = value; }
      }

      /// <summary>
      /// Get/set the maximum permitted value.
      /// </summary>
      public decimal Maximum
      {
         get { return maximum; }
         set { maximum = value; }
      }

      /// <summary>
      /// Get/set the increment value.
      /// </summary>
      public decimal Increment
      {
         get { return increment; }
         set { increment = value; }
      }

      /// <summary>
      /// Get/set the value of the Spinner as a System.Single value.
      /// </summary>
      public float FloatValue
      {
         get { return (float)Value; }
         set { Value = (decimal)value; }
      }

      /// <summary>
      /// Get/set the value of the Spinner as a System.Int32 value.
      /// </summary>
      public int IntValue
      {
         get { return (int)Value; }
         set { Value = (decimal)value; }
      }

      /// <summary>
      /// Get/set the default value of the Spinner.
      /// </summary>
      /// <remarks>
      /// This is the value that is set when the Spinner is first created, and unlike the native 3dsmax Spinner, it is also the value that the Spinner is reset to using a right-click.
      /// </remarks>
      public decimal DefaultValue
      {
         get { return defaultValue; }
         set { defaultValue = value; }
      }

      public Spinner()
         : this(0m, 100m, .1m, 0m)
      {
      }

      protected override Size DefaultSize
      {
         get
         {
            return new Size(60, 16);
         }
      }

      protected override Size DefaultMinimumSize
      {
         get
         {
            return new Size(Width, 16);
         }
      }

      protected override Size DefaultMaximumSize
      {
         get
         {
            return new Size(int.MaxValue, 16);
         }
      }

      private bool Dragging
      {
         get { return dragging; }
         set
         {
            dragging = value;
            if (value)
            {
               spinCancelled = false;
            }
            else
            {
               valueTimer.Stop();
            }
            Invalidate(btnUpBounds);
            Invalidate(btnDownBounds);
         }
      }

      public Spinner(decimal min, decimal max, decimal step, decimal defaultValue)
      {
         this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

         this.tbValue = new SpinnerTextBox();
         this.tbValue.ValueChanged += new EventHandler(tbValue_Changed);

         this.valueTimer = new Timer();
         this.valueTimer.Interval = 100;
         this.valueTimer.Tick += new EventHandler(valueTimer_Tick);

         this.Controls.Add(tbValue);

         this.Dragging = false;
         this.Minimum = min;
         this.Maximum = max;
         this.Increment = step;
         this.DefaultValue = Value = defaultValue;
         this.DecimalPlaces = 1;
         this.ResizeRedraw = true;

         this.BorderStyle = System.Windows.Forms.BorderStyle.None;

         MaxConnection.Instance.RegisterControl(this);
      }

      private void valueTimer_Tick(object sender, EventArgs e)
      {
         timerTicks++;
         if (timerTicks < 5) return;

         if (btnDnClicked)
         {
            Value -= increment;
         }
         else if (btnUpClicked)
         {
            Value += increment;
         }
         else { valueTimer.Stop(); }
      }

      private void tbValue_Changed(object sender, EventArgs e)
      {
         Value = tbValue.Value;
      }

      protected override void OnMouseMove(MouseEventArgs e)
      {
         base.OnMouseMove(e);
         if (valueTimer.Enabled && ClientRectangle.Contains(e.Location))
         {
            if (btnUpClicked && new Rectangle(0, 0, Width, 8).Contains(e.Location))
            {
               return;
            }
            else if (btnDnClicked && new Rectangle(0, 8, Width, 8).Contains(e.Location))
            {
               return;
            }
         }
         Cursor.Current = dragging ? Cursors.SizeNS : Cursors.Arrow;
         if (dragging)
         {
            int currentY = e.Y;
            valueTimer.Stop();
            Point scrPoint = PointToScreen(e.Location);
            if (scrPoint.Y >= screenBottom - 1)
            {
               if (!wrappedMouse)
               {
                  Cursor.Position = new Point(scrPoint.X, screenTop);
                  wrappedMouse = true;
                  cycleValue -= screenBottom;
                  currentY -= screenBottom;
               }
            }
            else if (scrPoint.Y <= screenTop)
            {
               if (!wrappedMouse)
               {
                  Cursor.Position = new Point(scrPoint.X, screenBottom);
                  wrappedMouse = true;
                  cycleValue += screenBottom;
                  currentY += screenBottom;
               }
            }
            else wrappedMouse = false;
            decimal modifier = (Control.ModifierKeys & Keys.Control) > 0 ? 10m :
                (Control.ModifierKeys & Keys.Alt) > 0 ? .1m :
                1m;
            Value = dragValue + (cycleValue + dragPoint.Y - currentY) * increment * modifier;
            btnUpClicked = btnDnClicked = true;
            Refresh();
         }
      }

      protected override void OnMouseUp(MouseEventArgs e)
      {
         base.OnMouseUp(e);
         if (btnUpClicked || btnDnClicked || spinCancelled)
         {
            Dragging = false;
            btnDnClicked = btnUpClicked = false;
            if (ButtonUp != null)
            {
               ButtonUp(this, new SpinnerButtonEventArgs() { SpinCancelled = spinCancelled });
            }
         }
      }

      protected override void OnMouseDown(MouseEventArgs e)
      {
         base.OnMouseDown(e);
         Focus();
         tbValue.ValidateText();

         btnDnClicked = btnDownBounds.Contains(e.Location);
         btnUpClicked = btnUpBounds.Contains(e.Location);
         bool arrowsClicked = btnDnClicked || btnUpClicked;
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            if (!dragging && arrowsClicked)
            {
               valueAtButtonDown = Value;
               if (ButtonDown != null) ButtonDown(this, EventArgs.Empty);
            }
            if (btnDnClicked)
            {
               Value -= increment;
               Invalidate(btnDownBounds, false);
            }
            else if (btnUpClicked)
            {
               Value += increment;
               Invalidate(btnUpBounds, false);
            }
            if (arrowsClicked && !dragging)
            {
               timerTicks = 0;
               valueTimer.Start();
               Dragging = true;
               dragPoint = e.Location;
               cycleValue = 0;
               dragValue = Value;
               Rectangle screenBounds = Screen.FromPoint(PointToScreen(e.Location)).Bounds;
               screenBottom = screenBounds.Bottom;
               screenTop = screenBounds.Top;
               wrappedMouse = false;
            }
         }
         else if (e.Button == System.Windows.Forms.MouseButtons.Right)
         {
            if (dragging)
            {
               CancelSpin();
            }
            else if (arrowsClicked)
            {
               spinCancelled = false;
               ResetValue();
               if (ButtonDown != null) ButtonDown(this, EventArgs.Empty);
            }
         }
      }

      private void CancelSpin()
      {
         Dragging = false;
         spinCancelled = true;
         Value = valueAtButtonDown;
      }

      /// <summary>
      /// Resets the value of the spinner to the default value. Equivilent to right-clicking the arrows part of the Spinner.
      /// </summary>
      public void ResetValue()
      {
         Value = defaultValue;
         Invalidate(false);
      }

      protected override void OnGotFocus(EventArgs e)
      {
         base.OnGotFocus(e);
         tbValue.SelectAll();
      }

      protected override void OnLostFocus(EventArgs e)
      {
         tbValue.ValidateText();
         base.OnLostFocus(e);
      }

      internal void FocusNext()
      {
         if (Parent != null)
         {
            int index = Parent.Controls.IndexOf(this);
            if (index == Parent.Controls.Count - 1) index = 0;
            Parent.Controls[index].Focus();
         }
      }

      protected override void OnLayout(LayoutEventArgs e)
      {
         base.OnSizeChanged(e);
         if (tbValue == null) return;
         tbValue.Bounds = new Rectangle(5, 1, Width - 21, 12);
         tbBounds = new Rectangle(1, 0, Width - 15, 16);
         btnUpBounds = new Rectangle(Width - 14, 0, 12, 9);
         btnDownBounds = new Rectangle(Width - 14, 8, 12, 8);
      }

      protected override void OnPaint(PaintEventArgs e)
      {
         //base.OnPaint(e);
         MaxConnection.Instance.DrawFrame(e.Graphics, btnUpBounds, btnUpClicked);
         MaxConnection.Instance.DrawFrame(e.Graphics, btnDownBounds, btnDnClicked);
         MaxConnection.Instance.DrawFrame(e.Graphics, tbBounds, true);

         Rectangle r = tbBounds;
         r.Inflate(-1, -1);
         e.Graphics.DrawLines(MaxConnection.Instance.BackPen, new Point[] {
                new Point(r.Left, r.Bottom - 1),
                new Point(r.Right - 1, r.Bottom - 1),
                new Point(r.Right - 1, r.Top)});

         e.Graphics.DrawLines(Pens.Black, new Point[] {
                new Point(r.Left, r.Bottom - 1),
                new Point(r.Left, r.Top),
                new Point(r.Right - 2, r.Top)});

         e.Graphics.FillRectangle(MaxConnection.Instance.WindowBrush, new Rectangle(3, 2, 2, 12));

         e.Graphics.FillPolygon(MaxConnection.Instance.ForeBrush, new Point[] {
                new Point(btnUpBounds.Right - 4, btnUpBounds.Bottom - 3),
                new Point(btnUpBounds.Left + btnUpBounds.Width / 2 - 1, btnUpBounds.Top + 2),
                new Point(btnUpBounds.Left + 2, btnUpBounds.Bottom - 3)});

         e.Graphics.FillPolygon(MaxConnection.Instance.ForeBrush, new Point[] {
                new Point(btnDownBounds.Left + 3, btnDownBounds.Top + 2),
                new Point(btnDownBounds.Left + btnDownBounds.Width / 2 - 1, btnDownBounds.Bottom - 3),
                new Point(btnDownBounds.Right - 4, btnDownBounds.Top + 2)});
      }

      /// <summary>
      /// Clears the text part of the Spinner, leaving it in an indeterminate state. The various Value properties still return the previous underlying value.
      /// </summary>
      public void Clear()
      {
         tbValue.Text = String.Empty;
      }

      protected override void Dispose(bool disposing)
      {
         base.Dispose(disposing);
         if (disposing)
         {
            valueTimer.Dispose();
            MaxConnection.Instance.UnregisterControl(this);
         }
      }
   }
}
