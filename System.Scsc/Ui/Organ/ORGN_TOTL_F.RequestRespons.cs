using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Organ
{
   partial class ORGN_TOTL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private string Fga_Uprv_U, Fga_Urgn_U;
      private List<long?> Fga_Uclb_U;
      //private long Rqid, FileNo;
      private bool isFirstLoaded = false;
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
            //if (!(Btn_Search.Focused))
            //   SendKeys.Send("{TAB}");
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
                  case "orgndesc_clm":
                     OrgnDesc_Clm.Caption = control.LABL_TEXT;
                     //OrgnDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OrgnDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "stat_clm":
                     Stat_Clm.Caption = control.LABL_TEXT;
                     //Stat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Stat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "todate_lb":
                     ToDate_Lb.Text = control.LABL_TEXT;
                     //ToDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ToDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "orgncode_gb":
                     OrgnCode_Gb.Text = control.LABL_TEXT;
                     //OrgnCode_Gb.Text = control.LABL_TEXT; // ToolTip
                     //OrgnCode_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "orgncode_clm":
                     OrgnCode_Clm.Caption = control.LABL_TEXT;
                     //OrgnCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OrgnCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "deptcode_gb":
                     DeptCode_Gb.Text = control.LABL_TEXT;
                     //DeptCode_Gb.Text = control.LABL_TEXT; // ToolTip
                     //DeptCode_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "deptcode_clm":
                     DeptCode_Clm.Caption = control.LABL_TEXT;
                     //DeptCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DeptCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "deptdesc_clm":
                     DeptDesc_Clm.Caption = control.LABL_TEXT;
                     //DeptDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DeptDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "buntcode_gb":
                     BuntCode_Gb.Text = control.LABL_TEXT;
                     //BuntCode_Gb.Text = control.LABL_TEXT; // ToolTip
                     //BuntCode_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "buntcode_clm":
                     BuntCode_Clm.Caption = control.LABL_TEXT;
                     //BuntCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //BuntCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "buntdesc_clm":
                     BuntDesc_Clm.Caption = control.LABL_TEXT;
                     //BuntDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //BuntDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode_gb":
                     SuntCode_Gb.Text = control.LABL_TEXT;
                     //SuntCode_Gb.Text = control.LABL_TEXT; // ToolTip
                     //SuntCode_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntcode_clm":
                     SuntCode_Clm.Caption = control.LABL_TEXT;
                     //SuntCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SuntCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "suntdesc_clm":
                     SuntDesc_Clm.Caption = control.LABL_TEXT;
                     //SuntDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SuntDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "discount_gb":
                     Discount_Gb.Text = control.LABL_TEXT;
                     //Discount_Gb.Text = control.LABL_TEXT; // ToolTip
                     //Discount_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rwno_clm":
                     Rwno_Clm.Caption = control.LABL_TEXT;
                     //Rwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Rwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "actntype_clm":
                     ActnType_Clm.Caption = control.LABL_TEXT;
                     //ActnType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ActnType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "reglyear_clm":
                     ReglYear_Clm.Caption = control.LABL_TEXT;
                     //ReglYear_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ReglYear_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "reglcode_clm":
                     ReglCode_Clm.Caption = control.LABL_TEXT;
                     //ReglCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ReglCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "epitcode_clm":
                     EpitCode_Clm.Caption = control.LABL_TEXT;
                     //EpitCode_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EpitCode_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "dscttype_clm":
                     DsctType_Clm.Caption = control.LABL_TEXT;
                     //DsctType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DsctType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amntdsct_clm":
                     AmntDsct_Clm.Caption = control.LABL_TEXT;
                     //AmntDsct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AmntDsct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "prctdsct_clm":
                     PrctDsct_Clm.Caption = control.LABL_TEXT;
                     //PrctDsct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //PrctDsct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "discountinfo_gb":
                     DiscountInfo_Gb.Text = control.LABL_TEXT;
                     //DiscountInfo_Gb.Text = control.LABL_TEXT; // ToolTip
                     //DiscountInfo_Gb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dsctdesc_lb":
                     DsctDesc_Lb.Text = control.LABL_TEXT;
                     //DsctDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DsctDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fromdate_lb":
                     FromDate_Lb.Text = control.LABL_TEXT;
                     //FromDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FromDate_Lb.Text = control.LABL_TEXT; // Place Holder
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
         if (!isFirstLoaded)
         {
            EpitBs1.DataSource = iScsc.Expense_Items.Where(ep => ep.TYPE == "001");
            DCetpBs1.DataSource = iScsc.D_CETPs;
            DActvBs1.DataSource = iScsc.D_ACTVs;
            DDsatBs1.DataSource = iScsc.D_DSATs;
            isFirstLoaded = true;
         }
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
         Execute_Query(true);

         job.Status = StatusType.Successful;
      }
   }
}
