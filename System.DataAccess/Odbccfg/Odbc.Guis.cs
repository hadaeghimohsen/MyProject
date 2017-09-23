using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.DataAccess.Odbccfg
{
    partial class Odbc
    {
        internal Guis.Build _Build { get; set; }
        internal Guis.OdbcSettings _OdbcSettings { get; set; }
        internal Guis.ChooseCommandType _ChooseCommandType { get; set; }
        internal Guis.SetInputParameter _SetInputParameter { get; set; }
        internal Guis.ExecuteCommandName _ExecuteCommandName { get; set; }
        internal Guis.ExecuteDsnName _ExecureDsnName { get; set; }
        internal Guis.ExecuteUserName _ExecuteUserName { get; set; }
    }
}
