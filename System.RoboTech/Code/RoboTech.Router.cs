using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.RoboTech.Code
{
   partial class RoboTech:IMP
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
            #region MasterPage
            case 02:
               Frst_Page_F(job);
               break;
            #endregion
            #region BaseDefinition
            case 03:
               Regn_Dfin_F(job);
               break;
            case 04:
               Isic_Dfin_F(job);
               break;
            case 05:
               Orgn_Dfin_F(job);
               break;
            case 10:
               Wgul_Dfin_F(job);
               break;
            #endregion
            #region DevelopmentApplication
            case 06:
               Orgn_Dvlp_F(job);
               break;
            case 07:
               Rbkn_Dvlp_F(job);
               break;
            case 08:
               Rbsr_Dvlp_F(job);
               break;
            case 09:
               Rbsa_Dvlp_F(job);
               break;
            case 12:
               Rbod_Dvlp_F(job);
               break;
            case 13:
               Rbmn_Dvlp_F(job);
               break;
            case 17:
               Odrm_Dvlp_F(job);
               break;
            case 18:
               Orml_Dvlp_F(job);
               break;
            case 19:
               Rbsm_Dvlp_F(job);
               break;
            case 20:
               Srbt_Info_F(job);
               break;
            case 21:
               Sale_Dvlp_F(job);
               break;
            case 22:
               Alpk_Dvlp_F(job);
               break;
            case 23:
               Ordr_Ship_F(job);
               break;
            case 24:
               Ordr_Rcpt_F(job);
               break;
            case 25:
               Bank_Dvlp_F(job);
               break;
            case 26:
               Prod_Dvlp_F(job);
               break;
            case 27:
               Wlet_Dvlp_F(job);
               break;
            case 28:
               Onro_Dvlp_F(job);
               break;
            case 29:
               Inst_Conf_F(job);
               break;
            case 30:
               Mesg_Dvlp_F(job);
               break;
            #endregion
            #region Action
            case 11:
               Strt_Robo_F(job);
               break;
            case 14:
               Stng_Rprt_F(job);
               break;
            case 15:
               Rpt_Mngr_F(job);
               break;
            case 16:
               Rpt_Lrfm_F(job);
               break;
            #endregion
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
            #region MasterPage
            case "FRST_PAGE_F":
               _Frst_Page_F.SendRequest(job);
               break;
            #endregion
            #region BaseDefinition
            case "REGN_DFIN_F":
               _Regn_Dfin_F.SendRequest(job);
               break;
            case "ISIC_DFIN_F":
               _Isic_Dfin_F.SendRequest(job);
               break;
            case "ORGN_DFIN_F":
               _Orgn_Dfin_F.SendRequest(job);
               break;
            case "WGUL_DFIN_F":
               _Wgul_Dfin_F.SendRequest(job);
               break;
            #endregion
            #region DevelopmentApplication
            case "ORGN_DVLP_F":
               _Orgn_Dvlp_F.SendRequest(job);
               break;
            case "RBKN_DVLP_F":
               _Rbkn_Dvlp_F.SendRequest(job);
               break;
            case "RBSR_DVLP_F":
               _Rbsr_Dvlp_F.SendRequest(job);
               break;
            case "RBSA_DVLP_F":
               _Rbsa_Dvlp_F.SendRequest(job);
               break;
            case "RBOD_DVLP_F":
               _Rbod_Dvlp_F.SendRequest(job);
               break;
            case "RBMN_DVLP_F":
               _Rbmn_Dvlp_F.SendRequest(job);
               break;
            case "ODRM_DVLP_F":
               _Odrm_Dvlp_F.SendRequest(job);
               break;
            case "ORML_DVLP_F":
               _Orml_Dvlp_F.SendRequest(job);
               break;
            case "RBSM_DVLP_F":
               _Rbsm_Dvlp_F.SendRequest(job);
               break;
            case "SRBT_INFO_F":
               _Srbt_Info_F.SendRequest(job);
               break;
            case "SALE_DVLP_F":
               _Sale_Dvlp_F.SendRequest(job);
               break;
            case "ALPK_DVLP_F":
               _Alpk_Dvlp_F.SendRequest(job);
               break;
            case "ORDR_SHIP_F":
               _Ordr_Ship_F.SendRequest(job);
               break;
            case "ORDR_RCPT_F":
               _Ordr_Rcpt_F.SendRequest(job);
               break;
            case "BANK_DVLP_F":
               _Bank_Dvlp_F.SendRequest(job);
               break;
            case "PROD_DVLP_F":
               _Prod_Dvlp_F.SendRequest(job);
               break;
            case "WLET_DVLP_F":
               _Wlet_Dvlp_F.SendRequest(job);
               break;
            case "ONRO_DVLP_F":
               _Onro_Dvlp_F.SendRequest(job);
               break;
            case "INST_CONF_F":
               _Inst_Conf_F.SendRequest(job);
               break;
            case "MESG_DVLP_F":
               _Mesg_Dvlp_F.SendRequest(job);
               break;
            #endregion
            #region Action
            case "STRT_ROBO_F":
               _Strt_Robo_F.SendRequest(job);
               break;
            case "STNG_RPRT_F":
               _Stng_Rprt_F.SendRequest(job);
               break;
            case "RPT_MNGR_F":
               _Rpt_Mngr_F.SendRequest(job);
               break;
            case "RPT_LRFM_F":
               _Rpt_Lrfm_F.SendRequest(job);
               break;
            #endregion
            default:
               break;
         }
      }
   }
}
