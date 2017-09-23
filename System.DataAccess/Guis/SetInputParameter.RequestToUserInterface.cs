using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.DataAccess.Guis
{
    partial class SetInputParameter
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
            cb_SetInputParam.Checked = (bool)(job.Input as List<object>)[0];
            cb_InputParamType.Text = (string)(job.Input as List<object>)[1];
            object val3 = (job.Input as List<object>)[2];
            if (val3 != null)
            {
                if (val3 is string)
                    txt_InputParamValue.Text = val3.ToString();
                else
                    txt_InputParamValue.Text = "(dataset)";
            }

            ShowDialog();
            job.Output = new List<object>{
                cb_SetInputParam.Checked,
                cb_InputParamType.Text,
                txt_InputParamValue.Text
            };
            job.Status = StatusType.Successful;
        }
    }
}
