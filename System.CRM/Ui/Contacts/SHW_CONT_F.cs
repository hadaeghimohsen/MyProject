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
using C1.Win.C1Ribbon;

namespace System.CRM.Ui.Contacts
{
   public partial class SHW_CONT_F : UserControl
   {
      public SHW_CONT_F()
      {
         InitializeComponent();
      }

      private string contactShow = "5";
      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private async void Execute_Query()
      {
         try
         {
            DateTime StrtDate = DateTime.Now, EndDate = DateTime.Now;
            object servData = null;

            if (contactShow == "4" || contactShow == "5")
            {
               servData = await Task.Run(() =>
               {
                  using (var ctx = new Data.iCRMDataContext(ConnectionString))
                  {
                     if (contactShow == "4")
                        return (object)ctx.Services.Where(s => s.CONF_STAT == "002" && s.ONOF_TAG_DNRM.CompareTo("100") <= 0).ToList();
                     else
                        return (object)ctx.Services.Where(s => s.CONF_STAT == "002" && s.ONOF_TAG_DNRM.CompareTo("101") >= 0).ToList();
                  }
               });
            }

            iCRM = new Data.iCRMDataContext(ConnectionString);
            switch (contactShow)
            {
               case "1":
                  ServBs.List.Clear();
                  break;
               case "2":
                  ServBs.List.Clear();
                  break;
               case "3":
                  ServBs.List.Clear();
                  break;
               case "4":
                  ServBs.DataSource = servData;
                  break;
               case "5":
                  ServBs.DataSource = servData;
                  break;
               default:
                  break;
            }
            //ServBs.DataSource = iCRM.Services.Where(s => s.CONF_STAT == "002");
         }
         catch (Exception exc) { }
      }

      private void Serv_Gv_DoubleClick(object sender, EventArgs e)
      {
         try
         {
            var serv = ServBs.Current as Data.Service;
            if (serv == null) return;

            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                 {                  
                   new Job(SendType.Self, 34 /* Execute Inf_Cont_F */),                
                   new Job(SendType.SelfToUserInterface, "INF_CONT_F", 10 /* Execute ACTN_CALF_P */){Input = new XElement("Service", new XAttribute("fileno", serv.FILE_NO))},
                 });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
         catch { }
      }

      private void ContactShow_Btn_Click(object sender, EventArgs e)
      {
         contactShow = ((RibbonButton)sender).Tag.ToString();
         Execute_Query();
      }
   }
}
