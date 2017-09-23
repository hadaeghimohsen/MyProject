using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.User.Code
{
   partial class User : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "SecurityPolicy":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
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
               DoWork4CreateNew(job);
               break;
            case 03:
               DoWork4UserPropertyMenu(job);
               break;
            case 04:
               Dowork4Profile(job);
               break;
            case 05:
               DoWork4NotExists(job);
               break;
            case 06:
               DoWork4NewUserPropertyMenu(job);
               break;
            case 07:
               DoWork4Duplicate(job);
               break;
            case 08:
               DoWork4CurrentUserInfo(job);
               break;
            case 09:
               DoWork4CurrentChangeUserName(job);
               break;
            case 10:
               DoWork4CurrentChangePassword(job);
               break;
            case 11:
               DoWork4CurrentChangeUserType(job);
               break;
            case 12:
               DoWork4OtherUsers(job);
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
            case "CreateNew":
               _CreateNew.SendRequest(job);
               break;
            case "PropertySingleMenu":
               _PropertySingleMenu.SendRequest(job);
               break;
            case "Duplicate":
               _Duplicate.SendRequest(job);
               break;
            case "Profile":
               _Profile.SendRequest(job);
               break;
            case "CurrentUserInfo":
               _CurrentUserInfo.SendRequest(job);
               break;
            case "CurrentChangeUserName":
               _CurrentChangeUserName.SendRequest(job);
               break;
            case "CurrentChangePassword":
               _CurrentChangePassword.SendRequest(job);
               break;
            case "CurrentChangeUserType":
               _CurrentChangeUserType.SendRequest(job);
               break;
            case "OtherUsers":
               _OtherUsers.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
