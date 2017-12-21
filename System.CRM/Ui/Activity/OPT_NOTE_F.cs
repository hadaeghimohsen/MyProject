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
   public partial class OPT_NOTE_F : UserControl
   {
      public OPT_NOTE_F()
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
            var note = NoteBs.Current as Data.Note;

            if (note.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  NoteBs.DataSource =
                     iCRM.Notes.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.NTID == iCRM.Notes.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.NTID));
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
            NoteBs.EndEdit();
            var note = NoteBs.Current as Data.Note;

            if (note == null) throw new Exception("خطا * شی یادداشت خالی می باشد");
            if (note.SERV_FILE_NO == null || note.SERV_FILE_NO == 0) { Serv_Lov.Focus(); throw new Exception("خطا * شماره پرونده یادداشت خالی می باشد"); }
            if (note.NOTE_DATE == null) { Mesg_Date.Focus(); throw new Exception("خطا * تاریخ یادداشت خالی می باشد"); }
            if (note.NOTE_CMNT == null || note.NOTE_CMNT == "") { Comment_Txt.Focus(); throw new Exception("خطا * متن یادداشت خالی می باشد"); }


            iCRM.OPR_NSAV_P(
               new XElement("Note",
                  new XAttribute("rqstrqid", rqstrqid),
                  new XAttribute("projrqstrqid", projrqstrqid),
                  new XAttribute("servfileno", note.SERV_FILE_NO),
                  new XAttribute("rqrorqstrqid", note.RQRO_RQST_RQID ?? 0),
                  new XAttribute("rqrorwno", note.RQRO_RWNO ?? 0),
                  new XAttribute("ntid", note.NTID),
                  new XAttribute("notedate", GetDateTimeString(note.NOTE_DATE)),
                  new XElement("Comment",
                     note.NOTE_CMNT
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
            var mesg = NoteBs.Current as Data.Message;            

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
            var mesg = NoteBs.Current as Data.Message;

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

      private void NoteBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rqst = NoteBs.Current as Data.Note;
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
            var note = NoteBs.Current as Data.Note;

            if (note.RQRO_RQST_RQID == null)
            {
               needclose = false;
               Save_Butn_Click(null, null);
               needclose = true;
               if (requery)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  NoteBs.DataSource =
                     iCRM.Notes.FirstOrDefault(t =>
                        t.SERV_FILE_NO == fileno &&
                        t.NTID == iCRM.Notes.Where(tt => tt.SERV_FILE_NO == fileno).Max(tt => tt.NTID));
                  requery = true;
               }
            }
            else
               requery = true;
         }
         catch {  }
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
