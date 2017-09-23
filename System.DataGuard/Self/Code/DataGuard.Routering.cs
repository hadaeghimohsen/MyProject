using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.DataGuard.Self.Code
{
   partial class DataGuard : IMP
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
             case "Login":
                _Login.Gateway(jobs);
                break;
             case "SecurityPolicy":
                _SecurityPolicy.Gateway(jobs);
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
               DoWork4SecuritySettings(job);
               break;
            case 04:
               DoWork4GetHosInfo(job);
               break;
            case 05:
               DoWork4TinyLock(job);
               break;
            case 06:
               DoWork4HashCode(job);
               break;
            case 07:
               DoWork4SecureHashCode(job);
               break;
            case 08:
               DoWork4UnSecureHashCode(job);
               break;
            case 09:
               DoWork4LockScreen(job);
               break;
            case 10:
               DoWork4TryLogin(job);
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
            case "Main":
               _Main.SendRequest(job);
               break;
            default:
               break;
         }
      }
   }
}
