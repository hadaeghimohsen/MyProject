using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_CHNG_F : ISendRequest
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
               Open(job);
               break;
            case 06:
               Close(job);
               break;
            case 07:
               LoadData(job);
               break;
            default:
               break;
         }
      }

      private int step = 10, interval = 5;
      private XDocument xProfiler { get; set; }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "PRF_CHNG_F", 06 /* Execute Close */);
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
         xProfiler = job.Input as XDocument;
         be_profiler_name.Text = xProfiler.Descendants("Profiler").First().Attribute("faName").Value;
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {Input = new List<object> {"Reporting:WorkFlow:PRF_CHNG_F", this}},
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "cntrhrz:default"} }               
            });
         _DefaultGateway.Gateway(_Paint);
         
         Top = Top - 100;
         Height = 0;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFormWall */){Input = this}
               }));

         Top = Top + 100;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void Open(Job job)
      {
         int height = 0;

         for (; ; )
         {
            if (height + step <= 200)
            {
               Invoke(new Action(() =>
               {
                  Height += step;
                  height += step;
               }));
               Thread.Sleep(interval);
            }
            else
               break;
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void Close(Job job)
      {
         int height = Height;

         for (; ; )
         {
            if (height - step >= 0)
            {
               Invoke(new Action(() =>
               {
                  Height -= step;
                  height -= step;
               }));
               Thread.Sleep(interval);
            }
            else
               break;
         }

         job.Next =
               new Job(SendType.SelfToUserInterface, "PRF_CHNG_F", 04 /* Execute UnPaint */);

         /* بارگذاری مجدد پارامترهای فرم */
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "PRF_SPRF_F", 07 /* Execute LoadData */, SendType.SelfToUserInterface)
         );

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
   }
}
