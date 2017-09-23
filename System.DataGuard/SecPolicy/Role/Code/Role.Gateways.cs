using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.DataGuard.SecPolicy.Role.Code
{
   partial class Role : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }

      internal IRouter _Commons { get; set; }      
   }
}
