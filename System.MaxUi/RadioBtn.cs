using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace System.MaxUi
{
    [DefaultBindingProperty("Caption")]
    //[Designer(typeof(RadioBtnDesing))]
    [DefaultEvent("StatusChange")]
    public partial class RadioBtn : UserControl
    {
        public RadioBtn()
        {
            InitializeComponent();
        }

        RadioBtn first, next;
        bool status;
        bool disabled = false;

        public RadioBtn First
        {
            get { return first; }
            set { first = value; }
        }
        public RadioBtn Next
        {
            get { return next; }
            set { next = value; }
        }      
        public bool Disabled
        {
            get { return disabled; }
            set 
            { 
                disabled = value;
                if (disabled == false) // Enabled
                {
                    if (Status)
                        Limage.ImageIndex = 3;
                    else
                        Limage.ImageIndex = 0;
                    Ltext.Enabled = true;                    
                }
                else // not Enabled
                {
                    if (Status)
                        Limage.ImageIndex = 4;
                    else
                        Limage.ImageIndex = 1;
                    Ltext.Enabled = false;
                }
                Limage.Refresh();
            }
        }
        public bool Status
        {
            get { return status; }
            set 
            {
                if (Disabled == false)
                {
                    status = value;
                    if (status)
                        Limage.ImageIndex = 3;
                    else
                        Limage.ImageIndex = 0;

                    if (status == true)
                    {
                        if (First != null)
                        {
                            RadioBtn F = First;
                            if (F == this)
                            {
                                F = Next;
                            }
                            //if (PickChecked) // true
                            {
                                while (F != null)
                                {
                                    if (F.Status) // true
                                    {
                                        F.Status = false;
                                        //PickChecked = true;
                                        break;
                                    }
                                    else // false
                                    {
                                        F = F.Next;
                                        if (F == this)
                                            F = F.Next;
                                    }
                                }
                            }
                        }
                    }
                    Limage.Refresh();
                    if (StatusChange != null)
                        StatusChange(this);
                }
                
            }
        }
        public string Caption
        {
            get { return Ltext.Text; }
            set { Ltext.Text = value; }
        }
        public Font FontName
        {
            get { return Ltext.Font; }
            set { Ltext.Font = value; }
        }



        private void LImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (!Disabled)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (!Status)
                    {
                        Limage.ImageIndex = 4;
                    }
                }
            }
        }

        private void LImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Disabled)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point p = new Point(e.X,e.Y);
                    Label l = (Label)GetChildAtPoint(p);
                    if (l != null)
                    {
                        if (!Status)
                        {
                            Status = true;
                        }
                    }
                    else
                    {
                        if (Status)
                            Limage.ImageIndex = 3;
                        else
                            Limage.ImageIndex = 0;
                    }
                }
            }
        }

        public delegate void StatusValueHandel(object sender);
        public event StatusValueHandel StatusChange;
    }

    public class RadioBtnActionList : DesignerActionList
    {
        private RadioBtn linkControl;
        public RadioBtnActionList(RadioBtn ctrl)
            : base(ctrl)
        {
            linkControl = ctrl;
        }

        public bool Status
        {
            get { return linkControl.Status; }
            set { GetPropertyByName("Status").SetValue(linkControl, value); }
        }
        public string Caption
        {
            get { return linkControl.Caption; }
            set { GetPropertyByName("Caption").SetValue(linkControl, value); }
        }
        public RadioBtn First
        {
            get { return linkControl.First; }
            set { GetPropertyByName("First").SetValue(linkControl, value); }
        }
        public RadioBtn Next
        {
            get { return linkControl.Next; }
            set { GetPropertyByName("Next").SetValue(linkControl, value); }
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

            items.Add(new DesignerActionPropertyItem("First", "First:", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Next", "Next:", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Caption", "Caption", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Status", "Status", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Disabled", "Disabled", "Misc", ""));


            return items;
        }
    }

    public class RadioBtnDesing : ParentControlDesigner
    {
        private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new RadioBtnActionList((RadioBtn)Control));
                }
                return actionLists;
            }
        }
    }
}
