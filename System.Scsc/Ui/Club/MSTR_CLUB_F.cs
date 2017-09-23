using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Club
{
   public partial class MSTR_CLUB_F : UserControl
   {
      public MSTR_CLUB_F()
      {
         InitializeComponent();
      }

      partial void clubsBindingSource_PositionChanged(object sender, EventArgs e);

      partial void Btn_SubmitClub_Click(object sender, EventArgs e);

      partial void Btn_DelClub_Click(object sender, EventArgs e);

      partial void Btn_SubmitClubMethod_Click(object sender, EventArgs e);
   }
}
