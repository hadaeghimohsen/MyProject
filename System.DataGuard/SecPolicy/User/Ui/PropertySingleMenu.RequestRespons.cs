﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;
using System.JobRouting.Jobs;
using System.Windows.Forms;

namespace System.DataGuard.SecPolicy.User.Ui
{
   partial class PropertySingleMenu : ISendRequest
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
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @".\Documents\DataGuard\SecPolicy\Role\PropertySingleRoleMenu.html"
                     }
                  });
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "PropertySingleMenu", 04 /* Execute UnPaint */);
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
         Enabled = true;
         InputXmlData = job.Input.ToString();
         if (InputXmlData == "CreateNewUser")         
            sb_profile.Enabled = sb_duplicate.Enabled = false;
         else
            sb_profile.Enabled = sb_duplicate.Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "User",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"DataGuard:SecurityPolicy:User:PropertySingleMenu", this}},
                  new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */){Input = new List<object>{this , "cntrhrz:normal"}},
              });
         _DefaultGateway.Gateway(_Paint);
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
   }
}
