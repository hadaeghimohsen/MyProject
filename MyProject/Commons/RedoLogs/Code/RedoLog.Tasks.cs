using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;

namespace MyProject.Commons.RedoLogs.Code
{
   partial class RedoLog
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void DoWork4RedLog(Job job)
      {
         var value = job.Input as List<string>;
         Job action = null;

         if (value[0].ToLower() == "savepoint")
         {
            action = new Job(SendType.External, "Commons",
               new List<Job>
               {
                  new Job(SendType.Self, 04 /* Execute DoWork4Odbc */)
                  {
                     Input = new List<object>
                     {
                        false,
                        "procedure",
                        true,
                        false,
                        "xml",
                        value[1],
                        "{ Call Commons.SaveRedoLog(?) }",
                        "iProject",
                        "scott"
                     }
                  }
               });
         }

         if (action != null)
            Manager(action);

         job.Status = StatusType.Successful;
      }
   }
}
