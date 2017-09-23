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
   public partial class PBL_RQST_F : UserControl
   {
      public PBL_RQST_F()
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

            Func<Data.Request_Row, XElement> InsertFbPb = new
               Func<Data.Request_Row, XElement>(
               r =>
               {
                  if (r.RWNO > 0)
                     return new XElement("Fighter_Public",
                           new XElement("Frst_Name", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").FRST_NAME),
                           new XElement("Last_Name", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").LAST_NAME),
                           new XElement("Fath_Name", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").FATH_NAME),
                           new XElement("Sex_Type", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").SEX_TYPE),
                           new XElement("Natl_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").NATL_CODE),
                           new XElement("Brth_Date", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").BRTH_DATE.Value.Date),
                           new XElement("Cell_Phon", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").CELL_PHON),
                           new XElement("Tell_Phon", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").TELL_PHON),
                           new XElement("Post_Adrs", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").POST_ADRS),
                           new XElement("Emal_Adrs", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").EMAL_ADRS),
                           new XElement("Dise_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").DISE_CODE),
                           new XElement("Club_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").CLUB_CODE),
                           new XElement("Type", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").TYPE),
                           new XElement("Coch_Deg", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").COCH_DEG),
                           new XElement("Gudg_Deg", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").GUDG_DEG),
                           new XElement("Glob_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").GLOB_CODE),
                           new XElement("Insr_Numb", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").INSR_NUMB),
                           new XElement("Insr_Date", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").INSR_DATE.Value.Date)
                        );
                  return null;
               }
               );
            
            iScsc.PBL_RQST_F( 
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
                           new XAttribute("fileno", r.FIGH_FILE_NO),
                           InsertFbPb(r)                           
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

               if (requestBindingSource.List.Count != 0)
                  fightersBindingSource.DataSource = iScsc.Fighters.Where(f => f.Region == (requestBindingSource.Current as Data.Request).Region && f.CONF_STAT == "002");

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
                     rqst.SSTT_MSTT_CODE == 1 &&
                     rqst.SSTT_CODE == 1 &&
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
                     rqst.SSTT_MSTT_CODE == 1 &&
                     rqst.SSTT_CODE == 1 &&
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
