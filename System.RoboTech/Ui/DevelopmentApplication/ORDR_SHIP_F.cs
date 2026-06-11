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

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ORDR_SHIP_F : UserControl
   {
      public ORDR_SHIP_F()
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

      private async void Execute_Query()
      {
         var result = await Task.Run(() =>
         {
            var db = new Data.iRoboTechDataContext(ConnectionString);
            var ordrs = db.Orders.Where(o => o.Robot.Organ.STAT == "002" && o.Robot.STAT == "002" && o.ORDR_TYPE == "004" && o.ORDR_STAT == "004").ToList();
            return new { ordrs };
         });

         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         OrdrBs.DataSource = result.ordrs;

         if (result.ordrs.Any(o => o.HOW_SHIP == "000"))
            OrdrShip000Bs.DataSource = result.ordrs.Where(o => o.HOW_SHIP == "000").ToList();
         else
            OrdrShip000Bs.List.Clear();

         if (result.ordrs.Any(o => o.HOW_SHIP == "001"))
            OrdrShip001Bs.DataSource = result.ordrs.Where(o => o.HOW_SHIP == "001").ToList();
         else
            OrdrShip001Bs.List.Clear();

         if (result.ordrs.Any(o => o.HOW_SHIP == "002"))
            OrdrShip002Bs.DataSource = result.ordrs.Where(o => o.HOW_SHIP == "002").ToList();
         else
            OrdrShip002Bs.List.Clear();

         if (result.ordrs.Any(o => o.HOW_SHIP == "003"))
            OrdrShip003Bs.DataSource = result.ordrs.Where(o => o.HOW_SHIP == "003").ToList();
         else
            OrdrShip003Bs.List.Clear();

         requery = false;
      }      
   }
}
