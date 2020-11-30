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
using System.Diagnostics;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   public partial class ORDR_RCPT_F : UserControl
   {
      public ORDR_RCPT_F()
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
         OdstBs.DataSource =
            iRoboTech.Order_States.Where(os => os.Order.Robot.Organ.STAT == "002" && os.Order.Robot.STAT == "002" && os.Order.ORDR_STAT == "001" && os.AMNT_TYPE == "005");

         if (OdstBs.List.OfType<Data.Order_State>().Any(od => od.CONF_STAT == "003"))
            Rcpt003Bs.DataSource = OdstBs.List.OfType<Data.Order_State>().Where(od => od.CONF_STAT == "003");
         else
            Rcpt003Bs.List.Clear();

         if (OdstBs.List.OfType<Data.Order_State>().Any(od => od.CONF_STAT == "002"))
            Rcpt002Bs.DataSource = OdstBs.List.OfType<Data.Order_State>().Where(od => od.CONF_STAT == "002");
         else
            Rcpt002Bs.List.Clear();

         if (OdstBs.List.OfType<Data.Order_State>().Any(od => od.CONF_STAT == "001"))
            Rcpt001Bs.DataSource = OdstBs.List.OfType<Data.Order_State>().Where(od => od.CONF_STAT == "001");
         else
            Rcpt001Bs.List.Clear();

         requery = false;
      }

      private void AcptRcpt_Butn_Click(object sender, EventArgs e)
      {
         long? odstcode = null ;
         try
         {
            WaitRcpt_Gv.PostEditor();

            var rcpt = Rcpt003Bs.Current as Data.Order_State;
            if (rcpt == null) return;

            rcpt.CONF_STAT = "002";

            odstcode = rcpt.CODE;

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
                              new XAttribute("actntype", "sendmesg"),
                              new XAttribute("chatid", rcpt.Order.CHAT_ID),
                              new XAttribute("command", "confrcpt"),
                              new XAttribute("rbid", rcpt.Order.SRBT_ROBO_RBID),
                              new XAttribute("mesg", "✅ رسید ارسالی مورد تایید واقع شد")
                           )
                     }                     
                  }
               )
            );
            #endregion

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
            {
               Execute_Query();
               // اگر سفارش به حالت تسویه حساب برود
               var rcpt = iRoboTech.Order_States.FirstOrDefault(os => os.CODE == odstcode);
               if(rcpt.Order.DEBT_DNRM == 0)
               {
                  var xResult = new XElement("Result");
                  iRoboTech.SAVE_PYMT_P(
                     new XElement("Payment",
                        new XAttribute("ordrcode", rcpt.Order.CODE),
                        new XAttribute("dircall", "002"),
                        new XAttribute("autochngamnt", "001")
                     ),
                     ref xResult
                  );

                  if (xResult.Element("Message").Attribute("rsltcode").Value == "002")
                  {
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
                                       new XAttribute("actntype", "savepymt"),
                                       new XAttribute("chatid", rcpt.Order.CHAT_ID),
                                       new XAttribute("rbid", rcpt.Order.SRBT_ROBO_RBID),
                                       xResult
                                    )
                              }                     
                           }
                        )
                     );
                     #endregion
                  }
               }
            }
         }
      }

      private void NotAcptRcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            WaitRcpt_Gv.PostEditor();

            var crnt = Rcpt003Bs.Current as Data.Order_State;
            if (crnt == null) return;

            crnt.CONF_STAT = "001";

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
                              new XAttribute("actntype", "sendmesg"),
                              new XAttribute("chatid", crnt.Order.CHAT_ID),
                              //new XAttribute("keypad", "inline"),
                              new XAttribute("command", "confrcpt"),
                              new XAttribute("rbid", /*ordr.SRBT_ROBO_RBID*/crnt.Order.SRBT_ROBO_RBID),
                              new XAttribute("mesg", "🚫 رسید ارسالی مورد تایید واقع نشد")
                           )
                     }                     
                  }
               )
            );
            #endregion

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

      private void SendAcpt2WaitRcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = Rcpt002Bs.Current as Data.Order_State;
            if (crnt == null) return;

            crnt.CONF_STAT = "003";

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

      private void SendNotAcpt2WaitRcpt_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var crnt = Rcpt001Bs.Current as Data.Order_State;
            if (crnt == null) return;

            crnt.CONF_STAT = "003";

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

      private void Rcpt003Bs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var rcpt = Rcpt003Bs.Current as Data.Order_State;
            if (rcpt == null) return;

            PrbtBs.DataSource =
               iRoboTech.Personal_Robots
               .Where(pr => pr.Robot == rcpt.Order.Robot && pr.Personal_Robot_Jobs.Any(prj => prj.Job.ORDR_TYPE == "025" /* مشاغل مربوط به پذیرش سفارش انلاین */ && prj.STAT == "002" /* مشاغل فعال */));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void RqstInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var rcpt = Rcpt003Bs.Current as Data.Order_State;
            if (rcpt == null) return;

            // شماره پیگیری وارد شده باشد
            if (rcpt.FILE_TYPE == "001")
            {
               Process.Start(string.Format("https://idpay.ir/t/{0}", rcpt.TXID));
               return;
            }

            var prbt = PrbtBs.List.OfType<Data.Personal_Robot>().FirstOrDefault(p => p.CHAT_ID == (long)Prbt_Lov.EditValue);
            if (prbt == null) return;

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
                                 new XAttribute("actntype", "showcart"),
                                 new XAttribute("rbid", rcpt.Order.Robot.RBID),
                                 new XAttribute("ordrcode", rcpt.Order.CODE),
                                 new XAttribute("ordrtype", rcpt.Order.ORDR_TYPE),
                                 new XAttribute("ordtrwno", "*"),
                                 new XAttribute("showunit", "rcpt"),
                                 new XAttribute("chatid", prbt.CHAT_ID)
                              )
                        }
                     }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }      
   }
}
