using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.Role.Code
{
   partial class Role : IMP
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
               DoWork4CreateNewRole(job);
               break;
            case 03:
               DoWork4LifeTimeRole(job);
               break;
            case 04:
               DoWork4RolePropertMenu(job);
               break;
            case 05:
               DoWork4RoleChangingName(job);
               break;
            case 06:
               DoWork4DuplicateRole(job);
               break;
            case 07:
               DoWork4NotExistsRole(job);
               break;
            case 08:
               DoWork4RoleSettings4CurrentUser(job);
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
            case "CreateNewRole":
               _CreateNewRole.SendRequest(job);
               break;
            case "PropertySingleRoleMenu":
               _PropertySingleRoleMenu.SendRequest(job);
               break;
            case "DuplicateRole":
               _DuplicateRole.SendRequest(job);
               break;
            case "JLCU2R":
               _JoinOrLeaveCurrentUserToRoles.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
