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

namespace System.Scsc.Ui.CalculateCalorie
{
   public partial class CLC_RQST_F : UserControl
   {
      public CLC_RQST_F()
      {
         InitializeComponent();
      }
      private bool requeryData = default(bool);

      private void REGN_CODE_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (REGN_CODE_ComboBox.SelectedValue == null) return;

         fighterBindingSource.DataSource = iScsc.Fighters.Where(f => f.REGN_CODE == REGN_CODE_ComboBox.SelectedValue.ToString() && f.CONF_STAT == "002");
      }

      private void Tsb_SaveItem_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            requestBindingSource.EndEdit();

            iScsc.CLC_RQST_P(
               new XElement("Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("rqtpcode", P_RqtpCode.Text),
                     new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue),
                     new XAttribute("prvncode", REGN_PRVN_CODE_ComboBox.SelectedValue),
                     new XAttribute("regncode", REGN_CODE_ComboBox.SelectedValue),
                     new XElement("Request_Row",
                        request_RowsBindingSource
                        .List.OfType<Data.Request_Row>()
                        .Where(f => f.FIGH_FILE_NO > 0 /*&& f.FIGH_FILE_NO == (request_RowsBindingSource.Current as Data.Request_Row).FIGH_FILE_NO*/)
                        .ToList()
                        .Select(f => 
                           new XElement("Fighter", 
                              new XAttribute("fileno", f.FIGH_FILE_NO)
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
            if(requeryData)
            {
               var CrntRqid = requestBindingSource.Position;
               var CrntRqro = request_RowsBindingSource.Position;
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
               requestBindingSource.Position = CrntRqid;
               request_RowsBindingSource.Position = CrntRqro;

               if (requestBindingSource.List.Count > 0) 
                  fighterBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");
               requeryData = false;
            }
         }
      }

      private void Tsb_SaveItemRqro_Click(object sender, EventArgs e)
      {
         try
         {
            Validate();
            request_RowsBindingSource.EndEdit();

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
               var CrntRqro = request_RowsBindingSource.Position;

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
               requestBindingSource.Position = CrntRqid;
               request_RowsBindingSource.Position = CrntRqro;

               if (requestBindingSource.List.Count > 0)
                  fighterBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");
               requeryData = false;
            }
         }
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

         var CrntRqid = requestBindingSource.Position;
         var CrntRqro = request_RowsBindingSource.Position;
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
         requestBindingSource.Position = CrntRqid;
         request_RowsBindingSource.Position = CrntRqro;

         if (requestBindingSource.List.Count > 0)
            fighterBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");
         requeryData = false;
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
                  new XAttribute("mtodnumb", 33)
               )
            );
         x = iScsc.GOTO_NEXT_F(x);

         /* Check Condition For Go Next Form */
         if (x.Descendants("NextForm").Attributes("mtodnumb").First().Value == "-1") return;

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "MSTR_PAGE_F", 08 /* Execute Goto_NextForm */, SendType.SelfToUserInterface) { Input = x }
         );
      }

      private void TSM_DelItem_Click(object sender, EventArgs e)
      {
         try
         {
            var CrntRqid = request_RowsBindingSource.Current as Data.Request_Row;
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
               var CrntRqro = request_RowsBindingSource.Position;
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
               requestBindingSource.Position = CrntRqid;
               request_RowsBindingSource.Position = CrntRqro;

               if (requestBindingSource.List.Count > 0)
                  fighterBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");
               requeryData = false;
            }
         }
      }
   }
}
