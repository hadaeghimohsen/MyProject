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
            if (tc_Settings.SelectedTab == tp_backuprestore)
            {
               StngBs1.DataSource = iScsc.Settings.Take(1);

               Execute_ClubShare_Query();
            }
            else if (tc_Settings.SelectedTab == tp_printmodual)
            {
               Execute_ModualReport_Query();
            }
         }
         catch { }
      }
      
      private void Execute_ModualReport_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         var _0 = MdrpBs1.Position;
         MdrpBs1.DataSource = iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == Modul_Name && mr.SECT_NAME == Section_Name);
         MdrpBs1.Position = _0;
      }

      private void Execute_ClubShare_Query()
      {
          try
          {
              if (tc_Settings.SelectedTab == tp_backuprestore)
              {
                  var Stng = StngBs1.Current as Data.Setting;
                  //if (Stng == null)
                  //{
                  //    Stng = new Data.Setting() { BACK_UP = true, BACK_UP_APP_EXIT = true, BACK_UP_IN_TRED = true, BACK_UP_OPTN_PATH = true, BACK_UP_OPTN_PATH_ADRS = @"D:\iData\Asre Andishe Project\Database Backup", BACK_UP_ROOT_PATH = @"C:\Backup", CLUB_CODE = (long)ClubStng_Lookup.EditValue, DRES_STAT = "002", DRES_AUTO = "002" };
                  //}
                  RestBs1.DataSource = new DirectoryInfo(Stng.BACK_UP_OPTN_PATH == true ? Stng.BACK_UP_OPTN_PATH_ADRS + @"\Backup" : Stng.BACK_UP_ROOT_PATH).GetFiles("*.bak");
              }
          }
          catch { }
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
            StngBs1.EndEdit();
            Data.Setting Stng = null;
            if(tc_Settings.SelectedTab == tp_backuprestore)
               Stng = StngBs1.List.OfType<Data.Setting>().FirstOrDefault();

            Stng.BACK_UP_OPTN_PATH = true;

            if(tc_Settings.SelectedTab == tp_backuprestore)
               iScsc.STNG_SAVE_P(
                  new XElement("Request",
                     new XElement("Settings",
                        new XAttribute("clubcode", 0),
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
         //Btn_Back_Click(sender, e);
      }

      string BackupType = "NORMAL";
      private void Btn_TakeBackup_Click(object sender, EventArgs e)
      {
         try
         {
            Btn_APPLY_Click(null, null);

            var stng = StngBs1.Current as Data.Setting;
            iScsc.CommandTimeout = int.MaxValue;
            iScsc.TAK_BKUP_P(new XElement("Request", new XElement("Backup", new XAttribute("type", BackupType), new XAttribute("clubcode", stng.CLUB_CODE))));
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
   }
}
