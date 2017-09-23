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

namespace System.Reporting.Self.Ui
{
   public partial class ReportCtrl : UserControl
   {
      public ReportCtrl()
      {
         InitializeComponent();
      }

      private void sb_settingsmetro_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Reporting",
               new List<Job>
               {
                  new Job(SendType.Self, 03 /* Execute DoWork4InteractWithSettingsMetro */)
               }));
      }

      private void sb_rpt_srpt_f_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "WorkFlow", 02 /* Execute DoWork4RPT_SRPT_F */, SendType.Self));
      }
   }
}
