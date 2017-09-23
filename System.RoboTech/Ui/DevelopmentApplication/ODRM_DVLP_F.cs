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
using System.RoboTech.ExceptionHandlings;
using DevExpress.XtraEditors;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ODRM_DVLP_F : UserControl
   {
      public ODRM_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long srrmRwno = 0;
      private long? servFileNo = 0;
      private long? roboRbid = 0;
      private long? ordtOrdrCode = 0;
      private long? ordtRwno = 0;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         SrrmBs.DataSource = 
            iRoboTech.Service_Robot_Replay_Messages
            .FirstOrDefault(
               m => 
                  m.SRBT_SERV_FILE_NO == servFileNo &&
                  m.SRBT_ROBO_RBID == roboRbid &&
                  (ordtOrdrCode != 0 ? (m.ORDT_ORDR_CODE == ordtOrdrCode && m.ORDT_RWNO == ordtRwno) : true)
             );
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            Invalidate();
            SrrmBs.EndEdit();
            // if need insert data
            foreach (var srrm in SrrmBs.List.OfType<Data.Service_Robot_Replay_Message>().Where(s => s.CRET_BY == null))
            {
               iRoboTech.INS_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO, srrm.SRMG_RWNO, ordtOrdrCode, ordtRwno, srrm.MESG_TEXT ?? "");
            }

            // if need updated data
            foreach (var srrm in SrrmBs.List.OfType<Data.Service_Robot_Replay_Message>().Where(s => s.CRET_BY != null))
            {
               iRoboTech.UPD_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO, srrm.SRMG_RWNO, srrm.MESG_TEXT, srrm.SEND_STAT);
            }
            requery = true;
         }
         catch (Exception exc)
         {
            ExceptionLogs.SaveException(iRoboTech, exc);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }


      private void SendInQeueMesg_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srrm = SrrmBs.Current as Data.Service_Robot_Replay_Message;

            if (srrm == null) return;
            switch (srrm.SEND_STAT)
            {
               case "001":
               case "002":
               case "003":
                  srrm.SEND_STAT = "005";
                  break;
               default:
                  MessageBox.Show(this, "پیام های ارسال شده قادر به تغییر نیستند");
                  return;
            }
            Tsb_SubmitChange_Click(null, null);
            requery = true;
         }
         catch (Exception exc)
         {
            ExceptionLogs.SaveException(iRoboTech, exc);
         }
         finally
         {
            if(requery)
            {
               Btn_Back_Click(null, null);
               requery = false;
            }
         }
      }
   }
}
