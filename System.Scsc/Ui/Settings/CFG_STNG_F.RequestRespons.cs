using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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
   partial class CFG_STNG_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string Crnt_User;
      private string Modul_Name, Section_Name;
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
            // 1396/10/23 * 
            switch (Modul_Name)
            {
               case "WHO_ARYU_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {                           
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),                           
                        })
                  );
                  break;
               case "MBSP_MARK_F":
                  _DefaultGateway.Gateway(                     
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 155 /* Execute Mbsp_Mark_F */)
                        }
                     )
                  );
                  break;
               default:
                  break;
            }
            Modul_Name = "";
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
         }
         else if (keyData == Keys.Enter)
         {
         }
         else if (keyData == Keys.F2)
         {
         }
         else if (keyData == Keys.F8)
         {
         }
         else if (keyData == Keys.F5)
         {
         }
         else if (keyData == Keys.F3)
         {
         }
         else if (keyData == Keys.F10)
         {
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
         Crnt_User = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

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
                  case "mudlname_clm":
                     MudlName_Clm.Caption = control.LABL_TEXT;
                     //MudlName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MudlName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sectname_lb":
                     SectName_Lb.Text = control.LABL_TEXT;
                     //SectName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SectName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "titlform_lb":
                     TitlForm_Lb.Text = control.LABL_TEXT;
                     //TitlForm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TitlForm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_backuprestore":
                     tp_backuprestore.Text = control.LABL_TEXT;
                     //tp_backuprestore.Text = control.LABL_TEXT; // ToolTip
                     //tp_backuprestore.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tp_printmodual":
                     tp_printmodual.Text = control.LABL_TEXT;
                     //tp_printmodual.Text = control.LABL_TEXT; // ToolTip
                     //tp_printmodual.Text = control.LABL_TEXT; // Place Holder
                     break;
                     break;
                  case "btn_takebackup":
                     Btn_TakeBackup.Text = control.LABL_TEXT;
                     //Btn_TakeBackup.Text = control.LABL_TEXT; // ToolTip
                     //Btn_TakeBackup.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_apply":
                     Btn_APPLY.Text = control.LABL_TEXT;
                     //Btn_APPLY.Text = control.LABL_TEXT; // ToolTip
                     //Btn_APPLY.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "optionpath_lb":
                     OptionPath_Lb.Text = control.LABL_TEXT;
                     //OptionPath_Lb.Text = control.LABL_TEXT; // ToolTip
                     //OptionPath_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "filename_clm":
                     FileName_Clm.Caption = control.LABL_TEXT;
                     //FileName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //FileName_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "directory_clm":
                     Directory_Clm.Caption = control.LABL_TEXT;
                     //Directory_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Directory_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_restoredb":
                     Btn_RestoreDb.Text = control.LABL_TEXT;
                     //Btn_RestoreDb.Text = control.LABL_TEXT; // ToolTip
                     //Btn_RestoreDb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_deletebackup":
                     Btn_DeleteBackup.Text = control.LABL_TEXT;
                     //Btn_DeleteBackup.Text = control.LABL_TEXT; // ToolTip
                     //Btn_DeleteBackup.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_querybackupfile":
                     Btn_QueryBackupFile.Text = control.LABL_TEXT;
                     //Btn_QueryBackupFile.Text = control.LABL_TEXT; // ToolTip
                     //Btn_QueryBackupFile.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rprtrwno_clm":
                     RprtRwno_Clm.Caption = control.LABL_TEXT;
                     //RprtRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RprtRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mudldesc_clm":
                     MudlDesc_Clm.Caption = control.LABL_TEXT;
                     //MudlDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MudlDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mudlname_lb":
                     MudlName_Lb.Text = control.LABL_TEXT;
                     //MudlName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MudlName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "showprvw_lb":
                     ShowPrvw_Lb.Text = control.LABL_TEXT;
                     //ShowPrvw_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ShowPrvw_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mudldesc_lb":
                     MudlDesc_Lb.Text = control.LABL_TEXT;
                     //MudlDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MudlDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "afterprntpay_lb":
                     AfterPrntPay_Lb.Text = control.LABL_TEXT;
                     //AfterPrntPay_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AfterPrntPay_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sectdesc_lb":
                     SectDesc_Lb.Text = control.LABL_TEXT;
                     //SectDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SectDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rprtpath_lb":
                     RprtPath_Lb.Text = control.LABL_TEXT;
                     //RprtPath_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RprtPath_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rprtdesc_lb":
                     RprtDesc_Lb.Text = control.LABL_TEXT;
                     //RprtDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RprtDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "stat_lb":
                     Stat_Lb.Text = control.LABL_TEXT;
                     //Stat_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Stat_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dflt_lb":
                     Dflt_Lb.Text = control.LABL_TEXT;
                     //Dflt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Dflt_Lb.Text = control.LABL_TEXT; // Place Holder
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
         //      //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */) {  Input = new List<object> { this, "left:in-screen:stretch:center" } }               
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
         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
         //         new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
         //         //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
         //      })
         //   );

         job.Next =
            new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
               new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

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
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         try
         {
            #region Settings
            //PrvnBs1.DataSource = iScsc.Provinces;
            
            DActvBs1.DataSource = iScsc.D_ACTVs;            
            DYsnoBs1.DataSource = iScsc.D_YSNOs;            
            DBcdtBs4.DataSource = iScsc.D_BCDTs;
            vUserBs.DataSource = iScsc.V_Users;
            vCompBs1.DataSource = iScsc.V_Computers;

            Printers_Lst.Items.Clear();
            foreach (string printname in PrinterSettings.InstalledPrinters)
            {
               Printers_Lst.Items.Add(printname);
            }
            Printers_Lst.SelectedIndex = 0;
            
            //Execute_Query();
            #endregion
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         //CochBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "002" || f.FGPB_TYPE_DNRM == "003") && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE));
         //MtodBs1.DataSource = iScsc.Methods;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         //tc_Settings.TabPages.Remove(tp_backuprestore);
         //tc_Settings.TabPages.Remove(tp_base);
         //tc_Settings.TabPages.Remove(tp_printmodual);
         tc_Settings.TabPages.Clear();
         var input = job.Input as XElement;
         switch (input.Attribute("type").Value)
         {
            case "BackupRestore":
               tc_Settings.TabPages.Add(tp_backuprestore);
               tc_Settings.SelectedTab = tp_backuprestore;               
               break;
            case "EmerjncyBackup":
               tc_Settings.TabPages.Add(tp_backuprestore);
               tc_Settings.SelectedTab = tp_backuprestore;
               Execute_Query();
               Btn_TakeBackup_Click(null, null);
               return;
            //case "UserRegionClub":
            //   tc_Settings.TabPages.Add(tp_base);
            //   tc_Settings.SelectedTab = tp_base;
            //   switch(input.Attribute("section").Value)
            //   {
            //      case "userview":
            //         tsm_bas_close_Click(null, null);
            //         tsm_bas_R1_Click(null, null);
            //         break;
            //      case "cash":
            //         tsm_bas_close_Click(null, null);
            //         tsm_bas_R2_Click(null, null);
            //      break;
            //      case "epit":
            //         tsm_bas_close_Click(null, null);
            //         tsm_bas_R3_Click(null, null);
            //      break;
            //      case "regn":
            //         tsm_bas_close_Click(null, null);
            //         tsm_bas_R4_Click(null, null);
            //      break;
            //   }
            //   break;
            case "ModualReport":
               tc_Settings.TabPages.Add(tp_printmodual);
               tc_Settings.SelectedTab = tp_printmodual;
               Modul_Name = input.Attribute("modul").Value;
               Section_Name = input.Attribute("section").Value;
               break;
            //case "tp_004":
            //   tc_Settings.TabPages.Add(tp_004);
            //   tc_Settings.SelectedTab = tp_004;
            //   break;
            default:
               break;
         }
         Execute_Query();
         //Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
