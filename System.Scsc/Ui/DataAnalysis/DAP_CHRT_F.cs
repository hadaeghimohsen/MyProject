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

      private async void Execute_Query()
      {
         var selectedTab = tc_master.SelectedTab;
         bool yearLov1Empty = Year_Lov1.Items.Count == 0;
         int yearLov1Index = Year_Lov1.SelectedIndex;
         object yearLov1Item = Year_Lov1.SelectedItem;
         bool yearLov2Empty = Year_Lov2.Items.Count == 0;
         int yearLov2Index = Year_Lov2.SelectedIndex;
         object yearLov2Item = Year_Lov2.SelectedItem;

         var result = await Task.Run(() =>
         {
            using (var db = new Data.iScscDataContext(ConnectionString))
            {
               if (selectedTab == tp_001)
               {
                  if (yearLov1Empty)
                  {
                     var srdList = db.VD_SaleReceiptDiscounts.ToList();
                     var years = srdList.Select(srd => (object)srd.YEAR).Distinct().ToArray();
                     return new { type = 1, srdList, years, loadAll = true };
                  }
                  else if (yearLov1Index >= 0)
                  {
                     var srdList = db.VD_SaleReceiptDiscounts.Where(srd => srd.YEAR.ToString() == yearLov1Item.ToString()).ToList();
                     return new { type = 1, srdList, years = (object[])null, loadAll = false };
                  }
                  return new { type = 0 };
               }
               else if (selectedTab == tp_002)
               {
                  if (yearLov2Empty)
                  {
                     var rqstList = db.Requests.ToList();
                     var years = rqstList.Select(r => (object)r.YEAR).Distinct().ToArray();
                     return new { type = 2, rqstList, years, loadAll = true };
                  }
                  else if (yearLov2Index >= 0)
                  {
                     var rqstList = db.Requests.Where(r => r.YEAR.ToString() == yearLov2Item.ToString()).ToList();
                     return new { type = 2, rqstList, years = (object[])null, loadAll = false };
                  }
                  return new { type = 0 };
               }
               else if (selectedTab == tp_003)
               {
                  return new { type = 3, tells = db.V_CellPhonCollections.ToList() };
               }
               return new { type = 0 };
            }
         });

         iScsc = new Data.iScscDataContext(ConnectionString);
         if (result.type == 1)
         {
            vdSrdBs1.DataSource = result.srdList;
            if (result.loadAll && result.years != null)
               Year_Lov1.Items.AddRange(result.years);
         }
         else if (result.type == 2)
         {
            RqstBs2.DataSource = result.rqstList;
            if (result.loadAll && result.years != null)
               Year_Lov2.Items.AddRange(result.years);
         }
         else if (result.type == 3)
         {
            TellsBs3.DataSource = result.tells;
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
