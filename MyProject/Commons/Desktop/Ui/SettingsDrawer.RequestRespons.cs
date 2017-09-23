using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject.Commons.Desktop.Ui
{
   partial class SettingsDrawer : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      int Sleeping = 5;
      int Step = 45;
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
               new Job(SendType.SelfToUserInterface, "SettingsDrawer", 06 /* Execute CloseDrawer */)
               {
                  Next = new Job(SendType.SelfToUserInterface, "SettingsDrawer", 04 /* Execute UnPaint */)
               };
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
         detailShow = DetailShow.Minimum;
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Commons:Desktop:SettingsDrawer", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */) {  Input = new List<object> { this, "right:off-screen:stretch" } }               
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
         bool toggleMode = (bool)job.Input;         

         switch (detailShow)
         {
            case DetailShow.None:
               break;
            case DetailShow.Minimum:
               for (int i = 0; i <= Width; i += Step)
               {
                  Invoke(new Action(() => { Left -= Step; }));
                  Thread.Sleep(Sleeping);
               }
               Invoke(new Action(() => { Left += toggleMode ? 35 : 45; }));
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
         for (int i = 0; i <= Width; i += Step)
         {
            Invoke(new Action(() => { Left += Step; }));
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

   }
}
