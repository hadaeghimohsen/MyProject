using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Emis.Sas.Controller
{
   partial class Sas
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "mstr_page_f")
         {
            if (_Mstr_Page_F == null)
               _Mstr_Page_F = new View.MSTR_PAGE_F { _DefaultGateway = this };
         }
         else if (value == "mstr_serv_f")
         {
            if (_Mstr_Serv_F == null)
               _Mstr_Serv_F = new View.MSTR_SERV_F { _DefaultGateway = this };
         }
         else if (value == "mstr_rqst_f")
         {
            if (_Mstr_Rqst_F == null)
               _Mstr_Rqst_F = new View.MSTR_RQST_F { _DefaultGateway = this };
         }
         else if (value == "serv_bill_f")
         {
            if (_SERV_BILL_F == null)
               _SERV_BILL_F = new View.SERV_BILL_F { _DefaultGateway = this };
         }
         else if (value == "serv_info_f")
         {
            if (_SERV_INFO_F == null)
               _SERV_INFO_F = new View.SERV_INFO_F { _DefaultGateway = this };
         }
         else if (value == "serv_dart_f")
         {
            if (_SERV_DART_F == null)
               _SERV_DART_F = new View.SERV_DART_F { _DefaultGateway = this };
         }
         else if (value == "serv_rqst_f")
         {
            if (_SERV_RQST_F == null)
               _SERV_RQST_F = new View.SERV_RQST_F { _DefaultGateway = this };
         }
         else if (value == "pblc_serv_f")
         {
            if (_PBLC_SERV_F == null)
               _PBLC_SERV_F = new View.PBLC_SERV_F { _DefaultGateway = this };
         }
         else if (value == "mstr_regl_f")
         {
            if (_MSTR_REGL_F == null)
               _MSTR_REGL_F = new View.MSTR_REGL_F { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Mstr_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_page_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 08 /* Execute LoadActiveDataSource */),
                  new Job(SendType.SelfToUserInterface, "MSTR_PAGE_F", 03 /* Execute Paint */)
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
      private void Mstr_Serv_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_serv_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_SERV_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "MSTR_SERV_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_SERV_F", 03 /* Execute Paint */)
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
      private void Mstr_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_RQST_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "MSTR_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_RQST_F", 03 /* Execute Paint */)
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
      private void Serv_Bill_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "serv_bill_f"},
                  new Job(SendType.SelfToUserInterface, "SERV_BILL_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SERV_BILL_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SERV_BILL_F", 03 /* Execute Paint */)
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
      private void Serv_Info_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "serv_info_f"},
                  new Job(SendType.SelfToUserInterface, "SERV_INFO_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SERV_INFO_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SERV_INFO_F", 03 /* Execute Paint */)
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
      private void Serv_Dart_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "serv_dart_f"},
                  new Job(SendType.SelfToUserInterface, "SERV_DART_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SERV_DART_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SERV_DART_F", 03 /* Execute Paint */)
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
      private void Serv_Rqst_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "serv_rqst_f"},
                  new Job(SendType.SelfToUserInterface, "SERV_RQST_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "SERV_RQST_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "SERV_RQST_F", 03 /* Execute Paint */)
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
      private void Pblc_Serv_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pblc_serv_f"},
                  new Job(SendType.SelfToUserInterface, "PBLC_SERV_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "PBLC_SERV_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "PBLC_SERV_F", 03 /* Execute Paint */)
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
      private void Mstr_Regl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mstr_regl_f"},
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 02 /* Execute Set */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 07 /* Execute LoadData */),
                  new Job(SendType.SelfToUserInterface, "MSTR_REGL_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
