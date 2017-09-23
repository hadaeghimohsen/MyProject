using System;
using System.Collections.Generic;
using System.JobRouting.Jobs;
using System.Linq;
using System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace System.Reporting.WorkFlow.Ui
{
   partial class WHR_SCON_F
   {
      private void CheckColumnsValidation()
      {
         mpbc_loading.Visible = false;
         flp_wherecondition.Controls.OfType<FlowLayoutPanel>()
            .ToList()
            .ForEach(flp =>
            {
               flp.Controls.OfType<Filter>()
                  .ToList()
                  .ForEach(f =>
                  {
                     if (!(xReportFileContent.Element("XmlReport").Elements("Table")
                           .Where(t =>
                              t.Attribute("name").Value == f.XmlTemplate.Element("Table").Attribute("enName").Value &&
                              t.Elements("Feild").Where(c =>
                                 c.Attribute("name").Value == f.XmlTemplate.Element("Column").Attribute("enName").Value
                              ).Count() == 1
                           ).Count() == 1))
                        f.Visible = false;
                  });
               if (flp.Controls.OfType<Filter>().Where(c => c.Visible).Count() == 0)
                  flp.Visible = false;
            });

         if (flp_wherecondition.Controls.OfType<FlowLayoutPanel>().Where(flp => flp.Visible).Count() == 0)         
            bp_flow.Buttons[2].Properties.Enabled = false;
         else
            bp_flow.Buttons[2].Properties.Enabled = true;
      }

      private void ShowFormulaSelection()
      {
         List<string> FormulaSelection = new List<string>();
         flp_wherecondition.Controls.OfType<FlowLayoutPanel>()
            .ToList()
            .ForEach(fl =>
               {
                  fl.Controls.OfType<Filter>()
                    .Where(f => f.Visible)
                    .Where(f => (f.Enabled && f.Checked == false && f.HasFiltering) || (f.Enabled && f.Checked) || f.Enabled == false)
                    .ToList()
                    .ForEach(f =>
                    {
                       if ((f.Enabled && f.Checked) || !(f.Enabled))
                       {
                          string formula = f.GetUserFormulaSelection("crystalreport");
                          if (formula.Length > 1)
                             FormulaSelection.Add(formula);
                       }
                       if (f.HasFiltering)
                       {
                          string formula = f.GetAdminFormulaSelection("crystalreport");
                          if (formula.Length > 1)
                             FormulaSelection.Add(formula);
                       }
                    });
               });

         MessageBox.Show(string.Join(" AND ", FormulaSelection));
      }

      private void ShowPreview()
      {
         List<string> FormulaSelection = new List<string>();
         string reportEngine = xRunReport.Descendants("Report").First().Element("PhysicalName").Value;
         reportEngine = reportEngine.Substring(reportEngine.LastIndexOf('.'));
         switch (reportEngine.ToLower())
         {
            case ".rpt":
               reportEngine = "crystalreport";
               break;
            default:
               break;
         }

         flp_wherecondition.Controls.OfType<FlowLayoutPanel>()
            .ToList()
            .ForEach(fl =>
            {
               fl.Controls.OfType<Filter>()
                 .Where(f => f.Visible)
                 .Where(f => (f.Enabled && f.Checked == false && f.HasFiltering) || (f.Enabled && f.Checked) || f.Enabled == false)
                 .ToList()
                 .ForEach(f =>
                 {
                    if ((f.Enabled && f.Checked) || !(f.Enabled))
                    {
                       string formula = f.GetUserFormulaSelection(reportEngine);
                       if (formula.Length > 1)
                          FormulaSelection.Add(formula);
                    }
                    if (f.HasFiltering)
                    {
                       string formula = f.GetAdminFormulaSelection(reportEngine);
                       if (formula.Length > 1)
                          FormulaSelection.Add(formula);
                    }
                 });
            });

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.External, "DefaultGateway",
                     new List<Job>
                     {
                        new Job(SendType.External, "ReportViewers",
                           new List<Job>
                           {
                              new Job(SendType.Self, 02 /* Execute DoWork4ReportViewers */),
                              new Job(SendType.SelfToUserInterface,"Viewers", 08 /* Execute ReportSourceSetup */){Input = xRunReport.Descendants("Report").First().Element("PhysicalName").Value},
                              new Job(SendType.SelfToUserInterface,"Viewers", 09 /* Execute VerifyReportSetup */){Input = cb_datasource.SelectedValue},
                              new Job(SendType.SelfToUserInterface,"Viewers", 10 /* Execute FormulaSelectionSetup */){Input = string.Join(" AND ", FormulaSelection) },
                              new Job(SendType.SelfToUserInterface,"Viewers", 11 /* Execute ParameterSetup */),
                              new Job(SendType.SelfToUserInterface,"Viewers", 12 /* Execute ReportShow */)
                           })
                     })
               }));
      }
   }
}
