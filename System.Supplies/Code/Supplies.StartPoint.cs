﻿using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Supplies.Code
{
   public partial class Supplies
   {
      public Supplies(IRouter _commons, ISendRequest _wall)
      {
         _Commons = _commons;
         _Wall = _wall;
      }
   }
}
