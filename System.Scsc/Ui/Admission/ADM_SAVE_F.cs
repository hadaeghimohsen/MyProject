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

namespace System.Scsc.Ui.Admission
{
   public partial class ADM_SAVE_F : UserControl
   {
      public ADM_SAVE_F()
      {
         InitializeComponent();
      }

      private bool requeryData = default(bool);

      private void Tsb_SaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (RQID_TextBox.Text == "") return;

            Validate();
            requestBindingSource.EndEdit();

            iScsc.ADM_SAVE_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue)
                  )
               )
            );
            
             /*
            iScsc.END_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text)
                  )
               )
            );
              * */
            requeryData = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if (requeryData)
            {
               requestBindingSource.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     rqst.RQTP_CODE == P_RqtpCode.Text &&
                     ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 4 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 3 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "002")) &&
                     rqst.SUB_SYS == 1
               );
               requeryData = false;
            }
         }
      }
   }
}
