using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.JobRouting.Jobs
{    
    public enum SendType 
    { 
        /// <summary>
        /// Call Internal Method Current Class
        /// </summary>
        Self = 0, 
        /// <summary>
        /// Send Message To Internal User Interface ( UI ) Current Class
        /// </summary>
        SelfToUserInterface = 1, 
        /// <summary>
        /// Returned The Latest Output From The Next Jobs Sequnce
        /// </summary>
        Colapse = 2, 
        /// <summary>
        /// Migration To Another Class Gateway
        /// </summary>
        External = 3
    };
}
