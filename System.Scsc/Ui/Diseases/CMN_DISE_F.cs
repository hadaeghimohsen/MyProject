using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Diseases
{
   public partial class CMN_DISE_F : UserControl
   {
      public CMN_DISE_F()
      {
         InitializeComponent();
      }

      private async void Execute_Query()
      {
         try
         {
            var result = await Task.Run(() =>
            {
               using (var db = new Data.iScscDataContext(ConnectionString))
               {
                  return db.Diseases_Types.ToList();
               }
            });
            diseases_TypeBindingSource.DataSource = result;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void diseases_TypeBindingNavigatorSaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            diseases_TypeBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            Execute_Query();
         }
      }
   }
}
