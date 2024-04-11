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
   partial class ATTN_DAYN_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      int Sleeping = 1;
      int step = 15;
      bool isPainted = false;
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
            job.Next =
               new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
            if (!(Reload_Butn.Focused))
               SendKeys.Send("{TAB}");
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

         FromAttnDate_Date.Value = DateTime.Now;

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
                  case "cbmtcode_lb":
                     CbmtCode_Lb.Text = control.LABL_TEXT;
                     //CbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno_clm":
                     CochFileNo_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "atendesc_lb":
                     //AtenDesc_Lb.Text = control.LABL_TEXT;
                     //AtenDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //AtenDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "attnparm_gb":
                     //AttnParm_Gb.Text = control.LABL_TEXT;
                     //AttnParm_Gb.Text = control.LABL_TEXT; // ToolTip
                     //AttnParm_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fromdate_lb":
                     FromDate_Lb.Text = control.LABL_TEXT;
                     //FromDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FromDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "todate_lb":
                     ToDate_Lb.Text = control.LABL_TEXT;
                     //ToDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ToDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno_clm":
                     FileNo_Clm.Caption = control.LABL_TEXT;
                     //FileNo_Clm.Text = control.LABL_TEXT; // ToolTip
                     //FileNo_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "entertime_clm":
                     EnterTime_Clm.Caption = control.LABL_TEXT;
                     //EnterTime_Clm.Text = control.LABL_TEXT; // ToolTip
                     //EnterTime_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "exittime_clm":
                     ExitTime_Clm.Caption = control.LABL_TEXT;
                     //ExitTime_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ExitTime_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodcode_clm":
                     MtodCode_Clm.Caption = control.LABL_TEXT;
                     //MtodCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //MtodCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "ctgycode_clm":
                     CtgyCode_Clm.Caption = control.LABL_TEXT;
                     //CtgyCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CtgyCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dersnum_clm":
                     DersNum_Clm.Caption = control.LABL_TEXT;
                     //DersNum_Clm.Text = control.LABL_TEXT; // ToolTip
                     //DersNum_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
               }
            }
         }
         #endregion


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
         var attnNotfSetting = iScsc.Settings.Where(s => Fga_Uclb_U.Contains(s.CLUB_CODE)).FirstOrDefault();
         if(attnNotfSetting.ATTN_NOTF_STAT == "002" && attnNotfSetting.ATTN_NOTF_CLOS_TYPE == "002")
         {
            Thread.Sleep((int)attnNotfSetting.ATTN_NOTF_CLOS_INTR);
            if(isPainted)
               Back_Butn_Click(null, null);
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
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "right:in-screen:stretch:center"} }               
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
         //         new Job(SendType.SelfToUserInterface, "MAIN_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
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
         CbmtBs1.DataSource = iScsc.Club_Methods.Where(cbmt => cbmt.MTOD_STAT == "002" && Fga_Uclb_U.Contains(cbmt.CLUB_CODE) && Convert.ToInt32(cbmt.Fighter.ACTV_TAG_DNRM ?? "101") >= 101);
         CochBs.DataSource = iScsc.Fighters.Where(f => f.FGPB_TYPE_DNRM == "003" && Convert.ToInt32(f.ACTV_TAG_DNRM) >= 101);
         MtodBs.DataSource = iScsc.Methods;//.Where(m => m.MTOD_STAT == "002");
         CtgyBs.DataSource = iScsc.Category_Belts;//.Where(c => c.CTGY_STAT == "002");
         DActnBs.DataSource = iScsc.D_ACTNs;
         DSxtpBs1.DataSource = iScsc.D_SXTPs;
         DYsnoBs.DataSource = iScsc.D_YSNOs;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if(xinput != null)
         {
            ToAttnDate_Date.Value = FromAttnDate_Date.Value = Convert.ToDateTime(xinput.Attribute("attndate").Value);
         }
         if(isPainted)
            Execute_Query();
         job.Status = StatusType.Successful;
      }
   }
}
