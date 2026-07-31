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
using DevExpress.XtraEditors;
using System.IO;
using System.Scsc.ExtCode;
using System.Threading;

namespace System.Scsc.Ui.Common
{
   public partial class SRCH_ENGN_F : UserControl
   {
      public SRCH_ENGN_F()
      {
         InitializeComponent();
      }

      int index = 0;
      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

private async void Execute_Query(string searchText)
      {
         try
         {
            // جستجوی همزمان در ۷ جدول (هر کدوم یه Thread جدا)
            var fighTask = Task.Run(() =>
            {
               using (var dc = new Data.iScscDataContext(ConnectionString))
               {
                  return dc.Fighters
                     .Where(f => f.NAME_DNRM.Contains(searchText)
                              || f.FRST_NAME_DNRM.Contains(searchText)
                              || f.LAST_NAME_DNRM.Contains(searchText)
                              || f.FATH_NAME_DNRM.Contains(searchText)
                              || f.POST_ADRS_DNRM.Contains(searchText)
                              || f.CELL_PHON_DNRM.Contains(searchText)
                              || f.TELL_PHON_DNRM.Contains(searchText)
                              || f.INSR_NUMB_DNRM.Contains(searchText)
                              || f.FILE_NO.ToString().Contains(searchText))
                     .ToList();
               }
            });

            var mtodTask = Task.Run(() =>
            {
               using (var dc = new Data.iScscDataContext(ConnectionString))
               {
                  return dc.Methods
                     .Where(m => m.MTOD_DESC.Contains(searchText)
                              || m.CODE.ToString().Contains(searchText))
                     .ToList();
               }
            });

            var cbmtTask = Task.Run(() =>
             {
                using (var dc = new Data.iScscDataContext(ConnectionString))
                {
                   return dc.Club_Methods
                      .Where(c => c.CBMT_DESC.Contains(searchText)
                               || c.DAY_TYPE.Contains(searchText)
                               || c.CODE.ToString().Contains(searchText)
                               || c.MTOD_CODE.ToString().Contains(searchText)
                               || c.CLUB_CODE.ToString().Contains(searchText)
                               || c.COCH_FILE_NO.ToString().Contains(searchText))
                      .ToList();
                }
             });

            var pymtTask = Task.Run(() =>
             {
                using (var dc = new Data.iScscDataContext(ConnectionString))
                {
                   return dc.Payments
                      .Where(p => p.CASH_BY.Contains(searchText)
                               || p.CASH_DATE.ToString().Contains(searchText)
                               || p.PYMT_STAT.Contains(searchText)
                               || p.TYPE.Contains(searchText)
                               || p.PYMT_TYPE.Contains(searchText)
                               || p.LETT_NO.Contains(searchText)
                               || p.LETT_DATE.ToString().Contains(searchText)
                               || p.DELV_STAT.Contains(searchText)
                               || p.DELV_DATE.ToString().Contains(searchText))
                      .ToList();
                }
             });

            await Task.WhenAll(fighTask, mtodTask, cbmtTask, pymtTask);

            FighsBs.DataSource = fighTask.Result;
            MtodBs.DataSource = mtodTask.Result;
            CbmtBs.DataSource = cbmtTask.Result;
            PymtBs.DataSource = pymtTask.Result;
         }
         catch (Exception ex)
         {
            MessageBox.Show("خطا در جستجو:\n" + ex.Message, "خطا", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }
      }
      
   }
}
