using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.Code
{
   partial class Commons
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.DateTimes _DateTimes { get; set; }
      internal Ui.Shutdown _Shutdown { get; set; }
      internal Ui.GMapNets _GMapNets { get; set; }
   }
}
