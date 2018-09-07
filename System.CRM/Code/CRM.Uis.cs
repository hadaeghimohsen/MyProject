using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.CRM.Code
{
   partial class CRM
   {
      public ISendRequest _Wall { get; set; }

      internal Ui.MasterPage.FRST_PAGE_F _Frst_Page_F { get; set; }
      //internal Ui.BaseDefination.STNG_DFIN_F _Stng_Dfin_F { get; set; }
      internal Ui.BaseDefination.REGN_DFIN_F _Regn_Dfin_F { get; set; }
      internal Ui.BaseDefination.EPIT_DFIN_F _Epit_Dfin_F { get; set; }
      internal Ui.BaseDefination.BTRF_DFIN_F _Btrf_Dfin_F { get; set; }
      internal Ui.BaseDefination.CASH_DFIN_F _Cash_Dfin_F { get; set; }
      internal Ui.BaseDefination.REGL_DFIN_F _Regl_Dfin_F { get; set; }
      internal Ui.BaseDefination.RQRQ_DFIN_F _Rqrq_Dfin_F { get; set; }
      internal Ui.BaseDefination.DCSP_DFIN_F _Dcsp_Dfin_F { get; set; }
      internal Ui.BaseDefination.ORGN_DFIN_F _Orgn_Dfin_F { get; set; }
      internal Ui.BaseDefination.JOBP_DFIN_F _Jobp_Dfin_F { get; set; }
      internal Ui.BaseDefination.ISIC_DFIN_F _Isic_Dfin_F { get; set; }
      internal Ui.Admission.ADM_CUST_F _Adm_Cust_F { get; set; }
      internal Ui.Payment.PAY_MTOD_F _Pay_Mtod_F { get; set; }
      internal Ui.PublicInformation.ADM_CHNG_F _Adm_Chng_F { get; set; }
      internal Ui.CommonInformation.LST_CUST_F _Lst_Cust_F { get; set; }
      internal Ui.CommonInformation.INF_CUST_F _Inf_Cust_F { get; set; }
      //internal Ui.Activity.ACT_LOGC_F _Act_Logc_F { get; set; }
      //internal Ui.Activity.ACT_SNDF_F _Act_Sndf_F { get; set; }
      //internal Ui.Activity.ACT_TRAT_F _Act_Trat_F { get; set; }
      internal Ui.Notification.NOTF_TOTL_F _Notf_Totl_F { get; set; }
      internal Ui.Notification.TASK_FLOW_F _Task_Flow_F { get; set; }
      internal Ui.Leads.SHW_LEAD_F _Shw_Lead_F { get; set; }
      internal Ui.Leads.SHW_OPRT_F _Shw_Oprt_F { get; set; }
      internal Ui.Leads.INF_LEAD_F _Inf_Lead_F { get; set; }
      internal Ui.Leads.RSL_LEAD_F _Rsl_Lead_F { get; set; }
      internal Ui.Activity.OPT_LOGC_F _Opt_Logc_F { get; set; }
      internal Ui.Activity.OPT_TASK_F _Opt_Task_F { get; set; }
      internal Ui.Activity.OPT_APON_F _Opt_Apon_F { get; set; }
      internal Ui.Activity.OPT_SNDF_F _Opt_Sndf_F { get; set; }
      internal Ui.Activity.OPT_CLON_F _Opt_Clon_F { get; set; }
      internal Ui.Activity.OPT_EMAL_F _Opt_Emal_F { get; set; }
      internal Ui.FileServerStorage.FSS_SHOW_F _Fss_Show_F { get; set; }
      internal Ui.Contacts.INF_CONT_F _Inf_Cont_F { get; set; }
      internal Ui.Contacts.SHW_CONT_F _Shw_Cont_F { get; set; }
      internal Ui.Deals.TOL_DEAL_F _Tol_Deal_F { get; set; }
      internal Ui.Deals.DTL_DEAL_F _Dtl_Deal_F { get; set; }
      internal Ui.Activity.OPT_CNVT_F _Opt_Cnvt_F { get; set; }
      internal Ui.Acounts.SHW_ACNT_F _Shw_Acnt_F { get; set; }
      internal Ui.Acounts.INF_ACNT_F _Inf_Acnt_F { get; set; }
      internal Ui.PublicInformation.CMN_DCMT_F _Cmn_Dcmt_F { get; set; }
      internal Ui.PublicInformation.SERV_CAMR_F _Serv_Camr_F { get; set; }
      internal Ui.PublicInformation.SERV_DCMT_F _Serv_Dcmt_F { get; set; }
      internal Ui.Deals.SHW_DEAL_F _Shw_Deal_F { get; set; }
      internal Ui.Deals.INF_DEAL_F _Inf_Deal_F { get; set; }
      internal Ui.TaskAppointment.TOL_TSAP_F _Tol_Tsap_F { get; set; }
      internal Ui.Activity.OPT_CLNC_F _Opt_Clnc_F { get; set; }
      internal Ui.PublicInformation.COMP_CHNG_F _Comp_Chng_F { get; set; }
      internal Ui.TaskAppointment.TSK_COLR_F _Tsk_Colr_F { get; set; }
      internal Ui.TaskAppointment.TSK_CKLS_F _Tsk_Ckls_F { get; set; }
      internal Ui.TaskAppointment.TSK_TAG_F _Tsk_Tag_F { get; set; }
      internal Ui.TaskAppointment.TSK_CMNT_F _Tsk_Cmnt_F { get; set; }
      internal Ui.TaskAppointment.TSK_CKLD_F _Tsk_Ckld_F { get; set; }
      internal Ui.Activity.OPT_MESG_F _Opt_Mesg_F { get; set; }
      internal Ui.PublicInformation.CHG_TARF_F _Chg_Tarf_F { get; set; }
      internal Ui.HistoryAction.HST_URQS_F _Hst_Urqs_F { get; set; }
      internal Ui.Activity.OPT_RQST_F _Opt_Rqst_F { get; set; }
      internal Ui.PublicInformation.LST_SERV_F _Lst_Serv_F { get; set; }
      internal Ui.PublicInformation.ADD_SERV_F _Add_Serv_F { get; set; }
      internal Ui.TaskAppointment.FIN_RSLT_F _Fin_Rslt_F { get; set; }
      internal Ui.PublicInformation.INF_CTWK_F _Inf_Ctwk_F { get; set; }
      internal Ui.HistoryAction.HST_LOGC_F _Hst_Logc_F { get; set; }
      internal Ui.HistoryAction.HST_MESG_F _Hst_Mesg_F { get; set; }
      internal Ui.HistoryAction.HST_EMAL_F _Hst_Emal_F { get; set; }
      internal Ui.HistoryAction.HST_TASK_F _Hst_Task_F { get; set; }
      internal Ui.HistoryAction.HST_APON_F _Hst_Apon_F { get; set; }
      internal Ui.HistoryAction.HST_SNDF_F _Hst_Sndf_F { get; set; }
      internal Ui.HistoryAction.HST_NOTE_F _Hst_Note_F { get; set; }
      internal Ui.BaseDefination.TMPL_DFIN_F _Tmpl_Dfin_F { get; set; }
      internal Ui.HistoryAction.HST_FINR_F _Hst_Finr_F { get; set; }
      internal Ui.PublicInformation.ADD_INFO_F _Add_Info_F { get; set; }
      internal Ui.BaseDefination.SJBP_DFIN_F _Sjbp_Dfin_F { get; set; }
      internal Ui.Notification.MNTN_TOTL_F _Mntn_Totl_F { get; set; }
      internal Ui.Activity.OPT_NOTE_F _Opt_Note_F { get; set; }
      internal Ui.HistoryAction.HST_UTAG_F _Hst_Utag_F { get; set; }
      internal Ui.HistoryAction.HST_URGN_F _Hst_Urgn_F { get; set; }
      internal Ui.HistoryAction.HST_FLTR_F _Hst_Fltr_F { get; set; }
      internal Ui.BaseDefination.APBS_DFIN_F _Apbs_Dfin_F { get; set; }
      internal Ui.HistoryAction.HST_UEXF_F _Hst_Uexf_F { get; set; }
      internal Ui.HistoryAction.HST_UCTF_F _Hst_Uctf_F { get; set; }
      internal Ui.Activity.OPT_AEML_F _Opt_Aeml_F { get; set; }
      internal Ui.HistoryAction.HST_SSID_F _Hst_Ssid_f { get; set; }
      internal Ui.PublicInformation.RLAT_SINF_F _Rlat_Sinf_F { get; set; }
      internal Ui.PublicInformation.RLAT_CINF_F _Rlat_Cinf_F { get; set; }
      internal Ui.BaseDefination.CMPH_DFIN_F _Cmph_Dfin_F { get; set; }
      internal Ui.BaseDefination.CJBP_DFIN_F _Cjbp_Dfin_F { get; set; }
      internal Ui.Activity.OPT_PRJT_F _Opt_Prjt_F { get; set; }
      internal Ui.BaseDefination.MSTT_DFIN_F _Mstt_Dfin_F { get; set; }
      internal Ui.BaseDefination.JBPD_DFIN_F _Jbpd_Dfin_F { get; set; }
      internal Ui.Activity.OPT_INFO_F _Opt_Info_F { get; set; }

      internal Ui.Competitor.SHW_CMPT_F _Shw_Cmpt_F { get; set; }
      internal Ui.Competitor.INF_CMPT_F _Inf_Cmpt_F { get; set; }

      internal Ui.MarketingList.INF_MKLT_F _Inf_Mklt_F { get; set; }
      internal Ui.MarketingList.SHW_MKLT_F _Shw_Mklt_F { get; set; }

      internal Ui.Campaign.INF_CAMP_F _Inf_Camp_F { get; set; }
      internal Ui.Campaign.SHW_CAMP_F _Shw_Camp_F { get; set; }

      internal Ui.CampaignQuick.INF_CAMQ_F _Inf_Camq_F { get; set; }
      internal Ui.CampaignQuick.SHW_CAMQ_F _Shw_Camq_F { get; set; }

      internal Ui.CampaignActivity.INF_CAMA_F _Inf_Cama_F { get; set; }
      internal Ui.CampaignActivity.SHW_CAMA_F _Shw_Cama_F { get; set; }

   }
}
