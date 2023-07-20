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
using System.Threading;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using System.Data.Entity.SqlServer;
using System.Net;
using System.IO;

namespace System.RoboTech.Ui.MasterPage
{
   public partial class FRST_PAGE_F : UserControl
   {
      public FRST_PAGE_F()
      {
         InitializeComponent();
      }

      private bool frstLoad = false;
      private int instagramTimerInterval = 1 * 60 * 1000;
      private bool instagramOperationStatus = false;
      private DateTime instagramTriggerTime = DateTime.Now;
      private bool jobAtService = true;
      private enum DoOprtInsta
      {
         DirectMessage,
         FollowNewUser
      }
      private DoOprtInsta _doOprtInsta = DoOprtInsta.FollowNewUser;
      private bool _CurrencyCalcStatus = false;
      private bool _CurrencyAutoUpdate = false;
      private int _CurrencyTimerInterval = 1 * 60 * 1000;
      private DateTime _CurrencyTriggerTime = DateTime.Now;

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

         if (jobAtService && instagramOperationStatus && instagramTriggerTime.AddMilliseconds(instagramTimerInterval) <= DateTime.Now)
         {
            // We Must Run Trigger "InstDoOprt"
            instagramTriggerTime = DateTime.Now;
            jobAtService = false;
            InstDoOprtAsync();
            jobAtService = true;
         }

         if (jobAtService && _CurrencyCalcStatus && _CurrencyTriggerTime.AddMilliseconds(_CurrencyTimerInterval) <= DateTime.Now)
         {
            _CurrencyTriggerTime = DateTime.Now;
            jobAtService = false;
            CrncDoOprtAsync();
            jobAtService = true;
         }
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

      private void OrderAction_Recipt()
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

                     // اگر سیستم اطلاع رسانی فعال باشد
                     if (ordrship.FirstOrDefault().Robot.NOTI_ORDR_SHIP_STAT == "002")
                        new Thread(new ParameterizedThreadStart(PlaySound)).Start(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_SHIP_PATH);
                        //PlaySound(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_SHIP_PATH);

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

