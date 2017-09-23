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
    internal partial class ChooseCommandType : Form
    {
        internal ChooseCommandType()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (cb_CommandType.Text.Trim().Length == 0)
            {
                cb_CommandType.SelectedIndex = 0;
                return;
            }
            Close();
        }
    }
}
