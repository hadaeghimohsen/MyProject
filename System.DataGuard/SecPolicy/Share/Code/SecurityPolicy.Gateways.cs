using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataGuard.SecPolicy.Share.Code
{
   partial class SecurityPolicy : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }

      public IRouter _Commons { get; set; }
      internal Role.Code.Role _Role { get; set; }
      internal User.Code.User _User { get; set; }
   }
}
