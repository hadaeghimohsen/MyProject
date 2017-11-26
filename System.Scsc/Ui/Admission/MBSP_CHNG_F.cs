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
using System.IO;

namespace System.Scsc.Ui.Admission
{
   public partial class MBSP_CHNG_F : UserControl
   {
      public MBSP_CHNG_F()
      {
         InitializeComponent();
      }

      bool requery = false;      
      bool checkOK = true;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);         
         requery = false;
      }      
   }
}
