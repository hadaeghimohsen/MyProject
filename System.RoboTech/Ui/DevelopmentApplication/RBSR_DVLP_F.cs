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
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class RBSR_DVLP_F : UserControl
   {
      public RBSR_DVLP_F()
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
         int srup = SrupBs.Position;
         
         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         SrbtBs.Position = srbt;
         SrbtMsgBs.Position = srbtmsg;
         SrrmBs.Position = srrm;
         SrrmCopyBs.Position = srrmcopy;
         SrmgCopyBs.Position = srmgcopy;
         SrupBs.Position = srup;
      }

      private void Tsb_SubmitChange_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Invalidate();

            SrbtBs.EndEdit();
            //SrrmBs.EndEdit();

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
            foreach (var srrm in SrrmBs.List.OfType<Data.Service_Robot_Replay_Message>().Where(s => s.CRET_BY != null && s.SEND_STAT != "004"))
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

      private void SearchReciveMessage_Butn_Click(object sender, EventArgs e)
      {
          try
          {
              /*if (MaxLenMesg_Txt.Text == "") { MaxLenMesg_Txt.Text = "1"; }
              if (SaveOrdr_Lov.Text == "") { SaveOrdr_Lov.Text = "خیر"; }*/              

              var srbt = SrbtBs.Current as Data.Service_Robot;
              if(srbt == null)return;

              if(MaxLenMesg_Txt.Text == "" && SaveOrdr_Lov.Text == "")
              {
                  SrbtMsgBs.DataSource =
                      iRoboTech.Service_Robot_Messages.
                      Where(srm => srm.Service_Robot == srbt);
                  return;
              }
              else if(MaxLenMesg_Txt.Text != "" && SaveOrdr_Lov.Text == "")
              {
                  SrbtMsgBs.DataSource =
                      iRoboTech.Service_Robot_Messages.
                      Where(srm => srm.Service_Robot == srbt && srm.MESG_TEXT.Length >= Convert.ToInt32(MaxLenMesg_Txt.Text));
                  return;
              }
              else if (MaxLenMesg_Txt.Text == "" && SaveOrdr_Lov.Text != "")
              {
                  SrbtMsgBs.DataSource =
                  iRoboTech.Service_Robot_Messages.
                      Where(srm =>                          
                          (SaveOrdr_Lov.Text == "بلی" || !iRoboTech.Orders.Any(o => o.Service_Robot == srbt && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT))) &&
                          (SaveOrdr_Lov.Text == "خیر" || iRoboTech.Orders.Any(o => o.Service_Robot == srbt && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT)))
                      );
                  return;
              }

              SrbtMsgBs.DataSource =
                  iRoboTech.Service_Robot_Messages.
                  Where(srm => 
                      srm.MESG_TEXT.Length >= Convert.ToInt32(MaxLenMesg_Txt.Text) &&
                      (SaveOrdr_Lov.Text == "بلی" || !iRoboTech.Orders.Any( o => o.Service_Robot == srbt && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT))) &&
                      (SaveOrdr_Lov.Text == "خیر" || iRoboTech.Orders.Any(o => o.Service_Robot == srbt && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT)))
                  );
          }
          catch (Exception exc)
          {
              MessageBox.Show(exc.Message);
              iRoboTech.SaveException(exc);
          }
      }

      private void SrbtBs_CurrentChanged(object sender, EventArgs e)
      {
          SearchReciveMessage_Butn_Click(null, null);
      }

      private void TotalSearch_Butn_Click(object sender, EventArgs e)
      {
          try
          {
              /*if (MaxLenMesg_Txt.Text == "") { MaxLenMesg_Txt.Text = "1"; }
              if (SaveOrdr_Lov.Text == "") { SaveOrdr_Lov.Text = "خیر"; }*/

              if (MaxLenMesg_Txt.Text == "" && SaveOrdr_Lov.Text == "")
              {
                  SrbtMsgBs.DataSource =
                      iRoboTech.Service_Robot_Messages;                      
                  return;
              }
              else if (MaxLenMesg_Txt.Text != "" && SaveOrdr_Lov.Text == "")
              {
                  SrbtMsgBs.DataSource =
                      iRoboTech.Service_Robot_Messages.
                      Where(srm => srm.MESG_TEXT.Length >= Convert.ToInt32(MaxLenMesg_Txt.Text));
                  return;
              }
              else if (MaxLenMesg_Txt.Text == "" && SaveOrdr_Lov.Text != "")
              {
                  SrbtMsgBs.DataSource =
                  iRoboTech.Service_Robot_Messages.
                      Where(srm =>
                          (SaveOrdr_Lov.Text == "بلی" || !iRoboTech.Orders.Any(o => o.Service_Robot == srm.Service_Robot && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT))) &&
                          (SaveOrdr_Lov.Text == "خیر" || iRoboTech.Orders.Any(o => o.Service_Robot == srm.Service_Robot && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT)))
                      );
                  return;
              }

              SrbtMsgBs.DataSource =
                  iRoboTech.Service_Robot_Messages.
                  Where(srm =>
                      srm.MESG_TEXT.Length >= Convert.ToInt32(MaxLenMesg_Txt.Text) &&
                      (SaveOrdr_Lov.Text == "بلی" || !iRoboTech.Orders.Any(o => o.Service_Robot == srm.Service_Robot && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT))) &&
                      (SaveOrdr_Lov.Text == "خیر" || iRoboTech.Orders.Any(o => o.Service_Robot == srm.Service_Robot && o.Order_Details.Any(od => od.ORDR_DESC == srm.MESG_TEXT)))
                  );
          }
          catch (Exception exc)
          {
              MessageBox.Show(exc.Message);
              iRoboTech.SaveException(exc);
          }
      }

      private void SaveOrder_Butn_Click(object sender, EventArgs e)
      {
          try
          {
              var v_Result = new XElement("Result");
              if (CheckAll_Ckb.Checked)
              {
                  SrbtMsgBs.List.OfType<Data.Service_Robot_Message>().
                      ToList().
                      ForEach(srm =>
                          iRoboTech.Analisis_Message_P(
                            new XElement("Robot",
                                new XAttribute("token", srm.Service_Robot.Robot.TKON_CODE),
                                new XElement("Message",
                                    new XAttribute("ussd", UssdCode_Txt.Text),
                                    new XAttribute("chatid", srm.CHAT_ID),
                                    new XAttribute("elmntype", "001"),
                                    new XAttribute("mesgid", srm.MESG_ID),
                                    new XElement("Text", srm.MESG_TEXT)
                                )
                            ),
                            ref v_Result
                          )
                      );
              }
              else
              {
                  var srm = SrbtMsgBs.Current as Data.Service_Robot_Message;
                  iRoboTech.Analisis_Message_P(
                    new XElement("Robot",
                        new XAttribute("token", srm.Service_Robot.Robot.TKON_CODE),
                        new XElement("Message",
                            new XAttribute("ussd", UssdCode_Txt.Text),
                            new XAttribute("chatid", srm.CHAT_ID),
                            new XAttribute("elmntype", "001"),
                            new XAttribute("mesgid", srm.MESG_ID),
                            new XElement("Text", srm.MESG_TEXT)
                        )
                        ),
                        ref v_Result
                    );
              }

              requery = true;
          }
          catch (Exception exc)
          {
              MessageBox.Show(exc.Message);
              iRoboTech.SaveException(exc);
          }
          finally {
              if (requery)
              {
                  TotalSearch_Butn_Click(null, null);
              }
          }
      }

      private void SendReplayMessage_Butn_Click(object sender, EventArgs e)
      {
         var sm = SrbtMsgBs.Current as Data.Service_Robot_Message;
         if (sm == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 17 /* Execute Odrm_Dvlp_F */),
                  new Job(SendType.SelfToUserInterface, "ODRM_DVLP_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input =
                        new XElement("Service_Robot_Message",
                           new XAttribute("servfileno", sm.SRBT_SERV_FILE_NO),
                           new XAttribute("roborbid", sm.SRBT_ROBO_RBID),
                           new XAttribute("srmgrwno", sm.RWNO),
                           new XAttribute("type", "new")
                        )
                  }
               }
            )
         );
      }

      private void ListSends_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var sm = SrbtMsgBs.Current as Data.Service_Robot_Message;
            if (sm == null) return;

            _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 18 /* Execute Orml_Dvlp_F */),
                        new Job(SendType.SelfToUserInterface, "ORML_DVLP_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input =
                              new XElement("Order_Detail",
                                 new XAttribute("servfileno", sm.SRBT_SERV_FILE_NO),
                                 new XAttribute("roborbid", sm.SRBT_ROBO_RBID),
                                 new XAttribute("srmgrwno", sm.RWNO)
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

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var srbt = SrbtBs.Current as Data.Service_Robot;
         if (srbt == null) return;

         switch(e.Button.Index)
         {
            case 0:
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 20 /* Execue Srbt_Info_F */),
                        new Job(SendType.SelfToUserInterface, "SRBT_INFO_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("Service_Robot",
                                 new XAttribute("servfileno", srbt.SERV_FILE_NO),
                                 new XAttribute("roborbid", srbt.ROBO_RBID)
                              )
                        }
                     }
                  )
               );                  
               break;
            case 1:
               var robot = RoboBs.Current as Data.Robot;
               if (robot == null || robot.UP_LOAD_FILE_PATH == null || !Directory.Exists(robot.UP_LOAD_FILE_PATH)) return;               

               var serv = SrbtBs.Current as Data.Service_Robot;
               if (serv == null) return;
               if (!Directory.Exists(robot.UP_LOAD_FILE_PATH + @"\" + serv.CHAT_ID.ToString()))
                  Directory.CreateDirectory(robot.UP_LOAD_FILE_PATH + @"\" + serv.CHAT_ID.ToString());
               
               // Open Service's Share Folder
               Process.Start("explorer.exe", robot.UP_LOAD_FILE_PATH + @"\" + serv.CHAT_ID.ToString());

               break;
         }
      }
   }
}
