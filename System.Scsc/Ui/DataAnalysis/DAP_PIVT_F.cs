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

namespace System.Scsc.Ui.DataAnalysis
{
   public partial class DAP_PIVT_F : UserControl
   {
      public DAP_PIVT_F()
      {
         InitializeComponent();
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);

         if(tc_master.SelectedTab == tp_001)
         {
            vSaleBs1.DataSource = iScsc.V_Sales;
         }
         else if (tc_master.SelectedTab == tp_002)
         {
            vRcptBs2.DataSource = iScsc.V_ReceiptPayments;
         }
         else if(tc_master.SelectedTab == tp_003)
         {
            vPydsBs3.DataSource = iScsc.V_DiscountPayments;
         }
      }

      private void RefreshBn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void ExitBn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
