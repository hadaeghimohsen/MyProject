using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ISP.Code
{
   partial class ISP :IMP
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
               Frst_Page_F(job);
               break;
            case 03:
               Regn_Dfin_F(job);
               break;
            case 04:
               Epit_Dfin_F(job);
               break;
            case 05:
               Btrf_Dfin_F(job);
               break;
            case 06:
               Cash_Dfin_F(job);
               break;
            case 07:
               Regl_Dfin_F(job);
               break;
            case 08:
               Rqrq_Dfin_F(job);
               break;
            case 09:
               Dcsp_Dfin_F(job);
               break;
            case 10:
               Orgn_Dfin_F(job);
               break;
            case 11:
               Adm_Adsl_F(job);
               break;
            case 12:
               Pay_Mtod_F(job);
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
            case "FRST_PAGE_F":
               _Frst_Page_F.SendRequest(job);
               break;
            case "REGN_DFIN_F":
               _Regn_Dfin_F.SendRequest(job);
               break;
            case "EPIT_DFIN_F":
               _Epit_Dfin_F.SendRequest(job);
               break;
            case "BTRF_DFIN_F":
               _Btrf_Dfin_F.SendRequest(job);
               break;
            case "CASH_DFIN_F":
               _Cash_Dfin_F.SendRequest(job);
               break;
            case "REGL_DFIN_F":
               _Regl_Dfin_F.SendRequest(job);
               break;
            case "RQRQ_DFIN_F":
               _Rqrq_Dfin_F.SendRequest(job);
               break;
            case "DCSP_DFIN_F":
               _Dcsp_Dfin_F.SendRequest(job);
               break;
            case "ORGN_DFIN_F":
               _Orgn_Dfin_F.SendRequest(job);
               break;
            case "ADM_ADSL_F":
               _Adm_Adsl_F.SendRequest(job);
               break;
            case "PAY_MTOD_F":
               _Pay_Mtod_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
