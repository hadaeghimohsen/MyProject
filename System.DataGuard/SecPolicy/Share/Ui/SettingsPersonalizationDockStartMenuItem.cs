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
   public partial class SettingsPersonalizationDockStartMenuItem : UserControl
   {
      public SettingsPersonalizationDockStartMenuItem()
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

            personalization.STRT_EXIT_BUTN = personalization.STRT_EXIT_BUTN == null ? "002" : personalization.STRT_EXIT_BUTN;
            personalization.STRT_STNG_BUTN = personalization.STRT_STNG_BUTN == null ? "002" : personalization.STRT_STNG_BUTN;
            personalization.STRT_ACTV_USER_BUTN = personalization.STRT_ACTV_USER_BUTN == null ? "001" : personalization.STRT_ACTV_USER_BUTN;
            personalization.STRT_CNTC_BUTN = personalization.STRT_CNTC_BUTN == null ? "001" : personalization.STRT_CNTC_BUTN;
            personalization.STRT_NOTF_MESG_BUTN = personalization.STRT_NOTF_MESG_BUTN == null ? "001" : personalization.STRT_NOTF_MESG_BUTN;
            personalization.STRT_PROF_USER_BUTN = personalization.STRT_PROF_USER_BUTN == null ? "002" : personalization.STRT_PROF_USER_BUTN;
            personalization.STRT_WETR_BUTN = personalization.STRT_WETR_BUTN == null ? "001" : personalization.STRT_WETR_BUTN;

            switch (personalization.STRT_EXIT_BUTN)
            {
               case "001":
                  Ts_StartExitButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartExitButn.IsOn = true;
                  break;
            }

            switch (personalization.STRT_STNG_BUTN)
            {
               case "001":
                  Ts_StartSettingButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartSettingButn.IsOn = true;
                  break;
            }

            switch (personalization.STRT_ACTV_USER_BUTN)
            {
               case "001":
                  Ts_StartActiveUsersButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartActiveUsersButn.IsOn = true;
                  break;
            }

            switch (personalization.STRT_CNTC_BUTN)
            {
               case "001":
                  Ts_StartContactButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartContactButn.IsOn = true;
                  break;
            }

            switch (personalization.STRT_NOTF_MESG_BUTN)
            {
               case "001":
                  Ts_StartMessageButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartMessageButn.IsOn = true;
                  break;
            }

            switch (personalization.STRT_PROF_USER_BUTN)
            {
               case "001":
                  Ts_StartUserProfileButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartUserProfileButn.IsOn = true;
                  break;
            }

            switch (personalization.STRT_WETR_BUTN)
            {
               case "001":
                  Ts_StartWheaterButn.IsOn = false;
                  break;
               case "002":
                  Ts_StartWheaterButn.IsOn = true;
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

      private void Ts_StartButn_Toggled(object sender, EventArgs e)
      {
         ToggleSwitch ts = (ToggleSwitch)sender;

         var personalization = PackageUserGatewayBs.Current as Data.Package_Instance_User_Gateway;
         if (personalization == null) return;

         switch (ts.Tag.ToString())
         {
            case "001":
               personalization.STRT_EXIT_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "002":
               personalization.STRT_STNG_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "003":
               personalization.STRT_ACTV_USER_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "004":
               personalization.STRT_CNTC_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "005":
               personalization.STRT_NOTF_MESG_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "006":
               personalization.STRT_PROF_USER_BUTN = ts.IsOn ? "002" : "001";
               break;
            case "007":
               personalization.STRT_WETR_BUTN = ts.IsOn ? "002" : "001";
               break;
         }

      }

   }
}
