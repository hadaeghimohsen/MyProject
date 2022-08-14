using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission
{
   partial class ADM_FIGH_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string formCaller;
      private bool isFirstLoaded = false;
      private string CurrentUser;
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
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

            switch (formCaller)
            {
               case "CFG_STNG_F":
               case "BAS_CPR_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", formCaller, 08 /* Exec LoadDataSource */, SendType.SelfToUserInterface)
                  );
                  break;
               default:
                  break;
            }
            //job.Next =
            //   new Job(SendType.SelfToUserInterface, "ADM_FIGH_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstRqt1_Click(null, null);
         }
         else if (keyData == Keys.Enter)
         {
            //if (!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_Cbmt1.Focused || Btn_Dise.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            //Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstDelete1_Click(null, null);
         }
         else if (keyData == Keys.F5)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstRqt1_Click(null, null);
         }
         else if (keyData == Keys.F10)
         {
         }
         //else if (keyData == (Keys.Control | Keys.P))
         //{
         //   RqstBnDefaultPrint_Click(null, null);
         //}
         //else if (keyData == (Keys.Control | Keys.Shift | Keys.P))
         //{
         //   RqstBnPrint_Click(null, null);
         //}
         else if (keyData == (Keys.Control | Keys.F2))
         {
            RqstBnEnrollFngrPrnt2_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.F3))
         {
            RqstBnDeleteFngrPrnt2_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.F4))
         {
            RqstBnEnrollFngrPrnt1_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.F5))
         {
            RqstBnDeleteFngrPrnt1_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.F6))
         {
            RqstBnDuplicateFngrPrnt1_Click(null, null);
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
                  case "numbattnmon_lb":
                     NumbAttnMon_Lb.Text = control.LABL_TEXT;
                     //NumbAttnMon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NumbAttnMon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode_lb":
                     SuntCode_Lb.Text = control.LABL_TEXT;
                     //SuntCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SuntCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chatid_lb":
                     ChatId_Lb.Text = control.LABL_TEXT;
                     //ChatId_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChatId_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sumexpnpric_clm":
                     SumExpnPric_Clm.Caption = control.LABL_TEXT;
                     //SumExpnPric_Clm.Text = control.LABL_TEXT; // ToolTip
                     //SumExpnPric_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnpric_clm":
                     ExpnPric_Clm.Caption = control.LABL_TEXT;
                     //ExpnPric_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ExpnPric_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsdesc_clm":
                     PydsDesc_Clm.Caption = control.LABL_TEXT;
                     //PydsDesc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PydsDesc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "actndate_clm":
                     ActnDate_Clm.Caption = control.LABL_TEXT;
                     //ActnDate_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ActnDate_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "endtime_clm":
                     EndTime_Clm.Caption = control.LABL_TEXT;
                     //EndTime_Clm.Text = control.LABL_TEXT; // ToolTip
                     //EndTime_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "timedesc_clm":
                     TimeDesc_Clm.Caption = control.LABL_TEXT;
                     //TimeDesc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //TimeDesc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_lb":
                     RqttCode_Lb.Text = control.LABL_TEXT;
                     //RqttCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqttCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstname_lb":
                     FrstName_Lb.Text = control.LABL_TEXT;
                     //FrstName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FrstName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "lastname_lb":
                     LastName_Lb.Text = control.LABL_TEXT;
                     //LastName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LastName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon_lb":
                     CellPhon_Lb.Text = control.LABL_TEXT;
                     //CellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_lb":
                     FngrPrnt_Lb.Text = control.LABL_TEXT;
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cardnumb_lb":
                     CardNumb_Lb.Text = control.LABL_TEXT;
                     //CardNumb_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CardNumb_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno_lb":
                     FileNo_Lb.Text = control.LABL_TEXT;
                     //FileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdate_lb":
                     BrthDate_Lb.Text = control.LABL_TEXT;
                     //BrthDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype_lb":
                     SexType_Lb.Text = control.LABL_TEXT;
                     //SexType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SexType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon_lb":
                     TellPhon_Lb.Text = control.LABL_TEXT;
                     //TellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "natlcode_lb":
                     NatlCode_Lb.Text = control.LABL_TEXT;
                     //NatlCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspdesc_lb":
                     //MbspDesc_Lb.Text = control.LABL_TEXT;
                     //MbspDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcode_lb":
                     CbmtCode_Lb.Text = control.LABL_TEXT;
                     //CbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgycode_lb":
                     CtgyCode_Lb.Text = control.LABL_TEXT;
                     //CtgyCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CtgyCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_autocalcattn":
                     Btn_AutoCalcAttn.Text = control.LABL_TEXT;
                     //Btn_AutoCalcAttn.Text = control.LABL_TEXT; // ToolTip
                     //Btn_AutoCalcAttn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strtdate_lb":
                     StrtDate_Lb.Text = control.LABL_TEXT;
                     //StrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //StrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_rb001":
                     EndDate_RB001.Text = control.LABL_TEXT;
                     //EndDate_RB001.Text = control.LABL_TEXT; // ToolTip
                     //EndDate_RB001.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbofmontdnrm_rb001":
                     NumbOfMontDnrm_RB001.Text = control.LABL_TEXT;
                     //NumbOfMontDnrm_RB001.Text = control.LABL_TEXT; // ToolTip
                     //NumbOfMontDnrm_RB001.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbmontoffr_lb":
                     //NumbMontOffr_Lb.Text = control.LABL_TEXT;
                     //NumbMontOffr_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NumbMontOffr_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "other_desc":
                     //Other_Desc.Text = control.LABL_TEXT;
                     //Other_Desc.Text = control.LABL_TEXT; // ToolTip
                     //Other_Desc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "globcode_lb":
                     GlobCode_Lb.Text = control.LABL_TEXT;
                     //GlobCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GlobCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "servno_lb":
                     ServNo_Lb.Text = control.LABL_TEXT;
                     //ServNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ServNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "request_lb":
                     //Request_Lb.Text = control.LABL_TEXT;
                     //Request_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Request_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "pymt_lb":
                     //Pymt_Lb.Text = control.LABL_TEXT;
                     //Pymt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Pymt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bn_paymentmethods1":
                     bn_PaymentMethods1.Text = control.LABL_TEXT;
                     //bn_PaymentMethods1.Text = control.LABL_TEXT; // ToolTip
                     //bn_PaymentMethods1.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashcode_clm":
                     CashCode_Clm.Caption = control.LABL_TEXT;
                     //CashCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CashCode_Clm.Text = control.LABL_TEXT; // Place Holder
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
                  case "rwnopdsc_clm":
                     RwnoPdsc_Clm.Caption = control.LABL_TEXT;
                     //RwnoPdsc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //RwnoPdsc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amntpdsc_clm":
                     AmntPdsc_Clm.Caption = control.LABL_TEXT;
                     //AmntPdsc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //AmntPdsc_Clm.Text = control.LABL_TEXT; // Place Holder
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
                  case "clubcode_clm":
                     ClubCode_Clm.Caption = control.LABL_TEXT;
                     //ClubCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ClubCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodcode_clm":
                     MtodCode_Clm.Caption = control.LABL_TEXT;
                     //MtodCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //MtodCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno_clm":
                     CochFileNo_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "daytype_clm":
                     DayType_Clm.Caption = control.LABL_TEXT;
                     //DayType_Clm.Text = control.LABL_TEXT; // ToolTip
                     //DayType_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strttime_clm":
                     StrtTime_Clm.Caption = control.LABL_TEXT;
                     //StrtTime_Clm.Text = control.LABL_TEXT; // ToolTip
                     //StrtTime_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtdesc_clm":
                     CbmtDesc_Clm.Caption = control.LABL_TEXT;
                     //CbmtDesc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CbmtDesc_Clm.Text = control.LABL_TEXT; // Place Holder
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
         //FighBs3.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006") && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         if (isFirstLoaded) goto finishcommand;
         #region Rqsw block
         try
         {
            RqttBs1.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "001" || rqtt.CODE == "004");
            DPydsBs1.DataSource = iScsc.D_PYDS;
            DAtypBs1.DataSource = iScsc.D_ATYPs;
            DSxtpBs1.DataSource = iScsc.D_SXTPs.Where(d => d.VALU != "003");
            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DRcmtBs1.DataSource = iScsc.D_RCMTs;
            DDsatBs.DataSource = iScsc.D_DSATs;
            DCetpBs.DataSource = iScsc.D_CETPs;

            PrvnBs1.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));
            
            isFirstLoaded = true;
         }
         catch { }         

      //Execute_Query();
         #endregion

      finishcommand:
         //FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && cbmt.Method.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101)/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
         OrgnBs1.DataSource = iScsc.Organs;
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
            //DstpBs1.DataSource = iScsc.Diseases_Types;            
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
            CardNumb_Text.Text = "";
            var xinput = job.Input as XElement;
            //tb_master.TabPages.Clear();
            switch (xinput.Attribute("type").Value)
            {
               case "fighter":
                  //tb_master.TabPages.Add(tp_001);
                  //tb_master.SelectedTab = tp_001;
                  break;
               case "coach":
                  //tb_master.TabPages.Add(tp_002);
                  //tb_master.SelectedTab = tp_002;
                  break;
               case "renewcontract":
                  //tb_master.TabPages.Add(tp_003);
                  //tb_master.SelectedTab = tp_003;
                  break;

            }
            if (xinput.Attribute("enrollnumber") != null)
            {
               if (xinput.Attribute("type").Value == "fighter")
               {
                  //INSR_NUMB_TextEdit.Text = FNGR_PRNT_TextEdit.Text = (job.Input as XElement).Attribute("enrollnumber").Value;
                  //INSR_DATE_PersianDateEdit.Value = DateTime.Now;
               }
               if (xinput.Attribute("type").Value == "coach")
               {
                  //Insr_Numb_TextEdit2.Text = Fngr_Prnt_TextEdit2.Text = (job.Input as XElement).Attribute("enrollnumber").Value;
                  //Insr_Date_PersianDateEdit2.Value = DateTime.Now;
               }
               else if (xinput.Attribute("type").Value == "renewcontract")
               {
                  var figh = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM == xinput.Attribute("enrollnumber").Value).FirstOrDefault();
                  //FIGH_FILE_NOLookUpEdit.EditValue = figh.FILE_NO;
                  //RQTT_CODE_LookUpEdit3.EditValue = figh.FGPB_TYPE_DNRM;
               }
               FNGR_PRNT_TextEdit.EditValue = xinput.Attribute("enrollnumber").Value;
            }
            else if (xinput.Attribute("type").Value == "setcard")
            {
               FNGR_PRNT_TextEdit.Text = xinput.Attribute("value").Value;
               //MaxF_Butn001_Click(null, null);
               CardNumb_Text.Text = xinput.Attribute("value").Value;
            }
            else if (xinput.Attribute("type").Value == "refresh")
               Execute_Query();
            else if (xinput.Attribute("type").Value == "rqidfocus")
            {
               ShowRqst_PickButn.PickChecked = false;
               RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.RQID == Convert.ToInt64(xinput.Attribute("rqid").Value)));
            }
            else if(xinput.Attribute("type").Value == "admcbmt")
            {
               if (xinput.Attribute("cbmtcode") != null)
                  cbmtcode = Convert.ToInt64(xinput.Attribute("cbmtcode").Value);
               else
                  cbmtcode = null;

               if (xinput.Attribute("ctgycode") != null)
                  ctgycode = Convert.ToInt64(xinput.Attribute("ctgycode").Value);
               else
                  ctgycode = null;

               Execute_Query();

               if(RqstBs1.List.Count >= 1)
                  RqstBs1.AddNew();

               CbmtCode_Lov.EditValue = cbmtcode;
               CtgyCode_Lov.EditValue = ctgycode;

               Btn_AutoCalcAttn_Click(null, null);
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

            // این گزینه برای حالتی می باشد که کل مبلغ پرداخت به صورت کامل روی دستگاه پایانه فروش قرار میگیرد
            if (UsePos_Cb.Checked)
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
               Btn_RqstSav1_Click(null, null);
            }
            // این گزینه برای پرداختی پایانه ای هست که به صورت کامل پرداخت نمی شود
            else
            {
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
            }
         }
         catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
