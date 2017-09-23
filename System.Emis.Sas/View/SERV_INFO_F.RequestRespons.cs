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
   partial class SERV_INFO_F : ISendRequest
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
               new Job(SendType.SelfToUserInterface, "SERV_INFO_F", 04 /* Execute UnPaint */);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Sas:SERV_INFO_F", this }  },
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
            CYCL_STATBindingSource.DataSource = domain.CYCST();
            ZONE_CODEBindingSource.DataSource = domain.ZONE();
            YES_NOBindingSource.DataSource = domain.YESNO();
            ONOF_TAGBindingSource.DataSource = domain.OFTAG();
            CLMT_TYPEBindingSource.DataSource = domain.CLMT();
            CONT_STATBindingSource.DataSource = domain.CONST();
            OWNR_TYPEBindingSource.DataSource = domain.OWNR();
            BILL_LCTNBindingSource.DataSource = domain.BLCTN();
            SERVBindingSource.DataSource = domain.SERV();
            REDSTBindingSource.DataSource = domain.REDST();
            CONSBindingSource.DataSource = domain.CONS();
            MTARFBindingSource.DataSource = domain.MTARF();
            FRIDBindingSource.DataSource = domain.FRID();
            PHASBindingSource.DataSource = domain.PHAS();
            VOLTBindingSource.DataSource = domain.VRANG();
            VOLT_TYPEBindingSource.DataSource = domain.VOLT();
            EXISTBindingSource.DataSource = domain.EXIST();
            MTYPEBindingSource.DataSource = domain.MTYPE();
            BRCTPBindingSource.DataSource = domain.BRCTP();
            HACT2BindingSource.DataSource = domain.HACT2();
            METRBindingSource.DataSource = domain.METR();
            CLIMTBindingSource.DataSource = domain.CLIMT();
            DVOLTBindingSource.DataSource = domain.DVOLT();
            PRANGBindingSource.DataSource = domain.PRANG();
            ITEMBindingSource.DataSource = domain.ITEM();
            SAS_PUBLICBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM Sas_Public WHERE Serv_File_No = {0} AND Rect_Code = 6 ORDER BY Rwno", FileNo));
            SERVICEBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT * FROM Service WHERE File_No = {0} ", FileNo));
            SERVICE_TARIFFBindingSource.DataSource = access_entity.Run_Qury_U(string.Format(@"SELECT serv_file_no, rwno, regn_code, tarf_tfid, strt_date_dnrm, chng_date,lett_no, lett_date FROM Service_Tariff WHERE Serv_File_No = {0} ORDER BY RWNO", FileNo));
            
            SERVICE_TARIFFBindingSource.MoveLast();
            SAS_PUBLICBindingSource.MoveLast();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
