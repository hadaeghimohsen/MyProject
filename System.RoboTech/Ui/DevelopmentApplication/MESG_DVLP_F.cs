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
using System.IO;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class MESG_DVLP_F : UserControl
   {
      public MESG_DVLP_F()
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
         int srbt = SrbtBs.Position;

         OrgnBs.DataSource = iRoboTech.Organs.Where(o => Fga_Ugov_U.Contains(o.OGID));

         OrgnBs.Position = orgn;
         RoboBs.Position = robo;
         SrbtBs.Position = srbt;

         requery = false;
      }

      private void RoboBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var robo = RoboBs.Current as Data.Robot;
            if (robo == null) return;

            var srbt = SrbtBs.Current as Data.Service_Robot;
            if(srbt == null)return;

            switch (ServMstr_Tc.SelectedTabPage.Name)
            {
               case "Tp01_Xtp":
                  SdadASBs.DataSource = iRoboTech.Send_Advertisings.Where(sa => sa.Robot == robo && sa.Service_Robot_Send_Advertisings.Any(srsd => srsd.Service_Robot == srbt && (srsd.AMNT ?? 0) > 0));
                  break;
               case "Tp02_Xtp":
                  SdadSSBs.DataSource = iRoboTech.Send_Advertisings.Where(sa => sa.Robot == robo && !sa.Service_Robot_Send_Advertisings.Any(srsd => srsd.Service_Robot == srbt && (srsd.AMNT ?? 0) > 0));
                  break;
               case "Tp03_Xtp":
                  Ordr012Bs.DataSource = iRoboTech.Orders.Where(o => o.ORDR_TYPE == "012" && o.Service_Robot == srbt);
                  break;
               case "Tp04_Xtp":
                  break;
               case "Tp05_Xtp":
                  break;
            }
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void ServMstr_Tc_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
      {
         RoboBs_CurrentChanged(null, null);
      }

      private void SrbtBs_CurrentChanged(object sender, EventArgs e)
      {
         RoboBs_CurrentChanged(null, null);
      }
   }
}
