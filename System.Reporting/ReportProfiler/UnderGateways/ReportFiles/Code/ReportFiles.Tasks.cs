using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;

namespace System.Reporting.ReportProfiler.UnderGateways.ReportFiles.Code
{
   partial class ReportFiles
   {
      /// <summary>
      /// Code 00
      /// </summary>
      /// <param name="job"></param>
      private void GetUi(Job job)
      {
         string value = job.Input.ToString().ToLower();
         if (value == "rpt_chng_f")
         {
            if (_RPT_CHNG_F == null)
               _RPT_CHNG_F = new Ui.RPT_CHNG_F { _DefaultGateway = this };
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void GetXMLReport(Job job)
      {
         string PhysicalFile = XDocument.Parse(job.Input.ToString()).Element("Report").Element("PhysicalName").Value;
         string ExtensionFile = XDocument.Parse(job.Input.ToString()).Element("Report").Element("PhysicalName").Value;
         switch (ExtensionFile.Substring(ExtensionFile.Length - 3, 3).ToUpper())
         {
            case "RPT":
               Manager(
                  new Job(SendType.Self, 02 /* Execute CRFetchTab_Fld */) { Input = PhysicalFile, AfterChangedOutput = new Action<object>((output) => job.Output = output) });
               break;
            case "JSXML":
               job.OwnerDefineWorkWith.Add(
                  new Job(SendType.Self, 03 /* Execute JRFetchTab_Fld */) { Input = PhysicalFile });
               break;
            default: 
               job.Status = StatusType.Failed;
               Gateway(
                  new Job(SendType.External, "Localhost",
                     new List<Job>
                  {
                     new Job(SendType.Self, 00 /* Execute GetUi */){Input = "RPT_CHNG_F"},
                     new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 02 /* Execute Set */){Input = PhysicalFile},
                     new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 03 /* Execute Paint */),
                     new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 07 /* Execute LoadData */)
                  })
                  );
               break;
         }
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 02
      /// </summary>
      /// <param name="job"></param>
      private void CRFetchTab_Fld(Job job)
      {
         try
         {
            string PhysicalFile = job.Input.ToString();
            ReportDocument crDoc = new ReportDocument();
            crDoc.Load(PhysicalFile);

            string xmlReport = @"<XmlReport>{0}</XmlReport>";
            List<string> xmlTables = new List<string>();
            string xmlTable = string.Empty;
            List<string> xmlFields = new List<string>();
            string xmlField = string.Empty;

            foreach (Table table in crDoc.Database.Tables)
            {
               xmlTable = string.Format(@"<Table name=""{0}"" enRealName=""{1}"">{{0}}
        </Table>", table.Name.ToUpper(), table.Location.ToUpper());
               xmlFields.Clear();
               foreach (DatabaseFieldDefinition field in table.Fields)
               {
                  xmlField = string.Format(@"<Feild name=""{0}"" type=""{1}"" formulaName=""{2}""/>", field.Name.ToUpper(), field.ValueType, field.FormulaName);
                  xmlFields.Add(xmlField);
               }
               xmlTables.Add(string.Format(xmlTable, "\n\t\t" + string.Join("\n\t\t", xmlFields)));
            }

            XDocument xdoc = XDocument.Parse(string.Format(xmlReport, "\n\t" + string.Join("\n\t", xmlTables) + "\r"));
            string tab, fld;

            /* Formula Fields */
            foreach (FormulaFieldDefinition item in crDoc.DataDefinition.FormulaFields)
            {
               if (item.Name.Contains("#"))
               {
                  tab = item.Name.Substring(0, item.Name.IndexOf("#"));
                  fld = item.Name.Substring(item.Name.IndexOf("#") + 1);
                  xdoc.Element("XmlReport").Elements("Table").Where(t => t.Attribute("name").Value == tab)
                     .ToList()
                     .ForEach(xt =>
                     {
                        xt.Add(new XElement("Feild", new XAttribute("name", fld.ToUpper()), new XAttribute("type", item.ValueType), new XAttribute("formulaName", item.FormulaName.ToUpper())));
                     });
               }

            }
            /* SQL Expressions */
            foreach (SQLExpressionFieldDefinition item in crDoc.DataDefinition.SQLExpressionFields)
            {
               if (item.Name.Contains("#"))
               {
                  tab = item.Name.Substring(0, item.Name.IndexOf("#"));
                  fld = item.Name.Substring(item.Name.IndexOf("#") + 1);
                  xdoc.Element("XmlReport").Elements("Table").Where(t => t.Attribute("name").Value == tab)
                     .ToList()
                     .ForEach(xt =>
                     {
                        xt.Add(new XElement("Feild", new XAttribute("name", fld.ToUpper()), new XAttribute("type", item.ValueType), new XAttribute("formulaName", item.FormulaName.ToUpper())));
                     });
               }
            }

            job.Output = xdoc.ToString();
            job.Status = StatusType.Successful;
         }
         catch
         {
            job.Status = StatusType.Failed;            
            Gateway(
               new Job(SendType.External, "Localhost",
                  new List<Job>
                  {
                     new Job(SendType.Self, 00 /* Execute GetUi */){Input = "RPT_CHNG_F"},
                     new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 02 /* Execute Set */){Input = job.Input},
                     new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 03 /* Execute Paint */),
                     new Job(SendType.SelfToUserInterface, "RPT_CHNG_F", 07 /* Execute LoadData */)
                  })
               );
         }
      }

      /// <summary>
      /// Code 03
      /// </summary>
      /// <param name="job"></param>
      private void JRFtechTab_Fld(Job job)
      {
         string PhysicalFile = job.Input.ToString();
         job.Status = StatusType.Successful;
      }      
   }
}
