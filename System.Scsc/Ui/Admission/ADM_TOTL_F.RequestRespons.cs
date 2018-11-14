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
   partial class ADM_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string formCaller = "";
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
            //   new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstRqt1_Click(null, null);
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
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstDelete1_Click(null, null);
            Btn_RqstDelete3_Click(null, null);
         }
         else if (keyData == Keys.F5)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstRqt1_Click(null, null);
            Btn_RqstRqt3_Click(null, null);
         }
         else if (keyData == Keys.F3)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   LL_MoreInfo_LinkClicked(null, null);
         }
         else if (keyData == Keys.F10)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstSav1_Click(null, null);
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

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         //LL_MoreInfo_LinkClicked(null, null);
         LL_MoreInfo2_LinkClicked(null, null);

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
                  case "pymtamnt_clm":
                     PymtAmnt_Clm.Caption = control.LABL_TEXT;
                     //PymtAmnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PymtAmnt_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtrcptmtod_clm":
                     PymtRcptMtod_Clm.Caption = control.LABL_TEXT;
                     //PymtRcptMtod_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PymtRcptMtod_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "actndate_clm":
                     ActnDate_Clm.Caption = control.LABL_TEXT;
                     //ActnDate_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ActnDate_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "numattn_lb":
                     NumAttn_Lb.Text = control.LABL_TEXT;
                     //NumAttn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NumAttn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amntdtil_lb":
                     AmntDtil_Lb.Text = control.LABL_TEXT;
                     //AmntDtil_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AmntDtil_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;                  
                  case "info_gb":
                     Info_Gb.Text = control.LABL_TEXT;
                     //Info_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Info_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_lb":
                     RqttCode_Lb.Text = control.LABL_TEXT;
                     //RqttCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqttCode_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "montcont_lb":
                     MontCont_Lb.Text = control.LABL_TEXT;
                     //MontCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MontCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "daycont_lb":
                     DayCont_Lb.Text = control.LABL_TEXT;
                     //DayCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DayCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_lb":
                     EndDate_Lb.Text = control.LABL_TEXT;
                     //EndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "montofer_lb":
                     MontOfer_Lb.Text = control.LABL_TEXT;
                     //MontOfer_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MontOfer_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgycode_lb":
                     CtgyCode_Lb.Text = control.LABL_TEXT;
                     //CtgyCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CtgyCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcode_lb":
                     CbmtCode_Lb.Text = control.LABL_TEXT;
                     //CbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "autoattn_btn":
                     AutoAttn_Btn.Text = control.LABL_TEXT;
                     //AutoAttn_Btn.Text = control.LABL_TEXT; // ToolTip
                     //AutoAttn_Btn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gb_expense3":
                     Gb_Expense3.Text = control.LABL_TEXT;
                     //Gb_Expense3.Text = control.LABL_TEXT; // ToolTip
                     //Gb_Expense3.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlexpn_lb":
                     TotlExpn_Lb.Text = control.LABL_TEXT;
                     //TotlExpn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TotlExpn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymt_lb":
                     Pymt_Lb.Text = control.LABL_TEXT;
                     //Pymt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Pymt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dscn_lb":
                     Dscn_Lb.Text = control.LABL_TEXT;
                     //Dscn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Dscn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bn_paymentmethods3":
                     bn_PaymentMethods3.Text = control.LABL_TEXT;
                     //bn_PaymentMethods3.Text = control.LABL_TEXT; // ToolTip
                     //bn_PaymentMethods3.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashcode_clm":
                     CashCode_Clm.Caption = control.LABL_TEXT;
                     //CashCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CashCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlamnt_clm":
                     TotlAmnt_Clm.Caption = control.LABL_TEXT;
                     //TotlAmnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //TotlAmnt_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlextramnt_clm":
                     TotlExtrAmnt_Clm.Caption = control.LABL_TEXT;
                     //TotlExtrAmnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //TotlExtrAmnt_Clm.Text = control.LABL_TEXT; // Place Holder
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
                  case "extramnt_clm":
                     ExtrAmnt_Clm.Caption = control.LABL_TEXT;
                     //ExtrAmnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ExtrAmnt_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "qnty_clm":
                     Qnty_Clm.Caption = control.LABL_TEXT;
                     //Qnty_Clm.Text = control.LABL_TEXT; // ToolTip
                     //Qnty_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sumpric_clm":
                     SumPric_Clm.Caption = control.LABL_TEXT;
                     //SumPric_Clm.Text = control.LABL_TEXT; // ToolTip
                     //SumPric_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dsctrwno_clm":
                     DsctRwno_Clm.Caption = control.LABL_TEXT;
                     //DsctRwno_Clm.Text = control.LABL_TEXT; // ToolTip
                     //DsctRwno_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dsctamnt_clm":
                     DsctAmnt_Clm.Caption = control.LABL_TEXT;
                     //DsctAmnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //DsctAmnt_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dsctdesc_clm":
                     DsctDesc_Clm.Caption = control.LABL_TEXT;
                     //DsctDesc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //DsctDesc_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtrwno_clm":
                     PymtRwno_Clm.Caption = control.LABL_TEXT;
                     //PymtRwno_Clm.Text = control.LABL_TEXT; // ToolTip
                     //PymtRwno_Clm.Text = control.LABL_TEXT; // Place Holder
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
         UserProFile_Rb.ImageVisiable = true;
         //**FighBs3.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006" || f.FGPB_TYPE_DNRM == "009") && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 );
         if (isFirstLoaded) goto finishcommand;
         #region Rqsw block
         try
         {
            RqttBs1.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "001" || rqtt.CODE == "004" /*|| rqtt.CODE == "005" || rqtt.CODE == "006"*/);
            
            DCyclBs1.DataSource = iScsc.D_CYCLs;

            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DRcmtBs1.DataSource = iScsc.D_RCMTs;
            DAtypBs1.DataSource = iScsc.D_ATYPs;
            DPydsBs1.DataSource = iScsc.D_PYDS;
            PrvnBs1.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));

            isFirstLoaded = true;
         }            
         catch { }
         //Execute_Query();
         #endregion

      finishcommand:
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;
         //MtodBs2.DataSource = iScsc.Methods.Where(m=> m.MTOD_STAT == "002");
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && cbmt.Method.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101)/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
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
            CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && cbmt.Method.MTOD_STAT == "002" && Convert.ToInt32( cbmt.Fighter.ACTV_TAG_DNRM ?? "101" ) >= 101 )/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
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
            if ((job.Input as XElement).Attribute("enrollnumber") != null)
            {
               if ((job.Input as XElement).Attribute("type").Value == "fighter")
               {
                  //INSR_NUMB_TextEdit.Text = FNGR_PRNT_TextEdit.Text = (job.Input as XElement).Attribute("enrollnumber").Value;
                  //INSR_DATE_PersianDateEdit.Value = DateTime.Now;
               }
               else if ((job.Input as XElement).Attribute("type").Value == "renewcontract")
               {
                  if (RqstBs3.Count > 0 && (RqstBs3.Current as Data.Request).RQID > 0)
                     RqstBs3.AddNew();

                  var figh = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM == (job.Input as XElement).Attribute("enrollnumber").Value).FirstOrDefault();
                  Figh_Lov.EditValue = figh.FILE_NO;
                  RQTT_CODE_LookUpEdit3.EditValue = "001";//figh.FGPB_TYPE_DNRM;
                  Btn_RqstRqt3_Click(null, null);

                  // 1396/11/04
                  if ((job.Input as XElement).Attribute("formcaller") != null)
                     formCaller = (job.Input as XElement).Attribute("formcaller").Value;
                  else
                     formCaller = "";
               }

            }
            else if ((job.Input as XElement).Attribute("type") != null)
            {
               if ((job.Input as XElement).Attribute("type").Value == "refresh" || (job.Input as XElement).Attribute("type").Value == "renewcontract")
               {
                  Execute_Query();

                  //ReloadSelectedData();
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
         XElement RcevXData = job.Input as XElement;

         var rqst = RqstBs3.Current as Data.Request;
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
         Btn_RqstSav3_Click(null, null);

         job.Status = StatusType.Successful;
      }
   }
}
