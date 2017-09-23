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

namespace System.CRM.Ui.Activity
{
   public partial class ACT_LOGC_F : UserControl
   {
      public ACT_LOGC_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int rqstindx = 0;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         rqstindx = RqstBs1.Position;
         if (tb_master.SelectedTab == tp_001)
         {            
            iCRM = new Data.iCRMDataContext(ConnectionString);
            var Rqids = iCRM.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "005" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.RQTT_CODE == "004" &&
                     (AllRequest_Tgl.IsOn ? true : rqst.CRET_BY == CurrentUser) &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iCRM.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID) &&
                     (AllJobWork_Tg.IsOn ? true : rqst.Job_Personnel.USER_NAME == CurrentUser)
               )
               .OrderByDescending(
                  rqst =>
                     rqst.RQST_DATE
               );

            RqstBs1.Position = rqstindx;
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;

            if (rqst == null) return;
            if (rqst.RQID == 0) return;

            if (MessageBox.Show(this, "آیا از انصراف درخواست مطمئن هستید؟", "انصراف درخواست", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.CNCL_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("rqtpcode", rqst.RQTP_CODE)
                  )
               )
            );
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
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

      private void RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            RqstBs1.EndEdit();
            RqroBs1.EndEdit();
            ActvBs1.EndEdit();
            LogcBs1.EndEdit();

            var rqst = RqstBs1.Current as Data.Request;
            var rqro = RqroBs1.Current as Data.Request_Row;
            //var actv = ActvBs1.Current as Data.Activity;

            //iCRM.ACT_LRQT_P(
            //   new XElement("Process",
            //      new XElement("Request",
            //         new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
            //         new XAttribute("rqstrqid", rqst == null ? 0 : rqst.RQST_RQID ?? 0),
            //         new XAttribute("rqstdesc", RqstDesc_Text.EditValue ?? ""),
            //         new XAttribute("jobpcode", JobpCode_Lov.EditValue ?? ""),
            //         new XAttribute("jobpdesc", JobpDesc_Text.EditValue ?? ""),
            //         new XElement("Service",
            //            new XAttribute("fileno", rqro == null ? FileNo_Lov.EditValue : rqro.SERV_FILE_NO)
            //         ),
            //         new XElement("Activity",
            //            new XAttribute("code", actv == null ? 0 : actv.CODE),
            //            new XElement("LogCalls",
            //               LogcBs1.List.OfType<Data.Log_Call>()
            //               .Select(l =>
            //                  new XElement("LogCall", 
            //                     new XAttribute("code", l.CODE),
            //                     new XAttribute("rwno", l.RWNO ?? 0),
            //                     new XAttribute("subjdesc", l.SUBJ_DESC ?? ""),
            //                     new XAttribute("logdate", l.LOG_DATE.Value.ToString("yyyy-MM-dd hh:mm:ss")),
            //                     new XAttribute("logstat", l.LOG_STAT ?? ""),
            //                     new XAttribute("logrslt", l.LOG_RSLT ?? ""),
            //                     new XAttribute("logtype", l.LOG_TYPE ?? "")
            //                  )
            //               )
            //            )
            //         )
            //      )
            //   )
            //);

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
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

      private void AllRequest_Tgl_Toggled(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void OKCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var log = LogcBs1.Current as Data.Log_Call;

            //log.LOG_STAT = "002";
            tb_callanswer.Visible = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void CancelCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var log = LogcBs1.Current as Data.Log_Call;

            //log.LOG_STAT = "001";
            tb_callanswer.Visible = false;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void LogcBs1_CurrentChanged(object sender, EventArgs e)
      {
         //try
         //{
         //   var log = LogcBs1.Current as Data.Log_Call;

         //   if (log == null)
         //      groupBox6.Visible = false;
         //   else
         //      groupBox6.Visible = true;

         //   if (log.LOG_STAT == "001")
         //      tb_callanswer.Visible = false;
         //   else if (log.LOG_STAT == "002")
         //      tb_callanswer.Visible = true;

         //}
         //catch (Exception exc)
         //{
         //   iCRM.SaveException(exc);
         //}
      }

      private void NewCall_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null || rqst.RQID == 0)
            {
               MessageBox.Show("ابتدا درخواست خود را ثبت کنید و سپس این دکمه را فشار دهید");
               return;
            }
            LogcBs1.AddNew();
            var log = LogcBs1.Current as Data.Log_Call;

            log.LOG_DATE = DateTime.Now;
            SubjDesc_Text.Focus();
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void Recall_Butn_Click(object sender, EventArgs e)
      {
         //try
         //{
         //   var currentLogCall = LogcBs1.Current as Data.Log_Call;
         //   currentLogCall.LOG_RSLT += string.Format("پیگیری برای {0} دقیقه دیگر", Minute_Text.Text);
         //   LogcBs1.AddNew();
         //   var log = LogcBs1.Current as Data.Log_Call;

         //   log.LOG_DATE = DateTime.Now.AddMinutes(Convert.ToInt32(Minute_Text.EditValue));
         //   SubjDesc_Text.Focus();
         //}
         //catch (Exception exc)
         //{
         //   iCRM.SaveException(exc);
         //}
      }

      private void SendToUser_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var parentRequest = RqstBs1.Current as Data.Request;
            RqstBs1.AddNew();
            var childRequest = RqstBs1.Current as Data.Request;
            childRequest.Request1 = parentRequest;
            FileNo_Lov.EditValue = parentRequest.Request_Rows.FirstOrDefault().SERV_FILE_NO;
            RqstBnARqt1_Click(null, null);
            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
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

      private void RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;

            if (rqst == null) return;

            iCRM.ACT_LSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID)
                  )
               )
            );

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request

            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iCRM.SaveException(exc);
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

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         var regl = iCRM.Regulations.FirstOrDefault(rg => rg.TYPE == "001" && rg.REGL_STAT == "002");

         if (regl == null) return;         
         
         Job _InteractWithCRM =
            new Job(SendType.External, "Localhost",
               new List<Job>
            {                  
               new Job(SendType.Self, 08 /* Execute Rqrq_Dfin_F */){Input = regl},
               new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 10 /* Execute Rqrq_Dfin_F */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "005")))}
            });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }
   }
}
