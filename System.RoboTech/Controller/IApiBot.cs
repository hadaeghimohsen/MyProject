using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.RoboTech.Controller
{
   internal interface IApiBot
   {
      Data.Robot Robot {get;}
      bool Started { get; }
      string BotName { get; }
      void StopReceiving();
   }
}
