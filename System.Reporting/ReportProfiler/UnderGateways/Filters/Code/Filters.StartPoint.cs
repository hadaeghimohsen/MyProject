using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.UnderGateways.Filters.Code
{
   internal partial class Filters
   {
      internal Filters(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
