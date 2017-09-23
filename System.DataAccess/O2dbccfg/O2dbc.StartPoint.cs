using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.O2dbccfg
{
    public partial class O2dbc
    {
        public O2dbc()
        {
            _Command = new Commands.Command() { _O2dbc = this };
            _Transaction = new Transactions.Transaction() { _O2dbc = this };
        }
    }
}
