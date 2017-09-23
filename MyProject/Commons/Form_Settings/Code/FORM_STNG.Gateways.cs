using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.Form_Settings.Code
{
   partial class FORM_STNG : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
   }
}
