using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.DataAccess.Guis
{
    internal partial class Build : Form
    {
        internal Build()
        {
            InitializeComponent();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            Job _GetAllInstalledDsn = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 08 /* Execute GetAllDataSourceName */)
                    {
                        AfterChangedOutput = new Action<object>(
                            (output) =>
                            {
                                lb_Dsnlist.Items.Clear();
                                (output as List<string>).ForEach(dsn => lb_Dsnlist.Items.Add(dsn));
                            })
                    }
                });
            _Odbc.Gateway(_GetAllInstalledDsn);
        }

        private void lb_Dsnlist_DoubleClick(object sender, EventArgs e)
        {
            Job _SetDsnNameOnOdbcCfgConnectionString = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "Build", 02 /* Execute Close */),
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 09 /* Execute SetDsnNameOnConnectionString */){Input = lb_Dsnlist.SelectedItem}
                });
            _Odbc.Gateway(_SetDsnNameOnOdbcCfgConnectionString);
        }

        private void btn_TestConnection_Click(object sender, EventArgs e)
        {
            if (txt_DsnName.Text.Trim() == "")
                return;
            if (txt_Server.Text.Trim() == "")
                return;
            if (txt_Database.Text.Trim() == "")
                return;

            Job _TestConnection = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.Self, 01 /* Execute TestConnection */){Input = string.Format("Dsn={0};")}
                });
        }

        private void btn_SaveDsn_Click(object sender, EventArgs e)
        {
            if (txt_DsnName.Text.Trim() == "")
                return;
            if (txt_Server.Text.Trim() == "")
                return;
            if (cb_DriverList.Text.Trim() == "")
                return;
            if (txt_Database.Text.Trim() == "")
                return;

            Job _SaveDsn = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 02){Input = new List<object>{txt_DsnName.Text, txt_Desc.Text,txt_Server.Text,cb_DriverList.Text,txt_Database.Text,cb_TrustedConnection.Checked}},
                    new Job(SendType.SelfToUserInterface, "Build", 02),
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 09){Input = txt_DsnName.Text}
                });
            _Odbc.Gateway(_SaveDsn);
        }
    }
}
