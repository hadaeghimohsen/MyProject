using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Setup.Code
{
   partial class Setup
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "frst_page_f")
         {
            if (_Frst_Page_F == null)
               _Frst_Page_F = new Ui.LTR.MasterPage.FRST_PAGE_F { _DefaultGateway = this };
         }
         else if (value == "chk_licn_f")
         {
            if (_Chk_Licn_F == null)
               _Chk_Licn_F = new Ui.LTR.License.CHK_LICN_F { _DefaultGateway = this };
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
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "FRST_PAGE_F", 03 /* Execute Paint */),                  
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
      private void Chk_Licn_F(Job job)
      {
         if (job.Status == StatusType.Running)
         {
            job.Status = StatusType.WaitForPreconditions;
            job.OwnerDefineWorkWith.AddRange(
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi */){Input = "chk_licn_f"},
                  new Job(SendType.SelfToUserInterface, "CHK_LICN_F", 02 /* Execute Set */),
                  new Job(SendType.SelfToUserInterface, "CHK_LICN_F", 07 /* Execute Load_Data */),
                  new Job(SendType.SelfToUserInterface, "CHK_LICN_F", 03 /* Execute Paint */),                  
               });
         }
         else if (job.Status == StatusType.SignalForPreconditions)
         {
            job.Status = StatusType.Successful;
         }
      }

   }
}
