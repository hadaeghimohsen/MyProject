using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace System.MaxUi
{
    public partial class NewValueTextBar : UserControl
    {
        public NewValueTextBar()
        {
            InitializeComponent();
            Height = 19;
            Max = 10;
            Min = -10;
            TextValue.MouseWheel += new MouseEventHandler(TextValue_MouseWheel);
        }

        int XX;
        int X;
        int Tmp;
        bool disabled;
        double value, min, max, PMax;
        int decimalPoint = 1;
        double inc = .1;
        string point = "0";

        public double Inc
        {
            get { return inc; }
            set { inc = value; }
        }

        public int DecimalPoint
        {
            get { return decimalPoint; }
            set { if (value > 0) { decimalPoint = value; Value = Value; } }
        }

        public double Value
        {
            get { return this.value; }
            set 
            {
                if (!disabled)
                {
                    if (value <= max && value >= min)
                    {
                        this.value = value;

                        Tmp = Convert.ToInt32(Math.Ceiling((Frame.Width - 15) * Math.Abs(Min) / PMax)) ;

                        if (this.value >= 0)
                        {
                            if (this.value == max)
                                ValueChange.Left = Frame.Width - 15;
                            else
                                ValueChange.Left = Tmp + Convert.ToInt32(Math.Ceiling((Frame.Width -15 ) * this.value / PMax)) - 2;
                        }
                        else
                        {
                            if (this.value == min)
                                ValueChange.Left = 2;
                            else
                                ValueChange.Left = Tmp - Convert.ToInt32(Math.Ceiling((Frame.Width - 15) * Math.Abs(this.value) / PMax)) + 3;
                        }

                        try
                        {
                            SetString();
                        }
                        catch (Exception) { }
                        if (ValChange != null)
                            ValChange(this);
                    }
                }

            }
        }

        public double Min
        {
            get { return min; }
            set { min = value; PMax = max - min; }
        }

        public double Max
        {
            get { return max; }
            set { max = value; }
        }

        public bool Disabled
        {
            get { return disabled; }
            set
            {
                disabled = value;
                TextValue.Enabled = !disabled;
                ShowCV.Disabled = disabled;
            }
        }

        private void CreateString()
        {
            try
            {
                point += ".";
                for (int i = 0; i < decimalPoint; i++)
                {
                    point += "0";
                }
            }
            catch (Exception)
            {
            }
        }

        private void ChangeValue(bool p)
        {
            try
            {
                //value = Convert.ToDouble(TextValue.Text);
                double tmp;
                switch (p)
                {
                    case true: // Increment
                        // Check Increment Value for Set on Control
                        #region Inc
                        tmp = value + Inc;
                        if (tmp <= Max)
                        {
                            value = tmp;
                        }
                        else
                        {
                            value = Max;
                        }
                        break;
                        #endregion
                    case false: // Decrement
                        #region Decrement
                        tmp = value - Inc;
                        if (tmp >= Min)
                        {
                            value = tmp;
                        }
                        else
                        {
                            value = Min;
                        }
                        #endregion
                        break;
                }
                // Set Format on Value for Show on Text
                //switch (DecimalPlace)
                //{
                //    case 0:
                //        #region Int
                //        ValueText.Text = Value.ToString(point);
                //        #endregion
                //        break;
                //    default:
                //        #region Double
                //        CreateString();
                //        ValueText.Text = Value.ToString(point);
                //        // reset point on default value "0"
                //        point = "0";
                //        #endregion
                //        break;
                //}
            }
            catch (Exception)
            {
            }
        }

        private void ShowCV_Click(object sender, EventArgs e)
        {
            if (Height == 19)
            {
                Height = 36;
            }
            else
            {
                Height = 19;
            }
        }

        private void ValueChange_MouseDown(object sender, MouseEventArgs e)
        {
            if (!disabled)
            {
                Bar.Enabled = true;
                X = e.Location.X;
                XX = PointToClient(Cursor.Position).X;
            }
        }

        private void Bar_Tick(object sender, EventArgs e)
        {
            Point pointMouse = Cursor.Position;
            pointMouse = PointToClient(pointMouse);

            if (pointMouse.X - X - 7 < 2)
                ValueChange.Left = 2;
            else if (pointMouse.X > Frame.Width - 15)
                ValueChange.Left = Frame.Width - 15;
            else
                ValueChange.Left = pointMouse.X - 7;

            int w = pointMouse.X - 2;            

            if (ValueChange.Left == Frame.Width - 15)
                value = max;
            else if (ValueChange.Left == 2)
                value = min;
            else if(decimalPoint > 0)
            {
                value = Convert.ToDouble((PMax * w / (Frame.Width - 15))) + Min;
                if (XX > pointMouse.X)
                    ChangeValue(true);
                else if(XX < pointMouse.X)
                    ChangeValue(false);
                XX = pointMouse.X;
            }
            else
                value = Convert.ToDouble((PMax * w / (Frame.Width - 15))) + Min;


            try
            {
                SetString();
            }
            catch (Exception) { }
            if (ValChange != null)
                ValChange(this);
        }

        private void ValueChange_MouseUp(object sender, MouseEventArgs e)
        {
            Bar.Enabled = false;
        }

        private void TextValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    double Tmp = Convert.ToDouble(TextValue.Text);
                    if (Tmp <= max && Tmp >= min)
                        Value = Tmp;
                    else
                        SetString();
                }
                catch (Exception) { SetString(); }
                TextValue.Select(0, TextValue.Text.Length);
            }
            else if (e.KeyCode == Keys.Up)
            {
                //Value += inc;
                ChangeValue(true);
                Value = value;
            }
            else if (e.KeyCode == Keys.Down)
            {
                ChangeValue(false);
                Value = value;
                //Value -= inc;
            }
        }

        void TextValue_MouseWheel(object sender, MouseEventArgs e)
        {
            switch (decimalPoint)
            {
                case 0:
                    if (e.Delta > 0)
                        Value++;
                    else
                        Value--;
                    break;
                default:
                    if (e.Delta > 0)
                        ChangeValue(true);
                    else
                        ChangeValue(false);
                    Value = value;
                    break;
            }
            //textValue.Text = value.ToString();
            SetString();
        }

        void TextValue_Leave(object sender, EventArgs e)
        {
            try
            {
                double Tmp = Convert.ToDouble(TextValue.Text);
                if (Tmp <= max && Tmp >= min)
                    Value = Tmp;
                else
                    //TextValue.Text = value.ToString();
                    SetString();
            }
            catch (Exception) { /*TextValue.Text = value.ToString();*/SetString(); }
        }

        private void SetString()
        {
            try
            {
                string sign = "";
                switch (decimalPoint)
                {
                    case 0:
                        if (value > 0) sign = "+";
                        else if (value < 0) sign = "-";
                        if (this.RightToLeft == RightToLeft.Yes)
                            TextValue.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            TextValue.Text = sign + Math.Abs(value).ToString(point);
                        //textValue.Text = value.ToString(point);
                        break;
                    default:
                        CreateString();
                        if (value > 0) sign = "+";
                        else if (value < 0) sign = "-";
                        if (this.RightToLeft == RightToLeft.Yes)
                            TextValue.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            TextValue.Text = sign + Math.Abs(value).ToString(point);
                        //textValue.Text = value.ToString(point);
                        point = "0";
                        break;
                }
            }
            catch (Exception) { }
        }

        public delegate void ValueChangeHandel(object sender);
        public event ValueChangeHandel ValChange;

        private void TextValue_Enter(object sender, EventArgs e)
        {
            TextValue.Select(0,TextValue.Text.Length);
        }

        private void NewValueTextBar_RightToLeftChanged(object sender, EventArgs e)
        {
            SetString();
        }

        private void NewValueTextBar_Load(object sender, EventArgs e)
        {
            Value = 0;
        }

    }
}
