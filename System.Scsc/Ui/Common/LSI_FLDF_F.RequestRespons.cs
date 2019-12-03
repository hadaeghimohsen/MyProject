using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Common
{
   partial class LSI_FLDF_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private bool isFirstLoaded = false;
      private string RegnLang = "054";
      private XElement HostNameInfo;


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
            case 10:
               Actn_CalF_F(job);
               break;
            case 21:
               Payg_Oprt_F(job);
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
         else if(keyData == Keys.Enter)
         {
            HL_INVSFILENO_ButtonClick(null, null);
         }
         else if (keyData == Keys.F11)
         {
            FrstName_Txt.Focus();
            FrstName_Txt.Text = LastName_Txt.Text = NatlCode_Txt.Text = FngrPrnt_Txt.Text = TellPhon_Txt.Text = CellPhon_Txt.Text = GlobCode_Txt.Text = ServNo_Txt.Text = "";
            SuntCode_Lov.EditValue = null;
            BothSex_Rb.Checked = true;
         }
         else if(keyData == (Keys.F11 | Keys.Control))
         {
            Search_Butn_Click(null, null);
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, /*"LSI_FLDF_F"*/GetType().Name, 04 /* Execute UnPaint */);
         }
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
         else if (keyData == (Keys.Control | Keys.Alt | Keys.F5))
         {
            PayCashDebt_Butn_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Alt | Keys.F6))
         {
            PayPosDebt_Butn_Click(null, null);
         }
         else if(keyData == (Keys.F3))
         {
            vF_Last_Info_FighterResultGridControl_DoubleClick(null, null);
         }
         else if(keyData == Keys.F8)
         {
            PymtBnDebt_Click(null, null);
         }
         else if(keyData == Keys.F5)
         {
            RqstBnNewMbsp_Click(null, null);
         }
         else if(keyData == Keys.F4)
         {
            RqstBnEditPblc_Click(null, null);
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

         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
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
                  case "cellphon_lb":
                     CellPhon_Lb.Text = control.LABL_TEXT;
                     //CellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "natlcode1_lb":
                     NatlCode1_Lb.Text = control.LABL_TEXT;
                     //NatlCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclenddate_clm":
                     CyclEndDAte_Clm.Caption = control.LABL_TEXT;
                     //CyclEndDAte_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclEndDAte_Clm.Caption = control.LABL_TEXT; // Place Holder
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
                  case "natlcode_lb":
                     NatlCode_Lb.Text = control.LABL_TEXT;
                     //NatlCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_lb":
                     FngrPrnt_Lb.Text = control.LABL_TEXT;
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "tellphon_lb":
                     TellPhon_Lb.Text = control.LABL_TEXT;
                     //TellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "men_rb":
                     Men_Rb.Text = control.LABL_TEXT;
                     //Men_Rb.Text = control.LABL_TEXT; // ToolTip
                     //Men_Rb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "women_rb":
                     Women_Rb.Text = control.LABL_TEXT;
                     //Women_Rb.Text = control.LABL_TEXT; // ToolTip
                     //Women_Rb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bothsex_rb":
                     BothSex_Rb.Text = control.LABL_TEXT;
                     //BothSex_Rb.Text = control.LABL_TEXT; // ToolTip
                     //BothSex_Rb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "lastname_clm":
                     LastName_Clm.Caption = control.LABL_TEXT;
                     //LastName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //LastName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstname_clm":
                     FrstName_Clm.Caption = control.LABL_TEXT;
                     //FrstName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FrstName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtdnrm_clm":
                     DebtDnrm_Clm.Caption = control.LABL_TEXT;
                     //DebtDnrm_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DebtDnrm_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "dpstamnt_clm":
                     DpstAmnt_Clm.Caption = control.LABL_TEXT;
                     //DpstAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DpstAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_clm":
                     FngrPrnt_Clm.Caption = control.LABL_TEXT;
                     //FngrPrnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chatid_clm":
                     ChatId_Clm.Caption = control.LABL_TEXT;
                     //ChatId_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChatId_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cycl_gb":
                     Cycl_Gb.Text = control.LABL_TEXT;
                     //Cycl_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Cycl_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon1_lb":
                     CellPhon1_Lb.Text = control.LABL_TEXT;
                     //CellPhon1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon1_lb":
                     TellPhon1_Lb.Text = control.LABL_TEXT;
                     //TellPhon1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TellPhon1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode1_lb":
                     SuntCode1_Lb.Text = control.LABL_TEXT;
                     //SuntCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SuntCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "globcode1_lb":
                     GlobCode1_Lb.Text = control.LABL_TEXT;
                     //GlobCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GlobCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlamnt_lb":
                     TotlAmnt_Lb.Text = control.LABL_TEXT;
                     //TotlAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TotlAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsamnt_lb":
                     PydsAmnt_Lb.Text = control.LABL_TEXT;
                     //PydsAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PydsAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtamnt_lb":
                     PymtAmnt_Lb.Text = control.LABL_TEXT;
                     //PymtAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PymtAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspvaldtype_butn":
                     MbspValdType_Butn.Text = control.LABL_TEXT;
                     //MbspValdType_Butn.Text = control.LABL_TEXT; // ToolTip
                     //MbspValdType_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspinfo_butn":
                     MbspInfo_Butn.Text = control.LABL_TEXT;
                     //MbspInfo_Butn.Text = control.LABL_TEXT; // ToolTip
                     //MbspInfo_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclrwno_clm":
                     CyclRwno_Clm.Caption = control.LABL_TEXT;
                     //CyclRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclstrtdate_clm":
                     CyclStrtDate_Clm.Caption = control.LABL_TEXT;
                     //CyclStrtDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclStrtDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclnumattn_clm":
                     CyclNumAttn_Clm.Caption = control.LABL_TEXT;
                     //CyclNumAttn_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclNumAttn_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclattnremn_clm":
                     CyclAttnRemn_Clm.Caption = control.LABL_TEXT;
                     //CyclAttnRemn_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclAttnRemn_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclnumday_clm":
                     CyclNumDay_Clm.Caption = control.LABL_TEXT;
                     //CyclNumDay_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclNumDay_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno_clm":
                     CochFileNo_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CochFileNo_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodcode_clm":
                     MtodCode_Clm.Caption = control.LABL_TEXT;
                     //MtodCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MtodCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgycode_clm":
                     CtgyCode_Clm.Caption = control.LABL_TEXT;
                     //CtgyCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CtgyCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "fighbnsettingprint":
                     FighBnSettingPrint.Text = control.LABL_TEXT;
                     //FighBnSettingPrint.Text = control.LABL_TEXT; // ToolTip
                     //FighBnSettingPrint.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fighbndefaultprint":
                     FighBnDefaultPrint.Text = control.LABL_TEXT;
                     //FighBnDefaultPrint.Text = control.LABL_TEXT; // ToolTip
                     //FighBnDefaultPrint.Text = control.LABL_TEXT; // Place Holder
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
         try
         {
            //if (isFirstLoaded) goto finishcommand;

            //provinceBindingSource.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));
            //methodBindingSource.DataSource = iScsc.Methods;
            //dSXTPBindingSource.DataSource = iScsc.D_SXTPs;
            //dFGTPBindingSource.DataSource = iScsc.D_FGTPs;
            //dFGSTBindingSource.DataSource = iScsc.D_FGSTs;
            CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003");
            SuntBs1.DataSource = iScsc.Sub_Units;

            UserProFile_Rb.ImageVisiable = true;
            //isFirstLoaded = true;

            //finishcommand:

            //if (InvokeRequired)
            //   Invoke(new Action(() => vF_Last_Info_FighterResultBindingSource.DataSource = iScsc.VF_Last_Info_Fighter(null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE)));
            //else
            //vF_Last_Info_FighterResultBindingSource.DataSource = iScsc.VF_Last_Info_Fighter(null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE);//.Where(f => Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Fga_Uclb_U.Contains(f.CLUB_CODE));
            if (iScsc.Settings.Any(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.RUN_QURY == "002"))
               Search_Butn_Click(null, null);

            if(job.Input != null && (job.Input as XElement).Attribute("requery").Value == "1")
               Search_Butn_Click(null, null);
         }
         catch (Exception ex) { MessageBox.Show(ex.Message); }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_F(Job job)
      {
         var x = job.Input as XElement;
         /*
          * <Fighter showlist="001" "002"/>
          */
         switch (x.Attribute("showlist").Value)
         {
            case "001": //  مشترییان
               PBLC.ActiveFilterString = "TYPE != '003'";
               break;
            case "003":
               PBLC.ActiveFilterString = "TYPE = '003'";
               break;
            case "004":
               PBLC.ActiveFilterString = "TYPE = '004'";
               break;
            default:
               break;
         }

         // 1398/08/24 * اعمال فیلترینگ بر روی داده ها
         if(x.Attribute("filtering") != null)
         {
            switch (x.Attribute("filtering").Value)
            {
               case "cellphon":
                  CellPhon_Txt.Text = x.Attribute("filter_value").Value;
                  break;
               default:
                  break;
            }
            Search_Butn_Click(null, null);
         }


      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void Payg_Oprt_F(Job job)
      {
         try
         {
            XElement RcevXData = job.Input as XElement;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            var paydebt = Convert.ToInt64(RcevXData.Attribute("amnt").Value);
            var termno = RcevXData.Attribute("termno").Value;
            var tranno = RcevXData.Attribute("tranno").Value;
            var cardno = RcevXData.Attribute("cardno").Value;
            var flowno = RcevXData.Attribute("flowno").Value;
            var refno = RcevXData.Attribute("refno").Value;
            var actndate = RcevXData.Attribute("actndate").Value;

            if (regl.AMNT_TYPE == "002")
               paydebt /= 10;

            var figh = vF_Fighs.Current as Data.VF_Last_Info_FighterResult;
            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);

            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "003"),
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

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
            //// این گزینه برای حالتی می باشد که کل مبلغ پرداخت به صورت کامل روی دستگاه پایانه فروش قرار میگیرد
            //if (UsePos_Cb.Checked)
            //{
            //   iScsc.PAY_MSAV_P(
            //      new XElement("Payment",
            //         new XAttribute("actntype", "CheckoutWithPOS"),
            //         new XElement("Insert",
            //            new XElement("Payment_Method",
            //               new XAttribute("cashcode", cashcode),
            //               new XAttribute("rqstrqid", rqid),
            //               new XAttribute("amnt", amnt),
            //               new XAttribute("termno", termno),
            //               new XAttribute("tranno", tranno),
            //               new XAttribute("cardno", cardno),
            //               new XAttribute("flowno", flowno),
            //               new XAttribute("refno", refno),
            //               new XAttribute("actndate", actndate)
            //            )
            //         )
            //      )
            //   );
            //}
            //// این گزینه برای پرداختی پایانه ای هست که به صورت کامل پرداخت نمی شود
            //else
            //{
            //   iScsc.PAY_MSAV_P(
            //      new XElement("Payment",
            //         new XAttribute("actntype", "InsertUpdate"),
            //         new XElement("Insert",
            //            new XElement("Payment_Method",
            //               new XAttribute("cashcode", cashcode),
            //               new XAttribute("rqstrqid", rqid),
            //               new XAttribute("rcptmtod", "003"),
            //               new XAttribute("amnt", amnt),
            //               new XAttribute("termno", termno),
            //               new XAttribute("tranno", tranno),
            //               new XAttribute("cardno", cardno),
            //               new XAttribute("flowno", flowno),
            //               new XAttribute("refno", refno),
            //               new XAttribute("actndate", actndate)
            //            )
            //         )
            //      )
            //   );
            //}
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
