﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   partial class ControlPanel : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iProjectDataContext iProject;
      private string ConnectionString;

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
               CheckSecurity(job);
               break;
            case 06:
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

         if (keyData == Keys.Escape)
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
         throw new NotImplementedException();
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         Enabled = true;
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };

         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iProject = new Data.iProjectDataContext(ConnectionString);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "DataGuard",
             new List<Job>
              {
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
                  new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */){Input = new List<object>{"DataGuard:SecurityPolicy:ControlPanel", this}},
                  new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){Input = this},
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
      private void CheckSecurity(Job job)
      {
         if (iProject.GET_UPWD_U(new XElement("Request", new XAttribute("Rqtp_Code", "FACTORY"))) == "002")
            LNK_LicSoftware.Visible = true;
         else
            LNK_LicSoftware.Visible = false;
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
