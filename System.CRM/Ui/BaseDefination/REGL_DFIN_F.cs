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
using System.Globalization;

namespace System.CRM.Ui.BaseDefination
{
   public partial class REGL_DFIN_F : UserControl
   {
      public REGL_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private string formcaller;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if(tb_master.SelectedTab == tp_001)
         {
            int b = ReglBs.Position;
            ReglBs.DataSource = iCRM.Regulations;
            ReglBs.Position = b;
         }
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      private void SubmitChanged_Clicked(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            ReglBs.EndEdit();

            iCRM.CommandTimeout = 180000;
            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if(requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void Tsb_DelRegl_Click(object sender, EventArgs e)
      {
         try
         {
            if (MessageBox.Show(this, "آیا تغییرات ذخیره گردد؟", "ثبت نتایج تغییرات", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var regl = ReglBs.Current as Data.Regulation;

            iCRM.DEL_REGL_P(regl.YEAR, regl.CODE);
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
            {
               Execute_Query();
               requery = false;
            }
         }
      }

      private void InvsRegl_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         var regl = ReglBs.Current as Data.Regulation;

         if (regl == null) return;

         if (e.Button.Index == 0 && regl.TYPE == "001")
         {
            Job _InteractWithCRM =
              new Job(SendType.External, "Localhost",
                 new List<Job>
              {                  
                new Job(SendType.Self, 08 /* Execute Rqrq_Dfin_F */){Input = regl},
              });
            _DefaultGateway.Gateway(_InteractWithCRM);
         }
      }

      private void AddNewRegl_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ReglBs.AddNew();

            var regl = ReglBs.Current as Data.Regulation;

            if (regl == null) return;

            regl.STRT_DATE = DateTime.Now;
            regl.END_DATE = DateTime.Now.AddYears(1);
            regl.LETT_DATE = DateTime.Now;
            regl.YEAR = (short)(new PersianCalendar()).GetYear(DateTime.Now);
         }
         catch
         {

         }
      }
   }
}
