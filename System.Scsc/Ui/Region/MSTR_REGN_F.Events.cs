using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Region
{
   partial class MSTR_REGN_F
   {
      partial void SubmitChange_Cnty_Click(object sender, EventArgs e)
      {
         try
         {
            Focus();
            Validate();
            countryBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            countryBindingSource.DataSource = iScsc.Countries;
         }
      }

      partial void SubmitChange_Prvn_Click(object sender, EventArgs e)
      {
         try
         {
            Focus();
            Validate();
            provincesBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            countryBindingSource.DataSource = iScsc.Countries;
         }
      }

      partial void SubmitChange_Regn_Click(object sender, EventArgs e)
      {
         try
         {
            Focus();
            Validate();
            regionsBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            countryBindingSource.DataSource = iScsc.Countries;
         }
      }

      partial void Delete_Cnty_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا از پاک کردن کشور انتخابی مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
               var Current = countryBindingSource.Current as Data.Country;
               iScsc.DEL_CNTY_P(Current.CODE);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            countryBindingSource.DataSource = iScsc.Countries;
         }
      }

      partial void Delete_Prvn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا از پاک کردن استان انتخابی مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
               var Current = provincesBindingSource.Current as Data.Province;
               iScsc.DEL_PRVN_P(Current.CNTY_CODE, Current.CODE);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            countryBindingSource.DataSource = iScsc.Countries;
         }
      }

      partial void Delete_Regn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا از پاک کردن ناحیه انتخابی مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
               var Current = regionsBindingSource.Current as Data.Region;
               iScsc.DEL_REGN_P(Current.PRVN_CNTY_CODE, Current.PRVN_CODE, Current.CODE);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            countryBindingSource.DataSource = iScsc.Countries;
         }
      }
   }
}
