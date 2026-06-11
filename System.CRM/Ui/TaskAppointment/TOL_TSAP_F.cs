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

namespace System.CRM.Ui.TaskAppointment
{
   public partial class TOL_TSAP_F : UserControl
   {
      public TOL_TSAP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private DateTime datetime;
      private string srchtype = "002";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private async void Execute_Query()
      {
         try
         {
            bool isTab001 = Tb_Master.SelectedTab == tp_001;
            bool isTab002 = Tb_Master.SelectedTab == tp_002;
            string _currentUser = CurrentUser;
            DateTime now = DateTime.Now;

            string apSrchType = null;
            DateTime apDatetime = datetime;
            bool apToDateHasValue = false;
            DateTime apToDateValue = DateTime.MinValue;
            if (isTab001)
            {
               apSrchType = Apon_Pn.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked).Tag.ToString();
               if (apSrchType == "003")
               {
                  apToDateHasValue = AppointmentToDate_Date.Value.HasValue;
                  if (apToDateHasValue)
                     apDatetime = AppointmentToDate_Date.Value.Value;
                  else
                  {
                     apDatetime = now.AddDays(1);
                  }
               }
            }

            string tkSrchType = null;
            DateTime tkDatetime = datetime;
            bool tkToDateHasValue = false;
            DateTime tkToDateValue = DateTime.MinValue;
            if (isTab002)
            {
               tkSrchType = Task_Pn.Controls.OfType<RadioButton>().FirstOrDefault(rb => rb.Checked).Tag.ToString();
               if (tkSrchType == "003")
               {
                  tkToDateHasValue = TaskToDate_Date.Value.HasValue;
                  if (tkToDateHasValue)
                     tkDatetime = TaskToDate_Date.Value.Value;
                  else
                  {
                     tkDatetime = now.AddDays(1);
                  }
               }
            }

            var result = await Task.Run(() =>
            {
               using (var db = new Data.iCRMDataContext(ConnectionString))
               {
                  List<Data.Appointment> appointments = null;
                  List<Data.Task> tasks = null;

                  if (isTab001)
                  {
                     #region Appointment
                     switch (apSrchType)
                     {
                        case "001":
                           appointments =
                              (from ap in db.Appointments
                               join cl in db.Collaborators on ap.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where ap.FROM_DATE.Value.Date < now.Date
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select ap).ToList();
                           break;
                        case "002":
                           appointments =
                              (from ap in db.Appointments
                               join cl in db.Collaborators on ap.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where ap.FROM_DATE.Value.Date == now.Date
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select ap).ToList();
                           break;
                        case "003":
                           appointments =
                              (from ap in db.Appointments
                               join cl in db.Collaborators on ap.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where ap.FROM_DATE.Value.Date >= apDatetime
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select ap).ToList();
                           break;
                     }
                     #endregion
                  }
                  else if (isTab002)
                  {
                     #region Task
                     switch (tkSrchType)
                     {
                        case "001":
                           tasks =
                              (from tk in db.Tasks
                               join cl in db.Collaborators on tk.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where tk.DUE_DATE.Value.Date < now.Date
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select tk).ToList();
                           break;
                        case "002":
                           tasks =
                              (from tk in db.Tasks
                               join cl in db.Collaborators on tk.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where tk.DUE_DATE.Value.Date == now.Date
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select tk).ToList();
                           break;
                        case "003":
                           tasks =
                              (from tk in db.Tasks
                               join cl in db.Collaborators on tk.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where tk.DUE_DATE.Value.Date >= tkDatetime.Date
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select tk).ToList();
                           break;
                        case "004":
                           tasks =
                              (from tk in db.Tasks
                               join cl in db.Collaborators on tk.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where (tk.TASK_STAT == "001" || tk.TASK_STAT == "002")
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select tk).ToList();
                           break;
                        case "005":
                           tasks =
                              (from tk in db.Tasks
                               join cl in db.Collaborators on tk.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where (tk.TASK_STAT == "003")
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select tk).ToList();
                           break;
                        case "006":
                           tasks =
                              (from tk in db.Tasks
                               join cl in db.Collaborators on tk.RQRO_RQST_RQID equals cl.RQRO_RQST_RQID
                               where (tk.TASK_STAT == "004")
                                  && cl.Job_Personnel.USER_NAME.ToUpper() == _currentUser.ToUpper()
                               select tk).ToList();
                           break;
                     }
                     #endregion
                  }

                  return new { Appointments = appointments, Tasks = tasks };
               }
            });

            iCRM = new Data.iCRMDataContext(ConnectionString);

            if (isTab001)
            {
               if (apSrchType == "003" && !AppointmentToDate_Date.Value.HasValue)
               {
                  datetime = AppointmentToDate_Date.Value = apDatetime;
               }
               AponBs.DataSource = result.Appointments;
            }
            else if (isTab002)
            {
               if (tkSrchType == "003" && !TaskToDate_Date.Value.HasValue)
               {
                  datetime = TaskToDate_Date.Value = tkDatetime;
               }
               TaskBs.DataSource = result.Tasks;
            }
         }
         catch { }
      }

      private void rb_appointmentsearch_CheckedChanged(object sender, EventArgs e)
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

      private void rb_tasksearch_CheckedChanged(object sender, EventArgs e)
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

      private void Apon_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "FROM_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "FROM_DATE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }

      private void Task_Gv_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         try
         {
            GridView view = sender as GridView;
            if (e.Column.FieldName == "DATE_TIME_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "DUE_DATE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            if (e.Column.FieldName == "DATE_TIME_DEAD_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "DEAD_LINE"));
               e.Value = GetTimePeriod(alrmdate);
            }
            else if (e.Column.FieldName == "GROP_DATE_DESC" && e.IsGetData)
            {
               var alrmdate = ((DateTime)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "DEAD_LINE"));
               e.Value = GetGroupTimePriod(alrmdate);
            }
         }
         catch { e.Value = "نامشخص"; }
      }
   }
}
