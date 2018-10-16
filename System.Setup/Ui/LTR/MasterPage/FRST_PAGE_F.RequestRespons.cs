using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Setup.Ui.LTR.MasterPage
{
   partial class FRST_PAGE_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }

      private XElement x = null;
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
               CheckSecurity(job);;
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               PostOnWall(job);
               break;
            case 09:
               TakeOnWall(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 40:
               SetToolTip(job);
               break;
            case 41:
               SetNotification(job);
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
         else if (keyData == Keys.F9)
         {
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
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
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("CRM:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         //Stop_BarCode();
         //Stop_FingerPrint();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
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
      private void PostOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;

            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Dock = DockStyle.Fill;
                  c.Visible = true;
                  Pnl_Desktop.Controls.Add(c);
                  Pnl_Desktop.Controls.SetChildIndex(c, 0);
               }), obj);
            else
            {
               obj.Dock = DockStyle.Fill;
               obj.Visible = true;
               Pnl_Desktop.Controls.Add(obj);
               Pnl_Desktop.Controls.SetChildIndex(obj, 0);
            }

         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void TakeOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => Pnl_Desktop.Controls.Remove(c)), obj);
            this.Focus();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Successful; UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => Pnl_Desktop.Controls.Remove(c)), obj);
            this.Focus();
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         //Execute_Query();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void SetToolTip(Job job)
      {
         //Lbs_Tooltip.Text = job.Input.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 41
      /// </summary>
      /// <param name="job"></param>
      private void SetNotification(Job job)
      {
         try
         {
            
         }
         catch (Exception exc){}
      }      
   }
}
