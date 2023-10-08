using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.ReportManager
{
   partial class RPT_PMMT_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
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

         if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
            if (!(Find_Butn.Focused || Back_Butn.Focused || Submit_Butn.Focused) )
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F5)
         {
            //Execute_Query();
            Find_Butn_Click(null, null);
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
                  case "rcptmtod_lb":
                     RcptMtod_Lb.Text = control.LABL_TEXT;
                     //RcptMtod_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RcptMtod_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_005":
                     //tp_005.Text = control.LABL_TEXT;
                     //tp_005.Text = control.LABL_TEXT; // ToolTip
                     //tp_005.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno2_clm":
                     FileNo2_Clm.Caption = control.LABL_TEXT;
                     //FileNo2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FileNo2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "todate3_lb":
                     ToDate3_Lb.Text = control.LABL_TEXT;
                     //ToDate3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ToDate3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno3_clm":
                     FileNo3_Clm.Caption = control.LABL_TEXT;
                     //FileNo3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FileNo3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnttype3_clm":
                     AmntType3_Clm.Caption = control.LABL_TEXT;
                     //AmntType3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AmntType3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby_clm":
                     CretBy_Clm.Caption = control.LABL_TEXT;
                     //CretBy_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretBy_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_002":
                     tp_002.Text = control.LABL_TEXT;
                     //tp_002.Text = control.LABL_TEXT; // ToolTip
                     //tp_002.Text = control.LABL_TEXT; // Place Holder
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
                  case "parm2_gb":
                     Parm2_Gb.Text = control.LABL_TEXT;
                     //Parm2_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Parm2_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fromdate2_lb":
                     FromDate2_Lb.Text = control.LABL_TEXT;
                     //FromDate2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FromDate2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "todate2_lb":
                     ToDate2_Lb.Text = control.LABL_TEXT;
                     //ToDate2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ToDate2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode2_lb":
                     RqtpCode2_Lb.Text = control.LABL_TEXT;
                     //RqtpCode2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno2_lb":
                     CochFileNo2_Lb.Text = control.LABL_TEXT;
                     //CochFileNo2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashier2_lb":
                     Cashier2_Lb.Text = control.LABL_TEXT;
                     //Cashier2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Cashier2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cbmtcode2_lb":
                     CbmtCode2_Lb.Text = control.LABL_TEXT;
                     //CbmtCode2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtCode2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "find2_butn":
                     Find2_Butn.Text = control.LABL_TEXT;
                     //Find2_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Find2_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "back2_butn":
                     Back2_Butn.Text = control.LABL_TEXT;
                     //Back2_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Back2_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode2_clm":
                     RqtpCode2_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype2_clm":
                     SexType2_Clm.Caption = control.LABL_TEXT;
                     //SexType2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SexType2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expncode2_clm":
                     ExpnCode2_Clm.Caption = control.LABL_TEXT;
                     //ExpnCode2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ExpnCode2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ownrfileno2_clm":
                     OwnrFileNo2_Clm.Caption = control.LABL_TEXT;
                     //OwnrFileNo2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OwnrFileNo2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydtdesc_clm":
                     PydtDesc_Clm.Caption = control.LABL_TEXT;
                     //PydtDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydtDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "issudate2_clm":
                     IssuDate2_Clm.Caption = control.LABL_TEXT;
                     //IssuDate2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //IssuDate2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "issutime2_clm":
                     IssuTime2_Clm.Caption = control.LABL_TEXT;
                     //IssuTime2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //IssuTime2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnpric2_clm":
                     ExpnPric2_Clm.Caption = control.LABL_TEXT;
                     //ExpnPric2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnPric2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "qnty2_clm":
                     Qnty2_Clm.Caption = control.LABL_TEXT;
                     //Qnty2_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Qnty2_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "totlpric_clm":
                     TotlPric_Clm.Caption = control.LABL_TEXT;
                     //TotlPric_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TotlPric_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "paystat_clm":
                     PayStat_Clm.Caption = control.LABL_TEXT;
                     //PayStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PayStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "parm1_gb":
                     Parm1_Gb.Text = control.LABL_TEXT;
                     //Parm1_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Parm1_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fromdate1_lb":
                     FromDate1_Lb.Text = control.LABL_TEXT;
                     //FromDate1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FromDate1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "todate1_lb":
                     ToDate1_Lb.Text = control.LABL_TEXT;
                     //ToDate1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ToDate1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode1_lb":
                     RqtpCode1_Lb.Text = control.LABL_TEXT;
                     //RqtpCode1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno1_lb":
                     CochFileNo1_Lb.Text = control.LABL_TEXT;
                     //CochFileNo1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashier1_lb":
                     Cashier1_Lb.Text = control.LABL_TEXT;
                     //Cashier1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Cashier1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "find_butn":
                     Find_Butn.Text = control.LABL_TEXT;
                     //Find_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Find_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "back_butn":
                     Back_Butn.Text = control.LABL_TEXT;
                     //Back_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Back_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode1_clm":
                     RqtpCode1_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno1_clm":
                     FileNo1_Clm.Caption = control.LABL_TEXT;
                     //FileNo1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FileNo1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype1_clm":
                     SexType1_Clm.Caption = control.LABL_TEXT;
                     //SexType1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SexType1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnt1_clm":
                     Amnt1_Clm.Caption = control.LABL_TEXT;
                     //Amnt1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Amnt1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rcptmtod_clm":
                     RcptMtod_Clm.Caption = control.LABL_TEXT;
                     //RcptMtod_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RcptMtod_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "actndate1_clm":
                     ActnDate1_Clm.Caption = control.LABL_TEXT;
                     //ActnDate1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ActnDate1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby1_clm":
                     CretBy1_Clm.Caption = control.LABL_TEXT;
                     //CretBy1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretBy1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby1_clm":
                     MdfyBy1_Clm.Caption = control.LABL_TEXT;
                     //MdfyBy1_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdfyBy1_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "parm3_gb":
                     Parm3_Gb.Text = control.LABL_TEXT;
                     //Parm3_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Parm3_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fromdate3_lb":
                     FromDate3_Lb.Text = control.LABL_TEXT;
                     //FromDate3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FromDate3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode3_lb":
                     RqtpCode3_Lb.Text = control.LABL_TEXT;
                     //RqtpCode3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno3_lb":
                     CochFileNo3_Lb.Text = control.LABL_TEXT;
                     //CochFileNo3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cashier3_lb":
                     Cashier3_Lb.Text = control.LABL_TEXT;
                     //Cashier3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Cashier3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pyds004_cb":
                     Pyds004_Cb.Text = control.LABL_TEXT;
                     //Pyds004_Cb.Text = control.LABL_TEXT; // ToolTip
                     //Pyds004_Cb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "find3_butn":
                     Find3_Butn.Text = control.LABL_TEXT;
                     //Find3_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Find3_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "back3_butn":
                     Back3_Butn.Text = control.LABL_TEXT;
                     //Back3_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Back3_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode3_clm":
                     RqtpCode3_Clm.Caption = control.LABL_TEXT;
                     //RqtpCode3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RqtpCode3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype3_clm":
                     SexType3_Clm.Caption = control.LABL_TEXT;
                     //SexType3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SexType3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expncode3_clm":
                     ExpnCode3_Clm.Caption = control.LABL_TEXT;
                     //ExpnCode3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnCode3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnt3_clm":
                     Amnt3_Clm.Caption = control.LABL_TEXT;
                     //Amnt3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Amnt3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretdate3_clm":
                     CretDate3_Clm.Caption = control.LABL_TEXT;
                     //CretDate3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretDate3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "pydsdesc3_clm":
                     PydsDesc3_Clm.Caption = control.LABL_TEXT;
                     //PydsDesc3_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PydsDesc3_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "parm4_gb":
                     Parm4_Gb.Text = control.LABL_TEXT;
                     //Parm4_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Parm4_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fromdate4_lb":
                     FromDate4_Lb.Text = control.LABL_TEXT;
                     //FromDate4_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FromDate4_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "todate4_lb":
                     ToDate4_Lb.Text = control.LABL_TEXT;
                     //ToDate4_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ToDate4_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "find5_butn":
                     Find5_Butn.Text = control.LABL_TEXT;
                     //Find5_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Find5_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "back5_butn":
                     Back5_Butn.Text = control.LABL_TEXT;
                     //Back5_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Back5_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "clubcode4_clm":
                     ClubCode4_Clm.Caption = control.LABL_TEXT;
                     //ClubCode4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ClubCode4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "epitcode4_clm":
                     EpitCode4_Clm.Caption = control.LABL_TEXT;
                     //EpitCode4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EpitCode4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno4_clm":
                     CochFileNo4_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CochFileNo4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expnamnt4_clm":
                     ExpnAmnt4_Clm.Caption = control.LABL_TEXT;
                     //ExpnAmnt4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnAmnt4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "delvby4_clm":
                     DelvBy4_Clm.Caption = control.LABL_TEXT;
                     //DelvBy4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DelvBy4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "delvdate4_clm":
                     DelvDate4_Clm.Caption = control.LABL_TEXT;
                     //DelvDate4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DelvDate4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "expndesc4_clm":
                     ExpnDesc4_Clm.Caption = control.LABL_TEXT;
                     //ExpnDesc4_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ExpnDesc4_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  //case "parm5_gb":
                  //   Parm5_Gb.Text = control.LABL_TEXT;
                  //   //Parm5_Gb.Text = control.LABL_TEXT; // ToolTip
                  //   //Parm5_Gb.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "fromdate5_lb":
                  //   FromDate5_Lb.Text = control.LABL_TEXT;
                  //   //FromDate5_Lb.Text = control.LABL_TEXT; // ToolTip
                  //   //FromDate5_Lb.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "todate5_lb":
                  //   ToDate5_Lb.Text = control.LABL_TEXT;
                  //   //ToDate5_Lb.Text = control.LABL_TEXT; // ToolTip
                  //   //ToDate5_Lb.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "rqtpcode5_lb":
                  //   RqtpCode5_Lb.Text = control.LABL_TEXT;
                  //   //RqtpCode5_Lb.Text = control.LABL_TEXT; // ToolTip
                  //   //RqtpCode5_Lb.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "cochfileno5_lb":
                  //   CochFileNo5_Lb.Text = control.LABL_TEXT;
                  //   //CochFileNo5_Lb.Text = control.LABL_TEXT; // ToolTip
                  //   //CochFileNo5_Lb.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "cashier5_lb":
                  //   Cashier5_Lb.Text = control.LABL_TEXT;
                  //   //Cashier5_Lb.Text = control.LABL_TEXT; // ToolTip
                  //   //Cashier5_Lb.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "find4_butn":
                  //   Find4_Butn.Text = control.LABL_TEXT;
                  //   //Find4_Butn.Text = control.LABL_TEXT; // ToolTip
                  //   //Find4_Butn.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "back4_butn":
                  //   Back4_Butn.Text = control.LABL_TEXT;
                  //   //Back4_Butn.Text = control.LABL_TEXT; // ToolTip
                  //   //Back4_Butn.Text = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "rqtpcode5_clm":
                  //   RqtpCode5_Clm.Caption = control.LABL_TEXT;
                  //   //RqtpCode5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //RqtpCode5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "fileno5_clm":
                  //   FileNo5_Clm.Caption = control.LABL_TEXT;
                  //   //FileNo5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //FileNo5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "expndesc_clm":
                  //   ExpnDesc_Clm.Caption = control.LABL_TEXT;
                  //   //ExpnDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //ExpnDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "ownrfileno5_clm":
                  //   OwnrFileNo5_Clm.Caption = control.LABL_TEXT;
                  //   //OwnrFileNo5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //OwnrFileNo5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "tranby5_clm":
                  //   TranBy5_Clm.Caption = control.LABL_TEXT;
                  //   //TranBy5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //TranBy5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "trandate5_clm":
                  //   TranDate5_Clm.Caption = control.LABL_TEXT;
                  //   //TranDate5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //TranDate5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "issudate5_clm":
                  //   IssuDate5_Clm.Caption = control.LABL_TEXT;
                  //   //IssuDate5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //IssuDate5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "issutime5_clm":
                  //   IssuTime5_Clm.Caption = control.LABL_TEXT;
                  //   //IssuTime5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //IssuTime5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
                  //case "expnpric5_clm":
                  //   ExpnPric5_Clm.Caption = control.LABL_TEXT;
                  //   //ExpnPric5_Clm.Caption = control.LABL_TEXT; // ToolTip
                  //   //ExpnPric5_Clm.Caption = control.LABL_TEXT; // Place Holder
                  //   break;
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
                              "<Privilege>236</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                              {
                                 VuserBs1.DataSource = iScsc.V_Users;
                                 User_Lov.Properties.AllowMultiSelect = User_Lov2.Properties.AllowMultiSelect = User_Lov3.Properties.AllowMultiSelect = true;
                                 User_Lov.Properties.SelectAllItemVisible = User_Lov2.Properties.SelectAllItemVisible = User_Lov3.Properties.SelectAllItemVisible = true;
                                 User_Lov.Properties.ReadOnly = User_Lov2.Properties.ReadOnly = User_Lov3.Properties.ReadOnly = false;
                              }
                              else
                              {
                                 VuserBs1.DataSource = iScsc.V_Users.FirstOrDefault(u => u.USER_DB == CurrentUser);
                                 User_Lov.Properties.GetItems();
                                 User_Lov2.Properties.GetItems();
                                 User_Lov3.Properties.GetItems();

                                 User_Lov.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(u => u.CheckState = CheckState.Checked);
                                 User_Lov2.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(u => u.CheckState = CheckState.Checked);
                                 User_Lov3.Properties.Items.OfType<CheckedListBoxItem>().ToList().ForEach(u => u.CheckState = CheckState.Checked);
                                 User_Lov.Properties.AllowMultiSelect = User_Lov2.Properties.AllowMultiSelect = User_Lov3.Properties.AllowMultiSelect = true;
                                 User_Lov.Properties.SelectAllItemVisible = User_Lov2.Properties.SelectAllItemVisible = User_Lov3.Properties.SelectAllItemVisible = false;
                                 User_Lov.Properties.ReadOnly = User_Lov2.Properties.ReadOnly = User_Lov3.Properties.ReadOnly = true;
                              }
                           })
                        },
                        #endregion
                     }),
               });
         _DefaultGateway.Gateway(_InteractWithScsc);

         DrcmtBs1.DataSource = iScsc.D_RCMTs;
         //VuserBs1.DataSource = iScsc.V_Users;
         DysnoBs2.DataSource = iScsc.D_YSNOs;
         DsxtpBs2.DataSource = iScsc.D_SXTPs;
         DpydsBs2.DataSource = iScsc.D_PYDS;
         DDytpBs2.DataSource = iScsc.D_DYTPs;
         DCatpBs2.DataSource = iScsc.D_CATPs;
         DCktpBs2.DataSource = iScsc.D_CKTPs;
         DatypBs.DataSource = iScsc.D_ATYPs;
         DittpBs.DataSource = iScsc.D_ITTPs;
         DLotpBs.DataSource = iScsc.D_LOTPs;
         DRpacBs.DataSource = iScsc.D_RPACs;
         RqtpBs1.DataSource = iScsc.Request_Types.Where(rt => rt.CODE == "001" || rt.CODE == "009" || rt.CODE == "016" || rt.CODE == "012");
         CbmtBs.DataSource = iScsc.Club_Methods.Where(cb => cb.MTOD_STAT == "002");
         FighBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM == "003" /*&& f.FGPB_TYPE_DNRM != "007" && !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         SuntBs1.DataSource = iScsc.Sub_Units;
         DevntBs.DataSource = iScsc.D_EVNTs;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if(xinput != null)
         {
            if (xinput.Attribute("formname").Value != null)
            {
               formName = xinput.Attribute("formname").Value;
            }
            else
               formName = "RPT_PMT2_F";
            if(xinput.Attribute("fromdate").Value != null)
            {
               FromDate1_Date.Value = FromDate2_Date.Value = FromDate3_Date.Value = FromDate4_Date.Value = FromDate6_Date.Value = FromDate7_Date.Value = FromDate8_Date.Value = FromDate9_Date.Value = FromDate10_Date.Value = FromDate11_Date.Value = FromDate12_Date.Value = FromDate13_Date.Value = FromDate14_Date.Value = Convert.ToDateTime(xinput.Attribute("fromdate").Value);
            }
            if (xinput.Attribute("todate").Value != null)
            {
               ToDate1_Date.Value = ToDate2_Date.Value = ToDate3_Date.Value = ToDate4_Date.Value = ToDate6_Date.Value = ToDate7_Date.Value = ToDate8_Date.Value = ToDate9_Date.Value = ToDate10_Date.Value = ToDate11_Date.Value = ToDate12_Date.Value = ToDate13_Date.Value = ToDate14_Date.Value = Convert.ToDateTime(xinput.Attribute("todate").Value);
            }
            if(xinput.Attribute("useraccount").Value != null)
            {
               if(xinput.Attribute("useraccount").Value == "manager")
               {
                  Amnt3_Clm.Visible = true;
                  Amnt1_Clm.Visible = true;
                  ExpnPric2_Clm.Visible = true;
                  Qnty2_Clm.Visible = true;
                  TotlPric_Clm.Visible = true;
               }
               else if(xinput.Attribute("useraccount").Value == "user")
               {
                  Amnt3_Clm.Visible = false;
                  Amnt1_Clm.Visible = false;
                  ExpnPric2_Clm.Visible = false;
                  Qnty2_Clm.Visible = false;
                  TotlPric_Clm.Visible = false;
               }
            }
            if (xinput.Attribute("cochfileno") != null)
               Figh_Lov2.EditValue = cochfileno = Convert.ToInt64(xinput.Attribute("cochfileno").Value);
            else
               Figh_Lov2.EditValue = cochfileno = null;

            if (xinput.Attribute("cbmtcode") != null)
               Cbmt_Lov2.EditValue = cbmtcode = Convert.ToInt64(xinput.Attribute("cbmtcode").Value);
            else
               Cbmt_Lov2.EditValue = cbmtcode = null;
         }
         Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
