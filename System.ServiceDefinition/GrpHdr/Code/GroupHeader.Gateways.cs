using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.GrpHdr.Code
{
   partial class GroupHeader : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }

      public IRouter _Commons { get; set; }
      
   }
}
