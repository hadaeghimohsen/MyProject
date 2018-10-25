using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Setup.Code
{
   partial class Setup
   {
      public ISendRequest _Wall { get; set; }

      internal Ui.LTR.MasterPage.FRST_PAGE_F _Frst_Page_F { get; set; }
      internal Ui.LTR.License.CHK_LICN_F _Chk_Licn_F { get; set; }
      internal Ui.LTR.Server.SQL_CONF_F _Sql_Conf_F { get; set; }
      internal Ui.LTR.License.CHK_TINY_F _Chk_Tiny_F { get; set; }
   }
}
