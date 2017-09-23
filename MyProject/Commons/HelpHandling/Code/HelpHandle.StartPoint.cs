using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.HelpHandling.Code
{
   internal partial class HelpHandle
   {
      internal HelpHandle(ISendRequest _wall)
      {
         _Wall = _wall;

         _HelpHandle = new Ui.HelpHandle { _DefaultGateway = this };
      }
   }
}
