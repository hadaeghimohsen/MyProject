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

namespace System.CRM.Ui.Campaign
{
   public partial class INF_CAMP_F : UserControl
   {
      public INF_CAMP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller;
      private XElement xinput;
      private long? campcode;

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

         if (campcode != null)
         {
            int cmdf = CampBs.Position;

            CampBs.DataSource = iCRM.Campaigns.Where(c => c.CMID == campcode);

            CampBs.Position = cmdf;
         }
         else
         {
            JobpBs.DataSource = iCRM.Job_Personnels.Where(o => o.STAT == "002");
            CampBs.AddNew();
            var camp = CampBs.Current as Data.Campaign;

            camp.OWNR_CODE = JobpBs.List.OfType<Data.Job_Personnel>().FirstOrDefault(jp => jp.USER_NAME == CurrentUser).CODE;
            camp.TEMP = "001";
            camp.STAT = "001";
            camp.TYPE = "001";           

            iCRM.Campaigns.InsertOnSubmit(camp);
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

            CampBs.EndEdit();

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
               if (campcode == null)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  var ownrcode = (long?)Ownr_Lov.EditValue;
                  campcode = iCRM.Campaigns.FirstOrDefault(c => c.OWNR_CODE == ownrcode && c.NAME == Name_Txt.Text && c.CRET_BY == CurrentUser && c.CRET_DATE.Value.Date == DateTime.Now.Date).CMID;
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
         if (campcode == null) return;
         if (NoteBs.List.OfType<Data.Note>().Any(n => n.NTID == 0)) return;

         NoteBs.AddNew();
         var note = NoteBs.Current as Data.Note;
         note.CAMP_CMID = campcode;

         Note_Gv.SelectRow(Note_Gv.RowCount - 1);

         iCRM.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (campcode == null) return;

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
   }
}
