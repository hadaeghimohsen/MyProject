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

namespace MyProject.Commons.Desktop.Ui
{
   public partial class StartDrawer : UserControl
   {
      public StartDrawer()
      {
         InitializeComponent();
      }

      private void Btn_SettingsAccount_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "StartDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         // Open Settings Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 04 /* Execute DoWork4SettingsDrawer */, SendType.Self) { Input = toggleMode }
         );
      }

      private void Btn_Settings_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "StartDrawer", 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );

         // Open Settings 
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons:Program:DataGuard:SecurityPolicy", 09 /* Execute DoWork4Settings */, SendType.Self)
         );
      }
   }
}
