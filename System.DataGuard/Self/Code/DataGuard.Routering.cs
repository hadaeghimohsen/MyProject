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
            case 11:
               DoWork4GetServer(job);
               break;
            case 12:
               DoWork4GetProcessorId(job);
               break;
            case 13:
               DoWork4GetHDDSerialNo(job);
               break;
            case 14:
               DoWork4GetMACAddress(job);
               break;
            case 15:
               DoWork4GetCdRomDrive(job);
               break;
            case 16:
               DoWork4GetBIOSmaker(job);
               break;
            case 17:
               DoWork4GetBIOSserNo(job);
               break;
            case 18:
               DoWork4GetBIOScaption(job);
               break;
            case 19:
               DoWork4GetAccountName(job);
               break;
            case 20:
               DoWork4GetPhysicalMemory(job);
               break;
            case 21:
               DoWork4GetNoRamSlots(job);
               break;
            case 22:
               DoWork4GetCPUManufacturer(job);
               break;
            case 23:
               DoWork4GetCPUCurrentClockSpeed(job);
               break;
            case 24:
               DoWork4GetDefaultIPGateway(job);
               break;
            case 25:
               DoWork4GetBoardMaker(job);
               break;
            case 26:
               DoWork4GetBoardProductId(job);
               break;
            case 27:
               DoWork4GetCpuSpeedInGHz(job);
               break;
            case 28:
               DoWork4GetCurrentLanguage(job);
               break;
            case 29:
               DoWork4GetOSInformation(job);
               break;
            case 30:
               DoWork4GetProcessorInformation(job);
               break;
            case 31:
               DoWork4GetComputerName(job);
               break;
            case 32:
               DoWork4CheckInstallTinyLock(job);
               break;
            case 33:
               DoWork4GetLicenseDay(job);
               break;
            case 34:
               DoWork4Backup(job);
               break;
            case 35:
               DoWork4PinCode(job);
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
