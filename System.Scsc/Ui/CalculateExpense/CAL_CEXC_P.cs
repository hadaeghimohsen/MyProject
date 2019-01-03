using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.JobRouting.Jobs;

namespace System.Scsc.Ui.CalculateExpense
{
   public partial class CAL_CEXC_P : UserControl
   {
      public CAL_CEXC_P()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Execute_Query()
      {
         iScsc = new Data.iScscDataContext(ConnectionString);
         if (tb_master.SelectedTab == tp_001)
         {
            MsexBs.DataSource = 
               iScsc.Misc_Expenses
               .Where(m => 
                           Fga_Urgn_U.Split(',').Contains(m.REGN_PRVN_CODE + m.REGN_CODE) && 
                           Fga_Uclb_U.Contains(m.CLUB_CODE) && m.VALD_TYPE == "002" && 
                           m.CALC_EXPN_TYPE == "001" &&
                           m.DELV_DATE.Value.Date >= (Pdt_FromDate.Value ?? DateTime.Now.AddYears(-100).Date)  &&
                           m.DELV_DATE.Value.Date <= (Pdt_ToDate.Value ?? DateTime.Now.Date)                           
               );
         }
         requery = false;
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void ExecQury_Butn_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void SettingPrint_Butn_Click(object sender, EventArgs e)
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

      private void Print_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Selection"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", ""))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }

      private void DefaultPrint_Butn_Click(object sender, EventArgs e)
      {
         Job _InteractWithScsc =
            new Job(SendType.External, "Localhost",
               new List<Job>
               {
                  new Job(SendType.Self, 84 /* Execute Cfg_Stng_F */){Input = new XElement("Print", new XAttribute("type", "Default"), new XAttribute("modual", GetType().Name), new XAttribute("section", GetType().Name.Substring(0,3) + "_001_F"), string.Format("Request.Rqid = {0}", ""))}
               });
         _DefaultGateway.Gateway(_InteractWithScsc);
      }
   }
}
