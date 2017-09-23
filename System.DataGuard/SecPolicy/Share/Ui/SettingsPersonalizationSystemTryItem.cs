using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsPersonalizationSystemTryItem : UserControl
   {
      public SettingsPersonalizationSystemTryItem()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         PackageUserGatewayBs.DataSource = iProject.Package_Instance_User_Gateways.Where(piug => piug == Piug); 
      }

      private void PackageUserGatewayBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
            if (personalization == null) return;

            personalization.STRY_DTIM_BUTN = personalization.STRY_DTIM_BUTN == null ? "002" : personalization.STRY_DTIM_BUTN;
            personalization.STRY_NOTF_MESG_BUTN = personalization.STRY_NOTF_MESG_BUTN == null ? "001" : personalization.STRT_NOTF_MESG_BUTN;
            personalization.STRY_NETW_BUTN = personalization.STRY_NETW_BUTN == null ? "002" : personalization.STRY_NETW_BUTN;
            personalization.STRY_BKRS_BUTN = personalization.STRY_BKRS_BUTN == null ? "001" : personalization.STRY_BKRS_BUTN;

            switch (personalization.STRY_DTIM_BUTN)
            {
               case "001":
                  Ts_SystemTryDateTimeButn.IsOn = false;
                  break;
               case "002":
                  Ts_SystemTryDateTimeButn.IsOn = true;
                  break;
            }

            switch (personalization.STRY_NOTF_MESG_BUTN)
            {
               case "001":
                  Ts_SystemTryMessageButn.IsOn = false;
                  break;
               case "002":
                  Ts_SystemTryMessageButn.IsOn = true;
                  break;
            }

            switch (personalization.STRY_NETW_BUTN)
            {
               case "001":
                  Ts_SystemTryNetworkButn.IsOn = false;
                  break;
               case "002":
                  Ts_SystemTryNetworkButn.IsOn = true;
                  break;
            }

            switch (personalization.STRY_BKRS_BUTN)
            {
               case "001":
                  Ts_SystemTryBackupRestoreButn.IsOn = false;
                  break;
               case "002":
                  Ts_SystemTryBackupRestoreButn.IsOn = true;
                  break;
            }
         }
         catch (Exception)
         {
            throw;
         }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PackageUserGatewayBs.EndEdit();
            iProject.SubmitChanges();
            SubmitChange_Butn.Visible = false;
         }
         catch { }
      }

      private void PackageUserGatewayBs_ListChanged(object sender, ListChangedEventArgs e)
      {
         SubmitChange_Butn.Visible = true;
      }

      private void Ts_SystemTryButn_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         switch (ts.Tag.ToString())
         {
            case "001":
               personalization.STRY_DTIM_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "002":
               personalization.STRY_NOTF_MESG_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "003":
               personalization.STRY_NETW_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "004":
               personalization.STRY_BKRS_BUTN = ts.IsOn ? "002" : "001";
               break;
         }
      }
   }
}
