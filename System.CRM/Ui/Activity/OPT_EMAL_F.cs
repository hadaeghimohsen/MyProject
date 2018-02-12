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
   public partial class OPT_EMAL_F : UserControl
   {
      public OPT_EMAL_F()
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
         ServBs.DataSource = iCRM.Services.Where(s => s.FILE_NO == fileno);
         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var emal = EmalBs.Current as Data.Email;

            if (emal.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  EmalBs.DataSource =
                     iCRM.Emails.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.EMID == iCRM.Emails.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.EMID));
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
            EmalBs.EndEdit();
            var email = EmalBs.Current as Data.Email;

            if (email == null) throw new Exception("خطا * شی ایمیل خالی می باشد");
            if (email.SERV_FILE_NO == null || email.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده ایمیل خالی می باشد");  }
            if (email.SEND_DATE == null) { Email_Date.Focus(); throw new Exception("خطا * تاریخ ارسال ایمیل خالی می باشد"); }
            if (email.SUBJ_DESC == null || email.SUBJ_DESC == "") { Subject_Txt.Focus(); throw new Exception("خطا * موضوع ایمیل خالی می باشد"); }
            if (email.EMAL_CMNT == null || email.EMAL_CMNT == "") { Comment_Txt.Focus(); throw new Exception("خطا * متن ایمیل خالی می باشد"); }
            if (EtoeBs.Count == 0) { ToMail_Txt.Focus(); throw new Exception("خطا * آدرس ایمیل های مقصد در ایمیل خالی می باشد"); }

            iCRM.OPR_MSAV_P(
               new XElement("Email",
                  new XAttribute("servfileno", email.SERV_FILE_NO),
                  new XAttribute("senddate", GetDateTimeString(email.SEND_DATE)),
                  new XAttribute("rqrorqstrqid", email.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", email.RQRO_RWNO ?? 0),
                  new XAttribute("colr", email.Request_Row != null ? email.Request_Row.Request.COLR : "#ADFF2F"),
                  new XAttribute("emid", email.EMID),
                  new XAttribute("sendstat", email.SEND_STAT ?? "001"),
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("projrqstrqid", projrqstrqid),
                  new XElement("Comment",
                     new XAttribute("subject", email.SUBJ_DESC),
                     email.EMAL_CMNT
                  ),
                  new XElement("EmailTos",
                     EtoeBs.List.OfType<Data.Email_To_Email>().
                     Select(to => 
                        new XElement("EmailTo",to.TO_MAIL)
                     )
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

      private void ToMail_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (ToMail_Txt.Text == "") { ToMail_Txt.Focus(); return; }

            if (EtoeBs.List.OfType<Data.Email_To_Email>().Any(to => to.TO_MAIL.ToLower() == ToMail_Txt.Text.ToLower())) { ToMail_Txt.Text = ""; return; }

            EtoeBs.AddNew();
            var tomail = EtoeBs.Current as Data.Email_To_Email;

            tomail.TO_MAIL = ToMail_Txt.Text;
            ToMail_Txt.Text = "";
         }
         catch (Exception exc)
         {

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

      private void FileAttachment_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            /*var emal = EmalBs.Current as Data.Email;

            if (emal.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  EmalBs.DataSource =
                     iCRM.Emails.FirstOrDefault(em =>
                        em.SERV_FILE_NO == fileno &&
                        em.EMID == iCRM.Emails.Where(email => email.SERV_FILE_NO == fileno).Max(email => email.EMID));
                  requery = true;
               }
            }
            else
               requery = true;*/

            requery = Apply();
            
         }
         catch (Exception exc)
         {}
         finally
         {
            if(requery)
            {
               var emal = EmalBs.Current as Data.Email;               

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                       {                  
                         new Job(SendType.Self, 32 /* Execute Fss_Show_F */),
                         new Job(SendType.SelfToUserInterface, "FSS_SHOW_F", 10 /* Execute ACTN_CALF_P */)
                         {
                            Input = 
                              new XElement("Service", 
                                 new XAttribute("fileno", emal.SERV_FILE_NO), 
                                 new XAttribute("rqstrqid", emal.RQRO_RQST_RQID),
                                 new XAttribute("projrqstrqid", projrqstrqid),
                                 //new XAttribute("tasktype", "new"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                         },
                       });
               _DefaultGateway.Gateway(_InteractWithCRM);
            }
         }
      }

      private void SendNow_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            /*var emal = EmalBs.Current as Data.Email;

            needclose = false;
            Save_Butn_Click(null, null);
            needclose = true;
            if (emal.RQRO_RQST_RQID == null)
            {
               iCRM = new Data.iCRMDataContext(ConnectionString);
               EmalBs.DataSource =
                  iCRM.Emails.FirstOrDefault(em =>
                     em.SERV_FILE_NO == fileno &&
                     em.EMID == iCRM.Emails.Where(email => email.SERV_FILE_NO == fileno).Max(email => email.EMID));
            }
            requery = true;*/
            requery = Apply();
         }
         catch (Exception exc)
         { }
         finally
         {
            if (requery)
            {
               var emal = EmalBs.Current as Data.Email;

               Job _InteractWithCRM =
                 new Job(SendType.External, "Localhost",
                    new List<Job>
                       {                  
                         new Job(SendType.External, "Commons",
                            new List<Job>
                            {
                               new Job(SendType.Self, 27 /* Execute SendMail */)
                               {
                                  Input = 
                                    new XElement("Email",
                                       new XElement("Comment",
                                          new XAttribute("subject", emal.SUBJ_DESC),
                                          emal.EMAL_CMNT
                                       ),
                                       new XElement("EmailTargets",
                                          emal.Email_To_Emails.
                                          Select( to =>
                                             new XElement("To",
                                                new XAttribute("email", to.TO_MAIL)
                                             )
                                          )
                                       ),
                                       new XElement("Attachments",
                                          iCRM.Send_Files.Where(em => em.SERV_FILE_NO == fileno && em.Request_Row.Request.Request2.Request_Rows.Any(rr => rr.Emails.FirstOrDefault().EMID == emal.EMID)).
                                          Select(f => 
                                             new XElement("File", 
                                                new XAttribute("path", f.FILE_SRVR_LINK)
                                             )
                                          )
                                       )
                                    )
                               }
                            }
                         )
                       });
               _DefaultGateway.Gateway(_InteractWithCRM);

               emal.SEND_STAT = "002";
               iCRM.SubmitChanges();
            }
         }
      }

      private void CallRequest_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as RoundedButton;
            if (!Apply()) return;
            var emal = EmalBs.Current as Data.Email;            

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
                                    new XAttribute("fileno", emal.SERV_FILE_NO),
                                    new XAttribute("rqid", emal.RQRO_RQST_RQID),
                                    new XAttribute("projrqstrqid", projrqstrqid)
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
                                    new XAttribute("rqid", emal.RQRO_RQST_RQID),
                                    new XAttribute("formcaller", GetType().Name),
                                    new XAttribute("projrqstrqid", projrqstrqid)
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
            var emal = EmalBs.Current as Data.Email;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 59 /* Execute Fin_Rslt_F */),
                     new Job(SendType.SelfToUserInterface, "FIN_RSLT_F", 10 /* Execute Actn_Calf_F */)
                     {
                        Input = 
                           new XElement("Final_Result",                              
                              new XAttribute("rqid", emal.RQRO_RQST_RQID)
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
                           new XAttribute("section", "email")
                        )
                  }
               }
            )
         );
      }

      private void EmalBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = EmalBs.Current as Data.Email;
            if (rqst == null) { RqstFolw_Butn.Visible = false; return; }

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

      private void Subject_Txt_TextChanged(object sender, EventArgs e)
      {
         var emal = EmalBs.Current as Data.Email;

         if (emal == null || !LinkText_Pk.PickChecked) return;

         emal.SUBJ_DESC = Subject_Txt.Text;
         emal.EMAL_CMNT = Subject_Txt.Text;
      }

      private void SelectColor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var emal = EmalBs.Current as Data.Email;

            if (emal.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  EmalBs.DataSource =
                     iCRM.Emails.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.EMID == iCRM.Emails.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.EMID));
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
