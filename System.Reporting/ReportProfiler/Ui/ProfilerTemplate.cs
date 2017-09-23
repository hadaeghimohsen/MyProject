using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.JobRouting.Jobs;
using System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Ui;
using System.Xml.Linq;

namespace System.Reporting.ReportProfiler.Ui
{
   public partial class ProfilerTemplate : UserControl
   {
      public ProfilerTemplate()
      {
         InitializeComponent();
      }

      private void lnb_profiler_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                   new Job(SendType.Self, 02 /* Execute DoWork4SpecifyReportProfiler */),
                   new Job(SendType.SelfToUserInterface, "SpecifyReportProfile", 11 /* DoWork4SelectingProfiler */){ Input = string.Format(@"<Profiler><Role all=""{0}""/></Profiler>", cb_allroles.Checked) }
               }));
      }

      private void lnb_reportfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         _DefaultGateway.Gateway(
             new Job(SendType.External, "Localhost",
                new List<Job>
                {
                   new Job(SendType.External, "DefaultGateway",
                      new List<Job>
                      {
                         new Job(SendType.External, "ReportUnitType",
                            new List<Job>
                            {
                               new Job(SendType.Self, 02 /* Execute DoWork4ReportUnitType */),
                               new Job(SendType.SelfToUserInterface, "SpecifyReportFile", 14 /* Execute ShowChooseReport */){Input = "ProfilerTemplate"}
                            })
                      })                
                }));
      }

      private void sb_print_Click(object sender, EventArgs e)
      {
         if (lnb_reportfile.Tag == null)
            return;
         List<string> FormulaSelection = new List<string>();
         string reportEngine = XDocument.Parse(lnb_reportfile.Tag.ToString()).Element("Report").Element("PhysicalName").Value;
         reportEngine = reportEngine.Substring(reportEngine.LastIndexOf('.'));
         switch (reportEngine.ToLower())
         {
            case ".rpt":
               reportEngine = "crystalreport";
               break;
            default:
               break;
         }

         flp_profiler.Controls.OfType<GroupBox>()
            .ToList()
            .ForEach(gb =>
            {
               gb.Controls.OfType<FlowLayoutPanel>().First()
                  .Controls.OfType<Filter>()
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

         //MessageBox.Show(string.Join(" AND ", FormulaSelection));
         //return;

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
                              new Job(SendType.SelfToUserInterface,"Viewers", 08 /* Execute ReportSourceSetup */){Input = XDocument.Parse(lnb_reportfile.Tag.ToString()).Element("Report").Element("PhysicalName").Value},
                              new Job(SendType.SelfToUserInterface,"Viewers", 09 /* Execute VerifyReportSetup */){Input = cb_datasource.SelectedValue},
                              new Job(SendType.SelfToUserInterface,"Viewers", 10 /* Execute FormulaSelectionSetup */){Input = string.Join(" AND ", FormulaSelection) },
                              new Job(SendType.SelfToUserInterface,"Viewers", 11 /* Execute ParameterSetup */),
                              new Job(SendType.SelfToUserInterface,"Viewers", 12 /* Execute ReportShow */)//{Executive = ExecutiveType.Asynchronous}
                           })
                     })
               }));
      }

      private void ShowFormulaSelection()
      {
         if (lnb_reportfile.Tag == null)
            return;
         List<string> FormulaSelection = new List<string>();
         string reportEngine = XDocument.Parse(lnb_reportfile.Tag.ToString()).Element("Report").Element("PhysicalName").Value;
         reportEngine = reportEngine.Substring(reportEngine.LastIndexOf('.'));
         switch (reportEngine.ToLower())
         {
            case ".rpt":
               reportEngine = "crystalreport";
               break;
            default:
               break;
         }

         flp_profiler.Controls.OfType<GroupBox>()
            .ToList()
            .ForEach(gb =>
            {
               gb.Controls.OfType<FlowLayoutPanel>().First()
                  .Controls.OfType<Filter>()
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

         MessageBox.Show(string.Join(" AND ", FormulaSelection));
      }
   }
}
