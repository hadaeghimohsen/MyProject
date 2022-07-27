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
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;

namespace System.Scsc.Ui.Advertising
{
   public partial class ADV_BASE_F : UserControl
   {
      public ADV_BASE_F()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int index = default(int);

      private void Execute_Query()
      {         
         try
         {
            iScsc = new Data.iScscDataContext(ConnectionString);

            AdvpBs.DataSource = iScsc.Advertising_Parameters;
            requery = false;
         }
         catch { }
         finally { requery = false; }
      }

      private void AddAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            if (AdvpBs.List.OfType<Data.Advertising_Parameter>().Any(a => a.CODE == 0)) return;

            var _advp = AdvpBs.AddNew() as Data.Advertising_Parameter;
            if (_advp == null) return;

            _advp.ADVP_NAME = "عنوان تخفیف";
            _advp.RECD_TYPE = "001";
            _advp.DSCT_TYPE = "001";
            _advp.STAT = "002";
            _advp.DSCT_AMNT = 10;
            _advp.DISC_CODE = Guid.NewGuid().ToString().Split('-')[0].ToUpper();
            _advp.NUMB_LAST_DAY = 0;
            _advp.EXPR_DATE = DateTime.Now.AddDays(7);

            iScsc.Advertising_Parameters.InsertOnSubmit(_advp);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }

      private void DelAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            if (MessageBox.Show(this, "آیا با حذف رکورد موافق هستید؟", "حذف رکورد", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            iScsc.Advertising_Parameters.DeleteOnSubmit(_advp);
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void SaveAdvp_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            advp_gv.PostEditor();

            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Rqtp_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  _advp.RQTP_CODE = null;
                  break;
               default:
                  break;
            }

            advp_gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }

      private void Ctgy_Lov_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var _advp = AdvpBs.Current as Data.Advertising_Parameter;
            if (_advp == null) return;

            switch (e.Button.Index)
            {
               case 1:
                  _advp.CTGY_CODE = null;
                  break;
               default:
                  break;
            }

            advp_gv.PostEditor();
            iScsc.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
         finally
         {
            if (requery)
               Execute_Query();
         }
      }      
   }
}
