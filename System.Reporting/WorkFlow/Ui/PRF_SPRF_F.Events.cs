using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Reporting.WorkFlow.Ui
{
   partial class PRF_SPRF_F
   {
      partial void bp_flow_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
      {
         switch (e.Button.Properties.Caption)
         {
            case "":
            case "1":
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, "PRF_SPRF_F", 04 /* Execute UnPaint */)
                     }));
               break;
         }
      }

      partial void Tile_Ctrl_MouseClick(object sender, MouseEventArgs e)
      {
         if (e.Button != Windows.Forms.MouseButtons.Right)
            return;

         Tile_Ctrl.ItemCheckMode = TileItemCheckMode.Multiple;
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 01 /* Execute GetUi*/){Input = "PRF_MBAR_M"},
                  new Job(SendType.SelfToUserInterface, "PRF_MBAR_M", 03 /* Execute Paint */),
                  new Job(SendType.SelfToUserInterface, "PRF_MBAR_M", 05 /* Execute Open */){Executive = ExecutiveType.Asynchronous}
               }));
      }

      partial void cbe_role_EditValueChanged(object sender, EventArgs e)
      {
         /* اگز عمل بارگذاری بر روی فرم به پایان نرسیده باشد */
         if (OperationComplete == false) return;

         cbe_role.Properties.GetItems().OfType<CheckedListBoxItem>().ToList()
            .ForEach(role =>
            {
               Tile_Ctrl.Groups.OfType<TileGroup>().Where(g => g.Tag.ToString() == role.Value.ToString()).First().Visible = role.CheckState == CheckState.Checked ? true : false;
            });
      }
   }
}
