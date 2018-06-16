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

namespace System.CRM.Ui.Competitor
{
   public partial class INF_CMPT_F : UserControl
   {
      public INF_CMPT_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller;
      private XElement xinput;
      private long? cmptcode;

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

         if (cmptcode != null)
         {
            int cmdf = CmdfBs.Position;

            CmptBs.DataSource = iCRM.Companies.Where(c => c.CODE == cmptcode);

            CmdfBs.Position = cmdf;
         }
         else
         {
            CmptBs.AddNew();
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
            var cmpt = CmptBs.Current as Data.Company;

            CntyBs.DataSource = iCRM.Countries;

            if(cmpt.CODE != 0)
            {               
               PrvnBs.DataSource = iCRM.Provinces.Where(p => p.CNTY_CODE == cmpt.REGN_PRVN_CNTY_CODE);
               RegnBs.DataSource = iCRM.Regions.Where(r => r.PRVN_CNTY_CODE == cmpt.REGN_PRVN_CNTY_CODE && r.PRVN_CODE == cmpt.REGN_PRVN_CODE);
            }
         }
         catch { }
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {            
            if (Name_Txt.EditValue == null || Name_Txt.EditValue.ToString() == "") { Name_Txt.Focus(); return; }
            if (PostAdrs_Txt.EditValue == null || PostAdrs_Txt.EditValue.ToString() == "") { PostAdrs_Txt.Focus(); return; }
            if (Cnty_Lov.EditValue == null || Cnty_Lov.EditValue.ToString() == "") { Cnty_Lov.Focus(); return; }
            if (Prvn_Lov.EditValue == null || Prvn_Lov.EditValue.ToString() == "") { Prvn_Lov.Focus(); return; }
            if (Regn_Lov.EditValue == null || Regn_Lov.EditValue.ToString() == "") { Regn_Lov.Focus(); return; }


            iCRM.COMP_CHNG_P(
               new XElement("Company",                  
                  new XAttribute("code", cmptcode ?? 0),
                  new XAttribute("type", "002"),

                  new XAttribute("cntycode", Cnty_Lov.EditValue ?? ""),
                  new XAttribute("prvncode", Prvn_Lov.EditValue ?? ""),
                  new XAttribute("regncode", Regn_Lov.EditValue ?? ""),

                  new XAttribute("name", Name_Txt.EditValue ?? ""),
                  new XAttribute("postadrs", PostAdrs_Txt.EditValue ?? ""),
                  
                  new XAttribute("website", WebSite_Txt.EditValue ?? ""),
                  new XAttribute("emaladrs", EmalAdrs_Txt.EditValue ?? ""),
                  new XAttribute("econcode", EconCode_Txt.EditValue ?? ""),
                  new XAttribute("revnamnt", RevnAmnt_Txt.EditValue ?? ""),

                  new XAttribute("trcbtcid", TrcbTcid_Lov.EditValue ?? ""),
                  new XAttribute("recdstat", "002"),

                  new XElement("Competitor_Differences",
                     CmdfBs.List.OfType<Data.Competitor_Difference>().
                     Select(cd =>
                        new XElement("Competitor_Difference",
                           new XAttribute("code", cd.CODE),
                           new XAttribute("type", cd.TYPE ?? "001"),
                           new XAttribute("cmnt", cd.CMNT ?? "")
                        )
                     )
                  )
               )
            );
            
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
               if (cmptcode == null)
               {
                  iCRM = new Data.iCRMDataContext(ConnectionString);                  
                  cmptcode = iCRM.Companies.FirstOrDefault(c => c.NAME == Name_Txt.Text && c.CRET_BY == CurrentUser && c.CRET_DATE.Value.Date == DateTime.Now.Date).CODE;
               }
               Execute_Query();
            }
         }
      }

      private void Cnty_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            PrvnBs.DataSource = iCRM.Provinces.Where(p => p.CNTY_CODE == e.NewValue.ToString());
         }
         catch { }
      }

      private void Prvn_Lov_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
      {
         try
         {
            RegnBs.DataSource = iCRM.Regions.Where(r => r.PRVN_CODE == e.NewValue.ToString() && r.PRVN_CNTY_CODE == Cnty_Lov.EditValue.ToString());
         }
         catch { }
      }

      private void AddCmdf_Butn_Click(object sender, EventArgs e)
      {
         if (CmdfBs.List.OfType<Data.Competitor_Difference>().Any(cd => cd.CODE == 0)) return;

         CmdfBs.AddNew();
      }

      private void DelCmdf_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "حذف کردن رکورد", "آیا با حذف رکورد انتخاب شده مطمئن هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var cmdf = CmdfBs.Current as Data.Competitor_Difference;
            iCRM.Competitor_Differences.DeleteOnSubmit(cmdf);

            iCRM.SubmitChanges();

            requery = true;
         }
         catch 
         {}
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SubmitChangeClose_Butn_Click(object sender, EventArgs e)
      {
         SubmitChange_Butn_Click(null, null);
         Btn_Back_Click(null, null);
      }
   }
}
