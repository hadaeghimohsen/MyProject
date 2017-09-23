using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.JobRouting.Routering
{
   public interface IDefaultGateway
   {
      IRouter _DefaultGateway { get; set; }
   }
}
