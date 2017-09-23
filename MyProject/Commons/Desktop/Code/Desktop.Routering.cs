using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace MyProject.Commons.Desktop.Code
{
   partial class Desktop : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "Commons":
               _DefaultGateway.Gateway(jobs);
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
               DoWork4Desktop(job);
               break;
            case 03:
               DoWork4StartDrawer(job);
               break;
            case 04:
               DoWork4SettingsDrawer(job);
               break;
            case 05:
               DoWork4StartMenu(job);
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
            case "Desktop":
               _Desktop.SendRequest(job);
               break;
            case "StartDrawer":
               _StartDrawer.SendRequest(job);
               break;
            case "SettingsDrawer":
               _SettingsDrawer.SendRequest(job);
               break;
            case "StartMenu":
               _StartMenu.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
