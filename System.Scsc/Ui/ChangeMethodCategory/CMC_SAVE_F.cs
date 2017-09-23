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

namespace System.Scsc.Ui.ChangeMethodCategory
{
   public partial class CMC_SAVE_F : UserControl
   {
      public CMC_SAVE_F()
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

            iScsc.CMC_SAVE_F( 
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("rqtpcode", P_RqtpCode.Text),
                     new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     requestRowsBindingSource.List.OfType<Data.Request_Row>().ToList()
                     .Select(r => 
                        new XElement("Request_Row",
                           new XAttribute("fileno", r.FIGH_FILE_NO)
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
                     ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 3 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "002")) &&
                     rqst.SUB_SYS == 1
               );
               requestBindingSource.Position = CurrentPosition;

               if (requestBindingSource.List.Count != 0)
                  fightersBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");

               requeryData = false;
            }
         }
      }

      private void Tsb_Sreach_Click(object sender, EventArgs e)
      {
         
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
                  ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 3 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "002")) &&
                  rqst.SUB_SYS == 1
            );
      }

      private void Btn_Next_Click(object sender, EventArgs e)
      {
         try
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
                     new XAttribute("mtodnumb", 47)
                  )
               );
            x = iScsc.GOTO_NEXT_F(x);

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "MSTR_PAGE_F", 08 /* Execute Goto_NextForm */, SendType.SelfToUserInterface) { Input = x }
            );
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      private void TSM_DelItem_Click(object sender, EventArgs e)
      {
         try
         {
            var CrntRqid = requestRowsBindingSource.Current as Data.Request_Row;
            iScsc.DEL_RQRO_P(CrntRqid.RQST_RQID, CrntRqid.RWNO);
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
               var CrntRqid = requestBindingSource.Position;
               var CrntRqro = requestRowsBindingSource.Position;
               iScsc = new Data.iScscDataContext(ConnectionString);
               requestBindingSource.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     rqst.RQTP_CODE == P_RqtpCode.Text &&
                     ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 3 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "002")) &&
                     rqst.SUB_SYS == 1
               );
               requestBindingSource.Position = CrntRqid;
               requestRowsBindingSource.Position = CrntRqro;

               if (requestBindingSource.List.Count > 0)
                  fightersBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");
               requeryData = false;
            }
         }
      }

      private void Tsb_SaveItemRqro_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            requestRowsBindingSource.EndEdit();

            iScsc.SubmitChanges();
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
               var CrntRqid = requestBindingSource.Position;
               var CrntRqro = requestRowsBindingSource.Position;

               iScsc = new Data.iScscDataContext(ConnectionString);
               requestBindingSource.DataSource =
               iScsc.Requests
               .Where(
                  rqst =>
                     rqst.RQTP_CODE == P_RqtpCode.Text &&
                     ((rqst.SSTT_MSTT_CODE == 1 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "001") || (rqst.SSTT_MSTT_CODE == 3 && rqst.SSTT_CODE == 1 && rqst.SEND_EXPN == "002")) &&
                     rqst.SUB_SYS == 1
               );
               requestBindingSource.Position = CrntRqid;
               requestRowsBindingSource.Position = CrntRqro;

               if (requestBindingSource.List.Count > 0)
                  fightersBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");
               requeryData = false;
            }
         }
      }
   }
}
