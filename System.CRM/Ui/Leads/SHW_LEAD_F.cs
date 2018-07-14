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
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;

namespace System.CRM.Ui.Leads
{
   public partial class SHW_LEAD_F : UserControl
   {
      public SHW_LEAD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string onoftag;
      private long compcode = 0;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
      }

      private void New_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),
                new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Lead",
                        new XAttribute("formcaller", GetType().Name),
                        new XAttribute("type", "newlead")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
   }
}
