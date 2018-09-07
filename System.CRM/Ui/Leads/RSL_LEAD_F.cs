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

namespace System.CRM.Ui.Leads
{
   public partial class RSL_LEAD_F : UserControl
   {
      public RSL_LEAD_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

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

            LeadBs.DataSource = iCRM.Leads.Where(l => l.LDID == ldid);            

            requery = false;
         }
         catch { }
      }

      #region Campatitor
      private void AddCampatitor_Butn_Click(object sender, EventArgs e)
      {
         var lead = LeadBs.Current as Data.Lead;
         if (lead == null) return;
         if (LdcmBs.List.OfType<Data.Lead_Competitor>().Any(lc => lc.LCID == 0)) return;

         LdcmBs.AddNew();
         var ldcm = LdcmBs.Current as Data.Lead_Competitor;
         ldcm.LEAD_LDID = lead.LDID;

         Ldcm_Gv.SelectRow(Ldcm_Gv.RowCount - 1);

         iCRM.Lead_Competitors.InsertOnSubmit(ldcm);
      }

      private void DelCampatitor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;


            var rows = Ldcm_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Lead_Competitor)Ldcm_Gv.GetRow(r);
               iCRM.Lead_Competitors.DeleteOnSubmit(row);
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

      private void SaveCampatitor_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            LdcmBs.EndEdit();
            Ldcm_Gv.PostEditor();

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

      private void ShowCampatitor_Butn_Click(object sender, EventArgs e)
      {

      }

      private void HelpCampatitor_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      private void LeadBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            switch (conftype)
            {
               case "003":
                  lead.STAT = "009";
                  break;
               case "004":
                  lead.STAT = "010";
                  break;
               default:
                  break;
            }
         }
         catch
         {}
      }

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var lead = LeadBs.Current as Data.Lead;
            if (lead == null) return;

            iCRM.CONF_LEAD_P(
               new XElement("Lead", 
                  new XAttribute("ldid", lead.LDID),
                  new XAttribute("rqid", lead.RQRO_RQST_RQID),
                  new XAttribute("colr", ColorTranslator.ToHtml( RqstMsttColor_Butn.NormalColorA )),
                  new XAttribute("conftype", conftype),
                  new XAttribute("emstclosdate", EmstClosDate_Dt.Value.HasValue ? EmstClosDate_Dt.Value.Value.ToString("yyyy-MM-dd") : ""),
                  new XAttribute("emstrevnamnt", EmstRevnAmnt_Txt.EditValue)
               )
            );

            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               // بستن فرم و اجرا دوباره فرم مبدا
               _DefaultGateway.Gateway(
                  new Job(SendType.External, "localhost",
                     new List<Job>
                     {
                        new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape},
                        new Job(SendType.SelfToUserInterface, formCaller, 10 /* Execute Actn_Calf_F */){Input = new XElement("Lead", new XAttribute("type", "refresh"))}
                     }
                  )
               );
            }
         }
      }
   }
}
