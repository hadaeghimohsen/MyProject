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

namespace System.CRM.Ui.Notification
{
   public partial class TASK_FLOW_F : UserControl
   {
      public TASK_FLOW_F()
      {
         InitializeComponent();
      }

      private void Execute_Query()
      { 
         iCRM = new Data.iCRMDataContext(ConnectionString);
         RqstBs1.DataSource =
            iCRM.Requests
            .Where(r => 
               r.RQST_STAT == "001" &&
               r.Job_Personnel.USER_NAME == CurrentUser
            );
      }

      private void ACTN_BUTN_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var rqst = RqstBs1.Current as Data.Request;
         if (rqst == null) return;
         string gatewayCall = "";
         int methodCall = 0;

         switch (rqst.RQTP_CODE)
         {
            case "005":
               gatewayCall = "ACT_LOGC_F";
               methodCall = 17;
               break;
            case "006":
               gatewayCall = "ACT_SNDF_F";
               methodCall = 18;
               break;
            case "007":
               gatewayCall = "ACT_TRAT_F";
               methodCall = 19;
               break;
            default:
               break;
         }

         switch (e.Button.Index)
         {
            case 0:
               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                    {                  
                      new Job(SendType.Self, methodCall /* Execute methodCall */),                
                      new Job(SendType.SelfToUserInterface, gatewayCall, 10 /* Execute Actn_CalF_F */)
                      {
                         Input = new XElement("Request", new XAttribute("rqid", rqst.RQID), new XAttribute("type", "find"))
                      },
                    });
               _DefaultGateway.Gateway(_InteractWithCRM);
               break;
            default:
               break;
         }
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Refresh_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }
   }
}
