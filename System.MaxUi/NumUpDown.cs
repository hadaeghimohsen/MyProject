using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using System.Text.RegularExpressions;

namespace System.MaxUi
{
    [DefaultBindingProperty("Value")]
    //[Designer(typeof(NumUpDownDesing))]
    [DefaultEvent("ValueChange")]
    public partial class NumUpDown : UserControl
    {
        public NumUpDown()
        {
            InitializeComponent();
            ValueText.MouseWheel += new MouseEventHandler(ValueText_MouseWheel);
        }

        void ValueText_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
                ChangeValue(true);
            else
                ChangeValue(false);
        }

        double min = 0, max = 100, increment = 1;
        int decimalPlace = 0;
        double value = 10, tmp;
        string point = "0";
        bool status = true;
        bool disabled = false;

        public bool Disabled
        {
            get { return disabled; }
            set 
            { 
                disabled = value;
                Down.Disabled = Up.Disabled = disabled;
                ValueText.Enabled = !disabled;
            }
        }
        public double Increment
        {
            get { return increment; }
            set { increment = value; }
        }
        public double Max
        {
            get { return max; }
            set { max = value; }
        }
        public double Min
        {
            get { return min; }
            set { min = value; Value = value; }
        }
        public int DecimalPlace
        {
            get { return decimalPlace; }
            set 
            { 
                decimalPlace = value;
                Value = Value;
                //Value = Min;
            }
        }
        public double Value
        {
            get { return this.value; }
            set 
            {
                this.value = value;
                SetString();
            }
        }

