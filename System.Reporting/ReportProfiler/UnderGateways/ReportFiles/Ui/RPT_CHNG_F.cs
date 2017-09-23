using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Reporting.ReportProfiler.UnderGateways.ReportFiles.Ui
{
   public partial class RPT_CHNG_F : UserControl
   {
      public RPT_CHNG_F()
      {
         InitializeComponent();
      }

      partial void be_report_name_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      partial void wbp_submit_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);
      
   }
}
