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
         if (tb_master.SelectedTab == tp_001)
         {
            MSEX_BindingSource.DataSource = 
               iScsc.Misc_Expenses
               .Where(m => 
                           Fga_Urgn_U.Split(',').Contains(m.REGN_PRVN_CODE + m.REGN_CODE) && 
                           Fga_Uclb_U.Contains(m.CLUB_CODE) && m.VALD_TYPE == "002" && 
                           m.CALC_EXPN_TYPE == "001" &&
                           m.DELV_DATE.Value.Date >= (Pdt_FromDate.Value ?? DateTime.Now.AddYears(-100).Date)  &&
                           m.DELV_DATE.Value.Date <= (Pdt_ToDate.Value ?? DateTime.Now.Date) &&
                           m.COCH_FILE_NO == (Coch_Lov.ItemIndex >= 0 ? (long?)Coch_Lov.EditValue : m.COCH_FILE_NO )
               );
         }
         else if (tb_master.SelectedTab == tp_002)
         {
            MOSX_BindingSource.DataSource = iScsc.Misc_Expenses.Where(m => Fga_Urgn_U.Split(',').Contains(m.REGN_PRVN_CODE + m.REGN_CODE) && Fga_Uclb_U.Contains(m.CLUB_CODE) && m.VALD_TYPE == "002" && m.CALC_EXPN_TYPE == "002");
         }
      }

      private void Commit()
      {
         try
         {
            Validate();
            MSEX_BindingSource.EndEdit();
            MOSX_BindingSource.EndEdit();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch(Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
         finally
         {
            if(requery)
            {
               iScsc = new Data.iScscDataContext(ConnectionString);
               Execute_Query();
               requery = false;
            }
         }
      }

      private void bt_calcexpn_Click(object sender, EventArgs e)
      {
         try
         {
            Execute_Query();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }      

      private void msex_save_Click(object sender, EventArgs e)
      {
         Commit();
      }

      private void tsb_oreload_Click(object sender, EventArgs e)
      {
         Execute_Query();
      }

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void PRVN_LOV_Enter(object sender, EventArgs e)
      {
         PrvnBs2.DataSource = iScsc.Provinces.Where(p => Fga_Uprv_U.Split(',').Contains(p.CODE));
      }

      private void REGN_LOV_Click(object sender, EventArgs e)
      {
         var Crnt = MOSX_BindingSource.Current as Data.Misc_Expense;

         if (Crnt == null) return;
         if (Crnt.REGN_PRVN_CODE == null) { return; }

         RegnBs2.DataSource = iScsc.Regions.Where(r => r.PRVN_CODE == Crnt.REGN_PRVN_CODE && Fga_Urgn_U.Split(',').Contains(r.PRVN_CODE + r.CODE));
      }

      private void CLUB_LOV_Click(object sender, EventArgs e)
      {
         var Crnt = MOSX_BindingSource.Current as Data.Misc_Expense;

         if (Crnt == null) return;
         if (Crnt.REGN_PRVN_CODE == null || Crnt.REGN_CODE == null) { return; }

         ClubBs2.DataSource = iScsc.Clubs.Where(r => r.REGN_PRVN_CODE == Crnt.REGN_PRVN_CODE && r.REGN_CODE == Crnt.REGN_CODE && Fga_Uclb_U.Contains(r.CODE));
      }
   }
}
