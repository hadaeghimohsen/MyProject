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

      private bool frstLoad = false;

      #region BaseDefinition

      #endregion

      #region DevelopmentApplication
      #endregion

      #region Action
      #endregion

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
                new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 10 /* Execute Actn_Calf_F */)
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

      private void Sale_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 21 /* Execute Sale_Dvlp_F */),
                new Job(SendType.SelfToUserInterface, "SALE_DVLP_F", 10 /* Execute Actn_CalF_F */)
              })
         );
      }

      private void OrdrShip_Butn_Click(object sender, EventArgs e)
      {
         NotfOrdrShip_Butn.Visible = false;
         OrdrShip_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 23 /* Execute Ordr_Ship_F */),
                new Job(SendType.SelfToUserInterface, "ORDR_SHIP_F", 10 /* Execute Actn_CalF_F */)
              })
         );
      }

      private void OrdrReceipt_Butn_Click(object sender, EventArgs e)
      {
         NotfOrdrReceipt_Butn.Visible = false;
         OrdrReceipt_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 24 /* Execute Ordr_Rcpt_F */),
                new Job(SendType.SelfToUserInterface, "ORDR_RCPT_F", 10 /* Execute Actn_CalF_F */)
              })
         );
      }

      private void OrderShipping_Recipt()
      {
         if (!frstLoad)
         {
            if (InvokeRequired)
               Invoke(new System.Action(() =>
               {
                  #region Check Order Ship
                  var ordrship =
                     iRoboTech.Orders.Where(o => o.Robot.Organ.STAT == "002" && o.Robot.STAT == "002" && o.ORDR_TYPE == "004" && o.ORDR_STAT == "004");

                  if (ordrship.Any())
                  {
                     NotfOrdrShip_Butn.Visible = true;
                     NotfOrdrShip_Butn.Caption = ordrship.Count().ToString();
                     OrdrShip_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        {
                           Input =
                              new List<object>
                              {
                                 ToolTipIcon.Info,
                                 "نحوه ارسال درخواست",
                                 string.Format("تعداد {0} عدد درخواست در مرحله تعیین نحوه ارسال میباشد", ordrship.Count()),
                                 2000
                              },
                              Executive = ExecutiveType.Asynchronous
                        }
                     );
                  }
                  else
                  {
                     NotfOrdrShip_Butn.Visible = false;
                     OrdrShip_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
                  }
                  #endregion

                  #region Order New Recipt
                  var ordrrcpt =
                     iRoboTech.Order_States.Where(os => os.Order.Robot.Organ.STAT == "002" && os.Order.Robot.STAT == "002" && os.Order.ORDR_STAT == "001" && os.AMNT_TYPE == "005" && os.CONF_STAT == "003");

                  if (ordrrcpt.Any())
                  {
                     NotfOrdrReceipt_Butn.Visible = true;
                     NotfOrdrReceipt_Butn.Caption = ordrrcpt.Count().ToString();
                     OrdrReceipt_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        {
                           Input =
                              new List<object>
                              {
                                 ToolTipIcon.Info,
                                 "رسید های ارسالی درخواست",
                                 string.Format("تعداد {0} عدد درخواست در مرحله تعیین تاییدیه رسید های ارسالی میباشد", ordrrcpt.Count()),
                                 2000
                              },
                              Executive = ExecutiveType.Asynchronous
                        }
                     );
                  }
                  else
                  {
                     NotfOrdrReceipt_Butn.Visible = false;
                     OrdrReceipt_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
                  }
                  #endregion

               }));
            else
            {
               #region Check Order Ship
               var ordrship =
                  iRoboTech.Orders.Where(o => o.Robot.Organ.STAT == "002" && o.Robot.STAT == "002" && o.ORDR_TYPE == "004" && o.ORDR_STAT == "004");

               if (ordrship.Any())
               {
                  NotfOrdrShip_Butn.Visible = true;
                  NotfOrdrShip_Butn.Caption = ordrship.Count().ToString();
                  OrdrShip_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                           {
                              ToolTipIcon.Info,
                              "نحوه ارسال درخواست",
                              string.Format("تعداد {0} عدد درخواست در مرحله تعیین نحوه ارسال میباشد", ordrship.Count()),
                              2000
                           },
                        Executive = ExecutiveType.Asynchronous
                     }
                  );
               }
               else
               {
                  NotfOrdrShip_Butn.Visible = false;
                  OrdrShip_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
               }
               #endregion

               #region Order New Recipt
               var ordrrcpt =
                  iRoboTech.Order_States.Where(os => os.Order.Robot.Organ.STAT == "002" && os.Order.Robot.STAT == "002" && os.Order.ORDR_STAT == "001" && os.AMNT_TYPE == "005" && os.CONF_STAT == "003");

               if (ordrrcpt.Any())
               {
                  NotfOrdrReceipt_Butn.Visible = true;
                  NotfOrdrReceipt_Butn.Caption = ordrrcpt.Count().ToString();
                  OrdrReceipt_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                           {
                              ToolTipIcon.Info,
                              "رسید های ارسالی درخواست",
                              string.Format("تعداد {0} عدد درخواست در مرحله تعیین تاییدیه رسید های ارسالی میباشد", ordrrcpt.Count()),
                              2000
                           },
                        Executive = ExecutiveType.Asynchronous
                     }
                  );
               }
               else
               {
                  NotfOrdrReceipt_Butn.Visible = false;
                  OrdrReceipt_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
               }
               #endregion
            }
            frstLoad = true;
         }
      }

      private void DefBankAcnt_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 25 /* Execute Bank_Dvlp_F */),
                new Job(SendType.SelfToUserInterface, "BANK_DVLP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void DefProdRobo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 26 /* Execute Prod_Dvlp_F */),
                new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void MangWletRobo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 27 /* Execute Wlet_Dvlp_F */),
                new Job(SendType.SelfToUserInterface, "WLET_DVLP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }
   }
}
