using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.ReportViewers.Code
{
   partial class ReportViewers : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }
      private IRouter _Commons { get; set; }
   }
}
