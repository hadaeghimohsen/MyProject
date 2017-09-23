using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.JobRouting.Jobs
{
    public enum FaultToleranceType 
    { 
        /// <summary>
        /// Ignore The Error And Continue To Run The Jobs List
        /// </summary>
        Ignore = 0, 
        /// <summary>
        /// Prevent The Continuation Of The Jobs List
        /// </summary>
        Abort = 1
    };
}
