using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.ServiceDefinition.GrpHdr.Code
{
   partial class GroupHeader
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input as string;
         if (value.ToLower() == "groupheadermenus")
         {
            if (_GroupHeaderMenus == null)
               _GroupHeaderMenus = new Ui.GroupHeaderMenus { _DefaultGateway = this };
         }
         else if (value.ToLower() == "create")
         {
            if (_Create == null)
               _Create = new Ui.Create { _DefaultGateway = this };
         }
         else if (value.ToLower() == "update")
         {
            if (_Update == null)
               _Update = new Ui.Update { _DefaultGateway = this };
         }
         else if (value.ToLower() == "duplicate")
         {
            if (_Duplicate == null)
               _Duplicate = new Ui.Duplicate { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4ReadAccessGroupHeader(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.Add(
               new Job(SendType.External, "Commons",
                  new List<Job>
                  {
                     new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
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
                        }
                     }
                  }));
         }
         else
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4GroupHeaderSettings4CurrentUser(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "GroupHeaderMenus"},
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "GroupHeaderMenus", 03 /* Execute Paint */)
               });
         }
         else
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 04
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
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "Create"},
                  new Job(SendType.SelfToUserInterface, "Create", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "Create", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "Create", 03 /* Execute Paint */)
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
      private void DoWork4Update(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "Update"},
                  new Job(SendType.SelfToUserInterface, "Update", 02 /* Execute Set */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "Update", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "Update", 03 /* Execute Paint */)
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
      private void DoWork4Duplicate(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "Duplicate"},
                  new Job(SendType.SelfToUserInterface, "Duplicate", 02 /* Execute Set */){Input = job.Input},
                  new Job(SendType.SelfToUserInterface, "Duplicate", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
