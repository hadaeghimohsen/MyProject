using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.MasterPage
{
   partial class MSTR_PAGE_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private bool requery = default(bool);

      private XElement x;
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
               Goto_NextForm(job);
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
               new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 04 /* Execute UnPaint */);
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
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Scsc:MSTR_PAGE_F", this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) {  Input = this }               
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
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         if (!InvokeRequired)
         {
            Execute_Query();
         }
         else
         {
            Invoke(new Action(() =>
               {
                  Execute_Query();
               }));
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void Goto_NextForm(Job job)
      {
         x = job.Input as XElement;
         switch (x.Descendants("NextForm").First().FirstAttribute.Value)
         {
            case "16":
               TSM_AdmSumF_Click(null, null);
               break;
            case "17":
               TSM_AdmSendExpnF_Click(null, null);
               break;
            case "18":
               TSM_AdmRecieveExpnF_Click(null, null);
               break;
            case "19":
               TSM_AdmFinalRequestF_Click(null, null);
               break;
            case "22":
               TSM_TestSendExpnF_Click(null, null);
               break;
            case "23":
               TSM_TstRecieveExpnF_Click(null, null);
               break;
            case "24":
               TSM_TstSaveF_Click(null, null);
               break;
            case "25":
               TSM_CmpRequestF_Click(null, null);
               break;
            case "26":
               TSM_CmpSendExpnF_Click(null, null);
               break;
            case "27":
               TSM_CmpReceiveF_Click(null, null);
               break;
            case "28":
               TSM_CmpSaveF_Click(null, null);
               break;
            case "29":
               TSM_PsfRequestF_Click(null, null);
               break;
            case "30":
               TSM_PsfSendExpnF_Click(null, null);
               break;
            case "31":
               TSM_PsfRecieveExpnF_Click(null, null);
               break;
            case "32":
               TSM_PsfSaveF_Click(null, null);
               break;
            case "33":
               TSM_ClcRequestF_Click(null, null);
               break;
            case "34":
               TSM_ClcSendExpnF_Click(null, null);
               break;
            case "35":
               TSM_ClcRecieveExpnF_Click(null, null);
               break;
            case "36":
               TSM_ClcSaveF_Click(null, null);
               break;
            case "37":
               TSM_HertRequestF_Click(null, null);
               break;
            case "38":
               TSM_HertSendExpnF_Click(null, null);
               break;
            case "39":
               TSM_HertRecieveExpnF_Click(null, null);
               break;
            case "40":
               TSM_HertSaveF_Click(null, null);
               break;
            case "41":
               TSM_ExamRequestF_Click(null, null);
               break;
            case "42":
               TSM_ExamSendExpnF_Click(null, null);
               break;
            case "43":
               TSM_ExamRecieveExpnF_Click(null, null);
               break;
            case "44":
               TSM_ExamSaveF_Click(null, null);
               break;
            case "47":
               TSM_FgPbRqstF_Click(null, null);
               break;
            case "48":
               TSM_FgPbSendExpnF_Click(null, null);
               break;
            case "49":
               TSM_FgPbRecieveExpnF_Click(null, null);
               break;
            case "50":
               TSM_FgPbSaveF_Click(null, null);
               break;
            case "51":
               TSM_CmcRqstF_Click(null, null);
               break;
            case "52":
               TSM_CmcSendExpnF_Click(null, null);
               break;
            case "53":
               TSM_CmcReceiveExpnF_Click(null, null);
               break;
            case "54":
               TSM_CmcSaveF_Click(null, null);
               break;
            case "55":
               TSM_UCCRqstF_Click(null, null);
               break;
            case "56":
               TSM_UCCSExpnF_Click(null, null);
               break;
            case "57":
               TSM_UCCRExpnF_Click(null, null);
               break;
            case "58":
               TSM_UCCSaveF_Click(null, null);
               break;
            case "60":
               TSM_MCCRqstF_Click(null, null);
               break;
            case "61":
               TSM_MCCSExpnF_Click(null, null);
               break;
            case "62":
               TSM_MCCRExpnF_Click(null, null);
               break;
            case "63":
               TSM_MCCSaveF_Click(null, null);
               break;
            default:
               break;
         }
         x = null;
      }
   }
}
