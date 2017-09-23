using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Gas.Self.Ui
{
   partial class STRT_MBAR_M
   {
      partial void wbp_book_mark_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Tag.ToString())
         {
            case "0":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "STRT_MBAR_M", 06 /* Execute Close */, SendType.SelfToUserInterface));
               break;
            default:
               break;
         }
      }
   }
}
