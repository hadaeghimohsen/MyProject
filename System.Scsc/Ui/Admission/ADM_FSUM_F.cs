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

namespace System.Scsc.Ui.Admission
{
   public partial class ADM_FSUM_F : UserControl
   {
      public ADM_FSUM_F()
      {
         InitializeComponent();
      }
      private bool requeryData = default(bool);

      private void Tsb_SaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            requestBindingSource.EndEdit();

            iScsc.TEST_RQT_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XElement("Request_Row",
                        new XAttribute("rwno", 1),
                        new XAttribute("fighfileno", FILE_NO_TextBox.Text),
                        new XElement("Test",
                           new XAttribute("ctgymtodcode", CTGY_MTOD_CODE_ComboBox.SelectedValue),
                           new XAttribute("ctgycode", CTGY_CODE_ComboBox.SelectedValue),
                           new XAttribute("crtfdate", CRTF_DATE_PersianDateEdit.DateTime.ToString("yyyy-MM-dd")),
                           new XAttribute("crtfnumb", CRTF_NUMB_TextBox.Text),
                           new XAttribute("testdate", TEST_DATE_PersianDateEdit.DateTime.ToString("yyyy-MM-dd")),
                           new XAttribute("rslt", RSLT_ComboBox.SelectedValue),
                           new XAttribute("globcode", GLOB_CODE_TextBox.Text)
                        )
                     )
                  )
               )
            );

            iScsc.NEXT_LEVL_F(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("msttcode", 4),
                     new XAttribute("ssttcode", 1)
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
               var CurrentPosition = requestBindingSource.Position;
               iScsc = new Data.iScscDataContext(ConnectionString);
               requestBindingSource.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     rqst.RQTP_CODE == P_RqtpCode.Text &&
                     (
                        (rqst.SSTT_MSTT_CODE == 4 || rqst.SSTT_MSTT_CODE == 1) &&
                        rqst.SSTT_CODE == 1
                     ) &&
                     rqst.SUB_SYS == 1 &&
                     rqst.Fighters.SingleOrDefault(f => f.RQST_RQID == rqst.RQID).Tests.SingleOrDefault(t => t.RQRO_RQST_RQID == rqst.RQID) != null
               );
               requestBindingSource.Position = CurrentPosition;
               requeryData = false;
            }
         }
      }

      private void Tsb_Sreach_Click(object sender, EventArgs e)
      {
         RQID_TextBox.ReadOnly = true;

         requestBindingSource.DataSource =
            iScsc.Requests
            .Where(
               rqst =>
                  rqst.RQTP_CODE == P_RqtpCode.Text &&
                  (
                     (rqst.SSTT_MSTT_CODE == 4 || rqst.SSTT_MSTT_CODE == 1) &&
                     rqst.SSTT_CODE == 1
                  ) &&
                  rqst.SUB_SYS == 1 &&
                  rqst.Fighters.SingleOrDefault(f => f.RQST_RQID == rqst.RQID).Tests.SingleOrDefault(t => t.RQRO_RQST_RQID == rqst.RQID) != null &&
                  (RQID_TextBox.Text == "" || rqst.RQID.ToString() == RQID_TextBox.Text)
            );
      }

      private void Tsb_Query_Click(object sender, EventArgs e)
      {
         RQID_TextBox.ReadOnly = false;
         requestBindingSource.Clear();
      }

      private void Btn_CnclRqst_Click(object sender, EventArgs e)
      {
         if (MessageBox.Show(this, "آیا با انصراف دادن درخواست موافق هستید؟", "هشدار", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
         iScsc.CNCL_RQST_F(
            new XElement("Process",
               new XElement("Request",
                  new XAttribute("rqid", RQID_TextBox.Text)
               )
            )
         );

         requestBindingSource.DataSource =
            iScsc.Requests
            .Where(
               rqst =>
                  rqst.RQTP_CODE == P_RqtpCode.Text &&
                  (
                     (rqst.SSTT_MSTT_CODE == 4 || rqst.SSTT_MSTT_CODE == 1) &&
                     rqst.SSTT_CODE == 1
                  ) &&
                  rqst.SUB_SYS == 1 &&
                  rqst.Fighters.SingleOrDefault(f => f.RQST_RQID == rqst.RQID).Tests.SingleOrDefault(t => t.RQRO_RQST_RQID == rqst.RQID) != null                  
            );
      }

      private void Btn_Next_Click(object sender, EventArgs e)
      {
         XElement x =
            new XElement(
               "Process",
               new XElement("Request",
                  new XAttribute("rqid", RQID_TextBox.Text),
                  new XAttribute("rqtpcode", P_RqtpCode.Text),
                  new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue),
                  new XAttribute("msttcode", SSTT_MSTT_CODE_ComboBox.SelectedValue),
                  new XAttribute("ssttcode", SSTT_CODE_ComboBox.SelectedValue)
               ),
               new XElement("Form",
                  new XAttribute("mtodnumb", 16)
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
