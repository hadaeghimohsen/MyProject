using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.RequesterType
{
   partial class RQTR_TYPE_F
   {
      partial void Btn_RqttSubmit_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            requester_TypeBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            requester_TypeBindingSource.DataSource = iScsc.Requester_Types;
         }
      }

      partial void Btn_RqttDel_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از حذف کردن متقاضی خود مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         try
         {
            iScsc.DEL_RQTT_P((requester_TypeBindingSource.Current as Data.Requester_Type).CODE);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            requester_TypeBindingSource.DataSource = iScsc.Requester_Types;
         }
      }
   }
}
