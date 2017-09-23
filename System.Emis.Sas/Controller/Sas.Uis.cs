using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Emis.Sas.Controller
{
   partial class Sas
   {
      internal ISendRequest _Wall { get; set; }

      internal View.MSTR_PAGE_F _Mstr_Page_F { get; set; }
      internal View.MSTR_SERV_F _Mstr_Serv_F { get; set; }
      internal View.MSTR_RQST_F _Mstr_Rqst_F { get; set; }
      internal View.SERV_BILL_F _SERV_BILL_F { get; set; }
      internal View.SERV_INFO_F _SERV_INFO_F { get; set; }
      internal View.SERV_DART_F _SERV_DART_F { get; set; }
      internal View.SERV_RQST_F _SERV_RQST_F { get; set; }
      internal View.PBLC_SERV_F _PBLC_SERV_F { get; set; }
      internal View.MSTR_REGL_F _MSTR_REGL_F { get; set; }
   }
}
