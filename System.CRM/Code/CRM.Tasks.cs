using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.CRM.Code
{
   partial class CRM
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if(value == "frst_page_f")
         {
            if (_Frst_Page_F == null)
               _Frst_Page_F = new Ui.MasterPage.FRST_PAGE_F { _DefaultGateway = this };
         }
         else if (value == "stng_dfin_f")
         {
            //if (_Stng_Dfin_F == null)
            //   _Stng_Dfin_F = new Ui.BaseDefination.STNG_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "regn_dfin_f")
         {
            if (_Regn_Dfin_F == null)
               _Regn_Dfin_F = new Ui.BaseDefination.REGN_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "epit_dfin_f")
         {
            if (_Epit_Dfin_F == null)
               _Epit_Dfin_F = new Ui.BaseDefination.EPIT_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "btrf_dfin_f")
         {
            if (_Btrf_Dfin_F == null)
               _Btrf_Dfin_F = new Ui.BaseDefination.BTRF_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "cash_dfin_f")
         {
            if (_Cash_Dfin_F == null)
               _Cash_Dfin_F = new Ui.BaseDefination.CASH_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "regl_dfin_f")
         {
            if (_Regl_Dfin_F == null)
               _Regl_Dfin_F = new Ui.BaseDefination.REGL_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "rqrq_dfin_f")
         {
            if (_Rqrq_Dfin_F == null)
               _Rqrq_Dfin_F = new Ui.BaseDefination.RQRQ_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "dcsp_dfin_f")
         {
            if (_Dcsp_Dfin_F == null)
               _Dcsp_Dfin_F = new Ui.BaseDefination.DCSP_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "orgn_dfin_f")
         {
            if (_Orgn_Dfin_F == null)
               _Orgn_Dfin_F = new Ui.BaseDefination.ORGN_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "jobp_dfin_f")
         {
            if (_Jobp_Dfin_F == null)
               _Jobp_Dfin_F = new Ui.BaseDefination.JOBP_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "isic_dfin_f")
         {
            if (_Isic_Dfin_F == null)
               _Isic_Dfin_F = new Ui.BaseDefination.ISIC_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "adm_cust_f")
         {
            if (_Adm_Cust_F == null)
               _Adm_Cust_F = new Ui.Admission.ADM_CUST_F { _DefaultGateway = this };
         }
         else if (value == "pay_mtod_f")
         {
            if (_Pay_Mtod_F == null)
               _Pay_Mtod_F = new Ui.Payment.PAY_MTOD_F { _DefaultGateway = this };
         }
         else if (value == "adm_chng_f")
         {
            if (_Adm_Chng_F == null)
               _Adm_Chng_F = new Ui.PublicInformation.ADM_CHNG_F { _DefaultGateway = this };
         }
         else if (value == "lst_cust_f")
         {
            if (_Lst_Cust_F == null)
               _Lst_Cust_F = new Ui.CommonInformation.LST_CUST_F { _DefaultGateway = this };
         }
         else if (value == "inf_cust_f")
         {
            if (_Inf_Cust_F == null)
               _Inf_Cust_F = new Ui.CommonInformation.INF_CUST_F { _DefaultGateway = this };
         }
         //else if (value == "act_logc_f")
         //{
         //   if (_Act_Logc_F == null)
         //      _Act_Logc_F = new Ui.Activity.ACT_LOGC_F { _DefaultGateway = this };
         //}
         //else if (value == "act_sndf_f")
         //{
         //   if (_Act_Sndf_F == null)
         //      _Act_Sndf_F = new Ui.Activity.ACT_SNDF_F { _DefaultGateway = this };
         //}
         //else if (value == "act_trat_f")
         //{
         //   if (_Act_Trat_F == null)
         //      _Act_Trat_F = new Ui.Activity.ACT_TRAT_F { _DefaultGateway = this };
         //}
         else if (value == "notf_totl_f")
         {
            if (_Notf_Totl_F == null)
               _Notf_Totl_F = new Ui.Notification.NOTF_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "task_flow_f")
         {
            if (_Task_Flow_F == null)
               _Task_Flow_F = new Ui.Notification.TASK_FLOW_F { _DefaultGateway = this };
         }
         else if (value == "shw_lead_f")
         {
            if (_Shw_Lead_F == null)
               _Shw_Lead_F = new Ui.Leads.SHW_LEAD_F { _DefaultGateway = this };
         }
         else if (value == "inf_lead_f")
         {
            if (_Inf_Lead_F == null)
               _Inf_Lead_F = new Ui.Leads.INF_LEAD_F { _DefaultGateway = this };
         }
         else if (value == "opt_logc_f")
         {
            if (_Opt_Logc_F == null)
               _Opt_Logc_F = new Ui.Activity.OPT_LOGC_F { _DefaultGateway = this };
         }
         else if (value == "opt_task_f")
         {
            if (_Opt_Task_F == null)
               _Opt_Task_F = new Ui.Activity.OPT_TASK_F { _DefaultGateway = this };
         }
         else if (value == "opt_apon_f")
         {
            if (_Opt_Apon_F == null)
               _Opt_Apon_F = new Ui.Activity.OPT_APON_F { _DefaultGateway = this };
         }
         else if (value == "opt_sndf_f")
         {
            if (_Opt_Sndf_F == null)
               _Opt_Sndf_F = new Ui.Activity.OPT_SNDF_F { _DefaultGateway = this };
         }
         else if (value == "opt_clon_f")
         {
            if (_Opt_Clon_F == null)
               _Opt_Clon_F = new Ui.Activity.OPT_CLON_F { _DefaultGateway = this };
         }
         else if (value == "opt_emal_f")
         {
            if (_Opt_Emal_F == null)
               _Opt_Emal_F = new Ui.Activity.OPT_EMAL_F { _DefaultGateway = this };
         }
         else if (value == "fss_show_f")
         {
            if (_Fss_Show_F == null)
               _Fss_Show_F = new Ui.FileServerStorage.FSS_SHOW_F { _DefaultGateway = this };
         }
         else if (value == "inf_cont_f")
         {
            if (_Inf_Cont_F == null)
               _Inf_Cont_F = new Ui.Contacts.INF_CONT_F { _DefaultGateway = this };
         }
         else if (value == "shw_cont_f")
         {
            if (_Shw_Cont_F == null)
               _Shw_Cont_F = new Ui.Contacts.SHW_CONT_F { _DefaultGateway = this };
         }
         else if (value == "tol_deal_f")
         {
            if (_Tol_Deal_F == null)
               _Tol_Deal_F = new Ui.Deals.TOL_DEAL_F { _DefaultGateway = this };
         }
         else if (value == "dtl_deal_f")
         {
            if (_Dtl_Deal_F == null)
               _Dtl_Deal_F = new Ui.Deals.DTL_DEAL_F { _DefaultGateway = this };
         }
         else if (value == "opt_cnvt_f")
         {
            if (_Opt_Cnvt_F == null)
               _Opt_Cnvt_F = new Ui.Activity.OPT_CNVT_F { _DefaultGateway = this };
         }
         else if (value == "shw_acnt_f")
         {
            if (_Shw_Acnt_F == null)
               _Shw_Acnt_F = new Ui.Acounts.SHW_ACNT_F { _DefaultGateway = this };
         }
         else if (value == "inf_acnt_f")
         {
            if (_Inf_Acnt_F == null)
               _Inf_Acnt_F = new Ui.Acounts.INF_ACNT_F { _DefaultGateway = this };
         }
         else if (value == "cmn_dcmt_f")
         {
            if (_Cmn_Dcmt_F == null)
               _Cmn_Dcmt_F = new Ui.PublicInformation.CMN_DCMT_F { _DefaultGateway = this };
         }
         else if (value == "serv_camr_f")
         {
            if (_Serv_Camr_F == null)
               _Serv_Camr_F = new Ui.PublicInformation.SERV_CAMR_F { _DefaultGateway = this };
         }
         else if (value == "serv_dcmt_f")
         {
            if (_Serv_Dcmt_F == null)
               _Serv_Dcmt_F = new Ui.PublicInformation.SERV_DCMT_F { _DefaultGateway = this };
         }
         else if (value == "shw_deal_f")
         {
            if (_Shw_Deal_F == null)
               _Shw_Deal_F = new Ui.Deals.SHW_DEAL_F { _DefaultGateway = this };
         }
         else if (value == "inf_deal_f")
         {
            if (_Inf_Deal_F == null)
               _Inf_Deal_F = new Ui.Deals.INF_DEAL_F { _DefaultGateway = this };
         }
         else if (value == "tol_tsap_f")
         {
            if (_Tol_Tsap_F == null)
               _Tol_Tsap_F = new Ui.TaskAppointment.TOL_TSAP_F { _DefaultGateway = this };
         }
         else if (value == "opt_clnc_f")
         {
            if (_Opt_Clnc_F == null)
               _Opt_Clnc_F = new Ui.Activity.OPT_CLNC_F { _DefaultGateway = this };
         }
         else if (value == "comp_chng_f")
         {
            if (_Comp_Chng_F == null)
               _Comp_Chng_F = new Ui.PublicInformation.COMP_CHNG_F { _DefaultGateway = this };
         }
         else if (value == "tsk_colr_f")
         {
            if (_Tsk_Colr_F == null)
               _Tsk_Colr_F = new Ui.TaskAppointment.TSK_COLR_F { _DefaultGateway = this };
         }
         else if (value == "tsk_ckls_f")
         {
            if (_Tsk_Ckls_F == null)
               _Tsk_Ckls_F = new Ui.TaskAppointment.TSK_CKLS_F { _DefaultGateway = this };
         }
         else if (value == "tsk_tag_f")
         {
            if (_Tsk_Tag_F == null)
               _Tsk_Tag_F = new Ui.TaskAppointment.TSK_TAG_F { _DefaultGateway = this };
         }
         else if (value == "tsk_cmnt_f")
         {
            if (_Tsk_Cmnt_F == null)
               _Tsk_Cmnt_F = new Ui.TaskAppointment.TSK_CMNT_F { _DefaultGateway = this };
         }
         else if (value == "tsk_ckld_f")
         {
            if (_Tsk_Ckld_F == null)
               _Tsk_Ckld_F = new Ui.TaskAppointment.TSK_CKLD_F { _DefaultGateway = this };
         }
         else if (value == "opt_mesg_f")
         {
            if (_Opt_Mesg_F == null)
               _Opt_Mesg_F = new Ui.Activity.OPT_MESG_F { _DefaultGateway = this };
         }
         else if (value == "chg_tarf_f")
         {
            if (_Chg_Tarf_F == null)
               _Chg_Tarf_F = new Ui.PublicInformation.CHG_TARF_F { _DefaultGateway = this };
         }
         else if (value == "hst_urqs_f")
         {
            if (_Hst_Urqs_F == null)
               _Hst_Urqs_F = new Ui.HistoryAction.HST_URQS_F { _DefaultGateway = this };
         }
         else if (value == "opt_rqst_f")
         {
            if (_Opt_Rqst_F == null)
               _Opt_Rqst_F = new Ui.Activity.OPT_RQST_F { _DefaultGateway = this };
         }
         else if (value == "lst_serv_f")
         {
            if (_Lst_Serv_F == null)
               _Lst_Serv_F = new Ui.PublicInformation.LST_SERV_F { _DefaultGateway = this };
         }
         else if (value == "add_serv_f")
         {
            if (_Add_Serv_F == null)
               _Add_Serv_F = new Ui.PublicInformation.ADD_SERV_F { _DefaultGateway = this };
         }
         else if (value == "fin_rslt_f")
         {
            if (_Fin_Rslt_F == null)
               _Fin_Rslt_F = new Ui.TaskAppointment.FIN_RSLT_F { _DefaultGateway = this };
         }
         else if (value == "inf_ctwk_f")
         {
            if (_Inf_Ctwk_F == null)
               _Inf_Ctwk_F = new Ui.PublicInformation.INF_CTWK_F { _DefaultGateway = this };
         }
         else if (value == "hst_logc_f")
         {
            if (_Hst_Logc_F == null)
               _Hst_Logc_F = new Ui.HistoryAction.HST_LOGC_F { _DefaultGateway = this };
         }
         else if (value == "hst_mesg_f")
         {
            if (_Hst_Mesg_F == null)
               _Hst_Mesg_F = new Ui.HistoryAction.HST_MESG_F { _DefaultGateway = this };
         }
         else if (value == "hst_emal_f")
         {
            if (_Hst_Emal_F == null)
               _Hst_Emal_F = new Ui.HistoryAction.HST_EMAL_F { _DefaultGateway = this };
         }
         else if (value == "hst_task_f")
         {
            if (_Hst_Task_F == null)
               _Hst_Task_F = new Ui.HistoryAction.HST_TASK_F { _DefaultGateway = this };
         }
         else if (value == "hst_apon_f")
         {
            if (_Hst_Apon_F == null)
               _Hst_Apon_F = new Ui.HistoryAction.HST_APON_F { _DefaultGateway = this };
         }
         else if (value == "hst_sndf_f")
         {
            if (_Hst_Sndf_F == null)
               _Hst_Sndf_F = new Ui.HistoryAction.HST_SNDF_F { _DefaultGateway = this };
         }
         else if (value == "hst_note_f")
         {
            if (_Hst_Note_F == null)
               _Hst_Note_F = new Ui.HistoryAction.HST_NOTE_F { _DefaultGateway = this };
         }
         else if (value == "tmpl_dfin_f")
         {
            if (_Tmpl_Dfin_F == null)
               _Tmpl_Dfin_F = new Ui.BaseDefination.TMPL_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "hst_finr_f")
         {
            if (_Hst_Finr_F == null)
               _Hst_Finr_F = new Ui.HistoryAction.HST_FINR_F { _DefaultGateway = this };
         }
         else if (value == "add_info_f")
         {
            if (_Add_Info_F == null)
               _Add_Info_F = new Ui.PublicInformation.ADD_INFO_F { _DefaultGateway = this };
         }
         else if (value == "sjbp_dfin_f")
         {
            if (_Sjbp_Dfin_F == null)
               _Sjbp_Dfin_F = new Ui.BaseDefination.SJBP_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "mntn_totl_f")
         {
            if (_Mntn_Totl_F == null)
               _Mntn_Totl_F = new Ui.Notification.MNTN_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "opt_note_f")
         {
            if (_Opt_Note_F == null)
               _Opt_Note_F = new Ui.Activity.OPT_NOTE_F { _DefaultGateway = this };
         }
         else if (value == "hst_utag_f")
         {
            if (_Hst_Utag_F == null)
               _Hst_Utag_F = new Ui.HistoryAction.HST_UTAG_F { _DefaultGateway = this };
         }
         else if (value == "hst_urgn_f")
         {
            if (_Hst_Urgn_F == null)
               _Hst_Urgn_F = new Ui.HistoryAction.HST_URGN_F { _DefaultGateway = this };
         }
         else if (value == "hst_fltr_f")
         {
            if (_Hst_Fltr_F == null)
               _Hst_Fltr_F = new Ui.HistoryAction.HST_FLTR_F { _DefaultGateway = this };
         }
         else if (value == "apbs_dfin_f")
         {
            if (_Apbs_Dfin_F == null)
               _Apbs_Dfin_F = new Ui.BaseDefination.APBS_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "hst_uexf_f")
         {
            if (_Hst_Uexf_F == null)
               _Hst_Uexf_F = new Ui.HistoryAction.HST_UEXF_F { _DefaultGateway = this };
         }
         else if (value == "hst_uctf_f")
         {
            if (_Hst_Uctf_F == null)
               _Hst_Uctf_F = new Ui.HistoryAction.HST_UCTF_F { _DefaultGateway = this };
         }
         else if (value == "opt_aeml_f")
         {
            if (_Opt_Aeml_F == null)
               _Opt_Aeml_F = new Ui.Activity.OPT_AEML_F { _DefaultGateway = this };
         }
         else if (value == "hst_ssid_f")
         {
            if (_Hst_Ssid_f == null)
               _Hst_Ssid_f = new Ui.HistoryAction.HST_SSID_F { _DefaultGateway = this };
         }
         else if (value == "rlat_sinf_f")
         {
            if (_Rlat_Sinf_F == null)
               _Rlat_Sinf_F = new Ui.PublicInformation.RLAT_SINF_F { _DefaultGateway = this };
         }
         else if (value == "rlat_cinf_f")
         {
            if (_Rlat_Cinf_F == null)
               _Rlat_Cinf_F = new Ui.PublicInformation.RLAT_CINF_F { _DefaultGateway = this };
         }
         else if (value == "cmph_dfin_f")
         {
            if (_Cmph_Dfin_F == null)
               _Cmph_Dfin_F = new Ui.BaseDefination.CMPH_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "cjbp_dfin_f")
         {
            if (_Cjbp_Dfin_F == null)
               _Cjbp_Dfin_F = new Ui.BaseDefination.CJBP_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "opt_prjt_f")
         {
            if (_Opt_Prjt_F == null)
               _Opt_Prjt_F = new Ui.Activity.OPT_PRJT_F { _DefaultGateway = this };
         }
         else if (value == "mstt_dfin_f")
         {
            if (_Mstt_Dfin_F == null)
               _Mstt_Dfin_F = new Ui.BaseDefination.MSTT_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "jbpd_dfin_f")
         {
            if (_Jbpd_Dfin_F == null)
               _Jbpd_Dfin_F = new Ui.BaseDefination.JBPD_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "opt_info_f")
         {
            if (_Opt_Info_F == null)
               _Opt_Info_F = new Ui.Activity.OPT_INFO_F { _DefaultGateway = this };
         }
         else if (value == "shw_cmpt_f")
         {
            if (_Shw_Cmpt_F == null)
               _Shw_Cmpt_F = new Ui.Competitor.SHW_CMPT_F { _DefaultGateway = this };
         }
         else if (value == "inf_cmpt_f")
         {
            if (_Inf_Cmpt_F == null)
               _Inf_Cmpt_F = new Ui.Competitor.INF_CMPT_F { _DefaultGateway = this };
         }
         else if (value == "shw_mklt_f")
         {
            if (_Shw_Mklt_F == null)
               _Shw_Mklt_F = new Ui.MarketingList.SHW_MKLT_F { _DefaultGateway = this };
         }
         else if (value == "inf_mklt_f")
         {
            if (_Inf_Mklt_F == null)
               _Inf_Mklt_F = new Ui.MarketingList.INF_MKLT_F { _DefaultGateway = this };
         }
         else if (value == "shw_camp_f")
         {
            if (_Shw_Camp_F == null)
               _Shw_Camp_F = new Ui.Campaign.SHW_CAMP_F { _DefaultGateway = this };
         }
         else if (value == "inf_camp_f")
         {
            if (_Inf_Camp_F == null)
               _Inf_Camp_F = new Ui.Campaign.INF_CAMP_F { _DefaultGateway = this };
         }
         else if (value == "shw_camq_f")
         {
            if (_Shw_Camq_F == null)
               _Shw_Camq_F = new Ui.CampaignQuick.SHW_CAMQ_F { _DefaultGateway = this };
         }
         else if (value == "inf_camq_f")
         {
            if (_Inf_Camq_F == null)
               _Inf_Camq_F = new Ui.CampaignQuick.INF_CAMQ_F { _DefaultGateway = this };
         }
         else if (value == "shw_cama_f")
         {
            if (_Shw_Cama_F == null)
               _Shw_Cama_F = new Ui.CampaignActivity.SHW_CAMA_F { _DefaultGateway = this };
         }
         else if (value == "inf_cama_f")
         {
            if (_Inf_Cama_F == null)
               _Inf_Cama_F = new Ui.CampaignActivity.INF_CAMA_F { _DefaultGateway = this };
         }
         else if (value == "rsl_lead_f")
         {
            if (_Rsl_Lead_F == null)
               _Rsl_Lead_F = new Ui.Leads.RSL_LEAD_F { _DefaultGateway = this };
         }
         else if (value == "shw_oprt_f")
         {
            if (_Shw_Oprt_F == null)
               _Shw_Oprt_F = new Ui.Leads.SHW_OPRT_F { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Frst_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "frst_page_f"},
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 10 /* Execute Actn_Calf_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void Regn_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regn_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void Epit_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "epit_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void Btrf_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "btrf_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void Cash_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cash_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void Regl_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regl_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void Rqrq_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rqrq_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void Dcsp_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dcsp_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Orgn_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "orgn_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Cust_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_cust_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ADM_CUST_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void Pay_Mtod_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pay_mtod_f"},
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Chng_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_chng_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void Lst_Cust_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "lst_cust_f"},
                  new Job(SendType.SelfToUserInterface, "LST_CUST_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "LST_CUST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "LST_CUST_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "LST_CUST_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Cust_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_cust_f"},
                  new Job(SendType.SelfToUserInterface, "INF_CUST_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CUST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CUST_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "INF_CUST_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void Jobp_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "jobp_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "JOBP_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "JOBP_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "JOBP_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "JOBP_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void Act_Logc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "act_logc_f"},
                  new Job(SendType.SelfToUserInterface, "ACT_LOGC_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ACT_LOGC_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ACT_LOGC_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ACT_LOGC_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 18
      /// </summary>
      /// <param name="job"></param>
      private void Act_Sndf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "act_sndf_f"},
                  new Job(SendType.SelfToUserInterface, "ACT_SNDF_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ACT_SNDF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ACT_SNDF_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ACT_SNDF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 19
      /// </summary>
      /// <param name="job"></param>
      private void Act_Trat_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "act_trat_f"},
                  new Job(SendType.SelfToUserInterface, "ACT_TRAT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ACT_TRAT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ACT_TRAT_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ACT_TRAT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void Isic_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "isic_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void Notf_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "notf_totl_f"},
                  new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "NOTF_TOTL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 22
      /// </summary>
      /// <param name="job"></param>
      private void Task_Flow_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "task_flow_f"},
                  new Job(SendType.SelfToUserInterface, "TASK_FLOW_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TASK_FLOW_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TASK_FLOW_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "TASK_FLOW_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 23
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Lead_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_lead_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SHW_LEAD_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 24
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Lead_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_lead_f"},
                  new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 25
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Logc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_logc_f"},                  
                  new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 26
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Task_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_task_f"},                  
                  new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_TASK_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 27
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Apon_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_apon_f"},
                  new Job(SendType.SelfToUserInterface, "OPT_APON_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_APON_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_APON_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 28
      /// </summary>
      /// <param name="job"></param>
      private void Stng_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "stng_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "STNG_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "STNG_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "STNG_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "STNG_DFIN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 29
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Sndf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_sndf_f"},
                  new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_SNDF_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 30
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Clon_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_clon_f"},
                  new Job(SendType.SelfToUserInterface, "OPT_CLON_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_CLON_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_CLON_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 31
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Emal_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_emal_f"},
                  new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_EMAL_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 32
      /// </summary>
      /// <param name="job"></param>
      private void Fss_Show_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "fss_show_f"},
                  new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "OPT_LOGC_F", 05 /* Execute OpenDrawer */){Executive = ExecutiveType.Asynchronous, Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 33
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Cont_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_cont_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SHW_CONT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 34
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Cont_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_cont_f"},
                  new Job(SendType.SelfToUserInterface, "INF_CONT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CONT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CONT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INF_CONT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 35
      /// </summary>
      /// <param name="job"></param>
      private void Tol_Deal_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tol_deal_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 36
      /// </summary>
      /// <param name="job"></param>
      private void Dtl_Deal_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dtl_deal_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "DTL_DEAL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "DTL_DEAL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "DTL_DEAL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 37
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Cnvt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_cnvt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_CNVT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_CNVT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_CNVT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 38
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Acnt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_acnt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SHW_ACNT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 39
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Acnt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_acnt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INF_ACNT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void Cmn_Dcmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmn_dcmt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 41
      /// </summary>
      /// <param name="job"></param>
      private void Serv_Camr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "serv_camr_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SERV_CAMR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SERV_CAMR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SERV_CAMR_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 42
      /// </summary>
      /// <param name="job"></param>
      private void Serv_Dcmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "serv_dcmt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SERV_DCMT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SERV_DCMT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SERV_DCMT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 43
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Deal_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_deal_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_DEAL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_DEAL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SHW_DEAL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 44
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Deal_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_deal_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_DEAL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_DEAL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INF_DEAL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 45
      /// </summary>
      /// <param name="job"></param>
      private void Tol_Tsap_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tol_tsap_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TOL_TSAP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TOL_TSAP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TOL_TSAP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 46
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Clnc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_clnc_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_CLNC_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_CLNC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_CLNC_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 47
      /// </summary>
      /// <param name="job"></param>
      private void Comp_Chng_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "comp_chng_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "COMP_CHNG_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 48
      /// </summary>
      /// <param name="job"></param>
      private void Tsk_Colr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tsk_colr_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TSK_COLR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TSK_COLR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TSK_COLR_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 49
      /// </summary>
      /// <param name="job"></param>
      private void Tsk_Ckls_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tsk_ckls_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TSK_CKLS_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TSK_CKLS_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TSK_CKLS_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 50
      /// </summary>
      /// <param name="job"></param>
      private void Tsk_Tag_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tsk_tag_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TSK_TAG_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 51
      /// </summary>
      /// <param name="job"></param>
      private void Tsk_Cmnt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tsk_cmnt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TSK_CMNT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TSK_CMNT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TSK_CMNT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 52
      /// </summary>
      /// <param name="job"></param>
      private void Tsk_Ckld_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tsk_ckld_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TSK_CKLD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TSK_CKLD_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TSK_CKLD_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 53
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Mesg_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_mesg_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_MESG_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 54
      /// </summary>
      /// <param name="job"></param>
      private void Chg_Tarf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "chg_tarf_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CHG_TARF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CHG_TARF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CHG_TARF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 55
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Urqs_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_urqs_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_URQS_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_URQS_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_URQS_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 56
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_rqst_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 05 /* Execute OpenDrawer */)//{Executive = ExecutiveType.Asynchronous}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 57
      /// </summary>
      /// <param name="job"></param>
      private void Lst_Serv_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "lst_serv_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "LST_SERV_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "LST_SERV_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "LST_SERV_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 58
      /// </summary>
      /// <param name="job"></param>
      private void Add_Serv_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "add_serv_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ADD_SERV_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADD_SERV_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADD_SERV_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 59
      /// </summary>
      /// <param name="job"></param>
      private void Fin_Rslt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "fin_rslt_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 60
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Ctwk_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_ctwk_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INF_CTWK_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 61
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Logc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_logc_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_LOGC_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_LOGC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_LOGC_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 62
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Mesg_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_mesg_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_MESG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_MESG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_MESG_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 63
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Emal_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_emal_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_EMAL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_EMAL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_EMAL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 64
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Task_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_task_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_TASK_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_TASK_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_TASK_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 65
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Apon_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_apon_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_APON_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_APON_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_APON_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 66
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Sndf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_sndf_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_SNDF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_SNDF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_SNDF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 67
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Note_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_note_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_NOTE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_NOTE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_NOTE_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 68
      /// </summary>
      /// <param name="job"></param>

      /// <summary>
      /// Code 69
      /// </summary>
      /// <param name="job"></param>
      private void Tmpl_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "TMPL_DFIN_F"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "TMPL_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TMPL_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TMPL_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 70
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Finr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_finr_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_FINR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_FINR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_FINR_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 71
      /// </summary>
      /// <param name="job"></param>
      private void Add_Info_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "add_info_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADD_INFO_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 72
      /// </summary>
      /// <param name="job"></param>
      private void Sjbp_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "sjbp_dfin_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SJBP_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SJBP_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SJBP_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 73
      /// </summary>
      /// <param name="job"></param>
      private void Mntn_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mntn_totl_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "MNTN_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MNTN_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MNTN_TOTL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 75
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Note_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_note_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_NOTE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_NOTE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_NOTE_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 76
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Utag_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_utag_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_UTAG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_UTAG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_UTAG_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 77
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Urgn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_urgn_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_URGN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_URGN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_URGN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 78
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Fltr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_fltr_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_FLTR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_FLTR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_FLTR_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 79
      /// </summary>
      /// <param name="job"></param>
      private void Apbs_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "apbs_dfin_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 80
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Uexf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_uexf_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_UEXF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_UEXF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_UEXF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 81
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Uctf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_uctf_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_UCTF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_UCTF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_UCTF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 82
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Aeml_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_aeml_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_AEML_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_AEML_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_AEML_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 83
      /// </summary>
      /// <param name="job"></param>
      private void Hst_Ssid_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hst_ssid_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "HST_SSID_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HST_SSID_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HST_SSID_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 84
      /// </summary>
      /// <param name="job"></param>
      private void Rlat_Sinf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rlat_sinf_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RLAT_SINF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RLAT_SINF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "RLAT_SINF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 85
      /// </summary>
      /// <param name="job"></param>
      private void Rlat_Cinf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rlat_cinf_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RLAT_CINF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RLAT_CINF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "RLAT_CINF_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 86
      /// </summary>
      /// <param name="job"></param>
      private void Cmph_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmph_dfin_f"},
                  //new Job(SendType.SelfToUserInterface, "TOL_DEAL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CMPH_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMPH_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CMPH_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 87
      /// </summary>
      /// <param name="job"></param>
      private void Cjbp_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cjbp_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "CJBP_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CJBP_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CJBP_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CJBP_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 88
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Prjt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_prjt_f"},
                  new Job(SendType.SelfToUserInterface, "OPT_PRJT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "OPT_PRJT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_PRJT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_PRJT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 89
      /// </summary>
      /// <param name="job"></param>
      private void Mstt_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstt_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "MSTT_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "MSTT_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTT_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MSTT_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 90
      /// </summary>
      /// <param name="job"></param>
      private void Jbpd_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "jbpd_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "JBPD_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "JBPD_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "JBPD_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "JBPD_DFIN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 91
      /// </summary>
      /// <param name="job"></param>
      private void Opt_Info_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "opt_info_f"},
                  new Job(SendType.SelfToUserInterface, "OPT_INFO_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OPT_INFO_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OPT_INFO_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "OPT_INFO_F", 05 /* Execute OpenDrawer */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 92
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Cmpt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_cmpt_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CMPT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_CMPT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_CMPT_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "SHW_CMPT_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 93
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Cmpt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_cmpt_f"},
                  new Job(SendType.SelfToUserInterface, "INF_CMPT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CMPT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CMPT_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "INF_CMPT_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 94
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Mklt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_mklt_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_MKLT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_MKLT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_MKLT_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "SHW_MKLT_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 95
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Mklt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_mklt_f"},
                  new Job(SendType.SelfToUserInterface, "INF_MKLT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_MKLT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_MKLT_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "INF_MKLT_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 96
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Camp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_camp_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CAMP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_CAMP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_CAMP_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "SHW_CAMP_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 97
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Camp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_camp_f"},
                  new Job(SendType.SelfToUserInterface, "INF_CAMP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CAMP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CAMP_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "INF_CAMP_F", 03 /* Execute Paint */),
                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 98
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Camq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_camq_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CAMQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_CAMQ_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_CAMQ_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "SHW_CAMQ_F", 03 /* Execute Paint */),
                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 99
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Camq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_camq_f"},
                  new Job(SendType.SelfToUserInterface, "INF_CAMQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CAMQ_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CAMQ_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "INF_CAMQ_F", 03 /* Execute Paint */),
                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 100
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Cama_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_cama_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_CAMA_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_CAMA_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_CAMA_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "SHW_CAMA_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 101
      /// </summary>
      /// <param name="job"></param>
      private void Inf_Cama_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inf_cama_f"},
                  new Job(SendType.SelfToUserInterface, "INF_CAMA_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INF_CAMA_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INF_CAMA_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "INF_CAMA_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 102
      /// </summary>
      /// <param name="job"></param>
      private void Rsl_Lead_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rsl_lead_f"},
                  new Job(SendType.SelfToUserInterface, "RSL_LEAD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RSL_LEAD_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RSL_LEAD_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "RSL_LEAD_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 103
      /// </summary>
      /// <param name="job"></param>
      private void Shw_Oprt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "shw_oprt_f"},
                  new Job(SendType.SelfToUserInterface, "SHW_OPRT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHW_OPRT_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SHW_OPRT_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "SHW_OPRT_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
