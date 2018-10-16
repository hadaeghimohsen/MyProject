using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Setup.Code
{
   public partial class Setup
   {
      public Setup(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
