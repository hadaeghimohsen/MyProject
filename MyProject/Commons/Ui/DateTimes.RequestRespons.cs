using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Globalization;

namespace MyProject.Commons.Ui
{
   partial class DateTimes : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }


      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               ActionCallForm(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         Enabled = true;
         SetTime_Tmr_Tick(null, null);
         PersianCalendar pc = new PersianCalendar();
         string year = pc.GetYear(DateTime.Now).ToString();
         string weekdaydesc = "";
         string monthdesc = "";
         switch(pc.GetDayOfWeek(DateTime.Now))
         {
            case DayOfWeek.Friday:
               weekdaydesc = "جمعه";
               break;
            case DayOfWeek.Saturday:
               weekdaydesc = "شنبه";
               break;
            case DayOfWeek.Sunday:
               weekdaydesc = "یکشنبه";
               break;
            case DayOfWeek.Monday:
               weekdaydesc = "دوشنبه";
               break;
            case DayOfWeek.Tuesday:
               weekdaydesc = "سه شنبه";
               break;
            case DayOfWeek.Wednesday:
               weekdaydesc = "چهارشنبه";
               break;
            case DayOfWeek.Thursday:
               weekdaydesc = "پنجشنبه";
               break;
         }
         string dayofmonthdesc = pc.GetDayOfMonth(DateTime.Now).ToString();
         switch(pc.GetMonth(DateTime.Now))
         {
            case 1:
               monthdesc = "فروردین";
               break;
            case 2:
               monthdesc = "اردیبهشت";
               break;
            case 3:
               monthdesc = "خرداد";
               break;
            case 4:
               monthdesc = "تیر";
               break;
            case 5:
               monthdesc = "مرداد";
               break;
            case 6:
               monthdesc = "شهریور";
               break;
            case 7:
               monthdesc = "مهر";
               break;
            case 8:
               monthdesc = "آبان";
               break;
            case 9:
               monthdesc = "آذر";
               break;
            case 10:
               monthdesc = "دی";
               break;
            case 11:
               monthdesc = "بهمن";
               break;
            case 12:
               monthdesc = "اسفند";
               break;
         }
         DateLongDesc_Lbl.Text = string.Format("{0}, {1} {2} {3}", weekdaydesc, dayofmonthdesc, monthdesc, year);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Commons:DateTimes", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "left:in-screen:normal:center"} }               
            });
         _DefaultGateway.Gateway(_Paint);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallForm(Job job)
      {         
         job.Status = StatusType.Successful;
      }  
   }
}
