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

namespace System.Scsc.Ui.Settings
{
   partial class CNF_STNG_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
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
            //if (!(Btn_Search.Focused))
            //   SendKeys.Send("{TAB}");
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
                  case "attnmtod_gb":
                     AttnMtod_Gb.Text = control.LABL_TEXT;
                     //AttnMtod_Gb.Text = control.LABL_TEXT; // ToolTip
                     //AttnMtod_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_002":
                     tp_002.Text = control.LABL_TEXT;
                     //tp_002.Text = control.LABL_TEXT; // ToolTip
                     //tp_002.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sharekomod_lb":
                     ShareKomod_Lb.Text = control.LABL_TEXT;
                     //ShareKomod_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ShareKomod_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "moreattnstat_lb":
                     MoreAttnStat_Lb.Text = control.LABL_TEXT;
                     //MoreAttnStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MoreAttnStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtattnchekstat_lb":
                     DebtAttnChekStat_Lb.Text = control.LABL_TEXT;
                     //DebtAttnChekStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtAttnChekStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_gb":
                     FngrPrnt_Gb.Text = control.LABL_TEXT;
                     //FngrPrnt_Gb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnstat2_lb":
                     AttnStat2_Lb.Text = control.LABL_TEXT;
                     //AttnStat2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnStat2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnstat3_lb":
                     AttnStat3_Lb.Text = control.LABL_TEXT;
                     //AttnStat3_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnStat3_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gatestat_lb":
                     GateStat_Lb.Text = control.LABL_TEXT;
                     //GateStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GateStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_001":
                     tp_001.Text = control.LABL_TEXT;
                     //tp_001.Text = control.LABL_TEXT; // ToolTip
                     //tp_001.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "club_gb":
                     Club_Gb.Text = control.LABL_TEXT;
                     //Club_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Club_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "komod_gb":
                     Komod_Gb.Text = control.LABL_TEXT;
                     //Komod_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Komod_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnctrl_gb":
                     AttnCtrl_Gb.Text = control.LABL_TEXT;
                     //AttnCtrl_Gb.Text = control.LABL_TEXT; // ToolTip
                     //AttnCtrl_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "showattn_lb":
                     ShowAttn_Lb.Text = control.LABL_TEXT;
                     //ShowAttn_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ShowAttn_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tryvaldmbsp_lb":
                     TryValdMbsp_Lb.Text = control.LABL_TEXT;
                     //TryValdMbsp_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TryValdMbsp_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnprnt_lb":
                     AttnPrnt_Lb.Text = control.LABL_TEXT;
                     //AttnPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtchek_gb":
                     DebtChek_Gb.Text = control.LABL_TEXT;
                     //DebtChek_Gb.Text = control.LABL_TEXT; // ToolTip
                     //DebtChek_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mostdebtamnt_lb":
                     MostDebtAmnt_Lb.Text = control.LABL_TEXT;
                     //MostDebtAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MostDebtAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "debtdayexp_lb":
                     DebtDayExp_Lb.Text = control.LABL_TEXT;
                     //DebtDayExp_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DebtDayExp_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cardreader_gb":
                     CardReader_Gb.Text = control.LABL_TEXT;
                     //CardReader_Gb.Text = control.LABL_TEXT; // ToolTip
                     //CardReader_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "comportname1_lb":
                     ComPortName1_Lb.Text = control.LABL_TEXT;
                     //ComPortName1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ComPortName1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bandrate1_lb":
                     BandRate1_Lb.Text = control.LABL_TEXT;
                     //BandRate1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BandRate1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnstat1_lb":
                     AttnStat1_Lb.Text = control.LABL_TEXT;
                     //AttnStat1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AttnStat1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "barcodedatatype_lb":
                     BarCodeDataType_Lb.Text = control.LABL_TEXT;
                     //BarCodeDataType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BarCodeDataType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "computer2_gb":
                     Computer2_Gb.Text = control.LABL_TEXT;
                     //Computer2_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Computer2_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "portnum1_lb":
                     PortNum1_Lb.Text = control.LABL_TEXT;
                     //PortNum1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PortNum1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "compname1_lb":
                     CompName1_Lb.Text = control.LABL_TEXT;
                     //CompName1_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CompName1_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "computer3_gb":
                     Computer3_Gb.Text = control.LABL_TEXT;
                     //Computer3_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Computer3_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "comportnum2_lb":
                     ComPortNum2_Lb.Text = control.LABL_TEXT;
                     //ComPortNum2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ComPortNum2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "compname2_lb":
                     CompName2_Lb.Text = control.LABL_TEXT;
                     //CompName2_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CompName2_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gatectrl_gb":
                     GateCtrl_Gb.Text = control.LABL_TEXT;
                     //GateCtrl_Gb.Text = control.LABL_TEXT; // ToolTip
                     //GateCtrl_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "portlist_lb":
                     PortList_Lb.Text = control.LABL_TEXT;
                     //PortList_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PortList_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "outport_lb":
                     OutPort_Lb.Text = control.LABL_TEXT;
                     //OutPort_Lb.Text = control.LABL_TEXT; // ToolTip
                     //OutPort_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gatebandrate_lb":
                     GateBandRate_Lb.Text = control.LABL_TEXT;
                     //GateBandRate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GateBandRate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "loaddatasserver_gb":
                     LoadDatasServer_Gb.Text = control.LABL_TEXT;
                     //LoadDatasServer_Gb.Text = control.LABL_TEXT; // ToolTip
                     //LoadDatasServer_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "loaddatastat_lb":
                     LoadDataStat_Lb.Text = control.LABL_TEXT;
                     //LoadDataStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LoadDataStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sharembsp_gb":
                     ShareMbsp_Gb.Text = control.LABL_TEXT;
                     //ShareMbsp_Gb.Text = control.LABL_TEXT; // ToolTip
                     //ShareMbsp_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sharembspstat_lb":
                     ShareMbspStat_Lb.Text = control.LABL_TEXT;
                     //ShareMbspStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ShareMbspStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tlgr_gb":
                     Tlgr_Gb.Text = control.LABL_TEXT;
                     //Tlgr_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Tlgr_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tlgrstat_lb":
                     TlgrStat_Lb.Text = control.LABL_TEXT;
                     //TlgrStat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TlgrStat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "option_gb":
                     Option_Gb.Text = control.LABL_TEXT;
                     //Option_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Option_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "removezero_lb":
                     RemoveZero_Lb.Text = control.LABL_TEXT;
                     //RemoveZero_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RemoveZero_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "holyday_lb":
                     HolyDay_Lb.Text = control.LABL_TEXT;
                     //HolyDay_Lb.Text = control.LABL_TEXT; // ToolTip
                     //HolyDay_Lb.Text = control.LABL_TEXT; // Place Holder
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
         Job _CheckSecurity =
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
                              "<Privilege>134</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 134 سطوح امینتی");
                           })
                        },
                        #endregion
                     }),
               });
         _DefaultGateway.Gateway(_CheckSecurity);
         job.Status = _CheckSecurity.Status;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         LoadDataSource(new Job(SendType.Self, 08));
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {         
         DysnoBs1.DataSource = iScsc.D_YSNOs;
         DbcdtBs4.DataSource = iScsc.D_BCDTs;
         DAeatBs.DataSource = iScsc.D_AEATs;
         vCompBs1.DataSource = iScsc.V_Computers;
         DatsmBs1.DataSource = iScsc.D_ATSMs;
         DActvBs1.DataSource = iScsc.D_ACTVs;
         DCstpBs1.DataSource = iScsc.D_CSTPs;         
         DIttpBs1.DataSource = iScsc.D_ITTPs;

         AttnComPortName_Lov.Items.Clear();
         GateComPortName_Lov.Items.Clear();
         ExpnComPortName_Lov.Items.Clear();

         AttnComPortName_Lov.Items.AddRange(SerialPort.GetPortNames());
         GateComPortName_Lov.Items.AddRange(SerialPort.GetPortNames());
         ExpnComPortName_Lov.Items.AddRange(SerialPort.GetPortNames());

         RqtpBs1.DataSource = iScsc.Request_Types;
         RqttBs1.DataSource = iScsc.Requester_Types;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         tc_master.TabPages.Clear();
         var xinput = job.Input as XElement;
         switch (xinput.Attribute("type").Value)
         {
            case "tp_001":
               tc_master.TabPages.Add(tp_001);
               tc_master.SelectedTab = tp_001;
               break;
            case "tp_002":
               tc_master.TabPages.Add(tp_002);
               tc_master.SelectedTab = tp_002;
               break;
         }
         Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
