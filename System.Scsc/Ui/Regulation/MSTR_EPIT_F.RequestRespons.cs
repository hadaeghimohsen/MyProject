﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Regulation
{
   partial class MSTR_EPIT_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
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
               new Job(SendType.SelfToUserInterface, "MSTR_EPIT_F", 04 /* Execute UnPaint */);
         }
         else if(keyData == (Keys.Control | Keys.Insert))
         {
            AddSubGrop_Butn_Click(null, null);
         }         
         else if(keyData == (Keys.F5))
         {
            GropBnASav1_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Delete))
         {
            DelGrop_Tsm_Click(null, null);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();

         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

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
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
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
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
               })
            );

         job.Status = StatusType.Successful;
      }


      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         EpitBs1.DataSource = iScsc.Expense_Items;
         DittpBs1.DataSource = iScsc.D_ITTPs;
         GropBs2.DataSource = iScsc.Group_Expenses;//.Where(ge => ge.STAT == "002" && iScsc.Methods.Any(m => m.MTOD_STAT == "002" && m.CODE == ge.CODE));
         DGptpBs1.DataSource = iScsc.D_GPTPs;
         string[] _rqtpCode = {"001", "009", "012", "016"};
         RqtpBs.DataSource = iScsc.Request_Types.Where(r => _rqtpCode.Contains(r.CODE));
         DActvBs.DataSource = iScsc.D_ACTVs;

         UExpn_Gv.ActiveFilterString = string.Format("EXPN_STAT = '002' AND GROP_CODE IS NULL");
         DExpn_Gv.ActiveFilterString = string.Format("EXPN_STAT = '002' AND GROP_CODE IS NOT NULL");
         

         var xinput = job.Input as XElement;
         switch(xinput.Attribute("type").Value)
         {
            case "001":
               Master_Tb.SelectedTab = tp_001;
               break;
            case "002":
               Master_Tb.SelectedTab = tp_002;
               break;
         }

         Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
