﻿using System;
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
   public partial class WLET_DVLP_F : UserControl
   {
      public WLET_DVLP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

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

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            WletBs.DataSource =
               iRoboTech.Wallets.Where(w => w.Robot == robo && w.Service_Robot.NATL_CODE != null);

            SrbtBs.DataSource =
               iRoboTech.Service_Robots.Where(sr => sr.Robot == robo && sr.NATL_CODE != null && sr.NATL_CODE != "" && sr.NATL_CODE.Length == 10);

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
   }
}
