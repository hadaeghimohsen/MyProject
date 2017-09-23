using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.Reporting.ReportUnitType.Code
{
   partial class ReportUnitType
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "specifyreportfile")
         {
            if (_SpecifyReportFile == null)
               _SpecifyReportFile = new Ui.SpecifyReportFile { _DefaultGateway = this };            
         }
         else if (value == "specifyappdecision")
         {
            if (_SpecifyAppDecision == null)
               _SpecifyAppDecision = new Ui.SpecifyAppDecision { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ReportUnitType(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "specifyreportfile"},
                  //new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportFile" , 03 /* Execute Paint */)
               });
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
      private void DoWork4SpecifyAppDecision(Job job)
      {
          if (job.Status == StatusType.Running)
          {
              job.Status = StatusType.WaitForPreconditions;
              job.OwnerDefineWorkWith.AddRange(
                 new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "specifyappdecision"},
                  //new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SpecifyAppDecision", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SpecifyAppDecision", 03 /* Execute Paint */)
               });
          }
          else if (job.Status == StatusType.SignalForPreconditions)
          {
              job.Status = StatusType.Successful;
          }
      }

   }
}
