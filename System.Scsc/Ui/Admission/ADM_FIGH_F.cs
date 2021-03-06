﻿using System;
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
            }
         }
         catch { }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               Gb_Expense.Visible = true;
               //Btn_RqstDelete1.Visible = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnASav1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               //DefaultTabPage001();
               RqstBnASav1.Enabled = false;
               RQTT_CODE_LookUpEdit1.EditValue = "001";
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            //DefaultTabPage001();
            RqstBnASav1.Enabled = false;
            RQTT_CODE_LookUpEdit1.EditValue = "001";
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
                                    new XElement("Frst_Name", FRST_NAME_TextEdit.Text),
                                    new XElement("Last_Name", LAST_NAME_TextEdit.Text),
                                    new XElement("Fath_Name", FATH_NAME_TextEdit.Text),
                                    new XElement("Sex_Type", SEX_TYPE_LookUpEdit.EditValue),
                                    new XElement("Natl_Code", NATL_CODE_TextEdit.Text),
                                    new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.Value == null ? "" : BRTH_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                                    new XElement("Cell_Phon", CELL_PHON_TextEdit.Text),
                                    new XElement("Tell_Phon", TELL_PHON_TextEdit.Text),
                                    new XElement("Type", RQTT_CODE_LookUpEdit1.EditValue),
                                    new XElement("Post_Adrs", POST_ADRS_TextEdit.Text),
                                    new XElement("Emal_Adrs", ""),
                                    new XElement("Insr_Numb", ""),
                                    new XElement("Insr_Date", ""),
                                    new XElement("Educ_Deg", ""),
                                    new XElement("Cbmt_Code", CbmtCode_Lov.EditValue ?? ""),
                                    new XElement("Dise_Code", ""),
                                    new XElement("Blod_Grop", ""),
                                    new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.EditValue ?? ""),
                                    new XElement("Sunt_Bunt_Dept_Orgn_Code", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Sunt_Bunt_Dept_Code", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Sunt_Bunt_Code", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Sunt_Code", SUNT_CODELookUpEdit.EditValue ?? ""),
                                    new XElement("Cord_X", ""),
                                    new XElement("Cord_Y", ""),
                                    new XElement("Glob_Code", Glob_Code_TextEdit.EditValue ?? ""),
                                    new XElement("Chat_Id", Chat_Id_TextEdit.EditValue ?? ""),
                                    new XElement("Ctgy_Code", CtgyCode_Lov.EditValue ?? ""),
                                    new XElement("Most_Debt_Clng", ""),
                                    new XElement("Serv_No", SERV_NO_TextEdit.EditValue ?? "")
                                 ),
                                 new XElement("Member_Ship",
                                    new XAttribute("strtdate", StrtDate_DateTime001.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : StrtDate_DateTime001.Value.Value.ToString("yyyy-MM-dd")),
                                    new XAttribute("enddate", NumbOfMontDnrm_RB001.Checked ? (StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd")) : (EndDate_DateTime001.Value == null ? StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(1).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd") : EndDate_DateTime001.Value.Value.ToString("yyyy-MM-dd"))),
                                    new XAttribute("numbmontofer", NumbMontOfer_TextEdit001.Text ?? "0"),
                                    new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit001.Text ?? "0"),
                                    new XAttribute("numbofattnweek", "0"),
                                    new XAttribute("attndaytype", /*AttnDayType_Lov001.EditValue ??*/ "7")
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
            if (MessageBox.Show(this, "آیا با انصراف درخواست مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst != null && Rqst.RQID > 0)
            {
               /*
                *  Remove Data From Tables
                */
               iScsc.ADM_TCNL_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
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
               if(GustSaveRqst_PickButn.PickChecked)
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
            FNGR_PRNT_TextEdit.EditValue = 
                iScsc.Fighters
                .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                .Select(f => f.FNGR_PRNT_DNRM)
                .ToList()
                .Where(f => f.All(char.IsDigit))
                .Max(f => Convert.ToInt64(f)) + 1;
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
            //if (tb_master.SelectedTab == tp_001)
            {
               //var rqst = RqstBs1.Current as Data.Request;
               //if (rqst == null) return;

               //long mtodcode = 0;//(long)MtodCode_LookupEdit001.EditValue;
               if (RQTT_CODE_LookUpEdit1.EditValue == null || RQTT_CODE_LookUpEdit1.EditValue.ToString() == "")
                  RQTT_CODE_LookUpEdit1.EditValue = "001";

               long ctgycode = (long)CtgyCode_Lov.EditValue;
               string rqttcode = (string)RQTT_CODE_LookUpEdit1.EditValue;
               var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "001" && exp.Expense_Type.Request_Requester.RQTT_CODE == "001" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && /*exp.MTOD_CODE == mtodcode &&*/ exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

               //StrtDate_DateTime001.Value = DateTime.Now;
               //if (MessageBox.Show(this, "تعداد جلسات با احتساب یک روز در میان می باشد؟", "مشخص شدن تاریخ پایان", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(2 * (expn.NUMB_OF_ATTN_MONT - 1)));
               //else
               //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_OF_ATTN_MONT ?? 30));
               
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
            }
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

            if (UsePos_Cb.Checked)
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
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.8"), new XAttribute("funcdesc", "Add User Info"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDeleteFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
               new Job(SendType.External, "Localhost",
                  new List<Job>
                     {                  
                        new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.3.5"), new XAttribute("funcdesc", "Delete User Info"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
                     });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
      }

      private void RqstBnDuplicateFngrPrnt1_Click(object sender, EventArgs e)
      {
         try
         {
            if (FNGR_PRNT_TextEdit.Text == "") { FNGR_PRNT_TextEdit.Focus(); return; }

            Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 43 /* DeviceControlFunction */){Input = new XElement("DeviceControlFunction", new XAttribute("functype", "5.2.7.2"), new XAttribute("funcdesc", "Duplicate User Info Into All Device"), new XAttribute("enrollnumb", FNGR_PRNT_TextEdit.Text))}
               });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         catch (Exception exc) { }
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

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text);

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

            switch (RcmtType_Butn.Tag.ToString())
            {
               case "0":
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                           )
                        )
                     )
                  );
                  break;
               case "1":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked)
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
                                             new XAttribute("amnt", Convert.ToInt64(PymtAmnt_Txt.EditValue) )
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
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))
                              )
                           )
                        )
                     );
                  }
                  break;
               default:
                  break;
            }

            PymtAmnt_Txt.EditValue = null;
            PymtDate_DateTime001.Value = DateTime.Now;
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
   }
}
