using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.DataAccess.Guis
{
    partial class ChooseCommandType
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
            switch (cb_CommandType.Text)
            {
                case "store procedure":
                    job.Output = "procedure";
                    break;
                default:
                    job.Output = "func";
                    break;
            }            
            job.Status = StatusType.Successful;
        }
    }
}
