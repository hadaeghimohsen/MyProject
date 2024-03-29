﻿using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.RoboTech.Code
{
   partial class RoboTech
   {
      public ISendRequest _Wall { get; set; }

      private Ui.MasterPage.FRST_PAGE_F _Frst_Page_F { get; set; }
      private Ui.BaseDefinition.REGN_DFIN_F _Regn_Dfin_F { get; set; }
      private Ui.BaseDefinition.ISIC_DFIN_F _Isic_Dfin_F { get; set; }
      private Ui.BaseDefinition.ORGN_DFIN_F _Orgn_Dfin_F { get; set; }
      private Ui.BaseDefinition.WGUL_DFIN_F _Wgul_Dfin_F { get; set; }
      private Ui.DevelopmentApplication.ORGN_DVLP_F _Orgn_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.RBKN_DVLP_F _Rbkn_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.RBSR_DVLP_F _Rbsr_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.RBSA_DVLP_F _Rbsa_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.RBOD_DVLP_F _Rbod_Dvlp_F { get; set; }
      private Ui.Action.STRT_ROBO_F _Strt_Robo_F { get; set; }
      private Ui.DevelopmentApplication.RBMN_DVLP_F _Rbmn_Dvlp_F { get; set; }
      private Ui.Action.STNG_RPRT_F _Stng_Rprt_F { get; set; }
      private Ui.Action.RPT_MNGR_F _Rpt_Mngr_F { get; set; }
      private Ui.Action.RPT_LRFM_F _Rpt_Lrfm_F { get; set; }
      private Ui.DevelopmentApplication.ODRM_DVLP_F _Odrm_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.ORML_DVLP_F _Orml_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.RBSM_DVLP_F _Rbsm_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.SRBT_INFO_F _Srbt_Info_F { get; set; }
      private Ui.DevelopmentApplication.SALE_DVLP_F _Sale_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.ALPK_DVLP_F _Alpk_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.ORDR_SHIP_F _Ordr_Ship_F { get; set; }
      private Ui.DevelopmentApplication.ORDR_RCPT_F _Ordr_Rcpt_F { get; set; }
      private Ui.DevelopmentApplication.BANK_DVLP_F _Bank_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.PROD_DVLP_F _Prod_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.WLET_DVLP_F _Wlet_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.ONRO_DVLP_F _Onro_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.INST_CONF_F _Inst_Conf_F { get; set; }
      private Ui.DevelopmentApplication.MESG_DVLP_F _Mesg_Dvlp_F { get; set; }
      private Ui.DevelopmentApplication.CASH_CNTR_F _Cash_Cntr_F { get; set; }
      private Ui.DevelopmentApplication.TREE_BASE_F _Tree_Base_F { get; set; }
      private Ui.DevelopmentApplication.INVC_OPRT_F _Invc_Oprt_F { get; set; }

      private Ui.Inspection.MNGR_INSP_F _Mngr_Insp_F { get; set; }
   }   
}


