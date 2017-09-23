using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Gas.Gnrt_Qrcd.Code
{
   partial class Gnrt_Qrcd
   {
      internal ISendRequest _Wall { get; set; }

      private Ui.RPRT_PBLC_F _RPRT_PBLC_F { get; set; }
      private Ui.RPRT_PRVT_F _RPRT_PRVT_F { get; set; }
   }
}
