using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.ServiceDefinition.GrpHdr.Code
{
   partial class GroupHeader : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "ServiceDefinition":
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
            case 01 :
               GetUi(job);
               break;
            case 02:
               DoWork4ReadAccessGroupHeader(job);
               break;
            case 03:
               DoWork4GroupHeaderSettings4CurrentUser(job);
               break;
            case 04:
               DoWork4CreateNew(job);
               break;
            case 05:
               DoWork4Update(job);
               break;
            case 06:
               DoWork4Duplicate(job);
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
            case "GroupHeaderMenus":
               _GroupHeaderMenus.SendRequest(job);
               break;
            case "Create":
               _Create.SendRequest(job);
               break;
            case "Update":
               _Update.SendRequest(job);
               break;
            case "Duplicate":
               _Duplicate.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
