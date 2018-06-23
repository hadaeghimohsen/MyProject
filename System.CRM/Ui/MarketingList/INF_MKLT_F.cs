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
      private long? mkltcode, campcode;

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
            JobpBs.DataSource = iCRM.Job_Personnels.Where(o => o.STAT == "002");
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
         try
         {
            var mklt = MkltBs.Current as Data.Marketing_List;
            if (mklt == null) return;

            Trgt_Tc.TabPages.Clear();
            switch (mklt.TRGT)
            {
               case "001":
                  Trgt_Tc.TabPages.Add(Lead_Tp);
                  LeadBs.DataSource = iCRM.Members.Where(m => m.MKLT_MLID == mkltcode && m.Lead != null);
                  break;
               case "002":
                  Trgt_Tc.TabPages.Add(Service_Tp);
                  ServBs.DataSource = iCRM.Members.Where(m => m.MKLT_MLID == mkltcode && m.SERV_FILE_NO != null);
                  break;
               case "003":
                  Trgt_Tc.TabPages.Add(Company_Tp);
                  CompBs.DataSource = iCRM.Members.Where(m => m.MKLT_MLID == mkltcode && m.COMP_CODE != null);
                  break;
               default:
                  break;
            }
         }
         catch (Exception exc)
         { MessageBox.Show(exc.Message); }
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

                  if (campcode != null)
                  {
                     AddCamp_Butn_Click(null, null);
                     SaveCamp_Butn_Click(null, null);
                  }
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
      private void Camp_Lov_AddNewValue(object sender, DevExpress.XtraEditors.Controls.AddNewValueEventArgs e)
      {
         NewCamp_Butn_Click(null, null);
      }

      private void NewCamp_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 97 /* Execute Inf_Camp_F */),
                new Job(SendType.SelfToUserInterface, "INF_CAMP_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Campaign",
                        new XAttribute("formcaller", GetType().Name),
                        new XAttribute("mkltcode", mkltcode)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void AddCamp_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;
         if (MklcBs.List.OfType<Data.Marketing_List_Campaign>().Any(mc => mc.MCID == 0)) return;

         MklcBs.AddNew();
         var mklc = MklcBs.Current as Data.Marketing_List_Campaign;
         mklc.MKLT_MLID = mkltcode;
         mklc.CAMP_CMID = campcode;

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
         if (mkltcode == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 96 /* Execute Shw_Camp_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CAMP_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Campaign", 
                        new XAttribute("onoftag", "on"),
                        new XAttribute("mkltcode", mkltcode)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void HelpCamp_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion      

      #region Quick Campaign
      private void NewCamq_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 99 /* Execute Inf_Camq_F */),
                new Job(SendType.SelfToUserInterface, "INF_CAMQ_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Campaign_Quick",
                        new XAttribute("formcaller", GetType().Name),
                        new XAttribute("mkltcode", mkltcode)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void AddCamq_Butn_Click(object sender, EventArgs e)
      {

      }

      private void DelCamq_Butn_Click(object sender, EventArgs e)
      {

      }

      private void SaveCamq_Butn_Click(object sender, EventArgs e)
      {

      }

      private void ShowCamq_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;

         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 98 /* Execute Shw_Camq_F */),
                new Job(SendType.SelfToUserInterface, "SHW_CAMQ_F", 10 /* Execute Actn_CalF_P */)
                {
                   Executive = ExecutiveType.Asynchronous,
                   Input = 
                     new XElement("Campaign_Quick", 
                        new XAttribute("onoftag", "on"),
                        new XAttribute("mkltcode", mkltcode)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void HelpCamq_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Service
      private void NewServ_Butn_Click(object sender, EventArgs e)
      {

      }

      private void AddServ_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;
         if (ServBs.List.OfType<Data.Member>().Any(m => m.MBID == 0)) return;

         ServBs.AddNew();
         var serv = ServBs.Current as Data.Member;
         serv.MKLT_MLID = mkltcode;

         Serv_Gv.SelectRow(Serv_Gv.RowCount - 1);

         iCRM.Members.InsertOnSubmit(serv);
      }

      private void DelServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (mkltcode == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Serv_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Member)Serv_Gv.GetRow(r);
               iCRM.Members.DeleteOnSubmit(row);
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

      private void SaveServ_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void ShowServ_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpServ_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Company
      private void NewComp_Butn_Click(object sender, EventArgs e)
      {

      }

      private void AddComp_Butn_Click(object sender, EventArgs e)
      {
         if (mkltcode == null) return;
         if (CompBs.List.OfType<Data.Member>().Any(m => m.MBID == 0)) return;

         CompBs.AddNew();
         var comp = CompBs.Current as Data.Member;
         comp.MKLT_MLID = mkltcode;

         Comp_Gv.SelectRow(Comp_Gv.RowCount - 1);

         iCRM.Members.InsertOnSubmit(comp);
      }

      private void DelComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (mkltcode == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Comp_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Member)Comp_Gv.GetRow(r);
               iCRM.Members.DeleteOnSubmit(row);
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

      private void SaveComp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            iCRM.SubmitChanges();

            requery = true;
         }
         catch (Exception exc)
         {}
         finally
         {
            if (requery)
               Execute_Query();
         }

      }

      private void ShowComp_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpComp_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion
   }
}
