using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.CRM.Ui.HistoryAction
{
   public partial class HST_FLTR_F : UserControl
   {
      public HST_FLTR_F()
      {
         InitializeComponent();

         var path = new System.Drawing.Drawing2D.GraphicsPath();
         path.AddEllipse(0, 0, Lb_TagFilter.Width, Lb_TagFilter.Height);

         this.Lb_FilterCount.Region = this.Lb_TagFilter.Region = this.Lb_RegionFilter.Region = this.Lb_ExtraInfoFilter.Region = this.Lb_ContactInfoFilter.Region = new Region(path);
      }

      private bool requery = false;
      private string formCaller = "";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Tag_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>{
                  new Job(SendType.Self, 76 /* Execute Hst_Utag_F */),
                  new Job(SendType.SelfToUserInterface, "HST_UTAG_F", 10 /* Execute Hst_Utag_F */)
                  {
                     Input = 
                        new XElement("Filter",
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void Location_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>{
                  new Job(SendType.Self, 77 /* Execute Hst_Urgn_F */),
                  new Job(SendType.SelfToUserInterface, "HST_URGN_F", 10 /* Execute Hst_Urgn_F */)
                  {                     
                     Input = 
                        new XElement("Filter",
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void ExtraInfo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>{
                  new Job(SendType.Self, 80 /* Execute Hst_Uexf_F */),
                  new Job(SendType.SelfToUserInterface, "HST_UEXF_F", 10 /* Execute Hst_Urgn_F */)
                  {                     
                     Input = 
                        new XElement("Filter",
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void ContactInfo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>{
                  new Job(SendType.Self, 81 /* Execute Hst_Uctf_F */),
                  new Job(SendType.SelfToUserInterface, "HST_UCTF_F", 10 /* Execute Hst_Urgn_F */)
                  {                     
                     Input = 
                        new XElement("Filter",
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void Lb_FilterCount_Click(object sender, EventArgs e)
      {
         Lb_TagFilter_Click(null, null);
         Lb_RegionFilter_Click(null, null);
         Lb_ExtraInfoFilter_Click(null, null);
         Lb_ContactInfoFilter_Click(null, null);
      }

      private void Lb_TagFilter_Click(object sender, EventArgs e)
      {
         Tag_Butn.Tag = null;
         Lb_TagFilter.Visible = false;
         SetFilterCountLabel();
      }

      private void Lb_RegionFilter_Click(object sender, EventArgs e)
      {
         Region_Butn.Tag = null;
         Lb_RegionFilter.Visible = false;
         SetFilterCountLabel();
      }

      private void Lb_ExtraInfoFilter_Click(object sender, EventArgs e)
      {
         ExtraInfo_Butn.Tag = null;
         Lb_ExtraInfoFilter.Visible = false;
         SetFilterCountLabel();
      }

      private void Lb_ContactInfoFilter_Click(object sender, EventArgs e)
      {
         ContactInfo_Butn.Tag = null;
         Lb_ContactInfoFilter.Visible = false;
         SetFilterCountLabel();
      }

      private void SetFilterCountLabel()
      {
         var count = 0;
         var Qxml = new XElement("Filtering");
         if (Tag_Butn.Tag != null)
         {
            count = Convert.ToInt32((Tag_Butn.Tag as XElement).Attribute("cont").Value);
            Qxml.Add(Tag_Butn.Tag as XElement);
         }
         if (Region_Butn.Tag != null)
         {
            count += Convert.ToInt32((Region_Butn.Tag as XElement).Attribute("cont").Value);
            Qxml.Add(Region_Butn.Tag as XElement);
         }
         if (ExtraInfo_Butn.Tag != null)
         {
            count += Convert.ToInt32((ExtraInfo_Butn.Tag as XElement).Attribute("cont").Value);
            Qxml.Add(ExtraInfo_Butn.Tag as XElement);
         }
         if (ContactInfo_Butn.Tag != null)
         {
            count += Convert.ToInt32((ContactInfo_Butn.Tag as XElement).Attribute("cont").Value);
            Qxml.Add(ContactInfo_Butn.Tag as XElement);
         }

         if (count != 0)
         {
            Lb_FilterCount.Visible = true;
            ApproveFilter_Butn.Tag = Qxml;
         }
         else
         {
            Lb_FilterCount.Visible = false;
            ApproveFilter_Butn.Tag = null;
         }
         Lb_FilterCount.Text = count.ToString();
      }

      private void ApproveFilter_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, formCaller, 100 /* Execute SetFilterOnQuery */ )
                  {
                     Input = ApproveFilter_Butn.Tag
                  },
                  new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape}
               }
            )
         );
      }

      private void Lb_FilterCount_MouseEnter(object sender, EventArgs e)
      {
         var lb = sender as Label;

         lb.Tag = lb.Text;
         lb.Text = "x";
      }

      private void Lb_FilterCount_MouseLeave(object sender, EventArgs e)
      {
         var lb = sender as Label;

         lb.Text = lb.Tag.ToString();
         lb.Tag = null;
      }
   }
}
