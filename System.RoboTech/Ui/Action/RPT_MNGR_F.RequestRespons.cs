using Stimulsoft.Report.Dictionary;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.RoboTech.Ui.Action
{
   partial class RPT_MNGR_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iRoboTechDataContext iRoboTech;
      private string ConnectionString;
      string PrintType;
      string ModualName, SectionName;
      string WhereClause;
      
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
            case 11:
               Do_Print(job);
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
            new Job(SendType.External, "Localhost", "Commons", 22 /* Execute GetConnectionString */, SendType.Self) { Input = "<Database>iRoboTech</Database><Dbms>SqlServer</Dbms>" };
         _DefaultGateway.Gateway(
            GetConnectionString
         );
         ConnectionString = GetConnectionString.Output.ToString();
         iRoboTech = new Data.iRoboTechDataContext(GetConnectionString.Output.ToString());

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 08 /* Execute LangChangToFarsi */, SendType.Self)
         );

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
               new Job(SendType.SelfToUserInterface, "Wall", 15 /* Execute Push */) {  Input = new List<object> { string.Format("RoboTech:{0}", this.GetType().Name), this }  },
               new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 08 /* Execute PostOnWall */) {  Input = this }               
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
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 09 /* Execute TakeOnWall */){Input = this},
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
         try
         {
            var crnt = (job.Input as List<object>)[0] as Data.Modual_Report;
            WhereClause = (job.Input as List<object>)[1] as string;
            if (crnt.SHOW_PRVW == "002") // Yes
            {
               Stimulsoft.Report.StiReport s = new Stimulsoft.Report.StiReport();
               s.Load(crnt.RPRT_PATH);
               s.Dictionary.Databases.Clear();
               s.Dictionary.Databases.Add(new StiSqlDatabase("iRoboTech", ConnectionString));
               vc_reportviewer.Report = s;
               s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

               s.Compile();
               s.Render();

               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                        {                        
                           new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 03 /* Execute Paint */)                        
                        })
                );
            }
            else // No
            {
               Stimulsoft.Report.StiReport s = new Stimulsoft.Report.StiReport();
               s.Load(crnt.RPRT_PATH);
               s.Dictionary.Databases.Clear();
               s.Dictionary.Databases.Add(new StiSqlDatabase("iRoboTech", ConnectionString));
               vc_reportviewer.Report = s;
               s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

               s.Compile();
               s.Render();
               s.Print(false);
            }
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

         if (PrintType == "Selection")
         {
            #region Selection
            if (iRoboTech.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002").Any())
            {
               Job _InteractWithScsc = new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 16 /* Execute RPT_LRFM_F */){Input = job.Input}
                  });
               _DefaultGateway.Gateway(_InteractWithScsc);
            }
            else
               MessageBox.Show(this,"برای فرم جاری هیچگونه چاپ گزارش مشخص نشده، لطفا از طریق تنظیمات چاپ همین فرم برای مشخص کردن چاپ گزارش اقدام فرمایید", "مشخص نبودن چاپ فرم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion
         }
         else if (PrintType == "Default")
         {
            #region Default
            var DfltPrint = iRoboTech.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002" && mr.DFLT == "002").OrderBy(r => r.RWNO);
            if(DfltPrint != null)
            {
               foreach (var rpt in DfltPrint)
               {
                  if (rpt.SHOW_PRVW == "002") // Yes
                  {
                     Stimulsoft.Report.StiReport s = new Stimulsoft.Report.StiReport();
                     s.Load(rpt.RPRT_PATH);
                     s.Dictionary.Databases.Clear();
                     s.Dictionary.Databases.Add(new StiSqlDatabase("iRoboTech", ConnectionString));
                     vc_reportviewer.Report = s;
                     s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

                     s.Compile();
                     s.Render();

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {                        
                              new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 03 /* Execute Paint */)                        
                           })
                      );
                  }
                  else // No
                  {
                     Stimulsoft.Report.StiReport s = new Stimulsoft.Report.StiReport();
                     s.Load(rpt.RPRT_PATH);
                     s.Dictionary.Databases.Clear();
                     s.Dictionary.Databases.Add(new StiSqlDatabase("iRoboTech", ConnectionString));
                     vc_reportviewer.Report = s;
                     s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

                     s.Compile();
                     s.Render();
                     s.Print(false);
                  }
               }               
            }
            else
            {
               if (iRoboTech.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002").Any())
               {
                  Job _InteractWithScsc = new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 85 /* Execute RPT_LRFM_F */){Input = job.Input}
                     });
                  _DefaultGateway.Gateway(_InteractWithScsc);
               }
               else
                  MessageBox.Show(this, "برای فرم جاری هیچگونه چاپ گزارش مشخص نشده، لطفا از طریق تنظیمات چاپ همین فرم برای مشخص کردن چاپ گزارش اقدام فرمایید", "مشخص نبودن چاپ فرم", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            #endregion
         }

         job.Status = StatusType.Successful;
      }
   }
}
