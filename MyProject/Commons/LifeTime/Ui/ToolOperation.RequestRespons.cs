using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;

namespace MyProject.Commons.LifeTime.Ui
{
   partial class ToolOperation : ISendRequest
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
            case 08:
               Finalization(job);
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
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Documents\MyProject\Commons\Recycle\Recycle.html"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.Escape)
         {
            job.Next = new Job(SendType.SelfToUserInterface, "ToolOperation", 04 /* Execute UnPaint */);
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
         var values = job.Input as List<object>;
         AfterSelectedAccepted = values[0] as Action;
         CommandName = values[2] as string;
         XMLParameter = values[3] as string;
         DsnName = values[4] as string;
         UserName = values[5] as string;
         RedoLog = values[6] as string;
         AccessPrivilege = values[7] as List<string>;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "ToolOperation",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Commons:LifeTime:ToolOperation", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */) {  Input = new List<object> { this, "cntrhrz:normal" }  }               
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
         Func<string, string, string, string> UpdateXmlRedoLog = (docRedolog, docRole, textExplain) =>
         {
            //var dsRedolog = XElement.Parse(docRedolog);
            var dsRole = XElement.Parse(docRole);

            return docRedolog = string.Format(docRedolog, string.Format(textExplain, dsRole.Descendants("RoleName").FirstOrDefault().Value));
         };

         int value = (int)job.Input;
         switch (value)
         {
            case 02: // Remove
               lb_title.Text = "عملیات حذف کردن";
               lb_caption.Text = "آیا با پاک کردن داده ها موافق هستید؟";
               BackColor = Color.Firebrick;
               RedoLog = UpdateXmlRedoLog(RedoLog, XMLParameter, "حذف گروه دسترسی {0}");               
               break;
            case 03: // Restore
               lb_title.Text = "عملیات بازیابی اطلاعات حذف شده";
               lb_caption.Text = "در صورت تمایل به برگرداندن داده های حذف از سیستم دکمه تایید را انتخاب کنید.";
               BackColor = Color.ForestGreen;
               RedoLog = UpdateXmlRedoLog(RedoLog, XMLParameter, "برگرداندن گروه دسترسی {0} به سیستم امنیتی");
               break;
            case 04: // Deactive
               lb_title.Text = "عملیات غیرفعال کردن";
               lb_caption.Text = "آیا با غیرفعال کردن داده مورد نظر موافقت می کنید؟ درصورت غیرفعال شدن، فعالیت محاسباتی و عملگری سیستم تحت تاثیر قرار میگیرد.در صورت تمایل دکمه تایید را فشار دهید";
               BackColor = Color.Goldenrod;
               RedoLog = UpdateXmlRedoLog(RedoLog, XMLParameter, "غیرفعال کردن گروه دسترسی {0}");               
               break;
            case 05:
               lb_title.Text = "عملیات بازیابی اطلاعات غیرفعال شده";
               lb_caption.Text = "در صورت تمایل به فعال کردن داده های غیرفعال شده در سیستم دکمه تایید را انتخاب کنید.";
               BackColor = Color.ForestGreen;
               RedoLog = UpdateXmlRedoLog(RedoLog, XMLParameter, "فعال کردن گروه دسترسی {0}");
               break;
            default:
               break;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void Finalization(Job job)
      {
         AfterSelectedAccepted.Invoke();
         job.Status = StatusType.Successful;
      }
   }
}
