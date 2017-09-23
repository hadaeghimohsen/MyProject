using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Code
{
   partial class ProfilerGroups : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
      private IRouter _Commons { get; set; }
   }
}
