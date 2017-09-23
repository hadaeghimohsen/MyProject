using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.JobRouting.Jobs;

namespace System.JobRouting.Routering
{
    public interface IRouter
    {
        void Gateway(Job worklist);
    }
}
