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
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.OtherIncome
{
   public partial class OIC_SRZH_F : UserControl
   {
      public OIC_SRZH_F()
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
                     rqst.RQTP_CODE == "016" &&
                     rqst.RQTT_CODE == "007" &&
                     rqst.RQST_STAT == "001" &&
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
         else if(tb_master.SelectedTab == tp_002)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "016" &&
                     rqst.RQTT_CODE == "007" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            SnmtBs2.DataSource =
               iScsc.Session_Meetings
               .Where(
                  s =>
                     Rqids.Contains(s.Session.Member_Ship.Request_Row.Request.RQID) &&
                     s.Session.Member_Ship.Request_Row.Request.MDUL_NAME == GetType().Name &&
                     s.Session.Member_Ship.Request_Row.Request.SECT_NAME == GetType().Name.Substring(0, 3) + "_001_F"
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
         }
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

            if (CLUB_CODELookUpEdit.ItemIndex == -1) { CLUB_CODELookUpEdit.Focus(); return; }

            iScsc.OIC_SRQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "016"),
                     new XAttribute("rqttcode", "007"),
                     new XAttribute("clubcode", CLUB_CODELookUpEdit.EditValue),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),

                     new XElement("Session",
                        new XAttribute("timewate", /*TIME_WATEMaskedTextBox.Text + */"00:00"),
                        SnmtBs1.List.OfType<Data.Session_Meeting>().Where(sm => !sm.END_TIME.HasValue).Select(sm =>
                           new XElement("Session_Meeting",
                              new XAttribute("rwno", sm.RWNO),
                              new XAttribute("valdtype", sm.VALD_TYPE ?? "002"),
                              new XAttribute("strttime", sm.STRT_TIME.HasValue ? sm.STRT_TIME.Value.ToString() : DateTime.Now.ToString("HH:mm:ss")),
                              new XAttribute("endtime", sm.END_TIME.HasValue ? sm.END_TIME.Value.ToString() : "00:00:00"),
                              new XAttribute("tempendtime", "00:00:00"),
                              new XAttribute("expncode", sm.EXPN_CODE)
                           )
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
            //OldRecdBs1.List.Clear();
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

            iScsc.OIC_SSAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
                  )
               )
            );
            //OldRecdBs1.List.Clear();
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
               RqstBnDelete1.Enabled = true;
               RqstBnASav1.Enabled = false;

               gb_comm.Enabled = Btn_CalcTempEndTime.Enabled = Btn_EndTimeForItem.Enabled = Btn_PymtSave.Enabled = false;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               RqstBnDelete1.Enabled = /*Btn_RqstSav1.Visible =*/ true;
               gb_comm.Enabled = Btn_CalcTempEndTime.Enabled = Btn_EndTimeForItem.Enabled = Btn_PymtSave.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               RqstBnDelete1.Enabled = RqstBnASav1.Enabled = false;
               Btn_CalcTempEndTime.Enabled = Btn_EndTimeForItem.Enabled = Btn_PymtSave.Enabled = false;
               gb_comm.Enabled = true;
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            RqstBnDelete1.Enabled = RqstBnASav1.Enabled = false;
            Btn_CalcTempEndTime.Enabled = Btn_EndTimeForItem.Enabled = Btn_PymtSave.Enabled = false;
            gb_comm.Enabled = true;
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

      private void PydtBn1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  /* Do Delete Payment_Detail */
                  var Crnt  = PydtsBs1.Current as Data.Payment_Detail;
                  var rqst = RqstBs1.Current as Data.Request;
                  iScsc.DEL_SEXP_P(
                     new XElement("Request",
                        new XAttribute("rqid", rqst.RQID),
                        new XElement("Payment",
                           new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                           new XElement("Payment_Detail",
                              new XAttribute("code", Crnt.CODE)
                           )
                        )
                     )
                  );
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.EndEdit:
                  /* Do Something for insert or update Payment Detail Price */
                  PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.CRET_BY == null).ToList()
                     .ForEach(pd =>
                        {
                           rqst = RqstBs1.Current as Data.Request;
                           iScsc.INS_SEPD_P(
                              new XElement("Request",
                                 new XAttribute("rqid", rqst.RQID),
                                 new XElement("Payment", 
                                    new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                                    new XElement("Payment_Detail",
                                       new XAttribute("expncode", pd.EXPN_CODE),
                                       new XAttribute("expnpric", pd.EXPN_PRIC),
                                       new XAttribute("pydtdesc", pd.PYDT_DESC)
                                    )
                                 )                                 
                              )
                           );
                        }
                  );

                  PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.CODE != 0).ToList()
                     .ForEach(pd =>
                     {
                        rqst = RqstBs1.Current as Data.Request;
                        iScsc.UPD_SEXP_P(
                           new XElement("Request",
                              new XAttribute("rqid", rqst.RQID),
                              new XElement("Payment",
                                 new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
                                 new XElement("Payment_Detail",
                                    new XAttribute("code", pd.CODE),
                                    new XAttribute("expnpric", pd.EXPN_PRIC),
                                    new XAttribute("pydtdesc", pd.PYDT_DESC)
                                 )
                              )
                           )
                        );
                     }
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
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }     

      private void ntb_POSPayment1_Click(object sender, EventArgs e)
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
      
      private void Btn_CalcTempEndTime_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
            
            if (Rqst.RQID == 0) return;
            /*if (REGN_PRVN_CODELookUpEdit.ItemIndex == -1) { REGN_PRVN_CODELookUpEdit.Focus(); return; }
            if (REGN_CODELookUpEdit.ItemIndex == -1) { REGN_CODELookUpEdit.Focus(); return; }
            if (CLUB_CODELookUpEdit.ItemIndex == -1) { CLUB_CODELookUpEdit.Focus(); return; }*/

            iScsc.OIC_SRQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID),
                     new XAttribute("rqtpcode", "016"),
                     new XAttribute("rqttcode", "007"),
                     /*new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
                     new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
                     new XAttribute("clubcode", CLUB_CODELookUpEdit.EditValue),*/
                     new XElement("Session",
                        new XAttribute("timewate", /*TIME_WATEMaskedTextBox.Text +*/ "00:00"),
                        SnmtBs1.List.OfType<Data.Session_Meeting>().Where(sm => !sm.END_TIME.HasValue).Select(sm =>
                           new XElement("Session_Meeting",
                              new XAttribute("rwno", sm.RWNO),
                              new XAttribute("valdtype", sm.VALD_TYPE ?? "002"),
                              new XAttribute("strttime", sm.STRT_TIME.HasValue ? sm.STRT_TIME.Value.ToString() : DateTime.Now.ToString("HH:mm:ss")),
                              new XAttribute("endtime", "00:00:00"),
                              new XAttribute("tempendtime", DateTime.Now.ToString("HH:mm:ss")),
                              new XAttribute("expncode", sm.EXPN_CODE)
                           )
                        )
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
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_EndTimeForItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (SnmtBs1.List.OfType<Data.Session_Meeting>().Where(sm => !sm.END_TIME.HasValue).Count() >= 1 &&  MessageBox.Show(this, "با انجام عملیات محاسبه هزینه متوقف می شود؟ آیا موافقید اجاره آیتم پایان یابد؟", "پایان اجاره آیتم", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

            if (Rqst.RQID == 0) return;
            /*if (REGN_PRVN_CODELookUpEdit.ItemIndex == -1) { REGN_PRVN_CODELookUpEdit.Focus(); return; }
            if (REGN_CODELookUpEdit.ItemIndex == -1) { REGN_CODELookUpEdit.Focus(); return; }
            if (CLUB_CODELookUpEdit.ItemIndex == -1) { CLUB_CODELookUpEdit.Focus(); return; }*/

            iScsc.OIC_SRQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID),
                     new XAttribute("rqtpcode", "016"),
                     new XAttribute("rqttcode", "007"),
                     /*new XAttribute("prvncode", REGN_PRVN_CODELookUpEdit.EditValue),
                     new XAttribute("regncode", REGN_CODELookUpEdit.EditValue),
                     new XAttribute("clubcode", CLUB_CODELookUpEdit.EditValue),*/
                     new XElement("Session",
                        new XAttribute("timewate", /*TIME_WATEMaskedTextBox.Text +*/ "00:00"),
                        SnmtBs1.List.OfType<Data.Session_Meeting>().Where(sm => !sm.END_TIME.HasValue).Select(sm =>
                           new XElement("Session_Meeting",
                              new XAttribute("rwno", sm.RWNO),
                              new XAttribute("valdtype", sm.VALD_TYPE ?? "002"),
                              new XAttribute("strttime", sm.STRT_TIME.HasValue ? sm.STRT_TIME.Value.ToString() : DateTime.Now.ToString("HH:mm:ss")),
                              new XAttribute("endtime", DateTime.Now.ToString("HH:mm:ss")),
                              new XAttribute("tempendtime", "00:00:00"),
                              new XAttribute("expncode", sm.EXPN_CODE)
                           )
                        )
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
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
         }
      }

      private void Btn_PymtSave_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "با انجام عملیات تسویه حساب دیگر قادر به تغییر نیستید؟ آیا عملیات تسویه حساب انجام شود", "انجام عملیات تسویه حساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            Btn_EndTimeForItem_Click(null, null);
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

            if (Rqst.RQID == 0) return;

            iScsc.OIC_SPYM_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID)
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
               Get_Current_Record();
               Execute_Query();
               Set_Current_Record();
               requery = false;
            }
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "016")))}
                  })
               );
         }
      }

      private void SendToolTip_MouseEnter(object sender, EventArgs e)
      {
         Control c = sender as Control;
         string tooltip = "";
         switch (c.Tag.ToString())
         {
            case "1":
               tooltip = "محاسبه هزینه کل آیتم های به اجاره رفته";
               break;
            case "2":
               tooltip = "اتمام زمان اجاره آیتم های رزرو شده";
               break;
            case "3":
               tooltip = "صدور و تسویه حساب تمامی آیتم های به اجاره رفته. بعد از صدور دیگر قادر به بازگشت نیستین";
               break;
            default:
               break;
         }

         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost", "FRST_PAGE_F", 40 /* SendToolTip */, SendType.SelfToUserInterface) { Input = tooltip }
         //);
      }

      private void Cacl_Expn_Item_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         RqstBs1.DataSource = (SnmtBs2.Current as Data.Session_Meeting).Session.Member_Ship.Request_Row.Request;
         SnmtBs1.DataSource = SnmtBs2.Current;
         
         Btn_CalcTempEndTime_Click(null, null);

         SnmtBs1.DataSource = SesnBs1;
         SnmtBs1.DataMember = "Session_Meetings";
      }

      private void End_Time_Item_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         RqstBs1.DataSource = (SnmtBs2.Current as Data.Session_Meeting).Session.Member_Ship.Request_Row.Request;
         SnmtBs1.DataSource = SnmtBs2.Current;

         Btn_EndTimeForItem_Click(null, null);

         SnmtBs1.DataSource = SesnBs1;
         SnmtBs1.DataMember = "Session_Meetings";
      }

      private void Issu_Expn_Item_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         RqstBs1.DataSource = (SnmtBs2.Current as Data.Session_Meeting).Session.Member_Ship.Request_Row.Request;
         SnmtBs1.DataSource = SnmtBs2.Current;

         Btn_PymtSave_Click(null, null);

         SnmtBs1.DataSource = SesnBs1;
         SnmtBs1.DataMember = "Session_Meetings";
      }

      private void Ins_Rqst_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         RqstBs1.DataSource = (SnmtBs2.Current as Data.Session_Meeting).Session.Member_Ship.Request_Row.Request;
      }

      private void Btn_CalcTempEndTime2_Click(object sender, EventArgs e)
      {
         for (int i = 0; i < SnmtBs2.Count; i++)
         {
            SnmtBs2.Position = i;
            Cacl_Expn_Item_ButtonClick(null, null);
         }
      }

      private void Btn_EndTimeForItem2_Click(object sender, EventArgs e)
      {

      }

      private void Btn_PymtSave2_Click(object sender, EventArgs e)
      {

      }
   }
}
