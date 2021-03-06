﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.RoboTech.ExtCode;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Diagnostics;

namespace System.RoboTech.Ui.MasterPage
{
   partial class FRST_PAGE_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iRoboTechDataContext iRoboTech;
      private string ConnectionString;
      private List<long?> Fga_Ugov_U;
      private string Crnt_User;

      private XElement x = null;
      public void SendRequest(Job job)
      {
         switch (job.Method)
         {
            case 00:
               ProcessCmdKey(job);
               break;
            case 01:
               Get(job);
               break;
            case 02:
               Set(job);
               break;
            case 03:
               Paint(job);
               break;
            case 04:
               UnPaint(job);
               break;
            case 05:
               CheckSecurity(job); ;
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               PostOnWall(job);
               break;
            case 09:
               TakeOnWall(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 40:
               SetToolTip(job);
               break;
            case 1000:
               if (InvokeRequired)
                  Invoke(new System.Action(() => Call_SystemService_P(job)));
               else
                  Call_SystemService_P(job);
               break;
            default:
               break;
         }
      }

      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void ProcessCmdKey(Job job)
      {
         Keys keyData = (Keys)job.Input;

         if (keyData == Keys.F1)
         {
            #region Key.F1
            job.Next =
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 03 /* Execute DoWork4HelpHandling */)
                     {
                        Input = @"<HTML>
                                    <body>
                                       <p style=""float:right"">
                                             <ol>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F10</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از سیستم</font></li>
                                                </ul>
                                                <li><font face=""verdana"" size=""3"" color=""red"">F9</font></li>
                                                <ul>
                                                   <li><font face=""Tahoma"" size=""3"" color=""green"">خروج از محیط کاربری</font></li>
                                                </ul>
                                             </ol>
                                       </p>
                                    </body>
                                    </HTML>"
                     }
                  });
            #endregion
         }
         else if (keyData == Keys.F9)
         {
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void Get(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Set(Job job)
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iRoboTech</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         if(GetConnectionString.Output != null)
            ConnectionString = GetConnectionString.Output.ToString();
         else
         {
            job.Status = StatusType.Failed;
            return;
         }

         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         Fga_Ugov_U = (iRoboTech.FGA_UGOV_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("RoboTech:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         //Stop_BarCode();
         //Stop_FingerPrint();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
               })
            );

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
         OrderAction_Recipt();
         InstagramOperationInit();

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void PostOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;

            if (InvokeRequired)
               Invoke(new Action<UserControl>(c =>
               {
                  c.Dock = DockStyle.Fill;
                  c.Visible = true;
                  Pnl_Desktop.Panel1.Controls.Add(c);
                  Pnl_Desktop.Panel1.Controls.SetChildIndex(c, 0);
               }), obj);
            else
            {
               obj.Dock = DockStyle.Fill;
               obj.Visible = true;
               Pnl_Desktop.Panel1.Controls.Add(obj);
               Pnl_Desktop.Panel1.Controls.SetChildIndex(obj, 0);
            }

         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void TakeOnWall(Job job)
      {
         try
         {
            UserControl obj = (UserControl)job.Input;
            if (obj == null || Pnl_Desktop.Panel1.Controls.IndexOf(obj) < 0)
            {
               job.Status = StatusType.Successful;
               return;
            }
            if (InvokeRequired)
               Invoke(new Action<UserControl>(c => Pnl_Desktop.Panel1.Controls.Remove(c)), obj);
            else
               Pnl_Desktop.Panel1.Controls.Remove(obj);
            this.Focus();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Successful; UserControl obj = (UserControl)job.Input;
            Invoke(new Action<UserControl>(c => Pnl_Desktop.Panel1.Controls.Remove(c)), obj);
            this.Focus();
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void SetToolTip(Job job)
      {
         //Lbs_Tooltip.Text = job.Input.ToString();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 1000
      /// </summary>
      /// <param name="job"></param>
      private void Call_SystemService_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if (xinput == null) return;

            var chatid = xinput.Attribute("chatid").Value.ToInt64();
            var cmnd = xinput.Attribute("cmnd").Value;
            var param = xinput.Attribute("param").Value;

            // اگر اتفاق جدیدی درون سیستم پایگاه داده رخ داده باشد بتوانیم آخرین اطلاعات را بازیابی کنیم
            iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

            switch (cmnd.ToLower())
            {
               case "loadrcpt":
                  #region LoadRcpt
                  frstLoad = false;
                  OrderAction_Recipt();

                  var ordrrcpt =
                     iRoboTech.Order_States.Where(os => os.Order.Robot.Organ.STAT == "002" && os.Order.Robot.STAT == "002" && os.Order.ORDR_STAT == "001" && os.AMNT_TYPE == "005" && os.CONF_STAT == "003");
                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10001),
                        new XAttribute("resultdesc", ordrrcpt.Count() > 0 ? string.Format("تعداد پیامهای ارسال رسید پرداخت درون صف {0} عدد میباشد، لطفا تا دریافت نتیجه صبر کنید", ordrrcpt.Count()) :
                                                                            "پیام شما برای واحد مربوطه ارسال شد، لطفا تا دریافت نتیجه صبر کنید" 
                                      ),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  #endregion
                  break;
               case "prodqury":
                  #region ProdQury
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost",
                       new List<Job>
                       {                  
                         //new Job(SendType.Self, 26 /* Execute Prod_Dvlp_F */),
                         new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 10 /* Execute Actn_CalF_P */)
                       })
                  );
                  #endregion
                  break;
               case "errornoti":
                  #region ErrorNoti
                  _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        {
                           Input =
                              new List<object>
                              {
                                 ToolTipIcon.Error,
                                 "عملیات ناموفق",
                                 param,
                                 1000
                              },
                           Executive = ExecutiveType.Asynchronous
                        }
                     );
                  #endregion
                  break;
               case "successfullnoti":
                  #region SuccessfulNoti
                  _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                        {
                           Input =
                              new List<object>
                              {
                                 ToolTipIcon.Info,
                                 "عملیات موفق",
                                 param,
                                 1000
                              },
                           Executive = ExecutiveType.Asynchronous
                        }
                     );
                  #endregion
                  break;
               case "receptionorder::ok":
                  #region Recption Online Order
                  iRoboTech.ExecuteCommand(string.Format("BEGIN UPDATE dbo.[Order] SET Ordr_Stat = '002' WHERE Code = {0}; END;", param));

                  frstLoad = false;
                  OrderAction_Recipt();
                  
                  var ordr25 =
                     iRoboTech.Orders.Where(o => o.ORDR_TYPE == "025" && (o.ORDR_STAT == "002" || o.ORDR_STAT == "016")).ToList();

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10001),
                        new XAttribute("resultdesc", "📨 *ارسال درخواست سفارش* " + Environment.NewLine + Environment.NewLine + "👈 *شماره درخواست شما* [ *" + param + "* ] - 025" + Environment.NewLine + "🔢 تعداد سفارشات درون صف [ *" + ordr25.Count().ToString() + "* ]"),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  #endregion
                  break;
               case "chkotrcpt":
                  #region Checkout Rcpt
                  // در مرحله اول سریع چک میکنیم که ایا برای این درخواست قبلا ردیف رسید در حال انتظار تایید وجود دارد یا خیر
                  iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
                  if(!iRoboTech.Order_States.Any(os => os.ORDR_CODE == param.ToInt64() && os.AMNT_TYPE == "005" && os.CONF_STAT == "003"))
                  {
                     // پیدا کردن شماره درخواست
                     var ordrserv = iRoboTech.Orders.FirstOrDefault(o => o.CODE == param.ToInt64());
                     // ثبت ردیفی برای درخواست به عنوان رسید در انتظار تایید وصولی
                     var ordrstat = new Data.Order_State() { ORDR_CODE = ordrserv.CODE, AMNT_TYPE = "005", AMNT = ordrserv.DEBT_DNRM, CONF_STAT = "003", RCPT_MTOD = "002", STAT_DATE = DateTime.Now, STAT_DESC = "استعلام درخواست تایید توسط مشتری", FILE_TYPE = "001"};
                     iRoboTech.Order_States.InsertOnSubmit(ordrstat);

                     // Save Change
                     iRoboTech.SubmitChanges();
                  }

                  frstLoad = false;
                  OrderAction_Recipt();
                  /// در این قسمت در حال حاضر که پرداختی ها به صورت دستی ثبت میشوند به این گونه انجام میدهیم که صفحه آی دی پی را با پارامترهای لازم اجرا میکنیم و به حسابداری نمایش میدهیم حسابدار با دیدن ان و تایید وصولی ان را تایید میکند
                  /// زمانی که تایید وصولی به صورت اتومات شد این گزینه به خودی خود غیر فعال میشود و ثبت وصولی اتومات میشود
                  string url = "https://idpay.ir/dashboard/deposits?status=All&account=All&gateway=All&web-service=All&track=&price={0}&phone={1}&desc={2}";
                  var ordr = iRoboTech.Orders.FirstOrDefault(o => o.CODE == param.ToInt64());
                  Process.Start(string.Format(url, (ordr.AMNT_TYPE == "001" ? ordr.DEBT_DNRM : ordr.DEBT_DNRM * 10), ordr.CELL_PHON, ordr.CODE));

                  job.Output =
                     new XElement("Output",
                        new XAttribute("resultcode", 10001),
                        new XAttribute("resultdesc", "پیام شما برای واحد حسابداری ارسال شد، لطفا تا دریافت نتیجه صبر کنید"),
                        new XAttribute("mesgtype", "1"),
                        new XAttribute("mesgdesc", "Text")
                     );
                  #endregion
                  break;
            }

            job.Status = StatusType.Successful;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }         
      }
   }
}
