using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Emis.Sas.Controller
{
   partial class Sas : IMP
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
               Mstr_Serv_F(job);
               break;
            case 04:
               Mstr_Rqst_F(job);
               break;
            case 05:
               Serv_Bill_F(job);
               break;
            case 06:
               Serv_Info_F(job);
               break;
            case 07:
               Serv_Dart_F(job);
               break;
            case 08:
               Serv_Rqst_F(job);
               break;
            case 09:
               Pblc_Serv_F(job);
               break;
            case 10:
               Mstr_Regl_F(job);
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
            case "MSTR_PAGE_F":
               _Mstr_Page_F.SendRequest(job);
               break;
            case "MSTR_SERV_F":
               _Mstr_Serv_F.SendRequest(job);
               break;
            case "MSTR_RQST_F":
               _Mstr_Rqst_F.SendRequest(job);
               break;
            case "SERV_BILL_F":
               _SERV_BILL_F.SendRequest(job);
               break;
            case "SERV_INFO_F":
               _SERV_INFO_F.SendRequest(job);
               break;
            case "SERV_DART_F":
               _SERV_DART_F.SendRequest(job);
               break;
            case "SERV_RQST_F":
               _SERV_RQST_F.SendRequest(job);
               break;
            case "PBLC_SERV_F":
               _PBLC_SERV_F.SendRequest(job);
               break;
            case "MSTR_REGL_F":
               _MSTR_REGL_F.SendRequest(job);
               break;
            default:
               break;
         }
      }
   }
}
