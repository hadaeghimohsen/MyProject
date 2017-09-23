using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.Reporting.WorkFlow.Code
{
   partial class WorkFlow : IMP
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
               DoWork4RPT_SRPT_F(job);
               break;
            case 03:
               DoWork4RPT_SRCH_F(job);
               break;
            case 04:
               DoWork4PRF_SRCH_F(job);
               break;
            case 05:
               DoWork4WHR_SCON_F(job);
               break;
            case 06:
               DoWork4PRF_CHNG_F(job);
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
            case "RPT_SRPT_F":
               _RPT_SRPT_F.SendRequest(job);
               break;
            case "RPT_SRCH_F":
               _RPT_SRCH_F.SendRequest(job);
               break;
            case "RPT_MBAR_M":
               _RPT_MBAR_M.SendRequest(job);
               break;
            case "PRF_SPRF_F":
               _PRF_SPRF_F.SendRequest(job);
               break;
            case "PRF_SRCH_F":
               _PRF_SRCH_F.SendRequest(job);
               break;
            case "PRF_MBAR_M":
               _PRF_MBAR_M.SendRequest(job);
               break;
            case "PRF_CHNG_F":
               _PRF_CHNG_F.SendRequest(job);
               break;
            case "WHR_SCON_F":
               _WHR_SCON_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
