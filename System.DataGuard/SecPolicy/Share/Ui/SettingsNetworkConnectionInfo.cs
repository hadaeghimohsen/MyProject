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
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsNetworkConnectionInfo : UserControl
   {
      public SettingsNetworkConnectionInfo()
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
         AccessDatasourceBs.DataSource = iProject.Access_User_Datasources.Where(aud => aud == AccessUserDatasource).ToList();
      }

      private void AccessUserDatasourceBs_CurrentChanged(object sender, EventArgs e)
      {
         var aud = AccessDatasourceBs.Current as Data.Access_User_Datasource;
         if (aud == null) return;

         UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.USER_ID == aud.USER_ID && ug.VALD_TYPE == "002");

         // AccessType
         aud.ACES_TYPE = aud.ACES_TYPE == null ? "001" : aud.ACES_TYPE;
         switch (aud.ACES_TYPE)
         {
            case "001":
               AccessType_Ts.IsOn = true;
               aud.STRT_DATE = aud.END_DATE = null;
               StartDate_Dt.Visible = EndDate_Dt.Visible = false;
               break;
            case "002":
               AccessType_Ts.IsOn = false;
               aud.STRT_DATE = aud.END_DATE = DateTime.Now;
               StartDate_Dt.Visible = EndDate_Dt.Visible = true;
               break;
         }

         // Status
         aud.STAT = aud.STAT == null ? "001" : aud.STAT;
         switch (aud.STAT)
         {
            case "002":
               Status_Ts.IsOn = true;
               break;
            case "001":
               Status_Ts.IsOn = false;
               break;
         }
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aud = AccessDatasourceBs.Current as Data.Access_User_Datasource;
            if (aud == null) return;

            
            iProject.SubmitChanges();

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     //new Job(SendType.Self, 25 /* Execute DoWork4SettingsAccountChangePassword */),                  
                     new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 10 /* Execute ActionCallWindows */){Input = new XElement("TabPage", new XAttribute("showtabpage", "tp_002"))}
                  }
               )
            );

            Back_Butn_Click(null, null);
         }
         catch (Exception)
         {

         }
      }

      private void AccessType_Ts_Toggled(object sender, EventArgs e)
      {
         var aud = AccessDatasourceBs.Current as Data.Access_User_Datasource;
         if (aud == null) return;

         var ts = sender as ToggleSwitch;

         aud.ACES_TYPE = ts.IsOn ? "001" : "002";
         switch (aud.ACES_TYPE)
         {
            case "001":
               AccessType_Ts.IsOn = true;
               aud.STRT_DATE = aud.END_DATE = null;
               StartDate_Dt.Visible = EndDate_Dt.Visible = false;
               break;
            case "002":
               AccessType_Ts.IsOn = false;
               aud.STRT_DATE = aud.END_DATE = DateTime.Now;
               StartDate_Dt.Visible = EndDate_Dt.Visible = true;
               break;
         }
      }

      private void Status_Ts_Toggled(object sender, EventArgs e)
      {
         var aud = AccessDatasourceBs.Current as Data.Access_User_Datasource;
         if (aud == null) return;

         var ts = sender as ToggleSwitch;

         aud.STAT = ts.IsOn ? "001" : "002";
         switch (aud.STAT)
         {
            case "001":
               Status_Ts.IsOn = true;
               break;
            case "002":
               Status_Ts.IsOn = false;
               break;
         }
      }

      private void User_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            UserGatewayBs.DataSource = iProject.User_Gateways.Where(ug => ug.USER_ID == (long)e.NewValue && ug.VALD_TYPE == "002");
         }
         catch (Exception)
         {

         }
      }

      private void DeleteConnection_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var aud = AccessDatasourceBs.Current as Data.Access_User_Datasource;
            if (aud == null || MessageBox.Show(this, "آیا با حذف ارتباط موافق هستید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            iProject.Access_User_Datasources.DeleteOnSubmit(aud);
            iProject.SubmitChanges();

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     //new Job(SendType.Self, 25 /* Execute DoWork4SettingsAccountChangePassword */),                  
                     new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 10 /* Execute ActionCallWindows */){Input = new XElement("TabPage", new XAttribute("showtabpage", "tp_002"))}
                  }
               )
            );

            Back_Butn_Click(null, null);
         }
         catch (Exception)
         {

         }
      }
   }
}
