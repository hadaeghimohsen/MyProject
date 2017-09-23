using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.JobRouting.Jobs;

namespace System.JobRouting.Routering
{
    public interface ISendRequest : IDefaultGateway
    {       
       void SendRequest(Job job);
    }
}
