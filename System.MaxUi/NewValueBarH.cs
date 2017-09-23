using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace System.MaxUi
{
    [DefaultBindingProperty("Value")]
    //[Designer(typeof(CheckBtnDesing))]
    [DefaultEvent("ValChange")]
    public partial class NewValueBarH : UserControl
    {
        public NewValueBarH()
        {
            InitializeComponent();
            Min = min;
            Max = max;
            //Disabled = Disabled;
        }

        double value = 0, min = -10, max = 10;
        bool decimalShow = true;
        TextBox textValue = null;
        bool disabled = false;
        int Tmp;
        Point pointMouse;
        double PMax;
        int decimalPoint = 1;
        string point = "0";
        double inc = .1;
        int XX;
        Font decimalFont;

        public double Inc
        {
            get { return inc; }
            set { inc = value; }
        }
        public int DecimalPoint
        {
            get { return decimalPoint; }
            set { if (value > 0)decimalPoint = value; }
        }
        public double Value
        {
            get { return value; }
            set
            {
                if (!disabled)
                    if (value <= Max && value >= min)
                    {
                        this.value = value;

                        Tmp = Convert.ToInt32(Math.Ceiling((Frame.Width - 4) * Math.Abs(Min) / PMax));

                        if (this.value >= 0)
                        {
                            if (this.value == max)
                                Progress.Width = Frame.Width - 6;
                            else
                                Progress.Width = Tmp + Convert.ToInt32(Math.Ceiling((Frame.Width - 4) * this.value / PMax)) - 3;
                        }
                        else
                        {
                            if (this.value == min)
                                Progress.Width = 0;
                            else
                                Progress.Width = Tmp - Convert.ToInt32(Math.Ceiling((Frame.Width - 4) * Math.Abs(this.value) / PMax));
                        }

                        ValueChange.Left = Progress.Width + 3;

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
        public double Min
        {
            get { return min; }
            set
            {
                DMin.Text = "";
                min = value;
                PMax = max - min;
                if (min > 0) DMin.Text = "+";
                else if(min < 0)DMin.Text = "-";
                if (this.RightToLeft == RightToLeft.Yes)
                    DMin.Text = Math.Abs(min).ToString() + DMin.Text;
                else
                    DMin.Text += Math.Abs(min).ToString();
            }
        }
        public double Max
        {
            get { return max; }
            set
            {
                DMax.Text = "";
                max = value;
                if (max > 0) DMax.Text = "+";
                else if(max < 0)DMax.Text = "-";
                if (this.RightToLeft == RightToLeft.Yes)
                    DMax.Text = Math.Abs(max).ToString() + DMax.Text;
                else
                    DMax.Text += Math.Abs(max).ToString();
                DMax.Left = Frame.Width - (DMax.Width - 15);
            }
        }
        public Color ProgressColor
        {
            get { return Progress.BackColor; }
            set { Progress.BackColor = value; Progress.Refresh(); }
        }
        public bool DecimalShow
        {
            get { return decimalShow; }
            set
            {
                decimalShow = value;
                DMax.Visible = DMin.Visible = decimalShow;
            }
        }
        public TextBox TextValue
        {
            get { return textValue; }
            set
            {
                textValue = value;
                if ((value != null))
                {
                    textValue.Text = this.value.ToString();
                    textValue.Leave += new EventHandler(textValue_Leave);
                    textValue.MouseWheel += new MouseEventHandler(textValue_MouseWheel);
                    textValue.KeyDown += new KeyEventHandler(textValue_KeyDown);
                    textValue.Enabled = !disabled;
                }
            }
        }
        public Font DecimalFont
        {
            get { return decimalFont; }
            set
            {
                if (!disabled)
                {
                    decimalFont = value;
                    DMin.Font = DMax.Font = decimalFont;
                }
            }
        }

        void textValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    double Tmp = 0;
                    Tmp = Convert.ToDouble(textValue.Text);
                    if (Tmp <= max && Tmp >= min)
                        Value = Tmp;
                    else
                        SetString();
                }
                catch (Exception) { SetString(); }
                textValue.Select(0, textValue.Text.Length);
            }
            else if (e.KeyCode == Keys.Up)
            {
                switch (decimalPoint)
                {
                    case 0:
                        Value++;
                        break;
                    default:
                        ChangeValue(true);
                        Value = value;
                        break;
                }
                //textValue.Text = value.ToString();
                SetString();
                //textValue.Select(0, textValue.Text.Length);
            }
            else if (e.KeyCode == Keys.Down)
            {
                switch (decimalPoint)
                {
                    case 0:
                        Value--;
                        break;
                    default:
                        ChangeValue(false);
                        Value = value;
                        break;
                }
                //textValue.Text = value.ToString();
                SetString();
                //textValue.Select(0, textValue.Text.Length);
            }
        }

        void textValue_Leave(object sender, EventArgs e)
        {
            try
            {
                double Tmp = 0;
                Tmp = Convert.ToDouble(textValue.Text);
                if (Tmp <= max && Tmp >= min)
                    Value = Tmp;
                else
                    SetString();
            }
            catch (Exception) { SetString(); }
        }

        void textValue_MouseWheel(object sender, MouseEventArgs e)
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

        public bool Disabled
        {
            get { return disabled; }
            set 
            { 
                disabled = value;
                DMax.Enabled = DMin.Enabled = Frame.Enabled = ValueChange.Enabled = !disabled;
                //DMax.Refresh();
                //DMin.Refresh();
                //Frame.Refresh();
                //ValueChange.Refresh();
                try
                {
                    textValue.Enabled = !disabled;
                    //textValue.Refresh();
                }
                catch (Exception)
                {
                }
            }
        }

        private void ValueBarH_SizeChanged(object sender, EventArgs e)
        {
            //ValueChange.Left = Progress.Width + 3;
            Tmp = Convert.ToInt32(Math.Ceiling((Frame.Width - 4) * Math.Abs(Min) / PMax));
            if (this.value >= 0)
            {
                if (this.value == max)
                    Progress.Width = Frame.Width - 6;
                else
                    Progress.Width = Tmp + Convert.ToInt32(Math.Ceiling((Frame.Width - 4) * this.value / PMax));
            }
            else
            {
                Progress.Width = Tmp - Convert.ToInt32(Math.Ceiling((Frame.Width - 4) * Math.Abs(this.value) / PMax));
            }

            ValueChange.Left = Progress.Width + 3;
        }

        private void ValueChange_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Disabled)
            {
                Bar.Enabled = true;
                Tmp = e.Location.X;
                XX = PointToClient(Cursor.Position).X;                
            }
        }

        private void Bar_Tick(object sender, EventArgs e)
        {
            pointMouse = Cursor.Position;
            pointMouse = PointToClient(pointMouse);


            if (pointMouse.X - Tmp < 3)
                ValueChange.Left = 3;
            else if (pointMouse.X - Tmp > Frame.Width - 3)
                ValueChange.Left = Frame.Width - 3;
            else
                ValueChange.Left = pointMouse.X - Tmp;

            Progress.Width = ValueChange.Left - 3;
            
            if (Progress.Width == Frame.Width - 6)
                value = max;
            else if (Progress.Width == 0)
                value = min;
            else if(decimalPoint > 0)
            {
                value = Convert.ToDouble((PMax * Progress.Width / (Frame.Width - 4))) + Min;

                if (XX > pointMouse.X)
                    ChangeValue(true);
                else if (XX < pointMouse.X)
                    ChangeValue(false);
                XX = pointMouse.X;
            }           
            else
                value = Convert.ToDouble((PMax * Progress.Width / (Frame.Width - 4))) + Min;


            try
            {
                //textValue.Text = value.ToString();
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
                //value = Convert.ToDouble(textValue.Text);
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
                            value = max;
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
                            value = min;
                        }
                        #endregion
                        break;
                }

            }
            catch (Exception)
            {
            }
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
                        if(this.RightToLeft == RightToLeft.Yes)
                            textValue.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            textValue.Text = sign + Math.Abs(value).ToString(point);
                        break;
                    default:
                        CreateString();
                        if (value > 0) sign = "+";
                        else if (value < 0) sign = "-";
                        if (this.RightToLeft == RightToLeft.Yes)
                            textValue.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            textValue.Text = sign + Math.Abs(value).ToString(point);
                        //textValue.Text = value.ToString(point);
                        point = "0";
                        break;
                }
            }
            catch (Exception) { }
        }

        public delegate void ValueChangeHandel(object sender);
        public event ValueChangeHandel ValChange;

        private void NewValueBarH_RightToLeftChanged(object sender, EventArgs e)
        {
            Min = min;
            Max = max;
            SetString();
        }
    }
}
