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

      private void Execute_Query()
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         OrdrBs.DataSource =
            iRoboTech.Orders.Where(o => o.Robot.Organ.STAT == "002" && o.Robot.STAT == "002" && o.ORDR_TYPE == "004" && o.ORDR_STAT == "004");

         if (OrdrBs.List.OfType<Data.Order>().Any(o => o.HOW_SHIP == "000"))
            OrdrShip000Bs.DataSource = OrdrBs.List.OfType<Data.Order>().Where(o => o.HOW_SHIP == "000");
         else
            OrdrShip000Bs.List.Clear();

         if (OrdrBs.List.OfType<Data.Order>().Any(o => o.HOW_SHIP == "001"))
            OrdrShip001Bs.DataSource = OrdrBs.List.OfType<Data.Order>().Where(o => o.HOW_SHIP == "001");
         else
            OrdrShip001Bs.List.Clear();

         if (OrdrBs.List.OfType<Data.Order>().Any(o => o.HOW_SHIP == "002"))
            OrdrShip002Bs.DataSource = OrdrBs.List.OfType<Data.Order>().Where(o => o.HOW_SHIP == "002");
         else
            OrdrShip002Bs.List.Clear();

         if (OrdrBs.List.OfType<Data.Order>().Any(o => o.HOW_SHIP == "003"))
            OrdrShip003Bs.DataSource = OrdrBs.List.OfType<Data.Order>().Where(o => o.HOW_SHIP == "003");
         else
            OrdrShip003Bs.List.Clear();

         requery = false;
      }      
   }
}
