using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.Code
{
   partial class ReportProfiler : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
      private IRouter _Commons { get; set; }
      private UnderGateways.ReportFiles.Code.ReportFiles _ReportFile { get; set; }
      private UnderGateways.ProfilerGroups.Code.ProfilerGroups _ProfilerGroup { get; set; }
   }
}
