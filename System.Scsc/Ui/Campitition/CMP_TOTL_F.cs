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
using System.Data.SqlClient;

namespace System.Scsc.Ui.Campitition
{
   public partial class CMP_TOTL_F : UserControl
   {
      public CMP_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);

      private void Execute_Query()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "008" &&                     
                     rqst.RQST_STAT == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            RqstBs1.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               );
         }
      }

      int RqstIndex;
      private void Get_Current_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Count >= 1)
               RqstIndex = RqstBs1.Position;
         }
      }

      private void Set_Current_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstIndex >= 0)
               RqstBs1.Position = RqstIndex;
         }
      }

      private void Create_Record()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            RqstBs1.AddNew();
            FILE_NO_LookUpEdit.Focus();
         }
      }

      private void RqroBs1_CurrentChanged(object sender, EventArgs e)
      {
         if (RqroBs1.Current == null) return;
         Data.Request_Row Rqro = RqroBs1.Current as Data.Request_Row;
         OldRecdBs1.DataSource = iScsc.Campititions.Where(c => c.FIGH_FILE_NO == Rqro.FIGH_FILE_NO && c.RECT_CODE == "004" && c.Fighter.CAMP_RWNO_DNRM == c.RWNO);
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
            Scsc.Data.Fighter Figh = FighBs1.Current as Scsc.Data.Fighter;

            iScsc.CMP_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "008"),
                     new XAttribute("rqttcode", RQTT_CODE_LookUpEdit.EditValue),
                     new XElement("Request_Row",
                           new XAttribute("fileno", Figh != null ? FILE_NO_TextEdit.EditValue : FILE_NO_LookUpEdit.EditValue),
                           new XElement("Campitition",
                              new XAttribute("campdate", CAMP_DATE_DateTime.Value == null ? "" : CAMP_DATE_DateTime.Value.Value.ToString("yyyy-MM-dd")),
                              new XAttribute("placadrs", PLAC_ADDR_TextEdit.EditValue ?? ""),
                              new XAttribute("sectnumb", SECT_NUMB_LookUpEdit.EditValue ?? ""),
                              new XAttribute("levlnumb", LEVL_NUMB_LookUpEdit.EditValue ?? "")
                           )
                        )
                  )
               )
            );
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            Data.Request Rqst = RqstBs1.Current as Data.Request;
            if (Rqst == null) return;

            if (MessageBox.Show(this, "آیا با انصراف دادن درخواست موافق هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            iScsc.CNCL_RQST_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );
            OldRecdBs1.List.Clear();
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
            Scsc.Data.Fighter Figh = FighBs1.Current as Scsc.Data.Fighter;
            if (Rqst == null) return;

            iScsc.CMP_SAVE_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "008"),
                     new XAttribute("rqttcode", RQTT_CODE_LookUpEdit.EditValue),
                     new XElement("Request_Row",
                           new XAttribute("fileno", Figh != null ? FILE_NO_TextEdit.EditValue : FILE_NO_LookUpEdit.EditValue),
                           new XElement("Campitition",
                              new XAttribute("campdate", CAMP_DATE_DateTime.Value == null ? "" : CAMP_DATE_DateTime.Value.Value.ToString("yyyy-MM-dd")),
                              new XAttribute("placadrs", PLAC_ADDR_TextEdit.EditValue ?? ""),
                              new XAttribute("sectnumb", SECT_NUMB_LookUpEdit.EditValue ?? ""),
                              new XAttribute("levlnumb", LEVL_NUMB_LookUpEdit.EditValue ?? "")
                           )
                        )
                  )
               )
            );
            OldRecdBs1.List.Clear();
            requery = true;
         }catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               Gb_Expense.Visible = true;
               Btn_RqstDelete1.Visible = true;
               Btn_RqstSav1.Visible = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
         }
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
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

      private void bn_PaymentMethods_Click(object sender, EventArgs e)
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

      private void bn_CashPayment_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی هنرجو پرداخت شده");
                  return;
               }*/

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

               /* Loop For Print After Pay */
               RqstBnPrintAfterPay_Click(null, null);

               /* End Request */
               Btn_RqstBnASav1_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void ntb_POSPayment_Click(object sender, EventArgs e)
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "008")))}
                  })
               );
         }
      }
   }
}
