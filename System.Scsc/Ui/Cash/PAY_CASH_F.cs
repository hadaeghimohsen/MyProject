using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.Cash
{
   public partial class PAY_CASH_F : UserControl
   {
      public PAY_CASH_F()
      {
         InitializeComponent();
      }
      private bool requery = false;

      private void Btn_CrntCashPayment_Click(object sender, EventArgs e)
      {
         try
         {            
            PmmtBs1.DataSource = 
               iScsc.VF_Payment_Delivers(
                  new XElement("Process", 
                     new XElement("Payment",
                        new XAttribute("fromdate", cb_cashincometoday.Status ? DateTime.Now.ToString("yyyy-MM-dd") : Pdt_CashInPaymentFromDate.Value == null ? "" : Pdt_CashInPaymentFromDate.Value.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("todate", cb_cashincometoday.Status ? DateTime.Now.ToString("yyyy-MM-dd") : Pdt_CashInPaymentToDate.Value == null ? "" : Pdt_CashInPaymentToDate.Value.Value.ToString("yyyy-MM-dd")), 
                        new XAttribute("delvstat", "001")
                     )
                  )
               );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_PayDelv_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا از بستن صندوق و تحویل درآمد صندوق مطمدن هستید؟", "بستن صندوق", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

         /*
         if (Cb_CloseCrntCashDateNow.Checked || Pdt_CashPaymentFromDate.DateTime.ToString("yyyy-MM-dd") == "0001-01-01" && MessageBox.Show(this, "تاریخ شروع درآمد صندوق مشخص نشده میخواهید تاریخ امروز قرار گیرد؟", "تاریخ شروع بستن صندوق", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            Pdt_CashPaymentFromDate.DateTime = DateTime.Now;
         else
            Pdt_CashPaymentFromDate.EditValue = DateTime.Now.AddDays(-1);

         if (Cb_CloseCrntCashDateNow.Checked || Pdt_CashPaymentToDate.DateTime.ToString("yyyy-MM-dd") == "0001-01-01" && MessageBox.Show(this, "تاریخ پایان درآمد صندوق مشخص نشده میخواهید تاریخ امروز قرار گیرد؟", "تاریخ پایان بستن صندوق", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            Pdt_CashPaymentToDate.DateTime = DateTime.Now;
         else
            Pdt_CashPaymentToDate.DateTime = DateTime.MaxValue;
         */

         try
         {
            iScsc.PAY_DELV_P(
               new XElement("Process",
                  new XElement("Payment",
                     new XAttribute("fromdate", cb_cashoutcometoday.Status ? DateTime.Now.ToString("yyyy-MM-dd") : Pdt_CashOutPaymentFromDate.Value == null ? "" : Pdt_CashOutPaymentFromDate.Value.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("todate", cb_cashoutcometoday.Status ? DateTime.Now.ToString("yyyy-MM-dd") : Pdt_CashOutPaymentToDate.Value == null ? "" : Pdt_CashOutPaymentToDate.Value.Value.ToString("yyyy-MM-dd")),
                     new XAttribute("cashby", CashBy_LookUpEdit.EditValue)
                  )
               )
            );
            MessageBox.Show(this, "درآمد صندوق تحویل داده شده. سند دریافت و پرداخت صندوق ذخیره گردید.");
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
               iScsc = new Data.iScscDataContext(ConnectionString);
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Execute_Query()
      {
         if (tb_master.SelectedTab == tp_002)
         {
            VCashBs1.DataSource = iScsc.VF_Cashiers.ToList();
            te_IN.Text = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));
         }
      }

      private void LoadCashBy_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void bt_crntcash_Click(object sender, EventArgs e)
      {
         if (CashBy_LookUpEdit.EditValue != null)
         {
            try
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
                                    "<Privilege>117</Privilege><Sub_Sys>5</Sub_Sys>", 
                                    "DataGuard"
                                 },
                                 AfterChangedOutput = new Action<object>((output) => {
                                    if ((bool)output)
                                       return;
                                    MessageBox.Show(this, "دسترسی به ردیف 117 مجاز نمی باشد", "خطا", MessageBoxButtons.OK);
                                 })
                              },
                              #endregion
                           })
                        });
               _DefaultGateway.Gateway(_InteractWithScsc);

               if (_InteractWithScsc.Status == StatusType.Successful)
               {
                  PmmtBs1.DataSource =
                     iScsc.VF_Payment_Delivers(
                        new XElement("Process",
                           new XElement("Payment",
                              new XAttribute("fromdate", cb_cashoutcometoday.Status ? DateTime.Now.ToString("yyyy-MM-dd") : Pdt_CashOutPaymentFromDate.Value == null ? "" : Pdt_CashOutPaymentFromDate.Value.Value.ToString("yyyy-MM-dd")),
                              new XAttribute("todate", cb_cashoutcometoday.Status ? DateTime.Now.ToString("yyyy-MM-dd") : Pdt_CashOutPaymentToDate.Value == null ? "" : Pdt_CashOutPaymentToDate.Value.Value.ToString("yyyy-MM-dd")),
                              new XAttribute("cashby", CashBy_LookUpEdit.EditValue),
                              new XAttribute("delvstat", "001")
                           )
                        )
                     );
                  //Te_CashPayment.EditValue = iScsc.VF_Payment_Delivers(new XElement("Process", new XElement("Payment", new XAttribute("fromdate", "0001-01-01"), new XAttribute("todate", "0001-01-01"), new XAttribute("delvstat", "001"), new XAttribute("cashby", CashBy_LookUpEdit.EditValue)))).Sum(p => p.EXPN_PRIC) ?? 0;
               }


            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
         }
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void cb_cashincometoday_StatusChange(object sender)
      {
         Gb_CashInDate.Visible = !cb_cashincometoday.Status;
      }

      private void cb_cashoutcometoday_StatusChange(object sender)
      {
         Gb_CashOutDate.Visible = !cb_cashoutcometoday.Status;
      }

      private void CashBy_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         try
         {
            Te_OUT.Text = CashBy_LookUpEdit.Text;
         }
         catch (Exception)
         {}
      }

   }
}
