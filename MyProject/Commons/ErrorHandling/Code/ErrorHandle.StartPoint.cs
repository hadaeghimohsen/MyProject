using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.ErrorHandling.Code
{
   internal partial class ErrorHandle
   {
      internal ErrorHandle(ISendRequest _wall)
      {
         _Wall = _wall;

         _ErrorHandle = new Ui.ErrorHandle { _DefaultGateway = this };
         _ErrorMessage = new Ui.ErrorMessage { _DefaultGateway = this };
      }
   }
}
