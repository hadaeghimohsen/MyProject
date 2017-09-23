using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.JobRouting.Jobs
{
    public enum StatusType 
    { 
        InQueue,
        EnteringToGateway,
        Running, 
        Waiting, 
        Successful, 
        Failed, 
        WaitForPreconditions, 
        SignalForPreconditions, 
        WaitForPostconditions, 
        SignalForPostcondition 
    };
}
