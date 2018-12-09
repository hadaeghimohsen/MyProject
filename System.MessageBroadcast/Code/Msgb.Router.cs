using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.MessageBroadcast.Code
{
   partial class Msgb :IMP
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
               Mstr_Page_F(job);
               break;
            case 03:
               Actn_Extr_P(job);
               break;
            case 04:
               Mesg_Chks_P(job);
               break;
            case 05:
               Mesg_Recv_P(job);
               break;
            case 06:
               Tree_Node_P(job);
               break;
            case 07:
               Send_Mesg_F(job);
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
            case "MSTR_PAGE_F":
               _Mstr_Page_F.SendRequest(job);
               break;
            case "SEND_MESG_F":
               _Send_Mesg_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
