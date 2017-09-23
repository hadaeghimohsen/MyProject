using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.SrvDef.Code
{
   partial class Service
   {
      internal ISendRequest _Wall { get; set; }

      private Ui.Create _Create { get; set; }
      private Ui.ShowUpdateRemove _ShowUpdateRemove { get; set; }
   }
}
