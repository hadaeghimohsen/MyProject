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

namespace System.CRM.Ui.BaseDefination
{
   public partial class STNG_DFIN_F : UserControl
   {
      public STNG_DFIN_F()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Btn_Back_Click(object sender, EventArgs e)
      {
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute ProcessCmdKey */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      private void Execute_Query()
      {
         iCRM = new Data.iCRMDataContext(ConnectionString);
         if (tb_master.SelectedTab == tp_001)
         {
            int g = StngBs.Position;
            StngBs.DataSource = iCRM.Settings;
            StngBs.Position = g;
         }
      }

      private void Refresh_Clicked(object sender, EventArgs e)
      {
         Execute_Query();
         requery = false;
      }

      private void RootPath_TextButn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var stng = StngBs.Current as Data.Setting;
            if (stng == null) return;

            if (FilePath_Fbd.ShowDialog() != DialogResult.OK) return;

            stng.BACK_UP_ROOT_PATH = FilePath_Fbd.SelectedPath;
         }
         catch (Exception exc)
         {}
         finally { StngBs.EndEdit(); StngStorage_Gv.Invalidate(); }
      }

      private void OptnPath_TextButn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var stng = StngBs.Current as Data.Setting;
            if (stng == null) return;

            if (FilePath_Fbd.ShowDialog() != DialogResult.OK) return;

            stng.BACK_UP_OPTN_PATH_ADRS = FilePath_Fbd.SelectedPath;
         }
         catch (Exception exc)
         { }
         finally { StngBs.EndEdit(); }
      }

      private void UpldFilePath_TextButn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var stng = StngBs.Current as Data.Setting;
            if (stng == null) return;

            if (FilePath_Fbd.ShowDialog() != DialogResult.OK) return;

            stng.UPLD_FILE = FilePath_Fbd.SelectedPath;
         }
         catch (Exception exc)
         { }
         finally { StngBs.EndEdit(); }
      }

      private void DfltStng_Butn_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
      {
         try
         {
            var stng = StngBs.Current as Data.Setting;
            if (stng == null) return;

            StngBs.List.OfType<Data.Setting>().FirstOrDefault(s => s.DFLT_STAT == "002").DFLT_STAT = "001";
            stng.DFLT_STAT = "002";
         }
         catch (Exception exc)
         { }
         finally { StngBs.EndEdit(); }
      }

      private void StngSubmitChange_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            StngBs.EndEdit();

            iCRM.SubmitChanges();
            requery = true;
         }
         catch (Exception exc)
         {}
         finally
         {
            if(requery)
            {
               Execute_Query();
            }
         }
      }
   }
}
