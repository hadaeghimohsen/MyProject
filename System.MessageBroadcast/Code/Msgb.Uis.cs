using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.MessageBroadcast.Code
{
   partial class Msgb
   {
      public ISendRequest _Wall { get; set; }

      internal Ui.MasterPage.MSTR_PAGE_F _Mstr_Page_F { get; set; }
   }
}
