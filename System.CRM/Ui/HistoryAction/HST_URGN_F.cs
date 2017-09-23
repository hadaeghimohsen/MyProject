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
using DevExpress.XtraEditors;
using System.MaxUi;

namespace System.CRM.Ui.HistoryAction
{
   public partial class HST_URGN_F : UserControl
   {
      public HST_URGN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formCaller = "";

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);

         RegnBs1.DataSource =
            iCRM.Regions.Distinct();

         requery = false;
      }

      private void Approve_Butn_Click(object sender, EventArgs e)
      {
         var Qxml =
            new XElement("Regions",
               new XAttribute("cont", Region_Gv.SelectedRowsCount),
               new XAttribute("type", "region")               
            );
         Region_Gv.GetSelectedRows().Select(rg => (Data.Region)Region_Gv.GetRow(rg)).ToList()
            .ForEach(rg => 
               Qxml.Add(
                  new XElement("Region",
                     new XAttribute("code", rg.CODE),
                     new XAttribute("prvncode", rg.PRVN_CODE),
                     new XAttribute("cntycode", rg.PRVN_CNTY_CODE)
                  )
               )
            );

         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.SelfToUserInterface, formCaller, 100 /* Execute SetFilterOnQuery */)
                  {
                     Input = Qxml
                  },
                  new Job(SendType.SelfToUserInterface, GetType().Name, 00 /* Execute ProcessCmdKey */){Input = Keys.Escape}
               }
            )
         );         
      }
   }
}
