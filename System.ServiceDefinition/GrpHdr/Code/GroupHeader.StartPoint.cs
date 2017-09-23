using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.GrpHdr.Code
{
   internal partial class GroupHeader
   {
      internal GroupHeader(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
