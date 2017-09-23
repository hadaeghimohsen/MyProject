using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.JobRouting.Jobs
{
    public enum ErrorType 
    { 
        ServerNotValid, 
        NoGateway, 
        BadInputData, 
        RejectRequest, 
        ServerIsNotReady 
    };
}
