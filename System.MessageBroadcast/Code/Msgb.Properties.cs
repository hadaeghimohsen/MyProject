using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.MessageBroadcast.Code
{
   partial class Msgb
   {
      #region Variable Block
      Data.iProjectDataContext iProject;
      string ConnectionString;
      /// <summary>
      /// وظیفه این تایمر این می باشد که بتوانیم پیام های آماده ارسال برای سامانه اس ام اس ای را ارسال کنیم.
      /// </summary>
      private Timer _SenderBgwk;

      /// <summary>
      /// وظیفه این تایمر این می باشد که بتوانیم به زیر سیستم های مختلف این دستور را ارسال کنیم که آیا گزینه ای برای ارسال پیام اختصاصی داریم یا خیر
      /// </summary>
      private Timer _CustBgwk;
      
      private bool SmsWorkerStat = true;
      private bool _JustToday = true;
      private int _UntilBeforeDay = 1;
      private SmsService.Sms SmsClient; // Web Service Sms Call Company * Mr Vahaj
      private iNotiSmsService.iNotiSMS iNotiSmsClient; // Web Service iNoti Sms Company * Mr Marashi
      private XElement xHost;
      private bool _PingStatus;
      #endregion

      #region Event Block
      void _CustBgwk_Tick(object sender, EventArgs e)
      {
         try
         {
            // 1398/06/09 * بررسی اینکه آیا باید از طریق این سیستم پیامک ارسال شود یا خیر
            // اولین گام بدست آوردن نام سیستم فعلی
            if (xHost == null)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "DataGuard", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
                  {
                     AfterChangedOutput =
                     new Action<object>((output) =>
                     {
                        xHost = output as XElement;
                     })
                  }
               ); 

            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings.Where(m => m.DFLT_STAT == "002");

            // 1398/06/09 * بررسی اینکه سامانه ارسال پیامک ایا با سیستم فعلی اجازه ارسال را دارد یا خیر
            if (xHost == null || xHost.Attribute("cpu").Value != smsConf.FirstOrDefault().GTWY_MAC_ADRS) { _CustBgwk.Interval = 1000 * 60 * 10; return; }

            if (smsConf.Count(sms => sms.TYPE == "001" && sms.CUST_BGWK_STAT == "002") == 0)
            {
               _CustBgwk.Enabled = false;
               _CustBgwk.Stop();
            }
            else
            {
               _CustBgwk.Interval = (int)smsConf.Where(sms => sms.TYPE == "001" && sms.BGWK_STAT == "002").Average(sms => sms.CUST_BGWK_INTR);
            }

            if (SmsWorkerStat)
            {
               iProject.PrepareSendCustSms(new XElement("Process", ""));
            }
         }
         catch (Exception Ex) { MessageBox.Show(Ex.Message); }
      }

      void _SenderBgwk_Tick(object sender, EventArgs e)
      {
         try
         {
            // 1398/06/09 * بررسی اینکه آیا باید از طریق این سیستم پیامک ارسال شود یا خیر
            // اولین گام بدست آوردن نام سیستم فعلی
            if(xHost == null)
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost", "DataGuard", 04 /* Execute DoWork4GetHostInfo */, SendType.Self)
                  {
                     AfterChangedOutput =
                     new Action<object>((output) =>
                     {
                        xHost = output as XElement;
                     })
                  }
               );            

            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings.Where(m => m.DFLT_STAT == "002");

            // 1398/06/09 * بررسی اینکه سامانه ارسال پیامک ایا با سیستم فعلی اجازه ارسال را دارد یا خیر
            if (xHost == null || xHost.Attribute("cpu").Value != smsConf.FirstOrDefault().GTWY_MAC_ADRS) { _SenderBgwk.Interval = 1000 * 60 * 10; return; }

            if(smsConf.Count(sms => sms.TYPE == "001" && sms.BGWK_STAT == "002") == 0)
            {
               _SenderBgwk.Enabled = false;
               _SenderBgwk.Stop();
               return;
            }
            else
            {
               _SenderBgwk.Interval = (int)smsConf.Where(sms => sms.TYPE == "001" && sms.BGWK_STAT == "002").Average(sms => sms.BGWK_INTR);
            }

            // 1398/07/05 * بررسی اینکه آیا اینترنت برقرار می باشد یا خیر
            #region Ping Network
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", "Commons", 38 /* Execute DoWork4PingNetwork */, SendType.Self)
               {
                  Input = smsConf.FirstOrDefault().PING_IP_ADRS ?? "google.com",
                  AfterChangedOutput = 
                     new Action<object>(
                        (pingStatus) =>
                        {
                           _PingStatus = (bool)pingStatus;

                           // نتیجه بررسی ارتباط اینترنتی
                           if(!(bool)pingStatus)
                           {
                              // اگر اینترنت قطع باشد یک وقفه ده دقیقه ای انجام میشود و پیام به برای 
                              // System Try
                              // ارسال میشود
                              _SenderBgwk.Interval = smsConf.FirstOrDefault().SLEP_INTR ?? 600000;
                              //_SenderBgwk.Interval = 60000;

                              // 1401/04/11 * IF Internet is not Connected System must be Show Alarm DC
                              Gateway(
                                 new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                                 {
                                    Input = new XElement("SmsConf", new XAttribute("actntype", "InternetDisconnected"))
                                 }
                              );

                              _DefaultGateway.Gateway(
                                 new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                                 {
                                    Input =
                                       new List<object>
                                       {
                                          ToolTipIcon.Warning,
                                          "بررسی وضعیت اتصال اینترنت",
                                          string.Format("اینترنت سیستم غیرفعال می باشد، سامانه پیامکی پس از {0} دقیقه مجددا فعال میشود", (smsConf.FirstOrDefault().SLEP_INTR ?? 600000) / 60000),
                                          2000
                                       }
                                 }
                              );
                           }                           
                        }
                     )
               }
            );
            #endregion

            // 1398/07/05 * اگر شبکه اینترنتی فعال باشد سامانه شروع به ارسال پیامک کند
            if (_PingStatus)
            {
               // 1401/04/11 * IF Internet is Connected System must be Show Alarm CONNECTED
               Gateway(
                  new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                  {
                     Input = new XElement("SmsConf", new XAttribute("actntype", "InternetConnected"))
                  }
               );

               int smsSendCount = 0;
               
               #region Send Bulk Sms
               // 1398/07/05 ارسال پیام های گروهی به صورت یکبار ارسال
               var bulkSms = iProject.Sms_Message_Boxes.Where(sms => sms.STAT == "001" && sms.MESG_ID == null && sms.SEND_TYPE == "002" && (sms.PHON_NUMB.StartsWith("09") || sms.PHON_NUMB.StartsWith("9")) && sms.PHON_NUMB.Length >= 10 && sms.PHON_NUMB.Length <= 11);
               if(bulkSms.Any())
               {
                  if(SmsWorkerStat)
                  {
                     // 1398/06/08 * اضافه شدن سامانه های جدید که مشتری بتواند انتخاب کند از کدام سامانه استفاده کند
                     if (smsConf.FirstOrDefault().SERV_TYPE == "001")
                     {
                        // Sms Call Provider * Mr Vahhaj
                        if (SmsClient == null)
                           SmsClient = new SmsService.Sms();
                     }
                     else if (smsConf.FirstOrDefault().SERV_TYPE == "002")
                     {
                        // iNoti Sms Provider * Mr Maraashi
                        if (iNotiSmsClient == null)
                           iNotiSmsClient = new iNotiSmsService.iNotiSMS();
                     }

                     // Check Line Type is Active
                     if (smsConf.FirstOrDefault(sc => sc.LINE_TYPE == bulkSms.FirstOrDefault().LINE_TYPE && sc.BGWK_STAT == "002") == null) return;

                     // 1397/12/06 * چک کردن گزینه اینکه قبل از ارسال بررسی کنیم که شارژ داریم یا خیر
                     #region Check Sms Server
                     var xsmsserver = _GetSmsServerStatus();
                     if (xsmsserver == null) return;
                     int SendCredit = 0;
                     // 1398/06/08 * بررسی اینکه آیا سامانه شارژ دارد یا خیر
                     if (smsConf.FirstOrDefault().SERV_TYPE == "001")
                     {
                        // Sms Call Provider
                        SendCredit = Convert.ToInt32(xsmsserver.Descendants("SendCredit").FirstOrDefault().Value.Split('.')[0]);
                     }
                     else if (smsConf.FirstOrDefault().SERV_TYPE == "002")
                     {
                        // iNoti Sms Provider
                        SendCredit = Convert.ToInt32(xsmsserver.Descendants("SendCredit").FirstOrDefault().Value.Split('.')[0]);
                     }

                     if (smsConf.FirstOrDefault(smst => smst.TYPE == "001" && smst.BGWK_STAT == "002").ALRM_MIN_REMN_CHRG >= SendCredit)
                     {
                        // اگر میزان شارژ باقیمانده کمتر تعداد مشخص شده باشد دیگر پیامک ارسال نمیشود
                        // ارسال پیامک به کاربری که باید اطلاع رسانی شود
                        // To Do List

                        if (smsConf.FirstOrDefault(smst => smst.TYPE == "001" && smst.BGWK_STAT == "002").MIN_STOP_CHRG >= SendCredit)
                        {
                           // اگر میزان شارژ کمتر میزان باشد سامانه پیامکی باید غیر فعال شود
                           Gateway(
                              new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                              {
                                 Input = new XElement("SmsConf", new XAttribute("actntype", "SmsServerWorkerOff"))
                              }
                           );
                           //SmsWorkerStat = false;
                           //_SenderBgwk.Enabled = false;
                           //_SenderBgwk.Stop();
                           //continue;

                           _SenderBgwk.Interval = smsConf.FirstOrDefault().SLEP_INTR ?? 600000;

                           _DefaultGateway.Gateway(
                              new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                              {
                                 Input =
                                    new List<object>
                                    {
                                       ToolTipIcon.Warning,
                                       "بررسی میزان شارژ پیامکی",
                                       "میزان اعتبار پیامکی کافی نمی باشد، لطفا جهت شارژ اقدام فرمایید",
                                       2000
                                    }
                              }
                           );
                           return;
                        }
                     }
                     #endregion

                     // 1399/01/03 * بررسی اینکه آیا پیامی به صورت گروهی داریم که بخواهیم ارسال کنیم
                     // این قابلیت فقط برای سامانه ای نوتی می باشد
                     if (smsConf.FirstOrDefault().SERV_TYPE == "002")
                     {
                        //MessageBox.Show(string.Format("BatchSms : {0}, {1}, {2}, \n{3}, {4}", smsConf.FirstOrDefault().USER_NAME, smsConf.FirstOrDefault().PASS_WORD, smsConf.FirstOrDefault().LINE_NUMB, bulkSms.Select(bs => bs.PHON_NUMB).ToArray(), bulkSms.FirstOrDefault().MSGB_TEXT));
                        var rslt = iNotiSmsClient.SendBatchSMS(smsConf.FirstOrDefault().USER_NAME, smsConf.FirstOrDefault().PASS_WORD, smsConf.FirstOrDefault().LINE_NUMB, bulkSms.Select(bs => bs.PHON_NUMB).ToArray(), bulkSms.FirstOrDefault().MSGB_TEXT);
                        if (rslt > 0)
                           bulkSms.ToList().ForEach(bs => bs.MESG_ID = rslt.ToString());
                        else
                        {
                           bulkSms.ToList()
                           .ForEach(sms =>
                           {
                              sms.MESG_ID = "0";
                              sms.EROR_CODE = rslt.ToString();
                              switch (rslt)
                              {
                                 case -1:
                                    sms.EROR_MESG = "اطلاعات کاربری نامعتبر";
                                    break;
                                 case -2:
                                    sms.EROR_MESG = "شماره خط نامعتبر";
                                    break;
                                 case -3:
                                    sms.EROR_MESG = "شماره موبایل نا معتبر";
                                    break;
                                 case -4:
                                    sms.EROR_MESG = "موجودی نا کافی";
                                    break;
                                 default:
                                    sms.EROR_MESG = "خطای ناشناخته";
                                    break;
                              }
                           });                           
                        }
                     }

                     iProject.SubmitChanges();

                     smsSendCount = bulkSms.Count();
                  }
               }
               #endregion
               #region Send Single Sms
               foreach (var sms in iProject.Sms_Message_Boxes.Where(sms => sms.STAT == "001" && sms.MESG_ID == null && ((_JustToday && sms.ACTN_DATE.Value.Date == DateTime.Now.Date) || (!_JustToday && sms.ACTN_DATE.Value.Date <= DateTime.Now.Date && sms.ACTN_DATE.Value.Date >= DateTime.Now.Date.AddDays(_UntilBeforeDay * -1))) && (sms.PHON_NUMB.StartsWith("09") || sms.PHON_NUMB.StartsWith("9")) && sms.PHON_NUMB.Length >= 10 && sms.PHON_NUMB.Length <= 11))
               {
                  // 1398/07/05 * برای محکم کاری بیشتر برای هر بار ارسال اطلاعات تست ارتباط انجام شود
                  #region Ping Network
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Commons", 38 /* Execute DoWork4PingNetwork */, SendType.Self)
                     {
                        Input = smsConf.FirstOrDefault().PING_IP_ADRS ?? "www.google.com",
                        AfterChangedOutput =
                           new Action<object>(
                              (pingStatus) =>
                              {
                                 _PingStatus = (bool)pingStatus;

                                 // نتیجه بررسی ارتباط اینترنتی
                                 if (!(bool)pingStatus)
                                 {
                                    // اگر اینترنت قطع باشد یک وقفه ده دقیقه ای انجام میشود و پیام به برای 
                                    // System Try
                                    // ارسال میشود
                                    _SenderBgwk.Interval = smsConf.FirstOrDefault().SLEP_INTR ?? 600000;

                                    // 1401/04/11 * IF Internet is not Connected System must be Show Alarm DC
                                    Gateway(
                                       new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                                       {
                                          Input = new XElement("SmsConf", new XAttribute("actntype", "InternetDisconnected"))
                                       }
                                    );                                    

                                    _DefaultGateway.Gateway(
                                       new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                                       {
                                          Input =
                                             new List<object>
                                             {
                                                ToolTipIcon.Warning,
                                                "بررسی وضعیت اتصال اینترنت",
                                                string.Format("اینترنت سیستم غیرفعال می باشد، سامانه پیامکی پس از {0} دقیقه مجددا فعال میشود", (smsConf.FirstOrDefault().SLEP_INTR ?? 600000) / 60000),
                                                2000
                                             }
                                       }
                                    );
                                 }
                              }
                           )
                     }
                  );

                  // اگر شبکه اینترنت قطع باشد از حلقه ارسال خارج میشویم
                  if (!_PingStatus)
                  {
                     // 1401/04/11 * IF Internet is not Connected System must be Show Alarm DC
                     Gateway(
                        new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                        {
                           Input = new XElement("SmsConf", new XAttribute("actntype", "InternetDisconnected"))
                        }
                     );
                     break;
                  }
                  #endregion

                  // 1401/04/11 * IF Internet is Connected System must be Show Alarm CONNECTED
                  Gateway(
                     new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                     {
                        Input = new XElement("SmsConf", new XAttribute("actntype", "InternetConnected"))
                     }
                  );

                  if (SmsWorkerStat)
                  {
                     // 1398/06/08 * اضافه شدن سامانه های جدید که مشتری بتواند انتخاب کند از کدام سامانه استفاده کند
                     if (smsConf.FirstOrDefault().SERV_TYPE == "001")
                     {
                        // Sms Call Provider * Mr Vahhaj
                        if (SmsClient == null)
                           SmsClient = new SmsService.Sms();
                     }
                     else if (smsConf.FirstOrDefault().SERV_TYPE == "002")
                     {
                        // iNoti Sms Provider * Mr Maraashi
                        if (iNotiSmsClient == null)
                           iNotiSmsClient = new iNotiSmsService.iNotiSMS();
                     }

                     // Check Line Type is Active
                     if (smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE && sc.BGWK_STAT == "002") == null) continue;

                     // 1397/12/06 * چک کردن گزینه اینکه قبل از ارسال بررسی کنیم که شارژ داریم یا خیر
                     #region Check Sms Server
                     var xsmsserver = _GetSmsServerStatus();
                     if (xsmsserver == null) return;
                     int SendCredit = 0;
                     // 1398/06/08 * بررسی اینکه آیا سامانه شارژ دارد یا خیر
                     if (smsConf.FirstOrDefault().SERV_TYPE == "001")
                     {
                        // Sms Call Provider
                        SendCredit = Convert.ToInt32(xsmsserver.Descendants("SendCredit").FirstOrDefault().Value.Split('.')[0]);
                     }
                     else if (smsConf.FirstOrDefault().SERV_TYPE == "002")
                     {
                        // iNoti Sms Provider
                        SendCredit = Convert.ToInt32(xsmsserver.Descendants("SendCredit").FirstOrDefault().Value.Split('.')[0]);
                     }

                     if (smsConf.FirstOrDefault(smst => smst.TYPE == "001" && smst.BGWK_STAT == "002").ALRM_MIN_REMN_CHRG >= SendCredit)
                     {
                        // اگر میزان شارژ باقیمانده کمتر تعداد مشخص شده باشد دیگر پیامک ارسال نمیشود
                        // ارسال پیامک به کاربری که باید اطلاع رسانی شود
                        // To Do List

                        if (smsConf.FirstOrDefault(smst => smst.TYPE == "001" && smst.BGWK_STAT == "002").MIN_STOP_CHRG >= SendCredit)
                        {
                           // اگر میزان شارژ کمتر میزان باشد سامانه پیامکی باید غیر فعال شود
                           Gateway(
                              new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                              {
                                 Input = new XElement("SmsConf", new XAttribute("actntype", "SmsServerWorkerOff"))
                              }
                           );

                           //SmsWorkerStat = false;
                           //_SenderBgwk.Enabled = false;
                           //_SenderBgwk.Stop();
                           //continue;

                           _SenderBgwk.Interval = smsConf.FirstOrDefault().SLEP_INTR ?? 600000;

                           _DefaultGateway.Gateway(
                              new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                              {
                                 Input =
                                    new List<object>
                                    {
                                       ToolTipIcon.Warning,
                                       "بررسی میزان شارژ پیامکی",
                                       "میزان اعتبار پیامکی کافی نمی باشد، لطفا جهت شارژ اقدام فرمایید",
                                       2000
                                    }
                              }
                           );
                           break;
                        }
                     }
                     #endregion

                     if (smsConf.FirstOrDefault().SERV_TYPE == "001")
                     {
                        // Send Sms For Phone Number
                        XDocument xmsRespons = XDocument.Parse(
                           SmsClient.XmsRequest(
                              new XElement("xmsrequest",
                                 new XElement("userid", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE).USER_NAME),
                                 new XElement("password", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE).PASS_WORD),
                                 new XElement("action", "smssend"),
                                 new XElement("body",
                                    new XElement("type", "oto"),
                                    new XElement("recipient",
                                       new XAttribute("mobile", sms.PHON_NUMB),
                                       sms.MSGB_TEXT
                                    )
                                 )
                              ).ToString()
                           ).ToString()
                        );

                        sms.MESG_ID = xmsRespons.Descendants("recipient").FirstOrDefault().Value;
                        sms.EROR_CODE = xmsRespons.Descendants("code").FirstOrDefault().Attribute("id").Value;
                        sms.EROR_MESG = xmsRespons.Descendants("code").FirstOrDefault().Value;
                     }
                     else if (smsConf.FirstOrDefault().SERV_TYPE == "002")
                     {
                        //MessageBox.Show(string.Format("SingleSms : {0}, {1}, {2}, \n{3}, {4}", smsConf.FirstOrDefault().USER_NAME, smsConf.FirstOrDefault().PASS_WORD, smsConf.FirstOrDefault().LINE_NUMB, sms.PHON_NUMB, sms.MSGB_TEXT));
                        var rslt = iNotiSmsClient.SendSingleSMS(smsConf.FirstOrDefault().USER_NAME, smsConf.FirstOrDefault().PASS_WORD, smsConf.FirstOrDefault().LINE_NUMB, sms.PHON_NUMB, sms.MSGB_TEXT);
                        if (rslt > 0)
                           sms.MESG_ID = rslt.ToString();
                        else
                        {
                           sms.MESG_ID = "0";
                           sms.EROR_CODE = rslt.ToString();
                           switch (rslt)
                           {
                              case -1:
                                 sms.EROR_MESG = "اطلاعات کاربری نامعتبر";
                                 break;
                              case -2:
                                 sms.EROR_MESG = "شماره خط نامعتبر";
                                 break;
                              case -3:
                                 sms.EROR_MESG = "شماره موبایل نا معتبر";
                                 break;
                              case -4:
                                 sms.EROR_MESG = "موجودی نا کافی";
                                 break;
                              default:
                                 sms.EROR_MESG = "خطای ناشناخته";
                                 break;
                           }
                        }
                     }

                     iProject.SubmitChanges();

                     ++smsSendCount;
                  }
                  else
                  {
                     break;
                  }
               }
               #endregion

               // بعد از ارسال اگر پیامکی ارسال شده باشد اطلاع رسانی میکنیم
               if(smsSendCount > 0)
               {
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "Wall", 22 /* Execute SetSystemNotification */, SendType.SelfToUserInterface)
                     {
                        Input =
                           new List<object>
                           {
                              ToolTipIcon.Info,
                              "سامانه اطلاع رسانی پیامکی",
                              string.Format("پیامک ارسالی {0} عدد می باشد", smsSendCount),
                              2000
                           }
                     }
                  );
               }

               // 1401/04/10 * عملیات ارسال پیام به نرم افزار بله
               // آماده سازی رکوردهای مورد نظر در جدول نرم افزار اپلیکیشن
               iProject.ExecuteJobScheduleSubSystem(
                  new XElement("Job",
                      new XAttribute("type", "SMSTOAPP"),
                      new XElement("Params",
                          new XAttribute("justtoday", _JustToday),
                          new XAttribute("untilbeforeday", _UntilBeforeDay)
                      )
                  )
               );

               // مرحله بعدی ارسال درخواست برای ارسال پیام به مخاطبین
            }
         }
         catch {}
         finally {}
      }
      #endregion

      #region procedure
      private void _GetConnectionString()
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iProject = new Data.iProjectDataContext(GetConnectionString.Output.ToString());
      }

      private XDocument _GetSmsServerStatus()
      {
         try
         {
            XDocument xoutput = new XDocument();
            
            //_DefaultGateway.Gateway(
            //   new Job(SendType.External, "localhost",
            //      new List<Job>
            //      {
            //         new Job(SendType.External, "Msgb",
            //            new List<Job>
            //            {
            //               new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 10 /* Execuet Actn_Calf_F */)
            //               {
            //                  Input =
            //                     new XElement("SmsConf",
            //                        new XAttribute("actntype", "getcredit")
            //                     ),
            //                  AfterChangedOutput =
            //                     new Action<object>((output) =>
            //                     {
            //                        xoutput = output as XDocument;
            //                        //SendCreditCount_Txt.Text = xoutput.Descendants("SendCredit").FirstOrDefault().Value;
            //                        //SendCreditAmnt_Txt.Text = xoutput.Descendants("SMS_SendFee").FirstOrDefault().Value;
            //                        //ReceiveCreditCount_Txt.Text = xoutput.Descendants("RecieveCredit").FirstOrDefault().Value;
            //                        //ReceiveCreditAmnt_Txt.Text = xoutput.Descendants("SMS_RecieveFee").FirstOrDefault().Value;                                                
            //                     })
            //               }
            //            }
            //         )
            //      }
            //   )
            //);

            // 1398/06/08 * مشخص کردن سامانه پیش فرض
            var smsConf = iProject.Message_Broad_Settings.Where(m => m.DFLT_STAT == "002");

            // 1398/06/08 * مشخص کردن اینکه کدام سامانه می خواهیم استفاده کنیم
            if (smsConf.FirstOrDefault().SERV_TYPE == "001")
            {
               // Sms Call Provider * Mr Vahhaj
               if (SmsClient == null)
                  SmsClient = new SmsService.Sms();
            }
            else if(smsConf.FirstOrDefault().SERV_TYPE == "002")
            {
               // iNoti Sms Provider * Mr Maraashi
               if (iNotiSmsClient == null)
                  iNotiSmsClient = new iNotiSmsService.iNotiSMS();
            }

            // 1398/06/08 * Sms Call Provider
            if (smsConf.FirstOrDefault().SERV_TYPE == "001")
            {
               XDocument xmsRespons = null;

               // Check Credit money for sms
               var crntsms = smsConf.FirstOrDefault();

               xmsRespons = XDocument.Parse(
                  SmsClient.XmsRequest(
                     new XElement("xmsrequest",
                        new XElement("userid", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == crntsms.LINE_TYPE).USER_NAME),
                        new XElement("password", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == crntsms.LINE_TYPE).PASS_WORD),
                        new XElement("action", "getcredit"),
                        new XElement("body", "")
                     ).ToString()
                  ).ToString()
               );

               if (xmsRespons.Descendants("SendCredit").Count() > 0)
               {
                  xoutput = xmsRespons;
                  return xoutput;
               }
               else return null;
            }
            else if(smsConf.FirstOrDefault().SERV_TYPE == "002")
            {
               return 
                  new XDocument(
                     new XElement("iNotiSms",
                        new XElement("SendCredit", iNotiSmsClient.GetChargeRemaining(smsConf.FirstOrDefault().USER_NAME, smsConf.FirstOrDefault().PASS_WORD))
                     )
                  );
            }
            return null;
            //return xoutput;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            return null;
         }
      }
      #endregion
   }
}
