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
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;

namespace System.DataGuard.SecPolicy.Share.Ui
{
   public partial class SettingsSystemScript : UserControl
   {
      public SettingsSystemScript()
      {
         InitializeComponent();
      }

      private bool requery = false;
      private int subsys;

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
         if(Tb_Master.SelectedTab == tp_001)
         {
            SubSysBs.DataSource = iProject.Sub_Systems.Where(s => s.STAT == "002" && s.SUB_SYS == subsys);
         }
         requery = false;
      }

      private void AddScript_Butn_Click(object sender, EventArgs e)
      {
         var subsys = SubSysBs.Current as Data.Sub_System;
         if (subsys == null) return;
         if (ScrpBs.List.OfType<Data.Script>().Any(s => s.CODE == 0)) return;

         ScrpBs.AddNew();
         var scrp = ScppBs.Current as Data.Script;
         scrp.SUB_SYS = subsys.SUB_SYS;

         Scrp_Gv.SelectRow(Scrp_Gv.RowCount - 1);

         iProject.Scripts.InsertOnSubmit(scrp);
      }

      private void DelScript_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var scrp = ScrpBs.Current as Data.Script;
            if (scrp == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rows = Scrp_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Script)Scrp_Gv.GetRow(r);
               iProject.Scripts.DeleteOnSubmit(row);
            }

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

      private void SaveScript_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ScrpBs.EndEdit();

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

      private void AddScriptParameter_Butn_Click(object sender, EventArgs e)
      {
         var scrp = ScppBs.Current as Data.Script;
         if (scrp == null) return;
         if (ScppBs.List.OfType<Data.Script_Parameter>().Any(s => s.CODE == 0)) return;

         ScppBs.AddNew();
         var scpp = ScppBs.Current as Data.Script_Parameter;
         scpp.SCRP_CODE = scrp.CODE;

         Scpp_Gv.SelectRow(Scpp_Gv.RowCount - 1);

         iProject.Script_Parameters.InsertOnSubmit(scpp);
      }

      private void DelScriptParameter_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            var scpp = ScppBs.Current as Data.Script_Parameter;
            if (scpp == null) return;

            if (MessageBox.Show(this, "حذف", "آیا با حذف رکورد موافق هستید؟", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            var rows = Scpp_Gv.GetSelectedRows();

            foreach (var r in rows)
            {
               var row = (Data.Script_Parameter)Scrp_Gv.GetRow(r);
               iProject.Script_Parameters.DeleteOnSubmit(row);
            }

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

      private void SaveScriptParameter_Butn_Click(object sender, EventArgs e)
      {
         try
         {
            ScppBs.EndEdit();

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
   }
}
