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
using System.Drawing.Imaging;

namespace System.CRM.Ui.Campaign
{
   public partial class SHW_CAMP_F : UserControl
   {
      public SHW_CAMP_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private long? mkltcode;
      private XElement xinput;

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
            CampBs.DataSource = iCRM.Campaigns.Where(c => (mkltcode == null || c.Marketing_List_Campaigns.Any(mc => mc.MKLT_MLID == mkltcode)));
               
            requery = false;
         }
         catch { }         
      }

      #region Menu
      private void New_Butn_Click(object sender, EventArgs e)
      {
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
                        (mkltcode != null ? new XAttribute("mkltcode", mkltcode) : null)
                     )
                }
              });
         _DefaultGateway.Gateway(_InteractWithCRM);
      }

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var camp = CampBs.Current as Data.Campaign;
            if (camp == null) return;

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
                           new XAttribute("campcode", camp.CMID)
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
         Camp_Gv.OptionsFind.AlwaysVisible = !Camp_Gv.OptionsFind.AlwaysVisible;
      }
      #endregion
   }
}
