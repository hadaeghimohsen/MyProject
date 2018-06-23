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
using DevExpress.XtraGrid.Controls;
using System.IO;

namespace System.Scsc.Ui.OtherIncome
{
   public partial class OIC_TOTL_F : UserControl
   {
      public OIC_TOTL_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);

      private void Execute_Query()
      {
         setOnDebt = false;
         //if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            int pydt = PydtsBs1.Position;

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
               .Where(rqst =>
                     rqst.RQTP_CODE == "016" &&
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

            ExpnBs1.DataSource =
            iScsc.Expenses.Where(ex =>
               ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
               ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
               ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
               ex.EXPN_STAT == "002" /* هزینه های فعال */
            );


            Grop_FLP.Controls.Clear();
            var allItems = new Button();

            allItems.Text = "همه موارد";
            allItems.Tag = 0;

            allItems.Click += GropButn_Click;
            Grop_FLP.Controls.Add(allItems);

            ExpnBs1.List.OfType<Data.Expense>().OrderBy(e => e.GROP_CODE).GroupBy(e => e.Group_Expense).ToList().ForEach(
               g =>
               {
                  var b = new Button();
                  if (g.Key != null)
                  {
                     b.Text = g.Key.GROP_DESC;
                     b.Tag = g.Key.CODE;
                  }
                  else
                     b.Text = "سایر موارد";                  
                  b.Click += GropButn_Click;
                  Grop_FLP.Controls.Add(b);
               }
            );

            PydtsBs1.Position = pydt;
         }
         requery = false;
      }

      void GropButn_Click(object sender, EventArgs e)
      {
         Button b = (Button) sender;
         if(b.Tag != null)
            if(Convert.ToInt64(b.Tag) != 0)
               Expn_Gv.ActiveFilterString = string.Format("GROP_CODE = {0}", b.Tag);
            else
               Expn_Gv.ActiveFilterString = "";
         else
            Expn_Gv.ActiveFilterString = "GROP_CODE IS NULL";
      }

