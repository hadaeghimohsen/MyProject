using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace MyProject.Commons.ChangeHandling.Code
{
   partial class ChangeHandle
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input as string;
         if(value.ToLower() == "changename")
         {
            if(_ChangeName == null)
               _ChangeName = new Ui.ChangeName { _DefaultGateway = this };
            job.Output = _ChangeName;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ChangeHandling(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ChangeName"},
                  new Job(SendType.SelfToUserInterface, "ChangeName", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ChangeName", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
