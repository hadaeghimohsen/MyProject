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

namespace System.CRM.Ui.CampaignQuick
{
   public partial class SHW_CAMQ_F : UserControl
   {
      public SHW_CAMQ_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private XElement xinput;
      private long? mkltcode;

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
            CamqBs.DataSource = iCRM.Campaign_Quicks.Where(cq => (mkltcode == null || cq.MKLT_MLID == mkltcode));
               
            requery = false;
         }
         catch { }         
      }

      #region Menu
      private void New_Butn_Click(object sender, EventArgs e)
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

      private void Edit_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var camq = CamqBs.Current as Data.Campaign_Quick;
            if (camq == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 99 /* Execute Inf_Camq_F */),
                   new Job(SendType.SelfToUserInterface, "INF_CAMQ_F", 10 /* Execute Actn_Calf_F */)
                   {
                      Input = 
                        new XElement("Campaign",
                           new XAttribute("formcaller", GetType().Name),
                           new XAttribute("camqcode", camq.QCID),
                           new XAttribute("mkltcode", camq.MKLT_MLID)
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
