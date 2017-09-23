using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.RequesterType
{
   public partial class RQTR_TYPE_F : UserControl
   {
      public RQTR_TYPE_F()
      {
         InitializeComponent();
      }

      partial void Btn_RqttSubmit_Click(object sender, EventArgs e);

      partial void Btn_RqttDel_Click(object sender, EventArgs e);
   }
}
