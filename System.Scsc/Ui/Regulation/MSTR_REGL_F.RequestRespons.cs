using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Regulation
{
   partial class MSTR_REGL_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      private bool CanDirectExitForm = true;
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
            case 06:
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
            if (CanDirectExitForm)
               job.Next =
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 04 /* Execute UnPaint */);
            else
               Btn_CnclInsRegl_Click(null, null);
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
            new Job(SendType.External, "Localhost","Commons", 22 /* Execute GetConnectionString */,  SendType.Self){Input = "<Database>iScsc</Database><Dbms>SqlServer</Dbms>"};
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iScsc = new Data.iScscDataContext(GetConnectionString.Output.ToString());

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
                  case "year_lb":
                     Year_Lb.Text = control.LABL_TEXT;
                     //Year_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Year_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "taxprct_lb":
                     TaxPrct_Lb.Text = control.LABL_TEXT;
                     //TaxPrct_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TaxPrct_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "titlform_lb":
                     TitlForm_Lb.Text = control.LABL_TEXT;
                     //TitlForm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TitlForm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "year_clm":
                     Year_Clm.Caption = control.LABL_TEXT;
                     //Year_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Year_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "code_clm":
                     Code_Clm.Caption = control.LABL_TEXT;
                     //Code_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Code_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "type_clm":
                     Type_Clm.Caption = control.LABL_TEXT;
                     //Type_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Type_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "reglstat_clm":
                     ReglStat_Clm.Caption = control.LABL_TEXT;
                     //ReglStat_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ReglStat_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "lettno_clm":
                     LettNo_Clm.Caption = control.LABL_TEXT;
                     //LettNo_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //LettNo_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "lettdate_clm":
                     LettDate_Clm.Caption = control.LABL_TEXT;
                     //LettDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //LettDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "lettownr_clm":
                     LettOwnr_Clm.Caption = control.LABL_TEXT;
                     //LettOwnr_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //LettOwnr_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "strtdate_clm":
                     StrtDate_Clm.Caption = control.LABL_TEXT;
                     //StrtDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //StrtDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_clm":
                     EndDate_Clm.Caption = control.LABL_TEXT;
                     //EndDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //EndDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "taxprct_clm":
                     TaxPrct_Clm.Caption = control.LABL_TEXT;
                     //TaxPrct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //TaxPrct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "dutyprct_clm":
                     DutyPrct_Clm.Caption = control.LABL_TEXT;
                     //DutyPrct_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //DutyPrct_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretby_clm":
                     CretBy_Clm.Caption = control.LABL_TEXT;
                     //CretBy_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretBy_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "cretdate_clm":
                     CretDate_Clm.Caption = control.LABL_TEXT;
                     //CretDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //CretDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfyby_clm":
                     MdfyBy_Clm.Caption = control.LABL_TEXT;
                     //MdfyBy_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdfyBy_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdfydate_clm":
                     MdfyDate_Clm.Caption = control.LABL_TEXT;
                     //MdfyDate_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdfyDate_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "amnttype_clm":
                     AmntType_Clm.Caption = control.LABL_TEXT;
                     //AmntType_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //AmntType_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "lettno_lb":
                     LettNo_Lb.Text = control.LABL_TEXT;
                     //LettNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LettNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "lettdate_lb":
                     LettDate_Lb.Text = control.LABL_TEXT;
                     //LettDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LettDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "lettownr_lb":
                     LettOwnr_Lb.Text = control.LABL_TEXT;
                     //LettOwnr_Lb.Text = control.LABL_TEXT; // ToolTip
                     //LettOwnr_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "btn_submitinsregl":
                     Btn_SubmitInsRegl.Text = control.LABL_TEXT;
                     //Btn_SubmitInsRegl.Text = control.LABL_TEXT; // ToolTip
                     //Btn_SubmitInsRegl.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "type_lb":
                     Type_Lb.Text = control.LABL_TEXT;
                     //Type_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Type_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "strtdate_lb":
                     StrtDate_Lb.Text = control.LABL_TEXT;
                     //StrtDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //StrtDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "enddate_lb":
                     EndDate_Lb.Text = control.LABL_TEXT;
                     //EndDate_Lb.Text = control.LABL_TEXT; // ToolTip
                     //EndDate_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "dutyprct_lb":
                     DutyPrct_Lb.Text = control.LABL_TEXT;
                     //DutyPrct_Lb.Text = control.LABL_TEXT; // ToolTip
                     //DutyPrct_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "reglacnt_lb":
                     ReglAcnt_Lb.Text = control.LABL_TEXT;
                     //ReglAcnt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ReglAcnt_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "reglpymt_lb":
                     ReglPymt_Lb.Text = control.LABL_TEXT;
                     //ReglPymt_Lb.Text = control.LABL_TEXT; // ToolTip
                     //ReglPymt_Lb.Text = control.LABL_TEXT; // Place Holder
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
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void LoadData(Job job)
      {
         regulationBindingSource.DataSource = iScsc.Regulations;
         dRGTPBindingSource.DataSource = iScsc.D_RGTPs;
         dACTVBindingSource.DataSource = iScsc.D_ACTVs;
         DatypBs.DataSource = iScsc.D_ATYPs;

         Txt_LettNo.EditValue = DateTime.Now.Year;
         Dat_LettDate.EditValue = DateTime.Now;
         Txt_LettOwnr.EditValue = "";
         //Lov_ReglType.EditValue = "001";
         Spn_TaxPrct.EditValue = 0;
         Spn_DutyPrct.EditValue = 0;
         Dat_StrtDate.EditValue = DateTime.Now;
         Dat_EndDate.EditValue = DateTime.Now.AddMonths(12);

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_P(Job job)
      {
         Btn_InsRegl_Click(null, null);
         job.Status = StatusType.Successful;
      }
   }
}
