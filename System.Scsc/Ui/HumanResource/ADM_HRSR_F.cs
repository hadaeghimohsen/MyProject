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

namespace System.Scsc.Ui.HumanResource
{
   public partial class ADM_HRSR_F : UserControl
   {
      public ADM_HRSR_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private int rqstindex = default(int);

      private void Execute_Query()
      {
         setOnDebt = false;
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);

               rqstindex = RqstBs1.Position;

               var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "022" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

               RqstBs1.DataSource =
                  iScsc.Requests
                  .Where(
                     rqst =>
                        Rqids.Contains(rqst.RQID)
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
               RqstBnASav1.Enabled = true; // false
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
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            //DefaultTabPage001();
            RqstBnASav1.Enabled = false;
         }
      }

      private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;
            rqstindex = RqstBs1.Position;

            if (Rqst == null || Rqst.RQID >= 0)
            {
               if (NumbOfMontDnrm_TextEdit001.Text.Trim() == "")
                  NumbOfMontDnrm_TextEdit001.Text = "1";

               if (Rqst == null || Rqst.RQST_STAT == null || Rqst.RQST_STAT == "001")
                  iScsc.HRS_TRQT_P(
                        new XElement("Process",
                           new XElement("Request",
                              new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                              new XAttribute("rqtpcode", "022"),
                              new XAttribute("rqttcode", "004"),
                              new XAttribute("prvncode", "017"),
                              new XAttribute("regncode", "001"),
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
                                 new XElement("Type", "004"),
                                 new XElement("Post_Adrs", POST_ADRS_TextEdit.Text),
                                 new XElement("Emal_Adrs", EMAL_ADRS_TextEdit.Text),
                                 new XElement("Insr_Numb", INSR_NUMB_TextEdit.Text),
                                 new XElement("Insr_Date", INSR_DATE_PersianDateEdit.Value == null ? "" : INSR_DATE_PersianDateEdit.Value.Value.ToString("yyyy-MM-dd")),
                                 new XElement("Educ_Deg", EDUC_DEG_LookUpEdit.EditValue ?? ""),
                                 //new XElement("Cbmt_Code", MtodCode_LookupEdit001.EditValue ?? ""),
                                 new XElement("Dise_Code", DISE_CODE_LookUpEdit.EditValue ?? ""),
                                 new XElement("Blod_Grop", BLOD_GROPLookUpEdit.EditValue ?? ""),
                                 new XElement("Fngr_Prnt", FNGR_PRNT_TextEdit.EditValue ?? ""),
                                 new XElement("Sunt_Bunt_Dept_Orgn_Code", SUNT_BUNT_DEPT_ORGN_CODELookUpEdit.EditValue ?? ""),
                                 new XElement("Sunt_Bunt_Dept_Code", SUNT_BUNT_DEPT_CODELookUpEdit.EditValue ?? ""),
                                 new XElement("Sunt_Bunt_Code", SUNT_BUNT_CODELookUpEdit.EditValue ?? ""),
                                 new XElement("Sunt_Code", SUNT_CODELookUpEdit.EditValue ?? ""),
                                 new XElement("Cord_X", CORD_XTextEdit.EditValue ?? ""),
                                 new XElement("Cord_Y", CORD_YTextEdit.EditValue ?? ""),
                                 new XElement("Mtod_Code", MtodCode_LookupEdit001.EditValue ?? ""),
                                 new XElement("Ctgy_Code", CtgyCode_LookupEdit001.EditValue ?? ""),
                                 new XElement("Most_Debt_Clng", SE_MostDebtClngAmnt.Value),
                                 new XElement("Serv_No", SERV_NO_TextEdit.EditValue ?? ""),
                                 new XElement("Cntr_Code", Rqst == null ? 0 : Rqst.RQID)
                              ),
                              new XElement("Member_Ship",
                                 new XAttribute("strtdate", StrtDate_DateTime001.Value == null ? DateTime.Now.ToString("yyyy-MM-dd") : StrtDate_DateTime001.Value.Value.ToString("yyyy-MM-dd")),
                                 new XAttribute("enddate", NumbOfMontDnrm_RB001.Checked ? (StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(Convert.ToInt32(NumbOfMontDnrm_TextEdit001.EditValue)).ToString("yyyy-MM-dd")) : (EndDate_DateTime001.Value == null ? StrtDate_DateTime001.Value != null ? StrtDate_DateTime001.Value.Value.AddMonths(1).ToString("yyyy-MM-dd") : DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd") : EndDate_DateTime001.Value.Value.ToString("yyyy-MM-dd")))
                                 //new XAttribute("numbmontofer", NumbMontOfer_TextEdit001.Text ?? "0"),
                                 //new XAttribute("numbofattnmont", NumbOfAttnMont_TextEdit001.Text ?? "0"),
                                 //new XAttribute("numbofattnweek", NumbOfAttnWeek_TextEdit001.Text ?? "0"),
                                 //new XAttribute("attndaytype", "7")
                              )
                           )
                        )
                     );
               else if (Rqst.RQST_STAT == "002") return;
               //MessageBox.Show(this, "هنرجوی جدید در سیستم ثبت گردید");
               requery = true;

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
            if (MessageBox.Show(this, "آیا با انصراف و حذف ثبت نام مطمئن هستید؟", "هشدار!", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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
               //MessageBox.Show(this, "هنرجو حذف گردید!");
            }
            requery = true;
            tc_pblc.SelectedTab = tp_pblcinfo;
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
               iScsc.HRS_TSAV_F(
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
               tc_pblc.SelectedTab = tp_pblcinfo;
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

      private void MaxF_Butn001_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               FNGR_PRNT_TextEdit.EditValue = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0).Max(f => Convert.ToInt32(f.FNGR_PRNT_DNRM)) + 1;
            }
         }
         catch
         {
            if (tb_master.SelectedTab == tp_001)
            {
               FNGR_PRNT_TextEdit.EditValue = 1;
            }
         }
      }

