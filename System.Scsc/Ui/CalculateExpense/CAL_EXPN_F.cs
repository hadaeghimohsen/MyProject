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
            MtodBs.DataSource = iScsc.Methods.Where(m => m.MTOD_STAT == "002");
         }

         requery = false;
      }

      private void Commit()
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            
            if(tb_master.SelectedTab == tp_001)
            {

            }
            else if(tb_master.SelectedTab == tp_002)
            {
               if (PrctValu_Txt.EditValue == null || PrctValu_Txt.EditValue.ToString() == "") PrctValu_Txt.EditValue = 0;
               if (Amnt_Txt.EditValue == null || Amnt_Txt.EditValue.ToString() == "") Amnt_Txt.EditValue = 0;

               if (cexc.CODE == 0)
                  iScsc.INS_CEXC_P((long?)CochFileNo_Lov.EditValue, null, null, (string)Degr_Lov.EditValue, (long?)ExtpCode_Lov.EditValue, (long?)Mtod_Lov.EditValue, (long?)Ctgy_Lov.EditValue, (string)CalcType_Lov.EditValue, Convert.ToInt64(Amnt_Txt.EditValue), Convert.ToDouble(PrctValu_Txt.EditValue), "002");
               else
                  iScsc.UPD_CEXC_P(cexc.CODE, (long?)CochFileNo_Lov.EditValue, null, null, (string)Degr_Lov.EditValue, (long?)ExtpCode_Lov.EditValue, (long?)Mtod_Lov.EditValue, (long?)Ctgy_Lov.EditValue, (string)CalcType_Lov.EditValue, Convert.ToInt64(Amnt_Txt.EditValue), Convert.ToDouble(PrctValu_Txt.EditValue), cexc.STAT);
            }
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

            CochFileNo_Lov.EditValue = cexc.COCH_FILE_NO;
            Degr_Lov.EditValue = cexc.COCH_DEG;
            ExtpCode_Lov.EditValue = cexc.EXTP_CODE;
            CalcType_Lov.EditValue = cexc.CALC_TYPE;
            Mtod_Lov.EditValue = cexc.MTOD_CODE;
            Ctgy_Lov.EditValue = cexc.CTGY_CODE;
            Amnt_Txt.EditValue = cexc.AMNT;
            PrctValu_Txt.EditValue = cexc.PRCT_VALU;
            Stat_Pkbt.PickChecked = cexc.STAT == "002" ? true : false;
         }
         catch (Exception ){}
      }

      private void Stat_Pkbt_PickCheckedChange(object sender)
      {
         try
         {
            var cexc = CexcBs.Current as Data.Calculate_Expense_Coach;
            if (cexc == null) return;

            cexc.STAT = Stat_Pkbt.PickChecked ? "002" : "001";
         }
         catch (Exception ) { }
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
   }
}
