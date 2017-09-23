using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Regulation
{
   public partial class MSTR_DCMT_F : UserControl
   {
      public MSTR_DCMT_F()
      {
         InitializeComponent();
      }

      partial void document_SpecBindingNavigatorSaveItem_Click(object sender, EventArgs e);

      partial void Btn_AddNewItem_Click(object sender, EventArgs e);

      partial void Btn_Cancel_Click(object sender, EventArgs e);

      partial void Btn_SubmitChange_Click(object sender, EventArgs e);

      partial void LV_UPDEL_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }
   }
}
