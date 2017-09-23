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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;

namespace System.Reporting.WorkFlow.Ui
{
   public partial class RPT_SRPT_F : UserControl
   {
      public RPT_SRPT_F()
      {
         InitializeComponent();
      }

      private XDocument xAccessReport { get; set; }
      private XDocument xRunReport { get; set; }
      private bool OperationComplete = true;
      private bool IsFirst = true;
      private string Next_Form { get; set; }

      private void cbe_role_EditValueChanged(object sender, EventArgs e)
      {
         /* اگز عمل بارگذاری بر روی فرم به پایان نرسیده باشد */
         if (OperationComplete == false) return;

         cbe_role.Properties.GetItems().OfType<CheckedListBoxItem>().ToList()
            .ForEach(role =>
            {
               Tile_Ctrl.Groups.OfType<TileGroup>().Where(g => g.Tag.ToString() == role.Value.ToString()).First().Visible = role.CheckState == CheckState.Checked ? true : false;
            });
      }

      partial void bp_flow_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);

      
      partial void Tile_Ctrl_MouseClick(object sender, MouseEventArgs e);
      
   }
}
