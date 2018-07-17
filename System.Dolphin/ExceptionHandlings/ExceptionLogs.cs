using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace System.Dolphin.ExceptionHandlings
{
   public static class ExceptionLogs
   {
      public static void SaveException(this Dolphin.Data.iDolphinDataContext iDolphin, Exception e)
      {
         //MessageBox.Show(e.Message);
      }
   }
}
