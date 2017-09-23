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

namespace System.Scsc.Ui.CalculateCalorie
{
   public partial class CLC_SAVE_F : UserControl
   {
      public CLC_SAVE_F()
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

            iScsc.CLC_SAVE_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue),
                     new XElement("Request_Row",
                        request_RowsBindingSource
                        .List.OfType<Data.Request_Row>()
                        .Select(r =>
                           new XElement("Fighter",
                             new XAttribute("fileno", r.FIGH_FILE_NO),
                             new XElement("CalculateCalorie",
                                new XAttribute("hegh", r.Calculate_Calories.SingleOrDefault(c => c.RECT_CODE == "001").HEGH),
                                new XAttribute("wegh", r.Calculate_Calories.SingleOrDefault(c => c.RECT_CODE == "001").WEGH),
                                new XAttribute("trantime", r.Calculate_Calories.Single(c => c.RECT_CODE == "001").TRAN_TIME)
                             )
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
               iScsc = new Data.iScscDataContext(ConnectionString);
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

      private void tRAN_TIMETextBox_TextChanged(object sender, EventArgs e)
      {
         try
         {
            Lbl_TTDesc.Text = string.Format("{0:N0}, {1:N0}", (int.Parse(TRAN_TIME_TextBox.Text) / 60), (int.Parse(TRAN_TIME_TextBox.Text) % 60));
         }
         catch { }
      }
   }
}
