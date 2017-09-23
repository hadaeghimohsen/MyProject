using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.ConnectionStrings
{
    partial class ConnectionString : IMP
    {
        protected override void ExternalAssistance(Job jobs)
        {
            switch (jobs.Gateway)
            {
                case "Command":
                    _Command.Gateway(jobs);
                    break;
                case "Crypto":
                    _Crypto.Gateway(jobs);
                    break;
            }
        }

        protected override void InternalAssistance(Job job)
        {
            switch (job.Method)
            {
                case 01:
                    CreateConnectionString(job);
                    break;
            }
        }

        protected override void RequestToUserInterface(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
