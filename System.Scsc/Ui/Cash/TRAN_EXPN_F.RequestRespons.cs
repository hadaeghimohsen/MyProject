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

namespace System.Scsc.Ui.Cash
{
   partial class TRAN_EXPN_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      int Sleeping = 1;
      int step = 15;
      private string CurrentUser;
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
         CurrentUser = iScsc.GET_CRNTUSER_U(new XElement("User", new XAttribute("actntype", "001")));

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
                  case "brthdate_lb":
                     BrthDate_Lb.Text = control.LABL_TEXT;
                     //BrthDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crnt_gb":
                     Crnt_Gb.Text = control.LABL_TEXT;
                     //Crnt_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Crnt_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "back_butn":
                     Back_Butn.Text = control.LABL_TEXT;
                     //Back_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Back_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "namednrm_lb":
                     NameDnrm_Lb.Text = control.LABL_TEXT;
                     //NameDnrm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NameDnrm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tellphon_lb":
                     TellPhon_Lb.Text = control.LABL_TEXT;
                     //TellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_lb":
                     FngrPrnt_Lb.Text = control.LABL_TEXT;
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntcochfileno_lb":
                     CrntCochFileNo_Lb.Text = control.LABL_TEXT;
                     //CrntCochFileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntCochFileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crnmtodcode_lb":
                     CrnMtodCode_Lb.Text = control.LABL_TEXT;
                     //CrnMtodCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrnMtodCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntctgycode_lb":
                     CrntCtgyCode_Lb.Text = control.LABL_TEXT;
                     //CrntCtgyCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntCtgyCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "time_lb":
                     Time_Lb.Text = control.LABL_TEXT;
                     //Time_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Time_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntexpncode_lb":
                     CrntExpnCode_Lb.Text = control.LABL_TEXT;
                     //CrntExpnCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntExpnCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tran_gb":
                     Tran_Gb.Text = control.LABL_TEXT;
                     //Tran_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Tran_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tranby_lb":
                     TranBy_Lb.Text = control.LABL_TEXT;
                     //TranBy_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TranBy_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "trancbmtcode_lb":
                     TranCbmtCode_Lb.Text = control.LABL_TEXT;
                     //TranCbmtCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TranCbmtCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tranctgycode_lb":
                     TranCtgyCode_Lb.Text = control.LABL_TEXT;
                     //TranCtgyCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TranCtgyCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "tranexpncode_lb":
                     TranExpnCode_Lb.Text = control.LABL_TEXT;
                     //TranExpnCode_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TranExpnCode_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "trandesc_lb":
                     TranDesc_Lb.Text = control.LABL_TEXT;
                     //TranDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TranDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqsttran_butn":
                     RqstTran_Butn.Text = control.LABL_TEXT;
                     //RqstTran_Butn.Text = control.LABL_TEXT; // ToolTip
                     //RqstTran_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqstsave_butn":
                     RqstSave_Butn.Text = control.LABL_TEXT;
                     //RqstSave_Butn.Text = control.LABL_TEXT; // ToolTip
                     //RqstSave_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqstcncl_butn":
                     RqstCncl_Butn.Text = control.LABL_TEXT;
                     //RqstCncl_Butn.Text = control.LABL_TEXT; // ToolTip
                     //RqstCncl_Butn.Text = control.LABL_TEXT; // Place Holder
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
            //RqstBnExit1_Click(null, null);
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
               new Job(SendType.SelfToUserInterface, "Wall", 0 /* Execute PastManualOnWall */) {  Input = new List<object> {this, "right:dock:default:center"} }               
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
         DdytpBs.DataSource = iScsc.D_DYTPs;
         DsxtpBs.DataSource = iScsc.D_SXTPs;
         CbmtBs.DataSource = 
            iScsc.Club_Methods
            .Where(cb =>
               Fga_Uclb_U.Contains(cb.CLUB_CODE) &&
               cb.MTOD_STAT == "002"
            );
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
            pydtcode = Convert.ToInt64(xinput.Attribute("pydtcode").Value);
            fileno = Convert.ToInt64(xinput.Attribute("fileno").Value);
            AutoChngPric_Cb.Checked = false;

            if(xinput.Attribute("formcaller") != null)
            {
               switch(xinput.Attribute("formcaller").Value)
               {
                  case "MBSP_CHNG_F":
                     AutoChngPric_Cb.Checked = true;
                     Cbmt_Lov.EditValue = Convert.ToInt64(xinput.Attribute("cbmtcode").Value);
                     Ctgy_Lov.EditValue = Convert.ToInt64(xinput.Attribute("ctgycode").Value);                     
                     break;
               }
            }
         }
         Execute_Query(true);
         job.Status = StatusType.Successful;
      }
   }
}
