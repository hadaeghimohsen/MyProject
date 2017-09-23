using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Transactions
{
    partial class Transaction : IMP
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
                    ExecuteOdbcTransaction(job);
                    break;
                case 02:
                    ExecuteOdbcNonQuery(job);
                    break;
                case 03:
                    ExecuteOdbcScalar(job);
                    break;
                case 04:
                    ExecuteOdbcFillSharedPoolQuery(job);
                    break;
               case 05:
                    ExecuteQuery(job);
                    break;
                //case 05:
                //    ExecuteO2dbcTransaction(job);
                //    break;
                //case 06:
                //    ExecuteO2dbcNonQuery(job);
                //    break;
                //case 07:
                //    ExecuteO2dbcScalar(job);
                //    break;
                //case 08:
                //    ExecuteO2dbcFillSharedPoolQuery(job);
                //    break;
            }
        }

        protected override void RequestToUserInterface(Job job)
        {
            throw new NotImplementedException();
        }
    }
}
