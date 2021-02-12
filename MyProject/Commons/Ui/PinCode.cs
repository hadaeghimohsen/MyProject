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
using System.Xml.Linq;

namespace MyProject.Commons.Ui
{
   public partial class PinCode : UserControl
   {
      public PinCode()
      {
         InitializeComponent();
      }

      private void Successful_Butn_Clicked(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
