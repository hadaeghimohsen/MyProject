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
      #region Exception Handling
      public static void SaveException(this Scsc.Data.iScscDataContext iScsc, Exception e)
      {
         //MessageBox.Show(e.Message);
      }
      #endregion

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
         return ((strNum == null || strNum == "") ? (short)0 : Int16.Parse(strNum));
      }

      public static Int32 ToInt32(this string strNum)
      {
         return ((strNum == null || strNum == "") ? 0 : Int32.Parse(strNum));
      }

      public static Int64 ToInt64(this string strNum)
      {
         return ((strNum == null || strNum == "") ? 0 : Int64.Parse(strNum));
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

      #region DragScrolling
      // =====================================================
      // ✅ Enable Drag Scroll For FlowLayoutPanel (VS2013)
      // =====================================================
      public static void EnableDragScroll(this FlowLayoutPanel panel)
      {
         panel.AutoScroll = true;

         DragScrollHandler handler = new DragScrollHandler(panel);
         handler.Attach(panel);
      }

      // =====================================================
      // ✅ Helper Class (State Holder)
      // =====================================================
      private class DragScrollHandler
      {
         private bool dragging = false;

         private Point startMouse;
         private Point startScroll;

         private FlowLayoutPanel panel;

         public DragScrollHandler(FlowLayoutPanel pnl)
         {
            panel = pnl;
         }

         // =====================================================
         // ✅ Recursive Attach (All Children + Nested)
         // =====================================================
         public void Attach(Control ctrl)
         {
            ctrl.MouseDown += Ctrl_MouseDown;
            ctrl.MouseMove += Ctrl_MouseMove;
            ctrl.MouseUp += Ctrl_MouseUp;

            ctrl.ControlAdded += Ctrl_ControlAdded;

            foreach (Control child in ctrl.Controls)
               Attach(child);
         }

         // اگر کنترل جدید اضافه شد
         void Ctrl_ControlAdded(object sender, ControlEventArgs e)
         {
            Attach(e.Control);
         }

         // =====================================================
         // ✅ MouseDown
         // =====================================================
         void Ctrl_MouseDown(object sender, MouseEventArgs e)
         {
            if (e.Button != MouseButtons.Left)
               return;

            Control clicked = sender as Control;

            // ❌ اگر کنترل Input بود Drag نکن
            if (clicked.IsInputControl())
               return;

            // ❌ اگر کنترل زیر موس Scroll داشت Drag نکن
            if (clicked.HasScrollBar())
               return;

            dragging = true;

            startMouse = Cursor.Position;
            startScroll = panel.AutoScrollPosition;

            panel.Cursor = Cursors.SizeAll; // بهتر از Hand
         }

         // =====================================================
         // ✅ MouseMove
         // =====================================================
         void Ctrl_MouseMove(object sender, MouseEventArgs e)
         {
            if (!dragging)
               return;

            Point currentMouse = Cursor.Position;

            int dx = currentMouse.X - startMouse.X;
            int dy = currentMouse.Y - startMouse.Y;

            panel.AutoScrollPosition = new Point(
                -(startScroll.X + dx),
                -(startScroll.Y + dy)
            );
         }

         // =====================================================
         // ✅ MouseUp
         // =====================================================
         void Ctrl_MouseUp(object sender, MouseEventArgs e)
         {
            dragging = false;
            panel.Cursor = Cursors.Default;
         }
      }

      // =====================================================
      // ✅ Detect Input Controls (Typing Controls)
      // =====================================================
      public static bool IsInputControl(this Control c)
      {
         return (c is TextBox)
             || (c is ComboBox)
             || (c is NumericUpDown)
             || (c is RichTextBox)
             || (c is MaskedTextBox);
      }

      // =====================================================
      // ✅ Detect Scrollable Controls (Very Important)
      // =====================================================
      public static bool HasScrollBar(this Control c)
      {
         if (c == null)
            return false;

         // DataGridView همیشه Scroll دارد
         if (c is DataGridView)
            return true;

         // ListBox / TreeView
         if (c is ListBox || c is TreeView)
            return true;

         if (c is DevExpress.XtraGrid.Scrolling.HCrkScrollBar || c is DevExpress.XtraGrid.Scrolling.VCrkScrollBar)
            return true;

         // اگر ScrollableControl بود
         ScrollableControl sc = c as ScrollableControl;
         if (sc != null)
         {
            // اگر AutoScroll روشن است
            if (sc.AutoScroll)
               return true;

            // اگر ScrollBar Visible است
            if (sc.VerticalScroll.Visible)
               return true;

            if (sc.HorizontalScroll.Visible)
               return true;
         }

         return false;
      }
      // =====================================================
      // ✅ Enable Drag Scroll For FlowLayoutPanel (VS2013)
      // =====================================================
      /*public static void EnableDragScroll(this FlowLayoutPanel panel)
      {
         panel.AutoScroll = true;

         DragScrollHandler handler = new DragScrollHandler(panel);
         handler.Attach(panel);
      }

      // =====================================================
      // ✅ Helper Class (State Holder)
      // =====================================================
      private class DragScrollHandler
      {
         private bool dragging = false;
         private Point startMouse;
         private Point startScroll;

         private FlowLayoutPanel panel;

         public DragScrollHandler(FlowLayoutPanel pnl)
         {
            panel = pnl;
         }

         // Attach Recursive
         public void Attach(Control ctrl)
         {
            ctrl.MouseDown += Ctrl_MouseDown;
            ctrl.MouseMove += Ctrl_MouseMove;
            ctrl.MouseUp += Ctrl_MouseUp;

            ctrl.ControlAdded += Ctrl_ControlAdded;

            foreach (Control child in ctrl.Controls)
            {
               Attach(child);
            }
         }

         // اگر کنترل جدید اضافه شد
         void Ctrl_ControlAdded(object sender, ControlEventArgs e)
         {
            Attach(e.Control);
         }

         // MouseDown
         void Ctrl_MouseDown(object sender, MouseEventArgs e)
         {
            if (e.Button != MouseButtons.Left)
               return;

            Control c = sender as Control;

            // ❌ روی Input ها Drag نکن
            if (IsInputControl(c))
               return;

            dragging = true;

            startMouse = Cursor.Position;
            startScroll = panel.AutoScrollPosition;

            panel.Cursor = Cursors.Hand;
         }

         // MouseMove
         void Ctrl_MouseMove(object sender, MouseEventArgs e)
         {
            if (!dragging)
               return;

            Point currentMouse = Cursor.Position;

            int dx = currentMouse.X - startMouse.X;
            int dy = currentMouse.Y - startMouse.Y;

            panel.AutoScrollPosition = new Point(
                -(startScroll.X + dx),
                -(startScroll.Y + dy)
            );
         }

         // MouseUp
         void Ctrl_MouseUp(object sender, MouseEventArgs e)
         {
            dragging = false;
            panel.Cursor = Cursors.Default;
         }

         // کنترل‌هایی که Drag نباید فعال کنه
         private bool IsInputControl(Control c)
         {
            return (c is TextBox)
                || (c is ComboBox)
                || (c is NumericUpDown)
                || (c is RichTextBox);
         }
      }*/

      // =====================================================
      // ✅ Enable Drag Scroll with Smooth Scroll / Inertia
      // =====================================================
      /*public static void EnableDragScrollSmooth(this FlowLayoutPanel panel)
      {
         panel.AutoScroll = true;

         var handler = new DragScrollHandler(panel);
         handler.Attach(panel);
      }

      // =====================================================
      // ✅ Internal Class to Hold State
      // =====================================================
      private class DragScrollHandler
      {
         private FlowLayoutPanel panel;
         private bool dragging = false;
         private Point startMouse;
         private Point startScroll;

         private Point lastDelta; // سرعت حرکت
         private Timer inertiaTimer;

         public DragScrollHandler(FlowLayoutPanel pnl)
         {
            panel = pnl;

            inertiaTimer = new Timer();
            inertiaTimer.Interval = 60; // تقریباً 60fps
            inertiaTimer.Tick += InertiaTimer_Tick;
         }

         public void Attach(Control ctrl)
         {
            ctrl.MouseDown += Ctrl_MouseDown;
            ctrl.MouseMove += Ctrl_MouseMove;
            ctrl.MouseUp += Ctrl_MouseUp;
            ctrl.ControlAdded += Ctrl_ControlAdded;

            foreach (Control child in ctrl.Controls)
               Attach(child);
         }

         private void Ctrl_ControlAdded(object sender, ControlEventArgs e)
         {
            Attach(e.Control);
         }

         private void Ctrl_MouseDown(object sender, MouseEventArgs e)
         {
            if (e.Button != MouseButtons.Left)
               return;

            Control c = sender as Control;

            if (IsInputControl(c))
               return;

            dragging = true;
            inertiaTimer.Stop(); // توقف Inertia قبلی

            startMouse = Cursor.Position;
            startScroll = panel.AutoScrollPosition;

            lastDelta = Point.Empty;
            panel.Cursor = Cursors.Hand;
         }

         private void Ctrl_MouseMove(object sender, MouseEventArgs e)
         {
            if (!dragging)
               return;

            Point currentMouse = Cursor.Position;
            int dx = currentMouse.X - startMouse.X;
            int dy = currentMouse.Y - startMouse.Y;

            panel.AutoScrollPosition = new Point(
                -(startScroll.X + dx),
                -(startScroll.Y + dy)
            );

            // ذخیره سرعت حرکت
            lastDelta = new Point(dx, dy);
         }

         private void Ctrl_MouseUp(object sender, MouseEventArgs e)
         {
            dragging = false;
            panel.Cursor = Cursors.Default;

            // شروع Inertia
            inertiaTimer.Start();
         }

         private void InertiaTimer_Tick(object sender, EventArgs e)
         {
            // کاهش سرعت به مرور
            lastDelta.X = (int)(lastDelta.X * 0.97);
            lastDelta.Y = (int)(lastDelta.Y * 0.97);

            if (Math.Abs(lastDelta.X) < 1 && Math.Abs(lastDelta.Y) < 1)
            {
               inertiaTimer.Stop();
               return;
            }

            Point currentScroll = panel.AutoScrollPosition;
            panel.AutoScrollPosition = new Point(
                -(currentScroll.X + lastDelta.X),
                -(currentScroll.Y + lastDelta.Y)
            );
         }

         private bool IsInputControl(Control c)
         {
            return (c is TextBox)
                || (c is ComboBox)
                || (c is NumericUpDown)
                || (c is RichTextBox);
         }
      }*/
      #endregion
   }
}
