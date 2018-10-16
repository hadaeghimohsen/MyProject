using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.JobRouting.Jobs;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.DataAccess.Guis
{
    internal partial class OdbcSettings : Form
    {
        internal OdbcSettings()
        {
            InitializeComponent();
        }

        private void rb_GlobalRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            switch (rb.Tag.ToString())
            {
                case "UseDsn":
                    cmb_dsnlist.Enabled = btn_Refresh.Enabled = true;
                    txt_dsnconnectionstring.Enabled = btn_Build.Enabled = false;
                    btn_Refresh_Click(null, null);
                    break;
                case "UseConnectionString":
                    cmb_dsnlist.Enabled = btn_Refresh.Enabled = false;
                    txt_dsnconnectionstring.Enabled = btn_Build.Enabled = true;
                    break;
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            Job _GetAllInstalledDsn = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "ODBCConfiguration", 08 /* Execute GetAllDataSourceName */)
                    {
                        AfterChangedOutput = new Action<object>(
                            (output) =>
                            {
                                cmb_dsnlist.Items.Clear();
                                (output as List<string>).ForEach(dsn => cmb_dsnlist.Items.Add(dsn));
                            })
                    }
                });
            _Odbc.Gateway(_GetAllInstalledDsn);
        }

        private void btn_Build_Click(object sender, EventArgs e)
        {
            Job _ShowDialog = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.SelfToUserInterface, "ODBCConfiguration", 05 /* Execute GetInstalledDrivers */,
                        new Job(SendType.SelfToUserInterface, "Build", 04 /* Execute GetInstalledDrivers */){WhereIsInputData = WhereIsInputDataType.StepBack}),
                    new Job(SendType.SelfToUserInterface, "Build", 03 /* Execute Refresh */),
                    new Job(SendType.SelfToUserInterface, "Build", 01 /* Execute ShowDialog */)
                });
            _Odbc.Gateway(_ShowDialog);
        }

        private void txt_username_Leave(object sender, EventArgs e)
        {
            if (txt_username.Text.Trim() == "")
                return;
            txt_dsnconnectionstring.Text = string.Format("{0};uid={1}", txt_dsnconnectionstring.Text.Split(';')[0], txt_username.Text);
        }

        private void btn_TestConnection_Click(object sender, EventArgs e)
        {
            if (txt_dsnconnectionstring.Text.Trim() == "")
                return;
            if (txt_username.Text.Trim() == "")
                return;

            Job _TestConnection = new Job(SendType.External, "Odbc",
                new List<Job>
                {
                    new Job(SendType.Self, 01 /* Execute TestConnection */)
                    {
                        Input = string.Format("{0};pwd={1};",txt_dsnconnectionstring.Text, txt_password.Text), 
                        Executive = ExecutiveType.Asynchronous,
                        AfterChangedStatus = new Action<StatusType>(
                            (status) => 
                            {
                                switch (status)
	                            {
                                    case StatusType.Successful:
                                        MessageBox.Show("Connection Test Successfully!");
                                        break;
                                    case StatusType.Failed:
                                        MessageBox.Show("Connection Test Unsuccessfully!");
                                        break;
	                            }
                            })
                    }
                });
            _Odbc.Gateway(_TestConnection);
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        private bool Is64Bit()
        {
           if (IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor()))
           {
              return true;
           }
           else
           {
              return false;
           }
        }

        private bool Is32BitProcessOn64BitProcessor()
        {
           bool retVal;

           IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);

           return retVal;
        }

        //[DllImport("ODBCCP32.dll")]
        //public static extern bool SQLConfigDataSource(IntPtr parent, int request, string driver, string attributes);

        [DllImport("ODBCCP32.dll")]
        public static extern int SQLGetPrivateProfileString(string lpszSection, string lpszEntry, string lpszDefault, string @RetBuffer, int cbRetBuffer, string lpszFilename);
    }
}
