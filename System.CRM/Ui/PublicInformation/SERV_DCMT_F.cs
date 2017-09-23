using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.CRM.Ui.PublicInformation
{
   public partial class SERV_DCMT_F : UserControl
   {
      public SERV_DCMT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private long fileno;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         vRqdcBs.DataSource = iCRM.VF_Request_Document(fileno);
         requery = false;
      }      

      private void ShowRecieveDocument_Butn_Click(object sender, EventArgs e)
      {
         if (vRqdcBs.Current == null) return;

         var CrntRqstDcmt = vRqdcBs.Current as Data.VF_Request_DocumentResult;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {
                 new Job(SendType.Self, 40 /* Execute Cmn_Dcmt_F */),
                 new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_Calf_F */){ Input = iCRM.Request_Rows.Where(rr => rr.RQST_RQID == CrntRqstDcmt.RQID && rr.RWNO == 1).Single() }
              })

         );
      }

   }
}
