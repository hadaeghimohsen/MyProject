using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.ServiceDefinition.SrvDef.UnderGateways.SrvUnitType.Code
{
   partial class UnitType
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         
         if(value == "sutm")
         {
            if (_ServiceUnitTypeMenus == null)
               _ServiceUnitTypeMenus = new Ui.UnitTypeMenus { _DefaultGateway = this };
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ServiceUnitType(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "sutm"},
                  new Job(SendType.SelfToUserInterface, "SUTM", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SUTM", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SUTM", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
