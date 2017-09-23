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

namespace System.CRM.Ui.CommonInformation
{
   public partial class LST_CUST_F : UserControl
   {
      public LST_CUST_F()
      {
         InitializeComponent();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void INF_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var serv = ServsBs1.Current as Data.Service;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 15 /* Execute Lst_Cust_F */),
                   new Job(SendType.SelfToUserInterface, "INF_CUST_F", 10 /* Execute Cal_Actnf_F */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))}
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }
   }
}
