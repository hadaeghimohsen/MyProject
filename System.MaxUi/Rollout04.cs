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
   [Designer(typeof(Rollout04Desing))]

   public partial class Rollout04 : UserControl
   {
      public Rollout04()
      {
         InitializeComponent();
         //Run();
      }

      #region Private
      int h = 215;//, w = 304;
      //Color nbackcolor = Color.DarkGray, nforecolor = Color.Black;
      //Color ebackcolor = Color.FromArgb(197, 197, 197), eforecolor = Color.Black;
      bool status = true;
      //Rollout04 obj;
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
            if(value != null)
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
               status = value;
               RolloutStatus();
               if (panelPage != null) panelPage.Focus();
            }
         }
      }

      public string Caption
      {
         get { return Rollout.name.Text; }
         set { Rollout.name.Text = value; }
      }

      public Font FontRollout
      {
         get { return Rollout.name.Font; }
         set { Rollout.name.Font = value; }
      }

      public int HeightSize
      {
         get { return h; }
         set { this.Height = h = value; }
      }

      //public Rollout04 Chain
      //{
      //   get { return obj; }
      //   set { obj = value; }
      //}

      #endregion

      #region Method
      //private void Rollout_MouseEnter(object sender, EventArgs e)
      //{
      //    plus_min.BackColor = Rollout.BackColor = ebackcolor;
      //    plus_min.ForeColor = Rollout.ForeColor = eforecolor;
      //}

      //private void Rollout_MouseLeave(object sender, EventArgs e)
      //{
      //    plus_min.BackColor = Rollout.BackColor = nbackcolor;
      //    plus_min.ForeColor = Rollout.ForeColor = nforecolor;
      //}

      public void Close()
      {
         plus_min.Text = "+";
         this.Height = 18;
         int mov = HeightSize;
         Rollout04 r = this;
         status = false;
         //while (true)
         //{
         //   Rollout04 t = r.Chain;
         //   if (r.Chain == null)
         //      break;
         //   t.Top = t.Top - mov + 20;
         //   r = t;
         //}
      }

      public void Open()
      {
         plus_min.Text = "‒";
         this.Height = HeightSize;
         int mov = HeightSize;
         //Rollout04 r = this;
         status = true;
         //while (true)
         //{
         //   Rollout04 t = r.Chain;
         //   if (r.Chain == null)
         //      break;
         //   t.Top = t.Top + mov - 20;
         //   r = t;
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
            obj.Top = obj.Left = 3;
            obj.Width = this.Width - 6;//298;
            obj.Height = this.Height - 6;//209;
            obj.BorderStyle = BorderStyle.FixedSingle;
            obj.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)));
            obj.DockPadding.All = 5;
            obj.DockPadding.Top = 15;
         }
         catch (Exception)
         {
            //MessageBox.Show("شما بایستی یک پنل به کنترل اضافه کنید!");
         }
         //obj.Bounds = new Rectangle(5,20,Rollout_Panel.Width ,Rollout_Panel.Height );
      }

      //public void Run()
      //{
      //    //plus_min.BackColor = Color.FromArgb(197, 197, 197);
      //    plus_min.ForeColor = Color.Black;
      //    //this.Width = w;
      //    //this.Height = h;
      //    Rollout.name.MouseUp += new MouseEventHandler(Rollout_MouseUp);
      //    RolloutStatus();
      //}

      private void Rollout_MouseUp(object sender, MouseEventArgs e)
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

      private void Rollout04_Resize(object sender, EventArgs e)
      {
         try
         {
            if (this.Height != 18)
               HeightSize = this.Height;

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

   public class Rollout04ActionList : DesignerActionList
   {
      private Rollout04 linkControl;
      public Rollout04ActionList(Rollout04 ctrl)
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

      public string Caption
      {
         get { return linkControl.Caption; }
         set { GetPropertyByName("Caption").SetValue(linkControl, value); }
      }

      public Font FontRollout
      {
         get { return linkControl.Rollout.Font; }
         set { GetPropertyByName("FontRollout").SetValue(linkControl, value); }
      }

      //public Rollout04 Chain
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
         items.Add(new DesignerActionPropertyItem("PanelPage", "Panel :", "Misc", ""));


         return items;
      }
   }

   public class Rollout04Desing : ParentControlDesigner
   {
      private DesignerActionListCollection actionLists;

      public override DesignerActionListCollection ActionLists
      {
         get
         {
            if (actionLists == null)
            {
               actionLists = new DesignerActionListCollection();
               actionLists.Add(new Rollout04ActionList((Rollout04)Control));
            }
            return actionLists;
         }
      }
   }

}
