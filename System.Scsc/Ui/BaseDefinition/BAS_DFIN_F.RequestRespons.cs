using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.BaseDefinition
{
   partial class BAS_DFIN_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
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
         }
         else if (keyData == Keys.Escape)
         {
            job.Next = new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
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
         else if(keyData == Keys.F5)
         {
            if(Tb_Master.SelectedTab == tp_005 || Tb_Master.SelectedTab == tp_006)
            {
               SaveClubMethod_Butn_Click(null, null);
            }
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

         var GetUserAccount =
            new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);

         _DefaultGateway.Gateway(
            GetUserAccount
         );
         CurrentUser = GetUserAccount.Output.ToString();

         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

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
                  case "tp_003":
                     tp_003.Text = control.LABL_TEXT;
                     //tp_003.Text = control.LABL_TEXT; // ToolTip
                     //tp_003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodtitl_lb":
                     MtodTitl_Lb.Text = control.LABL_TEXT;
                     //MtodTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MtodTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcbmttimestat_clm":
                     CbmtCbmtTimeStat_Clm.Caption = control.LABL_TEXT;
                     //CbmtCbmtTimeStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtCbmtTimeStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1cochfileno_clm":
                     Cbmt1CochFileNo_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1CochFileNo_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1CochFileNo_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1daytype_clm":
                     Cbmt1DayType_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1DayType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1DayType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "hldydesc_clm":
                     HldyDesc_Clm.Caption = control.LABL_TEXT;
                     //HldyDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //HldyDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "tilt_lb":
                     Tilt_Lb.Text = control.LABL_TEXT;
                     //Tilt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Tilt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cash_butn":
                     Cash_Butn.Text = control.LABL_TEXT;
                     //Cash_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Cash_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amail_butn":
                     Amail_Butn.Text = control.LABL_TEXT;
                     //Amail_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Amail_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "loginmethod_butn":
                     LoginMethod_Butn.Text = control.LABL_TEXT;
                     //LoginMethod_Butn.Text = control.LABL_TEXT; // ToolTip
                     //LoginMethod_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "prvnregn_butn":
                     PrvnRegn_Butn.Text = control.LABL_TEXT;
                     //PrvnRegn_Butn.Text = control.LABL_TEXT; // ToolTip
                     //PrvnRegn_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "coach_butn":
                     Coach_Butn.Text = control.LABL_TEXT;
                     //Coach_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Coach_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "club_butn":
                     Club_Butn.Text = control.LABL_TEXT;
                     //Club_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Club_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "hldyday_butn":
                     HldyDay_Butn.Text = control.LABL_TEXT;
                     //HldyDay_Butn.Text = control.LABL_TEXT; // ToolTip
                     //HldyDay_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashtitl_lb":
                     CashTitl_Lb.Text = control.LABL_TEXT;
                     //CashTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CashTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashname_clm":
                     CashName_Clm.Caption = control.LABL_TEXT;
                     //CashName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CashName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashtype_lb":
                     CashType_Lb.Text = control.LABL_TEXT;
                     //CashType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CashType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashstat_lb":
                     CashStat_Lb.Text = control.LABL_TEXT;
                     //CashStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CashStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bankname_lb":
                     BankName_Lb.Text = control.LABL_TEXT;
                     //BankName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BankName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bankbanccode_lb":
                     BankBancCode_Lb.Text = control.LABL_TEXT;
                     //BankBancCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BankBancCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashoptn_lb":
                     CashOptn_Lb.Text = control.LABL_TEXT;
                     //CashOptn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CashOptn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cnctsoftacnt_lnk":
                     CnctSoftAcnt_Lnk.Text = control.LABL_TEXT;
                     //CnctSoftAcnt_Lnk.Text = control.LABL_TEXT; // ToolTip
                     //CnctSoftAcnt_Lnk.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "regulation_lnk":
                     Regulation_Lnk.Text = control.LABL_TEXT;
                     //Regulation_Lnk.Text = control.LABL_TEXT; // ToolTip
                     //Regulation_Lnk.Text = control.LABL_TEXT; // Place Holder
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
                  case "tp_004":
                     tp_004.Text = control.LABL_TEXT;
                     //tp_004.Text = control.LABL_TEXT; // ToolTip
                     //tp_004.Text = control.LABL_TEXT; // Place Holder
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
                  case "tp_007":
                     tp_007.Text = control.LABL_TEXT;
                     //tp_007.Text = control.LABL_TEXT; // ToolTip
                     //tp_007.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnitemtitl_lb":
                     ExpnItemTitl_Lb.Text = control.LABL_TEXT;
                     //ExpnItemTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExpnItemTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "income_lb":
                     InCome_Lb.Text = control.LABL_TEXT;
                     //InCome_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InCome_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "iepitdesc_clm":
                     IEpitDesc_Clm.Caption = control.LABL_TEXT;
                     //IEpitDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //IEpitDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "exitname_lb":
                     ExitName_Lb.Text = control.LABL_TEXT;
                     //ExitName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExitName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode_lb":
                     RqtpCode_Lb.Text = control.LABL_TEXT;
                     //RqtpCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_lb":
                     RqttCode_Lb.Text = control.LABL_TEXT;
                     //RqttCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqttCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "outcome_lb":
                     OutCome_Lb.Text = control.LABL_TEXT;
                     //OutCome_Lb.Text = control.LABL_TEXT; // ToolTip
                     //OutCome_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "oexitcode_clm":
                     OExitCode_Clm.Caption = control.LABL_TEXT;
                     //OExitCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OExitCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "oexitname_lb":
                     OExitName_Lb.Text = control.LABL_TEXT;
                     //OExitName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //OExitName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtod_lb":
                     Mtod_Lb.Text = control.LABL_TEXT;
                     //Mtod_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Mtod_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtoddesc_clm":
                     MtodDesc_Clm.Caption = control.LABL_TEXT;
                     //MtodDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MtodDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodstat_clm":
                     MtodStat_Clm.Caption = control.LABL_TEXT;
                     //MtodStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MtodStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtoddfltstat_clm":
                     MtodDfltStat_Clm.Caption = control.LABL_TEXT;
                     //MtodDfltStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MtodDfltStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "chckattnalrm_clm":
                     ChckAttnAlrm_Clm.Caption = control.LABL_TEXT;
                     //ChckAttnAlrm_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ChckAttnAlrm_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgy_lb":
                     Ctgy_Lb.Text = control.LABL_TEXT;
                     //Ctgy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Ctgy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "exitcode_lb":
                     ExitCode_Lb.Text = control.LABL_TEXT;
                     //ExitCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExitCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgydesc_clm":
                     CtgyDesc_Clm.Caption = control.LABL_TEXT;
                     //CtgyDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CtgyDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbofattnmont_clm":
                     NumbOfAttnMont_Clm.Caption = control.LABL_TEXT;
                     //NumbOfAttnMont_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //NumbOfAttnMont_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbcyclday_clm":
                     NumbCyclDay_Clm.Caption = control.LABL_TEXT;
                     //NumbCyclDay_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //NumbCyclDay_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "numbmontofer_clm":
                     NumbMontOfer_Clm.Caption = control.LABL_TEXT;
                     //NumbMontOfer_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //NumbMontOfer_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pric_clm":
                     Pric_Clm.Caption = control.LABL_TEXT;
                     //Pric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Pric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgystat_clm":
                     CtgyStat_Clm.Caption = control.LABL_TEXT;
                     //CtgyStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CtgyStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgydfltstat_clm":
                     CtgyDfltStat_Clm.Caption = control.LABL_TEXT;
                     //CtgyDfltStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CtgyDfltStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "regntitl_lb":
                     RegnTitl_Lb.Text = control.LABL_TEXT;
                     //RegnTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RegnTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cntydesc_lb":
                     CntyDesc_Lb.Text = control.LABL_TEXT;
                     //CntyDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CntyDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cntycode_clm":
                     CntyCode_Clm.Caption = control.LABL_TEXT;
                     //CntyCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CntyCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cntyname_clm":
                     CntyName_Clm.Caption = control.LABL_TEXT;
                     //CntyName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CntyName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "prvndesc_lb":
                     PrvnDesc_Lb.Text = control.LABL_TEXT;
                     //PrvnDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PrvnDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "prvncode_clm":
                     PrvnCode_Clm.Caption = control.LABL_TEXT;
                     //PrvnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PrvnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "prvnname_clm":
                     PrvnName_Clm.Caption = control.LABL_TEXT;
                     //PrvnName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PrvnName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "regndesc_lb":
                     RegnDesc_Lb.Text = control.LABL_TEXT;
                     //RegnDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RegnDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "regncode_clm":
                     RegnCode_Clm.Caption = control.LABL_TEXT;
                     //RegnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RegnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "regnname_clm":
                     RegnName_Clm.Caption = control.LABL_TEXT;
                     //RegnName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RegnName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochtitl_lb":
                     CochTitl_Lb.Text = control.LABL_TEXT;
                     //CochTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochdesc1_lb":
                     CochDesc1_Lb.Text = control.LABL_TEXT;
                     //CochDesc1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochDesc1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newcoach_butn":
                     NewCoach_Butn.Text = control.LABL_TEXT;
                     //NewCoach_Butn.Text = control.LABL_TEXT; // ToolTip
                     //NewCoach_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochname_clm":
                     CochName_Clm.Caption = control.LABL_TEXT;
                     //CochName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CochName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype_clm":
                     SexType_Clm.Caption = control.LABL_TEXT;
                     //SexType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SexType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtdfltstat1_clm":
                     CbmtDfltStat1_Clm.Caption = control.LABL_TEXT;
                     //CbmtDfltStat1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtDfltStat1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtmtodstat1_clm":
                     CbmtMtodStat1_Clm.Caption = control.LABL_TEXT;
                     //CbmtMtodStat1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtMtodStat1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtamnt1_clm":
                     CbmtAmnt1_Clm.Caption = control.LABL_TEXT;
                     //CbmtAmnt1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtAmnt1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtstrttime_clm":
                     CbmtStrtTime_Clm.Caption = control.LABL_TEXT;
                     //CbmtStrtTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtStrtTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cmbtclastime_clm":
                     CmbtClasTime_Clm.Caption = control.LABL_TEXT;
                     //CmbtClasTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CmbtClasTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtendtime_clm":
                     CbmtEndTime_Clm.Caption = control.LABL_TEXT;
                     //CbmtEndTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtEndTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcpctnumb_clm":
                     CbmtCpctNumb_Clm.Caption = control.LABL_TEXT;
                     //CbmtCpctNumb_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtCpctNumb_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcpctstat_clm":
                     CbmtCpctStat_Clm.Caption = control.LABL_TEXT;
                     //CbmtCpctStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtCpctStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcbmttime_clm":
                     CbmtCbmtTime_Clm.Caption = control.LABL_TEXT;
                     //CbmtCbmtTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtCbmtTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtmtodcode_clm":
                     CbmtMtodCode_Clm.Caption = control.LABL_TEXT;
                     //CbmtMtodCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CbmtMtodCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmttitl_lb":
                     CbmtTitl_Lb.Text = control.LABL_TEXT;
                     //CbmtTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtdesc_lb":
                     CbmtDesc_Lb.Text = control.LABL_TEXT;
                     //CbmtDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "addclub_butn":
                     AddClub_Butn.Text = control.LABL_TEXT;
                     //AddClub_Butn.Text = control.LABL_TEXT; // ToolTip
                     //AddClub_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "clubname_clm":
                     ClubName_Clm.Caption = control.LABL_TEXT;
                     //ClubName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ClubName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1mtodcode_clm":
                     Cbmt1MtodCode_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1MtodCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1MtodCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1sextype_clm":
                     Cbmt1SexType_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1SexType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1SexType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1dfltstat_clm":
                     Cbmt1DfltStat_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1DfltStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1DfltStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1mtodstat_clm":
                     Cbmt1MtodStat_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1MtodStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1MtodStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1amnt_clm":
                     Cbmt1Amnt_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1Amnt_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1Amnt_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1strttime_clm":
                     Cbmt1StrtTime_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1StrtTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1StrtTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1clastime_clm":
                     Cbmt1ClasTime_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1ClasTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1ClasTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1endtime_clm":
                     Cbmt1EndTime_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1EndTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1EndTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1cpctnumb_clm":
                     Cbmt1CpctNumb_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1CpctNumb_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1CpctNumb_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1cpctstat_clm":
                     Cbmt1CpctStat_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1CpctStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1CpctStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1cbmttime_clm":
                     Cbmt1CbmtTime_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1CbmtTime_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1CbmtTime_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmt1cbmttimestat_clm":
                     Cbmt1CbmtTimeStat_Clm.Caption = control.LABL_TEXT;
                     //Cbmt1CbmtTimeStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Cbmt1CbmtTimeStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "holydaytitl_lb":
                     HolyDayTitl_Lb.Text = control.LABL_TEXT;
                     //HolyDayTitl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //HolyDayTitl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "holydaydesc1_lb":
                     HolyDayDesc1_Lb.Text = control.LABL_TEXT;
                     //HolyDayDesc1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //HolyDayDesc1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "hldydate_clm":
                     HldyDate_Clm.Caption = control.LABL_TEXT;
                     //HldyDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //HldyDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "hldyyear_clm":
                     HldyYear_Clm.Caption = control.LABL_TEXT;
                     //HldyYear_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //HldyYear_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "hldycycl_clm":
                     HldyCycl_Clm.Caption = control.LABL_TEXT;
                     //HldyCycl_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //HldyCycl_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "hldycretby_clm":
                     HldyCretBy_Clm.Caption = control.LABL_TEXT;
                     //HldyCretBy_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //HldyCretBy_Clm.Caption = control.LABL_TEXT; // Place Holder
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
         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {  GetType().Name, this }  },
         //      new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastOnWall */){ Input = this }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

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
         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
         //      new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
         //         new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

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
         Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>34</Privilege><Sub_Sys>0</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 34 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
         _DefaultGateway.Gateway(_InteractWithJob);
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         DActvBs1.DataSource = iScsc.D_ACTVs;
         DCstpBs1.DataSource = iScsc.D_CSTPs;
         DDytpBs1.DataSource = iScsc.D_DYTPs;
         DSxtpBs1.DataSource = iScsc.D_SXTPs;
         RqtpBs1.DataSource = iScsc.Request_Types.Where(rt => rt.CODE == "001" || rt.CODE == "009" || rt.CODE == "016" || rt.CODE == "012");
         RqttBs1.DataSource = iScsc.Requester_Types.Where(rt => rt.CODE == "001" || rt.CODE == "004");
         DCyclBs1.DataSource = iScsc.D_CYCLs;
         DWkdyBs1.DataSource = iScsc.D_WKDies;
         DysnoBs.DataSource = iScsc.D_YSNOs;
         VCompBs.DataSource = iScsc.V_Computers;
         DbcdtBs1.DataSource = iScsc.D_BCDTs;
         DAeatBs.DataSource = iScsc.D_AEATs;
         DatsmBs1.DataSource = iScsc.D_ATSMs;
         DAttpBs.DataSource = iScsc.D_ATTPs;
         DcxtpBs.DataSource = iScsc.D_CXTPs;
         DcetpBs.DataSource = iScsc.D_CETPs;
         DRectBs.DataSource = iScsc.D_RECTs;
         DConfBs.DataSource = iScsc.D_CONFs;
         UserBs1.DataSource = iScsc.V_Users;
         DMsgtBs.DataSource = iScsc.D_MSGTs;
         DEfdtBs.DataSource = iScsc.D_EFDTs;

         AttnComPortName_Lov.Items.Clear();
         GateComPortName_Lov.Items.Clear();
         ExpnComPortName_Lov.Items.Clear();

         AttnComPortName_Lov.Items.AddRange(SerialPort.GetPortNames());
         GateComPortName_Lov.Items.AddRange(SerialPort.GetPortNames());
         ExpnComPortName_Lov.Items.AddRange(SerialPort.GetPortNames());


         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         if (job.Input != null)
         {
            var xinput = job.Input as XElement;

            switch(xinput.Attribute("showtabpage").Value)
            {
               case "tp_005":
                  fetchagine = true;
                  SwitchButtonsTabPage(Coach_Butn);
                  break;
               case "tp_006":
                  SwitchButtonsTabPage(Club_Butn);
                  break;
               default:
                  SwitchButtonsTabPage(Cash_Butn);
                  break;
            }
            
         }
         else
         {
            SwitchButtonsTabPage(Cash_Butn);
         }
         job.Status = StatusType.Successful;
      }
   }
}
