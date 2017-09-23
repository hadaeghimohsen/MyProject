using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Gas.Gnrt_Qrcd.Code
{
   public partial class Gnrt_Qrcd
   {
      public Gnrt_Qrcd(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
