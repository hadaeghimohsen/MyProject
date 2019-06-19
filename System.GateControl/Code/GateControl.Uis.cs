using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.GateControl.Code
{
   partial class GateControl
   {
      public ISendRequest _Wall { get; set; }

      internal Ui.MasterPage.FRST_PAGE_F _Frst_Page_F { get; set; }
   }
}
