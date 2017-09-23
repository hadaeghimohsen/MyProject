using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Code
{
   internal partial class ProfilerGroups
   {
      internal ProfilerGroups(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
