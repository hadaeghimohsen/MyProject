using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace MyProject.Commons.RedoLogs.Ui
{
   partial class RedoLog : ISendRequest
   {
      public IRouter _DefaultGateway
      {
         get;
         set;
      }

      public void SendRequest(System.JobRouting.Jobs.Job job)
      {
         throw new NotImplementedException();
      }      
   }
}
