using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.RedoLogs.Code
{
   partial class RedoLog
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.RedoLog _RedoLog { get; set; }
   }
}
