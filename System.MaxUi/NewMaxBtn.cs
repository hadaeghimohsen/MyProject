using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace System.MaxUi
{
   [DefaultBindingProperty("Caption")]
   //[Designer(typeof(NewMaxBtnDesing))]
   [DefaultEvent("Click")]
   public partial class NewMaxBtn : UserControl
   {
      public NewMaxBtn()
      {
         InitializeComponent();
      }

      Color backGroundColor = Color.FromArgb(238, 204, 85);
      Color enterColor = Color.Transparent;
      bool disabled = false;
      Color textColor = Color.Black;
      bool inout = false;
      bool inToBold = false;

      public bool InToBold
      {
         get { return inToBold; }
         set { inToBold = value; }
      }
      public string Caption
      {
         get { return name.Text; }
         set
         {
            //if(Disabled == false)
            name.Text = value;
         }
      }
      public Color BackGroundColor
      {
         get { return backGroundColor; }
         set { backGroundColor = value; }
      }
      public Color EnterColor
      {
         get { return enterColor; }
         set { enterColor = value; }
      }
      public ImageList ImageList
      {
         get { return name.ImageList; }
         set
         {
            //if (Disabled == false)
            name.ImageList = value;
         }
      }
      public int ImageIndex
      {
         get { return name.ImageIndex; }
         set { name.ImageIndex = value; }
      }
      public ContentAlignment ImageAlign
      {
         get { return name.ImageAlign; }
         set { name.ImageAlign = value; }
      }
      public Font TextFont
      {
         get { return name.Font; }
         set { name.Font = value; }
      }
      public Color TextColor
      {
         get { return textColor; }
         set
         {
            textColor = value;
            Disabled = Disabled;
         }
      }
      public ContentAlignment TextAlign
      {
         get { return name.TextAlign; }
         set { name.TextAlign = value; }
      }
      public bool Disabled
      {
         get { return disabled; }
         set
         {
            disabled = value;
            if (disabled == false)
            {

               name.ForeColor = TextColor;
               top.BackColor = Color.FromArgb(225, 225, 225);
               left.BackColor = Color.FromArgb(225, 225, 225);
               down.BackColor = Color.FromArgb(130, 130, 130);
               right.BackColor = Color.FromArgb(130, 130, 130);
               TabStop = true;
            }
            else
            {
               name.ForeColor = SystemColors.GrayText;
               top.BackColor = down.BackColor = left.BackColor = right.BackColor = SystemColors.GrayText;
               TabStop = false;
            }
         }
      }

      private void name_MouseDown(object sender, MouseEventArgs e)
      {
         if (Disabled == false)
         {
            if (e.Button == MouseButtons.Left)
            {
               ClickDown();
            }
            base.OnMouseDown(e);
         }
      }

      private void ClickDown()
      {
         name.BackColor = BackGroundColor;

         top.BackColor = Color.FromArgb(130, 130, 130);
         left.BackColor = Color.FromArgb(130, 130, 130);
         down.BackColor = Color.FromArgb(225, 225, 225);
         right.BackColor = Color.FromArgb(225, 225, 225);
         inout = true;
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
         name.BackColor = EnterColor;//Color.Transparent;//Color.FromArgb(197, 197, 197);

         top.BackColor = Color.FromArgb(225, 225, 225);
         left.BackColor = Color.FromArgb(225, 225, 225);
         down.BackColor = Color.FromArgb(130, 130, 130);
         right.BackColor = Color.FromArgb(130, 130, 130);
         inout = false;
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

      private void NewMaxBtn_SizeChanged(object sender, EventArgs e)
      {
         base.OnResize(e);
         down.Width = top.Width = this.Width - 1;
         down.Top = this.Height - 1;

         left.Height = right.Height = this.Height - 1;
         right.Left = this.Width - 1;

         name.Height = this.Height - 4;
         name.Width = this.Width - 4;
      }

      private void NewMaxBtn_Enter(object sender, EventArgs e)
      {
         if (!Disabled)
         {
            if (!inout)
               name.BackColor = EnterColor;
            else
               name.BackColor = BackGroundColor;
            if (inToBold)
               name.Font = new Font(name.Font.Name, name.Font.Size, FontStyle.Bold, GraphicsUnit.Point, name.Font.GdiCharSet);
         }
      }

      private void NewMaxBtn_Leave(object sender, EventArgs e)
      {
         EnterColor = name.BackColor;
         name.BackColor = Color.Transparent;
         if (inToBold)
            name.Font = new Font(name.Font.Name, name.Font.Size, FontStyle.Regular, GraphicsUnit.Point, name.Font.GdiCharSet);
      }

      private void NewMaxBtn_KeyDown(object sender, KeyEventArgs e)
      {
         if (!Disabled)
            if (e.KeyCode == Keys.Enter)
            {
               ClickDown();
            }
      }

      private void NewMaxBtn_KeyUp(object sender, KeyEventArgs e)
      {
         if (!Disabled)
            if (e.KeyCode == Keys.Enter)
            {
               ClickUp();
               base.OnClick(e);
            }
      }

      private void name_MouseEnter(object sender, EventArgs e)
      {
         base.OnMouseEnter(e);
      }

   }

   public class NewMaxBtnActionList : DesignerActionList
   {
      private NewMaxBtn linkControl;
      public NewMaxBtnActionList(NewMaxBtn ctrl)
         : base(ctrl)
      {
         linkControl = ctrl;
      }

      public string Caption
      {
         get { return linkControl.Caption; }
         set { GetPropertyByName("Caption").SetValue(linkControl, value); }
      }
      public Color BackGroundColor
      {
         get { return linkControl.BackGroundColor; }
         set { GetPropertyByName("BackGroundColor").SetValue(linkControl, value); }
      }
      public Color EnterColor
      {
         get { return linkControl.EnterColor; }
         set { GetPropertyByName("EnterColor").SetValue(linkControl, value); }
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
      public Font TextFont
      {
         get { return linkControl.TextFont; }
         set { GetPropertyByName("TextFont").SetValue(linkControl, value); }
      }
      public Color TextColor
      {
         get { return linkControl.TextColor; }
         set { GetPropertyByName("TextColor").SetValue(linkControl, value); }
      }
      public ContentAlignment TextAlign
      {
         get { return linkControl.TextAlign; }
         set { GetPropertyByName("TextAlign").SetValue(linkControl, value); }
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
         items.Add(new DesignerActionPropertyItem("EnterColor", "EnterColor :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("TextColor", "TextColor :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("TextFont", "TextFont :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("TextAlign", "TextAlign :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("ImageList", "ImageList :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("ImageAlign", "ImageAlign :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("ImageIndex", "ImageIndex :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Disabled", "Disabled", "Misc", ""));

         return items;
      }
   }

   public class NewMaxBtnDesing : ParentControlDesigner
   {
      private DesignerActionListCollection actionLists;

      public override DesignerActionListCollection ActionLists
      {
         get
         {
            if (actionLists == null)
            {
               actionLists = new DesignerActionListCollection();
               actionLists.Add(new NewMaxBtnActionList((NewMaxBtn)Control));
            }
            return actionLists;
         }
      }
   }
}
