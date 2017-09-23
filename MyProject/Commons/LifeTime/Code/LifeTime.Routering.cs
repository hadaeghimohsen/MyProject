using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.LifeTime.Code
{
    partial class LifeTime : IMP
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
                 DoWork4ToolOperation(job);
                 break;
              default:
                 job.Status = StatusType.Failed;
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
              case "ToolOperation":
                 _ToolOperation.SendRequest(job);
                 break;
              default:
                 job.Status = StatusType.Failed;
                 break;
           }
        }
    }
}
