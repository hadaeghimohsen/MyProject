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
   public partial class HST_UTAG_F : UserControl
   {
      public HST_UTAG_F()
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

         TagBs1.DataSource =
            iCRM.App_Base_Defines.Where(a => a.ENTY_NAME == "TAG");

         requery = false;
      }

      private void Approve_Butn_Click(object sender, EventArgs e)
      {
         var Qxml =
            new XElement("Tags",
               new XAttribute("cont", Tag_Chkboxlst.CheckedItems.Count),
               new XAttribute("type", "tag"),
               new XAttribute("allany", Ckb_AllAny.Checked ? "ALL" : "ANY"),
               Tag_Chkboxlst.CheckedItems.OfType<Data.App_Base_Define>()
               .Select(i => 
                  new XElement("Tag",
                     new XAttribute("apbscode", i.CODE)
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
