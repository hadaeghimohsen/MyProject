using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   public partial class WHR_SCON_F : UserControl
   {
      public WHR_SCON_F()
      {
         InitializeComponent();
      }

      private object criticalSection = null;

      private XDocument xRunReport { get; set; }

      private XDocument xReportFileContent = null;
      private XDocument XReportFileContent {
         get{
            return xReportFileContent;
         }
         set{
            xReportFileContent = value;
            criticalSection = resultQuery;
            lock (criticalSection)
            {
               switch (resultQuery && xReportFileContent != null)
               {
                  case true:
                     /* برای اینکه فقط یک بار این عمل انجام شود */
                     resultQuery = false;
                     CheckColumnsValidation();
                     break;
               }
            }
         }
      }

      private bool resultQuery = false;
      private bool ResultQuery {
         get { return resultQuery; }
         set {
            resultQuery = value;
            criticalSection = resultQuery;
            lock (criticalSection)
            {
               switch (resultQuery && xReportFileContent != null)
               {
                  case true:
                     /* برای اینکه فقط یک بار این عمل انجام شود */
                     resultQuery = false;
                     CheckColumnsValidation();
                     break;
               }
            }
         }
      }

      partial void bp_flow_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e);
   }
}
