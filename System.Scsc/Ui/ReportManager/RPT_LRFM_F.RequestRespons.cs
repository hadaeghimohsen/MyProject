﻿using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.ReportManager
{
   partial class RPT_LRFM_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
      private string ConnectionString;
      string PrintType;
      string ModualName, SectionName;
      string WhereClause;
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

         if (keyData == Keys.F1)
         {

         }
         else if (keyData == Keys.Escape)         
         {
            switch (ModualName)
            {
               case "WHO_ARYU_F":
                  _DefaultGateway.Gateway(
                     new Job(SendType.External, "localhost",
                        new List<Job>
                        {                           
                           new Job(SendType.Self, 110 /* Execute WHO_ARYU_F */),                           
                        })
                  );
                  break;
               default:
                  break;
            }
            ModualName = "";
            job.Next =
               new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
         }
         else if (keyData == (Keys.Control | Keys.S))
         {
         }
         else if (keyData == Keys.Enter)
         {
         }
         else if (keyData == Keys.F2)
         {
         }
         else if (keyData == Keys.F8)
         {
         }
         else if (keyData == Keys.F5)
         {
         }
         else if (keyData == Keys.F3)
         {
         }
         else if (keyData == Keys.F10)
         {
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
                  case "rwno_clm":
                     Rwno_Clm.Caption = control.LABL_TEXT;
                     //Rwno_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //Rwno_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "titlform_lb":
                     TitlForm_Lb.Text = control.LABL_TEXT;
                     //TitlForm_Lb.Text = control.LABL_TEXT; // ToolTip
                     //TitlForm_Lb.Text = control.LABL_TEXT; // Place Holder
                     break;
                  case "mudldesc_clm":
                     MudlDesc_Clm.Caption = control.LABL_TEXT;
                     //MudlDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MudlDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "sectdesc_clm":
                     SectDesc_Clm.Caption = control.LABL_TEXT;
                     //SectDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //SectDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "showprvw_clm":
                     ShowPrvw_Clm.Caption = control.LABL_TEXT;
                     //ShowPrvw_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //ShowPrvw_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "rprtdesc_clm":
                     RprtDesc_Clm.Caption = control.LABL_TEXT;
                     //RprtDesc_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //RprtDesc_Clm.Caption = control.LABL_TEXT; // Place Holder
                     break;
                  case "mdulname_clm":
                     MdulName_Clm.Caption = control.LABL_TEXT;
                     //MdulName_Clm.Caption = control.LABL_TEXT; // ToolTip
                     //MdulName_Clm.Caption = control.LABL_TEXT; // Place Holder
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
               new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("Scsc:{0}", this.GetType().Name), this }  },
               //new Job(SendType.SelfToUserInterface, "Wall", 00 /* Execute PastManualOnWall */) {Input = new List<object> { this, "cntrhrz:default" }}
               new Job(SendType.SelfToUserInterface, "Wall", 01 /* Execute PastManualOnWall */) { Input = this }               
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
                  new Job(SendType.SelfToUserInterface, "Wall", 02 /* Execute RemoveFromWall */){Input = this},
                  new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         try
         {
            /*
             * PrintType = {ShowDefault, ShowSelection}
             * ModualName
             * SectionName
             * Stimulsoft Parameter(s) Report "@ParamName"
             */
            PrintType = (job.Input as XElement).Attribute("type").Value;
            ModualName = (job.Input as XElement).Attribute("modual").Value;
            SectionName = (job.Input as XElement).Attribute("section").Value;
            WhereClause = (job.Input as XElement).Value;
            DYsnoBs1.DataSource = iScsc.D_YSNOs;
            //MdrpBs1.DataSource = iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002");
            Execute_Query();
         }
         catch { }
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
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void Do_Print(Job job)
      {
         job.Status = StatusType.Successful;
      }
   }
}
