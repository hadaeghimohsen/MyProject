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
      private SmsService.Sms SmsClient;
      #endregion

      #region Event Block
      void _CustBgwk_Tick(object sender, EventArgs e)
      {
         try
         {
            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings;

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
            _GetConnectionString();
            var smsConf = iProject.Message_Broad_Settings;

            if(smsConf.Count(sms => sms.TYPE == "001" && sms.BGWK_STAT == "002") == 0)
            {
               _SenderBgwk.Enabled = false;
               _SenderBgwk.Stop();
            }
            else
            {
               _SenderBgwk.Interval = (int)smsConf.Where(sms => sms.TYPE == "001" && sms.BGWK_STAT == "002").Average(sms => sms.BGWK_INTR);
            }

            foreach (var sms in iProject.Sms_Message_Boxes.Where(sms => sms.STAT == "001" && sms.MESG_ID == null))
            {
               if (SmsWorkerStat)
               {
                  if (SmsClient == null)
                     SmsClient = new SmsService.Sms();

                  // Check Line Type is Active
                  if (smsConf.FirstOrDefault(sc => sc.LINE_TYPE == sms.LINE_TYPE && sc.BGWK_STAT == "002") == null) continue;

                  // 1397/12/06 * چک کردن گزینه اینکه قبل از ارسال بررسی کنیم که شارژ داریم یا خیر
                  #region Check Sms Server
                  var xsmsserver = _GetSmsServerStatus();
                  var SendCredit = Convert.ToInt32(xsmsserver.Descendants("SendCredit").FirstOrDefault().Value);
                  if(smsConf.FirstOrDefault(smst => smst.TYPE == "001" && smst.BGWK_STAT == "002").ALRM_MIN_REMN_CHRG <= SendCredit)
                  {
                     // اگر میزان شارژ باقیمانده کمتر تعداد مشخص شده باشد دیگر پیامک ارسال نمیشود
                     // ارسال پیامک به کاربری که باید اطلاع رسانی شود
                     // To Do List

                     if(smsConf.FirstOrDefault(smst => smst.TYPE == "001" && smst.BGWK_STAT == "002").MIN_STOP_CHRG <= SendCredit)
                     {
                        // اگر میزان شارژ کمتر میزان باشد سامانه پیامکی باید غیر فعال شود
                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost", "MSTR_PAGE_F", 10 /* Execute Actn_Calf_P */, SendType.SelfToUserInterface)
                           {
                              Input = new XElement("SmsConf", new XAttribute("actntype", "SmsServerWorkerOff"))
                           }
                        );
                        //SmsWorkerStat = false;
                        //_SenderBgwk.Enabled = false;
                        //_SenderBgwk.Stop();
                        continue;
                     }
                  }
                  #endregion

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

                  iProject.SubmitChanges();
               }
               else
               {
                  break;
               }               
            }
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {

         }
      }
      #endregion

      #region procedure
      void _GetConnectionString()
      {
         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iProject</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iProject = new Data.iProjectDataContext(GetConnectionString.Output.ToString());
      }

      XDocument _GetSmsServerStatus()
      {
         try
         {
            var xoutput = new XDocument("SmsServer");
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.External, "Msgb",
                        new List<Job>
                        {
                           new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 10 /* Execuet Actn_Calf_F */)
                           {
                              Input =
                                 new XElement("SmsConf",
                                    new XAttribute("actntype", "getcredit")
                                 ),
                              AfterChangedOutput =
                                 new Action<object>((output) =>
                                 {
                                    xoutput = output as XDocument;
                                    //SendCreditCount_Txt.Text = xoutput.Descendants("SendCredit").FirstOrDefault().Value;
                                    //SendCreditAmnt_Txt.Text = xoutput.Descendants("SMS_SendFee").FirstOrDefault().Value;
                                    //ReceiveCreditCount_Txt.Text = xoutput.Descendants("RecieveCredit").FirstOrDefault().Value;
                                    //ReceiveCreditAmnt_Txt.Text = xoutput.Descendants("SMS_RecieveFee").FirstOrDefault().Value;                                                
                                 })
                           }
                        }
                     )
                  }
               )
            );

            return xoutput;
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
