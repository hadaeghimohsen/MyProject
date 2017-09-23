using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ISP.Code
{
   partial class ISP
   {
      public ISendRequest _Wall { get; set; }
      
      internal Ui.MasterPage.FRST_PAGE_F _Frst_Page_F { get; set; }
      internal Ui.BaseDefination.REGN_DFIN_F _Regn_Dfin_F { get; set; }
      internal Ui.BaseDefination.EPIT_DFIN_F _Epit_Dfin_F { get; set; }
      internal Ui.BaseDefination.BTRF_DFIN_F _Btrf_Dfin_F { get; set; }
      internal Ui.BaseDefination.CASH_DFIN_F _Cash_Dfin_F { get; set; }
      internal Ui.BaseDefination.REGL_DFIN_F _Regl_Dfin_F { get; set; }
      internal Ui.BaseDefination.RQRQ_DFIN_F _Rqrq_Dfin_F { get; set; }
      internal Ui.BaseDefination.DCSP_DFIN_F _Dcsp_Dfin_F { get; set; }
      internal Ui.BaseDefination.ORGN_DFIN_F _Orgn_Dfin_F { get; set; }
      internal Ui.Admission.ADM_ADSL_F _Adm_Adsl_F { get; set; }
      internal Ui.Payment.PAY_MTOD_F _Pay_Mtod_F { get; set; }
   }
}
