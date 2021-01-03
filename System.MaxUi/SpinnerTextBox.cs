using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;

namespace System.MaxUi
{
   internal class SpinnerTextBox : System.Windows.Forms.TextBox, IMaxControl
   {
      public event EventHandler ValueChanged;

      private string valueFormatStr;
      private int decimalLength;

      private decimal value;

      private bool textValid;

      public void UpdateColors()
      {
         ForeColor = MaxConnection.Instance.WindowTextColor;
         BackColor = MaxConnection.Instance.WindowColor;
      }

      internal SpinnerTextBox()
      {
         TextAlign = HorizontalAlignment.Left;
         BorderStyle = System.Windows.Forms.BorderStyle.None;
         Text = "0";
         DecimalLength = 1;

         FontHeight = 13;

         Size = new Size(1, 1);

         if (DesignMode) return;
         textValid = true;
      }

      protected override void OnLayout(LayoutEventArgs levent)
      {
         base.OnLayout(levent);
         this.Region = new Region(new Rectangle(0, 1, this.Width, 12));
      }


      public decimal Value
      {
         get { return value; }
         set
         {
            this.value = value;
            UpdateText();
         }
      }

      protected override Size DefaultMinimumSize
      {
         get
         {
            return new Size(1, 1);
         }
      }

      public int DecimalLength
      {
         get { return decimalLength; }
         set
         {
            decimalLength = value;
            valueFormatStr = "F" + decimalLength.ToString();
            UpdateText();
         }
      }

      protected override void OnKeyUp(KeyEventArgs e)
      {
         base.OnKeyUp(e);
         if (e.KeyCode == Keys.Enter)
         {
            ValidateText();
            SelectAll();
         }
      }

      protected override void OnKeyPress(KeyPressEventArgs e)
      {
         base.OnKeyPress(e);
         textValid = false;
         if (e.KeyChar == (char)Keys.Enter)
         {
            ValidateText();
            SelectAll();
         }
         if (e.KeyChar == (char)Keys.Tab && Parent != null)
         {
            (Parent as Spinner).FocusNext();
         }
         string keyChar = e.KeyChar.ToString();
         if (!char.IsDigit(e.KeyChar) &&
             keyChar != CultureInfo.CurrentCulture.NumberFormat.NegativeSign &&
             keyChar != CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator &&
             e.KeyChar != (char)8) //backspace
         {
            e.Handled = true;
         }
      }

      internal void ValidateText()
      {
         if (textValid) return;
         textValid = true;
         decimal parsed;
         if (Decimal.TryParse(Text, out parsed))
         {
            value = parsed;
            UpdateText();
            if (ValueChanged != null) ValueChanged(this, EventArgs.Empty);
         }
         else UpdateText();
      }

      private void UpdateText()
      {
         Text = value.ToString(valueFormatStr, System.Globalization.CultureInfo.CurrentCulture);
      }

      protected override void OnGotFocus(EventArgs e)
      {
         if (!DesignMode) MaxConnection.Instance.DisableAccelerators();
         base.OnGotFocus(e);
      }

      protected override void OnLostFocus(EventArgs e)
      {
         ValidateText();
         if (!DesignMode) MaxConnection.Instance.EnableAccelerators();
         base.OnLostFocus(e);
      }
   }
}
