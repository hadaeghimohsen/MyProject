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
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         switch (value)
         {
            case "mstr_page_f":
               if (_Mstr_Page_F == null)
                  _Mstr_Page_F = new Ui.MasterPage.MSTR_PAGE_F { _DefaultGateway = this };
               break;
            case "send_mesg_f":
               if (_Send_Mesg_F == null)
                  _Send_Mesg_F = new Ui.SmsApp.SEND_MESG_F { _DefaultGateway = this };
               break;
            default:
               break;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Mstr_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_page_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void Actn_Extr_P(Job job)
      {
         try
         {
            var action = job.Input as XElement;

            switch (action.Element("Action").Attribute("type").Value)
            {
               case "001":
                  // _SenderBgwk Opration
                  switch (action.Element("Action").Attribute("value").Value)
                  {
                     case "true":
                        iProject.Message_Broad_Settings.Where(s => s.MBID == Convert.ToInt64(action.Descendants("LineNumber").FirstOrDefault().Attribute("mbid").Value)).ToList().ForEach(smsconf => { smsconf.BGWK_STAT = "002"; smsconf.CUST_BGWK_STAT = "002"; });
                        _CustBgwk.Enabled = true;
                        _SenderBgwk.Enabled = true;
                        break;
                     case "false":
                        iProject.Message_Broad_Settings.Where(s => s.MBID == Convert.ToInt64(action.Descendants("LineNumber").FirstOrDefault().Attribute("mbid").Value)).ToList().ForEach(smsconf => { smsconf.BGWK_STAT = "001"; smsconf.CUST_BGWK_STAT = "001"; });
                        _CustBgwk.Stop();
                        _SenderBgwk.Stop();
                        break;
                     default:
                        break;
                  }
                  iProject.SubmitChanges();
                  break;
               case "002":
                  // Emergency _SenderBgwk Opration
                  switch (action.Element("Action").Attribute("value").Value)
                  {
                     case "true":
                        SmsWorkerStat = false;
                        break;
                     case "false":
                        SmsWorkerStat = true;
                        break;
                     default:
                        break;
                  }
                  break;
               case "003":
                  switch (action.Element("Action").Attribute("value").Value)
                  {
                     case "true":
                        _CustBgwk.Start();
                        _SenderBgwk.Start();
                        break;
                     case "false":
                        _CustBgwk.Stop();
                        _SenderBgwk.Stop();
                        break;
                     default:
                        break;
                  }
                  break;
               case "004":
                  _SenderBgwk.Interval = Convert.ToInt32(action.Element("Action").Attribute("value").Value);
                  break;
               case "005":
                  _JustToday = Convert.ToBoolean(action.Element("Action").Attribute("value").Value);
                  break;
               case "006":
                  _UntilBeforeDay = Convert.ToInt32(action.Element("Action").Attribute("value").Value);
                  break;
               default:
                  break;
            }
         }
         catch(Exception ex)
         {
            //System.Windows.Forms.MessageBox.Show(ex.Message);
            System.Diagnostics.Debug.WriteLine(ex.Message);
         }
         finally
         {
            iProject = new Data.iProjectDataContext(ConnectionString);
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void Mesg_Chks_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if (xinput == null) return;

            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings.Where(m => m.DFLT_STAT == "002");

            var subsys = Convert.ToInt32(xinput.Attribute("subsys").Value);
            var rfid = Convert.ToInt32(xinput.Attribute("rfid").Value);

            var sms = iProject.Sms_Message_Boxes.FirstOrDefault(m => m.SUB_SYS == subsys && m.RFID == rfid);

            if(sms.MESG_ID == "0")
            {
               job.Output =
                  new XElement("Message",
                     new XAttribute("mesgid", sms.MESG_ID),
                     new XElement("Result", 
                        new XAttribute("code", "001"),
                        "پیامک قادر به ارسال نیست، ممکن است شماره مورد نظر در لیست سیاه قرارداده شده باشد")
                  );
               job.Status = StatusType.Successful;
               return;
            }

            //1398/06/08 * مشخص کردن نوع سامانه خدماتی
            if (smsConf.FirstOrDefault().SERV_TYPE == "001")
            {
               // Sms Call Provider
               if (SmsClient == null)
                  SmsClient = new SmsService.Sms();
            }
            else if(smsConf.FirstOrDefault().SERV_TYPE == "002")
            {
               // iNoti Sms Provider
               if (iNotiSmsClient == null)
                  iNotiSmsClient = new iNotiSmsService.iNotiSMS();
            }

            // 1398/06/08 * مشخص کردن سامانه پیامکی
            if (smsConf.FirstOrDefault().SERV_TYPE == "001")
            {
               XDocument xmsRespons = XDocument.Parse(
                  SmsClient.XmsRequest(
                     new XElement("xmsrequest",
                        new XElement("userid", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE).USER_NAME),
                        new XElement("password", smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE).PASS_WORD),
                        new XElement("action", "smsstatus"),
                        new XElement("body",
                           new XElement("message", sms.MESG_ID)
                        )
                     ).ToString()
                  ).ToString()
               );

               sms.SRVR_SEND_DATE = Convert.ToDateTime(xmsRespons.Descendants("message").Attributes("startdate").First().Value);
               sms.MESG_LENT = Convert.ToInt32(xmsRespons.Descendants("message").Attributes("messagelength").First().Value);

               job.Output =
                     new XElement("Message",
                        xmsRespons.Descendants("message"),
                        new XElement("Result",
                           new XAttribute("code", "100"),
                           "اطلاعات با موفقیت به شما برگشت داده شد")
                     );
            }
            else if(smsConf.FirstOrDefault().SERV_TYPE == "002")
            {
               var rslt = iNotiSmsClient.DeliverSMS(smsConf.FirstOrDefault().USER_NAME, smsConf.FirstOrDefault().PASS_WORD, smsConf.FirstOrDefault().LINE_NUMB, sms.PHON_NUMB, sms.MESG_ID);

               job.Output =
                     new XElement("Message",
                        rslt,
                        new XElement("Result",
                           new XAttribute("code", "100"),
                           "اطلاعات با موفقیت به شما برگشت داده شد")
                     );
            }
            job.Status = StatusType.Successful;

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);            
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void Mesg_Recv_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if (xinput == null) return;

            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings.FirstOrDefault(sc => sc.DFLT_STAT == "002");

            // 1398/06/08 * مشخص کردن سامانه پیامکی
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

            if (smsConf.SERV_TYPE == "001")
            {
               XDocument xmsRespons = XDocument.Parse(
                  SmsClient.XmsRequest(
                     new XElement("xmsrequest",
                        new XElement("userid", smsConf.USER_NAME),
                        new XElement("password", smsConf.PASS_WORD),
                        new XElement("action", "smsreceive"),
                        new XElement("body",
                           new XElement("lastsmsid", smsConf.LAST_ROW_FTCH),
                           new XElement("count", smsConf.FTCH_ROW)
                        )
                     ).ToString()
                  ).ToString()
               );

               smsConf.LAST_ROW_FTCH = Convert.ToInt64(xmsRespons.Descendants("message").Attributes("id").First().Value);

               job.Output =
                     new XElement("Message",
                        xmsRespons.Descendants("message"),
                        new XElement("Result",
                           new XAttribute("code", "100"),
                           "اطلاعات با موفقیت به شما برگشت داده شد")
                     );
            }
            else if(smsConf.SERV_TYPE == "002")
            {
               job.Output =
                     new XElement("Message",
                        "بدون عملیات از سمت سرور",
                        new XElement("Result",
                           new XAttribute("code", "100"),
                           "اطلاعات با موفقیت به شما برگشت داده شد")
                     );
            }
            job.Status = StatusType.Successful;

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void Tree_Node_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;
            if (xinput == null) return;

            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings.FirstOrDefault(sc => sc.DFLT_STAT == "002");

            // 1398/06/08 * مشخص کردن سامانه پیامکی
            if (smsConf.SERV_TYPE == "001")
            {
               // Sms Call Provider
               if (SmsClient == null)
                  SmsClient = new SmsService.Sms();
            }
            else if (smsConf.SERV_TYPE == "002")
            {
               // iNoti Sms Provider
               if (iNotiSmsClient == null)
                  iNotiSmsClient = new iNotiSmsService.iNotiSMS();
            }

            if (smsConf.SERV_TYPE == "001")
            {
               XDocument xmsRespons = XDocument.Parse(
                  SmsClient.XmsRequest(
                     new XElement("xmsrequest",
                        new XElement("userid", smsConf.USER_NAME),
                        new XElement("password", smsConf.PASS_WORD),
                        new XElement("action", "treenodes"),
                        new XElement("body",
                           new XElement("node",
                              new XAttribute("id", 5000000)
                           )
                        )
                     ).ToString()
                  ).ToString()
               );

               job.Output =
                     new XElement("Message",
                        xmsRespons.Descendants("message"),
                        new XElement("Result",
                           new XAttribute("code", "100"),
                           "اطلاعات با موفقیت به شما برگشت داده شد")
                     );
            }
            else if(smsConf.SERV_TYPE == "002")
            {
               job.Output =
                     new XElement("Message",
                        "بدون عملیات از سمت سرور",
                        new XElement("Result",
                           new XAttribute("code", "100"),
                           "اطلاعات با موفقیت به شما برگشت داده شد")
                     );
            }
            job.Status = StatusType.Successful;

            iProject.SubmitChanges();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void Send_Mesg_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "send_mesg_f"},
                  new Job(SendType.SelfToUserInterface, "SEND_MESG_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SEND_MESG_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SEND_MESG_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
