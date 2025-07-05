using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Scsc.ExtCode;
using System.Threading;
using System.Diagnostics;

namespace System.Scsc.Ui.MasterPage
{
   partial class MAIN_PAGE_F: ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private List<long?> Fga_Uclb_U;
      private string CurrentUser;
      private string RegnLang = "054";
      private DateTime ExprInstDate, CrntDate = DateTime.Now;
      private bool isFirstLoaded = false;

      //private bool requery = default(bool);

      private XElement x = null;
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
               PostOnWall(job);
               break;
            case 09:
               TakeOnWall(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;            
            case 40:
               SetToolTip(job);
               break;
            case 41:
               SetCardinDevice(job);
               break;
            case 42:
               ShowMessageOnLCD(job);
               break;
            case 43:
               DeviceControlFunction(job);
               break;
            case 44:
               PlaySystemSound(job);
               break;
            case 1000:
               if (InvokeRequired)
                  Invoke(new Action(() => Call_SystemService_P(job)));
               else
                  Call_SystemService_P(job);
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
         else if(keyData == Keys.F2)
         {
            bbi_adm1butn_ItemClick(null, null);
         }
         else if (keyData == Keys.F3)
         {
            bbi_mbs1butn_ItemClick(null, null);
         }
         else if(keyData == Keys.F4)
         {
            bbi_flstbutn_ItemClick(null, null);
         }
         else if(keyData == Keys.F5)
         {
            bbi_oincbutn_ItemClick(null, null);
         }
         else if(keyData == Keys.F6)
         {
            bbi_exdsbutn_ItemClick(null, null);
         }
         else if(keyData == Keys.F9)
         {
            var fngrprnt = Microsoft.VisualBasic.Interaction.InputBox("EnrollNumber", "Input EnrollNumber");

            CardNumb_Text.Text = fngrprnt.ToString();
            attnsystype = "002";
            axCZKEM1_OnAttTransactionEx(fngrprnt.ToString(), 1, 1, 1, 2016, 05, 10, 09, 31, 50, 20);
         }
         else if (keyData == Keys.Escape)
         {            
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData.ToString() == "Right, Control")
         {
            Open_Gate(null);            
         }
         else if (keyData.ToString() == "Left, Control")
         {
            Close_Gate(null);
         }
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         try
         {
            /* Get License Days Remind */
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 37 /* Execute DoWork4GetLicenseDay */, SendType.Self)
               {
                  Input =
                     new XElement("SubSys",
                        new XAttribute("type", "getlicenseday"),
                        new XAttribute("subsys", "5")
                     ),
                  AfterChangedOutput =
                     new Action<object>(
                        (output) =>
                        {
                           var licndate = (DateTime)output;

                           if ((licndate.Date - DateTime.Now.Date).Days >= 15)
                           {
                              Licnday_Lnk.Text = string.Format("پشتیبانی     ---     {0} روز", (licndate.Date - DateTime.Now.Date).Days);
                              Licnday_Lnk.LinkColor = Color.Green;
                           }
                           else if ((licndate.Date - DateTime.Now.Date).Days.IsBetween(0, 7))
                           {
                              //Licnday_Lnk.Text = "پشتیبانی نرم افزار شما کمتر 7 روز دیگر به اتمام میرسد، در صورت عدم پرداخت مبلغ پشتیبانی دیگر قادر به استفاده از نرم افزار و ثبت اطلاعات نیستید" + Environment.NewLine +
                              //                   "فقط دقت داشته باشید که لایسنس جدید نرم افزار فقط و فقط بر اساس شماره پیگیری واریز وجه بانکی شما قابل تمدید میباشد" + Environment.NewLine + Environment.NewLine +
                              //                   string.Format("تعداد روز باقیمانده     ---     {0} روز", (licndate.Date - DateTime.Now.Date).Days);
                              Licnday_Lnk.Text = "پشتیبانی نرم افزار شما کمتر 7 روز دیگر به اتمام میرسد" + Environment.NewLine + Environment.NewLine +
                                                 string.Format("تعداد روز باقیمانده     ---     {0} روز", (licndate.Date - DateTime.Now.Date).Days);
                              Licnday_Lnk.LinkColor = Color.Red;
                           }
                           else if ((licndate.Date - DateTime.Now.Date).Days.IsBetween(0, 15))
                           {
                              Licnday_Lnk.Text = "پشتیبانی رو به اتمام میباشد";
                              Licnday_Lnk.LinkColor = Color.Red;
                           }
                           else
                           {
                              Licnday_Lnk.Text = string.Format("پشتیبانی به پایان رسیده است", (licndate.Date - DateTime.Now.Date).Days);
                              Licnday_Lnk.LinkColor = Color.Red;
                           }

                           if ((licndate.Date - DateTime.Now.Date).Days >= 15)
                              //CertificateLogo_Pb.Image = System.Scsc.Properties.Resources.IMAGE_1656;
                           {
                              CertificateLogo_Pb.NormalColorA = CertificateLogo_Pb.NormalColorB = Color.YellowGreen;
                           }

                           else if ((licndate.Date - DateTime.Now.Date).Days <= 15 && (licndate.Date - DateTime.Now.Date).Days >= 0)
                           {
                              //CertificateLogo_Pb.Image = System.Scsc.Properties.Resources.IMAGE_1658;
                              CertificateLogo_Pb.NormalColorA = CertificateLogo_Pb.NormalColorB = Color.Gray;
                           }
                           else
                           {
                              //CertificateLogo_Pb.Image = System.Scsc.Properties.Resources.IMAGE_1657;
                              CertificateLogo_Pb.NormalColorA = CertificateLogo_Pb.NormalColorB = Color.Red;
                           }
                        }
                     )
               }
            );            
         }
         catch { }
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

         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         Lbs_CrntUser.Text = CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         //Tm_ShowTime_Tick(null, null);         

         // 1400/12/14 * بررسی اینکه نرم افزار تاریخ سر رسید پرداخت عقب افتاده نداشته باشد
         var _SubSys = iScsc.V_SubSies.FirstOrDefault();
         if (_SubSys.EXPR_INST_DATE == null || _SubSys.EXPR_INST_DATE == "")
         {
            MessageBox.Show("محتوای اطلاعات خالی میباشد", "خطای انقضای ماهیانه محصول", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            Application.Exit();
            Process.GetCurrentProcess().Kill();
         }

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 41 /* Execute DoWork4Decrypt */, SendType.Self)
            {
               Input =
                  _SubSys.EXPR_INST_DATE,
               AfterChangedOutput =
                  new Action<object>(
                     (output) =>
                     {
                        ExprInstDate = Convert.ToDateTime(output);
                        //CrntDate = DateTime.Now;

                        if ((ExprInstDate.Date - DateTime.Now.Date).Days < 0)
                        {
                           MessageBox.Show("خطای کد فعال ساز:" + Environment.NewLine + "سیستم شما به دلیل عدم دریافت کد فعال ساز سیستم گیت و کمد آنلاین غیر فعال شده است." + Environment.NewLine + "جهت دریافت کد فعال ساز با شماره 09919004540 تماس بگیرید.", "خطای عدم فعالسازی سخت افزار شرکت نورو", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                           //MessageBox.Show("تاریخ اعتبار شما به پایان رسیده", "خطای انقضای ماهیانه محصول", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                           Application.Exit();
                           Process.GetCurrentProcess().Kill();
                        }
                     }
                  )
            }
         );

         #region Package Item
         //rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self) ;
         _DefaultGateway.Gateway( GetHostInfo );

         var Pkac = iScsc.VF_AccessPackage(GetHostInfo.Output as XElement).ToList();

         //rd_mainmenu.CommandTabs.OfType<RibbonTab>().ToList().ForEach(rt => rt.Items.OfType<RadRibbonBarGroup>().ToList().ForEach(rrbg => rrbg.Items.OfType<RadButtonElement>().ToList().ForEach(rbe => rbe.Visibility = rbe.Tag == null || Pkac.Any(i => i.RWNO == Convert.ToInt32(rbe.Tag)) ? Telerik.WinControls.ElementVisibility.Visible : Telerik.WinControls.ElementVisibility.Collapsed)));
         #endregion

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         /* Initial Sp_Barcode For Running */
         //Start_BarCode();
         /* Initial Sp_FingerPrint For Running */
         //Start_FingerPrint();         

         #region Set Localization
         var regnlang = iScsc.V_User_Localization_Forms.Where(rl => rl.FORM_NAME == GetType().Name);
         if(regnlang.Count() > 0 && regnlang.First().REGN_LANG != RegnLang)
         {
            RegnLang = regnlang.First().REGN_LANG;
            // Ready To Change Text Title
            foreach (var control in regnlang)
            {
               switch (control.CNTL_NAME.ToLower())
               {
                  case "bbi_cmanbutn":
                     bbi_cmanbutn.Caption = control.LABL_TEXT;
                     //bbi_cmanbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_cmanbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_adm1butn":
                     bbi_adm1butn.Caption = control.LABL_TEXT;
                     //bbi_adm1butn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_adm1butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_exdsbutn":
                     bbi_exdsbutn.Caption = control.LABL_TEXT;
                     //bbi_exdsbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_exdsbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "report_rbpg":
                     Report_RbPg.Text = control.LABL_TEXT;
                     //Report_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //Report_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "systemadmin_rpgc":
                     SystemAdmin_RPgc.Text = control.LABL_TEXT;
                     //SystemAdmin_RPgc.Text = control.LABL_TEXT; // ToolTip
                     //SystemAdmin_RPgc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_basdifnbutn":
                     bbi_basdifnbutn.Caption = control.LABL_TEXT;
                     //bbi_basdifnbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_basdifnbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_coptbutn":
                     bbi_coptbutn.Caption = control.LABL_TEXT;
                     //bbi_coptbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_coptbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_crglbutn":
                     bbi_crglbutn.Caption = control.LABL_TEXT;
                     //bbi_crglbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_crglbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_orgnbutn":
                     bbi_orgnbutn.Caption = control.LABL_TEXT;
                     //bbi_orgnbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_orgnbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_mbs1butn":
                     bbi_mbs1butn.Caption = control.LABL_TEXT;
                     //bbi_mbs1butn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_mbs1butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_mbfzbutn":
                     bbi_mbfzbutn.Caption = control.LABL_TEXT;
                     //bbi_mbfzbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_mbfzbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_insrbutn":
                     bbi_insrbutn.Caption = control.LABL_TEXT;
                     //bbi_insrbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_insrbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_fmdfbutn":
                     bbi_fmdfbutn.Caption = control.LABL_TEXT;
                     //bbi_fmdfbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_fmdfbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "base_rbpg":
                     Base_RbPg.Text = control.LABL_TEXT;
                     //Base_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //Base_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adm_rbpg":
                     Adm_RbPg.Text = control.LABL_TEXT;
                     //Adm_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //Adm_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "serv_rbpg":
                     Serv_RbPg.Text = control.LABL_TEXT;
                     //Serv_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //Serv_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_flstbutn":
                     bbi_flstbutn.Caption = control.LABL_TEXT;
                     //bbi_flstbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_flstbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_fdlsbutn":
                     bbi_fdlsbutn.Caption = control.LABL_TEXT;
                     //bbi_fdlsbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_fdlsbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_attnbutn":
                     bbi_attnbutn.Caption = control.LABL_TEXT;
                     //bbi_attnbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_attnbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "othroption_rbpg":
                     OthrOption_RbPg.Text = control.LABL_TEXT;
                     //OthrOption_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //OthrOption_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "system_rpgc":
                     System_RPgc.Text = control.LABL_TEXT;
                     //System_RPgc.Text = control.LABL_TEXT; // ToolTip
                     //System_RPgc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "otherincome_rpgc":
                     OtherInCome_RPgc.Text = control.LABL_TEXT;
                     //OtherInCome_RPgc.Text = control.LABL_TEXT; // ToolTip
                     //OtherInCome_RPgc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_oincbutn":
                     bbi_oincbutn.Caption = control.LABL_TEXT;
                     //bbi_oincbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_oincbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_oexpbutn":
                     bbi_oexpbutn.Caption = control.LABL_TEXT;
                     //bbi_oexpbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_oexpbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_cpaybutn":
                     bbi_cpaybutn.Caption = control.LABL_TEXT;
                     //bbi_cpaybutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_cpaybutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "agractn_rpgc":
                     AgrActn_RPgc.Text = control.LABL_TEXT;
                     //AgrActn_RPgc.Text = control.LABL_TEXT; // ToolTip
                     //AgrActn_RPgc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "agroption_rbpg":
                     AgrOption_RbPg.Text = control.LABL_TEXT;
                     //AgrOption_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //AgrOption_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_smsbbutn":
                     bbi_smsbbutn.Caption = control.LABL_TEXT;
                     //bbi_smsbbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_smsbbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_agmbbutn":
                     bbi_agmbbutn.Caption = control.LABL_TEXT;
                     //bbi_agmbbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_agmbbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_agcbbutn":
                     bbi_agcbbutn.Caption = control.LABL_TEXT;
                     //bbi_agcbbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_agcbbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_aglsbutn":
                     bbi_aglsbutn.Caption = control.LABL_TEXT;
                     //bbi_aglsbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_aglsbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_agmcbutn":
                     bbi_agmcbutn.Caption = control.LABL_TEXT;
                     //bbi_agmcbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_agmcbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_incmbutn":
                     bbi_incmbutn.Caption = control.LABL_TEXT;
                     //bbi_incmbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_incmbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_bislbutn":
                     bbi_bislbutn.Caption = control.LABL_TEXT;
                     //bbi_bislbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_bislbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_bkupbutn":
                     bbi_bkupbutn.Caption = control.LABL_TEXT;
                     //bbi_bkupbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_bkupbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_rstrbutn":
                     bbi_rstrbutn.Caption = control.LABL_TEXT;
                     //bbi_rstrbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_rstrbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "bbi_embkbutn":
                     bbi_embkbutn.Caption = control.LABL_TEXT;
                     //bbi_embkbutn.Text = control.LABL_TEXT; // ToolTip
                     //bbi_embkbutn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "base_rbgp":
                     Base_RbGp.Text = control.LABL_TEXT;
                     //Base_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //Base_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "adm_rbgp":
                     Adm_RbGp.Text = control.LABL_TEXT;
                     //Adm_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //Adm_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "serv_rbgp":
                     Serv_RbGp.Text = control.LABL_TEXT;
                     //Serv_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //Serv_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "othrexpnoption_rbgp":
                     OthrExpnOption_RbGp.Text = control.LABL_TEXT;
                     //OthrExpnOption_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //OthrExpnOption_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "agrsmsoption_rbgp":
                     AgrSmsOption_RbGp.Text = control.LABL_TEXT;
                     //AgrSmsOption_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //AgrSmsOption_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "agroproption_rbgp":
                     AgrOprOption_RbGp.Text = control.LABL_TEXT;
                     //AgrOprOption_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //AgrOprOption_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mngr_rpgc":
                     Mngr_RPgc.Text = control.LABL_TEXT;
                     //Mngr_RPgc.Text = control.LABL_TEXT; // ToolTip
                     //Mngr_RPgc.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "report_rbgp":
                     Report_RbGp.Text = control.LABL_TEXT;
                     //Report_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //Report_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "systemadmin_rbpg":
                     SystemAdmin_RbPg.Text = control.LABL_TEXT;
                     //SystemAdmin_RbPg.Text = control.LABL_TEXT; // ToolTip
                     //SystemAdmin_RbPg.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "systemadmin_rbgp":
                     SystemAdmin_RbGp.Text = control.LABL_TEXT;
                     //SystemAdmin_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //SystemAdmin_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "othroption_rbgp":
                     OthrOption_RbGp.Text = control.LABL_TEXT;
                     //SystemAdmin_RbGp.Text = control.LABL_TEXT; // ToolTip
                     //SystemAdmin_RbGp.Text = control.LABL_TEXT; // Place Holder
                     break;

                  default:
                    break;
               }
            }
         }
         #endregion

         // 1403/02/21
         Main_Pnl.Controls.OfType<MaxUi.Rollout>().ToList().ForEach(r => r.RolloutStatus = (r.Tag == null || r.Tag.ToString() == "") ? false : true);
         Stng_Pnl.Controls.OfType<MaxUi.Rollout>().ToList().ForEach(r => r.RolloutStatus = (r.Tag == null || r.Tag.ToString() == "") ? false : true);

         // 1404/01/24 * Get subsystem version
         SubVerNo_Txt.EditValue = iScsc.V_SubSies.FirstOrDefault().VERS_NO;

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
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
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
         //Stop_BarCode();
         //Stop_FingerPrint();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         // 1401/12/18 * Remove Wallpaper for Arta System
         //try { spc_desktopnew.Panel1.BackgroundImage = Image.FromFile(@".\Media\SubSys\Scsc\Desktop\Wallpaper\Wallpaper.jpg"); }
         //catch { }
         
         //FighBs.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         if (isFirstLoaded) goto finishcommand;
         isFirstLoaded = true;

         // Play Enter Sound
         // 1404/03/30 ** New version for play sound
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "MAIN_PAGE_F", 44 /* PlaySystemSound */, SendType.SelfToUserInterface)
            {
               Input = new XElement("Sound", new XAttribute("type", "001"))
            }
         );

         DAeatBs.DataSource = iScsc.D_AEATs;
         DevntBs.DataSource = iScsc.D_EVNTs;
         DAtsmBs1.DataSource = iScsc.D_ATSMs;
         DActvBs1.DataSource = iScsc.D_ACTVs;
         DbcdtBs1.DataSource = iScsc.D_BCDTs;         
         DysnoBs.DataSource = iScsc.D_YSNOs;         
         DSondBs.DataSource = iScsc.D_SONDs;
         

         finishcommand:
         VCompBs.DataSource = iScsc.V_Computers;
         ANoteBs.DataSource = iScsc.App_Base_Defines.Where(a => a.ENTY_NAME == "Note_Tag_Info");
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         UserBs.DataSource = iScsc.V_Users;
         Execute_Query();
         FormHandle = this.Handle;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void PostOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;

            if(InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Dock = DockStyle.Fill;
                  c.Visible = true;
                  // 1401/08/07 * پایان حکومت آخوندی ضحاک خامنه ای حروم زاده
                  if (spc_desktopnew.Panel1.Controls.IndexOf(c) == -1)
                     spc_desktopnew.Panel1.Controls.Add(c);
                  spc_desktopnew.Panel1.Controls.SetChildIndex(c, 0);
               }), obj);
            else
            {
               obj.Dock = DockStyle.Fill;
               obj.Visible = true;
               // 1401/08/07 * پایان حکومت آخوندی ضحاک خامنه ای حروم زاده
               if (spc_desktopnew.Panel1.Controls.IndexOf(obj) == -1)
                  spc_desktopnew.Panel1.Controls.Add(obj);
               spc_desktopnew.Panel1.Controls.SetChildIndex(obj, 0);
            }

         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void TakeOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c =>
               {
                  if (spc_desktopnew.Panel1.Controls.IndexOf(c) >= 0)
                  {
                     //spc_desktopnew.Panel1.Controls.Remove(c);
                     c.Visible = false;
                     //spc_desktopnew.Panel1.Controls.SetChildIndex(spc_desktopnew.Panel1.Controls[1], 0);
                  }
               }
            ), obj);
            this.Focus();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Successful; UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c =>
               {
                  spc_desktopnew.Panel1.Refresh();
                  if (spc_desktopnew.Panel1.Controls.IndexOf(c) >= 0)
                  {
                     c.Visible = false;
                     //spc_desktopnew.Panel1.Controls.Remove(c);
                  }
               }
               ), obj);
            this.Focus();
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;

         //if (iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.NOTF_STAT == "002" && (s.NOTF_VIST_DATE.HasValue ? s.NOTF_VIST_DATE.Value : DateTime.Now.AddDays(-1)) != DateTime.Now).Count() >= 1)
         //{
         //   var expday = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE) && s.NOTF_STAT == "002" && (s.NOTF_VIST_DATE.HasValue ? s.NOTF_VIST_DATE.Value : DateTime.Now.AddDays(-1)) != DateTime.Now).ToList();
         //   expday.ForEach(s => s.NOTF_VIST_DATE = DateTime.Now);
         //   iScsc.SubmitChanges();

         //   Job _InteractWithScsc =
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */),
         //         new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "endfigh"), new XAttribute("expday", expday.Max(s => s.NOTF_EXP_DAY)))}
         //      });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}

         if(job.Input != null && (job.Input as XElement).Attribute("type").Value == "accesscontrol")
         {
            try
            {
               if ((job.Input as XElement).Attribute("attnsystype") != null)
                  attnsystype = (job.Input as XElement).Attribute("attnsystype").Value;
               else
                  attnsystype = "002";
               OnAttTransactionEx((job.Input as XElement).Attribute("fngrprnt").Value);
               attnsystype = "002";
            }
            catch { }
         }
         else if (job.Input != null && (job.Input as XElement).Attribute("type").Value == "gatecontrol")
         {
            if((job.Input as XElement).Attribute("gateactn").Value == "open")
            {
               Open_Gate(job.Input as XElement);
            }
            else if ((job.Input as XElement).Attribute("gateactn").Value == "close")
            {
               Close_Gate(job.Input as XElement);
            }
            else if((job.Input as XElement).Attribute("gateactn").Value == "error")
            {
               Error_Gate(job.Input as XElement);
            }
         }
         else if (job.Input != null && (job.Input as XElement).Attribute("type").Value == "fngrprntdev")
         {
            if ((job.Input as XElement).Attribute("fngractn").Value == "enroll")
            {
               Enroll_Finger((job.Input as XElement).Attribute("fngrprnt").Value);
            }
            else if ((job.Input as XElement).Attribute("fngractn").Value == "delete")
            {
               Delete_Finger((job.Input as XElement).Attribute("fngrprnt").Value);
            }
            else if ((job.Input as XElement).Attribute("fngractn").Value == "truncate")
            {
               Truncate_Finger();
            }
         }
         else if (job.Input != null && (job.Input as XElement).Attribute("type").Value == "zktfngrprnt")
         {
            if ((job.Input as XElement).Attribute("fngractn").Value == "enroll")
            {
               //Enroll_Finger((job.Input as XElement).Attribute("fngrprnt").Value);
               ZktFpAttnStat = "enroll";
            }
            else if ((job.Input as XElement).Attribute("fngractn").Value == "attendance")
            {
               //Truncate_Finger();
               ZktFpAttnStat = "attendance";
            }
            else if ((job.Input as XElement).Attribute("fngractn").Value == "delete")
            {
               //Delete_Finger((job.Input as XElement).Attribute("fngrprnt").Value);
            }
            else if ((job.Input as XElement).Attribute("fngractn").Value == "truncate")
            {
               //Truncate_Finger();
            }            
         }
         else if (job.Input != null && (job.Input as XElement).Attribute("type").Value == "sendoprtdres")
         {
            //var portName = (job.Input as XElement).Attribute("portname").Value;
            var cmndname = (job.Input as XElement).Attribute("cmndname").Value;
            var devIP = (job.Input as XElement).Attribute("devip").Value;
            var cmndsend = (job.Input as XElement).Attribute("cmndsend").Value;

            SendOprtDresser(devIP, cmndname, cmndsend);
         }
         else if(job.Input != null && (job.Input as XElement).Attribute("type").Value == "extdev")
         {
            OprtExtDev(job.Input as XElement);
         }
         else if (job.Input != null && (job.Input as XElement).Attribute("type").Value == "expnextr")
         {
            string devName = "";
            if (xinput.Attribute("macadrs") != null)
               devName = xinput.Attribute("macadrs").Value;
            else
            {
               var hasDevName = iScsc.External_Devices.FirstOrDefault(d => d.EXPN_CODE == (job.Input as XElement).Attribute("expncode").Value.ToInt64());
               if(hasDevName != null)
                  devName = iScsc.External_Devices.FirstOrDefault(d => d.EXPN_CODE == (job.Input as XElement).Attribute("expncode").Value.ToInt64()).DEV_NAME;
            }
            var cmndText = (job.Input as XElement).Attribute("cmndtext").Value;
            var fngrprnt = (job.Input as XElement).Attribute("fngrprnt").Value;

            if(devName != null && devName != "")
               SendCommandDevExpn(cmndText, devName, fngrprnt);
         }
         else if (xinput != null && xinput.Attribute("type").Value == "alarmshow")
         {
            new Thread(AlarmShow).Start();
         }
         else if(xinput != null && xinput.Attribute("type").Value == "fngrdevlst")
         {
            EdevFngr_Rlt.RolloutStatus = true;
            FngrPrntOpr_Txt.Text = xinput.Attribute("fngrprnt").Value;
            FngrPrntOpr_Txt.Focus();
         }
         else if (xinput != null && xinput.Attribute("type").Value == "getwletamnt")
         {
            CWlet_Pb_Click(null, null);

            if(CWlet_Pb.Visible)
            {
               job.Output = CWlet_Txt.EditValue;
            }
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void SetToolTip(Job job)
      {
         Lbs_Tooltip.Text = job.Input.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 41
      /// </summary>
      /// <param name="job"></param>
      private void SetCardinDevice(Job job)
      {
         //if (Fp1DevIsConnected == false)
         //{
         //   //MessageBox.Show("Please connect the device first!", "Error");
         //   return;
         //}

         if (!Fp1DevIsConnected) return;

         string EnrollNumber, CardNumber, NameDnrm;
         EnrollNumber = (job.Input as XElement).Attribute("enrollnumb").Value;
         CardNumber = (job.Input as XElement).Attribute("cardnumb").Value;
         NameDnrm = (job.Input as XElement).Attribute("namednrm").Value;

         int idwErrorCode = 0;

         bool bEnabled = true;
         int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
         Cursor = Cursors.WaitCursor;

         if (Fp1DevIsConnected)
         {
            axCZKEM1.EnableDevice(iMachineNumber, false);

            axCZKEM1.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
            {
               //MessageBox.Show("کارت شما در سیستم ثبت گردید");
               //FngrPrnt_Txt.Text = EnrollNumber;
               //if (CardNumb_Text.Text == "") CardNumb_Text.Text = "0";
               //CardNumb_Text.Text = (Convert.ToInt64(CardNumb_Text.Text) + 1).ToString();
               //MessageBox.Show(string.Format("FngrPrnt {0} CardNumb {1} NameDnrm {2}", EnrollNumber, CardNumber, NameDnrm));
            }
            else
            {
               axCZKEM1.GetLastError(ref idwErrorCode);
               MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM1.EnableDevice(iMachineNumber, true);
         }
         // Copy on 2nd fingerprint device
         if(Fp2DevIsConnected)
         {
            axCZKEM2.EnableDevice(iMachineNumber, false);

            axCZKEM2.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM2.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
            {
            }
            else
            {
               axCZKEM2.GetLastError(ref idwErrorCode);
               MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM2.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM2.EnableDevice(iMachineNumber, true);
         }
         if (Fp3DevIsConnected)
         {
            axCZKEM3.EnableDevice(iMachineNumber, false);

            axCZKEM3.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM3.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
            {
            }
            else
            {
               axCZKEM3.GetLastError(ref idwErrorCode);
               MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM3.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM3.EnableDevice(iMachineNumber, true);
         }
         if (Fp4DevIsConnected)
         {
            axCZKEM4.EnableDevice(iMachineNumber, false);

            axCZKEM4.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM4.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
            {
            }
            else
            {
               axCZKEM4.GetLastError(ref idwErrorCode);
               MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM4.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM4.EnableDevice(iMachineNumber, true);
         }
         if (Fp5DevIsConnected)
         {
            axCZKEM5.EnableDevice(iMachineNumber, false);

            axCZKEM5.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM5.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
            {
            }
            else
            {
               axCZKEM5.GetLastError(ref idwErrorCode);
               MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM5.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM5.EnableDevice(iMachineNumber, true);
         }
         if (Fp6DevIsConnected)
         {
            axCZKEM6.EnableDevice(iMachineNumber, false);

            axCZKEM6.SetStrCardNumber(CardNumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM6.SSR_SetUserInfo(iMachineNumber, EnrollNumber, NameDnrm, "", 0, bEnabled))//upload the user's information(card number included)
            {}
            else
            {
               axCZKEM6.GetLastError(ref idwErrorCode);
               MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
            axCZKEM6.RefreshData(iMachineNumber);//the data in the device should be refreshed
            axCZKEM6.EnableDevice(iMachineNumber, true);
         }

         Cursor = Cursors.Default;
      }

      /// <summary>
      /// Code 42
      /// </summary>
      /// <param name="job"></param>
      private void ShowMessageOnLCD(Job job)
      {
         var message = (job.Input as XElement).Value;
         // بررسی اینکه آیا دستگاه انگشتی متصل می باشد
         if (Fp1DevIsConnected)
         {
            //axCZKEM1.ClearLCD();
            //axCZKEM1.WriteLCD(0, 0, message);
            goto L_End;
         }

         // بررسی اینکه آیا دستگاه بارکد خوان متصل می باشد
         //if (Sp_Barcode.IsOpen)
         //{
         //   Sp_Barcode.WriteLine(message);
         //   goto L_End;
         //}

         L_End:
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 43
      /// </summary>
      /// <param name="job"></param>
      private void DeviceControlFunction(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            // بررسی اینکه آیا دستگاه انگشتی متصل می باشد
            if (Fp1DevIsConnected || Fp2DevIsConnected || Fp3DevIsConnected || Fp4DevIsConnected || Fp5DevIsConnected || Fp6DevIsConnected)
            {
               var result = false;
               int iMachineNumber = 1;//In fact,when you are using the tcp/ip communication,this parameter will be ignored,that is any integer will all right.Here we use 1.
               switch (xinput.Attribute("functype").Value)
               {
                  case "5.5.1": // ClearAdministrators
                     result = axCZKEM1.ClearAdministrators(iMachineNumber);
                     if (Fp2DevIsConnected) result = axCZKEM2.ClearAdministrators(iMachineNumber);
                     if (Fp3DevIsConnected) result = axCZKEM3.ClearAdministrators(iMachineNumber);
                     if (Fp4DevIsConnected) result = axCZKEM4.ClearAdministrators(iMachineNumber);
                     if (Fp5DevIsConnected) result = axCZKEM5.ClearAdministrators(iMachineNumber);
                     if (Fp6DevIsConnected) result = axCZKEM6.ClearAdministrators(iMachineNumber);

                     // Play Enter Sound
                     // 1404/03/30 ** New version for play sound
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "MAIN_PAGE_F", 44 /* PlaySystemSound */, SendType.SelfToUserInterface)
                        {
                           Input = new XElement("Sound", new XAttribute("type", "026"))
                        }
                     );
                     break;
                  case "5.2.3.5":
                     result = Delete_Enroll_Finger(xinput.Attribute("enrollnumb").Value);
                     break;
                  case "5.2.3.8":                     
                     result = Start_Enroll_Finger(xinput.Attribute("enrollnumb").Value);
                     break;
                  case "5.2.7.1":
                     result = Truncate_Enroll_Fingers();
                     break;
                  case "5.2.7.2":
                     result = Duplicate_Enroll_Fingers(xinput.Attribute("enrollnumb").Value);
                     break;
                  case "5.2.3.8.0":
                     result = SetPassword_Enroll_Finger(xinput.Attribute("enrollnumb").Value, xinput.Attribute("password").Value);
                     break;
                  case "5.2.3.8.1":
                     result = Start_Enroll_Face(xinput.Attribute("enrollnumb").Value);
                     break;
                  case "5.2.3.8.2":
                     result = Delete_Enroll_Face(xinput.Attribute("enrollnumb").Value);
                     break;
                  case "get":
                     job.Output = GetDataFromDev(xinput.Attribute("enrollnumb").Value);
                     break;
                  case "set":
                     job.Output = SetDataToDev(xinput.Attribute("enrollnumb").Value, new List<string> { xinput.Attribute("fngrprntupdate").Value, xinput.Attribute("fngrprnt").Value, xinput.Attribute("faceupdate").Value, xinput.Attribute("face").Value });
                     break;
               }
               //if (result) MessageBox.Show(this, "عملیات با موفقیت انجام شد", "نتجیه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Information);
               //else
               //{
               //   MessageBox.Show(this, "عملیات با شکست مواجه شد", "نتجیه عملیات", MessageBoxButtons.OK, MessageBoxIcon.Error);
               //}
               //goto L_End;
            }
         }
         catch { }
         //L_End:
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 44
      /// </summary>
      /// <param name="job"></param>
      private void PlaySystemSound(Job job)
      {         
         try
         {
            job.Status = StatusType.Successful;
            ///
            /// <Sound type="001"/>
            ///
            var _sondType = ((XElement)job.Input).Attribute("type").Value;

            // Find sound file should be to play with os
            // 1st find schema profile
            var _schp = iScsc.Schema_Profiles.Where(sp => sp.SCHM_BY == CurrentUser || sp.DFLT_STAT == "002").OrderByDescending(sp => sp.CRET_DATE).FirstOrDefault();
            if (_schp == null) return;
            
            // 2nd find sound type
            var _sond = _schp.Sounds.FirstOrDefault(s => s.SOND_TYPE == _sondType);
            if (_sond == null) return;

            // 3rd if sound must be active and set file path
            if (_sond.STAT == "001" || _sond.SOND_PATH == null || _sond.SOND_PATH == "") return;

            // At last play sound
            _wplayer_url = _sond.SOND_PATH;
            new Thread(AlarmShow).Start();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      /// <summary>
      /// Code 1000
      /// </summary>
      /// <param name="job"></param>
      private void Call_SystemService_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if (xinput == null) return;

            var chatid = xinput.Attribute("chatid").Value.ToInt64();
            var cmnd = xinput.Attribute("cmnd").Value;
            var param = xinput.Attribute("param").Value;

            // اگر اتفاق جدیدی درون سیستم پایگاه داده رخ داده باشد بتوانیم آخرین اطلاعات را بازیابی کنیم
            iScsc = new Data.iScscDataContext(ConnectionString);
            var fighs = iScsc.Fighters.Where(f => f.CHAT_ID_DNRM == @chatid);

            if (fighs == null || fighs.Count() == 0)
            {
               job.Output =
                  new XElement("Output",
                     new XAttribute("resultcode", -10001),
                     new XAttribute("resultdesc", "مشتری با کد شما قابل شناسایی نیست"),
                     new XAttribute("mesgtype", "1"),
                     new XAttribute("mesgdesc", "Text")
                  );
               job.Status = StatusType.Successful;
               return;
            }
            else if (fighs.Count() > 1)
            {
               job.Output =
                  new XElement("Output",
                     new XAttribute("resultcode", -10002),
                     new XAttribute("resultdesc", "با کد دستگاه شما *بیش از یک مشتری* وجود دارد، لطفا با قسمت پذیرش صحبت کنید"),
                     new XAttribute("mesgtype", "1"),
                     new XAttribute("mesgdesc", "Text")
                  );
               job.Status = StatusType.Successful;
               return;
            }

            var figh = fighs.FirstOrDefault();

            // 1402/08/13 * قفل انلاین
            if(cmnd == "opendres-tryopen")
            {
               if (RSignalOpen_Butn.Tag == null)
               {
                  RSignalOpen_Butn.Tag = "key";
                  LSignalOpen_Butn.Tag = null;
                  _wplayer_url = "";
                  new Thread(new ThreadStart(() => AlarmShow(RSignalOpen_Butn))).Start();
               }
               else
               {
                  RSignalOpen_Butn.Tag = null;
                  LSignalOpen_Butn.Tag = "key";
                  _wplayer_url = "";
                  new Thread(new ThreadStart(() => AlarmShow(LSignalOpen_Butn))).Start();
               }

               Partners_Butn.Visible = PrtnrCont_Butn.Visible = PrtnrPos_Butn.Visible = PartnerDresNum_Butn.Visible = false;
               PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = PartnerDresNum_Butn.Visible = false;
               var _attn = iScsc.Attendances.FirstOrDefault(a => a.FIGH_FILE_NO == figh.FILE_NO && a.EXIT_TIME == null && a.ATTN_STAT == "002" && a.ATTN_DATE.Date == DateTime.Now.Date);
               if(_attn != null)
               {
                  // ثبت ساعت باز کردن کمد            
                  iScsc.INS_DART_P(_attn.CODE, null, null);

                  // اینجا باید شماره سریال پورت را پیدا کنیم و پیام را بهش ارسال کنیم
                  var dresrattn = iScsc.Dresser_Attendances.FirstOrDefault(da => da.ATTN_CODE == _attn.CODE && da.DRAT_CODE == null);

                  cmnd = "opendres";
                  var _dres = iScsc.Dressers.FirstOrDefault(d => d.Computer_Action.COMP_NAME == xHost.Attribute("name").Value && d.REC_STAT == "002" && d.CODE == dresrattn.Dresser.CODE);
                  param = string.Format("{0},{1},{2},{3}", _dres.CODE, _dres.DRES_NUMB, _dres.IP_ADRS, _dres.CMND_SEND);

                  // 1402/10/21 * باز کردن کمدهای همراهان
                  if (iScsc.Dresser_Attendances.Any(da => da.ATTN_CODE == _attn.CODE && da.DRAT_CODE != null))
                  {                     
                     Partners_Butn.Visible = PartnerDresNum_Butn.Visible = PrtnrPos_Butn.Visible = true;
                     PrtnrCont_Butn.Visible = PrtnrProc1_Pbc.Visible = PrtnrProc2_Pbc.Visible = PrtnrProc3_Pbc.Visible = true;
                     new Thread(new ThreadStart(() => OpenDresPart_Tmr_Tick(_attn.CODE))).Start();
                  }
               }
            }

            bool result = false;
            switch (cmnd.ToLower())
            {
               case "startenroll":
                  result = Start_Enroll_Finger(figh.FNGR_PRNT_DNRM);
                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10001),
                        new XAttribute("resultdesc", "لطفا *☝️ انگشت خود* را *سه مرتبه* به صورت *متوالی* در قسمت سنسور دستگاه قرار دهید."),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "deleteenroll":
                  result = Delete_Enroll_Finger(figh.FNGR_PRNT_DNRM);
                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10002),
                        new XAttribute("resultdesc", "اثر انگشت شما از سیستم اثر انگشتی *پاک* شد"),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "attn":
                  Job _InteractWithScsc =
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 88 /* Execute Ntf_Totl_F */){Input = new XElement("Request", new XAttribute("actntype", "JustRunInBackground"))},
                           new Job(SendType.SelfToUserInterface, "NTF_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "attn"), new XAttribute("enrollnumber", figh.FNGR_PRNT_DNRM), new XAttribute("mbsprwno", param.Split(',')[1]), new XAttribute("chckattnalrm", "002"))}
                        });
                  _DefaultGateway.Gateway(_InteractWithScsc);

                  // نمایش اطلاعات ورود و خروج
                  var attn = iScsc.Attendances.Where(a => a.FNGR_PRNT_DNRM == figh.FNGR_PRNT_DNRM && a.ATTN_DATE == DateTime.Now.Date && a.MBSP_RWNO_DNRM == param.Split(',')[1].ToInt16()).OrderByDescending(a => a.CRET_DATE).FirstOrDefault();
                  if(attn == null)
                  {
                     job.Output =
                        new XElement("Output",
                           new XAttribute("resultcode", -10003),
                           new XAttribute("resultdesc", "ورود و خروج شما با خطا مواجه شد"),
                           new XAttribute("mesgtype", "1"),
                           new XAttribute("mesgdesc", "Text")
                        );
                  }
                  else
                  {
                     string resultdesc = "";
                     if (attn.EXIT_TIME == null)
                        resultdesc = 
                           string.Format("{0} {1} {2}",
                              "📥 ورود شما در ساعت",
                              attn.ENTR_TIME.Value.ToString().Substring(0, 5),
                              "ثبت شد"                              
                           );
                     else
                        resultdesc =
                           string.Format("{0} {1} {2}",
                              "📤 خروج شما در ساعت",
                              attn.EXIT_TIME.Value.ToString().Substring(0, 5),
                              "ثبت شد"
                           );

                     job.Output =
                        new XElement("Output",
                           new XAttribute("resultcode", 10003),
                           new XAttribute("resultdesc", resultdesc),
                           new XAttribute("mesgtype", "1"),
                           new XAttribute("mesgdesc", "Text")
                        );
                  }
                  break;
               case "openrelaygate":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("type", "gatecontrol"),
                                    new XAttribute("gateactn", "open")
                                 )
                           }
                        }
                     )
                  );
                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", "درخواست باز شدن گیت انجام شد"),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "closerelaygate":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("type", "gatecontrol"),
                                    new XAttribute("gateactn", "close")
                                 )
                           }
                        }
                     )
                  );
                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", "درخواست بسته شدن گیت انجام شد"),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "opendres":                  
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           //new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                           new Job(SendType.SelfToUserInterface, GetType().Name, 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("type", "sendoprtdres"),
                                    new XAttribute("cmndname", param.Split(',')[1]),
                                    new XAttribute("devip", param.Split(',')[2]),
                                    new XAttribute("cmndsend", param.Split(',')[3])
                                 )
                           }
                        }
                     )
                  );

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", string.Format("درخواست باز شدن قفل کمد شماره {0} انجام شد", param.Split(',')[1])),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "gameclub::slctexpn":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("appcmndtype", "gameclub::slctexpn"),
                                    new XAttribute("epitcode", param.Split(',')[0]),
                                    new XAttribute("extpcode", param.Split(',')[1]),
                                    new XAttribute("expncode", param.Split(',')[2])
                                 )
                           }
                        }
                     )
                  );

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", string.Format("درخواست باز شدن میز به سیستم ارسال شد")),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "gameclub::closopenexpn":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("appcmndtype", "gameclub::closopenexpn"),
                                    new XAttribute("agopcode", param.Split(',')[0]),
                                    new XAttribute("rwno", param.Split(',')[1])
                                 )
                           }
                        }
                     )
                  );

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", string.Format("درخواست بسته شدن میز به سیستم ارسال شد")),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "gameclub::closcashexpn":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("appcmndtype", "gameclub::closcashexpn"),
                                    new XAttribute("agopcode", param.Split(',')[0]),
                                    new XAttribute("rwno", param.Split(',')[1])
                                 )
                           }
                        }
                     )
                  );

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", string.Format("درخواست تسویه حساب نقدی میز(ها) به سیستم ارسال شد")),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               case "gameclub::reopenexpn":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "AOP_BUFE_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("MainPage",
                                    new XAttribute("appcmndtype", "gameclub::reopenexpn"),
                                    new XAttribute("agopcode", param.Split(',')[0]),
                                    new XAttribute("rwno", param.Split(',')[1])
                                 )
                           }
                        }
                     )
                  );

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10004),
                        new XAttribute("resultdesc", string.Format("درخواست باز و بسته کردن میز به سیستم ارسال شد")),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  break;
               default:
                  break;
            }

            job.Status = StatusType.Successful;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}

