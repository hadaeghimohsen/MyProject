using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DataAccess.Odbccfg;
using System.JobRouting.Routering;
using System.Xml.Linq;

namespace MyProject.Commons.Code
{
   internal partial class Commons
   {
      internal Commons(ISendRequest _wall)
      {
         _Wall = _wall;
         _DateTimes = new Ui.DateTimes { _DefaultGateway = this };

         //_ChangeHandle = new ChangeHandling.Code.ChangeHandle(_wall) { _DefaultGateway = this };
         //_LifeTime = new LifeTime.Code.LifeTime(_wall) { _DefaultGateway = this };
         _ErrorHandle = new ErrorHandling.Code.ErrorHandle(_wall) { _DefaultGateway = this };
         _HelpHandle = new HelpHandling.Code.HelpHandle(_wall) { _DefaultGateway = this };
         //_RedoLog = new RedoLogs.Code.RedoLog(_wall) { _DefaultGateway = this };
         _Desktop = new Desktop.Code.Desktop(_wall) { _DefaultGateway = this };
         //_FORM_STNG = new Form_Settings.Code.FORM_STNG(_wall) { _DefaultGateway = this };
         _Shutdown = new Ui.Shutdown { _DefaultGateway = this };
         _GMapNets = new Ui.GMapNets { _DefaultGateway = this };

         _Odbc = new Odbc { _DefaultGateway = this };
      }

      private XElement _HostInfo;
   }
}
