using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.Code
{
   internal partial class ReportProfiler
   {
      internal ReportProfiler(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _ReportFile = new UnderGateways.ReportFiles.Code.ReportFiles(_commons, _wall) { _DefaultGateway = this };
         _ProfilerGroup = new UnderGateways.ProfilerGroups.Code.ProfilerGroups(_commons, _wall) { _DefaultGateway = this };
      }
   }
}
