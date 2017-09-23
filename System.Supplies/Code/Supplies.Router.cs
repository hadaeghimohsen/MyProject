using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Supplies.Code
{
   partial class Supplies : IMP
   {
      protected override void ExternalAssistance(JobRouting.Jobs.Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DefaultGateway":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            default:
               jobs.Status = StatusType.Failed;
               break;
         }
      }

      protected override void InternalAssistance(JobRouting.Jobs.Job job)
      {
         switch (job.Method)
         {
            default:
               break;
         }
      }

      protected override void RequestToUserInterface(JobRouting.Jobs.Job job)
      {
         switch (job.Gateway)
         {
            case "Wall":
               _Wall.SendRequest(job);
               break;
            default:
               break;
         }
      }
   }
}
