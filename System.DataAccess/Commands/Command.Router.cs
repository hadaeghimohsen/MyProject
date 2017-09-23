using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Commands
{
    partial class Command : IMP
    {
        protected override void ExternalAssistance(Job jobs)
        {
            switch (jobs.Gateway)
            {
                case "Odbc":
                    _Odbc.Gateway(jobs);
                    break;
                case "O2dbc":
                    _O2dbc.Gateway(jobs);
                    break;
                default: break;
            }
        }

        protected override void InternalAssistance(Job job)
        {
            switch (job.Method)
            {
                case 01:
                    CreateOdbcCommand(job);
                    break;
                case 02:
                    CreateO2dbcCommand(job);
                    break;
                default: break;
            }
        }

        protected override void RequestToUserInterface(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
