using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
namespace System.Gas.Self.Code
{
   partial class Gas : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DefaultGateway":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            case "Gnrt_Qrcd":
               _Gnrt_Qrcd.Gateway(jobs);
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
               DoWork4Strt_Mrto_F(job);
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
            case "STRT_MTRO_M":
               _STRT_MTRO_M.SendRequest(job);
               break;
            case "STRT_MBAR_M":
               _STRT_MBAR_M.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
