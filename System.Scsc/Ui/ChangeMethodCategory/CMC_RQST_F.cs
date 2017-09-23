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
   public partial class CMC_RQST_F : UserControl
   {
      public CMC_RQST_F()
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

            Func<Data.Request_Row, XElement> INS_FPTS_U = new
               Func<Data.Request_Row, XElement>(
               r =>
               {
                  if (r.RWNO > 0)
                     if (r.Tests.Any(t => t.RECT_CODE == "001"))
                        return new XElement("ChngMtodCtgy",
                              new XElement("Crtf_Date", r.Tests.SingleOrDefault(t => t.RECT_CODE == "001").CRTF_DATE.Value.Date),
                              new XElement("Crtf_Numb", r.Tests.SingleOrDefault(t => t.RECT_CODE == "001").CRTF_NUMB),
                              new XElement("Test_Date", r.Tests.SingleOrDefault(t => t.RECT_CODE == "001").TEST_DATE.Value.Date),
                              new XElement("Glob_Code", r.Tests.SingleOrDefault(t => t.RECT_CODE == "001").GLOB_CODE),
                              new XElement("Mtod_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").MTOD_CODE),
                              new XElement("Ctgy_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").CTGY_CODE)
                           );
                     else
                        return new XElement("ChngMtodCtgy",
                              new XElement("Mtod_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").MTOD_CODE),
                              new XElement("Ctgy_Code", r.Fighter_Publics.SingleOrDefault(p => p.RECT_CODE == "001").CTGY_CODE)
                           );
                  return null;
               }
               );
            
            iScsc.CMC_RQST_F( 
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
                           INS_FPTS_U(r)                           
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
            Tsb_SaveItem_Click(null, null);

            XElement x =
               new XElement(
                  "Process",
                  new XElement("Request",
                     new XAttribute("rqid", RQID_TextBox.Text),
                     new XAttribute("rqtpcode", P_RqtpCode.Text),
                     new XAttribute("rqttcode", RQTT_CODE_ComboBox.SelectedValue)
                  ),
                  new XElement("Form",
                     new XAttribute("mtodnumb", 51)
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

      private void MTOD_CODE_LOV_EditValueChanged(object sender, EventArgs e)      
      {
         if(mTOD_CODELookUpEdit.EditValue.ToString() != "")
            categoryBeltsBindingSource.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == (long?)mTOD_CODELookUpEdit.EditValue);
      }
   }
}
