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

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            WletBs.DataSource =
               iRoboTech.Wallets.Where(w => w.Service_Robot.NATL_CODE != null);

            SrbtBs.DataSource =
               iRoboTech.Service_Robots.Where(sr => sr.NATL_CODE != null && sr.NATL_CODE != "" && sr.NATL_CODE.Length == 10);
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

      private void R24sBs_CurrentChanged(object sender, EventArgs e)
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

         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
