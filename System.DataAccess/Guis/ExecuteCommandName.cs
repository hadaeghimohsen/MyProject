using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.DataAccess.Guis
{
    internal partial class ExecuteCommandName : Form
    {
        internal ExecuteCommandName()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (txt_CommandName.Text.Trim().Length == 0)
                return;
            Close();
        }
    }
}
