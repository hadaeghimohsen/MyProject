using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Routering;

namespace System.Gas.Self.Code
{
   partial class Gas
   {
      public IRouter _DefaultGateway { get; set; }
      public IRouter _Commons { get; set; }

      private Gnrt_Qrcd.Code.Gnrt_Qrcd _Gnrt_Qrcd { get; set; }
   }
}
