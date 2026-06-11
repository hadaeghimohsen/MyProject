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

      private async void Execute_Query()
      {
         var selectedTab = tc_master.SelectedTab;

         var result = await Task.Run(() =>
         {
            using (var db = new Data.iScscDataContext(ConnectionString))
            {
               if (selectedTab == tp_001)
               {
                  return new { type = 1, vSales = db.V_Sales.ToList() };
               }
               else if (selectedTab == tp_002)
               {
                  return new { type = 2, vRcpts = db.V_ReceiptPayments.ToList() };
               }
               else if (selectedTab == tp_003)
               {
                  return new { type = 3, vPyds = db.V_DiscountPayments.ToList() };
               }
               return new { type = 0 };
            }
         });

         iScsc = new Data.iScscDataContext(ConnectionString);
         if (result.type == 1)
         {
            vSaleBs1.DataSource = result.vSales;
         }
         else if (result.type == 2)
         {
            vRcptBs2.DataSource = result.vRcpts;
         }
         else if (result.type == 3)
         {
            vPydsBs3.DataSource = result.vPyds;
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
