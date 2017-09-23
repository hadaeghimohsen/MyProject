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
    internal partial class SetInputParameter : Form
    {
        internal SetInputParameter()
        {
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if (cb_SetInputParam.Checked)
            {
                if (cb_InputParamType.Text.Trim().Length == 0)
                {
                    cb_InputParamType.SelectedIndex = 0;
                    return;
                }
                if (txt_InputParamValue.Text.Trim().Length == 0)
                {
                    txt_InputParamValue.Text = "Please enter your input parameter";
                    txt_InputParamValue.SelectAll();
                    return;
                }
            }
            Close();
        }
    }
}
