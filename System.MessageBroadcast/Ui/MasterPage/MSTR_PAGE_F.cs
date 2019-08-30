using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;
using System.JobRouting.Jobs;

namespace System.MessageBroadcast.Ui.MasterPage
{
   public partial class MSTR_PAGE_F : UserControl
   {
      public MSTR_PAGE_F()
      {
         InitializeComponent();
      }

      private void Execute_Query()
      {
         SmsBs.DataSource = iProject.Message_Broad_Settings.Where(m => m.TYPE == "001");
         HostBs.DataSource = iProject.Gateways.Where(g => g.CONF_STAT == "002" && g.VALD_TYPE_DNRM == "002" && g.AUTH_TYPE_DNRM == "002");
      }

      private void SmsBs_CurrentChanged(object sender, EventArgs e)
      {
         var smsconfig = SmsBs.Current as Data.Message_Broad_Setting;

         if (smsconfig == null)
         {
            Ts_SmsBgwkStat.IsOn = false;
            LL_SmsTotal.Text = "";
            LL_SmsSended.Text = "";
            LL_SmsNotSending.Text = "";
         }
         else
         {
            LB_SmsLineType.Text = string.Format("نوع خط : {0} ، نام کاربری : {1} ، شماره خط : {2} می باشد", iProject.D_LNTPs.FirstOrDefault(d => d.VALU == smsconfig.LINE_TYPE).DOMN_DESC, smsconfig.USER_NAME, smsconfig.LINE_NUMB);
            Ts_SmsBgwkStat.IsOn = smsconfig.BGWK_STAT == "002" ? true : false;
            LL_SmsTotal.Text = iProject.Sms_Message_Boxes.Count(m => m.STAT == "001" && m.MESG_ID == null).ToString();
            LL_SmsSendWebService.Text = iProject.Sms_Message_Boxes.Count(m => m.STAT == "001" && m.MESG_ID != null).ToString();
            LL_SmsSended.Text = iProject.Sms_Message_Boxes.Count(m => m.STAT == "002").ToString();
            LL_SmsNotSending.Text = iProject.Sms_Message_Boxes.Count(m => m.STAT == "003").ToString();
         }
      }

      private SmsService.Sms SmsClient;
      private iNotiSmsService.iNotiSMS iNotiSmsClient;

