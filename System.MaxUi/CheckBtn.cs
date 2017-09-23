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
   //[Designer(typeof(CheckBtnDesing))]
   [DefaultEvent("StatusChange")]
   public partial class CheckBtn : UserControl
   {
      public CheckBtn()
      {
         InitializeComponent();
      }

      bool status;
      bool disabled = false;

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
      
      public Font CaptionFont
      {
         get
         {
            return Ltext.Font;
         }
         set
         {            
            Ltext.Font = value;
         }
      }

      private void LImage_MouseDown(object sender, MouseEventArgs e)
      {
         if (!Disabled)
         {
            if (e.Button == MouseButtons.Left)
            {
               if (Status == false)
                  Limage.ImageIndex = 2;
               else
                  Limage.ImageIndex = 5;
               Limage.Refresh();
            }
         }
      }
      private void LImage_MouseUp(object sender, MouseEventArgs e)
      {
         if (!Disabled)
         {
            if (e.Button == MouseButtons.Left)
            {
               Point p = new Point(e.X, e.Y);
               Label l = (Label)GetChildAtPoint(p);
               if (l != null)
               {
                  Status = !Status;
               }
               else
               {
                  if (Status)
                     Limage.ImageIndex = 3;
                  else
                     Limage.ImageIndex = 0;
                  Limage.Refresh();
               }
            }
         }
      }

      public delegate void StatusValueHandel(object sender);
      public event StatusValueHandel StatusChange;

   }

   public class CheckBtnActionList : DesignerActionList
   {
      private CheckBtn linkControl;
      public CheckBtnActionList(CheckBtn ctrl)
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

         items.Add(new DesignerActionPropertyItem("Caption", "Caption", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Status", "Status", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Disabled", "Disabled", "Misc", ""));


         return items;
      }
   }

   public class CheckBtnDesing : ParentControlDesigner
   {
      private DesignerActionListCollection actionLists;

      public override DesignerActionListCollection ActionLists
      {
         get
         {
            if (actionLists == null)
            {
               actionLists = new DesignerActionListCollection();
               actionLists.Add(new CheckBtnActionList((CheckBtn)Control));
            }
            return actionLists;
         }
      }
   }
}
