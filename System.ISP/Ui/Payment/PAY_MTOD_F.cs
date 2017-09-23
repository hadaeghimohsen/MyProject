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
using System.ISP.ExceptionHandlings;
using System.Xml.Linq;

namespace System.ISP.Ui.Payment
{
   public partial class PAY_MTOD_F : UserControl
   {
      public PAY_MTOD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iISP = new Data.iISPDataContext(ConnectionString);

         PymtBs1.DataSource = iISP.Payments.Where(p => p == PymtBs1.Current);
         Te_TotlDebtAmnt.EditValue = (PymtBs1.Current as Data.Payment).SUM_EXPN_PRIC + (PymtBs1.Current as Data.Payment).SUM_EXPN_EXTR_PRCT;
         Te_TotlRemnAmnt.EditValue = (PymtBs1.Current as Data.Payment).SUM_EXPN_PRIC + (PymtBs1.Current as Data.Payment).SUM_EXPN_EXTR_PRCT - PmmtBs1.List.OfType<Data.Payment_Method>().Sum(pm => pm.AMNT);

         PmmtGC.DataSource = PmmtBs1;
      }

      #region Button Events
      private void AddPmmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if ((long)Te_TotlRemnAmnt.EditValue <= 0) return;
            PmmtBs1.AddNew();
            var pmmt = PmmtBs1.Current as Data.Payment_Method;
            pmmt.AMNT = (long)Te_TotlRemnAmnt.EditValue;
            pmmt.RCPT_MTOD = "001";
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void SavePmmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PmmtBs1.EndEdit();

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            var crnt = PmmtBs1.Current as Data.Payment_Method;
            iISP.PAY_MSAV_P(
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
            
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request

            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request

            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void DelPmmt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا با پاک کردن پرداختی موافقید؟", " حذف آیتم پرداخت", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            var Pmmt = PmmtBs1.Current as Data.Payment_Method;
            iISP.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "Delete"),
                  new XAttribute("cashcode", Pmmt.PYMT_CASH_CODE),
                  new XAttribute("rqstrqid", Pmmt.PYMT_RQST_RQID),
                  new XAttribute("rwno", Pmmt.RWNO)
               )
            );

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request

            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request

            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void AddDebtDiscount_Butn002_Click(object sender, EventArgs e)
      {
         try
         {
            StatusSaving_Sic.StateIndex = 2; // Saving Request

            var pymt = iISP.Payments.FirstOrDefault(p => p == PymtBs1.Current);
            if (pymt.SUM_EXPN_PRIC <= 0)
            {
               MessageBox.Show("هزینه برای هنرجو صفر میباشد. نیازی به محاسبه تخفیف مانده حساب نیست");
               return;
            }

            if (pymt.SUM_EXPN_PRIC - Convert.ToInt32(CashByDeposit_Txt002.EditValue == "" ? "0" : CashByDeposit_Txt002.EditValue) < 0)
            {
               if (MessageBox.Show(this, "مبلغ تخفیف وارد شده بیشتر از مبلغ هزینه جاری می باشد. آیا مایل به پایاپای کردن مبلغ تخفیف با مانده بدهی می باشید؟", "مبلغ بیرون از سقف بدهی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
               CashByDeposit_Txt002.EditValue = pymt.SUM_EXPN_PRIC;
            }

            if (CashByDeposit_Txt002.Text != "0")
            {
               PydsBs2.AddNew();
               var despositamnt = PydsBs2.Current as Data.Payment_Discount;
               despositamnt.Payment = pymt;
               despositamnt.RQRO_RWNO = 1;

               despositamnt.AMNT_TYPE = "003"; // تخفیف بابت مبلغ بستانکاری
               despositamnt.AMNT = Convert.ToInt32(CashByDeposit_Txt002.EditValue);
               despositamnt.PYDS_DESC = "تخفیف بخاطر مانده حساب از قبل";
               despositamnt.STAT = "002";

               iISP.SubmitChanges();               

               CashByDeposit_Txt002.Text = "0";             

               requery = true;
            }

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request

            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void AddPyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if ((long)Te_TotlRemnAmnt.EditValue <= 0) return;
            PydsBs2.AddNew();
            var pyds = PydsBs2.Current as Data.Payment_Discount;
            pyds.AMNT = Convert.ToInt32(Te_TotlRemnAmnt.EditValue);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydsBs2.EndEdit();

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            iISP.SubmitChanges();
            
            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request

            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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

      private void DelPyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pyds = PydsBs2.Current as Data.Payment_Discount;

            if (pyds == null) return;

            if (MessageBox.Show(this, "آیا با حذف شی مورد نظر موافقید؟", "عملیات حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            StatusSaving_Sic.StateIndex = 2; // Saving Request

            iISP.Payment_Discounts.DeleteOnSubmit(pyds);

            iISP.SubmitChanges();

            StatusSaving_Sic.StateIndex = 3; // Successfull Save Request

            requery = true;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request

            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
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
      #endregion

      #region BindingSource Event
      private void BindingSource_ListChanged(object sender, ListChangedEventArgs e)
      {
         try
         {
            if (requery)
               return;

            StatusSaving_Sic.StateIndex = 2;
         }
         catch (Exception exc)
         {
            StatusSaving_Sic.StateIndex = 1; // Error On Saved Request
            MessageBox.Show(exc.Message);
            iISP.SaveException(exc);
         }
      }
      #endregion
   }
}
