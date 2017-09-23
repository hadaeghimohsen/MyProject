using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.ChangeHandling.Code
{
   internal partial class ChangeHandle
   {
      public ChangeHandle(ISendRequest _wall)
      {
         _Wall = _wall;
      }
   }
}
