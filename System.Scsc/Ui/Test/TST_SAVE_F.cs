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

namespace System.Scsc.Ui.Test
{
   public partial class TST_SAVE_F : UserControl
   {
      public TST_SAVE_F()
      {
         InitializeComponent();
      }

      private void REGN_CODE_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (REGN_CODE_ComboBox.SelectedValue == null) return;

         fighterBindingSource.DataSource = iScsc.Fighters.Where(f => f.REGN_CODE == REGN_CODE_ComboBox.SelectedValue.ToString() && f.CONF_STAT == "002");
      }

      private bool requeryData = default(bool);

      private void Tsb_SaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            if (RQID_TextBox.Text == "") return;

            Validate();
            requestBindingSource.EndEdit();
            testsBindingSource.EndEdit();

            iScsc.TST_SAVE_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue),
                     new XElement("Request_Row",
                        new XAttribute("rwno", (request_RowsBindingSource.Current as Data.Request_Row).RWNO),
                        new XElement("Fighter",
                           new XAttribute("fileno", (request_RowsBindingSource.Current as Data.Request_Row).FIGH_FILE_NO),
                           new XElement("Test",
                              new XAttribute("crtfdate", cRTF_DATEPersianDateEdit.DateTime.Date.ToString("yyyy-MM-dd")),
                              new XAttribute("crtfnumb", CRTF_NUMB_TextBox.Text),
                              new XAttribute("globcode", GLOB_CODE_TextBox.Text),
                              new XAttribute("rslt", rSLTComboBox.EditValue)
                           )
                        )
                     )
                  )
               )
            );

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
                     ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 3 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "002")) &&
                     rqst.SUB_SYS == 1
               );
               requeryData = false;
            }
         }
      }
   }
}
