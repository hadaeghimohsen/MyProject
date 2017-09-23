using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace MyProject.Commons.RedoLogs.Code
{
   partial class RedoLog : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "Commons":
               _DefaultGateway.Gateway(jobs);
               break;               
            default:
               jobs.Status = StatusType.Failed;                  
               break;
         }
      }

      protected override void InternalAssistance(Job job)
      {
         switch (job.Method)
         {
            case 01:
               GetUi(job);
               break;
            case 02:
               DoWork4RedLog(job);
               break;
            default:
               break;
         }
      }

      protected override void RequestToUserInterface(Job job)
      {
         switch (job.Gateway)
         {
            case "Wall":
               _Wall.SendRequest(job);
               break;
            case "RedoLog":
               _RedoLog.SendRequest(job);
               break;
            default:
               break;
         }
      }
   }
}
