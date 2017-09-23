using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.Self.Code
{
   partial class Reporting : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }

      internal IRouter _Commons { get; set; }

      internal DataSource.Code.Datasource _Datasource { get; set; }
      internal ReportUnitType.Code.ReportUnitType _ReportUnitType { get; set; }
      internal ReportProfiler.Code.ReportProfiler _ReportProfiler { get; set; }
      internal ReportViewers.Code.ReportViewers _ReportViewers { get; set; }
      internal WorkFlow.Code.WorkFlow _WorkFlow { get; set; }
   }
}
