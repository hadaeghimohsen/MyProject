using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.JobRouting.Jobs
{
    public enum GenerateDataType 
    { 
        /// <summary>
        /// Constant Input Value Of Parent Job For Owners
        /// </summary>
        Static = 0, 
        /// <summary>
        /// Variable Input Value Of Parent Job For Owners
        /// </summary>
        Dynamic = 1 
    };
}
