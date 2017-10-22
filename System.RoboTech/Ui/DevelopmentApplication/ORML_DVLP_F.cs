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
using System.Xml.Linq;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ORML_DVLP_F : UserControl
   {
      public ORML_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? servFileNo = 0;
      private long? roboRbid = 0;
      private long? ordtOrdrCode = 0;
      private long? ordtRwno = 0;
      private long? srmgrwno = 0;

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
            .Where(
               m => 
                  m.SRBT_SERV_FILE_NO == servFileNo &&
                  m.SRBT_ROBO_RBID == roboRbid &&
                  ( (ordtOrdrCode != 0 ? (m.ORDT_ORDR_CODE == ordtOrdrCode && m.ORDT_RWNO == ordtRwno) : true) &&
                    (srmgrwno != 0 ? (m.SRMG_RWNO == srmgrwno ) : true) )
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
               iRoboTech.INS_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO, srrm.SRMG_RWNO, ordtOrdrCode, ordtRwno, srrm.MESG_TEXT ?? "",srrm.FILE_ID, srrm.FILE_PATH, srrm.MESG_TYPE, srrm.LAT, srrm.LON, srrm.CONT_CELL_PHON);
            }

            // if need updated data
            foreach (var srrm in SrrmBs.List.OfType<Data.Service_Robot_Replay_Message>().Where(s => s.CRET_BY != null))
            {
               iRoboTech.UPD_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO, srrm.SRMG_RWNO, srrm.MESG_TEXT, srrm.SEND_STAT,srrm.FILE_ID, srrm.FILE_PATH, srrm.MESG_TYPE, srrm.LAT, srrm.LON, srrm.CONT_CELL_PHON);
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
               Execute_Query();
            }
         }
      }

      private void MesgInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = SrrmBs.Current as Data.Service_Robot_Replay_Message;
            if (mesg == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 17 /* Execute Odrm_Dvlp_F */),
                     new Job(SendType.SelfToUserInterface, "ODRM_DVLP_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input =
                           new XElement("Service_Robot_Message",
                              new XAttribute("servfileno", mesg.SRBT_SERV_FILE_NO),
                              new XAttribute("roborbid", mesg.SRBT_ROBO_RBID),
                              new XAttribute("srmgrwno", mesg.SRMG_RWNO ?? 0),
                              new XAttribute("srrmrwno", mesg.RWNO),
                              //new XAttribute("ordtordrcode", mesg.ORDT_ORDR_CODE ?? 0),
                              //new XAttribute("ordtrwno", mesg.ORDT_RWNO ?? 0),
                              new XAttribute("type", "edit")
                           )
                     }
                  }
               )
            );
         }
         catch (Exception)
         {

            throw;
         }
      }
   }
}
