using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Reporting.WorkFlow.Ui
{
   partial class RPT_SRPT_F
   {
      partial void bp_flow_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Caption)
         {
            case "":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 04 /* Execute UnPaint */)
                     }));
               break;
         }
      }

      partial void Tile_Ctrl_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button != Windows.Forms.MouseButtons.Right)
            return;

         Tile_Ctrl.ItemCheckMode = TileItemCheckMode.Multiple;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "RPT_MBAR_M"},
                  new Job(SendType.SelfToUserInterface, "RPT_MBAR_M", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "RPT_MBAR_M", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous}
               }));
      }

      
       
   }
}
