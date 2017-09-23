using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.RequestType
{
   public partial class RQST_TYPE_F : UserControl
   {
      public RQST_TYPE_F()
      {
         InitializeComponent();
      }

      partial void Btn_RqstType_Submit_Click(object sender, EventArgs e);

      partial void Btn_ResnSubmit_Click(object sender, EventArgs e);

      partial void Btn_DelResn_Click(object sender, EventArgs e);
      
      
   }
}
