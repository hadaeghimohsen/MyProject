﻿using System;
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
   public partial class OPT_PRJT_F : UserControl
   {
      public OPT_PRJT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? fileno, rqid;
      private bool needclose = true;
      private long rqstrqid;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);

         int rqst = RqstBs.Position;
         int rqro = RqroBs.Position;
         int srpr = SrprBs.Position;

         RqstBs.DataSource =
            iCRM.Requests.Where(t =>
               t.RQTP_CODE == "013" &&
               t.RQTT_CODE == "004" &&
               t.RQID == (rqid != null ? rqid : t.RQID) &&
               t.Request_Rows.Any(rr => rr.SERV_FILE_NO == fileno)
            );

         RqstBs.Position = rqst;
         RqroBs.Position = rqro;
         SrprBs.Position = srpr;

         requery = false;
      }

      private bool Apply()
      {
         try
         {
            var rqst = RqstBs.Current as Data.Request;

            needclose = false;
            Save_Butn_Click(null, null);
            needclose = true;
            if (requery)
            {
               iCRM = new Data.iCRMDataContext(ConnectionString);
               RqstBs.DataSource =
                  iCRM.Requests.FirstOrDefault(t =>
                     t.RQTP_CODE == "013" &&
                     t.RQTT_CODE == "004" &&                     
                     t.RQST_DESC == RqstDesc_Txt.Text &&
                     t.CRET_BY == CurrentUser &&
                     t.CRET_DATE.Value.Date == DateTime.Now.Date &&
                     t.Request_Rows.Any(rr => rr.SERV_FILE_NO == fileno) &&
                     t.RQID == iCRM.Requests.Where(r =>
                        r.RQTP_CODE == "013" &&
                        r.RQTT_CODE == "004" &&                        
                        r.RQST_DESC == RqstDesc_Txt.Text &&
                        r.CRET_BY == CurrentUser &&
                        r.CRET_DATE.Value.Date == DateTime.Now.Date &&
                        r.Request_Rows.Any(rr => rr.SERV_FILE_NO == fileno)
                     ).Max(r => r.RQID)
                  );
               requery = true;
            }

            return true;
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
            RqstBs.EndEdit();
            var rqst = RqstBs.Current as Data.Request;
            var rqro = RqroBs.Current as Data.Request_Row;
            var srpr = SrprBs.Current as Data.Service_Project;

            if (rqst == null) throw new Exception("خطا * شی یادداشت خالی می باشد");
            if (rqst.RQST_DESC == null || rqst.RQST_DESC == "") { RqstDesc_Txt.Focus(); throw new Exception("خطا * عنوان پروژه خالی می باشد"); }
            if (SrprBs.List.Count == 0) { throw new Exception("خطا * پروژه باید قابل دسترس حداقل یک کاربر باشد"); }

            iCRM.OPR_PSAV_P(
               new XElement("Project",
                  new XAttribute("rqstrqid", 0),
                  new XAttribute("rqrorqstrqid", rqst.RQID),                  
                  new XAttribute("rqstdesc", rqst.RQST_DESC),
                  new XElement("Request_Rows",
                     new XElement("Service_Projects",                              
                        new XElement("Service_Project",
                           new XAttribute("jbprcode", srpr.JBPR_CODE),
                           new XAttribute("recstat", srpr.REC_STAT),
                           new XAttribute("code", srpr.CODE),
                           new XAttribute("rwno", rqro.RWNO),
                           new XAttribute("servfileno", rqro.SERV_FILE_NO)
                        )
                     )
                  )
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            requery = false;
            iCRM.SaveException(exc);
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
            //if (requery && needclose)
            //{
            //   _DefaultGateway.Gateway(
            //      new Job(SendType.External, "localhost", FormCaller, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) { Input = new XElement("Service", new XAttribute("fileno", fileno)) }
            //   );
            //   Btn_Back_Click(null, null);
            //}
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

      private void Jrpb_Lov_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  var jbpr = (long?)Jrpb_Lov.EditValue;
                  if (jbpr == null) return;
                  if (!SrprBs.List.OfType<Data.Service_Project>().Any(sp => sp.JBPR_CODE == jbpr))
                  {
                     SrprBs.AddNew();
                     var srpr = SrprBs.Current as Data.Service_Project;
                     //srpr.SERV_FILE_NO_DNRM = fileno;
                     srpr.JBPR_CODE = jbpr;
                     srpr.REC_STAT = "002";
                  }
                  Apply();
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
         }
      }

      private void Actn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srpr = SrprBs.Current as Data.Service_Project;
            if (srpr == null) return;

            srpr.REC_STAT = srpr.REC_STAT == "001" ? "002" : "001";

            Apply();
         }
         catch (Exception exc)
         { }
      }

      private void AddRqro_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RqroBs.List.OfType<Data.Request_Row>().Any(rr => rr.RWNO == 0)) return;
            RqroBs.AddNew();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Rqro_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqro = RqroBs.Current as Data.Request_Row;
            if (rqro == null || rqro.SERV_FILE_NO == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  break;
               case 1:
                  var serv = ServBs.List.OfType<Data.Service>().FirstOrDefault(s => s.FILE_NO == rqro.SERV_FILE_NO);
                  if (serv == null) return;
                  int method = serv.SRPB_TYPE_DNRM == "001" ? 24 : 34;
                  string formpage = serv.SRPB_TYPE_DNRM == "001" ? "INF_LEAD_F" : "INF_CONT_F";
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.Self, method /* Execute Inf_Lead_F */),
                           new Job(SendType.SelfToUserInterface, formpage, 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                        }
                     )
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}