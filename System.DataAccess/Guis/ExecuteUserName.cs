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
    public partial class ExecuteUserName : Form
    {
        public ExecuteUserName()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (txt_UserName.Text.Trim().Length == 0)
                return;
            Close();
        }
    }
}
