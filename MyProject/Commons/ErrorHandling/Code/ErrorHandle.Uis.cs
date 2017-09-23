using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.ErrorHandling.Code
{
   partial class ErrorHandle
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.ErrorHandle _ErrorHandle { get; set; }
      internal Ui.ErrorMessage _ErrorMessage { get; set; }
   }
}
