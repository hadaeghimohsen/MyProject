using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.CRM.Code
{
   partial class CRM :IMP
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
               Adm_Cust_F(job);
               break;
            case 12:
               Pay_Mtod_F(job);
               break;
            case 13:
               Adm_Chng_F(job);
               break;
            case 14:
               Lst_Cust_F(job);
               break;
            case 15:
               Inf_Cust_F(job);
               break;
            case 16:
               Jobp_Dfin_F(job);
               break;
            case 17:
               Act_Logc_F(job);
               break;
            case 18:
               Act_Sndf_F(job);
               break;
            case 19:
               Act_Trat_F(job);
               break;
            case 20:
               Isic_Dfin_F(job);
               break;
            case 21:
               Notf_Totl_F(job);
               break;
            case 22:
               Task_Flow_F(job);
               break;
            case 23:
               Shw_Lead_F(job);
               break;
            case 24:
               Inf_Lead_F(job);
               break;
            case 25:
               Opt_Logc_F(job);
               break;
            case 26:
               Opt_Task_F(job);
               break;
            case 27:
               Opt_Apon_F(job);
               break;
            case 28:
               Stng_Dfin_F(job);
               break;
            case 29:
               Opt_Sndf_F(job);
               break;
            case 30:
               Opt_Clon_F(job);
               break;
            case 31:
               Opt_Emal_F(job);
               break;
            case 32:
               Fss_Show_F(job);
               break;
            case 33:
               Shw_Cont_F(job);
               break;
            case 34:
               Inf_Cont_F(job);
               break;
            case 35:
               Tol_Deal_F(job);
               break;
            case 36:
               Dtl_Deal_F(job);
               break;
            case 37:
               Opt_Cnvt_F(job);
               break;
            case 38:
               Shw_Acnt_F(job);
               break;
            case 39:
               Inf_Acnt_F(job);
               break;
            case 40:
               Cmn_Dcmt_F(job);
               break;
            case 41:
               Serv_Camr_F(job);
               break;
            case 42:
               Serv_Dcmt_F(job);
               break;
            case 43:
               Shw_Deal_F(job);
               break;
            case 44:
               Inf_Deal_F(job);
               break;
            case 45:
               Tol_Tsap_F(job);
               break;
            case 46:
               Opt_Clnc_F(job);
               break;
            case 47:
               Comp_Chng_F(job);
               break;
            case 48:
               Tsk_Colr_F(job);
               break;
            case 49:
               Tsk_Ckls_F(job);
               break;
            case 50:
               Tsk_Tag_F(job);
               break;
            case 51:
               Tsk_Cmnt_F(job);
               break;
            case 52:
               Tsk_Ckld_F(job);
               break;
            case 53:
               Opt_Mesg_F(job);
               break;
            case 54:
               Chg_Tarf_F(job);
               break;
            case 55:
               Hst_Urqs_F(job);
               break;
            case 56:
               Opt_Rqst_F(job);
               break;
            case 57:
               Lst_Serv_F(job);
               break;
            case 58:
               Add_Serv_F(job);
               break;
            case 59:
               Fin_Rslt_F(job);
               break;
            case 60:
               Inf_Ctwk_F(job);
               break;
            case 61:
               Hst_Logc_F(job);
               break;
            case 62:
               Hst_Mesg_F(job);
               break;
            case 63:
               Hst_Emal_F(job);
               break;
            case 64:
               Hst_Task_F(job);
               break;
            case 65:
               Hst_Apon_F(job);
               break;
            case 66:
               Hst_Sndf_F(job);
               break;
            case 67:
               Hst_Note_F(job);
               break;
            case 68:               
               break;
            case 69:
               Tmpl_Dfin_F(job);
               break;
            case 70:
               Hst_Finr_F(job);
               break;
            case 71:
               Add_Info_F(job);
               break;
            case 72:
               Sjbp_Dfin_F(job);
               break;
            case 73:
               Mntn_Totl_F(job);
               break;
            case 75:
               Opt_Note_F(job);
               break;
            case 76:
               Hst_Utag_F(job);
               break;
            case 77:
               Hst_Urgn_F(job);
               break;
            case 78:
               Hst_Fltr_F(job);
               break;
            case 79:
               Apbs_Dfin_F(job);
               break;
            case 80:
               Hst_Uexf_F(job);
               break;
            case 81:
               Hst_Uctf_F(job);
               break;
            case 82:
               Opt_Aeml_F(job);
               break;
            case 83:
               Hst_Ssid_F(job);
               break;
            case 84:
               Rlat_Sinf_F(job);
               break;
            case 85:
               Rlat_Cinf_F(job);
               break;
            case 86:
               Cmph_Dfin_F(job);
               break;
            case 87:
               Cjbp_Dfin_F(job);
               break;
            case 88:
               Opt_Prjt_F(job);
               break;
            case 89:
               Mstt_Dfin_F(job);
               break;
            case 90:
               Jbpd_Dfin_F(job);
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
            case "STNG_DFIN_F":
               _Stng_Dfin_F.SendRequest(job);
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
            case "JOBP_DFIN_F":
               _Jobp_Dfin_F.SendRequest(job);
               break;
            case "ISIC_DFIN_F":
               _Isic_Dfin_F.SendRequest(job);
               break;
            case "ADM_CUST_F":
               _Adm_Cust_F.SendRequest(job);
               break;
            case "PAY_MTOD_F":
               _Pay_Mtod_F.SendRequest(job);
               break;
            case "ADM_CHNG_F":
               _Adm_Chng_F.SendRequest(job);
               break;
            case "LST_CUST_F":
               _Lst_Cust_F.SendRequest(job);
               break;
            case "INF_CUST_F":
               _Inf_Cust_F.SendRequest(job);
               break;
            //case "ACT_LOGC_F":
            //   _Act_Logc_F.SendRequest(job);
            //   break;
            //case "ACT_SNDF_F":
            //   _Act_Sndf_F.SendRequest(job);
            //   break;
            //case "ACT_TRAT_F":
            //   _Act_Trat_F.SendRequest(job);
            //   break;
            case "NOTF_TOTL_F":
               _Notf_Totl_F.SendRequest(job);
               break;
            case "TASK_FLOW_F":
               _Task_Flow_F.SendRequest(job);
               break;
            case "SHW_LEAD_F":
               _Shw_Lead_F.SendRequest(job);
               break;
            case "INF_LEAD_F":
               _Inf_Lead_F.SendRequest(job);
               break;
            case "OPT_LOGC_F":
               _Opt_Logc_F.SendRequest(job);
               break;
            case "OPT_TASK_F":
               _Opt_Task_F.SendRequest(job);
               break;
            case "OPT_APON_F":
               _Opt_Apon_F.SendRequest(job);
               break;
            case "OPT_SNDF_F":
               _Opt_Sndf_F.SendRequest(job);
               break;
            case "OPT_CLON_F":
               _Opt_Clon_F.SendRequest(job);
               break;
            case "OPT_EMAL_F":
               _Opt_Emal_F.SendRequest(job);
               break;
            case "FSS_SHOW_F":
               _Fss_Show_F.SendRequest(job);
               break;
            case "SHW_CONT_F":
               _Shw_Cont_F.SendRequest(job);
               break;
            case "INF_CONT_F":
               _Inf_Cont_F.SendRequest(job);
               break;
            case "TOL_DEAL_F":
               _Tol_Deal_F.SendRequest(job);
               break;
            case "DTL_DEAL_F":
               _Dtl_Deal_F.SendRequest(job);
               break;
            case "OPT_CNVT_F":
               _Opt_Cnvt_F.SendRequest(job);
               break;
            case "SHW_ACNT_F":
               _Shw_Acnt_F.SendRequest(job);
               break;
            case "INF_ACNT_F":
               _Inf_Acnt_F.SendRequest(job);
               break;
            case "CMN_DCMT_F":
               _Cmn_Dcmt_F.SendRequest(job);
               break;
            case "SERV_CAMR_F":
               _Serv_Camr_F.SendRequest(job);
               break;
            case "SERV_DCMT_F":
               _Serv_Dcmt_F.SendRequest(job);
               break;
            case "SHW_DEAL_F":
               _Shw_Deal_F.SendRequest(job);
               break;
            case "INF_DEAL_F":
               _Inf_Deal_F.SendRequest(job);
               break;
            case "TOL_TSAP_F":
               _Tol_Tsap_F.SendRequest(job);
               break;
            case "OPT_CLNC_F":
               _Opt_Clnc_F.SendRequest(job);
               break;
            case "COMP_CHNG_F":
               _Comp_Chng_F.SendRequest(job);
               break;
            case "TSK_COLR_F":
               _Tsk_Colr_F.SendRequest(job);
               break;
            case "TSK_CKLS_F":
               _Tsk_Ckls_F.SendRequest(job);
               break;
            case "TSK_TAG_F":
               _Tsk_Tag_F.SendRequest(job);
               break;
            case "TSK_CMNT_F":
               _Tsk_Cmnt_F.SendRequest(job);
               break;
            case "TSK_CKLD_F":
               _Tsk_Ckld_F.SendRequest(job);
               break;
            case "OPT_MESG_F":
               _Opt_Mesg_F.SendRequest(job);
               break;
            case "CHG_TARF_F":
               _Chg_Tarf_F.SendRequest(job);
               break;
            case "HST_URQS_F":
               _Hst_Urqs_F.SendRequest(job);
               break;
            case "OPT_RQST_F":
               _Opt_Rqst_F.SendRequest(job);
               break;
            case "LST_SERV_F":
               _Lst_Serv_F.SendRequest(job);
               break;
            case "ADD_SERV_F":
               _Add_Serv_F.SendRequest(job);
               break;
            case "FIN_RSLT_F":
               _Fin_Rslt_F.SendRequest(job);
               break;
            case "INF_CTWK_F":
               _Inf_Ctwk_F.SendRequest(job);
               break;
            case "HST_LOGC_F":
               _Hst_Logc_F.SendRequest(job);
               break;
            case "HST_MESG_F":
               _Hst_Mesg_F.SendRequest(job);
               break;
            case "HST_EMAL_F":
               _Hst_Emal_F.SendRequest(job);
               break;
            case "HST_TASK_F":
               _Hst_Task_F.SendRequest(job);
               break;
            case "HST_APON_F":
               _Hst_Apon_F.SendRequest(job);
               break;
            case "HST_SNDF_F":
               _Hst_Sndf_F.SendRequest(job);
               break;
            case "HST_NOTE_F":
               _Hst_Note_F.SendRequest(job);
               break;            
            case "TMPL_DFIN_F":
               _Tmpl_Dfin_F.SendRequest(job);
               break;
            case "HST_FINR_F":
               _Hst_Finr_F.SendRequest(job);
               break;
            case "ADD_INFO_F":
               _Add_Info_F.SendRequest(job);
               break;
            case "SJBP_DFIN_F":
               _Sjbp_Dfin_F.SendRequest(job);
               break;
            case "MNTN_TOTL_F":
               _Mntn_Totl_F.SendRequest(job);
               break;
            case "OPT_NOTE_F":
               _Opt_Note_F.SendRequest(job);
               break;
            case "HST_UTAG_F":
               _Hst_Utag_F.SendRequest(job);
               break;
            case "HST_URGN_F":
               _Hst_Urgn_F.SendRequest(job);
               break;
            case "HST_FLTR_F":
               _Hst_Fltr_F.SendRequest(job);
               break;
            case "APBS_DFIN_F":
               _Apbs_Dfin_F.SendRequest(job);
               break;
            case "HST_UEXF_F":
               _Hst_Uexf_F.SendRequest(job);
               break;
            case "HST_UCTF_F":
               _Hst_Uctf_F.SendRequest(job);
               break;
            case "OPT_AEML_F":
               _Opt_Aeml_F.SendRequest(job);
               break;
            case "HST_SSID_F":
               _Hst_Ssid_f.SendRequest(job);
               break;
            case "RLAT_SINF_F":
               _Rlat_Sinf_F.SendRequest(job);
               break;
            case "RLAT_CINF_F":
               _Rlat_Cinf_F.SendRequest(job);
               break;
            case "CMPH_DFIN_F":
               _Cmph_Dfin_F.SendRequest(job);
               break;
            case "CJBP_DFIN_F":
               _Cjbp_Dfin_F.SendRequest(job);
               break;
            case "OPT_PRJT_F":
               _Opt_Prjt_F.SendRequest(job);
               break;
            case "MSTT_DFIN_F":
               _Mstt_Dfin_F.SendRequest(job);
               break;
            case "JBPD_DFIN_F":
               _Jbpd_Dfin_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
