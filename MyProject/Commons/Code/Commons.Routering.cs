using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;

namespace MyProject.Commons.Code
{
   partial class Commons : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "Program":
               _DefaultGateway.Gateway(jobs);
               break;
            case "ChangeHandle":
               _ChangeHandle.Gateway(jobs);
               break;
            case "LifeTime":
               _LifeTime.Gateway(jobs);
               break;
            case "ErrorHandle":
               _ErrorHandle.Gateway(jobs);
               break;
            case "HelpHandle":
               _HelpHandle.Gateway(jobs);
               break;
            case "RedoLog":
               _RedoLog.Gateway(jobs);
               break;
            case "Odbc":
               _Odbc.Gateway(jobs);
               break;
            case "Desktop":
               _Desktop.Gateway(jobs);
               break;
            case "FORM_STNG":
               _FORM_STNG.Gateway(jobs);
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
               DoWork4LifeTimeObject(job);
               break;
            case 02:
               DoWork4ErrorHandling(job);
               break;
            case 03:
               DoWork4HelpHandling(job);
               break;
            case 04:
               DoWork4Odbc(job);
               break;
            case 05:
               DoWork4Desktop(job);
               break;
            case 06:
               DoWork4RedoLog(job);
               break;
            case 07:
               DoWork4AccessPrivilege(job);
               break;
            case 08:
               LangChangToFarsi(job);
               break;
            case 09:
               LangChangToEnglish(job);
               break;
            case 10 :
               DoWork4ChangeHandling(job);
               break;
            case 11:
               DoWork4ReadAllAccessRoles(job);
               break;
            case 12:
               DoWork4RoleSettings4CurrentUser(job);
               break;
            case 13:
               DoWork4Import2Odbc(job);
               break;
            case 14:
               DoWork4ReadUnitTypeOfRoles(job);
               break;
            case 15:
               DoWork4ReadGroupHeaderOfRoles(job);
               break;
            case 16:
               DoWork4LoadParentServicesOfGroupHeaders(job);
               break;
            case 17:
               DoWork4LoadServicesOfParentService(job);
               break;
            case 18:
               DoWork4GroupHeaderSettings4CurrentUser(job);
               break;
            case 19:
               DoWork4ServiceUnitType(job);
               break;
            case 20:
               DoWork4GetRegisterOdbcDatasource(job);
               break;
            case 21:
               DoWork4Form_Stng(job);
               break;
            case 22:
               DoWork4GetConnectionString(job);
               break;
            case 23:
               DoWork4AppForm_Stng(job);
               break;
            case 24:
               DoWork4GetHosInfo(job);
               break;
            case 25:
               DoWork4Shutingdown(job);
               break;
            case 26:
               DoWork4DateTimes(job);
               break;
            case 27:
               DoWork4SendMail(job);
               break;
            case 28:
               DoWork4GetUserProfile(job);
               break;
            case 29:
               DoWork4ShowUserProfile(job);
               break;
            case 30:
               DoWork4GetTimePeriod(job);;
               break;
            case 31:
               DoWork4GMapNets(job);
               break;
            case 32:
               DoWork4SendFeedBack(job);
               break;
            case 33:
               DoWork4PosSettings(job);
               break;
            case 34:
               DoWork4PaymentPos(job);
               break;
            case 35:
               DoWork4GetServer(job);
               break;
            case 36:
               DoWork4GetWindowsPlatform(job);
               break;
            case 37:
               DoWork4GetLicenseDay(job);
               break;
            case 38:
               DoWork4PingNetwork(job);
               break;
            case 39:
               DoWork4PinCode(job);
               break;
            case 40:
               DoWork4Encrypt(job);
               break;
            case 41:
               DoWork4Decrypt(job);
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
            case "DateTimes":
               _DateTimes.SendRequest(job);
               break;
            case "Shutdown":
               _Shutdown.SendRequest(job);
               break;
            case "GMapNets":
               _GMapNets.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
