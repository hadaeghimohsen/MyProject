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
   partial class SERV_DART_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private string ConnectionString;
      private OracleDataAdapter OraDA;
      private OracleTransaction OraTra;
      private DataSet OraPool;
      private string FileNo;
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
               new Job(SendType.SelfToUserInterface, "SERV_DART_F", 04 /* Execute UnPaint */);
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
         domain = new Model.Domain(ConnectionString);
         access_entity = new Model.Access_Entity(ConnectionString);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:SERV_DART_F", this }  },
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
            DEBTPBindingSource.DataSource = domain.DEBTP();
            AMACTBindingSource.DataSource = domain.AMACT();

            BIL_DEBITBindingSource.DataSource = access_entity.Run_Qury_U(string.Format("SELECT * FROM Bil_Debit WHERE Serv_File_No = {0}", FileNo));
            BIL_ACCOUNTBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT Serv_File_No, Rwno,Bill_No_Dnrm,Regn_Code,Actn_Code,DECODE (Debt_Stat, 1, Amnt, NULL) AS Bed_Amnt,DECODE (Debt_Stat, 2, Amnt, NULL) AS Bes_Amnt,Debt_Stat,Remn_Dnrm, Lett_No_Dnrm,Lett_Date_Dnrm, Amnt_Date FROM Bil_Account WHERE Serv_File_No = {0} ORDER BY Rwno DESC", FileNo));
            BIL_RCPT_ROW_ANNOUNCEMENTBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT B5.Code AS RCPA_BNKB_BANK_CODE,B5.Bank_Desc,B4.Code AS RCPA_BNKB_CODE,B4.NAME AS Brnc_Desc,B3.File_Name AS RCPN_FILE_NAME,B2.Rpan_Updt_Date AS RCPA_RPAN_UPDT_DATE ,B1.Rcpa_Rcpn_Rcpt_Seq,B1.Rcpa_Annc_Date,B1.Bill_Bill_No,B1.Rwno,B1.Rcpt_Amnt,B1.Insm_Ser_Dnrm FROM Bil_Rcpt_Row_Announcement B1,Bil_Rcpt_Announcement B2,Bil_Rcpt_Name B3,Bank_Branch B4,Bank B5 WHERE B1.Rcpa_Rcpn_Rcpt_Seq = B2.Rcpn_Rcpt_Seq AND B1.Rcpa_Annc_Date = B2.Annc_Date AND B1.Rcpa_Bnkb_Bank_Code = B2.Bnkb_Bank_Code AND B1.Rcpa_Bnkb_Code = B2.Bnkb_Code AND B2.Rcpn_Rcpt_Seq = B3.Rcpt_Seq AND B2.Bnkb_Bank_Code = B4.Bank_Code AND B2.Bnkb_Code = B4.Code AND B4.Bank_Code = B5.Code AND B1.Bill_Serv_File_No = {0} AND B1.Actn_Stat = '1' ORDER BY B1.Rcpa_Rcpn_Rcpt_Seq DESC", FileNo));

         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
