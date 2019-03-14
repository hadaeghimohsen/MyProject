using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Common
{
   partial class ALL_FLDF_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private long fileno;
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
            case 08:
               LoadDataSource(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 20:
               Pay_Oprt_F(job);
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
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 04 /* Execute UnPaint */);
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
         else if (keyData == (Keys.Control | Keys.F7))
         {
            RqstBnDeleteFngrNewEnrollPrnt1_Click(null, null);
         }
         else if(keyData == (Keys.Control | Keys.Alt | Keys.F5))
         {
            PayCashDebt_Butn_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Alt | Keys.F6))
         {
            PayPosDebt_Butn_Click(null, null);
         }
         else if(keyData == Keys.F8)
         {
            PymtBnDebt_Click(null, null);
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

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

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
                  case "cycltotlamnt_lb":
                     CyclTotlAmnt_Lb.Text = control.LABL_TEXT;
                     //CyclTotlAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CyclTotlAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon2_lb":
                     CellPhon2_Lb.Text = control.LABL_TEXT;
                     //CellPhon2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_004":
                     tp_004.Text = control.LABL_TEXT;
                     //tp_004.Text = control.LABL_TEXT; // ToolTip
                     //tp_004.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno_lb":
                     FileNo_Lb.Text = control.LABL_TEXT;
                     //FileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_lb":
                     FngrPrnt_Lb.Text = control.LABL_TEXT;
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclnumbattnmont_clm":
                     CyclNumbAttnMont_Clm.Caption = control.LABL_TEXT;
                     //CyclNumbAttnMont_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclNumbAttnMont_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqid1_lb":
                     Rqid1_Lb.Text = control.LABL_TEXT;
                     //Rqid1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rqid1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "servno_lb":
                     ServNo_Lb.Text = control.LABL_TEXT;
                     //ServNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ServNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt1_lb":
                     FngrPrnt1_lb.Text = control.LABL_TEXT;
                     //FngrPrnt1_lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt1_lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amntunittype_clm":
                     AmntUnitType_Clm.Caption = control.LABL_TEXT;
                     //AmntUnitType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AmntUnitType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstname1_lb":
                     FrstName1_Lb.Text = control.LABL_TEXT;
                     //FrstName1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FrstName1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "lastname1_lb":
                     LastName1_Lb.Text = control.LABL_TEXT;
                     //LastName1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LastName1_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "tp_002":
                     tp_002.Text = control.LABL_TEXT;
                     //tp_002.Text = control.LABL_TEXT; // ToolTip
                     //tp_002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_003":
                     tp_003.Text = control.LABL_TEXT;
                     //tp_003.Text = control.LABL_TEXT; // ToolTip
                     //tp_003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_005":
                     tp_005.Text = control.LABL_TEXT;
                     //tp_005.Text = control.LABL_TEXT; // ToolTip
                     //tp_005.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_006":
                     tp_006.Text = control.LABL_TEXT;
                     //tp_006.Text = control.LABL_TEXT; // ToolTip
                     //tp_006.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop1_gb":
                     Grop1_Gb.Text = control.LABL_TEXT;
                     //Grop1_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop1_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "namednrm_lb":
                     NameDnrm_Lb.Text = control.LABL_TEXT;
                     //NameDnrm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NameDnrm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdate_lb":
                     BrthDate_Lb.Text = control.LABL_TEXT;
                     //BrthDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDate_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "btn_insr":
                     Btn_Insr.Text = control.LABL_TEXT;
                     //Btn_Insr.Text = control.LABL_TEXT; // ToolTip
                     //Btn_Insr.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "actvtag_lb":
                     ActvTag_Lb.Text = control.LABL_TEXT;
                     //ActvTag_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ActvTag_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_blok":
                     Btn_Blok.Text = control.LABL_TEXT;
                     //Btn_Blok.Text = control.LABL_TEXT; // ToolTip
                     //Btn_Blok.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_mbsp":
                     Btn_Mbsp.Text = control.LABL_TEXT;
                     //Btn_Mbsp.Text = control.LABL_TEXT; // ToolTip
                     //Btn_Mbsp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_pblc":
                     Btn_Pblc.Text = control.LABL_TEXT;
                     //Btn_Pblc.Text = control.LABL_TEXT; // ToolTip
                     //Btn_Pblc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "natlcode_lb":
                     NatlCode_Lb.Text = control.LABL_TEXT;
                     //NatlCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dsptamnt_lb":
                     DsptAmnt_Lb.Text = control.LABL_TEXT;
                     //DsptAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DsptAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtamnt_lb":
                     DebtAmnt_Lb.Text = control.LABL_TEXT;
                     //DebtAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop2_gb":
                     Grop2_Gb.Text = control.LABL_TEXT;
                     //Grop2_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop2_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cycldiscountamnt_lb":
                     CyclDiscountAmnt_Lb.Text = control.LABL_TEXT;
                     //CyclDiscountAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CyclDiscountAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclpaymentamnt":
                     CyclPaymentAmnt.Text = control.LABL_TEXT;
                     //CyclPaymentAmnt.Text = control.LABL_TEXT; // ToolTip
                     //CyclPaymentAmnt.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspinfo_butn":
                     MbspInfo_Butn.Text = control.LABL_TEXT;
                     //MbspInfo_Butn.Text = control.LABL_TEXT; // ToolTip
                     //MbspInfo_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspvaldtype_butn":
                     MbspValdType_Butn.Text = control.LABL_TEXT;
                     //MbspValdType_Butn.Text = control.LABL_TEXT; // ToolTip
                     //MbspValdType_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclrwno_clm":
                     CyclRwno_Clm.Caption = control.LABL_TEXT;
                     //CyclRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "strtdate_clm":
                     StrtDate_Clm.Caption = control.LABL_TEXT;
                     //StrtDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //StrtDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_clm":
                     EndDate_Clm.Caption = control.LABL_TEXT;
                     //EndDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EndDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclattnremn_clm":
                     CyclAttnRemn_Clm.Caption = control.LABL_TEXT;
                     //CyclAttnRemn_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclAttnRemn_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclnumbday_clm":
                     CyclNumbDay_Clm.Caption = control.LABL_TEXT;
                     //CyclNumbDay_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclNumbDay_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclcochfileno_clm":
                     CyclCochFileNo_Clm.Caption = control.LABL_TEXT;
                     //CyclCochFileNo_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclCochFileNo_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclmtodcode_clm":
                     CyclMtodCode_Clm.Caption = control.LABL_TEXT;
                     //CyclMtodCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclMtodCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cyclctgycode_clm":
                     CyclCtgyCode_Clm.Caption = control.LABL_TEXT;
                     //CyclCtgyCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CyclCtgyCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "gorp3_gb":
                     Gorp3_Gb.Text = control.LABL_TEXT;
                     //Gorp3_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Gorp3_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsp1rwno_lb":
                     Mbsp1Rwno_Lb.Text = control.LABL_TEXT;
                     //Mbsp1Rwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mbsp1Rwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsp1mont_lb":
                     Mbsp1Mont_Lb.Text = control.LABL_TEXT;
                     //Mbsp1Mont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mbsp1Mont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsp1day_lb":
                     Mbsp1Day_Lb.Text = control.LABL_TEXT;
                     //Mbsp1Day_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mbsp1Day_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsp1strtdate_lb":
                     Mbsp1StrtDate_Lb.Text = control.LABL_TEXT;
                     //Mbsp1StrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mbsp1StrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsp1enddate_lb":
                     Mbsp1EndDate_Lb.Text = control.LABL_TEXT;
                     //Mbsp1EndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mbsp1EndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbco_butn":
                     Mbco_Butn.Text = control.LABL_TEXT;
                     //Mbco_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Mbco_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbfzrwno_lb":
                     MbfzRwno_Lb.Text = control.LABL_TEXT;
                     //MbfzRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbfzRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbfzday_lb":
                     MbfzDay_Lb.Text = control.LABL_TEXT;
                     //MbfzDay_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbfzDay_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "stopblocking_butn":
                     StopBlocking_Butn.Text = control.LABL_TEXT;
                     //StopBlocking_Butn.Text = control.LABL_TEXT; // ToolTip
                     //StopBlocking_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbfzstrtdate_lb":
                     MbfzStrtDate_Lb.Text = control.LABL_TEXT;
                     //MbfzStrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbfzStrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbfzenddate_lb":
                     MbfzEndDate_Lb.Text = control.LABL_TEXT;
                     //MbfzEndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbfzEndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop4_gb":
                     Grop4_Gb.Text = control.LABL_TEXT;
                     //Grop4_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop4_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqid_lb":
                     Rqid_Lb.Text = control.LABL_TEXT;
                     //Rqid_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rqid_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode_lb":
                     RqtpCode_Lb.Text = control.LABL_TEXT;
                     //RqtpCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fighstat_lb":
                     FighStat_Lb.Text = control.LABL_TEXT;
                     //FighStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FighStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop5_gb":
                     Grop5_Gb.Text = control.LABL_TEXT;
                     //Grop5_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop5_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon_lb":
                     CellPhon_Lb.Text = control.LABL_TEXT;
                     //CellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon_lb":
                     TellPhon_Lb.Text = control.LABL_TEXT;
                     //TellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop6_gb":
                     Grop6_Gb.Text = control.LABL_TEXT;
                     //Grop6_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop6_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop8_gb":
                     Grop8_Gb.Text = control.LABL_TEXT;
                     //Grop8_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop8_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno1_lb":
                     FileNo1_Lb.Text = control.LABL_TEXT;
                     //FileNo1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNo1_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "fathname_lb":
                     FathName_Lb.Text = control.LABL_TEXT;
                     //FathName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FathName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdate1_lb":
                     BrthDate1_Lb.Text = control.LABL_TEXT;
                     //BrthDate1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDate1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "actvtag1_lb":
                     ActvTag1_Lb.Text = control.LABL_TEXT;
                     //ActvTag1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ActvTag1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fgpbrwno_lb":
                     FgpbRwno_Lb.Text = control.LABL_TEXT;
                     //FgpbRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FgpbRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "natlcode1_lb":
                     NatlCode1_Lb.Text = control.LABL_TEXT;
                     //NatlCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype_lb":
                     SexType_Lb.Text = control.LABL_TEXT;
                     //SexType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SexType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "disecode_lb":
                     DiseCode_Lb.Text = control.LABL_TEXT;
                     //DiseCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DiseCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode1_lb":
                     RqtpCode1_Lb.Text = control.LABL_TEXT;
                     //RqtpCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop9_gb":
                     Grop9_Gb.Text = control.LABL_TEXT;
                     //Grop9_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop9_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "clubname_lb":
                     ClubName_Lb.Text = control.LABL_TEXT;
                     //ClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodcode_lb":
                     MtodCode_Lb.Text = control.LABL_TEXT;
                     //MtodCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MtodCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "globcode_lb":
                     GlobCode_Lb.Text = control.LABL_TEXT;
                     //GlobCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GlobCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochdeg_lb":
                     CochDeg_Lb.Text = control.LABL_TEXT;
                     //CochDeg_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochDeg_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "insrdate1_lb":
                     InsrDate1_Lb.Text = control.LABL_TEXT;
                     //InsrDate1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InsrDate1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno_lb":
                     CochFileNo_Lb.Text = control.LABL_TEXT;
                     //CochFileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgycode_lb":
                     CtgyCode_Lb.Text = control.LABL_TEXT;
                     //CtgyCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CtgyCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fgtptype_lb":
                     FgtpType_Lb.Text = control.LABL_TEXT;
                     //FgtpType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FgtpType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gugedeg_lb":
                     GugeDeg_Lb.Text = control.LABL_TEXT;
                     //GugeDeg_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GugeDeg_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "instdate1_lb":
                     InstDate1_Lb.Text = control.LABL_TEXT;
                     //InstDate1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InstDate1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bank_lb":
                     Bank_Lb.Text = control.LABL_TEXT;
                     //Bank_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Bank_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "histdesc_lb":
                     HistDesc_Lb.Text = control.LABL_TEXT;
                     //HistDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //HistDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop10_gb":
                     Grop10_Gb.Text = control.LABL_TEXT;
                     //Grop10_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop10_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon1_lb":
                     CellPhon1_Lb.Text = control.LABL_TEXT;
                     //CellPhon1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon1_lb":
                     TellPhon1_Lb.Text = control.LABL_TEXT;
                     //TellPhon1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "emaladrs_lb":
                     EmalAdrs_Lb.Text = control.LABL_TEXT;
                     //EmalAdrs_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EmalAdrs_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "postadrs_lb":
                     PostAdrs_Lb.Text = control.LABL_TEXT;
                     //PostAdrs_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PostAdrs_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "blobgrop_lb":
                     BlobGrop_Lb.Text = control.LABL_TEXT;
                     //BlobGrop_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BlobGrop_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthplac_lb":
                     BrthPlac_Lb.Text = control.LABL_TEXT;
                     //BrthPlac_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthPlac_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "issuplac_lb":
                     IssuPlac_Lb.Text = control.LABL_TEXT;
                     //IssuPlac_Lb.Text = control.LABL_TEXT; // ToolTip
                     //IssuPlac_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fathname1_lb":
                     FathName1_Lb.Text = control.LABL_TEXT;
                     //FathName1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FathName1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chatid_lb":
                     ChatId_Lb.Text = control.LABL_TEXT;
                     //ChatId_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChatId_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chatid1_lb":
                     ChatId1_Lb.Text = control.LABL_TEXT;
                     //ChatId1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChatId1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop11_gb":
                     Grop11_Gb.Text = control.LABL_TEXT;
                     //Grop11_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop11_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "prvncode1_lb":
                     PrvnCode1_Lb.Text = control.LABL_TEXT;
                     //PrvnCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PrvnCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "regncode1_lb":
                     RegnCode1_Lb.Text = control.LABL_TEXT;
                     //RegnCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RegnCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "grop12_gb":
                     Grop12_Gb.Text = control.LABL_TEXT;
                     //Grop12_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Grop12_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "orgncode_lb":
                     OrgnCode_Lb.Text = control.LABL_TEXT;
                     //OrgnCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //OrgnCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "deptcode_lb":
                     DeptCode_Lb.Text = control.LABL_TEXT;
                     //DeptCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DeptCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "buntcode_lb":
                     BuntCode_Lb.Text = control.LABL_TEXT;
                     //BuntCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BuntCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode_lb":
                     SuntCode_Lb.Text = control.LABL_TEXT;
                     //SuntCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SuntCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "reglyear_clm":
                     ReglYear_Clm.Caption = control.LABL_TEXT;
                     //ReglYear_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ReglYear_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode_clm":
                     RqtpCode_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlexpnpric_clm":
                     TotlExpnPric_Clm.Caption = control.LABL_TEXT;
                     //TotlExpnPric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlExpnPric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sumpymtdscn_clm":
                     SumPymtDscn_Clm.Caption = control.LABL_TEXT;
                     //SumPymtDscn_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SumPymtDscn_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sumrcptamnt_clm":
                     SumRcptAmnt_Clm.Caption = control.LABL_TEXT;
                     //SumRcptAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SumRcptAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totldebtamnt_clm":
                     TotlDebtAmnt_Clm.Caption = control.LABL_TEXT;
                     //TotlDebtAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlDebtAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby_clm":
                     CretBy_Clm.Caption = control.LABL_TEXT;
                     //CretBy_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretBy_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtcretdate_clm":
                     PymtCretDate_Clm.Caption = control.LABL_TEXT;
                     //PymtCretDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PymtCretDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby_clm":
                     MdfyBy_Clm.Caption = control.LABL_TEXT;
                     //MdfyBy_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdfyBy_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "natlcode2_lb":
                     NatlCode2_Lb.Text = control.LABL_TEXT;
                     //NatlCode2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "servno1_lb":
                     ServNo1_Lb.Text = control.LABL_TEXT;
                     //ServNo1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ServNo1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode1_lb":
                     SuntCode1_Lb.Text = control.LABL_TEXT;
                     //SuntCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SuntCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydtlist_lb":
                     PydtList_Lb.Text = control.LABL_TEXT;
                     //PydtList_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PydtList_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno1_clm":
                     CochFileNo1_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CochFileNo1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodcode1_clm":
                     MtodCode1_Clm.Caption = control.LABL_TEXT;
                     //MtodCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MtodCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgycode1_clm":
                     CtgyCode1_Lb.Caption = control.LABL_TEXT;
                     //CtgyCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CtgyCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expncode1_clm":
                     ExpnCode1_Clm.Caption = control.LABL_TEXT;
                     //ExpnCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExpnCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnpric_clm":
                     ExpnPric_Clm.Caption = control.LABL_TEXT;
                     //ExpnPric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnPric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnextrprct_clm":
                     ExpnExtrPrct_Clm.Caption = control.LABL_TEXT;
                     //ExpnExtrPrct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnExtrPrct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "qnty_clm":
                     Qnty_Clm.Caption = control.LABL_TEXT;
                     //Qnty_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Qnty_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlexpn_clm":
                     TotlExpn_Clm.Caption = control.LABL_TEXT;
                     //TotlExpn_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlExpn_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydslist_lb":
                     PydsList_Lb.Text = control.LABL_TEXT;
                     //PydsList_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PydsList_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "pydsdesc_clm":
                     PydsDesc_Clm.Caption = control.LABL_TEXT;
                     //PydsDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydsDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtlist_lb":
                     PymtList_Lb.Text = control.LABL_TEXT;
                     //PymtList_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PymtList_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtrwno_clm":
                     PymtRwno_Clm.Caption = control.LABL_TEXT;
                     //PymtRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PymtRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtamnt_clm":
                     PymtAmnt_Clm.Caption = control.LABL_TEXT;
                     //PymtAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PymtAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pymtrcptmtod_clm":
                     PymtRcptMtod_Clm.Caption = control.LABL_TEXT;
                     //PymtRcptMtod_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PymtRcptMtod_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "paiddate_clm":
                     PaidDate_Clm.Caption = control.LABL_TEXT;
                     //PaidDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PaidDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode1_clm":
                     RqtpCode1_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqid_clm":
                     Rqid_Clm.Caption = control.LABL_TEXT;
                     //Rqid_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Rqid_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpdesc1_clm":
                     RqtpDesc1_Clm.Caption = control.LABL_TEXT;
                     //RqtpDesc1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpDesc1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqstdate_clm":
                     RqstDate_Clm.Caption = control.LABL_TEXT;
                     //RqstDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqstDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "savedate_clm":
                     SaveDate_Clm.Caption = control.LABL_TEXT;
                     //SaveDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SaveDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode2_clm":
                     RqtpCode2_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpdesc2_clm":
                     RqtpDesc2_Clm.Caption = control.LABL_TEXT;
                     //RqtpDesc2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpDesc2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttdesc_clm":
                     RqttDesc_Clm.Caption = control.LABL_TEXT;
                     //RqttDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqttDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqstdate2_clm":
                     RqstDate2_Clm.Caption = control.LABL_TEXT;
                     //RqstDate2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqstDate2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqsttime2_clm":
                     RqstTime2_Clm.Caption = control.LABL_TEXT;
                     //RqstTime2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqstTime2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "savedate2_clm":
                     SaveDate2_Clm.Caption = control.LABL_TEXT;
                     //SaveDate2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SaveDate2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby2_clm":
                     CretBy2_Clm.Caption = control.LABL_TEXT;
                     //CretBy2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretBy2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlamnt_clm":
                     TotlAmnt_Clm.Caption = control.LABL_TEXT;
                     //TotlAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totldscnamnt_clm":
                     TotlDscnAmnt_Clm.Caption = control.LABL_TEXT;
                     //TotlDscnAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlDscnAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "toltrcptamnt_clm":
                     ToltRcptAmnt_Clm.Caption = control.LABL_TEXT;
                     //ToltRcptAmnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ToltRcptAmnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "actndate_clm":
                     ActnDate_Clm.Caption = control.LABL_TEXT;
                     //ActnDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ActnDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "entrtime_clm":
                     EntrTime_Clm.Caption = control.LABL_TEXT;
                     //EntrTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EntrTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "exittime_clm":
                     ExitTime_Clm.Caption = control.LABL_TEXT;
                     //ExitTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExitTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnstat_clm":
                     AttnStat_Clm.Caption = control.LABL_TEXT;
                     //AttnStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AttnStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsprwno_clm":
                     MbspRwno_Clm.Caption = control.LABL_TEXT;
                     //MbspRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MbspRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "attntype_clm":
                     AttnType_Clm.Caption = control.LABL_TEXT;
                     //AttnType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AttnType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno2_clm":
                     CochFileNo2_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CochFileNo2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby2_clm":
                     MdfyBy2_Clm.Caption = control.LABL_TEXT;
                     //MdfyBy2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdfyBy2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfydate2_clm":
                     MdfyDate2_Clm.Caption = control.LABL_TEXT;
                     //MdfyDate2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdfyDate2_Clm.Caption = control.LABL_TEXT; // Place Holder
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
            if (job.Input == null) return;

            /*Tc_Info.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 50, Screen.PrimaryScreen.WorkingArea.Height - 50);
            Tc_Info.Left = 30;
            Tc_Info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            */
            tb_master.SelectedTab = tp_001;
            UserProFile_Rb.ImageVisiable = true;

            fileno = Convert.ToInt64((job.Input as XElement).Attributes("fileno").First().Value);
            try
            {
               UserProFile_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
               else
                  UserProFile_Rb.ImageProfile = bm;
            }
            catch { //Pb_FighImg.Visible = false;
               UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482; 
            }

            // 1395/11/26 * اگر مشترک غیرفعال باشد باید از لیست مربوط به مشترکین غیرفعال استفاده کرد
            var crntinfo = iScsc.Fighters.First(f => f.FILE_NO == fileno);
            if (Convert.ToInt32(crntinfo.ACTV_TAG_DNRM) <= 100)
               vF_Last_Info_FighterBs.DataSource = iScsc.VF_Last_Info_Deleted_Fighter(fileno, null, null, null, null, null, null, null, null, null, null, null, null, null);
            else
               vF_Last_Info_FighterBs.DataSource = iScsc.VF_Last_Info_Fighter(fileno, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

            if (crntinfo.SEX_TYPE_DNRM == "001")
            {
               TitlForm_Lb.Appearance.Image = System.Scsc.Properties.Resources.IMAGE_1076;
               NameDnrm_Lb.Image = System.Scsc.Properties.Resources.IMAGE_1620;
            }
            else
            {
               TitlForm_Lb.Appearance.Image = System.Scsc.Properties.Resources.IMAGE_1507;
               NameDnrm_Lb.Image = System.Scsc.Properties.Resources.IMAGE_1621;
            }

            // 1397/05/14 * بررسی اینکه مشترک قفل می باشد
            if(crntinfo.FIGH_STAT == "002")
            {
               Rqid_Txt.BackColor = RqtpDesc_Txt.BackColor = FighStat_Txt.BackColor = SystemColors.Control;
               RqtpDesc_Txt.Text = "";
            }
            else
            {
               Rqid_Txt.BackColor = RqtpDesc_Txt.BackColor = FighStat_Txt.BackColor = Color.YellowGreen;
               RqtpDesc_Txt.Text = iScsc.Request_Types.First(rt => rt.CODE == rt.Requests.First(r => r.RQID == crntinfo.RQST_RQID).RQTP_CODE).RQTP_DESC; ;
            }

            vF_Request_DocumentBs.DataSource = iScsc.VF_Request_Document(fileno);
            var imag =
               iScsc.Image_Documents
               .Where(id =>
                  vF_Request_DocumentBs.List.OfType<Data.VF_Request_DocumentResult>()
                  .Select(rd => rd.RQID)
                  .Contains(id.Receive_Document.Request_Row.RQST_RQID)
               );
            TotlDcmt_Txt.Text = imag.Count().ToString();
            SaveDcmt_Txt.Text = imag.Where(id => id.IMAG.Length > 100).Count().ToString();
            NullDcmt_Txt.Text = (Convert.ToInt32(TotlDcmt_Txt.Text) - Convert.ToInt32(SaveDcmt_Txt.Text)).ToString();
               

            //vF_All_Info_FightersBs.DataSource = iScsc.VF_All_Info_Fighters(fileno).OrderByDescending(f => f.RWNO);
            //////vF_SavePaymentsBs.DataSource = iScsc.VF_Payments(null, null, fileno, null, null, null, null).OrderByDescending(p => p.ISSU_DATE);
            
            //vF_SavePaymentsBs.DataSource = iScsc.VF_Save_Payments(null, fileno).OrderByDescending(p => p.PYMT_CRET_DATE);
            //ShowCrntReglYear_Butn_Click(null, null);

            //vF_Request_ChangingBs.DataSource = iScsc.VF_Request_Changing(fileno).OrderBy(r => r.RQST_DATE);
            //vF_Request_DocumentBs.DataSource = iScsc.VF_Request_Document(fileno);
            //AttnBs2.DataSource = iScsc.Attendances.Where(a => a.FIGH_FILE_NO == fileno);
            
            CochBs1.DataSource = iScsc.Fighters.Where(c => c.FGPB_TYPE_DNRM == "003");

            //var crnt = vF_Last_Info_FighterBs.Current as Data.VF_Last_Info_FighterResult;

            if(Convert.ToInt32(crntinfo.ACTV_TAG_DNRM) <= 100)
            {
               // غیر فعال
               aCTV_TAGTextBox.BackColor = Color.PaleVioletRed;
               aCTV_TAGTextBox.ForeColor = Color.White;
            }
            else
            {
               // فعال
               aCTV_TAGTextBox.BackColor = Color.YellowGreen;
               aCTV_TAGTextBox.ForeColor = Color.Black;
            }

            //var mbcofigh = iScsc.Fighters.First(f => f.FILE_NO == fileno);
            if(crntinfo.MBCO_RWNO_DNRM != null)
            {
               Mbco_Pn.Visible = true;
               var mbco = iScsc.Member_Ships.First(m => m.FIGH_FILE_NO == crntinfo.FILE_NO && m.RWNO == crntinfo.MBCO_RWNO_DNRM && m.RECT_CODE == "004");
               StrtPrivSesn_Date.Value = mbco.STRT_DATE;
               EndPrivSesn_Date.Value = mbco.END_DATE;
               MbcoRwno_Txt.Text = mbco.RWNO.ToString();
               MbcoMont_Txt.Text = mbco.NUMB_OF_MONT_DNRM.ToString();
               MbcoDays_Txt.Text = mbco.NUMB_OF_DAYS_DNRM.ToString();
            }
            else
            {
               Mbco_Pn.Visible = false;
            }

            if (crntinfo.MBFZ_RWNO_DNRM != null)
            {
               var mbfz = iScsc.Member_Ships.First(m => m.FIGH_FILE_NO == crntinfo.FILE_NO && m.RWNO == crntinfo.MBFZ_RWNO_DNRM && m.RECT_CODE == "004");

               Mbfz_pn.Visible = mbfz.END_DATE.Value.Date >= DateTime.Now.Date ? true : false;
               StrtBlok_Date.Value = mbfz.STRT_DATE;
               EndBlok_Date.Value = mbfz.END_DATE;
               MbfzRwno_Txt.Text = mbfz.RWNO.ToString();
               MbfzDays_Txt.Text = mbfz.NUMB_OF_DAYS_DNRM.ToString();
            }
            else
            {
               Mbfz_pn.Visible = false;
            }

            // 1396/10/13 * نمایش لیست دوره های ثبت نام شده
            MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == fileno && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
            
            // مبلغ بدهی
            PayDebtAmnt_Txt.Text = DebtDnrm_TextBox.Text;

            if(isFirstLoaded) goto commandfinished;

            DDebtBs.DataSource = iScsc.D_DEBTs;
            DAttpBs.DataSource = iScsc.D_ATTPs;
            DActvBs.DataSource = iScsc.D_ACTVs;
            DRcmtBs.DataSource = iScsc.D_RCMTs;
            DAtypBs.DataSource = iScsc.D_ATYPs;
            SuntBs1.DataSource = iScsc.Sub_Units;
            DPydsBs1.DataSource = iScsc.D_PYDS;
            VPosBs1.DataSource = iScsc.V_Pos_Devices;
            if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
               Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;

            
            /*vF_TestBs.DataSource = iScsc.VF_Test(fileno);
            vF_CampititionBs.DataSource = iScsc.VF_Campitition(fileno);
            vF_Physical_FitnessBs.DataSource = iScsc.VF_Physical_Fitness(fileno).OrderByDescending(p => p.RWNO);
            vF_Calculate_CalorieBs.DataSource = iScsc.VF_Calculate_Calorie(fileno).OrderByDescending(c => c.RWNO);
            vF_Heart_ZoneBs.DataSource = iScsc.VF_Heart_Zone(fileno).OrderByDescending(h => h.RWNO);
            vF_ExamBs.DataSource = iScsc.VF_Exam(fileno).OrderByDescending(e => e.RWNO);
            */
            isFirstLoaded = true;
            commandfinished:
            ;
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void LoadDataSource(Job job)
      {
         Refresh_Butn_Click(null, null);
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput.Attribute("type").Value == "refresh")
         {
            Execute_Query();
            if(xinput.Attribute("tabfocued").Value == "tp_003")
            {
               tb_master.SelectedTab = tp_003;
            }
         }
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

            var rqst = vF_SavePaymentsBs.Current as Data.VF_Save_PaymentsResult;
            if (rqst == null) return;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            //var rqtpcode = rqst.RQTP_CODE;//RcevXData.Element("PosRespons").Attribute("rqtpcode").Value;
            var rqid = rqst.RQID;//RcevXData.Element("PosRespons").Attribute("rqid").Value;
            var fileno = rqst.FIGH_FILE_NO;//RcevXData.Element("PosRespons").Attribute("fileno").Value;
            var cashcode = rqst.CASH_CODE;//RcevXData.Element("PosRespons").Element("Payment").Attribute("cashcode").Value;
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
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
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

            foreach (var pymt in vF_SavePaymentsBs.List.OfType<Data.VF_Save_PaymentsResult>().Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date))
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
