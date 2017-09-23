using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Club
{
   partial class MSTR_CLUB_F
   {
      partial void clubsBindingSource_PositionChanged(object sender, EventArgs e)
      {
         if (clubsBindingSource.DataSource == null)
            return;

         fighter_PublicsBindingSource.DataSource = from p in iScsc.Fighter_Publics
                                                   join f in iScsc.Fighters
                                                   on p.FIGH_FILE_NO equals f.FILE_NO
                                                   where p.RECT_CODE == "004" &&
                                                   p.RWNO == f.FGPB_RWNO_DNRM &&
                                                   p.Club == clubsBindingSource.Current as Data.Club
                                                   orderby p.FIGH_FILE_NO, p.RWNO
                                                   select p;

      }

      partial void Btn_SubmitClub_Click(object sender, EventArgs e)
      {
         try
         {
            Focus();
            Validate();
            clubsBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var Current = clubsBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            regionBindingSource.DataSource = iScsc.Regions;
            clubsBindingSource.Position = Current;
         }
      }

      partial void Btn_DelClub_Click(object sender, EventArgs e)
      {
         if (clubsBindingSource.DataSource == null)
            return;

         try
         {
            if (MessageBox.Show(this, "آیا از پاک کردن باشگاه مطمئن هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes && (clubsBindingSource.Current as Data.Club).CODE >= 0)
            {
               clubsBindingSource.EndEdit();
               iScsc.DEL_CLUB_P((clubsBindingSource.Current as Data.Club).CODE);
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var Current = clubsBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            regionBindingSource.DataSource = iScsc.Regions;
            clubsBindingSource.Position = Current;
         }
      }

      partial void Btn_SubmitClubMethod_Click(object sender, EventArgs e)
      {
         if (club_MethodsBindingSource.DataSource == null)
            return;

         try
         {
            Focus();
            Validate();
            club_MethodsBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var CurrentClub = clubsBindingSource.Position;
            var CurrentCbMt = club_MethodsBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            regionBindingSource.DataSource = iScsc.Regions;
            clubsBindingSource.Position = CurrentClub;
            club_MethodsBindingSource.Position = CurrentCbMt;
         }
      }
   }
}
