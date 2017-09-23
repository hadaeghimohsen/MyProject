using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ISP.Code
{
   partial class ISP
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if(value == "frst_page_f")
         {
            if (_Frst_Page_F == null)
               _Frst_Page_F = new Ui.MasterPage.FRST_PAGE_F { _DefaultGateway = this };
         }
         else if (value == "regn_dfin_f")
         {
            if (_Regn_Dfin_F == null)
               _Regn_Dfin_F = new Ui.BaseDefination.REGN_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "epit_dfin_f")
         {
            if (_Epit_Dfin_F == null)
               _Epit_Dfin_F = new Ui.BaseDefination.EPIT_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "btrf_dfin_f")
         {
            if (_Btrf_Dfin_F == null)
               _Btrf_Dfin_F = new Ui.BaseDefination.BTRF_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "cash_dfin_f")
         {
            if (_Cash_Dfin_F == null)
               _Cash_Dfin_F = new Ui.BaseDefination.CASH_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "regl_dfin_f")
         {
            if (_Regl_Dfin_F == null)
               _Regl_Dfin_F = new Ui.BaseDefination.REGL_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "rqrq_dfin_f")
         {
            if (_Rqrq_Dfin_F == null)
               _Rqrq_Dfin_F = new Ui.BaseDefination.RQRQ_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "dcsp_dfin_f")
         {
            if (_Dcsp_Dfin_F == null)
               _Dcsp_Dfin_F = new Ui.BaseDefination.DCSP_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "orgn_dfin_f")
         {
            if (_Orgn_Dfin_F == null)
               _Orgn_Dfin_F = new Ui.BaseDefination.ORGN_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "adm_adsl_f")
         {
            if (_Adm_Adsl_F == null)
               _Adm_Adsl_F = new Ui.Admission.ADM_ADSL_F { _DefaultGateway = this };
         }
         else if (value == "pay_mtod_f")
         {
            if (_Pay_Mtod_F == null)
               _Pay_Mtod_F = new Ui.Payment.PAY_MTOD_F { _DefaultGateway = this };
         }
         
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void Frst_Page_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "frst_page_f"},
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 03 /* Execute Paint */)                  
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
      private void Regn_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regn_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "REGN_DFIN_F", 03 /* Execute Paint */)
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
      private void Epit_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "epit_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "EPIT_DFIN_F", 03 /* Execute Paint */)
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
      private void Btrf_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "btrf_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "BTRF_DFIN_F", 03 /* Execute Paint */)
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
      private void Cash_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cash_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "CASH_DFIN_F", 03 /* Execute Paint */)
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
      private void Regl_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "regl_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "REGL_DFIN_F", 03 /* Execute Paint */)
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
      private void Rqrq_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rqrq_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RQRQ_DFIN_F", 03 /* Execute Paint */)
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
      private void Dcsp_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "dcsp_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "DCSP_DFIN_F", 03 /* Execute Paint */)
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
      private void Orgn_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "orgn_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ORGN_DFIN_F", 03 /* Execute Paint */)
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
      private void Adm_Adsl_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "adm_adsl_f"},
                  new Job(SendType.SelfToUserInterface, "ADM_ADSL_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ADM_ADSL_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ADM_ADSL_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ADM_ADSL_F", 03 /* Execute Paint */)
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
      private void Pay_Mtod_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "pay_mtod_f"},
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "PAY_MTOD_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
