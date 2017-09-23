using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.DataGuard.Login.Code
{
   partial class Login : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DefaultGateway":
            case "DataGuard":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            default:
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
               DoWork4Login(job);
               break;
            case 03:
               DoWork4SaveHostInfo(job);
               break;
            case 04:
               DoWork4LockScreen(job);
               break;
            case 05:
               DoWork4LastUserLogin(job);
               break;
            case 06:
               DoWork4SelectedLastUserLogin(job);
               break;
            default:
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
            case "Login":
               _Login.SendRequest(job);
               break;
            case "LockScreen":
               _LockScreen.SendRequest(job);
               break;
            case "LastUserLogin":
               _LastUserLogin.SendRequest(job);
               break;
            case "SelectedLastUserLogin":
               _SelectedLastUserLogin.SendRequest(job);
               break;
            default:
               break;
         }
      }
   }
}
