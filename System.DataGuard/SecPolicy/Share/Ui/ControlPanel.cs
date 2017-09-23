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
   public partial class ControlPanel : UserControl
   {
      public ControlPanel()
      {
         InitializeComponent();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void LNK_SecurityManagment_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 04 /* Execute DoWork4SecurityManagment */, SendType.Self)
         );
      }

      private void LNK_UserNetwork_Click(object sender, EventArgs e)
      {
         
      }

      private void LNK_LicSoftware_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 05 /* Execute DoWork4SoftVersionControl */, SendType.Self)
         );
      }

      private void LNK_UserGroups_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 02 /* Execute DoWork4SecuritySettings */, SendType.Self)
         );
      }

      private void LNK_ActiveCyclData_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 06 /* Execute DoWork4ActiveCyclData */, SendType.Self)
         );
      }

   }
}
