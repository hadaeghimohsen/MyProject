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
   public partial class DAP_CHRT_F : UserControl
   {
      public DAP_CHRT_F()
      {
         InitializeComponent();
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if (tc_master.SelectedTab == tp_001)
         {
            if (Year_Lov1.Items.Count == 0)
            {
               vdSrdBs1.DataSource = iScsc.VD_SaleReceiptDiscounts;
               Year_Lov1.Items.AddRange(vdSrdBs1.List.OfType<Data.VD_SaleReceiptDiscount>().Select(srd => (object)srd.YEAR).Distinct().ToArray());
            }
            else if (Year_Lov1.SelectedIndex >= 0)
               vdSrdBs1.DataSource = iScsc.VD_SaleReceiptDiscounts.Where(srd => srd.YEAR.ToString() == Year_Lov1.SelectedItem.ToString());
            
            /*Year_Lov1.Items.Clear();

            Year_Lov1.Items.AddRange(vdSrdBs1.List.OfType<Data.VD_SaleReceiptDiscount>().Select(srd => (object)srd.YEAR).Distinct().ToArray());*/
         }
         else if(tc_master.SelectedTab == tp_002)
         {
            if (Year_Lov2.Items.Count == 0)
            {
               RqstBs2.DataSource = iScsc.Requests;
               Year_Lov2.Items.AddRange(RqstBs2.List.OfType<Data.Request>().Select(r => (object)r.YEAR).Distinct().ToArray());
            }
            else if (Year_Lov2.SelectedIndex >= 0)
               RqstBs2.DataSource = iScsc.Requests.Where(r => r.YEAR.ToString() == Year_Lov2.SelectedItem.ToString());

            /*Year_Lov2.Items.Clear();

            Year_Lov2.Items.AddRange(RqstBs2.List.OfType<Data.Request>().Select(r => (object)r.YEAR).Distinct().ToArray());*/
         }
         else if (tc_master.SelectedTab == tp_003)
         {
            TellsBs3.DataSource = iScsc.V_CellPhonCollections;
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

      private void Year_Lov_SelectedIndexChanged(object sender, EventArgs e)
      {
         Execute_Query();
      }
   }
}