      private void Btn_AutoCalcAttn_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               //var rqst = RqstBs1.Current as Data.Request;
               //if (rqst == null) return;

               //long mtodcode = 0;//(long)MtodCode_LookupEdit001.EditValue;
               long ctgycode = (long)CtgyCode_LookupEdit001.EditValue;

               var expn = iScsc.Expenses.Where(exp => exp.Expense_Type.Request_Requester.RQTP_CODE == "022" && exp.Expense_Type.Request_Requester.RQTT_CODE == "004" && exp.Expense_Type.Request_Requester.Regulation.REGL_STAT == "002" && exp.Expense_Type.Request_Requester.Regulation.TYPE == "001" && /*exp.MTOD_CODE == mtodcode &&*/ exp.CTGY_CODE == ctgycode && exp.EXPN_STAT == "002").FirstOrDefault();

               StrtDate_DateTime001.Value = DateTime.Now;
               //if (MessageBox.Show(this, "تعداد جلسات با احتساب یک روز در میان می باشد؟", "مشخص شدن تاریخ پایان", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
               //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(2 * (expn.NUMB_OF_ATTN_MONT - 1)));
               //else
               //   EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_OF_ATTN_MONT ?? 30));
               EndDate_DateTime001.Value = DateTime.Now.AddDays((double)(expn.NUMB_CYCL_DAY ?? 30));
               NumbOfAttnMont_TextEdit001.EditValue = expn.NUMB_OF_ATTN_MONT ?? 0;
               NumbOfAttnWeek_TextEdit001.EditValue = expn.NUMB_OF_ATTN_WEEK ?? 0;
               NumbMontOfer_TextEdit001.EditValue = expn.NUMB_MONT_OFER ?? 0;
            }
         }
         catch (Exception exc)
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
            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات استخدام و ذخیره نهایی کردن انجام شود؟", "ثبت استخدام و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
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
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void tbn_POSPayment1_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var rqst = RqstBs1.Current as Data.Request;
            var pymt = PymtsBs1.Current as Data.Payment;

            var xSendPos =
               new XElement("Form",
                  new XAttribute("name", GetType().Name),
                  new XAttribute("tabpage", "tp_001"),
                  new XElement("Request",
                     new XAttribute("rqid", rqst.RQID),
                     new XAttribute("rqtpcode", rqst.RQTP_CODE),
                     new XAttribute("fileno", rqst.Fighters.FirstOrDefault().FILE_NO),
                     new XElement("Payment",
                        new XAttribute("cashcode", pymt.CASH_CODE),
                        new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
                     )
                  )
               );

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 93 /* Execute Pos_Totl_F */),
                     new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void Btn_InDebt001_Click(object sender, EventArgs e)
      {
         try
         {
            setOnDebt = true;

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

            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات بدهکاری و ذخیره نهایی کردن انجام شود؟", "بدهکاری و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
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
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void bn_PaymentMethods1_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs1.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) { Input = pymt }
            );
         }
      }

      private void RqstBnADoc_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
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
            if (tb_master.SelectedTab == tp_001)
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

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
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
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
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
         if (tb_master.SelectedTab == tp_001)
         {
            var Rg1 = iScsc.Regulations.Where(r => r.REGL_STAT == "002" && r.TYPE == "001").Single();
            if (Rg1 == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 06 /* Execute Regl_Dcmt_F */){Input = new List<Data.Regulation>{Rg1, null}},
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "022")))}
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

      private void MtodCode_LookupEdit001_Popup(object sender, EventArgs e)
      {
         try
         {
            var mtod = MtodCode_LookupEdit001.EditValue;

            if (mtod == null || mtod.ToString() == "") return;            

            CtgyBs1.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long)mtod);
         }
         catch (Exception exc)
         {
                        
         }
      }

      private void BTN_MBSP_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         
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
               EndDate_DateTime001.Value = EndDate_DateTime001.Value.Value.AddMonths(1);
               break;
            case 0:
               EndDate_DateTime001.Value = EndDate_DateTime001.Value.Value.AddMonths(-1);
               break;
         }
      }

   }
}
