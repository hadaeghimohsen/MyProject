using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Base;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Regulation
{
   public partial class MSTR_REGL_F : UserControl
   {
      public MSTR_REGL_F()
      {
         InitializeComponent();
      }

      partial void ReglSaveItem_Click(object sender, EventArgs e);

      partial void Btn_InsRegl_Click(object sender, EventArgs e);

      partial void Btn_SubmitInsRegl_Click(object sender, EventArgs e);

      partial void Btn_CnclInsRegl_Click(object sender, EventArgs e);

      partial void HL_INVSREGL_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      partial void DEL_REGL_Click(object sender, EventArgs e);

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
