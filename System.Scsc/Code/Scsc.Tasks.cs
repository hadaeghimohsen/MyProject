using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Scsc.Code
{
   partial class Scsc
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "mstr_page_f")
         {
            //if (_Mstr_Page_F == null)
            //   _Mstr_Page_F = new Ui.MasterPage.MSTR_PAGE_F { _DefaultGateway = this };
         }
         else if (value == "strt_menu_f")
         {
            if (_Strt_Menu_F == null)
               _Strt_Menu_F = new Ui.MasterPage.STRT_MENU_F { _DefaultGateway = this };
         }
         else if (value == "mstr_regl_f")
         {
            if (_Mstr_Regl_F == null)
               _Mstr_Regl_F = new Ui.Regulation.MSTR_REGL_F { _DefaultGateway = this };
         }
         else if (value == "regl_acnt_f")
         {
            if (_Regl_Acnt_F == null)
               _Regl_Acnt_F = new Ui.Regulation.REGL_ACNT_F { _DefaultGateway = this };
         }
         else if (value == "regl_expn_f")
         {
            if (_Regl_Expn_F == null)
               _Regl_Expn_F = new Ui.Regulation.REGL_EXPN_F { _DefaultGateway = this };
         }
         else if (value == "regl_dcmt_f")
         {
            if (_Regl_Dcmt_F == null)
               _Regl_Dcmt_F = new Ui.Regulation.REGL_DCMT_F { _DefaultGateway = this };
         }
         else if (value == "mstr_dcmt_f")
         {
            if (_Mstr_Dcmt_F == null)
               _Mstr_Dcmt_F = new Ui.Regulation.MSTR_DCMT_F { _DefaultGateway = this };
         }
         else if (value == "mstr_mtod_f")
         {
            //if (_Mstr_Mtod_F == null)
            //   _Mstr_Mtod_F = new Ui.Method.MSTR_MTOD_F { _DefaultGateway = this };
         }
         else if (value == "mstr_regn_f")
         {
            //if (_Mstr_Regn_F == null)
            //   _Mstr_Regn_F = new Ui.Region.MSTR_REGN_F { _DefaultGateway = this };
         }
         else if (value == "mstr_epit_f")
         {
            if (_Mstr_Epit_F == null)
               _Mstr_Epit_F = new Ui.Regulation.MSTR_EPIT_F { _DefaultGateway = this };
         }
         else if (value == "mstr_club_f")
         {
            //if (_Mstr_Club_F == null)
            //   _Mstr_Club_F = new Ui.Club.MSTR_CLUB_F { _DefaultGateway = this };
         }
         else if (value == "main_subs_f")
         {
            //if (_Main_Subs_F == null)
            //   _Main_Subs_F = new Ui.MainSubStat.MAIN_SUBS_F { _DefaultGateway = this };
         }
         else if (value == "rqst_type_f")
         {
            //if (_Rqst_Type_F == null)
            //   _Rqst_Type_F = new Ui.RequestType.RQST_TYPE_F { _DefaultGateway = this };
         }
         else if (value == "rqtr_type_f")
         {
            //if (_Rqtr_Type_F == null)
            //   _Rqtr_Type_F = new Ui.RequesterType.RQTR_TYPE_F { _DefaultGateway = this };
         }
         //else if (value == "adm_rqst_f")
         //{
         //   if (_Adm_Rqst_F == null)
         //      _Adm_Rqst_F = new Ui.Admission.ADM_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "adm_fsum_f")
         //{
         //   if (_Adm_Fsum_F == null)
         //      _Adm_Fsum_F = new Ui.Admission.ADM_FSUM_F { _DefaultGateway = this };
         //}
         //else if (value == "adm_sexp_f")
         //{
         //   if (_Adm_Sexp_F == null)
         //      _Adm_Sexp_F = new Ui.Admission.ADM_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "adm_rexp_f")
         //{
         //   if (_Adm_Rexp_F == null)
         //      _Adm_Rexp_F = new Ui.Admission.ADM_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "adm_save_f")
         //{
         //   if (_Adm_Save_F == null)
         //      _Adm_Save_F = new Ui.Admission.ADM_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "atn_save_f")
         //{
         //   if (_Atn_Save_F == null)
         //      _Atn_Save_F = new Ui.Attendance.ATN_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "tst_rqst_f")
         //{
         //   if (_Tst_Rqst_F == null)
         //      _Tst_Rqst_F = new Ui.Test.TST_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "tst_sexp_f")
         //{
         //   if (_Tst_Sexp_F == null)
         //      _Tst_Sexp_F = new Ui.Test.TST_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "tst_rexp_f")
         //{
         //   if (_Tst_Rexp_F == null)
         //      _Tst_Rexp_F = new Ui.Test.TST_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "tst_save_f")
         //{
         //   if (_Tst_Save_F == null)
         //      _Tst_Save_F = new Ui.Test.TST_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "cmp_rqst_f")
         //{
         //   if (_Cmp_Rqst_F == null)
         //      _Cmp_Rqst_F = new Ui.Campitition.CMP_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "cmp_sexp_f")
         //{
         //   if (_Cmp_Sexp_F == null)
         //      _Cmp_Sexp_F = new Ui.Campitition.CMP_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "cmp_rexp_f")
         //{
         //   if (_Cmp_Rexp_F == null)
         //      _Cmp_Rexp_F = new Ui.Campitition.CMP_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "cmp_save_f")
         //{
         //   if (_Cmp_Save_F == null)
         //      _Cmp_Save_F = new Ui.Campitition.CMP_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "psf_rqst_f")
         //{
         //   if (_Psf_Rqst_F == null)
         //      _Psf_Rqst_F = new Ui.PhysicalFitness.PSF_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "psf_sexp_f")
         //{
         //   if (_Psf_Sexp_F == null)
         //      _Psf_Sexp_F = new Ui.PhysicalFitness.PSF_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "psf_rexp_f")
         //{
         //   if (_Psf_Rexp_F == null)
         //      _Psf_Rexp_F = new Ui.PhysicalFitness.PSF_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "psf_save_f")
         //{
         //   if (_Psf_Save_F == null)
         //      _Psf_Save_F = new Ui.PhysicalFitness.PSF_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "clc_rqst_f")
         //{
         //   if (_Clc_Rqst_F == null)
         //      _Clc_Rqst_F = new Ui.CalculateCalorie.CLC_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "clc_sexp_f")
         //{
         //   if (_Clc_Sexp_F == null)
         //      _Clc_Sexp_F = new Ui.CalculateCalorie.CLC_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "clc_rexp_f")
         //{
         //   if (_Clc_Rexp_F == null)
         //      _Clc_Rexp_F = new Ui.CalculateCalorie.CLC_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "clc_save_f")
         //{
         //   if (_Clc_Save_F == null)
         //      _Clc_Save_F = new Ui.CalculateCalorie.CLC_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "hrz_rqst_f")
         //{
         //   if (_Hrz_Rqst_F == null)
         //      _Hrz_Rqst_F = new Ui.HeartZone.HRZ_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "hrz_sexp_f")
         //{
         //   if (_Hrz_Sexp_F == null)
         //      _Hrz_Sexp_F = new Ui.HeartZone.HRZ_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "hrz_rexp_f")
         //{
         //   if (_Hrz_Rexp_F == null)
         //      _Hrz_Rexp_F = new Ui.HeartZone.HRZ_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "hrz_save_f")
         //{
         //   if (_Hrz_Save_F == null)
         //      _Hrz_Save_F = new Ui.HeartZone.HRZ_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "exm_rqst_f")
         //{
         //   if (_Exm_Rqst_F == null)
         //      _Exm_Rqst_F = new Ui.Exam.EXM_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "exm_sexp_f")
         //{
         //   if (_Exm_Sexp_F == null)
         //      _Exm_Sexp_F = new Ui.Exam.EXM_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "exm_rexp_f")
         //{
         //   if (_Exm_Rexp_F == null)
         //      _Exm_Rexp_F = new Ui.Exam.EXM_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "exm_save_f")
         //{
         //   if (_Exm_Save_F == null)
         //      _Exm_Save_F = new Ui.Exam.EXM_SAVE_F { _DefaultGateway = this };
         //}
         else if (value == "lsi_fldf_f")
         {
            if (_Lsi_Fldf_F == null)
               _Lsi_Fldf_F = new Ui.Common.LSI_FLDF_F { _DefaultGateway = this };
         }
         else if (value == "all_fldf_f")
         {
            if (_All_Fldf_F == null)
               _All_Fldf_F = new Ui.Common.ALL_FLDF_F { _DefaultGateway = this };
         }
         //else if (value == "pbl_rqst_f")
         //{
         //   if (_Pbl_Rqst_F == null)
         //      _Pbl_Rqst_F = new Ui.FighterPublic.PBL_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "pbl_sexp_f")
         //{
         //   if (_Pbl_Sexp_F == null)
         //      _Pbl_Sexp_F = new Ui.FighterPublic.PBL_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "pbl_rexp_f")
         //{
         //   if (_Pbl_Rexp_F == null)
         //      _Pbl_Rexp_F = new Ui.FighterPublic.PBL_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "pbl_save_f")
         //{
         //   if (_Pbl_Save_F == null)
         //      _Pbl_Save_F = new Ui.FighterPublic.PBL_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "cmc_rqst_f")
         //{
         //   if (_Cmc_Rqst_F == null)
         //      _Cmc_Rqst_F = new Ui.ChangeMethodCategory.CMC_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "cmc_sexp_f")
         //{
         //   if (_Cmc_Sexp_F == null)
         //      _Cmc_Sexp_F = new Ui.ChangeMethodCategory.CMC_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "cmc_rexp_f")
         //{
         //   if (_Cmc_Rexp_F == null)
         //      _Cmc_Rexp_F = new Ui.ChangeMethodCategory.CMC_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "cmc_save_f")
         //{
         //   if (_Cmc_Save_F == null)
         //      _Cmc_Save_F = new Ui.ChangeMethodCategory.CMC_SAVE_F { _DefaultGateway = this };
         //}
         //else if (value == "ucc_rqst_f")
         //{
         //    if (_Ucc_Rqst_F == null)
         //        _Ucc_Rqst_F = new Ui.UserClubCard.UCC_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "ucc_sexp_f")
         //{
         //    if (_Ucc_Sexp_F == null)
         //        _Ucc_Sexp_F = new Ui.UserClubCard.UCC_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "ucc_rexp_f")
         //{
         //    if (_Ucc_Rexp_F == null)
         //        _Ucc_Rexp_F = new Ui.UserClubCard.UCC_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "ucc_save_f")
         //{
         //    if (_Ucc_Save_F == null)
         //        _Ucc_Save_F = new Ui.UserClubCard.UCC_SAVE_F { _DefaultGateway = this };
         //}
         else if (value == "cmn_dcmt_f")
         {
            if (_Cmn_Dcmt_F == null)
               _Cmn_Dcmt_F = new Ui.Document.CMN_DCMT_F { _DefaultGateway = this };
         }
         //else if (value == "mcc_rqst_f")
         //{
         //   if (_Mcc_Rqst_F == null)
         //      _Mcc_Rqst_F = new Ui.MethodClubCard.MCC_RQST_F { _DefaultGateway = this };
         //}
         //else if (value == "mcc_sexp_f")
         //{
         //   if (_Mcc_Sexp_F == null)
         //      _Mcc_Sexp_F = new Ui.MethodClubCard.MCC_SEXP_F { _DefaultGateway = this };
         //}
         //else if (value == "mcc_rexp_f")
         //{
         //   if (_Mcc_Rexp_F == null)
         //      _Mcc_Rexp_F = new Ui.MethodClubCard.MCC_REXP_F { _DefaultGateway = this };
         //}
         //else if (value == "mcc_save_f")
         //{
         //   if (_Mcc_Save_F == null)
         //      _Mcc_Save_F = new Ui.MethodClubCard.MCC_SAVE_F { _DefaultGateway = this };
         //}
         else if (value == "adm_totl_f")
         {
            if (_Adm_Totl_F == null)
               _Adm_Totl_F = new Ui.Admission.ADM_TOTL_F { _DefaultGateway =  this };
         }
         else if (value == "cmn_dise_f")
         {
            if (_Cmn_Dise_F == null)
               _Cmn_Dise_F = new Ui.Diseases.CMN_DISE_F { _DefaultGateway = this };
         }
         else if (value == "frst_page_f")
         {
            //if (_Frst_Page_F == null)
            //   _Frst_Page_F = new Ui.MasterPage.FRST_PAGE_F { _DefaultGateway = this };
         }
         else if (value == "pay_cash_f")
         {
            //if (_Pay_Cash_F == null)
            //   _Pay_Cash_F = new Ui.Cash.PAY_CASH_F { _DefaultGateway = this };
         }
         else if (value == "cal_expn_f")
         {
            if (_Cal_Expn_F == null)
               _Cal_Expn_F = new Ui.CalculateExpense.CAL_EXPN_F { _DefaultGateway = this };
         }
         else if (value == "cal_cexc_f")
         {
            if (_Cal_Cexc_F == null)
               _Cal_Cexc_F = new Ui.CalculateExpense.CAL_CEXC_F { _DefaultGateway = this };
         }
         else if (value == "adm_chng_f")
         {
            if (_Adm_Chng_F == null)
               _Adm_Chng_F = new Ui.Admission.ADM_CHNG_F { _DefaultGateway = this };
         }
         else if (value == "exm_totl_f")
         {
            //if (_Exm_Totl_F == null)
            //   _Exm_Totl_F = new Ui.Exam.EXM_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "psf_totl_f")
         {
            //if (_Psf_Totl_F == null)
            //   _Psf_Totl_F = new Ui.PhysicalFitness.PSF_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "clc_totl_f")
         {
            //if (_Clc_Totl_F == null)
            //   _Clc_Totl_F = new Ui.CalculateCalorie.CLC_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "hrz_totl_f")
         {
            //if (_Hrz_Totl_F == null)
            //   _Hrz_Totl_F = new Ui.HeartZone.HRZ_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "cmc_totl_f")
         {
            //if (_Cmc_Totl_F == null)
            //   _Cmc_Totl_F = new Ui.ChangeMethodCategory.CMC_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "tst_totl_f")
         {
            //if (_Tst_Totl_F == null)
            //   _Tst_Totl_F = new Ui.Test.TST_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "cmp_totl_f")
         {
            //if (_Cmp_Totl_F == null)
            //   _Cmp_Totl_F = new Ui.Campitition.CMP_TOTL_F { _DefaultGateway = this };
         }
         //else if (value == "com_totl_f")
         //{
         //   if (_Com_Totl_F == null)
         //      _Com_Totl_F = new Ui.Committee.COM_TOTL_F { _DefaultGateway = this };
         //}
         else if (value == "mcc_totl_f")
         {
            //if (_Mcc_Totl_F == null)
            //   _Mcc_Totl_F = new Ui.MethodClubCard.MCC_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "ins_totl_f")
         {
            if (_Ins_Totl_F == null)
               _Ins_Totl_F = new Ui.Insurance.INS_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "cfg_stng_f")
         {
            if (_Cfg_Stng_F == null)
               _Cfg_Stng_F = new Ui.Settings.CFG_STNG_F { _DefaultGateway = this };
         }
         else if (value == "adm_ends_f")
         {
            if (_Adm_Ends_F == null)
               _Adm_Ends_F = new Ui.EnablingDisabling.ADM_ENDS_F { _DefaultGateway = this };
         }
         else if (value == "adm_dsen_f")
         {
            if (_Adm_Dsen_F == null)
               _Adm_Dsen_F = new Ui.EnablingDisabling.ADM_DSEN_F { _DefaultGateway = this };
         }
         else if (value == "rpt_mngr_f")
         {
            if (_Rpt_Mngr_F == null)
               _Rpt_Mngr_F = new Ui.ReportManager.RPT_MNGR_F { _DefaultGateway = this };
         }
         else if (value == "rpt_lrfm_f")
         {
            if (_Rpt_Lrfm_F == null)
               _Rpt_Lrfm_F = new Ui.ReportManager.RPT_LRFM_F { _DefaultGateway = this };
         }
         else if (value == "pay_mtod_f")
         {
            if (_Pay_Mtod_F == null)
               _Pay_Mtod_F = new Ui.PaymentMethod.PAY_MTOD_F { _DefaultGateway = this };
         }
         else if (value == "mng_rcan_f")
         {
            if (_Mng_Rcan_F == null)
               _Mng_Rcan_F = new Ui.ReceiptAnnouncement.MNG_RCAN_F { _DefaultGateway = this };
         }
         else if (value == "ntf_totl_f")
         {
            if (_Ntf_Totl_F == null)
               _Ntf_Totl_F = new Ui.Notifications.NTF_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "rpt_list_f")
         {
            //if (_Rpt_List_F == null)
            //   _Rpt_List_F = new Ui.ReportManager.RPT_LIST_F { _DefaultGateway = this };
         }
         else if (value == "cal_cexc_p")
         {
            if (_Cal_Cexc_P == null)
               _Cal_Cexc_P = new Ui.CalculateExpense.CAL_CEXC_P { _DefaultGateway = this };
         }
         else if (value == "acn_list_f")
         {
            //if (_Acn_List_F == null)
            //   _Acn_List_F = new Ui.ReportManager.ACN_LIST_F { _DefaultGateway = this };
         }
         else if (value == "oic_totl_f")
         {
            if (_Oic_Totl_F == null)
               _Oic_Totl_F = new Ui.OtherIncome.OIC_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "pos_totl_f")
         {
            if (_Pos_Totl_F == null)
               _Pos_Totl_F = new Ui.POS.POS_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "cmn_resn_f")
         {
            if (_Cmn_Resn_F == null)
               _Cmn_Resn_F = new Ui.Reason.CMN_RESN_F { _DefaultGateway = this };
         }
         else if (value == "oic_srzh_f")
         {
            //if (_Oic_Srzh_F == null)
            //   _Oic_Srzh_F = new Ui.OtherIncome.OIC_SRZH_F { _DefaultGateway = this };
         }
         else if (value == "oic_sone_f")
         {
            //if (_Oic_Sone_F == null)
            //   _Oic_Sone_F = new Ui.OtherIncome.OIC_SONE_F { _DefaultGateway = this };
         }
         else if (value == "oic_smor_f")
         {
            //if (_Oic_Smor_F == null)
            //   _Oic_Smor_F = new Ui.OtherIncome.OIC_SMOR_F { _DefaultGateway = this };
         }
         //else if (value == "bas_cpr_f")
         //{
         //   if (_Bas_Cpr_F == null)
         //      _Bas_Cpr_F = new Ui.Settings.BAS_CPR_F { _DefaultGateway = this };
         //}
         else if(value == "new_fngr_f")
         {
            if (_New_Fngr_F == null)
               _New_Fngr_F = new Ui.Admission.NEW_FNGR_F { _DefaultGateway = this };
         }
         else if (value == "fngr_rsvd_f")
         {
            if (_Fngr_Rsvd_F == null)
               _Fngr_Rsvd_F = new Ui.FingerPrint.FNGR_RSVD_F { _DefaultGateway = this };
         }
         else if (value == "orgn_totl_f")
         {
            if (_Orgn_Totl_F == null)
               _Orgn_Totl_F = new Ui.Organ.ORGN_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "rqst_trac_f")
         {
            //if (_Rqst_Trac_F == null)
            //   _Rqst_Trac_F = new Ui.ReportManager.RQST_TRAC_F { _DefaultGateway = this };
         }
         else if (value == "who_aryu_f")
         {
            if (_Who_Aryu_F == null)
               _Who_Aryu_F = new Ui.Notifications.WHO_ARYU_F { _DefaultGateway = this };
         }
         else if(value == "rfd_totl_f")
         {
            //if (_Rfd_Totl_F == null)
            //   _Rfd_Totl_F = new Ui.Financial.RFD_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "glr_totl_f")
         {
            //if (_Glr_Totl_F == null)
            //   _Glr_Totl_F = new Ui.ChangeRials.GLR_TOTL_F { _DefaultGateway = this };
         }
         else if (value == "oic_smsn_f")
         {
            //if (_Oic_Smsn_F == null)
            //   _Oic_Smsn_F = new Ui.OtherIncome.OIC_SMSN_F { _DefaultGateway = this };
         }
         //else if(value == "chos_clas_f")
         //{
         //   if (_Chos_Clas_F == null)
         //      _Chos_Clas_F = new Ui.Notifications.CHOS_CLAS_F { _DefaultGateway = this };
         //}
         else if (value == "msgb_totl_f")
         {
            if (_Msgb_Totl_F == null)
               _Msgb_Totl_F = new Ui.MessageBroadcast.MSGB_TOTL_F { _DefaultGateway = this };
         }
         else if(value == "debt_list_f")
         {
            if (_Debt_List_F == null)
               _Debt_List_F = new Ui.DebitsList.DEBT_LIST_F {_DefaultGateway = this };
         }
         else if (value == "bdf_pros_f")
         {
            if (_Bdf_Pros_F == null)
               _Bdf_Pros_F = new Ui.BodyFitness.BDF_PROS_F { _DefaultGateway = this };
         }
         else if (value == "bsc_bmov_f")
         {
            if (_Bsc_Bmov_F == null)
               _Bsc_Bmov_F = new Ui.BodyFitness.BSC_BMOV_F { _DefaultGateway = this };
         }
         else if (value == "aop_mbsp_f")
         {
            if (_Aop_Mbsp_F == null)
               _Aop_Mbsp_F = new Ui.AggregateOperation.AOP_MBSP_F { _DefaultGateway = this };         
         }
         else if (value == "main_page_f")
         {
            if (_Main_Page_F == null)
               _Main_Page_F = new Ui.MasterPage.MAIN_PAGE_F { _DefaultGateway = this };
         }
         else if(value == "adm_figh_f")
         {
            if (_Adm_Figh_F == null)
               _Adm_Figh_F = new Ui.Admission.ADM_FIGH_F { _DefaultGateway = this };
         }
         else if (value == "aop_cbmt_f")
         {
            if (_Aop_Cbmt_F == null)
               _Aop_Cbmt_F = new Ui.AggregateOperation.AOP_CBMT_F { _DefaultGateway = this };
         }
         else if (value == "aop_mtod_f")
         {
            if (_Aop_Mtod_F == null)
               _Aop_Mtod_F = new Ui.AggregateOperation.AOP_MTOD_F { _DefaultGateway = this };
         }
         else if (value == "aop_attn_f")
         {
            if (_Aop_Attn_F == null)
               _Aop_Attn_F = new Ui.AggregateOperation.AOP_ATTN_F { _DefaultGateway = this };
         }
         else if (value == "adm_mbco_f")
         {
            if (_Adm_Mbco_F == null)
               _Adm_Mbco_F = new Ui.Admission.ADM_MBCO_F { _DefaultGateway = this };
         }
         else if (value == "adm_hrsr_f")
         {
            //if (_Adm_Hrsr_F == null)
            //   _Adm_Hrsr_F = new Ui.HumanResource.ADM_HRSR_F { _DefaultGateway = this };
         }
         else if (value == "adm_brsr_f")
         {
            if (_Adm_Brsr_F == null)
               _Adm_Brsr_F = new Ui.OtherIncome.ADM_BRSR_F { _DefaultGateway = this };
         }
         else if (value == "aop_bufe_f")
         {
            if (_Aop_Bufe_F == null)
               _Aop_Bufe_F = new Ui.AggregateOperation.AOP_BUFE_F { _DefaultGateway = this };
            job.Output = _Aop_Bufe_F;
         }
         else if (value == "adm_hrsc_f")
         {
            //if (_Adm_Hrsc_F == null)
            //   _Adm_Hrsc_F = new Ui.HumanResource.ADM_HRSC_F { _DefaultGateway = this };
         }
         else if(value == "adm_mbfz_f")
         {
            if (_Adm_Mbfz_F == null)
               _Adm_Mbfz_F = new Ui.Admission.ADM_MBFZ_F { _DefaultGateway = this };
         }
         else if (value == "adm_mbsm_f")
         {
            if (_Adm_Mbsm_F == null)
               _Adm_Mbsm_F = new Ui.Admission.ADM_MBSM_F { _DefaultGateway = this };
         }
         else if(value == "rpt_pmmt_f")
         {
            if (_Rpt_Pmmt_F == null)
               _Rpt_Pmmt_F = new Ui.ReportManager.RPT_PMMT_F { _DefaultGateway = this };
         }
         else if(value == "dap_pivt_f")
         {
            //if (_Dap_Pivt_F == null)
            //   _Dap_Pivt_F = new Ui.DataAnalysis.DAP_PIVT_F { _DefaultGateway = this };
         }
         else if(value == "dap_chrt_f")
         {
            //if (_Dap_Chrt_F == null)
            //   _Dap_Chrt_F = new Ui.DataAnalysis.DAP_CHRT_F { _DefaultGateway = this };
         }
         else if (value == "cnf_stng_f")
         {
            if (_Cnf_Stng_F == null)
               _Cnf_Stng_F = new Ui.Settings.CNF_STNG_F { _DefaultGateway = this };
         }
         else if (value == "dap_dshb_f")
         {
            //if (_Dap_Dshb_F == null)
            //   _Dap_Dshb_F = new Ui.DataAnalysis.DAP_DSHB_F { _DefaultGateway = this };
         }
         else if (value == "lsi_fdlf_f")
         {
            if (_Lsi_Fdlf_F == null)
               _Lsi_Fdlf_F = new Ui.Common.LSI_FDLF_F { _DefaultGateway = this };
         }
         else if (value == "attn_dayn_f")
         {
            if (_Attn_Dayn_F == null)
               _Attn_Dayn_F = new Ui.Notifications.ATTN_DAYN_F { _DefaultGateway = this };
         }
         else if (value == "usr_actn_f")
         {
            if (_Usr_Actn_F == null)
               _Usr_Actn_F = new Ui.UserAction.USR_ACTN_F { _DefaultGateway = this };
         }
         else if (value == "usr_ctbl_f")
         {
            if (_Usr_Ctbl_F == null)
               _Usr_Ctbl_F = new Ui.UserAction.USR_CTBL_F { _DefaultGateway = this };
         }
         else if (value == "bas_dfin_f")
         {
            if (_Bas_Dfin_F == null)
               _Bas_Dfin_F = new Ui.BaseDefinition.BAS_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "bas_adch_f")
         {
            if (_Bas_Adch_F == null)
               _Bas_Adch_F = new Ui.BaseDefinition.BAS_ADCH_F { _DefaultGateway = this };
         }
         else if (value == "bas_adcl_f")
         {
            if (_Bas_Adcl_F == null)
               _Bas_Adcl_F = new Ui.BaseDefinition.BAS_ADCL_F { _DefaultGateway = this };
         }
         else if (value == "bas_gruc_f")
         {
            if (_Bas_Gruc_F == null)
               _Bas_Gruc_F = new Ui.BaseDefinition.BAS_GRUC_F { _DefaultGateway = this };
         }
         else if (value == "gate_actn_f")
         {
            if (_Gate_Actn_F == null)
               _Gate_Actn_F = new Ui.GateEntryExit.GATE_ACTN_F { _DefaultGateway = this };
         }
         else if (value == "bas_wkdy_f")
         {
            if (_Bas_Wkdy_F == null)
               _Bas_Wkdy_F = new Ui.BaseDefinition.BAS_WKDY_F { _DefaultGateway = this };
         }
         else if (value == "tran_expn_f")
         {
            if (_Tran_Expn_F == null)
               _Tran_Expn_F = new Ui.Cash.TRAN_EXPN_F { _DefaultGateway = this };
         }
         else if (value == "mbsp_chng_f")
         {
            if (_Mbsp_Chng_F == null)
               _Mbsp_Chng_F = new Ui.Admission.MBSP_CHNG_F { _DefaultGateway = this };
         }
         else if (value == "chos_mbsp_f")
         {
            if (_Chos_Mbsp_F == null)
               _Chos_Mbsp_F = new Ui.Notifications.CHOS_MBSP_F { _DefaultGateway = this };
         }
         else if(value ==  "glr_indc_f")
         {
            if (_Glr_Indc_F == null)
               _Glr_Indc_F = new Ui.ChangeRials.GLR_INDC_F { _DefaultGateway = this };
         }
         else if (value == "apbs_dfin_f")
         {
            if (_Apbs_Dfin_F == null)
               _Apbs_Dfin_F = new Ui.BaseDefinition.APBS_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "mbsp_mark_f")
         {
            if (_Mbsp_Mark_F == null)
               _Mbsp_Mark_F = new Ui.Admission.MBSP_MARK_F { _DefaultGateway = this };
         }
         else if(value == "aop_incm_f")
         {
            if (_Aop_Incm_F == null)
               _Aop_Incm_F = new Ui.AggregateOperation.AOP_INCM_F { _DefaultGateway = this };
         }
         else if (value == "ksk_incm_f")
         {
            if (_Ksk_Incm_F == null)
               _Ksk_Incm_F = new Ui.OtherIncome.KSK_INCM_F { _DefaultGateway = this };
         }
         else if (value == "dap_dsbr_f")
         {
            if (_Dap_Dsbr_F == null)
               _Dap_Dsbr_F = new Ui.DataAnalysis.DAP_DSBR_F { _DefaultGateway = this };
         }
         else if (value == "bas_cbmt_f")
         {
            if (_Bas_Cbmt_F == null)
               _Bas_Cbmt_F = new Ui.BaseDefinition.BAS_CBMT_F { _DefaultGateway = this };
         }

         // فرم های نمایش تغییرات
         else if (value == "show_atrq_f")
         {
            if (_Show_Atrq_F == null)
               _Show_Atrq_F = new Ui.Admission.ShowChanges.SHOW_ATRQ_F { _DefaultGateway = this };
         }
         else if (value == "show_acrq_f")
         {
            if (_Show_Acrq_F == null)
               _Show_Acrq_F = new Ui.Admission.ShowChanges.SHOW_ACRQ_F { _DefaultGateway = this };
         }
         else if (value == "show_ctrq_f")
         {
            if (_Show_Ctrq_F == null)
               _Show_Ctrq_F = new Ui.ChangeMethodCategory.ShowChanges.SHOW_CTRQ_F { _DefaultGateway = this };
         }
         else if (value == "show_derq_f")
         {
            if (_Show_Derq_F == null)
               _Show_Derq_F = new Ui.EnablingDisabling.ShowChanges.SHOW_DERQ_F { _DefaultGateway = this };
         }
         else if (value == "show_ucrq_f")
         {
            if (_Show_Ucrq_F == null)
               _Show_Ucrq_F = new Ui.Admission.ShowChanges.SHOW_UCRQ_F { _DefaultGateway = this };
         }
         else if (value == "show_omrq_f")
         {
            //if (_Show_Omrq_F == null)
            //   _Show_Omrq_F = new Ui.OtherIncome.ShowChanges.SHOW_OMRQ_F { _DefaultGateway = this };
         }
         else if (value == "show_emrq_f")
         {
            //if (_Show_Emrq_F == null)
            //   _Show_Emrq_F = new Ui.OtherExpense.ShowChanges.SHOW_EMRQ_F { _DefaultGateway = this };
         }
         else if (value == "show_rfdt_f")
         {
            //if (_Show_Rfdt_F == null)
            //   _Show_Rfdt_F = new Ui.Financial.ShowChanges.SHOW_RFDT_F { _DefaultGateway = this };
         }
         else if (value == "show_glrl_f")
         {
            //if (_Show_Glrl_F == null)
            //   _Show_Glrl_F = new Ui.ChangeRials.ShowChanges.SHOW_GLRL_F { _DefaultGateway = this };
         }
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Mstr_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_page_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 03 /* Execute Paint */),                  
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
      private void Mstr_Regl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_regl_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 03 /* Execute Paint */)
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
      private void Regl_Acnt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regl_acnt_f"},
                  new Job(SendType.SelfToUserInterface, "REGL_ACNT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGL_ACNT_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "REGL_ACNT_F", 03 /* Execute Paint */)
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
      private void Regl_Expn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regl_expn_f"},
                  new Job(SendType.SelfToUserInterface, "REGL_EXPN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGL_EXPN_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "REGL_EXPN_F", 03 /* Execute Paint */)
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
      private void Regl_Dcmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regl_dcmt_f"},
                  new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 03 /* Execute Paint */)
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
      private void Mstr_Dcmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_dcmt_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_DCMT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTR_DCMT_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_DCMT_F", 03 /* Execute Paint */)
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
      private void Mstr_Mtod_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_mtod_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_MTOD_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "MSTR_MTOD_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_MTOD_F", 03 /* Execute Paint */)
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
      private void Mstr_Regn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_regn_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_REGN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGN_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGN_F", 03 /* Execute Paint */)
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
      private void Mstr_Epit_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_epit_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_EPIT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTR_EPIT_F", 07 /* Execute LoadData */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "MSTR_EPIT_F", 03 /* Execute Paint */)
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
      private void Mstr_Club_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_club_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_CLUB_F", 02 /* Execute Set */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "MSTR_CLUB_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_CLUB_F", 03 /* Execute Paint */)
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
      private void Main_Subs_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "main_subs_f"},
                  new Job(SendType.SelfToUserInterface, "MAIN_SUBS_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MAIN_SUBS_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MAIN_SUBS_F", 03 /* Execute Paint */)
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
      private void Rqst_Type_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rqst_type_f"},
                  new Job(SendType.SelfToUserInterface, "RQST_TYPE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RQST_TYPE_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "RQST_TYPE_F", 03 /* Execute Paint */)
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
      private void Rqtr_Type_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rqtr_type_f"},
                  new Job(SendType.SelfToUserInterface, "RQTR_TYPE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RQTR_TYPE_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "RQTR_TYPE_F", 03 /* Execute Paint */)
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
      private void Adm_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_RQST_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "ADM_RQST_F", 03 /* Execute Paint */)
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
      private void Adm_Fsum_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_fsum_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_FSUM_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_FSUM_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "ADM_FSUM_F", 03 /* Execute Paint */)
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
      private void Adm_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "ADM_SEXP_F", 03 /* Execute Paint */)
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
      private void Adm_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "ADM_REXP_F", 03 /* Execute Paint */)
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
      private void Adm_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_save_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "ADM_SAVE_F", 03 /* Execute Paint */)
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
      private void Atn_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "atn_save_f"},
                  new Job(SendType.SelfToUserInterface, "ATN_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ATN_SAVE_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "ATN_SAVE_F", 03 /* Execute Paint */)
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
      private void Tst_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tst_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "TST_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TST_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "TST_RQST_F", 03 /* Execute Paint */)
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
      private void Tst_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tst_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "TST_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TST_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "TST_SEXP_F", 03 /* Execute Paint */)
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
      private void Tst_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tst_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "TST_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TST_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "TST_REXP_F", 03 /* Execute Paint */)
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
      private void Tst_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tst_save_f"},
                  new Job(SendType.SelfToUserInterface, "TST_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TST_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "TST_SAVE_F", 03 /* Execute Paint */)
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
      private void Cmp_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmp_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "CMP_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMP_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "CMP_RQST_F", 03 /* Execute Paint */)
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
      private void Cmp_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmp_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "CMP_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMP_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMP_SEXP_F", 03 /* Execute Paint */)
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
      private void Cmp_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmp_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "CMP_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMP_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMP_REXP_F", 03 /* Execute Paint */)
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
      private void Cmp_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmp_save_f"},
                  new Job(SendType.SelfToUserInterface, "CMP_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMP_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMP_SAVE_F", 03 /* Execute Paint */)
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
      private void Psf_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "psf_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "PSF_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PSF_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "PSF_RQST_F", 03 /* Execute Paint */)
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
      private void Psf_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "psf_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "PSF_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PSF_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "PSF_SEXP_F", 03 /* Execute Paint */)
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
      private void Psf_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "psf_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "PSF_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PSF_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "PSF_REXP_F", 03 /* Execute Paint */)
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
      private void Psf_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "psf_save_f"},
                  new Job(SendType.SelfToUserInterface, "PSF_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PSF_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "PSF_SAVE_F", 03 /* Execute Paint */)
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
      private void Clc_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "clc_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "CLC_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CLC_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "CLC_RQST_F", 03 /* Execute Paint */)
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
      private void Clc_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "clc_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "CLC_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CLC_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CLC_SEXP_F", 03 /* Execute Paint */)
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
      private void Clc_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "clc_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "CLC_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CLC_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CLC_REXP_F", 03 /* Execute Paint */)
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
      private void Clc_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "clc_save_f"},
                  new Job(SendType.SelfToUserInterface, "CLC_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CLC_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CLC_SAVE_F", 03 /* Execute Paint */)
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
      private void Hrz_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hrz_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "HRZ_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HRZ_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "HRZ_RQST_F", 03 /* Execute Paint */)
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
      private void Hrz_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hrz_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "HRZ_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HRZ_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "HRZ_SEXP_F", 03 /* Execute Paint */)
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
      private void Hrz_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hrz_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "HRZ_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HRZ_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "HRZ_REXP_F", 03 /* Execute Paint */)
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
      private void Hrz_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hrz_save_f"},
                  new Job(SendType.SelfToUserInterface, "HRZ_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HRZ_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "HRZ_SAVE_F", 03 /* Execute Paint */)
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
      private void Exm_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "exm_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "EXM_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EXM_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "EXM_RQST_F", 03 /* Execute Paint */)
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
      private void Exm_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "exm_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "EXM_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EXM_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "EXM_SEXP_F", 03 /* Execute Paint */)
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
      private void Exm_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "exm_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "EXM_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EXM_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "EXM_REXP_F", 03 /* Execute Paint */)
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
      private void Exm_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "exm_save_f"},
                  new Job(SendType.SelfToUserInterface, "EXM_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EXM_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "EXM_SAVE_F", 03 /* Execute Paint */)
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
      private void Lsi_Fldf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "lsi_fldf_f"},
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 02 /* Execute Set */){Executive = ExecutiveType.Synchronize},                  
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 07 /* Execute LoadData */)//{Executive = ExecutiveType.Asynchronous},                                    
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
      private void All_Fldf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "all_fldf_f"},
                  new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 07 /* Execute LoadData */){Input = job.Input/*, Executive = ExecutiveType.Asynchronous*/},                  
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
      private void Pbl_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pbl_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "PBL_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PBL_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "PBL_RQST_F", 03 /* Execute Paint */)
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
      private void Pbl_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pbl_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "PBL_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PBL_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "PBL_SEXP_F", 03 /* Execute Paint */)
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
      private void Pbl_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pbl_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "PBL_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PBL_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "PBL_REXP_F", 03 /* Execute Paint */)
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
      private void Pbl_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pbl_save_f"},
                  new Job(SendType.SelfToUserInterface, "PBL_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PBL_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "PBL_SAVE_F", 03 /* Execute Paint */)
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
      private void Cmc_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmc_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "CMC_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMC_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "CMC_RQST_F", 03 /* Execute Paint */)
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
      private void Cmc_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmc_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "CMC_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMC_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMC_SEXP_F", 03 /* Execute Paint */)
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
      private void Cmc_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmc_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "CMC_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMC_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMC_REXP_F", 03 /* Execute Paint */)
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
      private void Cmc_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmc_save_f"},
                  new Job(SendType.SelfToUserInterface, "CMC_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMC_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMC_SAVE_F", 03 /* Execute Paint */)
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
      private void Ucc_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ucc_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "UCC_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "UCC_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "UCC_RQST_F", 03 /* Execute Paint */)
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
      private void Ucc_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ucc_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "UCC_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "UCC_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "UCC_SEXP_F", 03 /* Execute Paint */)
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
      private void Ucc_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ucc_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "UCC_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "UCC_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "UCC_REXP_F", 03 /* Execute Paint */)
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
      private void Ucc_Save_F(Job job)
      {
          if (job.Status == StatusType.Running)
          {
              job.Status = StatusType.WaitForPreconditions;
              job.OwnerDefineWorkWith.AddRange(
                 new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ucc_save_f"},
                  new Job(SendType.SelfToUserInterface, "UCC_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "UCC_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "UCC_SAVE_F", 03 /* Execute Paint */)
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
      private void Cmn_Dcmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmn_dcmt_f"},
                  new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 03 /* Execute Paint */)
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
      private void Mcc_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mcc_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "MCC_RQST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MCC_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MCC_RQST_F", 03 /* Execute Paint */)
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
      private void Mcc_Sexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mcc_sexp_f"},
                  new Job(SendType.SelfToUserInterface, "MCC_SEXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MCC_SEXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "MCC_SEXP_F", 03 /* Execute Paint */)
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
      private void Mcc_Rexp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mcc_rexp_f"},
                  new Job(SendType.SelfToUserInterface, "MCC_REXP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MCC_REXP_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "MCC_REXP_F", 03 /* Execute Paint */)
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
      private void Mcc_Save_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mcc_save_f"},
                  new Job(SendType.SelfToUserInterface, "MCC_SAVE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MCC_SAVE_F", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "MCC_SAVE_F", 03 /* Execute Paint */)
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
      private void Adm_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_totl_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack/*, Executive = ExecutiveType.Asynchronous*/},
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 06 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 07 /* Execute LoadData */)//{Executive = ExecutiveType.Asynchronous},                  
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
      private void Cmn_Dise_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmn_dise_f"},
                  new Job(SendType.SelfToUserInterface, "CMN_DISE_F", 02 /* Execute Set */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "CMN_DISE_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "CMN_DISE_F", 03 /* Execute Paint */)
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
      private void Frst_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "frst_page_f"},
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 02 /* Execute Set */),//{Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 10 /* Execute Actn_CalF_P */)//{Executive = ExecutiveType.Asynchronous}
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
      private void Pay_Cash_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pay_cash_f"},
                  new Job(SendType.SelfToUserInterface, "PAY_CASH_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PAY_CASH_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "PAY_CASH_F", 03 /* Execute Paint */)
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
      private void Cal_Expn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cal_expn_f"},
                  new Job(SendType.SelfToUserInterface, "CAL_EXPN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CAL_EXPN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CAL_EXPN_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 69
      /// </summary>
      /// <param name="job"></param>
      private void Cal_Cexc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cal_cexc_f"},
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_F", 03 /* Execute Paint */)
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
      private void Adm_Chng_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_chng_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 02 /* Execute Set */){Executive = ExecutiveType.Synchronize},
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 07 /* Execute Load_Data */)//{Executive = ExecutiveType.Asynchronous},                  
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
      private void Exm_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "exm_totl_f"},
                  new Job(SendType.SelfToUserInterface, "EXM_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EXM_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "EXM_TOTL_F", 03 /* Execute Paint */)
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
      private void Psf_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "psf_totl_f"},
                  new Job(SendType.SelfToUserInterface, "PSF_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PSF_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "PSF_TOTL_F", 03 /* Execute Paint */)
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
      private void Clc_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "clc_totl_f"},
                  new Job(SendType.SelfToUserInterface, "CLC_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CLC_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CLC_TOTL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 74
      /// </summary>
      /// <param name="job"></param>
      private void Hrz_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "hrz_totl_f"},
                  new Job(SendType.SelfToUserInterface, "HRZ_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "HRZ_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "HRZ_TOTL_F", 03 /* Execute Paint */)
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
      private void Cmc_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmc_totl_f"},
                  new Job(SendType.SelfToUserInterface, "CMC_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMC_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CMC_TOTL_F", 03 /* Execute Paint */)
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
      private void Tst_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tst_totl_f"},
                  new Job(SendType.SelfToUserInterface, "TST_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "TST_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TST_TOTL_F", 03 /* Execute Paint */)
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
      private void Cmp_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmp_totl_f"},
                  new Job(SendType.SelfToUserInterface, "CMP_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMP_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CMP_TOTL_F", 03 /* Execute Paint */)
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
      private void Com_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "com_totl_f"},
                  new Job(SendType.SelfToUserInterface, "COM_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "COM_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "COM_TOTL_F", 03 /* Execute Paint */)
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
      private void Mcc_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mcc_totl_f"},
                  new Job(SendType.SelfToUserInterface, "MCC_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MCC_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MCC_TOTL_F", 03 /* Execute Paint */)
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
      private void Ins_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ins_totl_f"},
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 03 /* Execute Paint */)
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
      private void Cfg_Stng_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cfg_stng_f"},
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 03 /* Execute Paint */)
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
      private void Adm_Ends_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_ends_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_ENDS_F", 03 /* Execute Paint */)
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
      private void Adm_Dsen_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_dsen_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_DSEN_F", 03 /* Execute Paint */)
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
      private void Rpt_Mngr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_mngr_f"},
                  new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 11 /* Execute Do_Print */){WhereIsInputData = WhereIsInputDataType.StepBack}                  
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
      private void Rpt_Lrfm_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_lrfm_f"},
                  new Job(SendType.SelfToUserInterface, "RPT_LRFM_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RPT_LRFM_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RPT_LRFM_F", 03 /* Execute Paint */)
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
      private void Pay_Mtod_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pay_mtod_f"},
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
      /// Code 87
      /// </summary>
      /// <param name="job"></param>
      private void Mng_Rcan_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mng_rcan_f"},
                  new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 05 /* Execute CheckSecurity */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MNG_RCAN_F", 03 /* Execute Paint */)
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
      private void Ntf_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ntf_totl_f"},
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 03 /* Execute Paint */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 07 /* Execute Load_Data */)//{Executive = ExecutiveType.Asynchronous},                  
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
      private void Rpt_List_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_list_f"},
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "RPT_LIST_F", 03 /* Execute Paint */)
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
      private void Cal_Cexc_P(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cal_cexc_p"},
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_P", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_P", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CAL_CEXC_P", 03 /* Execute Paint */)
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
      private void Acn_List_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "acn_list_f"},
                  new Job(SendType.SelfToUserInterface, "ACN_LIST_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ACN_LIST_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ACN_LIST_F", 03 /* Execute Paint */)
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
      private void Oic_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "oic_totl_f"},
                  new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 03 /* Execute Paint */)
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
      private void Pos_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pos_totl_f"},
                  new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 07 /* Execute Load_Data */),
                  //new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 03 /* Execute Paint */)
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
      private void Cmn_Resn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cmn_resn_f"},
                  new Job(SendType.SelfToUserInterface, "CMN_RESN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CMN_RESN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CMN_RESN_F", 03 /* Execute Paint */)
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
      private void Oic_Srzh_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "oic_srzh_f"},
                  new Job(SendType.SelfToUserInterface, "OIC_SRZH_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OIC_SRZH_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "OIC_SRZH_F", 03 /* Execute Paint */)
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
      private void Oic_Sone_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "oic_sone_f"},
                  new Job(SendType.SelfToUserInterface, "OIC_SONE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OIC_SONE_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "OIC_SONE_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},                  
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
      private void Oic_Smor_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "oic_smor_f"},
                  new Job(SendType.SelfToUserInterface, "OIC_SMOR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OIC_SMOR_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "OIC_SMOR_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack/*, Executive = ExecutiveType.Asynchronous*/},                  
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
      private void Bas_Cpr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_cpr_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "BAS_CPR_F", 03 /* Execute Paint */)
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
      private void New_Fngr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "new_fngr_f"},
                  new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "NEW_FNGR_F", 03 /* Execute Paint */)
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
      private void Fngr_Rsvd_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "fngr_rsvd_f"},
                  new Job(SendType.SelfToUserInterface, "FNGR_RSVD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "FNGR_RSVD_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "FNGR_RSVD_F", 03 /* Execute Paint */)
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
      private void Show_Atrq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_atrq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_ATRQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_ATRQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_ATRQ_F", 03 /* Execute Paint */)
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
      private void Show_Acrq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_acrq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_ACRQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_ACRQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_ACRQ_F", 03 /* Execute Paint */)
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
      private void Show_Ctrq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_ctrq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_CTRQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_CTRQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_CTRQ_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 104
      /// </summary>
      /// <param name="job"></param>
      private void Show_Derq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_derq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_DERQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_DERQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_DERQ_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 105
      /// </summary>
      /// <param name="job"></param>
      private void Show_Ucrq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_ucrq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_UCRQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_UCRQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_UCRQ_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 106
      /// </summary>
      /// <param name="job"></param>
      private void Show_Omrq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_omrq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_OMRQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_OMRQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_OMRQ_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 107
      /// </summary>
      /// <param name="job"></param>
      private void Show_Emrq_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_emrq_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_EMRQ_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_EMRQ_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_EMRQ_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 108
      /// </summary>
      /// <param name="job"></param>
      private void Orgn_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "orgn_totl_f"},
                  new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ORGN_TOTL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 109
      /// </summary>
      /// <param name="job"></param>
      private void Rqst_Trac_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rqst_trac_f"},
                  new Job(SendType.SelfToUserInterface, "RQST_TRAC_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RQST_TRAC_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RQST_TRAC_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 110
      /// </summary>
      /// <param name="job"></param>
      private void Who_Aryu_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "who_aryu_f"},
                  new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "WHO_ARYU_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack}                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 111
      /// </summary>
      /// <param name="job"></param>
      private void Rfd_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rfd_totl_f"},
                  new Job(SendType.SelfToUserInterface, "RFD_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RFD_TOTL_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "RFD_TOTL_F", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "RFD_TOTL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack}                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 112
      /// </summary>
      /// <param name="job"></param>
      private void Show_Rfdt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_rfdt_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_RFDT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SHOW_RFDT_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SHOW_RFDT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 113
      /// </summary>
      /// <param name="job"></param>
      private void Glr_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "glr_totl_f"},
                  new Job(SendType.SelfToUserInterface, "GLR_TOTL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "GLR_TOTL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "GLR_TOTL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 114
      /// </summary>
      /// <param name="job"></param>
      private void Oic_Smsn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "oic_smsn_f"},
                  new Job(SendType.SelfToUserInterface, "OIC_SMSN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "OIC_SMSN_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "OIC_SMSN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack/*, Executive = ExecutiveType.Asynchronous*/},                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 115
      /// </summary>
      /// <param name="job"></param>
      private void Chos_Clas_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "chos_clas_f"},
                  new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CHOS_CLAS_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 116
      /// </summary>
      /// <param name="job"></param>
      private void Msgb_Totl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "msgb_totl_f"},
                  new Job(SendType.SelfToUserInterface, "MSGB_TOTL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "MSGB_TOTL_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "MSGB_TOTL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack, Executive = ExecutiveType.Synchronize},
                  new Job(SendType.SelfToUserInterface, "MSGB_TOTL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 117
      /// </summary>
      /// <param name="job"></param>
      private void Debt_List_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "debt_list_f"},
                  new Job(SendType.SelfToUserInterface, "DEBT_LIST_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "DEBT_LIST_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "DEBT_LIST_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack, Executive = ExecutiveType.Synchronize},
                  new Job(SendType.SelfToUserInterface, "DEBT_LIST_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }


      /// <summary>
      /// Code 118
      /// </summary>
      /// <param name="job"></param>
      private void Show_Glrl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "show_glrl_f"},
                  new Job(SendType.SelfToUserInterface, "SHOW_GLRL_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SHOW_GLRL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack, Executive = ExecutiveType.Synchronize},
                  new Job(SendType.SelfToUserInterface, "SHOW_GLRL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 119
      /// </summary>
      /// <param name="job"></param>
      private void Bdf_Pros_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bdf_pros_f"},
                  new Job(SendType.SelfToUserInterface, "BDF_PROS_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BDF_PROS_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack, Executive = ExecutiveType.Synchronize},
                  new Job(SendType.SelfToUserInterface, "BDF_PROS_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 120
      /// </summary>
      /// <param name="job"></param>
      private void Bsc_Bmov_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bsc_bmov_f"},
                  new Job(SendType.SelfToUserInterface, "BSC_BMOV_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "BSC_BMOV_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BSC_BMOV_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BSC_BMOV_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 121
      /// </summary>
      /// <param name="job"></param>
      private void Aop_Mbsp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_mbsp_f"},
                  new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "AOP_MBSP_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 122
      /// </summary>
      /// <param name="job"></param>
      private void Main_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "main_page_f"},
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 01 /* Execute Get */),                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 123
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Figh_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_figh_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 124
      /// </summary>
      /// <param name="job"></param>
      private void Aop_Cbmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_cbmt_f"},
                  new Job(SendType.SelfToUserInterface, "AOP_CBMT_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_CBMT_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_CBMT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "AOP_CBMT_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "AOP_CBMT_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 125
      /// </summary>
      /// <param name="job"></param>
      private void Aop_Mtod_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_mtod_f"},
                  new Job(SendType.SelfToUserInterface, "AOP_MTOD_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_MTOD_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_MTOD_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "AOP_MTOD_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "AOP_MTOD_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 126
      /// </summary>
      /// <param name="job"></param>
      private void Aop_Attn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_attn_f"},
                  new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "AOP_ATTN_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 127
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Mbco_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_mbco_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "ADM_MBCO_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 128
      /// </summary>
      /// <param name="job"></param>
      private void Strt_Menu_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "strt_menu_f"},
                  new Job(SendType.SelfToUserInterface, "STRT_MENU_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "STRT_MENU_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "STRT_MENU_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "STRT_MENU_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "STRT_MENU_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 129
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Hrsr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_hrsr_f"},
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 130
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Brsr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_brsr_f"},
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 131
      /// </summary>
      /// <param name="job"></param>
      private void Aop_Bufe_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_bufe_f"},
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 132
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Hrsc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_hrsc_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_HRSC_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_HRSC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_HRSC_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 133
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Mbfz_f(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_mbfz_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBFZ_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 134
      /// </summary>
      /// <param name="job"></param>
      private void Adm_Mbsm_f(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_mbsm_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_MBSM_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ADM_MBSM_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ADM_MBSM_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 135
      /// </summary>
      /// <param name="job"></param>
      private void Rpt_Pmmt_f(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_pmmt_f"},
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "RPT_PMMT_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 136
      /// </summary>
      /// <param name="job"></param>
      private void Dap_Pivt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dap_pivt_f"},
                  new Job(SendType.SelfToUserInterface, "DAP_PIVT_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "DAP_PIVT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "DAP_PIVT_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 137
      /// </summary>
      /// <param name="job"></param>
      private void Dap_Chrt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dap_chrt_f"},
                  new Job(SendType.SelfToUserInterface, "DAP_CHRT_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "DAP_CHRT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "DAP_CHRT_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 138
      /// </summary>
      /// <param name="job"></param>
      private void Cnf_Stng_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cnf_stng_f"},
                  new Job(SendType.SelfToUserInterface, "CNF_STNG_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "CNF_STNG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CNF_STNG_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 139
      /// </summary>
      /// <param name="job"></param>
      private void Dap_Dshb_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dap_dshb_f"},
                  new Job(SendType.SelfToUserInterface, "DAP_DSHB_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "DAP_DSHB_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "DAP_DSHB_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 140
      /// </summary>
      /// <param name="job"></param>
      private void Lsi_Fdlf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "lsi_fdlf_f"},
                  new Job(SendType.SelfToUserInterface, "LSI_FDLF_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "LSI_FDLF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "LSI_FDLF_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 141
      /// </summary>
      /// <param name="job"></param>
      private void Attn_Dayn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "attn_dayn_f"},
                  new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 142
      /// </summary>
      /// <param name="job"></param>
      private void Usr_Actn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "usr_actn_f"},
                  new Job(SendType.SelfToUserInterface, "USR_ACTN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "USR_ACTN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "USR_ACTN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 143
      /// </summary>
      /// <param name="job"></param>
      private void Usr_Ctbl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "usr_ctbl_f"},
                  new Job(SendType.SelfToUserInterface, "USR_CTBL_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "USR_CTBL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "USR_CTBL_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 144
      /// </summary>
      /// <param name="job"></param>
      private void Bas_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BAS_DFIN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 145
      /// </summary>
      /// <param name="job"></param>
      private void Bas_Adch_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_adch_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_ADCH_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BAS_ADCH_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BAS_ADCH_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 146
      /// </summary>
      /// <param name="job"></param>
      private void Bas_Adcl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_adcl_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_ADCL_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BAS_ADCL_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BAS_ADCL_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 147
      /// </summary>
      /// <param name="job"></param>
      private void Bas_Gruc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_gruc_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_GRUC_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BAS_GRUC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BAS_GRUC_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 148
      /// </summary>
      /// <param name="job"></param>
      private void Gate_Actn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "gate_actn_f"},
                  new Job(SendType.SelfToUserInterface, "GATE_ACTN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "GATE_ACTN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "GATE_ACTN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 149
      /// </summary>
      /// <param name="job"></param>
      private void Bas_Wkdy_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_wkdy_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_WKDY_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BAS_WKDY_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BAS_WKDY_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 150
      /// </summary>
      /// <param name="job"></param>
      private void Tran_Expn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "tran_expn_f"},
                  new Job(SendType.SelfToUserInterface, "TRAN_EXPN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "TRAN_EXPN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "TRAN_EXPN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 151
      /// </summary>
      /// <param name="job"></param>
      private void Mbsp_Chng_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mbsp_chng_f"},
                  new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 152
      /// </summary>
      /// <param name="job"></param>
      private void Chos_Mbsp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "chos_mbsp_f"},
                  new Job(SendType.SelfToUserInterface, "CHOS_MBSP_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "CHOS_MBSP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CHOS_MBSP_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 153
      /// </summary>
      /// <param name="job"></param>
      private void Glr_Indc_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "glr_indc_f"},
                  new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 154
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
                  new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 155
      /// </summary>
      /// <param name="job"></param>
      private void Mbsp_Mark_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mbsp_mark_f"},
                  new Job(SendType.SelfToUserInterface, "MBSP_MARK_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "MBSP_MARK_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MBSP_MARK_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 156
      /// </summary>
      /// <param name="job"></param>
      private void Aop_Incm_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "aop_incm_f"},
                  new Job(SendType.SelfToUserInterface, "AOP_INCM_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "AOP_INCM_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "AOP_INCM_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 157
      /// </summary>
      /// <param name="job"></param>
      private void Ksk_Incm_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ksk_incm_f"},
                  new Job(SendType.SelfToUserInterface, "KSK_INCM_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "KSK_INCM_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "KSK_INCM_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 158
      /// </summary>
      /// <param name="job"></param>
      private void Dap_Dsbr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dap_dsbr_f"},
                  new Job(SendType.SelfToUserInterface, "DAP_DSBR_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "DAP_DSBR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "DAP_DSBR_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 159
      /// </summary>
      /// <param name="job"></param>
      private void Bas_Cbmt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bas_cbmt_f"},
                  new Job(SendType.SelfToUserInterface, "BAS_CBMT_F", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "BAS_CBMT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BAS_CBMT_F", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