                     // 1399/09/08 * اطلاعات ارسال پیام به حسابداران رو ثبت میکنیم و به انها پیام میدهیم
                     var xRet = new XElement("Respons");
                     // ارسال پیام به واحد حسابداری جهت بررسی رسید های پرداخت شده                     
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
                                       new XAttribute("actntype", "pokejobpersonel"),
                                       new XAttribute("rbid", ordrrcpt.FirstOrDefault().Order.SRBT_ROBO_RBID),
                                       new XAttribute("chatid", 0),
                                       new XAttribute("oprttype", "poke4ordrrcpt")
                                    )
                              }                     
                           }
                        )
                     );
                     #endregion

                     // اگر سیستم اطلاع رسانی فعال باشد
                     if (ordrrcpt.FirstOrDefault().Order.Robot.NOTI_ORDR_RCPT_STAT == "002")
                        new Thread(new ParameterizedThreadStart(PlaySound)).Start(ordrrcpt.FirstOrDefault().Order.Robot.NOTI_SOND_ORDR_RCPT_PATH);
                        //PlaySound(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_RCPT_PATH);

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

                  #region Check New Online Reception Order
                  var ordr25 =
                     iRoboTech.Orders.Where(o => o.ORDR_TYPE == "025" && (o.ORDR_STAT == "002" || o.ORDR_STAT == "016")).ToList();

                  if(ordr25.Any())
                  {
                     NotfOrdrOnlineAdmission_Butn.Visible = true;
                     NotfOrdrOnlineAdmission_Butn.Caption = ordr25.Count().ToString();
                     OrderOnlineAdmission_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;

                     // اگر سیستم اطلاع رسانی فعال باشد
                     if (ordr25.FirstOrDefault().Robot.NOTI_ORDR_RECP_STAT == "002")
                        new Thread(new ParameterizedThreadStart(PlaySound)).Start(ordr25.FirstOrDefault().Robot.NOTI_SOND_ORDR_RECP_PATH);
                        //PlaySound(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_RECP_PATH);

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        {
                           Input =
                              new List<object>
                              {
                                 ToolTipIcon.Info,
                                 "پذیرش سفارش آنلاین ارسالی",
                                 string.Format("تعداد {0} عدد درخواست در صف انتظار میباشد", ordr25.Count()),
                                 2000
                              },
                           Executive = ExecutiveType.Asynchronous
                        }
                     );
                  }
                  else
                  {
                     NotfOrdrOnlineAdmission_Butn.Visible = false;
                     OrderOnlineAdmission_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
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

                  // اگر سیستم اطلاع رسانی فعال باشد
                  if (ordrship.FirstOrDefault().Robot.NOTI_ORDR_SHIP_STAT == "002")
                     new Thread(new ParameterizedThreadStart(PlaySound)).Start(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_SHIP_PATH);
                     //PlaySound(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_SHIP_PATH);

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

                  // 1399/09/08 * اطلاعات ارسال پیام به حسابداران رو ثبت میکنیم و به انها پیام میدهیم
                  var xRet = new XElement("Respons");
                  // ارسال پیام به واحد حسابداری جهت بررسی رسید های پرداخت شده                     
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
                                       new XAttribute("actntype", "pokejobpersonel"),
                                       new XAttribute("rbid", ordrrcpt.FirstOrDefault().Order.SRBT_ROBO_RBID),
                                       new XAttribute("chatid", 0),
                                       new XAttribute("oprttype", "poke4ordrrcpt")
                                    )
                              }                     
                           }
                     )
                  );
                  #endregion

                  // اگر سیستم اطلاع رسانی فعال باشد
                  if (ordrrcpt.FirstOrDefault().Order.Robot.NOTI_ORDR_RCPT_STAT == "002")
                     new Thread(new ParameterizedThreadStart(PlaySound)).Start(ordrrcpt.FirstOrDefault().Order.Robot.NOTI_SOND_ORDR_RCPT_PATH);
                     //PlaySound(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_RCPT_PATH);

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

               #region Check New Online Reception Order
               var ordr25 =
                  iRoboTech.Orders.Where(o => o.ORDR_TYPE == "025" && (o.ORDR_STAT == "002" || o.ORDR_STAT == "016")).ToList();

               if (ordr25.Any())
               {
                  NotfOrdrOnlineAdmission_Butn.Visible = true;
                  NotfOrdrOnlineAdmission_Butn.Caption = ordr25.Count().ToString();
                  OrderOnlineAdmission_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleRight;

                  // اگر سیستم اطلاع رسانی فعال باشد
                  if (ordr25.FirstOrDefault().Robot.NOTI_ORDR_RECP_STAT == "002")
                     new Thread(new ParameterizedThreadStart(PlaySound)).Start(ordr25.FirstOrDefault().Robot.NOTI_SOND_ORDR_RECP_PATH);
                     //PlaySound(ordrship.FirstOrDefault().Robot.NOTI_SOND_ORDR_RECP_PATH);

                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                              {
                                 ToolTipIcon.Info,
                                 "پذیرش سفارش آنلاین ارسالی",
                                 string.Format("تعداد {0} عدد درخواست در صف انتظار میباشد", ordr25.Count()),
                                 2000
                              },
                        Executive = ExecutiveType.Asynchronous
                     }
                  );
               }
               else
               {
                  NotfOrdrOnlineAdmission_Butn.Visible = false;
                  OrderOnlineAdmission_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
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

      private void OnlnRecpOrdrRobo_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 28 /* Execute Onro_Dvlp_F */),
                new Job(SendType.SelfToUserInterface, "ONRO_DVLP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void OrderOnlineAdmission_Butn_Click(object sender, EventArgs e)
      {
         NotfOrdrOnlineAdmission_Butn.Visible = false;
         OrderOnlineAdmission_Butn.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;

         OnlnRecpOrdrRobo_Butn_Click(null, null);
      }

      WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();
      private void PlaySound(object path)
      {
         if (InvokeRequired)
         {
            try
            {
               wplayer.URL = path.ToString();
               wplayer.controls.play();
            }
            catch { }
         }
      }

      private void InstPageConf_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 29 /* Execute Inst_Conf_F */),
                new Job(SendType.SelfToUserInterface, "INST_CONF_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void InstagramOperationInit()
      {
         try
         {
            var inst = iRoboTech.Robot_Instagrams.FirstOrDefault(i => i.PAGE_OWNR_TYPE == "002" && i.STAT == "002");
            if (inst == null) return;

            instagramTimerInterval = (int)(inst.CYCL_INTR * 60 * 1000 ?? 5 * 60 * 1000);
            instagramOperationStatus = inst.CYCL_STAT == "002" ? true : false;

            if (inst.CYCL_STAT == "002")
            {
               InstagramOperationStatus_Rb.NormalColorA = InstagramOperationStatus_Rb.NormalColorB = Color.Lime;
               if (InvokeRequired)
                  Invoke(new System.Action(() =>
                  {
                     var _count = inst.Robot_Instagram_DirectMessages.Where(d => d.SEND_STAT == "005").Count();
                     NotfInstagramOperation_Butn.Caption = _count.ToString();
                     NotfInstagramOperation_Butn.Visible = (_count > 0);
                  }));
               else
               {
                  var _count = inst.Robot_Instagram_DirectMessages.Where(d => d.SEND_STAT == "005").Count();
                  NotfInstagramOperation_Butn.Caption = _count.ToString();
                  NotfInstagramOperation_Butn.Visible = (_count > 0);
               }
            }
            else
            {
               InstagramOperationStatus_Rb.NormalColorA = InstagramOperationStatus_Rb.NormalColorB = Color.Red;
               if (InvokeRequired)
                  Invoke(new System.Action(() =>
                  {
                     NotfInstagramOperation_Butn.Visible = false;
                     NotfInstagramOperation_Butn.Caption = "0";
                  }));
               else
               {
                  NotfInstagramOperation_Butn.Visible = false;
                  NotfInstagramOperation_Butn.Caption = "0";
               }
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void InstDoOprtAsync()
      {
         try
         {            
            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
            var inst = iRoboTech.Robot_Instagrams.FirstOrDefault(i => i.PAGE_OWNR_TYPE == "002" && i.STAT == "002" && i.CYCL_STAT == "002");
            // اگر هیچ پیج اینستاگرامی وجود نداشته باشد
            if (inst == null) return;
            var _instagram = new Controller.Instagram(iRoboTech);

            if (_doOprtInsta == DoOprtInsta.DirectMessage)
            {
               #region Direction Message
               var dirMsgs = inst.Robot_Instagram_DirectMessages.Where(d => d.SEND_STAT == "005").OrderBy(d => d.CRET_DATE);

               // Refresh Control
               InstagramOperationStatus_Rb.NormalColorA = InstagramOperationStatus_Rb.NormalColorB = Color.Lime;
               if (InvokeRequired)
                  Invoke(new System.Action(() => { NotfInstagramOperation_Butn.Caption = dirMsgs.Count().ToString(); }));
               else
                  NotfInstagramOperation_Butn.Caption = dirMsgs.Count().ToString();

               foreach (var d in dirMsgs.Take((int)inst.CYCL_SEND_MESG_NUMB))
               {
                  BkgrOprt_Mpb.Visible = true;
                  // Send Message
                  var result = await _instagram.SendDirectMessageAsync(d);
                  if (result)
                  {
                     d.SEND_STAT = "004";
                     // Refresh Control
                     if (InvokeRequired)
                        Invoke(new System.Action(() => { NotfInstagramOperation_Butn.Caption = (NotfInstagramOperation_Butn.Caption.ToInt32() - 1).ToString(); }));
                     else
                        NotfInstagramOperation_Butn.Caption = (NotfInstagramOperation_Butn.Caption.ToInt32() - 1).ToString();
                  }
                  // Sleep
                  Thread.Sleep((int)(inst.CYCL_ACTN_SLEP ?? 10) * 1000);
               }
               #endregion

               // Next Cycle Step
               _doOprtInsta = DoOprtInsta.FollowNewUser;
            }
            else if (_doOprtInsta == DoOprtInsta.FollowNewUser)
            {
               #region New Follow
               var _newFollowUser = 
                  iRoboTech.Robot_Instagram_Follows
                  .Where(f => 
                     f.Robot_Instagram.PAGE_OWNR_TYPE == "001" && 
                     !iRoboTech.Robot_Instagrams
                     .Any(i => 
                        i.PAGE_OWNR_TYPE == "002" && 
                        i.STAT == "002" && 
                        i.CYCL_STAT == "002" && 
                        i.Robot_Instagram_Follows.Any(rif => rif.INST_PKID == f.INST_PKID)
                     )
                  )
                  //.ToList()
                  //.OrderBy(f => SqlFunctions.Rand())
                  .FirstOrDefault();

               if (_newFollowUser != null)
               {
                  BkgrOprt_Mpb.Visible = true;
                  // Send Message
                  var result = await _instagram.FolowNewUserAsync(_newFollowUser);
                  if (result)
                  {
                     // بررسی میکنیم که آیا پیام خودکار ارسال دایرکت به مشتری جدید داریم یا خیر
                     var _newFollowAutoDirectMessage = iRoboTech.Templates.FirstOrDefault(t => t.TMID == inst.CYCL_NFLW_TMPL_TMID);
                     if (_newFollowAutoDirectMessage != null)
                     {
                        // ثبت ارسال پیام جدید به مشتری که تازه فالو شده
                        iRoboTech.INS_RIDM_P(
                           new XElement("Instagram",
                               new XAttribute("code", inst.CODE),
                               new XAttribute("rbid", inst.ROBO_RBID),
                               new XElement("Direct",
                                   new XElement("Message", _newFollowAutoDirectMessage.TEMP_TEXT),
                                   new XElement("Users",
                                       new XElement("User",
                                           new XAttribute("pkid", _newFollowUser.INST_PKID),
                                           new XAttribute("chatid", _newFollowUser.CHAT_ID ?? 0)
                                       )
                                   )
                               )
                           )
                        );
                     }
                  }
               }
               BkgrOprt_Mpb.Visible = false;
               #endregion

               // Next Cycle Step
               _doOprtInsta = DoOprtInsta.DirectMessage;
            }

            BkgrOprt_Mpb.Visible = false;

            iRoboTech.SubmitChanges();
         }
         catch { }
      }

      private void CurrencyOperationInit()
      {
         try
         {
            var robo = iRoboTech.Robots.FirstOrDefault(r => Fga_Ugov_U.Contains(r.ORGN_OGID) && r.STAT == "002" && r.RUN_STAT == "002");
            if (robo == null) return;

            _CurrencyCalcStatus = robo.CRNC_CALC_STAT == "002" ? true : false;
            _CurrencyAutoUpdate = robo.CRNC_AUTO_UPDT_STAT == "002" ? true : false;
            _CurrencyTimerInterval = (int)robo.CRNC_CYCL_AUTO_UPDT * 1000;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private async void CrncDoOprtAsync()
      {
         try
         {
            var robo = iRoboTech.Robots.FirstOrDefault(r => Fga_Ugov_U.Contains(r.ORGN_OGID) && r.STAT == "002" && r.RUN_STAT == "002");
            if (robo == null) return;

            BkgrOprt_Mpb.Visible = true;
            var admin =
               iRoboTech.Service_Robots
               .FirstOrDefault(sr => sr.Service_Robot_Groups.Any(g => g.STAT == "002" && g.GROP_GPID == 131));

            foreach (var cs in robo.Robot_Currency_Sources.Where(cs => cs.STAT == "002"))
            {
               if(cs.TYPE == "001")
               { 
                  string urlAddress = cs.WEB_SITE;

                  HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
                  var response = (HttpWebResponse)(await request.GetResponseAsync());

                  if (response.StatusCode == HttpStatusCode.OK)
                  {
                     Stream receiveStream = response.GetResponseStream();
                     StreamReader readStream = null;

                     if (String.IsNullOrWhiteSpace(response.CharacterSet))
                        readStream = new StreamReader(receiveStream);
                     else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                     var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                     htmlDoc.LoadHtml(readStream.ReadToEnd());

                     var htmlBody = htmlDoc.DocumentNode.SelectNodes("//table/tbody/tr");

                     iRoboTech.GET_CSOR_P(
                        new XElement("Robot_Currency_Source",
                            new XAttribute("code", cs.CODE),
                            new XElement("Currencies",
                                htmlBody.Take(32)
                                .Select(c =>
                                    new XElement("Currency",
                                        new XAttribute("data", c.InnerText.Replace("\n", "#"))
                                    )
                                )

                            )
                        )
                     );

                     response.Close();
                     readStream.Close();
                  }
               }
            }
            BkgrOprt_Mpb.Visible = false;

            // Poke Bot For Send Message To Service
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
                              new XAttribute("chatid", admin.CHAT_ID),
                              new XAttribute("rbid", robo.RBID)
                           )
                     }                     
                  }
               )
            );
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AskQstnTlgm_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Diagnostics.Process.Start("https://www.telegram.me/mr_shop_iran");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AskQstnWats_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=989033927103&text=سلام خوب هستین، من یه سوال داشتم میشه بهم کمک کنید");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AskQstnInst_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Diagnostics.Process.Start("https://www.instagram.com/mr.shop.iran");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void AskQstnBale_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            Diagnostics.Process.Start("https://ble.ir/mr_shop_iran");
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void SendMesgConf_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 30 /* Execute Inst_Conf_F */),
                new Job(SendType.SelfToUserInterface, "MESG_DVLP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void CashCntr_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 31 /* Execute Cash_Cntr_F */),
                new Job(SendType.SelfToUserInterface, "CASH_CNTR_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void BaseDefAcnt_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 32 /* Execute Tree_Base_F */),
                new Job(SendType.SelfToUserInterface, "TREE_BASE_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void InvcOprt_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {
                new Job(SendType.Self, 33 /* Execute Invc_Oprt_F */),
                new Job(SendType.SelfToUserInterface, "INVC_OPRT_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }

      private void MangInsp_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
              new List<Job>
              {
                new Job(SendType.Self, 34 /* Execute Mngr_Insp_P */),
                new Job(SendType.SelfToUserInterface, "MNGR_INSP_F", 10 /* Execute Actn_CalF_P */)
              })
         );
      }
   }
}
