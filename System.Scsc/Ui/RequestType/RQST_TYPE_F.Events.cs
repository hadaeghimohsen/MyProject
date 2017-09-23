using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.RequestType
{
   partial class RQST_TYPE_F
   {
      partial void Btn_RqstType_Submit_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            request_TypeBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            request_TypeBindingSource.DataSource = iScsc.Request_Types;
         }
      }

      partial void Btn_ResnSubmit_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            reason_SpecsBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            request_TypeBindingSource.DataSource = iScsc.Request_Types;
         }
      }

      partial void Btn_DelResn_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از پاک کردن دلیل مربوط به نوع درخواست مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         try
         {
            Validate();
            reason_SpecsBindingSource.EndEdit();

            iScsc.DEL_RESN_P((request_TypeBindingSource.Current as Data.Request_Type).CODE, (reason_SpecsBindingSource.Current as Data.Reason_Spec).RWNO);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            request_TypeBindingSource.DataSource = iScsc.Request_Types;
         }
      }
   }
}
