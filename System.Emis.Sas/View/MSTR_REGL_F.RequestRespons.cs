using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Emis.Sas.View
{
   partial class MSTR_REGL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private string ConnectionString;
      private Model.Domain domain;
      private Model.Access_Entity access_entity;

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
               new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 04 /* Execute UnPaint */);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = string.Format(@"<Database value=""id"">{0}</Database><Dbms>Oracle</Dbms>", job.Input) };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();

         domain = new Model.Domain(ConnectionString);
         access_entity = new Model.Access_Entity(ConnectionString);

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:MSTR_REGL_F", this }  },
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
         try
         {
            nwtypBs1.DataSource = domain.NWTYP();
            PhasBs1.DataSource = domain.PHAS();
            ActstBs2.DataSource = domain.ACTST();

            EpitBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Sas_Expense_Item WHERE Sub_Sys = 3");
            CyclBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM CYCLE ORDER BY YEAR");
            
            AReg07Bs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Regulation WHERE Reg_Stat = 1 AND Sub_Sys = 3 AND Regl_Type = '07' AND CYCL_YEAR || CODE = (SELECT MAX(CYCL_YEAR || CODE) FROM Regulation WHERE Reg_Stat = 1 AND Sub_Sys = 3 AND Regl_Type = '07')");
            AReg11Bs2.DataSource = access_entity.Run_Qury_U("SELECT * FROM Regulation WHERE Reg_Stat = 1 AND Sub_Sys = 3 AND Regl_Type = '11' AND CYCL_YEAR || CODE = (SELECT MAX(CYCL_YEAR || CODE) FROM Regulation WHERE Reg_Stat = 1 AND Sub_Sys = 3 AND Regl_Type = '11')");
            
            RqttBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Requester_Type");
            RqtpBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Request_Type");
            BankBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Bank");
            EpitBs1.DataSource = access_entity.Run_Qury_U("SELECT * FROM Sas_Expense_Item");

            BankBs2.DataSource = access_entity.Run_Qury_U("SELECT * FROM Bank");

            CyclBs1.MoveLast();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
