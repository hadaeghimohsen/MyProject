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
   public partial class KSK_INCM_F : UserControl
   {
      public KSK_INCM_F()
      {
         InitializeComponent();
      }

      private bool requery = default(bool);
      private string sextype = "001";
      private bool frstload = false;

      private void Execute_Query()
      {
         setOnDebt = false;
         //if (tb_master.SelectedTab == tp_001)
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            int pydt = PydtsBs1.Position;

            var Rqids = iScsc.VF_Requests(new XElement("Request", new XElement("Fighter", new XAttribute("sextype", sextype), new XAttribute("fgpbtype", "005"))))
               .Where(rqst =>
                     rqst.RQTP_CODE == "016" &&
                     rqst.RQST_STAT == "001" &&
                     rqst.SUB_SYS == 1).Select(r => r.RQID).ToList();

            int rqstindx = RqstBs1.Position;
            RqstBs1.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     Rqids.Contains(rqst.RQID) &&
                     rqst.MDUL_NAME == GetType().Name &&
                     rqst.SECT_NAME == GetType().Name.Substring(0, 3) + "_001_F"
               );
            RqstBs1.Position = rqstindx;


            // 1397/05/15 * بدست آوردن شماره پرونده های درگیر در تمدید
            FighBs1.DataSource =
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

            if (!frstload)
            {
               ExpnBs1.DataSource =
               iScsc.Expenses.Where(ex =>
                  ex.Regulation.REGL_STAT == "002" /* آیین نامه فعال */ && ex.Regulation.TYPE == "001" /* آیین نامه هزینه */ &&
                  ex.Expense_Type.Request_Requester.RQTP_CODE == "016" &&
                  ex.Expense_Type.Request_Requester.RQTT_CODE == "001" &&
                  ex.EXPN_STAT == "002" /* هزینه های فعال */
               );

               Expn_FLP.Controls.Clear();
               ExpnBs1.List.OfType<Data.Expense>().ToList().ForEach(
                  exp =>
                  {
                     var b = new SimpleButton();
                     b.Anchor = System.Windows.Forms.AnchorStyles.Top;
                     b.Appearance.BackColor = System.Drawing.Color.Transparent;
                     b.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
                     b.Appearance.ForeColor = System.Drawing.Color.Black;
                     b.Appearance.Options.UseBackColor = true;
                     b.Appearance.Options.UseFont = true;
                     b.Appearance.Options.UseForeColor = true;
                     b.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                     if (exp.Expense_Type.Expense_Item.IMAG != null)
                     {
                        Byte[] img = ((System.Data.Linq.Binary)exp.Expense_Type.Expense_Item.IMAG).ToArray();

                        MemoryStream ms = new MemoryStream();
                        int offset = 0;
                        ms.Write(img, offset, img.Length - offset);
                        Bitmap bmp = new Bitmap(ms);
                        ms.Close();

                        b.Image = bmp;
                     }
                     else
                     {
                        b.Image = System.Scsc.Properties.Resources.IMAGE_1669;
                     }
                     b.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                     b.Location = new System.Drawing.Point(640, 3);
                     b.LookAndFeel.SkinName = "Office 2010 Blue";
                     b.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
                     b.LookAndFeel.UseDefaultLookAndFeel = false;
                     b.Name = "GropSmpl_Butn";
                     b.Size = new System.Drawing.Size(113, 101);
                     b.TabIndex = 98;
                     b.Tag = exp;
                     b.Text = string.Format("{1:n0}\n\r{0}", exp.EXPN_DESC, exp.PRIC + exp.EXTR_PRCT);

                     b.Click += ExpnButn_Click;

                     Expn_FLP.Controls.Add(b);
                  }
               );               

               Grop_FLP.Controls.Clear();
               var allItems = new SimpleButton();

               allItems.Anchor = System.Windows.Forms.AnchorStyles.Top;
               allItems.Appearance.BackColor = System.Drawing.Color.Transparent;
               allItems.Appearance.Font = new System.Drawing.Font("IRANSans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
               allItems.Appearance.ForeColor = System.Drawing.Color.Black;
               allItems.Appearance.Options.UseBackColor = true;
               allItems.Appearance.Options.UseFont = true;
               allItems.Appearance.Options.UseForeColor = true;
               allItems.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
               allItems.Image = global::System.Scsc.Properties.Resources.IMAGE_1086;
               allItems.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
               allItems.Location = new System.Drawing.Point(640, 3);
               allItems.LookAndFeel.SkinName = "Office 2010 Blue";
               allItems.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
               allItems.LookAndFeel.UseDefaultLookAndFeel = false;
               allItems.Name = "GropSmpl_Butn";
               allItems.Size = new System.Drawing.Size(113, 103);
               allItems.TabIndex = 98;
               allItems.Text = "همه موارد";
               allItems.Tag = 0;

               allItems.Click += GropButn_Click;
               Grop_FLP.Controls.Add(allItems);

               ExpnBs1.List.OfType<Data.Expense>().OrderBy(e => e.GROP_CODE).GroupBy(e => e.Group_Expense).ToList().ForEach(
                  g =>
                  {
                     var b = new SimpleButton();
                     b.Anchor = System.Windows.Forms.AnchorStyles.Top;
                     b.Appearance.BackColor = System.Drawing.Color.Transparent;
                     b.Appearance.Font = new System.Drawing.Font("IRANSans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
                     b.Appearance.ForeColor = System.Drawing.Color.Black;
                     b.Appearance.Options.UseBackColor = true;
                     b.Appearance.Options.UseFont = true;
                     b.Appearance.Options.UseForeColor = true;
                     b.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
                     b.Image = global::System.Scsc.Properties.Resources.IMAGE_1086;
                     b.ImageLocation = DevExpress.XtraEditors.ImageLocation.TopCenter;
                     b.Location = new System.Drawing.Point(640, 3);
                     b.LookAndFeel.SkinName = "Office 2010 Blue";
                     b.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
                     b.LookAndFeel.UseDefaultLookAndFeel = false;
                     b.Name = "GropSmpl_Butn";
                     b.Size = new System.Drawing.Size(113, 103);
                     b.TabIndex = 98;
                     b.Tag = "1";
                     b.Text = "انار شاپ";
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

               frstload = true;
            }

            PydtsBs1.Position = pydt;
         }
         requery = false;
      }

      private void GropButn_Click(object sender, EventArgs e)
      {
         SimpleButton bg = (SimpleButton) sender;
         if (bg.Tag != null)
         {
            var gropcode = Convert.ToInt64( bg.Tag );
            if (gropcode != 0)
               Expn_FLP.Controls.OfType<SimpleButton>().ToList()
                  .ForEach(
                     b =>
                     {
                        var exp = b.Tag as Data.Expense;
                        if (exp.GROP_CODE == gropcode)
                           b.Visible = true;
                        else
                           b.Visible = false;
                     }
                  );
            else
               Expn_FLP.Controls.OfType<SimpleButton>().ToList()
                  .ForEach(
                     b =>
                     {
                        b.Visible = true;
                     }
                  );
         }
         else
            Expn_FLP.Controls.OfType<SimpleButton>().ToList()
               .ForEach(
                  b =>
                  {
                     var exp = b.Tag as Data.Expense;
                     if (exp.GROP_CODE == null)
                        b.Visible = true;
                     else
                        b.Visible = false;
                  }
               );
      }

      private void ExpnButn_Click(object sender, EventArgs e)
      {
         try
         {
            var rqst = RqstBs1.Current as Data.Request;
            // اگر در جدول هزینه قبلا رکوردی درج شده باشد
            if (rqst == null) return;

            SimpleButton b = sender as SimpleButton;
            var expn = b.Tag as Data.Expense;

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
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Btn_RqstBnARqt1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;

            iScsc.OIC_ERQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", Rqst == null ? 0 : Rqst.RQID),
                     new XAttribute("rqtpcode", "016"),
                     new XAttribute("rqttcode", "001"),
                     new XAttribute("rqstrqid", rqstRqid),
                     new XAttribute("mdulname", GetType().Name),
                     new XAttribute("sctnname", GetType().Name.Substring(0, 3) + "_001_F"),
                     new XAttribute("rqstdesc", ""),
                     new XElement("Request_Row",
                        new XAttribute("fileno", fileno),
                        new XElement("Fighter_Public", 
                           new XAttribute("frstname", ""),
                           new XAttribute("lastname", ""),
                           new XAttribute("natlcode", NatlCode_Txt.EditValue ?? ""),
                           new XAttribute("cellphon", CellPhon_Txt.EditValue ?? ""),
                           new XAttribute("suntcode", ""),
                           new XAttribute("servno", "")
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
               Execute_Query();
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
               Execute_Query();
            }
         }
      }

      private void Btn_RqstBnASav1_Click(object sender, EventArgs e)
      {
         try
         {
            Scsc.Data.Request Rqst = RqstBs1.Current as Scsc.Data.Request;
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
               Execute_Query();
            }
         }
      }      

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         var password = Microsoft.VisualBasic.Interaction.InputBox("لطفا رمز عبور خود را وارد کنید", "رمز عبور");

         bool result = false;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "DefaultGateway:DataGuard:Login:Login", 10 /* Execute Actn_Calf_P  */, SendType.SelfToUserInterface)
            {
               Input =
                  new XElement("Data",
                     new XAttribute("type", "comparepassword"),
                     new XAttribute("value", password)
                  ),
               AfterChangedOutput =
                  new Action<object>(
                     (output) =>
                     {
                        result = (bool)output;
                     }
                  )
            }
         );

         if(result)
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
            );
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
               if (MessageBox.Show(this, "عملیات پرداخت به صورت نقدی و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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

      private void ntb_POSPayment1_Click(object sender, EventArgs e)
      {
         try
         {
            //if (tb_master.SelectedTab == tp_001)
            {
               //if (MessageBox.Show(this, "عملیات پرداخت توسط کارتخوان و ذخیره نهایی کردن انجام شود؟", "پرداخت و ذخیره نهایی", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

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
                  // 1397/01/07 * ثبت دستی مبلغ به صورت پایانه فروش
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

      private void RemoveExpn_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //if (MessageBox.Show(this, "آیا با پاک کردن هزینه درخواست موافقید؟", "حذف هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            /* Do Delete Payment_Detail */
            var Crnt = PydtsBs1.Current as Data.Payment_Detail;
            if (Crnt == null) return;
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

      private void PosStng_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 33 /* Execute PosSettings */, SendType.Self) { Input = "Pos_Butn" }
         );
      }

      private void Expn_Gv_DoubleClick(object sender, EventArgs e)
      {
         AddItem_ButtonClick(null, null);
      }
      
      private void FreeSale_Rb_Click(object sender, EventArgs e)
      {
         try
         {
            string sextypebutn = (sender as SimpleButton).Tag.ToString();

            switch (sextypebutn)
            {
               case "Man":
                  sextype = "001";
                  break;
               case "Woman":
                  sextype = "002";
                  break;
            }

            var qury = iScsc.Fighters.Where(f => f.SEX_TYPE_DNRM == sextype && f.FGPB_TYPE_DNRM == "005").ToList();
            FighBs1.DataSource = qury;

            var figh = qury.FirstOrDefault();
            fileno = figh.FILE_NO.ToString();

            if(figh.FIGH_STAT == "001")
            {
               // اگر قفل می باشد
               Execute_Query();
               // اگر ردیف خریدی در جدول وجود داشته باشد
               if(PydtsBs1.List.Count > 0)
                  switch (MessageBox.Show(this, "آیا همین درخواست را ادامه میدهید؟", "فاکتور", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                  {
                     case DialogResult.No:
                        foreach (var pydt in PydtsBs1.List.OfType<Data.Payment_Detail>())
                        {
                           iScsc.DEL_PYDT_P(pydt.CODE, pydt.PYMT_CASH_CODE, pydt.PYMT_RQST_RQID, pydt.RQRO_RWNO, pydt.EXPN_CODE);
                        }
                        requery = true;
                        break;
                  }
            }
            else
            {
               // اگر آزاد می باشد
               RqstBs1.AddNew();
               Btn_RqstBnARqt1_Click(null, null);
            }            
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

      private void PymtsBs1_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var pymt = PymtsBs1.Current as Data.Payment;
            if (pymt == null) { RemnAmnt_Txt.EditValue = 0; return; }

            RemnAmnt_Txt.EditValue = pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
