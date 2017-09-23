using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OracleClient;
using System.Data;
using DevExpress.XtraBars.Docking2010;

namespace System.Emis.Sas.View
{
   partial class MSTR_SERV_F : ISendRequest
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
               new Job(SendType.SelfToUserInterface, "MSTR_SERV_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.F11 | Keys.Control))
         {
            WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((IButton)WBtn_Search.Buttons[0]));
         }
         else if (keyData == Keys.F11)
         {
            WBtn_Search_ButtonClick(WBtn_Search, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((IButton)WBtn_Search.Buttons[1]));
         }
         else if (keyData == (Keys.F10 | Keys.Control))
         {
            WBtn_FIR_ButtonClick(WBtn_FIR, new DevExpress.XtraBars.Docking2010.ButtonEventArgs((IButton)WBtn_FIR.Buttons[0]));
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:MSTR_SERV_F", this }  },
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
            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D CYCST' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[0].TableName = "Cycl_Stat";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Request_Type ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[1].TableName = "Request_Type";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Region ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[2].TableName = "Region";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Requester_Type ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[3].TableName = "Requester_Type";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'ثبت موقت' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'ثبت دائم' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'انصراف درخواست' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[4].TableName = "Rqst_Stat";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'عادی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'دیماندی' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[5].TableName = "Serv_Type";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'شهری' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'روستایی' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[6].TableName = "Zone_Type";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D OFTAG' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[7].TableName = "Onof_Tag";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'یکماه' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'دوماه' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'سه ماه' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[8].TableName = "Read_Stat";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'خانگی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'عمومی' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'کشاورزی' AS RV_MEANING FROM DUAL UNION SELECT 4 AS RV_LOW_VALUE, 'صنعتی' AS RV_MEANING FROM DUAL UNION SELECT 5 AS RV_LOW_VALUE, 'سایر مصارف' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[9].TableName = "Cons_Code";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'خیر' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'بلی' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[10].TableName = "Yes_No";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'تکفاز' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'سه فاز' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[11].TableName = "Phas";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'دائم' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'آزاد' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'حاشیه شهرها' AS RV_MEANING FROM DUAL UNION SELECT 4 AS RV_LOW_VALUE, 'آزاد به دائم' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[12].TableName = "Brnc_Type";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'دیجیتالی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'مکانیکی' AS RV_MEANING FROM DUAL UNION SELECT 3 AS RV_LOW_VALUE, 'قرائت از راه دور' AS RV_MEANING FROM DUAL UNION SELECT 4 AS RV_LOW_VALUE, 'هوشمند کشاورزی' AS RV_MEANING FROM DUAL UNION SELECT 5 AS RV_LOW_VALUE, 'دستگاههاي هشدار دهنده' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[13].TableName = "Metr_Type";

            CyclStat_BindingSource.DataSource = OraPool.Tables["Cycl_Stat"];
            RqtpCode_BindingSource.DataSource = OraPool.Tables["Request_Type"];
            Region_BindingSource.DataSource = OraPool.Tables["Region"];
            RqttCode_BindingSource.DataSource = OraPool.Tables["Requester_Type"];
            RqstStat_BindingSource.DataSource = OraPool.Tables["Rqst_Stat"];
            ServType_BindingSource.DataSource = OraPool.Tables["Serv_Type"];
            ZoneCode_BindingSource.DataSource = OraPool.Tables["Zone_Type"];
            OnofTag_BindingSource.DataSource = OraPool.Tables["Onof_Tag"];
            ReadStat_BindingSource.DataSource = OraPool.Tables["Read_Stat"];
            ConsCode_BindingSource.DataSource = OraPool.Tables["Cons_Code"];
            YesNoBindingSource.DataSource = OraPool.Tables["Yes_No"];
            Phas_BindingSource.DataSource = OraPool.Tables["Phas"];
            BrncType_BindingSource.DataSource = OraPool.Tables["Brnc_Type"];
            MetrType_BindingSource.DataSource = OraPool.Tables["Metr_Type"];

            TCmb_Refresh.SelectedIndex = 1;
            Tc_Info.SelectTab(0);
            Txt_FileNo.Focus();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
