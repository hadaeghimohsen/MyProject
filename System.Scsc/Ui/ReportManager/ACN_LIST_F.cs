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
   public partial class ACN_LIST_F : UserControl
   {
      public ACN_LIST_F()
      {
         InitializeComponent();
      }

      private void RqstBnExit1_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         if (tb_master.SelectedPage == tp_001)
         {
            AcntBs1.DataSource = 
               iScsc.Accounts
               .Where(a => 
                  Fga_Urgn_U.Split(',').Contains(a.REGN_PRVN_CODE + a.REGN_CODE) && 
                  Fga_Uclb_U.Contains(a.CLUB_CODE) 
               );

            LB_AllInCome.Text = "جمع درآمد کل : " + ((long)AcntBs1.List.OfType<Data.Account>().Where(a => a.AMNT_TYPE == "002").Sum(a => a.SUM_AMNT)).ToString("n0") + " ریال";
            LB_AllOutCome.Text = "جمع هزینه کل : " + ((long)AcntBs1.List.OfType<Data.Account>().Where(a => a.AMNT_TYPE == "001").Sum(a => a.SUM_AMNT)).ToString("n0") + " ریال";
         }
      }

      private void RqstBnSettingPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedPage == tp_001)
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
         if (tb_master.SelectedPage == tp_001)
         {
            if (AcntBs1.Current == null) return;
            var crnt = AcntBs1.Current as Data.Account;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Account.Rwno = {0}", crnt.RWNO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void RqstBnDefaultPrint_Click(object sender, EventArgs e)
      {
         if (tb_master.SelectedPage == tp_001)
         {
            if (AcntBs1.Current == null) return;
            var crnt = AcntBs1.Current as Data.Account;

            Job _InteractWithScsc =
              new Job(SendType.External, "Localhost",
                 new List<Job>
                  {
                     new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Account.Rwno = {0}", crnt.RWNO))}
                  });
            _DefaultGateway.Gateway(_InteractWithScsc);
         }
      }

      private void AcdtBs1_ListChanged(object sender, ListChangedEventArgs e)
      {
         try
         {
            var crnt = AcdtBs1.Current as Data.Account_Detail;

            if (crnt == null) return;

            PymtBs1.DataSource = iScsc.Payments.Where(p => p.RQST_RQID == crnt.PYMT_RQST_RQID);
            PmmtBs1.DataSource = iScsc.Payment_Methods.Where(p => p.PYMT_RQST_RQID == crnt.PYMT_RQST_RQID);
            RqroBs1.DataSource = iScsc.Request_Rows.First(rr => rr.RQST_RQID == crnt.PYMT_RQST_RQID);
            var rqro = RqroBs1.Current as Data.Request_Row;
            FighBs1.DataSource = iScsc.Fighters.First(f => f.FILE_NO == rqro.FIGH_FILE_NO);
         }
         catch 
         {}
      }

      private void AcdtBs1_PositionChanged(object sender, EventArgs e)
      {
         AcdtBs1_ListChanged(null, null);
      }
   }
}
