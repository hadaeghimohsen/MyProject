﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class CurrentUserInfo : ISendRequest
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
               OpenDrawer(job);
               break;
            case 06:
               CloseDrawer(job);
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
            job.Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
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
         LB_CurrentUserName.Text = job.Input.ToString();
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "DataGuard:SecurityPolicy:User:CurrentUserInfo", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */) {  Input = this }               
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
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void CloseDrawer(Job job)
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
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void ActionCallForm(Job job)
      {
         var input = job.Input as XElement;
         switch (input.Attribute("type").Value)
         {
            case "changeusername":
               LB_CurrentUserName.Text = input.Attribute("value").Value;
               break;
            default:
               break;
         }
      }

   }
}
