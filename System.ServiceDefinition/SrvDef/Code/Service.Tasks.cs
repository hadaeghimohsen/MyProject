using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ServiceDefinition.SrvDef.Code
{
   partial class Service
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "create")
         {
            if (_Create == null)
               _Create = new Ui.Create { _DefaultGateway = this };
         }
         else if (value == "sur")
         {
            if (_ShowUpdateRemove == null)
               _ShowUpdateRemove = new Ui.ShowUpdateRemove { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LoadParentServicesOfGroupHeaders(Job job)
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
                           "{ Call ServiceDef.LoadParentServicesOfGroupHeaders(?) }",
                           "iProject",
                           "scott"
                        }
                     }
                  }));
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
      private void DoWork4CreateNew(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "create"},
                  new Job(SendType.SelfToUserInterface, "Create", 02 /* Execute Set */){ Input = job.Input },
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
      /// Code 04
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4LoadServicesOfParentService(Job job)
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
                           "{ Call ServiceDef.LoadServicesOfParentService(?) }",
                           "iProject",
                           "scott"
                        }
                     }
                  }));
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
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "sur"},
                  new Job(SendType.SelfToUserInterface, "SUR", 02 /* Execute Set */){ Input = job.Input },
                  new Job(SendType.SelfToUserInterface, "SUR", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SUR", 03 /* Execute Paint */)
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
      private void DoWork4LoadGrpSrvInGrpHdrWithCondition(Job job)
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
                           "{ Call ServiceDef.[LoadGrpSrvInGrpHdrWithCondition](?) }",
                           "iProject",
                           "scott"
                        }
                     }
                  }));
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      
   }
}
