using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.Share.Code
{
   public partial class Services
   {
      public Services(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _GroupHeader = new GrpHdr.Code.GroupHeader(_commons, _wall) { _DefaultGateway = this };
         _Service = new SrvDef.Code.Service(_commons, _wall) { _DefaultGateway = this };
      }
   }
}
