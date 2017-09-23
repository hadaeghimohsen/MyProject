using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Gas.Gnrt_Qrcd.Ui
{
   partial class RPRT_PBLC_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
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
               LoadDatabaseServers(job);
               break;
            case 09:
               SaveQRCodePicture(job);
               break;
            case 10:
               SaveQRCodeDatabase(job);
               break;
            case 11:
               GotoPrintBill(job);
               break;
            default:
               job.Status = StatusType.Failed;
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
               new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 04 /* Execute UnPaint */);
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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { "Gas:Gnrt_Qrcd:RPRT_PBLC_F", this }  },
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
         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         FaKeyboard();
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 08 /* Execute LoadDatabaseServers */),                  
               })
         );
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadDatabaseServers(Job job)
      {
         List<string> dsnList = null;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 20 /* Execute DoWork4GetRegisterOdbcDatasource */, SendType.Self)
            {
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     dsnList = output as List<string>;
                  })
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               #region Input
               Input = new List<object>
               {
                  false,
                  "procedure",
                  false,
                  true,
                  "",
                  "",
                  "{ Call Report.LoadDataSources }",
                  "iProject",
                  "scott"
               },
               #endregion
               #region After Changed Output
               AfterChangedOutput = new Action<object>(
                  (output) =>
                  {
                     DataSet ds = output as DataSet;
                     ds.Tables["Datasources"].Columns.Add(
                        new DataColumn("isValid", typeof(bool)) { DefaultValue = false });
                     var query = from dsn in dsnList
                                 join dbcs in ds.Tables["Datasources"].Rows.OfType<DataRow>()
                                    on dsn equals string.Format("Db_{0}", dbcs["ID"])
                                 select dbcs;
                     query.ToList().ForEach(c => c["isValid"] = true);

                     ds.Tables["Datasources"].Rows.OfType<DataRow>().Where(c => (!Convert.ToBoolean(c["isValid"]))).ToList().ForEach(c => c.Delete());
                     cbe_databaseservers.Properties.DataSource = ds.Tables["Datasources"];
                     cbe_databaseservers.Properties.DisplayMember = "TitleFa";
                     cbe_databaseservers.Properties.ValueMember = "ID";                        
                  })
               #endregion
            });
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void SaveQRCodePicture(Job job)
      {
         
         if (cbe_databaseservers.Text.Trim() == "")
            return;
         if (cbe_databaseservers.Text.Contains(","))
            return;

         if (cbe_seri_no.Text.Trim() == "")
            return;
         if (cbe_seri_no.Text.Contains(","))
            return;

         if (be_open_fldr.Text.Trim() == "")
            return;

         mpbc_saving.Visible = true;         
         Bitmap Bmp = new Bitmap(184, 167);
         bc_qrcode.DrawToBitmap(Bmp, bc_qrcode.DisplayRectangle);
         Bmp.Save( be_open_fldr.Text + "\\QR.BMP");

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void SaveQRCodeDatabase(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  false,
                  "xml",
                  string.Format(@"<Request type=""public""><Bill seri_no=""{0}""><QrCode><Desc>{1}</Desc><FilePath>{2}</FilePath></QrCode></Bill></Request>", cbe_seri_no.Text, tb_qr_desc.Text, be_open_fldr.Text + "\\QR.BMP"),
                  "{ CALL Qrcd_Bill_P.SetX_Pblc_P(?) }",
                  string.Format("DB_{0}", datasourceID),
                  userName,
                  password
               }
            }
         );
         mpbc_saving.Visible = false;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void GotoPrintBill(Job job)
      {
         #region Get Current User Name And Call GetAccessReports Procedure
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */)
                        {
                           AfterChangedOutput = new Action<object>(
                              (output) =>
                              {
                                 #region Prepare Execute Procedure
                                 _DefaultGateway.Gateway(
                                    new Job(SendType.External, "Localhost",
                                       new List<Job>
                                       {
                                          new Job(SendType.External, "Commons",
                                             new List<Job>
                                             {
                                                new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                                                {
                                                   #region Execute Procedure
                                                   Input = new List<object>
                                                   {
                                                      false,
                                                      "procedure",
                                                      true,
                                                      true,
                                                      "xml",
                                                      string.Format(@"<Request type=""GetReportRwno""><User>{0}</User><Report rwno=""1""/>{1}</Request>", output.ToString(), XPublic.ToString()),
                                                      "{ Call Global.[GetFormReport](?) }",
                                                      "iProject",
                                                      "scott"
                                                   }, 
                                                   AfterChangedOutput = new Action<object>(
                                                      (xoutput) =>
                                                         {  
                                                            try
                                                            {
                                                               DataSet ds = xoutput as DataSet;
                                                               XPrivate = XDocument.Parse(ds.Tables["RunReport"].Rows[0][0].ToString().Replace("<RunReports>", string.Empty).Replace("</RunReports>", string.Empty));
                                                               _DefaultGateway.Gateway(
                                                                  new Job(SendType.External, "Localhost", "DefaultGateway:DefaultGateway:Reporting:WorkFlow", 05 /* DoWork4WHR_SCON_F */, SendType.Self)
                                                                  {
                                                                     Input = XPrivate
                                                                  }
                                                               );
                                                            }
                                                            catch{}
                                                         })
                                                   #endregion
                                                }
                                             })
                                       }));                                       
                                 #endregion
                              })
                        }
                     })
               }));
         #endregion
         job.Status = StatusType.Successful;
      }
   }
}
