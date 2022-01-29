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
using System.IO;
using System.MaxUi;

namespace System.Scsc.Ui.AggregateOperation
{
   public partial class AOP_INSR_F : UserControl
   {
      public AOP_INSR_F()
      {
         InitializeComponent();
      }

      //private bool requery = false;
      private string attnsystype = "002";

      private void Execute_Query()
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
            
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }      
   }
}
