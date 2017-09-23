using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Commons.Form_Settings.Code
{
   partial class FORM_STNG
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();

         if (value == "frpt_stng_f")
         {
            if (_FRPT_STNG_F == null)
               _FRPT_STNG_F = new Ui.FRPT_STNG_F { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4FRPT_STNG_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "frpt_stng_f"},
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 07 /* Execute LoadData */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 03 /* Execute Paint */)
               }
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4AFRM_STNG_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "frpt_stng_f"},
                  new Job(SendType.SelfToUserInterface, "FRPT_STNG_F", 13 /* Execute LoadAppForm */){WhereIsInputData = WhereIsInputDataType.StepBack}
                  
               }
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

   }
}
