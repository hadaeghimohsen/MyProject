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

namespace System.Scsc.Ui.OtherIncome
{
   partial class ADM_BRSR_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private string formCaller;
      private bool isFirstLoaded = false;


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
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "Localhost", formCaller, 08 /* Exec LoadDataSource */, SendType.SelfToUserInterface)
                  );
                  break;
               default:
                  break;
            }
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

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         //LL_MoreInfo_LinkClicked(null, null);
         //LL_MoreInfo2_LinkClicked(null, null);
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
         //FighBs3.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && (f.FGPB_TYPE_DNRM == "001" || f.FGPB_TYPE_DNRM == "005" || f.FGPB_TYPE_DNRM == "006") && Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101);
         //if (isFirstLoaded) goto finishcommand;
         #region Rqsw block
         try
         {
            DSxtpBs1.DataSource = iScsc.D_SXTPs;
            isFirstLoaded = true;
         }
         catch { }
         ClubBs1.DataSource = iScsc.Clubs.Where(c => Fga_Uclb_U.Contains(c.CODE));
         MtodBs1.DataSource = iScsc.Methods.Where(m => m.EPIT_TYPE == "001");
         vf_FighBs.DataSource = iScsc.VF_Last_Info_Fighter(null, null, null, null, null, null, null, null, null, null).OrderBy(f => f.REGN_PRVN_CODE + f.REGN_CODE);//.Where(f => Fga_Urgn_U.Split(',').Contains(f.REGN_PRVN_CODE + f.REGN_CODE) && Fga_Uclb_U.Contains(f.CLUB_CODE));
      //Execute_Query();
         #endregion

      finishcommand:
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
            //tb_master.TabPages.Clear();
            switch ((job.Input as XElement).Attribute("type").Value)
            {
               case "fighter":
                  //tb_master.TabPages.Add(tp_001);
                  //tb_master.SelectedTab = tp_001;
                  break;
               case "coach":
                  //tb_master.TabPages.Add(tp_002);
                  //tb_master.SelectedTab = tp_002;
                  break;
               case "renewcontract":
                  //tb_master.TabPages.Add(tp_003);
                  //tb_master.SelectedTab = tp_003;
                  break;
            }
            if ((job.Input as XElement).Attribute("enrollnumber") != null)
            {
               if ((job.Input as XElement).Attribute("type").Value == "fighter")
               {
                  
               }
               if ((job.Input as XElement).Attribute("type").Value == "coach")
               {
                  //Insr_Numb_TextEdit2.Text = Fngr_Prnt_TextEdit2.Text = (job.Input as XElement).Attribute("enrollnumber").Value;
                  //Insr_Date_PersianDateEdit2.Value = DateTime.Now;
               }
               else if ((job.Input as XElement).Attribute("type").Value == "renewcontract")
               {
                  var figh = iScsc.Fighters.Where(f => f.FNGR_PRNT_DNRM == (job.Input as XElement).Attribute("enrollnumber").Value).FirstOrDefault();
                  //FIGH_FILE_NOLookUpEdit.EditValue = figh.FILE_NO;
                  //RQTT_CODE_LookUpEdit3.EditValue = figh.FGPB_TYPE_DNRM;
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

         var rqtpcode = RcevXData.Element("Request").Attribute("rqtpcode").Value;
         var rqid = RcevXData.Element("Request").Attribute("rqid").Value;
         var fileno = RcevXData.Element("Request").Attribute("fileno").Value;
         var cashcode = RcevXData.Element("Request").Element("Payment").Attribute("cashcode").Value;
         var amnt = RcevXData.Element("Request").Element("Payment").Attribute("amnt").Value;
         var termno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("termno").Value;
         var cardno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("cardno").Value;
         var flowno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("flowno").Value;
         var refno = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("refno").Value;
         var actndate = RcevXData.Element("Request").Element("Payment").Element("Payment_Method").Attribute("actndate").Value;

         // ثبت نام
         if (rqtpcode == "001")
         {
            iScsc.PAY_MSAV_P(
                  new XElement("Payment",
                     new XAttribute("actntype", "CheckoutWithPOS"),
                     new XElement("Insert",
                        new XElement("Payment_Method",
                           new XAttribute("cashcode", cashcode),
                           new XAttribute("rqstrqid", rqid),
                           new XAttribute("amnt", amnt),
                           new XAttribute("termno", termno),
                           new XAttribute("cardno", cardno),
                           new XAttribute("flowno", flowno),
                           new XAttribute("refno", refno),
                           new XAttribute("actndate", actndate)
                        )
                     )
                  )
               );

            /* Loop For Print After Pay */
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", rqid))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);

            /* End Request */
            try
            {
               iScsc.ADM_TSAV_F(
                  new XElement("Process",
                     new XElement("Request",
                        new XAttribute("rqid", rqid),
                        new XElement("Fighter",
                           new XAttribute("fileno", fileno)
                        ),
                        new XElement("Payment",
                           iScsc.Payment_Details.Where(pd => pd.PYMT_CASH_CODE == Convert.ToInt64(cashcode) && pd.PYMT_RQST_RQID == Convert.ToInt64(rqid)).ToList()
                           .Select(pd =>
                              new XElement("Payment_Detail",
                                 new XAttribute("code", pd.CODE),
                                 new XAttribute("rcptmtod", "003")
                              )
                           )
                        )
                     )
                  )
               );
               requery = true;
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            finally
            {
               if (requery)
               {
                  //Get_Current_Record();
                  Execute_Query();
                  //Set_Current_Record();
                  //Create_Record();
                  requery = false;
               }
            }
         }
         job.Status = StatusType.Successful;
      }
   }
}
