using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.Ui.DataAnalysis
{
   public partial class DAP_DSHB_F : UserControl
   {
      public DAP_DSHB_F()
      {
         InitializeComponent();
      }

      private async void Execute_Query()
      {
         await Task.Run(() =>
         {
            using (var db = new Data.iScscDataContext(ConnectionString))
            {
               // Empty query
            }
         });

         iScsc = new Data.iScscDataContext(ConnectionString);
      }
   }
}
