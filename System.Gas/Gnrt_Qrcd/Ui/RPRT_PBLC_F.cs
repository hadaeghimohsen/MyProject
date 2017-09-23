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

namespace System.Gas.Gnrt_Qrcd.Ui
{
   public partial class RPRT_PBLC_F : UserControl
   {
      public RPRT_PBLC_F()
      {
         InitializeComponent();

         XPublic = 
            new XDocument(
               new XElement("Form",
               new XAttribute("enname", "RPRT_PBLC_F"),
               new XElement("Application",
                  new XAttribute("enname", "System.Gas"),
                  new XAttribute("subsys", "3")),
               new XElement("Guid", "{6B3FD96D-9210-4BFE-911A-21D790C8E2F2}"))
            );
      }

      public XDocument XPublic { get; set; }

      public XDocument XPrivate { get; set; }

      partial void be_open_fldr_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      partial void cbe_databaseservers_EditValueChanged(object sender, EventArgs e);

      partial void cbe_databaseservers_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      partial void cbe_seri_no_buttonclick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e);

      partial void rb_qr_desc_type_CheckedChanged(object sender, EventArgs e);

      partial void tb_qr_desc_TextChanged(object sender, EventArgs e);

      partial void wbp_book_mark_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);
      
      
      
   }
}
