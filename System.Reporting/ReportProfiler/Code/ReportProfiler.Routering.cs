using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;

namespace System.Reporting.ReportProfiler.Code
{
   partial class ReportProfiler : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DefaultGateway":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            case "ReportFile":
               _ReportFile.Gateway(jobs);
               break;
            case "ProfilerGroup":
               _ProfilerGroup.Gateway(jobs);
               break;
            default:
               jobs.Status = StatusType.Failed;
               break;
         }
      }

      protected override void InternalAssistance(Job job)
      {
         switch (job.Method)
         {
            case 01:
               GetUi(job);
               break;
            case 02:
               DoWork4SpecifyReportProfiler(job);
               break;
            case 03:
               DoWork4SpecifyProfilerGroupHeader(job);
               break;
            case 04:
               DoWork4SpecifyReportGroupHeader(job);
               break;
            case 05:
               DoWork4SpecifyGroupItems(job);
               break;
            case 06:
               DoWork4SpecifyFilter(job);
               break;
            case 07:
               DoWork4ProfilerTemplate(job);
               break;
            default:
               job.Status = StatusType.Successful;
               break;
         }
      }

      protected override void RequestToUserInterface(Job job)
      {
         switch (job.Gateway)
         {
            case "Wall":
               _Wall.SendRequest(job);
               break;
            case "SpecifyReportProfile":
               _SpecifyReportProfile.SendRequest(job);
               break;
            case "SpecifyProfilerGroupHeader":
               _SpecifyProfilerGroupHeader.SendRequest(job);
               break;
            case "SpecifyReportGroupHeader":
               _SpecifyReportGroupHeader.SendRequest(job);
               break;
            case "SpecifyGroupItems":
               _SpecifyGroupItems.SendRequest(job);
               break;
            case "SpecifyFilter":
               _SpecifyFilter.SendRequest(job);
               break;
            case "ProfilerTemplate":
               _ProfilerTemplate.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
