using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.Form_Settings.Code
{
   partial class FORM_STNG : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "Commons":
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
         switch (job.Method)
         {
            case 01:
               GetUi(job);
               break;
            case 02:
               DoWork4FRPT_STNG_F(job);
               break;
            case 03:
               DoWork4AFRM_STNG_F(job);
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
            case "FRPT_STNG_F":
               _FRPT_STNG_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
