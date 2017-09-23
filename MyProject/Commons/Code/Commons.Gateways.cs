using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.DataAccess.Odbccfg;

namespace MyProject.Commons.Code
{
   partial class Commons : IDefaultGateway
   {
      public IRouter _DefaultGateway { get; set; }

      internal ChangeHandling.Code.ChangeHandle _ChangeHandle { get; set; }
      internal LifeTime.Code.LifeTime _LifeTime { get; set; }
      internal ErrorHandling.Code.ErrorHandle _ErrorHandle { get; set; }
      internal HelpHandling.Code.HelpHandle _HelpHandle { get; set; }
      internal RedoLogs.Code.RedoLog _RedoLog { get; set; }
      internal Desktop.Code.Desktop _Desktop { get; set; }
      internal Form_Settings.Code.FORM_STNG _FORM_STNG { get; set; }

      internal Odbc _Odbc { get; set; }
   }
}
