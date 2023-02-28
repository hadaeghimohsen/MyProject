using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission.ShowChanges
{
   partial class SHOW_MBSC_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      int Sleeping = 1;
      int step = 15;
      private string CurrentUser;
      private string formCaller = "";
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
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost",
                  new List<Job>
                  {
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */)
                  })
            );

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
            //job.Next =
            //   new Job(SendType.SelfToUserInterface, this.GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == Keys.Enter)
         {
            //if (!(Btn_Search.Focused))
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
                  case "crntconsattnmot_lb":
                     CrntConsAttnMot_Lb.Text = control.LABL_TEXT;
                     //CrntConsAttnMot_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntConsAttnMot_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "endtime_clm":
                     //EndTime_Clm.Caption = control.LABL_TEXT;
                     //EndTime_Clm.Text = control.LABL_TEXT; // ToolTip
                     //EndTime_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "namednrm_lb":
                     //NameDnrm_Lb.Text = control.LABL_TEXT;
                     //NameDnrm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NameDnrm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "brthdate_lb":
                     //BrthDate_Lb.Text = control.LABL_TEXT;
                     //BrthDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //BrthDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cellphon_lb":
                     //CellPhon_Lb.Text = control.LABL_TEXT;
                     //CellPhon_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CellPhon_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fngrprnt_lb":
                     //FngrPrnt_Lb.Text = control.LABL_TEXT;
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FngrPrnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntmbsp_gb":
                     //CrntMbsp_Gb.Text = control.LABL_TEXT;
                     //CrntMbsp_Gb.Text = control.LABL_TEXT; // ToolTip
                     //CrntMbsp_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntmbsprwno_lb":
                     CrntMbspRwno_Lb.Text = control.LABL_TEXT;
                     //CrntMbspRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntMbspRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntfgpbrwno_lb":
                     CrntFgpbRwno_Lb.Text = control.LABL_TEXT;
                     //CrntFgpbRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntFgpbRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntstrtdate_lb":
                     CrntStrtDate_Lb.Text = control.LABL_TEXT;
                     //CrntStrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntStrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntenddate_lb":
                     CrntEndDate_Lb.Text = control.LABL_TEXT;
                     //CrntEndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntEndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntdays_lb":
                     CrntDays_Lb.Text = control.LABL_TEXT;
                     //CrntDays_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntDays_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntmont_lb":
                     CrntMont_Lb.Text = control.LABL_TEXT;
                     //CrntMont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntMont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntnumbattnmont_lb":
                     CrntNumbAttnMont_Lb.Text = control.LABL_TEXT;
                     //CrntNumbAttnMont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //CrntNumbAttnMont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "crntcbmtcode_gb":
                     //CrntCbmtCode_Gb.Text = control.LABL_TEXT;
                     //CrntCbmtCode_Gb.Text = control.LABL_TEXT; // ToolTip
                     //CrntCbmtCode_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newmbsp_gb":
                     //NewMbsp_Gb.Text = control.LABL_TEXT;
                     //NewMbsp_Gb.Text = control.LABL_TEXT; // ToolTip
                     //NewMbsp_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newrwno_lb":
                     NewRwno_Lb.Text = control.LABL_TEXT;
                     //NewRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newfgpbrwno_lb":
                     NewFgpbRwno_Lb.Text = control.LABL_TEXT;
                     //NewFgpbRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewFgpbRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newstrtdate_lb":
                     NewStrtDate_Lb.Text = control.LABL_TEXT;
                     //NewStrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewStrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newenddate_lb":
                     NewEndDate_Lb.Text = control.LABL_TEXT;
                     //NewEndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewEndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newdays_lb":
                     NewDays_Lb.Text = control.LABL_TEXT;
                     //NewDays_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewDays_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newmonts_lb":
                     NewMonts_Lb.Text = control.LABL_TEXT;
                     //NewMonts_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewMonts_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newnumbattnmont_lb":
                     NewNumbAttnMont_Lb.Text = control.LABL_TEXT;
                     //NewNumbAttnMont_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewNumbAttnMont_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newconsattnmot_lb":
                     NewConsAttnMot_Lb.Text = control.LABL_TEXT;
                     //NewConsAttnMot_Lb.Text = control.LABL_TEXT; // ToolTip
                     //NewConsAttnMot_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "newcbmtcode_gb":
                     //NewCbmtCode_Gb.Text = control.LABL_TEXT;
                     //NewCbmtCode_Gb.Text = control.LABL_TEXT; // ToolTip
                     //NewCbmtCode_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqstmbsp_butn":
                     //RqstMbsp_Butn.Text = control.LABL_TEXT;
                     //RqstMbsp_Butn.Text = control.LABL_TEXT; // ToolTip
                     //RqstMbsp_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "savembsp_butn":
                     //SaveMbsp_Butn.Text = control.LABL_TEXT;
                     //SaveMbsp_Butn.Text = control.LABL_TEXT; // ToolTip
                     //SaveMbsp_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "back_butn":
                     //Back_Butn.Text = control.LABL_TEXT;
                     //Back_Butn.Text = control.LABL_TEXT; // ToolTip
                     //Back_Butn.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "clubcode_clm":
                     //ClubCode_Clm.Caption = control.LABL_TEXT;
                     //ClubCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //ClubCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mtodcode_clm":
                     //MtodCode_Clm.Caption = control.LABL_TEXT;
                     //MtodCode_Clm.Text = control.LABL_TEXT; // ToolTip
                     //MtodCode_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "cochfileno_clm":
                     //CochFileNo_Clm.Caption = control.LABL_TEXT;
                     //CochFileNo_Clm.Text = control.LABL_TEXT; // ToolTip
                     //CochFileNo_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "daytype_clm":
                     //DayType_Clm.Caption = control.LABL_TEXT;
                     //DayType_Clm.Text = control.LABL_TEXT; // ToolTip
                     //DayType_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strttime_clm":
                     //StrtTime_Clm.Caption = control.LABL_TEXT;
                     //StrtTime_Clm.Text = control.LABL_TEXT; // ToolTip
                     //StrtTime_Clm.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "timedesc_clm":
                     //TimeDesc_Clm.Caption = control.LABL_TEXT;
                     //TimeDesc_Clm.Text = control.LABL_TEXT; // ToolTip
                     //TimeDesc_Clm.Text = control.LABL_TEXT; // Place Holder
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
         UserProFile_Rb.ImageVisiable = true;
         DDytpBs1.DataSource = iScsc.D_DYTPs;
         ApbsBs.DataSource = iScsc.App_Base_Defines.Where(a => a.ENTY_NAME == "Member_Ship_Reason");
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         var xinput = job.Input as XElement;
         if (xinput != null)
         {
            rqid = Convert.ToInt64(xinput.Attribute("rqid").Value);
            fileno = Convert.ToInt64(xinput.Element("Request_Row").Attribute("fighfileno").Value);
            Execute_Query();

            try
            {
               UserProFile_Rb.ImageProfile = null;
               MemoryStream mStream = new MemoryStream();
               byte[] pData = iScsc.GET_PIMG_U(new XElement("Fighter", new XAttribute("fileno", fileno))).ToArray();
               mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
               Bitmap bm = new Bitmap(mStream, false);
               mStream.Dispose();

               //Pb_FighImg.Visible = true;

               if (InvokeRequired)
                  Invoke(new Action(() => UserProFile_Rb.ImageProfile = bm));
               else
                  UserProFile_Rb.ImageProfile = bm;
            }
            catch
            { //Pb_FighImg.Visible = false;
               UserProFile_Rb.ImageProfile = global::System.Scsc.Properties.Resources.IMAGE_1482;
            }
         }
         

         

         // 1396/11/04
         if ((job.Input as XElement).Attribute("formcaller") != null)
            formCaller = (job.Input as XElement).Attribute("formcaller").Value;
         else
            formCaller = "";
         job.Status = StatusType.Successful;
      }
   }
}
