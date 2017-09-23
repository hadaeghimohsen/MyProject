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
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.FighterPublic
{
   public partial class PBL_REXP_F : UserControl
   {
      public PBL_REXP_F()
      {
         InitializeComponent();
      }

      private void Btn_ApprExpn_Click(object sender, EventArgs e)
      {
         try
         {
            if (SSTT_MSTT_CODE_TextBox.Text == "3" && SSTT_CODE_TextBox.Text == "1") return;

            Validate();
            requestBindingSource.EndEdit();

            iScsc.INS_REXP_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text)
                  )
               )
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var CrntRqid = requestBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            requestBindingSource.DataSource =
            iScsc.Requests
            .Where(
               rqst =>
                  rqst.RQTP_CODE == P_RqtpCode.Text &&
                  ((rqst.SSTT_MSTT_CODE == 2 &&
                  rqst.SSTT_CODE == 2) ||
                  (rqst.SSTT_MSTT_CODE == 3 &&
                  rqst.SSTT_CODE == 1)) &&
                  rqst.SEND_EXPN == "002" &&
                  rqst.SUB_SYS == 1
            );
            requestBindingSource.Position = CrntRqid;
         }
      }

      private void Tsb_SaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            requestBindingSource.EndEdit();

            iScsc.SubmitChanges();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var CrntRqid = requestBindingSource.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            requestBindingSource.DataSource =
            iScsc.Requests
            .Where(
               rqst =>
                  rqst.RQTP_CODE == P_RqtpCode.Text &&
                  ((rqst.SSTT_MSTT_CODE == 2 &&
                  rqst.SSTT_CODE == 2) ||
                  (rqst.SSTT_MSTT_CODE == 3 &&
                  rqst.SSTT_CODE == 1)) &&
                  rqst.SEND_EXPN == "002" &&
                  rqst.SUB_SYS == 1
            );
            requestBindingSource.Position = CrntRqid;
         }
      }

      private void Btn_Next_Click(object sender, EventArgs e)
      {
         XElement x =
            new XElement(
               "Process",
               new XElement("Request",
                  new XAttribute("rqid", RQID_TextBox.Text),
                  new XAttribute("rqtpcode", P_RqtpCode.Text),
                  new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue)
               ),
               new XElement("Form",
                  new XAttribute("mtodnumb", 49)
               )
            );
         x = iScsc.GOTO_NEXT_F(x);

         /* Check Condition For Go Next Form */
         if (x.Descendants("NextForm").Attributes("mtodnumb").First().Value == "-1") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "MSTR_PAGE_F", 08 /* Execute Goto_NextForm */, SendType.SelfToUserInterface) { Input = x }
         );
      }
   }
}
