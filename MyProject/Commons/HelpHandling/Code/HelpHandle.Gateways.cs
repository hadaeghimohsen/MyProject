using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.HelpHandling.Code
{
   partial class HelpHandle : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }      
   }
}
