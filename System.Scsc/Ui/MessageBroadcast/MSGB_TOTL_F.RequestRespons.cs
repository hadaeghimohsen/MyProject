using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.MessageBroadcast
{
   partial class MSGB_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private bool isFirstLoaded = false;
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
            //case 11:
            //   ValidatePhoneNumber(job);
            //   break;
            default:
               break;
         }
      }

      //private void ValidatePhoneNumber(Job job)
      //{
      //  try
      //  {
      //     if (string.IsNullOrEmpty(cLUB_NAMETextEdit.Text))
      //          return;
      //     var r = new Regex(@"^09\d{2}\s*?\d{3}\s*?\d{4}$");
      //      MessageBox.Show( r.IsMatch(cLUB_NAMETextEdit.Text).ToString());
      //  }
      //  catch (Exception)
      //  {
      //      throw;
      //  }
      //}

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
         }
         //else if (keyData == Keys.F11)
         //{
         //   job.Next =
         //      new Job(SendType.SelfToUserInterface, this.GetType().Name, 11 /* Execute ValidatePhoneNumber */);
         //}
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
                  case "notcbmtinsclubstat_lb":
                     NotCbmtInsClubStat_Lb.Text = control.LABL_TEXT;
                     //NotCbmtInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtsmsstat_lb":
                     NotCbmtSmsStat_Lb.Text = control.LABL_TEXT;
                     //NotCbmtSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtclubname_lb":
                     NotCbmtClubName_Lb.Text = control.LABL_TEXT;
                     //NotCbmtClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtsmstextcont_lb":
                     NotCbmtSmsTextCont_Lb.Text = control.LABL_TEXT;
                     //NotCbmtSmsTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtSmsTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmttextcont_lb":
                     CbmtTextCont_Lb.Text = control.LABL_TEXT;
                     //CbmtTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_manualselection002":
                     Rb_ManualSelection002.Text = control.LABL_TEXT;
                     //Rb_ManualSelection002.Text = control.LABL_TEXT; // ToolTip
                     //Rb_ManualSelection002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtsmsstat_lb":
                     DebtSmsStat_Lb.Text = control.LABL_TEXT;
                     //DebtSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_manualselection004":
                     Rb_ManualSelection004.Text = control.LABL_TEXT;
                     //Rb_ManualSelection004.Text = control.LABL_TEXT; // ToolTip
                     //Rb_ManualSelection004.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adsinsclubstat_lb":
                     AdsInsClubStat_Lb.Text = control.LABL_TEXT;
                     //AdsInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_manualselection005":
                     Rb_ManualSelection005.Text = control.LABL_TEXT;
                     //Rb_ManualSelection005.Text = control.LABL_TEXT; // ToolTip
                     //Rb_ManualSelection005.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnsmstype_lb":
                     AttnSmsType_Lb.Text = control.LABL_TEXT;
                     //AttnSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attntext_lb":
                     AttnText_Lb.Text = control.LABL_TEXT;
                     //AttnText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnText_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "tp_008":
                     tp_008.Text = control.LABL_TEXT;
                     //tp_008.Text = control.LABL_TEXT; // ToolTip
                     //tp_008.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdatetelgstat_lb":
                     BrthDateTelgStat_Lb.Text = control.LABL_TEXT;
                     //BrthDateTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdatesmsstat_lb":
                     BrthDateSmsStat_Lb.Text = control.LABL_TEXT;
                     //BrthDateSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdateinsnamestat_lb":
                     BrthDateInsNameStat_Lb.Text = control.LABL_TEXT;
                     //BrthDateInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdateclubcode_lb":
                     BrthDateClubCode_Lb.Text = control.LABL_TEXT;
                     //BrthDateClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdatesmstype_lb":
                     BrthDateSmsType_Lb.Text = control.LABL_TEXT;
                     //BrthDateSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdateinsclubstat_lb":
                     BrthDateInsClubStat_Lb.Text = control.LABL_TEXT;
                     //BrthDateInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdateclubname_lb":
                     BrthDateClubName_Lb.Text = control.LABL_TEXT;
                     //BrthDateClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdatesmstext_lb":
                     BrthDateSmsText_Lb.Text = control.LABL_TEXT;
                     //BrthDateSmsText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateSmsText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdatesmstextcont_lb":
                     BrthDateSmsTextCont_Lb.Text = control.LABL_TEXT;
                     //BrthDateSmsTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDateSmsTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmttelgstat_lb":
                     NotCbmtTelgStat_Lb.Text = control.LABL_TEXT;
                     //NotCbmtTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtinsnamestat_lb":
                     NotCbmtInsNameStat_Lb.Text = control.LABL_TEXT;
                     //NotCbmtInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtclubcode_lb":
                     NotCbmtClubCode_Lb.Text = control.LABL_TEXT;
                     //NotCbmtClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtsmstype_lb":
                     NotCbmtSmsType_Lb.Text = control.LABL_TEXT;
                     //NotCbmtSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtsmstext_lb":
                     NotCbmtSmsText_Lb.Text = control.LABL_TEXT;
                     //NotCbmtSmsText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtSmsText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmttelgstat_lb":
                     CbmtTelgStat_Lb.Text = control.LABL_TEXT;
                     //CbmtTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtsmsstat_lb":
                     CbmtSmsStat_Lb.Text = control.LABL_TEXT;
                     //CbmtSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtinsnamestat_lb":
                     CbmtInsNameStat_Lb.Text = control.LABL_TEXT;
                     //CbmtInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtclubcode_lb":
                     CbmtClubCode_Lb.Text = control.LABL_TEXT;
                     //CbmtClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtsmstype_lb":
                     CbmtSmsType_Lb.Text = control.LABL_TEXT;
                     //CbmtSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtinsclubstat_lb":
                     CbmtInsClubStat_Lb.Text = control.LABL_TEXT;
                     //CbmtInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtclubname_lb":
                     CbmtClubName_Lb.Text = control.LABL_TEXT;
                     //CbmtClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtsmstext_lb":
                     CbmtSmsText_Lb.Text = control.LABL_TEXT;
                     //CbmtSmsText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtSmsText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_all002":
                     Rb_All002.Text = control.LABL_TEXT;
                     //Rb_All002.Text = control.LABL_TEXT; // ToolTip
                     //Rb_All002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_specfighter002":
                     Rb_SpecFighter002.Text = control.LABL_TEXT;
                     //Rb_SpecFighter002.Text = control.LABL_TEXT; // ToolTip
                     //Rb_SpecFighter002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtcbmtcode_lb":
                     NotCbmtCbmtCode_Lb.Text = control.LABL_TEXT;
                     //NotCbmtCbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtCbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtcochfileno_lb":
                     NotCbmtCochFileNo_Lb.Text = control.LABL_TEXT;
                     //NotCbmtCochFileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtCochFileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtmtodcode_lb":
                     NotCbmtMtodCode_Lb.Text = control.LABL_TEXT;
                     //NotCbmtMtodCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtMtodCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "notcbmtfilenos_lb":
                     NotCbmtFileNos_Lb.Text = control.LABL_TEXT;
                     //NotCbmtFileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NotCbmtFileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_all003":
                     Rb_All003.Text = control.LABL_TEXT;
                     //Rb_All003.Text = control.LABL_TEXT; // ToolTip
                     //Rb_All003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_specfighter003":
                     Rb_SpecFighter003.Text = control.LABL_TEXT;
                     //Rb_SpecFighter003.Text = control.LABL_TEXT; // ToolTip
                     //Rb_SpecFighter003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_manualselection003":
                     Rb_ManualSelection003.Text = control.LABL_TEXT;
                     //Rb_ManualSelection003.Text = control.LABL_TEXT; // ToolTip
                     //Rb_ManualSelection003.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcbmtcode_lb":
                     CbmtCbmtCode_Lb.Text = control.LABL_TEXT;
                     //CbmtCbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtCbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcochfileno_lb":
                     CbmtCochFileNo_Lb.Text = control.LABL_TEXT;
                     //CbmtCochFileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtCochFileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtmtodcode_lb":
                     CbmtMtodCode_Lb.Text = control.LABL_TEXT;
                     //CbmtMtodCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtMtodCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtfilenos_lb":
                     CbmtFileNos_Lb.Text = control.LABL_TEXT;
                     //CbmtFileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtFileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debttelgstat_lb":
                     DebtTelgStat_Lb.Text = control.LABL_TEXT;
                     //DebtTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtinsnamestat_lb":
                     DebtInsNameStat_Lb.Text = control.LABL_TEXT;
                     //DebtInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtclubcode_lb":
                     DebtClubCode_Lb.Text = control.LABL_TEXT;
                     //DebtClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtsmstype_lb":
                     DebtSmsType_Lb.Text = control.LABL_TEXT;
                     //DebtSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtinsclubstat_lb":
                     DebtInsClubStat_Lb.Text = control.LABL_TEXT;
                     //DebtInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtclubname_lb":
                     DebtClubName_Lb.Text = control.LABL_TEXT;
                     //DebtClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debttext_lb":
                     DebtText_Lb.Text = control.LABL_TEXT;
                     //DebtText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debttextcont_lb":
                     DebtTextCont_Lb.Text = control.LABL_TEXT;
                     //DebtTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtamnt_lb":
                     DebtAmnt_Lb.Text = control.LABL_TEXT;
                     //DebtAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_all004":
                     Rb_All004.Text = control.LABL_TEXT;
                     //Rb_All004.Text = control.LABL_TEXT; // ToolTip
                     //Rb_All004.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_specfighter004":
                     Rb_SpecFighter004.Text = control.LABL_TEXT;
                     //Rb_SpecFighter004.Text = control.LABL_TEXT; // ToolTip
                     //Rb_SpecFighter004.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtcbmtcode_lb":
                     DebtCbmtCode_Lb.Text = control.LABL_TEXT;
                     //DebtCbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtCbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtcochfileno_lb":
                     DebtCochFileNo_Lb.Text = control.LABL_TEXT;
                     //DebtCochFileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtCochFileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtmtodcode_lb":
                     DebtMtodCode_Lb.Text = control.LABL_TEXT;
                     //DebtMtodCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtMtodCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtfilenos_lb":
                     DebtFileNos_Lb.Text = control.LABL_TEXT;
                     //DebtFileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtFileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adstelgstat_lb":
                     AdsTelgStat_Lb.Text = control.LABL_TEXT;
                     //AdsTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adssmsstat_lb":
                     AdsSmsStat_Lb.Text = control.LABL_TEXT;
                     //AdsSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adsinsnamestat_lb":
                     AdsInsNameStat_Lb.Text = control.LABL_TEXT;
                     //AdsInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adsclubcode_lb":
                     AdsClubCode_Lb.Text = control.LABL_TEXT;
                     //AdsClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adssmstype_lb":
                     AdsSmsType_Lb.Text = control.LABL_TEXT;
                     //AdsSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adsclubname_lb":
                     AdsClubName_Lb.Text = control.LABL_TEXT;
                     //AdsClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adstext_lb":
                     AdsText_Lb.Text = control.LABL_TEXT;
                     //AdsText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adstextcont_lb":
                     AdsTextCont_Lb.Text = control.LABL_TEXT;
                     //AdsTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_phonenumberfromfile":
                     Rb_PhoneNumberFromFile.Text = control.LABL_TEXT;
                     //Rb_PhoneNumberFromFile.Text = control.LABL_TEXT; // ToolTip
                     //Rb_PhoneNumberFromFile.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_all005":
                     Rb_All005.Text = control.LABL_TEXT;
                     //Rb_All005.Text = control.LABL_TEXT; // ToolTip
                     //Rb_All005.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rb_specfighter005":
                     Rb_SpecFighter005.Text = control.LABL_TEXT;
                     //Rb_SpecFighter005.Text = control.LABL_TEXT; // ToolTip
                     //Rb_SpecFighter005.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adscbmtcode_lb":
                     AdsCbmtCode_Lb.Text = control.LABL_TEXT;
                     //AdsCbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsCbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adscochfileno_lb":
                     AdsCochFileNo_Lb.Text = control.LABL_TEXT;
                     //AdsCochFileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsCochFileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adsmtodcode_lb":
                     AdsMtodCode_Lb.Text = control.LABL_TEXT;
                     //AdsMtodCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsMtodCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adsfilenos_lb":
                     AdsFileNos_Lb.Text = control.LABL_TEXT;
                     //AdsFileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsFileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adscellphon_lb":
                     AdsCellPhon_Lb.Text = control.LABL_TEXT;
                     //AdsCellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdsCellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admtelgstat_lb":
                     AdmTelgStat_Lb.Text = control.LABL_TEXT;
                     //AdmTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admsmsstat_lb":
                     AdmSmsStat_Lb.Text = control.LABL_TEXT;
                     //AdmSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adminsnamestat_lb":
                     AdmInsNameStat_Lb.Text = control.LABL_TEXT;
                     //AdmInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admclubcode_lb":
                     AdmClubCode_Lb.Text = control.LABL_TEXT;
                     //AdmClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admsmstype_lb":
                     AdmSmsType_Lb.Text = control.LABL_TEXT;
                     //AdmSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adminsclubstat_lb":
                     AdmInsClubStat_Lb.Text = control.LABL_TEXT;
                     //AdmInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admclubname_lb":
                     AdmClubName_Lb.Text = control.LABL_TEXT;
                     //AdmClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admtext_lb":
                     AdmText_Lb.Text = control.LABL_TEXT;
                     //AdmText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "admtextcont_lb":
                     AdmTextCont_Lb.Text = control.LABL_TEXT;
                     //AdmTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AdmTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsptelgstat_lb":
                     MbspTelgStat_Lb.Text = control.LABL_TEXT;
                     //MbspTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspsmsstat_lb":
                     MbspSmsStat_Lb.Text = control.LABL_TEXT;
                     //MbspSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspinsnamestat_lb":
                     MbspInsNameStat_Lb.Text = control.LABL_TEXT;
                     //MbspInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspclubcode_lb":
                     MbspClubCode_Lb.Text = control.LABL_TEXT;
                     //MbspClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspsmstype_lb":
                     MbspSmsType_Lb.Text = control.LABL_TEXT;
                     //MbspSmsType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspSmsType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspinsclubstat_lb":
                     MbspInsClubStat_Lb.Text = control.LABL_TEXT;
                     //MbspInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbspclubname_lb":
                     MbspClubName_Lb.Text = control.LABL_TEXT;
                     //MbspClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsptext_lb":
                     MbspText_Lb.Text = control.LABL_TEXT;
                     //MbspText_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspText_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mbsptextcont_lb":
                     MbspTextCont_Lb.Text = control.LABL_TEXT;
                     //MbspTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MbspTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attntelgstat_lb":
                     AttnTelgStat_Lb.Text = control.LABL_TEXT;
                     //AttnTelgStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnTelgStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnsmsstat_lb":
                     AttnSmsStat_Lb.Text = control.LABL_TEXT;
                     //AttnSmsStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnSmsStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attninsnamestat_lb":
                     AttnInsNameStat_Lb.Text = control.LABL_TEXT;
                     //AttnInsNameStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnInsNameStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnclubcode_lb":
                     AttnClubCode_Lb.Text = control.LABL_TEXT;
                     //AttnClubCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnClubCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attninsclubstat_lb":
                     AttnInsClubStat_Lb.Text = control.LABL_TEXT;
                     //AttnInsClubStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnInsClubStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnclubname_lb":
                     AttnClubName_Lb.Text = control.LABL_TEXT;
                     //AttnClubName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnClubName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attntextcont_lb":
                     AttnTextCont_Lb.Text = control.LABL_TEXT;
                     //AttnTextCont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnTextCont_Lb.Text = control.LABL_TEXT; // Place Holder
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
         Job _InteractWithScsc =
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
                              "<Privilege>190</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              job.Status = StatusType.Successful;
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به کد 190");
                              #endregion                           
                           })
                        },
                        #endregion
                     })
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);         
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         if (!isFirstLoaded)
         {
            MsgbBs1.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "001");
            MsgbBs2.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "003");
            MsgbBs3.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "004");
            MsgbBs4.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "002");
            MsgbBs5.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "005");
            MsgbBs6.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "006");
            MsgbBs7.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "007");
            MsgbBs8.DataSource = iScsc.Message_Broadcasts.FirstOrDefault(m => m.MSGB_TYPE == "008");
            DActvBs1.DataSource = iScsc.D_ACTVs;
            DYsnoBs1.DataSource = iScsc.D_YSNOs;
            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DLntpBs.DataSource = iScsc.D_LNTPs;
            ClubBs1.DataSource = iScsc.Clubs;
            CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);               
            CochBs1.DataSource = iScsc.Fighters.Where(c => c.CONF_STAT == "002" && (c.FGPB_TYPE_DNRM == "002" || c.FGPB_TYPE_DNRM == "003"));
            //FighBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && !(f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") && Convert.ToInt32(f.ACTV_TAG_DNRM) >= 101);
            MtodBs1.DataSource = iScsc.Methods;
            isFirstLoaded = true;
         }
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
         job.Status = StatusType.Successful;
      }
   }
}
