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
using C1.Win.C1Input;
using System.Threading;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class CASH_CNTR_F : UserControl
   {
      public CASH_CNTR_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long ordrCode = 0;
      private long? chatid = 0;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Rocalhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

         RoboBs.DataSource = iRoboTech.Robots.Where(r => Fga_Ugov_U.Contains(r.Organ.OGID) && r.RBID == 401);

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            SrbtBs.DataSource =
               iRoboTech.Service_Robots
               .Where(sr =>
                  sr.Robot == robo
               );

            UserBs.DataSource = SrbtBs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => iRoboTech.V_Users.Any(u => u.USER_DB == CurrentUser.ToUpper() && u.CELL_PHON == sr.CELL_PHON));
            var user = UserBs.Current as Data.Service_Robot;
            if (user == null) { MessageBox.Show("متاسفانه عملیات اتصال به کاربر صندوق فروشگاه برقرار نشد"); return; }

            RbprBs.DataSource =
               from rp in iRoboTech.Robot_Products
               where rp.STAT == "002"
                  && !(
                        from rlcg in iRoboTech.Robot_Limited_Commodity_Groups
                        join rpl in iRoboTech.Robot_Product_Limiteds on rp equals rpl.Robot_Product
                        where rlcg.Robot == robo
                           && rpl.RLCG_CODE == rlcg.CODE
                           && rpl.STAT == "002"
                           && rlcg.STAT == "002"
                           && !rlcg.Service_Robot_Access_Limited_Group_Products
                              .Any(sral => sral.CHAT_ID == user.CHAT_ID && sral.STAT == "002")
                        select rlcg
                     ).Any()
               select rp;

            SrbtOrdr25Bs.DataSource =
               SrbtBs.List.OfType<Data.Service_Robot>()
               .Where(sr =>
                  sr.Robot == robo &&
                  sr.STAT == "002" &&
                  sr.Orders
                  .Any(o =>
                     o.ORDR_TYPE == "025" &&
                     o.ORDR_STAT == "016" &&
                     o.CRET_BY == CurrentUser.ToUpper()
                  )
               );

            SrbtOrdr25Cont_Lb.Text = SrbtOrdr25Bs.Count.ToString();
            SrbtOrdr25Indx_Lb.Text = "1";
         }
         catch {
            SrbtNameFrm1_Lb.Text = SrbtChatid_Lb.Text = SrbtCellPhon_Lb.Text = SrbtServAdrs_Lb.Text = "---";
            Ordr4CodeFrm1_Txt.Text = Ordr4FknoFrm1_Txt.Text = "";
            Ordt4Bs.Clear();
            SrbtOrdr25Indx_Lb.Text = SrbtOrdr25Cont_Lb.Text = "0";
            Ship001_Butn.NormalColorA = Ship001_Butn.NormalColorB =
            Ship002_Butn.NormalColorA = Ship002_Butn.NormalColorB =
            Ship003_Butn.NormalColorA = Ship003_Butn.NormalColorB =
            Ship004_Butn.NormalColorA = Ship004_Butn.NormalColorB = Color.White;
            ServAdrs_Pn.Visible = true;
         }         
      }

      private void KeyPadOprt_Butn_Clicked(object sender, EventArgs e)
      {
         try
         {
            var key = sender as C1.Win.C1Input.C1Button;

            #region Left Keypad Number
            switch (key.Tag.ToString())
            {
               case "RClear":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text = "";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text = "";
                  break;
               case "RBackSpace":
                  if (CustomerFrm1_Rb.Checked)
                  {
                     if(SrbtFrm1_Txt.Text.Length > 0)
                        SrbtFrm1_Txt.Text = SrbtFrm1_Txt.Text.Substring(0, SrbtFrm1_Txt.Text.Length - 1);
                  }
                  else if (ProdFrm1_Rb.Checked)
                  {
                     if (RbprFrm1_Txt.Text.Length > 0)
                        RbprFrm1_Txt.Text = RbprFrm1_Txt.Text.Substring(0, RbprFrm1_Txt.Text.Length - 1);
                  }
                  break;
               case "REnter":
                  //AddTarfWithQury_Butn_Click(null, null);
                  break;
               case "RZero":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "0";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "0";
                  break;
               case "ROne":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "1";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "1";
                  break;
               case "RTwo":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "2";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "2";
                  break;
               case "RThree":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "3";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "3";
                  break;
               case "RFour":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "4";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "4";
                  break;
               case "RFive":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "5";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "5";
                  break;
               case "RSix":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "6";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "6";
                  break;
               case "RSeven":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "7";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "7";
                  break;
               case "REight":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "8";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "8";
                  break;
               case "RNine":
                  if (CustomerFrm1_Rb.Checked)
                     SrbtFrm1_Txt.Text += "9";
                  else if (ProdFrm1_Rb.Checked)
                     RbprFrm1_Txt.Text += "9";
                  break;
            }
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SrbtProdOptnFrm1_Rb_CheckedChanged(object sender, EventArgs e)
      {
         CellPhonFrm1_Rb.Visible = ChatIdFrm1_Rb.Visible = NatlCodeFrm1_Rb.Visible = CustomerFrm1_Rb.Checked;
         TarfCodeFrm1_Rb.Visible = BarCodeFrm1_Rb.Visible = ProdFrm1_Rb.Checked;

         if (CustomerFrm1_Rb.Checked)
            CellPhonFrm1_Rb.Checked = true;

         if (ProdFrm1_Rb.Checked)
            BarCodeFrm1_Rb.Checked = true;
      }

      private void SrbtFrm1_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var srbt =
               SrbtBs.List.OfType<Data.Service_Robot>()
               .Where(sr =>
                     (CellPhonFrm1_Rb.Checked && sr.CELL_PHON != null && sr.CELL_PHON.StartsWith(e.NewValue.ToString())) ||
                     (NatlCodeFrm1_Rb.Checked && sr.NATL_CODE != null && sr.NATL_CODE.StartsWith(e.NewValue.ToString())) ||
                     (ChatIdFrm1_Rb.Checked && sr.CHAT_ID.ToString().StartsWith(e.NewValue.ToString()))
                  );

            SrbtFrm1_Txt.Properties.Buttons[1].Caption = srbt.Count().ToString();
            if (srbt == null) return;

            switch (srbt.Count())
            {
               case 1:
                  SrbtBs.Position = SrbtBs.IndexOf(srbt.FirstOrDefault());
                  //Srbt_Lov.EditValue = srbt.FirstOrDefault().CHAT_ID;
                  //if (AutoSaveOrdr_Cbx.Checked)
                  //{
                  // اگر مشتری قبلا درون سیستم درخواست براش از این طریق وارد کردن اطلاعات ثبت شده باشد دیگر نیازی به ثبت مجدد نیست مگر اینکه به صورت دستی اقدام کنند
                  if(!SrbtOrdr25Bs.List.OfType<Data.Service_Robot>().Any(sr => sr == srbt.FirstOrDefault()))
                     CretOrdr25_DoAction(srbt.FirstOrDefault().CHAT_ID);
                  //   Main_Tc.SelectedTab = Ordr4Stp2_Tp;
                  //   if (RbprBs.List.Count == 0)
                  //      SearchProduct_Butn_Click(null, null);
                  //   TarfNumb_Txt.EditValue = 1;                  
                  ComnOprt_Tm.Enabled = true;
                  //}
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SearchItemFrm1_Rb_CheckedChanged(object sender, EventArgs e)
      {
         var ui = sender as RadioButton;
         switch (ui.Tag.ToString())
         {
            case "srbt":
               SrbtFrm1_Txt.Focus();
               break;
            case "rbpr":
               RbprFrm1_Txt.Focus();
               break;
         }
      }

      private void CretOrdr25_DoAction(long? chatid)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            var srbt = UserBs.Current as Data.Service_Robot;
            //if (SrbtFrm1_Txt.EditValue != null && SrbtFrm1_Txt.Text.Length > 0)
            if(chatid != null)
               srbt = SrbtBs.Current as Data.Service_Robot;
                  
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
               SrbtOrdr25Bs.Position = SrbtOrdr25Bs.IndexOf(SrbtOrdr25Bs.List.OfType<Data.Service_Robot>().FirstOrDefault(sr => sr.CHAT_ID == chatid));
               Ordr25Bs.Position = Ordr25Bs.IndexOf(Ordr25Bs.List.OfType<Data.Order>().FirstOrDefault(o => o.CHAT_ID == chatid && o.CODE == ordrCode));
            }
         }
      }

      private void ComnOprt_Tm_Tick(object sender, EventArgs e)
      {
         if(Master000_Tc.SelectedTab == tp_001)
         {
            SrbtFrm1_Txt.EditValue = "";
            RbprFrm1_Txt.Focus();
            ProdFrm1_Rb.Checked = true;
         }
         else if(Master000_Tc.SelectedTab == tp_002)
         {

         }

         ComnOprt_Tm.Enabled = false;
      }

      private void UpSrbtOrdr25_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SrbtOrdr25Bs.Position > 0)
               SrbtOrdr25Bs.MovePrevious();

            SrbtOrdr25Indx_Lb.Text = (SrbtOrdr25Bs.Position + 1).ToString();
         }
         catch { }
      }

      private void DownSrbtOrdr25_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SrbtOrdr25Bs.Position < (SrbtOrdr25Bs.Count - 1))
               SrbtOrdr25Bs.MoveNext();

            SrbtOrdr25Indx_Lb.Text = (SrbtOrdr25Bs.Position + 1).ToString();
         }
         catch { }
      }

      private void SrbtOrdr25Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var srbtordr25 = SrbtOrdr25Bs.Current as Data.Service_Robot;
            if (srbtordr25 == null) return;

            Ordr25Bs.DataSource =
               iRoboTech.Orders.Where(o => o.Service_Robot == srbtordr25 && o.ORDR_TYPE == "025" && o.ORDR_STAT == "016");
         }
         catch { }
      }

      private void Ordr25Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var ordr25 = Ordr25Bs.Current as Data.Order;
            if (ordr25 == null) { CustomerFrm1_Rb.Checked = true; return; }

            Ordr4Bs.DataSource = ordr25.Orders;
            //Ordt4Bs.List.Clear();
            ProdFrm1_Rb.Checked = true;
         }
         catch { }
      }

      private void Ordr4Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var ordr4 = Ordr4Bs.Current as Data.Order;
            if (ordr4 == null) return;

            Ordr4CodeFrm1_Txt.Text = ordr4.CODE.ToString();
            Ordr4FknoFrm1_Txt.Text = ordr4.ORDR_TYPE_NUMB.ToString();
            PayAmnt_Txt.EditValue = ordr4.DEBT_DNRM;
            CredWlet_Txt.EditValue = ordr4.Service_Robot.Wallets.FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM;
            CashWlet_Txt.EditValue = ordr4.Service_Robot.Wallets.FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM;
            AcptPay_Butn.Visible = ordr4.DEBT_DNRM == 0 ? true : false;
            OthrPay_Pn.Visible = !AcptPay_Butn.Visible;
            OrdrWletCredPay_Butn.Enabled = (ordr4.DEBT_DNRM <= ordr4.Service_Robot.Wallets.FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM) ? true : false;
            OrdrWletCashPay_Butn.Enabled = (ordr4.DEBT_DNRM <= ordr4.Service_Robot.Wallets.FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM) ? true : false;
            AmntType_Lb.Text = DAmutBs.List.OfType<Data.D_AMUT>().FirstOrDefault(d => d.VALU == ordr4.AMNT_TYPE).DOMN_DESC;

            switch(ordr4.HOW_SHIP)
            {
               case "000":
                  Ship001_Butn.NormalColorA = Ship001_Butn.NormalColorB =
                  Ship002_Butn.NormalColorA = Ship002_Butn.NormalColorB =
                  Ship003_Butn.NormalColorA = Ship003_Butn.NormalColorB =
                  Ship004_Butn.NormalColorA = Ship004_Butn.NormalColorB = Color.White;
                  ServAdrs_Pn.Visible = true;
                  break;
               case "001":
                  Ship001_Butn.NormalColorA = Ship001_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  ServAdrs_Pn.Visible = false;
                  break;
               case "002":
                  Ship002_Butn.NormalColorA = Ship002_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  ServAdrs_Pn.Visible = true;
                  break;
               case "003":
                  Ship003_Butn.NormalColorA = Ship003_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  ServAdrs_Pn.Visible = true;
                  break;
               case "004":
                  Ship004_Butn.NormalColorA = Ship004_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  ServAdrs_Pn.Visible = true;
                  break;
            }
         }
         catch { }
      }

      private void RbprFrm1_Txt_KeyDown(object sender, KeyEventArgs e)
      {
         try
         {
            if (e.KeyCode == Keys.Enter)
            {
               var ordr4 = Ordr4Bs.Current as Data.Order;
               if (ordr4 == null) return;

               if (RbprFrm1_Txt.Text == "") return;

               RbprFrm1_Txt.Text = RbprFrm1_Txt.Text.Replace(Environment.NewLine, "");

               var prod =
                  RbprBs.List.OfType<Data.Robot_Product>()
                  .Where(rp =>
                     rp.Robot == ordr4.Robot &&
                     (
                        TarfCodeFrm1_Rb.Checked && rp.TARF_CODE == RbprFrm1_Txt.Text ||
                        BarCodeFrm1_Rb.Checked && rp.BAR_CODE == RbprFrm1_Txt.Text
                     )
                  );

               if (prod.Count() != 1)
               {
                  if (prod.Count() == 0)
                     throw new Exception("برای کد خوانده شده هیچ محصولی وجود ندارد" + " - " + RbprFrm1_Txt.Text);
                  else
                     throw new Exception("برای کد خوانده شده بیش از یک رکورد پیدا شد" + " - " + RbprFrm1_Txt.Text);
               }

               RbprBs.Position =
                  RbprBs.IndexOf(prod.FirstOrDefault());

               if (ordr4.HOW_SHIP == "000")
                  iRoboTech.ExecuteCommand(string.Format("UPDATE dbo.[Order] SET HOW_SHIP = '001' WHERE Code = {0};", ordr4.CODE));

               AddTarfToCart_DoAction();

               e.Handled = true;
               RbprFrm1_Txt.Text = "";
               ComnOprt_Tm.Enabled = true;
            }
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }

      private void AddTarfToCart_DoAction()
      {
         try
         {
            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            var prod = RbprBs.Current as Data.Robot_Product;
            if (prod == null) return;

            var textInput = prod.TARF_CODE;
            // IF NOT EXISTS PRODUCT IN CART
            if (!ordr.Order_Details.Any(od => od.TARF_CODE == prod.TARF_CODE))
            {
               textInput += "*n1";
            }
            // ELSE IF EXISTS PRODUCT IN CART
            else
            {
               textInput += "*+=1";
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
            }
         }
      }

      private void AddNewOrdr25Frm1_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var srbtordr25 = SrbtOrdr25Bs.Current as Data.Service_Robot;
            if (srbtordr25 == null) return;
            SrbtBs.Position = SrbtBs.IndexOf(srbtordr25);
            CretOrdr25_DoAction(srbtordr25.CHAT_ID);
         }
         catch { }
      }

      private void DelOrdr25Frm1_Tsm_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr25 = Ordr25Bs.Current as Data.Order;
            if (ordr25 == null) return;

            // اگر سفارش ردیف های پرداختی داشته باشد
            if (ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001").Order_States.Any())
            {
               if (
                  MessageBox.Show(this, 
                     "سفارش دارای مبلغ پرداختی می باشد." + Environment.NewLine + 
                     "آیا با حذف ردیف های پرداختی سفارش موافق هستید؟" + Environment.NewLine + 
                     "جمع مبلغ پرداختی : " +
                     ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001").Order_States.Sum(os => os.AMNT).Value.ToString("n0") + " " + AmntType_Lb.Text + Environment.NewLine + 
                     "تعداد ردیف پرداختی : " +
                     ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001").Order_States.Count().ToString() + " عدد", 
                     "حذف ردیف های پرداختی صورتحساب", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.RtlReading) != DialogResult.Yes) return;
               //iRoboTech.ExecuteCommand("DELETE dbo.Order_State WHERE Ordr_Code = {0};", ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001").CODE);
            }

            // اگر سفارش دارای اقلام کالا باشد
            if (ordr25.Orders.FirstOrDefault(o => o.ORDR_TYPE == "004" && o.ORDR_STAT == "001").Order_Details.Any())
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

      private void OrdtActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            Ordt4Stp2_Gv.PostEditor();

            var ordt = Ordt4Bs.Current as Data.Order_Detail;
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
               Execute_Query();
         }
      }

      private void LockUserFrm1_Butn_Click(object sender, EventArgs e)
      {

      }

      private void LockPinFrm1_Butn_Click(object sender, EventArgs e)
      {
         // نمایش پیام به فروشنده جهت صبر کردن برای اتمام عملیات
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "Commons", 39 /* Execute DoWork4PinCode */, SendType.Self)
            {
               Input =
                  new XElement("User",
                      new XAttribute("username", CurrentUser)                      
                  )
            }
         );
      }

      private void DelCrntOrdrFrm1_Butn_Click(object sender, EventArgs e)
      {
         DelOrdr25Frm1_Tsm_Click(null, null);
      }

      private void PayCrntOrdrFrm1_Butn_Click(object sender, EventArgs e)
      {
         Master000_Tc.SelectedTab = tp_003;
         tp_003.Tag = "From_tp_001";

         var ordr4 = Ordr4Bs.Current as Data.Order;
         if (ordr4 == null) return;

         AcptPay_Butn.Visible = ordr4.DEBT_DNRM == 0 ? true : false;
         OthrPay_Pn.Visible = !AcptPay_Butn.Visible;         
         OrdrWletCredPay_Butn.Enabled = (ordr4.DEBT_DNRM <= ordr4.Service_Robot.Wallets.FirstOrDefault(w => w.WLET_TYPE == "001").AMNT_DNRM) ? true : false;
         OrdrWletCashPay_Butn.Enabled = (ordr4.DEBT_DNRM <= ordr4.Service_Robot.Wallets.FirstOrDefault(w => w.WLET_TYPE == "002").AMNT_DNRM) ? true : false;
         AmntType_Lb.Text = DAmutBs.List.OfType<Data.D_AMUT>().FirstOrDefault(d => d.VALU == ordr4.AMNT_TYPE).DOMN_DESC;
      }

      private void OrdrCashPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            //if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            //if (MessageBox.Show(this, "تسویه حساب مالی به صورت نقدی", "انجام عملیات تسویه حساب نقدی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Print Default Active Reports
            //**DfltPrnt002_Butn_Click(null, null);
            //return;
            var user = UserBs.Current as Data.Service_Robot;
            if(PayAmnt_Txt.EditValue == null || PayAmnt_Txt.Text.In("", "0")){PayAmnt_Txt.Focus(); return;}

            //long? amnt = ordr.AMNT_TYPE == "001" ? PayAmnt_Txt.EditValue.ToString().ToInt64() : PayAmnt_Txt.EditValue.ToString().ToInt64() * 10;
            long? amnt = PayAmnt_Txt.EditValue.ToString().ToInt64();

            // Do Some things
            iRoboTech.INS_ODST_P(
               ordr.CODE, null, DateTime.Now, "واریزی مبلغ نقدی", amnt, "006", "001", null, ordr.DEST_CARD_NUMB_DNRM, user.CHAT_ID.ToString(), null, null, null, "002", DateTime.Now, "دریافت مبلغ نقدی توسط صندوقدار - " + user.NAME
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
            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            //if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            //if (MessageBox.Show(this, "تسویه حساب مالی به صورت کارتخوان", "انجام عملیات تسویه حساب کارتخوان", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            if (VPosBs1.List.Count == 0)
               UsePos_Cb.Checked = false;

            var user = UserBs.Current as Data.Service_Robot;
            if (PayAmnt_Txt.EditValue == null || PayAmnt_Txt.Text.In("", "0")) { PayAmnt_Txt.Focus(); return; }

            //long? amnt = ordr.AMNT_TYPE == "001" ? PayAmnt_Txt.EditValue.ToString().ToInt64() : PayAmnt_Txt.EditValue.ToString().ToInt64() * 10;
            long? amnt = PayAmnt_Txt.EditValue.ToString().ToInt64();

            if (UsePos_Cb.Checked)
            {
               #region Pos Operation
               //var amnt = ordr.DEBT_DNRM;
               if (amnt == 0) return;

               var robo = RoboBs.Current as Data.Robot;

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

               if (robo.AMNT_TYPE == "002")
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
                                       new XAttribute("subsys", 12),
                                       new XAttribute("rqid", ordr.CODE),
                                       new XAttribute("rqtpcode", "004"),
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
               #endregion
            }
            else
            {
               // Print Default Active Reports
               //**DfltPrnt002_Butn_Click(null, null);

               // Do Some things
               iRoboTech.INS_ODST_P(
                  ordr.CODE, null, DateTime.Now, "واریزی مبلغ غیر PCPOS کارتخوان", amnt, "006", "003", null, ordr.DEST_CARD_NUMB_DNRM, user.CHAT_ID.ToString(), null, null, null, "002", DateTime.Now, "دریافت مبلغ غیر PCPOS کارتخوان توسط صندوقدار - " + user.NAME
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

      private void OrdrWletCredPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            //if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            //if (MessageBox.Show(this, "تسویه حساب مالی از کیف پول اعتباری", "انجام عملیات تسویه حساب کیف پول اعتباری", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Print Default Active Reports
            //**DfltPrnt002_Butn_Click(null, null);

            var user = UserBs.Current as Data.Service_Robot;
            if (PayAmnt_Txt.EditValue == null || PayAmnt_Txt.Text.In("", "0")) { PayAmnt_Txt.Focus(); return; }

            //long? amnt = ordr.AMNT_TYPE == "001" ? PayAmnt_Txt.EditValue.ToString().ToInt64() : PayAmnt_Txt.EditValue.ToString().ToInt64() * 10;
            long? amnt = PayAmnt_Txt.EditValue.ToString().ToInt64();

            if ((CredWlet_Txt.EditValue.ToString().ToInt64() - ordr.Order_States.Where(os => os.AMNT_TYPE == "006" && os.RCPT_MTOD == "015").Sum(os => os.AMNT)) < amnt)
            {
               MessageBox.Show(this, "موجودی کیف پول اعتباری شما کم میباشد", "خطا - عدم موجودی", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               PayAmnt_Txt.EditValue = CredWlet_Txt.EditValue;
               return;
            }

            // Do Some things
            iRoboTech.INS_ODST_P(
               ordr.CODE, null, DateTime.Now, "واریزی مبلغ از کیف پول اعتباری", amnt, "006", "015", null, ordr.DEST_CARD_NUMB_DNRM, user.CHAT_ID.ToString(), null, null, null, "002", DateTime.Now, "دریافت مبلغ از کیف پول اعتباری توسط صندوقدار - " + user.NAME
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
            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            //if (ordr.HOW_SHIP == "000") { HowShip_lov.Focus(); MessageBox.Show("لطفا آدرس ارسال را مشخص کنید"); return; }

            //if (MessageBox.Show(this, "تسویه حساب مالی از کیف پول نقدی", "انجام عملیات تسویه حساب کیف پول نقدی", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            // Print Default Active Reports
            //**DfltPrnt002_Butn_Click(null, null);

            var user = UserBs.Current as Data.Service_Robot;
            if (PayAmnt_Txt.EditValue == null || PayAmnt_Txt.Text.In("", "0")) { PayAmnt_Txt.Focus(); return; }

            //long? amnt = ordr.AMNT_TYPE == "001" ? PayAmnt_Txt.EditValue.ToString().ToInt64() : PayAmnt_Txt.EditValue.ToString().ToInt64() * 10;
            long? amnt = PayAmnt_Txt.EditValue.ToString().ToInt64();

            if ((CashWlet_Txt.EditValue.ToString().ToInt64() - ordr.Order_States.Where(os => os.AMNT_TYPE == "006" && os.RCPT_MTOD == "014").Sum(os => os.AMNT)) < amnt)
            {
               MessageBox.Show(this, "موجودی کیف پول نقدی شما کم میباشد", "خطا - عدم موجودی", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               PayAmnt_Txt.EditValue = CashWlet_Txt.EditValue;
               return;
            }

            // Do Some things
            iRoboTech.INS_ODST_P(
               ordr.CODE, null, DateTime.Now, "واریزی مبلغ از کیف پول نقدی", amnt, "006", "014", null, ordr.DEST_CARD_NUMB_DNRM, user.CHAT_ID.ToString(), null, null, null, "002", DateTime.Now, "دریافت مبلغ از کیف پول نقدی توسط صندوقدار - " + user.NAME
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

      private void BackPay_Butn_Click(object sender, EventArgs e)
      {
         switch(tp_003.Tag.ToString())
         {
            case "From_tp_001":
               Master000_Tc.SelectedTab = tp_001;
               break;
            case "From_tp_002":
               Master000_Tc.SelectedTab = tp_002;
               break;
         }
      }

      private void AcptPay_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            if(ordr.DEBT_DNRM != 0)
            {
               MessageBox.Show(this, "صورتحساب شما به صورت کامل پرداخت نشده است", "خطا - عدم تسویه حساب صوراحساب", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               return;
            }


            // چاپ فیش فاکتور مشتری
            switch (tp_003.Tag.ToString())
            {
               case "From_tp_001":
                  DfltPrntFrm1_Butn_Click(DfltPrntFrm1_Butn, null);
                  break;
               case "From_tp_002":                  
                  break;
            }

            Master000_Tc.Enabled = false;

            // نمایش پیام به فروشنده جهت صبر کردن برای اتمام عملیات
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons:ErrorHandle", 04 /* Execute DoWork4ShowMessage */, SendType.Self)
               {
                  Input =
                     new XElement("Message",
                         new XAttribute("status", "show"),
                         new XAttribute("message", "لطفا کمی صبر کنید تا عملیات ذخیره سازی به اتمام برسد")
                     )
               }
            );

            #region Do Operation
            ThreadStart starter =
               () =>
               {
                  try
                  {
                     // ذخیره نهایی سفارش درون پایگاه داده
                     XElement xResult = new XElement("Result");
                     iRoboTech.CommandTimeout = 18000;
                     iRoboTech.REGS_ORDR_P(
                        new XElement("Order",
                            new XAttribute("code", ordr.CODE)
                        ),
                        ref xResult
                     );
                  }
                  catch (Exception exc) { MessageBox.Show(exc.Message); }
               };

            starter +=
               () =>
               {
                  if (InvokeRequired)
                  {
                     Invoke(
                        new System.Action(
                        () =>
                        {
                           Master000_Tc.Enabled = true;
                           // نمایش پیام به فروشنده جهت صبر کردن برای اتمام عملیات
                           _DefaultGateway.Gateway(
                              new Job(SendType.External, "localhost", "Commons:ErrorHandle", 04 /* Execute DoWork4ShowMessage */, SendType.Self)
                              {
                                 Input =
                                    new XElement("Message",
                                        new XAttribute("status", "close")
                                    )
                              }
                           );
                           Execute_Query();
                           SaveOrdrNewFrm1_Butn_Click(null, null);
                        }
                        )
                     );
                  }
                  else
                  {
                     // Do what you want in the callback
                     // نمایش پیام به فروشنده جهت صبر کردن برای اتمام عملیات
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "Commons:ErrorHandle", 04 /* Execute DoWork4ShowMessage */, SendType.Self)
                        {
                           Input =
                              new XElement("Message",
                                  new XAttribute("status", "close")
                              )
                        }
                     );
                     Execute_Query();
                     SaveOrdrNewFrm1_Butn_Click(null, null);
                  }
               };
            Thread thread = new Thread(starter) { IsBackground = true };
            thread.Start();
            #endregion            

            // بعد از اتمام ثبت عملیات به فرم ثبت سفارشات بر میگردیم
            switch (tp_003.Tag.ToString())
            {
               case "From_tp_001":
                  Master000_Tc.SelectedTab = tp_001;                  
                  break;
               case "From_tp_002":
                  Master000_Tc.SelectedTab = tp_002;
                  break;
            }

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         //finally
         //{
         //   if (requery)
         //      Execute_Query();
         //}
      }

      private void OdstActn_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var odst = OdstBs.Current as Data.Order_State;
            if (odst == null) return;

            if (MessageBox.Show(this, "آیا با حذف مبلغ پرداختی موافق هستید؟", "حذف مبلغ پرداختی", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            iRoboTech.DEL_ODST_P(odst.ORDR_CODE, odst.CODE);

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

      private void NewSaleFrm1_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            DelCrntOrdrFrm1_Butn_Click(null, null);

            var srbt = UserBs.Current as Data.Service_Robot;            
            if (srbt == null) return;

            SrbtBs.Position = SrbtBs.IndexOf(srbt);
            // اگر مشتری قبلا درون سیستم درخواست براش از این طریق وارد کردن اطلاعات ثبت شده باشد دیگر نیازی به ثبت مجدد نیست مگر اینکه به صورت دستی اقدام کنند
            if (!SrbtOrdr25Bs.List.OfType<Data.Service_Robot>().Any(sr => sr == srbt))
               CretOrdr25_DoAction(srbt.CHAT_ID);
            ComnOprt_Tm.Enabled = true;            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveOrdrNewFrm1_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srbt = UserBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            SrbtBs.Position = SrbtBs.IndexOf(srbt);
            // اگر مشتری قبلا درون سیستم درخواست براش از این طریق وارد کردن اطلاعات ثبت شده باشد دیگر نیازی به ثبت مجدد نیست مگر اینکه به صورت دستی اقدام کنند
            if (
                  (!SrbtOrdr25Bs.List.OfType<Data.Service_Robot>().Any(sr => sr == srbt)) ||
                  (SrbtOrdr25Bs.List.OfType<Data.Service_Robot>()
                   .Any(sr =>
                      sr == srbt &&
                      sr.Orders
                      .Where(o =>
                         o.ORDR_TYPE == "025" &&
                         o.ORDR_STAT == "016"
                      )
                      .All(o =>                      
                         o.Orders
                         .Where(o4 => 
                            o4.ORDR_TYPE == "004" &&
                            o4.ORDR_STAT == "001" 
                         )
                         .All(o4 =>                         
                            o4.Order_Details.Any()
                         )
                      )
                   )
                  )
            )
            {
               CretOrdr25_DoAction(srbt.CHAT_ID);
            }
            else
            {
               SrbtOrdr25Bs.Position = SrbtOrdr25Bs.IndexOf(srbt);
               Ordr25Bs.Position =
                  Ordr25Bs.IndexOf(
                     Ordr25Bs.List.OfType<Data.Order>()
                     .FirstOrDefault(o =>
                        o.Service_Robot == srbt &&
                        o.Orders
                        .Any(o4 =>
                           o4.ORDR_TYPE == "004" &&
                           o4.ORDR_STAT == "001" &&
                           !o4.Order_Details.Any()
                        )
                     )
                  );
            }            
            ComnOprt_Tm.Enabled = true;
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

            var ordr = Ordr4Bs.Current as Data.Order;
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

      private void DiscountOnSaleFrm1_Butn_Click(object sender, EventArgs e)
      {

      }

      private void NoteForSaleFrm1_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ShipForSaleFrm1_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ShowDestGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ordr = Ordr4Bs.Current as Data.Order;
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

      private void SaveAdrsFrm1_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            //OrdrSaveChanges_Butn_Click(null, null);

            var ordr = Ordr4Bs.Current as Data.Order;
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

      private void ShowOrdrFinlReg_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ShowDsctProd_Butn_Click(object sender, EventArgs e)
      {

      }

      private void DfltPrntFrm1_Butn_Click(object sender, EventArgs e)
      {
         var ordr = Ordr4Bs.Current as Data.Order;
         if (ordr == null) return;

         var ui = sender as C1Button;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 15 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + ui.Tag.ToString()), string.Format("dbo.[Order].Code = {0}", ordr.CODE))}
               }
            )
         );
      }

      private void SlctPrntFrm1_Butn_Click(object sender, EventArgs e)
      {
         var ordr = Ordr4Bs.Current as Data.Order;
         if (ordr == null) return;

         var ui = sender as C1Button;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 15 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + ui.Tag.ToString()), string.Format("dbo.[Order].Code = {0}", ordr.CODE))}
               }
            )
         );
      }

      private void ConfPrntFrm1_Butn_Click(object sender, EventArgs e)
      {
         var ui = sender as C1Button;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 14 /* Execute Stng_Rprt_F */),
                  new Job(SendType.SelfToUserInterface, "STNG_RPRT_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + ui.Tag.ToString()))}
               }
            )
         );
      }

      private void ShipOprt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var ship = sender as MaxUi.RoundedButton;

            var ordr = Ordr4Bs.Current as Data.Order;
            if (ordr == null) return;

            iRoboTech.ExecuteCommand("UPDATE dbo.[Order] SET HOW_SHIP = '{0}' WHERE CODE = {1}", ship.Tag, ordr.CODE);
            Ship001_Butn.NormalColorA = Ship001_Butn.NormalColorB =
            Ship002_Butn.NormalColorA = Ship002_Butn.NormalColorB =
            Ship003_Butn.NormalColorA = Ship003_Butn.NormalColorB =
            Ship004_Butn.NormalColorA = Ship004_Butn.NormalColorB = Color.White;

            switch (ship.Tag.ToString())
            {  
               case "001":
                  Ship001_Butn.NormalColorA = Ship001_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  break;
               case "002":
                  Ship002_Butn.NormalColorA = Ship002_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  break;
               case "003":
                  Ship003_Butn.NormalColorA = Ship003_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  break;
               case "004":
                  Ship004_Butn.NormalColorA = Ship004_Butn.NormalColorB = Color.FromArgb(255, 192, 128);
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Ordr4Bs.EndEdit();

            iRoboTech.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
