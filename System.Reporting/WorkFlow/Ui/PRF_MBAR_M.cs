using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Reporting.WorkFlow.Ui
{
   public partial class PRF_MBAR_M : UserControl
   {
      public PRF_MBAR_M()
      {
         InitializeComponent();
      }

      partial void wbp_profiler_desktop_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);
   }
}
