using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.DataGuard.SecPolicy.Role.Code
{
   internal partial class Role
   {
      internal Role(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;


      }
   }
}
