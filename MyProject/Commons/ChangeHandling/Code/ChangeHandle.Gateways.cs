using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.ChangeHandling.Code
{
   partial class ChangeHandle : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
   }
}
