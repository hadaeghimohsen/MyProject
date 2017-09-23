using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Routering;

namespace System.Gas.Self.Code
{
   public partial class Gas
   {
      public Gas(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _Gnrt_Qrcd = new Gnrt_Qrcd.Code.Gnrt_Qrcd(_commons, _wall) { _DefaultGateway = this };
      }
   }
}