      private void Btn_SmsServerRefresh_Click(object sender, EventArgs e)
      {
         try
         {
            Btn_SmsServerRefresh.Enabled = false;

            Action smsServerRefresh = new Action(
               () =>
               {
                  //1398/06/08 * مشخص کردن رکورد برای سامانه پیامکی
                  //var smsConf = iProject.Message_Broad_Settings;
                  var smsConf = SmsBs.Current as Data.Message_Broad_Setting;

                  if (smsConf.SERV_TYPE == "001")
                  {
                     // Sms Call Provider
                     if (SmsClient == null)
                        SmsClient = new SmsService.Sms();
                  }
                  else if(smsConf.SERV_TYPE == "002")
                  {
                     // iNoti Sms Provider
                     if (iNotiSmsClient == null)
                        iNotiSmsClient = new iNotiSmsService.iNotiSMS();
                  }

                  XDocument xmsRespons = null;
                  if (smsConf.SERV_TYPE == "001")
                  {
                     // Sms Call Provider
                     // Check Credit money for sms
                     var crntsms = SmsBs.Current as Data.Message_Broad_Setting;

                     xmsRespons = XDocument.Parse(
                        SmsClient.XmsRequest(
                           new XElement("xmsrequest",
                        //new XElement("userid", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == crntsms.LINE_TYPE).USER_NAME),
                        //new XElement("password", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == crntsms.LINE_TYPE).PASS_WORD),
                              new XElement("userid", smsConf.USER_NAME),
                              new XElement("password", smsConf.PASS_WORD),
                              new XElement("action", "getcredit"),
                              new XElement("body", "")
                           ).ToString()
                        ).ToString()
                     );
                  }
                  else if(smsConf.SERV_TYPE == "002")
                  {
                     xmsRespons =
                        new XDocument(
                           new XElement("iNotiSms",
                              new XElement("SendCredit", iNotiSmsClient.GetChargeRemaining(smsConf.USER_NAME, smsConf.PASS_WORD))
                           )
                        );
                  }

                  if (InvokeRequired)
                     Invoke(new Action(() =>
                     {
                        if (xmsRespons.Descendants("SendCredit").Count() > 0)
                        {
                           LL_SmsSendCredit.Text = xmsRespons.Descendants("SendCredit").FirstOrDefault().Value;
                           // 1397/09/01 * برای فایر شدن رخداد
                           if(_tmpjob != null) _tmpjob.Output = xmsRespons;
                           // ازاد کردن
                           _tmpjob = null;
                        }
                        Btn_SmsServerRefresh.Enabled = true;
                     }));

                  // 1395/12/21 * بررسی اینکه اطلاعات ارسال شده در چه وضعیتی هستند
                  foreach (var sms in iProject.Sms_Message_Boxes.Where(sms => sms.STAT == "001" && sms.MESG_ID != null))
                  {
                     try
                     {
                        // Check Line Type is Active
                        //if (smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE && sc.BGWK_STAT == "002") == null) continue;
                        if (smsConf.BGWK_STAT != "002") continue;

                        if (smsConf.SERV_TYPE == "001")
                        {
                           // Sms Call Provider
                           // Send Sms For Phone Number
                           xmsRespons = XDocument.Parse(
                              SmsClient.XmsRequest(
                                 new XElement("xmsrequest",
                                    new XElement("userid", smsConf.USER_NAME),
                                    new XElement("password", smsConf.PASS_WORD),
                                    new XElement("action", "smsstatus"),
                                    new XElement("body",
                                       new XElement("message", sms.MESG_ID)
                                    )
                                 ).ToString()
                              ).ToString()
                           );

                           sms.MESG_ID = xmsRespons.Descendants("message").FirstOrDefault().Attribute("id").Value;
                           sms.EROR_CODE = xmsRespons.Descendants("code").FirstOrDefault().Attribute("id").Value;
                           sms.EROR_MESG = xmsRespons.Descendants("code").FirstOrDefault().Value;
                        }
                        else if(smsConf.SERV_TYPE == "002")
                        {
                           // iNoti Sms Provider
                           var rslt = iNotiSmsClient.DeliverSMS(smsConf.USER_NAME, smsConf.PASS_WORD, smsConf.LINE_NUMB, sms.PHON_NUMB, sms.MESG_ID);
                           
                           sms.EROR_CODE = rslt.ToString();
                           switch (rslt)
                           {
                              case 6:
                                 sms.EROR_MESG = "لغو شده";
                                 break;
                              case 5:
                                 sms.EROR_MESG = "لیست سیاه";
                                 break;
                              case 4:
                                 sms.EROR_MESG = "رسیده به مخاطب";
                                 break;
                              case 3:
                                 sms.EROR_MESG = "نا موفق";
                                 break;
                              case 2:
                                 sms.EROR_MESG = "رسیده به مخابرات";
                                 break;
                              case 1:
                                 sms.EROR_MESG = "در انتظار دریافت وضعیت دلیوری از مخابرات";
                                 break;
                              case 0:
                                 sms.EROR_MESG = "چیزی یافت نشد";
                                 break;
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
                                 sms.EROR_MESG = "شماره شناسه نامعتبر";
                                 break;
                              default:
                                 sms.EROR_MESG = "خطای ناشناخته";
                                 break;
                           }
                        }
                        iProject.SubmitChanges();
                     }
                     catch (Exception ex)
                     {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                     }
                  }
               });

            Thread _tmpWorker = new Thread(new ThreadStart(smsServerRefresh));
            _tmpWorker.Start();
            //_tmpWorker.Join();
         }
         catch { }
      }

      private void Ts_SmsBgwkStat_Toggled(object sender, EventArgs e)
      {
         var smsconf = SmsBs.Current as Data.Message_Broad_Setting;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 03 /* Actn_Extr_P */, SendType.Self) 
            { 
               Input = 
                  new XElement("Process", 
                     new XElement("Action", 
                        new XAttribute("type", "001"),
                        new XAttribute("value", Ts_SmsBgwkStat.IsOn ? "true" : "false"),
                        new XElement("LineNumber", 
                           new XAttribute("mbid", smsconf.MBID)
                        )
                     )
                  )
            }
         );
      }

      private void Ts_SmsWorkerStat_Toggled(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", "", 03 /* Actn_Extr_P */, SendType.Self)
            {
               Input =
                  new XElement("Process",
                     new XElement("Action",
                        new XAttribute("type", "002"),
                        new XAttribute("value", Ts_SmsWorkerStat.IsOn ? "true" : "false")                        
                     )
                  )
            }
         );
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Host_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            var smsConf = SmsBs.Current as Data.Message_Broad_Setting;
            if (smsConf == null) return;

            smsConf.GTWY_MAC_ADRS = e.NewValue.ToString();
            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
