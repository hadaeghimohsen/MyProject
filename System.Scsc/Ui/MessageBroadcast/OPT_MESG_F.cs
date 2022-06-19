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
using System.Globalization;
using DevExpress.XtraEditors;

namespace System.Scsc.Ui.MessageBroadcast
{
   public partial class OPT_MESG_F : UserControl
   {
      public OPT_MESG_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query(bool runAllQuery)
      {
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);
         }
         catch (Exception exc) { MessageBox.Show(exc.Message); }
      }
   }
}
