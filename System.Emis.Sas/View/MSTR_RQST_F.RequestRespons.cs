using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Emis.Sas.View
{
   partial class MSTR_RQST_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private string ConnectionString;
      private OracleDataAdapter OraDA;
      private OracleTransaction OraTra;
      private DataSet OraPool;
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
               new Job(SendType.SelfToUserInterface, "MSTR_RQST_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.F11 | Keys.Control))
         {
            WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((DevExpress.XtraBars.Docking2010.IButton)WBtn_Search.Buttons[0]));
         }
         else if (keyData == Keys.F11)
         {
            WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((DevExpress.XtraBars.Docking2010.IButton)WBtn_Search.Buttons[1]));
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

         try
         {
            OraDA =
               new OracleDataAdapter(
                  new OracleCommand("SELECT SYSDATE FROM DUAL",
                     new OracleConnection(ConnectionString)
                     ) { CommandType = CommandType.Text }
               );
            OraPool = new DataSet();
         }
         catch (OracleException ex)
         {
            MessageBox.Show(ex.Message);
         }
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );
         TCmb_Refresh.SelectedIndex = 0;
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:MSTR_RQST_F", this }  },
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
            OraDA.SelectCommand.CommandText = "SELECT * FROM Request_Type ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[0].TableName = "Request_Type";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Region ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[1].TableName = "Region";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Requester_Type ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[2].TableName = "Requester_Type";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'ثبت موقت' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'ثبت دائم' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'انصراف درخواست' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[3].TableName = "Rqst_Stat";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'فروش انشعاب' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'فروش انرژی' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'خدمات پس از فروش' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[4].TableName = "Sub_Sys";

            RequestTypeBindingSource.DataSource = OraPool.Tables["Request_Type"];
            RequesterTypeBindingSource.DataSource = OraPool.Tables["Requester_Type"];
            RegionBindingSource.DataSource = OraPool.Tables["Region"];
            RqstStatBindingSource.DataSource = OraPool.Tables["Rqst_Stat"];
            SubSysBindingSource.DataSource = OraPool.Tables["Sub_Sys"];

            Tc_Info.SelectTab(0);
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
