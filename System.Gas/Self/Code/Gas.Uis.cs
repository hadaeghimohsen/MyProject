using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Routering;

namespace System.Gas.Self.Code
{
   partial class Gas
   {
      public ISendRequest _Wall { get; set; }

      private Ui.STRT_MTRO_M _STRT_MTRO_M { get; set; }
      private Ui.STRT_MBAR_M _STRT_MBAR_M { get; set; }
   }
}
