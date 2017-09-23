using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Gas.Gnrt_Qrcd.Ui
{
   partial class RPRT_PBLC_F
   {
      partial void be_open_fldr_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if (fbd_show.ShowDialog() != Windows.Forms.DialogResult.OK)
            return;

         be_open_fldr.Text = fbd_show.SelectedPath;
      }

      string userName = "", password = "";
      long datasourceID;
      partial void cbe_databaseservers_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            if (cbe_databaseservers.Text.Trim() == "")
               return;
            if (cbe_databaseservers.Text.Contains(","))
               return;
            
            datasourceID = (long)cbe_databaseservers.Properties.GetItems().OfType<CheckedListBoxItem>().Where(item => item.CheckState == Windows.Forms.CheckState.Checked).First().Value;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odc */, SendType.Self)
               {
                  Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  string.Format(@"<Request type=""Single""><DataSource id=""{0}""/></Request>", datasourceID),
                  "{ CALL Report.GetDataSource(?) }",
                  "iProject",
                  "scott"
               },
                  AfterChangedOutput = new Action<object>(
                     (output) =>
                     {
                        DataSet ds = output as DataSet;
                        var xData = XDocument.Parse(ds.Tables["DataSource"].Rows[0]["XmlData"].ToString());
                        userName = xData.Descendants("DataSource").First().Attribute("user").Value;
                        password = xData.Descendants("DataSource").First().Attribute("password").Value;
                     })
               }
            );

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
               {
                  Input = new List<object>
               {
                  false,
                  "text",
                  false,
                  true,
                  "",
                  "",
                  "SELECT DISTINCT Seri_No FROM GAZ_SERI ORDER BY Seri_No DESC",
                  string.Format("DB_{0}", datasourceID),
                  userName,
                  password
               },
                  AfterChangedOutput = new Action<object>(
                     (output) =>
                     {
                        DataSet ds = output as DataSet;
                        cbe_seri_no.Properties.DataSource = ds.Tables[0];
                        cbe_seri_no.Properties.DisplayMember = "SERI_NO";
                        cbe_seri_no.Properties.ValueMember = "SERI_NO";
                     })
               }
            );
         }
         catch { }
      }

      partial void cbe_databaseservers_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Tag.ToString())
         {
            case "0":
               cbe_databaseservers.Text = "";
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "RPRT_PBLC_F", 08 /* Execute LoadDatabaseServers */, SendType.SelfToUserInterface)
               );
               break;
         }
      }

      partial void cbe_seri_no_buttonclick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         switch (e.Button.Tag.ToString())
         {
            case "0":
               cbe_seri_no.Text = "";
               cbe_databaseservers_EditValueChanged(null, null);
               rb_qr_desc_type_CheckedChanged(null, null);
               break;
            default:
               break;
         }
      }

      partial void rb_qr_desc_type_CheckedChanged(object sender, EventArgs e)
      {
         if (rb_qr_desc_type.Checked)
         {
            if (cbe_seri_no.Text.Trim() == "")
               return;
            if (cbe_seri_no.Text.Contains(","))
               return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
               {
                  Input = new List<object>
                  {
                     false, 
                     "text",
                     false,
                     true,
                     "",
                     "",
                     string.Format("SELECT QRCD_DESC FROM GAZ_SERI WHERE Seri_No = {0} AND QRCD_DESC IS NOT NULL AND ROWNUM = 1", cbe_seri_no.Text),
                     string.Format("DB_{0}", datasourceID),
                     userName, 
                     password
                  },
                  AfterChangedOutput = new Action<object>(
                     (output) =>
                     {
                        try
                        {
                           DataSet ds = output as DataSet;
                           tb_qr_desc.Text = ds.Tables[0].Rows[0]["QRCD_DESC"].ToString();
                        }
                        catch { }
                     })
               }
            );
         }
         else
         {
            tb_qr_desc.Text = "";
         }
      }

      partial void tb_qr_desc_TextChanged(object sender, EventArgs e)
      {
         bc_qrcode.Text = tb_qr_desc.Text;
      }

      partial void wbp_book_mark_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Tag.ToString())
         {
            case "0":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "RPRT_PBLC_F", 04 /* Execute UnPaint */, SendType.SelfToUserInterface)
               );
               break;
            case "1":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {
                              #region Access Privilege
                              new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                              {
                                 Input = new List<string> 
                                 {
                                    "<Privilege>2</Privilege><Sub_Sys>3</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Show Error
                                    Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                                    {
                                       Input = @"<HTML>
                                                   <body>
                                                      <p style=""float:right"">
                                                         <ol>
                                                            <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                            <ul>
                                                               <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                            </ul>
                                                         </ol>
                                                      </p>
                                                   </body>
                                                   </HTML>"
                                    };
                                    _DefaultGateway.Gateway(_ShowError);
                                    #endregion                           
                                 })
                              },
                              #endregion                              
                           }
                        ),
                        new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 09 /* Execute SaveQRCodePicture */),
                        new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 10 /* Execute SaveQRCodeDatabase */)
                    })
                );
               break;
            case "2":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {
                              #region Access Privilege
                              new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                              {
                                 Input = new List<string> 
                                 {
                                    "<Privilege>2</Privilege><Sub_Sys>3</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Show Error
                                    Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                                    {
                                       Input = @"<HTML>
                                                   <body>
                                                      <p style=""float:right"">
                                                         <ol>
                                                            <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                            <ul>
                                                               <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                            </ul>
                                                         </ol>
                                                      </p>
                                                   </body>
                                                   </HTML>"
                                    };
                                    _DefaultGateway.Gateway(_ShowError);
                                    #endregion                           
                                 })
                              },
                              #endregion
                              #region Access Privilege
                              new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                              {
                                 Input = new List<string> 
                                 {
                                    "<Privilege>3</Privilege><Sub_Sys>3</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Show Error
                                    Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                                    {
                                       Input = @"<HTML>
                                                   <body>
                                                      <p style=""float:right"">
                                                         <ol>
                                                            <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                            <ul>
                                                               <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                            </ul>
                                                         </ol>
                                                      </p>
                                                   </body>
                                                   </HTML>"
                                    };
                                    _DefaultGateway.Gateway(_ShowError);
                                    #endregion                           
                                 })
                              },
                              #endregion                              
                           }
                        ),
                        new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 09 /* Execute SaveQRCodePicture */),
                        new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 10 /* Execute SaveQRCodeDatabase */),
                        new Job(SendType.SelfToUserInterface, "RPRT_PBLC_F", 11 /* Execute GotoPrintBill */)
                     })
                );
               break;
            case "3":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {                        
                        new Job(SendType.External, "Commons",
                           new List<Job>
                           {                              
                              #region Access Privilege
                              new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                              {
                                 Input = new List<string> 
                                 {
                                    "<Privilege>1</Privilege><Sub_Sys>4</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Show Error
                                    Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                                    {
                                       Input = @"<HTML>
                                                   <body>
                                                      <p style=""float:right"">
                                                         <ol>
                                                            <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                            <ul>
                                                               <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                            </ul>
                                                         </ol>
                                                      </p>
                                                   </body>
                                                   </HTML>"
                                    };
                                    _DefaultGateway.Gateway(_ShowError);
                                    #endregion                           
                                 })
                              },
                              #endregion    
                              #region Get Form Reports
                              new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                              {
                                 Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    true,
                                    "xml",
                                    string.Format(@"<Request type=""GetCurrentFormReports"">{0}</Request>", XPublic.ToString()),
                                    "{ CALL Global.GetFormReport(?) }",
                                    "iProject",
                                    "scott"
                                 }
                              },
                              #endregion
                              #region Show Form Settings
                              new Job(SendType.Self, 21 /* Execute DoWork4Form_Stng */){WhereIsInputData = WhereIsInputDataType.StepBack}
                              #endregion
                           }
                        ){GenerateInputData = GenerateDataType.Dynamic}
                    })
                );
               break;
            default:
               break;
         }
      }

      private void FaKeyboard()
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* LangChangToFarsi */, SendType.Self));
      }
   }
}
