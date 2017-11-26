using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Scsc.Code
{
   partial class Scsc
   {
      public ISendRequest _Wall { get; set; }

      internal Ui.MasterPage.MAIN_PAGE_F _Main_Page_F { get; set; }
      internal Ui.MasterPage.FRST_PAGE_F _Frst_Page_F { get; set; }
      internal Ui.MasterPage.STRT_MENU_F _Strt_Menu_F { get; set; }
      //internal Ui.MasterPage.MSTR_PAGE_F _Mstr_Page_F { get; set; }
      internal Ui.Regulation.MSTR_REGL_F _Mstr_Regl_F { get; set; }
      internal Ui.Regulation.REGL_ACNT_F _Regl_Acnt_F { get; set; }
      internal Ui.Regulation.REGL_EXPN_F _Regl_Expn_F { get; set; }
      internal Ui.Regulation.REGL_DCMT_F _Regl_Dcmt_F { get; set; }
      internal Ui.Regulation.MSTR_DCMT_F _Mstr_Dcmt_F { get; set; }
      internal Ui.Method.MSTR_MTOD_F _Mstr_Mtod_F { get; set; }
      internal Ui.Region.MSTR_REGN_F _Mstr_Regn_F { get; set; }
      internal Ui.Regulation.MSTR_EPIT_F _Mstr_Epit_F { get; set; }
      internal Ui.Club.MSTR_CLUB_F _Mstr_Club_F { get; set; }
      internal Ui.MainSubStat.MAIN_SUBS_F _Main_Subs_F { get; set; }
      internal Ui.RequestType.RQST_TYPE_F _Rqst_Type_F { get; set; }
      internal Ui.RequesterType.RQTR_TYPE_F _Rqtr_Type_F { get; set; }
      //internal Ui.Admission.ADM_RQST_F _Adm_Rqst_F { get; set; }
      //internal Ui.Admission.ADM_FSUM_F _Adm_Fsum_F { get; set; }
      //internal Ui.Admission.ADM_SEXP_F _Adm_Sexp_F { get; set; }
      //internal Ui.Admission.ADM_REXP_F _Adm_Rexp_F { get; set; }
      //internal Ui.Admission.ADM_SAVE_F _Adm_Save_F { get; set; }
      //internal Ui.Attendance.ATN_SAVE_F _Atn_Save_F { get; set; }
      //internal Ui.Test.TST_RQST_F _Tst_Rqst_F { get; set; }
      //internal Ui.Test.TST_SEXP_F _Tst_Sexp_F { get; set; }
      //internal Ui.Test.TST_REXP_F _Tst_Rexp_F { get; set; }
      //internal Ui.Test.TST_SAVE_F _Tst_Save_F { get; set; }
      //internal Ui.Campitition.CMP_RQST_F _Cmp_Rqst_F { get; set; }
      //internal Ui.Campitition.CMP_SEXP_F _Cmp_Sexp_F { get; set; }
      //internal Ui.Campitition.CMP_REXP_F _Cmp_Rexp_F { get; set; }
      //internal Ui.Campitition.CMP_SAVE_F _Cmp_Save_F { get; set; }
      //internal Ui.PhysicalFitness.PSF_RQST_F _Psf_Rqst_F { get; set; }
      //internal Ui.PhysicalFitness.PSF_SEXP_F _Psf_Sexp_F { get; set; }
      //internal Ui.PhysicalFitness.PSF_REXP_F _Psf_Rexp_F { get; set; }
      //internal Ui.PhysicalFitness.PSF_SAVE_F _Psf_Save_F { get; set; }
      //internal Ui.CalculateCalorie.CLC_RQST_F _Clc_Rqst_F { get; set; }
      //internal Ui.CalculateCalorie.CLC_SEXP_F _Clc_Sexp_F { get; set; }
      //internal Ui.CalculateCalorie.CLC_REXP_F _Clc_Rexp_F { get; set; }
      //internal Ui.CalculateCalorie.CLC_SAVE_F _Clc_Save_F { get; set; }
      //internal Ui.HeartZone.HRZ_RQST_F _Hrz_Rqst_F { get; set; }
      //internal Ui.HeartZone.HRZ_SEXP_F _Hrz_Sexp_F { get; set; }
      //internal Ui.HeartZone.HRZ_REXP_F _Hrz_Rexp_F { get; set; }
      //internal Ui.HeartZone.HRZ_SAVE_F _Hrz_Save_F { get; set; }
      //internal Ui.Exam.EXM_RQST_F _Exm_Rqst_F { get; set; }
      //internal Ui.Exam.EXM_SEXP_F _Exm_Sexp_F { get; set; }
      //internal Ui.Exam.EXM_REXP_F _Exm_Rexp_F { get; set; }
      //internal Ui.Exam.EXM_SAVE_F _Exm_Save_F { get; set; }
      internal Ui.Common.LSI_FLDF_F _Lsi_Fldf_F { get; set; }
      internal Ui.Common.LSI_FDLF_F _Lsi_Fdlf_F { get; set; }
      internal Ui.Common.ALL_FLDF_F _All_Fldf_F { get; set; }
      //internal Ui.FighterPublic.PBL_RQST_F _Pbl_Rqst_F { get; set; }
      //internal Ui.FighterPublic.PBL_SEXP_F _Pbl_Sexp_F { get; set; }
      //internal Ui.FighterPublic.PBL_REXP_F _Pbl_Rexp_F { get; set; }
      //internal Ui.FighterPublic.PBL_SAVE_F _Pbl_Save_F { get; set; }
      //internal Ui.ChangeMethodCategory.CMC_RQST_F _Cmc_Rqst_F { get; set; }
      //internal Ui.ChangeMethodCategory.CMC_SEXP_F _Cmc_Sexp_F { get; set; }
      //internal Ui.ChangeMethodCategory.CMC_REXP_F _Cmc_Rexp_F { get; set; }
      //internal Ui.ChangeMethodCategory.CMC_SAVE_F _Cmc_Save_F { get; set; }
      //internal Ui.UserClubCard.UCC_RQST_F _Ucc_Rqst_F { get; set; }
      //internal Ui.UserClubCard.UCC_SEXP_F _Ucc_Sexp_F { get; set; }
      //internal Ui.UserClubCard.UCC_REXP_F _Ucc_Rexp_F { get; set; }
      //internal Ui.UserClubCard.UCC_SAVE_F _Ucc_Save_F { get; set; }
      internal Ui.Document.CMN_DCMT_F _Cmn_Dcmt_F { get; set; }
      //internal Ui.MethodClubCard.MCC_RQST_F _Mcc_Rqst_F { get; set; }
      //internal Ui.MethodClubCard.MCC_SEXP_F _Mcc_Sexp_F { get; set; }
      //internal Ui.MethodClubCard.MCC_REXP_F _Mcc_Rexp_F { get; set; }
      //internal Ui.MethodClubCard.MCC_SAVE_F _Mcc_Save_F { get; set; }
      internal Ui.Admission.ADM_TOTL_F _Adm_Totl_F { get; set; }
      internal Ui.Diseases.CMN_DISE_F _Cmn_Dise_F { get; set; }
      internal Ui.Cash.PAY_CASH_F _Pay_Cash_F { get; set; }
      internal Ui.CalculateExpense.CAL_EXPN_F _Cal_Expn_F { get; set; }
      internal Ui.CalculateExpense.CAL_CEXC_F _Cal_Cexc_F { get; set; }
      internal Ui.Admission.ADM_CHNG_F _Adm_Chng_F { get; set; }
      internal Ui.Exam.EXM_TOTL_F _Exm_Totl_F { get; set; }
      internal Ui.PhysicalFitness.PSF_TOTL_F _Psf_Totl_F { get; set; }
      internal Ui.CalculateCalorie.CLC_TOTL_F _Clc_Totl_F { get; set; }
      internal Ui.HeartZone.HRZ_TOTL_F _Hrz_Totl_F { get; set; }
      internal Ui.ChangeMethodCategory.CMC_TOTL_F _Cmc_Totl_F { get; set; }
      internal Ui.Test.TST_TOTL_F _Tst_Totl_F { get; set; }
      internal Ui.Campitition.CMP_TOTL_F _Cmp_Totl_F { get; set; }
      //internal Ui.Committee.COM_TOTL_F _Com_Totl_F { get; set; }
      internal Ui.MethodClubCard.MCC_TOTL_F _Mcc_Totl_F { get; set; }
      internal Ui.Insurance.INS_TOTL_F _Ins_Totl_F { get; set; }
      internal Ui.Settings.CFG_STNG_F _Cfg_Stng_F { get; set; }
      internal Ui.EnablingDisabling.ADM_ENDS_F _Adm_Ends_F { get; set; }
      internal Ui.EnablingDisabling.ADM_DSEN_F _Adm_Dsen_F { get; set; }
      internal Ui.ReportManager.RPT_MNGR_F _Rpt_Mngr_F { get; set; }
      internal Ui.ReportManager.RPT_LRFM_F _Rpt_Lrfm_F { get; set; }
      internal Ui.PaymentMethod.PAY_MTOD_F _Pay_Mtod_F { get; set; }
      internal Ui.ReceiptAnnouncement.MNG_RCAN_F _Mng_Rcan_F { get; set; }
      internal Ui.Notifications.NTF_TOTL_F _Ntf_Totl_F { get; set; }
      internal Ui.ReportManager.RPT_LIST_F _Rpt_List_F { get; set; }
      internal Ui.CalculateExpense.CAL_CEXC_P _Cal_Cexc_P { get; set; }
      internal Ui.ReportManager.ACN_LIST_F _Acn_List_F { get; set; }
      internal Ui.OtherIncome.OIC_TOTL_F _Oic_Totl_F { get; set; }
      internal Ui.POS.POS_TOTL_F _Pos_Totl_F { get; set; }
      internal Ui.Reason.CMN_RESN_F _Cmn_Resn_F { get; set; }
      internal Ui.OtherIncome.OIC_SRZH_F _Oic_Srzh_F { get; set; }
      internal Ui.OtherIncome.OIC_SONE_F _Oic_Sone_F { get; set; }
      internal Ui.OtherIncome.OIC_SMOR_F _Oic_Smor_F { get; set; }
      internal Ui.Settings.BAS_CPR_F _Bas_Cpr_F { get; set; }
      internal Ui.Admission.NEW_FNGR_F _New_Fngr_F { get; set; }
      internal Ui.FingerPrint.FNGR_RSVD_F _Fngr_Rsvd_F { get; set; }
      internal Ui.Organ.ORGN_TOTL_F _Orgn_Totl_F { get; set; }
      internal Ui.ReportManager.RQST_TRAC_F _Rqst_Trac_F { get; set; }
      internal Ui.Notifications.WHO_ARYU_F _Who_Aryu_F { get; set; }
      internal Ui.Financial.RFD_TOTL_F _Rfd_Totl_F { get; set; }
      internal Ui.ChangeRials.GLR_TOTL_F _Glr_Totl_F { get; set; }
      internal Ui.OtherIncome.OIC_SMSN_F _Oic_Smsn_F { get; set; }
      internal Ui.Notifications.CHOS_CLAS_F _Chos_Clas_F { get; set; }
      internal Ui.MessageBroadcast.MSGB_TOTL_F _Msgb_Totl_F { get; set; }
      internal Ui.DebitsList.DEBT_LIST_F _Debt_List_F { get; set; }
      internal Ui.BodyFitness.BDF_PROS_F _Bdf_Pros_F { get; set; }
      internal Ui.BodyFitness.BSC_BMOV_F _Bsc_Bmov_F { get; set; }
      internal Ui.AggregateOperation.AOP_MBSP_F _Aop_Mbsp_F { get; set; }
      internal Ui.AggregateOperation.AOP_CBMT_F _Aop_Cbmt_F { get; set; }
      internal Ui.AggregateOperation.AOP_MTOD_F _Aop_Mtod_F { get; set; }
      internal Ui.AggregateOperation.AOP_ATTN_F _Aop_Attn_F { get; set; }
      internal Ui.Admission.ADM_FIGH_F _Adm_Figh_F { get; set; }
      internal Ui.Admission.ADM_MBCO_F _Adm_Mbco_F { get; set; }
      internal Ui.HumanResource.ADM_HRSR_F _Adm_Hrsr_F { get; set; }
      internal Ui.OtherIncome.ADM_BRSR_F _Adm_Brsr_F { get; set; }
      internal Ui.AggregateOperation.AOP_BUFE_F _Aop_Bufe_F { get; set; }
      internal Ui.HumanResource.ADM_HRSC_F _Adm_Hrsc_F { get; set; }
      internal Ui.Admission.ADM_MBFZ_F _Adm_Mbfz_F { get; set; }
      internal Ui.Admission.ADM_MBSM_F _Adm_Mbsm_F { get; set; }
      internal Ui.ReportManager.RPT_PMMT_F _Rpt_Pmmt_F { get; set; }
      internal Ui.DataAnalysis.DAP_PIVT_F _Dap_Pivt_F { get; set; }
      internal Ui.DataAnalysis.DAP_CHRT_F _Dap_Chrt_F { get; set; }
      internal Ui.Settings.CNF_STNG_F _Cnf_Stng_F { get; set; }
      internal Ui.DataAnalysis.DAP_DSHB_F _Dap_Dshb_F { get; set; }
      internal Ui.Notifications.ATTN_DAYN_F _Attn_Dayn_F { get; set; }
      internal Ui.UserAction.USR_ACTN_F _Usr_Actn_F { get; set; }
      internal Ui.UserAction.USR_CTBL_F _Usr_Ctbl_F { get; set; }
      internal Ui.BaseDefinition.BAS_DFIN_F _Bas_Dfin_F { get; set; }
      internal Ui.BaseDefinition.BAS_ADCH_F _Bas_Adch_F { get; set; }
      internal Ui.BaseDefinition.BAS_ADCL_F _Bas_Adcl_F { get; set; }
      internal Ui.BaseDefinition.BAS_GRUC_F _Bas_Gruc_F { get; set; }
      internal Ui.GateEntryExit.GATE_ACTN_F _Gate_Actn_F { get; set; }
      internal Ui.BaseDefinition.BAS_WKDY_F _Bas_Wkdy_F { get; set; }
      internal Ui.Cash.TRAN_EXPN_F _Tran_Expn_F { get; set; }
      internal Ui.Admission.MBSP_CHNG_F _Mbsp_Chng_F { get; set; }

      /// Show Change
      internal Ui.Admission.ShowChanges.SHOW_ATRQ_F _Show_Atrq_F { get; set; }
      internal Ui.Admission.ShowChanges.SHOW_ACRQ_F _Show_Acrq_F { get; set; }
      internal Ui.ChangeMethodCategory.ShowChanges.SHOW_CTRQ_F _Show_Ctrq_F { get; set; }
      internal Ui.EnablingDisabling.ShowChanges.SHOW_DERQ_F _Show_Derq_F { get; set; }
      internal Ui.Admission.ShowChanges.SHOW_UCRQ_F _Show_Ucrq_F { get; set; }
      internal Ui.OtherIncome.ShowChanges.SHOW_OMRQ_F _Show_Omrq_F { get; set; }
      internal Ui.OtherExpense.ShowChanges.SHOW_EMRQ_F _Show_Emrq_F { get; set; }
      internal Ui.Financial.ShowChanges.SHOW_RFDT_F _Show_Rfdt_F { get; set; }
      internal Ui.ChangeRials.ShowChanges.SHOW_GLRL_F _Show_Glrl_F { get; set; }
   }
}
