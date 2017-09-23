using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;

namespace System.Reporting.ReportViewers.Ui
{
   public partial class Viewers : UserControl
   {
      public Viewers()
      {
         InitializeComponent();
         //reportDocument = new ReportDocument();
      }

      private ReportDocument reportDocument;

   }
}
