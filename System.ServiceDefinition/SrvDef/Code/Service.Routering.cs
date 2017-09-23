using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.ServiceDefinition.SrvDef.Code
{
   partial class Service : IMP
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
            case "UnitType":
               _UnitType.Gateway(jobs);
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
               DoWork4LoadParentServicesOfGroupHeaders(job);
               break;
            case 03:
               DoWork4CreateNew(job);
               break;
            case 04:
               DoWork4LoadServicesOfParentService(job);
               break;
            case 05:
               DoWork4Update(job);
               break;
            case 06:
               DoWork4LoadGrpSrvInGrpHdrWithCondition(job);
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
            case "Create":
               _Create.SendRequest(job);
               break;
            case "SUR":
               _ShowUpdateRemove.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
