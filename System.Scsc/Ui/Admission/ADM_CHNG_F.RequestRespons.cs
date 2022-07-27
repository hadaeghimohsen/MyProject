using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission
{
   partial class ADM_CHNG_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private bool isFirstLoaded = false;
      private string CurrentUser;
      private string formCaller;
      private string RegnLang = "054";
      private string followups = "";
      private long rqstRqid = 0;

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
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

            switch (formCaller)
            {
               case "ALL_FLDF_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", formCaller, 08 /* Exec LoadDataSource */, SendType.SelfToUserInterface)
                  );
                  break;
               default:
                  break;
            }
            formCaller = "";
            //job.Next =
            //   new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {            
               Btn_RqstBnRqt_Click(null, null);
            
         }
         else if (keyData == Keys.Enter)
         {
            
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            
               Btn_RqstDelete_Click(null, null);            
         }
         else if (keyData == Keys.F5)
         {
            
               Btn_RqstBnRqt_Click(null, null);            
         }
         else if (keyData == Keys.F10)
         {
            
               Btn_RqstSav_Click(null, null);            
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
                  case "gb_info":
                     //Gb_Info.Text = control.LABL_TEXT;
                     //Gb_Info.Text = control.LABL_TEXT; // ToolTip
                     //Gb_Info.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fathjob_lb":
                     FathJob_Lb.Text = control.LABL_TEXT;
                     //FathJob_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FathJob_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "servno_lb":
                     ServNo_Lb.Text = control.LABL_TEXT;
                     //ServNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ServNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cycl_lb":
                     Cycl_Lb.Text = control.LABL_TEXT;
                     //Cycl_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Cycl_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "filenos_lb":
                     FileNos_Lb.Text = control.LABL_TEXT;
                     //FileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fgpbtype_lb":
                     FgpbType_Lb.Text = control.LABL_TEXT;
                     //FgpbType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FgpbType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "frstname_lb":
                     FrstName_Lb.Text = control.LABL_TEXT;
                     //FrstName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FrstName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_lb":
                     FngrPrnt_Lb.Text = control.LABL_TEXT;
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "lastname_lb":
                     LastName_Lb.Text = control.LABL_TEXT;
                     //LastName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LastName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdate_lb":
                     BrthDate_Lb.Text = control.LABL_TEXT;
                     //BrthDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "sextype_lb":
                     SexType_Lb.Text = control.LABL_TEXT;
                     //SexType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SexType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "natlcode_lb":
                     NatlCode_Lb.Text = control.LABL_TEXT;
                     //NatlCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NatlCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dpstacntslrybank_lb":
                     DpstAcntSlryBank_Lb.Text = control.LABL_TEXT;
                     //DpstAcntSlryBank_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DpstAcntSlryBank_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cardnumb_lb":
                     CardNumb_Lb.Text = control.LABL_TEXT;
                     //CardNumb_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CardNumb_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "chatid_lb":
                     ChatId_Lb.Text = control.LABL_TEXT;
                     //ChatId_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ChatId_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fathname_lb":
                     FathName_Lb.Text = control.LABL_TEXT;
                     //FathName_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FathName_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "insrnumb_lb":
                     InsrNumb_Lb.Text = control.LABL_TEXT;
                     //InsrNumb_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InsrNumb_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon_lb":
                     CellPhon_Lb.Text = control.LABL_TEXT;
                     //CellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gudgdeg_lb":
                     GudgDeg_Lb.Text = control.LABL_TEXT;
                     //GudgDeg_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GudgDeg_Lb.Text = control.LABL_TEXT; // Place Holder
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
                  case "cordx_lb":
                     CordX_Lb.Text = control.LABL_TEXT;
                     //CordX_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CordX_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthplac_lb":
                     BrthPlac_Lb.Text = control.LABL_TEXT;
                     //BrthPlac_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthPlac_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "histdesc_lb":
                     HistDesc_Lb.Text = control.LABL_TEXT;
                     //HistDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //HistDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "educdeg_lb":
                     EducDeg_Lb.Text = control.LABL_TEXT;
                     //EducDeg_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EducDeg_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "insrdate_lb":
                     InsrDate_Lb.Text = control.LABL_TEXT;
                     //InsrDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //InsrDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon_lb":
                     TellPhon_Lb.Text = control.LABL_TEXT;
                     //TellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "globcode_lb":
                     GlobCode_Lb.Text = control.LABL_TEXT;
                     //GlobCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //GlobCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cordy_lb":
                     CordY_Lb.Text = control.LABL_TEXT;
                     //CordY_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CordY_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "issuplac_lb":
                     IssuPlac_Lb.Text = control.LABL_TEXT;
                     //IssuPlac_Lb.Text = control.LABL_TEXT; // ToolTip
                     //IssuPlac_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "disecode_lb":
                     DiseCode_Lb.Text = control.LABL_TEXT;
                     //DiseCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DiseCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochdeg_lb":
                     CochDeg_Lb.Text = control.LABL_TEXT;
                     //CochDeg_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochDeg_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochcrtfdate_lb":
                     CochCrtfDate_Lb.Text = control.LABL_TEXT;
                     //CochCrtfDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CochCrtfDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "blodgrop_lb":
                     BlodGrop_Lb.Text = control.LABL_TEXT;
                     //BlodGrop_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BlodGrop_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode_lb":
                     SuntCode_Lb.Text = control.LABL_TEXT;
                     //SuntCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //SuntCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "gb_rqst":
                     Gb_Rqst.Text = control.LABL_TEXT;
                     //Gb_Rqst.Text = control.LABL_TEXT; // ToolTip
                     //Gb_Rqst.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqid_lb":
                     Rqid_Lb.Text = control.LABL_TEXT;
                     //Rqid_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rqid_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby_lb":
                     CretBy_Lb.Text = control.LABL_TEXT;
                     //CretBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CretBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby_lb":
                     MdfyBy_Lb.Text = control.LABL_TEXT;
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpcode_lb":
                     RqtpCode_Lb.Text = control.LABL_TEXT;
                     //RqtpCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretdate_lb":
                     CretDate_Lb.Text = control.LABL_TEXT;
                     //CretDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CretDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfydate_lb":
                     MdfyDate_Lb.Text = control.LABL_TEXT;
                     //MdfyDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqttcode_lb":
                     RqttCode_Lb.Text = control.LABL_TEXT;
                     //RqttCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqttCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "year_lb":
                     Year_Lb.Text = control.LABL_TEXT;
                     //Year_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Year_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
               }
            }
         }
         #endregion


         //LL_MoreInfo_LinkClicked(null, null);
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
                           Input = new List<string> {"<Privilege>260</Privilege><Sub_Sys>5</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 job.Status = StatusType.Successful;
                              else
                              {
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 260 سطوح امینتی", "عدم دسترسی");
                                 job.Status = StatusType.Failed;
                              }
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
         #region Rqsw block
         try
         {
            if (isFirstLoaded) goto finishcommand;           

            DDegrBs2.DataSource = iScsc.D_DEGRs;
            DCyclBs1.DataSource = iScsc.D_CYCLs;
            //MtodBs2.DataSource = iScsc.Methods;
            //CtgyBs2.DataSource = iScsc.Category_Belts.Where(c => c.Method == MtodBs2.Current as Scsc.Data.Method).OrderBy(c => c.ORDR);
            DSxtpBs1.DataSource = iScsc.D_SXTPs.Where(d => d.VALU != "003");
            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DEducBs1.DataSource = iScsc.D_EDUCs;
            DstpBs1.DataSource = iScsc.Diseases_Types;
            DFgtpBs1.DataSource = iScsc.D_FGTPs.Where(d => d.VALU != "002" && d.VALU != "006" && d.VALU != "007" && d.VALU != "008" && d.VALU != "009");
            DCetpBs1.DataSource = iScsc.D_CETPs;
            DBlodBs1.DataSource = iScsc.D_BLODs;
            DOwnrBs1.DataSource = iScsc.D_OWNRs;

            isFirstLoaded = true;
            finishcommand:
            
            if (InvokeRequired)
               Invoke(new Action(() => Execute_Query()));
            else
               Execute_Query();

            if (InvokeRequired)
               Invoke(new Action(() => 
                  {
                     //CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);
                     SuntBs1.DataSource = iScsc.Sub_Units;
                     CndoBs1.DataSource = iScsc.Candos;
                  })
               );
            else
            {
               //CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);
               SuntBs1.DataSource = iScsc.Sub_Units;
               //CndoBs1.DataSource = iScsc.Candos;
            }
            

            //if (InvokeRequired)
            //   Invoke(new Action(() => FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" && !f.NAME_DNRM.Contains("مشتری, جلسه ای") && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101).OrderBy(f => f.FGPB_TYPE_DNRM)));
            //else
            //   FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" && !f.NAME_DNRM.Contains("مشتری, جلسه ای") && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101).OrderBy(f => f.FGPB_TYPE_DNRM);
         }
         catch { }
         #endregion

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);
         DstpBs1.DataSource = iScsc.Diseases_Types;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         switch (xinput.Attribute("type").Value)
         {
            case "changeinfo":
               if (RqstBs1.Count > 0 && (RqstBs1.Current as Data.Request).RQID > 0)
                  RqstBs1.AddNew();
               FILE_NO_LookUpEdit.EditValue = Convert.ToInt64(xinput.Attribute("fileno").Value);
               if(xinput.Attribute("auto").Value == "true")
               {
                  // 1397/05/26 * rqstrqid
                  if ((job.Input as XElement).Attribute("rqstrqid") != null)
                     rqstRqid = Convert.ToInt64((job.Input as XElement).Attribute("rqstrqid").Value);
                  else
                     rqstRqid = 0;

                  Btn_RqstBnRqt_Click(null, null);

                  var figh = iScsc.Fighters.First(f => f.FILE_NO == Convert.ToInt64(xinput.Attribute("fileno").Value));

                  RqstBs1.Position = RqstBs1.List.OfType<Data.Request>().ToList().FindIndex(r => r.RQID == figh.RQST_RQID);

                  // 1396/11/04 * formcaller
                  if ((job.Input as XElement).Attribute("formcaller") != null)
                     formCaller = (job.Input as XElement).Attribute("formcaller").Value;
                  else
                     formCaller = "";

                  // 1397/05/26 * followups
                  if ((job.Input as XElement).Attribute("followups") != null)
                     followups = (job.Input as XElement).Attribute("followups").Value;
                  else
                     followups = "";
               }
               break;
            case "setcard":               
               CardNumb_Text.Text = (job.Input as XElement).Attribute("value").Value;
               FNGR_PRNT_TextEdit.EditValue = (job.Input as XElement).Attribute("value").Value;   
               //if (AutoTrans_Cb.Checked)
               //CardNumb_Text.EditValue = FNGR_PRNT_TextEdit.EditValue;
               break;
            case "rqidfocus":               
               ShowRqst_PickButn.PickChecked = false;
               RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.RQID == Convert.ToInt64(xinput.Attribute("rqid").Value)));
               break;
            default:
               break;
         }
         job.Status = StatusType.Successful;
      }
   }
}
