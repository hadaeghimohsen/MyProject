using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.Self.Code
{
   public partial class Reporting
   {
      public Reporting(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _Datasource = new DataSource.Code.Datasource(_commons, _wall) { _DefaultGateway = this };
         _ReportUnitType = new ReportUnitType.Code.ReportUnitType(_commons, _wall) { _DefaultGateway = this };
         _ReportProfiler = new ReportProfiler.Code.ReportProfiler(_commons, _wall) { _DefaultGateway = this };
         _ReportViewers = new ReportViewers.Code.ReportViewers(_commons, _wall) { _DefaultGateway = this };
         _WorkFlow = new WorkFlow.Code.WorkFlow(_commons, _wall) { _DefaultGateway = this };

      }
   }
}
