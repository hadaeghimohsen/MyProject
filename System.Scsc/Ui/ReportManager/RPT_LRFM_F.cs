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

namespace System.Scsc.Ui.ReportManager
{
   public partial class RPT_LRFM_F : UserControl
   {
      public RPT_LRFM_F()
      {
         InitializeComponent();
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         MdrpBs1.DataSource = iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002");
      }
      private void mb_reloading_Click(object sender, EventArgs e)
      {
         Execute_Query();         
      }

      private void mb_CfgStngF_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", ModualName), new XAttribute("section", SectionName))}
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void mb_SelectReport_Click(object sender, EventArgs e)
      {
         var crnt = MdrpBs1.Current as Data.Modual_Report;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "RPT_LRFM_F", 04 /* Execute UnPaint */, SendType.SelfToUserInterface)            
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "RPT_MNGR_F", 07 /* Execute LoadData */, SendType.SelfToUserInterface)
            {
               Input = new List<object>
               {
                  crnt, 
                  WhereClause
               }
            }
         );
      }

      private void mb_back_Click(object sender, EventArgs e)
      {
         //_DefaultGateway.Gateway(new Job(SendType.External, "Localhost", GetType().Name, 04 /* Execute Un_Paint */, SendType.SelfToUserInterface) );
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
