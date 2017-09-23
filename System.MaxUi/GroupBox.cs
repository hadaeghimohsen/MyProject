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
   [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
   [Designer(typeof(GroupBoxDesing))]
   public partial class GroupBox : UserControl
   {
      public GroupBox()
      {
         InitializeComponent();
         Caption = Caption;
      }

      const int m = 10;
      public enum StyleBorder { Windows98, Max };
      StyleBorder style = StyleBorder.Windows98;
      bool fullHeader = false;

      public StyleBorder Style
      {
         get { return style; }
         set
         {
            style = value;
            switch (style)
            {
               case StyleBorder.Windows98:
                  top.BorderStyle = left.BorderStyle = down.BorderStyle = right.BorderStyle = BorderStyle.Fixed3D;
                  break;
               case StyleBorder.Max:
                  top.BorderStyle = left.BorderStyle = down.BorderStyle = right.BorderStyle = BorderStyle.None;
                  break;
               default:
                  break;
            }
         }
      }

      public bool FullHeader
      {
         get
         {
            return fullHeader;
         }
         set
         {
            fullHeader = value;
            switch (fullHeader)
            {
               case true:
                  title.BorderStyle = Windows.Forms.BorderStyle.FixedSingle;
                  title.BackColor = Color.FromArgb(170, 170, 170);
                  title.AutoSize = false;
                  title.Height = 20;
                  title.Dock = DockStyle.Top;
                  title.TextAlign = ContentAlignment.MiddleCenter;
                  break;
               case false:
                  title.BorderStyle = Windows.Forms.BorderStyle.None;
                  title.BackColor = BackColor;
                  title.Dock = DockStyle.None;
                  title.AutoSize = true;
                  GroupBox_RightToLeftChanged(null, null);
                  break;
            }
         }
      }
      public string Caption
      {
         get { return title.Text; }
         set
         {
            title.Text = value;
            if (this.RightToLeft == RightToLeft.Yes)
               title.Left = this.Width - m - title.Width;
            else
               title.Left = m;
         }
      }

      public Font CaptionFont
      {
         get
         {
            return title.Font;
         }
         set
         {            
            title.Font = value;
         }
      }

      public override Color BackColor
      {
         get
         {
            return base.BackColor;
         }
         set
         {
            base.BackColor = value;
            title.BackColor = value;
         }
      }

      private void GroupBox_SizeChanged(object sender, EventArgs e)
      {
         right.Left = this.Width - 1;
         right.Height = this.Height - 11;
         //right.Refresh();

         left.Height = this.Height - 11;
         //left.Refresh();

         top.Width = this.Width - 1;
         //top.Refresh();

         down.Width = this.Width - 1;
         down.Top = this.Height - 1;
         //down.Refresh();

         title.Left = this.Width - m - title.Width;
      }

      private void GroupBox_RightToLeftChanged(object sender, EventArgs e)
      {
         if (this.RightToLeft == RightToLeft.Yes)
         {
            title.Left = this.Width - m - title.Width;
            title.RightToLeft = RightToLeft.Yes;
         }
         else
         {
            title.Left = m;
            title.RightToLeft = RightToLeft.No;
         }
      }
   }

   public class GroupBoxActionList : DesignerActionList
   {
      private GroupBox linkControl;
      public GroupBoxActionList(GroupBox ctrl)
         : base(ctrl)
      {
         linkControl = ctrl;
      }

      public string Caption
      {
         get { return linkControl.Caption; }
         set { GetPropertyByName("Caption").SetValue(linkControl, value); }
      }
      public Font CaptionFont
      {
         get { return linkControl.CaptionFont; }
         set { GetPropertyByName("CaptionFont").SetValue(linkControl, value); }
      }
      public GroupBox.StyleBorder Style
      {
         get { return linkControl.Style; }
         set { GetPropertyByName("Style").SetValue(linkControl, value); }
      }
      public bool FullHeader
      {
         get { return linkControl.FullHeader; }
         set { GetPropertyByName("FullHeader").SetValue(linkControl, value); }
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
         items.Add(new DesignerActionPropertyItem("CaptionFont", "Caption Font :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Style", "Style :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("FullHeader", "Full Header", "Misc", ""));

         return items;
      }
   }

   public class GroupBoxDesing : ParentControlDesigner
   {
      private DesignerActionListCollection actionLists;

      public override DesignerActionListCollection ActionLists
      {
         get
         {
            if (actionLists == null)
            {
               actionLists = new DesignerActionListCollection();
               actionLists.Add(new GroupBoxActionList((GroupBox)Control));
            }
            return actionLists;
         }
      }
   }
}
