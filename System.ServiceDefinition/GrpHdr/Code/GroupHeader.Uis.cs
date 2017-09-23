using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.GrpHdr.Code
{
   partial class GroupHeader
   {
      internal ISendRequest _Wall { get; set; }

      private Ui.GroupHeaderMenus _GroupHeaderMenus { get; set; }
      private Ui.Create _Create { get; set; }
      private Ui.Update _Update { get; set; }
      private Ui.Duplicate _Duplicate { get; set; }
   }
}
