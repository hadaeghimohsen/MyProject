using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Regulation
{
   partial class MSTR_EPIT_F
   {
      partial void expense_ItemBindingNavigatorSaveItem_Click(object sender, EventArgs e)
      {
          try
          {
              Focus();
              Validate();
              EpitBs1.EndEdit();

              iScsc.SubmitChanges();
          }
          catch (Exception ex)
          {
              MessageBox.Show(ex.Message);
          }
          finally
          {
              iScsc = new Data.iScscDataContext(ConnectionString);
              EpitBs1.DataSource = iScsc.Expense_Items;
          }
      }
   }
}
