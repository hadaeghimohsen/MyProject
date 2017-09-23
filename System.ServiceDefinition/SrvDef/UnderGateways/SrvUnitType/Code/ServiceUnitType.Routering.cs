using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Routering;

namespace System.ServiceDefinition.SrvDef.UnderGateways.SrvUnitType.Code
{
   partial class ServiceUnitType : IMP
   {
      protected override void ExternalAssistance(JobRouting.Jobs.Job jobs)
      {
         throw new NotImplementedException();
      }

      protected override void InternalAssistance(JobRouting.Jobs.Job job)
      {
         throw new NotImplementedException();
      }

      protected override void RequestToUserInterface(JobRouting.Jobs.Job job)
      {
         throw new NotImplementedException();
      }
   }
}
