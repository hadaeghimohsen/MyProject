using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Method
{
   public partial class MSTR_MTOD_F : UserControl
   {
      public MSTR_MTOD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      partial void Execute_Query();      

      partial void SubmitChange_Mtod_Click(object sender, EventArgs e);

      partial void SubmitChange_Ctgy_Click(object sender, EventArgs e);

      partial void SubmitChange_Dsct_Click(object sender, EventArgs e);

      partial void DelCtgy_Butn_Click(object sender, EventArgs e);

      partial void DelMtod_Butn_Click(object sender, EventArgs e);
      
      
      
   }
}
