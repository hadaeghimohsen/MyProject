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
   public partial class ADM_RQST_F : UserControl
   {
      public ADM_RQST_F()
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

            iScsc.ADM_RQST_F( 
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("rqtpcode", P_RqtpCode.Text),
                     new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     new XElement("Fighter",
                        new XAttribute("fileno", FILE_NO_TextBox.Text),
                        new XElement("Fighter_Public",
                           new XElement("Frst_Name", FRST_NAME_TextBox.Text),
                           new XElement("Last_Name", LAST_NAME_TextBox.Text),
                           new XElement("Fath_Name", FATH_NAME_TextBox.Text),
                           new XElement("Sex_Type",  SEX_TYPE_ComboBox.SelectedValue),
                           new XElement("Natl_Code", NATL_CODE_TextBox.Text),
                           new XElement("Brth_Date", BRTH_DATE_PersianDateEdit.EditValue),
                           new XElement("Cell_Phon", CELL_PHON_TextBox.Text),
                           new XElement("Tell_Phon", TELL_PHON_TextBox.Text),
                           new XElement("Post_Adrs", POST_ADRS_TextBox.Text),
                           new XElement("Emal_Adrs", EMAL_ADRS_TextBox.Text),
                           new XElement("Dise_Code", DISE_CODE_ComboBox.SelectedValue),
                           new XElement("Mtod_Code", MTOD_CODE_ComboBox.SelectedValue),
                           new XElement("Ctgy_Code", CTGY_CODE_ComboBox.SelectedValue),
                           //new XElement("Club_Code", CLUB_CODE_ComboBox.SelectedValue),
                           new XElement("Type", TYPE_ComboBox.SelectedValue),
                           new XElement("Coch_Deg", COCH_DEG_ComboBox.SelectedValue),
                           new XElement("Gudg_Deg", GUDG_DEG_ComboBox.SelectedValue),
                           new XElement("Glob_Code", GLOB_CODE_TextBox.Text),
                           new XElement("Insr_Numb", INSR_NUMB_TextBox.Text),
                           new XElement("Insr_Date", INSR_DATE_PersianDateEdit.EditValue),
                           new XElement("Educ_Deg", EDUC_DEG_LookUpEdit.EditValue ?? ""),
                           new XElement("Cbmt_Code", CBMT_CODE_GridLookUpEdit.EditValue ?? 0),
                           new XElement("Coch_Crtf_Date", COCH_CRTF_DATE_PersianDateEdit.EditValue)
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
            if(requeryData)
            {
               var CurrentPosition = requestBindingSource.Position;
               iScsc = new Data.iScscDataContext(ConnectionString);
               requestBindingSource.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     rqst.RQTP_CODE == P_RqtpCode.Text &&
                     rqst.SSTT_MSTT_CODE == 1 &&
                     rqst.SSTT_CODE == 1 &&
                     rqst.SUB_SYS == 1
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
                  rqst.SSTT_MSTT_CODE == 1 &&
                  rqst.SSTT_CODE == 1 &&
                  rqst.SUB_SYS == 1 &&
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
                  rqst.SSTT_MSTT_CODE == 1 &&
                  rqst.SSTT_CODE == 1 &&
                  rqst.SUB_SYS == 1
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
                  new XAttribute("mtodnumb", 15)
               )
            );
         x = iScsc.GOTO_NEXT_F(x);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "MSTR_PAGE_F", 08 /* Execute Goto_NextForm */, SendType.SelfToUserInterface) { Input = x }
         );
      }

      private void Bt_Dcmt_Click(object sender, EventArgs e)
      {
         if (requestBindingSource.Current == null) return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = (requestBindingSource.Current as Data.Request).Request_Rows.SingleOrDefault() }
         );
      }
   }
}
