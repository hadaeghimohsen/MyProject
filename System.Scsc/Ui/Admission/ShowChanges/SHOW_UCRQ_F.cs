using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using System.JobRouting.Jobs;
using System.Xml.Linq;

namespace System.Scsc.Ui.Admission.ShowChanges
{
   public partial class SHOW_UCRQ_F : UserControl
   {
      public SHOW_UCRQ_F()
      {
         InitializeComponent();
      }

      private void Execute_Query(bool runAllQuery)
      {
         if(tb_master.SelectedTab == tp_001 || runAllQuery)
         {
            RqstBs1.DataSource = iScsc.Requests.FirstOrDefault(r => r.RQID == Rqid);
         }
      }

      private void RqstBs1_CurrentChanged(object sender, EventArgs e)
      {
         RqroBs1.DataSource = iScsc.Request_Rows.FirstOrDefault(rr => rr.FIGH_FILE_NO == FileNo);
      }

      private void RqroBs1_CurrentChanged(object sender, EventArgs e)
      {
         var lastPblcRwno = iScsc.Fighters.FirstOrDefault(f => f.FILE_NO == FileNo);
         FgpbsBs1.DataSource = iScsc.Fighter_Publics.FirstOrDefault(fb => fb.RWNO == lastPblcRwno.FGPB_RWNO_DNRM && fb.FIGH_FILE_NO == FileNo && fb.RECT_CODE == "004");
         MbspBs1.DataSource = iScsc.Member_Ships.FirstOrDefault(mb => mb.RQRO_RQST_RQID == Rqid && mb.FIGH_FILE_NO == FileNo && mb.RECT_CODE == "004");

         var fgpb = FgpbsBs1.Current as Data.Fighter_Public;
         CtgyBs1.DataSource = iScsc.Category_Belts.Where(c => c.MTOD_CODE == fgpb.MTOD_CODE);
         /*RegnBs1.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == fgpb.REGN_PRVN_CODE);*/
      }

      private void PymtsBs1_CurrentChanged(object sender, EventArgs e)
      {
         if (PymtsBs1.List.Count > 0)
            Gb_Expense.Visible = true;
         else
            Gb_Expense.Visible = false;
      }

      private void DayType_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
      {
         GridView view = sender as GridView;
         if (e.Column.FieldName == "TIME_DESC" && e.IsGetData)
         {
            var h = ((TimeSpan)view.GetListSourceRowCellValue(e.ListSourceRowIndex, "END_TIME")).Hours;
            e.Value = h >= 0 && h < 12 ? "صبح" : h >= 12 && h < 15 ? "ظهر" : h >= 15 && h < 18 ? "بعد ظهر" : h >= 18 ? "عصر" : "نام مشخص";
         }
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void RqstBnADoc_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            _DefaultGateway.Gateway(
               new Job(SendType.External, "Localhost", "", 59 /* Execute Cmn_Dcmt_F */, SendType.Self) { Input = iScsc.Request_Rows.Where(rr => rr.RQST_RQID == rqst.RQID && rr.RWNO == 1).Single() }
            );
         }
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 81 /* Execute Cfg_Stng_F */),
                     new Job(SendType.SelfToUserInterface, "CFG_STNG_F", 10 /* Actn_CalF_P */){Input = new XElement("Request", new XAttribute("type", "ModualReport"), new XAttribute("modul", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnPrintAfterPay_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            if (RqstBs1.Current == null) return;
            var crnt = RqstBs1.Current as Data.Request;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Rpt_Mngr_F */){Input = new XElement("Print", new XAttribute("type", "PrntAftrPay"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", crnt.RQID))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnAResn_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedTab == tp_001)
         {
            var rqst = RqstBs1.Current as Data.Request;
            if (rqst == null) return;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 94 /* Execute Cmn_Resn_F */){Input = rqst.Request_Rows.FirstOrDefault()},
                     //new Job(SendType.SelfToUserInterface, "POS_TOTL_F", 10 /* Actn_CalF_F */){Input = xSendPos}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void SesnBs1_CurrentChanged(object sender, EventArgs e)
      {
         if (SesnBs1.List.Count > 0)
            sessionsGridControl.Visible = true;
         else
            sessionsGridControl.Visible = false;
      }

      private void MbspBs1_CurrentChanged(object sender, EventArgs e)
      {
         var mbsp = MbspBs1.Current as Data.Member_Ship;

         if (mbsp == null) return;
         Gb_Mbsp.Visible = true;
         SesnBs1.DataSource = iScsc.Sessions.Where(s => s.MBSP_FIGH_FILE_NO == FileNo && s.MBSP_RECT_CODE == "004" && s.MBSP_RWNO == mbsp.RWNO);
      }

      private void MbspRwno_Txt_DoubleClick(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 151 /* Execute Mbsp_Chng_F */),
                  new Job(SendType.SelfToUserInterface, "MBSP_CHNG_F", 10 /* execute Actn_CalF_F */)
                  {
                     Input = 
                        new XElement("Fighter",
                           new XAttribute("fileno", FileNo),
                           new XAttribute("mbsprwno", MbspRwno_Txt.Text)
                        )
                  }
               }
            )
         );
      }
   }
}
