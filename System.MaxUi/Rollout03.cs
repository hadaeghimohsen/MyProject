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
   [Designer(typeof(Rollout03Desing))]
   [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
   public partial class Rollout03 : UserControl
   {
      public Rollout03()
      {
         InitializeComponent();
      }

      #region Private
      int h = 215;//, w = 304;
      //color(180,180,180)
      Color nbackcolor = Color.FromArgb(180, 180, 180), nforecolor = Color.Black;
      Color ebackcolor = Color.FromArgb(180, 180, 180), eforecolor = Color.Black;
      bool status = true;
      //Rollout03 obj;
      Panel panelPage;
      #endregion

      #region Encapsulation
      public Panel PanelPage
      {
         get { return panelPage; }
         set
         {
            panelPage = value;
            AddControl(panelPage);
            if (value != null)
            {
               panelPage.Paint += PanelChangeBorder;
               panelPage.Resize += PanelChangeSize;
            }
         }
      }

      public bool Status
      {
         get { return status; }
         set
         {
            if (status != value)
            {
               //MessageBox.Show("hi");
               status = value;
               RolloutStatus();
               if (panelPage != null) panelPage.Focus();
            }
         }
      }

      public Color Eforecolor
      {
         get { return eforecolor; }
         set { eforecolor = value; }
      }

      public Color Ebackcolor
      {
         get { return ebackcolor; }
         set { ebackcolor = value; }
      }

      public Color Nforecolor
      {
         get { return nforecolor; }
         set { nforecolor = value; }
      }

      public Color Nbackcolor
      {
         get { return nbackcolor; }
         set { nbackcolor = value; }
      }

      public string Caption
      {
         get { return Rollout.Text; }
         set { Rollout.Text = value; }
      }

      public Font FontRollout
      {
         get { return Rollout.Font; }
         set { Rollout.Font = value; }
      }

      public int HeightSize
      {
         get { return h; }
         set { this.Height = h = value; }
      }

      //public Rollout03 Chain
      //{
      //   get { return obj; }
      //   set { obj = value; }
      //}

      public ContentAlignment TextAlign
      {
         get { return Rollout.TextAlign; }
         set { Rollout.TextAlign = value; }
      }
      #endregion

      #region Method
      private void Rollout_MouseEnter(object sender, EventArgs e)
      {
         plus_min.BackColor = Rollout.BackColor = ebackcolor;
         plus_min.ForeColor = Rollout.ForeColor = eforecolor;
      }

      private void Rollout_MouseLeave(object sender, EventArgs e)
      {
         plus_min.BackColor = Rollout.BackColor = nbackcolor;
         plus_min.ForeColor = Rollout.ForeColor = nforecolor;
      }

      public void Close()
      {
         plus_min.Text = "+";
         this.Height = 18;
         int mov = HeightSize;
         status = false;
         //Rollout03 r = this;
         ////if (Status != false)
         //{
         
         //   Rollout03 m = r;
         //   while (true)
         //   {
         //      Rollout03 t = r.Chain;

         //      if (r.Chain == null)
         //         break;
         //      int offset = t.Top - mov + 20;
         //      if (offset <= m.Top)
         //         t.Top = m.Top + m.Height + 3;
         //      else
         //         t.Top = offset;
         //      r = t;
         //      m = r;
         //   }
         //}
      }

      public void Open()
      {
         plus_min.Text = "‒";
         this.Height = HeightSize;
         int mov = HeightSize;
         status = true;
         //Rollout03 r = this;
         ////if (Status != true)
         //{         
         //   while (true)
         //   {
         //      Rollout03 t = r.Chain;
         //      if (r.Chain == null)
         //         break;
         //      t.Top = t.Top + mov - 20;
         //      r = t;
         //   }
         //}
      }

      private void RolloutStatus()
      {
         if (Status)
            Open();
         else
            Close();
      }

      public void AddControl(Panel obj)
      {
         try
         {
            this.Controls.Add(obj);
            obj.Top = 3; obj.Left = 3;
            obj.Width = this.Width - 6;//298;
            obj.Height = this.Height - 6;//209;
            //304, 215
            obj.BorderStyle = BorderStyle.FixedSingle;
            obj.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            obj.DockPadding.All = 5;
            obj.DockPadding.Top = 15;
         }
         catch (Exception)
         {
         }
      }

      private void Rollout_MouseDown(object sender, MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            if (this.Height == 18)
            {
               Open();
            }
            else
            {
               Close();
            }
         }
      }
      private void Rollout03_Resize(object sender, EventArgs e)
      {
         try
         {
            if (this.Height != 18)
               HeightSize = this.Height;
            //WidthSize = this.Width;
            PanelPage.Width = this.Width - 6;
            PanelPage.Height = this.Height - 6;
            Rollout.Width = this.Width - 14;
            plus_min.Left = this.Width - 29;
         }
         catch (Exception)
         {
         }
      }

      private void PanelChangeBorder(object sender, PaintEventArgs e)
      {
         //ControlPaint.DrawBorder(e.Graphics, panelPage., Color.FromArgb(128,128,128), ButtonBorderStyle.Solid);
         base.OnPaint(e);

         int borderWidth = 1;
         ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
            Color.White, borderWidth, ButtonBorderStyle.Solid,
            Color.White, borderWidth, ButtonBorderStyle.Solid,
            Color.White, borderWidth, ButtonBorderStyle.Inset,
            Color.White, borderWidth, ButtonBorderStyle.Inset);
      }
      private void PanelChangeSize(object sender, EventArgs e)
      {
         base.OnResize(e);
         Invalidate();
      }
      #endregion
   }

   public class Rollout03ActionList : DesignerActionList
   {
      private Rollout03 linkControl;
      public Rollout03ActionList(Rollout03 ctrl)
         : base(ctrl)
      {
         linkControl = ctrl;
      }

      public bool Status
      {
         get { return linkControl.Status; }
         set { GetPropertyByName("Status").SetValue(linkControl, value); }
      }

      public Panel PanelPage
      {
         get { return linkControl.PanelPage; }
         set { GetPropertyByName("PanelPage").SetValue(linkControl, value); }
      }

      public Color Eforecolor
      {
         get { return linkControl.Eforecolor; }
         set { GetPropertyByName("Eforecolor").SetValue(linkControl, value); }
      }

      public Color Ebackcolor
      {
         get { return linkControl.Ebackcolor; }
         set { GetPropertyByName("Ebackcolor").SetValue(linkControl, value); }
      }

      public Color Nforecolor
      {
         get { return linkControl.Nforecolor; }
         set { GetPropertyByName("Nforecolor").SetValue(linkControl, value); }
      }

      public Color Nbackcolor
      {
         get { return linkControl.Nbackcolor; }
         set { GetPropertyByName("Nbackcolor").SetValue(linkControl, value); }
      }

      public string Caption
      {
         get { return linkControl.Rollout.Text; }
         set { GetPropertyByName("Caption").SetValue(linkControl, value); }
      }

      public Font FontRollout
      {
         get { return linkControl.Rollout.Font; }
         set { GetPropertyByName("FontRollout").SetValue(linkControl, value); }
      }

      //public Rollout03 Chain
      //{
      //   get { return linkControl.Chain; }
      //   set { GetPropertyByName("Chain").SetValue(linkControl, value); }
      //}

      public ContentAlignment TextAlign
      {
         get { return linkControl.Rollout.TextAlign; }
         set { GetPropertyByName("TextAlign").SetValue(linkControl, value); }
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

         items.Add(new DesignerActionPropertyItem("Status", "Status Rollout", "Misc", ""));
         //items.Add(new DesignerActionPropertyItem("Chain", "Chain :", "Misc", ""));

         items.Add(new DesignerActionPropertyItem("Caption", "Caption :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("TextAlign", "Text Align :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("FontRollout", "Font Rollout :", "Misc", ""));

         items.Add(new DesignerActionPropertyItem("Eforecolor", "Enter ForeColor :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Ebackcolor", "Enter BackColor :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Nforecolor", "Normal ForeColor :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("Nbackcolor", "Normal BackColor :", "Misc", ""));
         items.Add(new DesignerActionPropertyItem("PanelPage", "Panel :", "Misc", ""));


         return items;
      }
   }

   public class Rollout03Desing : ParentControlDesigner
   {
      private DesignerActionListCollection actionLists;

      public override DesignerActionListCollection ActionLists
      {
         get
         {
            if (actionLists == null)
            {
               actionLists = new DesignerActionListCollection();
               actionLists.Add(new Rollout03ActionList((Rollout03)Control));
            }
            return actionLists;
         }
      }
   }
}