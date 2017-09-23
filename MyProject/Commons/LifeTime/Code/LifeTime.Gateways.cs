using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace MyProject.Commons.LifeTime.Code
{
    partial class LifeTime : IDefaultGateway
    {
       public IRouter _DefaultGateway { get; set; }
    }
}
