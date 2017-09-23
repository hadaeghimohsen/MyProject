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
using System.Xml.Linq;

namespace System.Scsc.Ui.ReportManager
{
   public partial class RPT_LIST_F : UserControl
   {
      public RPT_LIST_F()
      {
         InitializeComponent();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         if(tb_master.SelectedPage == tp_001)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13921122234205 && ar.UNIT_ID == 13941030181632);
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
         else if (tb_master.SelectedPage == tp_002)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13921205230916 && ar.UNIT_ID == 13941030181632);
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
         else if(tb_master.SelectedPage == tp_003)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13941030181543 && ar.UNIT_ID == 13941030181632);
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
         else if(tb_master.SelectedPage == tp_004)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13941030181608 && ar.UNIT_ID == 13941030181632);
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
         else if (tb_master.SelectedPage == tp_005)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13921122234205 && (ar.UNIT_ID == 13941030181655 || ar.UNIT_ID == 13941030181710)).OrderBy(ar => new { ar.UNIT_ID });
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
         else if (tb_master.SelectedPage == tp_006)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13921205230916 && (ar.UNIT_ID == 13941030181655 || ar.UNIT_ID == 13941030181710)).OrderBy(ar => new { ar.UNIT_ID });
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
         else if (tb_master.SelectedPage == tp_007)
         {
            VFAcrpBs1.DataSource = iScsc.VF_Access_Reports.Where(ar => ar.SUB_SYS == 5 && ar.TYPE_ID == 13941030181608 && (ar.UNIT_ID == 13941030181655 || ar.UNIT_ID == 13941030181710)).OrderBy(ar => new { ar.UNIT_ID });
            VFAcpfBs1.DataSource = iScsc.VF_Access_Profilers.Where(ar => ar.SUB_SYS == 5);
         }
      }

      private void Btn_RunReport_Click(object sender, EventArgs e)
      {
         var rpt = VFAcrpBs1.Current as Data.VF_Access_Report;
         var prf = VFAcpfBs1.Current as Data.VF_Access_Profiler;
         if (rpt == null || prf == null) return;

         var xRunReport = new XDocument(
                                 new XElement("RunReport",
                                    new XElement("StepOne", 
                                       new XElement("Report",
                                          new XAttribute("id", rpt.SERV_ID),
                                          new XAttribute("roleId", rpt.ROLE_ID),
                                          new XElement("LogicalName", rpt.SERV_NAME),
                                          new XElement("PhysicalName", rpt.SERV_FILE_PATH)
                                       )
                                    ),
                                    new XElement("StepTwo",
                                       new XElement("Profiler",
                                          new XAttribute("id", prf.PROF_ID),
                                          new XAttribute("role_id", prf.ROLE_ID),
                                          new XAttribute("dataSource", prf.DSRC_ID)
                                       )
                                    )
                                 )
                          );

         _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "DefaultGateway:Reporting:WorkFlow", 05 /* Execute DoWork4WHR_SCON_F */, SendType.Self)
               {
                  Input = xRunReport
               });
      }
   }
}
