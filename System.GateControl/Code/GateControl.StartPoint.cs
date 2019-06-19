using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.GateControl.Code
{
   public partial class GateControl
   {
      public GateControl(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
