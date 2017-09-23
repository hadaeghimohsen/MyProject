using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProject.Commons.ErrorHandling.Ui
{
   public partial class ErrorHandle : UserControl
   {
      public ErrorHandle()
      {
         InitializeComponent();
      }

      private Action AfterClose;
   }
}
