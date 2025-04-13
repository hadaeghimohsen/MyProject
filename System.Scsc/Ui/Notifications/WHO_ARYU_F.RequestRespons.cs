using System;
using System.Collections.Generic;
using System.Drawing;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Notifications
{
   partial class WHO_ARYU_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      //private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      int Sleeping = 1;
      int step = 15;
      bool isPainted = false;
      string fileno;
      long? attncode = 0;
      bool gateControl = false;
      private short? mbsprwno;
      private string formcaller = "";
      private bool isFirstLoaded = false;
      private string CurrentUser;
      private Data.Setting _stng;
      private XElement HostNameInfo;

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
               OpenDrawer(job);
               break;
            case 06:
               CloseDrawer(job);
               break;
            case 07:
               LoadData(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 21:
               Payg_Oprt_F(job);
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

         if (keyData == Keys.Escape)
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

            switch (formcaller)
            {               
               case "ATTN_DAYN_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {
                           new Job(SendType.Self, 141 /* Execute Attn_Dayn_F */),
                           //new Job(SendType.SelfToUserInterface, "ATTN_DAYN_F", 10 /* Execute Actn_CalF_F*/ )
                        })
                  );
                  break;
               default:
                  break;
            }

            switch (PlayHappyBirthDate_Butn.Tag.ToString())
            {
               case "play":
                  PlayHappyBirthDate_Butn.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }

            switch (PlayDurtAttn_Butn.Tag.ToString())
            {
               case "play":
                  PlayHappyBirthDate_Butn.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }

            switch (PlayDebtAlrm_Lb.Tag.ToString())
            {
               case "play":
                  PlayDebtAlrm_Lb.Tag = "stop";
                  new Threading.Thread(StopSound).Start();
                  break;
            }

            //job.Next =
            //   new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
            //if (!(Btn_Search.Focused))
            //   SendKeys.Send("{TAB}");
         }
         else if(keyData == Keys.F5)
         {
            Butn_Zoom_Click(null, null);
         }
         else if(keyData == Keys.F8)
         {
            Butn_TakePicture_Click(null, null);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());
         CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         //Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         //Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void OpenDrawer(Job job)
      {
         int width = 0;

         for (; ; )
         {
            if (width + step <= Width)
            {
               Invoke(new Action(() =>
               {
                  Left += step;
                  width += step;
               }));
               Thread.Sleep(Sleeping);
            }
            else
               break;
         }
         if(_stng == null)
            _stng = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
         if (_stng.ATTN_NOTF_STAT == "002" && _stng.ATTN_NOTF_CLOS_TYPE == "002")
         {
            Thread.Sleep((int)_stng.ATTN_NOTF_CLOS_INTR);
            if(isPainted)
               RqstBnExit1_Click(null, null);
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void CloseDrawer(Job job)
      {
         int width = Width;

         for (; ; )
         {
            if (width - step >= 0)
            {
               Invoke(new Action(() =>
               {
                  Left -= step;
                  width -= step;
               }));
               Thread.Sleep(Sleeping);
            }
            else
               break;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private new void Paint(Job job)
      {
         isPainted = true;
         //Job _Paint = new Job(SendType.External, "Desktop",
         //   new List<Job>
         //   {
         //      new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
         //      new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
         //   });
         //_DefaultGateway.Gateway(_Paint);

         Job _Paint = new Job(SendType.External, "Desktop",
            new List<Job>
            {
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}",GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "left:in-screen:default:center"} }               
            });
         _DefaultGateway.Gateway(_Paint);

         Enabled = true;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void UnPaint(Job job)
      {
         isPainted = false;
         //_DefaultGateway.Gateway(
         //   new Job(SendType.External, "Localhost",
         //      new List<Job>
         //      {
         //         new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
         //         new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
         //         new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
         //      })
         //   );
         job.Next =
               new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */,
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */,
                     new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)) { Input = this });

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
         if (isFirstLoaded) goto finishcommand;
         isFirstLoaded = true;

         DYsnoBs.DataSource = iScsc.D_YSNOs;
         DAttpBs.DataSource = iScsc.D_ATTPs;
         DRcmtBs.DataSource = iScsc.D_RCMTs;
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;


         finishcommand:
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cm => cm.MTOD_STAT == "002");
         

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         try
         {
            var xinput = job.Input as XElement;

            if (xinput.Attribute("cmndtype") != null)
            {
               if (xinput.Attribute("cmndtype").Value == "sendparam")
               {
                  WristBand_Txt.EditValue = xinput.Attribute("enroll").Value;
                  if(Convert.ToBoolean(xinput.Attribute("checkforce").Value))
                  {
                     var _attn = iScsc.Attendance_Wrists.FirstOrDefault(aw => aw.ATNW_FNGR_PRNT_DNRM == WristBand_Txt.Text && aw.STAT == "001").Attendance;
                     if (_attn != null)
                     {
                        attncode = _attn.CODE;
                        fileno = _attn.FIGH_FILE_NO.ToString();
                        AttnDate_Date.Value = _attn.ATTN_DATE;
                        mbsprwno = _attn.MBSP_RWNO_DNRM;

                        _DefaultGateway.Gateway(
                           new Job(SendType.External, "localhost",
                              new List<Job>
                              {
                                 new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */)
                              })
                        );

                        Execute_Query();
                     }
                  }
                  WristBand_Txt_ButtonClick(WristBand_Txt, new DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(WristBand_Txt.Properties.Buttons[0]));
                  Info_Tb.SelectedTabPage = Tp_DefWrst;
               }
            }
            else 
            {
               Info_Tb.SelectedTabPage = Tp_OldAttn;
               //Lbl_Dresser.BackColor = SystemColors.Control;
               fileno = xinput.Attribute("fileno").Value;
               AttnDate_Date.Value = Convert.ToDateTime(xinput.Attribute("attndate").Value);

               // 1396/07/16 * اضافه شدن 
               if (xinput.Attribute("attncode") != null)
                  attncode = Convert.ToInt64(xinput.Attribute("attncode").Value);
               else
                  attncode = null;

               if (xinput.Attribute("mbsprwno") != null)
                  mbsprwno = Convert.ToInt16(xinput.Attribute("mbsprwno").Value);
               else
                  mbsprwno = null;
            }            

            if (xinput.Attribute("formcaller") != null)
               formcaller = xinput.Attribute("formcaller").Value;
            else
               formcaller = "";

            if (xinput.Attribute("gatecontrol") != null)
               gateControl = true;
            else
               gateControl = false;
         }
         catch { }
         finally
         {
            Execute_Query();
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void Payg_Oprt_F(Job job)
      {
         try
         {
            XElement RcevXData = job.Input as XElement;

            var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

            var paydebt = Convert.ToInt64(RcevXData.Attribute("amnt").Value);
            var termno = RcevXData.Attribute("termno").Value;
            var tranno = RcevXData.Attribute("tranno").Value;
            var cardno = RcevXData.Attribute("cardno").Value;
            var flowno = RcevXData.Attribute("flowno").Value;
            var refno = RcevXData.Attribute("refno").Value;
            var actndate = RcevXData.Attribute("actndate").Value;

            if (regl.AMNT_TYPE == "002")
               paydebt /= 10;

            var figh = (AttnBs1.Current as Data.Attendance).Fighter1 as Data.Fighter;

            var vf_SavePayment =
               iScsc.VF_Save_Payments(null, figh.FILE_NO)
               .Where(p => ((p.SUM_EXPN_PRIC + p.SUM_EXPN_EXTR_PRCT) - (p.SUM_RCPT_EXPN_PRIC + p.SUM_PYMT_DSCN_DNRM)) > 0).OrderBy(p => p.PYMT_CRET_DATE.Value.Date);

            foreach (var pymt in vf_SavePayment)
            {
               var debt = (long)((pymt.SUM_EXPN_PRIC + pymt.SUM_EXPN_EXTR_PRCT) - (pymt.SUM_RCPT_EXPN_PRIC + pymt.SUM_PYMT_DSCN_DNRM));
               long amnt = 0;

               if (debt > paydebt)
                  // اگر بدهی صورتحساب بیشتر از مبلغ پرداخت مشتری باشد
                  amnt = paydebt;
               else
                  // اگر بدهی صورتحساب با مبلغ پرداخت مشتری مساوی یا کمتر باشد
                  amnt = debt;

               iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", pymt.CASH_CODE),
                           new XAttribute("rqstrqid", pymt.RQID),
                           new XAttribute("amnt", amnt),
                           new XAttribute("rcptmtod", "003"),
                           new XAttribute("termno", termno),
                           new XAttribute("tranno", tranno),
                           new XAttribute("cardno", cardno),
                           new XAttribute("flowno", flowno),
                           new XAttribute("refno", refno),
                           new XAttribute("actndate", actndate)
                        )
                     )
                  )
               );

               paydebt -= amnt;
               if (paydebt == 0) break;
            }            
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            Execute_Query();
         }
         job.Status = StatusType.Successful;
      }
   }
}
