using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.LifeTime.Code
{
    partial class LifeTime
    {
       internal LifeTime(ISendRequest _wall)
       {
          _Wall = _wall;

          _ToolOperation = new Ui.ToolOperation() { _DefaultGateway = this };
       }
    }
}
