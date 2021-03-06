﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace System.RoboTech.ExtCode
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

      #region CatchFinalyExtensaion
      public static CatchWrapper<T> Catch<T>(this TryWrapper<T> wrapper, Action<Exception> response)
      {
         if (wrapper.Exception == null) return wrapper as CatchWrapper<T>;

         response.Invoke(wrapper);
         wrapper.Exception = null;

         return wrapper as CatchWrapper<T>;
      }

      public static TryWrapper<T> Finally<T>(this TryWrapper<T> wrapper, Action<T> response)
      {
         response.Invoke(wrapper);

         return wrapper;
      }

      public static TryWrapper<T> Finally<T>(this TryWrapper<T> wrapper, Func<T> response)
      {
         wrapper.Result = response.Invoke();

         return wrapper;
      }

      public static TryWrapper<T> Finally<T>(this TryWrapper<T> wrapper, Action response)
      {
         response.Invoke();

         return wrapper;
      }
      #endregion

      #region XML Operation
      public static XDocument ToXml(this DataSet ds)
      {
         using (var memoryStream = new MemoryStream())
         {
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
               var xmlSerializer = new XmlSerializer(typeof(DataSet));
               xmlSerializer.Serialize(streamWriter, ds);
               return XDocument.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
            }
         }
      }
      #endregion
   }

   public class TryExtension
   {
      #region Try Catch Finaly Inline
      public static TryWrapper<T> Try<T>(Func<T> func)
      {
         var product = new TryWrapper<T>();

         try
         {
            product.Result = func.Invoke();
         }
         catch (Exception e)
         {
            product.Exception = e;
         }

         return product;
      }

      public static TryWrapper<T> Try<T>(Action action)
      {
         var product = new TryWrapper<T>();

         try
         {
            action.Invoke();
         }
         catch (Exception e)
         {
            product.Exception = e;
         }

         return product;
      }
      #endregion
   }
}
