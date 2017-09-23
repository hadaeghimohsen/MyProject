using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Regulation
{
   partial class REGL_ACNT_F
   {
      partial void Btn_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            expense_CashBindingSource.EndEdit();

            /*
            var sendUpdate = from n in ((IEnumerable<Data.Expense_Cash>)expense_CashBindingSource.DataSource).AsEnumerable()
                             join o in iScsc.Expense_Cashes
                             on new { n.REGN_PRVN_CNTY_CODE, n.REGN_PRVN_CODE, n.REGN_CODE, n.REGL_YEAR, n.REGL_CODE, n.CASH_CODE, n.EXTP_CODE } equals
                             new { o.REGN_PRVN_CNTY_CODE, o.REGN_PRVN_CODE, o.REGN_CODE, o.REGL_YEAR, o.REGL_CODE, o.CASH_CODE, o.EXTP_CODE }
                             where n.EXCS_STAT != o.EXCS_STAT
                             select n;

                             
            sendUpdate
               .ToList()
               .ForEach(ex => 
                  iScsc.UPD_EXCS_P(
                     ex.REGN_PRVN_CNTY_CODE, 
                     ex.REGN_PRVN_CODE,
                     ex.REGN_CODE,
                     ex.REGL_YEAR,
                     ex.REGL_CODE,
                     ex.EXTP_CODE,
                     ex.CASH_CODE,
                     ex.EXCS_STAT
                  )
               );
            */
            iScsc.SubmitChanges();
            iScsc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, iScsc.Expense_Cashes);
            expense_CashBindingSource.DataSource = iScsc.Expense_Cashes;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }   
   }
}
