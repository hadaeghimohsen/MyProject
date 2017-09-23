using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.UnderGateways.Filters.Code
{
   partial class Filters : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DefaultGateway":
               _DefaultGateway.Gateway(jobs);
               break;
            default:
               jobs.Status = StatusType.Failed;
               break;
         }
      }

      protected override void InternalAssistance(Job job)
      {
         throw new NotImplementedException();
      }

      protected override void RequestToUserInterface(Job job)
      {
         throw new NotImplementedException();
      }
   }
}
