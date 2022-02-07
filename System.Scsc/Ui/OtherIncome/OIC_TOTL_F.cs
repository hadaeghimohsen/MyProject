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
using System.Scsc.ExtCode;
using System.Threading;

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

            var Rqids = iScsc.VF_Requests(new XElement("Request", new XAttribute("cretby", ShowRqst_PickButn.PickChecked ? CurrentUser : "")))
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

            // 1397/05/15 * بدست آوردن شماره پرونده های درگیر در تمدید
            FighsBs1.DataSource = 
               iScsc.Fighters
               .Where(f => 
                  f.CONF_STAT == "002" &&
                  Rqids.Contains((long)f.RQST_RQID) &&
                  /*&& (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "004" || 
                   *    f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")*/ 
                  /*(Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || 
                      (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) &&*/
                  Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101)
               .OrderBy(f => f.FGPB_TYPE_DNRM);
            //FighBs1.DataSource = iScsc.Fighters.Where(f => );

            Grop_FLP.Controls.Clear();
            var allItems = new Button();

            allItems.Text = "همه موارد";
            allItems.Tag = 0;

            allItems.Click += GropButn_Click;
            Grop_FLP.Controls.Add(allItems);

            ExpnBs1.List.OfType<Data.Expense>().OrderBy(e => e.GROP_CODE).GroupBy(e => e.Group_Expense1).ToList().ForEach(
               g =>
               {
                  var b = new Button() { AutoSize = true };
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
               //*Gb_ExpenseItem.Visible = true;
               //Btn_RqstDelete1.Visible = true;
               //Btn_RqstSav1.Visible = false;
               RqstBnDelete1.Enabled = true;
            }
            else if (!(Rqst.SSTT_MSTT_CODE == 2 && (Rqst.SSTT_CODE == 1 || Rqst.SSTT_CODE == 2)) && Rqst.RQID > 0)
            {
               Gb_Expense.Visible = false;
               //*Gb_ExpenseItem.Visible = true;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = true;
               RqstBnDelete1.Enabled = true;
            }
            else if (Rqst.RQID == 0)
            {
               Gb_Expense.Visible = false;
               //*Gb_ExpenseItem.Visible = false;
               //Btn_RqstDelete1.Visible = Btn_RqstSav1.Visible = false;
               RqstBnDelete1.Enabled = false;
            }
         }
         catch
         {
            Gb_Expense.Visible = false;
            //*Gb_ExpenseItem.Visible = false;
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
                     new XAttribute("rqstrqid", rqstRqid),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XAttribute("rqstdesc", RqstDesc_Txt.EditValue ?? ""),
                     new XAttribute("lettno", Rqst == null ? "" : Rqst.LETT_NO ?? ""),
                     new XAttribute("lettdate", Rqst == null ? "" : (Rqst.LETT_NO == null ? "" : ( Rqst.LETT_DATE == null ? "" : Rqst.LETT_DATE.Value.ToString("yyyy-MM-dd")))),
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
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs1.List.Count == 0)
                  RqstBnExit1_Click(null, null);
               else
                  Create_Record();
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
            // 1398/04/03 * به درخواست باشگاه بهاران خانم نقیبی قرار شد برای مشتریان مهمان گزینه ای اضافه کنیم که مدام بعد از اتمام درخواست از فرم خارج نشوند
            //  برای اینکار ما بایستی گزینه ای طراحی کنیم که اگر فعال باشد بتوانیم مشخص کنیم که اگر هنرجو مهمان هست دوباره درخواست دیگیری برای ان ثبت شود
            if(GustSaveRqst_PickButn.PickChecked)
            {
               if(FighsBs1.List.OfType<Data.Fighter>().Any(f => f.FILE_NO ==  Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO && f.FGPB_TYPE_DNRM == "005"))
               {
                  if (RqstBs1.Count > 0)
                     RqstBs1.AddNew();

                  FILE_NO_LookUpEdit.EditValue = Rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO;

                  Btn_RqstBnARqt1_Click(null, null);
               }
            }

            // 1399/12/10 * اضافه کردن فرم مربوط به ثبت اطلاعات درخواست مشتریان
            if(followups != "")
            {
               switch (followups.Split(';').First())
               {
                  case "GLR_INDC_F":
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                              new List<Job>
                                 {                  
                                    new Job(SendType.Self, 153 /* Execute Glr_Indc_F */),
                                    new Job(SendType.SelfToUserInterface, "GLR_INDC_F", 10 /* Execute Actn_CalF_F */)
                                    {
                                       Input = 
                                          new XElement("Request", 
                                             new XAttribute("type", "newrequest"), 
                                             new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO),
                                             new XAttribute("followups", followups.Substring(followups.IndexOf(";") + 1)),
                                             new XAttribute("rqstrqid", Rqst.RQID),
                                             new XAttribute("formcaller", formCaller)
                                          )
                                    }
                                 })
                     );
                     formCaller = "";
                     break;
                  default:
                     break;
               }
               followups = "";
               rqstRqid = 0;
            }

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
               // 1397/05/16 * اگر درخواستی وجود نداشته باشد فرم مربوط را ببندیم
               if (RqstBs1.List.Count == 0)
                  RqstBnExit1_Click(null, null);
               else
               {
                  if(!GustSaveRqst_PickButn.PickChecked)
                     Create_Record();
               }
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
               var rqro = Rqst.Request_Rows.FirstOrDefault() as Data.Request_Row;
               try
               {
                  //var rqro = RqroBs1.Current as Data.Request_Row;

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

               MbspBs.DataSource = iScsc.Member_Ships.Where(mb => mb.FIGH_FILE_NO == rqro.FIGH_FILE_NO && mb.RECT_CODE == "004" && (mb.TYPE == "001" || mb.TYPE == "005"));
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrintAfterFinish"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
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
               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
               if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
               {
                  var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
                  if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
                  {
                     FrstName_Txt.Focus();
                     return;
                  }

                  if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
                  {
                     LastName_Txt.Focus();
                     return;
                  }

                  if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
                  {
                     CellPhon_Txt.Focus();
                     return;
                  }

                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
               }

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs1.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> نقدی << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }
               //var pymt = PymtsBs1.Current as Data.Payment;

               /*if ((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT) <= 0)
               {
                  MessageBox.Show(this, "تمام هزینه های بدهی مشتری پرداخت شده");
                  return;
               }*/

               // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
               if(PydtsBs1.List.Count == 0)
               {
                  MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
                  return;
               }

               foreach (Data.Payment pymt in PymtsBs1)
               {
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "CheckoutWithoutPOS"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                           )
                        )
                     )
                  );
               }

               // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
               PymtVldtType_Cbx.Checked = true;

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
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
               if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
               {
                  var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
                  if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
                  {
                     FrstName_Txt.Focus();
                     return;
                  }

                  if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
                  {
                     LastName_Txt.Focus();
                     return;
                  }

                  if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
                  {
                     CellPhon_Txt.Focus();
                     return;
                  }

                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
               }

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs1.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> کارتخوان << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }

               if (VPosBs1.List.Count == 0)
                  UsePos_Cb.Checked = false;

               // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
               if (PydtsBs1.List.Count == 0)
               {
                  MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
                  return;
               }

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
                  // 1397/01/07 * ثبت دستی مبلغ به صورت پایانه فروش
                  foreach (Data.Payment pymt in PymtsBs1)
                  {
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "CheckoutWithPOS"),
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", pymt.CASH_CODE),
                                 new XAttribute("rqstrqid", pymt.RQST_RQID),
                                 new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                              )
                           )
                        )
                     );
                  }

                  // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
                  PymtVldtType_Cbx.Checked = true;

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
         ////if (tb_master.SelectedTab == tp_001)
         //{
         //   if (RqstBs1.Current == null) return;
         //   var rqst = RqstBs1.Current as Data.Request;
         //   var pymt = PymtsBs1.Current as Data.Payment;

         //   var xSendPos =
         //      new XElement("Form",
         //         new XAttribute("name", GetType().Name),
         //         new XAttribute("tabpage", "tp_001"),
         //         new XElement("Request",
         //            new XAttribute("rqid", rqst.RQID),
         //            new XAttribute("rqtpcode", rqst.RQTP_CODE),
         //            new XAttribute("fileno", rqst.Fighters.FirstOrDefault().FILE_NO),
         //            new XElement("Payment",
         //               new XAttribute("cashcode", pymt.CASH_CODE),
         //               new XAttribute("amnt", (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - pymt.Payment_Methods.Sum(pm => pm.AMNT))
         //            )
         //         )
         //      );

         //   Job _InteractWithScsc =
         //     new Job(SendType.External, "Localhost",
         //        new List<Job>
         //         {
         //            new Job(SendType.Self, 93 /* Execute Pos_Totl_F */),
         //            new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
         //         });
         //   _DefaultGateway.Gateway(_InteractWithScsc);
         //}
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

               var rqst = RqstBs1.Current as Data.Request;
               if (rqst == null) return;

               // 1400/11/18 * If Service is Guest AND User Must Fill First, Last Name AND CellPhone
               if (FrstLastCellRqur_Cbx.Checked && rqst.Request_Rows.FirstOrDefault().Fighter.FGPB_TYPE_DNRM == "005")
               {
                  var fp = rqst.Request_Rows.FirstOrDefault().Fighter_Publics.FirstOrDefault();
                  if (fp.FRST_NAME == null || fp.FRST_NAME.Length == 0)
                  {
                     FrstName_Txt.Focus();
                     return;
                  }

                  if (fp.LAST_NAME == null || fp.LAST_NAME.Length == 0)
                  {
                     LastName_Txt.Focus();
                     return;
                  }

                  if (fp.CELL_PHON == null || fp.CELL_PHON.Length == 0)
                  {
                     CellPhon_Txt.Focus();
                     return;
                  }

                  iScsc.ExecuteCommand(string.Format("UPDATE dbo.Fighter_Public SET Frst_Name = N'{0}', Last_Name = N'{1}', Cell_Phon = '{2}' WHERE Rqro_Rqst_Rqid = {3};", FrstName_Txt.Text, LastName_Txt.Text, CellPhon_Txt.Text, rqst.RQID));
               }

               if (Accept_Cb.Checked)
               {
                  var pymt = PymtsBs1.Current as Data.Payment;
                  if (pymt == null) return;

                  var debtamnt = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);

                  string mesg = "";
                  if (debtamnt > 0)
                  {
                     mesg =
                        string.Format(
                           ">> مبلغ {0} {1} به صورت >> بدهکار << در تاریخ {2} در صندوق کاربر {3}  قرار میگیرد",
                           string.Format("{0:n0}", debtamnt),
                           DAtypBs1.List.OfType<Data.D_ATYP>().FirstOrDefault(d => d.VALU == pymt.AMNT_UNIT_TYPE_DNRM).DOMN_DESC,
                           "امروز",
                           CurrentUser);
                     mesg += Environment.NewLine;
                  }
                  else
                     setOnDebt = false;

                  mesg += ">> ذخیره و پایان درخواست";

                  if (MessageBox.Show(this, mesg, "عملیات ثبت نام", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               }

               // 1398/04/03 * اگر فاکتور فاقد آیتم هزینه باشد اجازه ثبت در سیستم را نداریم
               if (PydtsBs1.List.Count == 0)
               {
                  MessageBox.Show(this, "فاکتور بدون آیتم هزینه می باشد، لطفا آیتم مورد نظر خود را انتخاب کنید");
                  return;
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
               ExpnBs1.List.OfType<Data.Expense>().Where(ex => ex.CODE == expn.CODE).ToList().ForEach(ex => { pydt.EXPN_CODE = ex.CODE; pydt.EXPN_PRIC = ex.PRIC; pydt.EXPN_EXTR_PRCT = ex.EXTR_PRCT; pydt.QNTY = 1; pydt.PYDT_DESC = ex.EXPN_DESC; pydt.PAY_STAT = "001"; pydt.RQRO_RWNO = 1; pydt.MTOD_CODE_DNRM = expn.MTOD_CODE; pydt.CTGY_CODE_DNRM = expn.CTGY_CODE; pydt.EXPR_DATE = DateTime.Now.AddDays((int)expn.NUMB_CYCL_DAY).AddHours(expn.MIN_TIME.Value.Hour).AddMinutes(expn.MIN_TIME.Value.Minute).AddSeconds(expn.MIN_TIME.Value.Second); });
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
            //if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            ///* Do Delete Payment_Detail */
            //var Crnt = PydtsBs1.Current as Data.Payment_Detail;
            //var rqst = RqstBs1.Current as Data.Request;
            //iScsc.DEL_SEXP_P(
            //   new XElement("Request",
            //      new XAttribute("rqid", rqst.RQID),
            //      new XElement("Payment",
            //         new XAttribute("cashcode", rqst.Payments.SingleOrDefault().CASH_CODE),
            //         new XElement("Payment_Detail",
            //            new XAttribute("code", Crnt.CODE)
            //         )
            //      )
            //   )
            //);


            //if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            /* Do Delete Payment_Detail */
            var Crnt = PydtsBs1.Current as Data.Payment_Detail;
            if (Crnt == null) return;
            if (Crnt.QNTY > 1)
            {
               Crnt.QNTY--;
               PydtsBs1.EndEdit();
               iScsc.SubmitChanges();
            }
            else
            {
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
            }
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
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0),
                              new XAttribute("exprdate", pd.EXPR_DATE == null ? "" : pd.EXPR_DATE.Value.ToString("yyyy-MM-dd"))
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
                              new XAttribute("cbmtcodednrm", pd.CBMT_CODE_DNRM ?? 0),
                              new XAttribute("exprdate", pd.EXPR_DATE == null ? "" : pd.EXPR_DATE.Value.ToString("yyyy-MM-dd"))
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
            MbspRwnoPydt_Lov.EditValue = pydt.MBSP_RWNO;
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
                        new XAttribute("cbmtcodednrm", e.NewValue ?? 0),
                        new XAttribute("exprdate", pydt.EXPR_DATE == null ? "" : pydt.EXPR_DATE.Value.ToString("yyyy-MM-dd"))
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

      private void PydsType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PydsType_Butn.Text = PydsType_Butn.Tag.ToString() == "0" ? "مبلغی" : "درصدی";
            PydsType_Butn.Tag = PydsType_Butn.Tag.ToString() == "0" ? "1" : "0";
            if (PydsType_Lov.EditValue != null || PydsType_Lov.EditValue.ToString() != "") PydsType_Lov.EditValue = "002";

            if (PydsType_Butn.Tag.ToString() == "0")
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "درصد تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 3;
            }
            else
            {
               PydsAmnt_Txt.Properties.NullText = PydsAmnt_Txt.Properties.NullValuePrompt = "مبلغ تخفیف";
               PydsAmnt_Txt.Properties.MaxLength = 0;
            }
            PydsAmnt_Txt.Focus();
         }
         catch { }
      }

      private void RcmtType_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            RcmtType_Butn.Text = RcmtType_Butn.Tag.ToString() == "0" ? "POS" : "نقدی";
            RcmtType_Butn.Tag = RcmtType_Butn.Tag.ToString() == "0" ? "1" : "0";
            PymtAmnt_Txt.Focus();
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;
            PymtAmnt_Txt.EditValue = (pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
         }
         catch { }
      }

      private void SavePyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            long? amnt = null;
            switch (PydsType_Butn.Tag.ToString())
            {
               case "0":
                  if (!(Convert.ToInt32(PydsAmnt_Txt.EditValue) >= 0 && Convert.ToInt32(PydsAmnt_Txt.EditValue) <= 100))
                  {
                     PydsAmnt_Txt.EditValue = null;
                     PydsAmnt_Txt.Focus();
                  }

                  amnt = (pymt.SUM_EXPN_PRIC * Convert.ToInt64(PydsAmnt_Txt.EditValue)) / 100;
                  break;
               case "1":
                  amnt = Convert.ToInt32(PydsAmnt_Txt.EditValue);
                  if (amnt == 0) return;
                  break;
            }

            iScsc.INS_PYDS_P(pymt.CASH_CODE, pymt.RQST_RQID, (short?)1, null, amnt, PydsType_Lov.EditValue.ToString(), "002", PydsDesc_Txt.Text);

            PydsAmnt_Txt.EditValue = null;
            PydsDesc_Txt.EditValue = null;
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

      private void DeltPyds_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pyds = PydsBs1.Current as Data.Payment_Discount;
            if (pyds == null) return;

            iScsc.DEL_PYDS_P(pyds.PYMT_CASH_CODE, pyds.PYMT_RQST_RQID, pyds.RQRO_RWNO, pyds.RWNO);

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

      private void SavePymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            PymtDate_DateTime001.CommitChanges();
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) return;

            if (PymtAmnt_Txt.EditValue == null || PymtAmnt_Txt.EditValue.ToString() == "" || Convert.ToInt64(PymtAmnt_Txt.EditValue) == 0) return;

            switch (RcmtType_Butn.Tag.ToString())
            {
               case "0":
                  iScsc.PAY_MSAV_P(
                     new XElement("Payment",
                        new XAttribute("actntype", "InsertUpdate"),
                        new XElement("Insert",
                           new XElement("Payment_Method",
                              new XAttribute("cashcode", pymt.CASH_CODE),
                              new XAttribute("rqstrqid", pymt.RQST_RQID),
                              new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                              new XAttribute("rcptmtod", "001"),
                              new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                              new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                           )
                        )
                     )
                  );

                  // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
                  PymtVldtType_Cbx.Checked = true;
                  break;
               case "1":
                  if (VPosBs1.List.Count == 0) UsePos_Cb.Checked = false;

                  if (UsePos_Cb.Checked)
                  {
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
                        PymtAmnt_Txt.EditValue = Convert.ToInt64( PymtAmnt_Txt.EditValue ) * 10;

                     // از این گزینه برای این استفاده میکنیم که بعد از پرداخت نباید درخواست ثبت نام پایانی شود
                     UsePos_Cb.Checked = false;

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
                                             new XAttribute("amnt", Convert.ToInt64( PymtAmnt_Txt.EditValue) )
                                          )
                                    }
                                 }
                              )
                           }
                        )
                     );

                     UsePos_Cb.Checked = true;
                  }
                  else
                  {
                     iScsc.PAY_MSAV_P(
                        new XElement("Payment",
                           new XAttribute("actntype", "InsertUpdate"),
                           new XElement("Insert",
                              new XElement("Payment_Method",
                                 new XAttribute("cashcode", pymt.CASH_CODE),
                                 new XAttribute("rqstrqid", pymt.RQST_RQID),
                                 new XAttribute("amnt", PymtAmnt_Txt.EditValue ?? 0),
                                 new XAttribute("rcptmtod", "003"),
                                 new XAttribute("actndate", PymtDate_DateTime001.Value.HasValue ? PymtDate_DateTime001.Value.Value.Date.ToString("yyyy-MM-dd") : DateTime.Now.Date.ToString("yyyy-MM-dd")),
                                 new XAttribute("valdtype", PymtVldtType_Cbx.Checked ? "002" : "001")
                              )
                           )
                        )
                     );

                     // 1399/12/09 * بعد از اینکه مبلغ دریافتی درون سیستم ثبت شد گزینه به حالت فعال درآید
                     PymtVldtType_Cbx.Checked = true;
                  }
                  break;
               default:
                  break;
            }

            PymtAmnt_Txt.EditValue = null;
            PymtDate_DateTime001.Value = DateTime.Now;
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

      private void DeltPymt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pmmt = PmmtBs1.Current as Data.Payment_Method;
            if (pmmt == null) return;

            iScsc.PAY_MSAV_P(
               new XElement("Payment",
                  new XAttribute("actntype", "Delete"),
                  new XAttribute("cashcode", pmmt.PYMT_CASH_CODE),
                  new XAttribute("rqstrqid", pmmt.PYMT_RQST_RQID),
                  new XAttribute("rwno", pmmt.RWNO)
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

      private void Expn_Gv_DoubleClick(object sender, EventArgs e)
      {
         AddItem_ButtonClick(null, null);
      }

      private void ShowRqst_PickButn_PickCheckedChange(object sender)
      {
         Execute_Query();
      }

      private void LOV_EXPRDATE_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Index == 1)
            {
               Pydt_Gv.PostEditor();
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               if(pydt.EXPR_DATE == null && MessageBox.Show(this, "تاریخ خالی می باشد آیا میخواهید تاریخ اعتبار همگی خالی شود؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(p => p.EXPR_DATE = null);
               }
               else
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(p => p.EXPR_DATE = pydt.EXPR_DATE);
               }

               SaveExpn_Butn_Click(null, null);
            }
            else if(e.Button.Index == 2)
            {
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               if (pydt == null) return;

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 161 /* Execute Bas_Prod_F */),
                        new Job(SendType.SelfToUserInterface,"BAS_PROD_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Product", new XAttribute("epitcode", pydt.Expense.Expense_Type.Expense_Item.CODE), new XAttribute("pydtcode", pydt.CODE), new XAttribute("formstat", "sale"))}
                     }
                  )
               );
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SubmitExpiDate_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null || pymt.Payment_Details.Count == 0) return;

            if (SlctAllPydt_Cb.Checked)
            {
               if (NumbDay_Txt.Text == "0" && NumbMonth_Txt.Text == "0")
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(pd => pd.EXPR_DATE = null);
               }
               else
               {
                  PydtsBs1.List.OfType<Data.Payment_Detail>().ToList().ForEach(pd => pd.EXPR_DATE = DateTime.Now.AddDays(Convert.ToDouble( NumbDay_Txt.Text)).AddMonths(Convert.ToInt32(NumbMonth_Txt.Text)));
               }
            }
            else
            {
               var pydt = PydtsBs1.Current as Data.Payment_Detail;
               if (pydt == null) return;
               if (NumbDay_Txt.Text == "0" && NumbMonth_Txt.Text == "0")
               {
                  pydt.EXPR_DATE = null;
               }
               else
               {
                  pydt.EXPR_DATE = DateTime.Now.AddDays(Convert.ToDouble(NumbDay_Txt.Text)).AddMonths(Convert.ToInt32(NumbMonth_Txt.Text));
               }
            }

            SaveExpn_Butn_Click(null, null);
         }
         catch ( Exception exp)
         {
            MessageBox.Show(exp.Message);
         }
      }

      private void ExpnBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if(expn == null) return;

            ExpnItem_Tsmi.Text = ExpnDesc_Tsmi.Text = expn.EXPN_DESC;
            ExpnPric_Tsmi.Text = expn.PRIC.ToString();
            ExpnOrdrItem_Tsmi.Text = expn.ORDR_ITEM;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ExpnItem_Tsmi_Click(object sender, EventArgs e)
      {
         AddItem_ButtonClick(null, null);
         Expn_Gc.Focus();
      }

      private void ExpnEdit_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_DESC = ExpnDesc_Tsmi.Text;
            expn.PRIC = Convert.ToInt64(ExpnPric_Tsmi.Text);
            if (ExpnOrdrItem_Tsmi.Text == "")
               expn.ORDR_ITEM = null;
            else
               expn.ORDR_ITEM = ExpnOrdrItem_Tsmi.Text;

            iScsc.SubmitChanges();
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

      private void DupExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", expn.CODE),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", DupExpnText_Tsmi.Text),
                  new XAttribute("pric", DupExpnPric_Tsmi.Text),
                  new XAttribute("ordritem", DupExpnOrdrItem_Tsmi.Text)
               )
            );

            DupExpnPric_Tsmi.Text = DupExpnText_Tsmi.Text = DupExpnOrdrItem_Tsmi.Text = "";

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

      private void OffExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            expn.EXPN_STAT = "001";
            iScsc.SubmitChanges();
            requery = true;               
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
               Execute_Query();
         }
      }

      private void NewExpn_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            iScsc.DUP_EXPN_P(
               new XElement("Expense",
                  new XAttribute("code", ""),
                  new XAttribute("rqtpcode", expn.Expense_Type.Request_Requester.RQTP_CODE),
                  new XAttribute("rqttcode", expn.Expense_Type.Request_Requester.RQTT_CODE),
                  new XAttribute("desc", NewExpnText_Tsmi.Text),
                  new XAttribute("pric", NewExpnPric_Tsmi.Text),
                  new XAttribute("ordritem", NewExpnOrdrItem_Tsmi.Text)
               )
            );

            NewExpnPric_Tsmi.Text = NewExpnText_Tsmi.Text = "";

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

      private void ServFind_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            ServFind_Tsmi.Text = "جستجوی مشتری";

            var fighs = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text));
            if(fighs.Count() == 0)
               return;

            if(fighs.Count() == 1)
            {
               ServFind_Tsmi.Text = string.Format("{0} : {1}", "جستجوی مشتری", fighs.FirstOrDefault().NAME_DNRM);

               if(SaveAutoRqst_Tsmi.CheckState == CheckState.Checked)
               {
                  // Create Request for Service
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                        new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", fighs.FirstOrDefault().FILE_NO)))}
                        })
                  );

                  // اولین گام این هست که ببینیم آیا ما توانسته ایم برای مشترک درخواست درآمد متفرقه ثبت کنیم یا خیر
                  fighs = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text));
                  var figh = fighs.FirstOrDefault();
                  if(!(figh.FIGH_STAT == "001" && figh.RQST_RQID != null && figh.Request.RQTP_CODE == "016" && figh.Request.RQTT_CODE == "001"))
                  {
                     MessageBox.Show("ثبت درخواست برای مشتری با مشکلی مواجه شده است، لطفا بررسی کنید");
                     return;
                  }

                  // Find Request for Service
                  RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.Request_Rows.Any(rr => rr.Fighter.FILE_NO == fighs.FirstOrDefault().FILE_NO)));
               }

               // مبلغ بدهی مشتری
               PayDebtAmnt2_Tsmi.Text = fighs.FirstOrDefault().DEBT_DNRM.ToString();

               DebtMenu_Tsmi.Enabled = ServPymt_Tsmi.Enabled = true;
            }
            else
            {
               ServFind_Tsmi.Text = string.Format("{0} : {1} {2}", "جستجوی مشتری", fighs.Count(), "مشتری پیدا شد");
               // مبلغ بدهی مشتری
               PayDebtAmnt2_Tsmi.Text = "0";

               DebtMenu_Tsmi.Enabled = ServPymt_Tsmi.Enabled = false;
            }

            Serv_Cms.Show();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveAutoRqst_Tsmi_Click(object sender, EventArgs e)
      {
         switch (SaveAutoRqst_Tsmi.CheckState)
         {
            case CheckState.Checked:
               SaveAutoRqst_Tsmi.CheckState = CheckState.Unchecked;
               break;
            case CheckState.Unchecked:
               SaveAutoRqst_Tsmi.CheckState = CheckState.Checked;
               break;
         }

         Serv_Cms.Show();
      }

      private void FindResult_Tsmi_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {                  
                  new Job(SendType.Self, 45 /* Execute Lsi_Fldf_F */),
                  new Job(SendType.SelfToUserInterface, "LSI_FLDF_F", 10 /* Actn_CalF_P */){Input = new XElement("Fighter", new XAttribute("showlist", "001"), new XAttribute("filtering", "cellphon"), new XAttribute("filter_value", CellPhonFind_Tsmi.Text))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      #region پرداخت بدهی قبلی
      private void PayCashDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "001"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayPosDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "003"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }

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

      private void PayDepositeDebt2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            // اگر مشترکی وجود نداشته باشد
            if (figh == null) return;
            // اگر مشتری بدهی نداشته باشد
            if (figh.DEBT_DNRM == 0) return;
            // اگر مشتری در فرآیندی قفل باشد اجازه پرداخت بدهی وجود ندارد
            //if (figh.FIGH_STAT == "001") return;

            var paydebt = Convert.ToInt64(PayDebtAmnt2_Tsmi.Text.Replace(",", ""));
            // مبلغ پرداخت بیشتر از مبلغ بدهی می باشد
            if (paydebt > figh.DEBT_DNRM) return;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);
            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "InsertUpdate"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "005"),
                           new XAttribute("actndate", DateTime.Now.Date.ToString("yyyy-MM-dd"))
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }
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
      #endregion

      private void SaveServ2_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            iScsc.BYR_TRQT_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", 0),
                     new XAttribute("rqtpcode", "025"),
                     new XAttribute("rqttcode", "004"),
                     new XAttribute("prvncode", "017"),
                     new XAttribute("regncode", "001"),
                     new XElement("Fighter",
                        new XAttribute("fileno", 0),
                        new XElement("Frst_Name", FrstName2_Tsmi.Text),
                        new XElement("Last_Name", LastName2_Tsmi.Text),
                        new XElement("Fath_Name", ""),
                        new XElement("Sex_Type", "001"),
                        new XElement("Cell_Phon", CellPhon2_Tsmi.Text),
                        new XElement("Type", "001"),
                        new XElement("Fngr_Prnt",
                           iScsc.Fighters
                            .Where(f => f.FNGR_PRNT_DNRM != null && f.FNGR_PRNT_DNRM.Length > 0)
                            .Select(f => f.FNGR_PRNT_DNRM)
                            .ToList()
                            .Where(f => f.All(char.IsDigit))
                            .Max(f => Convert.ToInt64(f)) + 1)
                     ),
                     new XElement("Member_Ship",
                        new XAttribute("strtdate", DateTime.Now.ToString("yyyy-MM-dd")),
                        new XAttribute("enddate", DateTime.Now.AddYears(120).ToString("yyyy-MM-dd"))
                     )
                  )
               )
            );

            var Rqids = iScsc.VF_Requests(new XElement("Request"))
                  .Where(rqst =>
                        rqst.RQTP_CODE == "025" &&
                        rqst.RQST_STAT == "001" &&
                        rqst.RQTT_CODE == "004" &&
                        rqst.CRET_BY == CurrentUser &&
                        rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            var Rqst =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID)
               ).FirstOrDefault();

            iScsc.BYR_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", Rqst.RQID),
                        new XAttribute("prvncode", Rqst.REGN_PRVN_CODE),
                        new XAttribute("regncode", Rqst.REGN_CODE),
                        new XElement("Fighter",
                           new XAttribute("fileno", Rqst.Fighters.FirstOrDefault().FILE_NO)
                        )
                     )
                  )
               );

            FrstName2_Tsmi.Text = LastName2_Tsmi.Text = CellPhon2_Tsmi.Text = "";

            var figh = Rqst.Fighters.FirstOrDefault();

            if (SaveAutoRqst_Tsmi.CheckState == CheckState.Checked)
            {
               // Create Request for Service
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                        {                  
                           new Job(SendType.Self, 92 /* Execute Oic_Totl_F */),
                           new Job(SendType.SelfToUserInterface, "OIC_TOTL_F", 10 /* Execute Actn_CalF_F */){Input = new XElement("Request", new XAttribute("type", "01"), new XElement("Request_Row", new XAttribute("fileno", figh.FILE_NO)))}
                        })
               );

               // اولین گام این هست که ببینیم آیا ما توانسته ایم برای مشترک درخواست درآمد متفرقه ثبت کنیم یا خیر
               figh = iScsc.Fighters.Where(f => f.FILE_NO == figh.FILE_NO).FirstOrDefault();
               if (!(figh.FIGH_STAT == "001" && figh.RQST_RQID != null && figh.Request.RQTP_CODE == "016" && figh.Request.RQTT_CODE == "001"))
               {
                  MessageBox.Show("ثبت درخواست برای مشتری با مشکلی مواجه شده است، لطفا بررسی کنید");
                  return;
               }

               // Find Request for Service
               RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.Request_Rows.Any(rr => rr.Fighter.FILE_NO == figh.FILE_NO)));
            }

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

      private void ServPymt_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var fileno = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.ACTV_TAG_DNRM.CompareTo("101") >= 0 && f.CELL_PHON_DNRM.Contains(CellPhonFind_Tsmi.Text)).FirstOrDefault().FILE_NO;

            var figh = FighBs1.List.OfType<Data.Fighter>().FirstOrDefault(f => f.FILE_NO == (long)fileno);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 46 /* Execute All_Fldf_F */){
                        Input = 
                           new XElement("Fighter",
                              new XAttribute("fileno", figh.FILE_NO)
                           )
                     },
                     new Job(SendType.SelfToUserInterface, "ALL_FLDF_F", 10 /* Execute Actn_CalF_F*/ )
                     {
                        Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", figh.FILE_NO),
                           new XAttribute("type", "refresh"),
                           new XAttribute("tabfocued", "tp_003")
                        )
                     }
                  })
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ProdList_Tsmi_Click(object sender, EventArgs e)
      {
         try
         {
            var expn = ExpnBs1.Current as Data.Expense;
            if (expn == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 161 /* Execute Bas_Prod_F */),
                     new Job(SendType.SelfToUserInterface,"BAS_PROD_F",  10 /* Execute Actn_CalF_F */){Input = new XElement("Product", new XAttribute("epitcode", expn.Expense_Type.Expense_Item.CODE), new XAttribute("formstat", "show"))}
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void MbspRwnoPydt_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
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
                        new XAttribute("cbmtcodednrm", pydt.CBMT_CODE_DNRM ?? 0),
                        new XAttribute("exprdate", pydt.EXPR_DATE == null ? "" : pydt.EXPR_DATE.Value.ToString("yyyy-MM-dd")),
                        new XAttribute("mbsprwno", e.NewValue ?? 0)
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

      private void AutoRecalc_Tmr_Tick(object sender, EventArgs e)
      {
         try
         {
            var _pydts = 
               iScsc.Payment_Details
               .Where(
                  pd => 
                     pd.Payment.Request.RQTP_CODE == "016" &&
                     pd.Payment.Request.RQST_STAT == "002" &&
                     pd.EXPR_DATE.Value <= DateTime.Now &&
                     pd.TRAN_DATE == null
               );

            if (_pydts.Count() == 0) return;            

            List<string> evntLogs = new List<string>();

            if(_pydts.Count() > 0)
            {
               if (PlaySondAlrm_Cbx.Checked)
               {
                  if (evntLogs.Count == 0)
                     evntLogs.Add(DateTime.Now.ToString("HH:mm:ss => -----------------------------"));

                  if (evntLogs.Count == 1)
                  {
                     new Thread(AlarmShow).Start();
                  }

                  _pydts.ToList().ForEach(
                     pd =>
                     {
                        evntLogs.Add(string.Format(">> \"{0}\" - [ {1} ] <<", iScsc.GET_STRD_U(new XElement("Request", new XAttribute("type", "001"), new XAttribute("rqid", pd.PYMT_RQST_RQID))), pd.QNTY));
                     }
                  );                  
               }
            }

            iScsc.ExecuteCommand(string.Format("UPDATE Payment_Detail SET TRAN_DATE = GETDATE() WHERE Code IN ({0}) AND TRAN_DATE IS NULL;", string.Join(",", _pydts.Select(pd => pd.CODE))));

            if (evntLogs.Count() > 1)
            {
               foreach (var item in evntLogs)
               {
                  Evnt_Lbx.Items.Insert(0, item);
               }
               
               Gb_ExpenseItem.SelectedTab = tabPage3;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AutoRecalc_Cbx_CheckedChanged(object sender, EventArgs e)
      {
         AutoRecalc_Tmr.Enabled = AutoRecalc_Cbx.Checked;
         AutoRecalc_Tmr.Interval = IntervalRecalc_Txt.Text.ToInt32() * 60000;
      }

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      private void AlarmShow()
      {
         if (InvokeRequired)
         {
            try
            {
               wplayer.URL = @".\Media\SubSys\Kernel\Desktop\Sounds\Popcorn.mp3";
               wplayer.controls.play();
            }
            catch { }

            //var tempcolor = BackGrnd_Butn.NormalColorA;
            //for (int i = 0; i < 5; i++)
            //{
            //   if (i % 2 == 0)
            //      BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.YellowGreen;
            //   else
            //      BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = Color.LimeGreen;

            //   Thread.Sleep(100);
            //}
            //BackGrnd_Butn.NormalColorA = BackGrnd_Butn.NormalColorB = tempcolor;
         }
      }

      private void DelEvntLogs_Butn_Click(object sender, EventArgs e)
      {
         Evnt_Lbx.Items.Clear();
      }
   }
}
