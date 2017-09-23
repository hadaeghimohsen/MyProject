using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reporting.WorkFlow.Ui
{
   partial class WHR_SCON_F
   {
      partial void bp_flow_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Caption)
         {
            case "1":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 04 /* Execute UnPaint */),
                        new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 04 /* Execute UnPaint */)
                     }));
               break;
            case "":
            case "2":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", "WHR_SCON_F", 04 /* Execute UnPaint */, SendType.SelfToUserInterface));
               break;
            case "3":
               ShowPreview();
               break;
         }
      }
   }
}
