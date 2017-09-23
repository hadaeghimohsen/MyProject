using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Attendance
{
   public partial class ATN_SAVE_F : UserControl
   {
      public ATN_SAVE_F()
      {
         InitializeComponent();
      }
      private bool requeryData = default(bool);

      private void clubBindingSource_CurrentChanged(object sender, EventArgs e)
      {
         var CrntClub = clubBindingSource.Current as Data.Club;
         if(CrntClub == null) return;

         fighterBindingSource.DataSource =
            iScsc
            .Fighters
            .Where(f =>
               f.CONF_STAT == "002"
               && f.Fighter_Publics.First(p => p.RECT_CODE == "004" && p.RWNO == f.FGPB_RWNO_DNRM).Club == CrntClub
               //&& !f.Attendances.Any(a => DateTime.Compare(a.ATTN_DATE.Date, Pdt_AttnDate.DateTime.Date) == 0)
            );
         attendancesBindingSource.DataSource = iScsc.Attendances.Where(a => a.Club == CrntClub && DateTime.Compare(a.ATTN_DATE.Date, Pdt_AttnDate.DateTime.Date) == 0);
      }

      private void Tsb_SaveChange_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            attendancesBindingSource.EndEdit();
            attendancesBindingSource
               .List
               .OfType<Data.Attendance>()
               .Where(a => a.ATTN_DATE.Date.Year == 1)
               .ToList()
               .ForEach(a => { a.ATTN_DATE = Pdt_AttnDate.DateTime; a.CLUB_CODE = (long)ClubCode_ComboBox.SelectedValue; });

            iScsc.SubmitChanges();
            requeryData = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requeryData)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               var crntClub = clubBindingSource.Position;
               clubBindingSource.DataSource = iScsc.Clubs;
               clubBindingSource.Position = crntClub;
               requeryData = false;
            }
         }
      }
   }
}
