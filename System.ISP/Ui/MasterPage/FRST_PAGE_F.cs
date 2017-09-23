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

namespace System.ISP.Ui.MasterPage
{
   public partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();
      }

      #region basedefinition
      private void rb_regndfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }

      private void rb_epitdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 04 /* Execute Epit_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }

      private void rb_btrfdifn_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 05 /* Execute Btrf_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }

      private void rb_cashdfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 06 /* Execute Cash_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }

      private void rb_regldfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 07 /* Execute Regl_Dfin_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }

      private void rb_crgldfin_Click(object sender, EventArgs e)
      {
         var crntregl = iISP.Regulations.Where(rg => rg.REGL_STAT == "002" && rg.TYPE == "001");
         if(crntregl == null) return;
         if(crntregl.Count() > 1) return;

         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 08 /* Execute Rqrq_Dfin_F */){Input = crntregl.First()},
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }

      private void rb_orgndfin_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 10 /* Execute Orgn_Dfin_F */),
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }
      #endregion

      private void rb_adslat_Click(object sender, EventArgs e)
      {
         Job _InteractWithISP =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Adm_Adsl_F */),                
              });
         _DefaultGateway.Gateway(_InteractWithISP);
      }
   }
}
