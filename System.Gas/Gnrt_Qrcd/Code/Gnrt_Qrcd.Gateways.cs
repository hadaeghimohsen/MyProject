﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.Gas.Gnrt_Qrcd.Code
{
   partial class Gnrt_Qrcd
   {
      internal IRouter _DefaultGateway { get; set; }
      internal IRouter _Commons { get; set; }
   }
}
