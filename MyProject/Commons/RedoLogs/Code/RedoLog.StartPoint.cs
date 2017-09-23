using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.RedoLogs.Code
{
   internal partial class RedoLog
   {
      public RedoLog(ISendRequest _wall)
      {
         _Wall = _wall;

         _RedoLog = new Ui.RedoLog { _DefaultGateway = this };
      }
   }
}
