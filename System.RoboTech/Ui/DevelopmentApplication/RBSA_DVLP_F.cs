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

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class RBSA_DVLP_F : UserControl
   {
      public RBSA_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int srbt = SrbtBs.Position;
         int srbtmsg = SrbtMsgBs.Position;
         int srrm = SrrmBs.Position;
         int srrmcopy = SrrmCopyBs.Position;
         int srmgcopy = SrmgCopyBs.Position;
         int sdad = SdadBs.Position;
         int srsa = SrsaBs.Position;
         
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         SrbtBs.Position = srbt;
         SrbtMsgBs.Position = srbtmsg;
         SrrmBs.Position = srrm;
         SrrmCopyBs.Position = srrmcopy;
         SrmgCopyBs.Position = srmgcopy;
         SdadBs.Position = sdad;
         SrsaBs.Position = srsa;
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Invalidate();

            SrbtBs.EndEdit();
            SdadBs.EndEdit();

            iRoboTech.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void SrrmBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srrm = SrrmBs.Current as Data.Service_Robot_Replay_Message;

            switch (srrm.SEND_STAT)
            {
               case "003":
                  SRRM_MESG_TEXT_MemTxt.Properties.ReadOnly = false;
                  break;
               default:
                  SRRM_MESG_TEXT_MemTxt.Properties.ReadOnly = true;
                  break;
            }
         }
         catch (Exception exc)
         {
            ExceptionLogs.SaveException(iRoboTech, exc);
         }
      }

      private void SrrmCopyBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srrm = SrrmCopyBs.Current as Data.Service_Robot_Replay_Message;

            if (srrm == null) return;

            //SrmgCopyBs.List.Clear();
            SrmgCopyBs.DataSource = iRoboTech.Service_Robot_Messages.Where(s => s.Service_Robot == srrm.Service_Robot && s.RWNO == srrm.SRMG_RWNO);


            switch (srrm.SEND_STAT)
            {
               case "003":
                  SRRM_MESG_TEXT1_MemTxt.Properties.ReadOnly = false;
                  break;
               default:
                  SRRM_MESG_TEXT_MemTxt.Properties.ReadOnly = true;
                  break;
            }
         }
         catch (Exception exc)
         {
            ExceptionLogs.SaveException(iRoboTech, exc);
         }
      }

      private void Tsb_DelSrrm_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var srrm = SrrmBs.Current as Data.Service_Robot_Replay_Message;

            iRoboTech.DEL_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tsb_SubmitChangeSrrm_Click(object sender, EventArgs e)
      {
         try
         {
            SrrmBs.EndEdit();
            // if need insert data
            foreach (var srrm in SrrmBs.List.OfType<Data.Service_Robot_Replay_Message>().Where(s => s.CRET_BY == null))
            {
               iRoboTech.INS_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO, srrm.SRMG_RWNO, null, null, srrm.MESG_TEXT, srrm.FILE_ID, srrm.FILE_PATH, srrm.MESG_TYPE, srrm.LAT, srrm.LON, srrm.CONT_CELL_PHON);
            }

            // if need updated data
            foreach (var srrm in SrrmBs.List.OfType<Data.Service_Robot_Replay_Message>().Where(s => s.CRET_BY != null))
            {
               iRoboTech.UPD_SRRM_P(srrm.SRBT_SERV_FILE_NO, srrm.SRBT_ROBO_RBID, srrm.RWNO, srrm.SRMG_RWNO, srrm.MESG_TEXT, srrm.SEND_STAT, srrm.FILE_ID, srrm.FILE_PATH, srrm.MESG_TYPE, srrm.LAT, srrm.LON, srrm.CONT_CELL_PHON);
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
               requery = false;
            }
         }
      }

      private void Tsb_ReadyToSend_Click(object sender, EventArgs e)
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
            Tsb_SubmitChangeSrrm_Click(null, null);
         }
         catch (Exception exc)
         {
            ExceptionLogs.SaveException(iRoboTech, exc);
         }
      }

      private void Tsb_DelSdad_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var sdad = SdadBs.Current as Data.Send_Advertising;

            iRoboTech.DEL_SDAD_P(sdad.ROBO_RBID, sdad.ID);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tsb_ReadyToSendAdv_Click(object sender, EventArgs e)
      {
         try
         {
            var srrm = SdadBs.Current as Data.Send_Advertising;

            if (srrm == null) return;
            switch (srrm.STAT)
            {
               case "001":
               case "002":
               case "003":
                  srrm.STAT = "005";
                  break;
               default:
                  MessageBox.Show(this, "پیام های ارسال شده قادر به تغییر نیستند");
                  return;
            }
            iRoboTech.SubmitChanges();
         }
         catch (Exception exc)
         {
            ExceptionLogs.SaveException(iRoboTech, exc);
         }
      }
   }
}
