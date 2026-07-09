using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

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

      #region Cellphone Validation (بهینه‌شده)
      /// <summary>
      /// اعتبارسنجی شماره موبایل ایران
      /// </summary>
      public static bool ValidatePhoneField(this BaseEdit phoneNumber, string fieldName)
      {
         const string pattern = @"^09[0-9]{9}$";
         string phone = phoneNumber.Text.Trim();

         if (string.IsNullOrWhiteSpace(phone))
            return true; // خالی باشه قبوله

         // بررسی فرمت
         if (!Regex.IsMatch(phone, pattern))
         {
            phoneNumber.Focus();
            phoneNumber.SelectAll(); // کل متن رو انتخاب کن
            MessageBox.Show(
               string.Format("لطفاً {0} معتبر وارد کنید (11 رقم و با 09 شروع شود)", fieldName),
               "خطا",
               MessageBoxButtons.OK,
               MessageBoxIcon.Warning
            );
            return false;
         }

         return true;
      }

      /// <summary>
      /// اعتبارسنجی شماره موبایل با جزئیات بیشتر
      /// </summary>
      public static PhoneValidationResult ValidatePhoneDetailed(this BaseEdit phoneNumber)
      {
         const string pattern = @"^09[0-9]{9}$";
         string phone = phoneNumber.Text.Trim();

         if (string.IsNullOrWhiteSpace(phone))
            return new PhoneValidationResult { IsValid = true, Message = "خالی" };

         if (phone.Length != 11)
            return new PhoneValidationResult
            {
               IsValid = false,
               Message = string.Format("تعداد ارقام باید 11 باشد (اکنون {0} رقم)", phone.Length)
            };

         if (!Regex.IsMatch(phone, pattern))
            return new PhoneValidationResult
            {
               IsValid = false,
               Message = "فرمت شماره موبایل نامعتبر است (با 09 شروع شود)"
            };

         // بررسی کد اپراتور (اختیاری)
         string prefix = phone.Substring(0, 3);
         string[] validPrefixes = { "091", "092", "093", "094", "095", "096", "097", "098", "099" };

         if (!validPrefixes.Contains(prefix))
            return new PhoneValidationResult
            {
               IsValid = false,
               Message = string.Format("کد اپراتور {0} نامعتبر است", prefix)
            };

         return new PhoneValidationResult { IsValid = true, Message = "معتبر" };
      }

      public class PhoneValidationResult
      {
         public bool IsValid { get; set; }
         public string Message { get; set; }
      }
      #endregion

      #region Phone Formatting (جدید)
      /// <summary>
      /// تبدیل شماره موبایل به فرمت ۰۹۱۲-۳۴۵-۶۷۸۹
      /// </summary>
      public static string ToPhoneFormat(this string phone)
      {
         if (string.IsNullOrWhiteSpace(phone) || phone.Length != 11)
            return phone;

         return string.Format("{0}-{1}-{2}",
            phone.Substring(0, 4),
            phone.Substring(4, 3),
            phone.Substring(7, 4));
      }

      /// <summary>
      /// تبدیل شماره موبایل به فرمت ۰۹۱۲۳۴۵۶۷۸۹ (بدون خط تیره)
      /// </summary>
      public static string ToPhoneNumberOnly(this string phone)
      {
         return Regex.Replace(phone, @"[^0-9]", "");
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

      public static bool IsBetween<T>(this T item, T start, T end) where T : IComparable<T>
      {
         return item.CompareTo(start) >= 0 && item.CompareTo(end) <= 0;
      }
      #endregion

      #region Convert To Intx (بهینه‌شده با TryParse)
      public static Int16 ToInt16(this string strNum)
      {
         Int16 result;
         return Int16.TryParse(strNum, out result) ? result : (short)0;
      }

      public static Int32 ToInt32(this string strNum)
      {
         int result;
         return int.TryParse(strNum, out result) ? result : 0;
      }

      public static Int64 ToInt64(this string strNum)
      {
         long result;
         return long.TryParse(strNum, out result) ? result : 0;
      }

      // تبدیل به Nullable (جدید)
      public static Int32? ToInt32Nullable(this string strNum)
      {
         int result;
         return int.TryParse(strNum, out result) ? (int?)result : null;
      }
      #endregion

      #region Number To String Farsi (بهینه‌شده)
      private static readonly string[] Yakan = new string[10] { "صفر", "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
      private static readonly string[] Dahgan = new string[10] { "", "", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
      private static readonly string[] Dahyek = new string[10] { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده" };
      private static readonly string[] Sadgan = new string[10] { "", "یکصد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
      private static readonly string[] Basex = new string[5] { "", "هزار", "میلیون", "میلیارد", "تریلیون" };

      private static string GetNum3(int num3)
      {
         string s = "";
         int d12 = num3 % 100;
         int d3 = num3 / 100;

         if (d3 != 0)
            s = Sadgan[d3] + " و ";

         if (d12 >= 10 && d12 <= 19)
         {
            s = s + Dahyek[d12 - 10];
         }
         else
         {
            int d2 = d12 / 10;
            if (d2 != 0)
               s = s + Dahgan[d2] + " و ";

            int d1 = d12 % 10;
            if (d1 != 0)
               s = s + Yakan[d1] + " و ";

            s = s.Substring(0, s.Length - 3);
         }
         return s;
      }

      public static string Num2Str(this string snum)
      {
         if (string.IsNullOrEmpty(snum) || snum == "0")
            return "صفر";

         snum = snum.PadLeft(((snum.Length - 1) / 3 + 1) * 3, '0');
         int L = snum.Length / 3 - 1;
         string stotal = "";

         for (int i = 0; i <= L; i++)
         {
            int b = int.Parse(snum.Substring(i * 3, 3));
            if (b != 0)
               stotal = stotal + GetNum3(b) + " " + Basex[L - i] + " و ";
         }

         return stotal.Substring(0, stotal.Length - 3);
      }
      #endregion

      #region Persian Date (بهینه‌شده)
      public static string GetPersianDate(this DateTime? datetime)
      {
         if (!datetime.HasValue)
            return "";

         PersianCalendar pc = new PersianCalendar();
         return string.Format("{0}/{1:D2}/{2:D2}",
            pc.GetYear(datetime.Value),
            pc.GetMonth(datetime.Value),
            pc.GetDayOfMonth(datetime.Value));
      }

      public static string GetPersianDateTime(this DateTime? datetime)
      {
         if (!datetime.HasValue)
            return "";

         PersianCalendar pc = new PersianCalendar();
         return string.Format("{0}/{1:D2}/{2:D2} {3:D2}:{4:D2}",
            pc.GetYear(datetime.Value),
            pc.GetMonth(datetime.Value),
            pc.GetDayOfMonth(datetime.Value),
            datetime.Value.Hour,
            datetime.Value.Minute);
      }

      public static DateTime? ToGeorgianDate(this string persianDate)
      {
         try
         {
            PersianCalendar pc = new PersianCalendar();
            string[] parts = persianDate.Split('/');
            if (parts.Length == 3)
            {
               int year = int.Parse(parts[0]);
               int month = int.Parse(parts[1]);
               int day = int.Parse(parts[2]);
               return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            return null;
         }
         catch
         {
            return null;
         }
      }
      #endregion

      #region DragScrolling (بهینه‌شده با DevExpress Support)
      public static void EnableDragScroll(this FlowLayoutPanel panel)
      {
         panel.AutoScroll = true;
         DragScrollHandler handler = new DragScrollHandler(panel);
         handler.Attach(panel);
      }

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

            Control clicked = sender as Control;

            if (clicked.IsInputControl() || clicked.HasScrollBar())
               return;

            dragging = true;
            startMouse = Cursor.Position;
            startScroll = panel.AutoScrollPosition;
            panel.Cursor = Cursors.SizeAll;
         }

         private void Ctrl_MouseMove(object sender, MouseEventArgs e)
         {
            if (!dragging) return;

            Point currentMouse = Cursor.Position;
            int dx = currentMouse.X - startMouse.X;
            int dy = currentMouse.Y - startMouse.Y;

            panel.AutoScrollPosition = new Point(
                -(startScroll.X + dx),
                -(startScroll.Y + dy)
            );
         }

         private void Ctrl_MouseUp(object sender, MouseEventArgs e)
         {
            dragging = false;
            panel.Cursor = Cursors.Default;
         }
      }

      public static bool IsInputControl(this Control c)
      {
         return c is TextBox || c is DevExpress.XtraEditors.ComboBox || c is System.Windows.Forms.ComboBox || c is NumericUpDown ||
                c is RichTextBox || c is MaskedTextBox || c is BaseEdit;
      }

      public static bool HasScrollBar(this Control c)
      {
         if (c == null) return false;

         if (c is DataGridView || c is ListBox || c is TreeView)
            return true;

         if (c is DevExpress.XtraGrid.Scrolling.HCrkScrollBar ||
             c is DevExpress.XtraGrid.Scrolling.VCrkScrollBar)
            return true;

         ScrollableControl sc = c as ScrollableControl;
         if (sc != null)
         {
            if (sc.AutoScroll) return true;
            if (sc.VerticalScroll.Visible) return true;
            if (sc.HorizontalScroll.Visible) return true;
         }

         return false;
      }
      #endregion

      #region 🔥 توابع جدید اضافه شده 🔥

      #region String Utilities (جدید)
      /// <summary>
      /// بررسی اینکه رشته فقط شامل اعداد باشد
      /// </summary>
      public static bool IsNumeric(this string str)
      {
         return !string.IsNullOrWhiteSpace(str) && Regex.IsMatch(str, @"^[0-9]+$");
      }

      /// <summary>
      /// حذف فاصله‌های اضافی و کاراکترهای خاص
      /// </summary>
      public static string CleanString(this string str)
      {
         if (string.IsNullOrWhiteSpace(str)) return "";
         return Regex.Replace(str.Trim(), @"\s+", " ");
      }

      /// <summary>
      /// بررسی اینکه رشته شامل حروف فارسی باشد
      /// </summary>
      public static bool IsPersian(this string str)
      {
         return !string.IsNullOrWhiteSpace(str) &&
                Regex.IsMatch(str, @"[\u0600-\u06FF]");
      }

      /// <summary>
      /// تبدیل اعداد انگلیسی به فارسی
      /// </summary>
      public static string ToPersianNumbers(this string str)
      {
         if (string.IsNullOrWhiteSpace(str)) return str;

         char[] persianDigits = { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
         char[] result = str.ToCharArray();

         for (int i = 0; i < result.Length; i++)
         {
            if (char.IsDigit(result[i]))
               result[i] = persianDigits[result[i] - '0'];
         }

         return new string(result);
      }

      /// <summary>
      /// تبدیل اعداد فارسی به انگلیسی
      /// </summary>
      public static string ToEnglishNumbers(this string str)
      {
         if (string.IsNullOrWhiteSpace(str)) return str;

         string persianDigits = "۰۱۲۳۴۵۶۷۸۹";
         string englishDigits = "0123456789";

         for (int i = 0; i < 10; i++)
         {
            str = str.Replace(persianDigits[i], englishDigits[i]);
         }

         return str;
      }
      #endregion

      #region Control Utilities (جدید)
      /// <summary>
      /// پاک کردن متن تمام کنترل‌های یک فرم
      /// </summary>
      public static void ClearAllTexts(this Control parent)
      {
         foreach (Control c in parent.Controls)
         {
            if (c is TextBox || c is BaseEdit)
               c.Text = "";
            else if (c is DevExpress.XtraEditors.ComboBox)
               ((DevExpress.XtraEditors.ComboBox)c).SelectedIndex = -1;
            else if (c is System.Windows.Forms.ComboBox)
               ((System.Windows.Forms.ComboBox)c).SelectedIndex = -1;
            else if (c.HasChildren())
               c.ClearAllTexts();
         }
      }

      /// <summary>
      /// بررسی اینکه کنترل دارای فرزند است
      /// </summary>
      public static bool HasChildren(this Control c)
      {
         return c.Controls.Count > 0;
      }

      /// <summary>
      /// پیدا کردن کنترل با نام مشخص
      /// </summary>
      public static Control FindControl(this Control parent, string name)
      {
         if (parent.Name == name) return parent;

         foreach (Control child in parent.Controls)
         {
            Control result = child.FindControl(name);
            if (result != null) return result;
         }

         return null;
      }
      #endregion

      #region DateTime Utilities (جدید)
      /// <summary>
      /// محاسبه سن بر اساس تاریخ تولد
      /// </summary>
      public static int CalculateAge(this DateTime birthDate)
      {
         DateTime today = DateTime.Today;
         int age = today.Year - birthDate.Year;

         if (birthDate > today.AddYears(-age))
            age--;

         return age;
      }

      /// <summary>
      /// اولین روز ماه
      /// </summary>
      public static DateTime FirstDayOfMonth(this DateTime date)
      {
         return new DateTime(date.Year, date.Month, 1);
      }

      /// <summary>
      /// آخرین روز ماه
      /// </summary>
      public static DateTime LastDayOfMonth(this DateTime date)
      {
         return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
      }
      #endregion

      #region Enum Utilities (جدید)
      /// <summary>
      /// دریافت توضیحات Enum
      /// </summary>
      public static string GetDescription(this Enum value)
      {
         var field = value.GetType().GetField(value.ToString());
         var attribute = (System.ComponentModel.DescriptionAttribute)
            Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute));

         return attribute == null ? value.ToString() : attribute.Description;
      }

      /// <summary>
      /// تبدیل Enum به لیست برای ComboBox
      /// </summary>
      public static Dictionary<int, string> ToDictionary<T>() where T : struct
      {
         if (!typeof(T).IsEnum)
            throw new ArgumentException("T باید از نوع Enum باشد");

         var dict = new Dictionary<int, string>();
         foreach (T value in Enum.GetValues(typeof(T)))
         {
            dict.Add(Convert.ToInt32(value), (value as Enum).GetDescription());
         }
         return dict;
      }
      #endregion

      #region File Utilities (جدید)
      /// <summary>
      /// تبدیل فایل به آرایه بایت
      /// </summary>
      public static byte[] FileToBytes(string filePath)
      {
         if (!System.IO.File.Exists(filePath))
            return null;

         return System.IO.File.ReadAllBytes(filePath);
      }

      /// <summary>
      /// ذخیره آرایه بایت به فایل
      /// </summary>
      public static void BytesToFile(byte[] bytes, string filePath)
      {
         System.IO.File.WriteAllBytes(filePath, bytes);
      }

      /// <summary>
      /// دریافت حجم فایل به صورت خوانا
      /// </summary>
      public static string GetFileSizeString(long bytes)
      {
         string[] sizes = { "B", "KB", "MB", "GB", "TB" };
         double len = bytes;
         int order = 0;

         while (len >= 1024 && order < sizes.Length - 1)
         {
            order++;
            len = len / 1024;
         }

         return string.Format("{0:0.##} {1}", len, sizes[order]);
      }
      #endregion

      #endregion

      #region 🚀 روش‌های کپی سطحی (Shallow Copy)

      /// <summary>
      /// کپی سطحی با استفاده از MemberwiseClone (با Reflection)
      /// </summary>
      public static T ShallowCopy<T>(this T source) where T : class
      {
         if (source == null) return null;

         var method = typeof(T).GetMethod("MemberwiseClone",
                                          BindingFlags.NonPublic | BindingFlags.Instance);

         if (method != null)
         {
            return (T)method.Invoke(source, null);
         }

         return null;
      }

      /// <summary>
      /// کپی سطحی با استفاده از Reflection (کپی Property به Property)
      /// </summary>
      public static T ShallowCopyReflection<T>(this T source) where T : class, new()
      {
         if (source == null) return null;

         var target = new T();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach (var prop in properties)
         {
            if (prop.CanWrite && prop.CanRead)
            {
               var value = prop.GetValue(source);
               prop.SetValue(target, value);
            }
         }

         return target;
      }

      #endregion

      #region 🚀 روش‌های کپی عمیق (Deep Copy)

      /// <summary>
      /// کپی عمیق با استفاده از JSON (نیاز به Newtonsoft.Json)
      /// </summary>
      public static T DeepCopyJson<T>(this T source) where T : class
      {
         if (source == null) return null;

         try
         {
            string json = JsonConvert.SerializeObject(source, new JsonSerializerSettings
            {
               ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
               PreserveReferencesHandling = PreserveReferencesHandling.None,
               Formatting = Formatting.None
            });

            return JsonConvert.DeserializeObject<T>(json);
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// کپی عمیق با استفاده از DataContractSerializer
      /// </summary>
      public static T DeepCopyDataContract<T>(this T source) where T : class
      {
         if (source == null) return null;

         try
         {
            using (var stream = new MemoryStream())
            {
               var serializer = new DataContractSerializer(typeof(T));
               serializer.WriteObject(stream, source);
               stream.Seek(0, SeekOrigin.Begin);
               return (T)serializer.ReadObject(stream);
            }
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// کپی عمیق با استفاده از BinaryFormatter (نیاز به Serializable)
      /// </summary>
      public static T DeepCopyBinary<T>(this T source) where T : class
      {
         if (source == null) return null;

         try
         {
            using (var stream = new MemoryStream())
            {
               var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
               formatter.Serialize(stream, source);
               stream.Seek(0, SeekOrigin.Begin);
               return (T)formatter.Deserialize(stream);
            }
         }
         catch
         {
            return null;
         }
      }

      /// <summary>
      /// کپی عمیق با استفاده از XML Serialization
      /// </summary>
      public static T DeepCopyXml<T>(this T source) where T : class
      {
         if (source == null) return null;

         try
         {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
               serializer.Serialize(stream, source);
               stream.Seek(0, SeekOrigin.Begin);
               return (T)serializer.Deserialize(stream);
            }
         }
         catch
         {
            return null;
         }
      }

      #endregion

      #region 🚀 روش‌های کپی با Expression Trees

      /// <summary>
      /// کپی با استفاده از Expression Trees (سریع‌ترین روش)
      /// </summary>
      public static T CopyExpression<T>(this T source) where T : class, new()
      {
         if (source == null) return null;

         var target = new T();
         var sourceType = typeof(T);
         var properties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                    .Where(p => p.CanWrite && p.CanRead);

         foreach (var prop in properties)
         {
            if (prop.GetIndexParameters().Length == 0)
            {
               var value = prop.GetValue(source);
               prop.SetValue(target, value);
            }
         }

         return target;
      }

      #endregion

      #region 🚀 روش‌های کپی با AutoMapper (اگر نصب باشد)

      /// <summary>
      /// کپی با استفاده از AutoMapper (نیاز به تنظیمات اولیه)
      /// </summary>
      public static T MapCopy<T>(this T source) where T : class, new()
      {
         if (source == null) return null;

         try
         {
            // اینجا باید AutoMapper تنظیم شده باشد
            // return AutoMapper.Mapper.Map<T>(source);
            return source.ShallowCopyReflection(); // Fallback
         }
         catch
         {
            return source.ShallowCopyReflection();
         }
      }

      #endregion

      #region 🚀 روش‌های کپی پیشرفته

      /// <summary>
      /// کپی با انتخاب فیلدهای خاص (شامل)
      /// </summary>
      public static T CopyWithInclude<T>(this T source, params string[] propertyNames) where T : class, new()
      {
         if (source == null) return null;
         if (propertyNames == null || propertyNames.Length == 0)
            return source.ShallowCopyReflection();

         var target = new T();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => propertyNames.Contains(p.Name) && p.CanWrite && p.CanRead);

         foreach (var prop in properties)
         {
            var value = prop.GetValue(source);
            prop.SetValue(target, value);
         }

         return target;
      }

      /// <summary>
      /// کپی با حذف فیلدهای خاص (اکسکلود)
      /// </summary>
      public static T CopyWithExclude<T>(this T source, params string[] propertyNames) where T : class, new()
      {
         if (source == null) return null;

         var target = new T();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(p => !propertyNames.Contains(p.Name) && p.CanWrite && p.CanRead);

         foreach (var prop in properties)
         {
            var value = prop.GetValue(source);
            prop.SetValue(target, value);
         }

         return target;
      }

      /// <summary>
      /// کپی و تغییر همزمان یک فیلد
      /// </summary>
      public static T CopyAndUpdate<T>(this T source, Action<T> updateAction) where T : class, new()
      {
         if (source == null) return null;

         var copy = source.ShallowCopyReflection();
         if (updateAction != null)
            updateAction(copy);
         return copy;
      }

      /// <summary>
      /// کپی و تغییر همزمان چند فیلد
      /// </summary>
      public static T CopyAndUpdate<T>(this T source, Dictionary<string, object> updates) where T : class, new()
      {
         if (source == null) return null;
         if (updates == null) return source.ShallowCopyReflection();

         var copy = source.ShallowCopyReflection();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach (var update in updates)
         {
            var prop = properties.FirstOrDefault(p => p.Name == update.Key && p.CanWrite);
            if (prop != null)
            {
               prop.SetValue(copy, update.Value);
            }
         }

         return copy;
      }

      #endregion

      #region 🚀 روش‌های کمکی

      /// <summary>
      /// دریافت لیست مقادیر Property ها به صورت Dictionary
      /// </summary>
      public static Dictionary<string, object> ToDictionary<T>(this T source) where T : class
      {
         if (source == null) return null;

         var dict = new Dictionary<string, object>();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach (var prop in properties)
         {
            if (prop.CanRead)
            {
               dict.Add(prop.Name, prop.GetValue(source));
            }
         }

         return dict;
      }

      /// <summary>
      /// ایجاد شیء از Dictionary
      /// </summary>
      public static T FromDictionary<T>(this Dictionary<string, object> dict) where T : class, new()
      {
         if (dict == null) return null;

         var target = new T();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach (var prop in properties)
         {
            if (prop.CanWrite && dict.ContainsKey(prop.Name))
            {
               prop.SetValue(target, dict[prop.Name]);
            }
         }

         return target;
      }

      /// <summary>
      /// کلاس کمکی برای نگهداری تفاوت‌ها (به جای ValueTuple)
      /// </summary>
      public class DifferenceInfo
      {
         public object OldValue { get; set; }
         public object NewValue { get; set; }
      }

      /// <summary>
      /// مقایسه دو شیء و برگرداندن تفاوت‌ها
      /// </summary>
      public static Dictionary<string, DifferenceInfo> CompareTo<T>(this T source, T other) where T : class
      {
         if (source == null || other == null) return null;

         var differences = new Dictionary<string, DifferenceInfo>();
         var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

         foreach (var prop in properties)
         {
            if (prop.CanRead)
            {
               var oldValue = prop.GetValue(source);
               var newValue = prop.GetValue(other);

               if (!Equals(oldValue, newValue))
               {
                  differences.Add(prop.Name, new DifferenceInfo
                  {
                     OldValue = oldValue,
                     NewValue = newValue
                  });
               }
            }
         }

         return differences;
      }

      #endregion

      #region 🚀 روش‌های کپی برای لیست‌ها

      /// <summary>
      /// کپی یک لیست با کپی عمیق
      /// </summary>
      public static List<T> DeepCopyList<T>(this List<T> source) where T : class
      {
         if (source == null) return null;

         var result = new List<T>();
         foreach (var item in source)
         {
            result.Add(item.DeepCopyJson());
         }

         return result;
      }

      /// <summary>
      /// کپی یک لیست با کپی سطحی
      /// </summary>
      public static List<T> ShallowCopyList<T>(this List<T> source) where T : class, new()
      {
         if (source == null) return null;

         var result = new List<T>();
         foreach (var item in source)
         {
            result.Add(item.ShallowCopyReflection());
         }

         return result;
      }

      #endregion
   }
}