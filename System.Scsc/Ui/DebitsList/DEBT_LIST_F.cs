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

namespace System.Scsc.Ui.DebitsList
{
   public partial class DEBT_LIST_F : UserControl
   {
      public DEBT_LIST_F()
      {
         InitializeComponent();
      }

      bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         iScsc.CommandTimeout = 18000;
         vF_Last_Info_FighterResultBs1.DataSource = 
            iScsc.VF_Last_Info_Fighter(fileno, null, null,null, null, null, null, null).
            Where(f => 
               f.DEBT_DNRM != 0 ||
               iScsc.Requests.
               Where(r =>
                  iScsc.VF_Request_Changing(f.FILE_NO).
                  Where(rc => rc.RQTT_CODE != "004").
                  Select(rc => rc.RQID).Contains(r.RQID) &&
                  r.Payments.
                  Any(p =>
                     ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT + p.SUM_REMN_PRIC) -
                      (p.SUM_RCPT_EXPN_PRIC ?? 0 + p.SUM_RCPT_EXPN_EXTR_PRCT ?? 0 + p.SUM_RCPT_REMN_PRIC ?? 0 + p.SUM_PYMT_DSCN_DNRM ?? 0)) > 0 ||
                     p.Payment_Details.Any(pd => pd.PAY_STAT == "001"))
               ).Count() > 0
            );
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void vF_Last_Info_FighterResultBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var figh = vF_Last_Info_FighterResultBs1.Current as Data.VF_Last_Info_FighterResult;

            RqstBs2.List.Clear();
            GlrlBs3.List.Clear();

            if (figh == null) return;

            RqstBs2.DataSource = 
               iScsc.Requests.
               Where(r => 
                  iScsc.VF_Request_Changing(figh.FILE_NO).
                  Where(rc => rc.RQTT_CODE != "004").
                  Select(rc => rc.RQID).Contains(r.RQID) &&                   
                  r.Payments.
                  Any(p => 
                     ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT + p.SUM_REMN_PRIC) - 
                      (p.SUM_RCPT_EXPN_PRIC + p.SUM_RCPT_EXPN_EXTR_PRCT + p.SUM_RCPT_REMN_PRIC + p.SUM_PYMT_DSCN_DNRM ?? 0)) > 0 || 
                     p.Payment_Details.Any(pd => pd.PAY_STAT == "001"))
               );

            GlrlBs3.DataSource = 
               iScsc.Gain_Loss_Rials.
               Where(gl => 
                  gl.FIGH_FILE_NO == figh.FILE_NO &&
                  gl.CONF_STAT == "002"
               );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void tbn_CashPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_002)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs2.Current as Data.Request;
               if (rqst == null) return;
               //var pymt = PymtsBs2.Current as Data.Payment;

               foreach (Data.Payment pymt in PymtsBs2)
               {
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithoutPOS"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("paystat", "002"),
                              new XAttribute("fileno", rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO)
                           )
                        )
                     )
                  );
               }

               
            }
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Execute_Query();
               requery = false;
            }
         }
      }

      private void bn_PaymentMethods1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs2.Current as Data.Request;
            if (rqst == null) return;
            var pymt = PymtsBs2.Current as Data.Payment;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 86 /* Execute Pay_Mtod_F */, SendType.Self) { Input = pymt }
            );
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void Btn_Refresh_Click(object sender, EventArgs e)
      {
         Execute_Query();         
      }

      private void GlrlBs3_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var glrl = GlrlBs3.Current as Data.Gain_Loss_Rial;
            var glrlmax = GlrlBs3.List.OfType<Data.Gain_Loss_Rial>().Max(g => g.RWNO);

            if (glrl == null)
            {
               tbn_CashPayment2.Enabled = false;
               return;
            }
            
            if(glrl.CHNG_TYPE == "001")
            {
               // افزایش بدهی
               if(glrl.RWNO == glrlmax)
                  tbn_CashPayment2.Enabled = true;
               else
                  tbn_CashPayment2.Enabled = false;               
            }               
            else if(glrl.CHNG_TYPE == "002")
            {
               // کاهش بدهی
               tbn_CashPayment2.Enabled = false;               
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void tbn_CashPayment2_Click(object sender, EventArgs e)
      {
         try
         {
            var glrl = GlrlBs3.Current as Data.Gain_Loss_Rial;

            // این قسمت برنامه باید به واحد خودش در تغییرات ریالی انتقال پیدا کند تا از پرداکندگی کدها جلوگیری شود
            #region Gain_Loss_Rials
            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_003_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", glrl.FIGH_FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("chngtype", "002"),
                        new XAttribute("debttype", "001"),
                        new XAttribute("amnt", glrl.AMNT),
                        new XAttribute("paiddate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("chngresn", "004"),
                        new XAttribute("resndesc", "ثبت مبلغ تسویه بدهی از قبل * ثبت سیستمی")                        
                     )
                  )
               )
            );

            var rqst = iScsc.Requests.FirstOrDefault(r => r.RQTP_CODE == "020" && r.RQTT_CODE == "004" && r.MDUL_NAME == GetType().Name && r.SECT_NAME == GetType().Name.Substring(0, 3) + "_003_F" && r.Request_Rows.Any(rr => rr.FIGH_FILE_NO == glrl.FIGH_FILE_NO) && r.RQST_STAT == "001");

            if (rqst == null) return;

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID)
                  )
               )
            );
            #endregion

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
   }
}
