﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class ShowAll : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }      

      public void SendRequest(Job job)
      {
         throw new NotImplementedException();
      }      
   }
}
