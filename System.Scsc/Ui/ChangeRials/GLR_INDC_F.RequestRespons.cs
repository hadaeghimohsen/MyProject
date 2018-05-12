using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.ChangeRials
{
   partial class GLR_INDC_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string formCaller;
      private string CurrentUser;
      private XElement HostNameInfo;
      private string RegnLang = "054";

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
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstRqt1_Click(null, null);
         }
         else if (keyData == Keys.Enter)
         {
            //if (!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_Cbmt1.Focused || Btn_Dise.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            //Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstDelete1_Click(null, null);
         }
         else if (keyData == Keys.F5)
         {
            //if (tb_master.SelectedTab == tp_001)
               Btn_RqstRqt1_Click(null, null);
         }
         else if (keyData == Keys.F10)
         {
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
         //LL_MoreInfo2_LinkClicked(null, null);

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
                  case "mdfyby_lb":
                     MdfyBy_Lb.Text = control.LABL_TEXT;
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "info_gb":
                     Info_Gb.Text = control.LABL_TEXT;
                     //Info_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Info_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "filenos_lb":
                     FileNos_Lb.Text = control.LABL_TEXT;
                     //FileNos_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNos_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntdpstamnt_lb":
                     CrntDpstAmnt_Lb.Text = control.LABL_TEXT;
                     //CrntDpstAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntDpstAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnt_lb":
                     Amnt_Lb.Text = control.LABL_TEXT;
                     //Amnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Amnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntdebtamnt_lb":
                     CrntDebtAmnt_Lb.Text = control.LABL_TEXT;
                     //CrntDebtAmnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntDebtAmnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "paiddate_lb":
                     PaidDate_Lb.Text = control.LABL_TEXT;
                     //PaidDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PaidDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "glr_gb":
                     Glr_Gb.Text = control.LABL_TEXT;
                     //Glr_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Glr_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "incdpst_rb":
                     IncDpst_Rb.Text = control.LABL_TEXT;
                     //IncDpst_Rb.Text = control.LABL_TEXT; // ToolTip
                     //IncDpst_Rb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "decdspt_rb":
                     DecDspt_Rb.Text = control.LABL_TEXT;
                     //DecDspt_Rb.Text = control.LABL_TEXT; // ToolTip
                     //DecDspt_Rb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "paidtype_lb":
                     PaidType_Lb.Text = control.LABL_TEXT;
                     //PaidType_Lb.Text = control.LABL_TEXT; // ToolTip
                     //PaidType_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rwno_clm":
                     Rwno_Clm.Caption = control.LABL_TEXT;
                     //Rwno_Clm.Text = control.LABL_TEXT; // ToolTip
                     //Rwno_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnt_clm":
                     Amnt_Clm.Caption = control.LABL_TEXT;
                     //Amnt_Clm.Text = control.LABL_TEXT; // ToolTip
                     //Amnt_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rcptmtod_clm":
                     RcptMtod_Clm.Caption = control.LABL_TEXT;
                     //RcptMtod_Clm.Text = control.LABL_TEXT; // ToolTip
                     //RcptMtod_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqst_gb":
                     Rqst_Gb.Text = control.LABL_TEXT;
                     //Rqst_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Rqst_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqid_lb":
                     Rqid_Lb.Text = control.LABL_TEXT;
                     //Rqid_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rqid_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby_lb":
                     CretBy_Lb.Text = control.LABL_TEXT;
                     //CretBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CretBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretdate_lb":
                     CretDate_Lb.Text = control.LABL_TEXT;
                     //CretDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CretDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfydate_lb":
                     MdfyDate_Lb.Text = control.LABL_TEXT;
                     //MdfyDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //MdfyDate_Lb.Text = control.LABL_TEXT; // Place Holder
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
         DRcmtBs1.DataSource = iScsc.D_RCMTs;
         VPosBs1.DataSource = iScsc.V_Pos_Devices;
         if (VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value) != null)
            Pos_Lov.EditValue = VPosBs1.List.OfType<Data.V_Pos_Device>().FirstOrDefault(p => p.GTWY_MAC_ADRS == HostNameInfo.Attribute("cpu").Value).PSID;
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
         try
         {
            var xinput = job.Input as XElement;

            if (xinput.Attribute("formcaller") != null)
               formCaller = xinput.Attribute("formcaller").Value;
            else
               formCaller = "";
            if (xinput.Attribute("type").Value == "newrequest")
            {
               if (RqstBs1.Count > 0 && (RqstBs1.Current as Data.Request).RQID > 0)
                  RqstBs1.AddNew();

               FIGH_FILE_NOLookUpEdit.EditValue = Convert.ToInt64(xinput.Attribute("fileno").Value);
               Btn_RqstRqt1_Click(null, null);               
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

            var glrd = GlrdBs1.Current as Data.Gain_Loss_Rail_Detail;
            glrd.TERM_NO = termno;
            glrd.TRAN_NO = tranno;
            glrd.CARD_NO = cardno;
            glrd.FLOW_NO = flowno;
            glrd.REF_NO = refno;
            glrd.ACTN_DATE = Convert.ToDateTime(actndate);

            Glrd_gv.PostEditor();

            Btn_RqstRqt1_Click(null, null);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         job.Status = StatusType.Successful;
      }
   }
}
