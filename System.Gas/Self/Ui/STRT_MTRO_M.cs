using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace System.Gas.Self.Ui
{
   public partial class STRT_MTRO_M : UserControl
   {
      public STRT_MTRO_M()
      {
         InitializeComponent();
      }

      partial void Start_Menu_MouseClick(object sender, MouseEventArgs e);

      partial void RPRT_PBLC_M_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e);

      partial void RPRT_PRVT_M_ItemClick(object sender, DevExpress.XtraEditors.TileItemEventArgs e);
      
   }
}
