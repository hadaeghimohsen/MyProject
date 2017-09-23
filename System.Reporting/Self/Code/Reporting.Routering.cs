using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.Reporting.Self.Code
{
   partial class Reporting : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "Program":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            case "Datasource":
               _Datasource.Gateway(jobs);
               break;
            case "ReportUnitType":
               _ReportUnitType.Gateway(jobs);
               break;
            case "ReportProfiler":
               _ReportProfiler.Gateway(jobs);
               break;
            case "ReportViewers":
               _ReportViewers.Gateway(jobs);
               break;
            case "WorkFlow":
               _WorkFlow.Gateway(jobs);
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
               DoWork4InteractWithReport(job);
               break;
            case 03:
               DoWork4InteractWithSettingsMetro(job);
               break;
            default:
               job.Status = StatusType.Failed;
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
            case "ReportCtrl":
               _ReportCtrl.SendRequest(job);
               break;
            case "SettingsMetro":
               _SettingsMetro.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
