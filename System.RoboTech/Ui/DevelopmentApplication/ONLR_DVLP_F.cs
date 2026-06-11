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
   public partial class ONLR_DVLP_F : UserControl
   {
      public ONLR_DVLP_F()
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
         int orgn = OrgnBs.Position;
         int robo = RoboBs.Position;

         var result = await Task.Run(() =>
         {
            var db = new Data.iRoboTechDataContext(ConnectionString);
            var organs = db.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID)).ToList();
            return new { organs };
         });

         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         OrgnBs.DataSource = result.organs;
         OrgnBs.Position = orgn;
         RoboBs.Position = robo;

         requery = false;
      }
   }
}
