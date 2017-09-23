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

namespace System.Scsc.Ui.Exam
{
   public partial class EXM_SAVE_F : UserControl
   {
      public EXM_SAVE_F()
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

            iScsc.EXM_SAVE_P(
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
                             new XElement("Exam",
                                new XAttribute("type", TYPE_LookUpEdit.EditValue),
                                new XAttribute("time", r.Exams.SingleOrDefault(ex => ex.RECT_CODE == "001").TIME),
                                new XAttribute("cachnumb", r.Exams.SingleOrDefault(ex => ex.RECT_CODE == "001").CACH_NUMB),
                                new XAttribute("stephegh", r.Exams.SingleOrDefault(ex => ex.RECT_CODE == "001").STEP_HEGH),
                                new XAttribute("wegh", r.Exams.SingleOrDefault(ex => ex.RECT_CODE == "001").WEGH)
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

      private void request_RowsBindingSource_PositionChanged(object sender, EventArgs e)
      {
         try
         {
            TIME_TextBox.Enabled = false;
            CACH_NUMB_TextBox.Enabled = false;
            STEP_HEGH_TextBox.Enabled = false;
            WEGH_TextBox.Enabled = false;

            switch (TYPE_LookUpEdit.EditValue.ToString())
            {
               case "001":
               case "002":
               case "003":
                  TIME_TextBox.Enabled = true;
                  break;
               case "004":
                  CACH_NUMB_TextBox.Enabled = true;
                  break;
               case "005":
                  STEP_HEGH_TextBox.Enabled = true;
                  WEGH_TextBox.Enabled = true;
                  TIME_TextBox.Enabled = true;
                  break;
            }
         }
         catch { }
      }

      private void requestBindingSource_ListChanged(object sender, ListChangedEventArgs e)
      {
         request_RowsBindingSource_PositionChanged(null, null);
      }
   }
}
