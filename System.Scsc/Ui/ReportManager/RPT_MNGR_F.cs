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

namespace System.Scsc.Ui.ReportManager
{
   public partial class RPT_MNGR_F : UserControl
   {
      public RPT_MNGR_F()
      {
         InitializeComponent();
      }

      private void vc_reportviewer_Close(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", GetType().Name, 04 /* Execute UnPaint */, SendType.SelfToUserInterface));
      }
   }
}
