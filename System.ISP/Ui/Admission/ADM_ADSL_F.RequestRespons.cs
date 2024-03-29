﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.ISP.Ui.Admission
{
   partial class ADM_ADSL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iISPDataContext iISP;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uagc_U;

      private bool isFirstLoaded = false;


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
            case 10:
               Actn_CalF_P(job);
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

         if (keyData == (Keys.Control | Keys.Insert))
         {
            RqstBs1.AddNew();
         }
         else if(keyData == (Keys.Control | Keys.Delete))
         {
            RqstBnDelete1_Click(null, null);
         }
         else if(keyData == Keys.F5)
         {
            RqstBnARqt1_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.P))
         {
            RqstBnDefaultPrint1_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.Shift | Keys.P))
         {
            RqstBnPrint1_Click(null, null);
         }
         else if(keyData == Keys.F10 && RqstBnASav1.Enabled)
         {
            RqstBnASav1_Click(null, null);
         }
         else if (keyData == Keys.Enter)
         {
            if (!(Btn_Back.Focused))
               SendKeys.Send("{TAB}");
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
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iISP</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         
         ConnectionString = GetConnectionString.Output.ToString();
         iISP = new Data.iISPDataContext(GetConnectionString.Output.ToString());
         Fga_Uprv_U = iISP.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iISP.FGA_URGN_U() ?? "";
         Fga_Uagc_U = (iISP.FGA_UAGC_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         //var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         //_DefaultGateway.Gateway(GetHostInfo);

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
               //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("ISP:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
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
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>58</Privilege><Sub_Sys>10</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 58 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithJob);         
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if(!isFirstLoaded)
         {
            DowtpBs1.DataSource = iISP.D_OWTPs;
            DrcmtBs1.DataSource = iISP.D_RCMTs;
            PrvnBs1.DataSource = iISP.Provinces;
            OrgnBs1.DataSource = iISP.Organs;
            BtrfBs1.DataSource = iISP.Base_Tariffs;
            DCyclBs1.DataSource = iISP.D_CYCLs;
            RqtpBs1.DataSource = iISP.Request_Types;
            RqttBs1.DataSource = iISP.Requester_Types;
            isFirstLoaded = true;
         }

         StatusSaving_Sic.StateIndex = 0;
         requery = true;
         Execute_Query();
         requery = false;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         job.Status = StatusType.Successful;
      }

   }
}
