using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.DataGuard.Login.Ui
{
   partial class LockScreen : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      int Sleeping = 1;
      int step = 15;

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
               OpenDrawer(job);
               break;
            case 06:
               CloseDrawer(job);
               break;
            case 07:
               LoadData(job);
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
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Document\Login\Login.html"
                     }
                  });
         }
         else if (keyData == Keys.F2)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 02 /* Execute Set */);
         }
         else
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 06 /* Execute CloseDrawer */);
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
         
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 09 /* Execute LangChangToEnglish */, SendType.Self)
         );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {         
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
            {
               Next = new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */)
               {
                  Input = new List<object> { "DataGuard:Login:LockScreen", this },
                  Next = new Job(SendType.SelfToUserInterface, "Wall", 01 /* PastManualOnWall */)
                  {
                     Input = this ,//new List<object> { this, "cntrhrz:default:default" },
                     //Executive = ExecutiveType.Asynchronous,
                     Next = new Job(SendType.SelfToUserInterface, "Wall", 19 /* Execute SetClearForm */)
                  }
               }
            };

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */,
         //      new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
         //         new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */)
         //         {
         //            Input = this
         //         }
         //      )
         //   );

         job.Next = 
            new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */,
               new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
                     new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */, 
                        new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
                     ){Input = this}
               )
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void OpenDrawer(Job job)
      {
         int height = 0;

         for (; ; )
         {
            if (height + step <= Height)
            {
               Invoke(new Action(() =>
               {
                  Top += step;
                  height += step;
               }));
               Thread.Sleep(Sleeping);
            }
            else
               break;
         }
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void CloseDrawer(Job job)
      {
         this.Dock = DockStyle.None;

         int height = Height;

         for (; ; )
         {
            if (height - step >= 0)
            {
               Invoke(new Action(() =>
               {
                  Top -= step;
                  height -= step;
               }));
               Refresh();
               Thread.Sleep(Sleeping);
            }
            else
               break;
         }
         
         job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
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
