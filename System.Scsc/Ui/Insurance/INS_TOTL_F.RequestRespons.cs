using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Insurance
{
   partial class INS_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private XElement HostNameInfo;
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
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //if (tb_master.SelectedTab == tp_001)
               RqstBnARqt1_Click(null, null);
            
         }
         else if (keyData == Keys.Enter)
         {
            //if(!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            //if (tb_master.SelectedTab == tp_001)
               RqstBnDelete1_Click(null, null);            
         }
         else if (keyData == Keys.F5)
         {
            //if (tb_master.SelectedTab == tp_001)
               RqstBnARqt1_Click(null, null);            
         }
         else if (keyData == Keys.F3)
         {
            /*if (tabControl1.SelectedTab == tabPage1)
               LL_MoreInfo_LinkClicked(null, null);            */
         }
         else if (keyData == Keys.F10)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnASav1_Click(null, null);            
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();


         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;


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
                  case "insrinfo_lb":
                     InsrInfo_Lb.Text = control.LABL_TEXT;
                     //InsrInfo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InsrInfo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby_lb":
                     MdfyBy_Lb.Text = control.LABL_TEXT;
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymt_lb":
                     Pymt_Lb.Text = control.LABL_TEXT;
                     //Pymt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Pymt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashcode_clm":
                     CashCode_Clm.Caption = control.LABL_TEXT;
                     //CashCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CashCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rwnopdsc_clm":
                     RwnoPdsc_Clm.Caption = control.LABL_TEXT;
                     //RwnoPdsc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //RwnoPdsc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gb_info":
                     Gb_Info.Text = control.LABL_TEXT;
                     //Gb_Info.Text = control.LABL_TEXT; // ToolTip
                     //Gb_Info.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "filenos_lb":
                     FileNos_Lb.Text = control.LABL_TEXT;
                     //FileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_lb":
                     RqttCode_Lb.Text = control.LABL_TEXT;
                     //RqttCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqttCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "insrnumb_lb":
                     InsrNumb_Lb.Text = control.LABL_TEXT;
                     //InsrNumb_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InsrNumb_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "insrdate_lb":
                     InsrDate_Lb.Text = control.LABL_TEXT;
                     //InsrDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InsrDate_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "gb_expense":
                     Gb_Expense.Text = control.LABL_TEXT;
                     //Gb_Expense.Text = control.LABL_TEXT; // ToolTip
                     //Gb_Expense.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlamnt_lb":
                     TotlAmnt_Lb.Text = control.LABL_TEXT;
                     //TotlAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TotlAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dtilamnt_lb":
                     DtilAmnt_Lb.Text = control.LABL_TEXT;
                     //DtilAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DtilAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bn_paymentmethods":
                     bn_PaymentMethods.Text = control.LABL_TEXT;
                     //bn_PaymentMethods.Text = control.LABL_TEXT; // ToolTip
                     //bn_PaymentMethods.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sumexpnextrprct_clm":
                     SumExpnExtrPrct_Clm.Caption = control.LABL_TEXT;
                     //SumExpnExtrPrct_Clm.Text = control.LABL_TEXT; // ToolTip
                     //SumExpnExtrPrct_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expncode_clm":
                     ExpnCode_Clm.Caption = control.LABL_TEXT;
                     //ExpnCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ExpnCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnpric_clm":
                     ExpnPric_Clm.Caption = control.LABL_TEXT;
                     //ExpnPric_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ExpnPric_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnextrprct_clm":
                     ExpnExtrPrct_Clm.Caption = control.LABL_TEXT;
                     //ExpnExtrPrct_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ExpnExtrPrct_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "qnty_clm":
                     Qnty_Clm.Caption = control.LABL_TEXT;
                     //Qnty_Clm.Text = control.LABL_TEXT; // ToolTip
                     //Qnty_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlexpn_clm":
                     TotlExpn_Clm.Caption = control.LABL_TEXT;
                     //TotlExpn_Clm.Text = control.LABL_TEXT; // ToolTip
                     //TotlExpn_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amntpdsc_clm":
                     AmntPdsc_Clm.Caption = control.LABL_TEXT;
                     //AmntPdsc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //AmntPdsc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsdesc_clm":
                     PydsDesc_Clm.Caption = control.LABL_TEXT;
                     //PydsDesc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PydsDesc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtrwno_clm":
                     PymtRwno_Clm.Caption = control.LABL_TEXT;
                     //PymtRwno_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PymtRwno_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtamnt_clm":
                     PymtAmnt_Clm.Caption = control.LABL_TEXT;
                     //PymtAmnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PymtAmnt_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rcptmtod_clm":
                     RcptMtod_Clm.Caption = control.LABL_TEXT;
                     //RcptMtod_Clm.Text = control.LABL_TEXT; // ToolTip
                     //RcptMtod_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "actndate_clm":
                     ActnDate_Clm.Caption = control.LABL_TEXT;
                     //ActnDate_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ActnDate_Clm.Text = control.LABL_TEXT; // Place Holder
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
         #region Rqsw block
         RqttBs1.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "001" || rqtt.CODE == "004");
         DRcmtBs1.DataSource = iScsc.D_RCMTs;
         //Execute_Query();
         
         DSxtpBs1.DataSource = iScsc.D_SXTPs;
         //var iFighs = iScsc.VF_InsuranceFighter(DateTime.Now, 0).Select(f => f.FILE_NO);
         //FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && iFighs.Contains(f.FILE_NO) && Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101).OrderBy(f => f.FGPB_TYPE_DNRM);

         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;
         #endregion

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {         
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
            if (job.Input != null && (job.Input as XElement).Attribute("fileno") != null)
            {
               if ((job.Input as XElement).Attribute("type").Value == "renewinscard")
               {
                  FILE_NO_LookUpEdit.EditValue = Convert.ToInt64((job.Input as XElement).Attribute("fileno").Value);
                  RQTT_CODELookUpEdit.EditValue = "001";

                  RqstBnARqt1_Click(null, null);
               }
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
         try
         {
            XElement RcevXData = job.Input as XElement;

            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            var rqtpcode = rqst.RQTP_CODE;//RcevXData.Element("PosRespons").Attribute("rqtpcode").Value;
            var rqid = rqst.RQID;//RcevXData.Element("PosRespons").Attribute("rqid").Value;
            var fileno = rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO;//RcevXData.Element("PosRespons").Attribute("fileno").Value;
            var cashcode = rqst.Payments.FirstOrDefault().CASH_CODE;//RcevXData.Element("PosRespons").Element("Payment").Attribute("cashcode").Value;
            var amnt = Convert.ToInt64(RcevXData.Attribute("amnt").Value);
            var termno = RcevXData.Attribute("termno").Value;
            var tranno = RcevXData.Attribute("tranno").Value;
            var cardno = RcevXData.Attribute("cardno").Value;
            var flowno = RcevXData.Attribute("flowno").Value;
            var refno = RcevXData.Attribute("refno").Value;
            var actndate = RcevXData.Attribute("actndate").Value;

            if (regl.AMNT_TYPE == "002")
               amnt /= 10;

            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "CheckoutWithPOS"),
                  new XElement("Insert",
                     new XElement("Payment_Method",
                        new XAttribute("cashcode", cashcode),
                        new XAttribute("rqstrqid", rqid),
                        new XAttribute("amnt", amnt),
                        new XAttribute("termno", termno),
                        new XAttribute("tranno", tranno),
                        new XAttribute("cardno", cardno),
                        new XAttribute("flowno", flowno),
                        new XAttribute("refno", refno),
                        new XAttribute("actndate", actndate)
                     )
                  )
               )
            );

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstBnASav1_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
