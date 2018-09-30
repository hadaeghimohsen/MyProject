using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;

namespace System.DataGuard.SecPolicy.Share.Code
{
   partial class SecurityPolicy : IMP
   {
      protected override void ExternalAssistance(Job jobs)
      {
         switch (jobs.Gateway)
         {
            case "DataGuard":
               _DefaultGateway.Gateway(jobs);
               break;
            case "Commons":
               _Commons.Gateway(jobs);
               break;
            case "Role":
               _Role.Gateway(jobs);
               break;
            case "User":
               _User.Gateway(jobs);
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
               DoWork4SecuritySettings(job);
               break;
            case 03:
               DoWork4ControlPanel(job);
               break;
            case 04:
               DoWork4SecurityManagment(job);
               break;
            case 05:
               DoWork4SoftVersionControl(job);
               break;
            case 06:
               DoWork4ActiveCyclData(job);
               break;
            case 07:
               DoWork4ActionCenter(job);
               break;
            case 08:
               DoWork4ActionNotification(job);
               break;
            case 09:
               DoWork4Settings(job);
               break;
            case 10:
               DoWork4SettingsSystem(job);
               break;
            case 11:
               DoWork4SettingsDevice(job);
               break;
            case 12:
               DoWork4SettingsNetworkConnection(job);
               break;
            case 13:
               DoWork4SettingsPersonaliaztion(job);
               break;
            case 14:
               DoWork4SettingsAccount(job);
               break;
            case 15:
               DoWork4SettingsServiceApp(job);
               break;
            case 16:
               DoWork4SettingsPrivacy(job);
               break;
            case 17:
               DoWork4SettingsUpdateSecurity(job);
               break;
            case 18:
               DoWork4SettingsSystemPackage(job);
               break;
            case 19:
               DoWork4SettingsPersonalizationDockStartMenuItem(job);
               break;
            case 20:
               DoWork4SettingsPersonalizationSystemTryItem(job);
               break;
            case 21:
               DoWork4SettingsAccountCamera(job);
               break;
            case 22:
               DoWork4SettingsAccountGallery(job);
               break;
            case 23:
               DoWork4SettingsAccountChangePassword(job);
               break;
            case 24:
               DoWork4SettingsAccountLocalSecurityPolicy(job);
               break;
            case 25:
               DoWork4SettingsOtherAccounts(job);
               break;
            case 26:
               DoWork4SettingsNewAccount(job);
               break;
            case 27:
               DoWork4SettingsOtherAccount(job);
               break;
            case 28:
               DoWork4SettingsDuplicateAccount(job);
               break;
            case 29:
               DoWork4SettingsNetworkConnectionInfo(job);
               break;
            case 30:
               DoWork4SettingsChangeNetwork(job);
               break;
            case 31:
               DoWork4SettingsAuthoriseGateway(job);
               break;
            case 32:
               DoWork4SettingsBlockGateway(job);
               break;
            case 33:
               DoWork4SettingsUnAuthoriseGateway(job);
               break;
            case 34:
               DoWork4SettingsMailServer(job);
               break;
            case 35:
               DoWork4SettingsAccountSetEmailAddress(job);
               break;
            case 36:
               DoWork4SettingsSendEmail(job);
               break;
            case 37:
               DoWork4SettingsNewPos(job);
               break;
            case 38:
               DoWork4SettingsPaymentPos(job);
               break;
            case 39:
               DoWork4SettingsRegion(job);
               break;
            case 40:
               DoWork4SettingsSystemLicense(job);
               break;
            case 41:
               DoWork4SettingsSystemScript(job);
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
            case "SecuritySettings":
               _SecuritySettings.SendRequest(job);
               break;
            case "ControlPanel":
               _ControlPanel.SendRequest(job);
               break;
            case "SecurityManagment":
               _SecurityManagment.SendRequest(job);
               break;
            case "SoftVersionControl":
               _SoftVersionControl.SendRequest(job);
               break;
            case "ActiveCyclData":
               _ActiveCyclData.SendRequest(job);
               break;
            case "ActionCenter":
               _ActionCenter.SendRequest(job);
               break;
            case "ActionNotification":
               _ActionNotification.SendRequest(job);
               break;
            case "Settings":
               _Settings.SendRequest(job);
               break;
            case "SettingsSystem":
               _SettingsSystem.SendRequest(job);
               break;
            case "SettingsDevice":
               _SettingsDevice.SendRequest(job);
               break;
            case "SettingsNetworkConnection":
               _SettingsNetworkConnection.SendRequest(job);
               break;
            case "SettingsPersonalization":
               _SettingsPersonalization.SendRequest(job);
               break;
            case "SettingsAccount":
               _SettingsAccount.SendRequest(job);
               break;
            case "SettingsServicesApp":
               _SettingsServicesApp.SendRequest(job);
               break;
            case "SettingsPrivacy":
               _SettingsPrivacy.SendRequest(job);
               break;
            case "SettingsUpdateSecurity":
               _SettingsUpdateSecurity.SendRequest(job);
               break;
            case "SettingsSystemPackage":
               _SettingsSystemPackage.SendRequest(job);
               break;
            case "SettingsPersonalizationDockStartMenuItem":
               _SettingsPersonalizationDockStartMenuItem.SendRequest(job);
               break;
            case "SettingsPersonalizationSystemTryItem":
               _SettingsPersonalizationSystemTryItem.SendRequest(job);
               break;
            case "SettingsAccountCamera":
               _SettingsAccountCamera.SendRequest(job);
               break;
            case "SettingsAccountGallery":
               _SettingsAccountGallery.SendRequest(job);
               break;
            case "SettingsAccountChangePassword":
               _SettingsAccountChangePassword.SendRequest(job);
               break;
            case "SettingsAccountLocalSecurityPolicy":
               _SettingsAccountLocalSecurityPolicy.SendRequest(job);
               break;
            case "SettingsOtherAccounts":
               _SettingsOtherAccounts.SendRequest(job);
               break;
            case "SettingsNewAccount":
               _SettingsNewAccount.SendRequest(job);
               break;
            case "SettingsOtherAccount":
               _SettingsOtherAccount.SendRequest(job);
               break;
            case "SettingsDuplicateAccount":
               _SettingsDuplicateAccount.SendRequest(job);
               break;
            case "SettingsNetworkConnectionInfo":
               _SettingsNetworkConnectionInfo.SendRequest(job);
               break;
            case "SettingsChangeNetwork":
               _SettingsChangeNetwork.SendRequest(job);
               break;
            case "SettingsAuthoriseGateway":
               _SettingsAuthoriseGateway.SendRequest(job);
               break;
            case "SettingsBlockGateway":
               _SettingsBlockGateway.SendRequest(job);
               break;
            case "SettingsUnAuthoriseGateway":
               _SettingsUnAuthoriseGateway.SendRequest(job);
               break;
            case "SettingsMailServer":
               _SettingsMailServer.SendRequest(job);
               break;
            case "SettingsAccountSetEmailAddress":
               _SettingsAccountSetEmailAddress.SendRequest(job);
               break;
            case "SettingsSendEmail":
               _SettingsSendEmail.SendRequest(job);
               break;
            case "SettingsNewPos":
               _SettingsNewPos.SendRequest(job);
               break;
            case "SettingsPaymentPos":
               _SettingsPaymentPos.SendRequest(job);
               break;
            case "SettingsRegion":
               _SettingsRegion.SendRequest(job);
               break;
            case "SettingsSystemLicense":
               _SettingsSystemLicense.SendRequest(job);
               break;
            case "SettingsSystemScript":
               _SettingsSystemScript.SendRequest(job);
               break;
            default:
               job.Status = StatusType.Failed;
               break;
         }
      }
   }
}
