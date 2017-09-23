using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace MyProject.Commons.ErrorHandling.Code
{
   partial class ErrorHandle
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ErrorHandling(Job job)
      {
         job.Output = job.Input;
         job.Next =
            new Job(SendType.SelfToUserInterface, "ErrorHandle", 02 /* Execute Set */)
            {
               WhereIsInputData = WhereIsInputDataType.StepBack,
               Next = new Job(SendType.SelfToUserInterface, "ErrorHandle", 03 /* Execute Paint */)
               {
                  Next = new Job(SendType.SelfToUserInterface, "ErrorHandle", 05 /* Execute OpenDrawer */) 
               }
            };
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ErrorMessage(Job job)
      {
         job.Output = job.Input;
         job.Next =
            new Job(SendType.SelfToUserInterface, "ErrorMessage", 02 /* Execute Set */)
            {
               WhereIsInputData = WhereIsInputDataType.StepBack,
               Next = new Job(SendType.SelfToUserInterface, "ErrorMessage", 03 /* Execute Paint */)
               {
                  //Next = new Job(SendType.SelfToUserInterface, "ErrorHandle", 07 /* Execute LoadData */) { }
               }
            };
         job.Status = StatusType.Successful;
      }
   }
}
