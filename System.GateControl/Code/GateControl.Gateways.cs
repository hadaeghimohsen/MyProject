using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.GateControl.Code
{
   partial class GateControl
   {
      public IRouter _DefaultGateway { get; set; }
      public IRouter _Commons { get; set; }
   }
}
