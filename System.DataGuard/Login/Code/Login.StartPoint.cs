using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataGuard.Login.Code
{
   internal partial class Login
   {
      internal Login(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;

         _Login = new Ui.Login { _DefaultGateway = this };
         _LockScreen = new Ui.LockScreen { _DefaultGateway = this };
         _LastUserLogin = new Ui.LastUserLogin { _DefaultGateway = this };
         _SelectedLastUserLogin = new Ui.SelectedLastUserLogin { _DefaultGateway = this };
         _PinCode = new Ui.PinCode { _DefaultGateway = this };
      }

      private string ConnectionString;
      private Data.iProjectDataContext iProject;
   }
}
