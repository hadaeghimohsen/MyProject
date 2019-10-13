using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.Globalization;
using System.Windows.Forms;
using System.Xml.Linq;
using Itenso.TimePeriod;
using System.Net.NetworkInformation;

namespace MyProject.Commons.Code
{
   partial class Commons
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LifeTimeObject(Job job)
      {
         if(job.Status == StatusType.Running)
         {
            job.OwnerDefineWorkWith.AddRange(new List<Job>
               {
                  new Job(SendType.External, "LifeTime",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute DoWork4ToolOperation */){WhereIsInputData = WhereIsInputDataType.StepBack},                        
                     }){WhereIsInputData = WhereIsInputDataType.StepBack}
               });
            job.Status = StatusType.WaitForPreconditions;
         }
         else if(job.Status == StatusType.SignalForPreconditions)
            job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ErrorHandling(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.OwnerDefineWorkWith.AddRange(new List<Job>
               {
                  new Job(SendType.External, "ErrorHandle",
                     new List<Job>
                     {
                        new Job(SendType.Self, 02 /* Execute Dowork4ErrorHandling */){WhereIsInputData = WhereIsInputDataType.StepBack}
                     }){WhereIsInputData = WhereIsInputDataType.StepBack}
               });
            job.Status = StatusType.WaitForPreconditions;
         }
         else if (job.Status == StatusType.SignalForPreconditions)
            job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4HelpHandling(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External, "HelpHandle",
                     new List<Job>
                     {                        
                        new Job(SendType.Self, 02 /* Execute DoWork4HelpHandling */){WhereIsInputData = WhereIsInputDataType.StepBack}
                     }){WhereIsInputData = WhereIsInputDataType.StepBack}
               });
            job.Status = StatusType.WaitForPreconditions;
         }
         else if (job.Status == StatusType.SignalForPreconditions)
            job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Odbc(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.OwnerDefineWorkWith.AddRange(new List<Job>
               {
                  new Job(SendType.External, "Odbc",
                     new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute BeginTransaction */){WhereIsInputData = WhereIsInputDataType.StepBack}                  
                     }){WhereIsInputData = WhereIsInputDataType.StepBack}
               });
            job.Status = StatusType.WaitForPreconditions;
         }
         else if (job.Status == StatusType.SignalForPreconditions)
            job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Desktop(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External, "Desktop",
                     new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "desktop"},
                        new Job(SendType.Self, 02 /* Execute DoWork4Desktop */)
                     })
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RedoLog(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            //job.GenerateInputData = GenerateDataType.Dynamic;
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job> 
               {
                  new Job(SendType.External, "Program", "DataGuard:Login:Login",01 /* Execute Get */, SendType.SelfToUserInterface)
                  {
                     Input = "username",
                     AfterChangedOutput = new Action<object>
                     ((output) =>
                        {
                           (job.Input as List<string>)[1] = string.Format("<RedoLog><UserName>{0}</UserName>{1}</RedoLog>", output.ToString(), (job.Input as List<string>)[1]);
                        }
                     )
                  }
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Next =
               new Job(SendType.External, "RedoLog",
                  new List<Job>
                  {
                     new Job(SendType.Self, 02 /* Execute DoWork4RedoLog */){Input = job.Input}
                  }) ;
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4AccessPrivilege(Job job)
      {
         Job _GetUserName = new Job(SendType.External, "Program", "DataGuard:Login:Login", 01 /* Execute Get */, SendType.SelfToUserInterface) { Input = "username" };
         Manager(_GetUserName);

         var getInput = job.Input as List<string>;

         Job _AccessPrivilege = new Job(SendType.Self, 04 /* DoWork4Odbc */)
            {
               Input = new List<object>
                  {
                     false,
                     "func",
                     true,
                     true,
                     "xml",
                     string.Format("<AP><UserName>{0}</UserName>{1}</AP>", _GetUserName.Output, getInput[0]),
                     string.Format("Select {0}.AccessPrivilege(?)", getInput[1]),
                     "iProject",
                     "scott"
                  },
               AfterChangedOutput = new Action<object>((output) => { bool result = (bool)output; job.Status = result ? StatusType.Successful : StatusType.Failed; job.Output = result; })
            };
         Manager(_AccessPrivilege);         
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      public void LangChangToFarsi(Job job)
      {
         CultureInfo ci = new CultureInfo("fa-ir");
         ci.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;

         InputLanguage il = InputLanguage.FromCulture(ci);

         Application.CurrentInputLanguage = il;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      public void LangChangToEnglish(Job job)
      {
         System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("EN");
         InputLanguage il = InputLanguage.FromCulture(ci);
         Application.CurrentInputLanguage = il;

         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ChangeHandling(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(new Job(SendType.External, "ChangeHandle", "", 02 /* Execute DoWork4ChangeHandle */, SendType.Self){WhereIsInputData = WhereIsInputDataType.StepBack});
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 11
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ReadAllAccessRoles(Job job)
      {
         Job _GetUserName = new Job(SendType.External, "Program", "DataGuard:Login:Login", 01 /* Execute Get */, SendType.SelfToUserInterface) { Input = "username" };
         Manager(_GetUserName);

         Job _ReadAllAccessRoles = new Job(SendType.Self, 04 /* DoWork4Odbc */)
         {
            Input = new List<object>
                  {
                     false,
                     "procedure",
                     true,
                     true,
                     "xml",
                     string.Format("<AR><UserName>{0}</UserName></AR>", _GetUserName.Output),
                     "{ Call DataGuard.ReadAllAccessRoles(?) }",
                     "iProject",
                     "scott"
                  },
            AfterChangedOutput = new Action<object>((output) => 
            { 
               job.Output = output; 
            })
         };
         Manager(_ReadAllAccessRoles);
      }
      
      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RoleSettings4CurrentUser(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Program", "DataGuard:SecurityPolicy:Role", 08 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self));
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 13
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Import2Odbc(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.OwnerDefineWorkWith.AddRange(new List<Job>
               {
                  new Job(SendType.External, "Odbc",
                     new List<Job>
                     {
                        new Job(SendType.Self, 07 /* Execute Import2Odbc */){Input = job.Input}
                     })
               });
            job.Status = StatusType.WaitForPreconditions;
         }
         else if (job.Status == StatusType.SignalForPreconditions)
            job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ReadUnitTypeOfRoles(Job job)
      {
         Job _ReadAllAccessRoles = new Job(SendType.Self, 04 /* DoWork4Odbc */)
         {
            Input = new List<object>
                  {
                     false,
                     "procedure",
                     true,
                     true,
                     "xml",
                     job.Input,
                     "{ Call ServiceDef.ReadTypeOfRoles(?) }",
                     "iProject",
                     "scott"
                  },
            AfterChangedOutput = new Action<object>((output) =>
            {
               job.Output = output;
            })
         };
         Manager(_ReadAllAccessRoles);
      }

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ReadGroupHeaderOfRoles(Job job)
      {
         Job _ReadAllAccessRoles = new Job(SendType.Self, 04 /* DoWork4Odbc */)
         {
            Input = new List<object>
                  {
                     false,
                     "procedure",
                     true,
                     true,
                     "xml",
                     job.Input,
                     "{ Call ServiceDef.GetGroupHeadersOfRoles(?) }",
                     "iProject",
                     "scott"
                  },
            AfterChangedOutput = new Action<object>((output) =>
            {
               job.Output = output;
            })
         };
         Manager(_ReadAllAccessRoles);
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LoadParentServicesOfGroupHeaders(Job job)
      {
         Job _ReadAllAccessRoles = new Job(SendType.Self, 04 /* DoWork4Odbc */)
         {
            Input = new List<object>
                  {
                     false,
                     "procedure",
                     true,
                     true,
                     "xml",
                     job.Input,
                     "{ Call ServiceDef.LoadParentServicesOfGroupHeaders(?) }",
                     "iProject",
                     "scott"
                  },
            AfterChangedOutput = new Action<object>((output) =>
            {
               job.Output = output;
            })
         };
         Manager(_ReadAllAccessRoles);
      }

      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LoadServicesOfParentService(Job job)
      {
         Job _ReadAllAccessRoles = new Job(SendType.Self, 04 /* DoWork4Odbc */)
         {
            Input = new List<object>
                  {
                     false,
                     "procedure",
                     true,
                     true,
                     "xml",
                     job.Input,
                     "{ Call ServiceDef.LoadServicesOfParentService(?) }",
                     "iProject",
                     "scott"
                  },
            AfterChangedOutput = new Action<object>((output) =>
            {
               job.Output = output;
            })
         };
         Manager(_ReadAllAccessRoles);
      }

      /// <summary>
      /// Code 18
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GroupHeaderSettings4CurrentUser(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Program", "ServiceDefinition:GroupHeader", 03 /* Execute DoWork4GroupHeaderSettings4CurrentUser */, SendType.Self));
      }

      /// <summary>
      /// Code 19
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ServiceUnitType(Job job)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Program" ,"ServiceDefinition:Service:UnitType",02 /* Execute DoWork4ServiceUnitType */, SendType.Self){Input = 2 /* Type */});
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetRegisterOdbcDatasource(Job job)
      {
         Gateway(
            new Job(SendType.External, "Localhost", "Odbc:OdbcSettings", 06 /* Execute GetSystemDataSourceNames */, SendType.SelfToUserInterface)
            {
               AfterChangedOutput = new Action<object>(
                  (output) => 
                     {
                        job.Output = output;
                     })
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Form_Stng(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "FORM_STNG", "", 02 /* Execute DoWork4FRPT_STNG_F */, SendType.Self) 
               { WhereIsInputData = WhereIsInputDataType.StepBack }
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 22
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetConnectionString(Job job)
      {
         // Input <Database>iScsc</Database><Dbms>SqlServer</Dbms>
         // Get Current User <User>UserName</User>
         var GetCurrentUser = new Job(SendType.Self, 12 /* Execute DoWork4RoleSettings4CurrentUser */);
         Manager( GetCurrentUser );

         // 1395/12/22 * بدست آوردن اطلاعات کلاینت برای بدست آوردن و ذخیره کردن اطلاعات کلاینت درون سیستم
         if (_HostInfo == null)
         {
            var GetHostName = new Job(SendType.Self, 24 /* Execute DoWork4GetHostInfo */);
            Manager(GetHostName);
            _HostInfo = (XElement)GetHostName.Output;

            Job _SaveHostInfo = new Job(SendType.Self, 04 /* DoWork4Odbc */)
            {
               Input =
                  new List<object>
                  {
                     false,
                     "procedure",
                     true,
                     false,
                     "xml",
                     string.Format(@"<Request Rqtp_Code=""ManualSaveHostInfo"">{0}<User>{1}</User>{2}</Request>", job.Input, GetCurrentUser.Output.ToString() == "" ? "scott" : GetCurrentUser.Output.ToString(), _HostInfo.ToString()),
                     "{ CALL DataGuard.SaveHostInfo(?) }",
                     "iProject",
                     "scott"
                  },
               AfterChangedOutput = new Action<object>((output) => { if (!output.Equals(System.DBNull.Value)) { string result = (string)output; job.Status = string.IsNullOrEmpty(result) ? StatusType.Failed : StatusType.Successful; job.Output = result; } else { job.Status = StatusType.Failed; } })
            };
            Manager(_SaveHostInfo);  
         }

         Job _GetConnectionString = new Job(SendType.Self, 04 /* DoWork4Odbc */)
         {
            Input =
               new List<object>
               {
                  false,
                  "func",
                  true,
                  true,
                  "xml",
                  string.Format(@"<Request Rqtp_Code=""ConnectionString"">{0}<User>{1}</User>{2}</Request>", job.Input, GetCurrentUser.Output.ToString() == "" ? "scott" : GetCurrentUser.Output.ToString(), _HostInfo.ToString()),
                  "SELECT dbo.GET_CNST_U(?)",
                  "iProject",
                  "scott"
               },
            AfterChangedOutput = new Action<object>((output) => { if (!output.Equals(System.DBNull.Value)) { string result = (string)output; job.Status = string.IsNullOrEmpty(result) ? StatusType.Failed : StatusType.Successful; job.Output = result; } else { job.Status = StatusType.Failed; } })
         };
         Manager(_GetConnectionString);  
      }

      /// <summary>
      /// Code 23
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4AppForm_Stng(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "FORM_STNG", "", 03 /* Execute DoWork4AFRM_STNG_F */, SendType.Self) { WhereIsInputData = WhereIsInputDataType.StepBack }
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 24
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetHosInfo(Job job)
      {
         if(_HostInfo != null)
         {
            job.Output = _HostInfo;
            job.Status = StatusType.Successful;
            return;
         }

         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Program", "DataGuard", 04 /* Execute DoWork4GetHosInfo */, SendType.Self)
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 25
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Shutingdown(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  //new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_hrsr_f"},
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 05 /* Execute CheckSecurity */),                  
                  new Job(SendType.SelfToUserInterface, "Shutdown", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "Shutdown", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "Shutdown", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "ADM_HRSR_F", 10 /* Execute Actn_CalF_P */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 26
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4DateTimes(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "DateTimes", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "DateTimes", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "DateTimes", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 27
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SendMail(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy", 01 /* Execute GetUi */, SendType.Self){Input = "SettingsMailServer"},
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsMailServer", 02 /* Execute Set */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsMailServer", 100 /* Execute SendMail */, SendType.SelfToUserInterface){Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 28
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetUserProfile(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy", 01 /* Execute GetUi */, SendType.Self){Input = "SettingsAccount"},
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsAccount", 02 /* Execute Set */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsAccount", 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsAccount", 01 /* Execute Get */, SendType.SelfToUserInterface){Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 29
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ShowUserProfile(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy", 14 /* Execute DoWork4SettingsAccount */, SendType.Self),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsAccount", 10 /* Execute Actn_CalF_P */, SendType.SelfToUserInterface)                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 30
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetTimePeriod(Job job)
      {
         var xinput = job.Input as XElement;
         var currentdate = Convert.ToDateTime(xinput.Attribute("crntdate").Value);

         DateDiff datediff = new DateDiff(currentdate, DateTime.Now);

         switch (xinput.Attribute("timetype").Value)
         {
            case "normal":
               #region Normal Time Period
               // زمان گذشته
               if (currentdate <= DateTime.Now)
               {
                  if (datediff.ElapsedYears > 0)
                  {
                     job.Output = string.Format("برای {0} سال قبل", datediff.ElapsedYears);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedMonths > 0)
                  { 
                     job.Output = string.Format("برای {0} ماه قبل", datediff.ElapsedMonths);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays >= 2 || (datediff.ElapsedDays == 1 && datediff.ElapsedHours >= 12))
                  {
                     job.Output = string.Format("برای {0} روز قبل", datediff.ElapsedHours >= 12 ? datediff.ElapsedDays + 1 : datediff.ElapsedDays);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays == 1)
                  { 
                     job.Output = "دیروز";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedHours > 0)
                  { 
                     job.Output = string.Format("برای {0} ساعت قبل", datediff.ElapsedHours);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedMinutes > 0)
                  { 
                     job.Output = string.Format("برای {0} دقیقه قبل", datediff.ElapsedMinutes);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedSeconds > 0)
                  { 
                     job.Output = string.Format("برای {0} ثانیه قبل", datediff.ElapsedSeconds);
                     job.Status = StatusType.Successful;
                     return;
                  }
                  
                  job.Output = "همین الان";
               }
               // زمان آینده 
               else
               {
                  if (datediff.ElapsedYears < 0)
                  { 
                     job.Output = string.Format("برای {0} سال دیگه", datediff.ElapsedYears * -1);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedMonths < 0)
                  { 
                     job.Output = string.Format("برای {0} ماه دیگه", datediff.ElapsedMonths * -1);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays <= -2 || (datediff.ElapsedDays == -1 && datediff.ElapsedHours <= -12))
                  { 
                     job.Output = string.Format("برای {0} روز دیگه", datediff.ElapsedHours <= -12 ? (datediff.ElapsedDays * -1) + 1 : datediff.ElapsedDays * -1);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays == -1)
                  { 
                     job.Output = "فردا";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedHours < 0)
                  { 
                     job.Output = string.Format("برای {0} ساعت دیگه", datediff.ElapsedHours * -1);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedMinutes < 0)
                  { 
                     job.Output = string.Format("برای {0} دقیقه دیگه", datediff.ElapsedMinutes * -1);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedSeconds < 0)
                  { 
                     job.Output = string.Format("برای {0} ثانیه دیگه", datediff.ElapsedSeconds * -1);
                     job.Status = StatusType.Successful;
                     return;
                  }
               }
               #endregion
               break;
            case "group":
               #region Group Time Period
               if (currentdate <= DateTime.Now)
               {
                  if (datediff.ElapsedYears > 0)
                  { 
                     job.Output = string.Format("گام 11 : برای {0} سال قبل", datediff.ElapsedYears);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedMonths > 0)
                  { 
                     job.Output = string.Format("گام 10 : برای {0} ماه قبل", datediff.ElapsedMonths);
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays >= 7)
                  { 
                     job.Output = "گام 9 : برای ایام گذشته ماه جاری";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if ((datediff.ElapsedDays > 1 && datediff.ElapsedDays <= 6) || (datediff.ElapsedDays == 1 && datediff.ElapsedHours >= 12))
                  { 
                     job.Output = "گام 8 : برای هفته پیش";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays == 1 || (datediff.ElapsedDays == 0 && datediff.ElapsedHours >= 12))
                  { 
                     job.Output = "گام 7 : دیروز";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (datediff.ElapsedDays == 0)
                  { 
                     job.Output = "گام 6 : امروز";
                     job.Status = StatusType.Successful;
                     return;
                  }
               }
               // زمان آینده 
               else
               {
                  if (Math.Abs(datediff.ElapsedYears) > 0)
                  { 
                     job.Output = string.Format("گام 1 : برای {0} سال بعد", Math.Abs(datediff.ElapsedYears));
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (Math.Abs(datediff.ElapsedMonths) > 0)
                  { 
                     job.Output = string.Format("گام 2 : برای {0} ماه بعد", Math.Abs(datediff.ElapsedMonths));
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (Math.Abs(datediff.ElapsedDays) > 7)
                  {
                     job.Output = "گام 3 : برای ایام در پیشرو ماه جاری";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if ((Math.Abs(datediff.ElapsedDays) > 1 && Math.Abs(datediff.ElapsedDays) <= 6) || (Math.Abs(datediff.ElapsedDays) == 1 && Math.Abs(datediff.ElapsedHours) >= 12))
                  {
                     job.Output = "گام 4 : برای هفته در پیشرو";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  if (Math.Abs(datediff.ElapsedDays) == 1 || (datediff.ElapsedDays == 0 && Math.Abs(datediff.ElapsedHours) >= 12))
                  { 
                     job.Output = "گام 5 : فردا";
                     job.Status = StatusType.Successful;
                     return;
                  }

                  job.Output = "گام 6 : امروز";
                  job.Status = StatusType.Successful;
               }
               #endregion
               break;
            default:
               break;
         }

         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 31
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GMapNets(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, "GMapNets", 02 /* Execute Set */),                  
                  new Job(SendType.SelfToUserInterface, "GMapNets", 07 /* Execute Load_Data */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "GMapNets", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 32
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4SendFeedBack(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy", 01 /* Execute GetUi */, SendType.Self){Input = "settingssendemail"},
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsSendEmail", 02 /* Execute Set */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsSendEmail", 07 /* Execute Load */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsSendEmail", 03 /* Execute Paint */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsSendEmail", 10 /* Execute ActionCallWindows */, SendType.SelfToUserInterface){Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 33
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4PosSettings(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy", 01 /* Execute GetUi */, SendType.Self){Input = "settingsdevice"},
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsDevice", 02 /* Execute Set */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsDevice", 07 /* Execute Load */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsDevice", 03 /* Execute Paint */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsDevice", 10 /* Execute ActionCallWindows */, SendType.SelfToUserInterface){Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 34
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4PaymentPos(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy", 01 /* Execute GetUi */, SendType.Self){Input = "settingspaymentpos"},
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsPaymentPos", 02 /* Execute Set */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsPaymentPos", 07 /* Execute Load */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsPaymentPos", 03 /* Execute Paint */, SendType.SelfToUserInterface),
                  new Job(SendType.External,"Program", "DataGuard:SecurityPolicy:SettingsPaymentPos", 10 /* Execute ActionCallWindows */, SendType.SelfToUserInterface){Input = job.Input}
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 35
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetServer(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Program", "DataGuard", 11 /* Execute DoWork4GetServer */, SendType.Self) { WhereIsInputData = WhereIsInputDataType.StepBack}
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 36
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetWindowsPlatform(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.OwnerDefineWorkWith.AddRange(new List<Job>
               {
                  new Job(SendType.External, "Odbc",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface,"OdbcSettings", 11 /* Execute WindowsPlatform */)
                     })
               });
            job.Status = StatusType.WaitForPreconditions;
         }
         else if (job.Status == StatusType.SignalForPreconditions)
            job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 37
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GetLicenseDay(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Program", "DataGuard", 33 /* Execute DoWork4GetLicenseDay */, SendType.Self) { WhereIsInputData = WhereIsInputDataType.StepBack }
            );
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 38
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4PingNetwork(Job job)
      {
         bool pingStatus = false;

         using (Ping p = new Ping())
         {
            byte[] buffer = Encoding.ASCII.GetBytes("Hello World!");
            int timeout = 4444; // 4s

            try
            {
               PingReply reply = p.Send(job.Input.ToString(), timeout, buffer);
               job.Output = pingStatus = (reply.Status == IPStatus.Success);
               job.Status = StatusType.Successful;
            }
            catch (Exception)
            {
               job.Output = pingStatus = false;
               job.Status = StatusType.Successful;
            }
         }
      }
   }
}
