using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.JobRouting.Jobs;
using System.Data;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Drawing;
using DevExpress.XtraEditors;

namespace System.Reporting.ReportProfiler.UnderGateways.ProfilerGroups.Code
{
   partial class ProfilerGroups
   {
      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void PrepareProfilerDesign4Role(Job job)
      {
         var GetCrntUser = new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(GetCrntUser);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  string.Format(@"<Request type=""{0}"">{1}</Request>", "GetGroups", job.Input),
                  "{ Call Report.GetProfiler(?) }",
                  "iProject",
                  "scott"
               },
               Executive = ExecutiveType.Asynchronous,
               AfterChangedOutput = new Action<object>(
               (output) =>
                  {
                     DataSet ds = output as DataSet;
                     ds.Tables["Group"].Rows.OfType<DataRow>()
                        .ToList()
                        .ForEach(g =>
                        {
                           GroupBox gb = new GroupBox() 
                           { 
                              Text = g["GFaName"].ToString(), 
                              Tag = string.Format(@"<Group id=""{0}"" faName=""{1}"" dbsource=""{2}""/>", g["Group"], g["GFaName"], g["DataSource"]),
                              Height = 25
                           };
                           FlowLayoutPanel flp = new FlowLayoutPanel(){Dock = DockStyle.Fill};
                           gb.Controls.Add(flp);

                           /* Create All Filter Items */

                           Manager(
                              new Job(SendType.External, "Commons", "", 04 /* Execute DoWork4Odbc */, SendType.Self)
                              {
                                 Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    true,
                                    "xml",
                                    string.Format(@"<Request type=""{0}"" crntuser=""{2}"">{1}</Request>", "Filters", string.Format("<Group>{0}</Group>{1}", g["Group"], job.Input), GetCrntUser.Output),
                                    "{ Call Report.GetItemsInGroup(?) }",
                                    "iProject",
                                    "scott"
                                 },
                                 AfterChangedOutput = new Action<object>(
                                    (xmloutput) =>
                                       {
                                          try
                                          {
                                             DataSet xmlds = xmloutput as DataSet;
                                             XDocument xFilter = XDocument.Parse(xmlds.Tables["Filter"].Rows[0]["XmlData"].ToString());
                                             xFilter.Element("Filters")
                                                .Elements("Filter")
                                                .ToList()
                                                .ForEach(f =>
                                                {
                                                   Ui.Filter filter = new Ui.Filter() { _DefaultGateway = this, XmlTemplate = f, Margin = new Padding(0) };
                                                   filter.Init();
                                                   gb.Height += 48;
                                                   flp.Controls.Add(filter);
                                                });
                                          }
                                          catch { }
                                       })                                 
                              });

                           Gateway(
                              new Job(SendType.External, "Localhost", "DefaultGateway:ProfilerTemplate", 10 /* Execute AddGroup */, SendType.SelfToUserInterface)
                              {
                                 Input = gb
                              });
                        });
                     
                  })
            });
         job.Status = StatusType.Successful;
      }

      /// <summary>
      /// Code 01
      /// </summary>
      /// <param name="job"></param>
      private void PrepareNProfilerDesign4Role(Job job)
      {
         /* Get Current User Login */
         var GetCrntUser = new Job(SendType.External, "Localhost", "Commons", 12 /* Execute DoWork4RoleSettings4CurrentUser */, SendType.Self);
         _DefaultGateway.Gateway(GetCrntUser);

         _DefaultGateway.Gateway(
            new Job(SendType.External, "Localhost", "Commons", 04 /* Execute DoWork4Odbc */, SendType.Self)
            {
               Input = new List<object>
               {
                  false,
                  "procedure",
                  true,
                  true,
                  "xml",
                  string.Format(@"<Request type=""{0}"">{1}</Request>", "NGetGroups", job.Input),
                  "{ Call Report.GetProfiler(?) }",
                  "iProject",
                  "scott"
               },
               Executive = ExecutiveType.Asynchronous,
               AfterChangedOutput = new Action<object>(
               (output) =>
               {
                  DataSet ds = output as DataSet;
                  ds.Tables["Group"].Rows.OfType<DataRow>()
                     .ToList()
                     .ForEach(g =>
                     {                        
                        FlowLayoutPanel flp = new FlowLayoutPanel() 
                        { 
                          //Dock = DockStyle.Fill,
                          Tag = string.Format(@"<Group id=""{0}"" faName=""{1}"" dbsource=""{2}""/>", g["Group"], g["GFaName"], g["DataSource"]),
                          Height = 25,                          
                        };
                        flp.SizeChanged += new EventHandler((sender, e) =>
                        {
                           flp.SuspendLayout();
                           foreach (Control ctrl in flp.Controls)
                           {
                              ctrl.Width = flp.ClientSize.Width;
                           }
                           flp.ResumeLayout();
                        });

                        LabelControl lbc = new LabelControl()
                           {
                              AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None,
                              LineVisible = true,
                              Location = new System.Drawing.Point(5, 3),                              
                              Text = g["GFaName"].ToString()
                           };
                        //lbc.Appearance.BackColor = Color.Navy;
                        //lbc.Appearance.BackColor2 = Color.MidnightBlue;
                        lbc.Appearance.Font = new System.Drawing.Font("B Koodak", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
                        lbc.Appearance.ForeColor = Color.White;

                        flp.Controls.Add(lbc);
                        /* Create All Filter Items */

                        Manager(
                           new Job(SendType.External, "Commons", "", 04 /* Execute DoWork4Odbc */, SendType.Self)
                           {
                              Input = new List<object>
                                 {
                                    false,
                                    "procedure",
                                    true,
                                    true,
                                    "xml",
                                    string.Format(@"<Request type=""{0}"" crntuser=""{2}"">{1}</Request>", "Filters", string.Format("<Group>{0}</Group>{1}", g["Group"], job.Input), GetCrntUser.Output),
                                    "{ Call Report.GetItemsInGroup(?) }",
                                    "iProject",
                                    "scott"
                                 },
                              AfterChangedOutput = new Action<object>(
                                 (xmloutput) =>
                                 {
                                    try
                                    {
                                       DataSet xmlds = xmloutput as DataSet;
                                       XDocument xFilter = XDocument.Parse(xmlds.Tables["Filter"].Rows[0]["XmlData"].ToString());
                                       xFilter.Element("Filters")
                                          .Elements("Filter")
                                          .ToList()
                                          .ForEach(f =>
                                          {
                                             Ui.Filter filter = new Ui.Filter() { _DefaultGateway = this, XmlTemplate = f, Margin = new Padding(0) };
                                             filter.Init();
                                             flp.Height += 48;
                                             flp.Controls.Add(filter);
                                          });
                                    }
                                    catch { }
                                 })
                           });

                        Gateway(
                           new Job(SendType.External, "Localhost", "DefaultGateway:DefaultGateway:WorkFlow:WHR_SCON_F", 08 /* Execute AddGroup */, SendType.SelfToUserInterface)
                           {
                              Input = flp
                           });
                     });

               })
            });
         job.Output = "Sucessfully!";
         job.Status = StatusType.Successful;
      }

   }
}
