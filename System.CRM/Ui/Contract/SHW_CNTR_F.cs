﻿using System;
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

namespace System.CRM.Ui.Contract
{
   public partial class SHW_CNTR_F : UserControl
   {
      public SHW_CNTR_F()
      {
         InitializeComponent();
      }

      private string onoftag;
      private long compcode = 0;
      private string contractShow = "2";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);         
         switch (contractShow)
         {
            case "1":
               ContBs.List.Clear();
               break;
            case "2":
               ContBs.DataSource =
                  iCRM.Contracts.Where(
                     c => (c.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (c.COMP_CODE ?? 0))
                       && c.Request_Row.Request.RQST_STAT != "003"
                  //&& c.Request_Row.Request.SSTT_CODE == 1
                     );
               break;
            case "3":
               ContBs.DataSource =
                  iCRM.Contracts.Where(
                     c => (c.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (c.COMP_CODE ?? 0))
                       && c.Request_Row.Request.RQST_STAT != "003"
                       && c.STAT == "016"
                  //&& c.Request_Row.Request.SSTT_CODE == 1
                     );
               break;
            case "4":
               ContBs.DataSource =
                  iCRM.Contracts.Where(
                     c => (c.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (c.COMP_CODE ?? 0))
                       && c.Request_Row.Request.RQST_STAT != "003"
                       && c.Job_Personnel.USER_NAME == CurrentUser
                       //&& c.Request_Row.Request.SSTT_CODE == 1
                     );
               break;
            case "5":               
               ContBs.DataSource =
                  iCRM.Contracts.Where(
                     c => (c.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (c.COMP_CODE ?? 0))
                       && c.Request_Row.Request.RQST_STAT != "003"
                       //&& c.Request_Row.Request.SSTT_CODE == 1
                       && (c.STAT == "007" || c.STAT == "015" || c.STAT == "017" || c.STAT == "018")
                       && (c.END_DATE.Value.Date < DateTime.Now.Date)
                     );
               break;
            case "6":
               ContBs.DataSource =
                  iCRM.Contracts.Where(
                     c => (c.COMP_CODE ?? 0) == (compcode != 0 ? compcode : (c.COMP_CODE ?? 0))
                       && c.Request_Row.Request.RQST_STAT != "003"
                        //&& c.Request_Row.Request.SSTT_CODE == 1
                       && (c.STAT == "007")
                     );
               break;
         }         
      }


      #region Mouse Click
      private void Contract_Gv_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            var cont = ContBs.Current as Data.Contract;
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 108 /* Execute Inf_Cntr_F */),
                   new Job(SendType.SelfToUserInterface, "INF_CNTR_F", 10 /* Execute Actn_Calf_F */)
                   {
                      Input = 
                        new XElement("Case",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("type", "newcontractupdate"),
                           new XAttribute("rqid", cont.RQRO_RQST_RQID),
                           new XAttribute("formtype", cont.Request_Row.Request.RQST_STAT == "002" ? "showhistory" : "normal")
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
                new Job(SendType.Self, 108 /* Execute Inf_Cntr_F */),
                new Job(SendType.SelfToUserInterface, "INF_CNTR_F", 10 /* Execute Actn_Calf_F */)
                {
                   Input = 
                     new XElement("Contract",
                        new XAttribute("formcaller", GetType().Name),
                        new XAttribute("type", "newcontract")
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         Contract_Gv_DoubleClick(null, null);
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
         Contract_Gv.OptionsFind.AlwaysVisible = !Contract_Gv.OptionsFind.AlwaysVisible;
      }

      private void ContractShows_Btn_Click(object sender, EventArgs e)
      {
         contractShow = ((RibbonButton)sender).Tag.ToString();
         Execute_Query();
      }
      #endregion
   }
}
