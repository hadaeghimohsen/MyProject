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
   public partial class SettingsDrawer : UserControl
   {
      public SettingsDrawer()
      {
         InitializeComponent();
      }

      private void Btn_Logout_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "SettingsDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Desktop", 10 /* Execute ActionCallForm */, SendType.SelfToUserInterface) { Input = new XElement("Action", new XAttribute("type", "logout")) }
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
   }
}
