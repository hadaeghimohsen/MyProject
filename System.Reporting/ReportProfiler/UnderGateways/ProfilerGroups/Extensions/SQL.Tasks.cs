using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Extensions
{
   public static class SQL
   {
      public static XElement DoDistinct(this XElement xml, List<string> sql)
      {
         switch (xml.Attribute("distincted").Value)
         {
            case "false":
            case "0":
               sql.Add("SELECT");
               break;
            case "true":
            case "1":
               sql.Add("SELECT DISTINCT");
               break;
         }
         return xml;
      }

      public static XElement DoShowMeaningDesc(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Attribute("type").Value)
         {
            case "false":
            case "0":
               DoShowMeaningDesc4Domain(xml, sql);
               break;
            case "true":
            case "1":
               DoShowMeaningDesc4Table(xml, sql);
               break;
         }
         return xml;
      }

      public static XElement DoShowMeaningDesc4Domain(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Attribute("showmeaning").Value)
         {
            case "0":
               sql.Add("v.Code AS DESCTEXT, v.Code");
               break;
            case "1":
               sql.Add("v.Value AS DESCTEXT, v.Code");
               break;
            case "2":
               sql.Add("v.Code + ' - ' + v.Value AS DESCTEXT, v.Code");
               break;
            case "3":
               sql.Add("v.Value + ' - ' + v.Code AS DESCTEXT, v.Code");
               break;
         }
         sql.Add(string.Format(@"FROM Report.TableUsage t, Report.ColumnUsage c, Report.ColumnValues v WHERE t.TableType = 0 AND t.TitleEn = '{0}' AND t.ID = c.TableUsageID AND c.CodeEnName = '{1}' AND c.ID = v.ColumnUsageID",xml.Element("RefTable").Attribute("enName").Value , xml.Element("RefTable").Attribute("code").Value));
         return xml;
      }

      public static XElement DoShowMeaningDesc4Table(this XElement xml, List<string> sql)
      {
         switch (xml.Element("UseConStr").Attribute("dsType").Value)
         {
            case "0":
               DoShowMeaningDesc4TableOracle(xml,sql);
               break;
            case "1":
               DoShowMeaningDesc4TableSQLServer(xml, sql);
               break;
         }
         sql.Add(string.Format("FROM {0}.{1} WHERE 1=1", 
            xml.Element("RefTable").Element("Schema").Attribute("value").Value, 
            xml.Element("RefTable").Attribute("tabRealName").Value));
         return xml;
      }

      public static XElement DoShowMeaningDesc4TableSQLServer(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Attribute("showmeaning").Value)
         {
            case "0":
               sql.Add(string.Format("{0} AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value));
               break;
            case "1":
               sql.Add(string.Format("{1} AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
               break;
            case "2":
               sql.Add(string.Format("CAST({0} AS NVARCHAR(MAX)) + ' - ' + CAST({1} AS NVARCHAR(MAX)) AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
               break;
            case "3":
               sql.Add(string.Format("CAST({1} AS NVARCHAR(MAX)) + ' - ' + CAST({0} AS NVARCHAR(MAX)) AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
               break;
         }
         return xml;
      }

      public static XElement DoShowMeaningDesc4TableOracle(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Attribute("showmeaning").Value)
         {
            case "0":
               sql.Add(string.Format("{0} AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value));
               break;
            case "1":
               sql.Add(string.Format("{1} AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
               break;
            case "2":
               sql.Add(string.Format("{0} || ' - ' || {1} AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
               break;
            case "3":
               sql.Add(string.Format("{1} || ' - ' || {0} AS DESCTEXT, {0} AS CODE", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
               break;
         }
         return xml;
      }

      public static XElement DoWhere(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Element("Where").Attribute("checked").Value)
         {
            case "true":
            case "1":
               sql.Add(string.Format("AND {0}", xml.Element("RefTable").Element("Where").Value));
               break;
         }
         DoMinValue(xml, sql);
         DoMaxValue(xml, sql);
         DoNullValue(xml, sql);
         return xml;
      }

      public static XElement DoMinValue(this XElement xml, List<string> sql)
      {
         switch (xml.Element("MinValue").Attribute("checked").Value)
         {
            case "true":
            case "1":
               switch (GetType(xml))
               {
                  case "int":
                     DoMinimumInt(xml, sql);
                     break;
                  case "string":
                     DoMinimumString(xml, sql);
                     break;
                  case "stringdatetime":
                     DoMinimumStringDateTime(xml, sql);
                     break;
                  case "datetime":
                     DoMinimumDateTime(xml, sql);
                     break;
               }
               break;
         }
         return xml;
      }

      public static XElement DoMaxValue(this XElement xml, List<string> sql)
      {
         switch (xml.Element("MaxValue").Attribute("checked").Value)
         {
            case "true":
            case "1":
               switch (GetType(xml))
               {
                  case "int":
                     DoMaximumInt(xml, sql);
                     break;
                  case "string":
                     DoMaximumString(xml, sql);
                     break;
                  case "stringdatetime":
                     break;
                  case "datetime":
                     break;
               }
               break;
         }
         return xml;
      }

      public static XElement DoNullValue(this XElement xml, List<string> sql)
      {
         switch (xml.Descendants("NullValue").First().Attribute("checked").Value)
         {
            case "true":
            case "1":
               switch (xml.Descendants("NullValue").First().Attribute("value").Value)
	            {
                  case "0":
                     sql.Add(string.Format("AND {0} IS NULL", xml.Element("RefTable").Attribute("codeRealName").Value));
                     break;
                  case "1":
                     sql.Add(string.Format("AND {0} IS NOT NULL", xml.Element("RefTable").Attribute("codeRealName").Value));
                     break;
                  case "2":
                     sql.Add(string.Format("AND {0} IS NULL", xml.Element("RefTable").Attribute("valueRealName").Value));
                     break;
                  case "3":
                     sql.Add(string.Format("AND {0} IS NOT NULL", xml.Element("RefTable").Attribute("valueRealName").Value));
                     break;
                  case "4":
                     sql.Add(string.Format("AND {0} IS NULL AND {1} IS NULL", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
                     break;
                  case "5":
                     sql.Add(string.Format("AND {0} IS NOT NULL AND {1} IS NOT NULL", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("RefTable").Attribute("valueRealName").Value));
                     break;
	            }
               break;
         }         
         return xml;
      }
      
      public static XElement DoMinimumInt(this XElement xml, List<string> sql)
      {
         sql.Add(string.Format("AND {0} >= {1}", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("MinValue").Attribute("value").Value));
         return xml;
      }

      public static XElement DoMinimumString(this XElement xml, List<string> sql)
      {
         sql.Add(string.Format("AND {0} >= '{1}'", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("MinValue").Attribute("value").Value));
         return xml;
      }

      public static XElement DoMinimumStringDateTime(this XElement xml, List<string> sql)
      {
         sql.Add(string.Format("AND {0} >= '{1}'", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("MinValue").Attribute("value").Value));
         return xml;
      }

      public static XElement DoMinimumDateTime(this XElement xml, List<string> sql)
      {
         switch (xml.Element("UseConStr").Attribute("dsType").Value)
         {
            case "0":
               DoMinimumDateTimeOracle(xml, sql);
               break;
            case "1":
               DoMinimumDateTimeSQLServer(xml, sql);
               break;
         }
         return xml;
      }

      public static XElement DoMinimumDateTimeSQLServer(this XElement xml, List<string> sql)
      {
         return xml;
      }

      public static XElement DoMinimumDateTimeOracle(this XElement xml, List<string> sql)
      {
         return xml;
      }          

      public static string GetType(this XElement xml)
      {
         switch (xml.Element("Column").Attribute("type").Value)
         {
            case "CurrencyField":
            case "Int16sField":
            case "Int32sField":
            case "NumberField":
               return "int";
            case "StringField":
            case "PersistentMemoField":
               return xml.Element("StringDate").Attribute("checked").Value == "1" ? "stringdatetime" : "string";
            case "BooleanField":
               return "domain";
            case "DateTimeField":
               return "datetime";
            default:
               return "unknown";
         }
      }

      public static XElement DoMaximumInt(this XElement xml, List<string> sql)
      {
         sql.Add(string.Format("AND {0} <= {1}", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("MaxValue").Attribute("value").Value));
         return xml;
      }

      public static XElement DoMaximumString(this XElement xml, List<string> sql)
      {
         sql.Add(string.Format("AND {0} <= '{1}'", xml.Element("RefTable").Attribute("codeRealName").Value, xml.Element("MaxValue").Attribute("value").Value));
         return xml;
      }

      public static XElement DoHighAccessControl(this XElement xml, List<string> sql)
      {
         if (xml.Attribute("forConfig").Value == "true")
            return xml;

         switch (xml.Element("RefTable").Attribute("hac").Value)
         {
            case "1":
               switch (xml.Element("RefTable").Attribute("type").Value)
               {
                  case "0":
                     DoHighAccessControl4Domain(xml, sql);
                     break;
                  case "1":
                     DoHighAccessControl4Table(xml, sql);
                     break;
               }
               break;
         }         
         return xml;
      }

      public static XElement DoHighAccessControl4Domain(this XElement xml, List<string> sql)
      {
         if (xml.Element("RefTable").Element("DefaultValues").Elements("Value").Count() == 0)
            return xml;

         List<string> Codes = new List<string>();
         switch (xml.Element("RefTable").Element("DefaultValues").Attribute("checked").Value)
         {
            case "true":
            case "1":
               sql.Add("AND v.Code IN (");
               xml.Element("RefTable").Element("DefaultValues").Elements("Value").ToList().ForEach(value => Codes.Add(string.Format("'{0}'", value.Value)));
               sql.Add(string.Join(", ", Codes));
               sql.Add(")");
               break;
         }
         return xml;
      }

      public static XElement DoHighAccessControl4Table(this XElement xml, List<string> sql)
      {
         if (xml.Element("RefTable").Element("DefaultValues").Elements("Value").Count() == 0)
            return xml;

         List<string> Codes = new List<string>();
         switch (xml.Element("RefTable").Element("DefaultValues").Attribute("checked").Value)
         {
            case "true":
            case "1":
               sql.Add(string.Format("AND {0} IN (", xml.Element("RefTable").Attribute("codeRealName").Value));
               xml.Element("RefTable").Element("DefaultValues").Elements("Value").ToList().ForEach(value => Codes.Add(string.Format("'{0}'", value.Value)));
               sql.Add(string.Join(", ", Codes));
               sql.Add(")");
               break;
         }
         return xml;
      }

      public static XElement DoAddScopeFrom(this XElement xml, List<string> sql)
      {
         sql.Insert(0, "R.DESCTEXT, R.CODE FROM(");
         sql.Add(") R");
         return xml;
      }

      public static XElement DoOrderBy(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Element("IsOrdered").Attribute("checked").Value)
         {
            case "true":
            case "1":
               switch (xml.Element("RefTable").Element("IsOrdered").Attribute("sorted").Value)
	            {
                  case "0":
                     DoOrderByDesc(xml, sql);
                     break;
                  case "1":
                     DoOrderByAsc(xml, sql);
                     break;
	            }
               DoNullsFsLs(xml, sql);
               break;
         }         
         return xml;
      }

      public static XElement DoOrderByAsc(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Element("IsOrdered").Attribute("columns").Value)
         {
            case "0":
               sql.Add("ORDER BY R.CODE ASC");
               break;
            case "1":
               sql.Add("ORDER BY R.DESCTEXT ASC");
               break;
         }
         return xml;
      }

      public static XElement DoOrderByDesc(this XElement xml, List<string> sql)
      {
         switch (xml.Element("RefTable").Element("IsOrdered").Attribute("columns").Value)
         {
            case "0":
               sql.Add("ORDER BY R.CODE DESC");
               break;
            case "1":
               sql.Add("ORDER BY R.DESCTEXT DESC");
               break;
         }
         return xml;
      }

      public static XElement DoNullsFsLs(this XElement xml, List<string> sql)
      {
         switch (xml.Descendants("IsOrdered").First().Attribute("nullsFsLs").Value)
         {
            case "0":
               switch (xml.Element("UseConStr").Attribute("dsType").Value)
	            {
                  case "0":
                     sql.Add(" NULLS FIRST");
                     break;
                  case "1":
                     sql.Add(", CASE WHEN R.DESCTEXT IS NULL THEN 0 ELSE 1 END ");
                     break;
	            }
               break;
            case "1":
               switch (xml.Element("UseConStr").Attribute("dsType").Value)
               {
                  case "0":
                     sql.Add(" NULLS LAST");
                     break;
                  case "1":
                     sql.Add(", CASE WHEN R.DESCTEXT IS NULL THEN 1 ELSE 0 END ");
                     break;
               }
               break;
         }
         return xml;
      }

      public static XElement DoFetchRowCount(this XElement xml, List<string> sql)
      {
         sql.Insert(0, "SELECT");
         switch (xml.Element("RefTable").Element("NRowFetched").Attribute("checked").Value)
         {
            case "true":
            case "1":
               switch (xml.Element("UseConStr").Attribute("dsType").Value)
               {
                  case "0":
                     DoFetchRowCount4Oracle(xml, sql);
                     break;
                  case "1":
                     DoFetchRowCount4SQLServer(xml, sql);
                     break;
               }
               break;
         }         
         return xml;
      }

      public static XElement DoFetchRowCount4SQLServer(this XElement xml, List<string> sql)
      {
         sql.Insert(1 ,string.Format("TOP {0}", xml.Element("RefTable").Element("NRowFetched").Attribute("value").Value));
         return xml;
      }

      public static XElement DoFetchRowCount4Oracle(this XElement xml, List<string> sql)
      {
         sql.Insert(sql.Count - 1, string.Format("WHERE ROWNUM <= {0}", xml.Element("RefTable").Element("NRowFetched").Attribute("value").Value));
         return xml;
      }
   }
}
