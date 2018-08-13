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
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.CalculateExpense
{
   public partial class CAL_EXPN_F : UserControl
   {
      public CAL_EXPN_F()
      {
         InitializeComponent();
      }
      private bool requery = false;
      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);

         if(tb_master.SelectedTab == tp_001)
         {
            BcexBs.DataSource = iScsc.Base_Calculate_Expenses;
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            CexcBs.DataSource = iScsc.Calculate_Expense_Coaches;
            //MtodBs.DataSource = iScsc.Methods.Where(m => m.MTOD_STAT == "002");
         }

         requery = false;
      }

      private void Commit()
      {
         try         
         {
            BcexBs.EndEdit();
            CexcBs.EndEdit();

            Bcex_Gv.PostEditor();
            Cexc_Gv.PostEditor();

            iScsc.SubmitChanges();            
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {               
               Execute_Query();               
            }
         }
      }

      private void tsb_reloadbcex_Click(object sender, EventArgs e)
      {
         iScsc.RELD_CEXC_F(new XElement("Process"));
         Execute_Query();
      }

      private void tsb_savebcex_Click(object sender, EventArgs e)
      {
         Commit();
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void CexcBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            switch (cexc.STAT)
            {
               case "001":
                  Stat1_Tg.IsOn = false;
                  break;
               case "002":
                  Stat1_Tg.IsOn = true;
                  break;
            }

            switch (cexc.PYMT_STAT)
            {
               case "001":
                  PymtStat1_Tg.IsOn = false;
                  break;
               case "002":
                  PymtStat1_Tg.IsOn = true;
                  break;
            }
         }
         catch (Exception ){}
      }

      private void Mtod_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var mtod = (long)e.NewValue;

            CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == mtod && c.CTGY_STAT == "002");
         }
         catch (Exception ){}
      }

      private void Mtod1_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long)e.NewValue && c.CTGY_STAT == "002");
         }
         catch{}
      }

      private void BcexBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var bcex = BcexBs.Current as Data.Base_Calculate_Expense;
            if (bcex == null) return;

            switch (bcex.STAT)
            {
               case "001":
                  Stat_Tg.IsOn = false;
                  break;
               case "002":
                  Stat_Tg.IsOn = true;
                  break;
            }

            switch (bcex.PYMT_STAT)
            {
               case "001":
                  PymtStat_Tg.IsOn = false;
                  break;
               case "002":
                  PymtStat_Tg.IsOn = true;
                  break;
            }
         }
         catch{}
      }

      private void Stat_Tg_Toggled(object sender, EventArgs e)
      {
         try
         {
            var bcex = BcexBs.Current as Data.Base_Calculate_Expense;
            if (bcex == null) return;

            bcex.STAT = Stat_Tg.IsOn ? "002" : "001";
         }
         catch{}
      }

      private void PymtStat_Tg_Toggled(object sender, EventArgs e)
      {
         try
         {
            var bcex = BcexBs.Current as Data.Base_Calculate_Expense;
            if (bcex == null) return;

            bcex.PYMT_STAT = PymtStat_Tg.IsOn ? "002" : "001";
         }
         catch { }
      }

      private void Mtod2_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long)e.NewValue && c.CTGY_STAT == "002");
         }
         catch { }
      }

      private void Stat1_Tg_Toggled(object sender, EventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            cexc.STAT = Stat1_Tg.IsOn ? "002" : "001";
         }
         catch { }
      }

      private void PymtStat1_Tg_Toggled(object sender, EventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            cexc.PYMT_STAT = PymtStat1_Tg.IsOn ? "002" : "001";
         }
         catch { }
      }

      private void AddBcex_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            BcexBs.AddNew();

            var crnt = BcexBs.Current as Data.Base_Calculate_Expense;
            iScsc.Base_Calculate_Expenses.InsertOnSubmit(crnt);
         }
         catch{}
      }

      private void DelBcex_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var bcex = BcexBs.Current as Data.Base_Calculate_Expense;
            if (bcex == null) return;

            if (MessageBox.Show(this,"آیا با حذف رکورد موافق هستین؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            iScsc.DEL_BSEX_P(bcex.CODE);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void AddCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            CexcBs.AddNew();

            var crnt = CexcBs.Current as Data.Calculate_Expense_Coach;
            iScsc.Calculate_Expense_Coaches.InsertOnSubmit(crnt);
         }
         catch { }
      }

      private void DelCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستین؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            iScsc.DEL_CEXC_P(cexc.CODE);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }      
   }
}
