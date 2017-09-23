using System;
using System.Collections.Generic;
using System.JobRouting.Routering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Odbccfg
{
    partial class Odbc
    {
        internal Commands.Command _Command { get; set; }
        internal Transactions.Transaction _Transaction { get; set; }
        
        public IRouter _DefaultGateway { get; set; }
    }
}
