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
using DevExpress.XtraEditors.Controls;

namespace System.Scsc.Ui.Regulation
{
   public partial class REGL_EXPN_F : UserControl
   {
      public REGL_EXPN_F()
      {
         InitializeComponent();
      }

      partial void SubmitChange_Click(object sender, EventArgs e);

      partial void Btn_Epit_Click(object sender, EventArgs e);

      partial void Btn_ReglDcmt_Click(object sender, EventArgs e);

      partial void Bt_INDP_Click(object sender, EventArgs e);

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void CL_MTOD_EditValueChanged(object sender, EventArgs e)
      {
         if (CL_MTOD.Properties.GetItems().OfType<CheckedListBoxItem>().Where(r => r.CheckState == CheckState.Checked).Count() == 1)
         {
            CL_CTGY.Enabled = true;
            var mtod = CL_MTOD.Properties.GetItems().OfType<CheckedListBoxItem>().Where(r => r.CheckState == CheckState.Checked).FirstOrDefault();
            CtgyBs1.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long?)mtod.Value);
         }
         else
         {
            CL_CTGY.Enabled = false;
         }
      }

      
   }
}
