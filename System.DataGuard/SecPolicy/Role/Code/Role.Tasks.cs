using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.DataGuard.SecPolicy.Role.Code
{
   partial class Role
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input as string;

         if (value.ToLower() == "createnewrole")
         {
            if (_CreateNewRole == null)
               _CreateNewRole = new Ui.CreateNewRole { _DefaultGateway = this };
            job.Output = _CreateNewRole;
         }
         else if (value.ToLower() == "propertysinglerolemenu")
         {
            if (_PropertySingleRoleMenu == null)
               _PropertySingleRoleMenu = new Ui.PropertySingleRoleMenu { _DefaultGateway = this };
         }
         else if (value.ToLower() == "duplicaterole")
         {
            if (_DuplicateRole == null)
               _DuplicateRole = new Ui.DuplicateRole { _DefaultGateway = this };
            job.Output = _DuplicateRole;
         }
         else if (value.ToLower() == "jlcu2r")
         {
            if (_JoinOrLeaveCurrentUserToRoles == null)
               _JoinOrLeaveCurrentUserToRoles = new Ui.JoinOrLeaveCurrentUserToRoles { _DefaultGateway = this };
            job.Output = _JoinOrLeaveCurrentUserToRoles;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4CreateNewRole(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "createnewrole"},
                  new Job(SendType.SelfToUserInterface, "CreateNewRole", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CreateNewRole", 03 /* Execute Paint */),
                  //new Job(SendType.SelfToUserInterface, "CreateNewRole", 07 /* Execute LoadData */)
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
      private void DoWork4LifeTimeRole(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(new Job(SendType.External, "Commons", "", 01 /* Execute DoWork4LifeTimeObject */, SendType.Self) { WhereIsInputData = WhereIsInputDataType.StepBack });
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
      private void DoWork4RolePropertMenu(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "PropertySingleRoleMenu"},
                  new Job(SendType.SelfToUserInterface, "PropertySingleRoleMenu", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "PropertySingleRoleMenu", 03 /* Execute Paint */)
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
      private void DoWork4RoleChangingName(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Commons", "", 10 /* Execute DoWork4ChangeHandling */, SendType.Self) { WhereIsInputData = WhereIsInputDataType.StepBack});
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
      private void DoWork4DuplicateRole(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new[]
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "DuplicateRole"},
                  new Job(SendType.SelfToUserInterface, "DuplicateRole", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "DuplicateRole", 03 /* Execute Paint */)
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
      private void DoWork4NotExistsRole(Job job)
      {
         Job _AccessPrivilege =
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
                           "Select DataGuard.NotExistsRole(?)",
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
         Manager(_AccessPrivilege);
         //job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 08
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RoleSettings4CurrentUser(Job job)
      {
         Job _GetUserName = new Job(SendType.External, "SecurityPolicy", "DataGuard:Login:Login", 01 /* Execute Get */, SendType.SelfToUserInterface) { Input = "username" };
         Manager(_GetUserName);

         job.Status = StatusType.WaitForPostconditions;
         job.OwnerDefineWorkWith.AddRange(
            new List<Job>
            {
               new Job(SendType.Self, 01 /* Execute GetUi */ ){Input = "JLCU2R"},
               new Job(SendType.SelfToUserInterface, "JLCU2R", 02 /* Execute Set */) {Input = _GetUserName.Output},
               new Job(SendType.SelfToUserInterface, "JLCU2R", 07 /* Execute LoadData */),
               new Job(SendType.SelfToUserInterface, "JLCU2R", 03 /* Execute Paint */)
            });
         job.Output = _GetUserName.Output;
         job.Status = StatusType.Successful;
      }
   }
}
