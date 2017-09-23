using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Gas.Self.Ui
{
   partial class STRT_MTRO_M
   {
      partial void Start_Menu_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "strt_mbar_m"},
                  new Job(SendType.SelfToUserInterface, "STRT_MBAR_M", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "STRT_MBAR_M", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous}
               }));
      }

      partial void RPRT_PBLC_M_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Gnrt_Qrcd", 02 /* DoWork4RPRT_PBLC_F */, SendType.Self)
            );
      }

      partial void RPRT_PRVT_M_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Gnrt_Qrcd", 03 /* Execute DoWork4RPRT_PRVT_F */, SendType.Self)
            );
      }
   }
}
