using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.Desktop.Code
{
   partial class Desktop
   {
      internal ISendRequest _Wall { get; set; }

      internal Ui.Desktop _Desktop { get; set; }
      internal Ui.StartDrawer _StartDrawer { get; set; }
      internal Ui.SettingsDrawer _SettingsDrawer { get; set; }
      internal Ui.StartMenu _StartMenu { get; set; }
   }
}
