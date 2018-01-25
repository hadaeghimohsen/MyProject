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
   partial class ADM_CHNG_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      private bool isFirstLoaded = false;
      private string CurrentUser;
      private string formCaller;   

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
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
            if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnRqt_Click(null, null);
            
         }
         else if (keyData == Keys.Enter)
         {
            if(!(Btn_RqstRqt1.Focused || Btn_RqstSav1.Focused || Btn_RqstDelete1.Focused || Btn_Cbmt1.Focused || Btn_Dise.Focused || Btn_NewRecord.Focused))
               SendKeys.Send("{TAB}");
         }
         else if (keyData == Keys.F2)
         {
            Create_Record();
         }
         else if (keyData == Keys.F8)
         {
            if (tb_master.SelectedTab == tp_001)
               Btn_RqstDelete_Click(null, null);            
         }
         else if (keyData == Keys.F5)
         {
            if (tb_master.SelectedTab == tp_001)
               Btn_RqstBnRqt_Click(null, null);            
         }
         else if (keyData == Keys.F3)
         {
            if (tb_master.SelectedTab == tp_001)
               LL_MoreInfo_LinkClicked(null, null);            
         }
         else if (keyData == Keys.F10)
         {
            if (tb_master.SelectedTab == tp_001)
               Btn_RqstSav_Click(null, null);            
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
         CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

         //LL_MoreInfo_LinkClicked(null, null);
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
         #region Rqsw block
         try
         {
            if (isFirstLoaded) goto finishcommand;

            if (InvokeRequired)
               Invoke(new Action(() => Execute_Query()));
            else
               Execute_Query();

            DDegrBs2.DataSource = iScsc.D_DEGRs;
            DCyclBs1.DataSource = iScsc.D_CYCLs;
            //MtodBs2.DataSource = iScsc.Methods;
            //CtgyBs2.DataSource = iScsc.Category_Belts.Where(c => c.Method == MtodBs2.Current as Scsc.Data.Method).OrderBy(c => c.ORDR);
            DSxtpBs1.DataSource = iScsc.D_SXTPs;
            DDytpBs1.DataSource = iScsc.D_DYTPs;
            DEducBs1.DataSource = iScsc.D_EDUCs;
            DstpBs1.DataSource = iScsc.Diseases_Types;
            DFgtpBs1.DataSource = iScsc.D_FGTPs;
            DCetpBs1.DataSource = iScsc.D_CETPs;
            DBlodBs1.DataSource = iScsc.D_BLODs;
            OrgnBs1.DataSource = iScsc.Organs;

            isFirstLoaded = true;
            finishcommand:

            if (InvokeRequired)
               Invoke(new Action(() => CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101)));
            else
               CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);


            //if (InvokeRequired)
            //   Invoke(new Action(() => FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" && !f.NAME_DNRM.Contains("مشتری, جلسه ای") && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101).OrderBy(f => f.FGPB_TYPE_DNRM)));
            //else
            //   FighsBs1.DataSource = iScsc.Fighters.Where(f => f.CONF_STAT == "002" && f.FGPB_TYPE_DNRM != "007" && !f.NAME_DNRM.Contains("مشتری, جلسه ای") && (Fga_Uclb_U.Contains(f.CLUB_CODE_DNRM) || (f.CLUB_CODE_DNRM == null ? f.Club_Methods.Where(cb => Fga_Uclb_U.Contains(cb.CLUB_CODE)).Any() : false)) && Convert.ToInt32(f.ACTV_TAG_DNRM ?? "101") >= 101).OrderBy(f => f.FGPB_TYPE_DNRM);
         }
         catch { }
         #endregion

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      private void LoadDataSource(Job job)
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);
         DstpBs1.DataSource = iScsc.Diseases_Types;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         switch (xinput.Attribute("type").Value)
         {
            case "changeinfo":
               if (RqstBs1.Count > 0 && (RqstBs1.Current as Data.Request).RQID > 0)
                  RqstBs1.AddNew();
               FILE_NO_LookUpEdit.EditValue = Convert.ToInt64(xinput.Attribute("fileno").Value);
               if(xinput.Attribute("auto").Value == "true")
               {
                  Btn_RqstBnRqt_Click(null, null);

                  var figh = iScsc.Fighters.First(f => f.FILE_NO == Convert.ToInt64(xinput.Attribute("fileno").Value));

                  RqstBs1.Position = RqstBs1.List.OfType<Data.Request>().ToList().FindIndex(r => r.RQID == figh.RQST_RQID);

                  // 1396/11/04
                  if ((job.Input as XElement).Attribute("formcaller") != null)
                     formCaller = (job.Input as XElement).Attribute("formcaller").Value;
                  else
                     formCaller = "";
               }
               break;
            case "setcard":
               CardNumb_Text.Text = (job.Input as XElement).Attribute("value").Value;
               break;
            default:
               break;
         }
         job.Status = StatusType.Successful;
      }
   }
}
