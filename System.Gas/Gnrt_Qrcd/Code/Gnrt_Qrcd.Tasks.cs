using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.Gas.Gnrt_Qrcd.Code
{
   partial class Gnrt_Qrcd
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "rprt_pblc_f")
         {
            if (_RPRT_PBLC_F == null)
               _RPRT_PBLC_F = new Ui.RPRT_PBLC_F { _DefaultGateway = this };
         }
         else if (value == "rprt_prvt_f")
         {
            if (_RPRT_PRVT_F == null)
               _RPRT_PRVT_F = new Ui.RPRT_PRVT_F { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RPRT_PBLC_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rprt_pblc_f"},
                  new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 07 /* Execute LoadData */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 03 /* Execute Paint */)
               });
         }
         else if(job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RPRT_PRVT_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rprt_prvt_f"},
                  new Job(SendType.SelfToUserInterface, "RPRT_PRVT_F", 07 /* Execute LoadData */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "RPRT_PRVT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
