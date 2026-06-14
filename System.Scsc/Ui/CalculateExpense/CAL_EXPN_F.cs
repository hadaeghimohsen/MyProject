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

            if (cexc.CODE != 0)
            {
               CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == cexc.MTOD_CODE && c.CTGY_STAT == "002");
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

            if (bcex.CODE != 0)
               CtgyBs.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == bcex.MTOD_CODE && c.CTGY_STAT == "002");
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
            if (BcexBs.List.OfType<Data.Base_Calculate_Expense>().Any(b => b.CODE == 0)) return;
            BcexBs.AddNew();

            var crnt = BcexBs.Current as Data.Base_Calculate_Expense;
            iScsc.Base_Calculate_Expenses.InsertOnSubmit(crnt);
            crnt.STAT = "002";
            crnt.PYMT_STAT = "002";
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
            crnt.STAT = "002";
            crnt.PYMT_STAT = "002";
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

      private void DuplBcex_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var bcex = BcexBs.Current as Data.Base_Calculate_Expense;
            if (bcex == null) return;

            if (BcexBs.List.OfType<Data.Base_Calculate_Expense>().Any(b => b.CODE == 0)) return;

            BcexBs.AddNew();
            var crnt = BcexBs.Current as Data.Base_Calculate_Expense;

            crnt.COCH_DEG = bcex.COCH_DEG;
            crnt.RQTP_CODE = bcex.RQTP_CODE;
            crnt.RQTT_CODE = bcex.RQTT_CODE;
            crnt.EPIT_CODE = bcex.EPIT_CODE;
            crnt.CALC_TYPE = bcex.CALC_TYPE;
            crnt.PRCT_VALU = bcex.PRCT_VALU;
            crnt.PYMT_STAT = bcex.PYMT_STAT;
            crnt.MTOD_CODE = bcex.MTOD_CODE;
            crnt.CTGY_CODE = bcex.CTGY_CODE;
            crnt.CALC_EXPN_TYPE = bcex.CALC_EXPN_TYPE;

            iScsc.Base_Calculate_Expenses.InsertOnSubmit(crnt);
         }
         catch {}
      }

      private void DuplCexc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            if (CexcBs.List.OfType<Data.Calculate_Expense_Coach>().Any(c => c.CODE == 0)) return;

            CexcBs.AddNew();
            var crnt = CexcBs.Current as Data.Calculate_Expense_Coach;

            crnt.COCH_FILE_NO = cexc.COCH_FILE_NO;
            crnt.COCH_DEG = cexc.COCH_DEG;
            crnt.RQTP_CODE = cexc.RQTP_CODE;
            crnt.RQTT_CODE = cexc.RQTT_CODE;
            crnt.EPIT_CODE = cexc.EPIT_CODE;
            crnt.CALC_TYPE = cexc.CALC_TYPE;
            crnt.PRCT_VALU = cexc.PRCT_VALU;
            crnt.PYMT_STAT = cexc.PYMT_STAT;
            crnt.MTOD_CODE = cexc.MTOD_CODE;
            crnt.CTGY_CODE = cexc.CTGY_CODE;
            crnt.CALC_EXPN_TYPE = cexc.CALC_EXPN_TYPE;

            iScsc.Calculate_Expense_Coaches.InsertOnSubmit(crnt);
         }
         catch { }
      }

      private void CbmtCode_Lov_Popup(object sender, EventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            Cexc_Gv.ActiveFilterString = "1=1 ";

            if (cexc.MTOD_CODE != null)
               Cexc_Gv.ActiveFilterString = string.Format("AND MTOD_CODE = '{0}'", cexc.MTOD_CODE);
            if(cexc.COCH_FILE_NO != null)
               Cexc_Gv.ActiveFilterString = string.Format("AND COCH_FILE_NO = '{0}'", cexc.COCH_FILE_NO);
         }
         catch {}
      }

      private void CbmtCode_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            var cbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == (long)e.NewValue);

            if (cexc.CODE == 0)
            {
               cexc.COCH_FILE_NO = cbmt.COCH_FILE_NO;
               cexc.MTOD_CODE = cbmt.MTOD_CODE;
               CtgyBs.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");
            }
            else
            {
               if (cexc.COCH_FILE_NO == null)
                  cexc.COCH_FILE_NO = cbmt.COCH_FILE_NO;
               if (cexc.MTOD_CODE == null)
               {
                  cexc.MTOD_CODE = cbmt.MTOD_CODE;
                  CtgyBs.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");
               }

               if (cexc.COCH_FILE_NO != cbmt.COCH_FILE_NO && MessageBox.Show(this, "عنوان برنامه گروه با اسم سرپرست همخوانی ندارد، آیا مایل به جایگزین کردن هستین؟", "تغییر سرپرست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
               else
                  cexc.COCH_FILE_NO = cbmt.COCH_FILE_NO;
               if (cexc.MTOD_CODE != cbmt.MTOD_CODE && MessageBox.Show(this, "عنوان برنامه گروه با نام گروه همخوانی ندارد، آیا مایل به جایگزین کردن هستین؟", "تغییر گروه", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
               else
               {
                  cexc.MTOD_CODE = cbmt.MTOD_CODE;
                  CtgyBs.DataSource = iScsc.Category_Belts.Where(cb => cb.MTOD_CODE == cbmt.MTOD_CODE && cb.CTGY_STAT == "002");
               }
            }

            CexcBs.EndEdit();            
         }
         catch {}
      }

      private void Ctgy2_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var c = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (c == null || c.CODE == 0) return;

            iScsc.UPD_CEXC_P(
               c.CODE, c.EXPN_CODE, c.COCH_FILE_NO, c.EPIT_CODE, c.RQTT_CODE, c.COCH_DEG,
               c.EXTP_CODE, c.MTOD_CODE, (long?)e.NewValue, c.CALC_TYPE, c.PRCT_VALU, c.STAT,
               c.RQTP_CODE, c.CALC_EXPN_TYPE, c.PYMT_STAT, c.MIN_NUMB_ATTN, c.MIN_ATTN_STAT, 
               c.RDUC_AMNT, c.CBMT_CODE, c.EFCT_DATE_TYPE, c.EXPR_PAY_DAY, c.TAX_PRCT_VALU, c.FORE_GIVN_ATTN_NUMB
            );

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
