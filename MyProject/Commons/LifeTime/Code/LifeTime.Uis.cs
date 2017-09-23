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
       internal ISendRequest _Wall { get; set; }

       internal Ui.ToolOperation _ToolOperation {get; set;}
    }
}
