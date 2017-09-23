using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Transactions
{
    partial class Transaction
    {
        internal Odbccfg.Odbc _Odbc { get; set; }
        internal O2dbccfg.O2dbc _O2dbc { get; set; }
    }
}
