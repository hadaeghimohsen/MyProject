using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.PaymentMethod
{
   public partial class PAY_MTOD_F : UserControl
   {
      public PAY_MTOD_F()
      {
         InitializeComponent();
      }
      bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         PymtBs1.DataSource = iScsc.Payments.Where(p => p == PymtBs1.Current);
         PydtBs3.DataSource = iScsc.Payment_Details.Where(pd => pd.Payment == PymtBs1.Current && pd.ADD_QUTS == "002");
         Te_TotlDebtAmnt.EditValue = (PymtBs1.Current as Data.Payment).SUM_EXPN_PRIC + (PymtBs1.Current as Data.Payment).SUM_EXPN_EXTR_PRCT - ((PymtBs1.Current as Data.Payment).SUM_PYMT_DSCN_DNRM ?? 0);
         Te_TotlRemnAmnt.EditValue = (PymtBs1.Current as Data.Payment).SUM_EXPN_PRIC + (PymtBs1.Current as Data.Payment).SUM_EXPN_EXTR_PRCT - (((PymtBs1.Current as Data.Payment).SUM_PYMT_DSCN_DNRM ?? 0) + PmmtBs1.List.OfType<Data.Payment_Method>().Sum(pm => pm.AMNT));
         RemindAmnt_Txt002.EditValue = (long)DEBT_DNRMTextEdit.EditValue + Convert.ToInt64(CashByDeposit_Txt002.Text.Replace(",", "")) + PydsBs2.List.OfType<Data.Payment_Discount>().Where(pd => pd.STAT == "002" && pd.AMNT_TYPE == "003").Sum(pd => pd.AMNT);
      }

      private void PmmtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  e.Handled = true;
                  if (PmmtBs1.List.OfType<Data.Payment_Method>().Any(p => p.RWNO == 0)) return;

                  PmmtBs1.AddNew();
                  var crntp = PmmtBs1.Current as Data.Payment_Method;
                  crntp.AMNT = (long)Te_TotlRemnAmnt.EditValue;
                  crntp.RCPT_MTOD = "003";
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن پرداختی موافقید؟", " حذف آیتم پرداخت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  var Pmmt = PmmtBs1.Current as Data.Payment_Method;
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "Delete"),
                        new XAttribute("cashcode", Pmmt.PYMT_CASH_CODE),
                        new XAttribute("rqstrqid", Pmmt.PYMT_RQST_RQID),
                        new XAttribute("rwno", Pmmt.RWNO)
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  /* اگر از اطلاعاتی که می خواهیم ذخیره کنیم یکی از طریق پوز باشد باید ابتدا عملیات پوز را انجام دهیم 
                   * البته به ترتیب و اطلاعات بدست آماده از پوز رو یکی بررسی میکنیم. البته شرط وارد شدن به آیتم بعدی این هست که
                   * تمامی ردیف ها به درستی کار خود را انجام داده باشند
                   */
                  //bool oprt = true;

                  //if (PmmtBs1.List.OfType<Data.Payment_Method>().Where(c => c.CRET_BY == null && c.RCPT_MTOD == "003").Count() >= 1)
                  //   oprt = false;

                  //PmmtBs1.List.OfType<Data.Payment_Method>().Where(c => c.CRET_BY == null && c.RCPT_MTOD == "003")
                  //   .ToList()
                  //   .ForEach(p =>
                  //   {
                  //      Job _InteractWithScsc =
                  //        new Job(SendType.External, "Localhost",
                  //           new List<Job>
                  //            {                  
                  //               new Job(SendType.Self, 93 /* Execute Pay_Cash_F */),
                  //               new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Execute Actn_CalF_F */)
                  //               {
                  //                  Input = new XElement("Amnt", p.AMNT),
                  //                  AfterChangedOutput = (output) =>
                  //                  {
                  //                     XDocument xdoc = XDocument.Parse(string.Format("<Pos>{0}</Pos>", output));
                  //                     p.TERM_NO = xdoc.Descendants("TerminalId").FirstOrDefault().Value;
                  //                     p.TRAN_NO = xdoc.Descendants("SerialNumber").FirstOrDefault().Value;
                  //                     p.CARD_NO = xdoc.Descendants("CustomerPan").FirstOrDefault().Value;
                  //                     p.FLOW_NO = xdoc.Descendants("TraceNumber").FirstOrDefault().Value;
                  //                     p.REF_NO = xdoc.Descendants("ReferenceNumber").FirstOrDefault().Value;
                  //                     oprt = true;
                  //                  }
                  //               }
                  //            });
                  //      _DefaultGateway.Gateway(_InteractWithScsc);
                  //   });

                  //if(!oprt)
                  //{
                  //   MessageBox.Show("عملیات پرداخت کارت خوان ناموفق بوده");
                  //   return;
                  //}
                  // 1395/08/10 * چک کردن این مبلغ پرداختی از سپرده هنرجو بیشتر نباشد
                  var figh = iScsc.Fighters.First(f => f.Request_Rows.Any(rr => rr.RQST_RQID == (PymtBs1.Current as Data.Payment).RQST_RQID));
                  if (figh.DEBT_DNRM < 0)
                  {
                     var crntpay = PmmtBs1.Current as Data.Payment_Method;
                     if (iScsc.Payment_Methods.Where(pm => pm.PYMT_RQST_RQID == (PymtBs1.Current as Data.Payment).RQST_RQID && pm.RCPT_MTOD == "005").Sum(pm => pm.AMNT) + (crntpay.RCPT_MTOD == "005" ? crntpay.AMNT : 0) > -1 * figh.DEBT_DNRM)
                     {
                        MessageBox.Show(this, "مبلغ برداشتی از حساب هنرجو بیش از مبلغ سپرده می باشد", "");
                        return;
                     }
                  }

                  var crnt = PmmtBs1.Current as Data.Payment_Method;
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        PmmtBs1.List.OfType<Data.Payment_Method>().Where(c => c.CRET_BY == null).Select(c =>
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", (PymtBs1.Current as Data.Payment).CASH_CODE),
                                 new XAttribute("rqstrqid", (PymtBs1.Current as Data.Payment).RQST_RQID),
                                 new XAttribute("amnt", (PmmtBs1.Current as Data.Payment_Method).AMNT ?? 0),
                                 new XAttribute("rcptmtod", (PmmtBs1.Current as Data.Payment_Method).RCPT_MTOD ?? "001"),
                                 new XAttribute("termno", (PmmtBs1.Current as Data.Payment_Method).TERM_NO ?? ""),
                                 new XAttribute("tranno", (PmmtBs1.Current as Data.Payment_Method).TRAN_NO ?? ""),
                                 new XAttribute("cardno", (PmmtBs1.Current as Data.Payment_Method).CARD_NO ?? ""),
                                 new XAttribute("bank", (PmmtBs1.Current as Data.Payment_Method).BANK ?? ""),
                                 new XAttribute("flowno", (PmmtBs1.Current as Data.Payment_Method).FLOW_NO ?? ""),
                                 new XAttribute("refno", (PmmtBs1.Current as Data.Payment_Method).REF_NO ?? ""),
                                 new XAttribute("actndate", (PmmtBs1.Current as Data.Payment_Method).ACTN_DATE.HasValue ? (PmmtBs1.Current as Data.Payment_Method).ACTN_DATE.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd"))

                              )
                           )
                        ),
                        crnt.CRET_BY != null /*&& crnt.RCPT_MTOD != "003"*/ ?
                           new XElement("Update",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", (PymtBs1.Current as Data.Payment).CASH_CODE),
                                 new XAttribute("rqstrqid", (PymtBs1.Current as Data.Payment).RQST_RQID),
                                 new XAttribute("rwno", (PmmtBs1.Current as Data.Payment_Method).RWNO),
                                 new XAttribute("amnt", (PmmtBs1.Current as Data.Payment_Method).AMNT ?? 0),
                                 new XAttribute("rcptmtod", (PmmtBs1.Current as Data.Payment_Method).RCPT_MTOD ?? "001"),
                                 new XAttribute("termno", (PmmtBs1.Current as Data.Payment_Method).TERM_NO ?? ""),
                                 new XAttribute("tranno", (PmmtBs1.Current as Data.Payment_Method).TRAN_NO ?? ""),
                                 new XAttribute("cardno", (PmmtBs1.Current as Data.Payment_Method).CARD_NO ?? ""),
                                 new XAttribute("bank", (PmmtBs1.Current as Data.Payment_Method).BANK ?? ""),
                                 new XAttribute("flowno", (PmmtBs1.Current as Data.Payment_Method).FLOW_NO ?? ""),
                                 new XAttribute("refno", (PmmtBs1.Current as Data.Payment_Method).REF_NO ?? ""),
                                 new XAttribute("actndate", (PmmtBs1.Current as Data.Payment_Method).ACTN_DATE.HasValue ? (PmmtBs1.Current as Data.Payment_Method).ACTN_DATE.Value.Date : DateTime.Now.Date)
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
                  MessageBox.Show(se.Message);
                  break;
            }
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

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void PydsAdd_Butn_Click(object sender, EventArgs e)
      {
         if (PydsBs2.List.OfType<Data.Payment_Discount>().Any(p => p.RWNO == 0)) return;
         PydsBs2.AddNew();
      }

      private void PydsDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pyds = PydsBs2.Current as Data.Payment_Discount;

            if (pyds == null) return;

            if (MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            if (pyds.RWNO != 0)
            {
               iScsc.Payment_Discounts.DeleteOnSubmit(pyds);

               iScsc.SubmitChanges();
            }
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

      private void PydsSave_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.SubmitChanges();
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

      private void PydtAdd_Butn_Click(object sender, EventArgs e)
      {
         PydtBs3.AddNew();
         var pydt = PydtBs3.Current as Data.Payment_Detail;
         var pymt = PymtBs1.Current as Data.Payment;
         pydt.ADD_QUTS = "002";
         pydt.PAY_STAT = "001";
         pydt.PYMT_CASH_CODE = pymt.CASH_CODE;
         pydt.PYMT_RQST_RQID = pymt.RQST_RQID;
         pydt.RQRO_RWNO = 1;
         pydt.RCPT_MTOD = "001";
      }
      
      private void PydtDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pydt = PydtBs3.Current as Data.Payment_Detail;

            if (pydt == null) return;

            if (MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Payment_Details.DeleteOnSubmit(pydt);

            iScsc.SubmitChanges();
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

      private void PydtSave_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var newRec = PydtBs3.List.OfType<Data.Payment_Detail>().Where(pd => pd.CRET_BY == null);
            var oldRec = iScsc.Payment_Details.Where(pd => pd.Payment == PymtBs1.Current);
            if(newRec.Join(oldRec, n => n.EXPN_CODE, o => o.EXPN_CODE, (n, o) => n.EXPN_CODE).Any())
            {
               MessageBox.Show("برای این نوع درآمد قبلا ردیف به صورت سیستمی یا دستی وارد شده لطفا آیتم درآمدی دیگری را انتخاب کنید");
               return;
            }
            iScsc.SubmitChanges();
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

      private void Expn_Lov_EditValueChanged(object sender, EventArgs e)
      {
         var pydt = PydtBs3.Current as Data.Payment_Detail;

         if (((GridLookUpEdit)sender).EditValue != null)
         {
            if((pydt.EXPN_PRIC ?? 0) == 0)
            {
               ExpnBs3.List.OfType<Data.Expense>().Where(ex => ex.CODE == (long)((GridLookUpEdit)sender).EditValue).ToList().ForEach(ex => { pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; });
            }
         }
      }

      private void CashByDeposit_Txt002_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (DEBT_DNRMTextEdit.Text != "0")
            {
               if ((long)DEBT_DNRMTextEdit.EditValue + Convert.ToInt64(e.NewValue) > 0)
               {
                  MessageBox.Show("میزان مبلغ وارد شده برای تخفیف از میزان سپرده هنرجو بیشتر می باشد");
                  e.Cancel = true;
               }
               else
                  RemindAmnt_Txt002.EditValue = (long)DEBT_DNRMTextEdit.EditValue + Convert.ToInt64(e.NewValue) + PydsBs2.List.OfType<Data.Payment_Discount>().Where(pd => pd.STAT == "002" && pd.AMNT_TYPE == "003").Sum(pd => pd.AMNT);               
            }
            else
               e.Cancel = true;
         }
         catch
         {
            CashByDeposit_Txt002.EditValue = 0;
         }
      }

      private void AddDebtDiscount_Butn002_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = iScsc.Payments.FirstOrDefault(p => p == PymtBs1.Current);
            if(pymt.SUM_EXPN_PRIC <= 0)
            {
               MessageBox.Show("هزینه برای هنرجو صفر میباشد. نیازی به محاسبه تخفیف مانده حساب نیست");
               return;
            }

            if (pymt.SUM_EXPN_PRIC - Convert.ToInt32(CashByDeposit_Txt002.EditValue) < 0)
            {
               if (MessageBox.Show(this, "مبلغ تخفیف وارد شده بیشتر از مبلغ هزینه جاری می باشد. آیا مایل به پایاپای کردن مبلغ تخفیف با مانده بدهی می باشید؟", "مبلغ بیرون از سقف بدهی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                   CashByDeposit_Txt002.EditValue = pymt.SUM_EXPN_PRIC;
            }

            if(CashByDeposit_Txt002.Text != "0")
            {
               PydsBs2.AddNew();
               var despositamnt = PydsBs2.Current as Data.Payment_Discount;
               despositamnt.Payment = pymt;
               despositamnt.RQRO_RWNO = 1;

               despositamnt.AMNT_TYPE = "003"; // تخفیف بابت مبلغ بستانکاری
               despositamnt.AMNT = Convert.ToInt32(CashByDeposit_Txt002.EditValue);
               despositamnt.PYDS_DESC = "تخفیف بخاطر مانده حساب از قبل";
               despositamnt.STAT = "002";

               iScsc.SubmitChanges();
               requery = true;

               CashByDeposit_Txt002.Text = "0";
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void AddPmtc_Butn_Click(object sender, EventArgs e)
      {
         PmtcBs4.AddNew();
         var pmtc = PmtcBs4.Current as Data.Payment_Check;
         var pymt = PymtBs1.Current as Data.Payment;
         pmtc.CHEK_TYPE = "000";
         pmtc.PYMT_CASH_CODE = pymt.CASH_CODE;
         pmtc.PYMT_RQST_RQID = pymt.RQST_RQID;
         pmtc.RQRO_RQST_RQID = pymt.RQST_RQID;
         pmtc.RQRO_RWNO = 1;
      }

      private void DelPmtc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pmtc = PmtcBs4.Current as Data.Payment_Check;

            if (pmtc == null) return;

            if (MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iScsc.Payment_Checks.DeleteOnSubmit(pmtc);

            iScsc.SubmitChanges();
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

      private void SavePmtc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PmtcBs4.EndEdit();
            iScsc.SubmitChanges();
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

      private void ChekOK_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pmtc = PmtcBs4.Current as Data.Payment_Check;
            if(pmtc.CHEK_TYPE == "002")return;
            if(pmtc.RCPT_DATE == null)
            {
               MessageBox.Show("تاریخ پرداخت چک مشخص نیست");
               return;
            }

            pmtc.CHEK_TYPE = "002";

            PmmtBs1.AddNew();
            var crntp = PmmtBs1.Current as Data.Payment_Method;
            crntp.AMNT = pmtc.AMNT;
            crntp.RCPT_MTOD = "007";
            crntp.ACTN_DATE = pmtc.RCPT_DATE;
            crntp.BANK = pmtc.BANK;
            crntp.CARD_NO = pmtc.CHEK_NO;

            var figh = iScsc.Fighters.First(f => f.Request_Rows.Any(rr => rr.RQST_RQID == (PymtBs1.Current as Data.Payment).RQST_RQID));
            if (figh.DEBT_DNRM < 0)
            {
               var crntpay = PmmtBs1.Current as Data.Payment_Method;
               if (iScsc.Payment_Methods.Where(pm => pm.PYMT_RQST_RQID == (PymtBs1.Current as Data.Payment).RQST_RQID && pm.RCPT_MTOD == "005").Sum(pm => pm.AMNT) + (crntpay.RCPT_MTOD == "005" ? crntpay.AMNT : 0) > -1 * figh.DEBT_DNRM)
               {
                  MessageBox.Show(this, "مبلغ برداشتی از حساب هنرجو بیش از مبلغ سپرده می باشد", "");
                  return;
               }
            }

            var crnt = PmmtBs1.Current as Data.Payment_Method;
            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "InsertUpdate"),
                  PmmtBs1.List.OfType<Data.Payment_Method>().Where(c => c.CRET_BY == null).Select(c =>
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", (PymtBs1.Current as Data.Payment).CASH_CODE),
                           new XAttribute("rqstrqid", (PymtBs1.Current as Data.Payment).RQST_RQID),
                           new XAttribute("amnt", (PmmtBs1.Current as Data.Payment_Method).AMNT ?? 0),
                           new XAttribute("rcptmtod", (PmmtBs1.Current as Data.Payment_Method).RCPT_MTOD ?? "001"),
                           new XAttribute("termno", (PmmtBs1.Current as Data.Payment_Method).TERM_NO ?? ""),
                           new XAttribute("tranno", (PmmtBs1.Current as Data.Payment_Method).TRAN_NO ?? ""),
                           new XAttribute("cardno", (PmmtBs1.Current as Data.Payment_Method).CARD_NO ?? ""),
                           new XAttribute("bank", (PmmtBs1.Current as Data.Payment_Method).BANK ?? ""),
                           new XAttribute("flowno", (PmmtBs1.Current as Data.Payment_Method).FLOW_NO ?? ""),
                           new XAttribute("refno", (PmmtBs1.Current as Data.Payment_Method).REF_NO ?? "")

                        )
                     )
                  ),
                  crnt.CRET_BY != null && crnt.RCPT_MTOD != "003" ?
                     new XElement("Update",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", (PymtBs1.Current as Data.Payment).CASH_CODE),
                           new XAttribute("rqstrqid", (PymtBs1.Current as Data.Payment).RQST_RQID),
                           new XAttribute("rwno", (PmmtBs1.Current as Data.Payment_Method).RWNO),
                           new XAttribute("amnt", (PmmtBs1.Current as Data.Payment_Method).AMNT ?? 0)
                        )
                    ) : new XElement("Update")

               )
            );

            iScsc.SubmitChanges();
            requery = true;
         }
         catch 
         {}
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void ChekCancel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = PmtcBs4.Current as Data.Payment_Check;
            if (crnt.CHEK_TYPE == "003") return;

            if(crnt.CHEK_TYPE == "002")
            {
               MessageBox.Show("جک هایی که پرداخت شده اند دیگر فادر به برگشت نیستند");
               return;
            }

            crnt.CHEK_TYPE = "003";
            PymtBs1.EndEdit();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch 
         {}
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void SaveDiscountAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Convert.ToInt32(Te_TotlRemnAmnt.EditValue) == 0) return;

            PydsAdd_Butn_Click(null, null);

            var pyds = PydsBs2.Current as Data.Payment_Discount;
            pyds.AMNT = Convert.ToInt32(Te_TotlRemnAmnt.EditValue);

            PydsBs2.EndEdit();

            PydsSave_Butn_Click(null, null);
            requery = true;
         }
         catch { }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void SaveDifferenceAmnt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (Convert.ToInt32(Te_TotlRemnAmnt.EditValue) == 0) return;

            if (!CalcDiffAmnt_Cb.Checked)
            {
               PydsAdd_Butn_Click(null, null);

               var pyds = PydsBs2.Current as Data.Payment_Discount;
               pyds.AMNT = Convert.ToInt32(Te_TotlRemnAmnt.EditValue);
               pyds.AMNT_TYPE = "004";
               pyds.PYDS_DESC = "کسر مبلغ مابه التفاوت شهریه";

               PydsBs2.EndEdit();
            }
            else
            {
               var pymt = PymtBs1.Current as Data.Payment;
               if (pymt == null) return;

               if(pymt.Request.RQTP_CODE == "009")
               {
                  // 1396/08/18 * اگر درخواست تمدید باشد و فرد بخواهد از کلاسی به کلاس دیگر تغییر جلسه بدهد می توانیم محاسبه مبلغ مابه التفاوت را مشخص کنیم
                  var rqstchng = iScsc.VF_Request_Changing(pymt.Request.Request_Rows.FirstOrDefault().FIGH_FILE_NO).Where(r => r.RQTT_CODE != "004" && (r.RQTP_CODE == "001" || r.RQTP_CODE == "009")).OrderByDescending(r => r.SAVE_DATE).FirstOrDefault();
                  if (rqstchng == null) return;

                  var sumpric = rqstchng.TOTL_AMNT;
                  var sumrcptpric = rqstchng.TOTL_RCPT_AMNT;
                  var sumdsctpric = rqstchng.TOTL_DSCT_AMNT;

                  if(rqstchng.RQTP_CODE == "001")
                     rqstchng = iScsc.VF_Request_Changing(pymt.Request.Request_Rows.FirstOrDefault().FIGH_FILE_NO).FirstOrDefault(r => r.RQST_RQID == rqstchng.RQID);

                  if (
                     !iScsc.Member_Ships.Any(
                        m => m.RQRO_RQST_RQID == rqstchng.RQID &&
                             m.RECT_CODE == "004" &&
                             m.STRT_DATE.Value.Date == iScsc.Member_Ships.FirstOrDefault(mt => mt.RQRO_RQST_RQID == pymt.RQST_RQID && mt.RECT_CODE == "001").STRT_DATE.Value.Date
                     )
                  ) return;

                  if (sumrcptpric == 0) return;

                  PydsAdd_Butn_Click(null, null);

                  var pyds = PydsBs2.Current as Data.Payment_Discount;
                  pyds.AMNT = Convert.ToInt32(sumrcptpric);
                  pyds.AMNT_TYPE = "004";
                  pyds.PYDS_DESC = "کسر مبلغ مابه التفاوت شهریه پرداختی از قبل بابت جابه جایی کلاس";

                  PydsBs2.EndEdit();
               }
            }

            PydsSave_Butn_Click(null, null);
            requery = true;
         }
         catch { }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }      
   }
}
