using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission
{
   partial class ADM_MBFZ_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string formCaller;
      private bool isFirstLoaded = false;
      private string CurrentUser;
      private string RegnLang = "054";

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
               CheckSecurity(job);
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               LoadDataSource(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 20:
               Pay_Oprt_F(job);
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
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

            switch (formCaller)
            {
               case "ALL_FLDF_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", formCaller, 08 /* Exec LoadDataSource */, SendType.SelfToUserInterface)
                  );
                  break;
               default:
                  break;
            }

            formCaller = "";
            //job.Next =
            //   new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            
               Btn_RqstSav3_Click(null, null);
         }
         else if (keyData == Keys.Enter)
         {
            //if(!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_Cbmt1.Focused || Btn_Dise.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            
               Btn_RqstDelete3_Click(null, null);
         }
         else if (keyData == Keys.F5)
         {
            
               Btn_RqstRqt3_Click(null, null);
         }
         else if (keyData == Keys.F3)
         {
            
         }
         else if (keyData == Keys.F10)
         {
            
               Btn_RqstSav3_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.P))
         {
            RqstBnDefaultPrint_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Shift | Keys.P))
         {
            RqstBnPrint_Click(null, null);
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
         formCaller = job.Input != null ? job.Input.ToString() : "";

         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());
         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         #region Set Localization
         var regnlang = iScsc.V_User_Localization_Forms.Where(rl => rl.FORM_NAME == GetType().Name);
         if (regnlang.Count() > 0 && regnlang.First().REGN_LANG != RegnLang)
         {
            RegnLang = regnlang.First().REGN_LANG;
            // Ready To Change Text Title
            foreach (var control in regnlang)
            {
               switch (control.CNTL_NAME.ToLower())
               {
                  case "serv_gb":
                     Serv_Gb.Text = control.LABL_TEXT;
                     //Serv_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Serv_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "filenos_lb":
                     FileNos_Lb.Text = control.LABL_TEXT;
                     //FileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strtdate_lb":
                     StrtDate_Lb.Text = control.LABL_TEXT;
                     //StrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //StrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_lb":
                     EndDate_Lb.Text = control.LABL_TEXT;
                     //EndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "atendesc_lb":
                     AtenDesc_Lb.Text = control.LABL_TEXT;
                     //AtenDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AtenDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gb_rqst3":
                     Gb_Rqst3.Text = control.LABL_TEXT;
                     //Gb_Rqst3.Text = control.LABL_TEXT; // ToolTip
                     //Gb_Rqst3.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqid_lb":
                     Rqid_Lb.Text = control.LABL_TEXT;
                     //Rqid_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rqid_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby_lb":
                     CretBy_Lb.Text = control.LABL_TEXT;
                     //CretBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CretBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby_lb":
                     MdfyBy_Lb.Text = control.LABL_TEXT;
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpdesc_lb":
                     RqtpDesc_Lb.Text = control.LABL_TEXT;
                     //RqtpDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretdate_lb":
                     CretDate_Lb.Text = control.LABL_TEXT;
                     //CretDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CretDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfydate_lb":
                     MdfyDate_Lb.Text = control.LABL_TEXT;
                     //MdfyDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
               }
            }
         }
         #endregion


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
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
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
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         //**FighBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006" || f.FGPB_TYPE_DNRM == "009") && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 );
         if (isFirstLoaded) goto finishcommand;
         #region Rqsw block
         try
         {
            RqttBs1.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "001" || rqtt.CODE == "004");
            
            DSxtpBs1.DataSource = iScsc.D_SXTPs;
            
            isFirstLoaded = true;
         }            
         catch { }
         //Execute_Query();
         #endregion

         finishcommand:
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if (xinput.Attribute("enrollnumber") != null)
            {
               if (xinput.Attribute("type").Value == "block")
               {
                  if (RqstBs1.Count > 0 && (RqstBs1.Current as Data.Request).RQID > 0)
                     RqstBs1.AddNew();

                  Data.Fighter figh = null;
                  if (xinput.Attribute("enrollnumber").Value != "")
                     figh = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM == xinput.Attribute("enrollnumber").Value).FirstOrDefault();
                  else
                     figh = iScsc.Fighters.Where(f => f.FILE_NO == Convert.ToInt64(xinput.Attribute("fileno").Value)).FirstOrDefault();
                  FIGH_FILE_NOLookUpEdit.EditValue = figh.FILE_NO;
                  Btn_RqstRqt3_Click(null, null);

                  // 1396/11/04
                  if (xinput.Attribute("formcaller") != null)
                     formCaller = xinput.Attribute("formcaller").Value;
                  else
                     formCaller = "";
               }
            }
            else if (xinput.Attribute("type").Value == "rqidfocus")
            {
               ShowRqst_PickButn.PickChecked = false;
               RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.RQID == Convert.ToInt64(xinput.Attribute("rqid").Value)));
            }
            else
               Execute_Query();
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void Pay_Oprt_F(Job job)
      {
         XElement RcevXData = job.Input as XElement;

         var rqtpcode = RcevXData.Element("Request").Attribute("rqtpcode").Value;
         var rqid = RcevXData.Element("Request").Attribute("rqid").Value;
         var fileno = RcevXData.Element("Request").Attribute("fileno").Value;
         var cashcode = RcevXData.Element("Request").Element("Payment").Attribute("cashcode").Value;
         var amnt = RcevXData.Element("Request").Element("Payment").Attribute("amnt").Value;
         var termno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("termno").Value;
         var cardno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("cardno").Value;
         var flowno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("flowno").Value;
         var refno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("refno").Value;
         var actndate = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("actndate").Value;

         // ثبت نام
         if(rqtpcode == "001")
         {
            iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", cashcode),
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("amnt", amnt),
                           new XAttribute("termno", termno),
                           new XAttribute("cardno", cardno),
                           new XAttribute("flowno", flowno),
                           new XAttribute("refno", refno),
                           new XAttribute("actndate", actndate)
                        )
                     )
                  )
               );

            /* Loop For Print After Pay */
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", rqid))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            /* End Request */
            try
            {
               iScsc.ADM_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", rqid),
                        new XElement("Fighter",
                           new XAttribute("fileno", fileno)
                        ),
                        new XElement("Payment",
                           iScsc.Payment_Details.Where(pd => pd.PYMT_CASH_CODE == Convert.ToInt64(cashcode) && pd.PYMT_RQST_RQID == Convert.ToInt64(rqid)).ToList()
                           .Select(pd =>
                              new XElement("Payment_Detail",
                                 new XAttribute("code", pd.CODE),
                                 new XAttribute("rcptmtod", "003")
                              )
                           )
                        )
                     )
                  )
               );
               requery = true;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            finally
            {
               if (requery)
               {
                  Get_Current_Record();
                  Execute_Query();
                  Set_Current_Record();
                  Create_Record();
                  requery = false;
               }
            }
         }
         else if(rqtpcode == "009")
         {
            iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", cashcode),
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("amnt", amnt),
                           new XAttribute("termno", termno),
                           new XAttribute("cardno", cardno),
                           new XAttribute("flowno", flowno),
                           new XAttribute("refno", refno),
                           new XAttribute("actndate", actndate)
                        )
                     )
                  )
               );

            /* Loop For Print After Pay */
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_003_F"), string.Format("Request.Rqid = {0}", rqid))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            /* End Request */
            try
            {               
               iScsc.UCC_TSAV_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", rqid)
                     )
                  )
               );
               requery = true;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            finally
            {
               if (requery)
               {
                  Get_Current_Record();
                  Execute_Query();
                  Set_Current_Record();
                  Create_Record();
                  requery = false;
               }
            }
         }

         job.Status = StatusType.Successful;
      }
   }
}
