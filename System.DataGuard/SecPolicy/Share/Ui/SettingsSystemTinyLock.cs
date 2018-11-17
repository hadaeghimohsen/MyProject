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
using DevExpress.XtraEditors;
using System.Globalization;
using dataAccess;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsSystemTinyLock : UserControl
   {
      public SettingsSystemTinyLock()
      {
         InitializeComponent();
      }

      private bool requery = false;

      private void Back_Butn_Click(object sender, EventArgs e)
      {
         // Close Start Drawer
         _DefaultGateway.Gateway(
            new Job(SendType.External, "localhost", GetType().Name, 00 /* Execute DoWork4SettingsDrawer */, SendType.SelfToUserInterface) { Input = Keys.Escape }
         );
      }

      List<TabPage> listTabPages;
      private void SwitchButtonsTabPage(object sender)
      {
         try
         {
            #region Action on Buttons
            SimpleButton butn = sender as SimpleButton;
            var flowlayout = butn.Parent as FlowLayoutPanel;
            foreach (SimpleButton b in flowlayout.Controls)
            {
               b.ForeColor = Color.FromArgb(64, 64, 64);
            }
            butn.ForeColor = Color.DodgerBlue;
            #endregion
            #region Action on TabControl
            if (listTabPages == null)
               listTabPages = Tb_Master.TabPages.OfType<TabPage>().ToList();

            var selectedtabpage = listTabPages.Where(t => t.Tag == butn.Tag).First();
            Tb_Master.TabPages.Clear();
            Tb_Master.TabPages.Add(selectedtabpage);
            #endregion
         }
         catch { }
         finally { Execute_Query(); }
      }      

      private void RightButns_Click(object sender, EventArgs e)
      {
         SwitchButtonsTabPage(sender);
      }

      private void Execute_Query()
      {
         iProject = new Data.iProjectDataContext(ConnectionString);
         int serv = ServBs.Position;
         int oprt = OprtBs.Position;
         int tiny = TinyBs.Position;
         int tnss = TnssBs.Position;
         ServBs.DataSource = iProject.Services;
         ServBs.Position = serv;
         OprtBs.Position = oprt;
         TinyBs.Position = tiny;
         TnssBs.Position = tnss;

         requery = false;
      }

      private void SubmitChanges_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ServBs.EndEdit();
            OprtBs.EndEdit();
            TinyBs.EndEdit();
            TnssBs.EndEdit();

            Serv_Gv.PostEditor();
            Oprt_Gv.PostEditor();
            Tiny_Gv.PostEditor();
            Tnss_Gv.PostEditor();

            iProject.SubmitChanges();

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

      private void TinyBs_CurrentChanged(object sender, EventArgs e)
      {
         try
         {
            var tiny = TinyBs.Current as Data.Tiny_Lock;
            if (tiny == null || tiny.TNID == 0) return;

            if(tiny.TINY_CODE_DNRM != null)
               TinyHash_Txt.EditValue = soorenFunctions.getHsh(tiny.TINY_CODE_DNRM);
         }
         catch (Exception exc)
         {
            MessageBox.Show(exc.Message);
         }
      }
   }
}
