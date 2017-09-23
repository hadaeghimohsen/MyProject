using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.Code
{
   partial class ReportProfiler
   {
      private ISendRequest _Wall { get; set; }
      private Ui.SpecifyReportProfile _SpecifyReportProfile { get; set; }
      private Ui.SpecifyProfilerGroupHeader _SpecifyProfilerGroupHeader { get; set; }
      private Ui.SpecifyReportGroupHeader _SpecifyReportGroupHeader { get; set; }
      private Ui.SpecifyGroupItems _SpecifyGroupItems { get; set; }
      private Ui.SpecifyFilter _SpecifyFilter { get; set; }
      private Ui.ProfilerTemplate _ProfilerTemplate { get; set; }
   }
}
