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
using System.Diagnostics;
using System.Threading;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class WLET_DVLP_F : UserControl
   {
      public WLET_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      readonly string PasswordHash = "P@@Sw0rd";
      readonly string SaltKey = "S@LT&KEY";
      readonly string VIKey = "@1B2c3D4e5F6g7H8";

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
         int wlet = WletBs.Position;
         int srbt = SrbtBs.Position;
         int r24s = O24sBs.Position;
         int r24e = O24eBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));
         

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         WletBs.Position = wlet;
         SrbtBs.Position = srbt;
         ATxfeBs.DataSource = iRoboTech.Transaction_Fees.Where(t => t.TXFE_TYPE != "002");

         O24sBs.DataSource = iRoboTech.Orders.Where(o => o.ORDR_TYPE == "024" && o.ORDR_STAT == "001");
         O24eBs.DataSource = iRoboTech.Orders.Where(o => o.ORDR_TYPE == "024" && o.ORDR_STAT == "004");
         O13sBs.DataSource = iRoboTech.Orders.Where(o => o.ORDR_TYPE == "013" && o.ORDR_STAT == "001");

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            WletBs.DataSource =
               iRoboTech.Wallets.Where(w => w.Robot == robo && (w.Service_Robot.NATL_CODE != null || w.Service_Robot.REGS_TYPE == "002"));

            SrbtBs.DataSource =
               iRoboTech.Service_Robots.Where(sr => sr.Robot == robo && ((sr.NATL_CODE != null && sr.NATL_CODE != "" && sr.NATL_CODE.Length == 10) || sr.REGS_TYPE == "002"));

            int srbs = SrbsBs.Position;

            SrbsBs.DataSource = iRoboTech.Service_Robot_Sellers.Where(s => s.Service_Robot.Robot == robo);

            SrbsBs.Position = srbs;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void WldtBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var wldt = WldtBs.Current as Data.Wallet_Detail;
            if (wldt == null) return;

            if (wldt.Transaction_Fee != null)
            {
               TxfeBs.DataSource = wldt.Transaction_Fee;
               WldtGrop_Gb.Visible = true;
            }
            else
            {
               WldtGrop_Gb.Visible = false;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void O24sBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var o24s = O24sBs.Current as Data.Order;
            if (o24s == null) return;

            OrdrBs.DataSource = o24s;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void O24eBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var o24e = O24eBs.Current as Data.Order;
            if (o24e == null) return;

            OrdrBs.DataSource = o24e;
            O17Bs.DataSource = o24e.Orders.FirstOrDefault(o => o.ORDR_TYPE == "017");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void WletBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            wldt_gv.TopRowIndex = 0;            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AddServDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            if (SrdcBs.List.OfType<Data.Service_Robot_Discount_Card>().Any(d => d.DCID == 0)) return;

            var srdc = SrdcBs.AddNew() as Data.Service_Robot_Discount_Card;
            srdc.Service_Robot = srbt;

            iRoboTech.Service_Robot_Discount_Cards.InsertOnSubmit(srdc);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelServDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srdc = SrdcBs.Current as Data.Service_Robot_Discount_Card;
            if (srdc == null) return;

            if(srdc.VALD_TYPE == "001" || ( srdc.VALD_TYPE == "002" && MessageBox.Show(this, "آیا با حذف تخفیف موافق هستید؟", "حذف تخفیف", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes ))

            iRoboTech.ExecuteCommand("UPDATE dbo.Service_Robot_Discount_Card SET VALD_TYPE = '001' WHERE DCID = {0}", srdc.DCID);

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

      private void SaveServDsct_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srdc = SrdcBs.Current as Data.Service_Robot_Discount_Card;
            if (srdc == null) return;

            if (srdc.DCID == 0)
            {
               iRoboTech.INS_SRDC_P(
                  new XElement("Service_Robot",
                      new XAttribute("servfileno", srdc.SRBT_SERV_FILE_NO),
                      new XAttribute("rbid", srdc.SRBT_ROBO_RBID),
                      new XElement("Discount_Card",
                          new XAttribute("offprct", srdc.OFF_PRCT ?? 0),
                          new XAttribute("offtype", srdc.OFF_TYPE ?? "008"),
                          new XAttribute("offkind", srdc.OFF_KIND ?? "004"),
                          new XAttribute("fromamnt", srdc.FROM_AMNT ?? 0),
                          new XAttribute("disccode", srdc.DISC_CODE ?? ""),
                          new XAttribute("maxamntoff", srdc.MAX_AMNT_OFF ?? 0),
                          new XAttribute("exprdate", srdc.EXPR_DATE ?? DateTime.Now.AddYears(1))
                      )
                  )
               );
            }
            else
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
               Execute_Query();
         }
      }

      private void UpdtWalt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iRoboTech.ExecuteCommand("UPDATE Wallet_Detail SET CONF_STAT = CONF_STAT WHERE CONF_STAT IN ( '001', '003' );");

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

      private void AddSler_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            if (SrbsBs.List.OfType<Data.Service_Robot_Seller>().Any(s => s.Service_Robot == srbt)) return;

            var srbs = SrbsBs.AddNew() as Data.Service_Robot_Seller;
            srbs.Service_Robot = srbt;
            srbs.CONF_STAT = "002";
            srbs.CONF_DATE = DateTime.Now;

            // attach record to saving
            iRoboTech.Service_Robot_Sellers.InsertOnSubmit(srbs);

            // Save Record and Commit on database
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
               Execute_Query();
         }
      }

      private void AddSrsp_Butn_Click(object sender, EventArgs e)
      {
         var srbs = SrbsBs.Current as Data.Service_Robot_Seller;
         if (srbs == null) return;

         if (SrspBs.List.OfType<Data.Service_Robot_Seller_Product>().Any(sp => sp.CODE == 0)) return;

         var srsp = SrspBs.AddNew() as Data.Service_Robot_Seller_Product;
         srsp.Service_Robot_Seller = srbs;

         iRoboTech.Service_Robot_Seller_Products.InsertOnSubmit(srsp);
      }

      private void DelSrsp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srsp = SrspBs.Current as Data.Service_Robot_Seller_Product;
            if (srsp == null) return;

            if (MessageBox.Show(this, "آیا با حذف کالا از گروه تامین کننده موافق هستید؟", "حذف کالا از گروه تامین کننده", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            // حذف ردیف از جدول
            iRoboTech.Service_Robot_Seller_Products.DeleteOnSubmit(srsp);

            // ذخیره کردن اطلاعات درون پایگاه داده
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
               Execute_Query();
         }
      }

      private void SaveSrsp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Srsp_Gv.PostEditor();

            // ثبت اطلاعات درون پایگاه داده
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
               Execute_Query();
         }
      }

      private void SaveSrbs_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Srbs_Gv.PostEditor();

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
               Execute_Query();
         }
      }

      private void ShowGoogleMap_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 31 /* Execute DoWork4GMapNets */, SendType.Self)
               {
                  Input =
                     new XElement("GMapNets",
                        new XAttribute("requesttype", "get"),
                        new XAttribute("formcaller", "Program:RoboTech:" + GetType().Name),
                        new XAttribute("callback", 40 /* CordinateGetSet */),
                        new XAttribute("outputtype", "srbspostadrs"),
                        new XAttribute("initalset", true),
                        new XAttribute("cordx", robo.CORD_X == null ? "29.610420210528" : robo.CORD_X.ToString()),
                        new XAttribute("cordy", robo.CORD_Y == null ? "52.5152599811554" : robo.CORD_Y.ToString()),
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

      private void Scnt_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;

            switch (e.Button.Index)
            {
               case 0:
                  if(srbt.NATL_CODE != null && srbt.NATL_CODE.Length == 10)
                     Process.Start(string.Format("https://web.whatsapp.com/send?phone=98{0}", srbt.CELL_PHON.Substring(1)));
                  break;
               case 1:
                  Process.Start(string.Format("https://web.bale.ai/#/im/u{0}", srbt.CHAT_ID));
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SaveRwrdAmnt_Butn_Click(object sender, EventArgs e)
      {
         long? chatid = 0, rbid = 0;
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;
            rbid = robo.RBID;

            var srbt = SrbtBs.Current as Data.Service_Robot;
            if (srbt == null) return;
            chatid = srbt.CHAT_ID;

            if (!WletType02_Flb.SelectedIndex.In(0, 1)) { MessageBox.Show("لطفا نوع کیف پول را مشخص کنید"); WletType02_Flb.Focus(); return; }
            if (TotlAmntRwrd_Txt.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا مبلغ کل را وارد کنید"); TotlAmntRwrd_Txt.Focus(); return; }
            if (RwrdAmnt_Txt.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا مبلغ پاداش را وارد کنید"); RwrdAmnt_Txt.Focus(); return; }
            //if (PrctAmnt_Txt.Text.ToInt64() > 0) { MessageBox.Show("لطفا مبلغ پاداش را وارد کنید"); PrctAmnt_Txt.Focus(); return; }
            if (TarfCode_Lov.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا نوع پاداش رو مشخص کنید"); RwrdAmnt_Txt.Focus(); return; }
            if (IntrSrbt_Lov.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا واسطه پاداش رو مشخص کنید"); IntrSrbt_Lov.Focus(); return; }
            if (RwrdAmntDesc_Txt.Text == "") { MessageBox.Show("لطفا توضیحات پاداش رو مشخص کنید"); RwrdAmntDesc_Txt.Focus(); return; }

            iRoboTech.SAVE_RWRD_P(
               new XElement("Reward",
                   new XAttribute("rbid", robo.RBID),
                   new XAttribute("type", "001"),
                   new XAttribute("wlettype", WletType02_Flb.SelectedIndex == 0 ? "002" : "001"),
                   new XAttribute("totlamnt", TotlAmntRwrd_Txt.EditValue ?? 0),
                   new XAttribute("amnt", RwrdAmnt_Txt.EditValue ?? 0),
                   new XAttribute("prctamnt", PrctAmnt_Txt.EditValue ?? 0),
                   new XAttribute("confday", ConfDay_Pbtn.PickChecked ? 0 : ConfDay_Spn.Value),
                   new XAttribute("tarfcode", TarfCode_Lov.EditValue),
                   new XAttribute("chatid", srbt.CHAT_ID),
                   new XAttribute("intrchatid", IntrSrbt_Lov.EditValue),
                   new XAttribute("desc", RwrdAmntDesc_Txt.Text)
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
            {
               Execute_Query();
               #region Send Message
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
                                 new XAttribute("actntype", "sendordrs"),
                                 new XAttribute("chatid", chatid),
                                 new XAttribute("rbid", rbid)
                              )
                        }                     
                     }
                  )
               );
               #endregion
            }
         }
      }

      private void TotlAmntRwrd_Txt_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         if (CalcPrctRwrdAmntLock_Pkb.PickChecked)
         {
            RwrdAmnt_Txt.EditValue = (Convert.ToInt64(e.NewValue) * PrctAmnt_Txt.Text.ToInt64() / 100);
         }
      }

      private void AddStakHldr_Butn_Click(object sender, EventArgs e)
      {
         var robo = RoboBs.Current as Data.Robot;
         if (robo == null) return;

         if (SrshBs.List.OfType<Data.Service_Robot_Stakeholder>().Any(s => s.CODE == 0)) return;

         var srsh = SrshBs.AddNew() as Data.Service_Robot_Stakeholder;
         srsh.Robot = robo;
         iRoboTech.Service_Robot_Stakeholders.InsertOnSubmit(srsh);
      }

      private void DelStakHldr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var srsh = SrshBs.Current as Data.Service_Robot_Stakeholder;
            if (srsh == null) return;

            if (MessageBox.Show(this, "آیا با حذف فرد سهامدار موافق هستید؟", "حذف سهامدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            iRoboTech.Service_Robot_Stakeholders.DeleteOnSubmit(srsh);

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
               Execute_Query();
         }
      }

      private void SaveStakHldr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            StakHldr_Gv.PostEditor();
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
               Execute_Query();
         }
      }

      private void CalcStakHldr_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            if (TotlAmntRwrd1_Txt.EditValue.ToString() == "" || TotlAmntRwrd1_Txt.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا مبلغ کل را وارد کنید"); TotlAmntRwrd1_Txt.Focus(); return; }
            //if (RwrdAmnt1_Txt.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا مبلغ پاداش را وارد کنید"); RwrdAmnt1_Txt.Focus(); return; }
            //if (PrctAmnt_Txt.Text.ToInt64() > 0) { MessageBox.Show("لطفا مبلغ پاداش را وارد کنید"); PrctAmnt_Txt.Focus(); return; }
            if (TarfCode1_Lov.EditValue.ToString() == "" || TarfCode1_Lov.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا نوع سود سهامداری را مشخص کنید"); TarfCode1_Lov.Focus(); return; }
            if (IntrSrbt1_Lov.EditValue.ToString() == "" || IntrSrbt1_Lov.EditValue.ToString().ToInt64() == 0) { MessageBox.Show("لطفا واسطه سود سهامداری را مشخص کنید"); IntrSrbt1_Lov.Focus(); return; }
            if (RwrdAmntDesc1_Txt.Text == "") { MessageBox.Show("لطفا توضیحات سود سهامداری را مشخص کنید"); RwrdAmntDesc1_Txt.Focus(); return; }

            CalcStakHldr_Butn.Enabled = false;

            foreach (var stakhldr in SrshBs.List.OfType<Data.Service_Robot_Stakeholder>().Where(s => s.STAT == "002"))
            {
               iRoboTech.SAVE_RWRD_P(
                  new XElement("Reward",
                      new XAttribute("rbid", robo.RBID),
                      new XAttribute("type", "001"),
                      new XAttribute("wlettype", "002"),
                      new XAttribute("totlamnt", TotlAmntRwrd1_Txt.EditValue ?? 0),
                      new XAttribute("amnt", ((TotlAmntRwrd1_Txt.EditValue.ToString().ToInt64() * stakhldr.PRCT_VALU) / 100)),
                      new XAttribute("prctamnt", stakhldr.PRCT_VALU),
                      new XAttribute("confday", 0),
                      new XAttribute("tarfcode", TarfCode1_Lov.EditValue),
                      new XAttribute("chatid", stakhldr.CHAT_ID),
                      new XAttribute("intrchatid", IntrSrbt1_Lov.EditValue),
                      new XAttribute("desc", RwrdAmntDesc1_Txt.Text)
                  )
               );

               #region Send Message
               // فراخوانی ربات برای ارسال پیام ثبت شده
               if (iRoboTech.V_URLFGAs.Any(host => host.HOST_NAME == HostNameInfo.Attribute("cpu").Value))
               {
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
                                 new XAttribute("actntype", "sendordrs"),
                                 new XAttribute("chatid", stakhldr.CHAT_ID),
                                 new XAttribute("rbid", robo.RBID),
                                 HostNameInfo
                              )
                        }
                     }
                     )
                  );
               }
               #endregion
            }

            CalcStakHldr_Butn.Enabled = true;
         }
         catch (Exception exc)
         {
            CalcStakHldr_Butn.Enabled = true;
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Amnt013_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _sender = sender as MaxUi.Button;
            if (_sender == null) return;

            var _robot = RoboBs.Current as Data.Robot;
            var _admin = WletBs.Current as Data.Wallet;

            // Check Admin Wallet
            if(!(_admin.WLET_TYPE == "001" /* کیف پول اعتباری */ && _admin.Service_Robot.Service_Robot_Groups.Any(a => a.GROP_GPID == 131 && a.STAT == "002" && a.Group.ADMN_ORGN == "002" && a.Group.STAT == "002")))
            {
               if(MessageBox.Show(this, "آدرس کیف پولی را که انتخاب کرده اید متعلق به مدیریت نیست! آیا مایل به انتخاب کیف مدیریت هستید؟", "هشدار کیف پول", MessageBoxButtons.YesNo) != DialogResult.Yes)
                  throw new Exception("آدرس کیف پولی را که انتخاب کرده اید متعلق به مدیریت نیست");
               else
               {
                  _admin = 
                     WletBs.List.OfType<Data.Wallet>()
                     .FirstOrDefault(w => w.WLET_TYPE == "001" /* کیف پول اعتباری */ &&
                        w.Service_Robot.Service_Robot_Groups.Any(srg => srg.GROP_GPID == 131 && srg.STAT == "002" && srg.Group.STAT == "002" && srg.Group.ADMN_ORGN == "002")
                     );

                  if(_admin == null)
                     throw new Exception("آدرس کیف پولی را که انتخاب کرده اید متعلق به مدیریت نیست");
               }
            }

            XElement _param = null;
            Data.Order _ordr = null;

            switch (_sender.Tag.ToString())
            {
               case "0":
                  _ordr = 
                     iRoboTech.Orders
                     .FirstOrDefault(o => 
                        o.ORDR_TYPE == "013" && 
                        o.ORDR_STAT == "001" && 
                        o.CHAT_ID == _admin.CHAT_ID                         
                     );

                  if(_ordr == null)
                     throw new Exception("درخواستی ذخیره نشده");

                  _param =
                     new XElement("Robot", 
                         new XAttribute("token", _robot.TKON_CODE),
                         new XElement("Message",
                             new XAttribute("cbq", "002"),
                             new XAttribute("ussd", "*6*0*3#"),
                             new XAttribute("chatid", _admin.CHAT_ID),
                             new XElement("Text", 
                                 new XAttribute("param", _ordr.CODE),
                                 new XAttribute("postexec", "lessaddwlet"),
                                 new XAttribute("trigger", ""),
                                 "emptyamntwlet"
                             )
                         )
                     );
                  break;
               case "500K":
                  _param =
                     new XElement("Robot",
                         new XAttribute("token", _robot.TKON_CODE),
                         new XElement("Message",
                             new XAttribute("cbq", "002"),
                             new XAttribute("ussd", "*6*0*3#"),
                             new XAttribute("chatid", _admin.CHAT_ID),
                             new XElement("Text",
                                 new XAttribute("param", "howinccreditwlet,5000000"),
                                 new XAttribute("postexec", "lessaddwlet"),
                                 new XAttribute("trigger", ""),
                                 "addamntwlet"
                             )
                         )
                     );
                  break;
               case "1M":
                  _param =
                     new XElement("Robot",
                         new XAttribute("token", _robot.TKON_CODE),
                         new XElement("Message",
                             new XAttribute("cbq", "002"),
                             new XAttribute("ussd", "*6*0*3#"),
                             new XAttribute("chatid", _admin.CHAT_ID),
                             new XElement("Text",
                                 new XAttribute("param", "howinccreditwlet,10000000"),
                                 new XAttribute("postexec", "lessaddwlet"),
                                 new XAttribute("trigger", ""),
                                 "addamntwlet"
                             )
                         )
                     );
                  break;
               case "2M":
                  _param =
                     new XElement("Robot",
                         new XAttribute("token", _robot.TKON_CODE),
                         new XElement("Message",
                             new XAttribute("cbq", "002"),
                             new XAttribute("ussd", "*6*0*3#"),
                             new XAttribute("chatid", _admin.CHAT_ID),
                             new XElement("Text",
                                 new XAttribute("param", "howinccreditwlet,20000000"),
                                 new XAttribute("postexec", "lessaddwlet"),
                                 new XAttribute("trigger", ""),
                                 "addamntwlet"
                             )
                         )
                     );
                  break;
               case "3M":
                  _param =
                     new XElement("Robot",
                         new XAttribute("token", _robot.TKON_CODE),
                         new XElement("Message",
                             new XAttribute("cbq", "002"),
                             new XAttribute("ussd", "*6*0*3#"),
                             new XAttribute("chatid", _admin.CHAT_ID),
                             new XElement("Text",
                                 new XAttribute("param", "howinccreditwlet,30000000"),
                                 new XAttribute("postexec", "lessaddwlet"),
                                 new XAttribute("trigger", ""),
                                 "addamntwlet"
                             )
                         )
                     );
                  break;
            }

            XElement _result = null;
            iRoboTech.Analisis_Message_P(_param, ref _result);
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

      private void Confirm013_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _ordr = O13sBs.Current as Data.Order;
            if (_ordr == null) return;

            if (Txid013_Txt.Text == "" || Txid013_Txt.Text.Length < 4) { Txid013_Txt.Focus(); return; }
            if (Pwd013_Txt.Text != PasswordHash + SaltKey + VIKey) { Pwd013_Txt.Focus(); return; }

            XElement _result = null;

            iRoboTech.SAVE_PYMT_P(
               new XElement("Payment",
                   new XAttribute("ordrcode", _ordr.CODE),
                   new XAttribute("txid", Txid013_Txt.Text),
                   new XAttribute("totlamnt", _ordr.SUM_EXPN_AMNT_DNRM),
                   new XAttribute("rcptmtod", "009"),
                   new XAttribute("autochngamnt", "002")
               ),
               ref _result
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
   }
}
