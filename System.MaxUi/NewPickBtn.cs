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
    [DefaultBindingProperty("PickChecked")]    
    //[Designer(typeof(NewPickBtnDesing))]
    [DefaultEvent("PickCheckedChange")]
    public partial class NewPickBtn : UserControl
    {
        public NewPickBtn()
        {
            InitializeComponent();
        }

        Color pickUpTextColor, pickDownTextColor, backGroundColor = Color.FromArgb(238, 204, 85);
        string pickUpText, pickDownText;
        Font pickUpFont = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178))), pickDownFont = new Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
        NewPickBtn first, next;
        bool disabled;
        bool pickChecked;
        bool forceSelect;
        int imageIndexPickUp, imageIndexPickDown;

        public Color BackGroundColor
        {
            get { return backGroundColor; }
            set { backGroundColor = value; }
        }
        public Color PickUpTextColor
        {
            get { return pickUpTextColor; }
            set { pickUpTextColor = value; Disabled = Disabled; }
        }
        public Color PickDownTextColor
        {
            get { return pickDownTextColor; }
            set { pickDownTextColor = value; Disabled = Disabled; }
        }
        public string PickDownText
        {
            get { return pickDownText; }
            set
            {
                //if (!Disabled)
                {
                    pickDownText = value;
                    if (PickChecked)
                        name.Text = pickDownText;
                }
            }
        }
        public string PickUpText
        {
            get { return pickUpText; }
            set
            {
                //if (!Disabled)
                {
                    pickUpText = value;
                    if (PickChecked == false)
                        name.Text = pickUpText;
                }
            }
        }
        public Font PickDownFont
        {
            get { return pickDownFont; }
            set { pickDownFont = value; }
        }
        public Font PickUpFont
        {
            get { return pickUpFont; }
            set { pickUpFont = value; }
        }
        public NewPickBtn Next
        {
            get { return next; }
            set { next = value; }
        }
        public NewPickBtn First
        {
            get { return first; }
            set { first = value; }
        }
        public bool Disabled
        {
            get { return disabled; }
            set 
            { 
                disabled = value;
                if (disabled == false)
                {
                    if (PickChecked)
                    {
                        name.ForeColor = PickDownTextColor;

                        top.BackColor = Color.FromArgb(130, 130, 130);
                        left.BackColor = Color.FromArgb(130, 130, 130);
                        down.BackColor = Color.FromArgb(225, 225, 225);
                        right.BackColor = Color.FromArgb(225, 225, 225);
                    }
                    else
                    {
                        name.ForeColor = PickUpTextColor;

                        top.BackColor = Color.FromArgb(225, 225, 225);
                        left.BackColor = Color.FromArgb(225, 225, 225);
                        down.BackColor = Color.FromArgb(130, 130, 130);
                        right.BackColor = Color.FromArgb(130, 130, 130);
                    }

                }
                else
                {
                    name.ForeColor = SystemColors.GrayText;
                    top.BackColor = down.BackColor = left.BackColor = right.BackColor = SystemColors.GrayText;
                }
            }
        }
        public bool PickChecked
        {
            get { return pickChecked; }
            set
            {
                if (Disabled == false)
                {
                    pickChecked = value;
                    if (pickChecked)
                    {
                        PickDown(this);
                    }
                    else
                    {
                        PickUp(this);
                    }
                    if (PickChecked == true)
                    {
                        if (First != null)
                        {
                            NewPickBtn F = First;
                            if (F == this)
                            {
                                F = Next;
                            }
                            //if (PickChecked) // true
                            {
                                while (F != null)
                                {
                                    if (F.PickChecked) // true
                                    {
                                        PickUp(F);
                                        F.pickChecked = false;
                                        //PickChecked = true;
                                        //F = null;
                                        break;
                                    }
                                    else // false
                                    {
                                        F = F.Next;
                                        if (F == this)
                                        {
                                            F = F.Next;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (PickCheckedChange != null)
                        PickCheckedChange(this);
                }
            }
        }
        public bool ForceSelect
        {
            get { return forceSelect; }
            set { forceSelect = value; }
        }
        public ImageList ImageList
        {
            get { return name.ImageList; }
            set 
            {
                if (!Disabled)
                    name.ImageList = value;
            }
        }
        public ContentAlignment ImageAlign
        {
            get { return name.ImageAlign; }
            set { name.ImageAlign = value; }
        }
        public int ImageIndexPickUp
        {
            get { return imageIndexPickUp; }
            set
            {
                imageIndexPickUp = value;
                if (PickChecked == false)
                    name.ImageIndex = imageIndexPickUp;
            }
        }
        public int ImageIndexPickDown
        {
            get { return imageIndexPickDown; }
            set
            {
                imageIndexPickDown = value;
                if (PickChecked)
                    name.ImageIndex = imageIndexPickDown;
            }
        }
        public ContentAlignment TextAligns
        {
            get { return name.TextAlign; }
            set { name.TextAlign = value; }
        }
       

        private void name_MouseDown(object sender, MouseEventArgs e)
        {
            if (Disabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (PickChecked == false)
                    {
                        //PickDown(this);
                        PickChecked = true;
                    }
                    else
                    {
                        //PickUp(this);
                        PickChecked = false;
                    }
                }
                base.OnMouseDown(e);
            }
        }

        private void name_MouseUp(object sender, MouseEventArgs e)
        {
            if (Disabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    //if (PickChecked == true)
                    //{
                    //    if (First != null)
                    //    {
                    //        NewPickBtn F = First;
                    //        if (F == this)
                    //        {
                    //            F = Next;
                    //        }
                    //        //if (PickChecked) // true
                    //        {
                    //            while (F != null)
                    //            {
                    //                if (F.PickChecked) // true
                    //                {
                    //                    PickUp();
                    //                    F.PickChecked = false;
                    //                    //PickChecked = true;
                    //                    //F = null;
                    //                    break;
                    //                }
                    //                else // false
                    //                {
                    //                    F = F.Next;
                    //                    if (F == this)
                    //                    {
                    //                        F = F.Next;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    if(PickChecked == false)
                    {
                        if (ForceSelect)
                        {
                            pickChecked = true;
                            PickDown(this);
                        }
                    }
                    base.OnClick(e);
                }
                base.OnMouseUp(e);
            }
        }

        private void PickUp(NewPickBtn p)
        {
            p.name.Text = p.PickUpText;
            p.name.Font = p.PickUpFont;
            p.name.ForeColor = p.PickUpTextColor;
            p.name.BackColor = Color.Transparent;//Color.FromArgb(197, 197, 197);
            p.name.ImageIndex = ImageIndexPickUp;

            p.top.BackColor = Color.FromArgb(225, 225, 225);
            p.left.BackColor = Color.FromArgb(225, 225, 225);
            p.down.BackColor = Color.FromArgb(130, 130, 130);
            p.right.BackColor = Color.FromArgb(130, 130, 130);
        }

        private void PickDown(NewPickBtn p)
        {
            p.name.Text = p.PickDownText;
            p.name.Font = p.PickDownFont;
            p.name.ForeColor = p.PickDownTextColor;
            p.name.BackColor = p.BackGroundColor;
            p.name.ImageIndex = ImageIndexPickDown;

            p.top.BackColor = Color.FromArgb(130, 130, 130);
            p.left.BackColor = Color.FromArgb(130, 130, 130);
            p.down.BackColor = Color.FromArgb(225, 225, 225);
            p.right.BackColor = Color.FromArgb(225, 225, 225);
        }

        private void PickBtn_Load(object sender, EventArgs e)
        {
            if (PickChecked)
            {
                PickDown(this);
            }
            else
            {
                PickUp(this);
            }
        }

        private void name_DragDrop(object sender, DragEventArgs e)
        {
            base.OnDragDrop(e);
        }

        private void name_DragEnter(object sender, DragEventArgs e)
        {
            base.OnDragEnter(e);
        }

        private void name_DragLeave(object sender, EventArgs e)
        {
            base.OnDragLeave(e);
        }

        private void name_DragOver(object sender, DragEventArgs e)
        {
            base.OnDragOver(e);
        }

        private void name_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            base.OnGiveFeedback(e);
        }

        private void name_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            base.OnQueryContinueDrag(e);
        }

        private void NewPickBtn_SizeChanged(object sender, EventArgs e)
        {
            down.Width = top.Width = this.Width - 1;
            down.Top = this.Height - 1;

            left.Height = right.Height = this.Height - 1;
            right.Left = this.Width - 1;

            name.Height = this.Height - 4;
            name.Width = this.Width - 4;
        }

        public delegate void PickCheckedHandel(object sender);
        public event PickCheckedHandel PickCheckedChange;
    }

    public class NewPickBtnActionList : DesignerActionList
    {
        private NewPickBtn linkControl;
        public NewPickBtnActionList(NewPickBtn ctrl)
            : base(ctrl)
        {
            linkControl = ctrl;
        }

        public Color BackGroundColor
        {
            get { return linkControl.BackGroundColor; }
            set { GetPropertyByName("BackGroundColor").SetValue(linkControl, value); }
        }
        public Color PickUpTextColor
        {
            get { return linkControl.PickUpTextColor; }
            set { GetPropertyByName("PickUpTextColor").SetValue(linkControl, value); }
        }
        public Color PickDownTextColor
        {
            get { return linkControl.PickDownTextColor; }
            set { GetPropertyByName("PickDownTextColor").SetValue(linkControl, value); }
        }
        public string PickDownText
        {
            get { return linkControl.PickDownText; }
            set { GetPropertyByName("PickDownText").SetValue(linkControl, value); }
        }
        public string PickUpText
        {
            get { return linkControl.PickUpText; }
            set { GetPropertyByName("PickUpText").SetValue(linkControl, value); }
        }
        public Font PickDownFont
        {
            get { return linkControl.PickDownFont; }
            set { GetPropertyByName("PickDownFont").SetValue(linkControl, value); }
        }
        public Font PickUpFont
        {
            get { return linkControl.PickUpFont; }
            set { GetPropertyByName("PickUpFont").SetValue(linkControl, value); }
        }
        public NewPickBtn Next
        {
            get { return linkControl.Next; }
            set { GetPropertyByName("Next").SetValue(linkControl, value); }
        }
        public NewPickBtn First
        {
            get { return linkControl.First; }
            set { GetPropertyByName("First").SetValue(linkControl, value); }
        }
        public bool Disabled
        {
            get { return linkControl.Disabled; }
            set { GetPropertyByName("Disabled").SetValue(linkControl, value); }
        }
        public bool PickChecked
        {
            get { return linkControl.PickChecked; }
            set { GetPropertyByName("PickChecked").SetValue(linkControl, value); }
        }
        public bool ForceSelect
        {
            get { return linkControl.ForceSelect; }
            set { GetPropertyByName("ForceSelect").SetValue(linkControl, value); }
        }
        public ImageList ImageList
        {
            get { return linkControl.ImageList; }
            set { GetPropertyByName("ImageList").SetValue(linkControl, value); }
        }
        public ContentAlignment ImageAlign
        {
            get { return linkControl.ImageAlign; }
            set { GetPropertyByName("ImageAlign").SetValue(linkControl, value); }
        }
        public int ImageIndexPickUp
        {
            get { return linkControl.ImageIndexPickUp; }
            set { GetPropertyByName("ImageIndexPickUp").SetValue(linkControl, value); }
        }
        public int ImageIndexPickDown
        {
            get { return linkControl.ImageIndexPickDown; }
            set { GetPropertyByName("ImageIndexPickDown").SetValue(linkControl, value); }
        }
        public ContentAlignment TextAligns
        {
            get { return linkControl.TextAligns; }
            set { GetPropertyByName("TextAligns").SetValue(linkControl, value); }
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

            items.Add(new DesignerActionPropertyItem("First", "First :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Next", "Next :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ForceSelect", "ForceSelect", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickChecked", "PickChecked", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickDownText", "PickDownText :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickUpText", "PickUpText :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("BackGroundColor", "BackGroundColor :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickUpTextColor", "PickUpTextColor :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickDownTextColor", "PickDownTextColor :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickDownFont", "PickDownFont :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("PickUpFont", "PickUpFont :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("TextAligns", "TextAligns :", "Misc", ""));            
            items.Add(new DesignerActionPropertyItem("ImageList", "ImageList :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ImageAlign", "ImageAlign :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ImageIndexPickUp", "ImageIndexPickUp :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ImageIndexPickDown", "ImageIndexPickDown :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Disabled", "Disabled", "Misc", ""));


            return items;
        }
    }

    public class NewPickBtnDesing : ParentControlDesigner
    {
        private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new NewPickBtnActionList((NewPickBtn)Control));
                }
                return actionLists;
            }
        }
    }
}
