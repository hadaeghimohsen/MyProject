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
using System.Globalization;

namespace System.RoboTech.Ui.MasterPage
{
   public partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();
      }

      private void AdjustDateTime_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(new Job(SendType.External, "Localhost", "Commons", 26 /* Execute DoWork4DateTimes */, SendType.Self));
      }

      private void Tm_ShowTime_Tick(object sender, EventArgs e)
      {
         PersianCalendar pc = new PersianCalendar();
         AdjustDateTime_Butn.Text = 
            string.Format("{0}\n\r{1}/{2}/{3}", 
               DateTime.Now.ToString("HH:mm:ss"),
               pc.GetYear(DateTime.Now),
               pc.GetMonth(DateTime.Now),
               pc.GetDayOfMonth(DateTime.Now));
      }

      private void RegnDfin_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 03 /* Execute Regn_Dfin_F */),                
              })
         );
      }

      private void IsicDfin_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 04 /* Execute Isic_Dfin_F */),                
              })
         );
      }

      private void OrgnRobot_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 05 /* Execute Orgn_Dfin_F */),                
              })
         );
      }

      private void RobotDev_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 06 /* Execute Orgn_Dvlp_F */),
                new Job(SendType.SelfToUserInterface, "ORGN_DVLP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void RoboDevKnlg_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 07 /* Execute Rbkn_Dvlp_F */),                
              })
         );
      }

      private void RoboService_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 08 /* Execute Rbsr_Dvlp_F */),                
              })
         );
      }

      private void ServiceRobotSendAdvertising_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 09 /* Execute Rbsa_Dvlp_F */),                
              })
         );
      }

      private void WorkGroupAccess_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 10 /* Execute Wgul_Dfin_F */),                
              })
         );
      }

      private void StrtRobo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 11 /* Execute Strt_Robo_F */),                
              })
         );
      }

      private void Order_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 12 /* Execute Rbod_Dvlp_F */),
              })
         );
      }

      private void SpyMesg_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 19 /* Execute Rbod_Dvlp_F */),
              })
         );
      }
   }
}
