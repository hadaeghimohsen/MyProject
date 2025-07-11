﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using DevExpress.XtraEditors;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Linq;
using SSP1126.PcPos.BaseClasses;
using SSP1126.PcPos.Infrastructure;
using Intek.PcPosLibrary;
using POS_PC;
using VPCPOS;
using PosInterface;
using PcPosClassLibrary;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using Sadad;
using Sadad.PcPos.Core;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsPaymentPos : UserControl
   {
      public SettingsPaymentPos()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int? subsys;
      private string gtwymacadrs;
      private long? rqid;
      private string rqtpcode;
      private string router;
      private int? callback;

      private long? Tlid; // Transaction_Log

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4Settings */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         if (Pos_Device != null)
         {
            PosBs.DataSource = iProject.Pos_Devices.FirstOrDefault(p => p.PSID == Pos_Device.PSID);
         }

         if (Tb_Master.SelectedTab == tp_002)
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;

            TranBs.DataSource =
               iProject.Transaction_Logs.Where(
                  t => t.TRAN_DATE.Value.Date >= FromTranDate_Dt.Value.Value.Date &&
                       t.TRAN_DATE.Value.Date <= ToTranDate_Dt.Value.Value.Date &&
                       t.PAY_STAT == (PayConf_Rb.Checked ? "002" : (PayAll_Rb.Checked ? t.PAY_STAT : "001")) &&
                       t.POSD_PSID == pos.PSID

               );
         }

         requery = false;
      }

      private void PosInfo_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 37 /* Execute DoWork4SettingsNewPos */),
                     new Job(SendType.SelfToUserInterface, "SettingsNewPos", 10 /* Execute ActionCallWindow */){Input = pos}
                  }
               )
            );
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void PosPayment_Butn_Click(object sender, EventArgs e)
      {
         var pos = PosBs.Current as Data.Pos_Device;
         if (pos == null) return;

         Tlid = 0;
         PayResult_Lb.Appearance.Image = null;

         switch (pos.BANK_TYPE)
         {
            case "001":
               SamanPcPos();
               break;
            case "002":
               ParsianPcPos();
               break;
            case "003":
               MellatPcPos();
               break;
            case "004":
               MabnaCardAriaPcPos();
               break;
            case "005":
               AsanPardakhtPcPos();
               break;
            case "006":
               PasargadPcPos();
               break;
            case "007":
               SadadPcPos();
               break;
         }
      }

      private void SendCallBack2Router()
      {
         try
         {
            var tlog = iProject.Transaction_Logs.FirstOrDefault(t => t.TLID == Tlid);

            if (router == null || callback == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", string.Format("DataGuard:Program:{0}:{1}", iProject.Sub_Systems.FirstOrDefault(s => s.SUB_SYS == subsys).SCHM_NAME, router), (int)callback /* Execute CallBack Method  */, SendType.SelfToUserInterface)
               {
                  Input = 
                     new XElement("PosRespons",
                        new XAttribute("amnt", tlog.AMNT),
                        new XAttribute("termno", tlog.TERM_NO),
                        new XAttribute("tranno", tlog.SERL_NO),
                        new XAttribute("cardno", tlog.CARD_NO),
                        new XAttribute("flowno", tlog.FLOW_NO),
                        new XAttribute("refno", tlog.REF_NO),
                        new XAttribute("actndate", tlog.ISSU_DATE)
                     )
               }
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Back_Butn_Click(null, null);
         }
      }

      #region Saman Pos Bank
      #region Variable
      private PcPosFactory _PcPosFactory;
      private TransactionType _TranType = TransactionType.Purchase;
      #endregion
      private void SamanPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;

            if (_PcPosFactory == null)
            {
               _PcPosFactory = new PcPosFactory();
               _PcPosFactory.CardSwiped += SamanPosClient_CardSwiped;
               _PcPosFactory.PosResultReceived += SamanPosClient_PosResultReceived;
            }

            _TranType = TransactionType.Purchase;

            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  _PcPosFactory.SetCom(pos.COMM_PORT);
                  break;
               case "002":
                  _PcPosFactory.SetLan(pos.IP_ADRS);
                  break;
            }

            _PcPosFactory.Initialization(ResponseLanguage.Persian, 0, AsyncType.Sync);

            SSP1126.PcPos.Infrastructure.PosResult posResult = _PcPosFactory.PosStarterPurchaseInit();
            if (posResult != null)
               SamanPosClient_CardSwiped(posResult);
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
         }
      }

      private void SamanPosClient_CardSwiped(SSP1126.PcPos.Infrastructure.PosResult posResult)
      {
         try
         {
            if (posResult == null) return;

            var pos = PosBs.Current as Data.Pos_Device;
            if(pos.ACTN_TYPE == "001")
            {
               // حساب شخصی
               Threading.Thread.Sleep(1000);
               
               posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0, "", "", null, -1, null, -1);
            }
            else
            {
               // حساب دولتی
               if(pos.BILL_FIND_TYPE == "001")
               {
                  // شناسه مشترک
                  Threading.Thread.Sleep(1000);
                  
                  posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0, "", pos.BILL_NO, null, -1, null, -1);
               }
               else
               {
                  // شناسه متفاوت
                  var useraccesspos = iProject.User_Access_Pos.FirstOrDefault(uap => uap.User.USERDB.ToUpper() == CurrentUser.ToUpper() && uap.POSD_PSID == pos.PSID);
                  if ((useraccesspos.BILL_NO ?? "") == "")
                     useraccesspos.BILL_NO = pos.BILL_NO;
                  Threading.Thread.Sleep(1000);
                  //posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0);
                  posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0, "", useraccesspos.BILL_NO, null, -1, null, -1);
               }
            }            

            Tlid = SamanPcPos_SaveTransactionLog(posResult);

            if (posResult != null)
               SamanPosClient_PosResultReceived(posResult);
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }

      private void SamanPosClient_PosResultReceived(SSP1126.PcPos.Infrastructure.PosResult posResult)
      {
         try
         {
            if (posResult == null) return;

            Tlid = SamanPcPos_SaveTransactionLog(posResult);

            switch (posResult.ResponseCode)
            {
               case "00":
                  PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
                  SendCallBack2Router();
                  break;
               default:
                  PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
                  break;
            }
         }
         catch (Exception)
         {
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
         }
      }

      private long? SamanPcPos_SaveTransactionLog(SSP1126.PcPos.Infrastructure.PosResult posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", posResult.ResponseCode ?? ""),
                     new XAttribute("respdesc", posResult.ResponseDescription ?? ""),
                     new XAttribute("cardno", posResult.CardNumberMask ?? ""),
                     new XAttribute("termno", posResult.TerminalId ?? ""),
                     new XAttribute("serlno", posResult.SerialId ?? ""),
                     new XAttribute("flowno", posResult.TraceNumber ?? ""),
                     new XAttribute("refno", posResult.RRN ?? "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      #endregion

      #region Parsian Pos Bank
      #region Variable
      BTLV _Btlv;
      #endregion
      private void ParsianPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;
            
            _Btlv = new BTLV();            

            Tlid = ParsianPcPos_SaveTransactionLog(_Btlv);
            bool result = false;
            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  break;
               case "002":                  
                  result = _Btlv.SetLan(pos.IP_ADRS, (int)pos.BAND_RATE, Convert.ToInt64( Amnt_Txt.EditValue ), pos.PRNT_CUST, pos.PRNT_SALE, "", "");
                  break;
            }
            Tlid = ParsianPcPos_SaveTransactionLog(_Btlv);

            if (result)
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
               SendCallBack2Router();
            }
            else
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
         }
      }

      private long? ParsianPcPos_SaveTransactionLog(BTLV posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", posResult.GetRespCode() ?? ""),
                     new XAttribute("respdesc", ""),
                     new XAttribute("cardno", posResult.GetCardNo() ?? ""),
                     new XAttribute("termno", posResult.GetTerminalId() ?? ""),
                     new XAttribute("serlno", posResult.GetSerialId() ?? ""),
                     new XAttribute("flowno", posResult.GetTraceNo() ?? ""),
                     new XAttribute("refno", posResult.GetRRN() ?? "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      #endregion

      #region Mellat Pos Bank
      #region Variable
      Transaction _MellatPcPos;
      #endregion
      private void MellatPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;
            
            #region Old Version
            //_MellatPcPos = new Transaction();

            //Tlid = MellatPcPos_SaveTransactionLog(_MellatPcPos);
            //switch (pos.POS_CNCT_TYPE)
            //{
            //   case "001":
            //      _MellatPcPos.PC_PORT_Name = pos.COMM_PORT;
            //      _MellatPcPos.PC_PORT_BaudRate = (int)pos.BAND_RATE;
            //      break;
            //   case "002":
            //      break;
            //}

            //_MellatPcPos.PC_PORT_ReadTimeout = 180000;
            //Transaction.return_codes retCode;
            //do
            //{
            //   retCode = _MellatPcPos.Debits_Goods_And_Service(Amnt_Txt.EditValue.ToString(), "", "");

            //   Tlid = MellatPcPos_SaveTransactionLog(_MellatPcPos);

            //   if(retCode == Transaction.return_codes.RET_OK)
            //   {
            //      PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
            //      SendCallBack2Router();
            //   }
            //   else
            //   {
            //      PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
            //   }

            //} while (Globals.POSPC_CommunicationType.ToUpper() == "TCP/IP" && !_MellatPcPos.IS_FATAL_ERROR(retCode));
            #endregion

            TcpClient client = null;
            ServicePointManager.Expect100Continue = false;
            byte[] resvCommand = new byte[10025];
            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  client = new System.Net.Sockets.TcpClient("127.0.0.1", (int)pos.BAND_RATE); // Create a new connection  
                  break;
               case "002":
                  client = new System.Net.Sockets.TcpClient(pos.IP_ADRS, (int)pos.BAND_RATE); // Create a new connection  
                  break;
            }
            if(!client.Connected)
            {
               throw new Exception("Please Check Service Port");
            }

            Tlid = MellatPcPos_SaveTransactionLog(null);
            NetworkStream stream = client.GetStream();
            string str_comm = "" + "{\"ServiceCode\" : \"" + "1";
            if (Convert.ToInt64( Amnt_Txt.EditValue ) > 0)
               str_comm += "\" ,\"Amount\" : \"" + Amnt_Txt.EditValue.ToString();
            //if (txtDebitPayerId.Text.Length > 0)
            //   str_comm += "\",\"PayerId\":\"" + txtDebitPayerId.Text;
            //if (txtDebitMsg.Text.Length > 0)
            //   str_comm += "\",\"MerchantMsg\":\"" + txtDebitMsg.Text;
            //if (txtDebitPcID.Text.Length > 0)
            //   str_comm += "\",\"PcID\":\"" + txtDebitPcID.Text;
            str_comm += "\"}";

            //string str_comm = "" + "{\"ServiceCode\" :\"" + "1" + "\",\"Amount\":\"" + txtDebitAmount.Text + "\",\"PayerId\":\"" + txtDebitPayerId.Text + "\",\"MerchantMsg\":\"" + txtDebitMsg.Text + "\",\"PcID\":\"" + txtDebitPcID.Text + "\"}";
            byte[] sendCommand = System.Text.Encoding.ASCII.GetBytes(str_comm);
            stream.Write(sendCommand, 0, sendCommand.Length);
            stream.ReadTimeout = 180000;
            int recvSize = stream.Read(resvCommand, 0, resvCommand.Length);

            string jsonStr = Encoding.UTF8.GetString(resvCommand);
            Dictionary<String, String> posResult = JsonConvert.DeserializeObject<Dictionary<String, String>>(jsonStr);
            
            Tlid = MellatPcPos_SaveTransactionLog(posResult);

            if (posResult.FirstOrDefault(p => p.Key == "ReturnCode").Value == "100")
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
               SendCallBack2Router();
            }
            else
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
            }

            client.Close();
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
         }
      }

      private long? MellatPcPos_SaveTransactionLog(Dictionary<string, string> posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", posResult == null ? "" : posResult.FirstOrDefault(p => p.Key == "ReturnCode").Value ?? ""),
                     new XAttribute("respdesc", ""),
                     new XAttribute("cardno", posResult == null ? "" : posResult.FirstOrDefault(p => p.Key == "PAN").Value ?? ""),
                     new XAttribute("termno", posResult == null ? "" : posResult.FirstOrDefault(p => p.Key == "TerminalNo").Value ?? ""),
                     new XAttribute("serlno", posResult == null ? "" : posResult.FirstOrDefault(p => p.Key == "SerialTransaction").Value ?? ""),
                     new XAttribute("flowno", posResult == null ? "" : posResult.FirstOrDefault(p => p.Key == "TraceNumber").Value ?? ""),
                     new XAttribute("refno", "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      
      #endregion

      #region Mabna Card Aria
      #region Variable

      #endregion
      private void MabnaCardAriaPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;

            clsCommunication clsCommunicationObj = new clsCommunication();

            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  clsCommunicationObj.ConnType = (int)clsCommunication.connectionType.COM_USB;
                  clsCommunicationObj.PortNumber = int.Parse(pos.COMM_PORT.Remove(0, 3));
                  clsCommunicationObj.BaundRate = (int)pos.BAND_RATE;
                  break;
               case "002":
                  clsCommunicationObj.ConnType = (int)clsCommunication.connectionType.ETHERNET;
                  clsCommunicationObj.IPAddress = pos.IP_ADRS;
                  clsCommunicationObj.IPPort = (int)pos.BAND_RATE;
                  break;
            }
            clsCommunicationObj.TimeOut = 180000;

            clsMessage _MabnaPcPos = new clsMessage();
            clsMessage.request_s req = new clsMessage.request_s();
            Tlid = MabnaCardAria_SaveTransactionLog(_MabnaPcPos);
            req.msgTyp = clsMessage.msgType.Sale;
            req.bankId = 1;
            req.terminalID = "";
            req.amount = Amnt_Txt.EditValue.ToString();
            _MabnaPcPos.request = req;

            int retCode = _MabnaPcPos.SendMessage(0);
            if (retCode != 0) { throw new Exception("خطا در ارسال اطلاعات"); }

            retCode = _MabnaPcPos.ReceiveMessage(0);
            Tlid = MabnaCardAria_SaveTransactionLog(_MabnaPcPos);
            if(_MabnaPcPos.response.AppResponseCode == "00")
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
               SendCallBack2Router();
            }
            else
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
            }

         }catch(Exception exc)
         {
            MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
         }
      }

      private long? MabnaCardAria_SaveTransactionLog(clsMessage posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", posResult.response.AppResponseCode ?? ""),
                     new XAttribute("respdesc", ""),
                     new XAttribute("cardno", posResult.response.PAN ?? ""),
                     new XAttribute("termno", posResult.response.TerminalID ?? ""),
                     new XAttribute("serlno", posResult.response.STAN ?? ""),
                     new XAttribute("flowno", posResult.response.SystemTraceNumber ?? ""),
                     new XAttribute("refno", posResult.response.SystemTraceNumber ?? "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      #endregion

      #region Asan Pardakht
      #region Variable
      private PCPos _AsanPardakhtPcPos;
      #endregion
      private void AsanPardakhtPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;

            _AsanPardakhtPcPos = new PCPos();

            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  _AsanPardakhtPcPos.InitSerial(pos.COMM_PORT, (int)pos.BAND_RATE);
                  break;
               case "002":
                  _AsanPardakhtPcPos.InitLAN(pos.IP_ADRS, (int)pos.BAND_RATE);
                  break;
               default:
                  break;
            }

            Tlid = AsanPardakhtPcPos_SaveTransactionLog(new PaymentResult());

            var posResult = _AsanPardakhtPcPos.DoSyncPayment(Amnt_Txt.EditValue.ToString(), "", "", DateTime.Now);

            Tlid = AsanPardakhtPcPos_SaveTransactionLog(posResult);

            if (posResult.ErrorCode == 0)
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
               SendCallBack2Router();
            }
            else
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;            
         }
      }

      private long? AsanPardakhtPcPos_SaveTransactionLog(PaymentResult posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", (Math.Abs(posResult.ErrorCode)).ToString("D2") ?? ""),
                     new XAttribute("respdesc", ""),
                     new XAttribute("cardno", posResult.CardNumber ?? ""),
                     new XAttribute("termno", posResult.TerminalId ?? ""),
                     new XAttribute("serlno", posResult.MessageId ?? ""),
                     new XAttribute("flowno", posResult.Stan ?? ""),
                     new XAttribute("refno", posResult.RRN ?? "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      #endregion

      #region Pasargad
      #region Variable
      PCPOS _PasargadPcPos;
      #endregion
      private void PasargadPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;
            
            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  _PasargadPcPos = new PCPOS(pos.COMM_PORT);
                  break;
               case "002":
                  _PasargadPcPos = new PCPOS((int)pos.BAND_RATE, pos.IP_ADRS);
                  break;
               default:
                  break;
            }

            Tlid = PasargadPcPos_SaveTransactionLog(new RecievedData());

            _PasargadPcPos.DataRecieved += _PasargadPcPos_DataRecieved;
            _PasargadPcPos.Sale(Convert.ToInt64(Amnt_Txt.EditValue));
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
            _PasargadPcPos.Close();
         }
      }

      private void _PasargadPcPos_DataRecieved(object sender, DataRecievedArgs e)
      {
         try
         {
            Tlid = PasargadPcPos_SaveTransactionLog(e.recievedData);

            if (!e.recievedData.HasError && e.recievedData.ReferenceNumber != "")
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
               SendCallBack2Router();
            }               
            else
            {
               PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
            }
         }catch(Exception exc)
         {
            throw exc;
         }
         finally
         {
            _PasargadPcPos.Finish();
            _PasargadPcPos.Close();
         }
      }

      private long? PasargadPcPos_SaveTransactionLog(RecievedData posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", (Math.Abs(posResult.ErrorCode)).ToString("D2") ?? ""),
                     new XAttribute("respdesc", ""),
                     new XAttribute("cardno", posResult.CardNumber ?? ""),
                     new XAttribute("termno", posResult.TerminalCode ?? ""),
                     new XAttribute("serlno", posResult.SequenceNumber ?? ""),
                     new XAttribute("flowno", posResult.Stan ?? ""),
                     new XAttribute("refno", posResult.ReferenceNumber ?? "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      #endregion

      #region Sadad
      #region Variable
      PcPosBusiness _SadadPcPos;
      #endregion
      private void SadadPcPos()
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return;

            switch (pos.POS_CNCT_TYPE)
            {
               case "001":
                  _SadadPcPos = new PcPosBusiness() { ConnectionType = PcPosConnectionType.Serial, ComPortName = pos.COMM_PORT, Port = (int)pos.BAND_RATE};
                  break;
               case "002":
                  _SadadPcPos = new PcPosBusiness() { ConnectionType = PcPosConnectionType.Lan,  Ip = pos.IP_ADRS, Port = (int)pos.BAND_RATE};
                  break;
               default:
                  break;
            }

            Tlid = SadadPcPos_SaveTransactionLog(new Sadad.PcPos.Core.PosResult());

            //_SadadPcPos.OnSaleResult += _SadadPcPos_OnSaleResult;
            _SadadPcPos.Amount = Amnt_Txt.EditValue.ToString();
            _SadadPcPos.DeviceType = DeviceType.BlueBird;

            if (pos.ACTN_TYPE == "001")
            {
               // حساب شخصی
               //Threading.Thread.Sleep(1000);

               //_SadadPcPos.SyncSaleTransaction();
            }
            else
            {
               // حساب دولتی
               if (pos.BILL_FIND_TYPE == "001")
               {
                  // شناسه مشترک
                  //Threading.Thread.Sleep(1000);

                  //posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0, "", pos.BILL_NO, null, -1, null, -1);
                  _SadadPcPos.SetSaleId(pos.BILL_NO);
               }
               else
               {
                  // شناسه متفاوت
                  var useraccesspos = iProject.User_Access_Pos.FirstOrDefault(uap => uap.User.USERDB.ToUpper() == CurrentUser.ToUpper() && uap.POSD_PSID == pos.PSID);
                  if ((useraccesspos.BILL_NO ?? "") == "")
                     useraccesspos.BILL_NO = pos.BILL_NO;
                  Threading.Thread.Sleep(1000);
                  //posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0);
                  //posResult = _PcPosFactory.PosStarterPurchase(Amnt_Txt.EditValue.ToString(), null, "", "", 0, "", useraccesspos.BILL_NO, null, -1, null, -1);
                  _SadadPcPos.SetSaleId(useraccesspos.BILL_NO);
               }
            }

            _SadadPcPos_OnSaleResult(null, _SadadPcPos.SyncSaleTransaction());
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
            _SadadPcPos.Dispose();
         }
      }

      private void _SadadPcPos_OnSaleResult(object sender, Sadad.PcPos.Core.PosResult posResult)
      {
         try
         {
            if (posResult == null) return;

            Tlid = SadadPcPos_SaveTransactionLog(posResult);

            switch (posResult.ResponseCode)
            {
               case "00":
                  PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1603;
                  SendCallBack2Router();
                  break;
               default:
                  PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
                  break;
            }
         }
         catch (Exception)
         {
            PayResult_Lb.Appearance.Image = System.DataGuard.Properties.Resources.IMAGE_1577;
         }
      }

      private long? SadadPcPos_SaveTransactionLog(Sadad.PcPos.Core.PosResult posResult)
      {
         try
         {
            var pos = PosBs.Current as Data.Pos_Device;
            if (pos == null) return null;

            XElement xPcPos =
                  new XElement("PosRequest",
                     new XAttribute("psid", pos.PSID),
                     new XAttribute("subsys", subsys ?? 0),
                     new XAttribute("gtwymacadrs", gtwymacadrs),
                     new XAttribute("rqid", rqid ?? 0),
                     new XAttribute("rqtpcode", rqtpcode ?? ""),
                     new XAttribute("tlid", Tlid),
                     new XAttribute("amnt", Amnt_Txt.EditValue),
                     new XAttribute("respcode", posResult.ResponseCode ?? ""),
                     new XAttribute("respdesc", posResult.ResponseCodeMessage ?? ""),
                     new XAttribute("cardno", posResult.CardNo ?? ""),
                     new XAttribute("termno", posResult.TerminalId ?? ""),
                     new XAttribute("serlno", posResult.MerchantId ?? ""),
                     new XAttribute("flowno", posResult.TransactionNo ?? ""),
                     new XAttribute("refno", posResult.Rrn ?? "")
                  );
            iProject.SaveTransactionLog(ref xPcPos);

            Tlid = Convert.ToInt64(xPcPos.Attribute("tlid").Value);
            return Tlid;
         }
         catch (Exception exc)
         {
            throw exc;
         }
      }
      #endregion
   }
}
