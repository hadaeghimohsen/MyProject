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
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   public partial class PRF_CHNG_F : UserControl
   {
      public PRF_CHNG_F()
      {
         InitializeComponent();
      }

      partial void wbp_submit_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);

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

      partial void be_profiler_name_Enter(object sender, EventArgs e);

      private void be_profiler_name_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Tag.ToString())
         {
            case "0":
               be_profiler_name.Text = "";
               break;
            default:
               break;
         }
      }
      
   }
}
