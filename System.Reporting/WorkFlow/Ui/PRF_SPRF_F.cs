using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   public partial class PRF_SPRF_F : UserControl
   {
      public PRF_SPRF_F()
      {
         InitializeComponent();
      }

      private XDocument xAccessProfiler { get; set; }
      private XDocument xRunReport { get; set; }
      private bool OperationComplete = true;
      private string Next_Form { get; set; }

      partial void bp_flow_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);

      partial void Tile_Ctrl_MouseClick(object sender, MouseEventArgs e);

      partial void cbe_role_EditValueChanged(object sender, EventArgs e);
      
      
   }
}
