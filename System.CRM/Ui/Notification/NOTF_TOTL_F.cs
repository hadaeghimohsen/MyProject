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
using DevExpress.XtraGrid.Views.Grid;
using Itenso.TimePeriod;
using System.Xml.Linq;

namespace System.CRM.Ui.Notification
{
   public partial class NOTF_TOTL_F : UserControl
   {
      public NOTF_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private XElement xinput;

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         
         // 1396/09/07 * اگر برای نمایش اطلاعات گزینه های فیلترینگ گذاشته شده باشد
         if (xinput.Attribute("type") != null)
         {
            switch (xinput.Attribute("type").Value)
            {
               case "iTask":
                  RmndBs.DataSource =
                     from jp in iCRM.Job_Personnels
                     join rm in iCRM.Reminders on jp.CODE equals rm.TO_JOBP_CODE
                     join r in iCRM.Requests on rm.RQST_RQID equals r.RQID
                     join t in iCRM.Tasks on r.RQID equals t.RQRO_RQST_RQID
                     where jp.USER_NAME == CurrentUser
                        && r.RQTP_CODE == "009"
                        && (t.TASK_STAT == "001" || t.TASK_STAT == "002")
                        && rm.ALRM_DATE.HasValue
                     select rm;

                  colTO_JOBP_CODE.Visible = false;
                  colFROM_JOBP_CODE.Visible = true;
                  break;
               case "oTask":
                  RmndBs.DataSource =
                     from jp in iCRM.Job_Personnels
                     join rm in iCRM.Reminders on jp.CODE equals rm.FROM_JOBP_CODE
                     join r in iCRM.Requests on rm.RQST_RQID equals r.RQID
                     join t in iCRM.Tasks on r.RQID equals t.RQRO_RQST_RQID
                     where jp.USER_NAME == CurrentUser
                        && rm.TO_JOBP_CODE != jp.CODE
                        && r.RQTP_CODE == "009"
                        && (t.TASK_STAT == "001" || t.TASK_STAT == "002")
                        && rm.ALRM_DATE.HasValue
                     select rm;

                  colTO_JOBP_CODE.Visible = true;
                  colFROM_JOBP_CODE.Visible = false;
                  break;
               default:
                  break;
            }
         }
         else
         {
            RmndBs.DataSource = iCRM.Reminders.Where(r => r.Job_Personnel1.USER_NAME.ToLower() == CurrentUser.ToLower() && r.ALRM_DATE.HasValue/*&& r.ALRM_DATE.Value.Date <= DateTime.Now.Date*/);
            colTO_JOBP_CODE.Visible = false;
            colFROM_JOBP_CODE.Visible = true;
         }
         iCRM.Job_Personnels.FirstOrDefault(jp => jp.USER_NAME.ToLower() == CurrentUser.ToLower()).RMND_STAT = "001";
         iCRM.SubmitChanges();
         requery = false;
      }

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Notification_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
         {
            var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "ALRM_DATE"));
            e.Value = GetTimePeriod(alrmdate);
         }
         else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
         {
            var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "ALRM_DATE"));
            e.Value = GetGroupTimePriod(alrmdate);
         }
      }

      private string GetGroupTimePriod(DateTime currentdate)
      {
         var result = "نامشخص";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 30 /* Execute DoWork4GetTimePeriod */, SendType.Self)
            {
               Input =
                  new XElement("TimePeriod",
                     new XAttribute("timetype", "group"),
                     new XAttribute("crntdate", currentdate)
                  ),
               AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     if (output != null)
                        result = output.ToString();
                  })
            }
         );

         return result;
      }

      private string GetTimePeriod(DateTime currentdate)
      {
         var result = "نامشخص";
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 30 /* Execute DoWork4GetTimePeriod */, SendType.Self)
            {
               Input =
                  new XElement("TimePeriod",
                     new XAttribute("timetype", "normal"),
                     new XAttribute("crntdate", currentdate)
                  ),
               AfterChangedOutput =
                  new Action<object>((output) =>
                  {
                     if (output != null)
                        result = output.ToString();
                  })
            }
         );

         return result;
      }

      private void Refresh_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void Lnk_MarkAllAsRead_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا مطمئن هستید؟", "همه را به عنوان خوانده شده علامت بزن", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iCRM.Reminders.Where(r => r.Job_Personnel1.USER_NAME.ToLower() == CurrentUser.ToLower() && r.ALRM_DATE.Value.Date <= DateTime.Now.Date && r.READ_RMND == "001").ToList().ForEach(r => r.READ_RMND = "002");
            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         { }
         finally { 
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Notf_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var crnt = RmndBs.Current as Data.Reminder;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Request",
                        new XAttribute("rqtpcode", crnt.Request.RQTP_CODE),
                        new XAttribute("rqid", crnt.RQST_RQID)
                     )
               }
            );

            if (crnt.READ_RMND == "001")
            {
               crnt.READ_RMND = "002";
               iCRM.SubmitChanges();
               requery = true;

               // 1396/03/22 * بروز رسانی فرم اصلی برای یادآوری ها
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 41 /* Execute SetNotification */){Executive = ExecutiveType.Asynchronous},
                        //new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 42 /* Execute SetMention */){Executive = ExecutiveType.Asynchronous}
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }

      private void Notification_Gv_DoubleClick(object sender, EventArgs e)
      {
         Notf_Butn_ButtonClick(Notf_Butn, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Notf_Butn.Buttons[0]));
      }
   }
}
