using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.ConnectionStrings
{
    partial class Crypto : IMP
    {
        protected override void ExternalAssistance(Job jobs)
        {
            switch (jobs.Gateway)
            {
                case "ConnectionString":
                    _ConnectionString.Gateway(jobs);
                    break;
            }
        }

        protected override void InternalAssistance(Job job)
        {
            throw new NotImplementedException();
        }

        protected override void RequestToUserInterface(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
