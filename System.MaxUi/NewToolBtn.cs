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
    //[Designer(typeof(NewToolBtnDesing))]
    [DefaultEvent("Click")]
    public partial class NewToolBtn : UserControl
    {
        public NewToolBtn()
        {
            InitializeComponent();
        }

        //string caption;
        //ContentAlignment textAlign = ContentAlignment.MiddleCenter;
        Color backGroundColor = Color.FromArgb(238, 204, 85);
        Color textColor;
        Font toolUpFont, toolDownFont;
        bool disabled;

        public string Caption
        {
            get { return name.Text; }
            set { name.Text = value; }
        }
        public ContentAlignment TextAlign
        {
            get { return name.TextAlign; }
            set { name.TextAlign = value; }
        }
        public Color BackGroundColor
        {
            get { return backGroundColor; }
            set { backGroundColor = value; }
        }
        public Font ToolUpFont
        {
            get { return toolUpFont; }
            set { toolUpFont = value; }
        }
        public Font ToolDownFont
        {
            get { return toolDownFont; }
            set { toolDownFont = value; }
        }
        public ImageList ImageList
        {
            get { return name.ImageList; }
            set 
            { 
                if(!Disabled)
                    name.ImageList = value; 
            }
        }
        public ContentAlignment ImageAlign
        {
            get { return name.ImageAlign; }
            set { name.ImageAlign = value; }
        }
        public int ImageIndex
        {
            get { return name.ImageIndex; }
            set { name.ImageIndex = value; }
        }
        public bool Disabled
        {
            get { return disabled; }
            set 
            { 
                disabled = value;
                if (disabled == false)
                    name.ForeColor = TextColor;
                else
                    name.ForeColor = SystemColors.GrayText;
            }
        }
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; Disabled = Disabled; }
        }


        private void name_MouseDown(object sender, MouseEventArgs e)
        {
            if (Disabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point p = new Point(e.X, e.Y);
                    Label a = (Label)this.GetChildAtPoint(p);
                    if (a != null)
                    {
                        ClickDown();
                    }
                    else
                    {
                        ClickUp();
                    }
                }
                base.OnMouseDown(e);
            }
        }

        private void ClickDown()
        {
            name.BackColor = BackGroundColor;
            name.Font = ToolDownFont;

            top.BackColor = Color.FromArgb(130, 130, 130);
            left.BackColor = Color.FromArgb(130, 130, 130);
            down.BackColor = Color.FromArgb(225, 225, 225);
            right.BackColor = Color.FromArgb(225, 225, 225);
        }

        private void name_MouseUp(object sender, MouseEventArgs e)
        {
            if (Disabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Point p = new Point(e.X, e.Y);
                    Label a = (Label)this.GetChildAtPoint(p);
                    if (a != null)
                    {
                        ClickUp();
                        base.OnClick(e);
                    }
                    else
                    {
                        ClickUp();
                    }
                }
                base.OnMouseUp(e);
            }
        }

        private void ClickUp()
        {
            name.BackColor = Color.Transparent;
            name.Font = ToolUpFont;

            top.BackColor = Color.FromArgb(225, 225, 225);
            left.BackColor = Color.FromArgb(225, 225, 225);
            down.BackColor = Color.FromArgb(130, 130, 130);
            right.BackColor = Color.FromArgb(130, 130, 130);
        }

        private void name_MouseEnter(object sender, EventArgs e)
        {
            if (Disabled == false)
            {
                left.BackColor = top.BackColor = Color.FromArgb(225, 225, 225);
                right.BackColor = down.BackColor = Color.FromArgb(130, 130, 130);
            }
        }

        private void name_MouseLeave(object sender, EventArgs e)
        {
            if (Disabled == false)
            {
                top.BackColor = left.BackColor = down.BackColor = right.BackColor = Color.Transparent;
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

        private void NewToolBtn_SizeChanged(object sender, EventArgs e)
        {
            down.Width = top.Width = this.Width - 1;
            down.Top = this.Height - 1;

            left.Height = right.Height = this.Height - 1;
            right.Left = this.Width - 1;

            name.Height = this.Height - 4;
            name.Width = this.Width - 4;
        }


    }

    public class NewToolBtnActionList : DesignerActionList
    {
        private NewToolBtn linkControl;
        public NewToolBtnActionList(NewToolBtn ctrl)
            : base(ctrl)
        {
            linkControl = ctrl;
        }

        public string Caption
        {
            get { return linkControl.Caption; }
            set { GetPropertyByName("Caption").SetValue(linkControl, value); }
        }
        public ContentAlignment TextAlign
        {
            get { return linkControl.TextAlign; }
            set { GetPropertyByName("TextAlign").SetValue(linkControl, value); }
        }
        public Color BackGroundColor
        {
            get { return linkControl.BackGroundColor; }
            set { GetPropertyByName("BackGroundColor").SetValue(linkControl, value); }
        }
        public Font ToolUpFont
        {
            get { return linkControl.ToolUpFont; }
            set { GetPropertyByName("ToolUpFont").SetValue(linkControl, value); }
        }
        public Font ToolDownFont
        {
            get { return linkControl.ToolDownFont; }
            set { GetPropertyByName("ToolDownFont").SetValue(linkControl, value); }
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
        public int ImageIndex
        {
            get { return linkControl.ImageIndex; }
            set { GetPropertyByName("ImageIndex").SetValue(linkControl, value); }
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

            items.Add(new DesignerActionPropertyItem("Caption", "Caption :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("BackGroundColor", "BackGroundColor :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ToolUpFont", "ToolUpFont :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ToolDownFont", "ToolDownFont :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("TextAlign", "TextAlign :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ImageList", "ImageList :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ImageAlign", "ImageAlign :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("ImageIndex", "ImageIndex :", "Misc", ""));
            items.Add(new DesignerActionPropertyItem("Disabled", "Disabled", "Misc", ""));

            return items;
        }
    }

    public class NewToolBtnDesing : ParentControlDesigner
    {
        private DesignerActionListCollection actionLists;

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (actionLists == null)
                {
                    actionLists = new DesignerActionListCollection();
                    actionLists.Add(new NewToolBtnActionList((NewToolBtn)Control));
                }
                return actionLists;
            }
        }
    }
}
