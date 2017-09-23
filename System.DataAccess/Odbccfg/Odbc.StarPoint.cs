using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Odbccfg
{
    public partial class Odbc
    {
        public Odbc()
        {
            _Command = new Commands.Command { _Odbc = this };
            _Transaction = new Transactions.Transaction { _Odbc = this };
            _OdbcSettings = new Guis.OdbcSettings { _Odbc = this };
            _Build = new Guis.Build { _Odbc = this };
            _ChooseCommandType = new Guis.ChooseCommandType { _Odbc = this };
            _SetInputParameter = new Guis.SetInputParameter { _Odbc = this };
            _ExecuteCommandName = new Guis.ExecuteCommandName { _Odbc = this };
            _ExecureDsnName = new Guis.ExecuteDsnName { _Odbc = this };
            _ExecuteUserName = new Guis.ExecuteUserName { _Odbc = this };            
        }
    }
}
