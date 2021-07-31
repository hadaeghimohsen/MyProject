using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.RoboTech.Ui.DevelopmentApplication
{
   partial class CASH_CNTR_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iRoboTechDataContext iRoboTech;
      private string ConnectionString;
      private List<long?> Fga_Ugov_U;
      private XElement HostNameInfo;
      private string CurrentUser;

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
               CheckSecurity(job);
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 20:
               Pay_Oprt_F(job);
               break;
            case 40:
               CordinateGetSet(job);
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

         if (keyData == Keys.Enter)
         {
            SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.F2)
         {
            switch (Master000_Tc.SelectedTab.Name)
            {
               case "tp_001":
                  PayCrntOrdrFrm1_Butn_Click(null, null);
                  break;
               case "tp_003":
                  OrdrCashPay_Butn_Click(null, null);
                  break;
            }
         }
         else if (keyData == Keys.F3)
         {
            OrdrPosPay_Butn_Click(null, null);
         }
         else if (keyData == Keys.F5)
         {            
            AcptPay_Butn_Click(null, null);
         }
         else if(keyData == Keys.F6)
         {
            BackPay_Butn_Click(null, null);
         }
         else if(keyData == Keys.F7)
         {
            if(OrdrWletCashPay_Butn.Enabled)
               OrdrWletCashPay_Butn_Click(null, null);
         }
         else if(keyData == Keys.F8)
         {
            if(OrdrWletCredPay_Butn.Enabled)
               OrdrWletCredPay_Butn_Click(null, null);
         }
         else if(keyData == Keys.Add)
         {
            switch (Master000_Tc.SelectedTab.Name)
            {
               case "tp_001":
                  colActnSrbtFrm1_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(colActnSrbtFrm1.Buttons[0]));
                  break;
               case "tp_003":
                  colActnRbprFrm1_ButtonClick(null, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(colActnRbprFrm1.Buttons[0]));
                  break;
            }
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

         ConnectionString = GetConnectionString.Output.ToString();
         iRoboTech = new Data.iRoboTechDataContext(GetConnectionString.Output.ToString());
         CurrentUser = iRoboTech.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         Fga_Ugov_U = (iRoboTech.FGA_UGOV_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         //);

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
               //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> {string.Format("RoboTech:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PastOnWall */) { Input = this }               
            });
         _DefaultGateway.Gateway(_Paint);

         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("RoboTech:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "right:in-screen:stretch:center"} }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
               })
            );
         //job.Next =
         //   new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
         //      new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
         //         new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void CheckSecurity(Job job)
      {
         Job _InteractWithJob =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> {"<Privilege>79</Privilege><Sub_Sys>12</Sub_Sys>", "DataGuard"},
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              #region Show Error
                              job.Status = StatusType.Failed;
                              MessageBox.Show(this, "خطا - عدم دسترسی به ردیف 79 امنیتی", "خطا دسترسی");
                              #endregion                           
                           })
                        },
                        #endregion                        
                     })                     
                  });
            _DefaultGateway.Gateway(_InteractWithJob);
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         VPosBs1.DataSource = iRoboTech.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;

         DRcmtBs.DataSource = iRoboTech.D_RCMTs;
         DAmutBs.DataSource = iRoboTech.D_AMUTs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         UpSrbtOrdr25Frm1_Butn.ImageVisiable = DownSrbtOrdr25Frm1_Butn.ImageVisiable = SrbtOrdrHistFrm1_Butn.ImageVisiable = true;
         Ship001_Butn.ImageVisiable = Ship002_Butn.ImageVisiable = Ship003_Butn.ImageVisiable = Ship004_Butn.ImageVisiable = true;
         Execute_Query();
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void Pay_Oprt_F(Job job)
      {
         try
         {
            XElement RcevXData = job.Input as XElement;

            //var ordr = Ordr4Stp2Bs.Current as Data.Order;
            //if (ordr == null) return;

            //var robo = RoboBs.Current as Data.Robot;

            //var rqtpcode = ordr.ORDR_TYPE;//RcevXData.Element("PosRespons").Attribute("rqtpcode").Value;
            //var rqid = ordr.CODE;//RcevXData.Element("PosRespons").Attribute("rqid").Value;
            //var fileno = ordr.SRBT_SERV_FILE_NO;//RcevXData.Element("PosRespons").Attribute("fileno").Value;
            ////var cashcode = rqst.Payments.FirstOrDefault().CASH_CODE;//RcevXData.Element("PosRespons").Element("Payment").Attribute("cashcode").Value;
            //var amnt = Convert.ToInt64(RcevXData.Attribute("amnt").Value);
            //var termno = RcevXData.Attribute("termno").Value;
            //var tranno = RcevXData.Attribute("tranno").Value;
            //var cardno = RcevXData.Attribute("cardno").Value;
            //var flowno = RcevXData.Attribute("flowno").Value;
            //var refno = RcevXData.Attribute("refno").Value;
            //var actndate = RcevXData.Attribute("actndate").Value;

            //if (robo.AMNT_TYPE == "002")
            //   amnt /= 10;

            // Print Default Active Reports
            //DfltPrnt002_Butn_Click(null, null);

            #region Save Amount With Connect to POS AND Call Back
            // Do Some things
            //var xResult = new XElement("Respons");
            //iRoboTech.Analisis_Message_P(
            //   new XElement("Robot",
            //      new XAttribute("token", ordr.Robot.TKON_CODE),
            //      new XElement("Message",
            //            new XAttribute("cbq", "002"),
            //            new XAttribute("ussd", "*0*3*3#"),
            //            new XAttribute("childussd", ""),
            //            new XAttribute("chatid", ordr.CHAT_ID),
            //            new XAttribute("elmntype", "001"),
            //            new XElement("Text",
            //                new XAttribute("param", string.Format("howinccashwlet,{0}", ordr.AMNT_TYPE == "001" ? ordr.DEBT_DNRM : ordr.DEBT_DNRM * 10)),
            //                new XAttribute("postexec", "lessaddwlet"),
            //                "addamntwlet"
            //            )
            //      )
            //   ),
            //   ref xResult
            //);

            //iRoboTech = new Data.iRoboTechDataContext(ConnectionString);

            //var ordr15 = iRoboTech.Orders.Where(o => o.Service_Robot == ordr.Service_Robot && o.ORDR_TYPE == "015" && o.ORDR_STAT == "001" && o.DEBT_DNRM == ordr.DEBT_DNRM).FirstOrDefault();
            //if (ordr15 == null) { MessageBox.Show(this, "متاسفانه در ثبت مبلغ کارتخوان خطایی پیش آمده لطفا دوباره امتحان کنید"); return; }

            //iRoboTech.SAVE_PYMT_P(
            //   new XElement("Payment",
            //       new XAttribute("ordrcode", ordr15.CODE),
            //       new XAttribute("termno", termno),
            //       new XAttribute("tranno", tranno),
            //       new XAttribute("cardno", cardno),
            //       new XAttribute("txid", flowno),
            //       new XAttribute("refno", refno),
            //       new XAttribute("totlamnt", ordr15.DEBT_DNRM),
            //       new XAttribute("autochngamnt", "001"),
            //       new XAttribute("rcptmtod", "003")
            //   ),
            //   ref xResult
            //);

            //iRoboTech = new Data.iRoboTechDataContext(ConnectionString);
            //ordr15 = iRoboTech.Orders.FirstOrDefault(o => o.CODE == ordr15.CODE);
            //if (ordr15.ORDR_STAT != "004") { MessageBox.Show(this, "متاسفانه در ثبت مبلغ نقدی خطایی پیش آمده لطفا دوباره امتحان کنید"); return; }

            //iRoboTech.SAVE_WLET_P(
            //   new XElement("Wallet_Detail",
            //       new XAttribute("ordrcode", ordr.CODE),
            //       new XAttribute("rbid", ordr.Robot.RBID),
            //       new XAttribute("chatid", ordr.CHAT_ID),
            //       new XAttribute("oprttype", "add"),
            //       new XAttribute("wlettype", "002")
            //   ),
            //   ref xResult
            //);

            //iRoboTech.Analisis_Message_P(
            //   new XElement("Robot",
            //      new XAttribute("token", ordr.Robot.TKON_CODE),
            //      new XElement("Message",
            //            new XAttribute("cbq", "002"),
            //            new XAttribute("ussd", "*0#"),
            //            new XAttribute("childussd", ""),
            //            new XAttribute("chatid", ordr.CHAT_ID),
            //            new XAttribute("elmntype", "001"),
            //            new XElement("Text",
            //                new XAttribute("param", ordr.CODE),
            //                new XAttribute("postexec", "lessfinlcart"),
            //                "finalcart"
            //            )
            //      )
            //   ),
            //   ref xResult
            //);
            #endregion
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }
      /// <summary>
      /// Code 40
      /// </summary>
      /// <param name="job"></param>
      private void CordinateGetSet(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput != null)
         {
            var ordr = Ordr4Bs.Current as Data.Order;

            if (xinput.Attribute("outputtype").Value == "destpostadrs")
            {
               //MessageBox.Show(xinput.ToString());

               var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
               var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);

               if (cordx != ordr.CORD_X && cordy != ordr.CORD_Y)
               {
                  // Call Update Service_Public
                  try
                  {
                     ordr.HOW_SHIP = "002";
                     ordr.CORD_X = cordx;
                     ordr.CORD_Y = cordy;
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     MessageBox.Show(exc.Message);
                  }
               }
            }
            else if (xinput.Attribute("outputtype").Value == "sorcpostadrs")
            {
               //MessageBox.Show(xinput.ToString());

               var cordx = Convert.ToDouble(xinput.Attribute("cordx").Value);
               var cordy = Convert.ToDouble(xinput.Attribute("cordy").Value);

               if (cordx != ordr.SORC_CORD_X && cordy != ordr.SORC_CORD_Y)
               {
                  // Call Update Service_Public
                  try
                  {
                     ordr.SORC_CORD_X = cordx;
                     ordr.SORC_CORD_Y = cordy;
                     requery = true;
                  }
                  catch (Exception exc)
                  {
                     MessageBox.Show(exc.Message);
                  }
               }
            }
         }

         job.Status = StatusType.Successful;
      }
   }
}
