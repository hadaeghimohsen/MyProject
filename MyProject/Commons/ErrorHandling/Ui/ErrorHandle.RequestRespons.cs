using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;
using System.Threading;

namespace MyProject.Commons.ErrorHandling.Ui
{
   partial class ErrorHandle : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      int Sleeping = 1;
      int Step = 15;
      enum DetailShow { None, Minimum, Maximum };
      DetailShow detailShow = DetailShow.None;

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
            case 08:
               Finalization(job);
               break;
            default:
               job.Status = StatusType.Failed;
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
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Documents\MyProject\Commons\ErrorHandling\ErrorHandling.html"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "ErrorHandle", 06 /* Execute CloseDrawer */)
               {
                  Next = new Job(SendType.SelfToUserInterface, "ErrorHandle", 08 /* Execute Finalization */)
                  {
                     Next = new Job(SendType.SelfToUserInterface, "ErrorHandle", 04 /* Execute UnPaint */)
                  }
               };

         }
         else if (keyData == Keys.Up)
         {
            if (detailShow == DetailShow.None)
               return;

            job.Next =
               new Job(SendType.SelfToUserInterface, "ErrorHandle", 05 /* Execute OpenDrawer */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         detailShow = DetailShow.Minimum;
         if (job.Input is string)
         {
            Wb_ErrorHandle.DocumentText = job.Input as string;
         }
         else
         {
            var values = job.Input as List<object>;
            Wb_ErrorHandle.DocumentText = values[0].ToString();
            AfterClose = values[1] as Action;
         }

         Wb_ErrorHandle.Focus();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "ErrorHandle",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Commons:ErrorHandle:ErrorHandle", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */) {  Input = new List<object> { this, "bottom:off-screen:stretch" }  }               
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
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void OpenDrawer(Job job)
      {
         switch (detailShow)
         {
            case DetailShow.None:
               break;
            case DetailShow.Minimum:
               for (int i = 0; i <= Height - 300; i += Step)
               {
                  Invoke(new Action(() => { Top -= Step; }));
                  Thread.Sleep(Sleeping);
               }
               detailShow = DetailShow.Maximum;
               break;
            case DetailShow.Maximum:
               for (int i = 0; i <= Height - 90; i += Step)
               {
                  Invoke(new Action(() => { Top -= Step; }));
                  Thread.Sleep(Sleeping);
               }
               detailShow = DetailShow.None;
               break;
            default:
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
         for (int i = 0; i <= Height; i += Step)
         {
            Invoke(new Action(() => { Top += Step; }));
            Thread.Sleep(Sleeping);
         }
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
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void Finalization(Job job)
      {
         if (AfterClose != null)
            AfterClose.Invoke();
         job.Status = StatusType.Successful;
      }    
   }
}
