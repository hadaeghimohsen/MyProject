using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace MyProject.Commons.LifeTime.Code
{
   partial class LifeTime
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input as string;

         if (value.ToLower() == "tooloperation")
         {
            if (_ToolOperation == null)
               _ToolOperation = new Ui.ToolOperation { _DefaultGateway = this };
            job.Output = _ToolOperation;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ToolOperation(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            Func<string, int> GetFuncID = (c) =>
               {
                  switch (c.ToLower())
                  {
                     case "commons.removeobject":
                        return 02;
                     case "commons.restoreremovedobject":
                        return 03;
                     case "commons.deactiveobject":
                        return 04;
                     case "commons.restoredeactiveobject":
                        return 05;
                     default:
                        return 01;                        
                  }
               };

            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new []
               {
                  new Job(SendType.SelfToUserInterface, "ToolOperation", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ToolOperation", 07 /* Execute LoadData */){Input = GetFuncID((job.Input as List<object>)[1].ToString())},
                  new Job(SendType.SelfToUserInterface, "ToolOperation", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

   }
}
