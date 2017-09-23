using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.ErrorHandling.Code
{
   partial class ErrorHandle : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
   }
}
