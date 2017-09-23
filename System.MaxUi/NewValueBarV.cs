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
    public partial class NewValueBarV : UserControl
    {
        public NewValueBarV()
        {
            InitializeComponent();
            Min = min;
            Max = max;
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
        int YY;
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
                {
                    if (value <= max && value >= min)
                    {
                        this.value = value;

                        Tmp = Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * Math.Abs(Min) / PMax));

                        if (this.value >= 0)
                        {
                            if (this.value == max)
                            {
                                Progress.Top = 1;
                                Progress.Height = Frame.Height - 6;
                            }
                            else
                            {
                                Progress.Top = Frame.Height - (Tmp + Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * this.value / PMax))) - 2;
                                Progress.Height = Tmp + Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * this.value / PMax));// -5;
                            }
                        }
                        else
                        {
                            if (this.value == min)
                            {
                                Progress.Top = Frame.Height - 6;
                                Progress.Height = 0;
                            }
                            else
                            {
                                Progress.Top = Frame.Height - (Tmp - Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * Math.Abs(this.value) / PMax))) - 5;
                                Progress.Height = Tmp - Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * Math.Abs(this.value) / PMax));// -5;
                            }
                        }

                        ValueChange.Top = Progress.Top + 13;

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
                DMin.Left = (int)(Frame.Width - DMin.Width / 2);
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
                DMax.Left = (int)(Frame.Width - DMax.Width / 2);
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
                    //textValue.TextChanged += new EventHandler(textValue_TextChanged);
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


        //void textValue_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (textValue.Text != "-" && textValue.Text != "+")
        //        {
        //            Tmp = Convert.ToInt32(textValue.Text);
        //            if (Tmp <= max && Tmp >= min)
        //                Value = Tmp;
        //            else
        //            {
        //                textValue.Text = value.ToString();
        //                textValue.Select(0, textValue.Text.Length);
        //            }
        //        }
        //    }
        //    catch (Exception) 
        //    { 
        //        textValue.Text = value.ToString();
        //        textValue.Select(0, textValue.Text.Length);
        //    }
        //}

        void textValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    double Tmp = Convert.ToDouble(textValue.Text);
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
                //SetString();
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
                //SetString();
                //textValue.Select(0, textValue.Text.Length);
            }
        }

        void textValue_Leave(object sender, EventArgs e)
        {
            try
            {
                double Tmp = Convert.ToDouble(textValue.Text);
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
                }
                catch (Exception)
                {
                }
            }
        }

        private void ValueBarV_SizeChanged(object sender, EventArgs e)
        {
            //ValueChange.Left = Progress.Width + 3;
            DMin.Left = (int)((Frame.Width - DMin.Width) / 2) + 9;
            DMax.Left = (int)((Frame.Width - DMax.Width) / 2) + 9;

            DMax.Top = 3;
            DMin.Top = this.Height - (DMax.Height + 3);

            Tmp = Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * Math.Abs(Min) / PMax));

            if (this.value >= 0)
            {
                if (this.value == max)
                {
                    Progress.Top = 1;
                    Progress.Height = Frame.Height - 6;
                }
                else
                {
                    Progress.Top = Frame.Height - (Tmp + Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * this.value / PMax)));
                    Progress.Height = Tmp + Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * this.value / PMax)) - 5;
                }
            }
            else
            {
                if (this.value == min)
                {
                    Progress.Top = Frame.Height - 6;
                    Progress.Height = 0;
                }
                else
                {
                    Progress.Top = Frame.Height - (Tmp - Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * Math.Abs(this.value) / PMax)));
                    Progress.Height = Tmp - Convert.ToInt32(Math.Ceiling((Frame.Height - 4) * Math.Abs(this.value) / PMax)) - 5;
                }
            }

            ValueChange.Top = Progress.Top + 13;
        }

        private void ValueChange_MouseDown(object sender, MouseEventArgs e)
        {
            if (!disabled)
            {
                Bar.Enabled = true;
                Tmp = e.Location.Y;
                YY = PointToClient(Cursor.Position).Y;                
            }
        }

        private void Bar_Tick(object sender, EventArgs e)
        {
            pointMouse = Cursor.Position;
            pointMouse = PointToClient(pointMouse);

            if (pointMouse.Y - Tmp < 14)
                ValueChange.Top = 14;
            else if (pointMouse.Y - Tmp > Frame.Height + 8)
                ValueChange.Top = Frame.Height + 8;
            else 
                ValueChange.Top = pointMouse.Y - Tmp;

            Progress.Top = ValueChange.Top - 13;
            Progress.Height = Frame.Height - ValueChange.Top + 8;
            
            //value = Convert.ToDecimal(Math.Floor(PMax * Progress.Height / (Frame.Height - 4))) + Min;

            if (Progress.Height == Frame.Height - 6)
                value = max;
            else if (Progress.Height == 0)
                value = min;
            else if (decimalPoint > 0)
            {
                value = Convert.ToDouble((PMax * Progress.Height / (Frame.Height - 4))) + Min;                

                if (YY > pointMouse.Y)
                    ChangeValue(true);
                else if (YY < pointMouse.Y)
                    ChangeValue(false);
                YY = pointMouse.Y;
            }    
            else
                value = Convert.ToDouble((PMax * Progress.Height / (Frame.Height - 4))) + Min;

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
                //MessageBox.Show(textValue.Text);
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
                        if (this.RightToLeft == RightToLeft.Yes)
                            textValue.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            textValue.Text = sign + Math.Abs(value).ToString(point);
                        //textValue.Text = value.ToString(point);
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

        private void NewValueBarV_RightToLeftChanged(object sender, EventArgs e)
        {
            Min = min;
            Max = max;
            SetString();   
        }

        private void NewValueBarV_Load(object sender, EventArgs e)
        {
            ValueBarV_SizeChanged(null,null);
        }
    }
}
