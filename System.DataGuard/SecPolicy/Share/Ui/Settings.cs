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

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class Settings : UserControl
   {
      public Settings()
      {
         InitializeComponent();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name,  00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void System_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "" , 10 /* Execute DoWork4SettingsSystem */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 10 /* Execute DoWork4SettingsSystem */),
                  new Job(SendType.SelfToUserInterface, "SettingsSystem", 10 /* Execute ActionCallWindows */)
               }
            )
         );

      }

      private void Device_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 11 /* Execute DoWork4SettingsDevice */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 11 /* Execute DoWork4SettingsDevice */),
                  new Job(SendType.SelfToUserInterface, "SettingsDevice", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void NetworkConnection_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 12 /* Execute DoWork4SettingsNetworkConnection */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 12 /* Execute DoWork4SettingsNetworkConnection */),
                  new Job(SendType.SelfToUserInterface, "SettingsNetworkConnection", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void Personalization_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 13 /* Execute DoWork4SettingsPersonalization */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 13 /* Execute DoWork4SettingsPersonalization */),
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 08 /* Execute LoadDataAsync */){Executive = ExecutiveType.Asynchronous}, 
                  new Job(SendType.SelfToUserInterface, "SettingsPersonalization", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void Account_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 14 /* Execute DoWork4SettingsAccount */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 14 /* Execute DoWork4SettingsAccount */),
                  new Job(SendType.SelfToUserInterface, "SettingsAccount", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void ServiceApp_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 15 /* Execute DoWork4SettingsServiceApp */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 15 /* Execute DoWork4SettingsServicesApp */),
                  new Job(SendType.SelfToUserInterface, "SettingsServicesApp", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void Privacy_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 16 /* Execute DoWork4SettingsPrivacy */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 16 /* Execute DoWork4SettingsPrivacy */),
                  new Job(SendType.SelfToUserInterface, "SettingsPrivacy", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }

      private void UpdateSecurity_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            //new Job(SendType.External, "localhost", "", 17 /* Execute DoWork4SettingsUpdateSecurity */, SendType.Self)
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 17 /* Execute DoWork4SettingsUpdateSecurity */),
                  new Job(SendType.SelfToUserInterface, "SettingsUpdateSecurity", 10 /* Execute ActionCallWindows */)
               }
            )
         );
      }
   }
}
