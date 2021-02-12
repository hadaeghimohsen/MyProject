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

namespace MyProject.Commons.ErrorHandling.Ui
{
   public partial class ShowMessage : UserControl
   {
      public ShowMessage()
      {
         InitializeComponent();
      }

      private void Submit_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      int waitBlock = 0;
      private void Waiting_Tm_Tick(object sender, EventArgs e)
      {
         if (waitBlock == 0)
         {
            ShowMessage_Lbl.Text = ShowMessage_Lbl.Tag.ToString() + " .";
            ++waitBlock;
         }
         else if (waitBlock <= 3)
         {
            ShowMessage_Lbl.Text = ShowMessage_Lbl.Text + ".";
            ++waitBlock;
         }
         else
            waitBlock = 0;
      }
   }
}
