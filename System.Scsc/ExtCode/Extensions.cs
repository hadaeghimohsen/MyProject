using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
      #region Sql Operation
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

      public static bool IsBetween<T>(this T item, T start, T end)
      {
         return Comparer<T>.Default.Compare(item, start) >= 0
             && Comparer<T>.Default.Compare(item, end) <= 0;
      }
      #endregion

      #region Convert To Intx
      public static Int16 ToInt16(this string strNum)
      {
         return Int16.Parse(strNum);
      }

      public static Int32 ToInt32(this string strNum)
      {
         return Int32.Parse(strNum);
      }

      public static Int64 ToInt64(this string strNum)
      {
         return Int64.Parse(strNum);
      }
      #endregion

      #region Number To String Farsi
      private static string[] yakan = new string[10] { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
      private static string[] dahgan = new string[10] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
      private static string[] dahyek = new string[10] { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
      private static string[] sadgan = new string[10] { "", "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
      private static string[] basex = new string[5] { "", "هزار", "میلیون", "میلیارد", "تریلیون" };
      private static string getnum3(int num3)
      {
         string s = "";
         int d3, d12;
         d12 = num3 % 100;
         d3 = num3 / 100;
         if (d3 != 0)
            s = sadgan[d3] + " و ";
         if ((d12 >= 10) && (d12 <= 19))
         {
            s = s + dahyek[d12 - 10];
         }
         else
         {
            int d2 = d12 / 10;
            if (d2 != 0)
               s = s + dahgan[d2] + " و ";
            int d1 = d12 % 10;
            if (d1 != 0)
               s = s + yakan[d1] + " و ";
            s = s.Substring(0, s.Length - 3);
         };
         return s;
      }
      public static string Num2Str(this string snum)
      {
         string stotal = "";
         if (snum == "") return "صفر";
         if (snum == "0")
         {
            return yakan[0];
         }
         else
         {
            snum = snum.PadLeft(((snum.Length - 1) / 3 + 1) * 3, '0');
            int L = snum.Length / 3 - 1;
            for (int i = 0; i <= L; i++)
            {
               int b = int.Parse(snum.Substring(i * 3, 3));
               if (b != 0)
                  stotal = stotal + getnum3(b) + " " + basex[L - i] + " و ";
            }
            stotal = stotal.Substring(0, stotal.Length - 3);
         }
         return stotal;
      }
      #endregion      

      #region Persian Date
      public static string GetPersianDate(this DateTime? datetime)
      {
         PersianCalendar pc = new PersianCalendar();
         return
            string.Format("{0}/{1}/{2}",
               pc.GetYear((DateTime)datetime),
               pc.GetMonth((DateTime)datetime),
               pc.GetDayOfMonth((DateTime)datetime));
      }
      #endregion
   }
}
