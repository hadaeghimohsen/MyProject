using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.DataAccess.O2dbccfg
{
    partial class O2dbc : IMP
    {
        protected override void ExternalAssistance(Job jobs)
        {
            switch (jobs.Gateway)
            {
                case "Command":
                    _Command.Gateway(jobs);
                    break;
                case "Transaction":
                    _Transaction.Gateway(jobs);
                    break;
                case "DefaultGateway":
                    _Default.Gateway(jobs);
                    break;
            }
        }

        protected override void InternalAssistance(Job job)
        {
            switch (job.Method)
            {
                case 01:
                    BeginTransaction(job);
                    break;
                case 02:
                    CommandTypeValidation(job);
                    break;
                case 03:
                    InputParameterValidation(job);
                    break;
                case 04:
                    CommandNameValidation(job);
                    break;
                case 05:
                    TnsNameValidation(job);
                    break;
                case 06:
                    UserNameValidation(job);
                    break;

            }
        }

        protected override void RequestToUserInterface(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
