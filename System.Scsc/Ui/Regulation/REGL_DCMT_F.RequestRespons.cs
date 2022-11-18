using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Regulation
{
   partial class REGL_DCMT_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private Data.Regulation Rg1, Rg2;
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
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
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
               new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            SubmitRqrq_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.PageUp))
         {
            BackExtp_Butn_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.PageDown))
         {
            NextExtp_Butn_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.Up))
         {
            Expn_Gv.MovePrev();
            //ExpnBs.MovePrevious();
         }
         else if(keyData == (Keys.Control | Keys.Down))
         {
            Expn_Gv.MoveNext();
            //ExpnBs.MoveNext();
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
                  case "tp_002":
                     tp_002.Text = control.LABL_TEXT;
                     //tp_002.Text = control.LABL_TEXT; // ToolTip
                     //tp_002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbday_lb":
                     NumbDay_Lb.Text = control.LABL_TEXT;
                     //NumbDay_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NumbDay_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "covrdsct_clm":
                     CovrDsct_Clm.Caption = control.LABL_TEXT;
                     //CovrDsct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CovrDsct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "needtype_clm":
                     NeedType_Clm.Caption = control.LABL_TEXT;
                     //NeedType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //NeedType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_cncl":
                     Btn_Cncl.Text = control.LABL_TEXT;
                     //Btn_Cncl.Text = control.LABL_TEXT; // ToolTip
                     //Btn_Cncl.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "titlform_lb":
                     TitlForm_Lb.Text = control.LABL_TEXT;
                     //TitlForm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TitlForm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode_clm":
                     RqtpCode_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_clm":
                     RqttCode_Clm.Caption = control.LABL_TEXT;
                     //RqttCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqttCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "permstat_clm":
                     PermStat_Clm.Caption = control.LABL_TEXT;
                     //PermStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PermStat_Clm.Caption = control.LABL_TEXT; // Place Holder
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
                  case "epitcode_clm":
                     EpitCode_Clm.Caption = control.LABL_TEXT;
                     //EpitCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EpitCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "extrinfo_gb":
                     ExtrInfo_Gb.Text = control.LABL_TEXT;
                     //ExtrInfo_Gb.Text = control.LABL_TEXT; // ToolTip
                     //ExtrInfo_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expntype_lb":
                     ExpnType_Lb.Text = control.LABL_TEXT;
                     //ExpnType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExpnType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mintime_lb":
                     MinTime_Lb.Text = control.LABL_TEXT;
                     //MinTime_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MinTime_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gropexpn_lb":
                     GropExpn_Lb.Text = control.LABL_TEXT;
                     //GropExpn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GropExpn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expndesc_lb":
                     ExpnDesc_Lb.Text = control.LABL_TEXT;
                     //ExpnDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExpnDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbattn_lb":
                     NumbAttn_Lb.Text = control.LABL_TEXT;
                     //NumbAttn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NumbAttn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "numofer_lb":
                     NumOfer_Lb.Text = control.LABL_TEXT;
                     //NumOfer_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NumOfer_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "barcode_lb":
                     BarCode_Lb.Text = control.LABL_TEXT;
                     //BarCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BarCode_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "pric_clm":
                     Pric_Clm.Caption = control.LABL_TEXT;
                     //Pric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Pric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "covrtax_clm":
                     CovrTax_Clm.Caption = control.LABL_TEXT;
                     //CovrTax_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CovrTax_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "extrprct_clm":
                     ExtrPrct_Clm.Caption = control.LABL_TEXT;
                     //ExtrPrct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExtrPrct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlpric_clm":
                     TotlPric_Clm.Caption = control.LABL_TEXT;
                     //TotlPric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlPric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "addquts_clm":
                     AddQuts_Clm.Caption = control.LABL_TEXT;
                     //AddQuts_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AddQuts_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnstat_clm":
                     ExpnStat_Clm.Caption = control.LABL_TEXT;
                     //ExpnStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "dcmtdsid_clm":
                     DcmtDsid_Clm.Caption = control.LABL_TEXT;
                     //DcmtDsid_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DcmtDsid_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "origtype_clm":
                     OrigType_Clm.Caption = control.LABL_TEXT;
                     //OrigType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OrigType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstneed_clm":
                     FrstNeed_Clm.Caption = control.LABL_TEXT;
                     //FrstNeed_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FrstNeed_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_saverqdc":
                     Btn_SaveRqdc.Text = control.LABL_TEXT;
                     //Btn_SaveRqdc.Text = control.LABL_TEXT; // ToolTip
                     //Btn_SaveRqdc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dcmtdsid_lb":
                     DcmtDsid_Lb.Text = control.LABL_TEXT;
                     //DcmtDsid_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DcmtDsid_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "origtype_lb":
                     OrigType_Lb.Text = control.LABL_TEXT;
                     //OrigType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //OrigType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "needtype_lb":
                     NeedType_Lb.Text = control.LABL_TEXT;
                     //NeedType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NeedType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstneed_lb":
                     FrstNeed_Lb.Text = control.LABL_TEXT;
                     //FrstNeed_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FrstNeed_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "epitcode2_clm":
                     EpitCode2_Clm.Caption = control.LABL_TEXT;
                     //EpitCode2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EpitCode2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "prvncode_clm":
                     PrvnCode_Clm.Caption = control.LABL_TEXT;
                     //PrvnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PrvnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "regncode_clm":
                     RegnCode_Clm.Caption = control.LABL_TEXT;
                     //RegnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RegnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashcode_clm":
                     CashCode_Clm.Caption = control.LABL_TEXT;
                     //CashCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CashCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "excsstat_clm":
                     ExcsStat_Clm.Caption = control.LABL_TEXT;
                     //ExcsStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExcsStat_Clm.Caption = control.LABL_TEXT; // Place Holder
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
         if (!(job.Input is XElement))
         {
            Rg1 = (job.Input as List<Data.Regulation>)[0];
            Rg2 = (job.Input as List<Data.Regulation>)[1];
         }

         //Data.Regulation Current = job.Input as Data.Regulation;

         //ReglBs.DataSource = iScsc.Regulations.Where(r => r == Current);
         ReglBs.DataSource = iScsc.Regulations.Where(r => r == Rg1);
         RqtpBs.DataSource = iScsc.Request_Types;
         RqttBs.DataSource = iScsc.Requester_Types;
         DcspBs.DataSource = iScsc.Document_Specs;

         DActvBs.DataSource = iScsc.D_ACTVs;
         DRgtpBs.DataSource = iScsc.D_RGTPs;
         DPermBs.DataSource = iScsc.D_PERMs;
         DDcndBs.DataSource = iScsc.D_DCNDs;
         DDctpBs.DataSource = iScsc.D_DCTPs;
         DYsnoBs.DataSource = iScsc.D_YSNOs;
         DEXPTBs.DataSource = iScsc.D_EXPTs;
         AExcoBs.DataSource = iScsc.App_Base_Defines.Where(a => a.ENTY_NAME == "Expense_Cost");
         
         MtodBs.DataSource = iScsc.Methods;
         CtgyBs.DataSource = iScsc.Category_Belts;
         EpitBs.DataSource = iScsc.Expense_Items.Where(epit => epit.TYPE == "001" || epit.TYPE == "003");
         CashBs.DataSource = iScsc.Cashes;
         PrvnBs.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));
         GropBs.DataSource = iScsc.Group_Expenses.Where(ge => ge.GROP_TYPE == "001");
         BrndBs.DataSource = iScsc.Group_Expenses.Where(ge => ge.GROP_TYPE == "002");
         DDsatBs1.DataSource = iScsc.D_DSATs;
         SuntBs1.DataSource = iScsc.Sub_Units;
         DCetpBs1.DataSource = iScsc.D_CETPs;
         CochBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM == "101" && f.FGPB_TYPE_DNRM == "003");
         DegrBs.DataSource = iScsc.D_DEGRs;
         DEfdtBs.DataSource = iScsc.D_EFDTs;
         DcxtpBs.DataSource = iScsc.D_CXTPs;
         SubmitRqrq_Click(null, null);

         GV_RQRQ.ActiveFilterString = "RQTT_CODE != '002' AND RQTT_CODE != '006' AND RQTT_CODE != '008'";

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         if (job.Input == null) { job.Status = StatusType.Successful; return; }

         var input = job.Input as XElement;
         rqtpcode = input.Element("Request_Requester").Attribute("rqtpcode").Value;
         GV_RQRQ.ActiveFilterString = string.Format("RQTT_CODE != '002' AND RQTT_CODE != '006' AND RQTT_CODE != '008' AND Rqtp_Code = '{0}'", input.Element("Request_Requester").Attribute("rqtpcode").Value);
         job.Status = StatusType.Successful;
      }
   }
}
