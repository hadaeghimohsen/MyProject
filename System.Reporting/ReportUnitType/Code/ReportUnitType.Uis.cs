using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportUnitType.Code
{
   partial class ReportUnitType
   {
      private ISendRequest _Wall { get; set; }

      private Ui.SpecifyReportFile _SpecifyReportFile { get; set; }
      private Ui.SpecifyAppDecision _SpecifyAppDecision { get; set; }
   }
}
