using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace System.CRM.ExceptionHandlings
{
   public static class ExceptionLogs
   {
      public static void SaveException(this CRM.Data.iCRMDataContext iCRM, Exception e)
      {
         //MessageBox.Show(e.Message);
      }
   }
}
