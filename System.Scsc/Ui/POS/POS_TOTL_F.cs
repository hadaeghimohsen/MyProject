using SAMAN_PcToPos;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.POS
{
   partial class POS_TOTL_F
   {
      XElement xData;
      XDocument xPos;
      TTerminal _terminal;
      string xRslt;
      string rqtpcode,
             rqid,
             cashcode,
             fileno,
             amnt,
             formcaller,
             tabpage;
      string termno,
             cardno,
             flowno,
             refno,
             actndate;

      private TTerminal terminal
      {
         get
         {
            if (_terminal == null)
            {
               _terminal = new TTerminal();
               _terminal.ShowMessages = false;
               _terminal.XMLReceived += tp_XMLReceived;
            }
            return _terminal;
         }
      }

      private void tp_XMLReceived(object sender, XmlReceivedEventArgs e)
      {
         if (e.IsSuccessful)
         {
            xRslt = string.Format("<Pos>{0}</Pos>", e.XmlRecieve);
            xPos = XDocument.Parse(xRslt);
            termno = xPos.Element("TerminalId").Value;
            cardno = xPos.Element("CustomerPan").Value;
            flowno = xPos.Element("TraceNumber").Value;
            refno = xPos.Element("ReferenceNumber").Value;
            actndate = xPos.Element("DateTime").Value;
            var PosResult2Form = 
               new XElement("Pos",
                  new XElement("Request",
                     new XAttribute("rqid", rqid),
                     new XAttribute("rqtpcode", rqtpcode),
                     new XAttribute("fileno", fileno),
                     new XElement("Payment", 
                        new XAttribute("cashcode", cashcode),
                        new XAttribute("amnt", amnt),
                        new XElement("Payment_Method",
                           new XAttribute("termno", termno),
                           new XAttribute("cardno", cardno),
                           new XAttribute("flowno", flowno),
                           new XAttribute("refno", refno),
                           new XAttribute("actndate", actndate)
                        )
                     )
                  )
               );
            // ارسال اطلاعات به فرم ارسالی برای تایید پرداخت
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost", formcaller, 20 /* Pay_Oprt_F */, SendType.SelfToUserInterface) { Input = xPos };
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
         else
         {
            MessageBox.Show(e.PosResponse.Message + "\r\n" + string.Format("تاییدیه پرداخت مربوط به درخواست {0}، برای شماره پرونده {1} نا موفق بود. لطفا دوباره امتحان کنید.", rqid, fileno), "عدم تایید پرداخت", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
         }
      }

      private void SendToSerial(string msg)
      {
         OpenSerialPort();
         this.terminal.PrintFlag = (byte)3;
         terminal.WithHandShake = false;

         if (this.terminal.SendToCOM(msg))
         {
            
         }
         else
         {
            System.Windows.Forms.MessageBox.Show(terminal.XmlError);
         }
      }

      private void OpenSerialPort()
      {
         this.terminal.ClosePort();
         this.terminal.Port = "COM1";
         this.terminal.OpenPort();
      }
   }
}
