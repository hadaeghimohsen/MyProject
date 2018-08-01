using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Scsc.ExtCode
{
   public static class Extensions
   {
      public static bool In<T>(this T item, params T[] items)
      {
         if (items == null)
            return false;

         return items.Contains<T>(item);
      }

      public static bool NotIn<T>(this T item, params T[] items)
      {
         if (items == null)
            return true;
         
         return !items.Contains<T>(item);
      }

      public static bool Like(this string toSearch, string toFind)
      {
         return new Regex(@"\A" + new Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", RegexOptions.Singleline).IsMatch(toSearch);
      }
   }
}
