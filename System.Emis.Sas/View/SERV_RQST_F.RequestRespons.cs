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
   partial class SERV_RQST_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private string ConnectionString;
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
               new Job(SendType.SelfToUserInterface, "SERV_RQST_F", 04 /* Execute UnPaint */);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:SERV_RQST_F", this }  },
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
            REQTPBindingSource.DataSource = domain.REQTP();
            ROWNBindingSource.DataSource = domain.ROWN();

            RqstBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"
SELECT RQID,
   RQTP_CODE,
   RQTP_TYPE,
   RQTP_DESC,
   RWNO,
   LETT_NO,
   LETT_DATE,
   AGRE_DATE,
   NEW_OLD,
   SERV_FILE_NO 
FROM(

SELECT rqid,
          request_row.rqtp_code,
          request_type.rqtp_type,
          rqtp_desc,
          request_row.rwno,
          lett_no,
          lett_date,
          request.save_date agre_date,
          request.new_old,
          request_row.serv_file_no
     FROM request, request_row, request_type
    WHERE request.rqid = request_row.rqst_rqid
      AND request_row.rqtp_code = request_type.code
      AND request_type.code NOT IN (5, 6, 8, 9, 48, 22)
      AND request_row.rec_stat = '1'
      AND ( (request_row.sstt_mstt_row_no = 99
         AND request_row.sstt_row_no = 99
         AND request.cmps_rqst = '1')
        OR  (request_row.sstt_mstt_row_no <> 99
         AND request_row.sstt_row_no <> 99
         AND request.sell_type = '2'
         AND request.rqtp_code = 38))
      AND request_row.rqst_stat = 2
   UNION
   SELECT rqid,
          request_row.rqtp_code,
          request_type.rqtp_type,
          rqtp_desc,
          request_row.rwno,
          lett_no,
          lett_date,
          --request_row.mdfy_date agre_date, request.new_old,
          request_row.SAVE_date agre_date,
          request.new_old,
          request_row.serv_file_no
     FROM request, request_row, request_type
    WHERE request.rqid = request_row.rqst_rqid
      AND request_row.rqtp_code = request_type.code
      AND request_type.code IN (5, 6, 8, 9, 48)
      AND request_row.rec_stat = '1'
      AND ( (request_row.sstt_mstt_row_no = 99
         AND request_row.sstt_row_no = 99        --and request.Cmps_Rqst = '1'
                                         )
        OR  (request_row.sstt_mstt_row_no <> 99
         AND request_row.sstt_row_no <> 99
         AND request.sell_type = '2'
         AND request.rqtp_code = 38))
      AND request_row.rqst_stat = 2
   UNION
   SELECT rqid,
          request.rqtp_code,
          request_type.rqtp_type,
          rqtp_desc,
          bil_change_debt.rwno,
          lett_no,
          lett_date,
          request.save_date agre_date,
          request.new_old,
          bil_change_debt.serv_file_no
     FROM request, bil_change_debt, request_type
    WHERE request.rqid = bil_change_debt.rqst_rqid
      AND request.rqtp_code = request_type.code
      AND request.sstt_mstt_row_no = 99
      AND request.sstt_row_no = 99
      AND bil_change_debt.rqst_stat = 2
   UNION
   SELECT rqid,
          request.rqtp_code,
          request_type.rqtp_type,
          rqtp_desc,
          bil_prepaid.rwno,
          lett_no,
          lett_date,
          request.save_date agre_date,
          request.new_old,
          bil_prepaid.serv_file_no
     FROM request, bil_prepaid, request_type
    WHERE request.rqid = bil_prepaid.rqst_rqid
      AND request.rqtp_code = request_type.code
      AND request.sstt_mstt_row_no = 99
      AND request.sstt_row_no = 99
      AND bil_prepaid.rqst_stat = 2
   UNION
   SELECT rq.rqid,
          rq.rqtp_code,
          rt.rqtp_type,
          rt.rqtp_desc,
          1 rwno,
          rq.lett_no,
          rq.lett_date,
          rq.save_date agre_date,
          rq.new_old,
          rr.serv_file_no
     FROM request rq,
          request_type rt,
          installment_request ir,
          installment_row rr
    WHERE rq.rqid = ir.rqst_rqid
      AND rq.rqtp_code = rt.code
      AND ir.cycl_year = rr.inrq_cycl_year
      AND ir.regn_code = rr.inrq_regn_code
      AND ir.code = rr.inrq_code
      AND rq.rqtp_code = 52
      AND ir.inrq_type = '2'
      AND rq.sstt_mstt_row_no = 99
      AND rq.sstt_row_no = 99
      AND rq.rqst_stat = '2') T
WHERE T.Serv_File_No = {0}", FileNo));
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
