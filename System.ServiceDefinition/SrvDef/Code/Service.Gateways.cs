using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ServiceDefinition.SrvDef.Code
{
   partial class Service
   {
      public IRouter _DefaultGateway { get; set; }

      public IRouter _Commons { get; set; }

      private UnderGateways.SrvUnitType.Code.UnitType _UnitType { get; set; }
   }
}
