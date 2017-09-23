using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ServiceDefinition.Share.Code
{
   partial class Services
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string input = job.Input as string;
         if (input.ToLower() == "services")
         {
            if (_Services == null)
               _Services = new Ui.Services { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4InteractWithServiceDef(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "Services"},
                  new Job(SendType.SelfToUserInterface, "Services", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "Services", 07 /* Execute LoadData */)
               });
         }
         else
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
