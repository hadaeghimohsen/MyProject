using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataGuard.SecPolicy.Share.Code
{
   partial class SecurityPolicy
   {
      public ISendRequest _Wall { get; set; }

      internal Ui.SecuritySettings _SecuritySettings { get; set; }
      internal Ui.ControlPanel _ControlPanel { get; set; }
      internal Ui.SecurityManagment _SecurityManagment { get; set; }
      internal Ui.SoftVersionControl _SoftVersionControl { get; set; }
      internal Ui.ActiveCyclData _ActiveCyclData { get; set; }
      internal Ui.ActionCenter _ActionCenter { get; set; }
      internal Ui.ActionNotification _ActionNotification { get; set; }
      internal Ui.Settings _Settings { get; set; }
      internal Ui.SettingsSystem _SettingsSystem { get; set; }
      internal Ui.SettingsDevice _SettingsDevice { get; set; }
      internal Ui.SettingsNetworkConnection _SettingsNetworkConnection { get; set; }
      internal Ui.SettingsPersonalization _SettingsPersonalization { get; set; }
      internal Ui.SettingsAccount _SettingsAccount { get; set; }
      internal Ui.SettingsServicesApp _SettingsServicesApp { get; set; }
      internal Ui.SettingsPrivacy _SettingsPrivacy { get; set; }
      internal Ui.SettingsUpdateSecurity _SettingsUpdateSecurity { get; set; }
      internal Ui.SettingsSystemPackage _SettingsSystemPackage { get; set; }
      internal Ui.SettingsPersonalizationDockStartMenuItem _SettingsPersonalizationDockStartMenuItem { get; set; }
      internal Ui.SettingsPersonalizationSystemTryItem _SettingsPersonalizationSystemTryItem { get; set; }
      internal Ui.SettingsAccountCamera _SettingsAccountCamera { get; set; }
      internal Ui.SettingsAccountGallery _SettingsAccountGallery { get; set; }
      internal Ui.SettingsAccountChangePassword _SettingsAccountChangePassword { get; set; }
      internal Ui.SettingsAccountLocalSecurityPolicy _SettingsAccountLocalSecurityPolicy { get; set;}
      internal Ui.SettingsOtherAccounts _SettingsOtherAccounts { get; set; }
      internal Ui.SettingsNewAccount _SettingsNewAccount { get; set; }
      internal Ui.SettingsOtherAccount _SettingsOtherAccount { get; set; }
      internal Ui.SettingsDuplicateAccount _SettingsDuplicateAccount {get; set;}
      internal Ui.SettingsNetworkConnectionInfo _SettingsNetworkConnectionInfo { get; set; }
      internal Ui.SettingsChangeNetwork _SettingsChangeNetwork { get; set; }
      internal Ui.SettingsAuthoriseGateway _SettingsAuthoriseGateway { get; set; }
      internal Ui.SettingsBlockGateway _SettingsBlockGateway { get; set; }
      internal Ui.SettingsUnAuthoriseGateway _SettingsUnAuthoriseGateway { get; set; }
      internal Ui.SettingsMailServer _SettingsMailServer { get; set; }
      internal Ui.SettingsAccountSetEmailAddress _SettingsAccountSetEmailAddress { get; set; }
      internal Ui.SettingsSendEmail _SettingsSendEmail { get; set; }
      internal Ui.SettingsNewPos _SettingsNewPos { get; set; }
      internal Ui.SettingsPaymentPos _SettingsPaymentPos { get; set; }
      internal Ui.SettingsRegion _SettingsRegion { get; set; }
      internal Ui.SettingsSystemLicense _SettingsSystemLicense { get; set; }
      internal Ui.SettingsSystemScript _SettingsSystemScript { get; set; }
      internal Ui.SettingsSystemTinyLock _SettingsSystemTinyLock { get; set; }
   }
}
