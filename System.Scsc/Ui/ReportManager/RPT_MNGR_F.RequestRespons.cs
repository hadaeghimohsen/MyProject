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

namespace System.Scsc.Ui.ReportManager
{
   partial class RPT_MNGR_F : ISendRequest
   {
      public IRouter _DefaultGateway { get; set; }
      private Data.iScscDataContext iScsc;
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
               case "OIC_TOTL_F":
               case "AOP_BUFE_F":
                  // Nothing
                  break;
               default:
                  job.Next =
                     new Job(SendType.SelfToUserInterface, GetType().Name, 04 /* Execute UnPaint */);
                  break;
            }
            ModualName = "";
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
               //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */),
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
                  //new Job(SendType.SelfToUserInterface, "Wall", 17 /* Execute ResetUi */)
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
         Job _CheckSecurity =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "Commons",
                     new List<Job>
                     {
                        #region Access Privilege
                        new Job(SendType.Self, 07 /* Execute DoWork4AccessPrivilege */)
                        {
                           Input = new List<string> 
                           {
                              "<Privilege>238</Privilege><Sub_Sys>5</Sub_Sys>", 
                              "DataGuard"
                           },
                           AfterChangedOutput = new Action<object>((output) => {
                              if ((bool)output)
                                 return;
                              MessageBox.Show("خطا - عدم دسترسی به ردیف 238 سطوح امینتی");
                           })
                        },
                        #endregion
                     }),
               });
         _DefaultGateway.Gateway(_CheckSecurity);
         job.Status = _CheckSecurity.Status;
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
               s.Dictionary.Databases.Add(new StiSqlDatabase("iScsc", ConnectionString));
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
               s.Dictionary.Databases.Add(new StiSqlDatabase("iScsc", ConnectionString));
               vc_reportviewer.Report = s;
               s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

               s.Compile();
               s.Render();
               s.Print(false);
            }
         }
         catch (Exception exc)
         {
            //MessageBox.Show(exc.Message); 
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 07 /* Execute Load_Data */, SendType.SelfToUserInterface) { Input = job.Input }
            );
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
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void Do_Print(Job job)
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

            if (PrintType == "Selection")
            {
               #region Selection
               if (iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002").Any())
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
               #endregion
            }
            else if (PrintType == "Default")
            {
               #region Default
               var DfltPrint = iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002" && mr.DFLT == "002").SingleOrDefault();
               if (DfltPrint != null)
               {
                  if (DfltPrint.SHOW_PRVW == "002") // Yes
                  {
                     Stimulsoft.Report.StiReport s = new Stimulsoft.Report.StiReport();
                     s.Load(DfltPrint.RPRT_PATH);
                     s.Dictionary.Databases.Clear();
                     s.Dictionary.Databases.Add(new StiSqlDatabase("iScsc", ConnectionString));
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
                     s.Load(DfltPrint.RPRT_PATH);
                     s.Dictionary.Databases.Clear();
                     s.Dictionary.Databases.Add(new StiSqlDatabase("iScsc", ConnectionString));
                     vc_reportviewer.Report = s;
                     s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

                     s.Compile();
                     s.Render();
                     s.Print(false);

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           { 
                              new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 03 /* Execute Paint */)                        
                           })
                     );

                     // 1397/01/08 * بازگشت سریع به فرم صدا کننده
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
                     );

                     // 1398/07/09 * بسته شدن فرم نمایش
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
                     );
                  }
               }
               else
               {
                  if (iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002").Any())
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
            else if (PrintType == "PrintAfterFinish")
            {
               #region Print After Finish
               var DfltPrint = iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002" /*&& mr.DFLT == "002"*/ && mr.PRNT_AFTR_PAY == "002").SingleOrDefault();
               if (DfltPrint != null)
               {
                  if (DfltPrint.SHOW_PRVW == "002") // Yes
                  {
                     Stimulsoft.Report.StiReport s = new Stimulsoft.Report.StiReport();
                     s.Load(DfltPrint.RPRT_PATH);
                     s.Dictionary.Databases.Clear();
                     s.Dictionary.Databases.Add(new StiSqlDatabase("iScsc", ConnectionString));
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
                     s.Load(DfltPrint.RPRT_PATH);
                     s.Dictionary.Databases.Clear();
                     s.Dictionary.Databases.Add(new StiSqlDatabase("iScsc", ConnectionString));
                     vc_reportviewer.Report = s;
                     s.Dictionary.Variables.Add(new StiVariable("WhereClause", WhereClause));

                     s.Compile();
                     s.Render();
                     s.Print(false);

                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "Localhost",
                           new List<Job>
                           {                        
                              new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 03 /* Execute Paint */)                        
                           })
                      );

                     // 1397/01/08 * بازگشت سریع به فرم صدا کننده
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
                     );

                     // 1398/07/09 * بسته شدن فرم نمایش
                     _DefaultGateway.Gateway(
                        new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
                     );
                  }
               }
               //else
               //{
               //   if (iScsc.Modual_Reports.Where(mr => mr.MDUL_NAME == ModualName && mr.SECT_NAME == SectionName && mr.STAT == "002").Any())
               //   {
               //      Job _InteractWithScsc = new Job(SendType.External, "Localhost",
               //         new List<Job>
               //         {
               //            new Job(SendType.Self, 85 /* Execute RPT_LRFM_F */){Input = job.Input}
               //         });
               //      _DefaultGateway.Gateway(_InteractWithScsc);
               //   }
               //   else
               //      MessageBox.Show(this, "برای فرم جاری هیچگونه چاپ گزارش مشخص نشده، لطفا از طریق تنظیمات چاپ همین فرم برای مشخص کردن چاپ گزارش اقدام فرمایید", "مشخص نبودن چاپ فرم", MessageBoxButtons.OK, MessageBoxIcon.Information);
               //}
               #endregion
            }

         }
         catch(Exception exc)
         {
            //MessageBox.Show(exc.InnerException.Message);
            _DefaultGateway.Gateway(
               new Job(SendType.External, "localhost", GetType().Name, 11 /* Execute Do_Print */, SendType.SelfToUserInterface) { Input = job.Input }
            );
         }
         job.Status = StatusType.Successful;
      }
   }
}
