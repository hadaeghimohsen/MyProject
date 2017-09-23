using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.DataGuard.Login.Code
{
   partial class Login
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.Login _Login { get; set; }
      internal Ui.LockScreen _LockScreen { get; set; }
      internal Ui.LastUserLogin _LastUserLogin { get; set; }
      internal Ui.SelectedLastUserLogin _SelectedLastUserLogin { get; set; }
   }
}
