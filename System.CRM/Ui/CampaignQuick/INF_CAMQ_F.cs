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

namespace System.CRM.Ui.CampaignQuick
{
   public partial class INF_CAMQ_F : UserControl
   {
      public INF_CAMQ_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller;
      private XElement xinput;
      private long? camqcode;

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

         if (camqcode != null)
         {
            int camq = CamqBs.Position;

            CamqBs.DataSource = iCRM.Campaign_Quicks.Where(c => c.QCID == camqcode);

            CamqBs.Position = camq;
         }
         else
         {
            CamqBs.AddNew();
            var camq = CamqBs.Current as Data.Campaign_Quick;

            camq.OWNR_CODE = JobpBs.List.OfType<Data.Job_Personnel>().FirstOrDefault(jp => jp.USER_NAME == CurrentUser).CODE;
            camq.MEMB_TYPE = "001";
            camq.STAT_RESN = "001";
            camq.ACTV_TYPE = "001";           

            iCRM.Campaign_Quicks.InsertOnSubmit(camq);
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
            if (SubjDesc_Txt.EditValue == null || SubjDesc_Txt.EditValue.ToString() == "") { SubjDesc_Txt.Focus(); return; }

            CamqBs.EndEdit();

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
               if (camqcode == null)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);
                  var ownrcode = (long?)Ownr_Lov.EditValue;
                  camqcode = iCRM.Campaign_Quicks.FirstOrDefault(c => c.OWNR_CODE == ownrcode && c.SUBJ_DESC == SubjDesc_Txt.Text && c.CRET_BY == CurrentUser && c.CRET_DATE.Value.Date == DateTime.Now.Date).QCID;
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
         if (camqcode == null) return;
         if (NoteBs.List.OfType<Data.Note>().Any(n => n.NTID == 0)) return;

         NoteBs.AddNew();
         var note = NoteBs.Current as Data.Note;
         note.CAMQ_QCID = camqcode;

         Note_Gv.SelectRow(Note_Gv.RowCount - 1);

         iCRM.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (camqcode == null) return;

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
