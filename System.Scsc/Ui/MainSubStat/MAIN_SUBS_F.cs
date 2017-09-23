using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.MainSubStat
{
   public partial class MAIN_SUBS_F : UserControl
   {
      public MAIN_SUBS_F()
      {
         InitializeComponent();
      }

      partial void main_StateBindingNavigatorSaveItem_Click(object sender, EventArgs e);
      partial void Btn_SubStat_Submit_Click(object sender, EventArgs e);

      partial void Btn_DelMstt_Click(object sender, EventArgs e);
      partial void Btn_DelSstt_Click(object sender, EventArgs e);
   }
}
