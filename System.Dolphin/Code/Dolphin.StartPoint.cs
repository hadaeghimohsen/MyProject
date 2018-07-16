using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Dolphin.Code
{
   public partial class Dolphin
   {
      public Dolphin(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
