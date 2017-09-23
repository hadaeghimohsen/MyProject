using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.SrvDef.UnderGateways.SrvUnitType.Code
{
   partial class UnitType
   {
      internal ISendRequest _Wall { get; set; }

      private Ui.UnitTypeMenus _ServiceUnitTypeMenus { get; set; }
   }
}
