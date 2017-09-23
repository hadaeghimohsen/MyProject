using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.ServiceDefinition.SrvDef.Code
{
   internal partial class Service
   {
      internal Service(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _UnitType = new UnderGateways.SrvUnitType.Code.UnitType(_commons, _wall) { _DefaultGateway = this };
      }
   }
}
