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

namespace System.DataGuard.SecPolicy.User.Ui
{
   public partial class PropertySingleMenu : UserControl
   {
      public PropertySingleMenu()
      {
         InitializeComponent();
      }

      private string InputXmlData;

      private void sb_profile_Click(object sender, EventArgs e)
      {
         Job _Profile = new Job(SendType.External, "User",
            new List<Job>
            {
               new Job(SendType.Self, 04 /* Execute DoWork4Profile */){Input = InputXmlData}
            });
         _DefaultGateway.Gateway(_Profile);
      }

      private void sb_create_Click(object sender, EventArgs e)
      {
         Job _CreateNewUser = new Job(SendType.External, "User",
            new List<Job>
            {
               new Job(SendType.Self, 02 /* Execute DoWork4CreateNew */) 
            });
         _DefaultGateway.Gateway(_CreateNewUser);
      }

      private void sb_duplicate_Click(object sender, EventArgs e)
      {
         Job _Duplicate = new Job(SendType.External, "User",
            new List<Job>
            {
               new Job(SendType.Self, 07 /* Execute DoWork4Duplicate */){Input = InputXmlData}
            });
         _DefaultGateway.Gateway(_Duplicate);
      }
   }
}
