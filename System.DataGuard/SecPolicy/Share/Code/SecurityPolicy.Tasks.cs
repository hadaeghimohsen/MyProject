using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.Share.Code
{
   partial class SecurityPolicy
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = (job.Input as string).ToLower();
         if (value == "securitysettings")
         {
            if (_SecuritySettings == null)
               _SecuritySettings = new Ui.SecuritySettings { _DefaultGateway = this };
            job.Output = _SecuritySettings;
         }
         else if (value == "controlpanel")
         {
            if (_ControlPanel == null)
               _ControlPanel = new Ui.ControlPanel { _DefaultGateway = this };
            job.Output = _ControlPanel;
         }
         else if (value == "securitymanagment")
         {
            if (_SecurityManagment == null)
               _SecurityManagment = new Ui.SecurityManagment { _DefaultGateway = this };
            job.Output = _SecurityManagment;
         }
         else if (value == "softversioncontrol")
         {
            if (_SoftVersionControl == null)
               _SoftVersionControl = new Ui.SoftVersionControl { _DefaultGateway = this };
            job.Output = _SoftVersionControl;
         }
         else if (value == "activecycldata")
         {
            if (_ActiveCyclData == null)
               _ActiveCyclData = new Ui.ActiveCyclData { _DefaultGateway = this };
            job.Output = _ActiveCyclData;
         }
         else if (value == "actioncenter")
         {
            if (_ActionCenter == null)
               _ActionCenter = new Ui.ActionCenter { _DefaultGateway = this };
            job.Output = _ActionCenter;
         }
         else if (value == "actionnotification")
         {
            if (_ActionNotification == null)
               _ActionNotification = new Ui.ActionNotification { _DefaultGateway = this };
            job.Output = _ActionNotification;
         }
         else if (value == "settings")
         {
            if (_Settings == null)
               _Settings = new Ui.Settings { _DefaultGateway = this };
            job.Output = _Settings;
         }
         else if (value == "settingssystem")
         {
            if (_SettingsSystem == null)
               _SettingsSystem = new Ui.SettingsSystem { _DefaultGateway = this };
            job.Output = _SettingsSystem;
         }
         else if (value == "settingsdevice")
         {
            if (_SettingsDevice == null)
               _SettingsDevice = new Ui.SettingsDevice { _DefaultGateway = this };
            job.Output = _SettingsDevice;
         }
         else if (value == "settingsnetworkconnection")
         {
            if (_SettingsNetworkConnection == null)
               _SettingsNetworkConnection = new Ui.SettingsNetworkConnection { _DefaultGateway = this };
            job.Output = _SettingsNetworkConnection;
         }
         else if (value == "settingspersonalization")
         {
            if (_SettingsPersonalization == null)
               _SettingsPersonalization = new Ui.SettingsPersonalization { _DefaultGateway = this };
            job.Output = _SettingsPersonalization;
         }
         else if (value == "settingsaccount")
         {
            if (_SettingsAccount == null)
               _SettingsAccount = new Ui.SettingsAccount { _DefaultGateway = this };
            job.Output = _SettingsAccount;
         }
         else if (value == "settingsservicesapp")
         {
            if (_SettingsServicesApp == null)
               _SettingsServicesApp = new Ui.SettingsServicesApp { _DefaultGateway = this };
            job.Output = _SettingsServicesApp;
         }
         else if (value == "settingsprivacy")
         {
            if (_SettingsPrivacy == null)
               _SettingsPrivacy = new Ui.SettingsPrivacy { _DefaultGateway = this };
            job.Output = _SettingsPrivacy;
         }
         else if (value == "settingsupdatesecurity")
         {
            if (_SettingsUpdateSecurity == null)
               _SettingsUpdateSecurity = new Ui.SettingsUpdateSecurity { _DefaultGateway = this };
            job.Output = _SettingsUpdateSecurity;
         }
         else if (value == "settingssystempackage")
         {
            if (_SettingsSystemPackage == null)
               _SettingsSystemPackage = new Ui.SettingsSystemPackage { _DefaultGateway = this };
            job.Output = _SettingsUpdateSecurity;
         }
         else if (value == "settingspersonalizationdockstartmenuitem")
         {
            if (_SettingsPersonalizationDockStartMenuItem == null)
               _SettingsPersonalizationDockStartMenuItem = new Ui.SettingsPersonalizationDockStartMenuItem { _DefaultGateway = this };
            job.Output = _SettingsPersonalizationDockStartMenuItem;
         }
         else if (value == "settingspersonalizationsystemtryitem")
         {
            if (_SettingsPersonalizationSystemTryItem == null)
               _SettingsPersonalizationSystemTryItem = new Ui.SettingsPersonalizationSystemTryItem { _DefaultGateway = this };
            job.Output = _SettingsPersonalizationSystemTryItem;
         }
         else if (value == "settingsaccountcamera")
         {
            if (_SettingsAccountCamera == null)
               _SettingsAccountCamera = new Ui.SettingsAccountCamera { _DefaultGateway = this };
            job.Output = _SettingsAccountCamera;
         }
         else if (value == "settingsaccountgallery")
         {
            if (_SettingsAccountGallery == null)
               _SettingsAccountGallery = new Ui.SettingsAccountGallery { _DefaultGateway = this };
            job.Output = _SettingsAccountGallery;
         }
         else if (value == "settingsaccountchangepassword")
         {
            if (_SettingsAccountChangePassword == null)
               _SettingsAccountChangePassword = new Ui.SettingsAccountChangePassword { _DefaultGateway = this };
            job.Output = _SettingsAccountChangePassword;
         }
         else if (value == "settingsaccountlocalsecuritypolicy")
         {
            if (_SettingsAccountLocalSecurityPolicy == null)
               _SettingsAccountLocalSecurityPolicy = new Ui.SettingsAccountLocalSecurityPolicy { _DefaultGateway = this };
            job.Output = _SettingsAccountLocalSecurityPolicy;
         }
         else if (value == "settingsotheraccounts")
         {
            if (_SettingsOtherAccounts == null)
               _SettingsOtherAccounts = new Ui.SettingsOtherAccounts { _DefaultGateway = this };
            job.Output = _SettingsOtherAccounts;
         }
         else if (value == "settingsnewaccount")
         {
            if (_SettingsNewAccount == null)
               _SettingsNewAccount = new Ui.SettingsNewAccount { _DefaultGateway = this };
            job.Output = _SettingsOtherAccounts;
         }
         else if (value == "settingsotheraccount")
         {
            if (_SettingsOtherAccount == null)
               _SettingsOtherAccount = new Ui.SettingsOtherAccount { _DefaultGateway = this };
            job.Output = _SettingsOtherAccount;
         }
         else if (value == "settingsduplicateaccount")
         {
            if (_SettingsDuplicateAccount == null)
               _SettingsDuplicateAccount = new Ui.SettingsDuplicateAccount { _DefaultGateway = this };
            job.Output = _SettingsDuplicateAccount;
         }
         else if (value == "settingsnetworkconnectioninfo")
         {
            if (_SettingsNetworkConnectionInfo == null)
               _SettingsNetworkConnectionInfo = new Ui.SettingsNetworkConnectionInfo { _DefaultGateway = this };
            job.Output = _SettingsNetworkConnectionInfo;
         }
         else if (value == "settingschangenetwork")
         {
            if (_SettingsChangeNetwork == null)
               _SettingsChangeNetwork = new Ui.SettingsChangeNetwork { _DefaultGateway = this };
            job.Output = _SettingsChangeNetwork;
         }
         else if (value == "settingsauthorisegateway")
         {
            if (_SettingsAuthoriseGateway == null)
               _SettingsAuthoriseGateway = new Ui.SettingsAuthoriseGateway { _DefaultGateway = this };
            job.Output = _SettingsAuthoriseGateway;
         }
         else if (value == "settingsblockgateway")
         {
            if (_SettingsBlockGateway == null)
               _SettingsBlockGateway = new Ui.SettingsBlockGateway { _DefaultGateway = this };
            job.Output = _SettingsBlockGateway;
         }
         else if (value == "settingsunauthorisegateway")
         {
            if (_SettingsUnAuthoriseGateway == null)
               _SettingsUnAuthoriseGateway = new Ui.SettingsUnAuthoriseGateway { _DefaultGateway = this };
            job.Output = _SettingsUnAuthoriseGateway;
         }
         else if (value == "settingsmailserver")
         {
            if (_SettingsMailServer == null)
               _SettingsMailServer = new Ui.SettingsMailServer { _DefaultGateway = this };
            job.Output = _SettingsMailServer;
         }
         else if (value == "settingsaccountsetemailaddress")
         {
            if (_SettingsAccountSetEmailAddress == null)
               _SettingsAccountSetEmailAddress = new Ui.SettingsAccountSetEmailAddress { _DefaultGateway = this };
            job.Output = _SettingsAccountSetEmailAddress;
         }
         else if (value == "settingssendemail")
         {
            if (_SettingsSendEmail == null)
               _SettingsSendEmail = new Ui.SettingsSendEmail { _DefaultGateway = this };
            job.Output = _SettingsSendEmail;
         }
         else if (value == "settingsnewpos")
         {
            if (_SettingsNewPos == null)
               _SettingsNewPos = new Ui.SettingsNewPos { _DefaultGateway = this };
            job.Output = _SettingsNewPos;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SecuritySettings(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "securitysettings"},
                  new Job(SendType.SelfToUserInterface, "SecuritySettings", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SecuritySettings", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "SecuritySettings", 07 /* Execute LoadData */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ControlPanel(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "controlpanel"},
                  new Job(SendType.SelfToUserInterface, "ControlPanel", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ControlPanel", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ControlPanel", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "ControlPanel", 07 /* Execute LoadData */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SecurityManagment(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "securitymanagment"},
                  new Job(SendType.SelfToUserInterface, "SecurityManagment", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SecurityManagment", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SecurityManagment", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SecurityManagment", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SoftVersionControl(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "softversioncontrol"},
                  new Job(SendType.SelfToUserInterface, "SoftVersionControl", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SoftVersionControl", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SoftVersionControl", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SoftVersionControl", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ActiveCyclData(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "activecycldata"},
                  new Job(SendType.SelfToUserInterface, "ActiveCyclData", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "ActiveCyclData", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ActiveCyclData", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "ActiveCyclData", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ActionCenter(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "actioncenter"},
                  new Job(SendType.SelfToUserInterface, "ActionCenter", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "ActionCenter", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ActionCenter", 07 /* Execute LoadData */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ActionCenter", 03 /* Execute Paint */)

               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ActionNotification(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "actionnotidication"},
                  new Job(SendType.SelfToUserInterface, "ActionNotification", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "ActionCenter", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ActionNotification", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "ActionNotification", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Settings(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settings"},
                  new Job(SendType.SelfToUserInterface, "Settings", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "Settings", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "Settings", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "Settings", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsSystem(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingssystem"},
                  new Job(SendType.SelfToUserInterface, "SettingsSystem", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsSystem", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystem", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystem", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsDevice(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsdevice"},
                  new Job(SendType.SelfToUserInterface, "SettingsDevice", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsDevice", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsDevice", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsDevice", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsNetworkConnection(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsnetworkconnection"},
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsPersonaliaztion(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingspersonalization"},
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAccount(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsaccount"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccount", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsAccount", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccount", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccount", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsServiceApp(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsservicesapp"},
                  new Job(SendType.SelfToUserInterface, "SettingsServicesApp", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsServicesApp", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsServicesApp", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsServicesApp", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsPrivacy(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsprivacy"},
                  new Job(SendType.SelfToUserInterface, "SettingsPrivacy", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsPrivacy", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsPrivacy", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsPrivacy", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsUpdateSecurity(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsupdatesecurity"},
                  new Job(SendType.SelfToUserInterface, "SettingsUpdateSecurity", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "SettingsUpdateSecurity", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsUpdateSecurity", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsUpdateSecurity", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 18
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsSystemPackage(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingssystempackage"},
                  new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 19
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsPersonalizationDockStartMenuItem(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingspersonalizationdockstartmenuitem"},
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationDockStartMenuItem", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationDockStartMenuItem", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationDockStartMenuItem", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsPersonalizationSystemTryItem(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingspersonalizationsystemtryitem"},
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationSystemTryItem", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationSystemTryItem", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalizationSystemTryItem", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAccountCamera(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsaccountcamera"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccountCamera", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountCamera", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountCamera", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 22
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAccountGallery(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsaccountgallery"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccountGallery", 02 /* Execute Set */){Input = job.Input},                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountGallery", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountGallery", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 23
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAccountChangePassword(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsaccountchangepassword"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccountChangePassword", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountChangePassword", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountChangePassword", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 24
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAccountLocalSecurityPolicy(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsaccountlocalsecuritypolicy"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccountLocalSecurityPolicy", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountLocalSecurityPolicy", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountLocalSecurityPolicy", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 25
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsOtherAccounts(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsotheraccounts"},
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccounts", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 26
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsNewAccount(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsnewaccount"},
                  new Job(SendType.SelfToUserInterface, "SettingsNewAccount", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsNewAccount", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsNewAccount", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 27
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsOtherAccount(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsotheraccount"},
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccount", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccount", 07 /* Execute LoadData */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "SettingsOtherAccount", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 28
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsDuplicateAccount(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsduplicateaccount"},
                  new Job(SendType.SelfToUserInterface, "SettingsDuplicateAccount", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsDuplicateAccount", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsDuplicateAccount", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 29
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsNetworkConnectionInfo(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsnetworkconnectioninfo"},
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnectionInfo", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnectionInfo", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnectionInfo", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 30
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsChangeNetwork(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingschangenetwork"},
                  new Job(SendType.SelfToUserInterface, "SettingsChangeNetwork", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsChangeNetwork", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsChangeNetwork", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 31
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAuthoriseGateway(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsauthorisegateway"},
                  new Job(SendType.SelfToUserInterface, "SettingsAuthoriseGateway", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAuthoriseGateway", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAuthoriseGateway", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 32
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsBlockGateway(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsblockgateway"},
                  new Job(SendType.SelfToUserInterface, "SettingsBlockGateway", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsBlockGateway", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsBlockGateway", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 33
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsUnAuthoriseGateway(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsunauthorisegateway"},
                  new Job(SendType.SelfToUserInterface, "SettingsUnAuthoriseGateway", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsUnAuthoriseGateway", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsUnAuthoriseGateway", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 34
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsMailServer(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsmailserver"},
                  new Job(SendType.SelfToUserInterface, "SettingsMailServer", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsMailServer", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsMailServer", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 35
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsAccountSetEmailAddress(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsaccountsetemailaddress"},
                  new Job(SendType.SelfToUserInterface, "SettingsAccountSetEmailAddress", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountSetEmailAddress", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccountSetEmailAddress", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 36
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsSendEmail(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingssendemail"},
                  new Job(SendType.SelfToUserInterface, "SettingsSendEmail", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsSendEmail", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsSendEmail", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 37
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SettingsNewPos(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "settingsnewpos"},
                  new Job(SendType.SelfToUserInterface, "SettingsNewPos", 02 /* Execute Set */),                  
                  //new Job(SendType.SelfToUserInterface, "SettingsSystemPackage", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsNewPos", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SettingsNewPos", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
