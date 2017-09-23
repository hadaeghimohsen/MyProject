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
using System.Diagnostics;

namespace MyProject.Commons.Ui
{
   public partial class Shutdown : UserControl
   {
      public Shutdown()
      {
         InitializeComponent();
      }

      private void Cancl_Butn_Clicked(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Shutdown_Butn_Click(object sender, EventArgs e)
      {
         Application.Exit();
         Process.GetCurrentProcess().Kill();
      }

      private void Logout_Butn_Click(object sender, EventArgs e)
      {
         Job _Logout = new Job(SendType.External, "Localhost",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */),
               new Job(SendType.External, "Program","DataGuard", 10 /* Execute DoWork4TryLogin */, SendType.Self),
               new Job(SendType.External, "Program", "", 03 /* Execute Stop_Service_Component */, SendType.Self)               
            });
         _DefaultGateway.Gateway(_Logout);
      }
   }
}
