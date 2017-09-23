using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace System.Reporting.WorkFlow.Code
{
   partial class WorkFlow
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "rpt_srpt_f")
         {
            if (_RPT_SRPT_F == null)
               _RPT_SRPT_F = new Ui.RPT_SRPT_F { _DefaultGateway = this };
         }
         else if (value == "rpt_srch_f")
         {
            if (_RPT_SRCH_F == null)
               _RPT_SRCH_F = new Ui.RPT_SRCH_F { _DefaultGateway = this };
         }
         else if (value == "rpt_mbar_m")
         {
            if (_RPT_MBAR_M == null)
               _RPT_MBAR_M = new Ui.RPT_MBAR_M { _DefaultGateway = this };
         }
         else if (value == "prf_sprf_f")
         {
            if (_PRF_SPRF_F == null)
               _PRF_SPRF_F = new Ui.PRF_SPRF_F { _DefaultGateway = this };
         }
         else if (value == "prf_srch_f")
         {
            if (_PRF_SRCH_F == null)
               _PRF_SRCH_F = new Ui.PRF_SRCH_F { _DefaultGateway = this };
         }
         else if (value == "prf_mbar_m")
         {
            if (_PRF_MBAR_M == null)
               _PRF_MBAR_M = new Ui.PRF_MBAR_M { _DefaultGateway = this };
         }
         else if (value == "prf_chng_f")
         {
            if (_PRF_CHNG_F == null)
               _PRF_CHNG_F = new Ui.PRF_CHNG_F { _DefaultGateway = this };
         }
         else if (value == "whr_scon_f")
         {
            if (_WHR_SCON_F == null)
               _WHR_SCON_F = new Ui.WHR_SCON_F { _DefaultGateway = this };
         }
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RPT_SRPT_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_srpt_f"},
                  new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 02 /* Execute Set */){Input = "PRF_SPRF_F"},
                  new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 07 /* Execute LoadData */),//{Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "RPT_SRPT_F", 03 /* Execute Paint */)
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
      private void DoWork4RPT_SRCH_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_srch_f"},
                  new Job(SendType.SelfToUserInterface, "RPT_SRCH_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "RPT_SRCH_F", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "RPT_SRCH_F", 07 /* Execute LoadData */)
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
      private void DoWork4PRF_SRCH_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "prf_srch_f"},
                  new Job(SendType.SelfToUserInterface, "PRF_SRCH_F", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "PRF_SRCH_F", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous},
                  new Job(SendType.SelfToUserInterface, "PRF_SRCH_F", 07 /* Execute LoadData */)
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
      private void DoWork4WHR_SCON_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "WHR_SCON_F"},
                        new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                        new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 03 /* Execute Paint */),                        
                        new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 07 /* Execute LoadData */){Executive = ExecutiveType.Asynchronous}  ,                      
                        new Job(SendType.SelfToUserInterface, "WHR_SCON_F", 09 /* Execute LoadActiveDataSource */){Executive = ExecutiveType.Asynchronous},
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
      private void DoWork4PRF_CHNG_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
                     {
                        new Job(SendType.Self, 01 /* Execute GetUi */){Input = "PRF_CHNG_F"},
                        new Job(SendType.SelfToUserInterface, "PRF_CHNG_F", 02 /* Execute Set */){Input = job.Input},
                        new Job(SendType.SelfToUserInterface, "PRF_CHNG_F", 03 /* Execute Paint */),
                        new Job(SendType.SelfToUserInterface, "PRF_CHNG_F", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous}
                     });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
