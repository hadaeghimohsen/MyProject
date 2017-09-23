using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui
{
   partial class Filter
   {
      public string GetUserFormulaSelection(string reportEngine)
      {
         switch (reportEngine)
         {
            case "crystalreport":
               switch (Enabled)
               {
                  case true:
                     return getEnabledFormulaSelection4CR();
                  case false:
                     return getDisabledFormulaSelection4CR();
               }
               break;
            case "jasper":
               return "";
         }
         return "";
      }

      public string GetAdminFormulaSelection(string reportEngine)
      {
         switch (reportEngine)
         {
            case "crystalreport":
               switch (HasFiltering)
	            {
                  case true:
                     return getAdminEnabledFormulaSelection4CR();
	            }
               return "";
            default:
               return "";
         }         
      }

      #region Crystal Report SAP
      private string getDisabledFormulaSelection4CR()
      {
         List<string> items = new List<string>();
         switch (GetType)
         {
            case "int":
               xData.Element("RefTable").Element("DefaultValues").Elements("Value").ToList()
                  .ForEach(v => items.Add(v.Value.Replace(",", "")));
               if (items.Count == 0)
                  return "";
               return string.Format("{0} in [{1}]",
                  xData.Attribute("formulaName").Value,
                  string.Join(", ", items));
            case "string":
               xData.Element("RefTable").Element("DefaultValues").Elements("Value").ToList()
                  .ForEach(v => items.Add(string.Format(@"""{0}""", v.Value)));
               if (items.Count == 0)
                  return "";
               return string.Format("{0} in [{1}]",
                  xData.Attribute("formulaName").Value,
                  string.Join(", ", items));               
            case "datetime":
               break;
            case "stringatetime":
               break;
            case "domain":
               /* Boolean 0 is False, 1 is True */
               xData.Element("RefTable").Element("DefaultValues").Elements("Value").ToList()
                  .ForEach(v => items.Add(string.Format(@"{0}", v.Value == "0" ? "false" : "true")));
               if (items.Count == 0)
                  return "";
               return string.Format("{0} in [{1}]",
                  xData.Attribute("formulaName").Value,
                  string.Join(", ", items));
         }
         return "";
      }

      private string getEnabledFormulaSelection4CR()
      {
         switch (GetType)
         {
            case "int":
               if (Checked)
                  return getIntFormula4CR();
               else
                  return getIntDefaultFormula4CR();
            case "string":
               if (Checked)
                  return getStringFormula4CR();
               else
                  return getStringDefaultFormula4CR();
            case "stringdatetime":
               if (Checked)
                  return getStringDateTimeFormula4CR();
               else
                  return getStringDateTimeDefaultFormula4CR();
            case "datetime":
               if (Checked)
                  return getDateTimeFormula4CR();
               else
                  return getDateTimeDefaultFormula4CR();
            case "domain":
               if (Checked)
                  return getDomainFormula4CR();
               else
                  return getDomainDefaultFormula4CR();
         }
         return "";
      }

      private string getAdminEnabledFormulaSelection4CR()
      {
         switch (GetType)
         {
            case "int":
               return getIntDefaultFormula4CR();
            case "string":
               return getStringDefaultFormula4CR();
            case "stringdatetime":
               return getStringDateTimeDefaultFormula4CR();
            case "datetime":
               return getDateTimeDefaultFormula4CR();
            case "domain":
               return getDomainDefaultFormula4CR();
         }
         return "";
      }

      private string getIntFormula4CR()
      {
         te_minvalue.Text = getChekInptVald(te_minvalue.Text.Replace(",", ""), "min");
         te_maxvalue.Text = getChekInptVald(te_maxvalue.Text.Replace(",", ""), "max");         
         switch (cbx_sourcepath.SelectedIndex)
         {
            case 0:
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return 
                        string.Format("{0} = {1}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""));
                  case 1:
                     return
                        string.Format("{0} <> {1}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""));
                  case 2:
                     return
                        string.Format("{0} > {1}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""));
                  case 3:
                     return
                        string.Format("{0} >= {1}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""));
                  case 4:
                     return
                        string.Format("{0} < {1}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""));
                  case 5:
                     return
                        string.Format("{0} <= {1}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""));
                  case 6:
                     return
                        string.Format("{0} in {1} to {2}",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""),
                        te_maxvalue.Text.Replace(",", ""));
                  case 7:
                     return
                     string.Format("NOT({0} in {1} to {2})",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text.Replace(",", ""),
                        te_maxvalue.Text.Replace(",", ""));
                  case 8:
                     return
                     string.Format("ISNULL({0})",
                        xData.Attribute("formulaName").Value);
                  case 9:
                     return
                     string.Format("NOT ISNULL({0})",
                        xData.Attribute("formulaName").Value);
	            }
               break;
            case 1:               
            case 2:
               List<string> items = new List<string>();
               cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == Windows.Forms.CheckState.Checked)
                  .ToList()
                  .ForEach(c => items.Add(c.Value.ToString().Replace(",", "")));
               if (items.Count == 0)
                  return "";
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format("{0} in [{1}]",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
                  case 1:
                     return
                        string.Format("NOT({0} in [{1}])",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
	            }
               break;
         }
         return "";
      }

      private string getIntDefaultFormula4CR()
      {
         string tmp = getChekInptVald("", "listint");
         if (tmp.Length > 1)
            return
               string.Format("{0} in [{1}]",
                  xData.Attribute("formulaName").Value,
                  tmp);
         else
         {
            string min = getChekInptVald("0", "min");
            string max = getChekInptVald(long.MaxValue.ToString(), "max");
            return
               string.Format("{0} in {1} to {2}",
                  xData.Attribute("formulaName").Value,
                  min.Length != 0 ? min.Replace("," ,"") : max.Replace("," , ""),
                  max.Length != 0 ? max.Replace(",", "") : min.Replace("," ,""));
         }
      }

      private string getStringFormula4CR()
      {
         te_minvalue.Text = getChekInptVald(te_minvalue.Text, "min");
         te_maxvalue.Text = getChekInptVald(te_maxvalue.Text, "max");
         switch (cbx_sourcepath.SelectedIndex)
         {
            case 0:
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format(@"{0} = ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 1:
                     return
                        string.Format(@"{0} <> ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 2:
                     return
                        string.Format(@"{0} > ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 3:
                     return
                        string.Format(@"{0} >= ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 4:
                     return
                        string.Format(@"{0} < ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 5:
                     return
                        string.Format(@"{0} <= ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 6:
                     return
                        string.Format(@"{0} STARTWITH ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 7:
                     return
                        string.Format(@"NOT({0} STARTWITH ""{1}"")",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 8:
                     return
                        string.Format(@"{0} LIKE ""*{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 9:
                     return
                        string.Format(@"NOT({0} LIKE ""*{1}"")",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 10:
                     return
                        string.Format(@"{0} LIKE ""*{1}*""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 11:
                     return
                        string.Format(@"NOT({0} LIKE ""*{1}*"")",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 12:
                     return
                        string.Format(@"{0} in ""{1}"" to ""{2}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text,
                        te_maxvalue.Text);
                  case 13:
                     return
                        string.Format(@"NOT({0} in ""{1}"" to ""{2}"")",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text,
                        te_maxvalue.Text);
                  case 14:
                     return
                     string.Format("ISNULL({0})",
                        xData.Attribute("formulaName").Value);
                  case 15:
                     return
                     string.Format("NOT ISNULL({0})",
                        xData.Attribute("formulaName").Value);
	            }
               break;
            case 1:
            case 2:
               List<string> items = new List<string>();
               cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == Windows.Forms.CheckState.Checked)
                  .ToList()
                  .ForEach(c => items.Add(string.Format(@"""{0}""", c.Value.ToString())));
               if (items.Count == 0)
                  return "";
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format("{0} in [{1}]",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
                  case 1:
                     return
                        string.Format("NOT({0} in [{1}])",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
	            }
               break;
         }
         return "";
      }

      private string getStringDefaultFormula4CR()
      {
         string tmp = getChekInptVald("", "liststring");
         if (tmp.Length > 1)
            return
               string.Format("{0} in [{1}]",
                  xData.Attribute("formulaName").Value,
                  tmp);
         else
         {
            string min = getChekInptVald("", "min");
            string max = getChekInptVald("", "max");
            return
               string.Format("{0} in '{1}' to '{2}'",
                  xData.Attribute("formulaName").Value,
                  min.Length != 0 ? min : max,
                  max.Length != 0 ? max : min);
         }
      }

      /* واسه تاریخ باید حتما چک کنیم که هر دوبازه وارد شده باشد در غیر این صورت
       فقط آن بازه هایی که مشخص شده باید در لسیت قرار بگیرد
       مثلا حد پایین یا حد بالا
       باید به وضوح مشخص شده باشد که دقیقا چه بازه ای مد نظر داشته ایم
       */
      private string getDateTimeFormula4CR()
      {
         te_minvalue.Text = getChekInptVald(te_minvalue.Text, "min");
         te_maxvalue.Text = getChekInptVald(te_maxvalue.Text, "max");
         switch (cbx_sourcepath.SelectedIndex)
         {
            case 0:
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format(@"{0} = {1}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)));
                  case 1:
                     return
                        string.Format(@"{0} <> {1}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)));
                  case 2:
                     return
                        string.Format(@"{0} > {1}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)));
                  case 3:
                     return
                        string.Format(@"{0} >= {1}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)));
                  case 4:
                     return
                        string.Format(@"{0} < {1}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)));
                  case 5:
                     return
                        string.Format(@"{0} <= {1}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)));
                  case 6:
                     return
                        string.Format(@"{0} in {1} to {2}",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)),
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_maxvalue.Text)));
                  case 7:
                     return
                        string.Format(@"NOT ({0} in {1} to {2})",
                        xData.Attribute("formulaName").Value,
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)),
                        getDateTime4CR(getMiladiDateFromShamsiDate(te_maxvalue.Text)));
                  case 8:
                     return
                        string.Format(@"ISNULL({0})",
                        xData.Attribute("formulaName").Value);
                  case 9:
                     return
                        string.Format(@"NOT ISNULL({0})",
                        xData.Attribute("formulaName").Value);
	            }
               break;
            case 1:
               List<string> items = new List<string>();
               cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == Windows.Forms.CheckState.Checked)
                  .ToList()
                  .ForEach(c => items.Add(string.Format(@"{0}", getDateTime4CR(getMiladiDateFromShamsiDate(c.Value.ToString())))));
               if (items.Count == 0)
                  return "";
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format("{0} in [{1}]",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
                  case 1:
                     return
                        string.Format("NOT({0} in [{1}])",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
	            }
               break;
         }
         return "";
      }

      private string getDateTimeDefaultFormula4CR()
      {
         te_minvalue.Text = getChekInptVald(te_minvalue.Text, "min");
         te_maxvalue.Text = getChekInptVald(te_maxvalue.Text, "max");
         return
            string.Format(@"{0} in {1} to {2}",
            xData.Attribute("formulaName").Value,
            getDateTime4CR(getMiladiDateFromShamsiDate(te_minvalue.Text)),
            getDateTime4CR(getMiladiDateFromShamsiDate(te_maxvalue.Text)));
      }

      private string getStringDateTimeFormula4CR()
      {
         te_minvalue.Text = getChekInptVald(te_minvalue.Text, "min");
         te_maxvalue.Text = getChekInptVald(te_maxvalue.Text, "max");
         switch (cbx_sourcepath.SelectedIndex)
         {
            case 0:
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format(@"{0} = ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);
                  case 1:
                     return
                        string.Format(@"{0} <> ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);    
                  case 2:
                     return
                        string.Format(@"{0} > ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);    
                  case 3:
                     return
                        string.Format(@"{0} >= ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);    
                  case 4:
                     return
                        string.Format(@"{0} < ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);    
                  case 5:
                     return
                        string.Format(@"{0} <= ""{1}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text);    
                  case 6:
                     return
                        string.Format(@"{0} in ""{1}"" to ""{2}""",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text,
                        te_maxvalue.Text);
                  case 7:
                     return
                        string.Format(@"NOT({0} in ""{1}"" to ""{2}"")",
                        xData.Attribute("formulaName").Value,
                        te_minvalue.Text,
                        te_maxvalue.Text);
                  case 8:
                     return
                     string.Format("ISNULL({0})",
                        xData.Attribute("formulaName").Value);
                  case 9:
                     return
                     string.Format("NOT ISNULL({0})",
                        xData.Attribute("formulaName").Value);
	            }
               break;
            case 1:
            case 2:    
               List<string> items = new List<string>();
               cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == Windows.Forms.CheckState.Checked)
                  .ToList()
                  .ForEach(c => items.Add(string.Format(@"""{0}""", c.Value.ToString())));
               if (items.Count == 0)
                  return "";
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format("{0} in [{1}]",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
                  case 1:
                     return
                        string.Format("NOT({0} in [{1}])",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
	            }
               break;
         }
         return "";
      }

      private string getStringDateTimeDefaultFormula4CR()
      {
         //PersianCalendar pc = new PersianCalendar();
         //string FDateTime = string.Format("{0}/{1}/{2} {3}:{4}:{5}",pc.GetYear(DateTime.Now), pc.GetMonth(DateTime.Now), pc.GetDayOfMonth(DateTime.Now), pc.GetHour(DateTime.Now), pc.GetMinute(DateTime.Now), pc.GetSecond(DateTime.Now));
         //string TDateTime = FDateTime;
         //te_minvalue.Text = getChekInptVald("0000/00/00", "min");
         //te_maxvalue.Text = getChekInptVald("1499/12/30", "max");
         return
            string.Format(@"{0} in ""{1}"" to ""{2}""",
            xData.Attribute("formulaName").Value,
            getChekInptVald("0000/00/00", "min"),
            getChekInptVald("1499/12/30", "max"));
      }

      private string getDomainFormula4CR()
      {         
         switch (cbx_sourcepath.SelectedIndex)
         {
            case 0:
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                     string.Format("ISNULL({0})",
                        xData.Attribute("formulaName").Value);
                  case 1:
                     return
                     string.Format("NOT ISNULL({0})",
                        xData.Attribute("formulaName").Value);
	            }
               break;
            case 1:
            case 2:            
               List<string> items = new List<string>();
               cbe_lookupvalue.Properties.GetItems().OfType<CheckedListBoxItem>().Where(c => c.CheckState == Windows.Forms.CheckState.Checked)
                  .ToList()
                  .ForEach(c => items.Add(string.Format(@"{0}", c.Value.ToString() == "0" ? "false" : "true")));

               if (items.Count == 0)
                  return "";
               switch (cbx_compare.SelectedIndex)
	            {
                  case 0:
                     return
                        string.Format("{0} in [{1}]",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
                  case 1:
                     return
                        string.Format("NOT({0} in [{1}])",
                        xData.Attribute("formulaName").Value,
                        string.Join(", ", items));
	            }
               break;
         }
         return "";
      }

      private string getDomainDefaultFormula4CR()
      {
         return
            string.Format("{0} in [{1}, {2}]",
            xData.Attribute("formulaName").Value,
            getChekInptVald("", "min") == "0" ? "false" : "true",
            getChekInptVald("", "max") == "0" ? "false" : "true");
      }

      private string getMiladiDateFromShamsiDate(string datetime)
      {
         int year = 2014,
             mon  = 01,
             day  = 01,
             hour = 12,
             min  = 0,
             sec  = 0;

         try
         {
            year = Convert.ToInt32(datetime.Substring(0, 4));
            mon = Convert.ToInt32(datetime.Substring(5, 2));
            day = Convert.ToInt32(datetime.Substring(8, 2));
            hour = Convert.ToInt32(datetime.Substring(11, 2));
            min = Convert.ToInt32(datetime.Substring(14, 2));
            sec = Convert.ToInt32(datetime.Substring(17, 2));
         }
         catch { }
         PersianCalendar pc = new PersianCalendar();
         return pc.ToDateTime(year, mon, day, hour, min, sec, 0).ToString("yyyy/MM/dd HH:mm:ss");
      }

      private string getDateTime4CR(string datetime)
      {
         int year = 2014,
             mon = 01,
             day = 01,
             hour = 0,
             min = 0,
             sec = 0;

         try
         {
            year = Convert.ToInt32(datetime.Substring(0, 4));
            mon = Convert.ToInt32(datetime.Substring(5, 2));
            day = Convert.ToInt32(datetime.Substring(8, 2));
            hour = Convert.ToInt32(datetime.Substring(11, 2));
            min = Convert.ToInt32(datetime.Substring(14, 2));
            sec = Convert.ToInt32(datetime.Substring(17, 2));
         }
         catch { }
         return string.Format("DateTime ({0}, {1}, {2}, {3}, {4}, {5})", year, mon, day, hour, min, sec);
      }

      private string getChekInptVald(string input, string maxminlist)
      {
         string min = te_minvalue.Text.Replace(",", "");
         string max = te_maxvalue.Text.Replace(",", "");

         if (xData.Element("RefTable").Attribute("hac").Value == "1" &&
            xData.Element("RefTable").Element("Schema").Attribute("checked").Value == "1")        
         {
            int cnt =
               xData.Element("RefTable").Element("DefaultValues")
               .Elements("Value").Where(v => v.Value.ToUpper() == input.ToUpper()).Count();
            if (cnt >= 1)
               return input;
            else if (maxminlist == "min")
            {
               return xData.Element("RefTable").Element("DefaultValues")
               .Elements("Value").Min(v => v.Value);
            }
            else if(maxminlist == "max")
            {
               return xData.Element("RefTable").Element("DefaultValues")
               .Elements("Value").Max(v => v.Value);
            }
            else if (maxminlist == "listint")
            {
               return string.Join(", ", xData.Element("RefTable").Element("DefaultValues")
               .Elements("Value").Select(v => v.Value.Replace(",", "")).ToList());
            }
            else if (maxminlist == "liststring")
            {
               return string.Join(", ", xData.Element("RefTable").Element("DefaultValues")
               .Elements("Value").Select(v => "'" + v.Value + "'").ToList());
            }
         }
         else if (xData.Element("MinValue").Attribute("checked").Value == "1" &&
            xData.Element("MaxValue").Attribute("checked").Value == "1")
         {
            if (string.Compare(input, xData.Element("MinValue").Attribute("value").Value) >= 0 &&
               string.Compare(input, xData.Element("MaxValue").Attribute("value").Value) <= 0)
            {
               //return input;
               switch (maxminlist)
               {
                  case "min":
                     if (max.Length >= 1)
                        if (string.Compare(input, max) <= 0)
                           return input;
                        else
                           return max;
                     break;
                  case "max":
                     if (min.Length >= 1)
                        if (string.Compare(input, min) >= 0)
                           return input;
                        else
                           return min;
                     break;
               }
            }
            else if (maxminlist == "min")
            {
               if (max.Length >= 1)
                  if (string.Compare(input, max) >= 0)
                     return max;
                  else
                     return xData.Element("MinValue").Attribute("value").Value;
            }
            else if(maxminlist == "max")
            {
               if (min.Length >= 1)
                  if (string.Compare(input, min) >= 0)
                     return xData.Element("MaxValue").Attribute("value").Value;
                  else
                     return min;
            }
         }
         else if (xData.Element("MinValue").Attribute("checked").Value == "1")
         {
            switch (maxminlist)
            {
               case "min":
                  if (string.Compare(input, xData.Element("MinValue").Attribute("value").Value) >= 0)
                     return input;
                  else
                     return xData.Element("MinValue").Attribute("value").Value;                  
               case "max":
                  if (string.Compare(input, xData.Element("MinValue").Attribute("value").Value) >= 0)
                  {
                     if (min.Length >= 1)
                        if (string.Compare(input, min) >= 0)
                           return input;
                        else
                           return min;
                     return input;
                  }
                  else
                  {
                     if (min.Length >= 1)
                        return min;
                     else
                        return xData.Element("MinValue").Attribute("value").Value;
                  }                     
            }
         }
         else if (xData.Element("MaxValue").Attribute("checked").Value == "1")
         {
            switch (maxminlist)
            {
               case "min":
                  if (string.Compare(input, xData.Element("MaxValue").Attribute("value").Value) <= 0)
                  {
                     if (max.Length >= 1)
                        if (string.Compare(input, max) <= 0)
                           return input;
                        else
                           return max;
                     return input;
                  }
                  else
                  {
                     if (max.Length >= 1)
                        return max;
                     else
                        return xData.Element("MaxValue").Attribute("value").Value;
                  }                  
               case "max":
                  if (string.Compare(input, xData.Element("MaxValue").Attribute("value").Value) <= 0)
                     return input;
                  else
                     return xData.Element("MaxValue").Attribute("value").Value;
            }
         }
         return input;
      }
      #endregion
   }
}
