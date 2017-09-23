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
    public partial class ExecuteDsnName : Form
    {
        public ExecuteDsnName()
        {
            InitializeComponent();
        }

        private void ExecuteDnsName_Shown(object sender, EventArgs e)
        {
            Job _GetAllDataSourceName = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "OdbcSettings", 08 /* Execute GetAllDataSourceName */)
                    {
                        AfterChangedOutput = new Action<object>(
                            (output) =>
                            {
                                cmb_DnsNames.Items.Clear();
                                (output as List<string>).ForEach(dsn => cmb_DnsNames.Items.Add(dsn));
                            })
                    }
                });
            _Odbc.Gateway(_GetAllDataSourceName);
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (cmb_DnsNames.Text.Trim().Length == 0)
                return;
            Close();
        }
    }
}
