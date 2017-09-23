using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportUnitType.Code
{
   partial class ReportUnitType : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
      internal IRouter _Commons { get; set; }
   }
}
