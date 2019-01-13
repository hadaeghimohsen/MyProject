using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.OtherIncome
{
   partial class KSK_INCM_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private XElement HostNameInfo;
      private string RegnLang = "054";
      private string followups = "";
      private string formCaller = "";
      private long rqstRqid = 0;
      string fileno = "";

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
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

            switch (formCaller)
            {
               case "ADM_CHNG_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost", "", 46, SendType.Self) { Input = new XElement("Fighter", new XAttribute("fileno", fileno)) }
                  );
                  break;
               default:
                  break;
            }
            formCaller = "";

            //job.Next =
            //   new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnARqt1_Click(null, null);
            /*else if (tabControl1.SelectedTab == tabPage2)
               RqstBnASav1_Click(null, null);
            else if (tabControl1.SelectedTab == tabPage3)
               RqstBnASav3_Click(null, null);
             */
         }
         else if (keyData == Keys.Enter)
         {
            //if (!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
         }
         else if (keyData == Keys.F8)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnDelete1_Click(null, null);
            /*else if (tabControl1.SelectedTab == tabPage2)
               RqstBnDelete2_Click(null, null);
            else if (tabControl1.SelectedTab == tabPage3)
               RqstBnDelete3_Click(null, null);
             */
         }
         else if (keyData == Keys.F5)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnARqt1_Click(null, null);
            /*else if (tabControl1.SelectedTab == tabPage3)
               RqstBnARqt3_Click(null, null);
             */
         }
         else if (keyData == Keys.F3)
         {
            /*if (tabControl1.SelectedTab == tabPage1)
               LL_MoreInfo_LinkClicked(null, null);
            else if (tabControl1.SelectedTab == tabPage2)
               LL_MoreInfo2_LinkClicked(null, null);
             */
         }
         else if (keyData == Keys.F10)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnASav1_Click(null, null);
            /*else if (tabControl1.SelectedTab == tabPage2)
               RqstBnASav2_Click(null, null);
            else if (tabControl1.SelectedTab == tabPage3)
               RqstBnASav3_Click(null, null);
             */
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

         Fga_Uprv_U = iScsc.FGA_UPRV_U() ?? "";
         Fga_Urgn_U = iScsc.FGA_URGN_U() ?? "";
         Fga_Uclb_U = (iScsc.FGA_UCLB_U() ?? "").Split(',').Select(c => (long?)Int64.Parse(c)).ToList();

         var GetHostInfo = new Job(SendType.External, "Localhost", "Commons", 24 /* Execute DoWork4GetHosInfo */, SendType.Self);
         _DefaultGateway.Gateway(GetHostInfo);
         HostNameInfo = (XElement)GetHostInfo.Output;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         #region Set Localization
         var regnlang = iScsc.V_User_Localization_Forms.Where(rl => rl.FORM_NAME == GetType().Name);
         if (regnlang.Count() > 0 && regnlang.First().REGN_LANG != RegnLang)
         {
            RegnLang = regnlang.First().REGN_LANG;
            // Ready To Change Text Title
            foreach (var control in regnlang)
            {
               switch (control.CNTL_NAME.ToLower())
               {                  
                  case "gb_expenseitem":
                     Gb_ExpenseItem.Text = control.LABL_TEXT;
                     //Gb_ExpenseItem.Text = control.LABL_TEXT; // ToolTip
                     //Gb_ExpenseItem.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "selectexpense_clm":
                     SelectExpense_Clm.Caption = control.LABL_TEXT;
                     //SelectExpense_Clm.Text = control.LABL_TEXT; // ToolTip
                     //SelectExpense_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "pric_clm":
                     Pric_Clm.Caption = control.LABL_TEXT;
                     //Pric_Clm.Text = control.LABL_TEXT; // ToolTip
                     //Pric_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;                  
               }
            }
         }
         #endregion


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
         #region Rqsw block
         //FighsBs1.DataSource = iScsc.VF_Fighters(new XElement("Fighter")).Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006"));
         //FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" /*&& (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "004" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006")*/ && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101).OrderBy(f => f.FGPB_TYPE_DNRM);
         RqttBs1.DataSource = iScsc.Requester_Types.Where(rqtt => rqtt.CODE == "001" || rqtt.CODE == "004");
         DSxtpBs1.DataSource = iScsc.D_SXTPs;
         DCyclBs1.DataSource = iScsc.D_CYCLs;
         DAtypBs1.DataSource = iScsc.D_ATYPs;
         DRcmtBs1.DataSource = iScsc.D_RCMTs;
         //RQTT_CODE_LookUpEdit.EditValue = "001";
         DYsnoBs1.DataSource = iScsc.D_YSNOs;
         DDytpBs.DataSource = iScsc.D_DYTPs;
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(c => c.MTOD_STAT == "002");
         SuntBs1.DataSource = iScsc.Sub_Units;
         DPydsBs1.DataSource = iScsc.D_PYDS;
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;

         Execute_Query();         
         #endregion

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {  
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput.Element("Request_Row") != null)
            fileno = xinput.Element("Request_Row").Attribute("fileno").Value;
         else
            fileno = "";

         switch (xinput.Attribute("type").Value)
         {
            case "01":
               var figh = iScsc.Fighters.Where(f => f.FILE_NO == Convert.ToInt64(fileno)).First();
               if(figh.FIGH_STAT == "002")
               {
                  if(RqstBs1.Count > 0)
                     RqstBs1.AddNew();
                  

                  // 1397/05/26 * rqstrqid
                  if (xinput.Attribute("rqstrqid") != null)
                     rqstRqid = Convert.ToInt64(xinput.Attribute("rqstrqid").Value);
                  else
                     rqstRqid = 0;

                  Btn_RqstBnARqt1_Click(null, null);

                  // 1397/05/16
                  if (xinput.Attribute("formcaller") != null)
                     formCaller = xinput.Attribute("formcaller").Value;
                  else
                     formCaller = "";

                  // 1397/05/26 * followups
                  if (xinput.Attribute("followups") != null)
                     followups = xinput.Attribute("followups").Value;
                  else
                     followups = "";
               }
               break; 
            case "rqidfocus":
               RqstBs1.Position = RqstBs1.IndexOf(RqstBs1.List.OfType<Data.Request>().FirstOrDefault(r => r.RQID == Convert.ToInt64(xinput.Attribute("rqid").Value)));
               break;
            case "refresh":
               Execute_Query();
               break;
         }
            
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

            var rqst = RqstBs1.Current as Data.Request;
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
            Btn_RqstBnASav1_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