      int RqstIndex;
      private void Get_Current_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Count >= 1)
               RqstIndex = RqstBs1.Position;
         }
      }

      private void Set_Current_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            if (RqstIndex >= 0)
               RqstBs1.Position = RqstIndex;
         }
      }

      private void Create_Record()
      {
         //if (tb_master.SelectedTab == tp_001)
         {
            RqstBs1.AddNew();
            FILE_NO_LookUpEdit.Focus();
         }
      }

      private void RqroBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var Rqst = RqstBs1.Current as Data.Request;

            if (Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2))
            {
               Gb_Expense.Visible = true;
               Gb_ExpenseItem.Visible = true;
               //Btn_RqstDelete1.Visible = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnDelete1.Enabled = true;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               Gb_ExpenseItem.Visible = true;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnDelete1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               Gb_ExpenseItem.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               RqstBnDelete1.Enabled = false;
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            Gb_ExpenseItem.Visible = false;
            //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
            RqstBnDelete1.Enabled = false;
         }
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
            Scsc.Data.Fighter Figh = FighBs1.Current as Scsc.Data.Fighter;

            iScsc.OIC_ERQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "016"),
                     new XAttribute("rqttcode", "001"),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XAttribute("rqstdesc", RqstDesc_Txt.EditValue ?? ""),
                     new XElement("Request_Row",
                        new XAttribute("fileno", Figh == null ? FILE_NO_LookUpEdit.EditValue ?? "" : Figh.FILE_NO),
                        new XElement("Fighter_Public", 
                           new XAttribute("frstname", FrstName_Txt.Text),
                           new XAttribute("lastname", LastName_Txt.Text),
                           new XAttribute("natlcode", NatlCode_Txt.Text),
                           new XAttribute("cellphon", CellPhon_Txt.Text),
                           new XAttribute("suntcode", SUNT_CODELookUpEdit.EditValue ?? ""),
                           new XAttribute("servno", SERV_NO_TextEdit.EditValue ?? "")
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

            iScsc.OIC_ESAV_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst.RQID),
                     Rqst.Request_Rows
                     .Select(r =>
                        new XElement("Request_Row",
                           new XAttribute("rwno", r.RWNO),
                           new XAttribute("fileno", r.FIGH_FILE_NO)
                        )
                     ),
                     new XElement("Payment",
                        new XAttribute("setondebt", setOnDebt)
                     )
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
               //Btn_RqstSav1.Visible = false;
               RqstBnASav1.Enabled = false;

               UserProFile_Rb.ImageVisiable = true;
               try
               {
                  var rqro = RqroBs1.Current as Data.Request_Row;

                  UserProFile_Rb.ImageProfile = null;
                  MemoryStream mStream = new MemoryStream();
                  byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", rqro.FIGH_FILE_NO))).ToArray();
                  mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                  Bitmap bm = new Bitmap(mStream, false);
                  mStream.Dispose();

                  if (InvokeRequired)
                     Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
                  else
                     UserProFile_Rb.ImageProfile = bm;
               }
               catch { UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482; }
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               RqstBnDelete1.Enabled /*= Btn_RqstSav1.Visible */= true;
               RqstBnASav1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               RqstBnDelete1.Enabled = /*Btn_RqstSav1.Visible = */false;
               RqstBnASav1.Enabled = false;
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            RqstBnDelete1.Enabled /*= Btn_RqstSav1.Visible*/ = false;
            RqstBnASav1.Enabled = false;
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
         //if (tb_master.SelectedTab == tp_001)
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void bn_PaymentMethods_Click(object sender, EventArgs e)
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
         }
      }

      private void bn_CashPayment_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               //var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
                  return;
               }*/

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
            Pydt_Gv.PostEditor();
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
                  rqst = RqstBs1.Current as Data.Request;

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
                                       new XAttribute("pydtdesc", pd.PYDT_DESC ?? ""),
                                       new XAttribute("qnty", pd.QNTY ?? 1),
                                       new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                                       new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
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
                                    new XAttribute("expncode", pd.EXPN_CODE),
                                    new XAttribute("expnpric", pd.EXPN_PRIC),
                                    new XAttribute("pydtdesc", pd.PYDT_DESC),
                                    new XAttribute("qnty", pd.QNTY ?? 1),
                                    new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                                    new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
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

      private void RQTT_CODE_LookUpEdit_EditValueChanged(object sender, EventArgs e)
      {
         //try
         //{
         //   if (RQTT_CODE_LookUpEdit.ItemIndex == -1) return;

         //   if (PydtsBs1.List.Count > 0)
         //   {
         //      if (MessageBox.Show(this, "برای نوع متقاضی جدید باید هزینه های قبلی پاک شده و دوباره اعلام هزینه کنید. آیا موافقید؟", "حذف تمامی هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         //      PydtsBs1.List.OfType<Data.Payment_Detail>().ToList()
         //         .ForEach(pd =>
         //            {
         //               var rqst = RqstBs1.Current as Data.Request;
         //               iScsc.DEL_SEXP_P(
         //                  new XElement("Request",
         //                     new XAttribute("rqid", rqst.RQID),
         //                     new XElement("Payment",
         //                        new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
         //                        new XElement("Payment_Detail",
         //                           new XAttribute("code", pd.CODE)
         //                        )
         //                     )
         //                  )
         //               );
         //            }
         //         );
         //   }

         //   var rqro = RqroBs1.Current as Data.Request_Row;

         //   ExpnBs1.DataSource =
         //      iScsc.Expenses.Where(ex =>
         //         ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
         //         ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
         //         ex.Expense_Type.Request_Requester.RQTT_CODE == RQTT_CODE_LookUpEdit.EditValue.ToString() &&
         //         ex.MTOD_CODE == rqro.Fighter.MTOD_CODE_DNRM &&
         //         ex.CTGY_CODE == rqro.Fighter.CTGY_CODE_DNRM &&
         //         ex.EXPN_STAT == "002" /* هزینه های فعال */
         //      );
         //}catch(Exception)
         //{
            
         //}

      }

      private void ntb_POSPayment1_Click(object sender, EventArgs e)
      {
         //if (tb_master.SelectedTab == tp_001)
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
                     new Job(SendType.SelfToUserInterface, "REGL_DCMT_F", 10 /* Execute Actn_CalF_P */){Input = new XElement("Regulation", new XElement("Request_Requester", new XAttribute("rqtpcode", "016")))}
                  })
               );
         }         
      }
      private bool setOnDebt = false;
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

            //if (tb_master.SelectedTab == tp_001)
            {

               if (MessageBox.Show(this, "عملیات بدهکاری و ذخیره نهایی کردن انجام شود؟", "بدهکاری و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;
               var pymt = PymtsBs1.Current as Data.Payment;


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

      private void AddItem_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            // اگر در جدول هزینه قبلا رکوردی درج شده باشد
            if (rqst == null) return;

            var expn = ExpnBs1.Current as Data.Expense;

            // چک میکنیم که قبلا از این آیتم هزینه در جدول ریز هزینه وجود نداشته باشد
            if (!PydtsBs1.List.OfType<Data.Payment_Detail>().Any(p => p.EXPN_CODE == expn.CODE))
            {
               PydtsBs1.AddNew();
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; pydt.PAY_STAT = "001"; pydt.RQRO_RWNO = 1; pydt.MTOD_CODE_DNRM = expn.MTOD_CODE; pydt.CTGY_CODE_DNRM = expn.CTGY_CODE; });
            }
            else
            {
               var pydt = PydtsBs1.List.OfType<Data.Payment_Detail>().Where(p => p.EXPN_CODE == expn.CODE).First();
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.QNTY += 1; });
            }

            PydtsBs1.EndEdit();
            iScsc.SubmitChanges();

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
               Execute_Query();
               requery = false;
            }
         }
      }

      private void LOV_EXPN_EditValueChanged(object sender, EventArgs e)
      {
         var pydt = PydtsBs1.Current as Data.Payment_Detail;

         if (((LookUpEdit)sender).EditValue != null)
         {
            if ((pydt.EXPN_PRIC ?? 0) == 0)
            {
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == (long)((LookUpEdit)sender).EditValue).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; });
            }
         }
      }

      private void advBandedGridView1_RowCountChanged(object sender, EventArgs e)
      {
         if(Expn_Gv.RowCount == 1)
         {
            //AddItem_ButtonClick(null, null);
            //advBandedGridView1.ApplyFindFilter("");
            //FindControl find = advBandedGridView1.GridControl.Controls.Find("FindControl", true)[0] as FindControl;
            //find.FindEdit.Focus();
         }
      }

      private void Butn_TakePicture_Click(object sender, EventArgs e)
      {
         try
         {
            if (true)
            {
               var fileno = (RqroBs1.Current as Data.Request_Row).FIGH_FILE_NO;
               var rqst = (from r in iScsc.Requests
                           join rr in iScsc.Request_Rows on r.RQID equals rr.RQST_RQID
                           where rr.FIGH_FILE_NO == Convert.ToInt64(fileno)
                              && (r.RQTP_CODE == "001" || r.RQTP_CODE == "025")
                           select r).FirstOrDefault();
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

      private void Btn_IncressAccount_Click(object sender, EventArgs e)
      {
         try
         {
            //var glrl = GlrlBs3.Current as Data.Gain_Loss_Rial;
            var rqro = RqroBs1.Current as Data.Request_Row;

            if (rqro == null) return;
            //if (Txt_Amnt.EditValue == null || Txt_Amnt.EditValue.ToString() == "") return;

            if (MessageBox.Show(this, "آیا ثبت اعتبار برای عضو مورد نظر ثبت گردد؟", "افزایش اعتبار اعضا", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            // این قسمت برنامه باید به واحد خودش در تغییرات ریالی انتقال پیدا کند تا از پرداکندگی کدها جلوگیری شود
            #region Gain_Loss_Rials
            iScsc.GLR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqstrqid", rqro.RQST_RQID),
                     new XAttribute("rqid", 0),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_003_F"),
                     new XElement("Request_Row",
                        new XAttribute("fighfileno", rqro.FIGH_FILE_NO)
                     ),
                     new XElement("Gain_Loss_Rials",
                        new XAttribute("chngtype", "002"),
                        new XAttribute("debttype", "005"),
                        new XAttribute("amnt", 0 /*Txt_Amnt.EditValue*/),
                        new XAttribute("paiddate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("chngresn", "006"),
                        new XAttribute("resndesc", "ثبت مبلغ افزایش سپرده * ثبت سیستمی")
                     )
                  )
               )
            );

            var rqst = iScsc.Requests.FirstOrDefault(r => r.RQTP_CODE == "020" && r.RQTT_CODE == "004" && r.MDUL_NAME == GetType().Name && r.SECT_NAME == GetType().Name.Substring(0, 3) + "_003_F" && r.Request_Rows.Any(rr => rr.FIGH_FILE_NO == rqro.FIGH_FILE_NO) && r.RQST_STAT == "001");
            
            if (rqst == null) return;

            iScsc.GLR_TSAV_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", rqst == null ? 0 : rqst.RQID)
                  )
               )
            );
            #endregion

            //Txt_Amnt.EditValue = 0;

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

      private void ntb_POSPayment1_Click_1(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               if (MessageBox.Show(this, "عملیات پرداخت و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               if (VPosBs1.List.Count == 0)
                  UsePos_Cb.Checked = false;

               if (UsePos_Cb.Checked)
               {
                  foreach (Data.Payment pymt in PymtsBs1)
                  {
                     var amnt = ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
                     if (amnt == 0) return;

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
                  Btn_RqstBnASav1_Click(null, null);
               }
            }
         }
         catch (SqlException se)
         {
            MessageBox.Show(se.Message);
         }
      }

      private void FgpbBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            if (FgpbBs1.Current != null)
               FreeAdm_Pn.Visible = true;
            else
               FreeAdm_Pn.Visible = false;
         }
         catch (Exception )
         {

         }
      }

      private void RemoveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
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

      private void SaveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Pydt_Gv.PostEditor();
            /* Do Something for insert or update Payment Detail Price */
            var rqst = RqstBs1.Current as Data.Request;

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
                              new XAttribute("pydtdesc", pd.PYDT_DESC ?? ""),
                              new XAttribute("qnty", pd.QNTY ?? 1),
                              new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
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
                              new XAttribute("expncode", pd.EXPN_CODE),
                              new XAttribute("expnpric", pd.EXPN_PRIC),
                              new XAttribute("pydtdesc", pd.PYDT_DESC),
                              new XAttribute("qnty", pd.QNTY ?? 1),
                              new XAttribute("fighfileno", pd.FIGH_FILE_NO ?? 0),
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0)
                           )
                        )
                     )
                  );
               }
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

      private void PydtsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;

            requery = true;
            CBMT_CODE_GridLookUpEdit.EditValue = pydt.CBMT_CODE_DNRM;
            requery = false;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CBMT_CODE_GridLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            if (requery) { requery = false; return; }
            var pydt = PydtsBs1.Current as Data.Payment_Detail;
            if (pydt == null) return;
            
            //pydt.Club_Method = iScsc.Club_Methods.FirstOrDefault(cm => cm.CODE == (long?)e.NewValue);
            iScsc.UPD_SEXP_P(
               new XElement("Request",
                  new XAttribute("rqid", pydt.PYMT_RQST_RQID),
                  new XElement("Payment",
                     new XAttribute("cashcode", pydt.PYMT_CASH_CODE),
                     new XElement("Payment_Detail",
                        new XAttribute("code", pydt.CODE),
                        new XAttribute("expncode", pydt.EXPN_CODE),
                        new XAttribute("expnpric", pydt.EXPN_PRIC),
                        new XAttribute("pydtdesc", pydt.PYDT_DESC),
                        new XAttribute("qnty", pydt.QNTY ?? 1),
                        new XAttribute("fighfileno", pydt.FIGH_FILE_NO ?? 0),
                        new XAttribute("cbmtcodednrm", e.NewValue ?? 0)
                     )
                  )
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

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }
   }
}
