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
   }
}
