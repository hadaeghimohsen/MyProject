using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.WorkFlow.Code
{
   partial class WorkFlow
   {
      private ISendRequest _Wall { get; set; }

      private Ui.RPT_SRPT_F _RPT_SRPT_F { get; set; }
      private Ui.RPT_SRCH_F _RPT_SRCH_F { get; set; }
      private Ui.RPT_MBAR_M _RPT_MBAR_M { get; set; }

      private Ui.PRF_SPRF_F _PRF_SPRF_F { get; set; }
      private Ui.PRF_SRCH_F _PRF_SRCH_F { get; set; }
      private Ui.PRF_MBAR_M _PRF_MBAR_M { get; set; }
      private Ui.PRF_CHNG_F _PRF_CHNG_F { get; set; }

      private Ui.WHR_SCON_F _WHR_SCON_F { get; set; }
   }
}
