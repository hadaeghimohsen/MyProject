using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.Share.Code
{
   partial class Services
   {
      public IRouter _DefaultGateway { get; set; }

      public IRouter _Commons { get; set; }

      private SrvDef.Code.Service _Service { get; set; }
      private GrpHdr.Code.GroupHeader _GroupHeader { get; set; }
   }
}
