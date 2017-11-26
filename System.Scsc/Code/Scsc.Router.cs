using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Scsc.Code
{
   partial class Scsc :IMP
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
               Mstr_Regl_F(job);
               break;
            case 04:
               Regl_Acnt_F(job);
               break;
            case 05:
               Regl_Expn_F(job);
               break;
            case 06:
               Regl_Dcmt_F(job);
               break;
            case 07:
               Mstr_Dcmt_F(job);
               break;
            case 08:
               Mstr_Mtod_F(job);
               break;
            case 09:
               Mstr_Regn_F(job);
               break;
            case 10:
               Mstr_Epit_F(job);
               break;               
            case 11:
               Mstr_Club_F(job);
               break;
            case 12:
               Main_Subs_F(job);
               break;
            case 13:
               Rqst_Type_F(job);
               break;
            case 14:
               Rqtr_Type_F(job);
               break;
            case 15:
               Adm_Rqst_F(job);
               break;
            case 16:
               Adm_Fsum_F(job);
               break;
            case 17:
               Adm_Sexp_F(job);
               break;
            case 18:
               Adm_Rexp_F(job);
               break;
            case 19:
               Adm_Save_F(job);
               break;
            case 20:
               Atn_Save_F(job);
               break;
            case 21:
               Tst_Rqst_F(job);
               break;
            case 22:
               Tst_Sexp_F(job);
               break;
            case 23:
               Tst_Rexp_F(job);
               break;
            case 24:
               Tst_Save_F(job);
               break;
            case 25:
               Cmp_Rqst_F(job);
               break;
            case 26:
               Cmp_Sexp_F(job);
               break;
            case 27:
               Cmp_Rexp_F(job);
               break;
            case 28:
               Cmp_Save_F(job);
               break;
            case 29:
               Psf_Rqst_F(job);
               break;
            case 30:
               Psf_Sexp_F(job);
               break;
            case 31:
               Psf_Rexp_F(job);
               break;
            case 32:
               Psf_Save_F(job);
               break;
            case 33:
               Clc_Rqst_F(job);
               break;
            case 34:
               Clc_Sexp_F(job);
               break;
            case 35:
               Clc_Rexp_F(job);
               break;
            case 36:
               Clc_Save_F(job);
               break;
            case 37:
               Hrz_Rqst_F(job);
               break;
            case 38:
               Hrz_Sexp_F(job);
               break;
            case 39:
               Hrz_Rexp_F(job);
               break;
            case 40:
               Hrz_Save_F(job);
               break;
            case 41:
               Exm_Rqst_F(job);
               break;
            case 42:
               Exm_Sexp_F(job);
               break;
            case 43:
               Exm_Rexp_F(job);
               break;
            case 44:
               Exm_Save_F(job);
               break;
            case 45:
               Lsi_Fldf_F(job);
               break;
            case 46:
               All_Fldf_F(job);
               break;
            case 47:
               Pbl_Rqst_F(job);
               break;
            case 48:
               Pbl_Sexp_F(job);
               break;
            case 49:
               Pbl_Rexp_F(job);
               break;
            case 50:
               Pbl_Save_F(job);
               break;
            case 51:
               Cmc_Rqst_F(job);
               break;
            case 52:
               Cmc_Sexp_F(job);
               break;
            case 53:
               Cmc_Rexp_F(job);
               break;
            case 54:
               Cmc_Save_F(job);
               break;
            case 55:
               Ucc_Rqst_F(job);
               break;
            case 56:
               Ucc_Sexp_F(job);
               break;
            case 57:
               Ucc_Rexp_F(job);
               break;
            case 58:
               Ucc_Save_F(job);
               break;
            case 59:
               Cmn_Dcmt_F(job);
               break;
            case 60:
               Mcc_Rqst_F(job);
               break;
            case 61:
               Mcc_Sexp_F(job);
               break;
            case 62:
               Mcc_Rexp_F(job);
               break;
            case 63:
               Mcc_Save_F(job);
               break;
            case 64:
               Adm_Totl_F(job);
               break;
            case 65:
               Cmn_Dise_F(job);
               break;
            case 66:
               Frst_Page_F(job);
               break;
            case 67:
               Pay_Cash_F(job);
               break;
            case 68:
               Cal_Expn_F(job);
               break;
            case 69:
               Cal_Cexc_F(job);
               break;
            case 70:
               Adm_Chng_F(job);
               break;
            case 71:
               Exm_Totl_F(job);
               break;
            case 72:
               Psf_Totl_F(job);
               break;
            case 73:
               Clc_Totl_F(job);
               break;
            case 74:
               Hrz_Totl_F(job);
               break;
            case 75:
               Cmc_Totl_F(job);
               break;
            case 76:
               Tst_Totl_F(job);
               break;
            case 77:
               Cmp_Totl_F(job);
               break;
            case 78:
               Com_Totl_F(job);
               break;
            case 79:
               Mcc_Totl_F(job);
               break;
            case 80:
               Ins_Totl_F(job);
               break;
            case 81:
               Cfg_Stng_F(job);
               break;
            case 82:
               Adm_Ends_F(job);
               break;
            case 83:
               Adm_Dsen_F(job);
               break;
            case 84:
               Rpt_Mngr_F(job);
               break;
            case 85:
               Rpt_Lrfm_F(job);
               break;
            case 86:
               Pay_Mtod_F(job);
               break;
            case 87:
               Mng_Rcan_F(job);
               break;
            case 88:
               Ntf_Totl_F(job);
               break;
            case 89:
               Rpt_List_F(job);
               break;
            case 90:
               Cal_Cexc_P(job);
               break;
            case 91:
               Acn_List_F(job);
               break;
            case 92:
               Oic_Totl_F(job);
               break;
            case 93:
               Pos_Totl_F(job);
               break;
            case 94:
               Cmn_Resn_F(job);
               break;
            case 95:
               Oic_Srzh_F(job);
               break;
            case 96:
               Oic_Sone_F(job);
               break;
            case 97:
               Oic_Smor_F(job);
               break;
            case 98:
               Bas_Cpr_F(job);
               break;
            case 99:
               New_Fngr_F(job);
               break;
            case 100:
               Fngr_Rsvd_F(job);
               break;
            case 101:
               Show_Atrq_F(job);
               break;
            case 102:
               Show_Acrq_F(job);
               break;
            case 103:
               Show_Ctrq_F(job);
               break;
            case 104:
               Show_Derq_F(job);
               break;
            case 105:
               Show_Ucrq_F(job);
               break;
            case 106:
               Show_Omrq_F(job);
               break;
            case 107:
               Show_Emrq_F(job);
               break;
           case 108: 
               Orgn_Totl_F(job);
               break;
            case 109:
               Rqst_Trac_F(job);
               break;
            case 110:
               Who_Aryu_F(job);
               break;
            case 111:
               Rfd_Totl_F(job);
               break;
            case 112:
               Show_Rfdt_F(job);
               break;
            case 113:
               Glr_Totl_F(job);
               break;
            case 114:
               Oic_Smsn_F(job);
               break;
            case 115:
               Chos_Clas_F(job);
               break;
            case 116:
               Msgb_Totl_F(job);
               break;
            case 117:
               Debt_List_F(job);
               break;
            case 118:
               Show_Glrl_F(job);
               break;
            case 119:
               Bdf_Pros_F(job);
               break;
            case 120:
               Bsc_Bmov_F(job);
               break;
            case 121:
               Aop_Mbsp_F(job);
               break;
            case 122:
               Main_Page_F(job);
               break;
            case 123:
               Adm_Figh_F(job);
               break;
            case 124:
               Aop_Cbmt_F(job);
               break;
            case 125:
               Aop_Mtod_F(job);
               break;
            case 126:
               Aop_Attn_F(job);
               break;
            case 127:
               Adm_Mbco_F(job);
               break;
            case 128:
               Strt_Menu_F(job);
               break;
            case 129:
               Adm_Hrsr_F(job);
               break;
            case 130:
               Adm_Brsr_F(job);
               break;
            case 131:
               Aop_Bufe_F(job);
               break;
            case 132:
               Adm_Hrsc_F(job);
               break;
            case 133:
               Adm_Mbfz_f(job);
               break;
            case 134:
               Adm_Mbsm_f(job);
               break;
            case 135:
               Rpt_Pmmt_f(job);
               break;
            case 136:
               Dap_Pivt_F(job);
               break;
            case 137:
               Dap_Chrt_F(job);
               break;
            case 138:
               Cnf_Stng_F(job);
               break;
            case 139:
               Dap_Dshb_F(job);
               break;
            case 140:
               Lsi_Fdlf_F(job);
               break;
            case 141:
               Attn_Dayn_F(job);
               break;
            case 142:
               Usr_Actn_F(job);
               break;
            case 143:
               Usr_Ctbl_F(job);
               break;
            case 144:
               Bas_Dfin_F(job);
               break;
            case 145:
               Bas_Adch_F(job);
               break;
            case 146:
               Bas_Adcl_F(job);
               break;
            case 147:
               Bas_Gruc_F(job);
               break;
            case 148:
               Gate_Actn_F(job);
               break;
            case 149:
               Bas_Wkdy_F(job);
               break;
            case 150:
               Tran_Expn_F(job);
               break;
            case 151:
               Mbsp_Chng_F(job);
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
            case "MSTR_PAGE_F":
               //_Mstr_Page_F.SendRequest(job);
               break;
            case "STRT_MENU_F":
               _Strt_Menu_F.SendRequest(job);
               break;
            case "MSTR_REGL_F":
               _Mstr_Regl_F.SendRequest(job);
               break;
            case "REGL_ACNT_F":
               _Regl_Acnt_F.SendRequest(job);
               break;
            case "REGL_EXPN_F":
               _Regl_Expn_F.SendRequest(job);
               break;
            case "REGL_DCMT_F":
               _Regl_Dcmt_F.SendRequest(job);
               break;
            case "MSTR_DCMT_F":
               _Mstr_Dcmt_F.SendRequest(job);
               break;
            case "MSTR_MTOD_F":
               _Mstr_Mtod_F.SendRequest(job);
               break;
            case "MSTR_REGN_F":
               _Mstr_Regn_F.SendRequest(job);
               break;
            case "MSTR_EPIT_F":
               _Mstr_Epit_F.SendRequest(job);
               break;
            case "MSTR_CLUB_F":
               _Mstr_Club_F.SendRequest(job);
               break;
            case "MAIN_SUBS_F":
               _Main_Subs_F.SendRequest(job);
               break;
            case "RQST_TYPE_F":
               _Rqst_Type_F.SendRequest(job);
               break;
            case "RQTR_TYPE_F":
               _Rqtr_Type_F.SendRequest(job);
               break;
            //case "ADM_RQST_F":
            //   _Adm_Rqst_F.SendRequest(job);
            //   break;
            //case "ADM_FSUM_F":
            //   _Adm_Fsum_F.SendRequest(job);
            //   break;
            //case "ADM_SEXP_F":
            //   _Adm_Sexp_F.SendRequest(job);
            //   break;
            //case "ADM_REXP_F":
            //   _Adm_Rexp_F.SendRequest(job);
            //   break;
            //case "ADM_SAVE_F":
            //   _Adm_Save_F.SendRequest(job);
            //   break;
            //case "ATN_SAVE_F":
            //   _Atn_Save_F.SendRequest(job);
            //   break;
            //case "TST_RQST_F":
            //   _Tst_Rqst_F.SendRequest(job);
            //   break;
            //case "TST_SEXP_F":
            //   _Tst_Sexp_F.SendRequest(job);
            //   break;
            //case "TST_REXP_F":
            //   _Tst_Rexp_F.SendRequest(job);
            //   break;
            //case "TST_SAVE_F":
            //   _Tst_Save_F.SendRequest(job);
            //   break;
            //case "CMP_RQST_F":
            //   _Cmp_Rqst_F.SendRequest(job);
            //   break;
            //case "CMP_SEXP_F":
            //   _Cmp_Sexp_F.SendRequest(job);
            //   break;
            //case "CMP_REXP_F":
            //   _Cmp_Rexp_F.SendRequest(job);
            //   break;
            //case "CMP_SAVE_F":
            //   _Cmp_Save_F.SendRequest(job);
            //   break;
            //case "PSF_RQST_F":
            //   _Psf_Rqst_F.SendRequest(job);
            //   break;
            //case "PSF_SEXP_F":
            //   _Psf_Sexp_F.SendRequest(job);
            //   break;
            //case "PSF_REXP_F":
            //   _Psf_Rexp_F.SendRequest(job);
            //   break;
            //case "PSF_SAVE_F":
            //   _Psf_Save_F.SendRequest(job);
            //   break;
            //case "CLC_RQST_F":
            //   _Clc_Rqst_F.SendRequest(job);
            //   break;
            //case "CLC_SEXP_F":
            //   _Clc_Sexp_F.SendRequest(job);
            //   break;
            //case "CLC_REXP_F":
            //   _Clc_Rexp_F.SendRequest(job);
            //   break;
            //case "CLC_SAVE_F":
            //   _Clc_Save_F.SendRequest(job);
            //   break;
            //case "HRZ_RQST_F":
            //   _Hrz_Rqst_F.SendRequest(job);
            //   break;
            //case "HRZ_SEXP_F":
            //   _Hrz_Sexp_F.SendRequest(job);
            //   break;
            //case "HRZ_REXP_F":
            //   _Hrz_Rexp_F.SendRequest(job);
            //   break;
            //case "HRZ_SAVE_F":
            //   _Hrz_Save_F.SendRequest(job);
            //   break;
            //case "EXM_RQST_F":
            //   _Exm_Rqst_F.SendRequest(job);
            //   break;
            //case "EXM_SEXP_F":
            //   _Exm_Sexp_F.SendRequest(job);
            //   break;
            //case "EXM_REXP_F":
            //   _Exm_Rexp_F.SendRequest(job);
            //   break;
            //case "EXM_SAVE_F":
            //   _Exm_Save_F.SendRequest(job);
            //   break;
            case "LSI_FLDF_F":
               _Lsi_Fldf_F.SendRequest(job);
               break;
            case "ALL_FLDF_F":
               _All_Fldf_F.SendRequest(job);
               break;
            //case "PBL_RQST_F":
            //   _Pbl_Rqst_F.SendRequest(job);
            //   break;
            //case "PBL_SEXP_F":
            //   _Pbl_Sexp_F.SendRequest(job);
            //   break;
            //case "PBL_REXP_F":
            //   _Pbl_Rexp_F.SendRequest(job);
            //   break;
            //case "PBL_SAVE_F":
            //   _Pbl_Save_F.SendRequest(job);
            //   break;
            //case "CMC_RQST_F":
            //   _Cmc_Rqst_F.SendRequest(job);
            //   break;
            //case "CMC_SEXP_F":
            //   _Cmc_Sexp_F.SendRequest(job);
            //   break;
            //case "CMC_REXP_F":
            //   _Cmc_Rexp_F.SendRequest(job);
            //   break;
            //case "CMC_SAVE_F":
            //   _Cmc_Save_F.SendRequest(job);
            //   break;
            //case "UCC_RQST_F":
            //   _Ucc_Rqst_F.SendRequest(job);
            //   break;
            //case "UCC_SEXP_F":
            //   _Ucc_Sexp_F.SendRequest(job);
            //   break;
            //case "UCC_REXP_F":
            //   _Ucc_Rexp_F.SendRequest(job);
            //   break;
            //case "UCC_SAVE_F":
            //   _Ucc_Save_F.SendRequest(job);
            //   break;
            case "CMN_DCMT_F":
               _Cmn_Dcmt_F.SendRequest(job);
               break;
            //case "MCC_RQST_F":
            //   _Mcc_Rqst_F.SendRequest(job);
            //   break;
            //case "MCC_SEXP_F":
            //   _Mcc_Sexp_F.SendRequest(job);
            //   break;
            //case "MCC_REXP_F":
            //   _Mcc_Rexp_F.SendRequest(job);
            //   break;
            //case "MCC_SAVE_F":
            //   _Mcc_Save_F.SendRequest(job);
            //   break;
            case "ADM_TOTL_F":
               _Adm_Totl_F.SendRequest(job);
               break;
            case "CMN_DISE_F":
               _Cmn_Dise_F.SendRequest(job);
               break;
            case "FRST_PAGE_F":
               _Frst_Page_F.SendRequest(job);
               break;
            case "PAY_CASH_F":
               _Pay_Cash_F.SendRequest(job);
               break;
            case "CAL_EXPN_F":
               _Cal_Expn_F.SendRequest(job);
               break;
            case "CAL_CEXC_F":
               _Cal_Cexc_F.SendRequest(job);
               break;
            case "ADM_CHNG_F":
               _Adm_Chng_F.SendRequest(job);
               break;
            case "EXM_TOTL_F":
               _Exm_Totl_F.SendRequest(job);
               break;
            case "PSF_TOTL_F":
               _Psf_Totl_F.SendRequest(job);
               break;
            case "CLC_TOTL_F":
               _Clc_Totl_F.SendRequest(job);
               break;
            case "HRZ_TOTL_F":
               _Hrz_Totl_F.SendRequest(job);
               break;
            case "CMC_TOTL_F":
               _Cmc_Totl_F.SendRequest(job);
               break;
            case "TST_TOTL_F":
               _Tst_Totl_F.SendRequest(job);
               break;
            case "CMP_TOTL_F":
               _Cmp_Totl_F.SendRequest(job);
               break;
            //case "COM_TOTL_F":
            //   _Com_Totl_F.SendRequest(job);
            //   break;
            case "MCC_TOTL_F":
               _Mcc_Totl_F.SendRequest(job);
               break;
            case "INS_TOTL_F":
               _Ins_Totl_F.SendRequest(job);
               break;
            case "CFG_STNG_F":
               _Cfg_Stng_F.SendRequest(job);
               break;
            case "ADM_ENDS_F":
               _Adm_Ends_F.SendRequest(job);
               break;
            case "ADM_DSEN_F":
               _Adm_Dsen_F.SendRequest(job);
               break;
            case "RPT_MNGR_F":
               _Rpt_Mngr_F.SendRequest(job);
               break;
            case "RPT_LRFM_F":
               _Rpt_Lrfm_F.SendRequest(job);
               break;
            case "PAY_MTOD_F":
               _Pay_Mtod_F.SendRequest(job);
               break;
            case "MNG_RCAN_F":
               _Mng_Rcan_F.SendRequest(job);
               break;
            case "NTF_TOTL_F":
               _Ntf_Totl_F.SendRequest(job);
               break;
            case "RPT_LIST_F":
               _Rpt_List_F.SendRequest(job);
               break;
            case "CAL_CEXC_P":
               _Cal_Cexc_P.SendRequest(job);
               break;
            case "ACN_LIST_F":
               _Acn_List_F.SendRequest(job);
               break;
            case "OIC_TOTL_F":
               _Oic_Totl_F.SendRequest(job);
               break;
            case "POS_TOTL_F":
               _Pos_Totl_F.SendRequest(job);
               break;
            case "CMN_RESN_F":
               _Cmn_Resn_F.SendRequest(job);
               break;
            case "OIC_SRZH_F":
               _Oic_Srzh_F.SendRequest(job);
               break;
            case "OIC_SONE_F":
               _Oic_Sone_F.SendRequest(job);
               break;
            case "OIC_SMOR_F":
               _Oic_Smor_F.SendRequest(job);
               break;
            case "BAS_CPR_F":
               _Bas_Cpr_F.SendRequest(job);
               break;
            case "NEW_FNGR_F":
               _New_Fngr_F.SendRequest(job);
               break;
            case "FNGR_RSVD_F":
               _Fngr_Rsvd_F.SendRequest(job);
               break;
            case "ORGN_TOTL_F":
               _Orgn_Totl_F.SendRequest(job);
               break;
            case "RQST_TRAC_F":
               _Rqst_Trac_F.SendRequest(job);
               break;
            case "WHO_ARYU_F":
               _Who_Aryu_F.SendRequest(job);
               break;
            case "RFD_TOTL_F":
               _Rfd_Totl_F.SendRequest(job);
               break;
            case "GLR_TOTL_F":
               _Glr_Totl_F.SendRequest(job);
               break;
            case "OIC_SMSN_F":
               _Oic_Smsn_F.SendRequest(job);
               break;
            case "CHOS_CLAS_F":
               _Chos_Clas_F.SendRequest(job);
               break;
            case "MSGB_TOTL_F":
               _Msgb_Totl_F.SendRequest(job);
               break;
            case "DEBT_LIST_F":
               _Debt_List_F.SendRequest(job);
               break;
            case "BDF_PROS_F":
               _Bdf_Pros_F.SendRequest(job);
               break;
            case "BSC_BMOV_F":
               _Bsc_Bmov_F.SendRequest(job);
               break;
            case "AOP_MBSP_F":
               _Aop_Mbsp_F.SendRequest(job);
               break;
            case "MAIN_PAGE_F":
               _Main_Page_F.SendRequest(job);
               break;
            case "ADM_FIGH_F":
               _Adm_Figh_F.SendRequest(job);
               break;
            case "AOP_CBMT_F":
               _Aop_Cbmt_F.SendRequest(job);
               break;
            case "AOP_MTOD_F":
               _Aop_Mtod_F.SendRequest(job);
               break;
            case "AOP_ATTN_F":
               _Aop_Attn_F.SendRequest(job);
               break;
            case "ADM_MBCO_F":
               _Adm_Mbco_F.SendRequest(job);
               break;
            case "ADM_HRSR_F":
               _Adm_Hrsr_F.SendRequest(job);
               break;
            case "ADM_BRSR_F":
               _Adm_Brsr_F.SendRequest(job);
               break;
            case "AOP_BUFE_F":
               _Aop_Bufe_F.SendRequest(job);
               break;
            case "ADM_HRSC_F":
               _Adm_Hrsc_F.SendRequest(job);
               break;
            case "ADM_MBFZ_F":
               _Adm_Mbfz_F.SendRequest(job);
               break;
            case "ADM_MBSM_F":
               _Adm_Mbsm_F.SendRequest(job);
               break;
            case "RPT_PMMT_F":
               _Rpt_Pmmt_F.SendRequest(job);
               break;
            case "DAP_PIVT_F":
               _Dap_Pivt_F.SendRequest(job);
               break;
            case "DAP_CHRT_F":
               _Dap_Chrt_F.SendRequest(job);
               break;
            case "CNF_STNG_F":
               _Cnf_Stng_F.SendRequest(job);
               break;
            case "DAP_DSHB_F":
               _Dap_Dshb_F.SendRequest(job);
               break;
            case "LSI_FDLF_F":
               _Lsi_Fdlf_F.SendRequest(job);
               break;
            case "ATTN_DAYN_F":
               _Attn_Dayn_F.SendRequest(job);
               break;
            case "USR_ACTN_F":
               _Usr_Actn_F.SendRequest(job);
               break;
            case "USR_CTBL_F":
               _Usr_Ctbl_F.SendRequest(job);
               break;
            case "BAS_DFIN_F":
               _Bas_Dfin_F.SendRequest(job);
               break;
            case "BAS_ADCH_F":
               _Bas_Adch_F.SendRequest(job);
               break;
            case "BAS_ADCL_F":
               _Bas_Adcl_F.SendRequest(job);
               break;
            case "BAS_GRUC_F":
               _Bas_Gruc_F.SendRequest(job);
               break;
            case "GATE_ACTN_F":
               _Gate_Actn_F.SendRequest(job);
               break;
            case "BAS_WKDY_F":
               _Bas_Wkdy_F.SendRequest(job);
               break;
            case "TRAN_EXPN_F":
               _Tran_Expn_F.SendRequest(job);
               break;
            case "MBSP_CHNG_F":
               _Mbsp_Chng_F.SendRequest(job);
               break;

            /* Show Changed */
            case "SHOW_ATRQ_F":
               _Show_Atrq_F.SendRequest(job);
               break;
            case "SHOW_ACRQ_F":
               _Show_Acrq_F.SendRequest(job);
               break;
            case "SHOW_CTRQ_F":
               _Show_Ctrq_F.SendRequest(job);
               break;
            case "SHOW_DERQ_F":
               _Show_Derq_F.SendRequest(job);
               break;
            case "SHOW_UCRQ_F":
               _Show_Ucrq_F.SendRequest(job);
               break;
            case "SHOW_OMRQ_F":
               _Show_Omrq_F.SendRequest(job);
               break;
            case "SHOW_EMRQ_F":
               _Show_Emrq_F.SendRequest(job);
               break;
            case "SHOW_RFDT_F":
               _Show_Rfdt_F.SendRequest(job);
               break;
            case "SHOW_GLRL_F":
               _Show_Glrl_F.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
