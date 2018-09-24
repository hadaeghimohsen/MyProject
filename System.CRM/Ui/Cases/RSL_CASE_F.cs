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

namespace System.CRM.Ui.Cases
{
   public partial class RSL_CASE_F : UserControl
   {
      public RSL_CASE_F()
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

            CaseBs.DataSource = iCRM.Cases.Where(c => c.CSID == csid);            

            requery = false;
         }
         catch { }
      }      

      private void SubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cases = CaseBs.Current as Data.Case;
            if (cases == null) return;

            if (Stat_Lov.EditValue == null || Stat_Lov.EditValue.ToString() == "" || (Stat_Lov.EditValue.NotIn("013", "014"))) { Stat_Lov.Focus(); return; }
            if (RespDesc_Txt.EditValue == null || RespDesc_Txt.EditValue.ToString() == "") { RespDesc_Txt.Focus(); return; }
            if (TotlMinTime_Txt.EditValue == null || TotlMinTime_Txt.EditValue.ToString() == "") { TotlMinTime_Txt.Focus(); return; }
            if (RmrkDesc_Txt.EditValue == null || RmrkDesc_Txt.EditValue.ToString() == "") { RmrkDesc_Txt.Focus(); return; }

            iCRM.CONF_CASE_P(
               new XElement("Case", 
                  new XAttribute("csid", cases.CSID),
                  new XAttribute("rqid", cases.RQRO_RQST_RQID),
                  new XAttribute("colr", cases.STAT == "013" ? ColorTranslator.ToHtml( Color.Lime ) : ColorTranslator.ToHtml( Color.Cyan ) ),
                  new XAttribute("stat", Stat_Lov.EditValue),
                  new XAttribute("respdesc", RespDesc_Txt.EditValue),
                  new XAttribute("totlmintime", TotlMinTime_Txt.EditValue),
                  new XAttribute("rmrkdesc", RmrkDesc_Txt.EditValue)
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
                        new Job(SendType.SelfToUserInterface, formCaller, 10 /* Execute Actn_Calf_F */){Input = new XElement("Case", new XAttribute("type", "refresh"))}
                     }
                  )
               );
            }
         }
      }
   }
}
