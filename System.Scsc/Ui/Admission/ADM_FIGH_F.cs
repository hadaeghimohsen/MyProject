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
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.Scsc.ExtCode;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.Admission
{
   public partial class ADM_FIGH_F : UserControl
   {
      public ADM_FIGH_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int rqstindex = default(int);
      private long? cbmtcode = null, ctgycode = null;

      private void Execute_Query()
      {
         setOnDebt = false;
         try
         {
            if (true)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);

               rqstindex = RqstBs1.Position;

               var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "001" &&
                        rqst.RQST_STAT == "001" &&
                        (rqst.RQTT_CODE == "001" || rqst.RQTT_CODE == "004" || rqst.RQTT_CODE == "005" || rqst.RQTT_CODE == "006") &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

               RqstBs1.DataSource =
                  iScsc.Requests
                  .Where(
                     rqst =>
                        Rqids.Contains(rqst.RQID)
                  )
                  .OrderByDescending(
                     rqst =>
                        rqst.RQST_DATE
                  );

               RqstBs1.Position = rqstindex;

               // 1401/05/21 * Clear Advertising Campaing Items
               //AdvpBs1.List.Clear();
               Cbmt_Gv.ActiveFilterString = "";
            }
         }
         catch { }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;

            if (_rqst.SSTT_MSTT_CODE == 2 && (_rqst.SSTT_CODE == 1 || _rqst.SSTT_CODE == 2 || _rqst.SSTT_CODE == 3))
            {
               Gb_Expense.Visible = true;
               //Btn_RqstDelete1.Visible = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;
            }
            else if (!(_rqst.SSTT_MSTT_CODE == 2 && (_rqst.SSTT_CODE == 1 || _rqst.SSTT_CODE == 2 || _rqst.SSTT_CODE == 3)) && _rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnASav1.Enabled = true;
            }
            else if (_rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               //DefaultTabPage001();
               RqstBnASav1.Enabled = false;
               RQTT_CODE_LookUpEdit1.EditValue = "001";
               RefDesc_Txt.EditValue = "";
            }

            var _mbsp = _rqst.Request_Rows.FirstOrDefault().Member_Ships.FirstOrDefault();
            if (_mbsp != null)
            {
               Hldy_gv.ActiveFilterString =
                  string.Format("Hldy_Date >= #{0}# AND Hldy_Date <= #{1}#", _mbsp.STRT_DATE.Value.ToShortDateString(), _mbsp.END_DATE.Value.ToShortDateString());
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            //DefaultTabPage001();
            RqstBnASav1.Enabled = false;
            RQTT_CODE_LookUpEdit1.EditValue = "001";
            RefDesc_Txt.EditValue = "";
         }
      }

      private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            rqstindex = RqstBs1.Position;

            if (FNGR_PRNT_TextEdit.Text == "") { 
               if(MessageBox.Show(this, "کد شناسایی خالی میباشد آیا مایل به ایجاد کد پیش فرض هستید؟", "هشدار کد شناسایی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes) 
                  MaxF_Butn001_Click(null, null); 
               else
               {
                  FNGR_PRNT_TextEdit.Focus();
                  return;
               }
            }

            if (RQTT_CODE_LookUpEdit1.EditValue == null || RQTT_CODE_LookUpEdit1.EditValue.ToString() == "")
               RQTT_CODE_LookUpEdit1.EditValue = "001";

            if (Rqst == null || Rqst.RQID >= 0)
            {
               
               {
                  //fighterGridControl.Visible = false;
                  /*
                   * ثبت مشتریی جدید در سیستم
                   */
                  StrtDate_DateTime001.CommitChanges();
                  EndDate_DateTime001.CommitChanges();

                  if (!StrtDate_DateTime001.Value.HasValue) { StrtDate_DateTime001.Focus(); return; }
                  if (!EndDate_DateTime001.Value.HasValue) { EndDate_DateTime001.Focus(); return; }

                  if (StrtDate_DateTime001.Value.Value.Date > EndDate_DateTime001.Value.Value.Date)
                  {
                     throw new Exception("تاریخ شروع باید از تاریخ پایان کوچکتر با مساوی باشد");
                  }

                  if (NumbOfMontDnrm_TextEdit001.Text.Trim() == "")
                     NumbOfMontDnrm_TextEdit001.Text = "1";
                  
                  // 1400/04/25 * بررسی اینکه جنسیت مشتری در کلاس ثبت نامی درست میباشد یا خیر
                  if(CbmtBs1.List.OfType<Data.Club_Method>().Any(c => c.CODE == (long)CbmtCode_Lov.EditValue && c.SEX_TYPE != "003" && c.SEX_TYPE != SEX_TYPE_LookUpEdit.EditValue.ToString()))
                  {
                     if (MessageBox.Show(this, "جنسیت مشتری در گروه ثبت نامی قابل قبول نمیباشد، آیا با ثبت مشتری موافق هستید؟ در غیر اینصورت اطلاعات را اصلاح فرمایید", "عدم تطابق جنسیت در گروه ثبت نامی", MessageBoxButtons.YesNo) != DialogResult.Yes) { SEX_TYPE_LookUpEdit.Focus(); return; }
                  }

                  // 1400/06/08 * بررسی اینکه تعداد جلسات به نرخ تعرفه درست انتخاب شده یا خیر
                  if (!CtgyBs1.List.OfType<Data.Category_Belt>().Any(c => c.CODE == Convert.ToInt64(CtgyCode_Lov.EditValue) && c.NUMB_OF_ATTN_MONT == Convert.ToInt32(NumbOfAttnMont_TextEdit001.Text)) &&
                     MessageBox.Show(this, "اطلاعات ورودی با اطلاعات آیین نامه مغایرت دارد، آیا نیاز به اصلاح کردن اطلاعات را دارید؟", "مغایرت اطلاعات آیین نامه با اطلاعات ورودی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                  {
                     NumbOfAttnMont_TextEdit001.Focus();
                     return;
                  }

                  if (Rqst == null || Rqst.RQST_STAT == null || Rqst.RQST_STAT == "001")
                     iScsc.ADM_TRQT_F(
                           new XElement("Process",
                              new XElement("Request",
                                 new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                                 new XAttribute("rqtpcode", "001"),
                                 new XAttribute("rqttcode", RQTT_CODE_LookUpEdit1.EditValue),
                                 //new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
                                 //new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
                                 new XElement("Fighter",
                                    new XAttribute("fileno", Rqst == null ? 0 : Rqst.Fighters.FirstOrDefault() == null ? 0 : Rqst.Fighters.FirstOrDefault().FILE_NO),
                                    new XElement("Frst_Name", FRST_NAME_TextEdit.Text.Trim()),
                                    new XElement("Last_Name", LAST_NAME_TextEdit.Text.Trim()),
                                    new XElement("Fath_Name", FATH_NAME_TextEdit.Text.Trim()),
                                    new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue),
                                    new XElement("Natl_Code", new XAttribute("chckvald", NatlCodeVald_Cbx.Checked), NATL_CODE_TextEdit.Text.Trim()),
                                    new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                                    new XElement("Cell_Phon", CELL_PHON_TextEdit.Text.Trim()),
                                    new XElement("Tell_Phon", TELL_PHON_TextEdit.Text.Trim()),
                                    new XElement("Type", RQTT_CODE_LookUpEdit1.EditValue),
                                    new XElement("Post_Adrs", POST_ADRS_TextEdit.Text),
                                    new XElement("Emal_Adrs", ""),
                                    new XElement("Insr_Numb", (iNSR_NUMBTextEdit.Text ?? "").ToString().Trim()),
                                    new XElement("Insr_Date", iNSR_DATEPersianDateEdit.Value == null ? "" : iNSR_DATEPersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                                    new XElement("Educ_Deg", ""),
                                    new XElement("Cbmt_Code", CbmtCode_Lov.EditValue ?? ""),
                                    new XElement("Dise_Code", ""),
                                    new XElement("Blod_Grop", ""),
                                    new XElement("Fngr_Prnt", (FNGR_PRNT_TextEdit.EditValue ?? "").ToString().Trim()),
                                    new XElement("Sunt_Bunt_Dept_Orgn_Code", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Sunt_Bunt_Dept_Code", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Sunt_Bunt_Code", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Sunt_Code", SUNT_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Cord_X", ""),
                                    new XElement("Cord_Y", ""),
                                    new XElement("Glob_Code", (Glob_Code_TextEdit.EditValue ?? "").ToString().Trim()),
                                    new XElement("Chat_Id", (Chat_Id_TextEdit.EditValue ?? "").ToString().Trim()),
                                    new XElement("Ctgy_Code", CtgyCode_Lov.EditValue ?? ""),
                                    new XElement("Most_Debt_Clng", ""),
                                    new XElement("Serv_No", (SERV_NO_TextEdit.EditValue ?? "").ToString().Trim()),
                                    new XElement("Ref_Code", (RefCode_Lov.EditValue ?? "").ToString().Trim())
                                 ),
                                 new XElement("Member_Ship",
                                    new XAttribute("strtdate", StrtDate_DateTime001.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : StrtDate_DateTime001.Value.Value.ToString("yyyy-MM-dd")),
                                    new XAttribute("enddate", NumbOfMontDnrm_RB001.Checked ? (StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd")) : (EndDate_DateTime001.Value == null ? StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(1).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd") : EndDate_DateTime001.Value.Value.ToString("yyyy-MM-dd"))),
                                    new XAttribute("numbmontofer", NumbMontOfer_TextEdit001.Text ?? "0"),
                                    new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit001.Text ?? "0"),
                                    new XAttribute("numbofattnweek", "0"),
                                    new XAttribute("attndaytype", /*AttnDayType_Lov001.EditValue ??*/ "7"),
                                    new XAttribute("strttime", StrtTime_Te.Text),
                                    new XAttribute("endtime", EndTime_Te.Text)
                                 )
                              )
                           )
                        );
                  else if (Rqst.RQST_STAT == "002") return;
                  //MessageBox.Show(this, "مشتریی جدید در سیستم ثبت گردید");
                  requery = true;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               /*
                * Requery Data From Database
                */
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            if (Rqst == null) return;

            if (MessageBox.Show(this, "آیا با انصراف درخواست مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            
            if (Rqst != null && Rqst.RQID > 0)
            {
               /*
                *  Remove Data From Tables
                */
               /*iScsc.ADM_TCNL_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );*/
               iScsc.CNCL_RQST_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID)
                     )
                  )
               );
               //MessageBox.Show(this, "مشتری حذف گردید!");
            }
            requery = true;
            //tc_pblc.SelectedTab = tp_pblcinfo;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
               //Create_Record();
               requery = false;
            }
         }
      }

      bool setOnDebt = false;
      private void Btn_RqstSav1_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            if (Rqst != null && Rqst.RQST_STAT == "001")
            {
               iScsc.ADM_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                        new XAttribute("regncode", Rqst.REGN_CODE),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        ),
                        new XElement("Payment",
                           new XAttribute("setondebt", setOnDebt),
                           PydtsBs1.List.OfType<Data.Payment_Detail>().ToList()
                           .Select(pd =>
                              new XElement("Payment_Detail",
                                 new XAttribute("code", pd.CODE),
                                 new XAttribute("rcptmtod", pd.RCPT_MTOD ?? "")
                              )
                           )
                        )
                     )
                  )
               );
               requery = true;
               //tc_pblc.SelectedTab = tp_pblcinfo;

               // Save Card In Device
               if(CardNumb_Text.Text != "")
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", "MAIN_PAGE_F", 41, SendType.SelfToUserInterface) {
                        Input = 
                        new XElement("User",
                           new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text),
                           new XAttribute("cardnumb", CardNumb_Text.Text),
                           new XAttribute("namednrm", FRST_NAME_TextEdit.Text + ", " + LAST_NAME_TextEdit.Text)
                        )
                     }
                  );

               

               // ثبت حضوری به صورت اتوماتیک
               if (SaveAttn_PkBt.PickChecked)
                  AutoAttn();

               CardNumb_Text.Text = "";

               // 1397/05/26 * اگر درخواست گزینه های جانبی داشته باشد باید شماره پرونده ها رو به فرم های مربوطه ارسال کنیم
               string followups = "";
               if (InsrInfo_Ckbx.Checked)
                  followups += "INS_TOTL_F;";
               if (OthrPblc_Ckbx.Checked)
                  followups += "ADM_CHNG_F;";
               if (OthrExpnInfo_Ckbx.Checked)
                  followups += "OIC_TOTL_F;";

               #region 3th
               if (InsrInfo_Ckbx.Checked)
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 80 /* Execute Ins_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "INS_TOTL_F", 10 /* Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Request", 
                                    new XAttribute("type", "renewinscard"), 
                                    new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO), 
                                    new XAttribute("formcaller", GetType().Name),
                                    new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                    new XAttribute("rqstrqid", Rqst.RQID)
                                 )
                           }
                        })
                  );
               else if (OthrPblc_Ckbx.Checked)
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 70 /* Execute Adm_Chng_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */)
                           {
                              Input = 
                                 new XElement("Request", 
                                    new XAttribute("type", "changeinfo"), 
                                    new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO), 
                                    new XAttribute("auto", "true"), 
                                    new XAttribute("formcaller", GetType().Name),
                                    new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                    new XAttribute("rqstrqid", Rqst.RQID)
                                 )
                           }
                        })
                  );
               else if (OthrExpnInfo_Ckbx.Checked)
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                           new List<Job>
                           {                  
                              new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                              new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */)
                              {
                                 Input = 
                                    new XElement("Request", 
                                       new XAttribute("type", "01"), 
                                       new XElement("Request_Row", 
                                          new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)),
                                       new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                       new XAttribute("rqstrqid", Rqst.RQID)
                                    )
                              }
                           })
                  );
               #endregion

               // 1398/05/10 * ثبت پکیج کلاس ها به صورت گروهی
               //if(GustSaveRqst_PickButn.PickChecked)
               if (GustSaveRqst_Cbx.Checked)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 64 /* Execute Adm_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "renewcontract"), new XAttribute("enrollnumber", FNGR_PRNT_TextEdit.Text), new XAttribute("formcaller", GetType().Name))}
                        })
                  );
               }
               else if (GotoProfile_Cbx.Checked)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)) }
                  );
               }

               // 1402/02/26 * ذخیره کردن شماره درخواست برای نمونه برداری برای کارت مشتریان دیگر
               if (RqstDupl_Pbtn.PickChecked)
               {
                  iScsc.DUP_RQST_P(
                     new XElement("Duplicate",
                         new XAttribute("type", "SetInitRecord"),
                         new XAttribute("rqid", Rqst.RQID)
                     )
                  );
                  RqstDupl_Pbtn.PickChecked = false;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requery)
            {
               //Get_Current_Record();
               Execute_Query();
               //Set_Current_Record();
               //Create_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Btn_Cbmt1_Click(object sender, EventArgs e)
      {
         try
         {
            long code = (long)CbmtCode_Lov.EditValue;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 149 /* Execute Bas_Wkdy_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_WKDY_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("code", code), new XAttribute("showonly", "002"))}
                  }
               )
            );
         }
         catch { }
      }

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text != "" && MessageBox.Show(this, "آیا با تغییر کد شناسایی موافق هستید؟", "تغییر کد شناسایی", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            FNGR_PRNT_TextEdit.EditValue = 
                iScsc.Fighters
                .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                .Select(f => f.FNGR_PRNT_DNRM)
                .ToList()
                .Where(f => f.All(char.IsDigit))
                .Max(f => Convert.ToInt64(f)) + 1;

            BRTH_DATE_PersianDateEdit.Focus();
         }
         catch
         {
             FNGR_PRNT_TextEdit.EditValue = 1;
         }
      }

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            if (RQTT_CODE_LookUpEdit1.EditValue == null || RQTT_CODE_LookUpEdit1.EditValue.ToString() == "")
               RQTT_CODE_LookUpEdit1.EditValue = "001";

            // 1401/02/22 * 

            long ctgycode = (long)CtgyCode_Lov.EditValue;
            string rqttcode = (string)RQTT_CODE_LookUpEdit1.EditValue;
            var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "001" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && /*exp.MTOD_CODE == mtodcode &&*/ exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

            /// سیستم تغییر تاریخ شروع و پایان
            /// Ctrl : تاریخ پایان بر اساس تاریخ شروع به تعداد دوره
            /// 

            if (ModifierKeys == Keys.Control)
            {
               // تاریخ پایان بر اساس تاریخ شروعی که وارد شده محاسبه گردد
               StrtDate_DateTime001.CommitChanges();                  
               var strtdate = StrtDate_DateTime001.Value;
               if (strtdate.HasValue)
                  EndDate_DateTime001.Value = strtdate.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               else
               {
                  StrtDate_DateTime001.Value = DateTime.Now;
                  EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
            }
            else if (ModifierKeys == Keys.Shift)
            {
               // تاریخ شروع به اولین روز همان ماه برگردد
               StrtDate_DateTime001.CommitChanges();
               var strtdate = StrtDate_DateTime001.Value;
               if (strtdate.HasValue)
               {
                  var day = StrtDate_DateTime001.GetText("dd").ToInt32();
                  if(day != 1)
                     StrtDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((day - 1) * -1);
                  EndDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
               else
               {
                  StrtDate_DateTime001.Value = DateTime.Now;
                  var day = StrtDate_DateTime001.GetText("dd").ToInt32();
                  if (day != 1)
                     StrtDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((day - 1) * -1);
                  EndDate_DateTime001.Value = StrtDate_DateTime001.Value.Value.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               }
            }               
            else
            {
               StrtDate_DateTime001.Value = DateTime.Now;
               EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
            }

            NumbOfAttnMont_TextEdit001.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;               
            NumbMontOfer_TextEdit001.EditValue = expn.NUMB_MONT_OFER ?? 0;

            DfltPric_Lb.Text = string.Format("{0:n0} {1} *** {2}", expn.PRIC, DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(a => a.VALU == expn.Expense_Type.Request_Requester.Regulation.AMNT_TYPE).DOMN_DESC, expn.Method.MTOD_DESC);
            DfltPric_Lb.Tag = expn.PRIC;

            // 1401/05/22 * اگر ظرفیت کلاسی پر شده باشد به منشی اعلام میکنیم
            if (CapacityCycle_Lb.Tag != null && Convert.ToInt64(CapacityCycle_Lb.Tag) <= 0 && MessageBox.Show(this, "ظرفیت ثبت نام گروه انتخابی پر شده، آیا مایل به این هستید که گروه دیگری را انتخاب کنید؟", "محدودیت ظرفیت ثبت نام", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
               Cbmt_Gv.ActiveFilterString = string.Format("[Sex_Type] = '{0}' AND MTOD_CODE = {1} OR [Sex_Type] = '003' AND MTOD_CODE = {1}", SEX_TYPE_LookUpEdit.EditValue, expn.MTOD_CODE);
               CbmtCode_Lov.Focus(); 
               return; 
            }

            var _crntcbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(cm => cm.CODE == (long)CbmtCode_Lov.EditValue);
            // 1401/07/18 * روز سرنگونی حکومت فاسر آخوندی
            StrtTime_Te.EditValue = _crntcbmt.STRT_TIME;
            EndTime_Te.EditValue = _crntcbmt.END_TIME;

            Btn_RqstRqt1_Click(null, null);
         }
         catch (Exception)
         {
            MessageBox.Show("در آیین نامه نرخ و هزینه تعداد جلسات و اطلاعات اتوماتیک به درستی وارد نشده. لطفا آیین نامه را بررسی و اصلاح کنید");
         }

      }

      private void Btn_Dise_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 65 /* Execute CMN_DISE_F */){ Input = GetType().Name }
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      
      }

      private void tbn_CashPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
               if (pymt == null) return;

               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

               string mesg = "";
               if(debtamnt > 0)
               {
                  mesg = 
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> نقدی << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد", 
                        string.Format("{0:n0}",debtamnt), 
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC, 
                        "امروز", 
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }

            foreach (Data.Payment pymt in PymtsBs1)
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithoutPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID)
                     //new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
                        )
                     )
                  )
               );
            }
            //var pymt = PymtsBs1.Current as Data.Payment;

            /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
            {
               MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
               return;
            }*/

               

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstSav1_Click(null, null);
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void tbn_POSPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
               if (pymt == null) return;

               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

               string mesg = "";
               if (debtamnt > 0)
               {
                  mesg =
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> کارتخوان << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                        string.Format("{0:n0}", debtamnt), 
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                        "امروز",
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }

            if (VPosBs1.List.Count == 0)
               UsePos_Cb.Checked = false;

            if (UsePos_Cb.Checked /* 1402/11/01 if payment no debt */ && PymtsBs1.List.OfType<Data.Payment>().Any(p => (p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM) > 0))
            {
               foreach (Data.Payment pymt in PymtsBs1)
               {
                  var amnt = ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
                  if ( amnt== 0) return;

                  var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

                  long psid;
                  if (Pos_Lov.EditValue == null)
                  {
                     var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                     if (posdflts.Count() == 1)
                        Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
                     else
                     {
                        Pos_Lov.Focus();
                        return;
                     }
                  }
                  else
                  {
                     psid = (long)Pos_Lov.EditValue;
                  }

                  if (regl.AMNT_TYPE == "002")
                     amnt *= 10;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.External, "Commons",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 34 /* Execute PosPayment */)
                                 {
                                    Input = 
                                       new XElement("PosRequest",
                                          new XAttribute("psid", psid),
                                          new XAttribute("subsys", 5),
                                          new XAttribute("rqid", pymt.RQST_RQID),
                                          new XAttribute("rqtpcode", ""),
                                          new XAttribute("router", GetType().Name),
                                          new XAttribute("callback", 20),
                                          new XAttribute("amnt", amnt)
                                       )
                                 }
                              }
                           )                     
                        }
                     )
                  );
               }
            }
            else
            {
               // 1397/01/07 * ثبت دستی مبلغ به صورت پایانه فروش
               foreach (Data.Payment pymt in PymtsBs1)
               {
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithPOS"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID)
                           )
                        )
                     )
                  );
               }

               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstSav1_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void Btn_InDebt001_Click(object sender, EventArgs e)
      {
         try
         {
            setOnDebt = true;
            // بررسی دسترسی کاربر به بدهکاری ثبت کردن
            _DefaultGateway.Gateway(
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
                                    "<Privilege>192</Privilege><Sub_Sys>5</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    #region Show Error
                                    setOnDebt = false;
                                    MessageBox.Show("خطا - خطا - عدم دسترسی به ردیف 192 سطوح امینتی");
                                    #endregion                           
                                 })
                              },
                              #endregion
                           }
                        ){GenerateInputData = GenerateDataType.Dynamic}
                    })
            );

            if (setOnDebt == false) return;

            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
               if (pymt == null) return;

               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

               string mesg = "";
               if (debtamnt > 0)
               {
                  mesg =
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> بدهکار << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                        string.Format("{0:n0}", debtamnt), 
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                        "امروز",
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               else
                  setOnDebt = false;

               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }
            //var rqst = RqstBs1.Current as Data.Request;
            //if (rqst == null) return;
            //var pymt = PymtsBs1.Current as Data.Payment;

            /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
            {
               MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
               return;
            }*/

            /*iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "CheckoutWithoutPOS"),
                  new XElement("Insert",
                     new XElement("Payment_Method",
                        new XAttribute("cashcode", pymt.CASH_CODE),
                        new XAttribute("rqstrqid", pymt.RQST_RQID)                  
                     )
                  )
               )
            );*/

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstSav1_Click(null, null);
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void bn_Card2CardPayment_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
               if (pymt == null) return;

               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

               string mesg = "";
               if (debtamnt > 0)
               {
                  mesg =
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> کارت به کارت << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                        string.Format("{0:n0}", debtamnt),
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                        "امروز",
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }

            foreach (Data.Payment pymt in PymtsBs1)
            {
               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithCard2Card"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQST_RQID)
                        )
                     )
                  )
               );
            }

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstSav1_Click(null, null);
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void bn_PaymentMethods1_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs1.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 86 /* Execute Pay_Mtod_F */){Input = pymt},
                     new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 10 /* Execute Actn_CalF_F*/)
                     {
                        Input = 
                           new XElement("Payment_Method",
                              new XAttribute("callerform", GetType().Name)
                           )
                     }
                  }

               )
            );
            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) 
            //   { 
            //      Input = pymt 
            //   }
            //);
         }
      }

      private void RqstBnADoc_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
            );
         }
      }

      private void RqstBnADocPicProfile1_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               var result = (
                        from r in iScsc.Regulations
                        join rqrq in iScsc.Request_Requesters on r equals rqrq.Regulation
                        join rqdc in iScsc.Request_Documents on rqrq equals rqdc.Request_Requester
                        join rcdc in iScsc.Receive_Documents on rqdc equals rcdc.Request_Document
                        where r.TYPE == "001"
                           && r.REGL_STAT == "002"
                           && rqrq.RQTP_CODE == rqst.RQTP_CODE
                           && rqrq.RQTT_CODE == rqst.RQTT_CODE
                           && rqdc.DCMT_DSID == 13930903120048833 // عکس 4*3
                           && rcdc.RQRO_RQST_RQID == rqst.RQID
                           && rcdc.RQRO_RWNO == 1
                        select rcdc).FirstOrDefault();
               if (result == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self,  59 /* Execute Cmn_Dcmt_F */){ Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() },
                        new Job(SendType.SelfToUserInterface, "CMN_DCMT_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Action",
                                 new XAttribute("type", "001"),
                                 new XAttribute("typedesc", "Force Active Camera Picture Profile"),
                                 new XElement("Document",
                                    new XAttribute("rcid", result.RCID)
                                 )
                              )
                        }
                     }
                  )
               );

            }
         }
         catch
         {

         }

      }

      #region Finger Print Device Operation
      private void RqstBnEnrollFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            RqstBnDeleteFngrPrnt1_Click(null, null);

            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */)
                     {
                        Input = 
                           new XElement("DeviceControlFunction", 
                              new XAttribute("functype", (ModifierKeys == Keys.Control ? "5.2.3.8.1" /* Add Face */ : "5.2.3.8" /* Add Finger */)), 
                              new XAttribute("funcdesc", "Add User Info"), 
                              new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text)
                           )
                     }
                  })
            );
         }
         catch { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */)
                     {
                        Input = 
                           new XElement("DeviceControlFunction", 
                              new XAttribute("functype", (ModifierKeys == Keys.Control ? "5.2.3.8.2" /* Delete Face */ : "5.2.3.5" /* Delete Finger */)), 
                              new XAttribute("funcdesc", "Delete User Info"), 
                              new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text)
                           )
                     }
                  })
            );
         }
         catch { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {                  
                     new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */)
                     {
                        Input = new XElement("DeviceControlFunction", 
                           new XAttribute("functype", "5.2.7.2" /* Duplicate */), 
                           new XAttribute("funcdesc", "Duplicate User Info Into All Device"), 
                           new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text)
                        )
                     }
                  })
            );
         }
         catch { }
      }

      private void RqstBnEnrollFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "enroll"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt2_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "MAIN_PAGE_F", 10 /* Execute actn_Calf_F */, SendType.SelfToUserInterface)
               {
                  Input =
                     new XElement("Command",
                        new XAttribute("type", "fngrprntdev"),
                        new XAttribute("fngractn", "delete"),
                        new XAttribute("fngrprnt", FNGR_PRNT_TextEdit.Text)
                     )
               }
            );
         }
         catch (Exception exc) { }
      }      
      #endregion
      
      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 94 /* Execute Cmn_Resn_F */){Input = rqst.Request_Rows.FirstOrDefault()},
                     //new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnRegl01_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "001")))}
                  })
               );
         }         
      }

      private void sUNT_BUNT_DEPT_ORGN_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString();
            DeptBs1.DataSource = iScsc.Departments.Where(d => d.ORGN_CODE == crntorgn);*/
            OrgnBs1.Position = SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void sUNT_BUNT_DEPT_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString(); 
            var crntdept = sUNT_BUNT_DEPT_CODELookUpEdit.EditValue.ToString();            
            BuntBs1.DataSource = iScsc.Base_Units.Where(b => b.DEPT_CODE == crntdept && b.DEPT_ORGN_CODE == crntorgn);*/
            DeptBs1.Position = SUNT_BUNT_DEPT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_DEPT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_DEPT_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void sUNT_BUNT_CODELookUpEdit_Popup(object sender, EventArgs e)
      {
         try
         {
            /*var crntorgn = sUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue.ToString();
            var crntdept = sUNT_BUNT_DEPT_CODELookUpEdit.EditValue.ToString();
            var crntbunt = sUNT_BUNT_CODELookUpEdit.EditValue.ToString();
            SuntBs1.DataSource = iScsc.Sub_Units.Where(s => s.BUNT_CODE == crntbunt && s.BUNT_DEPT_CODE == crntdept && s.BUNT_DEPT_ORGN_CODE == crntorgn);*/
            BuntBs1.Position = SUNT_BUNT_CODELookUpEdit.Properties.GetDataSourceRowIndex(SUNT_BUNT_CODELookUpEdit.Properties.ValueMember, SUNT_BUNT_CODELookUpEdit.EditValue);
         }
         catch
         {
         }
      }

      private void CbmtCode_Lov_Popup(object sender, EventArgs e)
      {
         try
         {
            var cbmt = CbmtCode_Lov.EditValue;

            if (cbmt == null || cbmt.ToString() == "") return;

            var crntcbmt = CbmtBs1.List.OfType<Data.Club_Method>().FirstOrDefault(c => c.CODE == (long)cbmt);

            CtgyBs1.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == crntcbmt.MTOD_CODE && c.CTGY_STAT == "002");

            var cbmtt = iScsc.Club_Methods.First(cm => cm.CODE == (long)CbmtCode_Lov.EditValue);
            if (cbmtt == null) return;

            var cmwk = cbmtt.Club_Method_Weekdays.ToList();

            if (cmwk.Count == 0)
            {
               ClubWkdy_Pn.Controls.OfType<SimpleButton>().Where(sb => sb.Tag != null).ToList().ForEach(sb => sb.Appearance.BackColor = Color.Gold);
               return;
            }

            foreach (var wkdy in cmwk)
            {
               var rslt = ClubWkdy_Pn.Controls.OfType<SimpleButton>().FirstOrDefault(sb => sb.Tag != null && sb.Tag.ToString() == wkdy.WEEK_DAY);
               rslt.Appearance.BackColor = wkdy.STAT == "001" ? Color.LightGray : Color.GreenYellow;
            }

            // 1401/05/22 * امروز تولد ملودی هست که من بهش تبریک گفتم
            // ملودی عزیزم همیشه شاد خوشحال باشی، چون قشنگی دنیای من بسته به لبخندهای شیرین تو هست عزیز دلم
            // بررسی ظرفیت کلاسی
            if (cbmtt.CPCT_STAT == "002")
            {
               var listMbsp =
                  iScsc.Member_Ships
                  .Where(ms =>
                     ms.RECT_CODE == "004" &&
                     ms.VALD_TYPE == "002" &&
                     ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                     ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                     (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                     ms.Fighter_Public.CBMT_CODE == cbmtt.CODE
                  );
               CapacityCycle_Lb.Text = string.Format("ک.ظ :" + " {0} " + "ب.ظ :" + "{1}", cbmtt.CPCT_NUMB, (cbmtt.CPCT_NUMB - listMbsp.Count()));
               CapacityCycle_Lb.Tag = cbmtt.CPCT_NUMB - listMbsp.Count();

               if(cbmtt.CPCT_NUMB > listMbsp.Count())
                  CapacityCycle_Lb.ForeColor = Color.Black;
               else if(cbmtt.CPCT_NUMB < listMbsp.Count())
                  CapacityCycle_Lb.ForeColor = Color.Red;
               else
                  CapacityCycle_Lb.ForeColor = Color.Green;
            }
            else
            {
               CapacityCycle_Lb.Text = "نامحدود";
               CapacityCycle_Lb.Tag = null;
               CapacityCycle_Lb.ForeColor = Color.Green;
            }
         }
         catch {}
      }

      private void BTN_MBSP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         
      }

      private void DayType_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "TIME_DESC" && e.IsGetData)
         {
            var h = ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "END_TIME")).Hours;
            e.Value = h >= 0 && h < 12 ? "صبح" : h >= 12 && h < 15 ? "ظهر" : h >= 15 && h < 18 ? "بعد ظهر" : h >= 18 ? "عصر" : "نام مشخص";
         }
      }

      private void CopyDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            EndDate_DateTime001.Value = StrtDate_DateTime001.Value;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void CrntDate_Butn_Click(object sender, EventArgs e)
      {
         var strtdate = StrtDate_DateTime001.Value;
         if (strtdate != null && MessageBox.Show(this, "آیا تاریخ شروع را میخواهید اصلاح کنید", "اصلاح تاریخ شروع", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         StrtDate_DateTime001.Value = DateTime.Now;
      }

      private void IncDecMont_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var strtdate = StrtDate_DateTime001.Value;
         if (strtdate == null) StrtDate_DateTime001.Value = DateTime.Now;

         var enddate = EndDate_DateTime001.Value;
         if (enddate == null) EndDate_DateTime001.Value = StrtDate_DateTime001.Value;

         switch (e.Button.Index)
         {
            case 1:
               EndDate_DateTime001.Value = EndDate_DateTime001.Value.Value.AddDays(30);
               break;
            case 0:
               EndDate_DateTime001.Value = EndDate_DateTime001.Value.Value.AddDays(-30);
               break;
         }
      }

      private void INTR_FILE_NOLookUpEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
          try
          {
              if (e.Button.Index == 2) // بارگذاری لیست جدید
              {
                  iScsc = new Data.iScscDataContext(ConnectionString);
                  FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" /*&& !f.NAME_DNRM.Contains("مشتری, جلسه ای")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
                  return;
              }
              else if (e.Button.Index == 3) // تعریف کاربر جدید
              {
                  Job _InteractWithScsc =
                      new Job(SendType.External, "Localhost",
                         new List<Job>
                         {
                            new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                            new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "tp_001"))}
                         });
                  _DefaultGateway.Gateway(_InteractWithScsc);
                  return;
              }

              //if (INTR_FILE_NOLookUpEdit.EditValue.ToString() == "") return;

              //var fileno = Convert.ToInt64(INTR_FILE_NOLookUpEdit.EditValue);

              //switch (e.Button.Index)
              //{
              //    case 1:
              //        _DefaultGateway.Gateway(
              //           new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
              //        );
              //        break;
              //    default:
              //        break;
              //}
          }
          catch { }
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void AutoAttn()
      {
         try
         {
            var figh = FgpbsBs1.Current as Data.Fighter_Public;

            if (figh.FNGR_PRNT == "" && !(figh.TYPE == "002" || figh.TYPE == "003")) { MessageBox.Show(this, "برای عضو مورد نظر هیچ کد انگشتی وارد نشده، لطفا کد عضو را از طریق تغییرات مشخصات عمومی تغییر لازم را اعمال کنید"); return; }
            if (figh.COCH_FILE_NO == null && !(figh.TYPE == "009" || figh.TYPE == "002" || figh.TYPE == "003" || figh.TYPE == "004")) { MessageBox.Show(this, "برای عضو شما مربی و ساعت کلاسی مشخصی وجود ندارد که مشخص کنیم در چه کلاس حضوری ثبت کنیم"); return; }
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                     {                        
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "accesscontrol"), new XAttribute("fngrprnt", figh.FNGR_PRNT))}
                     })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FgpbsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var fgpb = FgpbsBs1.Current as Data.Fighter_Public;
            if (fgpb == null) return;

            //MaxAdm_Txt.Text = iScsc.Club_Methods.FirstOrDefault(cb => cb.CODE == fgpb.CBMT_CODE).CPCT_NUMB.ToString();
            //TotlAdm_Txt.Text = iScsc.Fighters.Where(f => f.CBMT_CODE_DNRM == fgpb.CBMT_CODE && Convert.ToInt32(f.ACTV_TAG_DNRM) >= 101 && f.MBSP_END_DATE.Value.Date >= DateTime.Now.Date).Count().ToString();
            //NewAdm_Txt.Text = (Convert.ToInt32(TotlAdm_Txt.Text) + 1).ToString();

            AdvpBs1.List.Clear();

            if(fgpb.REF_CODE != null)
            {
               if(!RServBs.List.OfType<Data.Fighter>().Any(s => s.FILE_NO == fgpb.REF_CODE))
                  RServBs.DataSource = iScsc.Fighters.Where(s => s.FILE_NO == fgpb.REF_CODE);

               var _refServ = iScsc.Fighters.FirstOrDefault(s => s.FILE_NO == fgpb.REF_CODE);
               RefDesc_Txt.Text = 
                  string.Format("نام : " + "{0} " + "شماره موبایل : " + "{1} " + Environment.NewLine + "تعداد معرفین : " + "{2}",
                     _refServ.NAME_DNRM,
                     _refServ.CELL_PHON_DNRM,
                     iScsc.Fighters.Where(s => s.REF_CODE_DNRM == _refServ.FILE_NO).Count()
                  );
            }

            if(fgpb.CELL_PHON != null)
            {
               AdvpBs1.DataSource =
                  iScsc.Advertising_Parameters
                  .Where(a => 
                     a.STAT == "002" &&
                     (a.RQTP_CODE == null || a.RQTP_CODE == "001") &&
                     a.Advertising_Campaigns
                     .Any(c => 
                        c.CELL_PHON == fgpb.CELL_PHON && 
                        c.RECD_STAT == "003" && 
                        c.ACTN_DATE != null
                     )
                  );
            }
         }
         catch (Exception )
         {

         }
      }

      private void FNGR_PRNT_TextEdit_TextChanged(object sender, EventArgs e)
      {
         if (AutoTrans_Cb.Checked)
            CardNumb_Text.Text = FNGR_PRNT_TextEdit.Text;
      }

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }

      private void PydsType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydsType_Butn.Text = PydsType_Butn.Tag.ToString() == "0" ? "مبلغی" : "درصدی";
            PydsType_Butn.Tag = PydsType_Butn.Tag.ToString() == "0" ? "1" : "0";
            if (PydsType_Lov.EditValue != null || PydsType_Lov.EditValue.ToString() != "") PydsType_Lov.EditValue = "002";

            if(PydsType_Butn.Tag.ToString() == "0")
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "درصد تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 3;
            }
            else
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "مبلغ تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 0;
            }
            PydsAmnt_Txt.Focus();

         }
         catch{}
      }

      private void RcmtType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RcmtType_Butn.Text = RcmtType_Butn.Tag.ToString() == "0" ? "POS" : "نقدی";
            RcmtType_Butn.Tag = RcmtType_Butn.Tag.ToString() == "0" ? "1" : "0";
            PymtAmnt_Txt.Focus();
            var pymt = PymtsBs1.Current as Data.Payment;
            if(pymt == null)return;
            PymtAmnt_Txt.EditValue = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
         }
         catch { }
      }

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

            long? amnt = null;
            switch(PydsType_Butn.Tag.ToString())
            {
               case "0":
                  if (!(Convert.ToInt32(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt32(PydsAmnt_Txt.EditValue) <= 100))
                  {
                     PydsAmnt_Txt.EditValue = null;
                     PydsAmnt_Txt.Focus();
                  }                                    

                  amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt64(PydsAmnt_Txt.EditValue)) / 100;
                  break;
               case "1":
                  amnt = Convert.ToInt32(PydsAmnt_Txt.EditValue);
                  if (amnt == 0) return;
                  break;
            }

            // 1401/09/19 * #MahsaAmini
            // اگر تخفیف برای پرسنل بخواهیم ثبت کنیم باید چک کنیم که آیا تخفیف وارد شده بیشتر سهم پرسنل نباشد
            if(PydsType_Lov.EditValue.ToString() == "005")
            {
               var _pydt = PydtsBs1.Current as Data.Payment_Detail;

               var _calcexpn = 
                  iScsc.CALC_EXPN_U(
                     new XElement("Request",
                         new XAttribute("rqid", pymt.RQST_RQID),
                         new XAttribute("expncode", _pydt.EXPN_CODE)
                     )
                  );

               // اگر مبلغ تخفیف بیشتر از سهم پرسنل باشد باید جلو آن گرفته شود
               if (_calcexpn < amnt)
               {
                  MessageBox.Show(this, "مبلغ تخفیف وارد شده از سهم پرسنل بیشتر حق پرداختی ایشان میباشد، لطفا درصد تخفیف یا مبلغ تخفیف را اصلاح کنید", "تخفیف غیرمجاز پرسنل");
                  return;
               }
            }

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, pydt.EXPN_CODE, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text, PydsDesc_Txt.Tag == null ? null : (long?)PydsDesc_Txt.Tag, null);

            PydsAmnt_Txt.EditValue = null;
            PydsDesc_Txt.EditValue = null;
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void DeltPyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pyds = PydsBs1.Current as Data.Payment_Discount;
            if (pyds == null) return;

            iScsc.DEL_PYDS_P(pyds.PYMT_CASH_CODE, pyds.PYMT_RQST_RQID, pyds.RQRO_RWNO, pyds.RWNO);

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SavePymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PymtDate_DateTime001.CommitChanges();
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            if(PymtAmnt_Txt.EditValue == null || PymtAmnt_Txt.EditValue.ToString() == "" || Convert.ToInt64(PymtAmnt_Txt.EditValue) == 0)return;
            
            //1403/08/26 * اگر تاریخ پرداخت بیشتر از تاریخ جاری باشد
            if (PymtDate_DateTime001.Value.HasValue && PymtDate_DateTime001.Value.Value.Date > DateTime.Now.Date)
            {
               MessageBox.Show(this, "پرداختی در گذشته داریم ولی پرداختی در آینده نداریم، اینجاست که باید بگم داش داری اشتباه میزنی");
               PymtDate_DateTime001.Focus();
               PymtDate_DateTime001.Value = DateTime.Now;
               return;
            }

            switch ((RcmtType_Lov.EditValue ?? "001").ToString())
            {               
               case "003":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked && (!PymtDate_DateTime001.Value.HasValue || PymtDate_DateTime001.Value.Value.Date == DateTime.Now.Date))
                  {
                     var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

                     long psid;
                     if (Pos_Lov.EditValue == null)
                     {
                        var posdflts = VPosBs1.List.OfType<Data.V_Pos_Device>().Where(p => p.POS_DFLT == "002");
                        if (posdflts.Count() == 1)
                           Pos_Lov.EditValue = psid = posdflts.FirstOrDefault().PSID;
                        else
                        {
                           Pos_Lov.Focus();
                           return;
                        }
                     }
                     else
                     {
                        psid = (long)Pos_Lov.EditValue;
                     }

                     if (regl.AMNT_TYPE == "002")
                        PymtAmnt_Txt.EditValue = Convert.ToInt64(PymtAmnt_Txt.EditValue) * 10;

                     // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
                     UsePos_Cb.Checked = false;

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost",
                           new List<Job>
                           {
                              new Job(SendType.External, "Commons",
                                 new List<Job>
                                 {
                                    new Job(SendType.Self, 34 /* Execute PosPayment */)
                                    {
                                       Input = 
                                          new XElement("PosRequest",
                                             new XAttribute("psid", psid),
                                             new XAttribute("subsys", 5),
                                             new XAttribute("rqid", pymt.RQST_RQID),
                                             new XAttribute("rqtpcode", ""),
                                             new XAttribute("router", GetType().Name),
                                             new XAttribute("callback", 20),
                                             new XAttribute("amnt", Convert.ToInt64(PymtAmnt_Txt.EditValue)),
                                             new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                                             new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                                             new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? "")
                                          )
                                    }
                                 }
                              )
                           }
                        )
                     );

                     UsePos_Cb.Checked = true;
                  }
                  else
                  {
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "InsertUpdate"),
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", pymt.CASH_CODE),
                                 new XAttribute("rqstrqid", pymt.RQST_RQID),
                                 new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                                 new XAttribute("rcptmtod", "003"),
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                                 new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                                 new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                                 new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? "")
                              )
                           )
                        )
                     );
                  }
                  break;
               default:
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", RcmtType_Lov.EditValue ?? "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                              new XAttribute("rcpttoothracnt", Rtoa_Lov.EditValue ?? ""),
                              new XAttribute("flowno", FlowNo_Txt.EditValue ?? ""),
                              new XAttribute("rcptfilepath", RcptFilePath_Txt.EditValue ?? "")
                           )
                        )
                     )
                  );
                  break;
            }

            PymtAmnt_Txt.EditValue = null;
            PymtDate_DateTime001.Value = DateTime.Now;
            Rtoa_Lov.EditValue = null;
            FlowNo_Txt.EditValue = null;
            RcptFilePath_Txt.EditValue = null;
            RcmtType_Lov.Focus();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void DeltPymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pmmt = PmmtBs1.Current as Data.Payment_Method;
            if (pmmt == null) return;

            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "Delete"),
                  new XAttribute("cashcode", pmmt.PYMT_CASH_CODE),
                  new XAttribute("rqstrqid", pmmt.PYMT_RQST_RQID),
                  new XAttribute("rwno", pmmt.RWNO)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void CbmtCode_Lov_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            //long code = (long)CbmtCode_Lov.EditValue;
            if (e.Button.Index == 1)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 159 /* Execute Bas_Cbmt_F */),
                        new Job(SendType.SelfToUserInterface,"BAS_CBMT_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Club_Method", new XAttribute("formcaller", GetType().Name))}
                     }
                  )
               );
            }
         }
         catch { }
      }

      private void SmplAdmnServ_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 130 /* Execute Adm_Brsr_F */),
                  new Job(SendType.SelfToUserInterface, "ADM_BRSR_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "fighter"), new XAttribute("enrollnumber", FNGR_PRNT_TextEdit.Text))}
               })
         );
      }

      private void IncDec_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            var pydt = PydtsBs1.Current as Data.Payment_Detail; 
            var fgpb = FgpbsBs1.Current as Data.Fighter_Public;

            switch (e.Button.Index)
            {
               case 0:
                  iScsc.ExecuteCommand(
                     string.Format(
                        "UPDATE dbo.Payment_Detail SET QNTY += 1 WHERE PYMT_RQST_RQID = {0};" + Environment.NewLine +
                        "UPDATE dbo.Member_Ship SET End_Date = DATEADD(Day, {2} * ({3} + 1), Strt_Date), Numb_Of_Attn_Mont = {1} * {2} WHERE RQRO_RQST_RQID = {0};", 
                        rqst.RQID,
                        fgpb.Category_Belt.NUMB_OF_ATTN_MONT,
                        pydt.QNTY + 1,
                        fgpb.Category_Belt.NUMB_CYCL_DAY)
                     );
                  requery = true;
                  break;
               case 1:
                  if (pydt.QNTY > 1)
                  {
                     iScsc.ExecuteCommand(
                        string.Format(
                           "UPDATE dbo.Payment_Detail SET QNTY -= 1 WHERE PYMT_RQST_RQID = {0};" + Environment.NewLine +
                           "UPDATE dbo.Member_Ship SET End_Date = DATEADD(Day, -({3} + 1), End_Date), Numb_Of_Attn_Mont = {1} * {2} WHERE RQRO_RQST_RQID = {0};",
                           rqst.RQID,
                           fgpb.Category_Belt.NUMB_OF_ATTN_MONT,
                           pydt.QNTY - 1,
                           fgpb.Category_Belt.NUMB_CYCL_DAY,
                           rqst.RQID)
                        );
                     requery = true;
                  }
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void bn_DpstPayment3_Click(object sender, EventArgs e)
      {

      }

      private void SaveRfrl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            Adm_Tc.SelectedTab = AdmMoreInfo_Tp;
            CellPhonRefCode_Txt.Focus();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CellPhonRefCode_Txt_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            if (CellPhonRefCode_Txt.EditValue == null || CellPhonRefCode_Txt.Text == "") { CellPhonRefCode_Txt.Focus(); return; }

            RServBs.DataSource = iScsc.Fighters.Where(s => s.CELL_PHON_DNRM.Contains(CellPhonRefCode_Txt.Text));
            if(RServBs.List.Count == 1)
            {
               RefCode_Lov.EditValue = RServBs.List.OfType<Data.Fighter>().FirstOrDefault().FILE_NO;
               CellPhonRefCode_Txt.EditValue = null;
            }
            else if(RServBs.List.Count > 1)
            {
               RefCode_Lov.Focus();
               RefCode_Lov.ShowPopup();
            }
            else 
            {
               RefCode_Lov.EditValue = null;
               RefDesc_Txt.Text = "";
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RcmtType_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         RcmtType_Butn_Click(null, null);
      }

      private void Advc_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null || _rqst.RQID == 0) return;

            var _advp = AdvpBs1.Current as Data.Advertising_Parameter;
            if(_advp == null) return;

            var _advc = 
               _advp
               .Advertising_Campaigns
               .FirstOrDefault(c => 
                  c.CELL_PHON == CELL_PHON_TextEdit.Text && 
                  c.RECD_STAT == "003" &&
                  c.ACTN_DATE != null
               );

            if (_advc == null) return;
            if (_advc.RQST_RQID != null) { MessageBox.Show(this, "این رکورد کد تخفیف قبلا درون درخواست ثبت شده است", "عدم ثبت کد تخفیف"); return; }
            if(_advp.CTGY_CODE != null && _rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault().CTGY_CODE != _advp.CTGY_CODE)
            {
               MessageBox.Show(this, "کاربر گرامی این کد تخفیف برای نرخ مورد نظر شما قابل استفاده نمی باشد", "عدم استفاده از کد تخفیف");
               return;
            }
            if (_advp.EXPR_DATE.Value.Date < DateTime.Now.Date) { MessageBox.Show(this, "تاریخ انقضای کد تخفیف شما تمام شده است", "عدم اعتبار تاریخ انقضا"); return; }

            switch (_advp.DSCT_TYPE)
            {
               case "001":
                  // %
                  // اگر دکمه عملیات تخفیف گذاری غیر محتوای درصدی باشد
                  if (PydsType_Butn.Tag.ToString() != "0") { PydsType_Butn_Click(null, null); }                  
                  break;
               case "002":
                  // $
                  // اگر دکمه عملیات تخفیف گذاری غیر محتوای مبلغی باشد
                  if (PydsType_Butn.Tag.ToString() != "1") { PydsType_Butn_Click(null, null); }
                  break;
               default:
                  break;
            }

            PydsAmnt_Txt.EditValue = _advp.DSCT_AMNT;
            PydsDesc_Txt.Text = string.Format("کد تخفیف " + "( {0} )" + " بابت : " + "( {1} )" + " توسط کاربر : " + "( {2} )" + " ذخیره شد.", _advp.DISC_CODE, _advp.ADVP_NAME, CurrentUser);
            PydsDesc_Txt.Tag = _advc.CODE;
            iScsc.ExecuteCommand(string.Format("UPDATE dbo.Advertising_Campaign SET RQST_RQID = {0} WHERE CODE = {1};", _rqst.RQID, _advc.CODE));
            SavePyds_Butn_Click(null, null);
            PydsDesc_Txt.Tag = null;
            PymtOprt_Tc.SelectedTab = PymtDsct_Tp;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Rtoa_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            switch (e.Button.Index)
            {
               case 1:
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                           new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                           {
                              Input = 
                                 new XElement("App_Base",
                                    new XAttribute("tablename", "Payment_To_Another_Account"),
                                    new XAttribute("formcaller", GetType().Name)
                                 )
                           }
                        }
                     )
                  );
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CapacityCycle_Lb_Click(object sender, EventArgs e)
      {
         try
         {
            var _cbmt = iScsc.Club_Methods.First(cm => cm.CODE == (long)CbmtCode_Lov.EditValue);
            if (_cbmt == null) return;

            ListMbspBs.DataSource =
               iScsc.Member_Ships
                  .Where(ms =>
                     ms.RECT_CODE == "004" &&
                     ms.VALD_TYPE == "002" &&
                     ms.STRT_DATE.Value.Date <= DateTime.Now.Date &&
                     ms.END_DATE.Value.Date >= DateTime.Now.Date &&
                     (ms.NUMB_OF_ATTN_MONT == 0 || ms.NUMB_OF_ATTN_MONT > ms.SUM_ATTN_MONT_DNRM) &&
                     ms.Fighter_Public.CBMT_CODE == _cbmt.CODE
                  );

            if (ListMbspBs.List.Count > 0)
            {
               Adm_Tc.SelectedTab = More_Tp;
               More_Tc.SelectedTab = tp_007;
               CochName_Txt.Text = _cbmt.Fighter.NAME_DNRM;
               MtodName_Txt.Text = _cbmt.Method.MTOD_DESC;
               QStrtTime_Tim.EditValue = _cbmt.STRT_TIME;
               QEndTime_Tim.EditValue = _cbmt.END_TIME;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ListMbspBs_CurrentChanged(object sender, EventArgs e)
      {
         var _mbsp = ListMbspBs.Current as Data.Member_Ship;
         if (_mbsp == null) return;

         long? _rqid = 0;
         if (_mbsp.RWNO == 1)
            _rqid = _mbsp.Request_Row.Request.Request1.RQID;
         else
            _rqid = _mbsp.RQRO_RQST_RQID;

         ExpnAmnt_Txt.EditValue = iScsc.Payment_Details.Where(pd => pd.PYMT_RQST_RQID == _rqid).Sum(pd => (pd.EXPN_PRIC + pd.EXPN_EXTR_PRCT) * pd.QNTY);
         DscnAmnt_Txt.EditValue = iScsc.Payment_Discounts.Where(pd => pd.PYMT_RQST_RQID == _rqid).Sum(pd => pd.AMNT);
         PymtAmnt1_Txt.EditValue = iScsc.Payment_Methods.Where(pd => pd.PYMT_RQST_RQID == _rqid).Sum(pd => pd.AMNT);
         DebtPymtAmnt1_Txt.EditValue = Convert.ToInt64(ExpnAmnt_Txt.EditValue) - (Convert.ToInt64(PymtAmnt1_Txt.EditValue) + Convert.ToInt64(DscnAmnt_Txt.EditValue));
      }

      private void Rcpt2OthrAcnt_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _pmmt = PmmtBs1.Current as Data.Payment_Method;
            if (_pmmt == null || _pmmt.RCPT_TO_OTHR_ACNT == null) return;            

            switch (e.Button.Index)
            {
               case 1:
                  Adm_Tc.SelectedTab = More_Tp;
                  More_Tc.SelectedTab = tp_008;
                  Rcpt2OthrAcnt2_Lov.EditValue = _pmmt.RCPT_TO_OTHR_ACNT;
                  Rcpt2OthrAcnt2_Lov_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(Rcpt2OthrAcnt2_Lov.Properties.Buttons[1]));
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Rcpt2OthrAcnt2_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (!FPmmt_Dt.Value.HasValue)
            {
               FPmmt_Dt.CommitChanges();
               var _fpmmt = FPmmt_Dt.Value;
               if (_fpmmt.HasValue)
               {
                  var day = FPmmt_Dt.GetText("dd").ToInt32();
                  if (day != 1)
                     FPmmt_Dt.Value = FPmmt_Dt.Value.Value.AddDays((day - 1) * -1);
                  TPmmt_Dt.Value = FPmmt_Dt.Value.Value.AddDays(30);
               }
               else
               {
                  FPmmt_Dt.Value = DateTime.Now;
                  var day = FPmmt_Dt.GetText("dd").ToInt32();
                  if (day != 1)
                     FPmmt_Dt.Value = FPmmt_Dt.Value.Value.AddDays((day - 1) * -1);
                  TPmmt_Dt.Value = FPmmt_Dt.Value.Value.AddDays(30);
               }
            }

            ListPmmtBs.DataSource =
               iScsc.Payment_Methods
               .Where(pm => pm.RCPT_TO_OTHR_ACNT == (long?)Rcpt2OthrAcnt2_Lov.EditValue 
                  && pm.ACTN_DATE.Value.Date >= FPmmt_Dt.Value.Value.Date 
                  && pm.ACTN_DATE.Value.Date <= TPmmt_Dt.Value.Value.Date
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MbspValdType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = ListMbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            if (_mbsp.TYPE == "005")
            {
               MessageBox.Show(this, "شما اجازه غیرفعال کردن رکورد بلوکه کردن را ندارید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
               return;
            }

            if (_mbsp.VALD_TYPE == "002")
            {
               if (MessageBox.Show(this, "آیا با غیرفعال کردن دوره موافق هستید؟", "غیرفعال کردن دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

               iScsc.ExecuteCommand(string.Format("UPDATE Member_Ship SET Vald_Type = '001' WHERE Rqro_Rqst_Rqid = {0};", _mbsp.RQRO_RQST_RQID));
            }
            else if (_mbsp.VALD_TYPE == "001")
            {
               if (ListMbspBs.List.OfType<Data.Member_Ship>().Any(m => m.RWNO > _mbsp.RWNO && m.TYPE == "005"))
               {
                  MessageBox.Show(this, "شما اجازه فعال کردن دوره ابطال شده توسط فرآیند بلوکه کردن را ندارید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  return;
               }

               if (MessageBox.Show(this, "آیا با فعال کردن دوره موافق هستید؟", "فعال کردن دوره", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

               iScsc.ExecuteCommand(string.Format("UPDATE Member_Ship SET Vald_Type = '002' WHERE Rqro_Rqst_Rqid = {0};", _mbsp.RQRO_RQST_RQID));
            }
            
            // Reload Data
            CapacityCycle_Lb_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MbspEdit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = ListMbspBs.Current as Data.Member_Ship;
            if (_mbsp == null) return;

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
                                 "<Privilege>231</Privilege><Sub_Sys>5</Sub_Sys>", 
                                 "DataGuard"
                              },
                              AfterChangedOutput = new Action<object>((output) => {
                                 if ((bool)output)
                                    return;
                                 MessageBox.Show("خطا - عدم دسترسی به ردیف 231 سطوح امینتی", "عدم دسترسی");
                              })
                           },
                           #endregion
                        }),
                     #region DoWork
                        new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                        new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("Fighter",
                                 new XAttribute("fileno", _mbsp.FIGH_FILE_NO),
                                 new XAttribute("mbsprwno", _mbsp.RWNO),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     #endregion
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch { }
      }

      private void ServProf_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = ListMbspBs.Current as Data.Member_Ship;
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _mbsp.FIGH_FILE_NO)) }
            );
         }
         catch { }
      }

      private void SEX_TYPE_LookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            Cbmt_Gv.ActiveFilterString = string.Format("[Sex_Type] = '{0}' OR [Sex_Type] = '003'", e.NewValue);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void bn_DiscountPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            if (Accept_Cb.Checked)
            {
               var pymt = PymtsBs1.Current as Data.Payment;
               if (pymt == null) return;

               // بدست آوردن مبلغ تخفیف از باقیمانده صورتحساب
               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

               string mesg = "";
               if (debtamnt > 0)
               {
                  mesg =
                     string.Format(
                        ">> مبلغ {0} {1} به صورت >> تخفیف << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                        string.Format("{0:n0}", debtamnt),
                        DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                        "امروز",
                        CurrentUser);
                  mesg += Environment.NewLine;
               }
               mesg += ">> ذخیره و پایان درخواست";

               if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
            }

            foreach (Data.Payment pymt in PymtsBs1)
            {
               var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
               iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, debtamnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text, PydsDesc_Txt.Tag == null ? null : (long?)PydsDesc_Txt.Tag, null);
            }

            /* Loop For Print After Pay */
            RqstBnPrintAfterPay_Click(null, null);

            /* End Request */
            Btn_RqstSav1_Click(null, null);
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void CELL_PHON_TextEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if(CELL_PHON_TextEdit.EditValue == null || CELL_PHON_TextEdit.Text == ""){ CELL_PHON_TextEdit.Focus(); return; }

            FighsBs1.DataSource = 
               iScsc.Fighters
                  .Where(
                     f => f.CELL_PHON_DNRM.Contains(CELL_PHON_TextEdit.Text) && 
                          f.CONF_STAT == "002" &&                        
                          (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || 
                              (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && 
                          Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101
                  );

            if (FighsBs1.Count > 0)
            {
               Adm_Tc.SelectedTab = More_Tp;
               More_Tc.SelectedTab = tp_009;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void NATL_CODE_TextEdit_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (NATL_CODE_TextEdit.EditValue == null || NATL_CODE_TextEdit.Text == "") { NATL_CODE_TextEdit.Focus(); return; }

            FighsBs1.DataSource =
               iScsc.Fighters
                  .Where(
                     f => f.NATL_CODE_DNRM.Contains(NATL_CODE_TextEdit.Text) &&
                          f.CONF_STAT == "002" &&
                          (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) ||
                              (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&
                          Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101
                  );

            if (FighsBs1.Count > 0)
            {
               Adm_Tc.SelectedTab = More_Tp;
               More_Tc.SelectedTab = tp_009;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void HL_INVSFILENO_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _crnt = FighsBs1.Current as Data.Fighter;
            if (_crnt == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", _crnt.FILE_NO)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void FRqpm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 154 /* Execute Apbs_Dfin_F */),
                     new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                     {
                        Input = 
                           new XElement("App_Base",
                              new XAttribute("tablename", "Request_Parameter"),
                              new XAttribute("formcaller", GetType().Name)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DRqpm_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqst = RqstBs1.Current as Data.Request;
            if (_rqst == null) return;

            var _drqpm = DRqpmBs.Current as Data.App_Base_Define;
            if (_drqpm == null) return;

            if (RqpmBs.List.OfType<Data.Request_Parameter>().Any(rp => rp.APBS_CODE == _drqpm.CODE)) return;

            var _rqpm = RqpmBs.AddNew() as Data.Request_Parameter;
            _rqpm.APBS_CODE = _drqpm.CODE;
            _rqpm.RQST_RQID = _rqst.RQID;

            iScsc.Request_Parameters
               .InsertOnSubmit(_rqpm);

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Rqpm_Btn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _rqpm = RqpmBs.Current as Data.Request_Parameter;
            if (_rqpm == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  iScsc.Request_Parameters.DeleteOnSubmit(_rqpm);
                  break;
               case 1:
                  iScsc.ExecuteCommand(
                     string.Format(
                        "UPDATE dbo.Request_Parameter SET Cmnt = N'{1}' WHERE Code = 0",
                        _rqpm.CODE,
                        _rqpm.CMNT                        
                     )
                  );
                  break;
               default:
                  break;
            }
            

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SendFngrToEdit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, "ADM_CHNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "setcard"), new XAttribute("value", FNGR_PRNT_TextEdit.Text))},
                     new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape}
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CretMbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = MbspBs1.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            var _fgpb = FgpbsBs1.Current as Data.Fighter_Public;

            iScsc.CRET_MBSS_P(
               new XElement("Param",
                   new XAttribute("rqrorqstrqid", _mbsp.RQRO_RQST_RQID),
                   new XAttribute("rqrorwno", _mbsp.RQRO_RWNO),
                   new XAttribute("fileno", _mbsp.FIGH_FILE_NO),
                   new XAttribute("rwno", _mbsp.RWNO),
                   new XAttribute("rectcode", _mbsp.RECT_CODE),
                   new XAttribute("strtdate", _mbsp.STRT_DATE.Value.ToString("yyyy-MM-dd")),
                   new XAttribute("enddate", _mbsp.END_DATE.Value.ToString("yyyy-MM-dd")),
                   new XAttribute("attnnumb", _mbsp.NUMB_OF_ATTN_MONT),
                   new XAttribute("cbmtcode", _fgpb.CBMT_CODE)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void DelMbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = MbspBs1.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            if (_mbsp.Member_Ship_Sessions.Any() && MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;

            iScsc.ExecuteCommand(
               string.Format("DELETE dbo.Member_Ship_Session WHERE Rqro_Rqst_Rqid = {0} AND Rqro_Rwno = {1}", _mbsp.RQRO_RQST_RQID, _mbsp.RQRO_RWNO)
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveMbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Mbss_Gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowInfoMbss_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _mbsp = MbspBs1.Current as Data.Member_Ship;
            if (_mbsp == null) return;

            Adm_Tc.SelectedTab = More_Tp;
            More_Tc.SelectedTab = tp_010;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }
   }
}