        private void Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Disabled)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ChangeValue(true);
                    Runner.Start();
                    status = true;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    Value = Max;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    Value = (Max + Min) / 2;
                }
            }
        }

        private void Up_MouseUp(object sender, MouseEventArgs e)
        {
            if(!Disabled)
                Runner.Stop();
        }
        
        private void Down_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Disabled)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ChangeValue(false);
                    Runner.Start();
                    status = false;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    Value = Min;
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    Value = (Max + Min) / 2;
                }
            }
        }

        private void Down_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Disabled)
                Runner.Stop();
        }
        
        private void ChangeValue(bool p)
        {
            try
            {
                //int indsignplus = ValueText.Text.IndexOf('+');
                //int indsignmins = ValueText.Text.IndexOf('-');

                //string scvalue = "";

                //if (this.RightToLeft == RightToLeft.Yes)
                //{
                //    if (indsignplus >= 1) { scvalue = ValueText.Text.Substring(0, indsignplus - 1); }
                //    else if (indsignmins >= 1) { scvalue = ValueText.Text.Substring(0, indsignmins - 1); }
                //    else { indsignplus = 1; }
                //    tmp = Convert.ToDouble(scvalue)*(indsignplus >= 1 ? 1 : -1) ;

                //}
                //else
                //{
                //}
                //Value = Convert.ToDouble(ValueText.Text);
                
                switch (p)
                {
                    case true: // Increment
                        // Check Increment Value for Set on Control
                        #region Inc
                        tmp = Value + Increment;
                        if (tmp <= Max)
                        {
                            Value = tmp;
                        }
                        else
                        {
                            Value = Max;
                        }
                        break;
                        #endregion
                    case false: // Decrement
                        #region Decrement
                        tmp = Value - Increment;
                        if (tmp >= Min)
                        {
                            Value = tmp;
                        }
                        else
                        {
                            Value = Min;
                        }
                        #endregion
                        break;
                }                
            }
            catch (Exception)
            {
            }
        }

        private void Runner_Tick(object sender, EventArgs e)
        {
            ChangeValue(status);
        }
        
        private void CreateString()
        {
            try
            {
                point += ".";
                for (int i = 0; i < DecimalPlace; i++)
                {
                    point += "0";
                }
            }
            catch (Exception)
            {
            }
        }

        private void ValueText_Leave(object sender, EventArgs e)
        {
            try
            {
                tmp = Convert.ToDouble(ValueText.Text);
                if (tmp <= Max && tmp >= Min)
                {
                    Value = tmp;
                }
                else
                {
                    //Value = value;
                    SetString();
                }
            }
            catch (Exception)
            {
                //Value = value;
                SetString();
            }
        }

        private void ValueText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ValueText_Leave(null, null);
                ValueText.Select(0,ValueText.Text.Length);
            }
            else if (e.KeyCode == Keys.Up)
            {
                ChangeValue(true);
            }
            else if (e.KeyCode == Keys.Down)
            {
                ChangeValue(false);
            }
        }

        private void NumUpDown_SizeChanged(object sender, EventArgs e)
        {
            Height = 18;
        }

        private void NumUpDown_Load(object sender, EventArgs e)
        {
            //Value = min;
            Up.name.Top = 1;
            Up.name.Left = 1;
            Up.name.Width += 2;
            Up.name.Height += 2;

            Down.name.Top = 1;
            Down.name.Left = 1;
            Down.name.Width += 2;
            Down.name.Height += 2;
        }

        private void Up_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Disabled)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ChangeValue(true);
                    Runner.Start();
                    status = true;
                }
            }
        }

        private void Up_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Disabled)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Runner.Stop();
                }
            }
        }

        private void Down_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Disabled)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    ChangeValue(false);
                    Runner.Start();
                    status = false;
                }
            }
        }

        private void Down_KeyUp(object sender, KeyEventArgs e)
        {
            if (!Disabled)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Runner.Stop();
                }
            }
        }

        public delegate void ValueHandel(object sender, EventArgs e);
        public event ValueHandel ValueChange;
        private void ValueText_TextChanged(object sender, EventArgs e)
        {
            if (ValueChange != null)
                ValueChange(sender, e);
        }

        private void SetString()
        {
            try
            {
                string sign = "";
                switch (decimalPlace)
                {
                    case 0:
                        if (value > 0) sign = "+";
                        else if (value < 0) sign = "-";
                        if (this.RightToLeft == RightToLeft.Yes)
                            ValueText.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            ValueText.Text = sign + Math.Abs(value).ToString(point);
                        //textValue.Text = value.ToString(point);
                        break;
                    default:
                        CreateString();
                        if (value > 0) sign = "+";
                        else if (value < 0) sign = "-";
                        if (this.RightToLeft == RightToLeft.Yes)
                            ValueText.Text = Math.Abs(value).ToString(point) + sign;
                        else
                            ValueText.Text = sign + Math.Abs(value).ToString(point);
                        //textValue.Text = value.ToString(point);
                        point = "0";
                        break;
                }
            }
            catch (Exception) { }
        }

        private void NumUpDown_RightToLeftChanged(object sender, EventArgs e)
        {
            SetString();
        }


    }

    public class NumUpDownActionList : DesignerActionList
    {
        private NumUpDown linkControl;
        public NumUpDownActionList(NumUpDown ctrl)
            : base(ctrl)
        {
            linkControl = ctrl;
        }

        public double Increment
        {
            get { return linkControl.Increment; }
            set { GetPropertyByName("Increment").SetValue(linkControl, value); }
        }

        public double Max
        {
            get { return linkControl.Max; }
            set { GetPropertyByName("Max").SetValue(linkControl, value); }
        }

        public double Min
        {
            get { return linkControl.Min; }
            set { GetPropertyByName("Min").SetValue(linkControl, value); }
        }

        public int DecimalPlace
        {
            get { return linkControl.DecimalPlace; }
            set { GetPropertyByName("DecimalPlace").SetValue(linkControl, value); }
        }

        public double Value
        {
            get { return linkControl.Value; }
            set { GetPropertyByName("Value").SetValue(linkControl, value); }
        }

        public bool Disabled
        {
            get { return linkControl.Disabled; }
            set { GetPropertyByName("Disabled").SetValue(linkControl, value); }
        }


        private PropertyDescriptor GetPropertyByName(string propName)
        {
            PropertyDescriptor prop;
            prop = TypeDescriptor.GetProperties(linkControl)[propName];
            if (null == prop)
            {
                throw new ArgumentException("Matching property not found.", propName);
            }
            else
            {
                return prop;
            }
        }

        public override DesignerActionItemCollection GetSortedActionItems()
        {
            // Create eight items.
            DesignerActionItemCollection items = new DesignerActionItemCollection();
            // Begin by creating the headers.
            items.Add(new DesignerActionHeaderItem("Misc"));
            //items.Add(new DesignerActionHeaderItem("Information"));

            items.Add(new DesignerActionPropertyItem("DecimalPlace", "DecimalPlace :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Increment", "Increment :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Max", "Max :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Min", "Min :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Value", "Value :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Disabled", "Disabled", "Misc", ""));


            return items;
        }
    }

    public class NumUpDownDesing : ParentControlDesigner
    {
        private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new NumUpDownActionList((NumUpDown)Control));
                }
                return actionLists;
            }
        }
    }
}
