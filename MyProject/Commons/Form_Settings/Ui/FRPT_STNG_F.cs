using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MyProject.Commons.Form_Settings.Ui
{
   public partial class FRPT_STNG_F : UserControl
   {
      public FRPT_STNG_F()
      {
         InitializeComponent();         
      }
      XDocument xdoc;

      partial void gv_Lov_Role_Prof_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e);

      partial void gv_Lov_Role_Serv_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e);

      partial void b_commit_Click(object sender, EventArgs e);

      partial void b_addnewreport_Click(object sender, EventArgs e);

      partial void b_removereport_Click(object sender, EventArgs e);

      partial void b_exit_Click(object sender, EventArgs e);
   }
}
