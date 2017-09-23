using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataGuard.SecPolicy.Share.Code
{
   internal partial class SecurityPolicy
   {
      internal SecurityPolicy(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _Role = new Role.Code.Role(_commons, _wall) { _DefaultGateway = this };
         _User = new User.Code.User(_commons, _wall) { _DefaultGateway = this };
      }
   }
}
