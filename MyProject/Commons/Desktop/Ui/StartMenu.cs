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
using System.Xml.Linq;

namespace MyProject.Commons.Desktop.Ui
{
   public partial class StartMenu : UserControl
   {
      public StartMenu()
      {
         InitializeComponent();
      }

      private void Btn_Logout_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4StartMenu */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_Shutdown_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "SettingsDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Desktop", 10 /* Execute ActionCallForm */, SendType.SelfToUserInterface) { Input = new XElement("Action", new XAttribute("type", "shutdown")) }
         );
      }

      private void Btn_CurrentUserInfo_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "SettingsDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         var GetCurrentUser = new Job(SendType.External, "Localhost", "Commons", 12 /* Execute GetCurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(
            GetCurrentUser
         );

         // Open Current User Info
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons:Program:DataGuard:SecurityPolicy:User", 08 /* Execute DoWork4CurrentUserInfo */, SendType.Self) { Input = GetCurrentUser.Output }
         );
      }

      private void Btn_CurrentUserChangePassword_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "SettingsDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         var GetCurrentUser = new Job(SendType.External, "Localhost", "Commons", 12 /* Execute GetCurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(
            GetCurrentUser
         );

         _DefaultGateway.Gateway(
            new Job(SendType.Self, "localhost", "Commons:Program:DataGuard:SecurityPolicy:User", 10 /* Execute DoWork4CurrentChangePassword */, SendType.Self) { Input = GetCurrentUser.Output }
         );
      }

      private void Btn_ControlPanel_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "SettingsDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         Job _ShowSecuritySettings = new Job(SendType.External, "Desktop", "Commons:Program:DataGuard", 03 /* Execute DoWork4SecuritySettings */, SendType.Self);
         _DefaultGateway.Gateway(_ShowSecuritySettings);
      }

      private void Shutdown_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Menu
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "StartMenu", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 25 /* Execute DoWork4Shutingdown */, SendType.Self));
      }

      private void Settings_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "StartMenu", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         // Open Settings 
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons:Program:DataGuard:SecurityPolicy", 09 /* Execute DoWork4Settings */, SendType.Self)
         );
      }

      private void ActiveUser_Butn_Click(object sender, EventArgs e)
      {

      }

      private void FeedBack_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 32 /* Execute DoWork4SendFeedBack */, SendType.Self)
         {
            Input =
               new XElement("SendEmail",
                  new XAttribute("username", ""),
                  new XAttribute("type", "feedback"),
                  new XAttribute("subsys", "0")
               )
         });
      }

      private void Message_Butn_Click(object sender, EventArgs e)
      {

      }

      private void CurrentUser_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "StartMenu", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.External, "Program",
                           new List<Job>
                           {               
                              new Job(SendType.External, "DataGuard",
                                 new List<Job>
                                 {
                                    new Job(SendType.External, "SecurityPolicy",
                                       new List<Job>
                                       {                  
                                          new Job(SendType.Self, 14 /* Execute DoWork4SettingsAccount */),
                                          new Job(SendType.SelfToUserInterface, "SettingsAccount", 10 /* Execute ActionCallWindows */)
                                       }
                                    )
                                 }
                              )
                           }
                        )
                     }
                  )
               }
            )
         );
      }

      private void Weather_Butm_Click(object sender, EventArgs e)
      {

      }

      private void GoogleMap_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
         );
      }
   }
}
