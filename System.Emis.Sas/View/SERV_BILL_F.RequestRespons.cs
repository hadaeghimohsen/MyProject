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
using System.Xml.Linq;

namespace System.Emis.Sas.View
{
   partial class SERV_BILL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private string ConnectionString;
      private OracleDataAdapter OraDA;
      private OracleTransaction OraTra;
      private DataSet OraPool;
      private string FileNo;
      private enum TableIndex
      {
         BILL_TYPE = 0,
         BILL_STAT ,
         MTARF ,
         RMTD ,
         HILO,
         RMETD,
         DEBTP,
         BAMNT,
         SEAS,
         EXCD,
         YESNO,
         FRID ,
         REGION ,
         BILL,
         BIL_PREVENT_CODE
      };      

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
               new Job(SendType.SelfToUserInterface, "SERV_BILL_F", 04 /* Execute UnPaint */);
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
         /*var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 , SendType.Self) { Input = string.Format(@"<Database value=""id"">{0}</Database><Dbms>Oracle</Dbms>", job.Input) };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();*/
         ConnectionString = (job.Input as XElement).Attribute("datasource").Value;
         FileNo = (job.Input as XElement).Attribute("fileno").Value;

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:SERV_BILL_F", this }  },
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
            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D BILTP' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.BILL_TYPE].TableName = "BILL_TYPE";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D BILST' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.BILL_STAT].TableName = "BILL_STAT";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D MTARF' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.MTARF].TableName = "MTARF";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D RMTD' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.RMTD].TableName = "RMTD";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D HILO' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.HILO].TableName = "HILO";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D RMETD' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.RMETD].TableName = "RMETD";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D DEBTP' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.DEBTP].TableName = "DEBTP";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D BAMNT' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.BAMNT].TableName = "BAMNT";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D SEAS' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.SEAS].TableName = "SEAS";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Cg_Ref_Codes WHERE RV_DOMAIN = 'D EXCD' ORDER BY RV_LOW_VALUE";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.EXCD].TableName = "EXCD";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'خیر' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'بلی' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.YESNO].TableName = "YESNO";

            OraDA.SelectCommand.CommandText = "SELECT 1 AS RV_LOW_VALUE, 'معمولی' AS RV_MEANING FROM DUAL UNION SELECT 2 AS RV_LOW_VALUE, 'جمعه' AS RV_MEANING FROM DUAL";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.FRID].TableName = "FRID";

            OraDA.SelectCommand.CommandText = "SELECT * FROM Region ORDER BY Code";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.REGION].TableName = "REGION";

            OraDA.SelectCommand.CommandText = string.Format(@"SELECT *
                                                                FROM bill
                                                               WHERE serv_file_no = {0}
                                                                 /*AND bill_vlid = '2'
                                                                 AND bill_type <> '0'
                                                                 AND bill_stat >= '4'
                                                                 AND bill_stat NOT IN ('8', 'A')
                                                                 AND prcd_code IN (SELECT code
                                                                                    FROM bil_prevent_code
                                                                                    WHERE actn_code IN ('1', '2', '5'))*/
                                                            ORDER BY Bill_No DESC", FileNo);
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.BILL].TableName = "BILL";

            OraDA.SelectCommand.CommandText = @"SELECT * FROM bil_prevent_code";
            OraDA.Fill(OraPool);
            OraPool.Tables[(int)TableIndex.BIL_PREVENT_CODE].TableName = "BIL_PREVENT_CODE";

            BILL_TYPE_BindingSource.DataSource = OraPool.Tables["BILL_TYPE"];
            BILL_STAT_BindingSource.DataSource = OraPool.Tables["BILL_STAT"];
            HILO_BindingSource.DataSource = OraPool.Tables["HILO"];
            YESNO_BindingSource.DataSource = OraPool.Tables["YESNO"];
            FRID_BindingSource.DataSource = OraPool.Tables["FRID"];
            MTARF_BindingSource.DataSource = OraPool.Tables["MTARF"];
            RMTD_BindingSource.DataSource = OraPool.Tables["RMTD"];
            RMETD_BindingSource.DataSource = OraPool.Tables["RMETD"];
            DEBTP_BindingSource.DataSource = OraPool.Tables["DEBTP"];
            BAMNT_BindingSource.DataSource = OraPool.Tables["BAMNT"];
            SEAS_BindingSource.DataSource = OraPool.Tables["SEAS"];
            EXCD_BindingSource.DataSource = OraPool.Tables["EXCD"];
            BILL_BindingSource.DataSource = OraPool.Tables["BILL"];
            BIL_PREVENT_CODE_BindingSource.DataSource = OraPool.Tables["BIL_PREVENT_CODE"];

            Tc_Bills.SelectTab(0);            
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
