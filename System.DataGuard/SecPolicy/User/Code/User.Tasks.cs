using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataGuard.SecPolicy.User.Code
{
   partial class User
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input as string;

         value = value.ToLower();
         if (value == "createnew")
         {
            if (_CreateNew == null)
               _CreateNew = new Ui.CreateNew { _DefaultGateway = this };
            job.Output = _CreateNew;
         }
         else if (value == "propertysinglemenu")
         {
            if (_PropertySingleMenu == null)
               _PropertySingleMenu = new Ui.PropertySingleMenu { _DefaultGateway = this };
            job.Output = _PropertySingleMenu;
         }
         else if (value == "duplicate")
         {
            if (_Duplicate == null)
               _Duplicate = new Ui.Duplicate { _DefaultGateway = this };
            job.Output = _Duplicate;
         }
         else if (value == "profile")
         {
            if (_Profile == null)
               _Profile = new Ui.Profile { _DefaultGateway = this };
            job.Output = _Profile;
         }
         else if(value == "currentuserinfo")
         {
            if (_CurrentUserInfo == null)
               _CurrentUserInfo = new Ui.CurrentUserInfo { _DefaultGateway = this };
            job.Output = _CurrentUserInfo;
         }
         else if (value == "currentchangeusername")
         {
            if (_CurrentChangeUserName == null)
               _CurrentChangeUserName = new Ui.CurrentChangeUserName { _DefaultGateway = this };
            job.Output = _CurrentChangeUserName;
         }
         else if (value == "currentchangepassword")
         {
            if (_CurrentChangePassword == null)
               _CurrentChangePassword = new Ui.CurrentChangePassword { _DefaultGateway = this };
            job.Output = _CurrentChangePassword;
         }
         else if (value == "currentchangeusertype")
         {
            if (_CurrentChangeUserType == null)
               _CurrentChangeUserType = new Ui.CurrentChangeUserType { _DefaultGateway = this };
            job.Output = _CurrentChangeUserType;
         }
         else if (value == "otherusers")
         {
            if (_OtherUsers == null)
               _OtherUsers = new Ui.OtherUsers { _DefaultGateway = this };
            job.Output = _OtherUsers;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4CreateNew(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "CreateNew"},
                  new Job(SendType.SelfToUserInterface, "CreateNew", 03 /* Execute Paint */),
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4UserPropertyMenu(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "PropertySingleMenu"},
                  new Job(SendType.SelfToUserInterface, "PropertySingleMenu", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "PropertySingleMenu", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void Dowork4Profile(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "Profile"},
                  new Job(SendType.SelfToUserInterface, "Profile", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "Profile", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "Profile", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 05
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4NotExists(Job job)
      {
         Job NotExistsUser =
            new Job(SendType.External, "Commons",
               new List<Job>
               {
                  new Job(SendType.Self, 04 /* DoWork4Odbc */)
                  {
                     Input = new List<object>
                        {
                           false,
                           "func",
                           true,
                           true,
                           "xml",
                           job.Input,
                           "Select DataGuard.NotExistsUser(?)",
                           "iProject",
                           "scott"
                        },
                  AfterChangedOutput = new Action<object>((output) => 
                  { 
                     bool result = (bool)output; 
                     job.Status = result ? StatusType.Successful : StatusType.Failed;
                     job.Output = result; 
                  })
                  }
               });
         Manager(NotExistsUser);
      }

      /// <summary>
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4NewUserPropertyMenu(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "PropertySingleMenu"},
                  new Job(SendType.SelfToUserInterface, "PropertySingleMenu", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "PropertySingleMenu", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 07
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4Duplicate(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "Duplicate"},
                  new Job(SendType.SelfToUserInterface, "Duplicate", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "Duplicate", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4CurrentUserInfo(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "currentuserinfo"},
                  new Job(SendType.SelfToUserInterface, "CurrentUserInfo", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CurrentUserInfo", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 09
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4CurrentChangeUserName(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "currentchangeusername"},
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserName", 01 /* Execute Get */),
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserName", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserName", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 10
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4CurrentChangePassword(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "currentchangepassword"},
                  new Job(SendType.SelfToUserInterface, "CurrentChangePassword", 01 /* Execute Get */),
                  new Job(SendType.SelfToUserInterface, "CurrentChangePassword", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CurrentChangePassword", 03 /* Execute Paint */)
               });
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
      private void DoWork4CurrentChangeUserType(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "currentchangeusertype"},
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserType", 01 /* Execute Get */),
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserType", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserType", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "CurrentChangeUserType", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 12
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4OtherUsers(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "otherusers"},
                  new Job(SendType.SelfToUserInterface, "OtherUsers", 01 /* Execute Get */),
                  new Job(SendType.SelfToUserInterface, "OtherUsers", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "OtherUsers", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "OtherUsers", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
