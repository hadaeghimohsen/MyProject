using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Guis
{
    partial class Build
    {
        internal Odbccfg.Odbc _Odbc { get; set; }

        internal void RequestToUserInterface(Job job)
        {
            switch (job.Method)
            {
                case 01:
                    ShowDialog(job);
                    break;
                case 02:
                    Close(job);
                    break;
                case 03:
                    Refresh(job);
                    break;
                case 04:
                    GetInstalledDrivers(job);
                    break;
            }
        }

        /// <summary>
        /// Code 01
        /// </summary>
        /// <param name="job"></param>
        private void ShowDialog(Job job)
        {
            job.Status = StatusType.Successful;
            ShowDialog();
        }

        /// <summary>
        /// Code 02
        /// </summary>
        /// <param name="job"></param>
        private void Close(Job job)
        {
            Close();
            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 03
        /// </summary>
        /// <param name="job"></param>
        private void Refresh(Job job)
        {
            btn_Refresh_Click(null, null);
            job.Status = StatusType.Successful;
        }

        /// <summary>
        /// Code 04
        /// </summary>
        /// <param name="job"></param>
        private void GetInstalledDrivers(Job job)
        {
            cb_DriverList.Items.Clear();
            (job.Input as List<string>).ForEach(driver => cb_DriverList.Items.Add(driver));
            job.Status = StatusType.Successful;
        }
    }
}
