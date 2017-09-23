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

namespace System.Scsc.Ui.ChangeRials
{
   public partial class GLR_TOTL_F : UserControl
   {
      public GLR_TOTL_F()
      {
         InitializeComponent();
      }

      bool requery = false;

      private void Execute_Query(bool runAllQuery)
      {
         if (tb_master.SelectedTab == tp_001 || runAllQuery)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "020" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.RQTT_CODE == "004" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID) &&
                     rqst.MDUL_NAME == GetType().Name &&
                     rqst.SECT_NAME == GetType().Name.Substring(0, 3) + "_001_F"
               );
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               RqstBnDelete1.Enabled = RqstBnASav1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               RqstBnDelete1.Enabled = RqstBnASav1.Enabled = false;               
            }
         }
         catch
         {
            RqstBnDelete1.Enabled = RqstBnASav1.Enabled = false;            
         }
      }

      private void RqroBs1_CurrentChanged(object sender, EventArgs e)
      {
         var crnt = RqroBs1.Current as Data.Request_Row;
         if (crnt == null) return;

         VF_RqstChngBs1.DataSource = iScsc.VF_Request_Changing(crnt.FIGH_FILE_NO).Where(r => r.RQTT_CODE != "004");
      }

      private void Btn_RqstRqt1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XElement("Request_Row", 
                        new XAttribute("fighfileno", FILE_NO_LookUpEdit.EditValue ?? "")
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("chngtype", cHNG_TYPELookUpEdit.EditValue ?? ""),
                        new XAttribute("debttype", dEBT_TYPELookUpEdit.EditValue ?? ""),
                        new XAttribute("amnt", aMNTTextEdit.EditValue ?? ""),
                        new XAttribute("paiddate", PaidDate_DateTime.Value == null ? "" : PaidDate_DateTime.Value.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("chngresn", cHNG_RESNLookUpEdit.EditValue ?? ""),
                        new XAttribute("resndesc", rESN_DESCTextEdit.EditValue ?? ""),
                        new XAttribute("rqstrqiddnrm", RqstRqidGridLookup.EditValue == null ? RqroRqstRqidDnrm_TextEdit.EditValue ?? "" : RqstRqidGridLookup.EditValue)
                     )
                  )
               )
            );

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
               /*
                * Requery Data From Database
                */
               Execute_Query(false);
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
               //MessageBox.Show(this, "هنرجو حذف گردید!");
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
               Execute_Query(false);
               requery = false;
            }
         }
      }

      private void Btn_RqstSav1_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            var glrl = GlrlBs1.Current as Data.Gain_Loss_Rial;

            string result = "تغییرات ریالی باعث ";
            if (glrl.CHNG_TYPE == "001" && glrl.DEBT_TYPE == "000")
               result += "بدهکار کردن به مبلغ " + ((int)glrl.AMNT).ToString("n0") + " ریال برای هنرجوی " + glrl.Fighter.NAME_DNRM + " و ثبت هزینه برای حسابداری در باشگاه می شود.";
            else if(glrl.CHNG_TYPE == "001" && glrl.DEBT_TYPE == "001")
               result += "بدهکار کردن به مبلغ " + ((int)glrl.AMNT).ToString("n0") + " ریال برای هنرجوی " + glrl.Fighter.NAME_DNRM + " می شود.";
            else if (glrl.CHNG_TYPE == "001" && glrl.DEBT_TYPE == "002")
               result += "کسب درآمد به مبلغ " + ((int)glrl.AMNT).ToString("n0") + " ریال در حسابداری باشگاه می شود." ;
            else if (glrl.CHNG_TYPE == "002" && glrl.DEBT_TYPE == "003")
               result += "بستانکار کردن به مبلغ " + ((int)glrl.AMNT).ToString("n0") + " ریال برای هنرجوی " + glrl.Fighter.NAME_DNRM + " می شود.";
            else if (glrl.CHNG_TYPE == "002" && glrl.DEBT_TYPE == "004")
               result += "ثبت هزینه به مبلغ " + ((int)glrl.AMNT).ToString("n0") + " ریال در حسابداری باشگاه می شود.";
            else if (glrl.CHNG_TYPE == "002" && glrl.DEBT_TYPE == "005")
               result += "کسب درآمد به مبلغ " + ((int)glrl.AMNT).ToString("n0") + " ریال در حسابداری باشگاه می شود و افزایش سپرده برای هنرجو " + glrl.Fighter.NAME_DNRM + " می شود. ";

            MessageBox.Show(this, result, "نتیجه عملیات");

            if (MessageBox.Show(this, "آیا با ذخیره کردن تغییرات ریالی موافق هستید؟", "ذخیره کردن تغییرات ریالی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID)
                  )
               )
            );
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
               Execute_Query(false);
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "020")))}
                  })
               );
         }
      }

      private void GlrlBs1_CurrentChanged(object sender, EventArgs e)
      {
         var crnt = GlrlBs1.Current as Data.Gain_Loss_Rial;

         if (crnt == null) return;

         RqstRqidGridLookup.EditValue = crnt.RQRO_RQST_RQID_DNRM;
      }

      private void Pblc_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = FILE_NO_LookUpEdit.EditValue;
            if (fileno == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

   }
}
