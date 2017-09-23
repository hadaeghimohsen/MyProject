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
using System.Xml.Linq;
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing.Imaging;

namespace System.CRM.Ui.HistoryAction
{
   public partial class HST_FINR_F : UserControl
   {
      public HST_FINR_F()
      {
         InitializeComponent();
      }

      private string srchtype = "001";
      private long fileno;
      private string formCaller;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            srchtype = Optn_Pn.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked).Tag.ToString();
            switch (srchtype)
            {
               case "001":
                  FinrBs.DataSource =
                     iCRM.Final_Results
                     .Where(l =>
                        l.SERV_FILE_NO == fileno &&
                        l.FINR_TYPE_STAT == "001"
                     );                     
                  break;
               case "002":
                  FinrBs.DataSource =
                     iCRM.Final_Results
                     .Where(l =>
                        l.SERV_FILE_NO == fileno &&
                        l.FINR_TYPE_STAT == "001" &&
                        l.CRET_DATE.Value.Date >= DateTime.Now.AddMonths(-1)
                     );                     
                  break;
               case "003":
                  FinrBs.DataSource =
                     iCRM.Final_Results
                     .Where(l =>
                        l.SERV_FILE_NO == fileno &&
                        l.FINR_TYPE_STAT == "001" &&
                        l.CRET_DATE.Value.Date >= DateTime.Now.AddDays(-7)
                     );                     
                  break;
               case "004":
                  FinrBs.DataSource =
                     iCRM.Final_Results
                     .Where(l =>
                        l.SERV_FILE_NO == fileno &&
                        l.FINR_TYPE_STAT == "001" &&
                        l.CRET_DATE.Value.Date == DateTime.Now.AddDays(-1).Date
                     );                     
                  break;
               case "005":
                  FinrBs.DataSource =
                     iCRM.Final_Results
                     .Where(l =>
                        l.SERV_FILE_NO == fileno &&
                        l.FINR_TYPE_STAT == "001" &&
                        l.CRET_DATE.Value.Date == DateTime.Now.Date
                     );                     
                  break;
               case "006":
                  if (!RqstToDate_Date.Value.HasValue)                     
                     RqstToDate_Date.Value = DateTime.Now;

                  FinrBs.DataSource =
                     iCRM.Final_Results
                     .Where(l =>
                        l.SERV_FILE_NO == fileno &&
                        l.FINR_TYPE_STAT == "001" &&
                        l.CRET_DATE.Value.Date == RqstToDate_Date.Value.Value.Date
                     );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }

      private void rb_requestsearch_CheckedChanged(object sender, EventArgs e)
      {
         try
         {
            var rb = sender as RadioButton;
            srchtype = rb.Tag.ToString();                        
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            Execute_Query();
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

      private void Logc_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "CRET_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "CRET_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void RqstDate_Butn_Click(object sender, EventArgs e)
      {
         ChosseDayRqst_Cb.Checked = true;
         Execute_Query();
      }

      private void Logc_Gv_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            var crnt = FinrBs.Current as Data.Final_Result;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "FRST_PAGE_F", 100 /* ShowRequest */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Request",
                        new XAttribute("rqtpcode", crnt.Request_Row.RQTP_CODE),
                        new XAttribute("rqid", crnt.RQRO_RQST_RQID)
                     )
               }
            );
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
      }
   }
}
