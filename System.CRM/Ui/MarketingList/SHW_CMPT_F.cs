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
using DevExpress.XtraEditors.Controls;
using System.CRM.ExceptionHandlings;
using System.IO;
using System.Drawing.Imaging;

namespace System.CRM.Ui.Competitor
{
   public partial class SHW_CMPT_F : UserControl
   {
      public SHW_CMPT_F()
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
            if (CompActn_Butn.Buttons.OfType<EditorButton>().Any(b => b.Tag != null && b.Visible == false))
               CompActn_Butn.Buttons.OfType<EditorButton>().FirstOrDefault(b => b.Tag != null && b.Visible == false).Visible = true;

            CompActn_Butn.Buttons.OfType<EditorButton>().FirstOrDefault(b => b.Tag != null && b.Tag.ToString() == (onoftag == "on" ? "002" : "001")).Visible = false;

            iCRM = new Data.iCRMDataContext(ConnectionString);

            var Qxml = Filter_Butn.Tag as XElement;
            
            if (Qxml == null)
               Qxml =
                  new XElement("Company",
                     new XAttribute("recdstat", onoftag == "on" ? "002" : "001"),
                     new XAttribute("type", "002")
                  );
            else
               Qxml.Add(
                  new XElement("Company",
                     new XAttribute("recdstat", onoftag == "on" ? "002" : "001"),
                     new XAttribute("type", "002")
                  )
               );

            ///******
            //switch(AcntsSearch_Lov.Tag.ToString())
            //{
            //   case "001": // نمایش کل شرکت ها                  
            //      break;
            //   case "002": // نمایش شرکت های پیش فرض نواحی
            //      Qxml.Add(
            //         new XElement("Company",
            //            new XAttribute("dfltstat", "002")
            //         )
            //      );
            //      break;
            //   case "003": // نمایش شرکت های غیر از پیش فرض نواحی
            //      Qxml.Add(
            //         new XElement("Company",
            //            new XAttribute("dfltstat", "001")
            //         )
            //      );
            //      break;
            //   case "004": // شرکت هایی که مشتری به آن متصل می باشد
            //      Qxml.Add(
            //         new XElement("Company",
            //            new XAttribute("empynumb", "002")
            //         )
            //      );
            //      break;
            //   case "005": // شرکت هایی که مشتری با آن متصل نمی باشد
            //      Qxml.Add(
            //         new XElement("Company",
            //            new XAttribute("empynumb", "001")
            //         )
            //      );
            //      break;
            //}
            CmptBs.DataSource =
               iCRM.VF_Companies(Qxml);
            requery = false;
         }
         catch { }
         finally
         {
            Comp_Gv.BestFitColumns();
         }
      }

      #region Menu
      private void New_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 93 /* Execute Inf_Cmpt_F */),
                new Job(SendType.SelfToUserInterface, "INF_CMPT_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Competitor",
                        new XAttribute("formcaller", GetType().Name)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var cmpt = CmptBs.Current as Data.VF_CompaniesResult;
            if (cmpt == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 93 /* Execute Inf_Cmpt_F */),
                   new Job(SendType.SelfToUserInterface, "INF_CMPT_F", 10 /* Execute Actn_Calf_F */)
                   {
                      Input = 
                        new XElement("Competitor",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("cmptcode", cmpt.CODE)
                        )
                   }
                 }
              );
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch
         {
            
            throw;
         }
      }

      private void Delete_Butn_Click(object sender, EventArgs e)
      {

      }

      private void BulkDelete_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Active_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Deactive_Butn_Click(object sender, EventArgs e)
      {

      }

      private void FindDuplicateSelected_Butn_Click(object sender, EventArgs e)
      {

      }

      private void FindDuplicateAll_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Follow_Butn_Click(object sender, EventArgs e)
      {

      }

      private void UnFollow_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Merge_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Report_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Import_Butn_Click(object sender, EventArgs e)
      {

      }

      private void Export_Butn_Click(object sender, EventArgs e)
      {

      }
      #endregion

      #region Search
      private void Filter_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 78 /* Execute Hst_Fltr_F */),
                  new Job(SendType.SelfToUserInterface, "HST_FLTR_F", 10 /* Execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Filter",
                           new XAttribute("formcaller", GetType().Name)
                        )
                  }
               
               }
            )
         );
      }

      private void ShowMap_Butn_Click(object sender, EventArgs e)
      {

      }

      private void GridFind_Tgbt_Click(object sender, EventArgs e)
      {
         Comp_Gv.OptionsFind.AlwaysVisible = !Comp_Gv.OptionsFind.AlwaysVisible;
      }
      #endregion
   }
}
