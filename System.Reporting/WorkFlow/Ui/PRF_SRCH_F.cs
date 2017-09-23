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

namespace System.Reporting.WorkFlow.Ui
{
   public partial class PRF_SRCH_F : UserControl
   {
      public PRF_SRCH_F()
      {
         InitializeComponent();
      }

      partial void be_search_EditValueChanged(object sender, EventArgs e);

      partial void sb_filtertype_Click(object sender, EventArgs e);

      private void FaKeyboard()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* LangChangToFarsi */, SendType.Self));
      }

      private void EnKeyboard()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 09 /* LangChangToEnglish */, SendType.Self));
      }
   }
}
