﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.Reason
{
   partial class CMN_RESN_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
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
               Actn_CalF_F(job);
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

         }
         else if (keyData == Keys.Escape)
         {
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
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
                  case "rqid_lb":
                     Rqid_Lb.Text = control.LABL_TEXT;
                     //Rqid_Lb.Text = control.LABL_TEXT; // ToolTip
                     //Rqid_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqtpdesc_lb":
                     RqtpDesc_Lb.Text = control.LABL_TEXT;
                     //RqtpDesc_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqtpDesc_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rqrorwno_lb":
                     RqroRwno_Lb.Text = control.LABL_TEXT;
                     //RqroRwno_Lb.Text = control.LABL_TEXT; // ToolTip
                     //RqroRwno_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "fileno_lb":
                     FileNo_Lb.Text = control.LABL_TEXT;
                     //FileNo_Lb.Text = control.LABL_TEXT; // ToolTip
                     //FileNo_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "rwno_clm":
                     Rwno_Clm.Caption = control.LABL_TEXT;
                     //Rwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Rwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "resnrwno_clm":
                     ResnRwno_Clm.Caption = control.LABL_TEXT;
                     //ResnRwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ResnRwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "othrdesc_clm":
                     OthrDesc_Clm.Caption = control.LABL_TEXT;
                     //OthrDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //OthrDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
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
         RqroBs1.DataSource = job.Input as Data.Request_Row;
         RqstBs1.DataSource = (RqroBs1.Current as Data.Request_Row).Request;
         ResnBs1.DataSource = iScsc.Reason_Specs.Where(resn => resn.RQTP_CODE == (RqstBs1.Current as Data.Request).RQTP_CODE);
         var rqst = RqstBs1.Current as Data.Request;
         rqrorqstrqid = rqst.RQID;
         rqrorwno = (RqroBs1.Current as Data.Request_Row).RWNO;
         rqtpcode = rqst.RQTP_CODE;
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void Actn_CalF_F(Job job)
      {
         job.Status = StatusType.Successful;
      }
   }
}
