using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.PaymentMethod
{
   partial class PAY_MTOD_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string CallerForm;
      private string TabFocued;
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
            case 06:
               break;
            case 07:
               LoadData(job);
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

            if(CallerForm != null && CallerForm != "")
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost", CallerForm, 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface) 
                  { 
                     Input = 
                        new XElement("Process",
                           new XAttribute("type", "refresh"),
                           new XAttribute("tabfocued", TabFocued ?? "")
                        )
                  }
               );
            }

            //job.Next =
            //   new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            
         }
         else if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            
         }
         else if (keyData == Keys.F8)
         {
            
         }
         else if (keyData == Keys.F5)
         {
            
         }
         else if (keyData == Keys.F3)
         {
          
         }
         else if (keyData == Keys.F10)
         {
          
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
                  case "chekcancel_butn":
                     ChekCancel_Butn.Text = control.LABL_TEXT;
                     //ChekCancel_Butn.Text = control.LABL_TEXT; // ToolTip
                     //ChekCancel_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chktype_lb":
                     ChkType_Lb.Text = control.LABL_TEXT;
                     //ChkType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChkType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expncode_clm":
                     ExpnCode_Clm.Caption = control.LABL_TEXT;
                     //ExpnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "adqexpnpric_clm":
                     AdqExpnPric_Clm.Caption = control.LABL_TEXT;
                     //AdqExpnPric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AdqExpnPric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chekdate_lb":
                     ChekDate_Lb.Text = control.LABL_TEXT;
                     //ChekDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChekDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chekno_lb":
                     ChekNo_Lb.Text = control.LABL_TEXT;
                     //ChekNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChekNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "titlform_lb":
                     TitlForm_Lb.Text = control.LABL_TEXT;
                     //TitlForm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TitlForm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_001":
                     tp_001.Text = control.LABL_TEXT;
                     //tp_001.Text = control.LABL_TEXT; // ToolTip
                     //tp_001.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_003":
                     tp_003.Text = control.LABL_TEXT;
                     //tp_003.Text = control.LABL_TEXT; // ToolTip
                     //tp_003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_004":
                     tp_004.Text = control.LABL_TEXT;
                     //tp_004.Text = control.LABL_TEXT; // ToolTip
                     //tp_004.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debt_gb":
                     Debt_Gb.Text = control.LABL_TEXT;
                     //Debt_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Debt_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashcode_lb":
                     CashCode_Lb.Text = control.LABL_TEXT;
                     //CashCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CashCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlamnt_lb":
                     TotlAmnt_Lb.Text = control.LABL_TEXT;
                     //TotlAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TotlAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "remnamnt_lb":
                     RemnAmnt_Lb.Text = control.LABL_TEXT;
                     //RemnAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RemnAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "savedifferenceamnt_butn":
                     SaveDifferenceAmnt_Butn.Text = control.LABL_TEXT;
                     //SaveDifferenceAmnt_Butn.Text = control.LABL_TEXT; // ToolTip
                     //SaveDifferenceAmnt_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "savediscountamnt_butn":
                     SaveDiscountAmnt_Butn.Text = control.LABL_TEXT;
                     //SaveDiscountAmnt_Butn.Text = control.LABL_TEXT; // ToolTip
                     //SaveDiscountAmnt_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnt_lb":
                     Amnt_Lb.Text = control.LABL_TEXT;
                     //Amnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Amnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rcptmtod_lb":
                     RcptMtod_Lb.Text = control.LABL_TEXT;
                     //RcptMtod_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RcptMtod_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtrwno_clm":
                     PymtRwno_Clm.Caption = control.LABL_TEXT;
                     //PymtRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PymtRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnt_clm":
                     Amnt_Clm.Caption = control.LABL_TEXT;
                     //Amnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Amnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rcptmtod_clm":
                     RcptMtod_Clm.Caption = control.LABL_TEXT;
                     //RcptMtod_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RcptMtod_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "termno_lb":
                     TermNo_Lb.Text = control.LABL_TEXT;
                     //TermNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TermNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tranno_lb":
                     TranNo_Lb.Text = control.LABL_TEXT;
                     //TranNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TranNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cardno_lb":
                     CardNo_Lb.Text = control.LABL_TEXT;
                     //CardNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CardNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bank_lb":
                     Bank_Lb.Text = control.LABL_TEXT;
                     //Bank_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Bank_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "flowno_lb":
                     FlowNo_Lb.Text = control.LABL_TEXT;
                     //FlowNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FlowNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "refno_lb":
                     RefNo_Lb.Text = control.LABL_TEXT;
                     //RefNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RefNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "actndate_lb":
                     ActnDate_Lb.Text = control.LABL_TEXT;
                     //ActnDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ActnDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsrwno_clm":
                     PydsRwno_Clm.Caption = control.LABL_TEXT;
                     //PydsRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydsRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsamnt_clm":
                     PydsAmnt_Clm.Caption = control.LABL_TEXT;
                     //PydsAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydsAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnttype_clm":
                     AmntType_Clm.Caption = control.LABL_TEXT;
                     //AmntType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AmntType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsstat_clm":
                     PydsStat_Clm.Caption = control.LABL_TEXT;
                     //PydsStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydsStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsdesc_lb":
                     PydsDesc_Lb.Text = control.LABL_TEXT;
                     //PydsDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydsDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "adqexpncode_clm":
                     AdqExpnCode_Clm.Caption = control.LABL_TEXT;
                     //AdqExpnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AdqExpnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "adqexpnextrprct_clm":
                     AdqExpnExtrPrct_Clm.Caption = control.LABL_TEXT;
                     //AdqExpnExtrPrct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AdqExpnExtrPrct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "adqqnty_clm":
                     AdqQnty_Clm.Caption = control.LABL_TEXT;
                     //AdqQnty_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AdqQnty_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "adqdesc_lb":
                     AdqDesc_Lb.Text = control.LABL_TEXT;
                     //AdqDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdqDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chkrwno_clm":
                     ChkRwno_Clm.Caption = control.LABL_TEXT;
                     //ChkRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChkRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chkamnt_clm":
                     ChkAmnt_Clm.Caption = control.LABL_TEXT;
                     //ChkAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChkAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chkbank_clm":
                     ChkBank_Clm.Caption = control.LABL_TEXT;
                     //ChkBank_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChkBank_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chktype_clm":
                     ChkType_Clm.Caption = control.LABL_TEXT;
                     //ChkType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChkType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chekdate_clm":
                     ChekDate_Clm.Caption = control.LABL_TEXT;
                     //ChekDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChekDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chekok_butn":
                     ChekOK_Butn.Text = control.LABL_TEXT;
                     //ChekOK_Butn.Text = control.LABL_TEXT; // ToolTip
                     //ChekOK_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chkamnt_lb":
                     ChkAmnt_Lb.Text = control.LABL_TEXT;
                     //ChkAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChkAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chekbank_lb":
                     ChekBank_Lb.Text = control.LABL_TEXT;
                     //ChekBank_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChekBank_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chekownr_lb":
                     ChekOwnr_Lb.Text = control.LABL_TEXT;
                     //ChekOwnr_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChekOwnr_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "paiddate_lb":
                     PaidDate_Lb.Text = control.LABL_TEXT;
                     //PaidDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PaidDate_Lb.Text = control.LABL_TEXT; // Place Holder
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
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         CallerForm = "";
         PymtBs1.DataSource = job.Input as Data.Payment;
         var p = PymtBs1.Current as Data.Payment;

         // 1396/08/19 * اگر درخواست به غسر از تمدید بود گزینه های تخفیف مبلغ مابه التفاوت باید خاموش باشد
         SaveDifferenceAmnt_Butn.Enabled = CalcDiffAmnt_Cb.Enabled = p.Request.RQTP_CODE == "009";

         CashBs1.DataSource = iScsc.Cashes;
         ExpnBs3.DataSource = 
            iScsc.Expenses.Where(
               e => 
                  p.Request.RQTP_CODE == e.Expense_Type.Request_Requester.RQTP_CODE &&
                  p.Request.RQTT_CODE == e.Expense_Type.Request_Requester.RQTT_CODE &&
                  e.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" &&
                  e.EXPN_STAT == "002" &&
                  e.ADD_QUTS == "002"
            );
         //DRcmtBs1.DataSource = iScsc.D_RCMTs.Where(d => d.VALU == "001" || d.VALU == "003" || d.VALU == "005" || d.VALU == "008");
         DRcmtBs1.DataSource = iScsc.D_RCMTs.Where(d => d.VALU != "006");
         DPydsBs2.DataSource = iScsc.D_PYDS;
         DActvBs2.DataSource = iScsc.D_ACTVs;
         DCktpBs4.DataSource = iScsc.D_CKTPs;
         DCatpBs.DataSource = iScsc.D_CATPs;
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(pos => pos.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(pos => pos.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;

         DEBT_DNRMTextEdit.EditValue = p.Request.Request_Rows.FirstOrDefault().Fighter.DEBT_DNRM;
         if (p.Request.Request_Rows.FirstOrDefault().Fighter.DEBT_DNRM >= 0)
         {
            CashByDeposit_Txt002.Properties.ReadOnly = true;
            dEBT_DNRMLabel.Text = "مبلغ بدهی :";
            GB_DebtStat.Text = "مشترک بدهکار یا هیچ سپرده ای ندارد و نمی توان از تخفیف سپرده استفاده کنید";
            AddDebtDiscount_Butn002.Enabled = false;
         }
         else
         {
            CashByDeposit_Txt002.Properties.ReadOnly = false;
            dEBT_DNRMLabel.Text = "مبلغ بستانکاری :";
            GB_DebtStat.Text = "مشترک بستانکار می باشد، می توانید از تخفیف سپرده استفاده کنید";
            AddDebtDiscount_Butn002.Enabled = true;
         }

         var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");
         if(regl.AMNT_TYPE == "001")
         {
            AmntType1_Lb.Text = AmntType2_Lb.Text = AmntType3_Lb.Text = AmntType4_Lb.Text = AmntType5_Lb.Text = AmntType6_Lb.Text = "( ریال )";
         }
         else
         {
            AmntType1_Lb.Text = AmntType2_Lb.Text = AmntType3_Lb.Text = AmntType4_Lb.Text = AmntType5_Lb.Text = AmntType6_Lb.Text = "( تومان )";
         }

         Execute_Query();

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P (Job job)
      {
         var xinput = job.Input as XElement;
         CallerForm = xinput.Attribute("callerform").Value;
         if(xinput.Attribute("tabfocued")!=null)
            TabFocued = xinput.Attribute("tabfocued").Value;
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

            var pymt = PymtBs1.Current as Data.Payment;
            if (pymt == null) return;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            var rqtpcode = pymt.Request.RQTP_CODE;//RcevXData.Element("PosRespons").Attribute("rqtpcode").Value;
            var rqid = pymt.RQST_RQID;//RcevXData.Element("PosRespons").Attribute("rqid").Value;
            //var fileno = pymt.Request.Request_Rows.FirstOrDefault().FIGH_FILE_NO;//RcevXData.Element("PosRespons").Attribute("fileno").Value;
            var cashcode = pymt.CASH_CODE;//RcevXData.Element("PosRespons").Element("Payment").Attribute("cashcode").Value;
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
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", cashcode),
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("rcptmtod", "003"),
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

            PosToolsVisiable(false);
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
               Execute_Query();
               requery = false;
            }
         }               
         job.Status = StatusType.Successful;
      }
   }
}
