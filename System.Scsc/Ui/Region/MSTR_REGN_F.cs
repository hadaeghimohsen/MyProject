using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Region
{
   public partial class MSTR_REGN_F : UserControl
   {
      public MSTR_REGN_F()
      {
         InitializeComponent();
      }

      partial void SubmitChange_Cnty_Click(object sender, EventArgs e);

      partial void SubmitChange_Prvn_Click(object sender, EventArgs e); 
      
      partial void SubmitChange_Regn_Click(object sender, EventArgs e);

      partial void Delete_Cnty_Click(object sender, EventArgs e);

      partial void Delete_Prvn_Click(object sender, EventArgs e);

      partial void Delete_Regn_Click(object sender, EventArgs e);
   }
}
