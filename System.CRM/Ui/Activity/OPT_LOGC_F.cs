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
   public partial class OPT_LOGC_F : UserControl
   {
      public OPT_LOGC_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool needclose = true;
      private long fileno;
      private long rqstrqid;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         ServBs.DataSource = iCRM.Services.Where(s => s.FILE_NO == fileno && s.CONF_STAT == "002" && Convert.ToInt32(s.ONOF_TAG_DNRM) >= 101);
         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var logc = LogcBs.Current as Data.Log_Call;

            if (logc.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  LogcBs.DataSource =
                     iCRM.Log_Calls.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.LCID == iCRM.Log_Calls.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.LCID));
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
            LogcBs.EndEdit();
            var logc = LogcBs.Current as Data.Log_Call;

            if (logc == null) throw new Exception("خطا * شی تماس تلفنی خالی می باشد");
            if (logc.LOG_TYPE == null || logc.LOG_TYPE == "") { LogCallType_Lov.Focus(); throw new Exception("خطا * نوع تماس تلفنی خالی می باشد"); }
            if (logc.SERV_FILE_NO == null || logc.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده تماس تلفنی خالی می باشد"); }
            if (logc.LOG_DATE == null) { LogCall_Date.Focus(); throw new Exception("خطا * تاریخ تماس تلفنی خالی می باشد"); }
            if (logc.SUBJ_DESC == null || logc.SUBJ_DESC == "") { Subject_Txt.Focus(); throw new Exception("خطا * موضوع تماس تلفنی خالی می باشد"); }
            if (logc.LOG_CMNT == null || logc.LOG_CMNT == "") { Comment_Txt.Focus(); throw new Exception("خطا * متن تماس تلفنی خالی می باشد"); }

            iCRM.OPR_LSAV_P(
               new XElement("LogCall",
                  new XAttribute("type", logc.LOG_TYPE),
                  new XAttribute("servfileno", logc.SERV_FILE_NO),
                  new XAttribute("datetime", GetDateTimeString(logc.LOG_DATE)),
                  new XAttribute("rqrorqstrqid", logc.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", logc.RQRO_RWNO ?? 0),
                  new XAttribute("lcid", logc.LCID),
                  new XAttribute("rsltstat", logc.RSLT_STAT),
                  new XAttribute("rqstrqid", rqstrqid),
                  new XElement("Comment",
                     new XAttribute("subject", logc.SUBJ_DESC),
                     logc.LOG_CMNT
                  )
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

      private void CallRequest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var logc = LogcBs.Current as Data.Log_Call;            

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
                                    new XAttribute("fileno", logc.SERV_FILE_NO),
                                    new XAttribute("rqid", logc.RQRO_RQST_RQID)
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
                                    new XAttribute("rqid", logc.RQRO_RQST_RQID),
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

      private void LogcBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = LogcBs.Current as Data.Log_Call;           

            if (rqst == null || rqst.Request_Row == null) { RqstFolw_Butn.Visible = false; return; }

            if (rqst.Request_Row.Request.RQST_RQID != null)
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

            // Set Rslt_Stat
            rqst.RSLT_STAT = rqst.RSLT_STAT == null ? "002" : rqst.RSLT_STAT;
            switch(rqst.RSLT_STAT)
            {
               case "001":
                  NoAnswer_Butn.NormalColorA = NoAnswer_Butn.NormalColorB = Color.PeachPuff;
                  Answer_Butn.NormalColorA = Answer_Butn.NormalColorB = Color.WhiteSmoke;
                  break;
               case "002":
                  Answer_Butn.NormalColorA = Answer_Butn.NormalColorB = Color.GreenYellow;
                  NoAnswer_Butn.NormalColorA = NoAnswer_Butn.NormalColorB = Color.WhiteSmoke;
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }         
      }

      private void AnswerStat_Butn_Click(object sender, EventArgs e)
      {
         var rb = sender as RoundedButton;

         var logc = LogcBs.Current as Data.Log_Call;
         if (logc == null) return;

         logc.RSLT_STAT = rb.Tag.ToString();
         switch (logc.RSLT_STAT)
         {
            case "001":
               NoAnswer_Butn.NormalColorA = NoAnswer_Butn.NormalColorB = Color.PeachPuff;
               Answer_Butn.NormalColorA = Answer_Butn.NormalColorB = Color.WhiteSmoke;
               break;
            case "002":
               Answer_Butn.NormalColorA = Answer_Butn.NormalColorB = Color.GreenYellow;
               NoAnswer_Butn.NormalColorA = NoAnswer_Butn.NormalColorB = Color.WhiteSmoke;
               break;
         }

         // اگر تماس تلفنی پاسخ داده نشده بررسی میکنیم که آیا برای مشتری نیاز هست تماس تلفنی دیگری پیرو این تماس ثبت گردد یا خیر
         if(logc.RSLT_STAT == "001")
         {
            var jbps = iCRM.Job_Personnels.FirstOrDefault(jp => jp.USER_NAME.ToUpper() == CurrentUser.ToUpper() && jp.DFLT_STAT == "002");
            // اگر غیر فعال باشد
            if((jbps.ADD_LOGC_RLST_NOAN ?? "001") == "001")
            {
               if (MessageBox.Show(this, string.Format("آیا مایل به ثبت تماس تلفنی برای {0} دقیقه دیگر هستید؟", jbps.ADD_LOGC_TIME_PROD ?? 15), "ثبت تماس تلفنی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            }

            if(logc.RQRO_RQST_RQID == null)
            {
               if (!Apply()) return;
            }
            logc = LogcBs.Current as Data.Log_Call;

            if (!iCRM.Log_Calls.Any(l => l.SERV_FILE_NO == logc.SERV_FILE_NO && l.LOG_DATE.Value >= DateTime.Now && l.Request_Row.Request.RQST_RQID == logc.RQRO_RQST_RQID))
            {
               iCRM.OPR_LSAV_P(
                  new XElement("LogCall",
                     new XAttribute("type", logc.LOG_TYPE),
                     new XAttribute("servfileno", logc.SERV_FILE_NO),
                     new XAttribute("datetime", GetDateTimeString(DateTime.Now.AddMinutes(jbps.ADD_LOGC_TIME_PROD ?? 15))),
                     new XAttribute("rqrorqstrqid", 0),
                     new XAttribute("rqrorwno", 0),
                     new XAttribute("lcid", 0),
                     new XAttribute("rsltstat", "002"),
                     new XAttribute("rqstrqid", logc.RQRO_RQST_RQID),
                     new XElement("Comment",
                        new XAttribute("subject", logc.SUBJ_DESC),
                        "پیگیری تماس"
                     )
                  )
               );

               // 1396/03/22 * بروز رسانی فرم اصلی برای یادآوری ها
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 41 /* Execute SetNotification */){Executive = ExecutiveType.Asynchronous}
                     }
                  )
               );
            }

         }
      }

      private void Finr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var logc = LogcBs.Current as Data.Log_Call;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 59 /* Execute Fin_Rslt_F */),
                     new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Final_Result",                              
                              new XAttribute("rqid", logc.RQRO_RQST_RQID)
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
                           new XAttribute("section", "logcall")
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

      private void FileAttachment_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            requery = Apply();

         }
         catch (Exception exc)
         { }
         finally
         {
            if (requery)
            {
               var logc = LogcBs.Current as Data.Log_Call;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                       {                  
                         new Job(SendType.Self, 32 /* Execute Fss_Show_F */),
                         new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", logc.SERV_FILE_NO), 
                                 new XAttribute("rqstrqid", logc.RQRO_RQST_RQID),
                                 //new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
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
   }
}
