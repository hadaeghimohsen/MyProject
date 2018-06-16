using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using System.CRM.ExceptionHandlings;
using DevExpress.XtraEditors;

namespace System.CRM.Ui.MarketingList
{
   public partial class INF_MKLT_F : UserControl
   {
      public INF_MKLT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller;
      private XElement xinput;
      private long? mkltcode;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);

         //CmptBs.List.Clear();

         if (mkltcode != null)
         {
            int cmdf = MkltBs.Position;

            MkltBs.DataSource = iCRM.Marketing_Lists.Where(m => m.MLID == mkltcode);

            MkltBs.Position = cmdf;
         }
         else
         {
            MkltBs.AddNew();
            var mklt = MkltBs.Current as Data.Marketing_List;

            mklt.OWNR_CODE = JobpBs.List.OfType<Data.Job_Personnel>().FirstOrDefault(jp => jp.USER_NAME == CurrentUser).CODE;
            mklt.LOCK_STAT = "001";
            mklt.LIST_TYPE = "001";
            mklt.TRGT = "001";           

            iCRM.Marketing_Lists.InsertOnSubmit(mklt);
         }

         requery = false;
      }

      private void RqstBnDefaultPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnPrint1_Click(object sender, EventArgs e)
      {

      }

      private void RqstBnSettingPrint1_Click(object sender, EventArgs e)
      {

      }

      private void ObjectBaseEdit_ButtonPressed(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Delete)
            {
               var control = sender as BaseEdit;
               if (control != null)
                  control.EditValue = null;
            }
         }
         catch { }
      }

      private void CmptBs_CurrentChanged(object sender, EventArgs e)
      {
         
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            if (Name_Txt.EditValue == null || Name_Txt.EditValue.ToString() == "") { Name_Txt.Focus(); return; }

            MkltBs.EndEdit();

            iCRM.SubmitChanges();
            
            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
            {
               if (mkltcode == null)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  var ownrcode = (long?)Ownr_Lov.EditValue;
                  mkltcode = iCRM.Marketing_Lists.FirstOrDefault(mk => mk.OWNR_CODE == ownrcode && mk.NAME == Name_Txt.Text && mk.CRET_BY == CurrentUser && mk.CRET_DATE.Value.Date == DateTime.Now.Date).MLID;
               }
               Execute_Query();
            }
         }
      }

      private void SubmitChangeClose_Butn_Click(object sender, EventArgs e)
      {
         SubmitChange_Butn_Click(null, null);
         Btn_Back_Click(null, null);
      }

      #region Note
      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;
         if (NoteBs.List.OfType<Data.Note>().Any(n => n.NTID == 0)) return;         

         NoteBs.AddNew();
         var note = NoteBs.Current as Data.Note;
         note.MKLT_MLID = mkltcode;

         Note_Gv.SelectRow(Note_Gv.RowCount - 1);

         iCRM.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (mkltcode == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Note_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Note)Note_Gv.GetRow(r);
               iCRM.Notes.DeleteOnSubmit(row);
            }

            iCRM.SubmitChanges();

            requery = true;
         }
         catch(Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            NoteBs.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }
      #endregion

      #region Campaign
      private void AddCamp_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;
         if (MklcBs.List.OfType<Data.Marketing_List_Campaign>().Any(mc => mc.MCID == 0)) return;

         MklcBs.AddNew();
         var mklc = MklcBs.Current as Data.Marketing_List_Campaign;
         mklc.MKLT_MLID = mkltcode;

         Mklc_Gv.SelectRow(Mklc_Gv.RowCount - 1);

         iCRM.Marketing_List_Campaigns.InsertOnSubmit(mklc);
      }

      private void DelCamp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (mkltcode == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Mklc_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Marketing_List_Campaign)Mklc_Gv.GetRow(r);
               iCRM.Marketing_List_Campaigns.DeleteOnSubmit(row);
            }

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveCamp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            MklcBs.EndEdit();

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            iCRM.SaveException(exc);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowCamp_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpCamp_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion      
   }
}
