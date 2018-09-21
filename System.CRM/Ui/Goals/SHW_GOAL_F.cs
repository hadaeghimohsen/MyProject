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

namespace System.CRM.Ui.Goals
{
   public partial class SHW_GOAL_F : UserControl
   {
      public SHW_GOAL_F()
      {
         InitializeComponent();
      }

      private string onoftag;
      private long compcode = 0;
      private string leadShow = "13";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         DateTime StrtDate = DateTime.Now, EndDate = DateTime.Now;
         switch (leadShow)
         {
            case "1":
               LeadBs.List.Clear();
               break;
            case "2":
               LeadBs.List.Clear();
               break;
            case "3":
               LeadBs.List.Clear();
               break;
            case "4":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && l.CAMP_CMID == null
                     );
               break;
            case "5":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && (l.STAT != "009" && l.STAT != "010")
                     );

               break;
            case "6":
               switch (DateTime.Now.DayOfWeek)
	            {
                  case DayOfWeek.Saturday:                     
                     break;
                  case DayOfWeek.Sunday:
                     StrtDate = DateTime.Now.AddDays(-1);
                     break;
                  case DayOfWeek.Monday:
                     StrtDate = DateTime.Now.AddDays(-2);
                     break;
                  case DayOfWeek.Tuesday:
                     StrtDate = DateTime.Now.AddDays(-3);
                     break;
                  case DayOfWeek.Wednesday:
                     StrtDate = DateTime.Now.AddDays(-4);
                     break;
                  case DayOfWeek.Thursday:
                     StrtDate = DateTime.Now.AddDays(-5);
                     break;
                  case DayOfWeek.Friday:
                     StrtDate = DateTime.Now.AddDays(-6);
                     break;
                  default:
                     break;
	            }
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && (l.STAT != "009" && l.STAT != "010")
                       && (l.CRET_DATE.Value.Date >= StrtDate)
                     );
               break;
            case "7":
               switch (DateTime.Now.DayOfWeek)
	            {
                  case DayOfWeek.Saturday:
                     StrtDate = DateTime.Now.AddDays(-7);
                     EndDate = DateTime.Now.AddDays(-1);
                     break;
                  case DayOfWeek.Sunday:
                     StrtDate = DateTime.Now.AddDays(-8);
                     EndDate = DateTime.Now.AddDays(-2);
                     break;
                  case DayOfWeek.Monday:
                     StrtDate = DateTime.Now.AddDays(-9);
                     EndDate = DateTime.Now.AddDays(-3);
                     break;
                  case DayOfWeek.Tuesday:
                     StrtDate = DateTime.Now.AddDays(-10);
                     EndDate = DateTime.Now.AddDays(-4);
                     break;
                  case DayOfWeek.Wednesday:
                     StrtDate = DateTime.Now.AddDays(-11);
                     EndDate = DateTime.Now.AddDays(-5);
                     break;
                  case DayOfWeek.Thursday:
                     StrtDate = DateTime.Now.AddDays(-12);
                     EndDate = DateTime.Now.AddDays(-6);
                     break;
                  case DayOfWeek.Friday:
                     StrtDate = DateTime.Now.AddDays(-13);
                     EndDate = DateTime.Now.AddDays(-7);
                     break;
                  default:
                     break;
	            }
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && (l.STAT != "009" && l.STAT != "010")
                       && (l.CRET_DATE.Value.Date >= StrtDate)
                       && (l.CRET_DATE.Value.Date <= EndDate)
                     );

               break;
            case "8":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && (l.STAT != "009" && l.STAT != "010")
                       && (l.Job_Personnel.USER_NAME == CurrentUser)
                     );

               break;
            case "9":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && (l.STAT == "009" || l.STAT == "010")
                     );

               break;
            case "10":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && l.CAMP_CMID != null
                     );

               break;
            case "11":
               LeadBs.List.Clear();
               break;
            case "12":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                       && l.CRET_DATE.Value.Date <= DateTime.Now.AddMonths(-6)
                     );
               break;
            case "13":
               LeadBs.DataSource =
                  iCRM.Leads.Where(
                     l => (l.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (l.COMP_CODE ?? 0))
                       && l.Request_Row.Request.RQST_STAT != "003"
                       && l.Request_Row.Request.SSTT_CODE == 1
                     );
               break;
            case "14":
               LeadBs.List.Clear();
               break;
         }         
      }


      #region Mouse Click
      private void Lead_Gv_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            var leads = LeadBs.Current as Data.Lead;
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),
                   new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute Actn_Calf_F */)
                   {
                      Input = 
                        new XElement("Lead",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("type", "newleadupdate"),
                           new XAttribute("rqid", leads.RQRO_RQST_RQID),
                           new XAttribute("formtype", leads.Request_Row.Request.RQST_STAT == "002" ? "showhistory" : "normal")
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
                new Job(SendType.Self, 24 /* Execute Inf_Lead_F */),
                new Job(SendType.SelfToUserInterface, "INF_LEAD_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Lead",
                        new XAttribute("formcaller", GetType().Name),
                        new XAttribute("type", "newlead")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         Lead_Gv_DoubleClick(null, null);
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
         Lead_Gv.OptionsFind.AlwaysVisible = !Lead_Gv.OptionsFind.AlwaysVisible;
      }

      private void LeadShows_Btn_Click(object sender, EventArgs e)
      {
         leadShow = ((RibbonButton)sender).Tag.ToString();
         Execute_Query();
      }
      #endregion
   }
}
