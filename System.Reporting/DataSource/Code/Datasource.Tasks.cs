using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.Reporting.DataSource.Code
{
   partial class Datasource
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "configuration")
         {
            if (_Configuration == null)
               _Configuration = new Ui.Configuration { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      private void DoWork4Configuration(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "configuration"},
                  new Job(SendType.SelfToUserInterface, "Configuration", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "Configuration", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "Configuration" , 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
