using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.RoboTech.Controller
{
   interface IRobotController
   {
      XElement GetData(XElement x);
   }
}
