using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission
{
   partial class ADM_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string formCaller = "";
      private bool isFirstLoaded = false;
      private string CurrentUser;
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
               CheckSecurity(job);
               break;
            case 06:
               break;
            case 07:
               LoadData(job);
               break;
            case 08:
               LoadDataSource(job);
               break;
            case 10:
               Actn_CalF_P(job);
               break;
            case 20:
               Pay_Oprt_F(job);
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
         else if (keyData == Keys.Escape)
         {
            switch (formCaller)
            {
               case "CFG_STNG_F":
               case "BAS_CPR_F":
               case "ALL_FLDF_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", formCaller, 08 /* Exec LoadDataSource */, SendType.SelfToUserInterface)
                  );
                  break;
               default:
                  break;
            }
            formCaller = "";
            job.Next =
               new Job(SendType.SelfToUserInterface, "ADM_TOTL_F", 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstRqt1_Click(null, null);
            if (tb_master.SelectedTab == tp_002)
               Btn_RqstSav1_Click(null, null);
            else if (tb_master.SelectedTab == tp_003) 
               Btn_RqstSav3_Click(null, null);
         }
         else if (keyData == Keys.Enter)
         {
            //if(!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_Cbmt1.Focused || Btn_Dise.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstDelete1_Click(null, null);
            if (tb_master.SelectedTab == tp_002)
               Btn_RqstDelete2_Click(null, null);
            else if (tb_master.SelectedTab == tp_003)
               Btn_RqstDelete3_Click(null, null);
         }
         else if (keyData == Keys.F5)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstRqt1_Click(null, null);
            if (tb_master.SelectedTab == tp_003)
               Btn_RqstRqt3_Click(null, null);
         }
         else if (keyData == Keys.F3)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   LL_MoreInfo_LinkClicked(null, null);
            if (tb_master.SelectedTab == tp_002)
               LL_MoreInfo2_LinkClicked(null, null);
         }
         else if (keyData == Keys.F10)
         {
            //if (tb_master.SelectedTab == tp_001)
            //   Btn_RqstSav1_Click(null, null);
            if (tb_master.SelectedTab == tp_002)
               Btn_RqstSav2_Click(null, null);
            else if (tb_master.SelectedTab == tp_003)
               Btn_RqstSav3_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.P))
         {
            RqstBnDefaultPrint_Click(null, null);
         }
         else if (keyData == (Keys.Control | Keys.Shift | Keys.P))
         {
            RqstBnPrint_Click(null, null);
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
         formCaller = job.Input != null ? job.Input.ToString() : "";

         var GetConnectionString =
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());
         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();
         CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         //LL_MoreInfo_LinkClicked(null, null);
         LL_MoreInfo2_LinkClicked(null, null);
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
               //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
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
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "Wall", 16 /* Execute Pop */),
                  new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
                  //new Job(SendType.SelfToUserInterface, "Wall", 20 /* Execute ResetUiWithoutEnabled */)
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
         UserProFile_Rb.ImageVisiable = true;
         //**FighBs3.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006" || f.FGPB_TYPE_DNRM == "009") && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101 );
         if (isFirstLoaded) goto finishcommand;
         #region Rqsw block
         try
         {
            RqttBs1.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "001" || rqtt.CODE == "004" || rqtt.CODE == "005" || rqtt.CODE == "006");
            RqttBs2.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "002" || rqtt.CODE == "003");
            
            DDegrBs2.DataSource = iScsc.D_DEGRs;
            
            DSxtpBs1.DataSource = iScsc.D_SXTPs;
            DCyclBs1.DataSource = iScsc.D_CYCLs;

            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DEducBs1.DataSource = iScsc.D_EDUCs;
            DRcmtBs1.DataSource = iScsc.D_RCMTs;
            DstpBs1.DataSource = iScsc.Diseases_Types;
            PrvnBs1.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));
            DCetpBs2.DataSource = iScsc.D_CETPs;
            DBlodBs1.DataSource = iScsc.D_BLODs;
            OrgnBs1.DataSource = iScsc.Organs;

            isFirstLoaded = true;
         }            
         catch { }
         //Execute_Query();
         #endregion

      finishcommand:
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;
         MtodBs2.DataSource = iScsc.Methods.Where(m=> m.MTOD_STAT == "002");
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && cbmt.MTOD_STAT == "002" && cbmt.Method.MTOD_STAT == "002" && Convert.ToInt32( cbmt.Fighter.ACTV_TAG_DNRM ?? "101" ) >= 101 )/*.OrderBy(cm => new { cm.CLUB_CODE, cm.COCH_FILE_NO, cm.DAY_TYPE, cm.STRT_TIME })*/;
            DstpBs1.DataSource = iScsc.Diseases_Types;
         }
         catch { }
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
            tb_master.TabPages.Clear();
            switch ((job.Input as XElement).Attribute("type").Value)
            {
               case "fighter":
                  //tb_master.TabPages.Add(tp_001);
                  //tb_master.SelectedTab = tp_001;
                  break;
               case "coach":
                  tb_master.TabPages.Add(tp_002);
                  tb_master.SelectedTab = tp_002;
                  break;
               case "refresh":
               case "renewcontract":
                  tb_master.TabPages.Add(tp_003);
                  tb_master.SelectedTab = tp_003;
                  break;
            }
            if ((job.Input as XElement).Attribute("enrollnumber") != null)
            {
               if ((job.Input as XElement).Attribute("type").Value == "fighter")
               {
                  //INSR_NUMB_TextEdit.Text = FNGR_PRNT_TextEdit.Text = (job.Input as XElement).Attribute("enrollnumber").Value;
                  //INSR_DATE_PersianDateEdit.Value = DateTime.Now;
               }
               if ((job.Input as XElement).Attribute("type").Value == "coach")
               {
                  Insr_Numb_TextEdit2.Text = Fngr_Prnt_TextEdit2.Text = (job.Input as XElement).Attribute("enrollnumber").Value;
                  Insr_Date_PersianDateEdit2.Value = DateTime.Now;
               }
               else if ((job.Input as XElement).Attribute("type").Value == "renewcontract")
               {
                  if (RqstBs3.Count > 0 && (RqstBs3.Current as Data.Request).RQID > 0)
                     RqstBs3.AddNew();

                  var figh = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM == (job.Input as XElement).Attribute("enrollnumber").Value).FirstOrDefault();
                  FIGH_FILE_NOLookUpEdit.EditValue = figh.FILE_NO;
                  RQTT_CODE_LookUpEdit3.EditValue = "001";//figh.FGPB_TYPE_DNRM;
                  Btn_RqstRqt3_Click(null, null);

                  // 1396/11/04
                  if ((job.Input as XElement).Attribute("formcaller") != null)
                     formCaller = (job.Input as XElement).Attribute("formcaller").Value;
                  else
                     formCaller = "";
               }

            }
            else if ((job.Input as XElement).Attribute("type") != null)
            {
               if ((job.Input as XElement).Attribute("type").Value == "refresh" || (job.Input as XElement).Attribute("type").Value == "renewcontract")
               {
                  Execute_Query();

                  ReloadSelectedData();
               }
            }
            else
               Execute_Query();
         }
         catch { }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void Pay_Oprt_F(Job job)
      {
         XElement RcevXData = job.Input as XElement;

         var rqst = RqstBs3.Current as Data.Request;
         if (rqst == null) return;

         var regl = iScsc.Regulations.FirstOrDefault(r => r.TYPE == "001" && r.REGL_STAT == "002");

         var rqtpcode = rqst.RQTP_CODE;//RcevXData.Element("PosRespons").Attribute("rqtpcode").Value;
         var rqid = rqst.RQID;//RcevXData.Element("PosRespons").Attribute("rqid").Value;
         var fileno = rqst.Request_Rows.FirstOrDefault().FIGH_FILE_NO;//RcevXData.Element("PosRespons").Attribute("fileno").Value;
         var cashcode = rqst.Payments.FirstOrDefault().CASH_CODE;//RcevXData.Element("PosRespons").Element("Payment").Attribute("cashcode").Value;
         var amnt = Convert.ToInt64(RcevXData.Attribute("amnt").Value);
         var termno = RcevXData.Attribute("termno").Value;
         var tranno = RcevXData.Attribute("tranno").Value;
         var cardno = RcevXData.Attribute("cardno").Value;
         var flowno = RcevXData.Attribute("flowno").Value;
         var refno = RcevXData.Attribute("refno").Value;
         var actndate = RcevXData.Attribute("actndate").Value;

         if (regl.AMNT_TYPE == "002")
            amnt /= 10;

         iScsc.PAY_MSAV_P(
            new XElement("Payment",
               new XAttribute("actntype", "CheckoutWithPOS"),
               new XElement("Insert",
                  new XElement("Payment_Method",
                     new XAttribute("cashcode", cashcode),
                     new XAttribute("rqstrqid", rqid),
                     new XAttribute("amnt", amnt),
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

         /* Loop For Print After Pay */
         RqstBnPrintAfterPay_Click(null, null);

         /* End Request */
         Btn_RqstSav3_Click(null, null);

         job.Status = StatusType.Successful;
      }
   }
}
