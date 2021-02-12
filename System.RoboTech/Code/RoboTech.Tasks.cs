using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.RoboTech.Code
{
   partial class RoboTech
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         #region MasterPage
         if (value == "frst_page_f")
         {
            if (_Frst_Page_F == null)
               _Frst_Page_F = new Ui.MasterPage.FRST_PAGE_F { _DefaultGateway = this };
         }
         #endregion
         #region BaseDefinition
         else if (value == "regn_dfin_f")
         {
            if (_Regn_Dfin_F == null)
               _Regn_Dfin_F = new Ui.BaseDefinition.REGN_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "isic_dfin_f")
         {
            if (_Isic_Dfin_F == null)
               _Isic_Dfin_F = new Ui.BaseDefinition.ISIC_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "orgn_dfin_f")
         {
            if (_Orgn_Dfin_F == null)
               _Orgn_Dfin_F = new Ui.BaseDefinition.ORGN_DFIN_F { _DefaultGateway = this };
         }
         else if (value == "wgul_dfin_f")
         {
            if (_Wgul_Dfin_F == null)
               _Wgul_Dfin_F = new Ui.BaseDefinition.WGUL_DFIN_F { _DefaultGateway = this };
         }
         #endregion
         #region DevelopmentApplication
         else if (value == "orgn_dvlp_f")
         {
            if (_Orgn_Dvlp_F == null)
               _Orgn_Dvlp_F = new Ui.DevelopmentApplication.ORGN_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "rbkn_dvlp_f")
         {
            if (_Rbkn_Dvlp_F == null)
               _Rbkn_Dvlp_F = new Ui.DevelopmentApplication.RBKN_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "rbsr_dvlp_f")
         {
            if (_Rbsr_Dvlp_F == null)
               _Rbsr_Dvlp_F = new Ui.DevelopmentApplication.RBSR_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "rbsa_dvlp_f")
         {
            if (_Rbsa_Dvlp_F == null)
               _Rbsa_Dvlp_F = new Ui.DevelopmentApplication.RBSA_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "rbod_dvlp_f")
         {
            if (_Rbod_Dvlp_F == null)
               _Rbod_Dvlp_F = new Ui.DevelopmentApplication.RBOD_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "rbmn_dvlp_f")
         {
            if (_Rbmn_Dvlp_F == null)
               _Rbmn_Dvlp_F = new Ui.DevelopmentApplication.RBMN_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "odrm_dvlp_f")
         {
            if (_Odrm_Dvlp_F == null)
               _Odrm_Dvlp_F = new Ui.DevelopmentApplication.ODRM_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "orml_dvlp_f")
         {
            if (_Orml_Dvlp_F == null)
               _Orml_Dvlp_F = new Ui.DevelopmentApplication.ORML_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "rbsm_dvlp_f")
         {
            if (_Rbsm_Dvlp_F == null)
               _Rbsm_Dvlp_F = new Ui.DevelopmentApplication.RBSM_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "srbt_info_f")
         {
            if (_Srbt_Info_F == null)
               _Srbt_Info_F = new Ui.DevelopmentApplication.SRBT_INFO_F { _DefaultGateway = this };
         }
         else if(value == "sale_dvlp_f")
         {
            if (_Sale_Dvlp_F == null)
               _Sale_Dvlp_F = new Ui.DevelopmentApplication.SALE_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "alpk_dvlp_f")
         {
            if (_Alpk_Dvlp_F == null)
               _Alpk_Dvlp_F = new Ui.DevelopmentApplication.ALPK_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "ordr_ship_f")
         {
            if (_Ordr_Ship_F == null)
               _Ordr_Ship_F = new Ui.DevelopmentApplication.ORDR_SHIP_F { _DefaultGateway = this };
         }
         else if (value == "ordr_rcpt_f")
         {
            if (_Ordr_Rcpt_F == null)
               _Ordr_Rcpt_F = new Ui.DevelopmentApplication.ORDR_RCPT_F { _DefaultGateway = this };
         }
         else if (value == "bank_dvlp_f")
         {
            if (_Bank_Dvlp_F == null)
               _Bank_Dvlp_F = new Ui.DevelopmentApplication.BANK_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "prod_dvlp_f")
         {
            if (_Prod_Dvlp_F == null)
               _Prod_Dvlp_F = new Ui.DevelopmentApplication.PROD_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "wlet_dvlp_f")
         {
            if (_Wlet_Dvlp_F == null)
               _Wlet_Dvlp_F = new Ui.DevelopmentApplication.WLET_DVLP_F { _DefaultGateway = this };
         }
         else if (value == "onro_dvlp_f")
         {
            if (_Onro_Dvlp_F == null)
               _Onro_Dvlp_F = new Ui.DevelopmentApplication.ONRO_DVLP_F { _DefaultGateway = this };
         }
         else if(value == "inst_conf_f")
         {
            if (_Inst_Conf_F == null)
               _Inst_Conf_F = new Ui.DevelopmentApplication.INST_CONF_F { _DefaultGateway = this };
         }
         else if(value == "mesg_dvlp_f")
         {
            if (_Mesg_Dvlp_F == null)
               _Mesg_Dvlp_F = new Ui.DevelopmentApplication.MESG_DVLP_F { _DefaultGateway = this };
         }
         else if(value == "cash_cntr_f")
         {
            if (_Cash_Cntr_F == null)
               _Cash_Cntr_F = new Ui.DevelopmentApplication.CASH_CNTR_F { _DefaultGateway = this };
         }
         #endregion
         #region Action
         else if (value == "strt_robo_f")
         {
            if (_Strt_Robo_F == null)
               _Strt_Robo_F = new Ui.Action.STRT_ROBO_F { _DefaultGateway = this };
         }
         else if (value == "stng_rprt_f")
         {
            if (_Stng_Rprt_F == null)
               _Stng_Rprt_F = new Ui.Action.STNG_RPRT_F { _DefaultGateway = this };
         }
         else if (value == "rpt_mngr_f")
         {
            if (_Rpt_Mngr_F == null)
               _Rpt_Mngr_F = new Ui.Action.RPT_MNGR_F { _DefaultGateway = this };
         }
         else if (value == "rpt_lrfm_f")
         {
            if (_Rpt_Lrfm_F == null)
               _Rpt_Lrfm_F = new Ui.Action.RPT_LRFM_F { _DefaultGateway = this };
         }
         #endregion
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
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 07 /* Execute Load_Data */){Executive = ExecutiveType.Asynchronous},
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
      private void Isic_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "isic_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ISIC_DFIN_F", 03 /* Execute Paint */)
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
      /// Code 06
      /// </summary>
      /// <param name="job"></param>
      private void Orgn_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "orgn_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "ORGN_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ORGN_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORGN_DVLP_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "ORGN_DVLP_F", 03 /* Execute Paint */)
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
      private void Rbkn_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rbkn_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "RBKN_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RBKN_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RBKN_DVLP_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RBKN_DVLP_F", 03 /* Execute Paint */)
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
      private void Rbsr_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rbsr_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "RBSR_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RBSR_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RBSR_DVLP_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RBSR_DVLP_F", 03 /* Execute Paint */)
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
      private void Rbsa_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rbsa_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "RBSA_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RBSA_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RBSA_DVLP_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RBSA_DVLP_F", 03 /* Execute Paint */)
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
      private void Wgul_Dfin_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "wgul_dfin_f"},
                  new Job(SendType.SelfToUserInterface, "WGUL_DFIN_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "WGUL_DFIN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "WGUL_DFIN_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "WGUL_DFIN_F", 03 /* Execute Paint */)
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
      private void Strt_Robo_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "strt_robo_f"},
                  new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "STRT_ROBO_F", 03 /* Execute Paint */)
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
      private void Rbod_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rbod_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 03 /* Execute Paint */)
               });
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
      private void Rbmn_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rbmn_dvlp_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RBMN_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RBMN_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "RBMN_DVLP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 14
      /// </summary>
      /// <param name="job"></param>
      private void Stng_Rprt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "stng_rprt_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "STNG_RPRT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "STNG_RPRT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "STNG_RPRT_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 15
      /// </summary>
      /// <param name="job"></param>
      private void Rpt_Mngr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_mngr_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 02 /* Execute Set */),
                  //new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 07 /* Execute Load_Data */),
                  //new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 03 /* Execute Paint */)
                  new Job(SendType.SelfToUserInterface, "RPT_MNGR_F", 11 /* Execute Do_Print */){WhereIsInputData = WhereIsInputDataType.StepBack}                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 16
      /// </summary>
      /// <param name="job"></param>
      private void Rpt_Lrfm_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rpt_lrfm_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RPT_LRFM_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RPT_LRFM_F", 07 /* Execute Load_Data */){WhereIsInputData = WhereIsInputDataType.StepBack},
                  new Job(SendType.SelfToUserInterface, "RPT_LRFM_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 17
      /// </summary>
      /// <param name="job"></param>
      private void Odrm_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "odrm_dvlp_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ODRM_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ODRM_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ODRM_DVLP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 18
      /// </summary>
      /// <param name="job"></param>
      private void Orml_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "orml_dvlp_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ORML_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORML_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ORML_DVLP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 19
      /// </summary>
      /// <param name="job"></param>
      private void Rbsm_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "rbsm_dvlp_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "RBSM_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "RBSM_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "RBSM_DVLP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 20
      /// </summary>
      /// <param name="job"></param>
      private void Srbt_Info_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "srbt_info_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SRBT_INFO_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SRBT_INFO_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SRBT_INFO_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 21
      /// </summary>
      /// <param name="job"></param>
      private void Sale_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "sale_dvlp_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "SALE_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "SALE_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "SALE_DVLP_F", 03 /* Execute Paint */)
               });
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
      private void Alpk_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "alpk_dvlp_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ALPK_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ALPK_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ALPK_DVLP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 23
      /// </summary>
      /// <param name="job"></param>
      private void Ordr_Ship_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ordr_ship_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ORDR_SHIP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORDR_SHIP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ORDR_SHIP_F", 03 /* Execute Paint */)
               });
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
      private void Ordr_Rcpt_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "ordr_rcpt_f"},
                  //new Job(SendType.SelfToUserInterface, "RBOD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ORDR_RCPT_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ORDR_RCPT_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ORDR_RCPT_F", 03 /* Execute Paint */)
               });
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
      private void Bank_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "bank_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "BANK_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "BANK_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "BANK_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "BANK_DVLP_F", 03 /* Execute Paint */)
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
      private void Prod_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "prod_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "PROD_DVLP_F", 03 /* Execute Paint */)
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
      private void Wlet_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "wlet_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "WLET_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "WLET_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "WLET_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "WLET_DVLP_F", 03 /* Execute Paint */)
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
      private void Onro_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "onro_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "ONRO_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "ONRO_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "ONRO_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "ONRO_DVLP_F", 03 /* Execute Paint */)
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
      private void Inst_Conf_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "inst_conf_f"},
                  new Job(SendType.SelfToUserInterface, "INST_CONF_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "INST_CONF_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "INST_CONF_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "INST_CONF_F", 03 /* Execute Paint */)
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
      private void Mesg_Dvlp_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "mesg_dvlp_f"},
                  new Job(SendType.SelfToUserInterface, "MESG_DVLP_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "MESG_DVLP_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "MESG_DVLP_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "MESG_DVLP_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

      /// <summary>
      /// Code 31
      /// </summary>
      /// <param name="job"></param>
      private void Cash_Cntr_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "cash_cntr_f"},
                  new Job(SendType.SelfToUserInterface, "CASH_CNTR_F", 05 /* Execute CheckSecurity */),
                  new Job(SendType.SelfToUserInterface, "CASH_CNTR_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CASH_CNTR_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CASH_CNTR_F", 03 /* Execute Paint */)
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }
   }
}
