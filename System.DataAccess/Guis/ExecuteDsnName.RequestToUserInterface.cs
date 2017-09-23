using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Guis
{
    partial class ExecuteDsnName
    {
        internal Odbccfg.Odbc _Odbc { get; set; }

        internal void RequestToUserInterface(Job job)
        {
            switch (job.Method)
            {
                case 01:
                    ShowDialog(job);
                    break;
            }
        }

        /// <summary>
        /// Code 01
        /// </summary>
        /// <param name="job"></param>
        private void ShowDialog(Job job)
        {
            ShowDialog();
            job.Output = cmb_DnsNames.Text;
            job.Status = StatusType.Successful;
        }
    }
}
