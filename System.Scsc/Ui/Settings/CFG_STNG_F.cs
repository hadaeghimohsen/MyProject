using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.IO;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.Settings
{
   public partial class CFG_STNG_F : UserControl
   {
      public CFG_STNG_F()
      {
         InitializeComponent();
      }

      bool requery = false;
      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            //if (tc_Settings.SelectedTab == tp_base)
            //{
            //   var prvncode = PrvnCode_LookupEdit.EditValue;
            //   var regncode = RegnCode_LookUpEdit.EditValue;
            //   PrvnBs1.DataSource = iScsc.Provinces;
            //   PrvnCode_LookupEdit.EditValue = prvncode;
            //   RegnCode_LookUpEdit.EditValue = regncode;

            //   Fga_URegn_Bs1.DataSource = iScsc.User_Region_Fgacs.Where(a => /*Fga_Urgn_U.Split(',').Contains(a.REGN_PRVN_CODE + a.REGN_CODE)*/ /*&& a.SYS_USER == Crnt_User*/ a.REC_STAT == "002" && a.VALD_TYPE == "002");
            //   Fga_URqrq_Bs1.DataSource = iScsc.User_Request_Requester_Fgacs.Where(a => a.REC_STAT == "002" && a.VALD_TYPE == "002");

            //   // Cash Rollout
            //   Execute_Cash_Query();
            //   // Expense Item Rollout
            //   Execute_Epit_Query();
            //   // Country / Province / Region / Club / Club_Method
            //   Execute_CPR2C2_Query();
            //}
            if (tc_Settings.SelectedTab == tp_backuprestore)
            {
               //var _0 = ClubStng_Lookup.ItemIndex;
               ClubBs1.DataSource = iScsc.Clubs.Where(c => Fga_Uclb_U.Contains(c.CODE));
               //ClubStng_Lookup.ItemIndex = _0;

               Execute_ClubShare_Query();
            }
            else if (tc_Settings.SelectedTab == tp_printmodual)
            {
               Execute_ModualReport_Query();
            }
            //else if (tc_Settings.SelectedTab == tp_004)
            //{
            //   //var _0 = UserClub_Lookup.EditValue;
            //   ClubBs1.DataSource = iScsc.Clubs.Where(c => Fga_Uclb_U.Contains(c.CODE));
            //   //if(_0 != null)
            //     // UserClub_Lookup.EditValue = _0;

            //   Execute_ClubShare_Query();
            //}
         }
         catch { }
      }
      
      //private void Execute_CPR2C2_Query()
      //{
      //   var _0 = CntyBs1.Position;
      //   var _1 = Prvn01Bs1.Position;
      //   var _2 = Regn01Bs1.Position;
      //   var _3 = Club01Bs1.Position;
      //   var _4 = CbmtBs1.Position;
      //   iScsc = new Data.iScscDataContext(ConnectionString);
      //   CntyBs1.DataSource = iScsc.Countries;
      //   CntyBs1.Position = _0;
      //   Prvn01Bs1.Position = _1;
      //   Regn01Bs1.Position = _2;
      //   Club01Bs1.Position = _3;
      //   CbmtBs1.Position = _4;
      //   //
      //   //CbmtBs1.AddNew();
      //}

      //private void Execute_Epit_Query()
      //{
      //   iScsc = new Data.iScscDataContext(ConnectionString);
      //   var _0 = EpitBs1.Position;
      //   EpitBs1.DataSource = iScsc.Expense_Items;
      //   EpitBs1.Position = _0;
      //   //
      //   //EpitBs1.AddNew();
      //}

      //private void Execute_Cash_Query()
      //{
      //   iScsc = new Data.iScscDataContext(ConnectionString);
      //   var _0 = CashBs1.Position;
      //   CashBs1.DataSource = iScsc.Cashes;
      //   CashBs1.Position = _0;
      //   //
      //   //CashBs1.AddNew();
      //}

      private void Execute_ModualReport_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         var _0 = MdrpBs1.Position;
         MdrpBs1.DataSource = iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == Modul_Name && mr.SECT_NAME == Section_Name);
         MdrpBs1.Position = _0;
         //
         //MdrpBs1.AddNew();
      }

      //private void Execute_URQRQ_Query()
      //{
      //   iScsc = new Data.iScscDataContext(ConnectionString);
      //   var _0 = Fga_URqrq_Bs1.Position;
      //   Fga_URqrq_Bs1.DataSource = iScsc.User_Request_Requester_Fgacs.Where(a => a.REC_STAT == "002" && a.VALD_TYPE == "002");
      //   Fga_URqrq_Bs1.Position = _0;
      //}

      private void Execute_ClubShare_Query()
      {
          try
          {
              if (tc_Settings.SelectedTab == tp_backuprestore)
              {
                  if (ClubStng_Lookup.ItemIndex == -1) return;

                  StngBs1.DataSource = iScsc.Settings.Where(c => c.CLUB_CODE == (long?)ClubStng_Lookup.EditValue);
                  var Stng = StngBs1.Current as Data.Setting;
                  if (Stng == null)
                  {
                      Stng = new Data.Setting() { BACK_UP = true, BACK_UP_APP_EXIT = true, BACK_UP_IN_TRED = true, BACK_UP_OPTN_PATH = true, BACK_UP_OPTN_PATH_ADRS = @"D:\iData\Asre Andishe Project\Database Backup", BACK_UP_ROOT_PATH = @"C:\Backup", CLUB_CODE = (long)ClubStng_Lookup.EditValue, DRES_STAT = "002", DRES_AUTO = "002" };
                  }
                  RestBs1.DataSource = new DirectoryInfo(Stng.BACK_UP_OPTN_PATH == true ? Stng.BACK_UP_OPTN_PATH_ADRS + @"\Backup" : Stng.BACK_UP_ROOT_PATH).GetFiles("*.bak");
              }
              //else if (tc_Settings.SelectedTab == tp_004)
              //{
              //    if (UserClub_Lookup.ItemIndex == -1) return;

              //    StngBs1.DataSource = iScsc.Settings.Where(s => s.CLUB_CODE == (long?)UserClub_Lookup.EditValue);
              //    DresBs1.DataSource = iScsc.Dressers.Where(d => d.CLUB_CODE == (long?)UserClub_Lookup.EditValue);
              //}
          }
          catch { }
      }

      private void Ckb_BACK_UP_StatusChange(object sender)
      {
         GB_BACKUP.Enabled = Ckb_BACK_UP.Status;
      }

      private void Ckb_BACK_UP_OPTN_PATH_StatusChange(object sender)
      {
         GB_BACK_UP_OPTN_PATH_ADRS.Enabled = Ckb_BACK_UP_OPTN_PATH.Status;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_APPLY_Click(object sender, EventArgs e)
      {
         try
         {
            Data.Setting Stng = null;
            if(tc_Settings.SelectedTab == tp_backuprestore)
               Stng = StngBs1.List.OfType<Data.Setting>().Where(s => s.CLUB_CODE == (long?)ClubStng_Lookup.EditValue).FirstOrDefault();
            //else if (tc_Settings.SelectedTab == tp_004)
            //   Stng = StngBs1.List.OfType<Data.Setting>().Where(s => s.CLUB_CODE == (long?)UserClub_Lookup.EditValue).FirstOrDefault(); 
            
            if (Stng == null)
            {
               long clubcode = 0;
               if (tc_Settings.SelectedTab == tp_backuprestore)
                  clubcode = (long)ClubStng_Lookup.EditValue;
               //else if (tc_Settings.SelectedTab == tp_004)
               //   clubcode = (long)UserClub_Lookup.EditValue;
               Stng = new Data.Setting { BACK_UP = true, BACK_UP_APP_EXIT = true, BACK_UP_IN_TRED = true, BACK_UP_OPTN_PATH = true, BACK_UP_OPTN_PATH_ADRS = @"D:\iData\Asre Andishe Project\Database Backup", BACK_UP_ROOT_PATH = @"C:\Backup", CLUB_CODE = clubcode, DRES_STAT = "002", DRES_AUTO = "002", MORE_ATTN_SESN = "002", NOTF_STAT = "002", NOTF_EXP_DAY = 3, ATTN_SYST_TYPE = "001", COMM_PORT_NAME = "COM1", BAND_RATE = 9600, BAR_CODE_DATA_TYPE = "001", DFLT_STAT = "001", ATTN_NOTF_STAT = "001", ATTN_NOTF_CLOS_TYPE = "002", ATTN_NOTF_CLOS_INTR = 1000, ATTN_COMP_CONCT = "", DEBT_CLNG_STAT = "001", MOST_DEBT_CLNG_AMNT = 0, IP_ADDR = "", PORT_NUMB = 0, NOTF_VIST_DATE = DateTime.Now, EXPR_DEBT_DAY = 7, TRY_VALD_SBMT = "002"};
            }            

            if(tc_Settings.SelectedTab == tp_backuprestore)
               iScsc.STNG_SAVE_P(
                  new XElement("Request",
                     new XElement("Settings",
                        new XAttribute("clubcode", ClubStng_Lookup.EditValue),
                        new XAttribute("dfltstat", Stng.DFLT_STAT ?? "001"),
                        new XAttribute("backup", Stng.BACK_UP ?? false),
                        new XAttribute("backupappexit", Stng.BACK_UP_APP_EXIT ?? false),
                        new XAttribute("backupintred", Stng.BACK_UP_IN_TRED ?? false),
                        new XAttribute("backupoptnpath", Stng.BACK_UP_OPTN_PATH ?? false),
                        new XAttribute("backupoptnpathadrs", Stng.BACK_UP_OPTN_PATH_ADRS ?? ""),
                        new XAttribute("backuprootpath", Stng.BACK_UP_ROOT_PATH ?? ""),
                        new XAttribute("dresstat", Stng.DRES_STAT ?? "002"),
                        new XAttribute("dresauto", Stng.DRES_AUTO ?? "001"),
                        new XAttribute("morefighonedres", Stng.MORE_FIGH_ONE_DRES ?? "001"),
                        new XAttribute("moreattnsesn", Stng.MORE_ATTN_SESN ?? "002"),
                        new XAttribute("notfstat", Stng.NOTF_STAT ?? "001"),
                        new XAttribute("notfexpday", Stng.NOTF_EXP_DAY ?? 3),
                        new XAttribute("attnsysttype", Stng.ATTN_SYST_TYPE ?? "000"),
                        new XAttribute("commportname", /*AttnComPortName_Lov.Text*/Stng.COMM_PORT_NAME ?? ""),
                        new XAttribute("bandrate", Stng.BAND_RATE ?? 0),
                        new XAttribute("barcodedatatype", Stng.BAR_CODE_DATA_TYPE ?? "000"),
                        new XAttribute("atn3evntactntype", Stng.ATN3_EVNT_ACTN_TYPE ?? "001"),

                        new XAttribute("ipaddr", Stng.IP_ADDR ?? ""),
                        new XAttribute("portnumb", Stng.PORT_NUMB ?? 0),
                        new XAttribute("attncompconct", Stng.ATTN_COMP_CONCT ?? ""),
                        new XAttribute("atn1evntactntype", Stng.ATN1_EVNT_ACTN_TYPE ?? "001"),

                        new XAttribute("ipadr2", Stng.IP_ADR2 ?? ""),
                        new XAttribute("portnum2", Stng.PORT_NUM2 ?? 0),
                        new XAttribute("attncompcnc2", Stng.ATTN_COMP_CNC2 ?? ""),
                        new XAttribute("atn2evntactntype", Stng.ATN2_EVNT_ACTN_TYPE ?? "001"),

                        new XAttribute("attnnotfstat", Stng.ATTN_NOTF_STAT ?? "002"),
                        new XAttribute("attnnotfclostype", Stng.ATTN_NOTF_CLOS_TYPE ?? ""),
                        new XAttribute("attnnotfclosintr", Stng.ATTN_NOTF_CLOS_INTR ?? 0),
                        new XAttribute("debtclngstat", Stng.DEBT_CLNG_STAT ?? "001"),
                        new XAttribute("mostdebtclngamnt", Stng.MOST_DEBT_CLNG_AMNT ?? 0),
                        new XAttribute("exprdebtday", Stng.EXPR_DEBT_DAY ?? 7),
                        new XAttribute("tryvaldsbmt", Stng.TRY_VALD_SBMT ?? "002"),
                        new XAttribute("debtchckstat", Stng.DEBT_CHCK_STAT ?? "002"),

                        new XAttribute("gateattnstat", Stng.GATE_ATTN_STAT ?? "001"),
                        new XAttribute("gatecommportname", /*GateComPortName_Lov.Text*/Stng.GATE_COMM_PORT_NAME ?? ""),
                        new XAttribute("gatebandrate", Stng.GATE_BAND_RATE ?? 9600),
                        new XAttribute("gatetimeclos", Stng.GATE_TIME_CLOS ?? 5),
                        new XAttribute("gateentropen", Stng.GATE_ENTR_OPEN ?? "002"),
                        new XAttribute("gateexitopen", Stng.GATE_EXIT_OPEN ?? "002"),

                        new XAttribute("expnextrstat", Stng.EXPN_EXTR_STAT ?? "001"),
                        new XAttribute("expncommportname", /*GateComPortName_Lov.Text*/Stng.EXPN_COMM_PORT_NAME ?? ""),
                        new XAttribute("expnbandrate", Stng.EXPN_BAND_RATE ?? 9600),

                        new XAttribute("runqury", Stng.RUN_QURY ?? "001"),
                        new XAttribute("attnprntstat", Stng.ATTN_PRNT_STAT ?? "001"),
                        new XAttribute("sharmbspstat", Stng.SHAR_MBSP_STAT ?? "001"),
                        new XAttribute("runrbot", Stng.RUN_RBOT ?? "001"),
                        new XAttribute("clerzero", Stng.CLER_ZERO ?? "001"),
                        new XAttribute("hldycont", Stng.HLDY_CONT ?? 1)
                     )
                  )
               );
            //else if(tc_Settings.SelectedTab == tp_004)
            //   iScsc.STNG_SAVE_P(
            //      new XElement("Request",
            //         new XElement("Settings",
            //            new XAttribute("clubcode", UserClub_Lookup.EditValue),
            //            new XAttribute("dfltstat", Stng.DFLT_STAT ?? "001"),
            //            new XAttribute("backup", Stng.BACK_UP ?? false),
            //            new XAttribute("backupappexit", Stng.BACK_UP_APP_EXIT ?? false),
            //            new XAttribute("backupintred", Stng.BACK_UP_IN_TRED ?? false),
            //            new XAttribute("backupoptnpath", Stng.BACK_UP_OPTN_PATH ?? false),
            //            new XAttribute("backupoptnpathadrs", Stng.BACK_UP_OPTN_PATH_ADRS ?? ""),
            //            new XAttribute("backuprootpath", Stng.BACK_UP_ROOT_PATH ?? ""),
            //            new XAttribute("dresstat", Stng.DRES_STAT ?? "002"),
            //            new XAttribute("dresauto", Stng.DRES_AUTO ?? "001"),
            //            new XAttribute("morefighonedres", Stng.MORE_FIGH_ONE_DRES ?? "001"),
            //            new XAttribute("moreattnsesn", Stng.MORE_ATTN_SESN ?? "002"),
            //            new XAttribute("notfstat", Stng.NOTF_STAT ?? "001"),
            //            new XAttribute("notfexpday", Stng.NOTF_EXP_DAY ?? 3),
            //            new XAttribute("attnsysttype", Stng.ATTN_SYST_TYPE ?? "000"),
            //            new XAttribute("commportname", /*AttnComPortName_Lov.Text*/Stng.COMM_PORT_NAME ?? ""),
            //            new XAttribute("bandrate", Stng.BAND_RATE ?? 0),
            //            new XAttribute("barcodedatatype", Stng.BAR_CODE_DATA_TYPE ?? "000"),
            //            new XAttribute("atn3evntactntype", Stng.ATN3_EVNT_ACTN_TYPE ?? "001"),

            //            new XAttribute("ipaddr", Stng.IP_ADDR ?? ""),
            //            new XAttribute("portnumb", Stng.PORT_NUMB ?? 0),
            //            new XAttribute("attncompconct", Stng.ATTN_COMP_CONCT ?? ""),
            //            new XAttribute("atn1evntactntype", Stng.ATN1_EVNT_ACTN_TYPE ?? "001"),

            //            new XAttribute("ipadr2", Stng.IP_ADR2 ?? ""),
            //            new XAttribute("portnum2", Stng.PORT_NUM2 ?? 0),
            //            new XAttribute("attncompcnc2", Stng.ATTN_COMP_CNC2 ?? ""),
            //            new XAttribute("atn2evntactntype", Stng.ATN2_EVNT_ACTN_TYPE ?? "001"),

            //            new XAttribute("attnnotfstat", Stng.ATTN_NOTF_STAT ?? "002"),
            //            new XAttribute("attnnotfclostype", Stng.ATTN_NOTF_CLOS_TYPE ?? ""),
            //            new XAttribute("attnnotfclosintr", Stng.ATTN_NOTF_CLOS_INTR ?? 0),
            //            new XAttribute("debtclngstat", Stng.DEBT_CLNG_STAT ?? "001"),
            //            new XAttribute("mostdebtclngamnt", Stng.MOST_DEBT_CLNG_AMNT ?? 0),
            //            new XAttribute("exprdebtday", Stng.EXPR_DEBT_DAY ?? 7),
            //            new XAttribute("tryvaldsbmt", Stng.TRY_VALD_SBMT ?? "002"),
            //            new XAttribute("debtchckstat", Stng.DEBT_CHCK_STAT ?? "002"),

            //            new XAttribute("gateattnstat", Stng.GATE_ATTN_STAT ?? "001"),
            //            new XAttribute("gatecommportname", /*GateComPortName_Lov.Text*/Stng.GATE_COMM_PORT_NAME ?? ""),
            //            new XAttribute("gatebandrate", Stng.GATE_BAND_RATE ?? 9600),
            //            new XAttribute("gatetimeclos", Stng.GATE_TIME_CLOS ?? 5),
            //            new XAttribute("gateentropen", Stng.GATE_ENTR_OPEN ?? "002"),
            //            new XAttribute("gateexitopen", Stng.GATE_EXIT_OPEN ?? "002"),

            //            new XAttribute("expnextrstat", Stng.EXPN_EXTR_STAT ?? "001"),
            //            new XAttribute("expncommportname", /*GateComPortName_Lov.Text*/Stng.EXPN_COMM_PORT_NAME ?? ""),
            //            new XAttribute("expnbandrate", Stng.EXPN_BAND_RATE ?? 9600)
            //         )
            //      )
            //   );
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
      }

      private void Btn_OK_Click(object sender, EventArgs e)
      {
         Btn_APPLY_Click(sender, e);
         Btn_Back_Click(sender, e);
      }

      string BackupType = "NORMAL";
      private void Btn_TakeBackup_Click(object sender, EventArgs e)
      {
         try
         {
            Btn_APPLY_Click(null, null);
            if (ClubStng_Lookup.ItemIndex == -1) return;

            iScsc.TAK_BKUP_P(new XElement("Request", new XElement("Backup", new XAttribute("type", BackupType), new XAttribute("clubcode", ClubStng_Lookup.EditValue))));
            Execute_Query();
            if (BackupType != "RESTORE")
               MessageBox.Show("عملیات پشتیبانی با موفقیت انجام شد");
            BackupType = "NORMAL";
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_QueryBackupFile_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void Btn_BackupOptnPathAdrs_Click(object sender, EventArgs e)
      {
         if (Ofd_BackupOptnPathAdrs.ShowDialog() != DialogResult.OK) return;

         Txt_BACK_UP_OPTN_PATH_ADRS.Text = Ofd_BackupOptnPathAdrs.SelectedPath;
      }

      private void Btn_RestoreDb_Click(object sender, EventArgs e)
      {
         try
         {
            if (RestBs1.Count == 0) return;
            var Rstrdb = RestBs1.Current as FileInfo;
            var BackupFile = Rstrdb.Directory + @"\" + Rstrdb.Name;
            if (MessageBox.Show(this, "آیا با بازگردانی اطلاعات پایگاه داده موافق هستید؟", "بازگردانی داده", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes) return;

            BackupType = "RESTORE";
            Btn_TakeBackup_Click(null, null);

            var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>master</Database><Dbms>SqlServer</Dbms>" };
            _DefaultGateway.Gateway(
               GetConnectionString
            );
            var MasterConnectionString = GetConnectionString.Output.ToString();

            iScsc = new Data.iScscDataContext(MasterConnectionString);

            var GetSqlServerActiveDbName =
               new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServerActiveDbName</Dbms>" };
            _DefaultGateway.Gateway(GetSqlServerActiveDbName);

            //iScsc.ExecuteCommand(string.Format(@"ALTER DATABASE [iScsc] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [iScsc] FROM  DISK = N'{0}' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 2; ALTER DATABASE [iScsc] SET MULTI_USER;", BackupFile));
            iScsc.ExecuteCommand(string.Format(@"ALTER DATABASE [{1}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [{1}] FROM  DISK = N'{0}' WITH  FILE = 1,  NOUNLOAD,  REPLACE,  STATS = 2; ALTER DATABASE [{1}] SET MULTI_USER;", BackupFile, GetSqlServerActiveDbName.Output));
            Execute_Query();
            MessageBox.Show("عملیات بازگردانی پابگاه داده با موفقیت انجام شد");
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_DeleteBackup_Click(object sender, EventArgs e)
      {
         if (RestBs1.Current == null) return;
         if (MessageBox.Show(this, "آیا با پاک کردن فایل پشتیبان موافق هستید؟", "بازگردانی داده", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes) return;
         var Rstrdb = RestBs1.Current as FileInfo;
         var BackupFile = Rstrdb.Directory + @"\" + Rstrdb.Name;
         FileSystem.Kill(BackupFile);

         Execute_Query();
      }

      int itemindex;
      //private void PrvnCode_LookupEdit_EditValueChanged(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      itemindex = PrvnCode_LookupEdit.ItemIndex;            
      //      RegnBs1.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == PrvnCode_LookupEdit.EditValue.ToString());
      //   }
      //   catch { }
      //}

      //private void PrvnCode_LookupEdit_Leave(object sender, EventArgs e)
      //{
      //}

      //private void Fga_URegn_Bs1_CurrentChanged(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var regn = Fga_URegn_Bs1.Current as Data.User_Region_Fgac;
      //      ClubBs1.DataSource = iScsc.Clubs.Where(c => c.REGN_PRVN_CODE == regn.REGN_PRVN_CODE && c.REGN_CODE == regn.REGN_CODE);
      //      Fga_UClub_Bs1.DataSource = iScsc.User_Club_Fgacs.Where(a => /*Fga_Uclb_U.Contains(a.CLUB_CODE)*/ /*&& a.SYS_USER == Crnt_User*/ a.REC_STAT == "002" && a.VALD_TYPE == "002" && a.SYS_USER == regn.SYS_USER);
      //   }
      //   catch { }
      //}

      //private void Btn_AddFgaURegn_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if (!(PrvnCode_LookupEdit.EditValue != null && RegnCode_LookUpEdit.EditValue != null && User_LookupEdit.EditValue != null)) return;

      //      iScsc.STNG_SAVE_P(
      //         new XElement("Config",
      //            new XAttribute("type", "001"),
      //            new XElement("FgaURegn",
      //               new XAttribute("prvncode", PrvnCode_LookupEdit.EditValue),
      //               new XAttribute("regncode", RegnCode_LookUpEdit.EditValue),
      //               new XAttribute("sysuser", User_LookupEdit.EditValue)
      //            )
      //         )
      //      );
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_Query();               
      //         requery = false;
      //      }
      //   }
      //}

      //private void Btn_DelFgaURegn_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if (MessageBox.Show(this, "آیا می خواهید کاربر را از ناحیه خارج کنید؟", "حذف کاربر", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes) return;

      //      var Fga_URegn = Fga_URegn_Bs1.Current as Data.User_Region_Fgac;
      //      if (Fga_URegn == null) return;

      //      iScsc.STNG_SAVE_P(
      //         new XElement("Config",
      //            new XAttribute("type", "002"),
      //            new XElement("FgaURegn",
      //               new XAttribute("prvncode", Fga_URegn.REGN_PRVN_CODE),
      //               new XAttribute("regncode", Fga_URegn.REGN_CODE),
      //               new XAttribute("sysuser", Fga_URegn.SYS_USER)
      //            )
      //         )
      //      );
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void Btn_AddFgaUClub_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      var Fga_URegn = Fga_URegn_Bs1.Current as Data.User_Region_Fgac;
      //      if (Fga_URegn == null) return;
      //      if (ClubCode_LookupEdit.EditValue == null) return;

      //      iScsc.STNG_SAVE_P(
      //         new XElement("Config",
      //            new XAttribute("type", "003"),
      //            new XElement("FgaUClub",
      //               new XAttribute("sysuser", Fga_URegn.SYS_USER),
      //               new XAttribute("mstrsysuser", MstrUser_LookupEdit.EditValue ?? ""),
      //               new XAttribute("clubcode", ClubCode_LookupEdit.EditValue)
      //            )
      //         )
      //      );
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void Del_FgaUClub_Click(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if (MessageBox.Show(this, "آیا می خواهید کاربر را از باشگاه خارج کنید؟", "حذف کاربر", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button3) != DialogResult.Yes) return;
      //      var Fga_Club = Fga_UClub_Bs1.Current as Data.User_Club_Fgac;

      //      iScsc.STNG_SAVE_P(
      //         new XElement("Config",
      //            new XAttribute("type", "004"),
      //            new XElement("FgaUClub",
      //               new XAttribute("sysuser", Fga_Club.SYS_USER),
      //               new XAttribute("mstrsysuser", Fga_Club.MAST_SYS_USER),
      //               new XAttribute("clubcode", Fga_Club.CLUB_CODE)
      //            )
      //         )
      //      );
      //      requery = true;
      //   }
      //   catch (Exception ex)
      //   {
      //      MessageBox.Show(ex.Message);
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //#region Mc_Bas
      //private void tsm_bas_close_Click(object sender, EventArgs e)
      //{
      //   foreach (System.MaxUi.Rollout03 R in flp_bas.Controls)
      //   {
      //      R.Status = false;
      //   }
      //}

      //private void tsm_bas_open_Click(object sender, EventArgs e)
      //{
      //   foreach (System.MaxUi.Rollout03 R in flp_bas.Controls)
      //   {
      //      R.Status = true;
      //   }
      //}

      //private void tsm_bas_R1_Click(object sender, EventArgs e)
      //{
      //   ro_1.Status = true;
      //}

      //private void tsm_bas_R2_Click(object sender, EventArgs e)
      //{
      //   ro_2.Status = true;
      //}

      //private void tsm_bas_R3_Click(object sender, EventArgs e)
      //{
      //   ro_3.Status = true;
      //}

      //private void tsm_bas_R4_Click(object sender, EventArgs e)
      //{
      //   ro_4.Status = true;
      //}
      //#endregion

      //private void CbmtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن ساعت کلاسی موافقید؟", "حذف ساعت کلاسی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "005"),
      //                  new XElement("Delete",
      //                     new XElement("Club_Method",
      //                        new XAttribute("code", (CbmtBs1.Current as Data.Club_Method).CODE)
      //                     )
      //                  )
      //               )
      //            );                  
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            CbmtBs1.EndEdit();
      //            var crnt = CbmtBs1.Current as Data.Club_Method;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "005"),
      //                  CbmtBs1.List.OfType<Data.Club_Method>().Where(c => c.CODE == 0).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Club_Method",
      //                           new XAttribute("clubcode", c.CLUB_CODE),
      //                           new XAttribute("mtodcode", c.MTOD_CODE),
      //                           new XAttribute("cochfileno", c.COCH_FILE_NO),
      //                           new XAttribute("daytype", c.DAY_TYPE),
      //                           new XAttribute("strttime", c.STRT_TIME.ToString()),
      //                           new XAttribute("endtime", c.END_TIME.ToString()),
      //                           new XAttribute("mtodstat", c.MTOD_STAT  ?? "002")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CODE != 0 ? 
      //                     new XElement("Update",
      //                        new XElement("Club_Method",
      //                           new XAttribute("code", crnt.CODE),
      //                           new XAttribute("clubcode", crnt.CLUB_CODE),
      //                           new XAttribute("mtodcode", crnt.MTOD_CODE),
      //                           new XAttribute("cochfileno", crnt.COCH_FILE_NO),
      //                           new XAttribute("daytype", crnt.DAY_TYPE),
      //                           new XAttribute("strttime", crnt.STRT_TIME.ToString()),
      //                           new XAttribute("endtime", crnt.END_TIME.ToString()),
      //                           new XAttribute("mtodstat", crnt.MTOD_STAT)
      //                        )
      //                    ) : new XElement("Update")
                       
      //               )
      //            );
      //            requery = true;
      //            break;
      //      }            
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         case 547:
      //            MessageBox.Show("زمان پایان کلاس باید از زمان شروع بزرگتر باشید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {               
      //         Execute_CPR2C2_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void COCH_FILE_NOLookUpEdit_EditValueChanged(object sender, EventArgs e)
      //{
      //   if (!pb_NoCheckMethod.PickChecked) return;
      //   if (CochBs1.List.Count == 0) return;
      //   if (MtodBs1.List.Count == 0) return;
      //   if (COCH_FILE_NOLookUpEdit.EditValue == null || COCH_FILE_NOLookUpEdit.EditValue.ToString() == "") return;

      //   var crntcoch = CochBs1.List.OfType<Data.Fighter>().Where(f => f.FILE_NO == (long)COCH_FILE_NOLookUpEdit.EditValue).Single();
      //   var crntmtod = MTOD_CODELookUpEdit.EditValue;
      //   if (crntmtod.ToString() == "" || crntcoch == null) return;
      //   if (crntcoch.MTOD_CODE_DNRM != (long)crntmtod)
      //   {
      //      MessageBox.Show(this, "سبک مربی با کلاسی که می خواهید زمان بندی کنید متفاوت می باشد", "اشکال در ایجاد کلاس");
      //      COCH_FILE_NOLookUpEdit.EditValue = null;
      //   }
      //}

      private void mb_createnewcoach_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 64 /* Execute Adm_Totl_F */){Input = GetType().Name},
                  new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "coach"))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void mb_createnewmtod_Click(object sender, EventArgs e)
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
                              "<Privilege>17</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              Job _ShowError = new Job(SendType.External, "Role", "Commons", 02 /* Execute DoWork4ErrorHandling */, SendType.Self)
                              {
                                 Input = @"<HTML>
                                             <body>
                                                <p style=""float:right"">
                                                   <ol>
                                                      <li><font face=""Tahoma"" size=""2"" color=""red"">خطا در مورد نداشتن دسترسی</font></li>
                                                      <ul>
                                                         <li><font face=""Tahoma"" size=""2"" color=""green"">احتمال زیاد شما کاربر گرامی دسترسی به ایجاد کردن گروه ندارید.</font></li>                                                                                 
                                                      </ul>
                                                   </ol>
                                                </p>
                                             </body>
                                             </HTML>"
                              };
                              _DefaultGateway.Gateway(_ShowError);
                              #endregion                           
                           })
                        },
                        #endregion
                     }),
                  #region DoWork
                  new Job(SendType.Self, 08 /* Execute Mstr_Mtod_F */){Input = GetType().Name}
                  #endregion
                  });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      //private void Club01Bn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {            
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن باشگاه موافقید؟", "حذف باشگاه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "006"),
      //                  new XElement("Delete",
      //                     new XElement("Club",
      //                        new XAttribute("code", (Club01Bs1.Current as Data.Club).CODE)
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = Club01Bs1.Current as Data.Club;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "006"),
      //                  Club01Bs1.List.OfType<Data.Club>().Where(c => c.CODE == 0).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Club",
      //                           new XAttribute("cntycode", (CntyBs1.Current as Data.Country).CODE ?? ""),
      //                           new XAttribute("prvncode", (Prvn01Bs1.Current as Data.Province).CODE ?? ""),
      //                           new XAttribute("regncode", (Regn01Bs1.Current as Data.Region).CODE ?? ""),
      //                           new XAttribute("name", c.NAME ?? ""),
      //                           new XAttribute("clubcode", c.CLUB_CODE ?? 0),
      //                           new XAttribute("postadrs", c.POST_ADRS ?? ""),
      //                           new XAttribute("emaladrs", c.EMAL_ADRS ?? ""),
      //                           new XAttribute("website", c.WEB_SITE ?? "")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CODE != 0 ? 
      //                     new XElement("Update",
      //                        new XElement("Club",
      //                           new XAttribute("code", crnt.CODE),
      //                           new XAttribute("name", crnt.NAME ?? ""),
      //                           new XAttribute("clubcode", crnt.CLUB_CODE ?? 0),
      //                           new XAttribute("postadrs", crnt.POST_ADRS ?? ""),
      //                           new XAttribute("emaladrs", crnt.EMAL_ADRS ?? ""),
      //                           new XAttribute("website", crnt.WEB_SITE ?? "")
      //                        )
      //                    ) : new XElement("Update")
                       
      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;               
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_CPR2C2_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void Regn01Bn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن ناحیه موافقید؟", "حذف ناحیه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "007"),
      //                  new XElement("Delete",
      //                     new XElement("Region",
      //                        new XAttribute("code", (Regn01Bs1.Current as Data.Region).CODE),
      //                        new XAttribute("cntycode", (Regn01Bs1.Current as Data.Region).PRVN_CNTY_CODE),
      //                        new XAttribute("prvncode", (Regn01Bs1.Current as Data.Region).PRVN_CODE)
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = Regn01Bs1.Current as Data.Region;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "007"),
      //                  Regn01Bs1.List.OfType<Data.Region>().Where(c => c.CRET_BY == null).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Region",
      //                           new XAttribute("cntycode", (CntyBs1.Current as Data.Country).CODE ?? ""),
      //                           new XAttribute("prvncode", (Prvn01Bs1.Current as Data.Province).CODE ?? ""),
      //                           new XAttribute("code", c.CODE ?? ""),
      //                           new XAttribute("regncode", c.REGN_CODE ?? ""),
      //                           new XAttribute("name", c.NAME ?? "")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CRET_BY != null ?
      //                     new XElement("Update",
      //                        new XElement("Region",
      //                           new XAttribute("cntycode", crnt.PRVN_CNTY_CODE ?? ""),
      //                           new XAttribute("prvncode", crnt.PRVN_CODE ?? ""),
      //                           new XAttribute("code", crnt.CODE ?? ""),
      //                           new XAttribute("regncode", crnt.REGN_CODE ?? ""),
      //                           new XAttribute("name", crnt.NAME ?? "")
      //                        )
      //                    ) : new XElement("Update")

      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_CPR2C2_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void Prvn01Bn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن استان موافقید؟", "حذف استان", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "008"),
      //                  new XElement("Delete",
      //                     new XElement("Province",
      //                        new XAttribute("code", (Prvn01Bs1.Current as Data.Province).CODE),
      //                        new XAttribute("cntycode", (Prvn01Bs1.Current as Data.Province).CNTY_CODE)                              
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = Prvn01Bs1.Current as Data.Province;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "008"),
      //                  Prvn01Bs1.List.OfType<Data.Province>().Where(c => c.CRET_BY == null).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Province",
      //                           new XAttribute("cntycode", (CntyBs1.Current as Data.Country).CODE ?? ""),
      //                           new XAttribute("code", c.CODE ?? ""),
      //                           new XAttribute("name", c.NAME ?? "")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CRET_BY != null ?
      //                     new XElement("Update",
      //                        new XElement("Province",
      //                           new XAttribute("cntycode", crnt.CNTY_CODE ?? ""),
      //                           new XAttribute("code", crnt.CODE ?? ""),
      //                           new XAttribute("name", crnt.NAME ?? "")
      //                        )
      //                    ) : new XElement("Update")

      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_CPR2C2_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void CntyBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;if (MessageBox.Show(this, "آیا با پاک کردن کشور موافقید؟", "حذف کشور", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "009"),
      //                  new XElement("Delete",
      //                     new XElement("Country",
      //                        new XAttribute("code", (CntyBs1.Current as Data.Country).CODE)                              
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = CntyBs1.Current as Data.Country;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "009"),
      //                  CntyBs1.List.OfType<Data.Country>().Where(c => c.CRET_BY == null).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Country",                                 
      //                           new XAttribute("code", c.CODE ?? ""),
      //                           new XAttribute("name", c.NAME ?? "")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CRET_BY != null ?
      //                     new XElement("Update",
      //                        new XElement("Country",                                 
      //                           new XAttribute("code", crnt.CODE ?? ""),
      //                           new XAttribute("name", crnt.NAME ?? "")
      //                        )
      //                    ) : new XElement("Update")

      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_CPR2C2_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void EpitBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   iScsc.CommandTimeout = 1800;
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن آیتم هزینه موافقید؟", "حذف آیتم هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "010"),
      //                  new XElement("Delete",
      //                     new XElement("Expense_Item",
      //                        new XAttribute("code", (EpitBs1.Current as Data.Expense_Item).CODE)
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = EpitBs1.Current as Data.Expense_Item;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "010"),
      //                  EpitBs1.List.OfType<Data.Expense_Item>().Where(c => c.CRET_BY == null).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Expense_Item",
      //                           new XAttribute("type", c.TYPE ?? "002"),
      //                           new XAttribute("epitdesc", c.EPIT_DESC ?? ""),
      //                           new XAttribute("autognrt", c.AUTO_GNRT ?? ""),
      //                           new XAttribute("rqtpcode", c.RQTP_CODE ?? ""),
      //                           new XAttribute("rqttcode", c.RQTT_CODE ?? "")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CRET_BY != null ?
      //                     new XElement("Update",
      //                        new XElement("Expense_Item",
      //                           new XAttribute("code", crnt.CODE),
      //                           new XAttribute("type", crnt.TYPE ?? "002"),
      //                           new XAttribute("epitdesc", crnt.EPIT_DESC ?? ""),
      //                           new XAttribute("autognrt", crnt.AUTO_GNRT ?? ""),
      //                           new XAttribute("rqtpcode", crnt.RQTP_CODE ?? ""),
      //                           new XAttribute("rqttcode", crnt.RQTT_CODE ?? "")
      //                        )
      //                    ) : new XElement("Update")

      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_Epit_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void CashBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   iScsc.CommandTimeout = 1800;
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن حساب مالی موافقید؟", " حذف حساب مالی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "011"),
      //                  new XElement("Delete",
      //                     new XElement("Cash",
      //                        new XAttribute("code", (CashBs1.Current as Data.Cash).CODE)
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:                  
      //            var crnt = CashBs1.Current as Data.Cash;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "011"),
      //                  CashBs1.List.OfType<Data.Cash>().Where(c => c.CRET_BY == null).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Cash",
      //                           new XAttribute("name", c.NAME ?? ""),
      //                           new XAttribute("type", c.TYPE ?? ""),
      //                           new XAttribute("cashstat", c.CASH_STAT ?? ""),
      //                           new XAttribute("bankname", c.BANK_NAME ?? ""),
      //                           new XAttribute("bankbrnccode", c.BANK_BRNC_CODE ?? ""),
      //                           new XAttribute("bankacntnumb", c.BANK_ACNT_NUMB ?? ""),
      //                           new XAttribute("shbaacnt", c.SHBA_ACNT ?? ""),
      //                           new XAttribute("cardnumb", c.CARD_NUMB ?? "")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CRET_BY != null ?
      //                     new XElement("Update",
      //                        new XElement("Cash",
      //                           new XAttribute("code", crnt.CODE),
      //                           new XAttribute("name", crnt.NAME ?? ""),
      //                           new XAttribute("type", crnt.TYPE ?? ""),
      //                           new XAttribute("cashstat", crnt.CASH_STAT ?? ""),
      //                           new XAttribute("bankname", crnt.BANK_NAME ?? ""),
      //                           new XAttribute("bankbrnccode", crnt.BANK_BRNC_CODE ?? ""),
      //                           new XAttribute("bankacntnumb", crnt.BANK_ACNT_NUMB ?? ""),
      //                           new XAttribute("shbaacnt", crnt.SHBA_ACNT ?? ""),
      //                           new XAttribute("cardnumb", crnt.CARD_NUMB ?? "")
      //                        )
      //                    ) : new XElement("Update")

      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_Cash_Query();
      //         requery = false;
      //      }
      //   }
      //}

      private void MdrpBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن چاپ فرم موافقید؟", " حذف چاپ فرم", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "012"),
                        new XElement("Delete",
                           new XElement("Modual_Report",
                              new XAttribute("code", (MdrpBs1.Current as Data.Modual_Report).CODE)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  var crnt = MdrpBs1.Current as Data.Modual_Report;

                  if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

                  iScsc.STNG_SAVE_P(
                     new XElement("Config",
                        new XAttribute("type", "012"),
                        MdrpBs1.List.OfType<Data.Modual_Report>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Modual_Report",
                                 new XAttribute("mdulname", Modul_Name ?? ""),
                                 new XAttribute("mduldesc", c.MDUL_DESC ?? ""),
                                 new XAttribute("sectname", Section_Name ?? ""),
                                 new XAttribute("sectdesc", c.SECT_DESC ?? ""),
                                 new XAttribute("rprtpath", c.RPRT_PATH ?? ""),
                                 new XAttribute("rprtdesc", c.RPRT_DESC ?? ""),
                                 new XAttribute("showprvw", c.SHOW_PRVW ?? ""),
                                 new XAttribute("dflt", c.DFLT ?? ""),
                                 new XAttribute("prntaftrpay", c.PRNT_AFTR_PAY ?? ""),
                                 new XAttribute("stat", c.STAT ?? "")
                              )
                           )
                        ),
                        crnt.CRET_BY != null ?
                           new XElement("Update",
                              new XElement("Modual_Report",
                                 new XAttribute("code", crnt.CODE),
                                 new XAttribute("mdulname", crnt.MDUL_NAME ?? ""),
                                 new XAttribute("mduldesc", crnt.MDUL_DESC ?? ""),
                                 new XAttribute("sectname", crnt.SECT_NAME ?? ""),
                                 new XAttribute("sectdesc", crnt.SECT_DESC ?? ""),
                                 new XAttribute("rprtpath", crnt.RPRT_PATH ?? ""),
                                 new XAttribute("rprtdesc", crnt.RPRT_DESC ?? ""),
                                 new XAttribute("showprvw", crnt.SHOW_PRVW ?? ""),
                                 new XAttribute("dflt", crnt.DFLT ?? ""),
                                 new XAttribute("prntaftrpay", crnt.PRNT_AFTR_PAY ?? ""),
                                 new XAttribute("stat", crnt.STAT ?? "")
                              )
                          ) : new XElement("Update")

                     )
                  );
                  requery = true;
                  break;
            }
         }
         catch (SqlException se)
         {
            switch (se.Number)
            {
               case 515:
                  MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
                  break;
               default:
                  MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
                  break;
            }
         }
         finally
         {
            if (requery)
            {
               Execute_ModualReport_Query();
               requery = false;
            }
         }
      }

      private void mb_ReportBrowser_Click(object sender, EventArgs e)
      {
         if (Ofd_ReportFiles.ShowDialog(this) != DialogResult.OK) return;
         RPRT_PATHTextEdit.EditValue = RPRT_PATHTextEdit.Text = Ofd_ReportFiles.FileName;
         MdrpBs1.EndEdit();
      }

      //private void URqrqBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن دسترسی قرارداد کاربر موافقید؟", "حذف دسترسی قرارداد کاربر", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "013"),
      //                  new XElement("Delete",
      //                     new XElement("Fga_URqrq",
      //                        new XAttribute("code", (Fga_URqrq_Bs1.Current as Data.User_Request_Requester_Fgac).FGA_CODE)
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = Fga_URqrq_Bs1.Current as Data.User_Request_Requester_Fgac;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "013"),
      //                  Fga_URqrq_Bs1.List.OfType<Data.User_Request_Requester_Fgac>().Where(c => c.CRET_BY == null).Select(c =>
      //                     new XElement("Insert",
      //                        new XElement("Fga_URqrq",
      //                           new XAttribute("sysuser", c.SYS_USER ?? ""),
      //                           new XAttribute("rqtpcode", c.RQTP_CODE ?? ""),
      //                           new XAttribute("rqttcode", c.RQTT_CODE ?? "")
      //                        )
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_URQRQ_Query();
      //         requery = false;
      //      }
      //   }
      //}

      //private void DresBn_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      //{
      //   try
      //   {
      //      switch (e.Button.ButtonType)
      //      {
      //         case DevExpress.XtraEditors.NavigatorButtonType.Append:
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.Remove:
      //            e.Handled = true;
      //            if (MessageBox.Show(this, "آیا با پاک کردن کمد باشگاه موافقید؟", "حذف کمد باشگاه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "014"),
      //                  new XElement("Delete",
      //                     new XElement("Dresser",
      //                        new XAttribute("code", (DresBs1.Current as Data.Dresser).CODE)
      //                     )
      //                  )
      //               )
      //            );
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
      //            requery = true;
      //            break;
      //         case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
      //            var crnt = DresBs1.Current as Data.Dresser;

      //            if (crnt.CRET_BY != null && MessageBox.Show(this, "آیا با ویرایش کردن رکورد جاری موافقید؟", "ویرایش اطلاعات", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

      //            iScsc.STNG_SAVE_P(
      //               new XElement("Config",
      //                  new XAttribute("type", "014"),
      //                  DresBs1.List.OfType<Data.Dresser>().Where(c => c.CRET_BY == null).Select(d =>
      //                     new XElement("Insert",
      //                        new XElement("Dresser",
      //                           new XAttribute("clubcode", UserClub_Lookup.EditValue),
      //                           new XAttribute("dresnumb", d.DRES_NUMB ?? 0),
      //                           new XAttribute("desc", d.DESC ?? ""),
      //                           new XAttribute("resstat", d.REC_STAT ?? "002")
      //                        )
      //                     )
      //                  ),
      //                  crnt.CRET_BY != null ?
      //                     new XElement("Update",
      //                        new XElement("Dresser",                                 
      //                           new XAttribute("code", crnt.CODE),
      //                           new XAttribute("dresnumb", crnt.DRES_NUMB ?? 0),
      //                           new XAttribute("desc", crnt.DESC ?? ""),
      //                           new XAttribute("resstat", crnt.REC_STAT ?? "002")
      //                        )
      //                     ) : new XElement("Update")
      //               )
      //            );
      //            requery = true;
      //            break;
      //      }
      //   }
      //   catch (SqlException se)
      //   {
      //      switch (se.Number)
      //      {
      //         case 515:
      //            MessageBox.Show("لطفا گزینه های ستاره دار را حتما وارد کنید");
      //            break;
      //         default:
      //            MessageBox.Show("خطا در انجام عملیات لطفا بررسی کنید");
      //            break;
      //      }
      //   }
      //   finally
      //   {
      //      if (requery)
      //      {
      //         Execute_ClubShare_Query();
      //         requery = false;
      //      }
      //   }
      //}

      private void ClubShare_Lookup_EditValueChanged(object sender, EventArgs e)
      {
         if(tc_Settings.SelectedTab == tp_backuprestore)
         {
            Execute_ClubShare_Query();
         }
         //else if(tc_Settings.SelectedTab == tp_004)
         //{
         //   Execute_ClubShare_Query();
         //}
      }

      //private void CASH_TYPE_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      //{
      //   if (CASH_TYPE_LookUpEdit.ItemIndex == -1) return;

      //   if(CASH_TYPE_LookUpEdit.EditValue.ToString() == "001")
      //   {
      //      bANK_BRNC_CODETextEdit.Enabled = true;
      //      bANK_ACNT_NUMBTextEdit.Enabled = true;
      //      cARD_NUMBTextEdit.Enabled = true;
      //      sHBA_ACNTTextEdit.Enabled = true;
      //   }
      //   else if (CASH_TYPE_LookUpEdit.EditValue.ToString() == "002")
      //   {
      //      bANK_BRNC_CODETextEdit.Enabled = false;
      //      bANK_ACNT_NUMBTextEdit.Enabled = false;
      //      cARD_NUMBTextEdit.Enabled = false;
      //      sHBA_ACNTTextEdit.Enabled = false;
      //   }         
      //}

      //private void STRT_TIMETextEdit_Leave(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if(END_TIMETextEdit.Text == "00:00" || END_TIMETextEdit.Text.CompareTo(STRT_TIMETextEdit.Text) < 0)
      //         END_TIMETextEdit.Text = Convert.ToDateTime(STRT_TIMETextEdit.Text).AddMinutes(90).ToString("HH:mm");
      //   }
      //   catch 
      //   {}
      //}

      //private void END_TIMETextEdit_Leave(object sender, EventArgs e)
      //{
      //   try
      //   {
      //      if(END_TIMETextEdit.Text.CompareTo(STRT_TIMETextEdit.Text) < 0)
      //         END_TIMETextEdit.Text = Convert.ToDateTime(STRT_TIMETextEdit.Text).AddMinutes(90).ToString("HH:mm");
      //   }
      //   catch 
      //   {}
      //}

      //private void Lb_ListPort_MouseDoubleClick(object sender, MouseEventArgs e)
      //{
      //   if(UserClub_Lookup.ItemIndex == -1) return;

      //   var Stng = StngBs1.List.OfType<Data.Setting>().Where(s => s.CLUB_CODE == (long?)UserClub_Lookup.EditValue).FirstOrDefault();

      //   if (Stng == null) return;

      //   cOMM_PORT_NAMETextEdit.EditValue = Stng.COMM_PORT_NAME = Lb_ListPort.SelectedItem.ToString();
      //}

      //private void tYPELookUpEdit1_EditValueChanged(object sender, EventArgs e)
      //{
      //   if (tYPELookUpEdit1.ItemIndex == -1) return;

      //   radGroupBox5.Enabled = tYPELookUpEdit1.EditValue.ToString() == "001" || tYPELookUpEdit1.EditValue.ToString() == "003" ? true : false;
      //}

   }
}
