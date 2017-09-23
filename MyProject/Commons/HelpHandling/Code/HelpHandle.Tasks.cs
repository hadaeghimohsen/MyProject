using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.HelpHandling.Code
{
   partial class HelpHandle
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4HelpHandling(Job job)
      {
         job.Output = job.Input;
         job.Next = 
            new Job(SendType.SelfToUserInterface, "HelpHandle", 02 /* Execute Set */,
               new Job(SendType.SelfToUserInterface, "HelpHandle", 03 /* Execute Paint */,
                  new Job(SendType.SelfToUserInterface, "HelpHandle", 05 /* Execute OpenDrawer */))) { WhereIsInputData = WhereIsInputDataType.StepBack};
         job.Status = StatusType.Successful;
      }
   }
}
