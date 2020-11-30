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
using System.RoboTech.ExceptionHandlings;
using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.RoboTech.ExtCode;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ONRO_DVLP_F : UserControl
   {
      public ONRO_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long ordrCode = 0;
      private long? chatid = 0;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;
         int prbt = PrbtBs.Position;
         int srbt = SrbtBs.Position;
         int ordr25stp2 = Ordr25Stp2Bs.Position;
         int ordt4stp2 = Ordt4Stp2Bs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));
         RcbaBs.DataSource = iRoboTech.Robot_Card_Bank_Accounts.Where(i => i.ACNT_TYPE == "002" && i.ORDR_TYPE == "004" && i.ACNT_STAT == "002");

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         PrbtBs.Position = prbt;
         SrbtBs.Position = srbt;
         Ordr25Stp2Bs.Position = ordr25stp2;
         Ordt4Stp2Bs.Position = ordt4stp2;

         SearchProduct_Butn_Click(null, null);

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            PrbtBs.DataSource =
               iRoboTech.Personal_Robots.Where(pr => pr.Robot == robo && pr.Personal_Robot_Jobs.Any(prj => prj.Job.ORDR_TYPE == "025" /* مشاغل مربوط به پذیرش سفارش انلاین */ && prj.STAT == "002" /* مشاغل فعال */));

            // TabMenu 1
            Ordr25Stp1Bs.DataSource = iRoboTech.Orders.Where(o => o.Robot == robo && o.ORDR_TYPE == "025" && o.ORDR_STAT == "002");

            // TabMenu 2
            SrbtBs.DataSource = iRoboTech.Service_Robots.Where(sr => sr.Robot == robo && sr.STAT == "002" && iRoboTech.Orders.Any(o => o.Robot == robo && o.ORDR_TYPE == "025" && o.ORDR_STAT == "016" && o.CHAT_ID == sr.CHAT_ID));
            SrbtsBs.DataSource = iRoboTech.Service_Robots.Where(sr => sr.Robot == robo);
            if (SrbtBs.List.Count == 0)
            {
               Ordr25Stp2Bs.List.Clear();
               Ordr4Stp2Bs.List.Clear();
               SrbtCredWletAmnt_Butn.Text = "موجودی اعتباری : 0";
               SrbtCashWletAmnt_Butn.Text = "موجودی نقدی : 0";
               //Ordt4Stp2Bs.List.Clear();
               //Odst4Stp2Bs.List.Clear();
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SavePersonelInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Prbt_gv.PostEditor();
            PrbtBs.EndEdit();

            iRoboTech.SubmitChanges();

            requery = true;
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
            }
         }
      }

      private void ShowCart_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as C1.Win.C1Input.C1Button;
            if (butn.Tag.ToString() == "Stp1")
            {
               var ordr25 = Ordr25Stp1Bs.Current as Data.Order;
               if (ordr25 == null) return;

               var prbt = PrbtBs.Current as Data.Personal_Robot;
               if (prbt == null) return;

               // فراخوانی ربات برای ارسال پیام ثبت شده به سفیران انتخاب شده
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("Robot", 
                                 new XAttribute("runrobot", "start"),
                                 new XAttribute("actntype", "showcart"),
                                 new XAttribute("rbid", ordr25.SRBT_ROBO_RBID),
                                 new XAttribute("ordrcode", ordr25.CODE),
                                 new XAttribute("ordrtype", ordr25.ORDR_TYPE),
                                 new XAttribute("ordtrwno", "*"),
                                 new XAttribute("chatid", prbt.CHAT_ID)
                              )
                        }
                     }
                  )
               );
            }
            else if (butn.Tag.ToString() == "Stp2")
            {
               var ordr25 = Ordr25Stp2Bs.Current as Data.Order;
               if (ordr25 == null) return;

               var prbt = PrbtBs.Current as Data.Personal_Robot;
               if (prbt == null) return;

               // فراخوانی ربات برای ارسال پیام ثبت شده به سفیران انتخاب شده
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("Robot", 
                                 new XAttribute("runrobot", "start"),
                                 new XAttribute("actntype", "showcart"),
                                 new XAttribute("rbid", ordr25.SRBT_ROBO_RBID),
                                 new XAttribute("ordrcode", ordr25.CODE),
                                 new XAttribute("ordrtype", ordr25.ORDR_TYPE),
                                 new XAttribute("ordtrwno", "*"),
                                 new XAttribute("chatid", prbt.CHAT_ID)
                              )
                        }
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

      private void ShowSlctItemCart_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr25 = Ordr25Stp1Bs.Current as Data.Order;
            if (ordr25 == null) return;

            var prbt = PrbtBs.Current as Data.Personal_Robot;
            if (prbt == null) return;

            var ordt25 = Ordt25Stp1Bs.Current as Data.Order_Detail;
            if (ordt25 == null) return;

            // فراخوانی ربات برای ارسال پیام ثبت شده به سفیران انتخاب شده
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                     new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                     {
                        Input = 
                           new XElement("Robot", 
                              new XAttribute("runrobot", "start"),
                              new XAttribute("actntype", "showcart"),
                              new XAttribute("rbid", ordr25.SRBT_ROBO_RBID),
                              new XAttribute("ordrcode", ordr25.CODE),
                              new XAttribute("ordrtype", ordr25.ORDR_TYPE),
                              new XAttribute("ordtrwno", ordt25.RWNO),
                              new XAttribute("chatid", prbt.CHAT_ID)
                           )
                     }
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SrbtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            Ordr25Stp2Bs.DataSource = iRoboTech.Orders.Where(o => o.Service_Robot == srbt && o.ORDR_TYPE == "025" && o.ORDR_STAT == "016");
            SrbpBs.DataSource = iRoboTech.Service_Robot_Publics.Where(p => p.Service_Robot == srbt && p.VALD_TYPE == "002");

            //WletBs.DataSource = iRoboTech.Wallets.Where(w => w.Service_Robot == srbt);

            //if (WletBs.List.Count == 2)
            //{
            //   SrbtCredWletAmnt_Butn.Text = string.Format("موجود اعتباری : " + "{0:n0}", WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM);
            //   SrbtCredWletAmnt_Butn.Tag = WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM;
            //   SrbtCashWletAmnt_Butn.Text = string.Format("موجود نقدی : " + "{0:n0}", WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM);
            //   SrbtCashWletAmnt_Butn.Tag = WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM;
            //}
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Ordr25Stp2Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr25Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            int ordt4 = Ordt4Stp2Bs.Position;

            Ordr4Stp2Bs.DataSource = iRoboTech.Orders.Where(o => o.ORDR_CODE == ordr.CODE && o.ORDR_TYPE == "004" && o.ORDR_STAT != "003");

            Ordt4Stp2Bs.Position = ordt4;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ReloadForm_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void AprvOrdr25_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr25Stp1Bs.Current as Data.Order;
            if (ordr == null) return;

            if (!ordr.Orders.Any(o => o.ORDR_TYPE == "004" && o.ORDR_STAT != "003"))
            {
               iRoboTech.INS_ORDR_P(ordr.SRBT_SERV_FILE_NO, ordr.SRBT_ROBO_RBID, ordr.SRBT_SRPB_RWNO, null, null, ordr.CHAT_ID, ordr.CODE, null, null, ordr.OWNR_NAME, "004", DateTime.Now, null, "001", ordr.CORD_X, ordr.CORD_Y, ordr.CELL_PHON, ordr.TELL_PHON, ordr.SERV_ADRS, ordr.ARCH_STAT, null, null, null, null, null, null, null, 5);
               
               ordrCode = ordr.CODE;
               chatid = ordr.CHAT_ID;

               iRoboTech.UPD_ORDR_P(
                  ordr.CODE, ordr.SRBT_SERV_FILE_NO, ordr.SRBT_ROBO_RBID, ordr.SRBT_SRPB_RWNO, ordr.PROB_SERV_FILE_NO, ordr.PROB_ROBO_RBID, ordr.CHAT_ID,
                  ordr.ORDR_CODE, ordr.ORDR_NUMB, ordr.SERV_ORDR_RWNO, ordr.OWNR_NAME, ordr.ORDR_TYPE, ordr.STRT_DATE, ordr.END_DATE, "016", ordr.HOW_SHIP,
                  ordr.CORD_X, ordr.CORD_Y, ordr.CELL_PHON, ordr.TELL_PHON, ordr.SERV_ADRS, ordr.ARCH_STAT, ordr.SERV_JOB_APBS_CODE, ordr.SERV_INTR_APBS_CODE, ordr.MDFR_STAT,
                  ordr.CRTB_SEND_STAT, ordr.CRTB_MAIL_NO, ordr.CRTB_MAIL_SUBJ, ordr.APBS_CODE, ordr.EXPN_AMNT, ordr.EXTR_PRCT, ordr.SORC_CORD_X, ordr.SORC_CORD_Y, ordr.SORC_POST_ADRS,
                  ordr.SORC_CELL_PHON, ordr.SORC_TELL_PHON, ordr.SORC_EMAL_ADRS, ordr.SORC_WEB_SITE, ordr.SUB_SYS, ordr.ORDR_DESC
               );

               requery = true;
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
               Main_Tc.SelectedTab = Ordr4Stp2_Tp;
               SrbtBs.Position = SrbtBs.IndexOf(SrbtBs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => sr.CHAT_ID == chatid));
               Ordr25Stp2Bs.Position = Ordr25Stp2Bs.IndexOf(Ordr25Stp2Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CHAT_ID == chatid && o.CODE == ordrCode));
            }
         }
      }

      private void OrdrSaveChanges_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            iRoboTech.UPD_ORDR_P(
               ordr.CODE, ordr.SRBT_SERV_FILE_NO, ordr.SRBT_ROBO_RBID, ordr.SRBT_SRPB_RWNO, ordr.PROB_SERV_FILE_NO, ordr.PROB_ROBO_RBID, ordr.CHAT_ID,
               ordr.ORDR_CODE, ordr.ORDR_NUMB, ordr.SERV_ORDR_RWNO, ordr.OWNR_NAME, ordr.ORDR_TYPE, DateTime.Now, ordr.END_DATE, ordr.ORDR_STAT, ordr.HOW_SHIP,
               ordr.CORD_X, ordr.CORD_Y, ordr.CELL_PHON, ordr.TELL_PHON, ordr.SERV_ADRS, ordr.ARCH_STAT, ordr.SERV_JOB_APBS_CODE, ordr.SERV_INTR_APBS_CODE, ordr.MDFR_STAT,
               ordr.CRTB_SEND_STAT, ordr.CRTB_MAIL_NO, ordr.CRTB_MAIL_SUBJ, ordr.APBS_CODE, ordr.EXPN_AMNT, ordr.EXTR_PRCT, ordr.SORC_CORD_X, ordr.SORC_CORD_Y, ordr.SORC_POST_ADRS, 
               ordr.SORC_CELL_PHON, ordr.SORC_TELL_PHON, ordr.SORC_EMAL_ADRS, ordr.SORC_WEB_SITE, ordr.SUB_SYS, ordr.ORDR_DESC
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

      private void Rcba_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            Rcba_Lov.Properties.PopupFormMinSize = Rcba_Lov.Size;

            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            if(e.NewValue != null)
            {
               var rcba = RcbaBs.List.OfType<Data.Robot_Card_Bank_Account>().FirstOrDefault(i => i.CODE == (long)e.NewValue);
               ordr.DEST_CARD_NUMB_DNRM = rcba.CARD_NUMB;
               BankAcntName_Txt.Text = rcba.ACNT_OWNR;
               BankAcntShba_Txt.Text = rcba.SHBA_NUMB_DNRM;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ServAdrs_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            ServAdrs_Lov.Properties.PopupFormMinSize = ServAdrs_Lov.Size;

            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            if (e.NewValue != null)
            {
               var adrs = SrbpBs.List.OfType<Data.Service_Robot_Public>().FirstOrDefault(i => i.RWNO == (int)e.NewValue);
               ordr.SERV_ADRS = adrs.SERV_ADRS;
               ordr.CORD_X = adrs.CORD_X;
               ordr.CORD_Y = adrs.CORD_Y;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void Ordr4Stp2Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            var rcba = RcbaBs.List.OfType<Data.Robot_Card_Bank_Account>().FirstOrDefault(i => i.CARD_NUMB == ordr.DEST_CARD_NUMB_DNRM);
            BankAcntName_Txt.Text = rcba.ACNT_OWNR;
            BankAcntShba_Txt.Text = rcba.SHBA_NUMB_DNRM;

            if (ordr.HOW_SHIP == "001")
               ServAdrs_Pn.Visible = false;
            else
               ServAdrs_Pn.Visible = true;

            WletBs.DataSource = iRoboTech.Wallets.Where(w => w.Service_Robot == ordr.Service_Robot);
            if (WletBs.List.Count == 2)
            {
               SrbtCredWletAmnt_Butn.Text = string.Format("موجودی اعتباری : " + "{0:n0}", WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM);
               SrbtCredWletAmnt_Butn.Tag = WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM;
               SrbtCashWletAmnt_Butn.Text = string.Format("موجودی نقدی : " + "{0:n0}", WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM);
               SrbtCashWletAmnt_Butn.Tag = WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM;
            }

            SendOrdrToPay_Butn.Enabled = CnclOrdr_Butn.Enabled = OrdrCashPay_Butn.Enabled = OrdrPosPay_Butn.Enabled = false;
            if (ordr.SUM_EXPN_AMNT_DNRM > 0)
               SendOrdrToPay_Butn.Enabled = CnclOrdr_Butn.Enabled = OrdrCashPay_Butn.Enabled = OrdrPosPay_Butn.Enabled = true;

            OrdrWletCredPay_Butn.Visible = false;
            // Show Credit Wallet Button
            if (SrbtCredWletAmnt_Butn.Tag != null)
            {
               if (ordr.SUM_EXPN_AMNT_DNRM <= SrbtCredWletAmnt_Butn.Tag.ToString().ToInt64())
                  OrdrWletCredPay_Butn.Visible = true;
            }

            OrdrWletCashPay_Butn.Visible = false;
            // Show Cash Wallet Button
            if (SrbtCashWletAmnt_Butn.Tag != null)
            {
               if (ordr.SUM_EXPN_AMNT_DNRM <= (long)SrbtCashWletAmnt_Butn.Tag.ToString().ToInt64())
                  OrdrWletCashPay_Butn.Visible = true;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SearchProduct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            var whereClause = string.Format("WHERE p.Robo_Rbid = {0} AND p.Stat = '002' ", robo.RBID);
            if(Brnd_Chkb.Checked)
            {
               if(VBexpBs.List.OfType<Data.V_Group_Expense>().Any(b => b.CODE == (long)Brnd_Lov.EditValue))
                  whereClause += string.Format("AND p.Brnd_Code_Dnrm = {0} ", Brnd_Lov.EditValue);
            }

            if(Grop_Chkb.Checked)
            {
               if(VGexpBs.List.OfType<Data.V_Group_Expense>().Any(g => g.CODE == (long)Grop_Lov.EditValue))
                  whereClause += string.Format("AND iScsc.dbo.LINK_GROP_U(p.Grop_Code_Dnrm, {0}) = 1 ", Grop_Lov.EditValue);
            }

            if(TarfCode_Chkb.Checked)
            {
               whereClause += string.Format("AND p.Tarf_Code LIKE '%{0}%' ", TarfCode_Txt.Text);
            }

            if(ProdInvr_Chkb.Checked)
            {
               whereClause += "AND (p.Crnt_Numb_Dnrm >= 1 OR p.Sale_Cart_Numb_Dnrm >= 1) ";
            }

            if(TarfName_Chkb.Checked)
            {
               whereClause += string.Format("AND ( p.Tarf_Text_Dnrm LIKE N'%{0}%' OR p.Tarf_Engl_Text LIKE N'%{0}%' OR p.Prod_Fetr LIKE N'%{0}%') ", TarfName_Txt.Text);
            }

            RbprBs.DataSource =
               iRoboTech.ExecuteQuery<Data.Robot_Product>(
                  "SELECT * " + 
                  "FROM dbo.Robot_Product p " + 
                  whereClause,
                  DateTime.Now
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddTarfTToCart_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            var textInput = prod.TARF_CODE;
            // IF NOT EXISTS PRODUCT IN CART
            if(!Ordt4Stp2Bs.List.OfType<Data.Order_Detail>().Any(od => od.TARF_CODE == prod.TARF_CODE))
            {
               textInput += "*n" + TarfCount_Txt.Text;
            }
            // ELSE IF EXISTS PRODUCT IN CART
            else
            {
               textInput += "*+=" + TarfCount_Txt.Text;
            }

            var xResult = new XElement("Result");

            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message", 
                      new XAttribute("ussd", "*0*2#"),
                      new XAttribute("childussd", ""),
                      new XAttribute("chatid", ordr.CHAT_ID),
                      new XAttribute("elmntype", "001"),
                      new XElement("Text", 
                          new XAttribute("param", ordr.CODE),
                         textInput
                      )
                  )
               ),
               ref xResult
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
            {
               Execute_Query();
               //SearchProduct_Butn_Click(null, null);
            }
         }
      }

      private void OrdtActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            Ordt4Stp2_Gv.PostEditor();

            var ordt = Ordt4Stp2Bs.Current as Data.Order_Detail;
            if (ordt == null) return;

            var xResult = new XElement("Result");

            switch (e.Button.Index)
            {
               case 0:
                  // -- n                  
                  iRoboTech.Analisis_Message_P(
                     new XElement("Robot",
                        new XAttribute("token", ordt.Order.Robot.TKON_CODE),
                        new XElement("Message",
                            new XAttribute("ussd", "*0*2#"),
                            new XAttribute("childussd", ""),
                            new XAttribute("chatid", ordt.Order.CHAT_ID),
                            new XAttribute("elmntype", "001"),
                            new XElement("Text", 
                                new XAttribute("param", ordt.ORDR_CODE),
                                (ordt.NUMB == 1 ? ordt.TARF_CODE + "*del" : ordt.TARF_CODE + "*--")
                            )
                        )
                     ),
                     ref xResult
                  );
                  break;
               case 1:
                  // delete
                  iRoboTech.Analisis_Message_P(
                     new XElement("Robot",
                        new XAttribute("token", ordt.Order.Robot.TKON_CODE),
                        new XElement("Message",
                            new XAttribute("ussd", "*0*2#"),
                            new XAttribute("childussd", ""),
                            new XAttribute("chatid", ordt.Order.CHAT_ID),
                            new XAttribute("elmntype", "001"),
                            new XElement("Text",
                                new XAttribute("param", ordt.ORDR_CODE),
                                ordt.TARF_CODE + "*del"
                            )
                        )
                     ),
                     ref xResult
                  );
                  break;
               case 2:
                  // ++ n
                  iRoboTech.Analisis_Message_P(
                     new XElement("Robot",
                        new XAttribute("token", ordt.Order.Robot.TKON_CODE),
                        new XElement("Message",
                            new XAttribute("ussd", "*0*2#"),
                            new XAttribute("childussd", ""),
                            new XAttribute("chatid", ordt.Order.CHAT_ID),
                            new XAttribute("elmntype", "001"),
                            new XElement("Text",
                                new XAttribute("param", ordt.ORDR_CODE),
                                ordt.TARF_CODE + "*++"
                            )
                        )
                     ),
                     ref xResult
                  );
                  break;
               case 3:
                  // save n
                  iRoboTech.Analisis_Message_P(
                     new XElement("Robot",
                        new XAttribute("token", ordt.Order.Robot.TKON_CODE),
                        new XElement("Message",
                            new XAttribute("ussd", "*0*2#"),
                            new XAttribute("childussd", ""),
                            new XAttribute("chatid", ordt.Order.CHAT_ID),
                            new XAttribute("elmntype", "001"),
                            new XElement("Text",
                                new XAttribute("param", ordt.ORDR_CODE),
                                ordt.TARF_CODE + "*n" + ordt.NUMB.ToString()
                            )
                        )
                     ),
                     ref xResult
                  );
                  break;
               default:
                  break;
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
            {
               Execute_Query();
               //SearchProduct_Butn_Click(null, null);
            }
         }
      }

      private void CnclOrdr25_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var butn = sender as C1.Win.C1Input.C1Button;
            
            if(butn.Tag.ToString() == "Stp1")
            {
               var ordr25 = Ordr25Stp1Bs.Current as Data.Order;
               if (ordr25 == null) return;

               iRoboTech.ExecuteCommand("DELETE dbo.[Order] WHERE Code = {0};", ordr25.CODE);
            }
            else if (butn.Tag.ToString() == "Stp2")
            {
               var ordr25 = Ordr25Stp2Bs.Current as Data.Order;
               if (ordr25 == null) return;

               // اگر سفارش دارای اقلام کالا باشد
               if(ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001").Order_Details.Any())
               {
                  var ordr = ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001");
                  
                  var xResult = new XElement("Respons");
                  iRoboTech.Analisis_Message_P(
                     new XElement("Robot",
                        new XAttribute("token", ordr.Robot.TKON_CODE),
                        new XElement("Message",
                            new XAttribute("ussd", "*0*2#"),
                            new XAttribute("childussd", ""),
                            new XAttribute("chatid", ordr.CHAT_ID),
                            new XAttribute("elmntype", "001"),
                            new XElement("Text",
                                new XAttribute("param", ordr.CODE),
                                "empty"
                            )
                        )
                     ),
                     ref xResult
                  );
               }

               // حذف کردن درخواست سفارش
               iRoboTech.ExecuteCommand(string.Format("DELETE dbo.[Order] WHERE Ordr_Code = {0};", ordr25.CODE));

               // حذف کردن درخواست پذیرش سفارش انلاین
               iRoboTech.ExecuteCommand("DELETE dbo.[Order] WHERE Code = {0};", ordr25.CODE);
            }

            //iRoboTech.SubmitChanges();
            requery = true;
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
               //SearchProduct_Butn_Click(null, null);
            }
         }
      }

      private void CnclOrdr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            var xResult = new XElement("Respons");
            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("ussd", "*0*2#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", ordr.CODE),
                            "empty"
                        )
                  )
               ),
               ref xResult
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
            {
               Execute_Query();
               //SearchProduct_Butn_Click(null, null);
            }
         }
      }

      private void SendOrdrToPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            iRoboTech.UPD_ORDR_P(
                  ordr.CODE, ordr.SRBT_SERV_FILE_NO, ordr.SRBT_ROBO_RBID, ordr.SRBT_SRPB_RWNO, ordr.PROB_SERV_FILE_NO, ordr.PROB_ROBO_RBID, ordr.CHAT_ID,
                  ordr.ORDR_CODE, ordr.ORDR_NUMB, ordr.SERV_ORDR_RWNO, ordr.OWNR_NAME, ordr.ORDR_TYPE, DateTime.Now, ordr.END_DATE, ordr.ORDR_STAT, ordr.HOW_SHIP,
                  ordr.CORD_X, ordr.CORD_Y, ordr.CELL_PHON, ordr.TELL_PHON, ordr.SERV_ADRS, ordr.ARCH_STAT, ordr.SERV_JOB_APBS_CODE, ordr.SERV_INTR_APBS_CODE, ordr.MDFR_STAT,
                  ordr.CRTB_SEND_STAT, ordr.CRTB_MAIL_NO, ordr.CRTB_MAIL_SUBJ, ordr.APBS_CODE, ordr.EXPN_AMNT, ordr.EXTR_PRCT, ordr.SORC_CORD_X, ordr.SORC_CORD_Y, ordr.SORC_POST_ADRS,
                  ordr.SORC_CELL_PHON, ordr.SORC_TELL_PHON, ordr.SORC_EMAL_ADRS, ordr.SORC_WEB_SITE, ordr.SUB_SYS, ordr.ORDR_DESC
               );

            // ارسال فاکتور برای مشتری
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("Robot", 
                                 new XAttribute("runrobot", "start"),
                                 new XAttribute("actntype", "sendinvoice"),
                                 new XAttribute("rbid", ordr.SRBT_ROBO_RBID),
                                 new XAttribute("ordrcode", ordr.CODE),
                                 new XAttribute("chatid", ordr.CHAT_ID)
                              )
                        }
                     }
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         
      }

      private void OrdrCashPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            if (MessageBox.Show(this, "تسویه حساب مالی به صورت نقدی", "انجام عملیات تسویه حساب نقدی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Do Some things
            var xResult = new XElement("Respons");
            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("cbq", "002"),
                        new XAttribute("ussd", "*0*3*3#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", string.Format("howinccashwlet,{0}", ordr.AMNT_TYPE == "001" ? ordr.DEBT_DNRM : ordr.DEBT_DNRM * 10)),
                            new XAttribute("postexec", "lessaddwlet"),
                            "addamntwlet"
                        )
                  )
               ),
               ref xResult
            );

            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

            var ordr15 = iRoboTech.Orders.Where(o => o.Service_Robot == ordr.Service_Robot && o.ORDR_TYPE == "015" && o.ORDR_STAT == "001" && o.DEBT_DNRM == ordr.DEBT_DNRM).FirstOrDefault();
            if (ordr15 == null) { MessageBox.Show(this, "متاسفانه در ثبت مبلغ نقدی خطایی پیش آمده لطفا دوباره امتحان کنید"); return; }

            iRoboTech.SAVE_PYMT_P(
               new XElement("Payment", 
                   new XAttribute("ordrcode", ordr15.CODE),
                   new XAttribute("txid", ordr15.CODE), 
                   new XAttribute("totlamnt", ordr15.DEBT_DNRM),
                   new XAttribute("autochngamnt", "001"),
                   new XAttribute("rcptmtod", "001")
               ),
               ref xResult
            );

            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
            ordr15 = iRoboTech.Orders.FirstOrDefault(o => o.CODE == ordr15.CODE);
            if (ordr15.ORDR_STAT != "004") { MessageBox.Show(this, "متاسفانه در ثبت مبلغ نقدی خطایی پیش آمده لطفا دوباره امتحان کنید"); return; }

            iRoboTech.SAVE_WLET_P(
               new XElement("Wallet_Detail",
                   new XAttribute("ordrcode", ordr.CODE),
                   new XAttribute("rbid", ordr.Robot.RBID),
                   new XAttribute("chatid", ordr.CHAT_ID),
                   new XAttribute("oprttype", "add"),
                   new XAttribute("wlettype", "002")
               ),
               ref xResult
            );

            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("cbq", "002"),
                        new XAttribute("ussd", "*0#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", ordr.CODE),
                            new XAttribute("postexec", "lessfinlcart"),
                            "finalcart"
                        )
                  )
               ),
               ref xResult
            );

            // ارسال پیام تشکر از خرید مشتری
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                     {
                        new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 10 /* Execute Actn_CalF_P */)
                        {
                           Input = 
                              new XElement("Robot", 
                                 new XAttribute("runrobot", "start"),
                                 new XAttribute("actntype", "sendmesg"),
                                 new XAttribute("rbid", ordr.SRBT_ROBO_RBID),
                                 new XAttribute("ordrcode", ordr.CODE),
                                 new XAttribute("chatid", ordr.CHAT_ID),
                                 xResult
                              )
                        }
                     }
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

      private void OrdrPosPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            if (MessageBox.Show(this, "تسویه حساب مالی به صورت کارتخوان", "انجام عملیات تسویه حساب کارتخوان", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Do Some things
            var xResult = new XElement("Respons");
            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("cbq", "002"),
                        new XAttribute("ussd", "*0*3*3#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", string.Format("howinccashwlet,{0}", ordr.AMNT_TYPE == "001" ? ordr.DEBT_DNRM : ordr.DEBT_DNRM * 10)),
                            new XAttribute("postexec", "lessaddwlet"),
                            "addamntwlet"
                        )
                  )
               ),
               ref xResult
            );

            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

            var ordr15 = iRoboTech.Orders.Where(o => o.Service_Robot == ordr.Service_Robot && o.ORDR_TYPE == "015" && o.ORDR_STAT == "001" && o.DEBT_DNRM == ordr.DEBT_DNRM).FirstOrDefault();
            if (ordr15 == null) { MessageBox.Show(this, "متاسفانه در ثبت مبلغ کارتخوان خطایی پیش آمده لطفا دوباره امتحان کنید"); return; }

            iRoboTech.SAVE_PYMT_P(
               new XElement("Payment",
                   new XAttribute("ordrcode", ordr15.CODE),
                   new XAttribute("txid", ordr15.CODE),
                   new XAttribute("totlamnt", ordr15.DEBT_DNRM),
                   new XAttribute("autochngamnt", "001"),
                   new XAttribute("rcptmtod", "003")
               ),
               ref xResult
            );

            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
            ordr15 = iRoboTech.Orders.FirstOrDefault(o => o.CODE == ordr15.CODE);
            if (ordr15.ORDR_STAT != "004") { MessageBox.Show(this, "متاسفانه در ثبت مبلغ نقدی خطایی پیش آمده لطفا دوباره امتحان کنید"); return; }

            iRoboTech.SAVE_WLET_P(
               new XElement("Wallet_Detail",
                   new XAttribute("ordrcode", ordr.CODE),
                   new XAttribute("rbid", ordr.Robot.RBID),
                   new XAttribute("chatid", ordr.CHAT_ID),
                   new XAttribute("oprttype", "add"),
                   new XAttribute("wlettype", "002")
               ),
               ref xResult
            );

            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("cbq", "002"),
                        new XAttribute("ussd", "*0#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", ordr.CODE),
                            new XAttribute("postexec", "lessfinlcart"),
                            "finalcart"
                        )
                  )
               ),
               ref xResult
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

      private void OrdrWletCredPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;
            
            if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            if (MessageBox.Show(this, "تسویه حساب مالی از کیف پول اعتباری", "انجام عملیات تسویه حساب کیف پول اعتباری", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Do Some things
            var xResult = new XElement("Respons");

            iRoboTech.SAVE_WLET_P(
               new XElement("Wallet_Detail",
                   new XAttribute("ordrcode", ordr.CODE),
                   new XAttribute("rbid", ordr.Robot.RBID),
                   new XAttribute("chatid", ordr.CHAT_ID),
                   new XAttribute("oprttype", "add"),
                   new XAttribute("wlettype", "001")
               ),
               ref xResult
            );

            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("cbq", "002"),
                        new XAttribute("ussd", "*0#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", ordr.CODE),
                            new XAttribute("postexec", "lessfinlcart"),
                            "finalcart"
                        )
                  )
               ),
               ref xResult
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

      private void OrdrWletCashPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            if (MessageBox.Show(this, "تسویه حساب مالی از کیف پول نقدی", "انجام عملیات تسویه حساب کیف پول نقدی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Do Some things
            var xResult = new XElement("Respons");

            iRoboTech.SAVE_WLET_P(
               new XElement("Wallet_Detail",
                   new XAttribute("ordrcode", ordr.CODE),
                   new XAttribute("rbid", ordr.Robot.RBID),
                   new XAttribute("chatid", ordr.CHAT_ID),
                   new XAttribute("oprttype", "add"),
                   new XAttribute("wlettype", "002")
               ),
               ref xResult
            );

            iRoboTech.Analisis_Message_P(
               new XElement("Robot",
                  new XAttribute("token", ordr.Robot.TKON_CODE),
                  new XElement("Message",
                        new XAttribute("cbq", "002"),
                        new XAttribute("ussd", "*0#"),
                        new XAttribute("childussd", ""),
                        new XAttribute("chatid", ordr.CHAT_ID),
                        new XAttribute("elmntype", "001"),
                        new XElement("Text",
                            new XAttribute("param", ordr.CODE),
                            new XAttribute("postexec", "lessfinlcart"),
                            "finalcart"
                        )
                  )
               ),
               ref xResult
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

      private void Srbt_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            //Srbt_Lov.Properties.PopupFormMinSize = Srbt_Lov.Size;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void CretOrdr25_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (Srbt_Lov.EditValue == null || Srbt_Lov.EditValue.ToString() == "") return;

            var srbt = SrbtsBs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => sr.Robot == robo && sr.CHAT_ID == (long)Srbt_Lov.EditValue);
            if (srbt == null) return;

            iRoboTech.INS_ORDR_P(srbt.SERV_FILE_NO, srbt.ROBO_RBID, srbt.SRPB_RWNO, null, null, srbt.CHAT_ID, null, null, null, srbt.NAME, "025", DateTime.Now, null, "001", null, null, srbt.CELL_PHON, null, srbt.SERV_ADRS, null, null, null, null, null, null, null, null, 12);

            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

            var ordr25 =
               iRoboTech.Orders.Where(o => o.Service_Robot == srbt && o.ORDR_TYPE == "025" && o.ORDR_STAT == "001" && !o.Order_Details.Any()).OrderByDescending(o => o.STRT_DATE).FirstOrDefault();

            if (ordr25 == null) { MessageBox.Show("در ثبت اطلاعات درخواست سفارش مشکلی پیش آمده لطفا بررسی کنید"); return; }

            iRoboTech.INS_ORDR_P(ordr25.SRBT_SERV_FILE_NO, ordr25.SRBT_ROBO_RBID, ordr25.SRBT_SRPB_RWNO, null, null, ordr25.CHAT_ID, ordr25.CODE, null, null, ordr25.OWNR_NAME, "004", DateTime.Now, null, "001", ordr25.CORD_X, ordr25.CORD_Y, ordr25.CELL_PHON, ordr25.TELL_PHON, ordr25.SERV_ADRS, ordr25.ARCH_STAT, null, null, null, null, null, null, null, 5);

            ordrCode = ordr25.CODE;
            chatid = ordr25.CHAT_ID;

            iRoboTech.UPD_ORDR_P(
               ordr25.CODE, ordr25.SRBT_SERV_FILE_NO, ordr25.SRBT_ROBO_RBID, ordr25.SRBT_SRPB_RWNO, ordr25.PROB_SERV_FILE_NO, ordr25.PROB_ROBO_RBID, ordr25.CHAT_ID,
               ordr25.ORDR_CODE, ordr25.ORDR_NUMB, ordr25.SERV_ORDR_RWNO, ordr25.OWNR_NAME, ordr25.ORDR_TYPE, ordr25.STRT_DATE, ordr25.END_DATE, "016", ordr25.HOW_SHIP,
               ordr25.CORD_X, ordr25.CORD_Y, ordr25.CELL_PHON, ordr25.TELL_PHON, ordr25.SERV_ADRS, ordr25.ARCH_STAT, ordr25.SERV_JOB_APBS_CODE, ordr25.SERV_INTR_APBS_CODE, ordr25.MDFR_STAT,
               ordr25.CRTB_SEND_STAT, ordr25.CRTB_MAIL_NO, ordr25.CRTB_MAIL_SUBJ, ordr25.APBS_CODE, ordr25.EXPN_AMNT, ordr25.EXTR_PRCT, ordr25.SORC_CORD_X, ordr25.SORC_CORD_Y, ordr25.SORC_POST_ADRS,
               ordr25.SORC_CELL_PHON, ordr25.SORC_TELL_PHON, ordr25.SORC_EMAL_ADRS, ordr25.SORC_WEB_SITE, ordr25.SUB_SYS, ordr25.ORDR_DESC
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
            {             
               Execute_Query();
               //Main_Tc.SelectedTab = Ordr4Stp2_Tp;
               SrbtBs.Position = SrbtBs.IndexOf(SrbtBs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => sr.CHAT_ID == chatid));
               Ordr25Stp2Bs.Position = Ordr25Stp2Bs.IndexOf(Ordr25Stp2Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CHAT_ID == chatid && o.CODE == ordrCode));
            }
         }
      }

      private void SaveAdrs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            OrdrSaveChanges_Butn_Click(null, null);

            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            var xResult = new XElement("Respons");

            iRoboTech.SAVE_SRBT_P(
               new XElement("Service",
                   new XAttribute("rbid", ordr.Robot.RBID),
                   new XAttribute("chatid", ordr.CHAT_ID), 
                   new XAttribute("actntype", "003"),
                   new XAttribute("postadrs", ordr.SERV_ADRS),
                   new XAttribute("cordx", ordr.CORD_X),
                   new XAttribute("cordy", ordr.CORD_Y)
               ),
               ref xResult
            );

            if (xResult.Elements("Message").FirstOrDefault().Attribute("rsltcode").Value == "002")
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

      private void ShowDestGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Stp2Bs.Current as Data.Order;
            if (ordr == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
               {
                  Input =
                     new XElement("GMapNets",
                        new XAttribute("requesttype", "get"),
                        new XAttribute("formcaller", "Program:RoboTech:" + GetType().Name),
                        new XAttribute("callback", 40 /* CordinateGetSet */),
                        new XAttribute("outputtype", "destpostadrs"),
                        new XAttribute("initalset", true),
                        new XAttribute("cordx", ordr.CORD_X == null ? "29.610420210528" : ordr.CORD_X.ToString()),
                        new XAttribute("cordy", ordr.CORD_Y == null ? "52.5152599811554" : ordr.CORD_Y.ToString()),
                        new XAttribute("zoom", "1600")
                     )
               }
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SearchOrderReport_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            FromDate3_Dt.CommitChanges();
            ToDate3_Dt.CommitChanges();

            if (!FromDate3_Dt.Value.HasValue) FromDate3_Dt.Value = DateTime.Now;
            if (!ToDate3_Dt.Value.HasValue) ToDate3_Dt.Value = DateTime.Now;

            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            SrbtStp3Bs.DataSource = 
               iRoboTech.Service_Robots.Where(sr => 
                  sr.Robot == robo && 
                  sr.Orders.Any(o => 
                     o.ORDR_TYPE == "004" && 
                     o.ORDR_STAT != "003" && 
                     o.STRT_DATE.Value.Date >= FromDate3_Dt.Value.Value.Date && 
                     o.STRT_DATE.Value.Date <= ToDate3_Dt.Value.Value.Date
                  )
               );

            var ordr =
               iRoboTech.Orders.Where(o => 
                  o.Robot == robo &&
                  o.ORDR_TYPE == "004" &&
                  o.ORDR_STAT != "003" &&
                  o.STRT_DATE.Value.Date >= FromDate3_Dt.Value.Value.Date &&
                  o.STRT_DATE.Value.Date <= ToDate3_Dt.Value.Value.Date
               );

            OrdrAllCount3_Txt.EditValue = ordr.Count();
            OrdrCloseCount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Count();
            OrdrOpenCount3_Txt.EditValue = ordr.Where(o => !(o.ORDR_STAT == "004" || o.ORDR_STAT == "009")).Count();
            OrdrAllAmount3_Txt.EditValue = ordr.Sum(o => o.SUM_EXPN_AMNT_DNRM);
            OrdrPymtAmount3_Txt.EditValue = ordr.Sum(o => o.PYMT_AMNT_DNRM);
            OrdrDsctAmount3_Txt.EditValue = ordr.Sum(o => o.DSCN_AMNT_DNRM);
            OrdrCostAmount3_Txt.EditValue = ordr.Sum(o => o.COST_AMNT_DNRM);
            OrdrTxfeAmount3_Txt.EditValue = ordr.Sum(o => o.SUM_FEE_AMNT_DNRM);
            OrdrCloseAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Sum(o => o.SUM_EXPN_AMNT_DNRM);
            OrdrPymtCloseAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Sum(o => o.PYMT_AMNT_DNRM);
            OrdrDsctCloseAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Sum(o => o.DSCN_AMNT_DNRM);
            OrdrCostCloseAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Sum(o => o.COST_AMNT_DNRM);
            OrdrTxfeCloseAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Sum(o => o.SUM_FEE_AMNT_DNRM);
            OrdrMaxAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Max(o => o.SUM_EXPN_AMNT_DNRM);
            OrdrMinAmount3_Txt.EditValue = ordr.Where(o => o.ORDR_STAT == "004" || o.ORDR_STAT == "009").Min(o => o.SUM_EXPN_AMNT_DNRM);
            OrdrOpenAmount3_Txt.EditValue = ordr.Where(o => !(o.ORDR_STAT == "004" || o.ORDR_STAT == "009")).Sum(o => o.SUM_EXPN_AMNT_DNRM);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SrbtStp3Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtStp3Bs.Current as Data.Service_Robot;
            if (srbt == null) return;

            WletBs.DataSource = iRoboTech.Wallets.Where(w => w.Service_Robot == srbt);

            if (WletBs.List.Count == 2)
            {
               SrbtCredWletAmnt3_Butn.Text = string.Format("موجود اعتباری : " + "{0:n0}", WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM);
               SrbtCredWletAmnt3_Butn.Tag = WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM;
               SrbtCashWletAmnt3_Butn.Text = string.Format("موجود نقدی : " + "{0:n0}", WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM);
               SrbtCashWletAmnt3_Butn.Tag = WletBs.List.OfType<Data.Wallet>().FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM;
            }

            FromDate3_Dt.CommitChanges();
            ToDate3_Dt.CommitChanges();

            if (!FromDate3_Dt.Value.HasValue) FromDate3_Dt.Value = DateTime.Now;
            if (!ToDate3_Dt.Value.HasValue) ToDate3_Dt.Value = DateTime.Now;

            Ordr4Stp3Bs.DataSource =
               iRoboTech.Orders.Where(o =>
                  o.Service_Robot == srbt &&
                  o.ORDR_TYPE == "004" &&
                  o.ORDR_STAT != "003" &&
                  o.STRT_DATE.Value.Date >= FromDate3_Dt.Value.Value.Date &&
                  o.STRT_DATE.Value.Date <= ToDate3_Dt.Value.Value.Date
               );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
