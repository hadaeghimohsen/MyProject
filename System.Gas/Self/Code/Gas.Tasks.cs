using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Jobs;

namespace System.Gas.Self.Code
{
   partial class Gas
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "strt_mtro_m")
         {
            if (_STRT_MTRO_M == null)
               _STRT_MTRO_M = new Ui.STRT_MTRO_M { _DefaultGateway = this };
         }
         else if (value == "strt_mbar_m")
         {
            if (_STRT_MBAR_M == null)
               _STRT_MBAR_M = new Ui.STRT_MBAR_M { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Strt_Mrto_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "strt_mtro_m"},
                  new Job(SendType.SelfToUserInterface, "STRT_MTRO_M", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
