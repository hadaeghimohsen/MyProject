using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Reporting.Self.Code
{
   partial class Reporting 
   {
      internal ISendRequest _Wall { get; set; }

      private Ui.ReportCtrl _ReportCtrl { get; set; }
      private Ui.SettingsMetro _SettingsMetro { get; set; }
   }
}
