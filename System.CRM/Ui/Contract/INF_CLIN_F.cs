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
using System.Globalization;
using System.IO;
using Itenso.TimePeriod;
using DevExpress.XtraGrid.Views.Grid;
using System.MaxUi;
using DevExpress.XtraEditors;
using System.CRM.ExtCode;

namespace System.CRM.Ui.Contract
{
   public partial class INF_CLIN_F : UserControl
   {
      public INF_CLIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private bool queryAllRequest = false;
      private int cntrindex;
      private string formType = "normal";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         try
         {
            iCRM = new Data.iCRMDataContext(ConnectionString);

            cntrindex = CntrBs.Position;

            CntrBs.DataSource =
               iCRM.Contracts.Where(c => c.CNID == cnid);
            
            CntrBs.Position = cntrindex;
            requery = false;
         }
         catch { }
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
            else if(e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.Self, 79 /* Execute Apbs_Dfin_F */),
                        new Job(SendType.SelfToUserInterface, "APBS_DFIN_F", 10 /* Execute Actn_CalF_F */)
                        {
                           Input = 
                              new XElement("App_Base",
                                 new XAttribute("tablename", "CONTRACT"),
                                 new XAttribute("formcaller", GetType().Name)
                              )
                        }
                     }
                  )
               );
            }
         }
         catch { }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            var cntrtype = xinput.Attribute("type").Value;

            var clin = ClinBs.Current as Data.Contract_Line;
            
            StrtDate_Dt.CommitChanges();
            EndDate_Dt.CommitChanges();

            if (Titl_Txt.EditValue == null || Titl_Txt.EditValue.ToString() == "") { Titl_Txt.Focus(); return; }
            if (!StrtDate_Dt.Value.HasValue) { StrtDate_Dt.Focus(); return; }
            if (!EndDate_Dt.Value.HasValue) { EndDate_Dt.Focus(); return; }

            ClinBs.EndEdit();

            iCRM.SubmitChanges();
            
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SubmitChangeClose_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ExecuteQuery_Butn_ButtonClick(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void ExecQuryCrntUser_Butn_Click(object sender, EventArgs e)
      {
         queryAllRequest = false;
         Execute_Query();
      }

      private void ExecQuryAllUser_Butn_Click(object sender, EventArgs e)
      {
         queryAllRequest = true;
         Execute_Query();
      }      

      #region Note
      private void AddNote_Butn_Click(object sender, EventArgs e)
      {
         var cnln = ClinBs.Current as Data.Contract_Line;
         if (cnln == null || cnln.CLID == 0) return;
         if (NoteBs.List.OfType<Data.Note>().Any(n => n.NTID == 0)) return;

         NoteBs.AddNew();
         var note = NoteBs.Current as Data.Note;
         note.CNLN_CLID = cnln.CLID;
         note.SERV_FILE_NO = cnln.Contract.SRPB_SERV_FILE_NO;
         note.COMP_CODE_DNRM = cnln.Contract.COMP_CODE;

         Note_Gv.SelectRow(Note_Gv.RowCount - 1);

         iCRM.Notes.InsertOnSubmit(note);
      }

      private void DelNote_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cnln = ClinBs.Current as Data.Contract_Line;
            if (cnln == null || cnln.CLID == 0) return;

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

      private void RightMenuLink_Lb_Click(object sender, EventArgs e)
      {
         var menulink = sender as LabelControl;
         switch (menulink.Tag.ToString())
         {
            case "1":
               Titl_Txt.Focus();
               break;
            default:
               break;
         }
      }

      private void ClinDel_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var clin = ClinBs.Current as Data.Contract_Line;
            if (clin == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            iCRM.Contract_Lines.DeleteOnSubmit(clin);

            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Back_Butn_Click(null, null);
         }
      }
   }
}
