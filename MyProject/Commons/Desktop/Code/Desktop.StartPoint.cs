using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.Desktop.Code
{
   internal partial class Desktop
   {
      internal Desktop(ISendRequest _wall)
      {
         _Wall = _wall;
      }
   }
}
