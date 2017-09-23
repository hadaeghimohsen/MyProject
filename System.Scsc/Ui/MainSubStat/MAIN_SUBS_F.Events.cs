using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.MainSubStat
{
   partial class MAIN_SUBS_F
   {
      partial void main_StateBindingNavigatorSaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            main_StateBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var CurnMain = main_StateBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            main_StateBindingSource.DataSource = iScsc.Main_States;
            main_StateBindingSource.Position = CurnMain;
         }
      }

      partial void Btn_SubStat_Submit_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            main_StateBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var CurnMain = main_StateBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            main_StateBindingSource.DataSource = iScsc.Main_States;
            main_StateBindingSource.Position = CurnMain;
         }
      }

      partial void Btn_DelMstt_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از حذف وضعیت اصلی درون سیستم مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  
         iScsc.DEL_MSTT_P((main_StateBindingSource.Current as Data.Main_State).CODE);
         iScsc = new Data.iScscDataContext(ConnectionString);
         main_StateBindingSource.DataSource = iScsc.Main_States;
      }

      partial void Btn_DelSstt_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از حذف وضعیت فرعی درون سیستم مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         iScsc.DEL_SSTT_P((main_StateBindingSource.Current as Data.Main_State).CODE, (sub_StatesBindingSource.Current as Data.Sub_State).CODE);
         iScsc = new Data.iScscDataContext(ConnectionString);
         main_StateBindingSource.DataSource = iScsc.Main_States;
      }
   }
}
