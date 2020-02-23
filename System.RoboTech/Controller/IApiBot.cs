using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.RoboTech.Controller
{
   internal interface IApiBot
   {
      Data.Robot Robot {get;}
      bool Started { get; }
      string BotName { get; }
      void StopReceiving();
      void SendAction(XElement x);
   }
}
