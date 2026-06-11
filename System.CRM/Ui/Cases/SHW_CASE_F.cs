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
using System.CRM.ExtCode;
using C1.Win.C1Ribbon;

namespace System.CRM.Ui.Cases
{
   public partial class SHW_CASE_F : UserControl
   {
      public SHW_CASE_F()
      {
         InitializeComponent();
      }

      private string onoftag;
      private long compcode = 0;
      private string caseShow = "15";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private async void Execute_Query()
       {
          var _caseShow = caseShow;
          var _compcode = compcode;
          var _currentUser = CurrentUser;

          List<Data.Case> result = null;
          bool clearList = false;

          switch (_caseShow)
          {
             case "1":
             case "2":
             case "3":
             case "10":
             case "12":
             case "16":
                clearList = true;
                break;
          }

          if (!clearList)
          {
             result = await Task.Run(() =>
             {
                DateTime StrtDate = DateTime.Now;
                using (var db = new Data.iCRMDataContext(ConnectionString))
                {
                   switch (_caseShow)
                   {
                      case "4":
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                            ).ToList();
                      case "5":
                         switch (DateTime.Now.DayOfWeek)
                         {
                            case DayOfWeek.Sunday: StrtDate = DateTime.Now.AddDays(-1); break;
                            case DayOfWeek.Monday: StrtDate = DateTime.Now.AddDays(-2); break;
                            case DayOfWeek.Tuesday: StrtDate = DateTime.Now.AddDays(-3); break;
                            case DayOfWeek.Wednesday: StrtDate = DateTime.Now.AddDays(-4); break;
                            case DayOfWeek.Thursday: StrtDate = DateTime.Now.AddDays(-5); break;
                            case DayOfWeek.Friday: StrtDate = DateTime.Now.AddDays(-6); break;
                         }
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && (c.STAT == "007" || c.STAT == "008" || c.STAT == "011" || c.STAT == "012")
                              && (c.CRET_DATE.Value.Date >= StrtDate)
                            ).ToList();
                      case "6":
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && (c.STAT == "013" || c.STAT == "014")
                            ).ToList();
                      case "7":
                         switch (DateTime.Now.DayOfWeek)
                         {
                            case DayOfWeek.Sunday: StrtDate = DateTime.Now.AddDays(-1); break;
                            case DayOfWeek.Monday: StrtDate = DateTime.Now.AddDays(-2); break;
                            case DayOfWeek.Tuesday: StrtDate = DateTime.Now.AddDays(-3); break;
                            case DayOfWeek.Wednesday: StrtDate = DateTime.Now.AddDays(-4); break;
                            case DayOfWeek.Thursday: StrtDate = DateTime.Now.AddDays(-5); break;
                            case DayOfWeek.Friday: StrtDate = DateTime.Now.AddDays(-6); break;
                         }
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && c.Job_Personnel.USER_NAME == _currentUser
                              && (c.STAT == "013" || c.STAT == "014")
                              && (c.CRET_DATE.Value.Date >= StrtDate)
                            ).ToList();
                      case "8":
                         switch (DateTime.Now.DayOfWeek)
                         {
                            case DayOfWeek.Sunday: StrtDate = DateTime.Now.AddDays(-1); break;
                            case DayOfWeek.Monday: StrtDate = DateTime.Now.AddDays(-2); break;
                            case DayOfWeek.Tuesday: StrtDate = DateTime.Now.AddDays(-3); break;
                            case DayOfWeek.Wednesday: StrtDate = DateTime.Now.AddDays(-4); break;
                            case DayOfWeek.Thursday: StrtDate = DateTime.Now.AddDays(-5); break;
                            case DayOfWeek.Friday: StrtDate = DateTime.Now.AddDays(-6); break;
                         }
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && (c.STAT == "013" || c.STAT == "014")
                              && (c.CRET_DATE.Value.Date >= StrtDate)
                            ).ToList();
                      case "9":
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && c.Job_Personnel.USER_NAME == _currentUser
                              && (c.STAT == "013" || c.STAT == "014")
                            ).ToList();
                      case "11":
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && (c.STAT == "007" || c.STAT == "008" || c.STAT == "011" || c.STAT == "012")
                            ).ToList();
                      case "13":
                         switch (DateTime.Now.DayOfWeek)
                         {
                            case DayOfWeek.Sunday: StrtDate = DateTime.Now.AddDays(-1); break;
                            case DayOfWeek.Monday: StrtDate = DateTime.Now.AddDays(-2); break;
                            case DayOfWeek.Tuesday: StrtDate = DateTime.Now.AddDays(-3); break;
                            case DayOfWeek.Wednesday: StrtDate = DateTime.Now.AddDays(-4); break;
                            case DayOfWeek.Thursday: StrtDate = DateTime.Now.AddDays(-5); break;
                            case DayOfWeek.Friday: StrtDate = DateTime.Now.AddDays(-6); break;
                         }
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && (c.STAT == "007" || c.STAT == "008" || c.STAT == "011" || c.STAT == "012")
                              && (c.CRET_DATE.Value.Date >= StrtDate)
                            ).ToList();
                      case "14":
                         switch (DateTime.Now.DayOfWeek)
                         {
                            case DayOfWeek.Sunday: StrtDate = DateTime.Now.AddDays(-1); break;
                            case DayOfWeek.Monday: StrtDate = DateTime.Now.AddDays(-2); break;
                            case DayOfWeek.Tuesday: StrtDate = DateTime.Now.AddDays(-3); break;
                            case DayOfWeek.Wednesday: StrtDate = DateTime.Now.AddDays(-4); break;
                            case DayOfWeek.Thursday: StrtDate = DateTime.Now.AddDays(-5); break;
                            case DayOfWeek.Friday: StrtDate = DateTime.Now.AddDays(-6); break;
                         }
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && c.Job_Personnel.USER_NAME == _currentUser
                              && (c.STAT == "007" || c.STAT == "008" || c.STAT == "011" || c.STAT == "012")
                              && (c.CRET_DATE.Value.Date >= StrtDate)
                            ).ToList();
                      case "15":
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                              && c.Job_Personnel.USER_NAME == _currentUser
                              && (c.STAT == "007" || c.STAT == "008" || c.STAT == "011" || c.STAT == "012")
                            ).ToList();
                      case "17":
                         return db.Cases.Where(
                            c => (c.COMP_CODE ?? 0) == (_compcode != 0 ? _compcode : (c.COMP_CODE ?? 0))
                              && c.Request_Row.Request.RQST_STAT != "003"
                            ).ToList();
                      default:
                         return null;
                   }
                }
             });
          }

          iCRM = new Data.iCRMDataContext(ConnectionString);

          if (result == null || clearList)
             CaseBs.List.Clear();
          else
             CaseBs.DataSource = result;
       }



      #region Mouse Click
      private void Case_Gv_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            var cases = CaseBs.Current as Data.Case;
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 105 /* Execute Inf_Case_F */),
                   new Job(SendType.SelfToUserInterface, "INF_CASE_F", 10 /* Execute Actn_Calf_F */)
                   {
                      Input = 
                        new XElement("Case",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("type", "newcaseupdate"),
                           new XAttribute("rqid", cases.RQRO_RQST_RQID),
                           new XAttribute("formtype", cases.Request_Row.Request.RQST_STAT == "002" ? "showhistory" : "normal")
                        )
                   }
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch {}         
      }
      #endregion

      #region Menu
      private void New_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithCRM =
           new Job(SendType.External, "Localhost",
              new List<Job>
              {                  
                new Job(SendType.Self, 105 /* Execute Inf_Case_F */),
                new Job(SendType.SelfToUserInterface, "INF_CASE_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Case",
                        new XAttribute("formcaller", GetType().Name),
                        new XAttribute("type", "newcase")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         Case_Gv_DoubleClick(null, null);
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
         Case_Gv.OptionsFind.AlwaysVisible = !Case_Gv.OptionsFind.AlwaysVisible;
      }

      private void CaseShows_Btn_Click(object sender, EventArgs e)
      {
         caseShow = ((RibbonButton)sender).Tag.ToString();
         Execute_Query();
      }
      #endregion
   }
}
