using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.Reporting.ReportProfiler.Code
{
   partial class ReportProfiler
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();

         if (value == "specifyreportprofile")
         {
            if (_SpecifyReportProfile == null)
               _SpecifyReportProfile = new Ui.SpecifyReportProfile { _DefaultGateway = this };
         }
         else if (value == "specifyprofilergroupheader")
         {
            if (_SpecifyProfilerGroupHeader == null)
               _SpecifyProfilerGroupHeader = new Ui.SpecifyProfilerGroupHeader { _DefaultGateway = this };
         }
         else if (value == "specifyreportgroupheader")
         {
            if (_SpecifyReportGroupHeader == null)
               _SpecifyReportGroupHeader = new Ui.SpecifyReportGroupHeader { _DefaultGateway = this };
         }
         else if (value == "specifygroupitems")
         {
            if (_SpecifyGroupItems == null)
               _SpecifyGroupItems = new Ui.SpecifyGroupItems { _DefaultGateway = this };
         }
         else if (value == "specifyfilter")
         {
            if (_SpecifyFilter == null)
               _SpecifyFilter = new Ui.SpecifyFilter { _DefaultGateway = this };
         }
         else if (value == "profilertemplate")
         {
            if (_ProfilerTemplate == null)
               _ProfilerTemplate = new Ui.ProfilerTemplate { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SpecifyReportProfiler(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "SpecifyReportProfile"},
                  new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 03 /* Execute Paint */)
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
      private void DoWork4SpecifyProfilerGroupHeader(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "SpecifyProfilerGroupHeader"},
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SpecifyProfilerGroupHeader", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SpecifyReportGroupHeader(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "SpecifyReportGroupHeader"},
                  //new Job(SendType.SelfToUserInterface, "SpecifyReportGroupHeader", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SpecifyReportGroupHeader", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SpecifyReportGroupHeader", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SpecifyGroupItems(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "specifygroupitems"},
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SpecifyGroupItems", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SpecifyFilter(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "specifyfilter"},
                  //new Job(SendType.SelfToUserInterface, "SpecifyFilter", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},                  
                  new Job(SendType.SelfToUserInterface, "SpecifyFilter", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "SpecifyFilter", 07 /* Execute LoadData */){WhereIsInputData = WhereIsInputDataType.StepBack},
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ProfilerTemplate(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "profilertemplate"},
                  new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "ProfilerTemplate", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
