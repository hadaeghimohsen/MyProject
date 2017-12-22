using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;
using System.CRM.ExceptionHandlings;
using System.MaxUi;

namespace System.CRM.Ui.Activity
{
   public partial class OPT_MESG_F : UserControl
   {
      public OPT_MESG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long fileno;
      private bool needclose = true;
      private long rqstrqid, projrqstrqid;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => /*s.SRPB_TYPE_DNRM == srpbtype*/ s.FILE_NO == fileno && s.CONF_STAT == "002" && Convert.ToInt32(s.ONOF_TAG_DNRM) >= 101);
         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var mesg = MesgBs.Current as Data.Message;

            if (mesg.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  MesgBs.DataSource =
                     iCRM.Messages.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.MSID == iCRM.Messages.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.MSID));
                  requery = true;
               }
            }
            else
               requery = true;

            return requery;

         }
         catch { return false; }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Save_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            MesgBs.EndEdit();
            var mesg = MesgBs.Current as Data.Message;

            if (mesg == null) throw new Exception("خطا * شی پیامک خالی می باشد");
            if (mesg.SERV_FILE_NO == null || mesg.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده پیامک خالی می باشد"); }
            if (mesg.MESG_DATE == null) { Mesg_Date.Focus(); throw new Exception("خطا * تاریخ پیامک خالی می باشد"); }
            if (mesg.CELL_PHON == null || mesg.CELL_PHON == "") { CellPhon_Txt.Focus(); throw new Exception("خطا * شماره تلفن پیامک خالی می باشد"); }
            if (mesg.MESG_STAT == null || mesg.MESG_STAT == "") { MesgStat_Lov.Focus(); mesg.MESG_STAT = "003"; throw new Exception("خطا * وضعیت پیامک خالی می باشد"); }
            if (mesg.MESG_TYPE == null || mesg.MESG_TYPE == "") { MesgType_Lov.Focus(); throw new Exception("خطا * نوع پیامک خالی می باشد"); }
            if (mesg.MESG_CMNT == null || mesg.MESG_CMNT == "") { Comment_Txt.Focus(); throw new Exception("خطا * متن پیامک خالی می باشد"); }
            

            iCRM.OPR_GSAV_P(
               new XElement("Message",
                  new XAttribute("servfileno", mesg.SERV_FILE_NO),
                  new XAttribute("mesgdate", GetDateTimeString(mesg.MESG_DATE)),
                  new XAttribute("rqrorqstrqid", mesg.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", mesg.RQRO_RWNO ?? 0),
                  new XAttribute("colr", mesg.Request_Row != null ? mesg.Request_Row.Request.COLR : "#ADFF2F"),
                  new XAttribute("msid", mesg.MSID),
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("projrqstrqid", projrqstrqid),
                  new XAttribute("cellphon", mesg.CELL_PHON),
                  new XAttribute("mesgstat", mesg.MESG_STAT ?? "003"),
                  new XAttribute("mesgtype", mesg.MESG_TYPE),
                  new XAttribute("sendstat", mesg.SEND_STAT ?? "001"),
                  new XElement("Comment", mesg.MESG_CMNT)                  
               )
            );

            // 1396/03/22 * بروز رسانی فرم اصلی برای یادآوری ها
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 41 /* Execute SetNotification */){Executive = ExecutiveType.Asynchronous},
                     new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 42 /* Execute SetMention */){Executive = ExecutiveType.Asynchronous}
                  }
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery && needclose)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno)) }
               );
               Btn_Back_Click(null, null);
            }
         }
      }      

      private string GetDateTimeString(DateTime? dt)
      {
         return 
            string.Format("{0}-{1}-{2} {3}:{4}:{5}",
               dt.Value.Year,
               dt.Value.Month,
               dt.Value.Day,
               dt.Value.Hour,
               dt.Value.Minute,
               dt.Value.Second
            );
      }

      private void SendNow_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = MesgBs.Current as Data.Message;
            mesg.SEND_STAT = "002";

            needclose = false;
            Save_Butn_Click(null, null);
            needclose = true;
            if (mesg.RQRO_RQST_RQID == null)
            {
               iCRM = new Data.iCRMDataContext(ConnectionString);
               MesgBs.DataSource =
                  iCRM.Messages.FirstOrDefault(ms =>
                     ms.SERV_FILE_NO == fileno &&
                     ms.MSID == iCRM.Messages.Where(msg => msg.SERV_FILE_NO == fileno && msg.CRET_BY.ToUpper() == CurrentUser.ToUpper()).Max(msg => msg.MSID));
            }
            requery = true;
         }
         catch (Exception exc)
         {
            var mesg = MesgBs.Current as Data.Message;
            mesg.SEND_STAT = "001";

            iCRM.SaveException(exc);
         }
         finally
         {
            if(requery)
            {
               var mesg = MesgBs.Current as Data.Message;

               try
               {
                  iCRM.MSG_SEND_P(
                     new XElement("Process",
                        new XElement("Contacts",
                           new XAttribute("subsys", 11),
                           new XAttribute("linetype", "001"),
                           new XElement("Contact",
                              new XAttribute("phonnumb", mesg.CELL_PHON),
                              new XElement("Message",
                                 new XAttribute("rfid", mesg.MSID),
                                 new XAttribute("type", "003"),
                                 mesg.MESG_CMNT
                              )
                           )
                        )
                     )
                  );
               }
               catch (Exception exc)
               {
                  MessageBox.Show(exc.Message);
               }
            }
         }
      }

      private void CallRequest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var mesg = MesgBs.Current as Data.Message;            

            switch (butn.Tag.ToString())
            {
               case "002":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 56 /* Execute Opt_Rqst_F */),
                           new Job(SendType.SelfToUserInterface, "OPT_RQST_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input = 
                                 new XElement("Request",
                                    new XAttribute("fileno", mesg.SERV_FILE_NO),
                                    new XAttribute("rqid", mesg.RQRO_RQST_RQID)
                                 )
                           }                     
                        }
                     )
                  );
                  break;
               case "001":
                  Btn_Back_Click(null, null);

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 57 /* Execute Lst_Serv_F */),
                           new Job(SendType.SelfToUserInterface, "LST_SERV_F", 10 /* Execute Actn_Calf_F */)
                           {
                              Input = 
                                 new XElement("Request",
                                    new XAttribute("fileno", ""),
                                    new XAttribute("rqid", mesg.RQRO_RQST_RQID),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }                     
                        }
                     )
                  );
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void Finr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var mesg = MesgBs.Current as Data.Message;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 59 /* Execute Fin_Rslt_F */),
                     new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Final_Result",                              
                              new XAttribute("rqid", mesg.RQRO_RQST_RQID)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void InfoServ_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 101 /* Execute ShowServiceInfo */)
                  {
                     Input = 
                        new XElement("Service", 
                           new XAttribute("fileno", fileno),
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               }
            )
         );
      }

      private void TemplateText_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Btn_Back_Click(null, null);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 69 /* Execute Tmpl_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "TMPL_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("Request",
                              new XAttribute("formcaller", GetType().Name),
                              new XAttribute("fileno", fileno)
                           )
                     }
                  }
               )
            );
         }
         catch { }
      }

      private void UserMentioned_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 72 /* Execute Sjbp_Dfin_F */),
                  new Job(SendType.SelfToUserInterface, "SJBP_DFIN_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Mentioned",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("section", "message")
                        )
                  }
               }
            )
         );
      }

      private void MessageCheckStat_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = MesgBs.Current as Data.Message;
            if(mesg == null)return;

            var _MessageCheckStat =
               new Job(SendType.External, "localhost", "Commons:Program:Msgb", 04 /* Execute Mesg_Chks_P */, SendType.Self)
               {
                  Input =
                     new XElement("Message",
                        new XAttribute("subsys", 11),
                        new XAttribute("rfid", mesg.MSID)
                     )
               };
            _DefaultGateway.Gateway(_MessageCheckStat);


         }catch(Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void MessageRecieve_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = MesgBs.Current as Data.Message;
            if (mesg == null) return;
            if (mesg.CELL_PHON == null) return;

            var _MessageRecieve =
               new Job(SendType.External, "localhost", "Commons:Program:Msgb", 05 /* Execute Mesg_Recv_P */, SendType.Self)
               {
                  Input =
                     new XElement("Message",
                        new XAttribute("subsys", 11),
                        new XAttribute("cellphon", mesg.CELL_PHON)
                     )
               };
            _DefaultGateway.Gateway(_MessageRecieve);


         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void roundedButton1_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = MesgBs.Current as Data.Message;
            if (mesg == null) return;
            if (mesg.CELL_PHON == null) return;

            var _MessageRecieve =
               new Job(SendType.External, "localhost", "Commons:Program:Msgb", 06 /* Execute Tree_Node_P */, SendType.Self)
               {
                  Input =
                     new XElement("Message",
                        new XAttribute("subsys", 11),
                        new XAttribute("cellphon", mesg.CELL_PHON)
                     )
               };
            _DefaultGateway.Gateway(_MessageRecieve);


         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void RqstFolw_Butn_Click(object sender, EventArgs e)
      {
         var xinput = (sender as RoundedButton).Tag as XElement;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("Request",
                     new XAttribute("rqtpcode", xinput.Attribute("rqtpcode").Value),
                     new XAttribute("rqid", xinput.Attribute("rqid").Value)
                  )
            }
         );
      }

      private void MesgBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = MesgBs.Current as Data.Message;
            if (rqst == null ) { RqstFolw_Butn.Visible = false; return; }

            if (rqst.Request_Row != null && rqst.Request_Row.Request.RQST_RQID != null)
            {
               RqstFolw_Butn.Visible = true;
               RqstFolw_Butn.Tooltip = string.Format("درخواست پیرو {0}", rqst.Request_Row.Request.Request1.Request_Type.RQTP_DESC);
               RqstFolw_Butn.Tag =
                  new XElement("Request", new XAttribute("rqtpcode", rqst.Request_Row.Request.Request1.RQTP_CODE), new XAttribute("rqid", rqst.Request_Row.Request.Request1.RQID));
            }
            else
            {
               RqstFolw_Butn.Visible = false;
            }

            if (rqst.Request_Row != null)
            {
               rqst.Request_Row.Request.COLR = rqst.Request_Row.Request.COLR == null ? "#ADFF2F" : rqst.Request_Row.Request.COLR;
               SelectColor_Butn.NormalColorA = SelectColor_Butn.NormalColorB = SelectColor_Butn.HoverColorA = SelectColor_Butn.HoverColorB = ColorTranslator.FromHtml(rqst.Request_Row.Request.COLR);
            }

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SelectColor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var mesg = MesgBs.Current as Data.Message;

            if (mesg.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  MesgBs.DataSource =
                     iCRM.Messages.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.MSID == iCRM.Messages.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.MSID));
                  requery = true;
               }
            }
            else
               requery = true;
         }
         catch { }
         finally
         {
            if (requery)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 48 /* Execute Tsk_Colr_F */),
                        new Job(SendType.SelfToUserInterface, "TSK_COLR_F", 10 /* Execute Actn_Calf_P */)
                        {
                            Input = 
                              new XElement("Request", 
                                 //new XAttribute("fileno", task.SERV_FILE_NO), 
                                 //new XAttribute("tkid", task.TKID),
                                 //new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         }
                     }
                  )
               );
            }
         }
      }
   }
}
