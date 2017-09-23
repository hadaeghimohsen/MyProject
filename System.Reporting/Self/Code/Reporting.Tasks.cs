using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.Reporting.Self.Code
{
   partial class Reporting
   {

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if(value == "reportctrl")
         {
            if (_ReportCtrl == null)
               _ReportCtrl = new Ui.ReportCtrl { _DefaultGateway = this };
         }
         else if (value == "settingsmetro")
         {
            if (_SettingsMetro == null)
               _SettingsMetro = new Ui.SettingsMetro { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4InteractWithReport(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ReportCtrl"},
                  new Job(SendType.SelfToUserInterface, "ReportCtrl", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ReportCtrl", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "ReportCtrl", 03 /* Execute Paint */)
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
      private void DoWork4InteractWithSettingsMetro(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "SettingsMetro"},
                  new Job(SendType.SelfToUserInterface, "SettingsMetro", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
