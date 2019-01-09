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
using DevExpress.XtraEditors;
using System.Data.SqlClient;

namespace System.Scsc.Ui.BodyFitness
{
   public partial class BDF_PROS_F : UserControl
   {
      public BDF_PROS_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);
      private bool setOnDebt = false;

      private void Execute_Query()
      {
         if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "018" &&
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

         RqstBs1.Position = RqstIndex;
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
            if (tb_master.SelectedTab == tp_001)
            {
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

               if (FILE_NO_LookUpEdit01.ItemIndex == -1) { FILE_NO_LookUpEdit01.Focus(); return; }
               if (RQTT_CODE_LookUpEdit01.ItemIndex == -1) { RQTT_CODE_LookUpEdit01.Focus(); return; }

               var rqro = RqroBs1.Current as Data.Request_Row;

               BdftBs1.EndEdit();
               BdfmBs1.EndEdit();
               ChbfBs1.EndEdit();
               BfmmBs1.EndEdit();

               RqstIndex = RqstBs1.Position;

               iScsc.BDF_MRQT_P(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                        new XAttribute("rqtpcode", "018"),
                        new XAttribute("rqttcode", RQTT_CODE_LookUpEdit01.EditValue),
                        new XAttribute("mdulname", GetType().Name),
                        new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                        new XElement("Request_Row",
                           new XAttribute("fileno", rqro == null ? FILE_NO_LookUpEdit01.EditValue : rqro.FIGH_FILE_NO),
                           RqroBs1.List.OfType<Data.Request_Row>().Select(rr => 
                              rr.Body_Fitnesses.Select(bd =>
                                 new XElement("Body_Fitness",
                                    new XAttribute("rwno", bd.RWNO),
                                    new XAttribute("bdfttype", bd.BDFT_TYPE),
                                    new XAttribute("mesrtype", bd.MESR_TYPE),
                                    new XAttribute("totlexrstime", bd.TOTL_EXRS_TIME.Value.ToString()),
                                    new XAttribute("resttimebtwnset", bd.REST_TIME_BTWN_SET.Value.ToString()),
                                    new XAttribute("numbdayexrsprog", bd.NUMB_DAY_EXRS_PROG),
                                    new XAttribute("premovechck", bd.PRE_MOVE_CHCK),
                                    new XAttribute("bdftdesc", bd.BDFT_DESC ?? ""),
                                    new XElement("Body_Fitness_Measurements",
                                       bd.Body_Fitness_Measurements.Select(bdms =>
                                          new XElement("Body_Fitness_Measurement",
                                             new XAttribute("bodytype", bdms.BODY_TYPE),
                                             new XAttribute("rwno", bdms.RWNO),
                                             new XAttribute("bdfmdesc", bdms.BDFM_DESC ?? ""),
                                             new XAttribute("mesrvalu", bdms.MESR_VALU ?? 0)
                                          )
                                       )
                                    ),
                                    new XElement("Change_Body_Fitnesses",
                                       ChbfBs1.List.OfType<Data.Change_Body_Fitness>().Select(chbf =>
                                          new XElement("Change_Body_Fitness",
                                             new XAttribute("bodytype", chbf.BODY_TYPE),
                                             new XAttribute("rwno", chbf.RWNO),
                                             new XAttribute("efcttype", chbf.EFCT_TYPE),
                                             new XAttribute("chbfdesc", chbf.CHBF_DESC ?? ""),
                                             new XAttribute("prtynumb", chbf.PRTY_NUMB ?? 1),
                                             new XAttribute("indcweghdumb", chbf.INDC_WEGH_DUMB ?? "001"),
                                             new XAttribute("indcamntweghtype", chbf.INDC_AMNT_WEGH_TYPE ?? "001"),
                                             new XAttribute("indcamntwegh", chbf.INDC_AMNT_WEGH ?? 0),
                                             new XAttribute("stat", chbf.STAT ?? "002"),
                                             new XElement("Body_Fitness_Movements",
                                                BfmmBs1.List.OfType<Data.Body_Fitness_Movement>().Where(bfmm =>
                                                   bfmm.CHBF_BODY_TYPE == chbf.BODY_TYPE &&
                                                   bfmm.CHBF_RWNO == chbf.RWNO
                                                ).Select(bfmm =>
                                                   new XElement("Body_Fitness_Movement",
                                                      new XAttribute("bodytype", chbf.BODY_TYPE),
                                                      new XAttribute("rwno", bfmm.RWNO),
                                                      new XAttribute("weekdaytype", bfmm.WEEK_DAY_TYPE),
                                                      new XAttribute("bbfmbfid", bfmm.BBFM_BFID),
                                                      new XAttribute("resttimeinset", bfmm.REST_TIME_IN_SET.Value.ToString() ?? "00:00:45"),
                                                      new XAttribute("timeperset", bfmm.TIME_PER_SET .Value.ToString() ?? "00:01:00"),
                                                      new XAttribute("numbofmoveinset", bfmm.NUMB_OF_MOVE_IN_SET ?? 10),
                                                      new XAttribute("amntwegh", bfmm.AMNT_WEGH ?? 10),
                                                      new XAttribute("ordr", bfmm.ORDR ?? 1),
                                                      new XAttribute("premove", bfmm.PRE_MOVE ?? "001")
                                                   )
                                                )
                                             )
                                          )
                                       )
                                    )
                                 )
                              )
                           )
                        )
                     )
                  )
               );
            }
            requery = true;
         }
         catch (Exception ex)
         {
            switch (ex.Message)
            {
               case "Value cannot be null.\r\nParameter name: value":
                  MessageBox.Show("در بین اطلاعات وارد شده مقادیری هستند که باید مقدار دهی شوند. لطفا بررسی و اصلاح کنید");
                  break;
               default:
                  MessageBox.Show(ex.Message);
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

      private void Btn_RqstBnDelete1_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
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
            }
            //OldRecdBs1.List.Clear();
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

      private void Btn_RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            if (tb_master.SelectedTab == tp_001)
            {
               Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
               if (Rqst == null) return;

               /////// Save Data
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
               //Btn_RqstDelete1.Visible = true;
               RqstBnASav1.Enabled = false;
               //Btn_PymtSave001.Enabled = false;
               //Gb_Common001.Enabled = false;
               var chbf = ChbfBs1.Current as Data.Change_Body_Fitness;
               BfmmtBs1.DataSource = iScsc.Body_Fitness_Movements.Where(b => b.CHBF_BDFT_FIGH_FILE_NO == chbf.BDFT_FIGH_FILE_NO && b.CHBF_BDFT_RECT_CODE == "001" && b.CHBF_BDFT_RWNO == chbf.BDFT_RWNO);
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               //Btn_RqstDelete1.Visible = /*Btn_RqstSav1.Visible =*/ true;
               RqstBnASav1.Enabled = true;
               //Btn_PymtSave001.Enabled = true;
               //Gb_Common001.Enabled = true;
               var chbf = ChbfBs1.Current as Data.Change_Body_Fitness;
               BfmmtBs1.DataSource = iScsc.Body_Fitness_Movements.Where(b => b.CHBF_BDFT_FIGH_FILE_NO == chbf.BDFT_FIGH_FILE_NO && b.CHBF_BDFT_RECT_CODE == "001" && b.CHBF_BDFT_RWNO == chbf.BDFT_RWNO);
            }
            else if (Rqst.RQID == 0)
            {
               RqstBnASav1.Enabled = false;
               //Gb_Expense001.Visible = false;
               //Btn_RqstDelete1.Visible = /*Btn_RqstSav1.Visible =*/ false;

               //Btn_PymtSave001.Enabled = false;
               //Gb_Common001.Enabled = true;
            }

            
         }
         catch
         {
            RqstBnASav1.Enabled = false;
            //Gb_Expense001.Visible = false;
            //Btn_RqstDelete1.Visible = /*Btn_RqstSav1.Visible =*/ false;

            //Btn_PymtSave001.Enabled = false;
            //Gb_Common001.Enabled = true;
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                  MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
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
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            throw;
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
                  var Crnt = PydtsBs1.Current as Data.Payment_Detail;
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
                                    new XAttribute("pydtdesc", pd.PYDT_DESC ?? "")
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
         catch 
         {
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

      private void Btn_PymtSave_Click(object sender, EventArgs e)
      {

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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "018")))}
                  })
               );
         }
      }

      private void ChbfBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var crnt = ChbfBs1.Current as Data.Change_Body_Fitness;

            BbfmBs1.DataSource = iScsc.Basic_Body_Fitness_Movements.Where(b => b.BODY_TYPE == crnt.BODY_TYPE && b.EFCT_TYPE == crnt.EFCT_TYPE);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }

      private void change_Body_FitnessesGridControl_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن تغییرات عضو بدن موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  /* Do Delete Payment_Detail */
                  var Crnt = ChbfBs1.Current as Data.Change_Body_Fitness;

                  iScsc.Change_Body_Fitnesses.DeleteOnSubmit(Crnt);

                  iScsc.SubmitChanges();
                  
                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
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

      private void AddMove_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var crntbbfm = BbfmBs1.Current as Data.Basic_Body_Fitness_Movement;
            var crntchbf = ChbfBs1.Current as Data.Change_Body_Fitness;
            var crntbdft = BdftBs1.Current as Data.Body_Fitness;

            if(Wkdy_lov.EditValue == null || Wkdy_lov.Text == "") 
               Wkdy_lov.EditValue = "001";

            //long _sumticks = BfmmBs1.List.OfType<Data.Body_Fitness_Movement>().Sum(b => (b.TIME_PER_SET.Value.Ticks + b.REST_TIME_IN_SET.Value.Ticks) * b.NUMB_OF_MOVE_IN_SET.Value * b.CONT.Value);
            //if(crntbdft.TOTL_EXRS_TIME.Value.CompareTo(new TimeSpan(_sumticks)) == -1)
            //{
            //   throw new Exception("تعداد حرکات برنامه ورزشی مشتری از میزان مدت تمرین بیشتر شده");
            //}

            if(crntbdft.PRE_MOVE_CHCK == "002")
            {
               PmbfBs1.List.OfType<Data.Premovement_Body_Fitness>().ToList().ForEach(p => 
                  {
                     if (!BfmmBs1.List.OfType<Data.Body_Fitness_Movement>().Any(pt => pt.BBFM_BFID == p.PRE_BBFM_BFID && pt.WEEK_DAY_TYPE == Wkdy_lov.EditValue.ToString()))
                     {
                        BfmmBs1.AddNew();
                        var crnt = BfmmBs1.Current as Data.Body_Fitness_Movement;
                        crnt.WEEK_DAY_TYPE = Wkdy_lov.EditValue.ToString();
                        crnt.BBFM_BFID = p.PRE_BBFM_BFID;
                        crnt.PRE_MOVE = "002";
                        crnt.REST_TIME_IN_SET = crntbbfm.REST_TIME_IN_SET;
                        crnt.TIME_PER_SET = crntbbfm.TIME_PER_SET;
                     }
                  });
            }

            if(!BfmmBs1.List.OfType<Data.Body_Fitness_Movement>().Any(pt => pt.BBFM_BFID == crntbbfm.BFID && pt.WEEK_DAY_TYPE == Wkdy_lov.EditValue.ToString()))
            {
               BfmmBs1.AddNew();
               var crnt = BfmmBs1.Current as Data.Body_Fitness_Movement;
               crnt.WEEK_DAY_TYPE = Wkdy_lov.EditValue.ToString();
               crnt.BBFM_BFID = crntbbfm.BFID;
               crnt.PRE_MOVE = "001";
               crnt.REST_TIME_IN_SET = crntbbfm.REST_TIME_IN_SET;
               crnt.TIME_PER_SET = crntbbfm.TIME_PER_SET;                        
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void body_Fitness_MovementsGridControl_EmbeddedNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
      {
         try
         {
            switch (e.Button.ButtonType)
            {
               case DevExpress.XtraEditors.NavigatorButtonType.Append:
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.Remove:
                  e.Handled = true;
                  if (MessageBox.Show(this, "آیا با پاک کردن برنامه حرکتی موافقید؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                  /* Do Delete Payment_Detail */
                  var Crnt = BfmmBs1.Current as Data.Body_Fitness_Movement;

                  iScsc.Body_Fitness_Movements.DeleteOnSubmit(Crnt);

                  iScsc.SubmitChanges();

                  requery = true;
                  break;
               case DevExpress.XtraEditors.NavigatorButtonType.CancelEdit:
                  requery = true;
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
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Btn_InDebt_Click(object sender, EventArgs e)
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
               Btn_RqstBnASav1_Click(null, null);
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }
   }
}
