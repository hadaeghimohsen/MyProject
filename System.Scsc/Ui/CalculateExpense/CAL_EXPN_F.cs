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
         if(tabControl1.SelectedTab == tabPage1)
         {
            BCEX_BindingSource.DataSource = iScsc.Base_Calculate_Expenses;
         }
         else if (tabControl1.SelectedTab == tabPage2)
         {
            CEXC_BindingSource.DataSource = iScsc.Calculate_Expense_Coaches;
         }
      }

      private void Commit()
      {
         try
         {
            BCEX_BindingSource.EndEdit();
            CEXC_BindingSource.EndEdit();
            gridView1.PostEditor();
            gridView2.PostEditor();

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
               iScsc = new Data.iScscDataContext(ConnectionString);
               Execute_Query();
               requery = false;
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
   }
}
