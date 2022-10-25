using DevExpress.XtraGrid.Views.Base;
using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Scsc.Ui.Regulation
{
   partial class REGL_DCMT_F
   {
      partial void SubmitRqrq_Click(object sender, EventArgs e)
      {
         try
         {
            this.Focus();
            Validate();

            ExpnBs.EndEdit();
            RqdcBs2.EndEdit();
            RqrqBs.EndEdit();
            PexpBs1.EndEdit();
            Bcds_Gv.PostEditor();
            ExtsGv.PostEditor();

            iScsc.SubmitChanges();
            /*iScsc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, iScsc.Request_Requesters);            
            request_RequestersBindingSource.DataSource = iScsc.Request_Requesters.Where(rg => rg.Regulation == (Data.Regulation)regulationBindingSource.Current);*/
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            var CrntRqrq = RqrqBs.Position;
            var CrntExtp = EXTPBS.Position;
            var CrntExpn = ExpnBs.Position;
            var CrntExcs = EXCSBS.Position;
            var crntpexp = PexpBs1.Position;
            var crntexts = ExtsBs.Position;
            iScsc = new Data.iScscDataContext(ConnectionString);
            RqrqBs.DataSource = iScsc.Request_Requesters.Where(rg => rg.Regulation == (Data.Regulation)ReglBs.Current).OrderBy(rq => rq.RQTP_CODE).ThenBy(rq => rq.RQTT_CODE);
            GV_RQRQ.TopRowIndex = CrntRqrq;
            RqrqBs.Position = CrntRqrq;
            EXTPBS.Position = CrntExtp;
            ExpnBs.Position = CrntExpn;
            EXCSBS.Position = CrntExcs;
            PexpBs1.Position = crntpexp;
            ExtsBs.Position = crntexts;
            AllExpnBs1.DataSource = iScsc.Expenses.Where(ex => ex.Regulation == (Data.Regulation)ReglBs.Current && ex.EXPN_STAT == "002" && ex.Expense_Type.Request_Requester.PERM_STAT == "002");
         }
      }

      partial void SubmitRqdc_Click(object sender, EventArgs e)
      {
         try
         {
            /*
            this.Focus();
            Validate();
            request_DocumentsBindingSource.EndEdit();

            request_DocumentsBindingSource.MoveFirst();
            for (int i = 0; i < request_DocumentsBindingSource.Count; i++)
            {
               var rqdc = request_DocumentsBindingSource.Current as Data.Request_Document;
               if (rqdc.RQRQ_CODE == 0)
               {
                  rqdc.RQRQ_CODE = (request_RequestersBindingSource.Current as Data.Request_Requester).CODE;
               }
               request_DocumentsBindingSource.MoveNext();
            }

            iScsc.SubmitChanges();*/
            Rqdc_Gv.PostEditor();
            SubmitRqrq_Click(null, null);
            /*iScsc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, iScsc.Request_Documents);
            request_DocumentsBindingSource.DataSource = regulationBindingSource;*/
            /*iScsc
            .Request_Documents
            .Where(rqdc => 
               rqdc.Regulation == ((Data.Regulation)regulationBindingSource.Current) && 
               rqdc.Request_Requester == ((Data.Request_Requester)request_RequestersBindingSource.Current)
             );*/
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      partial void HL_NEW_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         if (e.Button.Tag.ToString() == "New")
         {
            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 07 /* Execute Mstr_Dcmt_F */)
                  })
            );
         }
         else if(e.Button.Tag.ToString() == "Load")
         {
            DcspBs.DataSource = iScsc.Document_Specs.ToList();
         }
      }

      partial void RqrqBs_BindingComplete(object sender, BindingCompleteEventArgs e)
      {
         if (RqrqBs.DataSource != null)
         {
            var Current = RqrqBs.Current as Data.Request_Requester;

            //RqdcBs2.DataSource = iScsc.Request_Documents.Where(r => r.Request_Requester == Current);
         }
      }

      partial void request_RequestersBindingSource_PositionChanged(object sender, EventArgs e)
      {
         if (RqrqBs.DataSource != null)
         {
            var Current = RqrqBs.Current as Data.Request_Requester;

            RqdcBs2.DataSource = iScsc.Request_Documents.Where(r => r.Request_Requester == Current);
         }
      }

      partial void RqdcBsAddNewItem_Click(object sender, EventArgs e)
      {
         Pnl_Rqdc.Visible = true;
         RqdcBsAddNewItem.Enabled = false;
         DelRqdc_Butn.Enabled = false;
         SubmitRqdc.Enabled = false;
      }

      partial void Btn_SaveRqdc_Click(object sender, EventArgs e)
      {
         try
         {
            var Regl = ReglBs.Current as Data.Regulation;
            var Rqrq = RqrqBs.Current as Data.Request_Requester;

            iScsc.INS_RQDC_P(Convert.ToInt64(Cmb_DcmtDsid.EditValue), Rqrq.CODE, Cmb_NeedType.EditValue.ToString(), Cmb_OrigType.EditValue.ToString(), Cmb_FrstNeed.EditValue.ToString());
            Btn_Cncl_Click(null, null);
            requery = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               SubmitRqrq_Click(null, null);
            }
         }
      }

      partial void Btn_Cncl_Click(object sender, EventArgs e)
      {
         Pnl_Rqdc.Visible = false;
         RqdcBsAddNewItem.Enabled = true;
         DelRqdc_Butn.Enabled = true;
         SubmitRqdc.Enabled = true;

         iScsc.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, iScsc.Request_Documents);
         if (RqrqBs.DataSource != null)
         {
            var Current = RqrqBs.Current as Data.Request_Requester;

            RqdcBs2.DataSource = iScsc.Request_Documents.Where(r => r.Request_Requester == Current);
         }
      }

      partial void HL_ACTV_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var CrntExtp = EXTPBS.Current as Data.Expense_Type;
         if (e.Button.Index == 0)
         {
            if (EXTPBS
               .List
               .OfType<Data.Expense_Type>()
               .Any(extp => extp == CrntExtp && extp.Expenses.Any(expn => expn.EXPN_STAT == "002")))
               ExpnBs.List.OfType<Data.Expense>()
                  .Where(expn => expn.Expense_Type == CrntExtp)
                  .ToList()
                  .ForEach(expn => expn.EXPN_STAT = "001");
            else
               ExpnBs.List.OfType<Data.Expense>()
                  .Where(expn => expn.Expense_Type == CrntExtp)
                  .ToList()
                  .ForEach(expn => expn.EXPN_STAT = "002");
         }
         else if(e.Button.Index == 1)
         {
            if (EXTPBS
               .List
               .OfType<Data.Expense_Type>()
               .Any(extp => extp == CrntExtp && extp.Expenses.Any(expn => expn.COVR_TAX == "002")))
               ExpnBs.List.OfType<Data.Expense>()
                  .Where(expn => expn.Expense_Type == CrntExtp)
                  .ToList()
                  .ForEach(expn => expn.COVR_TAX = "001");
            else
               ExpnBs.List.OfType<Data.Expense>()
                  .Where(expn => expn.Expense_Type == CrntExtp)
                  .ToList()
                  .ForEach(expn => expn.COVR_TAX = "002");
         }
         else if (e.Button.Index == 2)
         {
            try
            {
               if (MessageBox.Show(this, "آیا با حذف کردن نوع هزینه موافقید؟", "حذف نوع هزینه", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
               iScsc.REGL_TOTL_P(
                  new XElement("Config",
                     new XAttribute("type", "001"),
                     new XElement("Delete",
                        new XElement("Expense_Type",
                           new XAttribute("code", (EXTPBS.Current as Data.Expense_Type).CODE)
                        )
                     )
                  )
               );
               requery = true;
            }
            catch { }
            finally
            {
               if (requery)
               {
                  if (requery)
                  {
                     var CrntRqrq = RqrqBs.Position;
                     var CrntExtpIndex = EXTPBS.Position;
                     var CrntExpn = ExpnBs.Position;
                     var CrntExcs = EXCSBS.Position;
                     iScsc = new Data.iScscDataContext(ConnectionString);
                     RqrqBs.DataSource = iScsc.Request_Requesters.Where(rg => rg.Regulation == (Data.Regulation)ReglBs.Current).OrderBy(rq => rq.RQTP_CODE).ThenBy(rq => rq.RQTT_CODE);
                     GV_RQRQ.TopRowIndex = CrntRqrq;
                     RqrqBs.Position = CrntRqrq;
                     EXTPBS.Position = CrntExtpIndex;
                     ExpnBs.Position = CrntExpn;
                     EXCSBS.Position = CrntExcs;
                     requery = false;
                  }
               }
            }
         }
      }
   }
}
